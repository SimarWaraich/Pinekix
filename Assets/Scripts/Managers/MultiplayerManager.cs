using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;


public class MultiplayerManager : Singleton<MultiplayerManager>
{
	public bool ReadyToJoinRoom;
	public bool RoomJoined;
	public string RoomName;
	public GameObject WaitingScreen;

	public bool _forCoOp = false;
	public bool _isReciever = false;
	public bool _isFlatPartyReciever = false;
	public bool _flatParty = false;
	public bool _societyParty = false;
	public bool _onPublicArea = false;
	public int CoOpEventId = 0;
	public int FriendId = 0;


	Vector2 CharacterOriginalPosition;


	public PublicAreaData[] PublicAreas;
	public PublicAreaData SelectedPublicArea;

	public MultiplayerPositions CurrentPublicArea;


	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.autoJoinLobby = false;
	}

	void Update ()
	{
		RoomJoined = PhotonNetwork.inRoom;
		ReadyToJoinRoom = PhotonNetwork.connectedAndReady;
	}

	public void WaitingScreenState (bool isActive)
	{
		Vector3 scale = isActive ? Vector3.one : Vector3.zero;
		iTween.ScaleTo (WaitingScreen, scale, 0.1f);
	}

	void ShowPopUpOfInterNetReachabilty (string message)
	{

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;// "You require an internet connection to access \"Public Areas\"." +
		//" Please make sure you have working internet connection.";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}

	#region On Ui Click Events

	public void OnClickPublicplaces ()
	{
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();

		if (!Tut._PublicAreaAccessed) {
			ScreenAndPopupCall.Instance.PublicAreaSelection ();
			GameManager.Instance.GetComponent <Tutorial> ().PublicAreaAccessing ();
		} else
			ConnectToServer ();
	}

	public void ConnectToServer ()
	{
		_onPublicArea = true;
		if (!PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.IsBusy) {
			
			if (!ParenteralController.Instance.activateParentel) {
				_forCoOp = false;
				_flatParty = false;
				if (Application.internetReachability == NetworkReachability.NotReachable) {
					ShowPopUpOfInterNetReachabilty ("You require an internet connection to access \"Public Areas\".Please make sure you have working internet connection.");
					return;
				} else
//					print ("Conecction ===== >>>>>> " + Application.internetReachability);

				if (PhotonNetwork.connectedAndReady) {
					ScreenAndPopupCall.Instance.PublicAreaSelection ();
//					GameManager.Instance.GetComponent <Tutorial> ().PublicAreaAccessing ();
					return;
				} else
					PhotonNetwork.ConnectUsingSettings (null);
				WaitingScreenState (true);
			} else {
				ParenteralController.Instance.ShowPopUpMessageForParentel ();
			}
		} else {
			ShowPopUpOfInterNetReachabilty ("Your Player is currently busy");
		}
	}



	public void ConnectToServerforCoOp ()
	{
		_forCoOp = true;
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			ShowPopUpOfInterNetReachabilty ("You require an internet connection to access \"Public Areas\".Please make sure you have working internet connection.");
			return;
		} else
//			print ("Conecction ===== >>>>>> " + Application.internetReachability);

		if (PhotonNetwork.connectedAndReady) {
			//            ScreenAndPopupCall.Instance.PublicAreaSelection ();
			//            GameManager.Instance.GetComponent <Tutorial> ().PublicAreaAccessing ();
			return;
		} else
			PhotonNetwork.ConnectUsingSettings (null);
		WaitingScreenState (true);
	}

	public void JoinRoomRandomly ()
	{
		RoomOptions roomOption = new RoomOptions ();
		roomOption.MaxPlayers = 2;
		_isReciever = true;
		_forCoOp = true;
		ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable ();
		expectedCustomRoomProperties.Add ("map", "" + EventManagment.Instance.CurrentEvent.Event_id);

		var expectedMaxPlayers = System.Convert.ToByte (2);
		PhotonNetwork.JoinRandomRoom (expectedCustomRoomProperties, expectedMaxPlayers);
		WaitingScreenState (true);
	}

	public void JoinRoomOnNetwork (string roomName, int PlayerLimit)
	{
		RoomOptions roomOption = new RoomOptions ();
		roomOption.MaxPlayers = System.Convert.ToByte (PlayerLimit);
		roomOption.IsVisible = false;
		TypedLobby lobby = new TypedLobby (); //("Public Places", LobbyType.Default);
		PhotonNetwork.JoinOrCreateRoom (roomName, roomOption, lobby);
		WaitingScreenState (true);
	}


	public void JoinorCreateRoomForCoOp (string roomName)
	{
		RoomOptions roomOption = new RoomOptions ();
		roomOption.MaxPlayers = 2;
		roomOption.IsVisible = false;
		roomOption.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable ();

		TypedLobby lobby = new TypedLobby ();// ("Coop Lobby", LobbyType.SqlLobby);
		PhotonNetwork.JoinOrCreateRoom (roomName, roomOption, lobby);
		WaitingScreenState (true);
	}

	public void LeaveRoom ()
	{
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut._PublicAreaAccessed || Tut.publicAreaAccess == 5) {
			if (Tut.publicAreaAccess < 5)
				Tut.DecreasePublicAreaAccess ();
			else
				Tut.PublicAreaAccessing ();
			ScreenAndPopupCall.Instance.MoveOutOfPublicArea ();
			if (GameObject.Find ("TutorialPlayer"))
				Destroy (GameObject.Find ("TutorialPlayer"));
			return;
		}


		if (ChatManager.Instance.ChatConnected)
			ChatManager.Instance.DisConnectChatFromServer ();
		PhotonNetwork.LeaveRoom ();
		PhotonNetwork.Disconnect ();
		WaitingScreenState (false);
		_onPublicArea = false;
		ScreenAndPopupCall.Instance.IsChatOpen = false;
