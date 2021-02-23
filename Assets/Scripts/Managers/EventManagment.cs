using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Simple_JSON;

//using System.Xml.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using System.Linq;

/// <summary>
/// E type.declaration for selected event type weather its type is event or voting 
/// </summary>
public enum eType
{
	Event = 0,
	Voting = 1

}

public enum eventType
{
	none = 0,
	Fashion_Event = 1,
	Decor_Event = 2,
	CatWalk_Event = 3,
	CoOp_Event = 4,
	Society_Event = 5
}

[Serializable]
/// <summary>
/// Event mangmet struct. are used for declare the variable of the Event type fucntion 
/// </summary>
public class EventManagmentStruct
{
	public eventType category;
	public string EventTitle;
	public string EventDescription;
	public int Event_id;
	public string EventTheme;

	public List<string> ConditionName = new List<string> ();
	public List<string> ConditionValue = new List<string> ();
    public int VipSubscription;
	//	public eType EventType;
	//	public bool IsEventCompleted;
	//	public float EventCompletion;
	//	public bool IsGoingOn;

	public bool IsForRewards;
	public DateTime StartTime;
	public DateTime RegistrationTime;
	public DateTime EndTime;
	public List<string> Reward = new List<string> ();
	public List<string> RewardValue = new List<string> ();

}

/// <summary>
/// Event managment. is Singleton script which is managing the whole event and it atribute
/// </summary>
public class EventManagment : Singleton<EventManagment>
{

	public List <EventManagmentStruct> AllEventList;

	public EventDataStructure[] NewEvents;

	//	public EventDataStructure[] CompletedEvents;

	public EventDataStructure CurrentEvent;

	public List<GameObject> SelectedRoommates = new List<GameObject> ();

	public GameObject EventListPrefab;
	public GameObject EventContainer;
	public GameObject registrationButton;

	public GameObject LeaderboardPrefab;
	public GameObject LeaderboardContainer;

	public GameObject RewardPrefab;
	public GameObject RewardContainer;

	public eType EventType;
	public eventType category;

	public Dictionary<int, string> DecorEventList = new Dictionary<int, string> ();
	public List<RewardObject> Rewards = new List<RewardObject> ();


	[Header ("Decor Event Button")]
	public Button _wallButton;
	public Button _decorButton;
	public Button _groundButton;
	public Button _registerButton;
	public Button _createButton;

	public Dictionary<int, int> PlayerRegistered = new Dictionary<int, int> ();


//	string path;




	void Awake ()
	{
//		var Temp = new List<int> ();
//		Temp.Add (42);
//
//		PersistanceManager.Instance.SaveClaimRewardsEvents (Temp);
//
//		var AllEvents = PersistanceManager.Instance.GetEventToClaimRewards ();
//
//		foreach( var id in AllEvents){
//			Debug.LogError ("Event id to claim rewards is ------->>>>>>>>>>>>" + id);
//		}

		this.Reload ();
//		path = Application.persistentDataPath + "event.bytes";
	}

	public void EventRegisterType ()
	{
		EventType = eType.Event;
	}

	public void VotingType ()
	{
		EventType = eType.Voting;
	}

	public void OnViewFriendsClicked ()
	{
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().interactable = true;

		VotingPairManager.Instance.viewFriends = true;
		VotingPairManager.Instance.viewStatus = false;
		VotingPairManager.Instance.pairIndex = 0;

		if (EventManagment.Instance.category == eventType.Fashion_Event)
			StartCoroutine (VotingPairManager.Instance.GetFriendInFashionEvent ());
		else if (EventManagment.Instance.category == eventType.CatWalk_Event) {
			StartCoroutine (VotingPairManager.Instance.GetFriendInCatWalkEvent ());
		} else if (EventManagment.Instance.category == eventType.Decor_Event) {
			StartCoroutine (VotingPairManager.Instance.GetFriendInDecorEvent ());
		} else if (EventManagment.Instance.category == eventType.CoOp_Event)
			StartCoroutine (VotingPairManager.Instance.GetFriendInCoOpEvent ());
		else if (EventManagment.Instance.category == eventType.Society_Event)
			StartCoroutine (VotingPairManager.Instance.GetFriendInSocietyEvent ());
	}


	/// <summary>
	/// Gets the event data from server. this functiion will get all currnt event data form the server 
	/// </summary>
	public void GetEventDataFromServer ()
	{
		/// this if condition will check if event tutorial is done then fetch the event data from the server else use locle event data
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (Tut._FashionEventCompleate) {
//			category = (eventType)Eventcategory;
			if (!ParenteralController.Instance.activateParentel) {
				StartCoroutine (GetEventData ());
			} else {
				ParenteralController.Instance.ShowPopUpMessageForParentel ();
			}

		} else
			CheckAllEvents ();
	}

	IEnumerator GetEventData ()
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();
		CoroutineWithData cd = new CoroutineWithData (ConnectionController.Instance, ConnectionController.Instance.IeCheckServices ());
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {

			PlayerRegistered.Clear ();

			var encoding = new System.Text.UTF8Encoding ();


			Dictionary<string,string> postHeader = new Dictionary<string,string> ();
			var jsonElement = new Simple_JSON.JSONClass ();

			jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

			postHeader.Add ("Content-Type", "application/json");
			postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

			WWW www = new WWW ("http://pinekix.ignivastaging.com/events/getevents", encoding.GetBytes (jsonElement.ToString ()), postHeader);

//			print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
			yield return www;

			if (www.error == null) {
				JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//				print ("www.text ==>> " + www.text);
//				print ("_jsnode ==>> " + _jsnode.ToString ());

				if (_jsnode ["description"].ToString ().Contains ("Events data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
//					print ("Success");

					JSONNode dataArray = _jsnode ["data"];

					var count = dataArray.Count;
					List<EventManagmentStruct> _events = new List <EventManagmentStruct> ();

					for (int i = 0; i < count; i++) {
						EventManagmentStruct eventdata = new EventManagmentStruct ();

						eventdata.Event_id = int.Parse (dataArray [i] ["event_id"]);

						eventdata.category = (eventType)int.Parse (dataArray [i] ["event_cat"]);
						eventdata.EventTitle = dataArray [i] ["event_name"];
						eventdata.EventDescription = "Let's participate in " + dataArray [i] ["event_theme"] + " Event";
						eventdata.IsForRewards = false;
                        eventdata.EventTheme = dataArray [i] ["event_theme"];
                        eventdata.VipSubscription = int.Parse (dataArray [i] ["event_vip_subscription"]);

						if (dataArray [i].ToString ().Contains ("coins")) {
							eventdata.ConditionName.Add ("Coins");
							eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["fee_coins"]).ToString ());
						}

						if (dataArray [i].ToString ().Contains ("gems")) {
							eventdata.ConditionName.Add ("Gems");
							eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["fee_gems"]).ToString ());
						}

						if (dataArray [i].ToString ().Contains ("level")) {
							eventdata.ConditionName.Add ("Level");
							eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["level_required"]).ToString ());
						}

						eventdata.Reward.Add ("link");
						eventdata.RewardValue.Add (dataArray [i] ["rewards"].ToString ().Trim ('"'));

						DateTime time = Convert.ToDateTime (dataArray [i] ["start_time"]);
						eventdata.StartTime = time;

						DateTime Regtime = Convert.ToDateTime (dataArray [i] ["registration_time"]);
						eventdata.RegistrationTime = Regtime;

						DateTime endtime = Convert.ToDateTime (dataArray [i] ["end_time"]);
						eventdata.EndTime = endtime;

						/// Change value name as per requirements
//						eventdata.category = (eventType)Enum.Parse (typeof(eventType), dataArray [i] ["category"].ToString ());


						var reward = Rewards.Find (item => item.NameofEvent == eventdata.EventTitle);
						//if (reward != null)
						_events.Add (eventdata);

//						yield return StartCoroutine (IePlayerCount (eventdata.Event_id));
					}
					AllEventList = _events;
					yield return StartCoroutine (GetEventsToClaimRewards ());

					CheckAllEvents ();

					yield return true;
				} else {
//					print ("error" + www.error);
					AllEventList.Clear ();
					yield return StartCoroutine (GetEventsToClaimRewards ());
					CheckAllEvents ();

					yield return false;
				}
			} else {
				yield return false;
			}
			ScreenAndPopupCall.Instance.LoadingScreenClose ();
		}
	}


	IEnumerator GetEventsToClaimRewards ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "events";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/unclaimedResults/eventList", encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());


			List<EventManagmentStruct> _events = new List <EventManagmentStruct> ();

			foreach (var previousEvent in AllEventList) {
				_events.Add (previousEvent);
			}

			if (_jsnode ["description"].ToString ().Contains ("Events data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");

				JSONNode dataArray = _jsnode ["data"];

				var count = dataArray.Count;


				for (int i = 0; i < count; i++) {
					EventManagmentStruct eventdata = new EventManagmentStruct ();

					eventdata.Event_id = int.Parse (dataArray [i] ["event_id"]);

					eventdata.category = (eventType)int.Parse (dataArray [i] ["event_cat"]);
					eventdata.EventTitle = dataArray [i] ["event_name"];
					eventdata.EventDescription = "Let's participate in " + dataArray [i] ["event_theme"] + " Event";
					eventdata.IsForRewards = true;
					eventdata.EventTheme = dataArray [i] ["event_theme"];

					if (dataArray [i].ToString ().Contains ("coins")) {
						eventdata.ConditionName.Add ("Coins");
						eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["fee_coins"]).ToString ());
					}

					if (dataArray [i].ToString ().Contains ("gems")) {
						eventdata.ConditionName.Add ("Gems");
						eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["fee_gems"]).ToString ());
					}

					if (dataArray [i].ToString ().Contains ("level")) {
						eventdata.ConditionName.Add ("Level");
						eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["level_required"]).ToString ());
					}

					eventdata.Reward.Add ("link");
					eventdata.RewardValue.Add (dataArray [i] ["rewards"].ToString ().Trim ('"'));

					DateTime time = Convert.ToDateTime (dataArray [i] ["start_time"]);
					eventdata.StartTime = time;

					DateTime Regtime = Convert.ToDateTime (dataArray [i] ["registration_time"]);
					eventdata.RegistrationTime = Regtime;

					DateTime endtime = Convert.ToDateTime (dataArray [i] ["end_time"]);
					eventdata.EndTime = endtime;

					/// Change value name as per requirements
					//						eventdata.category = (eventType)Enum.Parse (typeof(eventType), dataArray [i] ["category"].ToString ());


					var reward = Rewards.Find (item => item.NameofEvent == eventdata.EventTitle);
					//if (reward != null)
					_events.Add (eventdata);
				}
			}


			AllEventList.Clear ();
			var _fashionEvent = new List<EventManagmentStruct> ();
			var _decorEvent = new List<EventManagmentStruct> ();
			var _catWalkEvent = new List<EventManagmentStruct> ();
			var _coOpEvent = new List<EventManagmentStruct> ();
			var _societyEvent = new List<EventManagmentStruct> ();

			foreach (var _event in _events) {
				switch (_event.category) {
				case eventType.Fashion_Event:
					_fashionEvent.Add (_event);
					break;
				case eventType.Decor_Event:
					_decorEvent.Add (_event);
					break;
				case eventType.CatWalk_Event:
					_catWalkEvent.Add (_event);
					break;
				case eventType.CoOp_Event:
					_coOpEvent.Add (_event);
					break;
				case eventType.Society_Event:
					_societyEvent.Add (_event);
					break;
				}
			}

			_fashionEvent.Sort ((E1, E2) => E2.EndTime.CompareTo (E1.EndTime));
			_decorEvent.Sort ((E1, E2) => E2.EndTime.CompareTo (E1.EndTime));
			_catWalkEvent.Sort ((E1, E2) => E2.EndTime.CompareTo (E1.EndTime));
			_coOpEvent.Sort ((E1, E2) => E2.EndTime.CompareTo (E1.EndTime));
			_societyEvent.Sort ((E1, E2) => E2.EndTime.CompareTo (E1.EndTime));


			AllEventList.AddRange (_societyEvent);
			AllEventList.AddRange (_catWalkEvent);
			AllEventList.AddRange (_fashionEvent);
			AllEventList.AddRange (_coOpEvent);
			AllEventList.AddRange (_decorEvent);

