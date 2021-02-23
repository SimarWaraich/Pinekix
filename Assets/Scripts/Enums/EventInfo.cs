using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Simple_JSON;

public class EventInfo : MonoBehaviour
{

	public string EventName;
	public eventType category;
	public string EventDetails;
	public string EventTheme;
	public int Event_id;

	public DateTime StartTime;
	//	public DateTime RegistrationTime;
	public DateTime EndTime;
    public bool VipSubscription;

	public List<string> ConditionName;
	public List<string> ConditionValue;


	public Dictionary<string, string> conditions;
	//	public bool IsEventCompleted;
	//	public float EventCompletion;
	//	public eType Etype;

	public List<string> RewardName;
	public List<string> RewardValue;

	//	public bool isEventAvailable;
	//	public bool IsGoingOn;
	//	public bool IsRegisterd;
	public bool isForRewards;

	public Dictionary<string, string> EventRewards;

	public Text TimeText;
	public Text TypeOfTimeText;
	public Text DaysText;

    public Text LevelText;

	public Sprite FashionShowBg;
	public Sprite CatwalkBg;
	public Sprite CoopBg;
	public Sprite DecorBg;
	public Sprite SocietyBg;


	void Start ()
	{
		this.transform.GetChild (0).GetComponent<Text> ().text = EventName;
      
		switch (category) {
		case eventType.Fashion_Event:
			GetComponent<Image> ().sprite = FashionShowBg; 
			break;
		case eventType.CatWalk_Event:
			GetComponent<Image> ().sprite = CatwalkBg;  
			break;
		case eventType.CoOp_Event:
			GetComponent<Image> ().sprite = CoopBg;  
			break;
		case eventType.Decor_Event:
			GetComponent<Image> ().sprite = DecorBg; 
			break;
		case eventType.Society_Event:
			GetComponent<Image> ().sprite = SocietyBg; 
			break;
		default:
			GetComponent<Image> ().sprite = FashionShowBg; 
			break;
		}

		transform.GetComponent<Button> ().onClick.AddListener (() => OpenTasks ());

        if (conditions.ContainsKey ("Level")) {
            LevelText.text = "Level - " + int.Parse(conditions["Level"]);
        }
        if(VipSubscription)
            LevelText .text += "Only for Vip subscriber ";

		DaysText.text = "";
		TypeOfTimeText.text = "";
		TimeText.text = "";
	}

