using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Simple_JSON;
using System;

/// <summary>
/// Co op event controller.
/// </summary>
public class CoOpEventController : Singleton<CoOpEventController>
{
	public Text WiatingTimerText;
	public Text ReadyTimerText;
	public Text WardrobeTimerText;
	public Text ChatBagdeCountText;

	public Button SubmitButton;

	public Text TitleText;
	public Text ThemeText;
	public Text Player1Name;
	public Text Player2Name;
	public bool hasOtherPlayerEntered;
	public bool hasMyPlayerEntered;
	public int hasSubmitCompleted = 0;
	public int playerCount = 0;

	public DateTime EndTime;
	public float Timer;
	public bool isTimerRunning;
	public CoOpPlayerData Player1Data;
	public CoOpPlayerData Player2Data;

	public string Player1NameString;
	public string Player2NameString;

	void Start ()
	{
	}

	void Awake ()
	{
		Reload ();
	}

	public void StartTimer (float _time)
	{
		EndTime = DateTime.UtcNow.AddMinutes (_time);
		isTimerRunning = true;
	}

	public void OnClickRandom ()
	{
		MultiplayerManager.Instance.JoinRoomRandomly ();
	}

	void Update ()
	{

		if (isTimerRunning) {

			if (MultiplayerManager.Instance._isReciever) {
				Player1NameString = Player2Data.UserName;
				Player2NameString = Player1Data.UserName;

				Player2Name.text = Player1Data.UserName;
				Player1Name.text = Player2Data.UserName;
			} else {
				Player1NameString = Player1Data.UserName;
				Player2NameString = Player2Data.UserName;

				Player1Name.text = Player1Data.UserName;
				Player2Name.text = Player2Data.UserName;
			}
			var Diff = EndTime - DateTime.UtcNow;
			Timer = (float)Diff.TotalSeconds;
			if (EndTime < DateTime.UtcNow) {
				isTimerRunning = false;	
				CloseScreen ();
				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
				return;
			}
		}

		if (!CoOpEventController.Instance.SubmitButton.gameObject.activeInHierarchy) {
			if (hasOtherPlayerEntered && !MultiplayerManager.Instance._isReciever && hasMyPlayerEntered) {
				SubmitButton.gameObject.SetActive (true);
			}
		}

//		if(EndTimer>=DateTime.UtcNow && isTimerRunning)
//		{
//			EndTimer -= Time.deltaTime;
//			if(MultiplayerManager.Instance._isReciever)
//			{
//				Player2Name.text = Player1Data.UserName;
//				Player1Name.text = Player2Data.UserName;
//			}
//			else{
//				Player1Name.text = Player1Data.UserName;
//				Player2Name.text = Player2Data.UserName;
//			}
//		}
//		else if(EndTimer<= 0 && isTimerRunning)
//		{
//			isTimerRunning = false;
//
//			CloseScreen ();
//		}	

		string TimeString = ExtensionMethods.GetShortTimeStringFromFloat (Mathf.Max (0.0f, Timer));
		WiatingTimerText.text = ReadyTimerText.text = WardrobeTimerText.text = TimeString;
	}

	public void CloseScreen ()
	{
		Timer = 0f;

		if (ChatManager.Instance.ChatConnected)
			ChatManager.Instance.DisConnectChatFromServer ();
		if (PhotonNetwork.inRoom)
			PhotonNetwork.LeaveRoom ();
		
		PhotonNetwork.Disconnect ();
		ScreenAndPopupCall.Instance.CloseScreen ();
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
//		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
		MultiplayerManager.Instance._forCoOp = false;
		MultiplayerManager.Instance._isReciever = false;
		Player1Data = new CoOpPlayerData ();
		Player2Data = new CoOpPlayerData ();
		hasMyPlayerEntered = false;
		hasOtherPlayerEntered = false;
		ScreenManager.Instance.CoOpChatScreen.SetActive (false);
		CoOpEventController.Instance.hasSubmitCompleted = 0;
	}

	public void OnDressConfirmInCoop ()
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
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();