//				AllEventList = _events.ToArray ();
			
		}
	}

	public void CheckAllEvents ()
	{

		List<EventDataStructure> _newEvent = new List<EventDataStructure> ();

		List<EventDataStructure> compleateEvent = new List<EventDataStructure> ();

		for (int i = 0; i < AllEventList.Count; i++) {
			var _event = new EventDataStructure ();

			_event.EventName = AllEventList [i].EventTitle;
			_event.EventDetails = AllEventList [i].EventDescription;
			_event.EventTheme = AllEventList [i].EventTheme;
			_event.Event_id = AllEventList [i].Event_id;
            _event.VipSubscription = AllEventList [i].VipSubscription ==1?true:false;
//			_event.Completion = AllEventList [i].EventCompletion;
//			_event.IsCompleted = AllEventList [i].IsEventCompleted;
//			_event.IsGoingOn = AllEventList [i].IsGoingOn;
			_event.isForRewards = AllEventList [i].IsForRewards;
			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (!Tut._FashionEventCompleate) {
//				if (Tut.fashionEvent < 9) {
//					_event.RegistrationTime = DateTime.UtcNow.AddMinutes (5);	
//					_event.StartTime = DateTime.UtcNow.AddMinutes (10);
//					_event.EndTime = DateTime.UtcNow.AddMinutes (15);
//
//				} else if (Tut.fashionEvent > 9) {
				_event.RegistrationTime = DateTime.UtcNow.AddMinutes (-10);	
				_event.StartTime = DateTime.UtcNow.AddMinutes (-5);
				_event.EndTime = DateTime.UtcNow.AddMinutes (5);
//				}
			} else {

				_event.StartTime = AllEventList [i].StartTime;
				_event.EndTime = AllEventList [i].EndTime;
				_event.RegistrationTime = AllEventList [i].RegistrationTime;
			}
			_event.category = AllEventList [i].category;

			for (int x = 0; x < Mathf.Min (AllEventList [i].ConditionName.Count, AllEventList [i].ConditionValue.Count); x++) {
				_event.conditions.Add (AllEventList [i].ConditionName [x], AllEventList [i].ConditionValue [x]);
			}
			for (int x = 0; x < Mathf.Min (AllEventList [i].Reward.Count, AllEventList [i].RewardValue.Count); x++) {
				_event.Rewards.Add (AllEventList [i].Reward [x], AllEventList [i].RewardValue [x]);
			}

//			if (_event.IsCompleted) {
//				compleateEvent.Add (_event);
//			} else {
			_newEvent.Add (_event);
//				
//			}
		}
		        
//		CompletedEvents = compleateEvent.ToArray ();
		NewEvents = _newEvent.ToArray ();
		if (NewEvents.Length == 0) {
			ShowPopOfDescription ("Currently there is no event available. Please come back after sometime!!", () => {
				ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (ScreenAndPopupCall.Instance._CellPhoneCalled);
			});

		} else {
			DeleteOldEvent ();
			CreateNewEvent ();
			ScreenAndPopupCall.Instance.FashionEventListScreenSelection ();
//            EventManagment.Instance.ScrollToName("co op ");
		}

	}

	/// <summary>
	/// Changes the currunt event status. this function will check if availavle event is ongoing and completed bool done then this event data list will move to the compleated event list 
	/// </summary>
	//	public void ChangeCurruntEventStatus ()
	//	{
	//		List<EventDataStructure> _newEvent = new List<EventDataStructure> ();
	//
	//
	//		List<EventDataStructure> compleateEvent = new List<EventDataStructure> ();
	//
	//		for (int i = 0; i < NewEvents.Length; i++) {
	//			_newEvent.Add (NewEvents [i]);
	//		}
	//
	//
	//
	//		for (int i = 0; i < CompletedEvents.Length; i++) {
	//			compleateEvent.Add (CompletedEvents [i]);
	//		}
	//
	//		if (CurrentEvent.IsCompleted) {			
	//			var _eventManagment = EventManagment.Instance.EventContainer.GetComponentInChildren<EventInfo> ();
	//			_eventManagment.isEventAvailable = false;
	//			_eventManagment.IsGoingOn = false;
	//
	//		}
	//
	//		for (int i = 0; i < _newEvent.Count; i++) {
	//			if (_newEvent [i] == CurrentEvent) {
	//				compleateEvent.Add (CurrentEvent);
	//				_newEvent.Remove (_newEvent [i]);
	//				break;
	//			}
	//		}
	//
	//
	//		NewEvents = _newEvent.ToArray ();
	//		CompletedEvents = compleateEvent.ToArray ();
	//
	//		// Give rewards for performed event 
	//		//		GiveRewardsForEvents ();
	//	}

	/// <summary>
	/// Gives the rewards for events. this function is using for Rewards point when event get compleated 
	/// </summary>
	public void GiveRewardsForEvents ()
	{

		foreach (var Key in CurrentEvent.Rewards.Keys) {
			switch (Key) {
			case "ExperiencePoints":
//				PlayerPrefs.SetFloat ("ExperiencePoints" + GameManager.Instance.level, PlayerPrefs.GetFloat ("ExperiencePoints" + GameManager.Instance.level) + int.Parse (CurrentEvent.Rewards ["ExperiencePoints"]));
				GameManager.Instance.AddExperiencePoints (int.Parse (CurrentEvent.Rewards ["ExperiencePoints"]));
				break;
			case "Gems":
//				PlayerPrefs.SetInt ("Gems", PlayerPrefs.GetInt ("Gems") + int.Parse (CurrentEvent.Rewards ["Gems"]));
				GameManager.Instance.AddGemsWithGemBonus (int.Parse (CurrentEvent.Rewards ["Gems"]));
				break;
			case  "Coins":
				GameManager.Instance.AddCoins (int.Parse (CurrentEvent.Rewards ["Coins"]));
				break;
			}
		}

		CurrentEvent = null;
	}

	#region EventListSelection


	/// <summary>
	/// select the event. 
	/// this fuction is using for select the event from the event list 
	/// which event will added to the currnt event and ongoing event
	/// </summary>
	/// <param name="selEvent">Sel event.</param>


	public void EventSelection (string selEvent)
	{
		//		if (CurrentEvent != null)
		//			return;
		for (int i = 0; i < NewEvents.Length; i++) {
			if (NewEvents [i].EventName == selEvent) {
//				NewEvents [i].IsGoingOn = true;

//				for (int x = 0; x < NewEvents.Length; x++) {
//					{
//						if (AllEventList [x].EventTitle == NewEvents [i].EventName)
//							AllEventList [x].IsGoingOn = true;
//					}
				CurrentEvent = NewEvents [i];

				break;
			}
//			}
		}


	}

	#endregion

	/// <summary>
	/// /*Deletes the old event.*/ this fuction is using for delete the event form the list if event are no more valid for currunt time
	/// </summary>
	public void DeleteOldEvent ()
	{
		for (int i = 0; i < EventContainer.transform.childCount; i++) {
			Destroy (EventContainer.transform.GetChild (i).gameObject);
		}
	}

	public void DeleteOldLeaderboardItem ()
	{
		for (int i = 0; i < LeaderboardContainer.transform.childCount; i++) {
			Destroy (LeaderboardContainer.transform.GetChild (i).gameObject);
		}
	}

	public void DeleteoldRewardItems ()
	{
		for (int i = 0; i < RewardContainer.transform.childCount; i++) {
			Destroy (RewardContainer.transform.GetChild (i).gameObject);
		}
	}


	public void CreateNewEvent ()
	{
		DeleteOldEvent ();

		for (int i = 0; i < NewEvents.Length; i++) {
			var EventButton = (GameObject)Instantiate (EventListPrefab, Vector2.zero, Quaternion.identity);

			EventButton.transform.SetParent (EventContainer.transform);
			EventButton.transform.localScale = Vector3.one;
			EventButton.name = NewEvents [i].EventName;
			var _Event = EventButton.GetComponent<EventInfo> ();

			_Event.EventName = NewEvents [i].EventName;
			_Event.Event_id = NewEvents [i].Event_id;
			_Event.category = NewEvents [i].category;
            _Event.VipSubscription = NewEvents [i].VipSubscription;
			_Event.EventDetails = NewEvents [i].EventDetails;
			_Event.EventTheme = NewEvents [i].EventTheme;
//			_Event.EventCompletion = NewEvents [i].Completion;
			_Event.StartTime = NewEvents [i].StartTime;
//			_Event.RegistrationTime = NewEvents [i].RegistrationTime;

			_Event.EndTime = NewEvents [i].EndTime;
			_Event.conditions = NewEvents [i].conditions;
			_Event.isForRewards = NewEvents [i].isForRewards;
//			_Event.IsEventCompleted = NewEvents [i].IsCompleted;
//			_Event.IsGoingOn = NewEvents [i].IsGoingOn;

//			foreach (var keys in NewEvents[i].conditions.Keys) {
//
//				if (keys.Contains ("Level")) {
//					var Value = int.Parse (NewEvents [i].conditions ["Level"]);
//					if (GameManager.Instance.level < Value)
//						_Event.isEventAvailable = false;
//					else
//						_Event.isEventAvailable = true;
//				}
//			}

			foreach (var keys in NewEvents[i].conditions.Keys) {
				_Event.ConditionName.Add (keys);

			}
			foreach (var values in NewEvents[i].conditions.Values) {
				_Event.ConditionValue.Add (values);
			}

			foreach (var keys in NewEvents[i].Rewards.Keys) {
				_Event.RewardName.Add (keys);

			}
			foreach (var values in NewEvents[i].Rewards.Values) {
				_Event.RewardValue.Add (values);
			}
		}

		IndicationManager.Instance.IncrementIndicationFor ("Event", 3);
	}





	#region Fashion_Event

	public void OnRegistration ()
	{
		if (ScreenManager.Instance.OpenedCustomizationScreen == "FashionEventDressUp") {
			OnRegistration_FashionEvent ();
		} else if (ScreenManager.Instance.OpenedCustomizationScreen == "CatWalkEventDressUp") {
			OnCatWalkRegistration ();
		} else if (ScreenManager.Instance.OpenedCustomizationScreen == "DecorEvent") {
			OnDecorEventRegistration ();	
		} else if (ScreenManager.Instance.OpenedCustomizationScreen == "SocietyEventDressUp") {
			OnRegistration_SocietyEvent ();
		}

	}

	public void OnRegistration_SocietyEvent ()
	{
        foreach (var dress in PurchaseDressManager.Instance.selectedDresses)
        {
            if (DressManager.Instance.SelectedCharacter.GetComponent<Flatmate> ().data.Dress.ContainsKey("Dresses"))
            {
                DressManager.Instance.SelectedCharacter.GetComponent<Flatmate> ().data.Dress.Remove("Dresses");

                var allCustom = Resources.Load<CustomisationAllData>("CustomisationAllData");
                if(GameManager.GetGender()== GenderEnum.Female)
                {
                    DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllGirl.BodyPartName.ToArray(), allCustom.EmptyAllGirl.DressesSprites.ToArray());
                }else
                {
                    DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllBoy.BodyPartName.ToArray(), allCustom.EmptyAllBoy.DressesSprites.ToArray());
                }
            }
            DressManager.Instance.ChangeFlatMateDress (dress.Value.PartName, dress.Value.DressesImages);        
        }	
		PurchaseDressManager.Instance.UpdateDressofAllCharacter ();
		PurchaseSaloonManager.Instance.UpdateHairofAllCharacter ();

		StartCoroutine (RegisterForSocietyEvent ());
	}


	/// <summary>
	/// Raises the registration fashion event event.
	/// </summary>
	public void OnRegistration_FashionEvent ()
	{
        foreach (var dress in PurchaseDressManager.Instance.selectedDresses)
        {
            if (DressManager.Instance.SelectedCharacter.GetComponent<Flatmate> ().data.Dress.ContainsKey("Dresses"))
            {
                DressManager.Instance.SelectedCharacter.GetComponent<Flatmate> ().data.Dress.Remove("Dresses");

                var allCustom = Resources.Load<CustomisationAllData>("CustomisationAllData");
                if(GameManager.GetGender()== GenderEnum.Female)
                {
                    DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllGirl.BodyPartName.ToArray(), allCustom.EmptyAllGirl.DressesSprites.ToArray());
                }else
                {
                    DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllBoy.BodyPartName.ToArray(), allCustom.EmptyAllBoy.DressesSprites.ToArray());
                }
            }
            DressManager.Instance.ChangeFlatMateDress (dress.Value.PartName, dress.Value.DressesImages);        
        }

		var tut = GameManager.Instance.GetComponent<Tutorial> ();

		if (!tut._FashionEventCompleate && tut._SaloonPurchased) {	
			tut.FashionEventStart ();
			ScreenAndPopupCall.Instance.CloseScreen ();

			//			RoommateManager.Instance.SelectedRoommate = RoommateManager.Instance.SelectedRoommate;
			tut.Registered = true;
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			PlayerMessageAfterRegistration ();

		} else {
			PurchaseDressManager.Instance.UpdateDressofAllCharacter ();
			PurchaseSaloonManager.Instance.UpdateHairofAllCharacter ();

			StartCoroutine (RegisterForAnFashionEvent ());
		}
	}


	/// <summary>
	/// Registers for an fashion event.
	/// </summary>
	/// <returns>The for an fashion event.</returns>
	public IEnumerator RegisterForAnFashionEvent ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = (PlayerPrefs.GetInt ("PlayerId")).ToString ();
		jsonElement ["event_id"] = CurrentEvent.Event_id.ToString ();
		jsonElement ["experience_level"] = GameManager.Instance.level.ToString ();

		var Flatmate = RoommateManager.Instance.SelectedRoommate.GetComponent<Flatmate> ();
		jsonElement ["gender"] = PlayerPrefs.GetString ("CharacterType");
		jsonElement ["flatmate_id"] = Flatmate.data.Id.ToString ();

		var jsonarray = new JSONArray ();

		foreach (var item in Flatmate.data.Dress) {
			var jsonItem = new JSONClass ();
			int itemId = 0;
//			if (item.Key.Contains ("Clothes") || item.Key.Contains ("Tops") || item.Key.Contains ("Jeans")) {
//				itemId = item.Value;
//			} else if (item.Key.Contains ("Hair")) {
			itemId = item.Value;
//			}

			jsonItem ["item_id"] = item.Value.ToString ();
			jsonItem ["item_type"] = item.Key;
			jsonarray.Add (jsonItem);
		}

		var jsonItem2 = new JSONClass ();
		jsonItem2 ["item_id"] = Flatmate.GetComponent <CharacterProperties> ().PlayerType;
		jsonItem2 ["item_type"] = "CharacterType";
		
		jsonarray.Add (jsonItem2);

		jsonElement ["items"] = jsonarray;


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (PinekixUrls.EventRegistrationUrl ("Fashion_Event"), encoding.GetBytes (jsonElement.ToString ()), postHeader);
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");
				
				PayFeeForRegistartion (CurrentEvent);

				Flatmate.data.BusyType = BusyType.FashionEvents;
				Flatmate.data.EventBusyId = CurrentEvent.Event_id;

				RoommateManager.Instance.StartBusyTimerForSelectedRoomMate (600f);// 7200 Seconds = 2x60x60 ie 2 hours 

				//TODO this will be changed to minutes instead of seconds. When FlatmateBusyTimer.cs <Line no. 41> is changed to AddMinutes instead of AddSeconds.
				ShowPopOfDescription ("You are successfully registered for " + CurrentEvent.EventName, () => { 
					WaitingForMyPair_fashion ();
//					ScreenAndPopupCall.Instance.CloseCharacterCamera ();
					ScreenAndPopupCall.Instance.CloseScreen ();
				});

				yield return true;
			} else {
//				print ("error" + _jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));
				ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));

				yield return false;
			}


		} else {
			yield return false;
		}
	}

	int FindIdOfDressByName (string Name, string Category)
	{
		foreach (var dress in PurchaseDressManager.Instance.AllDresses) {
			if (dress.Name == Name && dress.Catergory.ToString () == Category)
				return dress.Id;
		}
		return 0;
	}

	int FindIdOfSaloonByName (string Name, string Category)
	{
		foreach (var saloon in PurchaseSaloonManager.Instance.AllItems) {
			if (saloon.Name == Name && saloon.Catergory.ToString () == Category)
				return saloon.item_id;
		}
		return 0;
	}

	public void PayFeeForRegistartion (EventDataStructure Event)
	{
		if (Event.conditions.ContainsKey ("Coins")) {
			//			if (PlayerPrefs.GetInt ("Money") >= int.Parse (Event.conditions ["Coins"])) {
			GameManager.Instance.SubtractCoins (int.Parse (Event.conditions ["Coins"]));
			//			}
		}
		if (Event.conditions.ContainsKey ("Gems")) {
			//			if (GameManager.Instance.Gems >= int.Parse (Event.conditions ["Gems"])) {
			//			} TODO: Subtract Gems here
		}
	}

	/// <summary>
	/// shows the message to player after registration.
	/// </summary>
	void PlayerMessageAfterRegistration ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		string FlatMateName = RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ().data.Name.Trim ('"');
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		if (PlayerPrefs.GetString ("CharacterType") == "Female")
			ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Hi, " + FlatMateName + " is unavailable till she is taking part in the event";
		else
			ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Hi, " + FlatMateName + " is unavailable till he is taking part in the event";

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());		
	}


	/// <summary>
	/// Shows the pop of description.
	/// </summary>
	/// <param name="Message">Message.</param>
	void ShowPopOfDescription (string Message, UnityEngine.Events.UnityAction OnClickOkAction = null)
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent <Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = Message;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			ScreenAndPopupCall.Instance.CloseScreen ();
			ScreenAndPopupCall.Instance.CloseRoomCamera ();
			ScreenAndPopupCall.Instance.DeleteRoom ();
			if (OnClickOkAction != null)
				OnClickOkAction ();
		});		
	}

	/// <summary>
	/// Event or voting selection.
	/// </summary>
	//	public void EventandVotingSelection ()
	//	{
	//		if (ScreenManager.Instance.OpenedCustomizationScreen.Contains ("Fashion")) {
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.ClosePopup ();
	//			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.SetActive (true);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (true);
	//			var tut = GameManager.Instance.GetComponent<Tutorial> ();
	//			if (!tut._FashionEventCompleate && tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (!tut._FashionEventCompleate && !tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (tut._FashionEventCompleate) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//			}
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Register";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Vote";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Welcome " + PlayerPrefs.GetString ("UserName") + " choose your interest!";
	//			// Event Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Event);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//
	//			// Voting Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Voting);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());	
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//		}
	//
	//		if (ScreenManager.Instance.OpenedCustomizationScreen.Contains ("Decor")) {
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.ClosePopup ();
	//			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.SetActive (true);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (true);
	//			var tut = GameManager.Instance.GetComponent<Tutorial> ();
	//			if (!tut._FashionEventCompleate && tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (!tut._FashionEventCompleate && !tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (tut._FashionEventCompleate) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//			}
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Register";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Vote";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Welcome " + PlayerPrefs.GetString ("UserName") + " choose your interest!";
	//			// Event Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Event);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//
	//			// Voting Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Voting);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());	
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//		}
	//
	//		if (ScreenManager.Instance.OpenedCustomizationScreen.Contains ("CatWalk")) {
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.ClosePopup ();
	//			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.SetActive (true);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (true);
	//			var tut = GameManager.Instance.GetComponent<Tutorial> ();
	//			if (!tut._FashionEventCompleate && tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (!tut._FashionEventCompleate && !tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (tut._FashionEventCompleate) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//			}
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Register";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Vote";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Welcome " + PlayerPrefs.GetString ("UserName") + " choose your interest!";
	//			// Event Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Event);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//
	//			// Voting Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Voting);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());	
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//		}
	//
	//		if (ScreenManager.Instance.OpenedCustomizationScreen.Contains ("CoOp")) {
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//			ScreenManager.Instance.ClosePopup ();
	//			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.SetActive (true);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (true);
	//			var tut = GameManager.Instance.GetComponent<Tutorial> ();
	//			if (!tut._FashionEventCompleate && tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (!tut._FashionEventCompleate && !tut.Registered) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = false;
	//
	//			} else if (tut._FashionEventCompleate) {
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
	//				ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.GetComponent<Button> ().interactable = true;
	//			}
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Register";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Vote";
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Welcome " + PlayerPrefs.GetString ("UserName") + " choose your interest!";
	//			// Event Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Event);
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//
	//			// Voting Button Function
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.EventType = eType.Voting);
	//
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => EventManagment.Instance.CreateNewEvent ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());	
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ());
	//			ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenAndPopupCall.Instance.FashionEventListScreenSelection ());
	//		}
	//	}


	#endregion


	#region CatWalk_Event
	/// <summary>
	/// Raises the cat walk registration event.
	/// </summary>
	public void OnCatWalkRegistration ()
	{

//		if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
//			DressManager.Instance.ChangeFlatMateDress (PurchaseDressManager.Instance.selectedDress.thisDress.PartName, PurchaseDressManager.Instance.selectedDress.thisDress.White_Images);
//
//		if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//			DressManager.Instance.ChangeFlatMateDress (PurchaseDressManager.Instance.selectedDress.thisDress.PartName, PurchaseDressManager.Instance.selectedDress.thisDress.Brown_Images);
//
//		if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
//			DressManager.Instance.ChangeFlatMateDress (PurchaseDressManager.Instance.selectedDress.thisDress.PartName, PurchaseDressManager.Instance.selectedDress.thisDress.Black_Images);
//
//
//		PurchaseDressManager.Instance.UpdateDressofAllCharacter ();
//		PurchaseSaloonManager.Instance.UpdateHairofAllCharacter ();
//
//		if (!SelectedRoommates.Contains (DressManager.Instance.SelectedCharacter))
//			SelectedRoommates.Add (DressManager.Instance.SelectedCharacter);
//		if (SelectedRoommates.Count < 3) {
//			ScreenAndPopupCall.Instance.CloseScreen ();
//			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
//			ScreenAndPopupCall.Instance.ShowCharacterSelectionForCatWalk ();
//		} else
		StartCoroutine (RegisterForCatWalkEvent ());
	}


	/// <summary>
	/// Registers for cat walk event.
	/// </summary>
	/// <returns>The for cat walk event.</returns>
	public IEnumerator RegisterForCatWalkEvent ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var element = new CatWalkEventRegistration ();

		element.data_type = "save";
		element.event_id = CurrentEvent.Event_id;
		element.player_id = PlayerPrefs.GetInt ("PlayerId");
		element.experience_level = GameManager.Instance.level;

        for(int i = 0; i< SelectedRoommates.Count; i++) 
        {		
            var selected = SelectedRoommates[i];

            element.items.Add (AdditemObj ("item_id", selected.GetComponent<Flatmate> ().data.Id.ToString (), "item_type", "Flatmates_"+i));					

			foreach (var kvp in selected.GetComponent<Flatmate> ().data.Dress) {
				if (kvp.Key == "Hair") {
//					var Id = FindIdOfSaloonByName (kvp.Value, kvp.Key);	
                    element.items.Add (AdditemObj ("item_id", kvp.Value.ToString (), "item_type", "Hair_"+ i));					
				} 
//                else if (kvp.Key == "Shoes") {
//					var Id = FindIdOfDressByName (kvp.Value, kvp.Key);						
//					element.items.Add (AdditemObj ("item_id", Id.ToString (), "item_type", "Shoes"));	
//				}
            else {				
//					var Id = FindIdOfDressByName (kvp.Value, kvp.Key);						
                    element.items.Add (AdditemObj ("item_id", kvp.Value.ToString (), "item_type", "Clothes_"+ i));				
				}

//				element.items.Add (AdditemObj ("item_id", kvp.Value.ToString (), "item_type", kvp.Key));       
			}
		}

		if (PlayerPrefs.GetString ("CharacterType") == "Male")
			element.items.Add (AdditemObj ("item_id", "1", "item_type", "Gender"));
		else
			element.items.Add (AdditemObj ("item_id", "2", "item_type", "Gender"));


		var json = new JSONClass ();


		json ["data_type"] = element.data_type.ToString ();
		json ["event_id"] = element.event_id.ToString ();
		json ["player_id"] = (element.player_id).ToString ();
		json ["experience_level"] = element.experience_level.ToString ();

		json ["items"] = new JSONArray ();

		foreach (var item in element.items) {
			var jsonarray = new JSONClass ();
			foreach (var kvp in item) {
				jsonarray.Add (kvp.Key, kvp.Value);
			}
			json ["items"].Add (jsonarray);
		}


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
//		print ("jsonDtat is ==>> " + json.ToString ()); 
		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/catwalkRegistration", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");
				PayFeeForRegistartion (CurrentEvent);
				ShowPopOfDescription ("You are successfully registered for " + CurrentEvent.EventName, () => {
					ScreenAndPopupCall.Instance.CloseCharacterCamera ();
					ScreenAndPopupCall.Instance.CloseScreen ();
					CatWalk_ShowMyPair ();

				});	