//		GameManager.Instance.GetComponent <Tutorial> ().PublicAreaAccessing ();
	}

	public void LeavRoomForFlatParty ()
	{
		if (ChatManager.Instance.ChatConnected)
			ChatManager.Instance.DisConnectChatFromServer ();
		PhotonNetwork.LeaveRoom ();
		PhotonNetwork.Disconnect ();
		PhotonNetwork.offlineMode = false;
		ScreenManager.Instance.FlatPartyChatScreen.SetActive (false);
		ScreenManager.Instance.SocietyPartyChatScreen.SetActive (false);
		HostPartyManager.Instance.isChatInFlatPartyIsOn = false;
		SocietyPartyManager.Instance.ChatInSocietyParty = false;
		WaitingScreenState (false);
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
	}

	public void DisconnectNetworkForFlatParty ()
	{
		PhotonNetwork.LeaveRoom ();
		PhotonNetwork.Disconnect ();
		PhotonNetwork.offlineMode = false;
		WaitingScreenState (false);
	}

	public void CloseMultiplayerNetwork ()
	{
		_forCoOp = false;
		_isReciever = false;
		_isFlatPartyReciever = false;
		_flatParty = false;
		_societyParty = false;
		PhotonNetwork.Disconnect ();
		PhotonNetwork.offlineMode = false;
		WaitingScreenState (false);
	}

	#endregion

	#region Photon MultiPlayer Callbacks

	public virtual void OnConnectedToMaster ()
	{
		ReadyToJoinRoom = PhotonNetwork.connectedAndReady;
		if (_forCoOp) {
			CoOpEventController.Instance.Timer = 0;
			if (!_isReciever) {

				ScreenAndPopupCall.Instance.ShowCharacterSelectionForCoOp ();
//				ScreenAndPopupCall.Instance.ShowCoOpPanel ();
				
			} else {
				ScreenAndPopupCall.Instance.ShowCharacterSelectionForCoOp ();
			}
			WaitingScreenState (false);
		} else if (_flatParty) {
			ConnectToServerforFlatParty ();
//			ScreenAndPopupCall.Instance.ShowHostPartyListandCreate ();
//			HostPartyManager.Instance.GetFlatParty (1);
			WaitingScreenState (false);
		} else if (_societyParty) {
			ConnectToServerforSocietyParty ();
			WaitingScreenState (false);
		} else if (_onPublicArea) {
			ScreenAndPopupCall.Instance.PublicAreaSelection ();
//			GameManager.Instance.GetComponent <Tutorial> ().PublicAreaAccessing ();
		
			WaitingScreenState (false);

		}
	}

	public virtual void OnFailedToConnectToPhoton (DisconnectCause cause)
	{
		Debug.LogError ("Cause: " + cause);
		//        GameManager.Instance.GetComponent <Tutorial> ().UpdateTutorial ();
		GameManager.Instance.GetComponent <Tutorial> ().DecreasePublicAreaAccess ();
	}

	public virtual void OnJoinedLobby ()
	{
		Debug.Log ("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom ();
	}

	public virtual void OnPhotonRandomJoinFailed ()
	{
		Debug.Log ("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

		RoomOptions roomOption = new RoomOptions ();
		roomOption.MaxPlayers = 2;
		roomOption.IsVisible = true;
		string[] myPropertiesForLobby = { "map", "ai" };
		roomOption.CustomRoomPropertiesForLobby = myPropertiesForLobby;
		roomOption.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable ();
		roomOption.CustomRoomProperties.Add ("map", "" + EventManagment.Instance.CurrentEvent.Event_id);
		PhotonNetwork.CreateRoom (null, roomOption, null);
		_isReciever = false;
	}


	public void OnJoinedRoom ()
	{
		RoomJoined = PhotonNetwork.inRoom;
		RoomName = PhotonNetwork.room.name;
		var room = PhotonNetwork.room.customProperties;
		Debug.Log ("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");

		int PublicAreaTypeInt = 0;

		switch (RoomName) {
		case "Garden":
			PublicAreaTypeInt = 0;
			break;
		case "Cafe":
			PublicAreaTypeInt = 1;
			break;
		default:

			//            if(IsRandom)
			//            {
			//                if (RoomName.Contains ("CoOpEventRoom_"))
			//                {
			//                    PhotonNetwork.LeaveRoom ();
			//                    PhotonNetwork.CreateRoom (null, new RoomOptions () { MaxPlayers = 2 }, null);
			//                    _isReciever = false;
			//                }
			//            }
			//            else
			if (RoomName.Contains ("CoOpEventRoom_")) {
				var arrayString = RoomName.Split ('_');
				string FriendNametoInvite = "";
				CoOpEventController.Instance.Timer = 0;
				if (arrayString.Length == 3) {
					if (arrayString [1] == PlayerPrefs.GetString ("UserName")) {
						/// This means i am sender of Invitation and should send Push Notifiction to other user..
						FriendNametoInvite = arrayString [2];
					
						string message = string.Format ("Hey! {0} has sent you a request to join him in a \'{1}\' Coop Event", 
							                 PlayerPrefs.GetString ("UserName"), EventManagment.Instance.CurrentEvent.EventName);	
						
						PushScript.Instance.SendPushToUser (FriendNametoInvite, message);

						NotificationManager.Instance.SendInvitationToUser (FriendId, message, EventManagment.Instance.CurrentEvent.Event_id, MultiplayerManager.Instance.RoomName);
						if (PhotonNetwork.room.playerCount == 2) {
							ScreenAndPopupCall.Instance.ShowReadyScreen (false);// For First PlayerOn Join Room - Called Rarely
							CoOpEventController.Instance.StartTimer (10f);

						} else {
							ScreenAndPopupCall.Instance.ShowCoOpWaiting ();
							CoOpEventController.Instance.StartTimer (5f);
						}
						
					} else {
						/// This means i am on receivers side of Invitation and come here after accepting Push notification
						FriendNametoInvite = arrayString [1];
						//                        PushScript.Instance.SendPushToUser (FriendNametoInvite, ", Your invite is Accepted! ," + PlayerPrefs.GetString ("UserName") + "," +
						// EventManagment.Instance.CurrentEvent.Event_id + "," + MultiplayerManager.Instance.RoomName +",");
						EventManagment.Instance.EventType = eType.Event;
						CoOpEventController.Instance.playerCount = PhotonNetwork.room.playerCount;
						EventManagment.Instance.GetCoOpEvents (CoOpEventId, PhotonNetwork.room.playerCount);
//						    EventManagment.Instance.GetEventDataFromServer ();
						NotificationManager.Instance.DeleteInvitation (NotificationManager.Instance.lastIvitationClicked_Id, null);

					}
				}
				ChatManager.Instance.AddChannelToConnect (RoomName);
				ChatManager.Instance.ConnectToChatServer ();
				_forCoOp = true;

			} else {
				ChatManager.Instance.AddChannelToConnect (RoomName);
				ChatManager.Instance.ConnectToChatServer ();

				if (_isReciever) {
					ScreenAndPopupCall.Instance.ShowReadyScreen (false); // For Second Player On Join Room
					CoOpEventController.Instance.StartTimer (10f);
				} else {
					ScreenAndPopupCall.Instance.ShowCoOpWaiting ();
					CoOpEventController.Instance.StartTimer (5f);
				}
			}
			break;
		}
		///
		if (_forCoOp) {

		} else if (_flatParty) {
			ChatManager.Instance.AddChannelToConnect (RoomName);
//			ChatManager.Instance.ConnectToChatServer ();
//				ChatManager.Instance.DisConnectChatFromServer ();
			_societyParty = false;
			MoveToPublicAreaForFlat ();


		} else if (_societyParty) {
			ChatManager.Instance.AddChannelToConnect (RoomName);
//				ChatManager.Instance.ConnectToChatServer ();
			_flatParty = false;
			MoveToPublicAreaForSocietyParty ();

		} else {
			ChatManager.Instance.SelectChannelToConnect (RoomName);
			ChatManager.Instance.ConnectToChatServer ();
			_onPublicArea = true;

			ScreenAndPopupCall.Instance.MoveToPublicArea (PublicAreaTypeInt);
			MoveToPublicArea (false);
//			GameManager.Instance.GetComponent <Tutorial> ().PublicAreaAccessing ();

		}

//		print (PhotonNetwork.playerName + " : " + PhotonNetwork.player.name + " : " + PhotonNetwork.player.ID + " : " + PhotonNetwork.player.isLocal);
	}

	public void OnLeftRoom ()
	{
		if (_flatParty) {
			HostPartyManager.Instance.OnPartyLeaveFunction ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			_flatParty = false;
			return;
		} else if (_societyParty) {
			SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			_societyParty = false;
			return;
		} else if (!_forCoOp) {
			ScreenAndPopupCall.Instance.MoveOutOfPublicArea ();
			GameManager.Instance.GetComponent <Tutorial> ().DecreasePublicAreaAccess ();
			WaitingScreenState (false);
			return;

		} else {
			LeaveRoom ();
		}
	}

	public void  OnDisconnectedFromPhoton ()
	{

		if (_forCoOp) {
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
			CoOpEventController.Instance.CloseScreen ();
			CoOpEventController.Instance.hasSubmitCompleted = 0;
//			SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
//			HostPartyManager.Instance.OnPartyLeaveFunction ();

		} else if (_flatParty) {
			HostPartyManager.Instance.OnPartyLeaveFunction ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			_flatParty = false;
			return;
		} else if (_societyParty) {
			SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			_societyParty = false;
			return;
		} else {
			LeaveRoom ();
			ScreenAndPopupCall.Instance.MoveOutOfPublicArea ();
			//			if (GameManager.Instance.level >= 5 && RoommateManager.Instance.RoommatesHired.Length >= 3)
			//				GameManager.Instance.GetComponent <Tutorial> ().CheckForQuest ();
			//        else
			//            GameManager.Instance.GetComponent <Tutorial> ().EnablebuttonsAfterSecondPhase ();
			GameManager.Instance.GetComponent <Tutorial> ().DecreasePublicAreaAccess ();
			WaitingScreenState (false);
			//			SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
			//			HostPartyManager.Instance.OnPartyLeaveFunction ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
			CoOpEventController.Instance.CloseScreen ();
		} 
	}

	public void OnPhotonPlayerConnected (PhotonPlayer newPlayer)
	{
		if (_forCoOp) {
//			CoOpEventController.Instance.hasOtherPlayerEntered = true;
			//            ScreenAndPopupCall.Instance.ShowCoOpPanel ();
			ScreenAndPopupCall.Instance.ShowReadyScreen (false);	// For First Player, After Second Player Joins
			CoOpEventController.Instance.StartTimer (10);
		}
		if (_flatParty) {
			
		}
		//        Debug.LogError ("Second Player has entered the room");
	}

	public void OnPhotonPlayerDisconnected (PhotonPlayer newPlayer)
	{
		if (_forCoOp) {
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

			ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
			ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

			ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
			ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

			if (CoOpEventController.Instance.hasSubmitCompleted == 1)
				ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "You are successfully registered for CoOp Event";
			else if (CoOpEventController.Instance.hasSubmitCompleted == 2)
				ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Problem in registering your pair";
			else
				ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Oops! Looks like your partner has left the Event";

			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {

				ScreenManager.Instance.ClosePopup ();
				ScreenAndPopupCall.Instance.CloseCharacterCamera ();
				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
				if (CoOpEventController.Instance.hasSubmitCompleted == 1) {
					var flatmate = RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ();
					flatmate.data.BusyType = BusyType.CoopEvent;
					flatmate.data.EventBusyId = EventManagment.Instance.CurrentEvent.Event_id;
					RoommateManager.Instance.StartBusyTimerForSelectedRoomMate (600);
					StartCoroutine (EventManagment.Instance.WaitingForMyPair_Coop (false));
					
				}
				CoOpEventController.Instance.CloseScreen ();

				CoOpEventController.Instance.hasSubmitCompleted = 0;
				ScreenAndPopupCall.Instance.gameObject.GetComponentInChildren <CharacterSelectionController> ().ThisCamera.enabled = false;
			});
//		} else if (_societyParty) {
////			SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
//		} else if (_flatParty) {
//
//		} else if(_onPublicArea) {
//			CurrentPublicArea.ResetAllPoints ();
//			SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
//			HostPartyManager.Instance.OnPartyLeaveFunction ();

		}

	}

	#endregion

	public void DisconnectPublicArea ()
	{
		_onPublicArea = false;
		PhotonNetwork.Disconnect ();
	}

	void InstantiatePlayerForTutorial ()
	{
		var Position = CurrentPublicArea.GetEmptyPosition ();
       
		GameObject Go = GameObject.Instantiate (PlayerManager.Instance.MainCharacter, Position.position, Quaternion.identity)as GameObject;
		Go.name = "TutorialPlayer";
		Destroy (Go.GetComponent<Flatmate> ());
		Destroy (Go.GetComponent<GenerateMoney> ().MoneyIcon);
		Destroy (Go.GetComponent<GenerateMoney> ());
        Destroy(Go.GetComponent<RoomMateMovement> ());

		Camera.main.transform.position = new Vector3 (Mathf.Clamp (Position.position.x, SelectedPublicArea.Starting_x, SelectedPublicArea.Ending_x), 
			Mathf.Clamp (Position.position.y, SelectedPublicArea.Starting_y, SelectedPublicArea.Ending_y), -10f); 
	}

	IEnumerator SpawnPlayersEverywhere (GameObject Go)
	{
//		Vector3 position = new Vector2 (UnityEngine.Random.Range (SelectedPublicArea.Starting_x, SelectedPublicArea.Ending_x),
//			UnityEngine.Random.Range (SelectedPublicArea.Starting_y, SelectedPublicArea.Ending_y));

		yield return new WaitForSeconds (0.4f);
		Transform PositionGameObject = Go.GetComponent<MultiplayerPositions> ().GetEmptyPosition ();

		Camera.main.transform.position = new Vector3 (Mathf.Clamp (PositionGameObject.position.x, SelectedPublicArea.Starting_x, SelectedPublicArea.Ending_x), 
			Mathf.Clamp (PositionGameObject.position.y, SelectedPublicArea.Starting_y, SelectedPublicArea.Ending_y), -10f);
		GameObject go = null;
		if (GameManager.GetGender () == GenderEnum.Male)
			go = PhotonNetwork.Instantiate ("MaleCharacter", PositionGameObject.position, Quaternion.identity, 0);
		else
			go = PhotonNetwork.Instantiate ("FemaleCharacter", PositionGameObject.position, Quaternion.identity, 0);

//        var publicPlayer = go.AddComponent<PublicAreaPlayer>();
//        go.GetComponent<PhotonView>().ObservedComponents[0] = publicPlayer;
//        go.AddComponent<OpenViewMenu>();

		go.transform.localScale = new Vector3 (0.4f, 0.4f, 1);

        go.GetComponent <PublicAreaPlayer> ().PositionIndexInMp = int.Parse (PositionGameObject.name);

		if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
			AchievementsManager.Instance.CheckAchievementsToUpdate ("visitMultiplayerAreas");
	}




	public void SpawnPlayersForCoOp (string Name)
	{
		GameObject go = null;
		string CharacterName = "";
		if (GameManager.GetGender () == GenderEnum.Male) {
            CharacterName = "CharacterCoOp" + Name;

            //TODO change flatmates..
            go = PhotonNetwork.Instantiate ("CharacterCoOp", Vector3.zero, Quaternion.identity, 0);

		} else {
            CharacterName = "CharacterCoOp";
			go = PhotonNetwork.Instantiate (CharacterName, Vector3.zero, Quaternion.identity, 0);
		}



		if (_isReciever) {
			if (ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0).childCount != 0)
				GameObject.Destroy (ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0).GetChild (0).gameObject);
			go.transform.SetParent (ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0));
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one * 0.3f;
		} else {
			if (ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0).childCount != 0)
				GameObject.Destroy (ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0).GetChild (0).gameObject);
			go.transform.SetParent (ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
			go.transform.localPosition = Vector3.zero;
			go.transform.localScale = Vector3.one * 0.3f;
		}
	}


	public void MoveToPublicArea (bool IsForTutorial)
	{
//		SelectedPublicArea = PublicAreas [i];
		CharacterOriginalPosition = PlayerManager.Instance.MainCharacter.transform.position;

//		Camera.main.transform.position = new Vector3 (UnityEngine.Random.Range (SelectedPublicArea.Starting_x, SelectedPublicArea.Ending_x),
//			UnityEngine.Random.Range (SelectedPublicArea.Starting_y, SelectedPublicArea.Ending_y), -10f);
		//
		//		transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = SelectedPublicArea.Background;

		GameObject Go = Instantiate (SelectedPublicArea.BackgroundPrefab, new Vector3 (211, 5, 0), Quaternion.identity) as GameObject;
		CurrentPublicArea = Go.GetComponent<MultiplayerPositions> ();
		Go.name = "PublicAreaBackground";

		Camera.main.GetComponent <DragCamera1> ().enabled = false;
		Camera.main.GetComponent <DragableCamera> ().enabled = true;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMinX = SelectedPublicArea.Starting_x;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMaxX = SelectedPublicArea.Ending_x;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMaxY = SelectedPublicArea.Starting_y;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMinY = SelectedPublicArea.Ending_y;


		Camera.main.orthographicSize = 10f;
		if (IsForTutorial)
			InstantiatePlayerForTutorial ();
		else
			StartCoroutine (SpawnPlayersEverywhere (Go));
		//Delete all old messeage gameobjects in scene....
		for (int x = 0; x < ChatManager.Instance.message_panel.childCount; x++) {
			Destroy (ChatManager.Instance.message_panel.GetChild (x).gameObject);
		}
		
	}

	public void MoveToPublicAreaForFlat ()
	{

		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		//		SelectedPublicArea = PublicAreas [i];
		CharacterOriginalPosition = PlayerManager.Instance.MainCharacter.transform.position;

		Camera.main.transform.position = new Vector3 (212f, 4.8f, -10f);
//		transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = SelectedPublicArea.Background;
		GameObject Go;
		if (!Tut.HostPartyCreated) {
			SelectedPublicArea = PublicAreas [5];
			Go = Instantiate (SelectedPublicArea.BackgroundPrefab, new Vector3 (211, 5, 0), Quaternion.identity) as GameObject;
		} else
			Go = Instantiate (SelectedPublicArea.BackgroundPrefab, new Vector3 (211, 5, 0), Quaternion.identity) as GameObject;
		
		CurrentPublicArea = Go.GetComponent<MultiplayerPositions> ();
		Go.name = "FlatPartyPublicArea";
		GameObject roomConcatiner = GameObject.Instantiate (HostPartyManager.Instance.FlatPartyRoomContainer, Vector3.zero, Quaternion.identity)as GameObject;
		roomConcatiner.transform.parent = Go.transform;
		roomConcatiner.transform.localPosition = Vector3.zero;
		roomConcatiner.name = "FlatpartyRoomContainer";
		Camera.main.GetComponent <DragCamera1> ().enabled = false;
		Camera.main.GetComponent <DragableCamera> ().enabled = true;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMinX = SelectedPublicArea.Starting_x;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMaxX = SelectedPublicArea.Ending_x;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMaxY = SelectedPublicArea.Starting_y;
		Camera.main.GetComponent <DragableCamera> ().RestrictedMinY = SelectedPublicArea.Ending_y;


		Camera.main.orthographicSize = 10f;
	
//		GameObject Flat = Instantiate (RoomPurchaseManager.Instance.RoomTypePrefeb [1], new Vector3 (212, -3, 0), Quaternion.identity) as GameObject;
//		Flat.name = "FlatPartyRoom";

		HostPartyManager.Instance.SelectedPartyDecor.Clear ();
		SetRoomAndDecorForFlatParty ();
	
		GameObject[] room = GameObject.FindGameObjectsWithTag ("SocietyRoom");
		int randomRoom = UnityEngine.Random.Range (0, room.Length);
		if (Tut.HostPartyCreated) {
			ChatManager.Instance.MessageCountForFlatParty = 0;
			HostPartyManager.Instance.SpwanRealPlayerForFlatParty (room [randomRoom]);
		} else {
			// Spwan Offline player for flat party.
			HostPartyManager.Instance.SpwanPlayerForOfllineFlatParty (room [randomRoom]);
		}
		DisableWalls ();
		//Delete all old messeage gameobjects in scene....
//		for (int x = 0; x < ChatManager.Instance.message_panel.childCount; x++) {
//			Destroy (ChatManager.Instance.message_panel.GetChild (x).gameObject);
//		}	
	}

	/// <summary>
	/// Sets the room and decor for flat party.
	/// </summary>
	public List<GameObject> FlatPartyRoom = new List<GameObject> ();

	public void SetRoomAndDecorForFlatParty ()
	{
		FlatPartyRoom.Clear ();
		for (int i = 0; i < HostPartyManager.Instance.selectedFlatParty.RoomIetm.Count; i++) {
			GameObject GO = GameObject.Instantiate (RoomPurchaseManager.Instance.RoomTypePrefeb [0], Vector3.one, Quaternion.identity)as GameObject;

			///Set Room Types on Instantiate Gameobject
			string roomTypes = HostPartyManager.Instance.selectedFlatParty.RoomIetm [i].types.ToString ();
			string[] splitStringofRoomTyps = roomTypes.Split (new string[] { "/" }, StringSplitOptions.None);

			GO.GetComponent<Flat3D> ().data.AreaName = splitStringofRoomTyps [0];	
			GO.GetComponent<Flat3D> ().WallColourNames = splitStringofRoomTyps [1];
			GO.GetComponent<Flat3D> ().GroundTextureName = splitStringofRoomTyps [2];					
			if (!string.IsNullOrEmpty (GO.GetComponent<Flat3D> ().WallColourNames))
				GO.GetComponent<Flat3D> ().ChangeGroungColor (VotingPairManager.Instance.FindGroundTexture (GO.GetComponent<Flat3D> ().WallColourNames));
			if (!string.IsNullOrEmpty (GO.GetComponent<Flat3D> ().GroundTextureName))
				GO.GetComponent<Flat3D> ().ChangeGroungColor (VotingPairManager.Instance.FindGroundTexture (GO.GetComponent<Flat3D> ().GroundTextureName));
			
			/// Set Room Proprties on Instantiate Gameobject
			GameObject Area = GameObject.Find ("FlatPartyPublicArea");
			GO.transform.parent = Area.transform.FindChild ("FlatpartyRoomContainer").transform;
			GO.transform.localPosition = ExtensionMethods.DeserializeVector3ArrayExtented (HostPartyManager.Instance.selectedFlatParty.RoomIetm [i].propties.ToString ().Trim ('|'));
			GO.transform.name = splitStringofRoomTyps [0]; //"FlatParty" + i;
			GO.gameObject.tag = "SocietyRoom";
			GO.gameObject.GetComponent<ResizeGrid> ().gridHeight = 3;
			GO.gameObject.GetComponent<ResizeGrid> ().gridWidth = 3;
			GO.transform.FindChild ("grid Container").gameObject.SetActive (false);

			//Split Room Name 
			string[] splitRoomName = splitStringofRoomTyps [0].Split (new string[] { ":" }, StringSplitOptions.None);
			int.TryParse (splitRoomName [0], out GO.GetComponent<Flat3D> ().data.x);
			int.TryParse (splitRoomName [1], out GO.GetComponent<Flat3D> ().data.y);

			//Add in the list
			FlatPartyRoom.Add (GO);

		}

		for (int j = 0; j < HostPartyManager.Instance.selectedFlatParty.DecorIetm.Count; j++) {
			
			HostPartyManager.DecorObject Decor = new HostPartyManager.DecorObject ();
			Decor.Id = HostPartyManager.Instance.selectedFlatParty.DecorIetm [j].id;
			string DecorType = HostPartyManager.Instance.selectedFlatParty.DecorIetm [j].types.ToString ();
			string[] splitStringofRoomTyps = DecorType.Split (new string[] { "/" }, StringSplitOptions.None);
			Decor.name = splitStringofRoomTyps [1];
			Decor.ItemType = splitStringofRoomTyps [0];

			///Split Decor Vector
			string DecorVector = HostPartyManager.Instance.selectedFlatParty.DecorIetm [j].propties.ToString ();
			string[] splitStringofVector = DecorVector.Split (new string[] { "/" }, StringSplitOptions.None);
		
			Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (splitStringofVector [0]);

			int DecorDir = 0;
			int.TryParse (splitStringofVector [1], out DecorDir);
			Decor.Direction = DecorDir;
			HostPartyManager.Instance.SelectedPartyDecor.Add (Decor);
		
		}
		GameObject SelectedArea = GameObject.Find ("FlatPartyPublicArea");

		foreach (var decor in HostPartyManager.Instance.SelectedPartyDecor) {
			var _dec = VotingPairManager.Instance.FindDecore (decor.Id);
			var parent = SelectedArea;
			Create3DDecoreForHostParty (_dec, decor.Position, decor.Direction, parent);

		}
	
		ScreenAndPopupCall.Instance.ShowRunningPartyScreen ();

	}

	public void DisableWalls ()
	{
		GameObject Go = GameObject.Find ("FlatpartyRoomContainer");
		for (int i = 0; i < FlatPartyRoom.Count; i++) {
	
			if (Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x + 1).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ())
			    && !Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x + 1).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ()).GetComponent <ConstructionTimer> ()) {
				if (Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ()).GetChild (0).name.Contains ("Walls")) {
					Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ()).GetChild (0).GetChild (2).gameObject.SetActive (false);
					Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ()).GetChild (0).GetChild (3).gameObject.SetActive (false);
				}
			}
			if (Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y + 1).ToString ())
			    && !Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y + 1).ToString ()).GetComponent <ConstructionTimer> ()) {
				if (Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ()).GetChild (0).name.Contains ("Walls")) {
					Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ()).GetChild (0).GetChild (4).gameObject.SetActive (false);
					Go.transform.FindChild ((FlatPartyRoom [i].GetComponent<Flat3D> ().data.x).ToString () + ":" + (FlatPartyRoom [i].GetComponent<Flat3D> ().data.y).ToString ()).GetChild (0).GetChild (5).gameObject.SetActive (false);
				}
			}
		}
	}

	public void Create3DDecoreForHostParty (DecorData decor, Vector3 Pos, int dir, GameObject Parent)
	{
		if (decor == null)
			return;

		var asset = Resources.Load<Decor3DView> ("Decors/" + decor.Name.Trim ('"'));
		if (asset == null) {
			for (int i = 0; i < DecorController.Instance.DownloadedDecors.Count; i++) {
				if (DecorController.Instance.DownloadedDecors [i].GetComponent <Decor3DView> ().name == decor.Name.Trim ('"') || DecorController.Instance.DownloadedDecors [i].GetComponent <Decor3DView> ().name == DecorController.FirstCharToUpper (decor.Name.Trim ('"')))
					asset = DecorController.Instance.DownloadedDecors [i].GetComponent<Decor3DView> ();
			}
		}
		var _Layer = LayerMask.NameToLayer ("Default");
		GameObject Go = (GameObject)Instantiate (asset.gameObject, Pos, Quaternion.identity);	
		Destroy (Go.GetComponent<DragSnap> ());
		Go.transform.parent = Parent.transform;
		Go.transform.localPosition = Pos;	

//		var drs = Go.AddComponent<DragSnap> ();
//		drs.grid = Parent.transform.GetChild (5).gameObject;
		Go.SetLayerRecursively (_Layer);
		Go.SetMaterialRecursively ();
		Go.GetComponent<Decor3DView> ().direction = dir; // To be Confirmed taht this fires after Start of Decor3D 
		Go.GetComponent<Decor3DView> ().CreateDecore (decor);	
		Go.GetComponent<Decor3DView> ().Start ();
		Go.transform.FindChild ("SelectionParent").gameObject.SetActive (false);
		Destroy (Go.GetComponent<Decor3DView> ());

	
	}


	public void MoveToPublicAreaForSocietyParty ()
	{
		//		SelectedPublicArea = PublicAreas [i];
		CharacterOriginalPosition = PlayerManager.Instance.MainCharacter.transform.position;

        Camera.main.transform.position = new Vector3(-1000, 0,Camera.main.transform.position.z);
        //		
		//		transform.GetChild (0).GetComponent <SpriteRenderer> ().sprite = SelectedPublicArea.Background;

        GameObject BG = Instantiate (SelectedPublicArea.BackgroundPrefab, new Vector3 (-1000, 0, 0), Quaternion.identity) as GameObject;
		CurrentPublicArea = BG.GetComponent<MultiplayerPositions> ();

		BG.name = "SocietyPartyArea";

		Camera.main.GetComponent <DragCamera1> ().enabled = false;
		Camera.main.GetComponent <DragableCamera> ().enabled = true;

        Camera.main.GetComponent <DragableCamera> ().RestrictedMinX = -1050 ;
        Camera.main.GetComponent <DragableCamera> ().RestrictedMaxX = -950;
        Camera.main.GetComponent <DragableCamera> ().RestrictedMaxY = 50;
        Camera.main.GetComponent <DragableCamera> ().RestrictedMinY = -50;


		Camera.main.orthographicSize = 10f;

        int RoomIndex = SocietyManager.Instance.SelectedSociety.RoomType;
        var Room = SocietyManager.Instance.RoomPrefabsList[RoomIndex];

        GameObject Flat = Instantiate (Room, BG.transform) as GameObject;
        Flat.name = "SocietyPartyRoom";
        Flat.transform.localPosition = Vector3.zero;
        Flat.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
        Flat.transform.localScale = new Vector3(1f, 1f, 1f);

//		Flat.transform.FindChild ("grid Container").gameObject.SetActive (false);
		ScreenAndPopupCall.Instance.ShowSocietyRunningPartyScreen ();
		SocietyPartyManager.Instance.SpwanRealPlayerForSocietyParty (Flat);
		ChatManager.Instance.MessageCountForSocietyParty = 0;

        ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().GetAllDecorInSocietyRoom();

		//Delete all old messeage gameobjects in scene....
//		for (int x = 0; x < ChatManager.Instance.message_panel.childCount; x++) {
//			Destroy (ChatManager.Instance.message_panel.GetChild (x).gameObject);
//		}
	}


	public void MoveOutOfPublicArea ()
	{
		PlayerManager.Instance.MainCharacter.transform.position = CharacterOriginalPosition;
		Camera.main.transform.position = new Vector3 (0, 0, -10f);
		Camera.main.GetComponent <DragCamera1> ().enabled = true;
		Camera.main.GetComponent <DragableCamera> ().enabled = false;
		FriendsManager.Instance.ShowFriendInPublicArea = false;
		if (CurrentPublicArea)
			Destroy (CurrentPublicArea.gameObject);

		SelectedPublicArea = new PublicAreaData ();
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
		ScreenManager.Instance.ClosePopup ();
	}

	public void MoveOutOfPublicAreaForTutFlatParty ()
	{
		PlayerManager.Instance.MainCharacter.transform.position = CharacterOriginalPosition;
		Camera.main.transform.position = new Vector3 (0, 0, -10f);
		Camera.main.GetComponent <DragCamera1> ().enabled = true;
		Camera.main.GetComponent <DragCamera1> ().Cam_dragging = false;
		Camera.main.GetComponent <DragableCamera> ().enabled = false;
		FriendsManager.Instance.ShowFriendInPublicArea = false;
		if (CurrentPublicArea)
			Destroy (CurrentPublicArea.gameObject);

		SelectedPublicArea = new PublicAreaData ();
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
//		ScreenManager.Instance.ClosePopup ();
	}

	public void SelectPublicAreaToJoin (string Name)
	{
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();

		if (!Tut._PublicAreaAccessed) {
			SelectedPublicArea = PublicAreas [0];
			ScreenAndPopupCall.Instance.MoveToPublicArea (0);
			MoveToPublicArea (true);
			GameManager.Instance.GetComponent <Tutorial> ().PublicAreaAccessing ();
		} else {
			if (!ParenteralController.Instance.activateParentel) {
				foreach (var area in PublicAreas) {
					if (area.Name == Name)
						SelectedPublicArea = area;
				}
				if (!string.IsNullOrEmpty (SelectedPublicArea.Name))
					JoinRoomOnNetwork (SelectedPublicArea.Name, SelectedPublicArea.BackgroundPrefab.transform.childCount);
			} else {
				ParenteralController.Instance.ShowPopUpMessageForParentel ();
			}
		}
		if (GameObject.Find ("SocietyPartyRoom"))
			Destroy (GameObject.Find ("SocietyPartyRoom").gameObject);
	}

	#region Flat Party conectevity

	public void ConnectToServerforFlatParty ()
	{
		_flatParty = true;
		_societyParty = false;
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			ShowPopUpOfInterNetReachabilty ("You require an internet connection to access \"Flat Party\". Please make sure you have working internet connection.");
			return;
		} else
