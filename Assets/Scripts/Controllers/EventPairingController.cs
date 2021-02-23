using UnityEngine;
using System.Collections;
using Simple_JSON;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class EventPairingController : Singleton<EventPairingController>
{

	DateTime EndTime;
	public bool isTimerRunning;
	public float Timer = 0;
	int HitCount = 0;
	public float TotalTimer = 0;

	public Text TimeText;

	void Awake ()
	{
		this.Reload ();
	}

	public void StartTimerGetForPair (float _time)
	{	
		HitCount = 0;
		EndTime = DateTime.UtcNow.AddMinutes (_time);
		var Diff = EndTime - DateTime.UtcNow;
		TotalTimer = (float)Diff.TotalSeconds;
		if (EventManagment.Instance.category == eventType.Fashion_Event)
			StartCoroutine (GetFashionPairAfterRegistration ());
		else if (EventManagment.Instance.category == eventType.Decor_Event) {
			StartCoroutine (GetDecorPairAfterRegistration ());
		} else if (EventManagment.Instance.category == eventType.CatWalk_Event)
			StartCoroutine (GetCatwalkPairAfterRegistration ());
		else if (EventManagment.Instance.category == eventType.CoOp_Event)
			StartCoroutine (GetCoopPairAfterRegistration ());
		else if (EventManagment.Instance.category == eventType.Society_Event)
			StartCoroutine (GetSocietyPairAfterRegistration ());

		isTimerRunning = true;
	}

	public void StartTimerGetForCheckOnly (float _time)
	{
		HitCount = 0;
		EndTime = DateTime.UtcNow.AddMinutes (_time);
		var Diff = EndTime - DateTime.UtcNow;
		TotalTimer = (float)Diff.TotalSeconds;
		StartCoroutine (GetCoopPairForCheck ());
		isTimerRunning = true;
	}

	void Update ()
	{
		if (isTimerRunning) {
			var Diff = EndTime - DateTime.UtcNow;
			Timer = (float)Diff.TotalSeconds;
			TimeText.text = ExtensionMethods.GetShortTimeStringFromFloat (Timer);

			if (EndTime < DateTime.UtcNow) {
				isTimerRunning = false;	
				return;
			}
		}
	}



	public IEnumerator GetFashionPairAfterRegistration ()
	{		
		HitCount++;
		Debug.LogError ("Web service hitted -" + HitCount + " Times");
		VotingPairManager.Instance.viewStatus = false;
		VotingPairManager.Instance.viewFriends = false;
		var cd = new CoroutineWithData (this, VotingPairManager.Instance.IeGetMyPair_Fashion (HitCount, EventManagment.Instance.CurrentEvent.Event_id, false));
		yield return cd.coroutine;

		if (cd.result != null) {
			EndTime = DateTime.UtcNow;
			isTimerRunning = false;	
			if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
				AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
		} else {
			if (HitCount < 5) {
				yield return new WaitForSeconds (TotalTimer / 4f);
				StartCoroutine (GetFashionPairAfterRegistration ());
			} else {
				EndTime = DateTime.UtcNow;
				isTimerRunning = false;	
				//TODO Make a pair with AI..

				ScreenAndPopupCall.Instance.CloseScreen ();
				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
				ScreenAndPopupCall.Instance.CloseCharacterCamera ();

				// change according to AI pair
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");


			}
		}
	}

	//	public IEnumerator GetDecorPairAfterRegistration ()
	//	{
	//		HitCount++;
	//		Debug.LogError ("Web service hitted -" + HitCount + " Times");
	//		VotingPairManager.Instance.viewStatus = false;
	//		var cd = new CoroutineWithData (this, EventManagment.Instance.IeCheckForPair (HitCount));
	//		yield return cd.coroutine;
	//
	//		if (cd.result != null) {
	//			EndTime = DateTime.UtcNow;
	//			isTimerRunning = false;
	//		} else {
	//			if (HitCount < 5) {
	//				yield return new WaitForSeconds (TotalTimer / 4f);
	//				StartCoroutine (GetDecorPairAfterRegistration ());
	//			} else {
	//				EndTime = DateTime.UtcNow;
	//				isTimerRunning = false;
	//				//TODO Make a pair with AI..
	//
	//				ScreenAndPopupCall.Instance.CloseScreen ();
	//				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
	//				ScreenAndPopupCall.Instance.CloseCharacterCamera ();
	//			}
	//		}
	//	}


	public IEnumerator GetSocietyPairAfterRegistration ()
	{		
		HitCount++;
		Debug.LogError ("Web service hitted -" + HitCount + " Times");
		VotingPairManager.Instance.viewFriends = false;
		VotingPairManager.Instance.viewStatus = false;
        VotingPairManager.Instance.viewFriends = false;        

		var cd = new CoroutineWithData (this, VotingPairManager.Instance.IeGetMyPair_SocietyEvent (HitCount, EventManagment.Instance.CurrentEvent.Event_id, SocietyManager.Instance._mySociety.Id, false));
		yield return cd.coroutine;

		if (cd.result != null) {
			EndTime = DateTime.UtcNow;
			isTimerRunning = false;	

			if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
				AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
		} else {
			if (HitCount < 5) {
				Debug.LogError ("Waiting.... for   -" + TotalTimer / 4f + " Seconds");
				yield return new WaitForSeconds (TotalTimer / 4f);
				Debug.LogError ("Waiting Ended -" + HitCount + " Times");
				StartCoroutine (GetSocietyPairAfterRegistration ());
			} else {
				EndTime = DateTime.UtcNow;
				isTimerRunning = false;	
				//TODO Make a pair with AI..

				ScreenAndPopupCall.Instance.CloseScreen ();
				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
				ScreenAndPopupCall.Instance.CloseCharacterCamera ();
				//change according to AI pair
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
			}
		}
	}

	public IEnumerator GetCoopPairAfterRegistration ()
	{		
		HitCount++;
		Debug.LogError ("Web service hitted -" + HitCount + " Times");
		VotingPairManager.Instance.viewFriends = false;
		VotingPairManager.Instance.viewStatus = false;
		var cd = new CoroutineWithData (EventManagment.Instance, EventManagment.Instance.GetCoOpRegistration (EventManagment.Instance.CurrentEvent.Event_id));
		yield return cd.coroutine;

		if (cd.result != null) {
			var Registered = (CoopRegisterPlayers)cd.result;
//			StartCoroutine (VotingPairManager.Instance.IeGetMyPair_CoOp (Registered.Player1Id, Registered.Player2Id, HitCount, false));

			var newcd = new CoroutineWithData (VotingPairManager.Instance, VotingPairManager.Instance.IeGetMyPair_CoOp (Registered.Player1Id, Registered.Player2Id, HitCount, false));
			yield return newcd.coroutine;
		
			if (newcd.result != null) {
				EndTime = DateTime.UtcNow;
				isTimerRunning = false;	
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
			} else {				
				if (HitCount < 5) {
					yield return new WaitForSeconds (TotalTimer / 4f);
					StartCoroutine (GetCoopPairAfterRegistration ());
				} else {
					EndTime = DateTime.UtcNow;
					isTimerRunning = false;	
					//TODO Make a pair with AI..

					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
					ScreenAndPopupCall.Instance.CloseCharacterCamera ();

					// Change according to AI Player
					if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
						AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
				}
			}
		} else {
			
			ScreenAndPopupCall.Instance.CloseScreen ();
			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
		}
	}

	public IEnumerator GetCoopPairForCheck ()
	{
		HitCount++;
		var cd = new CoroutineWithData (EventManagment.Instance, EventManagment.Instance.GetCoOpRegistration (EventManagment.Instance.CurrentEvent.Event_id));
		yield return cd.coroutine;

		if (cd.result != null) {
			var Registered = (CoopRegisterPlayers)cd.result;
			//			StartCoroutine (VotingPairManager.Instance.IeGetMyPair_CoOp (Registered.Player1Id, Registered.Player2Id, HitCount, false));
			VotingPairManager.Instance.viewFriends = false;
			VotingPairManager.Instance.viewStatus = false;
			var newcd = new CoroutineWithData (VotingPairManager.Instance, VotingPairManager.Instance.IeGetMyPair_CoOp (Registered.Player1Id, Registered.Player2Id, 1, false));
			yield return newcd.coroutine;
			if (newcd.result != null) {
				EndTime = DateTime.UtcNow;
				isTimerRunning = false;	
			} else {
				if (HitCount < 5) {
					yield return new WaitForSeconds (TotalTimer / 4f);
					StartCoroutine (GetCoopPairForCheck ());
				} else {
					EndTime = DateTime.UtcNow;
					isTimerRunning = false;	
					//TODO get my pair for last time.....

					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
					ScreenAndPopupCall.Instance.CloseCharacterCamera ();
				}
			}
		} else {

		}
	}

	public IEnumerator GetDecorPairAfterRegistration ()
	{		
		HitCount++;
		Debug.LogError ("Web service hitted -" + HitCount + " Times");
		VotingPairManager.Instance.viewFriends = false;
		VotingPairManager.Instance.viewStatus = false;
		var cd = new CoroutineWithData (VotingPairManager.Instance, VotingPairManager.Instance.IeShowMyPairOnScreenOfDecor (HitCount, false));
		yield return cd.coroutine;

		if (cd.result != null) {
			EndTime = DateTime.UtcNow;
			isTimerRunning = false;	
			if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
				AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
		} else {
			if (HitCount < 5) {
				yield return new WaitForSeconds (TotalTimer / 4f);
				StartCoroutine (GetDecorPairAfterRegistration ());
			} else {
				EndTime = DateTime.UtcNow;
				isTimerRunning = false;	
				//TODO Make a pair with AI..
				ScreenAndPopupCall.Instance.CloseScreen ();
				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
				ScreenAndPopupCall.Instance.CloseCharacterCamera ();
				/// Change if pair with AI
				EventManagment.Instance.DeselectEventCategory ();

				/// Change Accorting to AI Pair
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
			}
		}
	}



	public IEnumerator GetCatwalkPairAfterRegistration ()
	{		
		HitCount++;
		Debug.LogError ("Web service hitted -" + HitCount + " Times");
		VotingPairManager.Instance.viewFriends = false;
		VotingPairManager.Instance.viewStatus = false;
        VotingPairManager.Instance.viewFriends = false;
		var cd = new CoroutineWithData (EventManagment.Instance, VotingPairManager.Instance.IeCatWalkShowMyPair (HitCount, EventManagment.Instance.CurrentEvent.Event_id, false));
		yield return cd.coroutine;

		if (cd.result != null) {
			EndTime = DateTime.UtcNow;
			isTimerRunning = false;		

			for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
				for (int j = 0; j < EventManagment.Instance.SelectedRoommates.Count; j++) {
					if (RoommateManager.Instance.RoommatesHired [i].GetComponent <Flatmate> ().data.Id == EventManagment.Instance.SelectedRoommates [j].GetComponent <Flatmate> ().data.Id) {
						RoommateManager.Instance.SelectedRoommate = RoommateManager.Instance.RoommatesHired [i];
						RoommateManager.Instance.RoommatesHired [i].GetComponent <Flatmate> ().data.EventBusyId = EventManagment.Instance.CurrentEvent.Event_id;
						RoommateManager.Instance.RoommatesHired [i].GetComponent <Flatmate> ().data.BusyType = BusyType.CatwalkEvents;
						RoommateManager.Instance.StartBusyTimerForSelectedRoomMate (600f);
					}
				}
			}

			if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
				AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");

		} else {
			if (HitCount < 5) {
				yield return new WaitForSeconds (TotalTimer / 4f);
				StartCoroutine (GetCatwalkPairAfterRegistration ());
			} else {
				EndTime = DateTime.UtcNow;
				isTimerRunning = false;	
				//TODO Make a pair with AI..
				yield return StartCoroutine (VotingPairManager.Instance.DeleteMyPairCatWalk (EventManagment.Instance.CurrentEvent.Event_id));
				ScreenAndPopupCall.Instance.CloseScreen ();
				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
				ScreenAndPopupCall.Instance.CloseCharacterCamera ();
				//Change according to AI pair
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
			}
		}
	}

}