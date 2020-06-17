using UnityEngine;


/// <summary>
/// 
/// </summary>
public class Plume : MonoBehaviour {

	[SerializeField] private LineRenderer _lineRenderer;
	private Vector3 _startPos;


    /// <summary>
    /// MonoBehaviour function
    /// called when the script instance is being loaded. 
    /// </summary>
    public void Awake () {
        InitPlume();
    }

    /// <summary>
    /// MonoBehaviour function
    /// called after all Update functions have been called
    /// </summary>
    private void LateUpdate () {
		if (transform.hasChanged) {
			DrawPlume();
			transform.hasChanged = false;
		}
	}

    /// <summary>
    /// Method draws plume    
    /// </summary>
	private void DrawPlume() {
		_lineRenderer.SetPosition(1, transform.position);
	}

    /// <summary>
    /// Method initialized plume
    /// </summary>
    private void InitPlume()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, transform.position);
    }

}