		hasMyPlayerEntered = true;// Done
		ScreenAndPopupCall.Instance.ShowReadyScreen (true); //After Dress Step Up
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
	}


	public void SelectPlayerToSpwan ()
	{	
//		PurchaseDressManager.Instance.UpdateDressofAllCharacter ();
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
		string NameString =	RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ().data.Name;
		MultiplayerManager.Instance.SpawnPlayersForCoOp (NameString);

//		ScreenAndPopupCall.Instance.ShowCoOpPanel ();
//		ScreenAndPopupCall.Instance.ShowReadyScreen ();

//		hasMyPlayerEntered = true;
		if (hasOtherPlayerEntered && !MultiplayerManager.Instance._isReciever) {
			SubmitButton.gameObject.SetActive (true);
		}
//		else
//		{
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
//		}

//		ScreenManager.Instance.CoOpReadyScreen.transform.FindChild ("DressUp").gameObject.SetActive (false);
	}


	public void RegisterForCoOpEvent ()
	{
		StartCoroutine (IRegisterForCoOpEvent ());
	}

	public IEnumerator IRegisterForCoOpEvent ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

//		element.data_type = "save";
//		element.event_id = CurrentEvent.Event_id;
//		element.player_id = PlayerPrefs.GetInt ("PlayerId");
//		element.experience_level = GameManager.Instance.level;
//
//		foreach (var selected in SelectedRoommates) {
//			foreach (var downloadeditem in DownloadContent.Instance.downloaded_items) {
//				if (selected.GetComponent<Flatmate> ().data.Name == downloadeditem.Name) {
//					element.items.Add (AdditemObj ("item_id", downloadeditem.Item_id.ToString (), "item_type", "Flatmates"));
//				}
//			}
//
//			foreach (var key in selected.GetComponent<Flatmate> ().data.Dress.Keys) {
//				if (key == "Shoes") {
//					foreach (var downloaditem in DownloadContent.Instance.downloaded_items) {
//						if (selected.GetComponent<Flatmate> ().data.Dress [key] == downloaditem.Name) {
//							element.items.Add (AdditemObj ("item_id", downloaditem.Item_id.ToString (), "item_type", "Shoes"));
//						}
//					}
//				} else if (key == "hairs") {
//					foreach (var downloaditem in DownloadContent.Instance.downloaded_items) {
//						if (selected.GetComponent<Flatmate> ().data.Dress [key] == downloaditem.Name) {
//							element.items.Add (AdditemObj ("item_id", downloaditem.Item_id.ToString (), "item_type", "hairs"));
//						}
//					}
//				} else {
//					foreach (var downloaditem in DownloadContent.Instance.downloaded_items) {
//						if (selected.GetComponent<Flatmate> ().data.Dress [key] == downloaditem.Name) {
//							element.items.Add (AdditemObj ("item_id", downloaditem.Item_id.ToString (), "item_type", "dress"));
//						}
//					}
//				}
//			}
//		}