	void Update ()
	{
//		if (RegistrationTime > DateTime.UtcNow) {
//			TimeSpan t = RegistrationTime.Subtract (DateTime.UtcNow);
//			DaysText.text = string.Format ("{0}", t.Days);
//			if (t.Days == 0) {
//				TypeOfTimeText.text = "Registration Time left - "; 
//				TimeText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
//			} else
//				TimeText.text = "";
//			TypeOfTimeText.text = "Registration Time left - "; 
//
//		} else 
		if (StartTime > DateTime.UtcNow) {
			DateTime dsghf = DateTime.UtcNow;
			TimeSpan t = StartTime.Subtract (DateTime.UtcNow);
			DaysText.text = string.Format ("{0}", t.Days);
			if (t.Days == 0) {
				TypeOfTimeText.text = "Event starts In - "; 
				TimeText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
			} else
				TimeText.text = string.Format ("{0} {1}", t.Days, "Days Left");
			
			TypeOfTimeText.text = "Event starts In - "; 

		} else if (EndTime > DateTime.UtcNow && StartTime < DateTime.UtcNow) {
			TimeSpan t = EndTime.Subtract (DateTime.UtcNow);

			DaysText.text = string.Format ("{0}", t.Days);
			if (t.Days == 0) {
				TypeOfTimeText.text = "Event Ends In - "; 
				TimeText.text = string.Format ("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
			} else
				TimeText.text = string.Format ("{0} {1}", t.Days, "Days Left");
				
			TypeOfTimeText.text = "Event Ends In - "; 
			
		} else {			
			TypeOfTimeText.text = "Event is already closed"; 
			DaysText.text = string.Format ("{0}", 0);
			TimeText.text = "";
		}
	}

	public void OnBeforeSerialize ()
	{
		conditions.Clear ();
		EventRewards.Clear ();
		for (int i = 0; i < Mathf.Min (ConditionName.Count, ConditionValue.Count); i++) {
			conditions.Add (ConditionName [i], ConditionValue [i]);
		}

		for (int i = 0; i < Mathf.Min (RewardName.Count, RewardValue.Count); i++) {
			EventRewards.Add (RewardName [i], RewardValue [i]);
		}
	}


	void OpenTasks ()
	{
		SelectEvent ();
		var tut = GameManager.Instance.GetComponent<Tutorial> ();

		VotingPairManager.Instance.viewFriends = false;
		VotingPairManager.Instance.viewStatus = false;

		if (PlayerPrefs.GetInt ("Tutorial_Progress") == 14) {			
//			IsGoingOn = true;
			tut.FashionEventStart ();
			if (EventManagment.Instance.EventType == eType.Event && !tut.Registered) {
                EventIntro ();
                ScreenAndPopupCall.Instance.CloseCharacterCamera();
			} else if (!tut._FashionEventCompleate && tut.Registered) {
				EventVoting ();
			}
		} else if (PersistanceManager.Instance.GetEventToClaimRewards ().Contains (Event_id) || isForRewards) {
			ClaimRewards ();
            ScreenAndPopupCall.Instance.CloseCharacterCamera();
        } else {
			if (StartTime > DateTime.UtcNow) {
				TimeSpan t = StartTime.Subtract (DateTime.UtcNow);
				string TimeString;
				if (t.Days == 0)
					TimeString = string.Format ("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds) + "minutes";
				else
					TimeString = string.Format ("{0:D2} ", t.Days) + "Day(s)";
				
				ShowPopUpWithMessage ("This event will start in next " + TimeString);
				
			} else if (EndTime > DateTime.UtcNow && StartTime < DateTime.UtcNow) {
				if (CheckIfConditionTrue () == true) {
					EventOnlineIntro ();
                    ScreenAndPopupCall.Instance.CloseCharacterCamera();
				}
			} else if (EndTime < DateTime.UtcNow) {
				//					Rewards ();
				ShowPopUpWithMessage ("Event is already closed");
			}			
		}
		if (GetComponent<Badge> ().thisIndication != null) {
			IndicationManager.Instance.ShowBadges (GetComponent<Badge> ().thisIndication, 4);
			IndicationManager.Instance.DeleteIndication (Event_id, "Event");
		}
	}

	bool IsFlatMateAvailable (int Count)
	{
		var CheckCount = 0;

		for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
			if (!RoommateManager.Instance.RoommatesHired [i].GetComponent <Flatmate> ().data.IsBusy)
				CheckCount++;
		}
		return CheckCount >= Count;		
	}

	IEnumerator CheckIfIHaveRegistered (int societyId = 0)
	{
		VotingPairManager.Instance.viewFriends = false;
		VotingPairManager.Instance.viewStatus = false;

		var cd = new CoroutineWithData (EventManagment.Instance, EventManagment.Instance.IePlayerCount (Event_id, societyId));
		yield return cd.coroutine;
		VotingPairManager.Instance.viewFriends = false;
		VotingPairManager.Instance.viewStatus = false;
		if (cd.result != null) {
			bool alreadyRegistered = (bool)cd.result;
			if (alreadyRegistered) {				
				ShowPopUpWithMessage ("You have already registered in this event");
			} else {
				
				if (category == eventType.Fashion_Event) {
					ScreenManager.Instance.ClosePopup ();
					ScreenAndPopupCall.Instance.ShowCharacterSelectionForEvent ();
				} else if (category == eventType.Decor_Event) {
					ScreenManager.Instance.OpenedCustomizationScreen = "DecorEvent";
					ScreenManager.Instance.ClosePopup ();
					RoomPurchaseManager.Instance.DecorEventButonAction ();
					EventManagment.Instance._createButton.gameObject.SetActive (false);
					ScreenAndPopupCall.Instance.DecorEventRoomScreenSelection ();
					RoomPurchaseManager.Instance.CreateRoomForDecorEvent ();
				} else if (category == eventType.CatWalk_Event) {
					ScreenManager.Instance.OpenedCustomizationScreen = "CatWalkEvent";
					ScreenManager.Instance.ClosePopup ();
					EventManagment.Instance.SelectedRoommates.Clear ();
					ScreenManager.Instance.CatWalkCharacterDressUp.GetComponent<CatwalkCharacterDressUp> ().ChangedDressChars.Clear ();
					ScreenAndPopupCall.Instance.ShowCharacterSelectionForCatWalk ();
				} else if (category == eventType.CoOp_Event) {
					ScreenManager.Instance.ClosePopup ();
					MultiplayerManager.Instance.ConnectToServerforCoOp ();
				} else if (category == eventType.Society_Event) {
					//TODO Society Events Calls
					ScreenManager.Instance.ClosePopup ();
					ScreenAndPopupCall.Instance.ShowCharacterSelectionForSocietyEvent ();
				}
			}
		} else {
				
		}
	
	}


	bool CheckIfConditionTrue ()
	{
        if(VipSubscription && !VipSubscriptionManager.Instance.VipSubscribed)
        {
            ShowPopUpWithMessage ("This event is only available for Vip subscribers. Buy Vip subscription to participate");
            return false;
        }

		if (conditions.ContainsKey ("Coins")) {
			if (PlayerPrefs.GetInt ("Money") < int.Parse (conditions ["Coins"])) {
				ShowPopUpWithMessage (string.Format ("You need {0} coins to participate in this Event", int.Parse (conditions ["Coins"]).ToString ()));
				return false;
			}
		}
		if (conditions.ContainsKey ("Gems")) {
			if (GameManager.Instance.Gems < int.Parse (conditions ["Gems"])) {
				ShowPopUpWithMessage (string.Format ("You need {0} gems to participate in this Event", int.Parse (conditions ["Gems"]).ToString ()));		
				return false;
			} 

			if (conditions.ContainsKey ("Level")) {
				if (GameManager.Instance.level < int.Parse (conditions ["Level"])) {
					ShowPopUpWithMessage (string.Format ("You need level {0} to participate in this Event", int.Parse (conditions ["Level"]).ToString ()));	
					return false;
				}
			}
		}
		return true;
	}

	public void ShowPopUpWithMessage (string message)
	{		
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}


	public void EventIntro ()
	{
		if (category == eventType.Fashion_Event) {
			var tut = GameManager.Instance.GetComponent<Tutorial> ();

			ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (false);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "ENTER";
			if (tut._FashionEventCompleate && !tut.enabled) {
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (true);
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (true);
			} else {
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (false);
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (false);
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").gameObject.SetActive (false);
			}
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();
			//		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.OpenedCustomizationScreen = "FashionEventDressUp");
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Event);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.ShowCharacterSelectionForEvent ());
		}
	}

	public void EventOnlineIntro ()
	{
		ScreenManager.Instance.EventsIntroScreen.transform.GetChild (0).FindChild ("Back").GetComponent<Button> ().interactable = true;
		if (category == eventType.CatWalk_Event) {
			EventManagment.Instance.SelectedRoommates.Clear ();
			ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (true);

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "ENTER";


			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => {
				if (IsFlatMateAvailable (3))
					StartCoroutine (CheckIfIHaveRegistered ());
				else
					ShowPopOfDescription ("Your all flatmates are busy.So you require at least 3 flatmate to participate in this event", null);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.AddListener (() => { 

				if (VotingPairManager.Instance.CheckVotingBonus (Event_id)) {
					VotingPairManager.Instance.viewFriends = false;
					VotingPairManager.Instance.pairIndex = 0;
					VotingPairManager.Instance.viewStatus = false;
					VotingPairManager.Instance.GetCatWalkPairs (false);
				} else {
					ShowPopOfDescription ("You have used your all 10 votes for this Event. You can refresh it with 5 gems", null, () => {
						ShowPopOfDescription ("Doing this will erase your acquired voting bonus, if you haven't already applied it so far. " +
						"You can use your acquired Votig bonus first and then refresh your cooldown too. Do you want to continue and refresh it anyway or wait?", () => {

							PersistanceManager.Instance.DeleteThisVotingBonus (Event_id);
							PlayerPrefs.SetInt ("VotedCount" + Event_id + PlayerPrefs.GetInt ("PlayerId"), 0);
							PlayerPrefs.DeleteKey ("VotingRefreshTime" + Event_id + PlayerPrefs.GetInt ("PlayerId"));
							VotingPairManager.Instance.InitBonuses ();
							GameManager.Instance.SubtractGems (5);
						});
					});
				}
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.OnViewFriendsClicked ());//TODO change webservice

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => { 
				ScreenAndPopupCall.Instance.ResultPanelClose ();
				EventManagment.Instance.LeaderBoard (category.ToString (), Event_id);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().onClick.AddListener (() => { 
				ScreenManager.Instance.ClosePopup ();
				VotingPairManager.Instance.viewFriends = false;
				VotingPairManager.Instance.pairIndex = 0;
				VotingPairManager.Instance.viewStatus = true;	
				VotingPairManager.Instance.CatWalk_ShowMyPair ();				 		
			});

		} else if (category == eventType.CoOp_Event) {

			ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").gameObject.SetActive (false);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "ENTER";
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (true);

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => {
				if (IsFlatMateAvailable (1))
					StartCoroutine (CheckIfIHaveRegistered ());
				else
					ShowPopOfDescription ("Your all flatmates are busy.So you require at least 1 flatmate to participate in this event", null);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.AddListener (() => { 

				if (VotingPairManager.Instance.CheckVotingBonus (Event_id)) {	
					VotingPairManager.Instance.viewFriends = false;
					VotingPairManager.Instance.pairIndex = 1;
					VotingPairManager.Instance.viewStatus = false;
					VotingPairManager.Instance.GetAllVotingPair_coOp (false);
				} else {
					ShowPopOfDescription ("You have used your all 10 votes for this Event. You can refresh it with 5 gems", null, () => {
						ShowPopOfDescription ("Doing this will erase your acquired voting bonus, if you haven't already applied it so far. " +
						"You can use your acquired Votig bonus first and then refresh your cooldown too. Do you want to continue and refresh it anyway or wait?", () => {

							PersistanceManager.Instance.DeleteThisVotingBonus (Event_id);
							PlayerPrefs.SetInt ("VotedCount" + Event_id + PlayerPrefs.GetInt ("PlayerId"), 0);
							PlayerPrefs.DeleteKey ("VotingRefreshTime" + Event_id + PlayerPrefs.GetInt ("PlayerId"));
							VotingPairManager.Instance.InitBonuses ();
							GameManager.Instance.SubtractGems (5);
						});
					});
				}
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.OnViewFriendsClicked ());//TODO change webservice

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => { 
				ScreenAndPopupCall.Instance.ResultPanelClose ();
				EventManagment.Instance.LeaderBoard (category.ToString (), Event_id);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().onClick.AddListener (() => { 
				ScreenManager.Instance.ClosePopup ();
				VotingPairManager.Instance.viewFriends = false;
				VotingPairManager.Instance.pairIndex = 0;
				VotingPairManager.Instance.viewStatus = true;		            
				StartCoroutine (VotingPairManager.Instance.GetMyPair_CoOp (1, false));                	
			});
		} else if (category == eventType.Decor_Event) {
			///To Set That Event Is On Decor
			VotingPairManager.Instance.SelectedEventType = "Decor";
			ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").gameObject.SetActive (false);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "ENTER";
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (true);

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => {
				EventManagment.Instance.EventType = eType.Event;
				if (IsFlatMateAvailable (1))
					StartCoroutine (CheckIfIHaveRegistered ());
				else
					ShowPopOfDescription ("Your all flatmates are busy.So you require at least 1 flatmate to participate in this event", null);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.AddListener (() => { 
				EventManagment.Instance.EventType = eType.Voting;
				if (VotingPairManager.Instance.CheckVotingBonus (Event_id)) {
					VotingPairManager.Instance.viewFriends = false;
					VotingPairManager.Instance.pairIndex = 0;
					VotingPairManager.Instance.viewStatus = false;
					VotingPairManager.Instance.ShowOnePairOnScreenOfDecor (false);
				} else {
					ShowPopOfDescription ("You have used your all 10 votes for this Event. You can refresh it with 5 gems", null, () => {
						ShowPopOfDescription ("Doing this will erase your acquired voting bonus, if you haven't already applied it so far. " +
						"You can use your acquired Votig bonus first and then refresh your cooldown too. Do you want to continue and refresh it anyway or wait?", () => {
							PersistanceManager.Instance.DeleteThisVotingBonus (Event_id);
							PlayerPrefs.SetInt ("VotedCount" + Event_id + PlayerPrefs.GetInt ("PlayerId"), 0);
							PlayerPrefs.DeleteKey ("VotingRefreshTime" + Event_id + PlayerPrefs.GetInt ("PlayerId"));
							VotingPairManager.Instance.InitBonuses ();
							GameManager.Instance.SubtractGems (5);
						});
					});
				}
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.AddListener (() => {
				EventManagment.Instance.EventType = eType.Voting;
				EventManagment.Instance.OnViewFriendsClicked ();
			});

//			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => { 
//				ScreenAndPopupCall.Instance.ResultPanelClose ();
//				EventManagment.Instance.LeaderBoard (category.ToString (), Event_id);
//			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().onClick.AddListener (() => { 
				EventManagment.Instance.EventType = eType.Voting;
				ScreenManager.Instance.ClosePopup ();
				VotingPairManager.Instance.viewFriends = false;
				VotingPairManager.Instance.pairIndex = 0;
				VotingPairManager.Instance.viewStatus = true;	
				VotingPairManager.Instance.ShowMyPairOnScreenOfDecor (false);
			});
		} else if (category == eventType.Fashion_Event) {

			ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").gameObject.SetActive (false);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "ENTER";
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (true);

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => {
				if (IsFlatMateAvailable (1))
					StartCoroutine (CheckIfIHaveRegistered ());
				else
					ShowPopOfDescription ("Your all flatmates are busy.So you require at least 1 flatmate to participate in this event", null);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.AddListener (() => { 
				if (VotingPairManager.Instance.CheckVotingBonus (Event_id)) {	
					VotingPairManager.Instance.viewFriends = false;
					VotingPairManager.Instance.pairIndex = 1;
					VotingPairManager.Instance.viewStatus = false;
					VotingPairManager.Instance.GetVotingPair_Fashion (false);
				} else {
					ShowPopOfDescription ("You have used your all 10 votes for this Event. You can refresh it with 5 gems", null, () => {

						ShowPopOfDescription ("Doing this will erase your acquired voting bonus, if you haven't already applied it so far. You can use your acquired Votig bonus first and then refresh your cooldown too. Do you want to continue and refresh it anyway or wait?", () => {
							
							PersistanceManager.Instance.DeleteThisVotingBonus (Event_id);
							PlayerPrefs.SetInt ("VotedCount" + Event_id + PlayerPrefs.GetInt ("PlayerId"), 0);
							PlayerPrefs.DeleteKey ("VotingRefreshTime" + Event_id + PlayerPrefs.GetInt ("PlayerId"));
							VotingPairManager.Instance.InitBonuses ();
							GameManager.Instance.SubtractGems (5);
						});
					});
				}
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.OnViewFriendsClicked ());//TODO change webservice

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => { 
				ScreenAndPopupCall.Instance.ResultPanelClose ();
				EventManagment.Instance.LeaderBoard (category.ToString (), Event_id);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().onClick.AddListener (() => { 
				ScreenManager.Instance.ClosePopup ();
				VotingPairManager.Instance.viewFriends = false;
				VotingPairManager.Instance.pairIndex = 0;
				VotingPairManager.Instance.viewStatus = true;   
				VotingPairManager.Instance.GetMyPair_Fashion ();		
			});
		} else if (category == eventType.Society_Event) {

			ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "ENTER";

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").gameObject.SetActive (false);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (true);

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => {
				if (IsFlatMateAvailable (1))
					StartCoroutine (OnClickEnterForSocietyEvent ());
				else
					ShowPopOfDescription ("Your all flatmates are busy.So you require at least 1 flatmate to participate in this event", null);
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.AddListener (() => {
				if (VotingPairManager.Instance.CheckVotingBonus (Event_id)) {
					VotingPairManager.Instance.viewFriends = false;
					VotingPairManager.Instance.pairIndex = 0;
					VotingPairManager.Instance.viewStatus = false;
					StartCoroutine (VotingPairManager.Instance.IeGetPairSocietyEvent (false));
				} else {
					ShowPopOfDescription ("You have used your all 10 votes for this Event. You can refresh it with 5 gems", null, () => {

						ShowPopOfDescription ("Doing this will erase your acquired voting bonus, if you haven't already applied it so far. You can use your acquired Votig bonus first and then refresh your cooldown too. Do you want to continue and refresh it anyway or wait?", () => {

							PersistanceManager.Instance.DeleteThisVotingBonus (Event_id);
							PlayerPrefs.SetInt ("VotedCount" + Event_id + PlayerPrefs.GetInt ("PlayerId"), 0);
							PlayerPrefs.DeleteKey ("VotingRefreshTime" + Event_id + PlayerPrefs.GetInt ("PlayerId"));
							VotingPairManager.Instance.InitBonuses ();
							GameManager.Instance.SubtractGems (5);
						});
					});
				}
			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.AddListener (() => {
				EventManagment.Instance.OnViewFriendsClicked ();
			});//TODO change webservice

//			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => { 
//				ScreenAndPopupCall.Instance.ResultPanelClose ();
////				EventManagment.Instance.LeaderBoard (category.ToString (), Event_id);
//			});

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().onClick.AddListener (() => { 
				ScreenManager.Instance.ClosePopup ();

				VotingPairManager.Instance.viewFriends = false;
				VotingPairManager.Instance.pairIndex = 0;
				VotingPairManager.Instance.viewStatus = true;

				StartCoroutine (VotingPairManager.Instance.GetMyPair_SocietyEvent ());	

			});

		}
	}

	void EventVoting ()
	{

		if (category == eventType.Fashion_Event) {

			ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (true);
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (false);

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.AddListener (() => { 
				EventManagment.Instance.EventType = eType.Voting;
				ScreenManager.Instance.ClosePopup (); 
				GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ();
				StartCoroutine (ScreenAndPopupCall.Instance.ActiveCameraForVoting (RoommateManager.Instance.SelectedRoommate));

				StartCoroutine (ScreenAndPopupCall.Instance.ActiveCameraForAIVoting (RoommateManager.Instance.RoommatesHired [1]));
				ScreenAndPopupCall.Instance.VotingScreenSelection ();
				VotingManager.Instance.OnIntelazition ();
			});		
		}
	}

	//	IEnumerator OnClickViewStatusForSocietyEvent()
	//	{
	//
	//	}

	IEnumerator OnClickEnterForSocietyEvent ()
	{
		int SocietyId = 0;

		if (SocietyManager.Instance._mySociety != null && SocietyManager.Instance._mySociety.Id != 0) {
			SocietyId = SocietyManager.Instance._mySociety.Id;
			StartCoroutine (CheckIfIHaveRegistered (SocietyManager.Instance._mySociety.Id));
		} else {
			var cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.IGetSocieties (SocietyManager.SeachType.mine, "", true));
			yield return cd.coroutine;
			if (cd.result != null) {
				StartCoroutine (CheckIfIHaveRegistered (SocietyManager.Instance._mySociety.Id));
			} else {
				ShowPopOfDescription ("You are not a member of any Society, Please join or create a new society", () => ScreenAndPopupCall.Instance.ShowSocietyList ());
			}
		}
	}

	void ClaimRewards ()
	{
		ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to " + EventName.ToString () + " " + EventDetails.ToString ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();

		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (false);
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "CLAIM REWARDS";
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").gameObject.SetActive (false);
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").gameObject.SetActive (false);
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").gameObject.SetActive (false);

		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => {
			StartCoroutine (GetMyPairToShowRewards (Event_id));
		});

	}

	IEnumerator GetMyPairToShowRewards (int Event_Id)
	{
		var myId = PlayerPrefs.GetInt ("PlayerId");
		if (category == eventType.Fashion_Event) {
			var cd = new CoroutineWithData (this, VotingPairManager.Instance.IeGetMyPair_Fashion (1, Event_Id, true));
			yield return cd.coroutine;

			if (cd.result != null) {

				var vp = (VotingPair)cd.result;

				ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player1Name.ToString ();
				ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player2Name.ToString ();
				if (myId == vp.player1_id)
					yield return StartCoroutine (VotingPairManager.Instance.GetVotesResult_Fashion (PlayerPrefs.GetInt ("PlayerId"), vp.player1_flatmate_id, vp.pair_id, Event_Id, true));
				else if (myId == vp.player2_id)
					yield return StartCoroutine (VotingPairManager.Instance.GetVotesResult_Fashion (PlayerPrefs.GetInt ("PlayerId"), vp.player2_flatmate_id, vp.pair_id, Event_Id, true));

			} else {
				ScreenAndPopupCall.Instance.CloseScreen ();
			}
			yield return StartCoroutine (VotingPairManager.Instance.DeleteMyPair (Event_Id, 0));
		} else if (category == eventType.CatWalk_Event) {
			var cd = new CoroutineWithData (this, VotingPairManager.Instance.IeCatWalkShowMyPair (1, Event_Id, true));
			yield return cd.coroutine;

			if (cd.result != null) {
				var vp = (CatWalkVotingPair)cd.result;               
				ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player1Name.ToString ();
				ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player2Name.ToString ();

				yield return StartCoroutine (VotingPairManager.Instance.GetVotesResult_CatWalk (PlayerPrefs.GetInt ("PlayerId"), vp.pair_id, Event_Id, true));
			} else {
				ScreenAndPopupCall.Instance.CloseScreen ();
			}
			yield return StartCoroutine (VotingPairManager.Instance.DeleteMyPairCatWalk (Event_Id));

		} else if (category == eventType.CoOp_Event) {
			var cd = new CoroutineWithData (EventManagment.Instance, EventManagment.Instance.GetCoOpRegistration (Event_Id));
			yield return cd.coroutine;
			var vp = new CoOpVotingPair ();
			if (cd.result != null) {
				var Registered = (CoopRegisterPlayers)cd.result;

				var newcd = new CoroutineWithData (VotingPairManager.Instance, VotingPairManager.Instance.IeGetMyPair_CoOp (Registered.Player1Id, Registered.Player2Id, 1, true));
				yield return newcd.coroutine;

				if (newcd.result != null) {
					vp = (CoOpVotingPair)newcd.result;

					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Name_Text").GetComponent<Text> ().text = vp.set1_player1Name + "/" + vp.set1_player2Name;
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Name_Text").GetComponent<Text> ().text = vp.set2_player1Name + "/" + vp.set2_player2Name;

					yield return StartCoroutine (VotingPairManager.Instance.GetVotesResult_CoOp (vp.event_id, vp.pair_id, true));
				}
				yield return StartCoroutine (VotingPairManager.Instance.DeleteMyPair_Coop (Registered.EventId, Registered.Player1Id, Registered.Player2Id));
			} else {
				print ("Player not found in registered table");
				ScreenAndPopupCall.Instance.CloseScreen ();
			}
		} else if (category == eventType.Society_Event) {
			var cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.IGetSocieties (SocietyManager.SeachType.mine, "", true));
			yield return cd.coroutine;
			if (cd.result != null) {
				var newcd = new CoroutineWithData (this, VotingPairManager.Instance.IeGetMyPair_SocietyEvent (1, Event_id, SocietyManager.Instance._mySociety.Id, true));
				yield return newcd.coroutine;

				if (newcd.result != null) {
					var vp = (SocietyVotingPair)newcd.result;

					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player1Name;
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player2Name;

					yield return StartCoroutine (VotingPairManager.Instance.GetVotesResultSocietyEvent (PlayerPrefs.GetInt ("PlayerId"), vp.pair_id, Event_Id, true));

				} else {
					ScreenAndPopupCall.Instance.CloseScreen ();
				}
				yield return StartCoroutine (VotingPairManager.Instance.DeleteMyPair_Society (Event_id));
			}

		} else if (category == eventType.Decor_Event) {
			var cd = new CoroutineWithData (VotingPairManager.Instance, VotingPairManager.Instance.IeShowMyPairOnScreenOfDecor (1, true));
			yield return cd.coroutine;

			if (cd.result != null) {

				var vp = (VotingPairForDecor)cd.result;

				ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player1Name;
				ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Name_Text").GetComponent<Text> ().text = vp.player2Name;
				yield return StartCoroutine (VotingPairManager.Instance.VoteForPlayer_Decor (PlayerPrefs.GetInt ("PlayerId"), vp.event_id, vp.pair_id, true));
			} else {
				ScreenAndPopupCall.Instance.CloseScreen ();
			}
			yield return StartCoroutine (VotingPairManager.Instance.DeleteMyPairDecor (Event_Id));
		}
		PersistanceManager.Instance.DeleteEventFromClaimedRewards (Event_id);
	}

	void Rewards ()
	{
		StartCoroutine (IeReward ());
	}

	IEnumerator IeReward ()
	{
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
		if (category == eventType.CatWalk_Event) {

//			yield return VotingPairManager.Instance.ResultShow ("CatWalk_Event", Event_id);
//			yield return VotingPairManager.Instance.StartCoroutine (VotingPairManager.Instance.GetAllVotingPairs_Catwalk_ForCheck ());
//
//			if (VotingPairManager.Instance.AllPlayerCatWalkPair.Count != 0) {
//
//				ScreenManager.Instance.OpenedCustomizationScreen = "CatWalkEvent";
//				ScreenAndPopupCall.Instance.RewardScreenSelection ();
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.ClaimReward (Event_id, RewardValue [0], EventName));
//
//				var resultobj = VotingPairManager.Instance.result.Find (item => (item.player1_id == PlayerPrefs.GetInt ("PlayerId") || item.player2_id == PlayerPrefs.GetInt ("PlayerId")));
//			
//
//				if (resultobj != null && resultobj.player1_id == PlayerPrefs.GetInt ("PlayerId")) {
//					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
//					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;
//
//					if (player1_score >= player2_score) {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
//					} else {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
//					}
//				}
//
//				if (resultobj != null && resultobj.player2_id == PlayerPrefs.GetInt ("PlayerId")) {
//					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
//					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;
//
//					if (player2_score >= player1_score) {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
//					} else {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
//					}
//				}
//
//				if (resultobj == null) {
//					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
//				}
//
//				if (VotingPairManager.Instance.result.Count == 1) {
//					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (false);
//				} else {
//					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (true);
//				}
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.ResultPanelClose ());
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.LeaderBoard ("CatWalk_Event", Event_id));
//			} else {
			// not participated
			ShowPopOfDescription ("You didn't participated in this event... You can try next time...", null);	
			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
//			}

		}

		///TODO: Change the code for coop
		if (category == eventType.CoOp_Event) {

//			yield return VotingPairManager.Instance.ResultShow ("CoOp_Event", Event_id);
//			yield return VotingPairManager.Instance.StartCoroutine (VotingPairManager.Instance.GetAllVotingPairs_CoOpForCheck ());
//
//			if (VotingPairManager.Instance.MyPlayersInCoOp.Count != 0) {
//
//				ScreenManager.Instance.OpenedCustomizationScreen = "CoOpEvent";
//				ScreenAndPopupCall.Instance.RewardScreenSelection ();
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.ClaimReward (Event_id, RewardValue [0], EventName));
//
//				var resultobj = VotingPairManager.Instance.result.Find (item => (item.player1_id == PlayerPrefs.GetInt ("PlayerId") || item.player2_id == PlayerPrefs.GetInt ("PlayerId")));
//
//				if (resultobj.player1_id == PlayerPrefs.GetInt ("PlayerId")) {
//					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
//					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;
//
//					if (player1_score >= player2_score) {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
//					} else {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
//					}
//				}
//
//				if (resultobj.player2_id == PlayerPrefs.GetInt ("PlayerId")) {
//					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
//					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;
//
//					if (player2_score >= player1_score) {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
//					} else {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
//					}
//				}
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.ResultPanelClose ());
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.LeaderBoard ("CoOp_Event",Event_id));
//			} else {
			// not participated
			ShowPopOfDescription ("You didn't participated in this event... You can try next time...", null);	
			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
//			}

		}
		if (category == eventType.Decor_Event) {

//			yield return VotingPairManager.Instance.ResultShow ("Decor_Event", Event_id);
//			yield return VotingPairManager.Instance.StartCoroutine (VotingPairManager.Instance.GetAllVotingPairs_Decor_ForCheck ());
//
//			if (VotingPairManager.Instance.AllPlayerPairOfDecorEvent.Count != 0) {
//
//				ScreenManager.Instance.OpenedCustomizationScreen = "DecorEvent";
//				ScreenAndPopupCall.Instance.RewardScreenSelection ();
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.ClaimReward (Event_id, RewardValue [0], EventName));
//			
//
//				var resultobj = VotingPairManager.Instance.result.Find (item => (item.player1_id == PlayerPrefs.GetInt ("PlayerId") || item.player2_id == PlayerPrefs.GetInt ("PlayerId")));
//
//				if (resultobj.player1_id == PlayerPrefs.GetInt ("PlayerId")) {
//					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
//					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;
//
//					if (player1_score >= player2_score) {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
//					} else {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
//					}
//				}
//
//				if (resultobj.player2_id == PlayerPrefs.GetInt ("PlayerId")) {
//					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
//					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;
//
//					if (player2_score >= player1_score) {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
//					} else {
//						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
//					}
//				}
//
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.ResultPanelClose ());
//
//				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.LeaderBoard ("Decor_Event", Event_id));
//			} else {
			// not participated
			ShowPopOfDescription ("You didn't participated in this event... You can try next time...", null);	
			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
//			}
		}
		if (category == eventType.Fashion_Event) {
			yield return VotingPairManager.Instance.ResultShow ("Fashion_Event", Event_id);
			yield return VotingPairManager.Instance.StartCoroutine (VotingPairManager.Instance.GetAllVotingPairs_Fashion_ForCheck ());

			if (VotingPairManager.Instance.AllPlayerPair.Count != 0) {

				ScreenManager.Instance.OpenedCustomizationScreen = "FashionEvent";
				ScreenAndPopupCall.Instance.RewardScreenSelection ();
				ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.ClaimReward (Event_id, RewardValue [0], EventName));
			
				var resultobj = VotingPairManager.Instance.result.Find (item => (item.player1_id == PlayerPrefs.GetInt ("PlayerId") || item.player2_id == PlayerPrefs.GetInt ("PlayerId")));

				if (resultobj.player1_id == PlayerPrefs.GetInt ("PlayerId")) {
					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;

					if (player1_score >= player2_score) {
						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
					} else {
						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
					}
				}

				if (resultobj.player2_id == PlayerPrefs.GetInt ("PlayerId")) {
					var player1_score = (resultobj.player1_voteCount * 10) + resultobj.player1_friendBonus + resultobj.player1_votingBonus;
					var player2_score = (resultobj.player2_voteCount * 10) + resultobj.player2_friendBonus + resultobj.player2_votingBonus;

					if (player2_score >= player1_score) {
						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = true;
					} else {
						ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().interactable = false;
					}
				}


				if (VotingPairManager.Instance.result.Count == 1) {
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (false);
				} else {
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (true);
				}


				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.RemoveAllListeners ();

				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.ResultPanelClose ());
				ScreenManager.Instance.ResultScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.LeaderBoard ("Fashion_Event", Event_id));
			} else {
				// not participated
				ShowPopOfDescription ("You didn't participated in this event... You can try next time...", null);	
				ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
			}
		}


	}

	void ShowPopOfDescription (string Message, UnityEngine.Events.UnityAction OnClickOkAction, UnityEngine.Events.UnityAction OnClickCloseAction = null)
	{
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		if (OnClickCloseAction != null) {
			ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Refresh";
			ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		} else {
			ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
			ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		}

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = Message;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickOkAction != null) {
				OnClickOkAction ();
			}
		});	

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickCloseAction != null) {
				OnClickCloseAction ();
			}
		});	
	}





	public void SelectEvent ()
	{
		EventManagment.Instance.EventSelection (EventName);
		EventManagment.Instance.category = category;
	}
}