//				for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
//					for (int j = 0; j < SelectedRoommates.Count; j++) {
//						if (RoommateManager.Instance.RoommatesHired [i].GetComponent <Flatmate> ().data.Id == SelectedRoommates [j].GetComponent <Flatmate> ().data.Id) {
//							RoommateManager.Instance.SelectedRoommate = RoommateManager.Instance.RoommatesHired [i];
//							RoommateManager.Instance.RoommatesHired [i].GetComponent <Flatmate> ().data.EventBusyId = CurrentEvent.Event_id;
//							RoommateManager.Instance.RoommatesHired [i].GetComponent <Flatmate> ().data.BusyType = BusyType.CatwalkEvents;
//							RoommateManager.Instance.StartBusyTimerForSelectedRoomMate (600f);
//						}
//					}
//				}
					
				yield return true;
			} else {
//				print ("error" + _jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));
				ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));
				yield return false;
			}


		} else {
			yield return false;
		}

		PurchaseDressManager.Instance.UpdateDressofAllCharacter ();
		PurchaseSaloonManager.Instance.UpdateHairofAllCharacter (); 
	}



	public void CatWalk_ShowMyPair ()
	{
		ScreenAndPopupCall.Instance.ShowRegisteredPairForWaiting ();
//		ScreenManager.Instance.Decor_ShowPair.transform.GetChild (4).GetChild (0).GetChild (2).GetComponent <Text> ().text = PlayerManager.Instance.playerInfo.Name;
		int Layer = LayerMask.NameToLayer ("UI3D");


		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;

		var Player1 = GameObject.Instantiate (SelectedRoommates [0], ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0)) as GameObject;
		Player1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Player1.transform.localScale = Vector3.one * 0.3f;
		Player1.transform.localPosition = new Vector3 (0, 0f, 0);		
		Player1.SetLayerRecursively (Layer);	
		if (Player1.GetComponent<Flatmate> ())
			Destroy (Player1.GetComponent<Flatmate> ());

		if (Player1.GetComponent<RoomMateMovement> ())
			Destroy (Player1.GetComponent<RoomMateMovement> ());
		Player1.GetComponent <GenerateMoney> ().enabled = false;
		var moneyIcon = Player1.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon.gameObject);

		var Player2 = GameObject.Instantiate (SelectedRoommates [1], ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0)) as GameObject;
		Player2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Player2.transform.localScale = Vector3.one * 0.3f;
		Player2.transform.localPosition = new Vector3 (1, 0f, 0);

		Player2.SetLayerRecursively (Layer);	
		if (Player2.GetComponent<Flatmate> ())
			Destroy (Player2.GetComponent<Flatmate> ());

		if (Player2.GetComponent<RoomMateMovement> ())
			Destroy (Player2.GetComponent<RoomMateMovement> ());
		Player2.GetComponent <GenerateMoney> ().enabled = false;
		var moneyIcon2 = Player2.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon2.gameObject);

		var Player3 = GameObject.Instantiate (SelectedRoommates [2], ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0)) as GameObject;
		Player3.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Player3.transform.localScale = Vector3.one * 0.3f;
		Player3.transform.localPosition = new Vector3 (-1, 0f, 0);

		Player3.SetLayerRecursively (Layer);	
		if (Player3.GetComponent<Flatmate> ())
			Destroy (Player3.GetComponent<Flatmate> ());

		if (Player3.GetComponent<RoomMateMovement> ())
			Destroy (Player3.GetComponent<RoomMateMovement> ());
		Player3.GetComponent <GenerateMoney> ().enabled = false;
		var moneyIcon3 = Player3.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon3.gameObject);

		Player3.SetLayerRecursively (Layer);

		Player1.SetActive (true);
		Player2.SetActive (true);
		Player3.SetActive (true);

		Player1.SetOrderInLayerRecursively (100);
		Player2.SetOrderInLayerRecursively (200);
		Player3.SetOrderInLayerRecursively (300);



		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);

		ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Title Text").GetComponent <Text> ().text = CurrentEvent.EventName;
		ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Theme Name").GetComponent <Text> ().text = CurrentEvent.EventTheme;
        ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player1 Name").GetComponent <Text> ().text = PlayerPrefs.GetString ("UserName");

		EventPairingController.Instance.StartTimerGetForPair (2f); // for catwalk

	}


