using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Simple_JSON;

public class SocietyPartyControler : Singleton<SocietyPartyControler>
{
	public const string HostSocietyPartyLink = "http://pinekix.ignivastaging.com/societyParties/party";
	[Header (" Host party atribute")]
	public int PartyId;
	public InputField SocietyPartyName;
	public InputField SocietyPartyDiscription;
	public Dropdown PartyTimeDropDown;
	public Dropdown TotelGuestDropDown;
	public float ScietyPartyTime;
	public int SocietyTotelGuest;
	public bool isPaid = false;
	public bool PartyHosted;
	public float HostPartyCoolDown;
	float HostPartyCoolDownreset = 180f;


	public DateTime SocietyPartyCoolDownEnd;
	public Text CooldownText;
	public GameObject CooldownShow;
	public bool ScreenCanMove = false;
	public bool TimeExtended;
	SocietyPartyManager.SocietyPartyHost tempScietyParty;

	[Header ("Other party atribute")]
	public string SocietypartyName;
	public string SocietypartyDiscription;
	public bool myPartyEnding = false;
	public GameObject TimeExtendPanel;

	void Awake ()
	{
		Reload ();
	}
	// Use this for initialization
	void Start ()
	{
	
	}

	public void SubmitSocietyPartyName ()
	{
		SocietypartyName = SocietyPartyName.text;
	}

	public void SubmitSocietyDiscription ()
	{
		SocietypartyDiscription = SocietyPartyDiscription.text;
	}

	public void SocietyPartyTimeSelection ()
	{		
		if (PartyTimeDropDown.captionText.text.Contains ("3")) {
			ScietyPartyTime = 3 * 60;
		} else if (PartyTimeDropDown.captionText.text.Contains ("8")) {
			ScietyPartyTime = 8 * 60;
		} else if (PartyTimeDropDown.captionText.text.Contains ("15")) {
			ScietyPartyTime = 15 * 60;
		} else if (PartyTimeDropDown.captionText.text.Contains ("25")) {
			ScietyPartyTime = 25 * 60;
		} else if (PartyTimeDropDown.captionText.text == "Select") {
			ScietyPartyTime = 0;
		}
	}

	public void SocietyPartyMembersSelection ()
	{		
		if (TotelGuestDropDown.captionText.text == "3") {
			SocietyTotelGuest = 3;
		} else if (TotelGuestDropDown.captionText.text == "8") {
			SocietyTotelGuest = 8;
		} else if (TotelGuestDropDown.captionText.text == "15") {
			SocietyTotelGuest = 15;
		} else if (TotelGuestDropDown.captionText.text == "25") {
			SocietyTotelGuest = 25;
		} else if (TotelGuestDropDown.captionText.text == "Select") {
			SocietyTotelGuest = 0;
		}
	}

	public void SocietyHostPartySubmit ()
	{
		if (!PartyHosted) {
			if (SocietypartyName != "" && SocietypartyDiscription != "" && ScietyPartyTime >= 3 && SocietyTotelGuest >= 3) {
				FlatPartyHostingFee ();
			} else
				ErrorMessageShow ();
		} else {
			ShowLessAmmountMesg ("Your society party is on cooldown till " + CooldownText.text);
		}
	}

	public IEnumerator IeSocietyHostPartySubmit ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "register";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyManager.Instance.SelectedSociety.Id.ToString ();
		jsonElement ["party_name"] = SocietypartyName;
		jsonElement ["party_desc"] = SocietypartyDiscription;
		jsonElement ["max_no_of_guests"] = SocietyTotelGuest.ToString ();

		var temEndTime = DateTime.Now.AddSeconds (ScietyPartyTime);
		DateTime EndTime = temEndTime;

		print (EndTime);
		jsonElement ["end_time_of_party"] = EndTime.ToString ();

		var tempCoolDownTime = EndTime.AddSeconds (HostPartyCoolDown);
		DateTime CoolDownTime = tempCoolDownTime;

		jsonElement ["cooldown_end_time"] = CoolDownTime.ToString ();
	
