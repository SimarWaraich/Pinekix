using UnityEngine;
using System.Collections;

public class DragableCamera : MonoBehaviour
{
	public float RestrictedMinX;
	public float RestrictedMaxX;

	public float RestrictedMinY;
	public float RestrictedMaxY;

	Vector3 NewTouchPos;
	Vector3 OldCameraPos;
	Vector3 ActPos;

	bool dragging = false;

	public float LerpSpeed = 1;

	Rect blockingArea;
    SocietyHomeRoom societyHomeRoom ;
	void Start ()
	{
        societyHomeRoom = ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>();
	}

	void OnGUI ()
	{
		blockingArea = new Rect (Screen.width / 1.81f, Screen.height / 8f, Screen.width / 2f, Screen.height / 1.2f);
	
//		if (GUI.Button (blockingArea, "this area ")) {
//	
//		}
//		print ("Width ==>> " + Screen.width + "Screen Height ==>> " + Screen.height);
	}

	void Update ()
	{
		


		if (Input.GetMouseButtonDown (0)) {

			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

			if (hit.collider != null) {
                if (hit.collider.gameObject.name == "PublicAreaBackground" || hit.collider.gameObject.name == "FlatPartyPublicArea" || hit.collider.gameObject.name == "SocietyPartyArea" || hit.collider.gameObject.name == "HomeRoomBackGround")
					dragging = true;
				else
					dragging = false;			
			} else
				dragging = false;		
		}

		if (Input.GetMouseButtonUp (0))
			dragging = false;	
	

		if (Input.GetMouseButton (0)) {
			if (blockingArea.Contains (Input.mousePosition)) {
				if (ScreenAndPopupCall.Instance.ChatInFlatPartyIsShown) {					
					dragging = false;
					print ("dragging is false");
				} else if (ScreenAndPopupCall.Instance.ChatInSocietyPartyIsShown) {
					dragging = false;
					print ("dragging is false");
				} else if (ScreenAndPopupCall.Instance.IsChatOpen) {
					dragging = false;
					print ("dragging is false");
				} else if (ScreenAndPopupCall.Instance.IsAddFriendOpen) {
					dragging = false;
					print ("dragging is false");
				}
//				else if (MultiplayerManager.Instance.RoomName == "Public Park" || MultiplayerManager.Instance.RoomName == "Lounge" ||
//				           MultiplayerManager.Instance.RoomName == "Library" || MultiplayerManager.Instance.RoomName == "Activity Area" ||
//				           MultiplayerManager.Instance.RoomName == "Party Area") {
//					dragging = false;
//					print ("dragging is false");
//				}
			}
		}

		CamreaZoomInOut ();

		CameraDragging ();

        if(ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.HomeRoomSociety)
        {
            if (Camera.main.rect.Contains (Camera.main.WorldToViewportPoint (new Vector3 (-1000, 0, 0)))) 
            {         
                societyHomeRoom.ReCenterButton.gameObject.SetActive (false);
            } else {
                societyHomeRoom.ReCenterButton.gameObject.SetActive (true);
            }
        }
	}


	void CameraDragging ()
	{		
		if (dragging) {
            if (ScreenManager.Instance.ScreenMoved && ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.PublicAreaMenu &&  ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.HomeRoomSociety ) {
				if (!FlatPartyHostingControler.Instance.ScreenCanMove)
					return;
			}

			if (Input.GetMouseButtonDown (0)) {

				OldCameraPos = transform.position;				               
				NewTouchPos = Camera.main.ScreenToViewportPoint (Input.mousePosition);			
				return;
			}

			if (!Input.GetMouseButton (0))
				return;
			
			Vector3 Pos = Camera.main.ScreenToViewportPoint (Input.mousePosition) - NewTouchPos;

			transform.position = OldCameraPos - Pos * LerpSpeed;

			Vector3 restricted_Position = transform.position;
			restricted_Position.x = Mathf.Clamp (restricted_Position.x, RestrictedMinX, RestrictedMaxX);
			restricted_Position.y = Mathf.Clamp (restricted_Position.y, RestrictedMinY, RestrictedMaxY);

			transform.position = restricted_Position;
		}
	}

	void CamreaZoomInOut ()
	{
		if (!dragging) {

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
				if (Camera.main.orthographic) {

//					if (ScreenManager.Instance.ScreenMoved && ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.PublicAreaMenu)
//						return;
					
					/*cameraDragging = false;*/
					// ... change the orthographic size based on the change in distance between the touches.
					Camera.main.orthographicSize += deltaMagnitudeDiff * 0.5f * Time.deltaTime;

					// Make sure the orthographic size never drops below zero.
					Camera.main.orthographicSize = Mathf.Max (Camera.main.orthographicSize, 3.8f);
					Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize, 10.5f);
				} 
			} 
		}
		if (!dragging) {
			Camera.main.orthographicSize += Input.GetAxis ("Mouse ScrollWheel") * 0.5f;
			Camera.main.orthographicSize = Mathf.Max (Camera.main.orthographicSize, 6f);
			Camera.main.orthographicSize = Mathf.Min (Camera.main.orthographicSize, 9.5f);
		}

	}
}