//	public IEnumerator IeCatWalkShowMyPair (int count)
//	{
//		var encoding = new System.Text.UTF8Encoding ();
//	
//		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
//	
//		var jsonElement = new JSONClass ();
//	
//		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
//		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
//		jsonElement ["count"] = count.ToString ();
//	
//		postHeader.Add ("Content-Type", "application/json");
//		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
////		print ("jsonDtat is ==>> " + jsonElement.ToString ());
//		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/catwalkGetVotingPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);
//	
//		yield return www;
//	
//		if (www.error == null) {
//			JSONNode _jsnode = JSON.Parse (www.text);
////			print ("_jsnode ==>> " + _jsnode.ToString ());
//			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
////				print ("Success");
//	
//	
//				JSONNode dataArray = _jsnode ["data"];
//	
//				CatWalkVotingPair vp = new CatWalkVotingPair ();	
//
//				int.TryParse (dataArray ["event_id"], out vp.event_id);
//				int.TryParse (dataArray ["pair_id"], out vp.pair_id);
//
//				int.TryParse (dataArray ["player1_id"], out vp.player1_id);
//				int.TryParse (dataArray ["player2_id"], out vp.player2_id);
//				vp.player1Name = dataArray ["player1_name"].ToString ().Trim ('"');
//				vp.player2Name = dataArray ["player2_name"].ToString ().Trim ('"');	
//	
//	
//				for (int j = 0; j < _jsnode ["data"] ["player1_item_data"].Count; j++) {
//					if (_jsnode ["data"] ["player1_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "Flatmates") {
//						vp.Player1_flatmate_id.Add (int.Parse (_jsnode ["data"] ["player1_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] ["player1_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "hair") {
//						vp.Player1_hair_id.Add (int.Parse (_jsnode ["data"] ["player1_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] ["player1_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "dress") {
//						vp.Player1_dress_id.Add (int.Parse (_jsnode ["data"] ["player1_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] [0] ["player1_item_data"] ["item_type"].ToString ().Trim ('"') == "shoes") {
//						vp.Player1_shoes_id.Add (int.Parse (_jsnode ["data"] ["player1_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] [0] ["player1_item_data"] ["item_type"].ToString ().Trim ('"') == "Gender") {
//						int temp = int.Parse (_jsnode ["data"] ["player1_item_data"] [j] ["item_id"].ToString ().Trim ('"'));
//						if (temp == 1) {
//							vp.player1_Gender = "Male";
//						} else {
//							vp.player1_Gender = "Female";
//						}
//					}
//	
//				}
//	
//				for (int j = 0; j < _jsnode ["data"] ["player2_item_data"].Count; j++) {
//					if (_jsnode ["data"] ["player2_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "Flatmates") {
//						vp.Player2_flatmate_id.Add (int.Parse (_jsnode ["data"] ["player2_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] ["player2_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "hair") {
//						vp.Player2_hair_id.Add (int.Parse (_jsnode ["data"] ["player2_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] ["player2_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "dress") {
//						vp.Player2_dress_id.Add (int.Parse (_jsnode ["data"] ["player2_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] ["player2_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "shoes") {
//						vp.Player2_shoes_id.Add (int.Parse (_jsnode ["data"] ["player2_item_data"] [j] ["item_id"].ToString ().Trim ('"')));
//					}
//					if (_jsnode ["data"] ["player2_item_data"] [j] ["item_type"].ToString ().Trim ('"') == "shoes") {
//						int temp = int.Parse (_jsnode ["data"] ["player1_item_data"] [j] ["item_id"].ToString ().Trim ('"'));
//						if (temp == 1) {
//							vp.player2_Gender = "Male";
//						} else {
//							vp.player2_Gender = "Female";
//						}
//					}
//				}
//	
//
//	
//                ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player1 Name").GetComponent <Text> ().text = vp.player1Name;
//	
//	
//				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
//	
//
//	
//				GameObject Player_Char1 = null;
//				GameObject Player_Char2 = null;
//				GameObject Player_Char3 = null;
//
//
//				var previous = DressManager.Instance.dummyCharacter;
//				int Layer = LayerMask.NameToLayer ("UI3D");
//
//
//
//				if (vp.player1_id != PlayerPrefs.GetInt ("PlayerId")) {
//					
//
//					if (vp.player1_Gender == "Male") {
//						Player_Char1 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char2 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char3 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
//					} else {
//						Player_Char1 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char2 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char3 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);
//					}
//
//
//					Player_Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
//					Player_Char1.transform.localScale = Vector3.one * 0.3f;
//					Player_Char1.transform.localPosition = new Vector3 (0, 0f, 0);
//
//					Player_Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
//					Player_Char2.transform.localScale = Vector3.one * 0.3f;
//					Player_Char2.transform.localPosition = new Vector3 (1, 0f, 0);
//
//
//					Player_Char3.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
//					Player_Char3.transform.localScale = Vector3.one * 0.3f;
//					Player_Char3.transform.localPosition = new Vector3 (-1, 0f, 0);
//
//
//
//					for (int i = 0; i < Mathf.Min (vp.Player1_flatmate_id.Count, vp.Player1_dress_id.Count); i++) {
//						var dress = VotingPairManager.Instance.FindDressWithId (vp.Player1_dress_id [i]);
//						if (dress != null) {
//							if (i == 0)
//								DressManager.Instance.dummyCharacter = Player_Char1;
//							if (i == 1)
//								DressManager.Instance.dummyCharacter = Player_Char2;
//							if (i == 2)
//								DressManager.Instance.dummyCharacter = Player_Char3;
//
////							if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
//								DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
////							if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
////								DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
////							if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
////								DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						}
//					}
//
//
//
//					Player_Char1.SetLayerRecursively (Layer);
//					Player_Char2.SetLayerRecursively (Layer);
//					Player_Char3.SetLayerRecursively (Layer);
//
//
//					Player_Char1.SetOrderInLayerRecursively (100);
//					Player_Char2.SetOrderInLayerRecursively (200);
//					Player_Char3.SetOrderInLayerRecursively (300);
//
//				} else {
//					if (vp.player2_Gender == "Male") {
//						Player_Char1 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char2 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char3 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
//					} else {
//
//						Player_Char1 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char2 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);
//						Player_Char3 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);
//					}
//
//
//
//					Player_Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
//					Player_Char1.transform.localScale = Vector3.one * 0.3f;
//					Player_Char1.transform.localPosition = new Vector3 (0, 0f, 0);
//
//
//					Player_Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
//					Player_Char2.transform.localScale = Vector3.one * 0.3f;
//					Player_Char2.transform.localPosition = new Vector3 (1, 0f, 0);
//
//
//					Player_Char3.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
//					Player_Char3.transform.localScale = Vector3.one * 0.3f;
//					Player_Char3.transform.localPosition = new Vector3 (-1, 0f, 0);
//
//
//
//
//					for (int i = 0; i < Mathf.Min (vp.Player2_flatmate_id.Count, vp.Player2_dress_id.Count); i++) {
//						var dress = VotingPairManager.Instance.FindDressWithId (vp.Player2_dress_id [i]);
//						if (dress != null) {
//							if (i == 0)
//								DressManager.Instance.dummyCharacter = Player_Char1;
//							if (i == 1)
//								DressManager.Instance.dummyCharacter = Player_Char2;
//							if (i == 2)
//								DressManager.Instance.dummyCharacter = Player_Char3;
//
////							if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
//								DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
////							if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
////								DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
////							if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
////								DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						}
//					}
//
//					Player_Char1.SetLayerRecursively (Layer);
//					Player_Char2.SetLayerRecursively (Layer);
//					Player_Char3.SetLayerRecursively (Layer);
//
//					Player_Char1.SetOrderInLayerRecursively (100);
//					Player_Char2.SetOrderInLayerRecursively (200);
//					Player_Char3.SetOrderInLayerRecursively (300);
//
//				}
//	
//				DressManager.Instance.dummyCharacter = previous;
//				ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
//				ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;
//				ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);
//	
//
//	
//				yield return vp;
//			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
//				if (VotingPairManager.Instance.viewStatus)
//					ShowPopOfDescription ("No pair found");
//				yield return null;
//			} else {
////				print ("error" + _jsnode ["description"].ToString ());
//				yield return null;
//			}
//		} else {
//			yield return null;
//		}
//		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
//	}



	#endregion




	public Dictionary<string, string> AdditemObj (string key1, string value1, string key2, string value2)
	{
		Dictionary<string, string> itemobj = new Dictionary<string, string> ();
		itemobj.Add (key1, value1);
		itemobj.Add (key2, value2);

		return itemobj;
	}


	#region Decor Event Registration

	/// <summary>
	/// Raises the decor event registration event.
	/// </summary>
	public void OnDecorEventRegistration ()
	{
		StartCoroutine (RegisterForDecorEvent ());
	}


	/// <summary>
	/// Registers for decor event.
	/// </summary>
	/// <returns>The for decor event.</returns>
	public IEnumerator RegisterForDecorEvent ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();



		var Flat = ScreenAndPopupCall.Instance.RoomCamera.transform.GetComponentInChildren<Flat3D> ();
		var json = new JSONClass ();

		CurrentEvent.IsRegisterd = true;
		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		json ["event_id"] = CurrentEvent.Event_id.ToString ();
		json ["experience_level"] = GameManager.Instance.level.ToString ();


		json ["ground_texture_name"] = Flat.GroundTextureName;
		json ["wall_texture_name"] = Flat.WallColourNames;



		foreach (var item in DecorEventList) {			
			var jsonarray = new JSONClass ();
			jsonarray.Add ("id", item.Key.ToString ());

			json ["object_id"].Add (jsonarray);
		}

		foreach (var item in DecorEventList) {
			var jsonarray2 = new JSONClass ();
			jsonarray2.Add ("position", item.Value.ToString ());
			json ["object_position"].Add (jsonarray2);
		}

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
//		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/decor_eventregister", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());

			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");
