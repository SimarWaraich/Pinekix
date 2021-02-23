using UnityEngine;
using System.Collections;
using Simple_JSON;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class FriendsManager : Singleton<FriendsManager>
{
	const string FriendActionUrl = "http://pinekix.ignivastaging.com/friends/friendAction";

    public GameObject FriendUiPrefab;
    public GameObject FriendPrefabforPublicArea;
	public GameObject AddFriendContainer;
	public GameObject FriendsPanel;
	public GameObject CoOpFriendsPanel;
	//	public GameObject CoOpFriendsObject1;
	//	public GameObject CoOpFriendsObject2;

	public Button FreindsButton;
	public Button RequestsButton;
	//	public InputField SearchBar;
	public bool RequestButton;

	public List<FriendData> AllAddedFriends = new List<FriendData> ();
	public List<FriendData> NewRequests = new List<FriendData> ();
	public List<FriendData> PendingRequests = new List<FriendData> ();

	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
		GetAllFriendData (false);
	}

	public void SearchFriend (InputField SearchBar)
	{
		for (int i = 0; i < FriendsPanel.transform.childCount; i++) {
			if (FriendsPanel.transform.GetChild (i).name.ToLower ().Contains (SearchBar.text.ToLower ())) {
				FriendsPanel.transform.GetChild (i).gameObject.SetActive (true);
			} else if (string.IsNullOrEmpty (SearchBar.text.ToString ())) {
				FriendsPanel.transform.GetChild (i).gameObject.SetActive (true);
			} else {
				FriendsPanel.transform.GetChild (i).gameObject.SetActive (false);
			}
		}
	}

	public void SearchFriendInCoOp (InputField SearchBar)
	{
		
		var container = SearchBar.transform.parent.GetComponentInChildren <GridLayoutGroup> ().transform; //ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ().transform;

		for (int i = 0; i < container.childCount; i++) {
			if (container.GetChild (i).name.ToLower ().Contains (SearchBar.text.ToLower ())) {
				container.GetChild (i).gameObject.SetActive (true);
			} else if (string.IsNullOrEmpty (SearchBar.text.ToString ())) {
				container.GetChild (i).gameObject.SetActive (true);
			} else {
				container.GetChild (i).gameObject.SetActive (false);
			}
		}
	}

	public void GetAllFriendData (bool openScreen)
	{
		StartCoroutine (GetAllFriendsList (openScreen));
		StartCoroutine (GetAllFriendRequests (openScreen));     
   

		//TODO show Badges...

	}

	public void GetFriendListForProfile ()
	{
		StartCoroutine (GetAllFriendsList (false));
	}

	/// <summary>
	/// Creates all friends for user interface. For My Friends list in Contacts
	/// </summary>
	public void CreateAllFriendsForUi ()
	{
		// Delete old childs if any...
		for (int i = 0; i < FriendsPanel.transform.childCount; i++) {
			Destroy (FriendsPanel.transform.GetChild (i).gameObject);
		}
			
		AllAddedFriends.Sort ();
		foreach (var frnd in AllAddedFriends) {
			GameObject Go = Instantiate (FriendUiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			Go.transform.parent = FriendsPanel.transform;
			Go.transform.localScale = Vector3.one;
			Go.transform.localPosition = Vector3.zero;

			FriendData friendData = new FriendData ();				
			friendData.Id = frnd.Id;
			friendData.Username = frnd.Username;
			friendData.Status = frnd.Status;
			friendData.Type = FriendData.FriendType.Friend;
			Go.GetComponent <AddFriendUi> ().thisData = friendData;
			Go.name = frnd.Username;
		}	
		FreindsButton.interactable = false;
		RequestsButton.interactable = true;
	
		if (AllAddedFriends.Count == 0)
			ScreenManager.Instance.FriendsScreen.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = "No friends to show!";
		else
			ScreenManager.Instance.FriendsScreen.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = "";

		ScreenManager.Instance.FriendsScreen.transform.GetChild (0).GetComponentInChildren <InputField> ().text = "";
	}

    /// <summary>
    /// Creates all friends for join. For All My Friends list in Scociety to add as a society member
    /// </summary>
	public void CreateAllFriendsForJoin ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
		ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";

		var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();

		for (int i = 0; i < container.transform.childCount; i++) {
			Destroy (container.transform.GetChild (i).gameObject);
		}

		if (AllAddedFriends.Count == 0)
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"No Friends to show";
		else
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";


        ScreenManager.Instance.FriendsInvitePopUp.transform.GetChild (1).GetChild (1).GetComponent <Button> ().onClick.RemoveListener (() => SocietyDescriptionController.Instance.SocietyMemberList (true));
        ScreenManager.Instance.FriendsInvitePopUp.transform.GetChild (1).GetChild (1).GetComponent <Button> ().onClick.AddListener (() => SocietyDescriptionController.Instance.SocietyMemberList (true));
		


		AllAddedFriends.Sort ();
		AllAddedFriends.ForEach (friend => {
			GameObject Go = Instantiate (FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
			//										ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsScreen);
			Go.transform.parent = container.transform;
			Go.transform.localScale = Vector3.one;
			Go.transform.localPosition = Vector3.zero;
			Go.GetComponent <AddFriendUi> ().thisData = friend;
			if (IsAlreadyMember (friend.Id))
				Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.Friend;
			else
				Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.SocietyJoinCall;
			Go.name = friend.Username;	
		});
	}

	//	FriendData FindFriendWithName(string Name, List<FriendData> ListToFindFrom)
	//	{
	//		foreach(var friend in ListToFindFrom)
	//		{
	//			if (friend.Username == Name)
	//				return friend;
	//		}
	//		return null;
	//	}
	//
	//	void CreateFriendGameObjectForJoin(FriendData friend, GridLayoutGroup container)
	//	{
	//		GameObject Go = Instantiate (FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
	//		//										ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsScreen);
	//		Go.transform.parent = container.transform;
	//		Go.transform.localScale = Vector3.one;
	//		Go.transform.localPosition = Vector3.zero;
	//		Go.GetComponent <AddFriendUi> ().thisData = friend;
	//		if(IsAlreadyMember(friend.Id))
	//			Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.Friend;
	//		else
	//			Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.SocietyJoinCall;
	//		Go.name = friend.Username;
	//	}

	bool IsAlreadyMember (int id)
	{
		foreach (var membr in SocietyManager.Instance._allMembers) {
			if (membr.player_id == id)
				return true;
		}
		return false;
	}


	/// <summary>
    /// Creates all request for user interface. For New Requests i have received in Contacts
	/// </summary>
	public void CreateAllRequestForUi ()
	{
		// Delete old childs if any...
		for (int i = 0; i < FriendsPanel.transform.childCount; i++) {
			Destroy (FriendsPanel.transform.GetChild (i).gameObject);
		}
		NewRequests.Sort ();
		foreach (var frnd in NewRequests) {
			GameObject Go = Instantiate (FriendUiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			Go.transform.parent = FriendsPanel.transform;
			Go.transform.localScale = Vector3.one;
			Go.transform.localPosition = Vector3.zero;


			FriendData friendData = new FriendData ();				
			friendData.Id = frnd.Id;
			friendData.Username = frnd.Username;
			friendData.Message = frnd.Message;
			friendData.Status = frnd.Status;
			friendData.Type = FriendData.FriendType.Request;
			Go.GetComponent <AddFriendUi> ().thisData = friendData;
			Go.name = frnd.Username;
		}

		FreindsButton.interactable = true;
		RequestsButton.interactable = false;
		if (NewRequests.Count == 0)
			ScreenManager.Instance.FriendsScreen.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = "No new request received!";
		else
			ScreenManager.Instance.FriendsScreen.transform.GetChild (0).FindChild ("NotFoundText").GetComponent <Text> ().text = " ";
	}

	/// <summary>
	/// Shows List of all player to be added as a freinds.
	/// </summary>
	public bool ShowFriendInPublicArea;

	public void ShowAllPlayerToAddAsFreinds ()
	{
		ShowFriendInPublicArea = true;
		ShowUserProfile.Instance.WaitingScreenState (true);
		FriendProfileManager.Instance.GetOtherBlockUserProfile ();
		FriendProfileManager.Instance.GetMyProfileBlockedUserList (false);
		StartCoroutine (GetPendingRequest ());
	}

	IEnumerator GetPendingRequest ()
	{
		PendingRequests.Clear ();

		yield return StartCoroutine (GetAllFriendsList (false));
		yield return StartCoroutine (GetAllFriendRequests (false));

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["sender_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["data_type"] = "get_send_friend_request";

		//		{  
		//			"sender_player_id":206,  
		//			"data_type": "get_send_friend_request"  
		//		}
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (FriendActionUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("success") || _jsnode ["status"].ToString ().Contains ("200")) { 
				JSONNode data = _jsnode ["data"];
				//				{"status":"200",
				//				"description":"success",
				//				"data":[{"sender_player_id":"206","message":"friend Request","receiver_player_id":"100","status":"1"},
				//				{"sender_player_id":"206","message":"friend Request","receiver_player_id":"110","status":"0"}
				//				]
				//			}
				for (int i = 0; i < data.Count; i++) {
					FriendData friend = new FriendData ();
					var senderid = int.Parse (data [i] ["sender_player_id"]);
					var receiverid = int.Parse (data [i] ["receiver_player_id"]); 

					if (senderid == PlayerPrefs.GetInt ("PlayerId")) {
						friend.Id = receiverid;
					} else {
						friend.Id = senderid;
					}
					friend.Status = int.Parse (data [i] ["status"]);
					friend.Type = FriendData.FriendType.Request;

					PendingRequests.Add (friend);
				}
			
			}
			InstaitePlayerToAdd ();
		}
	}

	public void GetPendingRequestFor_Party ()
	{
		StartCoroutine (GetPendingRequestForParty ());
	}

	IEnumerator GetPendingRequestForParty ()
	{
		PendingRequests.Clear ();

		yield return StartCoroutine (GetAllFriendsList (false));
		yield return StartCoroutine (GetAllFriendRequests (false));

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["sender_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["data_type"] = "get_send_friend_request";

		//		{  
		//			"sender_player_id":206,  
		//			"data_type": "get_send_friend_request"  
		//		}
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (FriendActionUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("success") || _jsnode ["status"].ToString ().Contains ("200")) { 
				JSONNode data = _jsnode ["data"];
				//				{"status":"200",
				//				"description":"success",
				//				"data":[{"sender_player_id":"206","message":"friend Request","receiver_player_id":"100","status":"1"},
				//				{"sender_player_id":"206","message":"friend Request","receiver_player_id":"110","status":"0"}
				//				]
				//			}
				for (int i = 0; i < data.Count; i++) {
					FriendData friend = new FriendData ();
					var senderid = int.Parse (data [i] ["sender_player_id"]);
					var receiverid = int.Parse (data [i] ["receiver_player_id"]); 

					if (senderid == PlayerPrefs.GetInt ("PlayerId")) {
						friend.Id = receiverid;
					} else {
						friend.Id = senderid;
					}
					friend.Status = int.Parse (data [i] ["status"]);
					friend.Type = FriendData.FriendType.Request;

					PendingRequests.Add (friend);
				}
			}
		}
	}

	bool isMyFriend (int Id)
	{
		foreach (var friends in AllAddedFriends) {
			if (friends.Id == Id)
				return true;
		}
		return false;
	}

	bool IsRequestPending (int Id)
	{
		foreach (var pending in PendingRequests) {
			if (pending.Id == Id)
				return true;
		}
		return false;
	}

	bool IsMyBlockList (int Id)
	{
		foreach (var blocked in FriendProfileManager.Instance.MyBlockList) {
			if (blocked.PlayerId == Id)
				return true;
		}
		return false;
	}

	bool IsBlockedByList (int Id)
	{
		foreach (var blockedBy in FriendProfileManager.Instance.BlockedByList) {
			if (blockedBy.PlayerId == Id)
				return true;
		}
		return false;
	}
    /// <summary>
    /// Create a lists of all users present in public areas, so that i can view / send a request to another player
    /// </summary>
	void InstaitePlayerToAdd ()
	{
		// Delete old childs if any...
		for (int i = 0; i < AddFriendContainer.transform.childCount; i++) {
			Destroy (AddFriendContainer.transform.GetChild (i).gameObject);
		}

		List<PlayerData> allmultiPlayersExceptme = new List<PlayerData> ();

		// to sort all multiplayers and to ensure that multiplayers does not contains playerId same as mine
        var AllPlayers = GameObject.FindObjectsOfType <PublicAreaPlayer> ();

		foreach (var player in AllPlayers) {		
			// Check if player data is valid or not as same as my player..
			if (player.PlayerData != null && player.PlayerData.player_id != PlayerManager.Instance.playerInfo.player_id && player.PlayerData.player_id != 0) {
				allmultiPlayersExceptme.Add (player.PlayerData);
			}
		}
	

		if (allmultiPlayersExceptme.Count == 0)
			ScreenManager.Instance.AddFriendScreen.transform.FindChild ("NoFoundText").GetComponent <Text> ().text = "No Players found in this public area";
		else
			ScreenManager.Instance.AddFriendScreen.transform.FindChild ("NoFoundText").GetComponent <Text> ().text = " ";

//		if (ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("AddFriendButton").GetComponent <Button> ().interactable != false)
		//			ScreenManager.Instance.AddFriendScreen.transform.FindChild ("NoFoundText").GetComponent <Text> ().text = " ";

		foreach (var player in allmultiPlayersExceptme) {
			GameObject Go = Instantiate (FriendPrefabforPublicArea, Vector3.zero, Quaternion.identity) as GameObject;
			Go.transform.parent = AddFriendContainer.transform;
			Go.transform.localScale = Vector3.one;
			Go.transform.localPosition = Vector3.zero;


			FriendData friendData = new FriendData ();				
			friendData.Id = player.player_id;
			friendData.Username = player.Username;
		
			if (IsRequestPending (player.player_id)) {
				friendData.Status = 0;
				friendData.Type = FriendData.FriendType.Pending;
			} else if (isMyFriend (player.player_id)) {
				friendData.Status = 1;
				friendData.Type = FriendData.FriendType.Friend;
			} else if (IsBlockedByList (player.player_id)) {
				friendData.Type = FriendData.FriendType.BlockedBy;
			} else if (IsMyBlockList (player.player_id)) {
				friendData.Type = FriendData.FriendType.Blocked;
			} else {
				friendData.Status = 0;
				friendData.Type = FriendData.FriendType.Unknown;
			}
			FriendProfileManager.Instance.ShowBlockList = true;
			Go.GetComponent <AddFriendUi> ().thisData = friendData;
			Go.GetComponent<AddFriendUi> ().AddButton.onClick.RemoveAllListeners ();
			Go.GetComponent<AddFriendUi> ().AddButton.onClick.AddListener (() => {
				FriendProfileManager.Instance.BackToFriendList.SetActive (false);
				FriendProfileManager.Instance.BackToPublicArea.SetActive (true);
				FriendProfileManager.Instance.BackToFlatParty.SetActive (false);
				FriendProfileManager.Instance.BackToSocietyParty.SetActive (false);
				FriendProfileManager.Instance.BackToMyProfile.SetActive (false);
			});
		}
		ShowUserProfile.Instance.WaitingScreenState (false);
	}

	public void OnClickFriendsInFriendList ()
	{
		FreindsButton.interactable = false;
		RequestsButton.interactable = true;
		StartCoroutine (GetAllFriendsList (true));
	}

	public IEnumerator GetAllFriendsList (bool openScreenAfrerSuccess)
	{
		AllAddedFriends.Clear ();
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["data_type"] = "get_friends_list";

		//		{  
		//			"receiver_player_id":100,  
		//		"data_type": "get_friends_list"  	
		//		}

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (FriendActionUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
//			{"status":"200","description":"success","data":[{"sender_player_id":"200","message":"friend request","receiver_player_id":"100","status":"1"}]}
			if (_jsnode ["description"].ToString ().Contains ("success") || _jsnode ["status"].ToString ().Contains ("200")) { 

				JSONNode data = _jsnode ["data"];

				for (int i = 0; i < data.Count; i++) {
					FriendData friend = new FriendData ();

					var senderid = int.Parse (data [i] ["sender_player_id"]);
					var receiverid = int.Parse (data [i] ["receiver_player_id"]); 

					string jsonString = data [i] ["message"].ToString ();
					var Values = jsonString.Split ('_');
					if (senderid == PlayerPrefs.GetInt ("PlayerId")) {
						friend.Id = receiverid;
						friend.Username = Values [1].ToString ().Trim ('"').Trim ('\\').Trim ('"');
					} else {
						friend.Id = senderid;
						friend.Username = Values [0].ToString ().Trim ('"').Trim ('\\').Trim ('"');
					}
					friend.Status = int.Parse (data [i] ["status"]);
					friend.Type = FriendData.FriendType.Friend;
					AllAddedFriends.Add (friend);
				}

				if (openScreenAfrerSuccess)
					CreateAllFriendsForUi ();
			} else if (_jsnode ["description"].ToString ().Contains ("No friends found") || _jsnode ["status"].ToString ().Contains ("400")) {
				if (openScreenAfrerSuccess)
					CreateAllFriendsForUi ();
			}
		}
	}

	public void OnClickRequests ()
	{
		FreindsButton.interactable = true;
		RequestsButton.interactable = false;
		StartCoroutine (GetAllFriendRequests (true));
	}

	IEnumerator GetAllFriendRequests (bool OpneScreen)
	{
		NewRequests.Clear ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["receiver_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["data_type"] = "get_recieved_friend_request";

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (FriendActionUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("success") || _jsnode ["status"].ToString ().Contains ("200")) { 
				JSONNode data = _jsnode ["data"];

				for (int i = 0; i < data.Count; i++) {
					FriendData friend = new FriendData ();
					var senderid = int.Parse (data [i] ["sender_player_id"]);
					var receiverid = int.Parse (data [i] ["receiver_player_id"]); 

					string jsonString = data [i] ["message"].ToString ();
					var Values = jsonString.Split ('_');

					if (senderid == PlayerPrefs.GetInt ("PlayerId")) {
						friend.Id = receiverid;
						friend.Username = Values [1].ToString ().Trim ('"').Trim ('\\').Trim ('\"');
					} else {
						friend.Id = senderid;
						friend.Username = Values [0].ToString ().Trim ('"').Trim ('\\').Trim ('\"');
					}
					friend.Message = Values [2].ToString ().Trim ('"').Trim ('\\').Trim ('\"');
					friend.Status = int.Parse (data [i] ["status"]);
					friend.Type = FriendData.FriendType.Request;
					NewRequests.Add (friend);
				}
			}
			if (OpneScreen) {
				CreateAllRequestForUi ();
				IndicationManager.Instance.IncrementIndicationForRequest (4);
			}
		}
	}

	/// <summary>
	/// Sends the freind request to user.
	/// </summary>
	/// <param name="FriendId">Friend identifier.</param>
	/// <param name="FriendUsername">Friend username.</param>
	public void SendFreindRequestToUser (GameObject Go, string FriendName, int FriendId)
	{
		StartCoroutine (SendFreindrequestToUser (Go, FriendName, FriendId));		
	}

	IEnumerator SendFreindrequestToUser (GameObject Go, string friendName, int FriendId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["sender_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["receiver_player_id"] = FriendId.ToString ();

		string json = PlayerPrefs.GetString ("UserName") + "_" + friendName + "_" + " Hey! Add me as your friend";
//		json ["SenderUserName"] = PlayerPrefs.GetString ("UserName");
//		json ["RealMessage"] = "Hey! Add me as your friend ";

		jsonElement ["message"] = json.ToString ();
		jsonElement ["data_type"] = "send_friend_request";

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (FriendActionUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("success") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print (_jsnode ["description"]);
				ScreenManager.Instance.ClosePopup ();
				if (Go) {
					Go.GetComponent <AddFriendUi> ().AddButton.interactable = false;
					Go.GetComponent <AddFriendUi> ().AddButton.GetComponentInChildren <Text> ().text = "Pending";
				}
				PushScript.Instance.SendPushToUser (friendName, "You have recieved a new friend request from " + PlayerPrefs.GetString ("UserName"));
				IndicationManager.Instance.SendIndicationToUsers (new int[] { FriendId }, "Request");
				FriendProfileManager.Instance.PendingRequest.SetActive (true);
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				ShowPopUp ("Friend request has been sent successfully.");
			} else if (_jsnode ["description"].ToString ().Contains ("already send") || _jsnode ["status"].ToString ().Contains ("400")) {
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.AddFriendButton.SetActive (true);
				ShowPopUp ("The player has already sent a friend request to you.");
			}
		}
	}

	//	public void SendCoOpRequestToUser (GameObject Go, string FriendName, int FriendId, string FriendUsername)
	//	{
	//		StartCoroutine (SendCoOprequestToUser (Go, FriendName, FriendId, FriendUsername));
	//	}
	//
	//	IEnumerator SendCoOprequestToUser (GameObject Go, string friendName, int FriendId, string FriendUserName)
	//	{
	//		var encoding = new System.Text.UTF8Encoding ();
	//
	//		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
	//		var jsonElement = new Simple_JSON.JSONClass ();
	//
	//		jsonElement ["sender_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
	//		jsonElement ["receiver_player_id"] = FriendId.ToString ();
	//
	//		string json = PlayerPrefs.GetString ("UserName") + "_" + friendName + "_" + " Hey! Be My Partner in the CoOp Event" + "_" + MultiplayerManager.Instance.RoomName;
	//		//		json ["SenderUserName"] = PlayerPrefs.GetString ("UserName");
	//		//		json ["RealMessage"] = "Hey! Add me as your friend ";
	//
	//		jsonElement ["message"] = json.ToString ();
	//		jsonElement ["data_type"] = "Invite_Request";
	//
	//		postHeader.Add ("Content-Type", "application/json");
	//		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
	//
	//		WWW www = new WWW (FriendActionUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);
	//
	//		print ("jsonDtat is ==>> " + jsonElement.ToString ());
	//		yield return www;
	//
	//		if (www.error == null) {
	//			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
	//			print ("www.text ==>> " + www.text);
	//			print ("_jsnode ==>> " + _jsnode.ToString ());
	//			if (_jsnode ["description"].ToString ().Contains ("success") || _jsnode ["status"].ToString ().Contains ("200")) {
	//				print (_jsnode ["description"]);
	//				ScreenManager.Instance.ClosePopup ();
	//				Go.GetComponent <AddFriendUi> ().AddButton.interactable = false;
	//				Go.GetComponent <AddFriendUi> ().AddButton.GetComponentInChildren <Text> ().text = "Pending";
	//				PushScript.Instance.SendPushToUser (FriendUserName, ", You have recieved a new Invite! ," + PlayerPrefs.GetString ("UserName") + "," +
	//				PlayerPrefs.GetInt ("PlayerId").ToString () + "," + MultiplayerManager.Instance.RoomName);
	//
	//				ShowPopUp (_jsnode ["description"]);
	//			} else if (_jsnode ["description"].ToString ().Contains ("already send") || _jsnode ["status"].ToString ().Contains ("400")) {
	//				ShowPopUp (_jsnode ["description"]);
	//			}
	//		}
	//	}



	/// <summary>
	/// Accepts the request.
	/// </summary>
	/// <param name="friendId">Friend identifier.</param>
	public void AcceptRequest (string FriendName, int friendId, GameObject Go = null)
	{
		StartCoroutine (UpdateStatusOfRequest (FriendName, friendId, 1, Go));
	}

	/// <summary>
	/// Rejects the request.
	/// </summary>
	/// <param name="friendId">Friend identifier.</param>
	public void RejectRequest (string FriendName, int friendId, GameObject Go = null)
	{
		StartCoroutine (UpdateStatusOfRequest (FriendName, friendId, 2, Go));
	}

	IEnumerator UpdateStatusOfRequest (string friendName, int FriendId, int status, GameObject Go = null)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["sender_player_id"] = FriendId.ToString ();
		jsonElement ["receiver_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["message"] = friendName + "_" + PlayerPrefs.GetString ("UserName") + "_" + " Hey! Add me as your friend ";
		jsonElement ["status"] = status.ToString ();
		jsonElement ["data_type"] = "accept_or_reject_FR";
//		{
//			"sender_player_id": 206,
//			"receiver_player_id":100,
//			"message": "Friend Request accepted"
//			"status": 1,
//			"data_type": "accept_or_reject_FR"  
//		}
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (FriendActionUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("success") || _jsnode ["status"].ToString ().Contains ("200")) {
				var temp = new List<FriendData> ();
				foreach (var rqusts in NewRequests) {
					if (rqusts.Id != FriendId) {
						temp.Add (rqusts);
					}
					NewRequests = temp;
				}	
				if (Go)
					Destroy (Go);
//				ShowPopUp (_jsnode ["description"]);
				if (status == 1) {
					FriendProfileManager.Instance.UnblockButton.SetActive (false);
					FriendProfileManager.Instance.BlockButton.SetActive (true);
					FriendProfileManager.Instance.AddFriendButton.SetActive (false);
					FriendProfileManager.Instance.Accept.SetActive (false);
					FriendProfileManager.Instance.Decline.SetActive (false);
					FriendProfileManager.Instance.PendingRequest.SetActive (false);
				} else if (status == 2) {
					FriendProfileManager.Instance.UnblockButton.SetActive (false);
					FriendProfileManager.Instance.BlockButton.SetActive (true);
					FriendProfileManager.Instance.AddFriendButton.SetActive (true);
					FriendProfileManager.Instance.Accept.SetActive (false);
					FriendProfileManager.Instance.Decline.SetActive (false);
					FriendProfileManager.Instance.PendingRequest.SetActive (false);
				}
			} 
//			else if (_jsnode ["description"].ToString ().Contains ("You have not recieved friend request from this user") || _jsnode ["status"].ToString ().Contains ("400"))
//				ShowPopUp (_jsnode ["description"]);
			ScreenManager.Instance.ClosePopup ();
			yield return StartCoroutine (GetAllFriendsList (false));
			yield return StartCoroutine (GetAllFriendRequests (false));
		}
	}

	void ShowPopUp (string message)
	{
		ScreenManager.Instance.ClosePopup ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Cancle";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());	

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	}

	#region CoOp Event

	//	public void OnClickFriendsInCoOp ()
	//	{
	//		CreateAllFriendsForCoOpUi ();
	//	}

    /// <summary>
    /// Creates list of all my friends, so that i can send an invitation to them in co op event.
    /// </summary>
	public void CreateAllFriendsForCoOpUi ()
	{
		StartCoroutine (ICreateAllFriendsForCoOpUi ());
	}

	IEnumerator ICreateAllFriendsForCoOpUi ()
	{
		yield return StartCoroutine (GetAllFriendsList (false));
		// Delete old childs if any...
		for (int i = 0; i < CoOpFriendsPanel.transform.childCount; i++) {
			Destroy (CoOpFriendsPanel.transform.GetChild (i).gameObject);
		}
		AllAddedFriends.Sort ();
		foreach (var frnd in AllAddedFriends) {
			GameObject Go = Instantiate (FriendUiPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			Go.transform.parent = CoOpFriendsPanel.transform;
			Go.transform.localScale = Vector3.one;
			Go.transform.localPosition = Vector3.zero;

			var AddFriend = Go.GetComponent <AddFriendUi> ();

			if (AllAddedFriends.FindIndex (item => item == frnd) % 2 == 0) {
				Go.GetComponent <Image> ().sprite = AddFriend.PinkBg;           
			} else {
				Go.GetComponent <Image> ().sprite = AddFriend.WhiteBg;
			}

			FriendData friendData = new FriendData ();              
			friendData.Id = frnd.Id;
			friendData.Username = frnd.Username;
			friendData.Status = frnd.Status;
			friendData.Type = FriendData.FriendType.Invite;
			AddFriend.thisData = friendData;
			Go.name = frnd.Username;
		}
		FreindsButton.interactable = false;
		RequestsButton.interactable = true;

		if (AllAddedFriends.Count == 0)
			ScreenManager.Instance.CoOpFriendList.transform.GetChild (0).FindChild ("Message").GetComponent <Text> ().text = "No friends to show!";
		else
			ScreenManager.Instance.CoOpFriendList.transform.GetChild (0).FindChild ("Message").GetComponent <Text> ().text = " ";
	}



	#endregion



}