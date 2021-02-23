using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Simple_JSON;

public class HostPartyUI : MonoBehaviour
{
	public const string GetKickOutPlayerData = "http://pinekix.ignivastaging.com/flats/getKickedOutPlayers";

	public HostPartyManager.HostParty HostParty;
	public Text TitleText;
	public Text DiscriptionText;
	//	public Image PartyIcon;
	private bool PartyType;
	public Text Members;
	public Text TimeDuration;
	public DateTime EndTime;
	public DateTime CteatedTime;
	public float partyEndTime;
	public bool openUI = false;

	public void Start ()
	{		 
//		if (PlayerPrefs.HasKey ("FlatPartyEndTime")) {
//			var Temp = Convert.ToInt64 (PlayerPrefs.GetString ("FlatPartyEndTime"));
//			EndTime = DateTime.FromBinary (Temp);
//		}
//		ScreenManager.Instance.RunningParty.gameObject.GetComponent<FlatPartyScreenControle> ().enabled = false;
		TitleText.text = HostParty.Name;
		DiscriptionText.text = HostParty.Description;
		var Count = PhotonNetwork.countOfPlayersInRooms + 1;
		Members.text = Count.ToString () + "/" + HostParty.TotleMember.ToString ();
//		TimeDuration.text = HostParty.PartyEndTime.ToString ("hh:mm:ss");
		EndTime = HostParty.PartyEndTime;
		CteatedTime = HostParty.PartyCreatedTime;
		PartyType = HostParty.PartyType;
//		if (HostPartyManager.Instance.PartyIconList.Count <= HostParty.Icon || HostParty.Icon < 0) {
//			HostParty.Icon = Random.Range (0, HostPartyManager.Instance.PartyIconList.Count);
//		}
//		PartyIcon.sprite = HostPartyManager.Instance.PartyIconList [HostParty.Icon];
//		SaveEndTimeForSubscription ();

	}

	public void OpenFlatParty ()
	{		
		GameManager.Instance.FlatPartyWayPoints.Clear ();//Clear all Way Point
		ScreenAndPopupCall.Instance.IsChatOpen = false;
		if (PhotonNetwork.countOfPlayersInRooms < HostParty.TotleMember) {
			if (!PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.IsBusy) {
				HostPartyManager.Instance.KickedOutPlayerList.Clear ();
				StartCoroutine (CheckPlayerKickedOut ());

			} else {
				FlatPartyHostingControler.Instance.ShowLessAmmountMesg ("Player is busy please try after some time.");
			}
		} else {
			SocietyDescriptionController.Instance.ShowPopupIfTotelMemberLimitDone (HostParty.Name + " flat party member limit has been reached. Please try again later.");
		}
	}

	public void OpenOfflineFlatParty ()
	{	
		if (!PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.IsBusy) {
			HostPartyManager.Instance.selectedFlatParty = HostParty;					
			ShowMyJoindParty ();
			openUI = true;
		} else {
			FlatPartyHostingControler.Instance.ShowLessAmmountMesg ("Player is busy please try after some time.");
		}

	}

	public void ShowMyJoindParty ()
	{		
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		ShowThisScreen ();

		if (Tut.HostPartyCreated)
			MultiplayerManager.Instance.JoinorCreateRoomForFlatParty ("Flat_" + HostParty.Name, HostParty.TotleMember);
		else
			MultiplayerManager.Instance.MoveToPublicAreaForFlat ();
	}



	public void ShowThisScreen ()
	{
//		ScreenAndPopupCall.Instance.CloseScreen ();
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Button.AllButtons [0].transform.parent.gameObject.SetActive (false);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = false;
		HostPartyManager.Instance.GameGUIControle (false);
		ScreenManager.Instance.RunningParty.gameObject.GetComponent<FlatPartyScreenControle> ().enabled = true;
//		ScreenAndPopupCall.Instance.ShowRunningPartyScreen ();
		FlatPartyHostingControler.Instance.ScreenCanMove = true;
		HostPartyManager.Instance.AttendingParty = true;

		DeleteAllParty ();
	}

