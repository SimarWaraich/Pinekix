using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Simple_JSON;
using System.Text.RegularExpressions;

public class SocietyPartyManager : Singleton<SocietyPartyManager>
{
	public const string GetSocietyPartyLink = "http://pinekix.ignivastaging.com/societyParties/party";

	public List<SocietyKickedOutPlayerList> KickedOutPlayerList;

	public List<SocietyPartyHost> AllSocietyParty;
	public GameObject AllSocietyPartyContainer;
	public GameObject AllFlatPartyParent;

	public GameObject SocieryPartyPrefabsList;

	public bool HostedParty = false;
	public bool AttendingParty = false;
	public bool ChatInSocietyParty;
	public SocietyPartyHost selectedSocietyParty;
	public GameObject CreateButton;


	void Awake ()
	{
		this.Reload ();
	}
	// Use this for initialization
	void Start ()
	{
	
	}

	public void JoinParty ()
	{
		CreateSocietyPartyList (AllSocietyParty, AllSocietyPartyContainer);
	}

	public void CreateSocietyPartyList (List<SocietyPartyHost> SocietyParty, GameObject Flatcontainer)
	{
		DeleteGameObjects (Flatcontainer);
		AllFlatPartyParent.SetActive (true);
		if (Flatcontainer == AllSocietyPartyContainer) {	

			if (SocietyParty.Count == 0)
				CreateButton.SetActive (true);
			else
				CreateButton.SetActive (false);			
		} 
		SocietyParty.ForEach (AllSocietyParty => {
			if (AllSocietyParty.PartyEndTime > DateTime.Now) {
				GameObject Go = GameObject.Instantiate (SocieryPartyPrefabsList, Flatcontainer.transform)as GameObject;
				Go.transform.localPosition = Vector3.zero;
				Go.transform.localScale = Vector3.one;
				Go.GetComponent <SocietyPartyUI> ().mySocietyParty = AllSocietyParty;
				Go.GetComponent <SocietyPartyUI> ().Start ();
				Go.GetComponent <SocietyPartyUI> ().OpenSocietyParty ();
				IndicationManager.Instance.IncrementIndicationFor ("Society", 6);

			} else if (AllSocietyParty.PartyEndTime < DateTime.Now && AllSocietyParty.PartyCoolDown > DateTime.Now) {
				print ("Society Party Has ended ");		
			} 	
		});
	}

	public void CreateSocietyPartyListIfCoolDown (List<SocietyPartyHost> SocietyParty, GameObject Flatcontainer)
	{
		DeleteGameObjects (Flatcontainer);
		AllFlatPartyParent.SetActive (true);
		if (Flatcontainer == AllSocietyPartyContainer) {	

			if (SocietyParty.Count == 0)
				CreateButton.SetActive (true);
			else
				CreateButton.SetActive (false);			
		} 
		SocietyParty.ForEach (AllSocietyParty => {
			GameObject Go = GameObject.Instantiate (SocieryPartyPrefabsList, Flatcontainer.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.GetComponent <SocietyPartyUI> ().mySocietyParty = AllSocietyParty;
			Go.GetComponent<SocietyPartyUI> ().StartTimerIfEnded ();
		});
	}

	public void OnSocietyPartyLeaveFunction ()
	{		
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		selectedSocietyParty = null;		

		Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
		Button.AllButtons [6].gameObject.SetActive (true);
		HostPartyManager.Instance.GameGUIControle (true);
		FlatPartyHostingControler.Instance.ScreenCanMove = false;
		ScreenAndPopupCall.Instance.CloseScreen ();
		SocietyPartyManager.Instance.AttendingParty = false;
		if (GameObject.Find ("SocietyPartyRoom"))
			Destroy (GameObject.Find ("SocietyPartyRoom").gameObject);
		ScreenManager.Instance.SocietyPartyScreen.transform.FindChild ("Name").GetChild (0).GetComponent<Text> ().text = "";
		ScreenManager.Instance.SocietyPartyScreen.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = "";
		MultiplayerManager.Instance.LeavRoomForFlatParty ();
		MultiplayerManager.Instance.MoveOutOfPublicArea ();
		//		ScreenAndPopupCall.Instance.MoveOutOfPublicArea ();
		//		MultiplayerManager.Instance._societyParty = false;
	}

	public void LeaveSocietyPartyButtom ()
	{
		ShowPartyLeaveConfirmationForSociety ("Are you sure you want to leave this society party?");
	}

	void SocietyLeaveFunction ()
	{
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		selectedSocietyParty = null;		

		Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
		Button.AllButtons [6].gameObject.SetActive (true);
		HostPartyManager.Instance.GameGUIControle (true);
		FlatPartyHostingControler.Instance.ScreenCanMove = false;
		ScreenAndPopupCall.Instance.CloseScreen ();
		SocietyPartyManager.Instance.AttendingParty = false;
		if (GameObject.Find ("SocietyPartyRoom"))
			Destroy (GameObject.Find ("SocietyPartyRoom").gameObject);
		ScreenManager.Instance.SocietyPartyScreen.transform.FindChild ("Name").GetChild (0).GetComponent<Text> ().text = "";
		ScreenManager.Instance.SocietyPartyScreen.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = "";
		MultiplayerManager.Instance.LeavRoomForFlatParty ();
		MultiplayerManager.Instance.MoveOutOfPublicArea ();
		//		ScreenAndPopupCall.Instance.MoveOutOfPublicArea ();
		//		MultiplayerManager.Instance._societyParty = false;
	}

	public void ShowPartyLeaveConfirmationForSociety (string message)
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
			SocietyLeaveFunction ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
	}


