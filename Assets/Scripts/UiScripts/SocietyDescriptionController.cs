using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Simple_JSON;
using System.Text.RegularExpressions;
using System;

public class SocietyDescriptionController : Singleton<SocietyDescriptionController>
{

	[Header ("Admin Discprition Screens")]
	public Text ScreenTitle;
	public GameObject AdminDetailPanel;
	public GameObject ButtonsForAdmin;
	public GameObject ButtonsForMember;
	public GameObject ButtonsFor_NonMember;

	public GameObject ChatScreen;
	public Text ChatBagdeCountText;

	public List<Sprite> AchivementImage = new List<Sprite> ();
	public List<Sprite> SocietyIcon = new List<Sprite> ();
	//		public GameObject

	[Header ("Admin Attribures")]
	public Text SocietyName;
	public Text SocietyTagName;
	public Text SocietyDiscription;

	public Image SocietyEmblemIcon;
	public Image AchivementBadgeIcon;

	public Button SocietyMember;
	public Button AddMember;
	public Button LeaderBoard;
	public Button Achivement;
	public Button Chat;
	public Button RequestsButton;
	public Button SocietyEvent;
	public Button HostParty;

	[Header ("Society Party Admin Atribute Manage")]
	public GameObject HostPartyButton;
	public GameObject JoinPartyButton;
	public GameObject CoolDownButton;
	[Header ("Society Party Member Atribute Manage")]
	public GameObject MHostPartyButton;
	public GameObject MJoinPartyButton;
	public GameObject MCoolDownButton;
	public Text TotalMemberCounts;


	void Awake ()
	{
		Reload ();

	}

	public void OpenSocietyDiscription (int Role, SocietyManager.Society society)
	{
		if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.Admin_MemberDiscriptionPanel) {
		} else {
			ScreenAndPopupCall.Instance.OpenAnd_MemberDiscriptionPanel ();
		}
      
		transform.GetChild (0).FindChild ("Back").gameObject.SetActive (GameManager.Instance.GetComponent<Tutorial> ()._SocietyCreated); 
        

		SocietyName.text = society.Name;
		SocietyDiscription.text = society.Description;
		string tagsString = "";
//		foreach (var tag in society.Tags) {
//			tagsString += tag + ","; 
//		}
//		SocietyTagName.text = tagsString.Remove (tagsString.Length - 1);