	IEnumerator DeleteAllParty ()
	{
		yield return new WaitForSeconds (1.8f);
		for (int i = 0; i < HostPartyManager.Instance.AllFlatPartyContainer.transform.childCount; i++) {
			GameObject.Destroy (HostPartyManager.Instance.AllFlatPartyContainer.transform.GetChild (i).gameObject);	
			if (HostPartyManager.Instance.selectedFlatParty.Id != HostParty.Id) {
				
			}
		}
		for (int i = 0; i < HostPartyManager.Instance.MyFlatPartyContainer.transform.childCount; i++) {
			GameObject.Destroy (HostPartyManager.Instance.MyFlatPartyContainer.transform.GetChild (i).gameObject);	
			if (HostPartyManager.Instance.selectedFlatParty.Id != HostParty.Id) {

			}
		}
	}

	void SaveEndTimeForSubscription ()
	{
		print (EndTime);
		PlayerPrefs.SetString ("FlatPartyEndTime", EndTime.ToBinary ().ToString ());
	}



	void Update ()
	{
		if (EndTime > DateTime.Now) {
			transform.FindChild ("Join Party").GetComponent <Button> ().interactable = true;
			var Diff = EndTime - DateTime.Now;
			float TimeRemain = (float)Diff.TotalSeconds;
			partyEndTime = TimeRemain;
			TimeDuration.text = ExtensionMethods.GetTimeStringFromFloat (partyEndTime);
			Members.text = PhotonNetwork.countOfPlayersInRooms.ToString () + "/" + HostParty.TotleMember.ToString ();
//			Members.text = HostParty.PresentMembers.ToString () + "/" + HostParty.TotleMember.ToString ();
		
			if (partyEndTime < 0.5f) {
				if (HostParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) {
					FlatPartyHostingControler.Instance.myPartyEnding = true;
					FlatPartyHostingControler.Instance.CooldownShow.SetActive (true);
					FlatPartyHostingControler.Instance.PartyHostCoolDown ();

					if (partyEndTime < 0.08f) {
						HostPartyManager.Instance.DeleteGameObjects (HostPartyManager.Instance.MyFlatPartyContainer);
					}
				}
//				HostPartyManager.Instance.DeleteFlatParty ();
//				HostPartyManager.Instance.GetFlatParty (2);			
				return;
			}

		} else {
			TimeDuration.text = "Party has ended";
			transform.FindChild ("Join Party").GetComponent <Button> ().interactable = false;
			//			if (EndTime < DateTime.Now) {
//				PlayerPrefs.DeleteKey ("FlatPartyEndTime");		
//				ScreenAndPopupCall.Instance.CloseScreen ();
//				HostPartyManager.Instance.AttendingParty = false;
//				HostPartyManager.Instance.CheckPartyStatus ();
//				return;
//			}
		
		}
	}

