using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Simple_JSON;
using System.Text.RegularExpressions;

public class HostPartyManager :  Singleton<HostPartyManager>
{

	public FlatPartyHostingControler FlatPartyHostingCreationController;
	public List<HostParty> AllFlatParty;
	public GameObject AllFlatPartyContainer;
	public GameObject MyFlatPartyContainer;
	public GameObject AllFlatPartyParent;
	public GameObject MyFlatPartyParent;
	public List<HostPartyKickedOutPlayerList> KickedOutPlayerList;
	public List<DecorObject> SelectedPartyDecor = new List<DecorObject> ();

	public GameObject FlatPartyListButton;
	public GameObject FlatHostingButton;

	public List<Sprite> PartyIconList;
	public GameObject FlatPartyPrefabsList;

	public List<HostParty> _myFlatParty;
	public int pageNo;

	public const string FlatPartyLinkLink = "http://pinekix.ignivastaging.com/flats/getFlatParty";
	public const string ViewMyFlatPartyLink = "http://pinekix.ignivastaging.com/flats/getPlayerFlatParty";
	public const string DeleteFlatPartyLink = "http://pinekix.ignivastaging.com/flats/deleteFlatParty";
	public bool HostedParty = false;
	public bool AttendingParty = false;
	public bool isChatInFlatPartyIsOn = false;
	public HostParty selectedFlatParty;

	public GameObject CreateButton;
	public GameObject FlatPartyRoomContainer;
	public GameObject GameGIU;

	// Ofline Player Prebeb GameObject
	public GameObject[] FlatPartyPlayer;

	void Awake ()
	{
		Reload ();
	}

	void Start ()
	{
		CheckPartyStatus ();
	}


	/// <summary>
	/// Creates all flat party list.
	/// </summary>
	/// <param name="FlatParty">Flat party.</param>
	/// <param name="Flatcontainer">Flatcontainer.</param>
	public void CreateFlatPartyList (List<HostParty> FlatParty, GameObject Flatcontainer)
	{
		DeleteGameObjects (Flatcontainer);
		AllFlatPartyParent.SetActive (true);
		MyFlatPartyParent.SetActive (false);
		if (Flatcontainer == AllFlatPartyContainer) {	

			if (FlatParty.Count == 0)
				AllFlatPartyParent.transform.FindChild ("Message").gameObject.SetActive (true);
			else
				AllFlatPartyParent.transform.FindChild ("Message").gameObject.SetActive (false);	
		} 
		FlatParty.ForEach (flatParty => {
			if (flatParty.PartyEndTime > DateTime.Now) {	
			
				GameObject Go = GameObject.Instantiate (FlatPartyPrefabsList, Flatcontainer.transform)as GameObject;
				Go.transform.localPosition = Vector3.zero;
				Go.transform.localScale = Vector3.one;
				Go.GetComponent <HostPartyUI> ().HostParty = flatParty;
			
//				if (flatParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) {
//					if (flatParty.PartyEndTime > DateTime.Now) {
//						FlatPartyHostingControler.Instance.PartyHosted = true;
//						FlatPartyHostingControler.Instance.isPaid = true;
//					} else {
//						if (flatParty.PartyEndTime.AddSeconds (FlatPartyHostingControler.Instance.HostPartyCoolDown) > DateTime.Now) {
//							FlatPartyHostingControler.Instance.myPartyEnding = true;
//							FlatPartyHostingControler.Instance.CooldownShow.SetActive (true);
//							FlatPartyHostingControler.Instance.PartyHosted = true;
//							FlatPartyHostingControler.Instance.isPaid = true;
//							var dif = flatParty.PartyEndTime.AddSeconds (FlatPartyHostingControler.Instance.HostPartyCoolDown) - DateTime.Now;
//							float TimeRemain = (float)dif.TotalSeconds;
//							FlatPartyHostingControler.Instance.PartyHostCoolDownIfEnded (TimeRemain);
//						} else if (flatParty.PartyEndTime.AddSeconds (FlatPartyHostingControler.Instance.HostPartyCoolDown) < DateTime.Now) {
//							FlatPartyHostingControler.Instance.myPartyEnding = false;
//							FlatPartyHostingControler.Instance.CooldownShow.SetActive (false);
//							FlatPartyHostingControler.Instance.PartyHosted = false;
//							FlatPartyHostingControler.Instance.isPaid = false;
//						}
//						StartCoroutine (IDeleteSelectedFlatParty (flatParty.Id, 1));
//					}
//				}
			}
		});
	}