		jsonElement ["present_members"] = "0";
		jsonElement ["room_index"] = SocietyManager.Instance.SelectedSociety.RoomType.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (HostSocietyPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (isPaid) {
				if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Your society party has created successfully.")) {
					JSONNode data = _jsnode ["data"];
					int partyId = 0;
					int.TryParse (data ["party_id"], out partyId);
					PartyId = partyId;
					PlayerPrefs.SetInt ("SocietyPartyId", PartyId);
					OnMyFlatPartySubmited (CoolDownTime);
					///TODO
					SocietyPartyManager.Instance.GetAllSocietyParty ();
					StartCoroutine (CreatePartyOnScreen ());

					yield return StartCoroutine (SocietyManager.Instance.IGetAllPlayers (SocietyManager.Instance._mySociety.Id));
					List<int> Allmemebers = new List<int> ();
					SocietyManager.Instance._allMembers.ForEach (mem => {
						if (mem.player_id != PlayerPrefs.GetInt ("PlayerId"))
							Allmemebers.Add (mem.player_id);
					});
					IndicationManager.Instance.SendIndicationToUsers (Allmemebers.ToArray (), "Society", SocietyManager.Instance._mySociety.Id);
				}
			}

		}

	}

	IEnumerator CreatePartyOnScreen ()
	{
		yield return new WaitForSeconds (2f);
		SocietyPartyManager.Instance.JoinParty ();
	}

	public void FlatPartyHostingFee ()
	{
		if (SocietyTotelGuest == 3) {
			ComfrimSocietyPartySubmited ("Are you sure you want to host free party ", "");

		} else if (SocietyTotelGuest == 8) {
			if (PlayerPrefs.GetInt ("Money") >= 500 && PlayerPrefs.GetInt ("Gems") >= 10) {
				ComfrimSocietyPartySubmited ("Are you sure you want to pay 500 coins & ", "10 gems to ");

			} else {
				ShowLessAmmountMesg ("You need atleast 500 coins & 10 Gems");
				isPaid = false;
			}

		} else if (SocietyTotelGuest == 15) {
			if (PlayerPrefs.GetInt ("Money") >= 800 && PlayerPrefs.GetInt ("Gems") >= 30) {
				ComfrimSocietyPartySubmited ("Are you sure you want to pay 800 coins & ", "30 gems to ");

			} else {
				ShowLessAmmountMesg ("You need atleast 800 coins & 30 Gems");
				isPaid = false;
			}
		} else if (SocietyTotelGuest == 25) {
			if (VipSubscriptionManager.Instance.VipSubscribed) {
				if (PlayerPrefs.GetInt ("Money") >= 1000 && PlayerPrefs.GetInt ("Gems") >= 50) {
					ComfrimSocietyPartySubmited ("Are you sure you want to pay 1000 coins & ", "50 gems to ");
				} else {
					ShowLessAmmountMesg ("You need atleast 1000 coins and 50 gems");	
				}
			} else {
				ShowLessAmmountMesg ("You must have VIP subscription, 1000 coins and 50 gems");	
				isPaid = false;
			}
		}
	}

	public void OnMyFlatPartySubmited (DateTime CoolDown)
	{
		PartyHosted = true;
		SocietyPartyManager.Instance.AttendingParty = true;
		clearFeild ();
		var coolDowntimeDiff = CoolDown - DateTime.Now;
		float TimeRemain = (float)coolDowntimeDiff.TotalSeconds;
		PlayerPrefs.SetFloat ("SocietyPartyCoolDownTime", TimeRemain);
		// update status for society party end 
	}

	public void clearFeild ()
	{
		SocietypartyName = "";
		SocietyPartyName.text = "";
		SocietypartyDiscription = "";
		SocietyPartyDiscription.text = "";
		PartyTimeDropDown.captionText.text = "Select";
		PartyTimeDropDown.value = 0;
		TotelGuestDropDown.captionText.text = "Select";
		TotelGuestDropDown.value = 0;
	}



	public void ComfrimSocietyPartySubmited (string CoinMsg, string GemsMsg)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = CoinMsg + GemsMsg + SocietypartyName + " society party?";	
		if (SocietyTotelGuest == 3) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				isPaid = true;
				StartCoroutine (IeSocietyHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();
			});
		} else if (SocietyTotelGuest == 8) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				GameManager.Instance.SubtractCoins (500);
				GameManager.Instance.SubtractGems (10);
				isPaid = true;
				StartCoroutine (IeSocietyHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();
			});

		} else if (SocietyTotelGuest == 15) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				GameManager.Instance.SubtractCoins (800);
				GameManager.Instance.SubtractGems (30);
				isPaid = true;
				StartCoroutine (IeSocietyHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();
			});

		} else if (SocietyTotelGuest == 25) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 					
				GameManager.Instance.SubtractCoins (1000);
				GameManager.Instance.SubtractGems (50);
				isPaid = true;
				StartCoroutine (IeSocietyHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();
			});

		}


		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}


	public void ErrorMessageShow ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Please fill all the required details!";	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}

	public void ShowLessAmmountMesg (string msg)
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

	public void  SocietyPartyHostCoolDown ()
	{
		SocietyDescriptionController.Instance.CoolDownButton.SetActive (true);
		SocietyDescriptionController.Instance.MCoolDownButton.SetActive (true);
		SocietyDescriptionController.Instance.JoinPartyButton.SetActive (false);
		SocietyDescriptionController.Instance.MJoinPartyButton.SetActive (false);
		SocietyDescriptionController.Instance.HostPartyButton.SetActive (false);
		SocietyDescriptionController.Instance.MHostPartyButton.SetActive (false);
		if (myPartyEnding) {
			SocietyPartyCoolDownEnd = SocietyPartyManager.Instance.selectedSocietyParty.PartyCoolDown;
			print (SocietyPartyCoolDownEnd);
			PlayerPrefs.SetString ("SocietyPartyCooldownTime", SocietyPartyCoolDownEnd.ToBinary ().ToString ());
			MultiplayerManager.Instance.LeavRoomForFlatParty ();
			MultiplayerManager.Instance._societyParty = false;
		}
	}

	public void SocietyPartyCoolDownIfEnded (float time)
	{
		if (myPartyEnding) {
			SocietyPartyCoolDownEnd = SocietyPartyManager.Instance.selectedSocietyParty.PartyCoolDown;
			PlayerPrefs.SetString ("SocietyPartyCooldownTime", SocietyPartyCoolDownEnd.ToBinary ().ToString ());
		}
	}

	void Update ()
	{
		if (SocietyPartyCoolDownEnd > DateTime.Now) {
			var Diff = SocietyPartyCoolDownEnd - DateTime.Now;
			float TimeRemain = (float)Diff.TotalSeconds;
			HostPartyCoolDown = TimeRemain;
			CooldownText.text = ExtensionMethods.GetTimeStringFromFloat (HostPartyCoolDown);
			SocietyDescriptionController.Instance.CoolDownButton.transform.GetChild (0).GetComponent<Text> ().text = ExtensionMethods.GetTimeStringFromFloat (HostPartyCoolDown);
			SocietyDescriptionController.Instance.MCoolDownButton.transform.GetChild (0).GetComponent<Text> ().text = ExtensionMethods.GetTimeStringFromFloat (HostPartyCoolDown);
			if (HostPartyCoolDown < 0.5f) {
				if (HostPartyCoolDown < 0.05f) {
					StartCoroutine (SocietyPartyManager.Instance.IDeletSocietyParty (PlayerPrefs.GetInt ("SocietyPartyId")));
					HostPartyCoolDown = HostPartyCoolDownreset;
				}
				
				PlayerPrefs.DeleteKey ("SocietyPartyCooldownTime");			
				PartyHosted = false;
				isPaid = false;
				TimeExtended = false;
				FlatPartyHostingControler.Instance.CooldownShow.SetActive (false);
				CooldownText.text = "";			

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
			
				return;
			}
		}
	}

}
