using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Simple_JSON;
using System;

public class FriendProfileManager : Singleton<FriendProfileManager>
{
	/// <summary>
	/// The name of the user.script is created by Rehan on  25/05/2017
	/// </summary>

	// Webservice variable 
	public const string ReportLink = "http://pinekix.ignivastaging.com/playerProfiles/userReport";
	public const string BlockPlayerLink = "http://pinekix.ignivastaging.com/playerProfiles/blockUserProfile";
	public const string UnBlockPlayerLink = "http://pinekix.ignivastaging.com/playerProfiles/unblockPlayerProfile";
	public const string GetMyBlockUserProfile = "http://pinekix.ignivastaging.com/playerProfiles/getMyBlockUserProfile";
	public const string GetBlockUserProfileOther = "http://pinekix.ignivastaging.com/playerProfiles/getBlockUserProfile";

	//Globle Variable
	public Text UserName;
	public Text GameLevel;
	public Text PlayerStatus;
	public Text CurruntSociety;
	public Text VIPSubsctiption;
	public Image MedalIcon;
	public string PlayerGender;

	public GameObject AddFriendButton;
	public GameObject BlockButton;
	public GameObject PendingRequest;
	public GameObject ReportButton;
	public GameObject UnblockButton;
	public GameObject Accept;
	public GameObject Decline;
	public GameObject BlockListContainer;
	public GameObject BlockUserPrefeb;

	[Header ("User Back Button")]
	public GameObject BackToFriendList;
	public GameObject BackToPublicArea;
	public GameObject BackToFlatParty;
	public GameObject BackToSocietyParty;
	public GameObject BackToMyProfile;
	public bool ShowBlockList;

	[Header ("User Blocked List")]
	public List<BlockeList> MyBlockList = new List<BlockeList> ();
	public List<BlockeList> BlockedByList = new List<BlockeList> ();
	public FriendData seletedPlayerData;

	void Awake ()
	{
		this.Reload ();
	}

	public void AcitveNormalPackButton ()
	{
		FriendProfileManager.Instance.BackToFriendList.SetActive (true);
		FriendProfileManager.Instance.BackToPublicArea.SetActive (false);
		FriendProfileManager.Instance.BackToFlatParty.SetActive (false);
		FriendProfileManager.Instance.BackToSocietyParty.SetActive (false);
		FriendProfileManager.Instance.BackToMyProfile.SetActive (false);
	}

	public void ActiveBackButtonForUserProfile ()
	{
		FriendProfileManager.Instance.BackToFriendList.SetActive (false);
		FriendProfileManager.Instance.BackToPublicArea.SetActive (false);
		FriendProfileManager.Instance.BackToFlatParty.SetActive (false);
		FriendProfileManager.Instance.BackToSocietyParty.SetActive (false);
		FriendProfileManager.Instance.BackToMyProfile.SetActive (true);
	}

	public void ShowMyProfileWithBlockList ()
	{
		ShowUserProfile.Instance.ShowMyProfile ();
		ScreenAndPopupCall.Instance.ShowProfileScreen ();
		GetMyProfileBlockedUserList (true);
		AcitveNormalPackButton ();
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
	}

	public void BlockThisPlayer ()
	{
		/// Check the condition of player id weather this player id is your friend or not 
//		FriendsManager.Instance.GetFriendListForProfile (); // Retriv friend List
		print (FriendsManager.Instance.AllAddedFriends.Count.ToString ());
		for (int i = 0; i < FriendsManager.Instance.AllAddedFriends.Count; i++) {
			if (FriendsManager.Instance.AllAddedFriends [i].Id == seletedPlayerData.Id) {
				if (FriendsManager.Instance.AllAddedFriends [i].Type == FriendData.FriendType.Friend) {
					string msg = "This player is your friend and if you block this player then he/she will be removed from your friends list. Still you want to block this player? ";
					ShowPopUpForBlock (msg, seletedPlayerData.Id);
					return;
				}
			}
		}
		string msg1 = "Are you sure you want to block this player?";
		ShowPopUpForBlock (msg1, seletedPlayerData.Id);
	}