//		{
//			"event_id": "67",
//			"data_type":"save",
//			"player1_id": "71",
//			"player1_experience_level": " 4 ",
//			"player1_items": [{
//				"item_id": "89",
//				"item_type": "781123"
//			}, {
//				"item_id": "51",
//				"item_type": "781123"
//			}],
//			"player2_id": "72",
//			"player2_experience_level": " 8 ",
//			"player2_items": [{
//				"item_id": "71",
//				"item_type": "781123"
//			}, {
//				"item_id": "80",
//				"item_type": "781123"
//			}]
//		}

		var json = new JSONClass ();


		json ["data_type"] = "save";
		json ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();

		json ["player1_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		json ["player1_experience_level"] = GameManager.Instance.level.ToString ();
		json ["player1_flatmate_id"] = Player1Data.FlatmateId.ToString ();
		var jsonarray = new JSONArray ();
		int genderInt = 0;
		if (Player1Data.Gender == GenderEnum.Male)
			genderInt = 1;
		else
			genderInt = 2;
		
		var jsonGender = new JSONClass ();
		jsonGender ["item_id"] = genderInt.ToString ();
		jsonGender ["item_type"] = "Gender";
		jsonarray.Add (jsonGender);

		var jsonSkin = new JSONClass ();
		jsonSkin ["item_id"] = Player1Data.SkinColor.ToString ();
		jsonSkin ["item_type"] = "SkinColor";
		jsonarray.Add (jsonSkin);

		foreach (var item in Player1Data.Dresses) {
			var jsonItem = new JSONClass ();
//			int itemId = 0;
//			if (item.Key.Contains ("Clothes") || item.Key.Contains ("Tops") || item.Key.Contains ("Jeans")) {
//				itemId = FindIdOfDressByName (item.Value, item.Key);
//			} else if (item.Key.Contains ("Hair")) {
//				itemId = FindIdOfSaloonByName (item.Value, item.Key);
//			}
			jsonItem ["item_id"] = item.Value.ToString ();
			jsonItem ["item_type"] = item.Key;
			jsonarray.Add (jsonItem);
		}

		json ["player1_items"] = jsonarray;


		json ["player2_id"] = Player2Data.PlayerId.ToString ();
		json ["player2_experience_level"] = Player2Data.Level.ToString ();
		json ["player2_flatmate_id"] = Player2Data.FlatmateId.ToString ();
		var jsonarray2 = new JSONArray ();

		int P2genderInt = 0;
		if (Player2Data.Gender == GenderEnum.Male)
			P2genderInt = 1;
		else
			P2genderInt = 2;
		
		var jsonGender2 = new JSONClass ();
		jsonGender2 ["item_id"] = P2genderInt.ToString ();
		jsonGender2 ["item_type"] = "Gender";
		jsonarray2.Add (jsonGender2);
	
		var jsonSkin2 = new JSONClass ();
		jsonSkin2 ["item_id"] = Player2Data.SkinColor.ToString ();
		jsonSkin2 ["item_type"] = "SkinColor";
		jsonarray2.Add (jsonSkin2);

		foreach (var item in Player2Data.Dresses) {
			var jsonItem = new JSONClass ();

//			int itemId = 0;
//			if (item.Key.Contains ("Clothes") || item.Key.Contains ("Tops") || item.Key.Contains ("Jeans")) {
//				itemId = FindIdOfDressByName (item.Value, item.Key);
//			} else if (item.Key.Contains ("Hair")) {
//				itemId = FindIdOfSaloonByName (item.Value, item.Key);
//			}
			jsonItem ["item_id"] = item.Value.ToString ();
			jsonItem ["item_type"] = item.Key;

			jsonarray2.Add (jsonItem);
		}
		json ["player2_items"] = jsonarray2;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
		print ("jsonDtat is ==>> " + json.ToString ()); 
		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/coopRegistration", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");
				hasSubmitCompleted = 1;
				EventManagment.Instance.PayFeeForRegistartion (EventManagment.Instance.CurrentEvent);

				var flatmate = RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ();
				flatmate.data.BusyType = BusyType.CoopEvent;
				flatmate.data.EventBusyId = EventManagment.Instance.CurrentEvent.Event_id;

				RoommateManager.Instance.StartBusyTimerForSelectedRoomMate (600);//2 hours

				ShowPopOfDescription ("You are successfully registered for CoOp Event", () => {
//					var p1Data = new CoOpPlayerData (Player1Data);
////					p1Data = Player1Data;
//					var p2Data = new CoOpPlayerData (Player2Data);
//					p2Data = Player2Data;

					StartCoroutine (EventManagment.Instance.WaitingForMyPair_Coop (true));
				});
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("enterUniversityEvents");
				yield return true;
			} else {
				hasSubmitCompleted = 2;
				print ("error" + _jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));
				yield return false;
				ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()));

			}


		} else {
			yield return false;
		}
	}

	void ShowPopOfDescription (string message, UnityEngine.Events.UnityAction OnClickOkAction = null)
	{

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			ScreenManager.Instance.ClosePopup ();
			CloseScreen ();	
			if (OnClickOkAction != null)
				OnClickOkAction ();		
		});	
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

}

[Serializable]
public class CoOpPlayerData
{
	public string UserName;
	public int PlayerId;
	public int Level;
	public GenderEnum Gender;
	public int FlatmateId;
	public int SkinColor;
	public Dictionary<string , int > Dresses = new Dictionary<string, int> ();

	//	public string CharacterType;

	public CoOpPlayerData (CoOpPlayerData data)
	{
		UserName = data.UserName;
		PlayerId = data.PlayerId;
		Level = data.Level;
		Gender = data.Gender;
		FlatmateId = data.FlatmateId;
		SkinColor = data.SkinColor;
		Dresses = data.Dresses;
	}

	public CoOpPlayerData ()
	{
	}
}