	/// <summary>
	/// Creates only my flat party list.
	/// </summary>
	/// <param name="FlatParty">Flat party.</param>
	/// <param name="Flatcontainer">Flatcontainer.</param>
	public void CreateMyFlatPartyList (List<HostParty> FlatParty, GameObject Flatcontainer)
	{
		DeleteGameObjects (Flatcontainer);
		AllFlatPartyParent.SetActive (false);
		MyFlatPartyParent.SetActive (true);
		if (Flatcontainer == MyFlatPartyContainer) {	
			for (int i = 0; i < FlatParty.Count; i++) {  
				if (FlatParty [i].PlayerId != PlayerPrefs.GetInt ("PlayerId")) {
					CreateButton.SetActive (true);
				} else {
					CreateButton.SetActive (false);	
				}
			}		
						
		} 

		FlatParty.ForEach (flatParty => {
			if (flatParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) { // this check will not needed if view party 
				GameObject Go = GameObject.Instantiate (FlatPartyPrefabsList, Flatcontainer.transform)as GameObject;
				Go.transform.localPosition = Vector3.zero;
				Go.transform.localScale = Vector3.one;
				Go.GetComponent <HostPartyUI> ().HostParty = flatParty;

				if (flatParty.PartyEndTime > DateTime.Now) {
					FlatPartyHostingControler.Instance.PartyHosted = true;
					FlatPartyHostingControler.Instance.isPaid = true;
				} else {
					if (flatParty.PartyEndTime.AddSeconds (FlatPartyHostingControler.Instance.HostPartyCoolDown) > DateTime.Now) {
						FlatPartyHostingControler.Instance.myPartyEnding = true;
						FlatPartyHostingControler.Instance.CooldownShow.SetActive (true);
						FlatPartyHostingControler.Instance.PartyHosted = true;
						FlatPartyHostingControler.Instance.isPaid = true;
						FlatPartyHostingControler.Instance.HostPartyCoolDown = 180f;
						var dif = flatParty.PartyEndTime.AddSeconds (FlatPartyHostingControler.Instance.HostPartyCoolDown) - DateTime.Now;
						float TimeRemain = (float)dif.TotalSeconds;
						FlatPartyHostingControler.Instance.PartyHostCoolDownIfEnded (TimeRemain);
					} else if (flatParty.PartyEndTime.AddSeconds (FlatPartyHostingControler.Instance.HostPartyCoolDown) < DateTime.Now) {
						FlatPartyHostingControler.Instance.myPartyEnding = false;
						FlatPartyHostingControler.Instance.CooldownShow.SetActive (false);
						FlatPartyHostingControler.Instance.PartyHosted = false;
						FlatPartyHostingControler.Instance.isPaid = false;					
						CreateButton.SetActive (true);	
						StartCoroutine (IDeleteSelectedFlatParty (flatParty.Id));
					}
					DeleteGameObjects (Flatcontainer);

				}
			} 
		});
	}

	/// <summary>
	/// Deletes the game objects.
	/// </this function will delete the whole party befor its creating from me the party container>
	/// <param name="container">Container.</param>

	public void DeleteGameObjects (GameObject container)
	{
		for (int i = 0; i < container.transform.childCount; i++) {
			GameObject.Destroy (container.transform.GetChild (i).gameObject);
		}
	}