	public void DeleteGameObjects (GameObject container)
	{
		for (int i = 0; i < container.transform.childCount; i++) {
			GameObject.Destroy (container.transform.GetChild (i).gameObject);
		}
	}

	public void SpwanRealPlayerForSocietyParty (GameObject obj)
	{
		if (SocietyPartyManager.Instance.selectedSocietyParty.PresentMembers <= SocietyPartyManager.Instance.selectedSocietyParty.TotleMember) {
			GameObject go = null;
			if (GameManager.GetGender () == GenderEnum.Male) {
				go = PhotonNetwork.Instantiate ("MaleCharacter ForSociety", obj.transform.FindChild ("grid Container").transform.GetChild (UnityEngine.Random.Range (0, 49)).position, Quaternion.identity, 0);
			} else {
				go = PhotonNetwork.Instantiate ("FemaleCharacter ForSociety", obj.transform.FindChild ("grid Container").transform.GetChild (UnityEngine.Random.Range (0, 49)).position, Quaternion.identity, 0);

            }
			go.transform.localScale = new Vector3 (0.4f, 0.4f, 1);
			Vector3 Pos = go.transform.localPosition;
			Pos.z = -1f;
			go.transform.localPosition = Pos;
			var target = GameManager.Instance.SocietyPartyWayPoints [UnityEngine.Random.Range (0, GameManager.Instance.SocietyPartyWayPoints.Count)];

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
			//Achivement Count

			if (!PlayerPrefs.HasKey ("IsSocietyAttended")) {
				PlayerPrefs.SetInt ("IsSocietyAttended", SocietyPartyManager.Instance.selectedSocietyParty.PartyId + PlayerPrefs.GetInt ("PlayerId"));
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("attendSocietyParties");
			} else if (PlayerPrefs.GetInt ("IsSocietyAttended") != SocietyPartyManager.Instance.selectedSocietyParty.PartyId + PlayerPrefs.GetInt ("PlayerId")) {
				PlayerPrefs.SetInt ("IsSocietyAttended", SocietyPartyManager.Instance.selectedSocietyParty.PartyId + PlayerPrefs.GetInt ("PlayerId"));
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("attendSocietyParties");
			}
		}
	}

	public void GetAllSocietyParty ()
	{
		AllSocietyParty.Clear ();
		selectedSocietyParty = null;
		StartCoroutine (IGetAllSocietyParty (SocietyManager.Instance.SelectedSociety.Id));		

	}

