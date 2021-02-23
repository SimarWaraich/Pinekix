using UnityEngine;
using System.Collections;

public class ScaleCoinWithZoom : MonoBehaviour {

    Camera mainCamera;
	public float posOffset = 0.4f;// -0.8 for bed // 0.4 for coin on player head
    public float zoomOffset = 1f;

    void OnEnable () {
        mainCamera = Camera.main;	       

        float Zoom = mainCamera.orthographicSize - 5.0f;
        transform.localScale = Vector3.one + Vector3.one * 0.5f *(Zoom * zoomOffset);
		transform.localPosition = new Vector2(0, Zoom * posOffset);

	}	
	void Update () {      
        float Zoom = mainCamera.orthographicSize - 5.0f;
        transform.localScale = Vector3.one + Vector3.one * 0.5f *(Zoom * zoomOffset);
		transform.localPosition = new Vector2(0, Zoom * posOffset);
	}
}