	public void OnPartyLeaveFunction ()
	{	
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		selectedFlatParty = null;		

		Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
		Button.AllButtons [6].gameObject.SetActive (true);
		GameGUIControle (true);
		FlatPartyHostingControler.Instance.ScreenCanMove = false;
		ScreenAndPopupCall.Instance.CloseScreen ();
		HostPartyManager.Instance.AttendingParty = false;
		CheckPartyStatus ();
		if (GameObject.Find ("FlatPartyPublicArea"))
			Destroy (GameObject.Find ("FlatPartyRoom"));
		ScreenManager.Instance.RunningParty.transform.FindChild ("Name").GetChild (0).GetComponent<Text> ().text = "";
		ScreenManager.Instance.RunningParty.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = "";
		MultiplayerManager.Instance.LeavRoomForFlatParty ();
		MultiplayerManager.Instance.MoveOutOfPublicArea ();
		//		MultiplayerManager.Instance._flatParty = false;
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		tut.DecreaseHostPartyTutorial ();
	}

	public void OnPartyLeaveButton ()
	{
		ShowPartyLeaveConfirmation ("Are you sure you want to leave this flat party?");
	}

	public void LeaveFunction ()
	{
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		selectedFlatParty = null;		

		Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
		Button.AllButtons [6].gameObject.SetActive (true);
		GameGUIControle (true);
		FlatPartyHostingControler.Instance.ScreenCanMove = false;
		ScreenAndPopupCall.Instance.CloseScreen ();
		HostPartyManager.Instance.AttendingParty = false;
		CheckPartyStatus ();
		if (GameObject.Find ("FlatPartyPublicArea"))
			Destroy (GameObject.Find ("FlatPartyRoom"));
		ScreenManager.Instance.RunningParty.transform.FindChild ("Name").GetChild (0).GetComponent<Text> ().text = "";
		ScreenManager.Instance.RunningParty.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = "";
		MultiplayerManager.Instance.LeavRoomForFlatParty ();
		MultiplayerManager.Instance.MoveOutOfPublicArea ();
		//		MultiplayerManager.Instance._flatParty = false;
	}