	public IEnumerator IGetSocietyPartyForCheck (int SocietyId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
		///ToDO
		jsonElement ["data_type"] = "view";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (GetSocietyPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			//TODO
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Society party data has following")) {

				JSONNode data = _jsnode ["data"];
				var SocietyPartys = new List<SocietyPartyHost> ();

				int partyId = 0;
				int.TryParse (data ["party_id"], out partyId);

				int plyrId = 0;
				int.TryParse (data ["player_id"], out plyrId);

				int socityId = 0;
				int.TryParse (data ["society_id"], out socityId);

				string Partyname = data ["party_name"];
				string Partydescription = data ["party_desc"];

				int max_no_of_guests = 0;
				int.TryParse (data ["max_no_of_guests"], out max_no_of_guests);

				DateTime TimeEnd = Convert.ToDateTime (data ["end_time_of_party"]);
				DateTime CoolDownTime = Convert.ToDateTime (data ["cooldown_end_time"]);
				DateTime CreatedTime = Convert.ToDateTime (data ["party_created_time"]);

				int no_of_present_member = 0;
				int.TryParse (data ["present_members"], out no_of_present_member);  

				int roomIndex = 0;
				int.TryParse (data ["room_index"], out roomIndex);

				//              CreateSocietyPartyList (AllSocietyParty, AllSocietyPartyContainer);

				/// Check Party Status
				if (TimeEnd > DateTime.Now) {                   
            
					yield return true;
				} else if (TimeEnd < DateTime.Now && CoolDownTime > DateTime.Now) {
              
					yield return false;
				} else if (CoolDownTime < DateTime.Now) {                   
            
					yield return false;
				}

			} else if (_jsnode ["status"].ToString ().Contains ("400") || _jsnode ["description"].ToString ().Contains ("No society party is avajlable in our database")) {
                    
				yield return false;
			}
		}
	}


	public IEnumerator IGetAllSocietyParty (int SocietyId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
		///ToDO
		jsonElement ["data_type"] = "view";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (GetSocietyPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			//TODO
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Society party data has following")) {

				JSONNode data = _jsnode ["data"];
				var SocietyPartys = new List<SocietyPartyHost> ();
				 
				int partyId = 0;
				int.TryParse (data ["party_id"], out partyId);

				int plyrId = 0;
				int.TryParse (data ["player_id"], out plyrId);

				int socityId = 0;
				int.TryParse (data ["society_id"], out socityId);

				string Partyname = data ["party_name"];
				string Partydescription = data ["party_desc"];
				
				int max_no_of_guests = 0;
				int.TryParse (data ["max_no_of_guests"], out max_no_of_guests);

				DateTime TimeEnd = Convert.ToDateTime (data ["end_time_of_party"]);
				DateTime CoolDownTime = Convert.ToDateTime (data ["cooldown_end_time"]);
				DateTime CreatedTime = Convert.ToDateTime (data ["party_created_time"]);

				int no_of_present_member = 0;
				int.TryParse (data ["present_members"], out no_of_present_member);	

				int roomIndex = 0;
				int.TryParse (data ["room_index"], out roomIndex);
				SocietyPartyHost Sociertyparty = new SocietyPartyHost (plyrId, socityId, partyId, Partyname, Partydescription, max_no_of_guests, TimeEnd, CoolDownTime, no_of_present_member, roomIndex, CreatedTime);

				SocietyPartys.Add (Sociertyparty);
				AllSocietyParty = SocietyPartys;
//				CreateSocietyPartyList (AllSocietyParty, AllSocietyPartyContainer);

				/// Check Party Status
				if (TimeEnd > DateTime.Now) {					
					SocietyDescriptionController.Instance.HostPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.MHostPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.CoolDownButton.SetActive (false);
					SocietyDescriptionController.Instance.MCoolDownButton.SetActive (false);
					SocietyDescriptionController.Instance.JoinPartyButton.SetActive (true);
					SocietyDescriptionController.Instance.MJoinPartyButton.SetActive (true);
					SocietyDescriptionController.Instance.JoinPartyButton.GetComponent<Button> ().interactable = true;
					SocietyDescriptionController.Instance.MJoinPartyButton.GetComponent<Button> ().interactable = true;


					MultiplayerManager.Instance.ConnectToServerforSocietyParty ();
					yield return true;
				} else if (TimeEnd < DateTime.Now && CoolDownTime > DateTime.Now) {
					CreateSocietyPartyListIfCoolDown (AllSocietyParty, AllSocietyPartyContainer);
					SocietyPartyControler.Instance.myPartyEnding = true;
					SocietyPartyManager.Instance.selectedSocietyParty = Sociertyparty;
					SocietyPartyControler.Instance.SocietyPartyHostCoolDown ();
					SocietyDescriptionController.Instance.CoolDownButton.SetActive (true);
					SocietyDescriptionController.Instance.MCoolDownButton.SetActive (true);
					SocietyDescriptionController.Instance.JoinPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.MJoinPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.HostPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.MHostPartyButton.SetActive (false);
					yield return true;
				} else if (CoolDownTime < DateTime.Now) {					
					StartCoroutine (IDeletSocietyParty (partyId));
					SocietyDescriptionController.Instance.CoolDownButton.SetActive (false);
					SocietyDescriptionController.Instance.MCoolDownButton.SetActive (false);
					SocietyDescriptionController.Instance.JoinPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.MJoinPartyButton.SetActive (false);

					if (SocietyManager.Instance.myRole == 0) {
						SocietyDescriptionController.Instance.HostPartyButton.SetActive (true);
						SocietyDescriptionController.Instance.MHostPartyButton.SetActive (true);
					} else if (SocietyManager.Instance.myRole == 1) {
						SocietyDescriptionController.Instance.HostPartyButton.SetActive (true);
						SocietyDescriptionController.Instance.MHostPartyButton.SetActive (true);
					} else if (SocietyManager.Instance.myRole == 2) {
						SocietyDescriptionController.Instance.HostPartyButton.SetActive (false);
						SocietyDescriptionController.Instance.MHostPartyButton.SetActive (false);
						SocietyDescriptionController.Instance.JoinPartyButton.SetActive (true);
						SocietyDescriptionController.Instance.MJoinPartyButton.SetActive (true);
						SocietyDescriptionController.Instance.JoinPartyButton.GetComponent<Button> ().interactable = false;
						SocietyDescriptionController.Instance.MJoinPartyButton.GetComponent<Button> ().interactable = false;
					}
					yield return false;
				}

			} else if (_jsnode ["status"].ToString ().Contains ("400") || _jsnode ["description"].ToString ().Contains ("No society party is avajlable in our database")) {
				CreateButton.SetActive (true);
				SocietyDescriptionController.Instance.CoolDownButton.SetActive (false);
				SocietyDescriptionController.Instance.MCoolDownButton.SetActive (false);
				SocietyDescriptionController.Instance.JoinPartyButton.SetActive (false);
				SocietyDescriptionController.Instance.MJoinPartyButton.SetActive (false);
				if (SocietyManager.Instance.myRole == 0) {
					SocietyDescriptionController.Instance.HostPartyButton.SetActive (true);
					SocietyDescriptionController.Instance.MHostPartyButton.SetActive (true);
				} else if (SocietyManager.Instance.myRole == 1) {
					SocietyDescriptionController.Instance.HostPartyButton.SetActive (true);
					SocietyDescriptionController.Instance.MHostPartyButton.SetActive (true);
				} else if (SocietyManager.Instance.myRole == 2) {
					SocietyDescriptionController.Instance.HostPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.MHostPartyButton.SetActive (false);
					SocietyDescriptionController.Instance.JoinPartyButton.SetActive (true);
					SocietyDescriptionController.Instance.MJoinPartyButton.SetActive (true);
					SocietyDescriptionController.Instance.JoinPartyButton.GetComponent<Button> ().interactable = false;
					SocietyDescriptionController.Instance.MJoinPartyButton.GetComponent<Button> ().interactable = false;
				}           
				yield return false;
			}
		}
	}

	public IEnumerator IDeletSocietyParty (int partyId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
		///ToDO
		jsonElement ["data_type"] = "delete";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyManager.Instance.SelectedSociety.Id.ToString ();
		jsonElement ["party_id"] = partyId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (GetSocietyPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			//TODO
			if (_jsnode ["status"].ToString ().Contains ("200") || _jsnode ["description"].ToString ().Contains ("Society party has deleted successfully")) {
				print ("Society Party Deleted");			
			} 	
		}
	}

	public void PartyOnCoolDownMsg ()
	{
		ShowWarningMesg ("Society party hosting is on cooldown. Please come back after some time!");
		
	}

	public void ShowWarningMesg (string msg)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
	}

	public void ConnectToChatInSocietyParty ()
	{
		ChatManager.Instance.ChatConnected = false;
		if (!ChatManager.Instance.ChatConnected) {
			MultiplayerManager.Instance.WaitingScreenState (true);
			ChatManager.Instance.AddChannelToConnect ("Society" + selectedSocietyParty.Name);
			ChatManager.Instance.ConnectToChatServer ();
			ChatInSocietyParty = true;

		} else {
			ScreenAndPopupCall.Instance.ChatScreenInSocietyPartyStatus ();
		}
	}

	public void SendMessageInSocietyParty (InputField inputField)
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

	[Serializable]
	public class SocietyPartyHost
	{
		public int PlayerId;
		public int SocietyId;
		public int PartyId;
		public string Name;
		public string Description;
		public int TotleMember;
		public DateTime PartyEndTime;
		public DateTime PartyCoolDown;
		public int PresentMembers;
		public int RoomIndex;
		public DateTime PartyCreatedTime;

		public SocietyPartyHost (int playerId, int societyId, int partyid, string name, string descrptn, int totleMember, DateTime partyEndTime, DateTime pCoolDown, int presentMembers, int roomIndx, DateTime partyCreated)
		{	
			PlayerId = playerId;
			SocietyId = societyId;
			PartyId = partyid;
			Name = name;
			Description = descrptn;
			TotleMember = totleMember;
			PartyEndTime = partyEndTime;
			PartyCoolDown = pCoolDown;
			PresentMembers = presentMembers;
			RoomIndex = roomIndx;
			PartyCreatedTime = partyCreated;
		}
	}

	[Serializable]
	public class SocietyKickedOutPlayerList
	{
		public int PlayerId;
		public int PartyId;
		public int partyType;

		public SocietyKickedOutPlayerList (int playerId, int id, int ptype)
		{	
			PlayerId = playerId;
			PartyId = id;
			partyType = ptype;
		}
	}
}
