using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using Simple_JSON;
using UnityEngine.UI;

public class NotificationManager : Singleton<NotificationManager>
{
	public List<Notifications> AllNotifications = new List<Notifications> ();
	public List<Invitations> AllInvitations = new List<Invitations> ();

	public GameObject NotificationContainer;
	public GameObject InvitationPrefab;
	public GameObject NotificationPrefab;

	public int lastIvitationClicked_Id;

	const string NotificationUrl = "http://pinekix.ignivastaging.com/notifications/notification";
	const string InvitationUrl = "http://pinekix.ignivastaging.com/invitations/invitation";

	void Awake ()
	{
		Reload ();
	}


	public void CreateNotifications (List<Notifications> _List)
	{		
		DeleteGameObjects ();
		ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("NotificationsButton").GetComponent <Button> ().interactable = false;
		ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("ClearAll").gameObject.SetActive (true);
		ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("InvitationsButton").GetComponent <Button> ().interactable = true;

		if (_List.Count == 0) {
			ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = "No Notifications to show";
			ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("ClearAll").gameObject.SetActive (false);
		} else {
			ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = "";
			ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("ClearAll").gameObject.SetActive (true);
		}
		_List.ForEach (notif => {
			GameObject Go = GameObject.Instantiate (NotificationPrefab, NotificationContainer.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.AddComponent <NotificationsUi> ().Data = notif;
		});
	}

	void DeleteGameObjects ()
	{
		for (int i = 0; i < NotificationContainer.transform.childCount; i++) {
			GameObject.Destroy (NotificationContainer.transform.GetChild (i).gameObject);
		}
	}

	public void CreateInvitations (List<Invitations> _List)
	{
		DeleteGameObjects ();
		ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("NotificationsButton").GetComponent <Button> ().interactable = true;
		ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("ClearAll").gameObject.SetActive (false);

		ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("InvitationsButton").GetComponent <Button> ().interactable = false;

//		var _List = GetInvitations ();
		if (_List.Count == 0)
			ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = "No Invitations to show";
		else
			ScreenManager.Instance.Notifications.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = "";

		_List.ForEach (invite => {
			GameObject Go = GameObject.Instantiate (InvitationPrefab, NotificationContainer.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.AddComponent <InvitationsUi> ().Data = invite;
		});
	}

	public void SendInvitationToUser (int playerId, string message, int EventOrSocietyId)
	{
		StartCoroutine (ISendInvitationToUser (playerId, message, EventOrSocietyId, true, null));
	}

	public void SendInvitationToUser (int playerId, string message, int EventOrSocietyId, string RoomName)
	{
		StartCoroutine (ISendInvitationToUser (playerId, message, EventOrSocietyId, true, RoomName));
	}

	public void SendNotificationToUser (int playerId, string message)
	{
		StartCoroutine (ISendNotificationtoUser (playerId, message));
	}

	public IEnumerator ISendInvitationToUser (int Id, string message, int EventOrSocietyId, bool shoudShowPopUp, string RoomName = null)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "save";
		jsonElement ["sender_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["reciever_player_id"] = Id.ToString ();
		jsonElement ["message"] = message;
		var jsonarray = new JSONArray ();

		if (RoomName == null) {
			var jsonItem = new JSONClass ();
			jsonItem ["item_id"] = EventOrSocietyId.ToString ();
			jsonItem ["item_type"] = "Society";
			jsonarray.Add (jsonItem);
		} else {	
			var jsonItem = new JSONClass ();

			jsonItem ["item_id"] = EventOrSocietyId.ToString ();
			jsonItem ["item_type"] = "CoOp";

			var jsonItem2 = new JSONClass ();
			jsonItem2 ["item_id"] = EventOrSocietyId.ToString ();
			jsonItem2 ["item_type"] = RoomName;
			jsonarray.Add (jsonItem);
			jsonarray.Add (jsonItem2);
		}

		jsonElement ["items"] = jsonarray;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (InvitationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {

//				print ("Success");

				if (RoomName == null) {
					if (shoudShowPopUp)
						ShowPopUp ("Invitation sent successfully", () => ScreenManager.Instance.Admin_MemberDiscriptionPanel.GetComponent <SocietyDescriptionController> ().SocietyMemberList (true));               
					IndicationManager.Instance.SendIndicationToUsers (new int[] { Id }, "Invitation");
                    
				}
				yield return true;
			} else
				yield return false;
		} else
			yield return false;
	}

	
	public IEnumerator ISendNotificationtoUser (int  playerId, string message)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "save";
		jsonElement ["sender_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["reciever_player_id"] = playerId.ToString ();
		jsonElement ["message"] = message;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (NotificationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {

//				print ("Success");
				IndicationManager.Instance.SendIndicationToUsers (new int[] { playerId }, "Notification");
				yield return true;
			}
		}		
	}

	public void GetInvitations ()
	{
		StartCoroutine (IGetInvitations ());
		IndicationManager.Instance.IncrementIndicationFor ("Invitation", 4);
	}

	public void GetNotifications ()
	{
		StartCoroutine (IGetNotifications ());
	}

	IEnumerator IGetInvitations ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "view";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (InvitationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());

			List<Invitations> Invitations = new List<Invitations> ();
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Invitations are following")) {
				var data = _jsnode ["data"];

				for (int i = 0; i < data.Count; i++) {

					int invitation_id = 0;
					int.TryParse (data [i] ["invitation_id"], out invitation_id);

					int friendId = 0;
					int.TryParse (data [i] ["sender_id"], out friendId);
					string message = data [i] ["message"];

					string Title = "";
					string RoomName = "";
					var items = data [i] ["items"];
			
					int eventId = 0;
					string Type = items [0] ["item_type"];

					// society
					if (Type.Contains ("Society")) {
						Title = "Society";
						int.TryParse (items [0] ["item_id"], out eventId);
					}  // Coop
					if (Type.Contains ("CoOp")) {
						Title = "CoOp Event";
						int.TryParse (items [0] ["item_id"], out eventId);
						RoomName = items [1] ["item_type"];
					}

					Invitations Invite = new Invitations (invitation_id, eventId, friendId, message, Title, RoomName);
					Invitations.Add (Invite);
				}
			} else if (_jsnode ["status"].ToString ().Contains ("400")) {
				
			}

			CreateInvitations (Invitations);
		}
	}


	IEnumerator IGetNotifications ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "view";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		WWW www = new WWW (NotificationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());

			List<Notifications> Notifications = new List<Notifications> ();
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Invitations are following")) {
				var data = _jsnode ["data"];

