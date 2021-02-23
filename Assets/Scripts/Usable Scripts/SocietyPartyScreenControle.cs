using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using Simple_JSON;

public class SocietyPartyScreenControle : Singleton<SocietyPartyScreenControle>
{

	public Text PartyName;
	public Text PartyDiscription;
	public Text PartyTime;
	public Text Members;
	public float partyEndTimeInscreen;
	public int TimeAdd;

	public float TotelTimeCalculate;
	public bool Show;
	public bool FirstMsgSend = false;
	public bool SecondMsgSend = false;
	public bool ThirdMsgSend = false;
	public Text ChatBagdeCountText;
	public Sprite[] ChatButton;

	void Awake ()
	{
		this.Reload ();
	}
	// Use this for initialization
	void Start ()
	{
		PartyName.text = SocietyPartyManager.Instance.selectedSocietyParty.Name;
		Members.text = PhotonNetwork.countOfPlayersInRooms + "/" + SocietyPartyManager.Instance.selectedSocietyParty.TotleMember;
		PartyDiscription.text = SocietyPartyManager.Instance.selectedSocietyParty.Description;
	}

	public void SetTotelTime ()
	{
		///Compair end time and created time to show the time duration
		var Diff = SocietyPartyManager.Instance.selectedSocietyParty.PartyEndTime - SocietyPartyManager.Instance.selectedSocietyParty.PartyCreatedTime;
		float TimeRemain = (float)Diff.TotalSeconds;
		TotelTimeCalculate = TimeRemain;
		ScreenManager.Instance.SocietyPartyChatScreen.SetActive (true);
		if (ScreenAndPopupCall.Instance.ChatInSocietyPartyIsShown)
			ScreenAndPopupCall.Instance.ChatScreenInSocietyPartyStatus ();
		ScreenAndPopupCall.Instance.ChatInSocietyPartyIsShown = false;
		SocietyPartyManager.Instance.ConnectToChatInSocietyParty ();
//		if (GameObject.Find ("SocietyPartyRoom"))
//			Destroy (GameObject.Find ("SocietyPartyRoom"));
		FirstMsgSend = false;
		SecondMsgSend = false;
		ThirdMsgSend = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (SocietyPartyManager.Instance.selectedSocietyParty == null)
			return;

		if (SocietyPartyManager.Instance.selectedSocietyParty.PartyEndTime.AddSeconds (TimeAdd) > DateTime.Now) {
			var Diff = SocietyPartyManager.Instance.selectedSocietyParty.PartyEndTime - DateTime.Now;
			float TimeRemain = (float)Diff.TotalSeconds;
			TimeRemain = Mathf.Round (TimeRemain);
			partyEndTimeInscreen = TimeRemain;
			PartyDiscription.text = SocietyPartyManager.Instance.selectedSocietyParty.Description;
			PartyTime.text = ExtensionMethods.GetTimeStringFromFloat (partyEndTimeInscreen);
			PartyName.text = SocietyPartyManager.Instance.selectedSocietyParty.Name;
			var Count = PhotonNetwork.playerList.Length;
			Members.text = Count + "/" + SocietyPartyManager.Instance.selectedSocietyParty.TotleMember;

			/// To send the status of time remaing trhough chat
			if (SocietyPartyManager.Instance.selectedSocietyParty.PlayerId == PlayerPrefs.GetInt ("PlayerId"))
				FlatPartyTimeRemaingMsg ();
			
			if (partyEndTimeInscreen < 0.5f) {
				FlatPartyHostingControler.Instance.ScreenCanMove = false;						

				if (SocietyPartyManager.Instance.AttendingParty) {
					ScreenAndPopupCall.Instance.CloseScreen ();
					///TODO
					SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
					MultiplayerManager.Instance._societyParty = false;
					if (GameObject.Find ("FlatPartyPublicArea"))
						Destroy (GameObject.Find ("FlatPartyPublicArea"));
					SocietyPartyManager.Instance.AttendingParty = false;
				}
				return;
			}
		}
	}


	public void FlatPartyTimeRemaingMsg ()
	{
		float FirstWarning = TotelTimeCalculate / 2f; // send msg when party deuration is remaing of 50%
		float SecondWarning = TotelTimeCalculate / 5f;
		float ThirdWarning = TotelTimeCalculate / 10f;
		FirstWarning = Mathf.Round (FirstWarning);
		SecondWarning = Mathf.Round (SecondWarning);
		ThirdWarning = Mathf.Round (ThirdWarning);

		if (partyEndTimeInscreen == FirstWarning) {
			Show = true;
			if (Show && !FirstMsgSend) {
				count = 1;
				SendThisMessage (FirstWarning, SecondWarning, ThirdWarning);
			}
		} else if (partyEndTimeInscreen == SecondWarning) {
			Show = true;
			if (Show && !SecondMsgSend) {
				count = 2; 
				SendThisMessage (FirstWarning, SecondWarning, ThirdWarning);
			}
		} else if (partyEndTimeInscreen == ThirdWarning) {
			Show = true;
			if (Show && !ThirdMsgSend) {
				count = 3;
				SendThisMessage (FirstWarning, SecondWarning, ThirdWarning);
			}
		}		
	}

	int count = 0;

	public void SendThisMessage (float fisrt, float second, float third)
	{
		print (count.ToString ());
		if (Show && count == 1) {
			//			SocietyDescriptionController.Instance.ChatScreenStatus ();
			if (ChatManager.Instance.ChatConnected) {
				SendMessageInFlatParty ("Your " + SocietyPartyManager.Instance.selectedSocietyParty.Name + " society party end time remaining 50%");
				Show = false;
				FirstMsgSend = true;
			}		
		} else if (Show && count == 2) {
			if (ChatManager.Instance.ChatConnected) {
				SendMessageInFlatParty ("Your " + SocietyPartyManager.Instance.selectedSocietyParty.Name + " society party end time remaining 20%");
				Show = false;
				SecondMsgSend = true;
			}

		} else if (Show && count == 3) {
			if (ChatManager.Instance.ChatConnected) {
				SendMessageInFlatParty ("Your " + SocietyPartyManager.Instance.selectedSocietyParty.Name + " society party end time remaining 10%");
				Show = false;
				ThirdMsgSend = true;
				count = 0;
			}
		}
	}

	public void SendMessageInFlatParty (string msg)
	{
		if (!string.IsNullOrEmpty (msg.ToString ()) && !Regex.IsMatch (msg.ToString (), "^[ \t\r\n\u0200]*$")) {
			var Json = new JSONClass ();
			Json ["message"] = msg.ToString ();
			Json ["time"] = DateTime.UtcNow.ToBinary ().ToString ();
			string message = Json.ToString ();
			ChatManager.Instance.SendChatMessage (message);
		}
		msg = "";

	}

	public void TimeExtendForFlatParty ()
	{
		if (HostPartyManager.Instance.selectedFlatParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) {
			if (!FlatPartyHostingControler.Instance.TimeExtended) {
				if (partyEndTimeInscreen >= 899f && partyEndTimeInscreen <= 900f) {
//					FlatPartyHostingControler.Instance.TimeExtendAlart ("Do you want to extend flat party time?");
				} else if (partyEndTimeInscreen >= 149f && partyEndTimeInscreen <= 150f) {
//					FlatPartyHostingControler.Instance.TimeExtendAlart ("Do you want to extend flat party time?");

				}
			}
		}
	}
}
