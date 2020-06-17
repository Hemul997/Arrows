using UnityEngine;

public class Circle : MonoBehaviour {
	
	[SerializeField][Range(0.1f, 100f)]
	public float _radius = 1.0f;
	
	[SerializeField][Range(0.01f, 100f)]
	public float _width = 1.0f;
	
	[SerializeField][Range(3, 256)] 
	public int _numSegments = 128;
	
	[SerializeField] public Color _color = new Color(1.0f, 0.1f, 0.1f, 1.0f);

    /// <summary>
    /// MonoBehaviour function
    /// Called when the script instance is being loaded.
    /// </summary>
	private void Awake() {
		DrawCircle();
	}

	/// <summary>
    /// Draws circle using LineRenderer
    /// </summary>
	public void DrawCircle() {
		var lineRenderer = gameObject.GetComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
		lineRenderer.startColor = _color;
		lineRenderer.endColor = _color;
		lineRenderer.startWidth = _width;
		lineRenderer.endWidth = _width;
		lineRenderer.positionCount = _numSegments + 1;
		lineRenderer.useWorldSpace = false;
 
		float deltaTheta = (float) (2.0 * Mathf.PI) / _numSegments;
		float theta = 0f;
 
		for (int i = 0 ; i < _numSegments + 1 ; i++) {
			float x = _radius * Mathf.Cos(theta);
			float y = _radius * Mathf.Sin(theta);
			Vector3 pos = new Vector3(x, y, 0);
			lineRenderer.SetPosition(i, pos);
			theta += deltaTheta;
		}
	}
}