	public void ShowPopUpForBlock (string message, int blockePlayeId)
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
			StartCoroutine (BlockPlayer (blockePlayeId));
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
	}

	public void OnClickOnPendingButton ()
	{
		ShowPopUpErrorMsg ("Your friend request pending. Please wait for him/her responce.");
	}

	public void ShowPopUpErrorMsg (string message)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();		
		});
	}

	public IEnumerator BlockPlayer (int BlockplayerId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["blocked_by_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["blocked_to_player_id"] = BlockplayerId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (BlockPlayerLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Player has blocked successfully.")) {
				ScreenManager.Instance.ClosePopup ();
				BlockButton.SetActive (false);
				UnblockButton.SetActive (true);
				AddFriendButton.SetActive (false);
				ShowPopUpErrorMsg ("Player has blocked successfully.");
			} else if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("You have already block this player.")) {
				ScreenManager.Instance.ClosePopup ();
				BlockButton.SetActive (false);
				UnblockButton.SetActive (true);
				AddFriendButton.SetActive (false);
				ShowPopUpErrorMsg ("You have already block this player.");
			} else {
				ScreenManager.Instance.ClosePopup ();
				ShowPopUpErrorMsg ("Somthing went wrong!!!");
			}
		}

	}

	public void UnBlockThisPlayer ()
	{
		ShowPopUpForUnBlock ("Are you sure you want to unblock this Player?", seletedPlayerData.Id);
	}

	public void ShowPopUpForUnBlock (string message, int unblockePlayeId)
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
			StartCoroutine (UnBlockPlayer (unblockePlayeId));
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
	}


	public IEnumerator UnBlockPlayer (int UnBlockplayerId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["main_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["unblocked_player_id"] = UnBlockplayerId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (UnBlockPlayerLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Player has unblocked successfully")) {
				ScreenManager.Instance.ClosePopup ();
				BlockButton.SetActive (true);
				UnblockButton.SetActive (false);
				AddFriendButton.SetActive (true);
				ShowPopUpErrorMsg ("Player has unblocked successfully");
			} else if (_jsnode ["status"].ToString ().Contains ("400") && _jsnode ["description"].ToString ().Contains ("Invalid unblocked player id.")) {
				ScreenManager.Instance.ClosePopup ();
				ShowPopUpErrorMsg ("Invalid unblocked player id.");
			}
		}

	}

	public void BackFormMultiplayerArea ()
	{
		if (MultiplayerManager.Instance._flatParty)
			ScreenManager.Instance.FlatPartyChatScreen.SetActive (true);
		else if (MultiplayerManager.Instance._societyParty)
			ScreenManager.Instance.SocietyPartyChatScreen.SetActive (true);
			
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
	}

	public void OnClickReportButtonOnProfile ()
	{		
		ScreenAndPopupCall.Instance.RportThisProfilePupUp ();
	}

	public void ReportThisPlayer (InputField msg)
	{		
		//Send Reported Player Id
		if (msg.text.ToString () == "") {
			ShowPopUpErrorMsg ("Please enter some message to report this player");
		} else {
			ScreenManager.Instance.ProfileReportScreen.transform.GetChild (0).GetChild (4).GetComponent<Button> ().interactable = false;
			StartCoroutine (ReportThisPlayer (seletedPlayerData.Id, msg.text.ToString ()));
		}
	}

	public IEnumerator ReportThisPlayer (int playerId, string reportMsg)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["reported_player_id"] = playerId.ToString ();
		jsonElement ["message"] = reportMsg;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (ReportLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Your report has saved successfully")) {

				ScreenManager.Instance.ClosePopup ();
				ShowPopUpErrorMsg ("Your have reported successfully to " + seletedPlayerData.Username + ".");

			} else if (_jsnode ["status"].ToString ().Contains ("400") && _jsnode ["description"].ToString ().Contains ("You have already reported this player."))
				ShowPopUpErrorMsg ("You have reported already to " + seletedPlayerData.Username + ".");
			else {
				ShowPopUpErrorMsg ("Somthing went wrong!!! Please try again.");
			}
		}

	}

	public IEnumerator RetutnWihtYeild ()
	{
		yield return StartCoroutine (MyBlockUserList (false));
		yield return StartCoroutine (IeGetOtherBlockUserProfile ());
	}

	public void DeleteContainerGameObject (GameObject Container)
	{
		for (int i = 0; i < Container.transform.childCount; i++) {
			Destroy (Container.transform.GetChild (i).gameObject);
		}
	}

	public void GetMyProfileBlockedUserList (bool show)
	{
		MyBlockList.Clear ();
		DeleteContainerGameObject (BlockListContainer);
		StartCoroutine (MyBlockUserList (show));
	}

	public IEnumerator MyBlockUserList (bool show)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (GetMyBlockUserProfile, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Blocked players list has followed.")) {
				/// Fatch Blocke List
				var JsonData = _jsnode ["data"];
				BlockeList temList = null;
				for (int i = 0; i < JsonData.Count; i++) {
					GameObject Go = GameObject.Instantiate (BlockUserPrefeb, Vector3.zero, Quaternion.identity) as GameObject;
					int playerId = 0;
					int.TryParse (JsonData [i] ["id"], out playerId);
					Go.GetComponent<AddFriendUi> ().thisData.Id = playerId;
					Go.GetComponent<AddFriendUi> ().thisData.Username = JsonData [i] ["name"];
					Go.GetComponent<AddFriendUi> ().thisData.Type = FriendData.FriendType.Blocked;
					//ToDo:
					if (show) {
						ShowBlockList = true;
						Go.GetComponent<AddFriendUi> ().AddButton.onClick.RemoveAllListeners ();
						Go.GetComponent<AddFriendUi> ().AddButton.onClick.AddListener (() => FriendProfileManager.Instance.ActiveBackButtonForUserProfile ());
					} else
						ShowBlockList = false;
					Go.transform.parent = BlockListContainer.transform;
					// add into block List
					temList = new BlockeList (playerId, JsonData [i] ["name"].ToString ().Trim ('\"'));
					MyBlockList.Add (temList);
				}
				if (show)
					ScreenAndPopupCall.Instance.MyProfileBlockListPopup ();
				if (BlockListContainer.transform.childCount == 0)
					ScreenManager.Instance.MyProfileBlockList.transform.GetChild (1).transform.GetChild (3).gameObject.SetActive (true);
				else
					ScreenManager.Instance.MyProfileBlockList.transform.GetChild (1).transform.GetChild (3).gameObject.SetActive (false);
				BlockListContainer.transform.parent.parent.parent.GetChild (2).GetComponent<Text> ().text = "BLOCKED LIST";
			} else if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("No data has found.")) {
				if (show)
					ShowPopUpErrorMsg (" You have not blocked anyone yet.");
			}
				
		}

	}

	public void ShowBlockListBool (bool status)
	{
		ShowBlockList = status;
	}

	public void GetOtherBlockUserProfile ()
	{
		BlockedByList.Clear ();
		StartCoroutine (IeGetOtherBlockUserProfile ());
	}

	public IEnumerator IeGetOtherBlockUserProfile ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (GetBlockUserProfileOther, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Blocked players list has followed")) {
				/// Fatch Blocke List
				var JsonData = _jsnode ["data"];
				BlockeList temList = null;
				for (int i = 0; i < JsonData.Count; i++) {
//					GameObject Go = GameObject.Instantiate (BlockUseePrefeb, Vector3.zero, Quaternion.identity) as GameObject;
					int playerId = 0;
					int.TryParse (JsonData [i] ["id"], out playerId);
//					Go.GetComponent<AddFriendUi> ().thisData.Id = playerId;
//					Go.GetComponent<AddFriendUi> ().thisData.Username = JsonData [i] ["name"];
//					Go.GetComponent<AddFriendUi> ().thisData.Type = FriendData.FriendType.BlockedBy;
//					Go.transform.parent = BlockListContainer.transform;
//
					temList = new BlockeList (playerId, JsonData [i] ["name"].ToString ().Trim ('\"'));
					BlockedByList.Add (temList);

				}
//				ScreenAndPopupCall.Instance.MyProfileBlockListPopup ();
//				if (BlockListContainer.transform.childCount == 0)
//					ScreenManager.Instance.MyProfileBlockList.transform.GetChild (1).transform.GetChild (3).gameObject.SetActive (true);
//				else
//					ScreenManager.Instance.MyProfileBlockList.transform.GetChild (1).transform.GetChild (3).gameObject.SetActive (false);
//				BlockListContainer.transform.parent.parent.parent.GetChild (2).GetComponent<Text> ().text = "BLOCKED BY USER LIST";

			} 
		}
	}

	public void AddThisFriend ()
	{
		FriendsManager.Instance.SendFreindRequestToUser (null, seletedPlayerData.Username, seletedPlayerData.Id);
	}

	public void AcceptThisFriendRequest ()
	{
		FriendsManager.Instance.AllAddedFriends.Add (seletedPlayerData);
		FriendsManager.Instance.AcceptRequest (seletedPlayerData.Username, FriendProfileManager.Instance.seletedPlayerData.Id, null);
	}

	public void DeclineThisFriendRequest ()
	{
		FriendsManager.Instance.RejectRequest (seletedPlayerData.Username, FriendProfileManager.Instance.seletedPlayerData.Id, null);
	}





	/// <summary>
	/// Blocke list. is created for constructor of blokList in the FriendProfileManager 
	/// </summary>
	[Serializable]
	public class BlockeList
	{
		public int PlayerId;
		public string UserName;

		public BlockeList (int pId, string pName)
		{
			PlayerId = pId;
			UserName = pName;
		}
	}
}