	public void ShowPartyLeaveConfirmation (string message)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();	
			LeaveFunction ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
	}

	public void ConnectToChatInFlatParty ()
	{
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (Tut.HostPartyCreated) {
			ChatManager.Instance.ChatConnected = false;
			if (!ChatManager.Instance.ChatConnected) {
				MultiplayerManager.Instance.WaitingScreenState (true);
				ChatManager.Instance.AddChannelToConnect ("Flat_" + selectedFlatParty.Name);
				ChatManager.Instance.ConnectToChatServer ();
				isChatInFlatPartyIsOn = true;
			} else {
				ScreenAndPopupCall.Instance.ChatScreenInFlatPartyStatus ();
			}
		}

	}

	public void SendMessageInFlatParty (InputField inputField)
	{
		if (!string.IsNullOrEmpty (inputField.text) && !Regex.IsMatch (inputField.text, "^[ \t\r\n\u0200]*$")) {
			var Json = new JSONClass ();
			Json ["message"] = inputField.text;
			Json ["time"] = DateTime.UtcNow.ToBinary ().ToString ();
			string message = Json.ToString ();
			ChatManager.Instance.SendChatMessage (message);
		}
		inputField.text = "";
	}



	public void CloseFlatPartyFromList ()
	{		
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		selectedFlatParty = null;		

		Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
		Button.AllButtons [6].gameObject.SetActive (true);
		GameGUIControle (true);
		FlatPartyHostingControler.Instance.ScreenCanMove = false;
		ScreenAndPopupCall.Instance.CloseScreen ();
		HostPartyManager.Instance.AttendingParty = false;
		CheckPartyStatus ();
		MultiplayerManager.Instance.DisconnectNetworkForFlatParty ();

	}

	public void GameGUIControle (bool show)
	{
		if (show)
			GameGIU.GetComponent<Transform> ().localScale = Vector3.one;
		else
			GameGIU.GetComponent<Transform> ().localScale = Vector3.zero;
	}

	public void CheckPartyStatus ()
	{
//		if (AttendingParty) {
//			FlatMateOnCoolDown (false);
//		} else {
//			FlatMateOnCoolDown (true);
//		}		
	}

	public void GetAllNextDataAccordingToPaqge ()
	{
		pageNo++;
		GetFlatParty (1);
	}

	public void GetAllPreviousDataAccordingToPaqge ()
	{
		if (pageNo < 0)
			pageNo = 0;
		pageNo--;
		GetFlatParty (1);
	}

	public void GetFlatParty (int num)
	{
		
		StartCoroutine (IGetAllFlatParty (num));

	}

	public IEnumerator IGetAllFlatParty (int num)
	{
//		ScreenAndPopupCall.Instance.LoadingScreen ();
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["page_no"] = pageNo.ToString ();
//		pageNo.ToString ();
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (FlatPartyLinkLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Parties list are following")) {

				JSONNode data = _jsnode ["data"];
				var FlatPartys = new List<HostParty> ();
				for (int i = 0; i < data.Count; i++) {
					
					int partyId = 0;
					int.TryParse (data [i] ["party_id"], out partyId);

					string Partyname = data [i] ["party_name"];
					string Partydescription = data [i] ["party_desc"];
					int playerId = 0;
					int.TryParse (data [i] ["player_id"], out playerId);
					if (playerId == PlayerPrefs.GetInt ("PlayerId")) {
						PlayerPrefs.SetInt ("flat_party_id", partyId);
					}
					int max_no_of_guests = 0;
					int.TryParse (data [i] ["max_no_of_guests"], out max_no_of_guests);
					//TODO;
					int partyType = 0;
					bool pTpye;
					int.TryParse (data [i] ["party_type"], out partyType);
					if (partyType == 1) {
						pTpye = false;
					} else {
						pTpye = true;
					}

					DateTime TimeEnd = Convert.ToDateTime (data [i] ["party_end_time"]);
					DateTime partyCreatedTime = Convert.ToDateTime (data [i] ["party_created_time"]);

					int no_of_present_member = 0;
					int.TryParse (data [i] ["no_of_present_member"], out no_of_present_member);	
					int rooms;
					int.TryParse (data [i] ["no_of_rooms"], out rooms);

					var AllDecorList = new List<FlatPartyHostingControler.GameItemPropties> ();
					var AllRoomList = new List<FlatPartyHostingControler.GameItemPropties> ();
					FlatPartyHostingControler.GameItemPropties Decoritem;
					FlatPartyHostingControler.GameItemPropties RoomItem;
					JSONNode Decordata = data [i] ["items"];
					for (int j = 0; j < data [i] ["items"].Count; j++) {	

						int ChekcItemId = 0;
						int.TryParse (Decordata [j] ["item_id"], out ChekcItemId);
						if (ChekcItemId == 0) {
							int itemId = 0;
							int.TryParse (Decordata [j] ["item_id"], out itemId);
							string ItemTyp = "";
							ItemTyp = Decordata [j] ["item_type"].ToString ().Trim ('"');
							string properties = "";
							properties = Decordata [j] ["properties"].ToString ().Trim ('"');
							RoomItem = new FlatPartyHostingControler.GameItemPropties (itemId, ItemTyp, properties);
							AllRoomList.Add (RoomItem);
						} else {	
							int itemId = 0;
							int.TryParse (Decordata [j] ["item_id"], out itemId);
							string ItemTyp = "";
							ItemTyp = Decordata [j] ["item_type"].ToString ().Trim ('"');
							string properties = "";
							properties = Decordata [j] ["properties"].ToString ().Trim ('"');

							Decoritem = new FlatPartyHostingControler.GameItemPropties (itemId, ItemTyp, properties);
							AllDecorList.Add (Decoritem);

						}
					}
					HostParty hosparty = new HostParty (playerId, partyId, Partyname, Partydescription, max_no_of_guests, pTpye, TimeEnd, no_of_present_member, rooms, AllDecorList, AllRoomList, partyCreatedTime);

					FlatPartys.Add (hosparty);
				}
				AllFlatParty = FlatPartys;
				if (num == 1) {
					AllFlatPartyParent.transform.FindChild ("Message").gameObject.GetComponent<Text> ().text = "";
					CreateFlatPartyList (AllFlatParty, AllFlatPartyContainer);
				} 
			} else if (_jsnode ["status"].ToString ().Contains ("400") || _jsnode ["description"].ToString ().Contains ("No party has found")) {

				AllFlatPartyParent.transform.FindChild ("Message").gameObject.GetComponent<Text> ().text = "No flat party hosted";
				AllFlatParty.Clear ();
				_myFlatParty.Clear ();
				CreateFlatPartyList (AllFlatParty, AllFlatPartyContainer);
				AllFlatPartyParent.SetActive (true);
				MyFlatPartyParent.SetActive (false);
			}	
		}