	public IEnumerator CheckPlayerKickedOut ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["party_id"] = HostParty.Id.ToString ();
		jsonElement ["party_type"] = "1";

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (GetKickOutPlayerData, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Kicked out players are following")) {
				JSONNode data = _jsnode ["data"];
				var kickedOutPlyr = new List<HostPartyManager.HostPartyKickedOutPlayerList> ();
				for (int i = 0; i < data.Count; i++) {
					int PlayeId = 0;
					int.TryParse (data [i] ["player_id"], out PlayeId);
					int PartyId = 0;
					int.TryParse (data [i] ["party_id"], out PartyId);
					int prtyType = 0;
					int.TryParse (data [i] ["party_type"], out prtyType);

					HostPartyManager.HostPartyKickedOutPlayerList kickedOutPlayer = new HostPartyManager.HostPartyKickedOutPlayerList (PlayeId, PartyId, prtyType);
					kickedOutPlyr.Add (kickedOutPlayer);				
				}
				HostPartyManager.Instance.KickedOutPlayerList = kickedOutPlyr;

				for (int x = 0; x < HostPartyManager.Instance.KickedOutPlayerList.Count; x++) {
					if (HostPartyManager.Instance.KickedOutPlayerList [x].PlayerId != PlayerPrefs.GetInt ("PlayerId") && HostPartyManager.Instance.KickedOutPlayerList [x].PartyId == HostParty.Id) {						
						HostPartyManager.Instance.selectedFlatParty = HostParty;					
						ShowMyJoindParty ();
						openUI = true;
					} else if (HostPartyManager.Instance.KickedOutPlayerList [x].PlayerId == PlayerPrefs.GetInt ("PlayerId") && HostPartyManager.Instance.KickedOutPlayerList [x].PartyId == HostParty.Id) {
						ResponceMessageShow ("You were kicked out from " + HostParty.Name + " flat party.");
					}
				}
			} else if (_jsnode ["status"].ToString ().Contains ("400") && _jsnode ["description"].ToString ().Contains ("No players has kicked out")) {
				HostPartyManager.Instance.selectedFlatParty = HostParty;
				ShowMyJoindParty ();
				openUI = true;
			}
		}

	}

	public void ResponceMessageShow (string msg)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}


	//	void Update ()
	//	{ backup Script
	//		if (EndTime > DateTime.Now) {
	//			transform.FindChild ("Join Party").GetComponent <Button> ().interactable = true;
	//			var Diff = EndTime - DateTime.Now;
	//			float TimeRemain = (float)Diff.TotalSeconds;
	//			partyEndTime = TimeRemain;
	//			TimeDuration.text = ExtensionMethods.GetTimeStringFromFloat (partyEndTime);
	//			Members.text = PhotonNetwork.countOfPlayersInRooms.ToString () + "/" + HostParty.TotleMember.ToString ();
	//			//			Members.text = HostParty.PresentMembers.ToString () + "/" + HostParty.TotleMember.ToString ();
	//			if (HostPartyManager.Instance.AttendingParty) {
	//
	//				ScreenManager.Instance.RunningParty.transform.FindChild ("time").GetChild (0).GetComponent<Text> ().text = ExtensionMethods.GetTimeStringFromFloat (partyEndTime);
	//				var Count = PhotonNetwork.countOfPlayersInRooms + 1;
	//				ScreenManager.Instance.RunningParty.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = Count + "/" + HostPartyManager.Instance.selectedFlatParty.TotleMember;
	//			} else {
	//				ScreenManager.Instance.RunningParty.transform.FindChild ("time").GetChild (0).GetComponent<Text> ().text = "";
	//				ScreenManager.Instance.RunningParty.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = "";
	//			}
	//
	//			if (partyEndTime < 0.5f) {
	//				HostPartyManager.Instance.AttendingParty = false;
	//				HostPartyManager.Instance.CheckPartyStatus ();
	//				FlatPartyHostingControler.Instance.ScreenCanMove = false;
	//				var Button = GameManager.Instance.GetComponent<Tutorial> ();
	//				Button.AllButtons [0].interactable = true;
	//				Button.AllButtons [6].interactable = true;
	//				if (HostParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) {
	//					FlatPartyHostingControler.Instance.myPartyEnding = true;
	//					FlatPartyHostingControler.Instance.CooldownShow.SetActive (true);
	//					FlatPartyHostingControler.Instance.PartyHostCoolDown ();
	//				}
	//
	//				HostPartyManager.Instance.DeleteFlatParty ();
	//				if (HostPartyManager.Instance.AttendingParty) {
	//					ScreenAndPopupCall.Instance.CloseScreen ();
	//					MultiplayerManager.Instance._flatParty = false;
	//					MultiplayerManager.Instance.LeaveRoom ();
	//					Destroy (GameObject.Find ("FlatPartyRoom").gameObject);
	//					PlayerPrefs.DeleteKey ("FlatPartyEndTime");
	//				}
	//
	//				return;
	//			}
	//
	//		} else {
	//			TimeDuration.text = "Party has ended";
	//			transform.FindChild ("Join Party").GetComponent <Button> ().interactable = false;
	//			//			if (EndTime < DateTime.Now) {
	//			//				PlayerPrefs.DeleteKey ("FlatPartyEndTime");
	//			//				ScreenAndPopupCall.Instance.CloseScreen ();
	//			//				HostPartyManager.Instance.AttendingParty = false;
	//			//				HostPartyManager.Instance.CheckPartyStatus ();
	//			//				return;
	//			//			}
	//
	//		}
	//	}


}