		for (int i = 0; i < society.Tags.Count; i++) {
			if (i == society.Tags.Count - 1)
				tagsString += society.Tags [i];
			else
				tagsString += society.Tags [i] + ","; 
		}
		SocietyTagName.text = tagsString;
		SocietyEmblemIcon.sprite = SocietyManager.Instance.EmblemList [society.EmblemType];

//		SocietyEmblemIcon.sprite = SocietyManager.Instance.EmblemList [0]; //[society.EmblemType];
//		AchivementBejjIcon.sprite = ;TODO
		//		role id - 2(Normal Member), 1(Committee member), 0(President), 3(Unknown)
		Chat.interactable = true;

		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut._SocietyCreated) {
			ButtonsForMember.SetActive (false);
			ButtonsFor_NonMember.SetActive (false);
			ButtonsForAdmin.SetActive (true);
			ButtonsForAdmin.transform.FindChild ("Report").gameObject.SetActive (false);

			SocietyMember.interactable = false;
			AddMember.interactable = false;
			LeaderBoard.interactable = false;
			Achivement.interactable = false;
			Chat.interactable = false;
			RequestsButton.interactable = false;
			SocietyEvent.interactable = false;
			HostParty.interactable = true;
		} else if (Role == 0) {
			ButtonsForMember.SetActive (false);
			ButtonsFor_NonMember.SetActive (false);
			ButtonsForAdmin.SetActive (true);
			ButtonsForAdmin.transform.FindChild ("Report").gameObject.SetActive (false);

		} else if (Role == 1) {
			ButtonsForMember.SetActive (false);
			ButtonsFor_NonMember.SetActive (false);
			ButtonsForAdmin.SetActive (true);
			ButtonsForAdmin.transform.FindChild ("Report").gameObject.SetActive (true);
		} else if (Role == 2) {
			ButtonsForMember.SetActive (true);
			ButtonsFor_NonMember.SetActive (false);
			ButtonsForAdmin.SetActive (false);
		} else if (Role == 3) {
			ButtonsForMember.SetActive (false);
			ButtonsFor_NonMember.SetActive (true);
			ButtonsForAdmin.SetActive (false);
			Chat.interactable = false;
		}
		MessageCount = 0;
		ChatBagdeCountText.gameObject.SetActive (false);

		SocietyMember.onClick.RemoveAllListeners ();
		LeaderBoard.onClick.RemoveAllListeners ();
		Achivement.onClick.RemoveAllListeners ();
		AddMember.onClick.RemoveAllListeners ();
		RequestsButton.onClick.RemoveAllListeners ();
		SocietyMemberList (false);
		SocietyMember.onClick.AddListener (() => SocietyMemberList (true));
		RequestsButton.onClick.AddListener (() => StartCoroutine (SocietyManager.Instance.GetSocietyInvitations ()));

		Achivement.onClick.AddListener (() => SocietyShowAllAchievements ());
		AddMember.onClick.AddListener (() => SocietyManager.Instance.AddMember ());

		LeaderBoard.onClick.AddListener (() => {
			StartCoroutine (GetCurrentSocietyEvent ());
		});

		ChatScreen.SetActive (false);
	}

	IEnumerator GetCurrentSocietyEvent ()
	{
		yield return (EventManagment.Instance.GetAllEventsForCheck ());
		var currentEvent = GetOnGoingSocietyEvent ();

		if (currentEvent != null) {
			StartCoroutine (SocietyGetLeaderboard (currentEvent.Event_id));
		} else {
			ScreenAndPopupCall.Instance.SocietyLeaderBoard ();
			GenerateLeaderboard (new List<LeaderboardData> ());

		}
	}

	IEnumerator SocietyGetLeaderboard (int _eventId)
	{
		var Leaderboard = new List<LeaderboardData> ();
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var json = new JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		json ["event_id"] = _eventId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/societyEvents/getSocietyEventVoteData", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				for (int i = 0; i < _jsnode ["data"].Count; i++) {
					var data = _jsnode ["data"] [i];
					var LB = new LeaderboardData ();

					int vote_count = 0;
					int vote_bonus = 0;
					int friend_bonus = 0;
					int totalScore = 0;

					int.TryParse (data ["vote_count"], out vote_count);
					int.TryParse (data ["vote_bonus"], out vote_bonus);
					int.TryParse (data ["friend_bonus"], out friend_bonus);

					totalScore = (vote_count * 10) + friend_bonus + vote_bonus;

					int.TryParse (data ["society_id"], out LB.player_id);
					LB.player_name = data ["society_name"];
					LB.player_score = totalScore;

					Leaderboard.Add (LB);
				}
			} else {
				
			}
			Leaderboard.Sort ((s2, s1) => s1.player_score.CompareTo (s2.player_score));
			ScreenAndPopupCall.Instance.SocietyLeaderBoard ();
			GenerateLeaderboard (Leaderboard);
		} else {
			
		}
	}

	void GenerateLeaderboard (List<LeaderboardData> leaderboard)
	{	
		bool societyIncluded = false;
		var LeaderboardContainer = SocietyManager.Instance.LeaderBoardContainer;
		var LeaderboardPrefab = SocietyManager.Instance.SocietyLBPrefab;
		for (int i = 0; i < SocietyManager.Instance.LeaderBoardContainer.childCount; i++) {
			Destroy (LeaderboardContainer.transform.GetChild (i).gameObject);
		}

		if (leaderboard.Count >= 1) {
			foreach (var _object in leaderboard) {

				GameObject obj = (GameObject)Instantiate (LeaderboardPrefab, Vector2.zero, Quaternion.identity);
				obj.transform.SetParent (LeaderboardContainer.transform);
				obj.transform.localScale = Vector3.one;

				obj.transform.GetChild (0).GetComponent<Text> ().text = (leaderboard.IndexOf (_object) + 1).ToString ();
				obj.transform.GetChild (1).GetComponent <Text> ().text = _object.player_name.ToString ();
				obj.transform.GetChild (2).GetComponent <Text> ().text = _object.player_score.ToString ();

				if (_object.player_id == SocietyManager.Instance.SelectedSociety.Id) {
					obj.GetComponent <Image> ().enabled = true;

					societyIncluded = true;
				} else
					obj.GetComponent <Image> ().enabled = false;
				
				if (leaderboard.IndexOf (_object) == 11 || leaderboard.IndexOf (_object) == 10) {
					obj.GetComponent<Image> ().enabled = true;
					for (int i = 0; i < obj.transform.childCount; i++)
						obj.transform.GetChild (i).gameObject.SetActive (true);
					if (leaderboard.IndexOf (_object) == 11)
						break;
				}
			}

			if (!societyIncluded) {
//				List<int> indexes = new List<int> ();
//
//				foreach (var _object in leaderboard) {
//					if (_object.player_id == PlayerPrefs.GetInt ("PlayerId"))
//						indexes.Add (leaderboard.IndexOf (_object));
//				}
//
//				foreach (int index in indexes) {
//					GameObject obj_pre = (GameObject)Instantiate (LeaderboardPrefab, LeaderboardContainer.transform, false);
//
//					obj_pre.transform.GetChild (0).GetComponent<Text> ().text = (index).ToString ();
//					obj_pre.transform.GetChild (1).GetComponent <Text> ().text = leaderboard [index - 1].player_name.ToString ();
//					obj_pre.transform.GetChild (2).GetComponent <Text> ().text = leaderboard [index - 1].player_score.ToString ();
//
//
				GameObject obj = (GameObject)Instantiate (LeaderboardPrefab, LeaderboardContainer.transform, false);

				obj.transform.GetChild (0).GetComponent<Text> ().text = (leaderboard.Count + 1).ToString ();
				obj.transform.GetChild (1).GetComponent <Text> ().text = SocietyManager.Instance.SelectedSociety.Name;
				obj.transform.GetChild (2).GetComponent <Text> ().text = "0";
//
//
//					GameObject obj_after = (GameObject)Instantiate (LeaderboardPrefab, LeaderboardContainer.transform, false);
//
//					obj_after.transform.GetChild (0).GetComponent<Text> ().text = (index + 2).ToString ();
//					obj_after.transform.GetChild (1).GetComponent <Text> ().text = leaderboard [index + 1].player_name.ToString ();
//					obj_after.transform.GetChild (2).GetComponent <Text> ().text = leaderboard [index + 1].player_score.ToString ();
//				}
			}
		} else {
			GameObject obj = (GameObject)Instantiate (LeaderboardPrefab, Vector2.zero, Quaternion.identity);
			obj.transform.SetParent (LeaderboardContainer.transform);
			obj.transform.localScale = Vector3.one;

			obj.transform.GetChild (0).gameObject.SetActive (false);
			obj.transform.GetChild (1).GetComponent <Text> ().text = "No Leaderboard Available";
			obj.transform.GetChild (2).gameObject.SetActive (false);

		}
	}

	EventManagmentStruct GetOnGoingSocietyEvent ()
	{
		var Temp = new List<EventManagmentStruct> ();

		foreach (var Event in EventManagment.Instance.AllEventList) {
			if (Event.category == eventType.Society_Event && Event.EndTime > System.DateTime.UtcNow) {
				Temp.Add (Event);
			}				
		}
		Temp.Sort ((E1, E2) => E1.EndTime.CompareTo (E2.EndTime));

		if (Temp.Count == 0) {
			foreach (var Event in EventManagment.Instance.AllEventList) {
				if (Event.category == eventType.Society_Event) {
					Temp.Add (Event);
				}
			}
			Temp.Sort ((E1, E2) => E2.EndTime.CompareTo (E1.EndTime));
		}
		if (Temp.Count == 1) {
			return Temp [0];
		} else if (Temp.Count > 1) {
			return Temp [0];
		}
		return null;	
	}

	public void SocietyMemberList (bool openScreen)
	{
		StopAllCoroutines ();
		StartCoroutine (IeSocietyMemberList (openScreen));
	}


	IEnumerator IeSocietyMemberList (bool openScreen)
	{
		
		yield return SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.IGetAllPlayers (SocietyManager.Instance.SelectedSociety.Id));      
		TotalMemberCounts.text = SocietyManager.Instance._allMembers.Count.ToString () + "/50";
        
		if (openScreen) {   
			for (int i = 0; i < SocietyManager.Instance.MemberContainer.childCount; i++) {
				Destroy (SocietyManager.Instance.MemberContainer.GetChild (i).gameObject);
			}
			ScreenAndPopupCall.Instance.MemberListPopup ();

			SocietyManager.Instance._allMembers.Sort ();
			foreach (var player in SocietyManager.Instance._allMembers) {
				var obj = Instantiate (SocietyManager.Instance.SocietyMemberPrefab, Vector3.one, Quaternion.identity) as GameObject;
				obj.transform.SetParent (SocietyManager.Instance.MemberContainer);
				obj.transform.localScale = Vector3.one;

				obj.transform.GetComponent <SocietyMember> ()._thisMember = player;
//			if (player.role_id == 0)
//				obj.transform.SetAsFirstSibling ();
//			else if (player.role_id == 1)
//				obj.transform.SetSiblingIndex (1);
//			else if (player.role_id == 2)
//				obj.transform.SetAsLastSibling ();

			}

			SocietyManager.Instance._memberCountText.text = SocietyManager.Instance._allMembers.Count + "/50";
			yield return SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.IeGetMyRole (SocietyManager.Instance.SelectedSociety));
		}
	}

	public void SocietyShowAllAchievements ()
	{
		for (int i = 0; i < SocietyManager.Instance.AcheivementContainer.childCount; i++) {
			Destroy (SocietyManager.Instance.AcheivementContainer.GetChild (i).gameObject);
		}

		StartCoroutine (IeSocietyShowAchievements ());
	}

	IEnumerator IeSocietyShowAchievements ()
	{
		yield return SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.IeGetSocietyAchievements (SocietyManager.Instance.SelectedSociety.Id));
		ScreenAndPopupCall.Instance.AchievementPopup ();

		foreach (var achievement in SocietyManager.Instance._allachievements) {
			var obj = Instantiate (SocietyManager.Instance.SocietyAchievementPrefab, Vector3.one, Quaternion.identity) as GameObject;
			obj.transform.SetParent (SocietyManager.Instance.AcheivementContainer);
		}
	}


	public void DeleteAllAcheivements ()
	{
		for (int i = 0; i < SocietyManager.Instance.AcheivementContainer.childCount; i++)
			SocietyManager.Instance.AcheivementContainer.GetChild (i);
	}


	public void GetAllPlayersofThisSociety ()
	{
		SocietyManager.Instance.GetAllPlayers (SocietyManager.Instance.SelectedSociety.Id);
	}


	public void OpenHomeRoomForSociety ()
	{
        ScreenAndPopupCall.Instance.CloseScreen();
        ScreenAndPopupCall.Instance.CloseCharacterCamera();
        ScreenAndPopupCall.Instance.OpenSocietyHomeRoomScreen();
        ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().GetAllDecorInSocietyRoom();
        int RoomIndex = SocietyManager.Instance.SelectedSociety.RoomType;
        var Room = SocietyManager.Instance.RoomPrefabsList[RoomIndex];

        GameObject BG = Instantiate (MultiplayerManager.Instance.PublicAreas [5].BackgroundPrefab, new Vector3 (-1000, 0, 0), Quaternion.identity) as GameObject;
        BG.name = "SocietyPartyArea";

        GameObject Flat = Instantiate (Room, BG.transform) as GameObject;
        Flat.name = "SocietyPartyRoom";

        Flat.transform.localPosition = Vector3.zero;
//        Flat.transform.localEulerAngles = new Vector3(0f, 0f, 45f);
        Flat.transform.localScale = new Vector3(1f, 1f, 1f);

        Camera.main.GetComponent <DragCamera1> ().enabled = false;
        Camera.main.GetComponent <DragableCamera> ().enabled = true;
        Camera.main.GetComponent <DragableCamera> ().RestrictedMinX = -1050 ;
        Camera.main.GetComponent <DragableCamera> ().RestrictedMaxX = -950;
        Camera.main.GetComponent <DragableCamera> ().RestrictedMaxY = 50;
        Camera.main.GetComponent <DragableCamera> ().RestrictedMinY = -50;

        Camera.main.transform.position = new Vector3(-1000, 0,Camera.main.transform.position.z);
	}

	public void BackToSocietyScreenFromHomeRoom ()
	{
        DecorController.Instance.isForSociety = false;
        ScreenAndPopupCall.Instance.CloseScreen();
        OpenSocietyDiscription(SocietyManager.Instance.myRole, SocietyManager.Instance._mySociety);
        var HomeRoom = GameObject.Find("SocietyPartyArea");
        Destroy(HomeRoom);
        Camera.main.transform.position = new Vector3(-0, 0,Camera.main.transform.position.z);     
        Camera.main.GetComponent <DragCamera1> ().enabled = true;
        Camera.main.GetComponent <DragableCamera> ().enabled = false;

        ScreenManager.Instance.MenuScreen.transform.GetChild (5).gameObject.SetActive (true);
        ScreenManager.Instance.MenuScreen.gameObject.SetActive (true);
        ScreenManager.Instance.HudPanel.SetActive (true);
    }

	public void JoinSociety ()
	{
		StartCoroutine (IeRequestToJoinSociety ());
	}

	IEnumerator IeRequestToJoinSociety ()
	{
		yield return StartCoroutine (SocietyManager.Instance.IGetSocieties (SocietyManager.SeachType.mine, "", true));

		if (SocietyManager.Instance._mySociety != null)
			SocietyManager.Instance.ShowPopUp ("You are already a member of some another society", null);
		else {
			string message = string.Format ("Hey! {0} wants to join your society - {1}", PlayerPrefs.GetString ("UserName"), SocietyManager.Instance.SelectedSociety.Name);
			StartCoroutine (SocietyManager.Instance.SendSocietyInvitations (message));
		}
	}


	//	IEnumerator IGetHigherMembersofSociety (int society_id)
	//	{
	//		var encoding = new System.Text.UTF8Encoding ();
	//
	//		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
	//		var jsonElement = new Simple_JSON.JSONClass ();
	//
	//		jsonElement ["data_type"] = "get_member";
	//		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
	//		jsonElement ["society_id"] = society_id.ToString ();
	//
	//		postHeader.Add ("Content-Type", "application/json");
	//		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
	//
	//		WWW www = new WWW ("http://pinekix.ignivastaging.com/societies/society", encoding.GetBytes (jsonElement.ToString ()), postHeader);
	//
	//		print ("jsonDtat is ==>> " + jsonElement.ToString ());
	//		yield return www;
	//
	//		if (www.error == null) {
	//			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
	//			print ("_jsnode ==>> " + _jsnode.ToString ());
	//			if (_jsnode ["status"].ToString ().Contains ("200")) {
	//
	//				print ("Success");
	//				var data = _jsnode ["data"];
	//
	//				List<int> IdArray = new List<int> ();
	//				for (int i = 0; i < data.Count; i++) {
	//					int role_id = -1;
	//					int.TryParse (data [i] ["role_id"], out role_id);
	//
	//					if (role_id == 0 || role_id == 1) {
	//						int playerId = 0;
	//						int.TryParse (data [i] ["player_id"], out playerId);
	//						if (playerId != 0)
	//							IdArray.Add (playerId);
	//					}
	//				}
	//				x = 0;
	//				arryLenght = IdArray.Count;
	//				foreach (var id in IdArray) {
	//					yield return StartCoroutine (SendNotificationsToAuth (id));
	//				}
	//			}
	//		}
	//	}
	//
	//	int x = 0;
	//	int arryLenght = 0;

	//	IEnumerator SendNotificationsToAuth (int Id)
	//	{
	//		ScreenAndPopupCall.Instance.LoadingScreen ();
	//
	//		string message = string.Format ("Hey! {0} wants to join your society - {1}", PlayerPrefs.GetString ("UserName"),SocietyManager.Instance.SelectedSociety.Name);
	//		CoroutineWithData cd = new CoroutineWithData (NotificationManager.Instance, NotificationManager.Instance.ISendInvitationToUser (Id, message, SocietyManager.Instance.SelectedSociety.Id,false, null));
	//
	//		yield return cd.coroutine;
	//
	//		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
	//			x++;
	//			if (x == arryLenght) {
	//				ScreenAndPopupCall.Instance.LoadingScreenClose ();
	//
	//				SocietyManager.Instance.ShowPopUp (string.Format ("Request to join \"{0}\" society sent successfully", SocietyManager.Instance.SelectedSociety.Name), null);
	//			}
	//		} else {
	//			ScreenAndPopupCall.Instance.LoadingScreenClose ();
	//			SocietyManager.Instance.ShowPopUp (string.Format ("Request to join \"{0}\" society failed", SocietyManager.Instance.SelectedSociety.Name), null);
	//		}
	//	}

	//	IEnumerator ISendNotificationsToAuth(int Id)
	//	{
	//		var encoding = new System.Text.UTF8Encoding ();
	//
	//		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
	//		var jsonElement = new Simple_JSON.JSONClass ();
	//
	//		jsonElement ["data_type"] = "save";
	//		jsonElement ["sender_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();;
	//		jsonElement ["reciever_player_id"] =Id.ToString ();
	//		jsonElement ["message"] ="Hey! add me in your society";
	//		var jsonarray = new JSONArray ();
	//
	//		var jsonItem = new JSONClass ();
	//		jsonItem ["item_id"] = SocietyManager.Instance.SelectedSociety.Id.ToString ();
	//		jsonItem ["item_type"]= "Society";
	//		jsonarray.Add (jsonItem);
	//
	//		jsonElement ["items"] = jsonarray;
	//
	//		postHeader.Add ("Content-Type", "application/json");
	//		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
	//
	//		WWW www = new WWW ("http://pinekix.ignivastaging.com/invitations/invitation", encoding.GetBytes (jsonElement.ToString ()), postHeader);
	//
	//		print ("jsonDtat is ==>> " + jsonElement.ToString ());
	//		yield return www;
	//
	//		if (www.error == null) {
	//			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
	//			print ("_jsnode ==>> " + _jsnode.ToString ());
	//			if (_jsnode ["status"].ToString ().Contains ("200")) {
	//
	//				print ("Success");
	//				yield return true;
	//			}
	//		}
	//	}


	public void LeaveSociety ()
	{
		if (SocietyManager.Instance.myRole == 2)
			ShowPopupofLeaveSociety ();
		else
			ShowPopupofError ();
	}


	public void ShowPopupofLeaveSociety ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Do you really want to Leave the Society?";	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => SocietyManager.Instance.RemovePlayer (PlayerPrefs.GetInt ("PlayerId")));
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}


	public void ShowPopupofError ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "You Can't leave society. You have to first demote yourself";	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}

	public void ShowPopupIfTotelMemberLimitDone (string msg)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}

	public void ConnectToChatInSociety ()
	{
		if (!ChatManager.Instance.ChatConnected) {
			MultiplayerManager.Instance.WaitingScreenState (true);
			ChatManager.Instance.AddChannelToConnect (SocietyManager.Instance.SelectedSociety.Name);
			ChatManager.Instance.ConnectToChatServer ();
			SocietyManager.Instance.isChatInSocietyOn = true;

		} else {
			ChatScreenStatus ();
		}
	}

	public void DeleteOldMessageGameObjects ()
	{
		for (int i = 0; i < ChatContainer.childCount; i++) {
			Destroy (ChatContainer.GetChild (i).gameObject);
		}
	}

	int MessageCount = 0;

	public void GenerateMessageInSociety (string message, string sender)
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