//		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	public void ShowMyParty ()
	{
//		AllFlatParty.Clear ();
//		_myFlatParty.Clear ();
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (Tut.HostPartyCreated) {
			StartCoroutine (IEGetMyParty ());
		} else {
			AllFlatPartyParent.SetActive (false);
			MyFlatPartyParent.SetActive (true);
			AllFlatParty.Clear ();
			_myFlatParty.Clear ();
			CreateMyFlatPartyList (_myFlatParty, MyFlatPartyContainer);	
			CreateButton.SetActive (true);
		}
		ScreenAndPopupCall.Instance.LoadingScreenClose ();

	}

	public IEnumerator IEGetMyParty ()
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (ViewMyFlatPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Party data has followed")) {

				JSONNode data = _jsnode ["data"];
				var FlatPartys = new List<HostParty> ();

				int partyId = 0;
				int.TryParse (data ["party_id"], out partyId);

				int playerId = 0;
				int.TryParse (data ["player_id"], out playerId);
				string Partyname = data ["party_name"];
				string Partydescription = data ["party_desc"];

				int max_no_of_guests = 0;
				int.TryParse (data ["max_no_of_guests"], out max_no_of_guests);
				//TODO;
				int partyType = 0;
				bool pTpye;
				int.TryParse (data ["party_type"], out partyType);
				if (partyType == 1) {
					pTpye = false;
				} else {
					pTpye = true;
				}

				DateTime TimeEnd = Convert.ToDateTime (data ["party_end_time"]);
				DateTime partyCreatedTime = Convert.ToDateTime (data ["party_created_time"]);

				int no_of_present_member = 0;
				int.TryParse (data ["no_of_present_member"], out no_of_present_member);	
				int rooms;
				int.TryParse (data ["no_of_rooms"], out rooms);

				var AllDecorList = new List<FlatPartyHostingControler.GameItemPropties> ();
				var AllRoomList = new List<FlatPartyHostingControler.GameItemPropties> ();
				FlatPartyHostingControler.GameItemPropties Decoritem;
				FlatPartyHostingControler.GameItemPropties RoomItem;
				JSONNode Decordata = data ["items"];
				for (int j = 0; j < Decordata.Count; j++) {

					int ChekcItemId = 0;
					int.TryParse (Decordata [j] ["item_id"], out ChekcItemId);

					if (ChekcItemId == 0) {
						int itemId = 0;
						int.TryParse (Decordata [j] ["item_id"], out itemId);
						string ItemTyp = "";
						ItemTyp = Decordata [j] ["item_type"].ToString ().Trim ('"');
						string properties = "";
						properties = Decordata [j] ["properties"].ToString ().Trim ('"');
						RoomItem = new FlatPartyHostingControler.GameItemPropties (itemId, ItemTyp, properties);
						AllRoomList.Add (RoomItem);
					} else {	
						int itemId = 0;
						int.TryParse (Decordata [j] ["item_id"], out itemId);
						string ItemTyp = "";
						ItemTyp = Decordata [j] ["item_type"].ToString ().Trim ('"');
						string properties = "";
						properties = Decordata [j] ["properties"].ToString ().Trim ('"');

						Decoritem = new FlatPartyHostingControler.GameItemPropties (itemId, ItemTyp, properties);
						AllDecorList.Add (Decoritem);

					}
				}
				HostParty hosparty = new HostParty (playerId, partyId, Partyname, Partydescription, max_no_of_guests, pTpye, TimeEnd, no_of_present_member, rooms, AllDecorList, AllRoomList, partyCreatedTime);

				FlatPartys.Add (hosparty);

				_myFlatParty = FlatPartys;			
				CreateMyFlatPartyList (_myFlatParty, MyFlatPartyContainer);	
			} else if (_jsnode ["status"].ToString ().Contains ("400") || _jsnode ["description"].ToString ().Contains ("No party has found")) {
				AllFlatPartyParent.SetActive (false);
				MyFlatPartyParent.SetActive (true);
				AllFlatParty.Clear ();
				_myFlatParty.Clear ();
				CreateMyFlatPartyList (_myFlatParty, MyFlatPartyContainer);	
				CreateButton.SetActive (true);
			}
		}
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}


	//	public void DeleteFlatParty ()
	//	{
	//		StartCoroutine (IDeleteAllFlatParty ());
	//
	//	}
	//
	//	public IEnumerator IDeleteAllFlatParty ()
	//	{
	//		ScreenAndPopupCall.Instance.LoadingScreen ();
	//		var encoding = new System.Text.UTF8Encoding ();
	//
	//		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
	//		var jsonElement = new Simple_JSON.JSONClass ();
	//
	//
	//		//		pageNo.ToString ();
	//		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
	//		jsonElement ["flat_party_id"] = PlayerPrefs.GetInt ("flat_party_id").ToString ();
	//
	//		postHeader.Add ("Content-Type", "application/json");
	//		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
	//
	//		WWW www = new WWW (DeleteFlatPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);
	//
	//		print ("jsonDtat is ==>> " + jsonElement.ToString ());
	//		yield return www;
	//
	//		if (www.error == null) {
	//			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
	//			print ("_jsnode ==>> " + _jsnode.ToString ());
	//			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Flat party has removed successfully")) {
	//
	//				print ("flat party deleted from flatparty hosting controler");
	//
	//			}
	//		}
	//		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	//	}

	public IEnumerator IDeleteSelectedFlatParty (int flatNo)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();


		//		pageNo.ToString ();
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["flat_party_id"] = flatNo.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (DeleteFlatPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Flat party has removed successfully")) {
//				print ("flat party deleted from host party manager list");

			} 	
		}
	}


	/// <summary>
	/// Flats the mate on cool down.
	/// </this function will be activate when player is taking part in  flat party and till the party duration all the flatmate will be on cooldown>
	/// <param name="gameObjStatus">If set to <c>true</c> game object status.</param>
	public void FlatMateOnCoolDown (bool gameObjStatus)
	{
		for (int i = 1; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
			RoommateManager.Instance.RoommatesHired [i].gameObject.SetActive (gameObjStatus);
		}		 
	}

	public void ComfrimFlatPartyJoin ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Are you sure you want to join?";	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		///TODO
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => SocietyManager.Instance.RemovePlayer (PlayerPrefs.GetInt ("PlayerId")));
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}

	public void SpwanRealPlayerForFlatParty (GameObject obj)
	{
		if (HostPartyManager.Instance.selectedFlatParty.PresentMembers <= HostPartyManager.Instance.selectedFlatParty.TotleMember) {
			GameObject go = null;
			if (GameManager.GetGender () == GenderEnum.Male) {
				go = PhotonNetwork.Instantiate ("MaleCharacter ForFlat", obj.transform.FindChild ("grid Container").transform.GetChild (UnityEngine.Random.Range (0, 49)).position, Quaternion.identity, 0);
			} else {
				go = PhotonNetwork.Instantiate ("FemaleCharacter ForFlat", obj.transform.FindChild ("grid Container").transform.GetChild (UnityEngine.Random.Range (0, 49)).position, Quaternion.identity, 0);

			}
			go.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
			Vector3 Pos = go.transform.localPosition;
			Pos.z = -1f;
			var target = GameManager.Instance.FlatPartyWayPoints [UnityEngine.Random.Range (0, GameManager.Instance.FlatPartyWayPoints.Count)];

			go.transform.localPosition = target.transform.localPosition;
			go.gameObject.AddComponent<RoomMateMovement> ().currentWaypoint = target.GetComponent<WayPoint> ();
			var movmentScript = go.GetComponent<RoomMateMovement> ();
			if (go.GetComponent<CharacterProperties> ().Gender == "Male") {
				movmentScript.animatorFront = go.transform.FindChild ("Boy_Front").GetComponent<Animator> ();
				movmentScript.animatorBack = go.transform.FindChild ("Boy_Back").GetComponent<Animator> ();
			}else if(go.GetComponent<CharacterProperties> ().Gender == "Female")
			{
				movmentScript.animatorFront = go.transform.FindChild ("Girl_Front").GetComponent<Animator> ();
				movmentScript.animatorBack = go.transform.FindChild ("Girl_Back").GetComponent<Animator> ();
			}
			movmentScript.NowMove ();
			go.transform.localPosition = Pos;
		}
	}

	public void SpwanPlayerForOfllineFlatParty (GameObject obj)
	{
		
		GameObject go = null;

//		if (GameManager.GetGender () == GenderEnum.Male) {
            go = GameObject.Instantiate (PlayerManager.Instance.MainCharacter, obj.transform.FindChild ("grid Container").transform.GetChild (UnityEngine.Random.Range (0, 49)).position, Quaternion.identity)as GameObject;
//		} else {
//			go = GameObject.Instantiate (FlatPartyPlayer [1], obj.transform.FindChild ("grid Container").transform.GetChild (UnityEngine.Random.Range (0, 49)).position, Quaternion.identity)as GameObject;

//		}

        Destroy(go.GetComponent<Flatmate>());
        Destroy(go.GetComponent<GenerateMoney>());

		go.transform.localScale = new Vector3 (0.4f, 0.4f, 1);
		Vector3 Pos = go.transform.localPosition;
		Pos.z = -1f;
		go.name = "OfflineChar";
		var target = GameManager.Instance.FlatPartyWayPoints [UnityEngine.Random.Range (0, GameManager.Instance.FlatPartyWayPoints.Count)];

		go.transform.localPosition = target.transform.localPosition;
		go.gameObject.AddComponent<RoomMateMovement> ().currentWaypoint = target.GetComponent<WayPoint> ();
		var movmentScript = go.GetComponent<RoomMateMovement> ();
		if (go.GetComponent<CharacterProperties> ().Gender == "Male") {
			movmentScript.animatorFront = go.transform.FindChild ("Boy_Front").GetComponent<Animator> ();
			movmentScript.animatorBack = go.transform.FindChild ("Boy_Back").GetComponent<Animator> ();
		}else if(go.GetComponent<CharacterProperties> ().Gender == "Female")
		{
			movmentScript.animatorFront = go.transform.FindChild ("Girl_Front").GetComponent<Animator> ();
			movmentScript.animatorBack = go.transform.FindChild ("Girl_Back").GetComponent<Animator> ();
		}
		movmentScript.NowMove ();
		go.transform.localPosition = Pos;


	}



	[Serializable]
	public class HostParty
	{
		public int PlayerId;
		public int Id;
		public string Name;
		public string Description;
		public int TotleMember;
		public bool PartyType;
		public DateTime PartyEndTime;
		public int PresentMembers;
		public int Room;
		public List<FlatPartyHostingControler.GameItemPropties> DecorIetm;
		public List<FlatPartyHostingControler.GameItemPropties> RoomIetm;
		public DateTime PartyCreatedTime;

		public HostParty (int playerId, int id, string name, string descrptn, int totleMember, bool partyType, DateTime partyEndTime, int presentMembers,
		                  int _room, List<FlatPartyHostingControler.GameItemPropties> _Decor, List<FlatPartyHostingControler.GameItemPropties> _RoomItem, DateTime pratyCreated)
		{	
			PlayerId = playerId;
			Id = id;
			Name = name;
			Description = descrptn;
			TotleMember = totleMember;
			PartyType = partyType;
			PartyEndTime = partyEndTime;
			PresentMembers = presentMembers;
			Room = _room;
			DecorIetm = _Decor;
			RoomIetm = _RoomItem;
			PartyCreatedTime = pratyCreated;
		}
	}

	[Serializable]
	public class HostPartyKickedOutPlayerList
	{
		public int PlayerId;
		public int PartyId;
		public int Partytyp;

		public HostPartyKickedOutPlayerList (int playerId, int id, int pTyp)
		{	
			PlayerId = playerId;
			PartyId = id;	
			Partytyp = pTyp;
		}
	}


	[Serializable]
	public class DecorObject
	{
		public int Id;
		public Vector3 Position;
		public string name;
		public string ItemType;
		public int Direction;
	}



}


