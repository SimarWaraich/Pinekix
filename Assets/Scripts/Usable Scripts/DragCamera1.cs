/// <summary>
/// Created By ::==>> Rehan... Dated 25 Aug 2k16
﻿/// <summary>
/// Created By ::==>> Rehan... Dated 25 Aug 2k16
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DragCamera1 : MonoBehaviour
{
	ScreenAndPopupCall ScreenController;

	public GameObject SwipeLeft;
	public GameObject SwipeRight;

	public Transform[] CameraSwipeAnimationObjects;

	private Vector3 FirstTouchPos;
	private Vector3 OldCameraPos;
	// The rate of change of the field of view in perspective mode.
	// The rate of change of the orthographic size in orthographic mode.
	private int areaCount = 0;

	public float orthoZoomSpeed = 0.5f;
	public float dragSpeed = 0.2f;
	public float perspectiveZoomSpeed = 0.5f;

	public float RestrictedMinX;
	public float RestrictedMaxX;

	public float RestrictedMinY;
	public float RestrictedMaxY;
	float LerpSpeed = 30;

	public static float mousedowntime;

	private Vector2 initialPosition;
	private Vector2 movedPosition;
	private Vector2 endPosition;

	Camera cam;
	public bool cameraDragging;
	public bool Cam_dragging = false;
	//	private bool moveLeft = false;

	void Start ()
	{
		cam = Camera.main;
		ScreenAndPopupCall.Instance.ReCenterButton.onClick.RemoveAllListeners ();
		ScreenAndPopupCall.Instance.ReCenterButton.onClick.AddListener (() => MoveBackToHome ());
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Cam_dragging = true;
//			// this is for placement button interactable while placing the decor 
//			if (ScreenAndPopupCall.Instance.placementbutton.interactable && ScreenAndPopupCall.Instance.placementEnabled) {
//				for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
//					RoommateManager.Instance.RoommatesHired [i].gameObject.GetComponent<ManageOrderInLayer> ().enabled = true;
//					RoommateManager.Instance.RoommatesHired [i].gameObject.GetComponent<BoxCollider2D> ().enabled = true;
//				}
//			}
		}

		if (Input.GetMouseButtonUp (0)) {
			Cam_dragging = false;
//			// this is for placement button interactable while placing the decor 
//			Invoke ("DesableCharAtribute", 0.3f);
		}


		if (ScreenManager.Instance.ScreenMoved == null && ScreenManager.Instance.PopupShowed == null) {
			CameraZoomInOut ();
		
//		var jhs = PlayerPrefs.GetInt ("Tutorial_Progress");
			if (Cam_dragging) {
//			if (PlayerPrefs.GetInt ("Tutorial_Progress") == 25)
//				CameraScrollToWayPoints ();
//			else if (PlayerPrefs.GetInt ("Tutorial_Progress") > 25)

				CameraDragging ();
			}
		}


		if (Camera.main.rect.Contains (Camera.main.WorldToViewportPoint (new Vector3 (0, 2, 0)))) {			
			ScreenAndPopupCall.Instance.ReCenterButton.gameObject.SetActive (false);
		} else {
			ScreenAndPopupCall.Instance.ReCenterButton.gameObject.SetActive (true);
		}
	}