//		if (sender == PlayerManager.Instance.playerInfo.Username)
//			obj = (GameObject)Instantiate (ChatManager.Instance.players_message);
//		else
//			obj = (GameObject)Instantiate (ChatManager.Instance.other_message);

		if (sender == PlayerManager.Instance.playerInfo.Username) {
			
			obj = (GameObject)Instantiate (ChatManager.Instance.players_message);
		} else {
			obj = (GameObject)Instantiate (ChatManager.Instance.other_message);

		}

		obj.transform.SetParent (ChatContainer, false);
		obj.transform.localScale = Vector3.zero;

		iTween.ScaleTo (obj, iTween.Hash ("scale", Vector3.one, "time", 0.1f, "easeType", "easeInCirc"));

		obj.transform.FindChild ("Message").GetComponent<Text> ().text = _message;
		obj.transform.FindChild ("UserName").GetComponent<Text> ().text = sender;
		obj.transform.FindChild ("Time Text").GetComponent<Text> ().text = Localtime.ToString ("hh:mm tt");

		Invoke ("scrollValue", 0.1f);

		if (!IsShown) {
			ChatBagdeCountText.gameObject.SetActive (true);
			MessageCount++;

		} else {
			MessageCount = 0;
			ChatBagdeCountText.gameObject.SetActive (false);
		}
		ChatBagdeCountText.text = MessageCount.ToString ();
	}

	void scrollValue ()
	{
		ChatContainer.GetComponentInParent<ScrollRect> ().verticalScrollbar.value = 0;
	}

	//	Vector3 OriginalPosition = Vector3.zero;
	bool IsShown = false;
	public Transform ChatPosition;
	public Transform ChatContainer;

	public void ChatScreenStatus ()
	{
		if (!IsShown) {			
			ChatScreen.SetActive (true);
			IsShown = true;
			MessageCount = 0;
			ChatBagdeCountText.gameObject.SetActive (false);
		} else if (IsShown) {
			ChatScreen.SetActive (false);
			IsShown = false;
		}
	}

	public void CloseChatscreen ()
	{
		ChatScreen.SetActive (false);
		IsShown = false;
	}

	public void OnClickBackFromSociety ()
	{
		SocietyManager.Instance.isChatInSocietyOn = false;
		ChatScreen.SetActive (false);
		if (ChatManager.Instance.ChatConnected)
			ChatManager.Instance.DisConnectChatFromServer ();
        
		for (int i = 0; i < SocietyManager.Instance.MySocietyContainer.transform.childCount; i++) {
			GameObject.Destroy (SocietyManager.Instance.MySocietyContainer.transform.GetChild (i).gameObject);
		}
	}

	public void SendMessageInSociety (InputField inputField)
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
}