using UnityEngine;

public class DragCamera : MonoBehaviour
{
	public float dragSpeed = 2;
	public float perspectiveZoomSpeed = 0.5f;
	// The rate of change of the field of view in perspective mode.
	public float orthoZoomSpeed = 0.5f;
	// The rate of change of the orthographic size in orthographic mode.
	public float MaxZoom;
	public float MinZoom;
	Camera cam;

	Vector2 InitialPosition;
	Vector2 InitialMousePosition;

	void Start ()
	{
		cam = Camera.main;
	}

	void Update ()
	{
		if (!enabled)
			return;


		if (Input.touchCount == 2) {
			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			// If the camera is orthographic...
			if (cam.orthographic) {
				// ... change the orthographic size based on the change in distance between the touches.
				if (cam.orthographicSize >= MinZoom && cam.orthographicSize <= MaxZoom) {
					cam.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

					// Make sure the orthographic size never drops below zero.
					cam.orthographicSize = Mathf.Max (cam.orthographicSize, 0.1f);
				} else {
					if (cam.orthographicSize < MinZoom)
						cam.orthographicSize = MinZoom;
					else if (cam.orthographicSize > MaxZoom)
						cam.orthographicSize = MaxZoom;
				}

			} else {
				// Otherwise change the field of view based on the change in distance between the touches.
				cam.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

				// Clamp the field of view to make sure it's between 0 and 180.
				cam.fieldOfView = Mathf.Clamp (cam.fieldOfView, 0.1f, 179.9f);
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				InitialPosition = transform.parent.position;
				InitialMousePosition = Input.mousePosition;
			}
			if (Input.GetMouseButton (0)) {
				float h = dragSpeed * (Input.mousePosition.y - InitialMousePosition.y);
				float v = dragSpeed * (Input.mousePosition.x - InitialMousePosition.x);
				transform.parent.position = new Vector3 (transform.parent.position.x - h, transform.parent.position.y + v, 0);
			}
		}

		cam.orthographicSize += Input.GetAxis ("Mouse ScrollWheel") * orthoZoomSpeed;
		if (cam.orthographicSize < 1)
			cam.orthographicSize = 1;
		else if (cam.orthographicSize > 7)
			cam.orthographicSize = 7;
		

	}


}
