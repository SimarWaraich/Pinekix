using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using ExitGames.Client.Photon.Chat;
using System.Collections.Generic;
using Simple_JSON;


[Serializable]
public struct PublicAreaData
{
	public string Name;
	public float Starting_x;
	public float Ending_x;
	public float Starting_y;
	public float Ending_y;
	public GameObject BackgroundPrefab;
}

public class ChatManager : Singleton<ChatManager>, IChatClientListener
{

	public GameObject other_message;
	public GameObject players_message;
	public GameObject OtherEventInfoMsg;
	public GameObject SeftEventInfoMsg;
	public Transform message_panel;
	public InputField Inputfield;


	[Header ("Variables for Chat in CoOp Events")]
	public Transform message_panelForCoOp;
	public Transform message_panelForFlatParty;
	public Transform message_panelForSocietyParty;
	public InputField InputfieldCoOp;
	public GameObject CoOp_message1;
	public GameObject CoOp_message2;
	//	public GameObject ChatenterButton;
	//	public Button SendMessageButton;

	#region MyVariable for Chat

	public const string ChatAppId = "c702080c-0fb6-42ed-9d2c-8121cd06c015";
	public ChatClient chatClient;
	public string selectedChannelName;
	public string[] ChannelsToJoinOnConnect = new string[4]{ "Garden", "Cafe", "Campus", "Dummy" };
	int HistoryLength = 20;
	bool ChatJoin = false;

	#endregion


	void Awake ()
	{

		this.Reload ();
	}

	public void DisConnectChatFromServer ()
	{
//		if(this.chatClient.State != ChatState.Disconnected)
		this.chatClient.Disconnect ();
	}

	public void AddChannelToConnect (string newChannel)
	{
		bool alreadyAdded = false;
		var Temp = new List<string> ();

		foreach (var chnl in ChannelsToJoinOnConnect) {
			Temp.Add (chnl);

			if (chnl == newChannel)
				alreadyAdded = true;
		}
		if (!alreadyAdded)
			Temp.Add (newChannel);
		ChannelsToJoinOnConnect = Temp.ToArray ();
		SelectChannelToConnect (newChannel);
	}

	public void SelectChannelToConnect (string channel)
	{		
		foreach (var chnl in ChannelsToJoinOnConnect) {
			if (chnl == channel) {
				selectedChannelName = chnl;
				return;
			}
		}
	}

	#region ChatServer CallBacks

	public void ConnectToChatServer ()
	{
		Application.runInBackground = true; // this must run in background or it will drop connection if not focussed.

		if (string.IsNullOrEmpty (ChatAppId)) {
			Debug.LogError ("You need to set the chat app ID in the inspector in order to continue.");

			return;
		}

		this.chatClient = new ChatClient (this);
		this.chatClient.Connect (ChatAppId, "1.0", new ExitGames.Client.Photon.Chat.AuthenticationValues (PlayerPrefs.GetString ("UserName")));

	}

	public void Update ()
	{
		if (this.chatClient != null) {
			this.chatClient.Service (); // make sure to call this regularly! it limits effort internally, so calling often is ok!
		}

		//		this.StateText.gameObject.SetActive(ShowState); // this could be handled more elegantly, but for the demo it's ok.
	}

	public void SendChatMessage (string message)
	{
		this.chatClient.PublishMessage (this.selectedChannelName, message);
	}

	public void OnGetMessages (string channelName, string[] senders, object[] messages)
	{
		if (channelName.Equals (this.selectedChannelName)) {// update text
			ShowChannel (this.selectedChannelName);		

			if (SocietyManager.Instance.isChatInSocietyOn) {
				for (int i = 0; i < Mathf.Min (senders.Length, messages.Length); i++) {
					ScreenManager.Instance.Admin_MemberDiscriptionPanel.GetComponent<SocietyDescriptionController> ().GenerateMessageInSociety (messages [i].ToString (), senders [i].ToString ());
				}
				return;
			}

//			if (!MultiplayerManager.Instance._forCoOp) {
//				for (int i = 0; i < Mathf.Min (senders.Length, messages.Length); i++) {
//					GenerateMessage (messages [i].ToString (), senders [i].ToString ());
//				}
//			}
			if (MultiplayerManager.Instance._forCoOp) {
				for (int i = 0; i < Mathf.Min (senders.Length, messages.Length); i++) {
					GenerateMessageInCoOp (messages [i].ToString (), senders [i].ToString ());
				}
			} else if (MultiplayerManager.Instance._flatParty && HostPartyManager.Instance.isChatInFlatPartyIsOn) {
				for (int i = 0; i < Mathf.Min (senders.Length, messages.Length); i++) {
					GenerateMessageInFlatParty (messages [i].ToString (), senders [i].ToString ());
				}
			} else if (MultiplayerManager.Instance._societyParty && SocietyPartyManager.Instance.ChatInSocietyParty) {
				for (int i = 0; i < Mathf.Min (senders.Length, messages.Length); i++) {
					GenerateMessageInSocietyParty (messages [i].ToString (), senders [i].ToString ());
				}
			} else {
				for (int i = 0; i < Mathf.Min (senders.Length, messages.Length); i++) {
					GenerateMessage (messages [i].ToString (), senders [i].ToString ());
				}
			}
		}
	}