//			print ("Conecction ===== >>>>>> " + Application.internetReachability);

		if (PhotonNetwork.connectedAndReady) {
			///TODO: set here;
//			FlatPartyHostingControler.Instance.HostPartySubmit ();
			return;
		} else
			PhotonNetwork.ConnectUsingSettings (null);
		WaitingScreenState (true);
	}

	public void ConnectToServerforFlatPartyFromList ()
	{
		_flatParty = true;
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			ShowPopUpOfInterNetReachabilty ("You require an internet connection to access \"Flat Party\". Please make sure you have working internet connection.");
			return;
		} else
//			print ("Conecction ===== >>>>>> " + Application.internetReachability);

		if (PhotonNetwork.connectedAndReady) {
			///TODO: set here;
			//			FlatPartyHostingControler.Instance.HostPartySubmit ();
			return;
		} else
			PhotonNetwork.ConnectUsingSettings (null);
		WaitingScreenState (true);
	}

	public void JoinorCreateRoomForFlatParty (string roomName, int player)
	{
		
		SelectPublicAreaForFlat (roomName);
		RoomOptions roomOption = new RoomOptions ();
		roomOption.MaxPlayers = System.Convert.ToByte (player);
		roomOption.IsVisible = false;
		roomOption.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable ();

		TypedLobby lobby = new TypedLobby ();// ("Coop Lobby", LobbyType.SqlLobby);
		PhotonNetwork.JoinOrCreateRoom (roomName, roomOption, lobby);
		WaitingScreenState (true);
	}

	public void SelectPublicAreaForFlat (string name)
	{
		string temp = "Flat Party Area";
		foreach (var area in PublicAreas) {
			if (area.Name == temp)
				SelectedPublicArea = area;
		}
		if (!string.IsNullOrEmpty (SelectedPublicArea.Name)) {
			SelectedPublicArea.Name = name;
			JoinRoomOnNetwork (SelectedPublicArea.Name, HostPartyManager.Instance.selectedFlatParty.TotleMember);
		}

	}

	#endregion


	#region Scoiety Party conectevity

	public void ConnectToServerforSocietyParty ()
	{
		_societyParty = true;
		_flatParty = false;
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			ShowPopUpOfInterNetReachabilty ("You require an internet connection to access \"Flat Party\". Please make sure you have working internet connection.");
			return;
		} else