//
//				foreach (var item in DecorEventList) {			
//					DecorController.Instance.BusyDecor (item.Key, CurrentEvent.Event_id);
//				}

				PayFeeForRegistartion (CurrentEvent);
				ShowPopOfDescription ("You are successfully registered for " + CurrentEvent.EventName, () => ShowPairofDecor ());
			
			} else
				ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));
			ReModelShopController.Instance.isForEvent = false;
		} else {
			yield return false;
		}
	}



	public void ShowPairofDecor ()
	{
		//showmydata
		ScreenAndPopupCall.Instance.ShowRegisteredPairForWaiting ();
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 16;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 16;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);

		var myFlat = GameObject.Find ("RoomForDecorEvent");
		myFlat.transform.SetParent (ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
		myFlat.SetLayerRecursively (LayerMask.NameToLayer ("UI3D"));


		myFlat.transform.localPosition = new Vector3 (0, -2.14f, 0);
		myFlat.transform.localScale = Vector3.one;

//		ScreenManager.Instance.WaitingForOpponentScreen.transform.GetChild (3).GetChild (1).gameObject.SetActive (true); // Waitinf for oppo text
		ScreenManager.Instance.WaitingForOpponentScreen.transform.GetChild (5).gameObject.SetActive (true);

		ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Title Text").GetComponent <Text> ().text = CurrentEvent.EventName;
		ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Theme Name").GetComponent <Text> ().text = CurrentEvent.EventTheme;
        ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player1 Name").GetComponent <Text> ().text = PlayerPrefs.GetString ("UserName");

//		StartCoroutine (IeShowPairofDecor ());

		//wait for opponent data
		EventPairingController.Instance.StartTimerGetForPair (2f);// for decor
	}



	IEnumerator IeShowPairofDecor ()
	{
		var count = 1;
		var time = DateTime.Now;
		var time2 = DateTime.Now.AddMinutes (2);

		for (float timer = 0; timer < time2.Second; timer += Time.deltaTime) {
			var ts = time2 - time;
			ScreenManager.Instance.WaitingForOpponentScreen.transform.GetChild (5).GetChild (0).GetComponent <Text> ().text = string.Format ("{0}:{1}", ts.Minutes, ts.Seconds);
			if ((int)(timer / count) == 30 || timer == 0) {
				if (count < 5) {
					StartCoroutine (IeCheckForPair (count));
					count++;
				} else if (count >= 5) {
					//TODO: Show dummy character
				}

			}
			yield return null;
		}
		//Fetch the data of pair
	
		//show opponent data
	}

	public IEnumerator IeCheckForPair (int count)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var Flat = ScreenAndPopupCall.Instance.RoomCamera.transform.GetComponentInChildren<Flat3D> ();
		var json = new JSONClass ();

		CurrentEvent.IsRegisterd = true;
		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		json ["event_id"] = CurrentEvent.Event_id.ToString ();
		json ["count"] = count.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
//		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/decor_getSinglePair", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			EventManagment.Instance.EventType = eType.Voting;
			if (_jsnode ["status"].ToString ().Contains ("200")) {

				var dataArray = _jsnode ["data"];
				VotingPairForDecor Vp = new VotingPairForDecor ();
				/// this data fatching for player 2
				Vp.pair_id = int.Parse (dataArray ["pair_id"]);
				Vp.event_id = int.Parse (dataArray ["event_id"]);

				Vp.player1_id = int.Parse (dataArray ["player1_id"]);
				Vp.player1Name = dataArray ["player1_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player1_GroundTexture_Name = dataArray ["player1_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player1_WallTexture_Name = dataArray ["player1_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());

				List <int> ObjIds = new List<int> ();
				var DecoreObjectIds = dataArray ["player1_decor_object_data"];
				for (int x = 0; x < DecoreObjectIds.Count; x++) {
					ObjIds.Add (int.Parse (DecoreObjectIds [x] [0].ToString ().Trim ('"')));
				}

				List <string> TransfomArray = new List<string> ();
				var ObjPosition = dataArray ["player1_decor_object_position"];
				for (int x = 0; x < ObjPosition.Count; x++) {
					TransfomArray.Add (ObjPosition [x] [0].ToString ().Trim ('"'));
				}
				for (int x = 0; x < Mathf.Min (TransfomArray.Count, ObjIds.Count); x++) {
					DecorObject Decor = new DecorObject ();
					Decor.Id = ObjIds [x];
					string[] Values = TransfomArray [x].Split ('/');
					if (Values.Length == 3) {
						Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (Values [0].Trim ('|'));
						Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
						//							Decor.Rotation = ExtensionMethods.DeserializeVector3ArrayExtented (Values [2]);
						Decor.Direction = int.Parse (Values [2]);
					} else {
//						print (" Transform of player 1 not in correct format");
					}

					Vp.Player1AllDecorOnFlat.Add (Decor);
				}

				/// this data fatching for player 2
				Vp.player2_id = int.Parse (dataArray ["player2_id"]);
				Vp.player2Name = dataArray ["player2_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player2_GroundTexture_Name = dataArray ["player2_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player2_WallTexture_Name = dataArray ["player2_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());

				List <int> Player2ObjIds = new List<int> (); 
				var Player2DecoreObjectIds = dataArray ["player2_decor_object_data"];
				for (int x = 0; x < Player2DecoreObjectIds.Count; x++) {
					Player2ObjIds.Add (int.Parse (Player2DecoreObjectIds [x] [0].ToString ().Trim ('"')));
				}

				List <string> Player2TransfomArray = new List<string> ();
				var Player2ObjPosition = dataArray ["player2_decor_object_position"];
				for (int x = 0; x < Player2ObjPosition.Count; x++) {
					Player2TransfomArray.Add (Player2ObjPosition [x] [0].ToString ().Trim ('"'));
				}

				for (int x = 0; x < Mathf.Min (Player2TransfomArray.Count, Player2ObjIds.Count); x++) {
					DecorObject Player2Decor = new DecorObject ();
					Player2Decor.Id = Player2ObjIds [x];
					string[] Values = Player2TransfomArray [x].Split ('/');
					if (Values.Length == 3) {
						Player2Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (Values [0].Trim ('|'));
						Player2Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
						//							Player2Decor.Rotation = ExtensionMethods.DeserializeVector3ArrayExtented (Values [2]);
						Player2Decor.Direction = int.Parse (Values [2]);
					} else {
//						print (" Transform of player 2 not in correct format");
					}

					Vp.Player2AllDecorOnFlat.Add (Player2Decor);
				}

				if (Vp.player2_id == PlayerPrefs.GetInt ("PlayerId")) {
					//2nd player

                    ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player1 Name").GetComponent <Text> ().text = PlayerPrefs.GetString ("UserName");
                    ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild("TitleBg").FindChild("Player2 Name").GetComponent <Text>().text = Vp.player1Name;

					GameObject Player1Room = null;
					///TODO:
					//var previous = RoomPurchaseManager.Instance.RoomTypePrefeb [0];

					Player1Room = (GameObject)Instantiate (VotingPairManager.Instance.RoomPrefeb, Vector3.zero, Quaternion.identity);
					Player1Room.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
					Player1Room.transform.localScale = Vector3.one;
					Player1Room.transform.localPosition = new Vector3 (0, -2.14f, 0);
					int Layer = LayerMask.NameToLayer ("UI3D");
					Player1Room.SetLayerRecursively (Layer);
					// Temp Commented unitill wall fix
					//			Player1Room.GetComponent<Flat3D> ().Walls.ChangeWallColors (FindWallTextures (pair.player1_WallTexture_Name));
					if (!string.IsNullOrEmpty (Vp.player1_GroundTexture_Name))
						Player1Room.GetComponent<Flat3D> ().ChangeGroungColor (VotingPairManager.Instance.FindGroundTexture (Vp.player1_GroundTexture_Name));

					foreach (var decor in Vp.Player1AllDecorOnFlat) {
						var _dec = VotingPairManager.Instance.FindDecore (decor.Id);
						var parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0).gameObject;
						VotingPairManager.Instance.Create3DDecoreForUi (_dec, decor.Position, decor.Direction, parent);

					}
				} else if (Vp.player1_id == PlayerPrefs.GetInt ("PlayerId")) {
					// 1st player

                    ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player1 Name").GetComponent <Text> ().text = PlayerPrefs.GetString ("UserName");
                    ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player2 Name").GetComponent <Text> ().text = Vp.player2Name;

                    GameObject Player2Room = null;
					Player2Room = (GameObject)Instantiate (VotingPairManager.Instance.RoomPrefeb, Vector3.zero, Quaternion.identity);
					Player2Room.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
					Player2Room.transform.localScale = Vector3.one;
					Player2Room.transform.localPosition = new Vector3 (0, -2.14f, 0);
					int Layer = LayerMask.NameToLayer ("UI3D");
					Player2Room.SetLayerRecursively (Layer);

					if (!string.IsNullOrEmpty (Vp.player2_GroundTexture_Name))
						Player2Room.GetComponent<Flat3D> ().ChangeGroungColor (VotingPairManager.Instance.FindGroundTexture (Vp.player2_GroundTexture_Name));

					foreach (var decor in Vp.Player2AllDecorOnFlat) {
						var _dec = VotingPairManager.Instance.FindDecore (decor.Id);
						var parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0).gameObject;
						VotingPairManager.Instance.Create3DDecoreForUi (_dec, decor.Position, decor.Direction, parent);
					}
				}

				StopAllCoroutines ();
				AddBusyTime ();
				yield return Vp;
			} else {
//				print ("No Pair found!!!");
				yield return null;
			}
		} else {
			ShowPopOfDescription ("An error occurred!!!", null);
			yield return null;
		}
	}


	#endregion

	public IEnumerator  IeDeletePair ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var Flat = ScreenAndPopupCall.Instance.RoomCamera.transform.GetComponentInChildren<Flat3D> ();
		var json = new JSONClass ();

		CurrentEvent.IsRegisterd = true;
		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		json ["event_id"] = CurrentEvent.Event_id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