//				{"status":"200","description":"Messages are following.","data":[{"sender_id":"119","reciever_id":"111","message":"1asdasdasdsad"}]}

				for (int i = 0; i < data.Count; i++) {
					int friendId = 0;
					int.TryParse (data [i] ["sender_id"], out friendId);
					string message = data [i] ["message"];
					string Title = "";

					if (message.Contains ("society"))
						Title = "Society";
					else
						Title = "Notification";
		
					Notifications Notif = new Notifications (message, Title);
					Notifications.Add (Notif);
				}
			} else if (_jsnode ["status"].ToString ().Contains ("400")) {

			}

			CreateNotifications (Notifications);
		}
	}

	public void ClearAllNotifications ()
	{
		StartCoroutine (IDeleteNotification ());
	}

	IEnumerator IDeleteNotification ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "delete";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
	

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (NotificationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {

//				print ("Success");
				CreateNotifications (new List<Notifications> ());
				yield return true;
			}
		}
	}

	public void DeleteInvitation (int invitation_id, GameObject Go)
	{
		StartCoroutine (IDeleteInvitations (invitation_id, Go));
	}

	public IEnumerator IDeleteInvitations (int invitation_id, GameObject Go)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "delete";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["invitation_id"] = invitation_id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (InvitationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				if (Go)
					Destroy (Go);
//				print ("Success");
				yield return true;
			} else
				yield return false;
		}	
	}


	public void ShowPopUp (string message, UnityEngine.Events.UnityAction OnClickActions)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickActions != null)
				OnClickActions ();
		});
	}

	//	void DeleteNotification(int id)
	//	{
	//		JSONClass arrayJson = new JSONClass ();
	//		var notifications = GetNotifications ();
	//		notifications.ForEach (notify =>
	//			{
	//				if(notify.EventId != id)
	//				{
	//				var JsonElement = new JSONClass ();
	//				JsonElement ["Id"] = notify.EventId.ToString ();
	//				JsonElement ["Message"] = notify.Message.ToString ();
	//				arrayJson.Add (JsonElement);
	//				}
	//		});
	//		CreateOrWriteFile (fileNameforNotifications, arrayJson.ToString ());
	//	}
	//
	//	void DeleteNotification(int id, string friendName)
	//	{
	//		JSONClass arrayJson = new JSONClass ();
	//		var invitations = GetInvitations ();
	//		invitations.ForEach (notify => {
	//			if(notify.EventId != id && notify.FriendName != friendName)
	//			{
	//				var JsonElement = new JSONClass ();
	//				JsonElement ["Id"] = notify.EventId.ToString ();
	//				JsonElement ["Message"] = notify.Message;
	//				JsonElement ["RoomName"] = notify.RoomName;
	//				JsonElement ["FriendName"] = notify.FriendName;
	//				arrayJson.Add (JsonElement);
	//			}
	//		});
	//		CreateOrWriteFile (fileNameforInvitations, arrayJson.ToString ());
	//	}
	//
	//
	//	void AddNotification(Notifications notification)
	//	{
	//		JSONClass arrayJson = new JSONClass ();
	//		var notifications = GetNotifications ();
	//		notifications.ForEach (notify => {
	//			var JsonElement = new JSONClass ();
	//			JsonElement ["Id"] = notify.EventId.ToString ();
	//			JsonElement ["UserName"] = notify.UserName;
	//			JsonElement ["Message"] = notify.Message.ToString ();
	//			arrayJson.Add (JsonElement);
	//		});
	//		var newJson = new JSONClass ();
	//		newJson ["Id"] = notification.EventId.ToString ();
	//		newJson ["UserName"] =notification.UserName;
	//		newJson ["Message"] = notification.Message.ToString ();
	//		arrayJson.Add (newJson);
	//		CreateOrWriteFile (fileNameforNotifications, arrayJson.ToString ());
	//	}
	//
	//	void AddNotification(Invitations invitation)
	//	{
	//		JSONClass arrayJson = new JSONClass ();
	//		var invitations = GetInvitations ();
	//		invitations.ForEach (notify => {
	//			var JsonElement = new JSONClass ();
	//			JsonElement ["Id"] = notify.EventId.ToString ();
	//			JsonElement ["Message"] = notify.Message;
	//			JsonElement ["RoomName"] = notify.RoomName;
	//			JsonElement ["FriendName"] = notify.FriendName;
	//			arrayJson.Add (JsonElement);
	//		});
	//		var newJson = new JSONClass ();
	//		newJson ["Id"] = invitation.EventId.ToString ();
	//		newJson ["Message"] = invitation.Message;
	//		newJson ["RoomName"] = invitation.RoomName;
	//		newJson ["FriendName"] = invitation.FriendName;
	//		arrayJson.Add (newJson);
	//		CreateOrWriteFile (fileNameforInvitations, arrayJson.ToString ());
	//	}
	//
	//	List <Notifications> GetNotifications()
	//	{
	//		var Notifications = new List <Notifications> ();
	//		string JsonString = ReadFile (fileNameforNotifications);
	//		if (JsonString != "") {
	//			var Json = JSON.Parse (JsonString);
	//			var count = Json.Count;
	//
	//			for (int i = 0; i < count; i++) {
	//				int id = int.Parse (Json [i] ["Id"].ToString ().Trim ("\"".ToCharArray ()));
	//				string message = Json [i] ["Message"].ToString ().Trim ("\"".ToCharArray ());
	//				string username = Json [i] ["UserName"].ToString ().Trim ("\"".ToCharArray ());
	//
	//				Notifications notif = new Notifications (id, message,username);
	//				Notifications.Add (notif);
	//			}
	//		}
	//		return Notifications;
	//	}
	//
	//	List<Invitations> GetInvitations()
	//	{
	//		var Invitations = new List <Invitations> ();
	//		string JsonString = ReadFile (fileNameforInvitations);
	//		if (JsonString != "") {
	//			var Json = JSON.Parse (JsonString);
	//			var count = Json.Count;
	//
	//			for (int i = 0; i < count; i++) {
	//				int id = int.Parse (Json [i] ["Id"].ToString ().Trim ("\"".ToCharArray ()));
	//				string message = Json [i] ["Message"].ToString ().Trim ("\"".ToCharArray ());
	//				string frndName = Json [i] ["FriendName"].ToString ().Trim ("\"".ToCharArray ());
	//				string roomName = Json [i] ["RoomName"].ToString ().Trim ("\"".ToCharArray ());
	//				Invitations invite = new Invitations (id, message,frndName,roomName);
	//				Invitations.Add (invite);
	//			}
	//		}
	//		return Invitations;
	//	}
	//
	//	void CreateOrWriteFile(string fileName , string data)
	//	{
	//
	//		if (File.Exists(fileName))
	//		{
	//			var filestream = File.Open(fileName, FileMode.Truncate);
	//			formatter.Serialize (filestream,data);
	//			filestream.Close ();
	//		}else
	//		{
	//			var filestream = File.Open(fileName, FileMode.Create);
	//			formatter.Serialize (filestream,data);
	//			filestream.Close ();
	//		}
	//	}
	//
	//	string ReadFile(string file){
	//
	//		if (File.Exists (file)) {
	//			var filestream = File.Open (file, FileMode.Open);
	//			string Data ="";
	//			if(filestream.Length != 0)
	//				Data = (string)formatter.Deserialize (filestream);
	//			filestream.Close ();
	//			return Data;
	//			//			Debug.LogError (string.Format ("DestroyTime - {0}",text));// Id-  {1} ,Type - {2}",text, text, text));
	//		} else
	//			return string.Empty;
	//	}

	[Serializable]
	public class Notifications
	{
		public Notifications (string message, string title)
		{
//			EventId = id;
			Message = message;
			Title = title;
		}
		//		public int EventId;
		public string Title;
		public string Message;
	}

	[Serializable]
	public class Invitations
	{

		public Invitations (int id, int eventid, int senderId, string message, string title, string roomName = null)
		{
			Id = id;
			EventId = eventid;
			SenderId = senderId;
			Message = message;
			Title = title;
			RoomName = roomName;
		}

		public int Id;
		public int EventId;
		public int SenderId;
		public string Message;
		public string Title;
		public string RoomName;
	}
}