//	void DesableCharAtribute ()
//	{
//		if (ScreenAndPopupCall.Instance.placementbutton.interactable && ScreenAndPopupCall.Instance.placementEnabled) {
//			for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
//				RoommateManager.Instance.RoommatesHired [i].gameObject.GetComponent<ManageOrderInLayer> ().enabled = false;
//				RoommateManager.Instance.RoommatesHired [i].gameObject.GetComponent<BoxCollider2D> ().enabled = false;
//			}
//		}
//	}

	public void MoveBackToHome ()
	{
		Vector3 Pos = new Vector3 (0, 2, transform.position.z); 
		iTween.MoveTo (gameObject, iTween.Hash ("position", Pos, "time", 3f));

		SwipeLeft.SetActive (false);
		SwipeRight.SetActive (false);
//		PlayerPrefs.SetInt ("Tutorial_Progress", 26);
//		GameManager.Instance.GetComponent <Tutorial>().UpdateTutorial ();
	}

	void CameraScrollToWayPoints ()
	{
		if (Input.touchCount > 0) {
			Touch touch1 = Input.GetTouch (0);
			if (touch1.phase == TouchPhase.Began) {
				initialPosition = new Vector2 (touch1.position.x, touch1.position.y);
			}
			if (touch1.phase == TouchPhase.Moved) {
				movedPosition = new Vector2 (touch1.deltaPosition.x, touch1.deltaPosition.y);
			}
			if (touch1.phase == TouchPhase.Ended) {
				endPosition = new Vector2 (touch1.position.x, touch1.position.y);
				if (movedPosition.magnitude > 80)
					CalculateSwipeDirection (initialPosition, endPosition);
			}
		}
	}

	void CalculateSwipeDirection (Vector2 firstPosition, Vector2 lastPosition)
	{

//		if ((firstPosition.x - lastPosition.x) < 0 && areaCount == 10) {
//			iTween.MoveTo (gameObject,iTween.Hash("position", new Vector3 (0, 0f, -10), "time", 5f));
//			
//		}
//		else 
		if ((firstPosition.x - lastPosition.x) > 0) {
			iTween.MoveTo (gameObject, iTween.Hash ("position", CameraSwipeAnimationObjects [areaCount], "time", 5f));
			if (areaCount < 10)
				areaCount++;
			else {
				areaCount = 0;	
				SwipeLeft.SetActive (false);
				SwipeRight.SetActive (false);
//				PlayerPrefs.SetInt ("Tutorial_Progress", 25);
//				GameManager.Instance.GetComponent <Tutorial>().UpdateTutorial ();
			}
		}
	}

	void CameraDragging ()
	{
		
		if (ScreenManager.Instance.ScreenMoved && ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.PublicAreaMenu)
			return;

		if (Input.GetMouseButtonDown (0)) {

			OldCameraPos = transform.position;				               
			FirstTouchPos = Camera.main.ScreenToViewportPoint (Input.mousePosition);			
			return;
		}

		Vector3 Pos = Camera.main.ScreenToViewportPoint (Input.mousePosition) - FirstTouchPos;

		Vector3 restricted_Position = OldCameraPos - Pos * LerpSpeed;
		restricted_Position.x = Mathf.Clamp (restricted_Position.x, RestrictedMinX, RestrictedMaxX);
		restricted_Position.y = Mathf.Clamp (restricted_Position.y, RestrictedMinY, RestrictedMaxY);

        transform.position = Vector3.Lerp (transform.position, restricted_Position, dragSpeed);		
	}

	//	float InitTime = 0;
	//	float EndTime = 0;
	//	void NewCameraDragging()
	//	{
	//		Vector3 MovedPos = Vector3.zero;
	//
	//		if (Input.GetMouseButtonDown (0)) {
	//
	//			OldCameraPos = transform.position;
	//			FirstTouchPos = Camera.main.ScreenToViewportPoint (Input.mousePosition);
	//			InitTime = Time.time;
	//			Cam_dragging = true;
	//		}
	//		if(Input.GetMouseButton (0) && Cam_dragging){
	//
	//			MovedPos = Camera.main.ScreenToViewportPoint (Input.mousePosition) - FirstTouchPos;
	//
	//		}else if (Input.GetMouseButtonUp (0))
	//		{
	//			EndTime = Time.time;
	//			MovedPos = Camera.main.ScreenToViewportPoint (Input.mousePosition)- FirstTouchPos;
	//			Vector3 EndPos = OldCameraPos -new Vector3(MovedPos.x * 6, MovedPos.y * 6, MovedPos.z);
	//			float distance = Vector3.Distance (EndPos, OldCameraPos);
	//			float time = EndTime - InitTime;//distance / LerpSpeed;
	////			var endpos = EndPos * 2;
	////			EndPos = new Vector3 (endpos.x, endpos.y, EndPos.z);
	//			iTween.MoveTo (gameObject, iTween.Hash ("position",new Vector3 (EndPos.x, EndPos.y, EndPos.z), "time", 2f));
	//			Cam_dragging = false;
	//		}
	//
	//
	//	}

	void CameraZoomInOut ()
	{
		//If there are two touches on the device...

		if (Input.touchCount == 2) {
			Cam_dragging = false;
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
				/*cameraDragging = false;*/
				// ... change the orthographic size based on the change in distance between the touches.
				var Zoomed = cam.orthographicSize;
				Zoomed += deltaMagnitudeDiff * orthoZoomSpeed * Time.deltaTime;
				// Make sure the orthographic size never drops below zero.

				cam.orthographicSize = Mathf.Clamp (Zoomed, 5f, 8f);
			} 
		}

		// Mouse ScrollWheel	
		var ScZoomed = cam.orthographicSize;
		ScZoomed += Input.GetAxis ("Mouse ScrollWheel") * orthoZoomSpeed;

		cam.orthographicSize = Mathf.Clamp (ScZoomed, 5f, 8f);


		//		if (camera.orthographicSize < 4.3f)
		//			camera.orthographicSize = 4.3f;
		//		else if (camera.orthographicSize > 8.0f)
		//			camera.orthographicSize = 10.5f;

	}


	//	void CameraDragging ()
	//	{
	//		// Mouse Drag
	//		if (dragging) {
	//
	//			if (ScreenManager.Instance.ScreenMoved)
	//				return;
	//
	//			if (Input.GetMouseButtonDown (0)) {
	//
	//
	//				oldPos = transform.position;
	//				dragOrigin = Camera.main.ScreenToViewportPoint (Input.mousePosition);
	//				return;
	//			}
	//
	//			if (!Input.GetMouseButton (0))
	//				return;
	//			Vector3 pos1 = Camera.main.ScreenToViewportPoint (Input.mousePosition) - dragOrigin;
	//			Vector3 curruntCamPos = Camera.main.transform.position;
	//
	//			if (Camera.main.transform.position.x <= outerRight && Camera.main.transform.position.x >= outerLeft
	//			    && Camera.main.transform.position.y >= outerBtm && Camera.main.transform.position.y <= outerTop) {
	//				transform.position = oldPos - pos1 * dragSpeed;
	//			}
	//			// Boundry Limit to Drag
	//			Vector3 snapCamX = new Vector3 (0.1f, 0, 0);
	//			Vector3 snapCamY = new Vector3 (0, 0.1f, 0);
	//
	//			if (Camera.main.transform.position.x <= outerLeft) {
	//				Camera.main.transform.position = curruntCamPos + snapCamX;
	//			}
	//			if (Camera.main.transform.position.x >= outerRight) {
	//				Camera.main.transform.position = curruntCamPos - snapCamX;
	//			}
	//			if (Camera.main.transform.position.y <= outerBtm) {
	//				Camera.main.transform.position = curruntCamPos + snapCamY;
	//			}
	//			if (Camera.main.transform.position.y >= outerTop) {
	//				Camera.main.transform.position = curruntCamPos - snapCamY;
	//			}
	//
	//			Camera.main.transform.position = new Vector3 (Mathf.Clamp (Camera.main.transform.position.x, outerLeft, outerRight), Mathf.Clamp (Camera.main.transform.position.y, outerBtm, outerTop), Camera.main.transform.position.z);
	//
	////			var tut = GameManager.Instance.GetComponent<Tutorial> ();
	////
	////			if (!tut.enabled) {
	////				if (ScreenManager.Instance.ScreenMoved == null)
	////					return;
	////
	////				if (ScreenManager.Instance.ScreenMoved.gameObject.name == "CellPhone" || ScreenManager.Instance.ScreenMoved.gameObject.name == "Menu") {
	////
	////					if (ScreenManager.Instance.CatalogOpen) {
	////
	////						ScreenManager.Instance.ClosePopup ();
	////
	////					} else {
	////						if (ScreenManager.Instance.MenuScreenOpen && ScreenManager.Instance.CatalogOpen) {
	////							ScreenAndPopupCall.Instance.CloseScreen ();
	////							ScreenManager.Instance.ClosePopup ();
	////							ScreenAndPopupCall.Instance._MenuCalled = false;
	////							ScreenManager.Instance.MenuScreenOpen = false;
	////							ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = ScreenAndPopupCall.Instance.MenuOpen;
	////						}
	////						if (ScreenManager.Instance.CellPhoneOpen) {
	////							ScreenAndPopupCall.Instance.CloseScreen ();
	////							ScreenAndPopupCall.Instance._MenuCalled = false;
	////							ScreenManager.Instance.MenuScreenOpen = false;
	////							//							ScreenAndPopupCall.Instance._CellPhoneCalled = false;
	////							//							ScreenManager.Instance.CellPhoneOpen = false;
	////							ScreenAndPopupCall.Instance.DesablePhone ();
	////						}
	////					}
	////				}
	////			}
	//
	//
	//		}
	//
	//	}



}