	public void ShowChannel (string channelName)
	{
		if (string.IsNullOrEmpty (channelName)) {
			return;
		}

		ChatChannel channel = null;
		bool found = this.chatClient.TryGetChannel (channelName, out channel);
		if (!found) {
			Debug.Log ("ShowChannel failed to find channel: " + channelName);
			return;
		}

		this.selectedChannelName = channelName;
		Debug.Log ("ShowChannel: " + this.selectedChannelName);

	}

	public void DebugReturn (ExitGames.Client.Photon.DebugLevel level, string message)
	{
		if (level == ExitGames.Client.Photon.DebugLevel.ERROR) {
			UnityEngine.Debug.LogError (message);
		} else if (level == ExitGames.Client.Photon.DebugLevel.WARNING) {
			UnityEngine.Debug.LogWarning (message);
		} else {
			UnityEngine.Debug.Log (message);
		}
	}

	public bool ChatConnected = false;

	public void OnConnected ()
	{
		if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0) {
			this.chatClient.Subscribe (this.ChannelsToJoinOnConnect, HistoryLength);
		}
		ChatConnected = true;
		MultiplayerManager.Instance.WaitingScreenState (false);
//		this.chatClient.AddFriends (new string[] { "tobi", "ilya" }); // Add some users to the server-list to get their status updates
		this.chatClient.SetOnlineStatus (ChatUserStatus.Online); // You can set your online state (without a mesage).

