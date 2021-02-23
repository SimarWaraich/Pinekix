using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Simple_JSON;
using System.Text.RegularExpressions;

public class FlatPartyScreenControle : Singleton<FlatPartyScreenControle>
{

	public Text PartyName;
	public Text PartyDiscription;
	public Text PartyTime;
	public Text Members;
	public float partyEndTimeInscreen;
	public int ExtraTime;
	float TotleTimeofParty;
	public float TotelTimeCalculate;
	public bool Show;
	public bool FirstMsgSend = false;
	public bool SecondMsgSend = false;
	public bool ThirdMsgSend = false;
	public Text ChatBagdeCountText;
	public Sprite[] ChatButton;
	// Use this for initialization

	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
		PartyName.text = HostPartyManager.Instance.selectedFlatParty.Name;
		Members.text = PhotonNetwork.countOfPlayersInRooms + "/" + HostPartyManager.Instance.selectedFlatParty.TotleMember;
		PartyDiscription.text = HostPartyManager.Instance.selectedFlatParty.Description;
		var Diff = HostPartyManager.Instance.selectedFlatParty.PartyEndTime - DateTime.Now;
		float TimeRemain = (float)Diff.TotalSeconds;
		TotleTimeofParty = TimeRemain;

		print (TotleTimeofParty.ToString ());
	}


	public void SetTotelTime ()
	{
		///Compair end time and created time to show the time duration
		var Diff = HostPartyManager.Instance.selectedFlatParty.PartyEndTime - HostPartyManager.Instance.selectedFlatParty.PartyCreatedTime;
		float TimeRemain = (float)Diff.TotalSeconds;
		TotelTimeCalculate = TimeRemain;
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (Tut.HostPartyCreated)
			ScreenManager.Instance.FlatPartyChatScreen.SetActive (true);
		if (ScreenAndPopupCall.Instance.ChatInFlatPartyIsShown)
			ScreenAndPopupCall.Instance.ChatScreenInFlatPartyStatus ();
		ScreenAndPopupCall.Instance.ChatInFlatPartyIsShown = false;
		HostPartyManager.Instance.ConnectToChatInFlatParty ();
		if (GameObject.Find ("SocietyPartyRoom"))
			Destroy (GameObject.Find ("SocietyPartyRoom").gameObject);
		FirstMsgSend = false;
		SecondMsgSend = false;
		ThirdMsgSend = false;
	}
	// Update is called once per frame
	void Update ()
	{
		if (HostPartyManager.Instance.selectedFlatParty == null)
			return;
		
		if (HostPartyManager.Instance.selectedFlatParty.PartyEndTime.AddSeconds (ExtraTime) > DateTime.Now) {
			var Diff = HostPartyManager.Instance.selectedFlatParty.PartyEndTime - DateTime.Now;
			float TimeRemain = (float)Diff.TotalSeconds;
			TimeRemain = Mathf.Round (TimeRemain);
			partyEndTimeInscreen = TimeRemain;
			PartyDiscription.text = HostPartyManager.Instance.selectedFlatParty.Description;
			PartyTime.text = ExtensionMethods.GetTimeStringFromFloat (partyEndTimeInscreen);
			PartyName.text = HostPartyManager.Instance.selectedFlatParty.Name;
			var Count = PhotonNetwork.playerList.Length;
			ScreenManager.Instance.RunningParty.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = Count + "/" + HostPartyManager.Instance.selectedFlatParty.TotleMember;
			/// To send the status of time remaing trhough chat
			if (HostPartyManager.Instance.selectedFlatParty.PlayerId == PlayerPrefs.GetInt ("PlayerId"))
				FlatPartyTimeRemaingMsg ();
			if (partyEndTimeInscreen < 0.5f) {
				
				HostPartyManager.Instance.CheckPartyStatus ();
				FlatPartyHostingControler.Instance.ScreenCanMove = false;
				var Button = GameManager.Instance.GetComponent<Tutorial> ();			

				Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
				Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
				Button.AllButtons [6].gameObject.SetActive (true);
				HostPartyManager.Instance.GameGUIControle (true);
				if (HostPartyManager.Instance.selectedFlatParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) {
					FlatPartyHostingControler.Instance.myPartyEnding = true;
					FlatPartyHostingControler.Instance.CooldownShow.SetActive (true);
					FlatPartyHostingControler.Instance.PartyHostCoolDown ();
				}
				if (HostPartyManager.Instance.AttendingParty) {
					ScreenAndPopupCall.Instance.CloseScreen ();
					MultiplayerManager.Instance._flatParty = false;
					MultiplayerManager.Instance.LeavRoomForFlatParty ();

					if (GameObject.Find ("FlatPartyPublicArea"))
						Destroy (GameObject.Find ("FlatPartyPublicArea"));
					HostPartyManager.Instance.AttendingParty = false;
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
				SendMessageInFlatParty ("Your " + HostPartyManager.Instance.selectedFlatParty.Name + " flat party end time remaining 50%");
				Show = false;
				FirstMsgSend = true;
			}		
		} else if (Show && count == 2) {
			if (ChatManager.Instance.ChatConnected) {
				SendMessageInFlatParty ("Your " + HostPartyManager.Instance.selectedFlatParty.Name + " flat party end time remaining 20%");
				Show = false;
				SecondMsgSend = true;
			}

		} else if (Show && count == 3) {
			if (ChatManager.Instance.ChatConnected) {
				SendMessageInFlatParty ("Your " + HostPartyManager.Instance.selectedFlatParty.Name + " flat party end time remaining 10%");
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
				bool extendEnable = false;
				if (partyEndTimeInscreen >= 899f && partyEndTimeInscreen <= 900f && !extendEnable) {
					extendEnable = true;
					FlatPartyHostingControler.Instance.TimeExtendAlart ("Do you want to extend flat party time?", extendEnable);
				} else if (partyEndTimeInscreen >= 149f && partyEndTimeInscreen <= 150f && !extendEnable) {
					extendEnable = true;
					FlatPartyHostingControler.Instance.TimeExtendAlart ("Do you want to extend flat party time?", extendEnable);

				}
			}
		}
	}
}
