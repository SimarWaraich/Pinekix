using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Simple_JSON;

public class SocietyPartyUI : MonoBehaviour
{
	public const string GetKickOutPlayerData = "http://pinekix.ignivastaging.com/flats/getKickedOutPlayers";

	public SocietyPartyManager.SocietyPartyHost mySocietyParty;

	public Text SocietyPartyTitle;
	public Text SocietyPartyDiscription;
	public Text SocietyPartyMebmer;
	public Text SocietyPartyTime;
	public Image Icon;
	public DateTime EndTime;
	public DateTime CoolDownTimel;
	public float societyPartyEndTime;



	// Use this for initialization
	public void Start ()
	{
		SocietyPartyTitle.text = mySocietyParty.Name;
		SocietyPartyDiscription.text = mySocietyParty.Description;
		SocietyPartyMebmer.text = mySocietyParty.PresentMembers + "/" + mySocietyParty.TotleMember;
		EndTime = mySocietyParty.PartyEndTime;
		CoolDownTimel = mySocietyParty.PartyCoolDown;

		for (int i = 0; i < SocietyManager.Instance.EmblemList.Count; i++) {
			if (SocietyManager.Instance.SelectedSociety.EmblemType == i) {
				Icon.sprite = SocietyManager.Instance.EmblemList [i];
			}
		}
	}

	public void OpenSocietyParty ()
	{
		GameManager.Instance.SocietyPartyWayPoints.Clear ();//Clear All the waypoint
		ScreenAndPopupCall.Instance.IsChatOpen = false;
		if (PhotonNetwork.countOfPlayersInRooms < mySocietyParty.TotleMember) {
			if (!PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.IsBusy) {
				SocietyPartyManager.Instance.KickedOutPlayerList.Clear ();
				StartCoroutine (CheckPlayerKickedOut ());
			} else {
				SocietyPartyControler.Instance.ShowLessAmmountMesg ("Player is busy please try after some time.");
			}
		} else {
			SocietyDescriptionController.Instance.ShowPopupIfTotelMemberLimitDone (mySocietyParty.Name + " society party member limit has been reached. Please try again later.");
		}
	}

	public IEnumerator CheckPlayerKickedOut ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["party_id"] = mySocietyParty.PartyId.ToString ();
		jsonElement ["party_type"] = "2";

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
				var kickedOutPlyr = new List<SocietyPartyManager.SocietyKickedOutPlayerList> ();
				for (int i = 0; i < data.Count; i++) {
					int PlayeId = 0;
					int.TryParse (data [i] ["player_id"], out PlayeId);
					int PartyId = 0;
					int.TryParse (data [i] ["party_id"], out PartyId);
					int partyType = 0;
					int.TryParse (data [i] ["party_type"], out partyType);

					SocietyPartyManager.SocietyKickedOutPlayerList kickedOutPlayer = new SocietyPartyManager.SocietyKickedOutPlayerList (PlayeId, PartyId, partyType);
					kickedOutPlyr.Add (kickedOutPlayer);				
				}
				SocietyPartyManager.Instance.KickedOutPlayerList = kickedOutPlyr;

				for (int x = 0; x < SocietyPartyManager.Instance.KickedOutPlayerList.Count; x++) {
					if (SocietyPartyManager.Instance.KickedOutPlayerList [x].PlayerId != PlayerPrefs.GetInt ("PlayerId") && SocietyPartyManager.Instance.KickedOutPlayerList [x].PartyId == mySocietyParty.PartyId) {						
						SocietyPartyManager.Instance.selectedSocietyParty = mySocietyParty;
						MultiplayerManager.Instance.JoinorCreateRoomForSocietyParty ("Society" + mySocietyParty.Name, mySocietyParty.TotleMember);
						OpenPartyArea ();
//						ShowMyJoindParty ();
					} else if (SocietyPartyManager.Instance.KickedOutPlayerList [x].PlayerId == PlayerPrefs.GetInt ("PlayerId") && SocietyPartyManager.Instance.KickedOutPlayerList [x].PartyId == mySocietyParty.PartyId) {
						ResponceMessageShow ("You were kicked out from " + mySocietyParty.Name + " society party.");
					}
				}
			} else if (_jsnode ["status"].ToString ().Contains ("400") && _jsnode ["description"].ToString ().Contains ("No players has kicked out")) {
				SocietyPartyManager.Instance.selectedSocietyParty = mySocietyParty;
				MultiplayerManager.Instance.JoinorCreateRoomForSocietyParty ("Society" + mySocietyParty.Name, mySocietyParty.TotleMember);
				OpenPartyArea ();
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

	public void OpenPartyArea ()
	{
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		Button.AllButtons [0].transform.parent.gameObject.SetActive (false);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = false;
		HostPartyManager.Instance.GameGUIControle (false);
		SocietyPartyManager.Instance.AttendingParty = true;
//		ScreenAndPopupCall.Instance.ShowSocietyRunningPartyScreen ();
		FlatPartyHostingControler.Instance.ScreenCanMove = true;
//		SocietyPartyManager.Instance.DeleteGameObjects ();
	}

	public void StartTimerIfEnded ()
	{
		SocietyPartyManager.Instance.selectedSocietyParty = mySocietyParty;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (EndTime > DateTime.Now) {
			transform.FindChild ("Join Party").GetComponent <Button> ().interactable = true;
			var Diff = EndTime - DateTime.Now;
			float TimeRemain = (float)Diff.TotalSeconds;
			societyPartyEndTime = TimeRemain;
			SocietyPartyTime.text = ExtensionMethods.GetTimeStringFromFloat (societyPartyEndTime);
			SocietyPartyMebmer.text = PhotonNetwork.countOfPlayersInRooms.ToString () + "/" + mySocietyParty.TotleMember.ToString ();

			if (societyPartyEndTime < 0.5f) {
				var Button = GameManager.Instance.GetComponent<Tutorial> ();
				Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
				Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
				Button.AllButtons [6].gameObject.SetActive (true);
				HostPartyManager.Instance.GameGUIControle (true);
				SocietyPartyControler.Instance.PartyHosted = false;
				SocietyPartyControler.Instance.isPaid = false;
				SocietyPartyControler.Instance.myPartyEnding = true;			
				SocietyPartyManager.Instance.selectedSocietyParty = mySocietyParty;
				SocietyPartyControler.Instance.SocietyPartyHostCoolDown ();
				return;
			}

		} else {
			SocietyPartyTime.text = "Party has ended";
			transform.FindChild ("Join Party").GetComponent <Button> ().interactable = false;
		}
	}
}
