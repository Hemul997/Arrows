using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : MonoBehaviour {

	private const float _MOVE_TIME = 1.0f;
	private const float _WAIT_TIME = 2.0f;
	

    // Lines
	[SerializeField] private Transform _horTr;
	[SerializeField] private Transform _verTr;
	[SerializeField] private Transform _horTrAuto;
	[SerializeField] private Transform _verTrAuto;

    //UI
    [SerializeField]
    private TestResultsPrinter ResultsPrinter;

    // Animation params
    [SerializeField] private readonly float _maxMoveRange = 1.5f;
	[SerializeField] private readonly float _speed = 1.0f;
	[SerializeField] private readonly AnimationCurve _curve;
	[SerializeField] private readonly int _testCountMax = 5;
    // Results data
    private readonly List<Vector2> _results = new List<Vector2>();

    // Keyboard Input Control
    private readonly KeyCode _keyToMoveUp = KeyCode.UpArrow;
	private readonly KeyCode _keyToMoveDown = KeyCode.DownArrow;
	private readonly KeyCode _keyToMoveLeft = KeyCode.LeftArrow;
	private readonly KeyCode _keyToMoveRight = KeyCode.RightArrow;
	private readonly KeyCode _keySubmit = KeyCode.Space;

	private int _testCount;

	private bool _isCanControll;

    /// <summary>
    /// MonoBehaviour function
    /// called before the first frame update
    /// </summary>
    private void Start()
    {
		StartTest();
	}

    /// <summary>
    /// MonoBehaviour function
    /// called once per frame
    /// </summary>
    private void Update()
    {
		if (_isCanControll) {
			MoveHor();
			MoveVer();
			Submit();
		}
	}

	/// <summary>
    /// Method moves horizontal arrow
    /// </summary>
	private void MoveHor()
    {
		if (!Input.anyKey) return;
		if (Input.GetKey(_keyToMoveLeft) && IsCanMove(_horTr, Vector3.left * Time.deltaTime * _speed))
			_horTr.localPosition += Vector3.left * Time.deltaTime * _speed;
		if (Input.GetKey(_keyToMoveRight) && IsCanMove(_horTr, Vector3.right * Time.deltaTime * _speed))
			_horTr.localPosition += Vector3.right * Time.deltaTime * _speed;
		_horTr.rotation = Quaternion.Euler(0, 0, _horTr.localPosition.x < 0 ? 90 : -90.0f);
	}

    /// <summary>
    /// Method moves vertical arrow
    /// </summary>
    private void MoveVer()
    {
		if (!Input.anyKey) return;
		if (Input.GetKey(_keyToMoveUp) && IsCanMove(_verTr, Vector3.up * Time.deltaTime * _speed))
			_verTr.localPosition += Vector3.up * Time.deltaTime * _speed;
		if (Input.GetKey(_keyToMoveDown) && IsCanMove(_verTr, Vector3.down * Time.deltaTime * _speed))
			_verTr.localPosition += Vector3.down * Time.deltaTime * _speed;
		_verTr.rotation = Quaternion.Euler(0, 0, _verTr.localPosition.y < 0 ? 180.0f : 0);
	}

	/// <summary>
    /// Method for submitting end of test iteration
    /// </summary>
	private void Submit()
    {
		if (Input.GetKeyDown(_keySubmit)) {
			_isCanControll = false;
			var horDelta = Mathf.Abs(_horTrAuto.localPosition.x) - Mathf.Abs(_horTr.localPosition.x);
			var verDelta = Mathf.Abs(_verTrAuto.localPosition.y) - Mathf.Abs(_verTr.localPosition.y);

            ResultsPrinter.PrintCurrResult(horDelta, verDelta);

			_results.Add(new Vector2(horDelta, verDelta));

            if (_testCount < _testCountMax)
				StartCoroutine(WaitAndDo(_WAIT_TIME, StartTest));
			else
                ResultsPrinter.PrintAllResults(_results);
		}
	}

    /// <summary>
    /// Method check is can transform moving to target position
    /// </summary>
    /// <param name="tr">Checked transform</param>
    /// <param name="addPos">Target position</param>
    /// <returns></returns>
	private bool IsCanMove(Transform tr, Vector3 addPos)
    {
		var newPos = tr.localPosition + addPos;
		return Mathf.Abs(newPos.x) < _maxMoveRange && Mathf.Abs(newPos.y) < _maxMoveRange;
	}

    /// <summary>
    /// Method started next test iteration
    /// </summary>
	private void StartTest()
    {
        ++_testCount;
		ResetParams();
		AutoMove();
        ResultsPrinter.ResetCurrResultText();
	}

    /// <summary>
    /// Reset local positions of arrows
    /// </summary>
	private void ResetParams()
    {
		_horTr.localPosition = Vector3.zero;
		_verTr.localPosition = Vector3.zero;
	}

    /// <summary>
    /// Method moves arrows to random generated positions 
    /// </summary>
	private void AutoMove()
    {
        GenerateArrowsPositions(out Vector3 posHor, out Vector3 posVer);
        
		StartCoroutine(MoveTo(_horTrAuto, posHor, _MOVE_TIME, _curve));
		StartCoroutine(MoveTo(_verTrAuto, posVer, _MOVE_TIME, _curve));
		StartCoroutine(RotateToFront(_horTrAuto, _MOVE_TIME, false));
		StartCoroutine(RotateToFront(_verTrAuto, _MOVE_TIME, true));
		StartCoroutine(WaitAndDo(_MOVE_TIME, () => _isCanControll = true));
	}


    /// <summary>
    /// Method generates random positions for arrow line ending. 
    /// </summary>
    /// <param name="posHor">Horizontal arrows end position</param>
    /// <param name="posVer">Vertical arrows end position<</param>
    private void GenerateArrowsPositions(out Vector3 posHor, out Vector3 posVer)
    {
        posHor = new Vector3(UnityEngine.Random.Range(-_maxMoveRange, _maxMoveRange), 0);
        posVer = new Vector3(0, UnityEngine.Random.Range(-_maxMoveRange, _maxMoveRange));
    }

    /// <summary>
    /// Method perform an action through timeout
    /// </summary>
    /// <param name="time">Timeout</param>
    /// <param name="action">Performed action</param>
    /// <returns></returns>
    public static IEnumerator WaitAndDo(float time, Action action)
    {
		yield return new WaitForSeconds(time);
		action();
	}


	/// <summary>
    /// Method for rotating end of arrow
    /// </summary>
    /// <param name="obj">Transform object for rotating</param>
    /// <param name="time"></param>
    /// <param name="isVertical"></param>
    /// <returns></returns>
	public static IEnumerator RotateToFront(Transform obj, float time, bool isVertical)
    {
		var timer = 0.0f;
		while (timer < time) {
			timer += Time.deltaTime;
			
			obj.rotation = isVertical ? Quaternion.Euler(0, 0, obj.localPosition.y < 0 ? 180.0f : 0) 
				: Quaternion.Euler(0, 0, obj.localPosition.x < 0 ? 90 : -90.0f);
			
			yield return new WaitForEndOfFrame();
		}
	}

    /// <summary>
    /// Method moved object transform to target position
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="posEnd"></param>
    /// <param name="time"></param>
    /// <param name="curve"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEnumerator MoveTo(Transform obj, Vector3 posEnd,
		float time, AnimationCurve curve = null, Action callback = null)
    {
		return MoveTo(obj, posEnd, time, curve, callback, true, true, true);
	}


    /// <summary>
    /// Method moved object transform to target position 
    /// </summary>
    /// <param name="obj">Moved transform</param>
    /// <param name="posEnd">Target position</param>
    /// <param name="time">Moving time</param>
    /// <param name="curve">Moving curve</param>
    /// <param name="callback">Invoked action after tranform moving</param>
    /// <param name="isX">Flag is moving transform in X-coordinates</param>
    /// <param name="isY">Flag is moving transform in Y-coordinates</param>
    /// <param name="isZ">Flag is moving transform in Z-coordinates</param>
    /// <returns></returns>
	private static IEnumerator MoveTo(Transform obj, Vector3 posEnd, float time, AnimationCurve curve,
		Action callback, bool isX, bool isY, bool isZ)
    {

		if (curve == null) curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
		var posStart = obj.localPosition;
		var timer = 0.0f;
		while (timer < 1.0f) {
			timer = Mathf.Min(1.0f, timer + Time.deltaTime/time);
			var value = curve.Evaluate(timer);
			var pos = obj.localPosition;
			var x = isX ? Mathf.Lerp(posStart.x, posEnd.x, value) : pos.x;
			var y = isY ? Mathf.Lerp(posStart.y, posEnd.y, value) : pos.y;
			var z = isZ ? Mathf.Lerp(posStart.z, posEnd.z, value) : pos.z;
			obj.localPosition = new Vector3(x, y, z);

			yield return new WaitForEndOfFrame();
		}
        callback?.Invoke();
    }
	
}