//			print ("Conecction ===== >>>>>> " + Application.internetReachability);

		if (PhotonNetwork.connectedAndReady) {
			///TODO: set here;
			//			FlatPartyHostingControler.Instance.HostPartySubmit ();
			return;
		} else
			PhotonNetwork.ConnectUsingSettings (null);
		WaitingScreenState (true);
	}

	public void JoinorCreateRoomForSocietyParty (string roomName, int player)
	{

		SelectPublicAreaForSocietyParty (roomName);
		RoomOptions roomOption = new RoomOptions ();
		roomOption.MaxPlayers = System.Convert.ToByte (player);
		roomOption.IsVisible = false;
		roomOption.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable ();

		TypedLobby lobby = new TypedLobby ();// ("Coop Lobby", LobbyType.SqlLobby);
		PhotonNetwork.JoinOrCreateRoom (roomName, roomOption, lobby);
		WaitingScreenState (true);
	}

	public void SelectPublicAreaForSocietyParty (string name)
	{
		string temp = "Society Party Area";
		foreach (var area in PublicAreas) {
			if (area.Name == temp)
				SelectedPublicArea = area;
		}
		if (!string.IsNullOrEmpty (SelectedPublicArea.Name)) {
			SelectedPublicArea.Name = name;
			JoinRoomOnNetwork (SelectedPublicArea.Name, SocietyPartyManager.Instance.selectedSocietyParty.TotleMember);
		}

	}

	#endregion

}