//		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/decor_getSinglePair", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				
			} else {
				
			}
		} else {
			
		}
	}

	public void AddBusyTime ()
	{
		var time = DateTime.Now;
		time.AddHours (2);
	}


	public void AddCooldown ()
	{
		var time = DateTime.Now;
		time.AddHours (6);
	}



	/// <summary>
	/// The leaderboard data saving list.
	/// </summary>
	public List<LeaderboardData> _leaderboardObject = new List<LeaderboardData> ();


	/// <summary>
	/// used for showing the leaderboard of the game.
	/// </summary>
	/// <param name="_eventType">Event type.</param>
	/// <param name="_eventId">Event identifier.</param>
	public void LeaderBoard (string _eventType, int _eventId)
	{
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().interactable = false;
		StartCoroutine (IeLeaderboard (_eventType, _eventId));
	}

	/// <summary>
	/// It's the Ienumerator of the leaderboard for downloading the data from the server
	/// </summary>
	/// <returns>The leaderboard.</returns>
	/// <param name="_eventType">Event type.</param>
	/// <param name="_eventId">Event identifier.</param>
	IEnumerator IeLeaderboard (string _eventType, int _eventId)
	{
		_leaderboardObject.Clear ();
		DeleteOldLeaderboardItem ();

		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkGetVoteData";
			break;
		case "CoOp_Event":
			link = "http://pinekix.ignivastaging.com/events/coopGetVoteData";
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decorGetVoteData";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshowGetVoteData";
			break;
		case "Society_Event":
			link = "";//TODO To Add Link for society
			break;
		}

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var json = new JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		json ["event_id"] = _eventId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
//		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW (link, encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			for (int i = 0; i < _jsnode ["data"].Count; i++) {

				if (_eventType == "CoOp_Event") {
					
					int player1_id = 0;
					int player2_id = 0;
					string player1_name = _jsnode ["data"] [i] ["player1_name"].ToString ().Trim ('"');
					string player2_name = _jsnode ["data"] [i] ["player2_name"].ToString ().Trim ('"');
					int player1_voteCount = 0;
					int player1_votescore = 0;
					int player1_votebonus = 0;
					int player1_friendbonus = 0;


					int.TryParse (_jsnode ["data"] [i] ["player1_id"].ToString ().Trim ('"'), out player1_id);
					int.TryParse (_jsnode ["data"] [i] ["player2_id"].ToString ().Trim ('"'), out player2_id);

					int.TryParse (_jsnode ["data"] [i] ["player_vote_count"].ToString ().Trim ('"'), out  player1_voteCount);
					int.TryParse (_jsnode ["data"] [i] ["player_vote_score"].ToString ().Trim ('"'), out  player1_votescore);
					int.TryParse (_jsnode ["data"] [i] ["player_vote_bonus"].ToString ().Trim ('"'), out  player1_votebonus);
					int.TryParse (_jsnode ["data"] [i] ["player_friend_bonus"].ToString ().Trim ('"'), out  player1_friendbonus);

					LeaderboardData player1 = new LeaderboardData ();
					player1.player_id = player1_id;
					player1.player_name = player1_name;
					player1.player_score = (player1_voteCount * 10) + player1_votebonus + player1_friendbonus;	

					///add player1 score with bonus point
					if (player1.player_id == PlayerPrefs.GetInt ("PlayerId")) {
						if (CurrentEvent.category == eventType.Fashion_Event) {
							for (int k = 0; k <= RoommateManager.Instance.RoommatesHired.Length; k++) {
								if (RoommateManager.Instance.RoommatesHired [i].GetComponent<Flatmate> ().data.Id == player1.player_id) {
									float thisPlayerPerkValue = RoommateManager.Instance.RoommatesHired [i].GetComponent<Flatmate> ().PerkValue;
									var thisFlatmte = RoommateManager.Instance.RoommatesHired [i].GetComponent<Flatmate> ();								
									float TempPerkValue = 0f;
									float FinalScoreWithBonusPoint = 0;
									if (thisFlatmte.data.Perk == "Fashion Event Bonus") {
										if (thisPlayerPerkValue == 1)
											TempPerkValue = 0;
										else if (thisPlayerPerkValue == 2)
											TempPerkValue = 3;
										else if (thisPlayerPerkValue == 3)
											TempPerkValue = 5;
										else if (thisPlayerPerkValue == 4)
											TempPerkValue = 9;
										else if (thisPlayerPerkValue == 5)
											TempPerkValue = 15;
										else if (thisPlayerPerkValue == 6)
											TempPerkValue = 20;

										FinalScoreWithBonusPoint = player1.player_score * TempPerkValue / 100;
									}
									int roundFiger = Mathf.RoundToInt (FinalScoreWithBonusPoint);
									player1.player_score = player1.player_score + roundFiger;

									/// Give Fashion Experience Points Bonus point to Real Player
									var flatmate = RoommateManager.Instance.RoommatesHired [0].GetComponent<Flatmate> ();
									if (thisFlatmte.data.Perk == "Fashion Experience Points Bonus") {

										int FashionExperioncePoint = flatmate.GetPerkValueCorrespondingToString ("Fashion Experience Points Bonus");
										float ExpTogive = player1.player_score * FashionExperioncePoint * GameManager.Instance.level / FashionExperioncePoint;
										GameManager.Instance.AddExperiencePoints (ExpTogive);
									}

									/// Give Coin to real Player
									if (thisFlatmte.data.Perk == "Fashion Event Coin Bonus") {
										
										int flatmatePerkValue = flatmate.GetPerkValueCorrespondingToString ("Fashion Event Coin Bonus");
										int GiveCoin = player1.player_score * flatmatePerkValue * GameManager.Instance.level;
										GameManager.Instance.AddCoins (GiveCoin);
									}
								}
							}
						}
					}

					_leaderboardObject.Add (player1);

					LeaderboardData player2 = new LeaderboardData ();
					player2.player_id = player2_id;
					player2.player_name = player2_name;
					player2.player_score = (player1_voteCount * 10) + player1_votebonus + player1_friendbonus;	
					///add player2 score with bonus point
					if (player2.player_id == PlayerPrefs.GetInt ("PlayerId")) {
						if (CurrentEvent.category == eventType.Fashion_Event) {
							for (int k = 0; k <= RoommateManager.Instance.RoommatesHired.Length; k++) {
								if (RoommateManager.Instance.RoommatesHired [i].GetComponent<Flatmate> ().data.Id == player2.player_id) {
									float thisPlayerPerkValue = RoommateManager.Instance.RoommatesHired [i].GetComponent<Flatmate> ().PerkValue;
									var thisFlatmte = RoommateManager.Instance.RoommatesHired [i].GetComponent<Flatmate> ();								
									float TempPerkValue = 0f;
									float FinalScoreWithBonusPoint = 0;
									if (thisFlatmte.data.Perk == "Fashion Event Bonus") {
										if (thisPlayerPerkValue == 1)
											TempPerkValue = 0;
										else if (thisPlayerPerkValue == 2)
											TempPerkValue = 3;
										else if (thisPlayerPerkValue == 3)
											TempPerkValue = 5;
										else if (thisPlayerPerkValue == 4)
											TempPerkValue = 9;
										else if (thisPlayerPerkValue == 5)
											TempPerkValue = 15;
										else if (thisPlayerPerkValue == 6)
											TempPerkValue = 20;

										FinalScoreWithBonusPoint = player2.player_score * TempPerkValue / 100;
									}
									int roundFiger = Mathf.RoundToInt (FinalScoreWithBonusPoint);
									player2.player_score = player2.player_score + roundFiger;

									/// Give Fashion Experience Points Bonus point to Real Player
									var flatmate = RoommateManager.Instance.RoommatesHired [0].GetComponent<Flatmate> ();
									if (thisFlatmte.data.Perk == "Fashion Experience Points Bonus") {

										int FashionExperioncePoint = flatmate.GetPerkValueCorrespondingToString ("Fashion Experience Points Bonus");
										float ExpTogive = player2.player_score * FashionExperioncePoint * GameManager.Instance.level / FashionExperioncePoint;
										GameManager.Instance.AddExperiencePoints (ExpTogive);
									}

									/// Give Coin to real Player
									if (thisFlatmte.data.Perk == "Fashion Event Coin Bonus") {
										int flatmatePerkValue = flatmate.GetPerkValueCorrespondingToString ("Fashion Event Coin Bonus");
										int GiveCoin = player2.player_score * flatmatePerkValue * GameManager.Instance.level;
										GameManager.Instance.AddCoins (GiveCoin);
									}
								}
							}
						}
					}
					_leaderboardObject.Add (player2);


				} else {

					int player1_id = 0;
					string player1_name = _jsnode ["data"] [i] ["player_name"].ToString ().Trim ('"');
					int player1_voteCount = 0;
					int player1_votescore = 0;
					int player1_votebonus = 0;
					int player1_friendbonus = 0;


					int.TryParse (_jsnode ["data"] [i] ["player_id"].ToString ().Trim ('"'), out player1_id);
					int.TryParse (_jsnode ["data"] [i] ["player_vote_count"].ToString ().Trim ('"'), out  player1_voteCount);
					int.TryParse (_jsnode ["data"] [i] ["player_vote_score"].ToString ().Trim ('"'), out  player1_votescore);
					int.TryParse (_jsnode ["data"] [i] ["player_vote_bonus"].ToString ().Trim ('"'), out  player1_votebonus);
					int.TryParse (_jsnode ["data"] [i] ["player_friend_bonus"].ToString ().Trim ('"'), out  player1_friendbonus);


					LeaderboardData player1 = new LeaderboardData ();
					player1.player_id = player1_id;
					player1.player_name = player1_name;
					player1.player_score = (player1_voteCount * 10) + player1_votebonus + player1_friendbonus;		

					_leaderboardObject.Add (player1);
				}
			}
		} else {

		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().interactable = true;
		_leaderboardObject.Sort ((s2, s1) => s1.player_score.CompareTo (s2.player_score));
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
		//		ScreenAndPopupCall.Instance.CloseScreen ();
		//		ScreenAndPopupCall.Instance.ShowLeaderboard ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Leaderboard").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.Leaderboard);
		GenerateLeaderboard ();
	}



	void GenerateLeaderboard ()
	{
		bool playerincluded = false;

		DeleteOldLeaderboardItem ();
		if (_leaderboardObject.Count >= 1) {
			foreach (var _object in _leaderboardObject) {

				GameObject obj = (GameObject)Instantiate (LeaderboardPrefab, Vector2.zero, Quaternion.identity);
				obj.transform.SetParent (LeaderboardContainer.transform);
				obj.transform.localScale = Vector3.one;

				obj.transform.GetChild (0).GetComponent<Text> ().text = (_leaderboardObject.IndexOf (_object) + 1).ToString ();
				obj.transform.GetChild (1).GetComponent <Text> ().text = _object.player_name.ToString ();
				obj.transform.GetChild (2).GetComponent <Text> ().text = _object.player_score.ToString ();

				if (_object.player_id != PlayerPrefs.GetInt ("PlayerId")) {
//					if (_leaderboardObject.IndexOf (_object) == 11 || _leaderboardObject.IndexOf (_object) == 10) {
//						obj.GetComponent<Image> ().enabled = false;
//						for (int i = 0; i < obj.transform.childCount; i++)
//							obj.transform.GetChild (i).gameObject.SetActive (false);
//						if (_leaderboardObject.IndexOf (_object) == 11)
//							break;
//					}
				} else {
					if (_leaderboardObject.IndexOf (_object) == 11 || _leaderboardObject.IndexOf (_object) == 10) {
						obj.GetComponent<Image> ().enabled = true;
						for (int i = 0; i < obj.transform.childCount; i++)
							obj.transform.GetChild (i).gameObject.SetActive (true);
						if (_leaderboardObject.IndexOf (_object) == 11)
							break;
					}
					playerincluded = true;
				}


			}

			if (!playerincluded) {
				List<int> indexes = new List<int> ();

				foreach (var _object in _leaderboardObject) {
					if (_object.player_id == PlayerPrefs.GetInt ("PlayerId"))
						indexes.Add (_leaderboardObject.IndexOf (_object));
				}

				foreach (int index in indexes) {
					GameObject obj_pre = (GameObject)Instantiate (LeaderboardPrefab, LeaderboardContainer.transform, false);

					obj_pre.transform.GetChild (0).GetComponent<Text> ().text = (index).ToString ();
					obj_pre.transform.GetChild (1).GetComponent <Text> ().text = _leaderboardObject [index - 1].player_name.ToString ();
					obj_pre.transform.GetChild (2).GetComponent <Text> ().text = _leaderboardObject [index - 1].player_score.ToString ();


					GameObject obj = (GameObject)Instantiate (LeaderboardPrefab, LeaderboardContainer.transform, false);

					obj.transform.GetChild (0).GetComponent<Text> ().text = (index + 1).ToString ();
					obj.transform.GetChild (1).GetComponent <Text> ().text = _leaderboardObject [index].player_name.ToString ();
					obj.transform.GetChild (2).GetComponent <Text> ().text = _leaderboardObject [index].player_score.ToString ();


					GameObject obj_after = (GameObject)Instantiate (LeaderboardPrefab, LeaderboardContainer.transform, false);

					obj_after.transform.GetChild (0).GetComponent<Text> ().text = (index + 2).ToString ();
					obj_after.transform.GetChild (1).GetComponent <Text> ().text = _leaderboardObject [index + 1].player_name.ToString ();
					obj_after.transform.GetChild (2).GetComponent <Text> ().text = _leaderboardObject [index + 1].player_score.ToString ();
				}
			}
		} else {
			GameObject obj = (GameObject)Instantiate (LeaderboardPrefab, Vector2.zero, Quaternion.identity);
			obj.transform.SetParent (LeaderboardContainer.transform);
			obj.transform.localScale = Vector3.one;

			obj.transform.GetChild (0).gameObject.SetActive (false);
			obj.transform.GetChild (1).GetComponent <Text> ().text = "No User Available";
			obj.transform.GetChild (2).gameObject.SetActive (false);

		}

	}


	/// <summary>
	/// Checks if any event is over.
	/// if any event is over then we will show a icon of count so that player has info that any event is over and he has to claim the rewards.
	/// </summary>
	public void CheckIfAnyEventIsOver ()
	{
		List<EventDataStructure> _fashionEvent = new List<EventDataStructure> ();
		List<EventDataStructure> _decorEvent = new List<EventDataStructure> ();
		List<EventDataStructure> _catWalkEvent = new List<EventDataStructure> ();
		List<EventDataStructure> _coOpEvent = new List<EventDataStructure> ();
		List<EventDataStructure> _societyEvent = new List<EventDataStructure> ();

		List<EventDataStructure> compleateEvent = new List<EventDataStructure> ();

		for (int i = 0; i < AllEventList.Count; i++) {
			var _event = new EventDataStructure ();

			_event.EventName = AllEventList [i].EventTitle;
			_event.EventDetails = AllEventList [i].EventDescription;
			_event.EventTheme = AllEventList [i].EventTheme;
			_event.Event_id = AllEventList [i].Event_id;
//			_event.Completion = AllEventList [i].EventCompletion;
//			_event.IsCompleted = AllEventList [i].IsEventCompleted;
//			_event.IsGoingOn = AllEventList [i].IsGoingOn;
			_event.StartTime = AllEventList [i].StartTime;
			_event.EndTime = AllEventList [i].EndTime;
			_event.RegistrationTime = AllEventList [i].RegistrationTime;
			_event.category = AllEventList [i].category;

			for (int x = 0; x < Mathf.Min (AllEventList [i].ConditionName.Count, AllEventList [i].ConditionValue.Count); x++) {
				_event.conditions.Add (AllEventList [i].ConditionName [x], AllEventList [i].ConditionValue [x]);
			}
			for (int x = 0; x < Mathf.Min (AllEventList [i].Reward.Count, AllEventList [i].RewardValue.Count); x++) {
				_event.Rewards.Add (AllEventList [i].Reward [x], AllEventList [i].RewardValue [x]);
			}

//			if (_event.IsCompleted) {
//				compleateEvent.Add (_event);
//			} else {
			if (_event.category == eventType.Fashion_Event)
				_fashionEvent.Add (_event);
			else if (_event.category == eventType.Decor_Event)
				_decorEvent.Add (_event);
			else if (_event.category == eventType.CatWalk_Event)
				_catWalkEvent.Add (_event);
			else if (_event.category == eventType.CoOp_Event)
				_coOpEvent.Add (_event);
			else if (_event.category == eventType.Society_Event)
				_societyEvent.Add (_event);
//			}
		}

		int _fashionCount = 0;
		int _decorCount = 0;
		int _catwalkCount = 0;
		int _CoOpCount = 0;
		int _SocietyCount = 0;
		foreach (var _event in _fashionEvent) {
			if (_event.EndTime < DateTime.UtcNow) {
				_fashionCount++;
			}
		}
		foreach (var _event in _decorEvent) {
			if (_event.EndTime < DateTime.UtcNow) {
				_decorCount++;
			}
		}
		foreach (var _event in _catWalkEvent) {
			if (_event.EndTime < DateTime.UtcNow) {
				_catwalkCount++;
			}
		}
		foreach (var _event in _coOpEvent) {
			if (_event.EndTime < DateTime.UtcNow) {
				_CoOpCount++;
			}

		}
		foreach (var _event in _societyEvent) {
			if (_event.EndTime < DateTime.UtcNow) {
				_SocietyCount++;
			}

		}
	}



	public void ClaimReward (int eventid, string link, string _eventName)
	{
		StartCoroutine (IeClaimreward (eventid, link, _eventName));
	}

	IEnumerator IeClaimreward (int eventid, string link, string _eventName)
	{
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.ClaimReward (link));
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
			StartCoroutine (TryUpdateReward (eventid, link, _eventName));
		}
	}


	IEnumerator TryUpdateReward (int eventid, string link, string _eventName)
	{
		CoroutineWithData update_cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.UpdateClaimedRewards (eventid, _eventName, link));
		yield return update_cd.coroutine;
		if (update_cd.result.ToString () == "true" || update_cd.result.ToString () == "True") {
//			print ("updated");
			yield return true;
		} else {
			StartCoroutine (TryUpdateReward (eventid, link, _eventName));
			yield return false;
		}
	}

	public void ShowRewards ()
	{
		DeleteoldRewardItems ();
		foreach (var rewardobj in Rewards) {
			GameObject go = (GameObject)Instantiate (RewardPrefab, Vector3.zero, Quaternion.identity);
			go.transform.SetParent (RewardContainer.transform);
			go.transform.localScale = Vector3.one;


			go.transform.GetChild (0).GetComponent<Text> ().text = rewardobj.NameofEvent;
			go.transform.GetChild (1).GetChild (0).GetComponent<Text> ().text = rewardobj.coinWin.ToString ();
			go.transform.GetChild (2).GetChild (0).GetComponent<Text> ().text = rewardobj.gemsWin.ToString ();
			go.transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = rewardobj.dressName [0];
			go.transform.GetChild (3).GetComponent<Image> ().sprite = rewardobj.dressIcon [0];
			go.transform.GetChild (4).GetChild (0).GetComponent<Text> ().text = rewardobj.decorName [0];
			go.transform.GetChild (4).GetComponent<Image> ().sprite = rewardobj.decorIcon [0];

		}
	}



	void SelectEventForCoOp (int eventId, int count)
	{
		foreach (var _event in NewEvents) {
			if (_event.Event_id == eventId)
				CurrentEvent = _event;
		}			
		ScreenAndPopupCall.Instance.ShowCoOpPanel ();
		
		if (count == 2) {
			ScreenAndPopupCall.Instance.ShowReadyScreen (false);
			CoOpEventController.Instance.StartTimer (10f);
		} else {
			ScreenAndPopupCall.Instance.ShowCoOpWaiting ();
			CoOpEventController.Instance.StartTimer (5f);
		}

//        ScreenAndPopupCall.Instance.ShowCharacterSelectionForCoOp();
	}

	public IEnumerator IePlayerCount (int Event_id, int SocietyId = 0)
	{
		var link = "";

		switch (category) {
		case eventType.CatWalk_Event:
			link = "http://pinekix.ignivastaging.com/events/catwalkGetEventRegistration";
			break;
		case eventType.CoOp_Event:
			link = "http://pinekix.ignivastaging.com/events/getCoopEventRegistration";
			break;
		case eventType.Decor_Event:
			link = "http://pinekix.ignivastaging.com/events/decor_getregisterevent";
			break;
		case eventType.Fashion_Event:
			link = "http://pinekix.ignivastaging.com/events/fashionshow_getregisterevent";
			break;
		case eventType.Society_Event:
			link = "http://pinekix.ignivastaging.com/societyEvents/getSocialEventRegistration";
			break;
		}

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var json = new JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		if (category == eventType.Society_Event) {
			json ["society_id"] = SocietyId.ToString ();
		}
		json ["event_id"] = Event_id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
//		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW (link, encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				yield return true;
			} else {

				yield return false;
			}
		} else
			yield return false;
	}



	public IEnumerator GetCoOpRegistration (int Event_Id)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = Event_Id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/getCoopEventRegistration", encoding.GetBytes (jsonElement.ToString ()), postHeader);


		yield return www;

		if (www.error == null) {

			JSONNode _jsnode = JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Coop Event registration data is following")) {
				JSONNode dataArray = _jsnode ["data"];
				CoopRegisterPlayers RegPlayers = new CoopRegisterPlayers ();
				int.TryParse (dataArray ["player1_id"], out RegPlayers.Player1Id);
				int.TryParse (dataArray ["player2_id"], out RegPlayers.Player2Id);

				RegPlayers.P1Name = dataArray ["player1_name"];
				RegPlayers.P2Name = dataArray ["player2_name"];
				int.TryParse (dataArray ["event_id"], out RegPlayers.EventId);
//				{"status":"200","description":"Coop Event registration data is following.","data":
//				{"event_id":"19","player1_id":"124","player1_flatmate_id":"60","player1_experience_level":"5",
//				"player2_id":"123","player2_flatmate_id":"55","player2_experience_level":"3","item_data":
//				[{"item_id":"80","item_type":"test1"},{"item_id":"71","item_type":"test"},{"item_id":"51","item_type":"test1"},{"item_id":"89","item_type":"test"}]}}


				JSONNode p1Dress_data = dataArray ["palyer1_item_data"];


				for (int a = 0; a < p1Dress_data.Count; a++) {
					var itemId = int.Parse (p1Dress_data [a] ["item_id"]);
					var itemCat = p1Dress_data [a] ["item_type"];
					if (itemCat.ToString ().Contains ("Gender")) {
						if (itemId == 1)
							RegPlayers.Player1Gender = "Male";
						else
							RegPlayers.Player1Gender = "Female";
					} else if (itemCat.ToString ().Contains ("SkinColor")) {
						RegPlayers.Player1Skin = itemId;
					} else {
						if (RegPlayers.player1_dressData.ContainsKey (itemId))
							RegPlayers.player1_dressData [itemId] = itemCat;
						else
							RegPlayers.player1_dressData.Add (itemId, itemCat);
					}
				}	

				JSONNode p2Dress_data = dataArray ["palyer2_item_data"];


				for (int a = 0; a < p2Dress_data.Count; a++) {
					var itemId = int.Parse (p2Dress_data [a] ["item_id"]);
					var itemCat = p2Dress_data [a] ["item_type"];
					if (itemCat.ToString ().Contains ("Gender")) {
						if (itemId == 1)
							RegPlayers.Player2Gender = "Male";
						else
							RegPlayers.Player2Gender = "Female";
					} else if (itemCat.ToString ().Contains ("SkinColor")) {
						RegPlayers.Player2Skin = itemId;
					} else {
						if (RegPlayers.player2_dressData.ContainsKey (itemId))
							RegPlayers.player2_dressData [itemId] = itemCat;
						else
							RegPlayers.player2_dressData.Add (itemId, itemCat);
					}
				}

				yield return RegPlayers;

			} else {
				yield return null;
			}
		} else
			yield return null;
	}




	public void CloseDescription ()
	{
		GetEventDataFromServer ();
	}

	public void WaitingForMyPair_fashion ()
	{
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;

		var Char = ScreenAndPopupCall.Instance.CharacterCamera.transform.GetChild (0).GetChild (0);
		Char.SetParent (ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
		Char.localPosition = Vector3.zero;
		Char.localScale = Vector3.one * 0.3f;

		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = false;
		ScreenAndPopupCall.Instance.ShowRegisteredPairForWaiting ();
		ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Title Text").GetComponent <Text> ().text = CurrentEvent.EventName;
		ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Theme Name").GetComponent <Text> ().text = CurrentEvent.EventTheme;
        ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player1 Name").GetComponent <Text> ().text = PlayerPrefs.GetString ("UserName");


		EventPairingController.Instance.StartTimerGetForPair (2f); // fashion event

		
	}

	public IEnumerator WaitingForMyPair_Coop (bool is1stPlayer)
	{	
		yield return new WaitForSeconds (0.5f);
		var cd = new CoroutineWithData (EventManagment.Instance, EventManagment.Instance.GetCoOpRegistration (EventManagment.Instance.CurrentEvent.Event_id));
		yield return cd.coroutine;

		if (cd.result != null) {
			
			var Registered = (CoopRegisterPlayers)cd.result;

			ScreenAndPopupCall.Instance.CloseScreen ();

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = false;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = false;

			ScreenAndPopupCall.Instance.ShowRegisteredPairForWaiting ();
			GameObject Player1 = null;
			GameObject Player2 = null;

			if (Registered.Player1Gender.Contains ("Male")) {
				Player1 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
			} else {
				Player1 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);		
			}


			Player1.transform.SetParent (ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
			Player1.transform.localScale = Vector3.one * 0.3f;
			Player1.transform.localPosition = new Vector3 (0.5f, 0f, 0);

			var Originaldumyy = DressManager.Instance.dummyCharacter;

			foreach (var dress in Registered.player1_dressData) {
				int id = dress.Key;
				string cat = dress.Value;
				if (cat.Contains ("Hair")) {
					var hairItem = FindSaloonWithId (id);
					if (hairItem != null) {
						DressManager.Instance.dummyCharacter = Player1;
//						if (Registered.Player1Skin == 1)
//							DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.HairImages);
//						else if (Registered.Player1Skin == 2)
//							DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.Brown_Images);
//						else if (Registered.Player1Skin == 3)
//							DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.Black_Images);
//						else
                        DressManager.Instance.ChangeHairsForDummyCharacter (hairItem.PartName, hairItem.HairImages);

					}
				} else {
					DressItem mydress = FindDressWithId (id);		
					if (mydress != null) {
						DressManager.Instance.dummyCharacter = Player1;
//						if (Registered.Player1Skin == 1)
							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//						else if (Registered.Player1Skin == 2)
//							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//						else if (Registered.Player1Skin == 3)
//							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//						else
//							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
					}
				}
			}

			if (Registered.Player1Gender.Contains ("Male")) {
				Player2 = (GameObject)Instantiate (VotingPairManager.Instance.MalePrefab, Vector3.zero, Quaternion.identity);
			} else {
				Player2 = (GameObject)Instantiate (VotingPairManager.Instance.FemalePrefab, Vector3.zero, Quaternion.identity);		
			}
			Player2.transform.SetParent (ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
			//		Char2.localPosition = Vector3.zero;
			Player2.transform.localScale = Vector3.one * 0.3f;
			Player2.transform.localPosition = new Vector3 (-0.5f, 0f, 0);

			foreach (var dress in Registered.player2_dressData) {
				int id = dress.Key;
				string cat = dress.Value;
				if (cat.Contains ("Hair")) {
					SaloonItem hairItem = FindSaloonWithId (id);
					if (hairItem != null) {
						DressManager.Instance.dummyCharacter = Player2;
//						if (Registered.Player2Skin == 1)
//							DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.HairImages);
//						else if (Registered.Player2Skin == 2)
//							DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.Brown_Images);
//						else if (Registered.Player2Skin == 3)
//							DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.Black_Images);
//						else
                        DressManager.Instance.ChangeHairsForDummyCharacter (hairItem.PartName, hairItem.HairImages);

					}
				} else {
					DressItem mydress = FindDressWithId (id);		
					if (mydress != null) {
						DressManager.Instance.dummyCharacter = Player2;
//						if (Registered.Player2Skin == 1)
							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//						else if (Registered.Player2Skin == 2)
//							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//						else if (Registered.Player2Skin == 3)
//							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//						else
//							DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
					}
				}
			}

			int Layer = LayerMask.NameToLayer ("UI3D");

			Player1.SetLayerRecursively (Layer);
			Player2.SetLayerRecursively (Layer);

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.565f, 0.0f, 0.41f, 0.8f);
			ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Title Text").GetComponent <Text> ().text = CurrentEvent.EventName;
			ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Theme Name").GetComponent <Text> ().text = CurrentEvent.EventTheme;
            ScreenManager.Instance.WaitingForOpponentScreen.transform.FindChild ("TitleBg").FindChild ("Player1 Name").GetComponent <Text> ().text = PlayerPrefs.GetString ("UserName");

			if (is1stPlayer)
				EventPairingController.Instance.StartTimerGetForPair (2.1f); // co-op event
			else
				EventPairingController.Instance.StartTimerGetForCheckOnly (2.12f);

			DressManager.Instance.dummyCharacter = Originaldumyy;
		} else {
			
		}		
	}

	DressItem FindDressWithId (int id)
	{
		foreach (var Item in PurchaseDressManager.Instance.AllDresses) {
			if (Item.Id == id) {				
				return Item;				
			} //else
			//return null;
		}
		return null;
	}

	SaloonItem FindSaloonWithId (int id)
	{
		foreach (var Item in PurchaseSaloonManager.Instance.AllItems) {
			if (Item.item_id == id) {				
				return Item;				
			} //else
			//return null;
		}
		return null;
	}

	public void GetCoOpEvents (int eventId, int count)
	{
		StartCoroutine (IGetCoOpEvent (eventId, count));
	}

	IEnumerator IGetCoOpEvent (int eventId, int count)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

//		jsonElement ["event_cat"] = "4";
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/getevents", encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());

			if (_jsnode ["description"].ToString ().Contains ("Events data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");

				JSONNode dataArray = _jsnode ["data"];
				List<EventDataStructure> _events = new List <EventDataStructure> ();

				for (int i = 0; i < dataArray.Count; i++) {
					EventDataStructure eventdata = new EventDataStructure ();

					eventdata.Event_id = int.Parse (dataArray [i] ["event_id"]);

					eventdata.EventName = dataArray [i] ["event_name"];
					eventdata.EventDetails = "Let's participate in" + dataArray [i] ["event_theme"] + " Event";

					eventdata.EventTheme = dataArray [i] ["event_theme"];

					if (dataArray [i].ToString ().Contains ("coins")) {
						eventdata.conditions.Add ("Coins", int.Parse (dataArray [i] ["fee_coins"]).ToString ());
					}

					if (dataArray [i].ToString ().Contains ("gems")) {
						eventdata.conditions.Add ("Gems", int.Parse (dataArray [i] ["fee_gems"]).ToString ());
					}

					if (dataArray [i].ToString ().Contains ("level")) {
						eventdata.conditions.Add ("Level", int.Parse (dataArray [i] ["level_required"]).ToString ());
					}

					eventdata.Rewards.Add (dataArray [i] ["rewards"], "1");

					DateTime time = Convert.ToDateTime (dataArray [i] ["start_time"]);
					eventdata.StartTime = time;

					DateTime Regtime = Convert.ToDateTime (dataArray [i] ["registration_time"]);
					eventdata.RegistrationTime = Regtime;

					DateTime endtime = Convert.ToDateTime (dataArray [i] ["end_time"]);
					eventdata.EndTime = endtime;

					eventdata.category = (eventType)int.Parse (dataArray [i] ["event_cat"]);

					_events.Add (eventdata);
				}
				NewEvents = _events.ToArray ();
			
				SelectEventForCoOp (eventId, count);
			
				yield return true;
			} else {
//				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	public IEnumerator GetAllEventsForCheck ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		jsonElement ["event_cat"] = "4";
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/getevents", encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());

			if (_jsnode ["description"].ToString ().Contains ("Events data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");

				JSONNode dataArray = _jsnode ["data"];

				var count = dataArray.Count;
				List<EventManagmentStruct> _events = new List <EventManagmentStruct> ();
				//				for (int x = 0; x < AllEventList.Length; x++) {
				//					_events.Add (AllEventList [x]);
				//				}

				for (int i = 0; i < count; i++) {
					EventManagmentStruct eventdata = new EventManagmentStruct ();

					eventdata.Event_id = int.Parse (dataArray [i] ["event_id"]);

					eventdata.EventTitle = dataArray [i] ["event_name"];
					eventdata.EventDescription = "Let's participate in" + dataArray [i] ["event_theme"] + " Event";

					eventdata.EventTheme = dataArray [i] ["event_theme"];

					if (dataArray [i].ToString ().Contains ("coins")) {
						eventdata.ConditionName.Add ("Coins");
						eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["fee_coins"]).ToString ());
					}

					if (dataArray [i].ToString ().Contains ("gems")) {
						eventdata.ConditionName.Add ("Gems");
						eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["fee_gems"]).ToString ());
					}

					if (dataArray [i].ToString ().Contains ("level")) {
						eventdata.ConditionName.Add ("Level");
						eventdata.ConditionValue.Add (int.Parse (dataArray [i] ["level_required"]).ToString ());
					}

					eventdata.RewardValue.Add (dataArray [i] ["rewards"]);

					DateTime time = Convert.ToDateTime (dataArray [i] ["start_time"]);
					eventdata.StartTime = time;

					DateTime Regtime = Convert.ToDateTime (dataArray [i] ["registration_time"]);
					eventdata.RegistrationTime = Regtime;

					DateTime endtime = Convert.ToDateTime (dataArray [i] ["end_time"]);
					eventdata.EndTime = endtime;

					eventdata.category = (eventType)int.Parse (dataArray [i] ["event_cat"]);

					_events.Add (eventdata);
				}
				AllEventList = _events;

				yield return true;
			} else {
//				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	public IEnumerator RegisterForSocietyEvent ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyManager.Instance._mySociety.Id.ToString ();
		jsonElement ["event_id"] = CurrentEvent.Event_id.ToString ();
		jsonElement ["experience_level"] = GameManager.Instance.level.ToString ();

		var Flatmate = RoommateManager.Instance.SelectedRoommate.GetComponent<Flatmate> ();
		jsonElement ["gender"] = PlayerPrefs.GetString ("CharacterType");
		jsonElement ["flatmate_id"] = Flatmate.data.Id.ToString ();

		var jsonarray = new JSONArray ();

		foreach (var item in Flatmate.data.Dress) {
			var jsonItem = new JSONClass ();
//			int itemId = 0;
//			if (item.Key.Contains ("Clothes") || item.Key.Contains ("Tops") || item.Key.Contains ("Jeans")) {
//				itemId = FindIdOfDressByName (item.Value, "Clothes");
//			} else if (item.Key.Contains ("Hair")) {
//				itemId = FindIdOfSaloonByName (item.Value, item.Key);
//			}

			jsonItem ["item_id"] = item.Value.ToString ();
			jsonItem ["item_type"] = item.Key;
			jsonarray.Add (jsonItem);
		}

		jsonElement ["items"] = jsonarray;


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (PinekixUrls.EventRegistrationUrl (category.ToString ()), encoding.GetBytes (jsonElement.ToString ()), postHeader);
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");

				PayFeeForRegistartion (CurrentEvent);

				Flatmate.data.BusyType = BusyType.FashionEvents;
				Flatmate.data.EventBusyId = CurrentEvent.Event_id;

				RoommateManager.Instance.StartBusyTimerForSelectedRoomMate (600f);// 7200 Seconds = 2x60x60 ie 2 hours 

				//TODO this will be changed to minutes instead of seconds. When FlatmateBusyTimer.cs <Line no. 41> is changed to AddMinutes instead of AddSeconds.
				ShowPopOfDescription ("You are successfully registered for " + CurrentEvent.EventName, () => { 

					ScreenAndPopupCall.Instance.CloseScreen ();
					WaitingForMyPair_fashion ();
				});
			
				
				yield return true;
			} else {
//				print ("error" + _jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));
				ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));

				yield return false;
			}

		} else {
			yield return false;
		}
	}


	public void ScrollToName (string Name)
	{
		var FindedObj = EventContainer.transform.FindChild (Name);
		var List = new List<string> ();

		if (FindedObj != null) {
			SnapTo (FindedObj.GetComponent<RectTransform> ());
		}
	}

	public void SnapTo (RectTransform target)
	{
		Canvas.ForceUpdateCanvases ();
		var scrollRect = EventContainer.GetComponentInParent <ScrollRect> ();
		var containerPanel = EventContainer.GetComponent<RectTransform> ();
		containerPanel.anchoredPosition =
			(Vector2)scrollRect.transform.InverseTransformPoint (containerPanel.position)
		- (Vector2)scrollRect.transform.InverseTransformPoint (target.position);
	}

	public void DeselectEventCategory ()
	{
		category = eventType.none;
	}
}


[Serializable]
public class LeaderboardData
{
	public int player_id;
	public int player_rank;
	public string player_name;
	public int player_score;
}

public class DecorEventRegistrationData
{
	public int player_id;
	public int event_id;
	public int experience_level;
	public string ground_texture_name = "";
	public string wall_texture_name = "";
	public List<KeyValuePair<string, int>> object_id = new List<KeyValuePair<string, int>> ();
	public List<KeyValuePair<string, string>> object_position = new List<KeyValuePair<string, string>> ();
}


public class CatWalkEventRegistration
{
	public string data_type;
	public int player_id;
	public int event_id;
	public int experience_level;
	public List<Dictionary<string, string>> items = new List<Dictionary<string, string>> ();
}

public class CoopRegisterPlayers
{
	public int EventId;
	public int Player1Id;
	public int Player2Id;
	public string P1Name;
	public string P2Name;
	public string Player1Gender;
	public string Player2Gender;
	public int Player1Skin;
	public int Player2Skin;
	public Dictionary <int, string> player1_dressData = new Dictionary<int, string> ();
	public Dictionary <int, string> player2_dressData = new Dictionary<int, string> ();
}