		if (SocietyManager.Instance.isChatInSocietyOn) {
			var societyDescriptionController = ScreenManager.Instance.Admin_MemberDiscriptionPanel.GetComponent<SocietyDescriptionController> ();
			societyDescriptionController.DeleteOldMessageGameObjects ();
			societyDescriptionController.ChatScreen.SetActive (true);
			societyDescriptionController.ChatScreenStatus ();
			return;
		} else if (HostPartyManager.Instance.isChatInFlatPartyIsOn) {
			for (int i = 0; i < message_panelForFlatParty.childCount; i++) {
				Destroy (message_panelForFlatParty.GetChild (i).gameObject);
			}		
			FlatPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (false);
			MessageCountForFlatParty = 0;
			ChatJoin = true;
			StartCoroutine (OnJoinChat ());
			return;
		} else if (SocietyPartyManager.Instance.ChatInSocietyParty) {
			for (int i = 0; i < message_panelForSocietyParty.childCount; i++) {
				Destroy (message_panelForSocietyParty.GetChild (i).gameObject);
			}
			SocietyPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (false);
			MessageCountForSocietyParty = 0;
			ChatJoin = true;
			StartCoroutine (OnJoinChat ());
		} 
		if (!MultiplayerManager.Instance._forCoOp) {
			ScreenAndPopupCall.Instance.IsChatOpen = false;
			if (!MultiplayerManager.Instance._societyParty)
				ScreenAndPopupCall.Instance.ChatInPublicArea ();
		} else {
//			if (MultiplayerManager.Instance._isReciever) {
//				CoOpEventController.Instance.StartTimer (10f);
//			} else {
//				CoOpEventController.Instance.StartTimer (5f);
//			}
			for (int i = 0; i < message_panelForCoOp.childCount; i++) {
				Destroy (message_panelForCoOp.GetChild (i).gameObject);
			}
			for (int i = 0; i < message_panelForFlatParty.childCount; i++) {
				Destroy (message_panelForFlatParty.GetChild (i).gameObject);
			}
			MessageCount = 0;
			MessageCountForFlatParty = 0;
		}

	}

	public void OnDisconnected ()
	{
		//		ScreenAndPopupCall.Instance.BackFromChat ();
		ChatConnected = false;
		ChatJoin = false;
		Application.runInBackground = false;

		if (SocietyManager.Instance.isChatInSocietyOn) {	
			SocietyDescriptionController.Instance.CloseChatscreen ();
			return;
		} else if (HostPartyManager.Instance.isChatInFlatPartyIsOn) {
			ScreenManager.Instance.FlatPartyChatScreen.SetActive (false);
			return;
		} else if (SocietyPartyManager.Instance.ChatInSocietyParty) {
			ScreenManager.Instance.SocietyPartyChatScreen.SetActive (false);
		}
		if (!MultiplayerManager.Instance._forCoOp) {
			if (MultiplayerManager.Instance._flatParty) {
//				print ("Network Disconnected");	
				MultiplayerManager.Instance._flatParty = false;
			} else
				ScreenAndPopupCall.Instance.MoveOutOfPublicArea ();
		}

	}

	public void OnChatStateChange (ChatState state)
	{
		// use OnConnected() and OnDisconnected()
		// this method might become more useful in the future, when more complex states are being used.

		//		this.StateText.text = state.ToString();
	}

	public void OnStatusUpdate (string user, int status, bool gotMessage, object message)
	{
		// this is how you get status updates of friends.
		// this demo simply adds status updates to the currently shown chat.
		// you could buffer them or use them any other way, too.

		// TODO: add status updates
		//if (activeChannel != null)
		//{
		//    activeChannel.Add("info", string.Format("{0} is {1}. Msg:{2}", user, status, message));
		//}

		Debug.LogWarning ("status: " + string.Format ("{0} is {1}. Msg:{2}", user, status, message));
	}

	public void OnPrivateMessage (string sender, object message, string channelName)
	{
		// as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
		// you also get messages that you sent yourself. in that case, the channelName is determinded by the target of your msg

		byte[] msgBytes = message as byte[];
		if (msgBytes != null) {
			Debug.Log ("Message with byte[].Length: " + msgBytes.Length);
		}
		if (this.selectedChannelName.Equals (channelName)) {

		}
	}

	public void OnUnsubscribed (string[] channels)
	{

		foreach (string channelName in channels) {
			if (channelName == selectedChannelName && channels.Length > 0) {

			}
		}
	}

	public void OnSubscribed (string[] channels, bool[] results)
	{
		foreach (string channel in channels) {
			//			this.chatClient.PublishMessage (channel, "says 'hi'."); // you don't HAVE to send a msg on join but you could.
		}
	}

	#endregion

	public void GenerateMessage (string message, string sender)
	{
		GameObject obj = null;

		if (sender == PlayerManager.Instance.playerInfo.Username)
			obj = (GameObject)Instantiate (players_message);
		else
			obj = (GameObject)Instantiate (other_message);			


		obj.transform.SetParent (message_panel, false);
		obj.transform.localScale = Vector3.one;

		// get messages from others and apply here....
		obj.transform.FindChild ("Message").GetComponent<Text> ().text = message;
		//		obj.transform.FindChild ("Image").GetComponent<Image> ().sprite = PlayerImages [UnityEngine.Random.Range (0, PlayerImages.Length)];
		obj.transform.FindChild ("UserName").GetComponent<Text> ().text = sender;
		obj.transform.FindChild ("Time Text").GetComponent<Text> ().text = DateTime.Now.ToString ("hh:mm tt");

		Invoke ("scrollValue", 0.1f);

	}

	void scrollValue ()
	{
		message_panel.parent.parent.FindChild ("Scrollbar Vertical").GetComponent<Scrollbar> ().value = 0;
	}

	public void Send ()
	{
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut._PublicAreaAccessed) {
			if (!string.IsNullOrEmpty (Inputfield.text) && !Regex.IsMatch (Inputfield.text, "^[ \t\r\n\u0200]*$")) {
				GenerateMessage (this.Inputfield.text, PlayerPrefs.GetString ("UserName"));
				this.Inputfield.text = "";
				if (Tut.publicAreaAccess < 5)
					Tut.PublicAreaAccessing ();
			}
			return;
		}

		if (!MultiplayerManager.Instance._forCoOp) {
			if (!string.IsNullOrEmpty (Inputfield.text) && !Regex.IsMatch (Inputfield.text, "^[ \t\r\n\u0200]*$")) {
//				if (GameManager.Instance.GetComponent<Tutorial> ().publicAreaAccess < 5)
//					GameManager.Instance.GetComponent<Tutorial> ().PublicAreaAccessing ();
				SendChatMessage (this.Inputfield.text);
				this.Inputfield.text = "";

			} else {
				this.Inputfield.text = "";
			}
		} else {
			if (!string.IsNullOrEmpty (InputfieldCoOp.text) && !Regex.IsMatch (InputfieldCoOp.text, "^[ \t\r\n\u0200]*$")) {
				SendChatMessage (this.InputfieldCoOp.text);
				this.InputfieldCoOp.text = "";
			} else {
				this.InputfieldCoOp.text = "";
			}
		}
	}




	//	public void OnSubmitText ()
	//	{
	//		if (string.IsNullOrEmpty (Inputfield.text)) {
	//			SendMessageButton.interactable = false;
	//		} else {
	//			SendMessageButton.interactable = true;
	//		}
	//	}

	public void GenerateMessageInCoOp (string message, string sender)
	{
		GameObject obj = null;

		if (sender == PlayerManager.Instance.playerInfo.Username)
			obj = (GameObject)Instantiate (players_message);
		else
			obj = (GameObject)Instantiate (other_message);

		obj.transform.SetParent (message_panelForCoOp, false);
		obj.transform.localScale = Vector3.one;

		// get messages from others and apply here....
		obj.transform.FindChild ("Message").GetComponent<Text> ().text = message;
		//		obj.transform.FindChild ("Image").GetComponent<Image> ().sprite = PlayerImages [UnityEngine.Random.Range (0, PlayerImages.Length)];
		obj.transform.FindChild ("UserName").GetComponent<Text> ().text = sender;
		obj.transform.FindChild ("Time Text").GetComponent<Text> ().text = DateTime.Now.ToString ("hh:mm tt");

		Invoke ("scrollValueinCoOp", 0.1f);

		if (!ScreenAndPopupCall.Instance.ChatInCoopIsShown) {
			CoOpEventController.Instance.ChatBagdeCountText.gameObject.SetActive (true);
			MessageCount++;

		} else {
			MessageCount = 0;
			CoOpEventController.Instance.ChatBagdeCountText.gameObject.SetActive (false);
		}
		CoOpEventController.Instance.ChatBagdeCountText.text = MessageCount.ToString ();
	}

	int MessageCount = 0;

	void scrollValueinCoOp ()
	{
		message_panelForCoOp.GetComponentInParent <ScrollRect> ().verticalScrollbar.value = 0;
	}

	public void GenerateMessageInFlatParty (string message, string sender)
	{

		GameObject obj = null;

		var json = JSON.Parse (message);
		string _message = json ["message"].ToString ().Trim ('\"');
		string time = json ["time"].ToString ().Trim ('\"');

		var Temptime = Convert.ToInt64 (time);
		DateTime Realtime = DateTime.FromBinary (Temptime);

		DateTime runtimeKnowsThisIsUtc = DateTime.SpecifyKind (
			                                 Realtime,
			                                 DateTimeKind.Utc);
		DateTime Localtime = runtimeKnowsThisIsUtc.ToLocalTime ();	


		if (sender == PlayerManager.Instance.playerInfo.Username) {
			if (message.Contains ("party end time remaining"))
				obj = (GameObject)Instantiate (ChatManager.Instance.SeftEventInfoMsg);
			else
				obj = (GameObject)Instantiate (ChatManager.Instance.players_message);
		} else {
			if (message.Contains ("party end time remaining"))
				obj = (GameObject)Instantiate (ChatManager.Instance.OtherEventInfoMsg);
			else
				obj = (GameObject)Instantiate (ChatManager.Instance.other_message);
		}
		obj.transform.SetParent (message_panelForFlatParty, false);
		obj.transform.localScale = Vector3.one;

		// get messages from others and apply here....
		obj.transform.FindChild ("Message").GetComponent<Text> ().text = _message;
		//		obj.transform.FindChild ("Image").GetComponent<Image> ().sprite = PlayerImages [UnityEngine.Random.Range (0, PlayerImages.Length)];
		if (message.Contains ("party end time remaining"))
			obj.transform.FindChild ("UserName").GetComponent<Text> ().text = "Information";
		else
			obj.transform.FindChild ("UserName").GetComponent<Text> ().text = sender;
		obj.transform.FindChild ("Time Text").GetComponent<Text> ().text = Localtime.ToString ("hh:mm tt");

		Invoke ("scrollValueinFlatParty", 0.1f);

		if (!ChatJoin) {
			if (!ScreenAndPopupCall.Instance.ChatInFlatPartyIsShown) {
				FlatPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (true);
				MessageCountForFlatParty++;
			} else {
				MessageCountForFlatParty = 0;
				FlatPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (false);
			}
		}
		FlatPartyScreenControle.Instance.ChatBagdeCountText.text = MessageCountForFlatParty.ToString ();
	}

	public int MessageCountForFlatParty = 0;

	void scrollValueinFlatParty ()
	{
		message_panelForFlatParty.GetComponentInParent <ScrollRect> ().verticalScrollbar.value = 0;
	}

	public void GenerateMessageInSocietyParty (string message, string sender)
	{

		GameObject obj = null;

		var json = JSON.Parse (message);
		string _message = json ["message"].ToString ().Trim ('\"');
		string time = json ["time"].ToString ().Trim ('\"');

		var Temptime = Convert.ToInt64 (time);
		DateTime Realtime = DateTime.FromBinary (Temptime);

		DateTime runtimeKnowsThisIsUtc = DateTime.SpecifyKind (
			                                 Realtime,
			                                 DateTimeKind.Utc);
		DateTime Localtime = runtimeKnowsThisIsUtc.ToLocalTime ();	


		if (sender == PlayerManager.Instance.playerInfo.Username) {
			if (message.Contains ("party end time remaining"))
				obj = (GameObject)Instantiate (ChatManager.Instance.SeftEventInfoMsg);
			else
				obj = (GameObject)Instantiate (ChatManager.Instance.players_message);
		} else {
			if (message.Contains ("party end time remaining"))
				obj = (GameObject)Instantiate (ChatManager.Instance.OtherEventInfoMsg);
			else
				obj = (GameObject)Instantiate (ChatManager.Instance.other_message);
		}
		obj.transform.SetParent (message_panelForSocietyParty, false);
		obj.transform.localScale = Vector3.one;

		// get messages from others and apply here....
		obj.transform.FindChild ("Message").GetComponent<Text> ().text = _message;
		//		obj.transform.FindChild ("Image").GetComponent<Image> ().sprite = PlayerImages [UnityEngine.Random.Range (0, PlayerImages.Length)];
		if (message.Contains ("party end time remaining"))
			obj.transform.FindChild ("UserName").GetComponent<Text> ().text = "Information";
		else
			obj.transform.FindChild ("UserName").GetComponent<Text> ().text = sender;
		obj.transform.FindChild ("Time Text").GetComponent<Text> ().text = Localtime.ToString ("hh:mm tt");

		Invoke ("scrollValueinSocietyParty", 0.1f);

		if (!ChatJoin) {
			if (!ScreenAndPopupCall.Instance.ChatInSocietyPartyIsShown) {
				SocietyPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (true);
				MessageCountForSocietyParty++;
			} else {
				MessageCountForSocietyParty = 0;
				SocietyPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (false);
			}
		}
		SocietyPartyScreenControle.Instance.ChatBagdeCountText.text = MessageCountForSocietyParty.ToString ();
	}

	public int MessageCountForSocietyParty = 0;

	void scrollValueinSocietyParty ()
	{
		message_panelForSocietyParty.GetComponentInParent <ScrollRect> ().verticalScrollbar.value = 0;
	}
	//	void Addfriends(List<FriendData> friendsData)
	//	{
	//		var Temp = new List<string> ();
	//		foreach(var friend in friendsData)
	//		{
	//			Temp.Add (friend.Username);
	//		}
	//		chatClient.AddFriends (Temp.ToArray ());
	//	}
	//
	//	bool GetIsFriendOnline(string Name)
	//	{
	////		chatClien
	//	}

	IEnumerator OnJoinChat ()
	{
		yield return new WaitForSeconds (5);
		ChatJoin = false;
	}
}