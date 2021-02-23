using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Simple_JSON;
using System;
using System.Text;

public class FlatPartyHostingControler : Singleton<FlatPartyHostingControler>
{
	public const string HostPartyLink = "http://pinekix.ignivastaging.com/flats/updateFlatParty";
	[Header (" Host party atribute")]
	public InputField HostPartyName;
	public InputField HostPartyDiscription;
	public Dropdown PartyTimeDropDown;
	public Dropdown TotelGuestDropDown;
	public float PartyTime;
	public int TotelGuest;
	public bool PrivateParty;
	public Toggle PartyTypeToggel;
	public Toggle PartyTypePrivate;
	public Text ToggleText;
	public bool isPaid = false;
	public bool PartyHosted;
	public float HostPartyCoolDown;
	//	86400;
	public DateTime HostparyCoolDownEnd;
	public Text CooldownText;
	public GameObject CooldownShow;
	public bool ScreenCanMove = false;
	public bool TimeExtended;
	HostPartyManager.HostParty tempFlatParty;


	[Header ("Other party atribute")]
	public GameObject IconContainerScreen;
	public Image PartyIcon;
	public GameObject IconContainer;
	public GameObject IconPrefeb;
	public string partyName;
	public string partyDiscription;
	public bool myPartyEnding = false;
	public GameObject TimeExtendPanel;

	public List<GameItemPropties> DecorItemPropties;
	public List<GameItemPropties> RoomPropties;

	void Awake ()
	{
		Reload ();
	}

	void Start ()
	{
		TimeExtendPanel.SetActive (false);
		PartyHosted = false;
		isPaid = false;
		ScreenCanMove = false;

		if (PlayerPrefs.HasKey ("HostPartyCooldownTime")) {
			var Temp = Convert.ToInt64 (PlayerPrefs.GetString ("HostPartyCooldownTime"));
			HostparyCoolDownEnd = DateTime.FromBinary (Temp);
			if (HostparyCoolDownEnd > DateTime.Now) {
				PartyHosted = true;
				isPaid = true;
				FlatPartyHostingControler.Instance.CooldownShow.SetActive (true);
			} else {
				PartyHosted = false;
				isPaid = false;
				FlatPartyHostingControler.Instance.CooldownShow.SetActive (false);
				PlayerPrefs.DeleteKey ("HostPartyCooldownTime");
			}
				
		}

//		PartyHostCoolDownIfEnded (float time)
	}


	public void SubmitPartyName (InputField inputfeild)
	{
		partyName = inputfeild.text;

		if (inputfeild.text != "") {
			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 5)
				Tut.HostPartyTutorial ();
		}
		
	}

	public void SubmitDiscription (InputField disFeild)
	{
		partyDiscription = disFeild.text;

		if (disFeild.text != "") {
			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 6)
				Tut.HostPartyTutorial ();
		}
	}

	//	public void PartyType ()
	//	{
	//		if (!PartyTypeToggel.isOn) {
	//
	//			PartyTypeToggel.isOn = false;
	//			PartyTypePrivate.isOn = true;
	//			PrivateParty = PartyTypePrivate.isOn;
	//		}
	
	//	}
	//
	//	public void PartyTypePriv ()
	//	{
	//		if (!PartyTypePrivate.isOn) {
	//			PartyTypeToggel.isOn = true;
	//			PartyTypePrivate.isOn = false;
	//
	//			PrivateParty = PartyTypePrivate.isOn;
	//		}
	//	}


	public void PartyTypeCommon ()
	{
		PartyTypeToggel.isOn = !PartyTypeToggel.isOn;
		PartyTypePrivate.isOn = !PartyTypePrivate.isOn;
		PrivateParty = !PrivateParty;
	}

	public void PartyTimeSelection ()
	{		
		if (PartyTimeDropDown.captionText.text.Contains ("3")) {
			PartyTime = 3 * 60;
		} else if (PartyTimeDropDown.captionText.text.Contains ("8")) {
			PartyTime = 8 * 60;
		} else if (PartyTimeDropDown.captionText.text.Contains ("15")) {
			PartyTime = 15 * 60;
		} else if (PartyTimeDropDown.captionText.text.Contains ("25")) {
			PartyTime = 25 * 60;
		} else if (PartyTimeDropDown.captionText.text == "Select") {
			PartyTime = 0;
		}
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 8)
			Tut.HostPartyTutorial ();
	}

	public void PartyMembersSelection ()
	{		
		if (TotelGuestDropDown.captionText.text == "3") {
			TotelGuest = 3;
		} else if (TotelGuestDropDown.captionText.text == "8") {
			TotelGuest = 8;
		} else if (TotelGuestDropDown.captionText.text == "15") {
			TotelGuest = 15;
		} else if (TotelGuestDropDown.captionText.text == "25") {
			TotelGuest = 25;
		} else if (TotelGuestDropDown.captionText.text == "Select") {
			TotelGuest = 0;
		}
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 7)
			Tut.HostPartyTutorial ();
	}

	public void GetAllIcon ()
	{
		OpenAndCloseIconScreen (true);
		DeleteAllIconFromContainer ();
		for (int i = 0; i < HostPartyManager.Instance.PartyIconList.Count; i++) {
			GameObject obj = GameObject.Instantiate (IconPrefeb, IconContainer.transform)as GameObject;
			obj.GetComponent <Image> ().sprite = HostPartyManager.Instance.PartyIconList [i];
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localScale = Vector3.one;
			obj.gameObject.name = i.ToString ();
		}

	}

	public void DeleteAllIconFromContainer ()
	{
		for (int i = 0; i < IconContainer.transform.childCount; i++) {
			GameObject.Destroy (IconContainer.transform.GetChild (i).gameObject);
		}
	}

	public void OpenAndCloseIconScreen (bool active)
	{		
		IconContainerScreen.gameObject.SetActive (active);
	}

	public void HostPartySubmit ()
	{
		if (!PartyHosted) {
			if (partyName != "" && partyDiscription != "" && PartyTime >= 3 && TotelGuest >= 3) {
				FlatPartyHostingFee ();
			} else
				ErrorMessageShow ();
		} else {
			ShowLessAmmountMesg ("Your host party in on cooldown till " + CooldownText.text);
		}
	}

	public IEnumerator IeHostPartySubmit ()
	{
		DecorItemPropties.Clear ();
		RoomPropties.Clear ();
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["party_name"] = partyName;
		jsonElement ["party_desc"] = partyDiscription;
		jsonElement ["max_no_of_guests"] = TotelGuest.ToString ();
	
//		string eTime = ExtensionMethods.GetTimeStringFromFloat (PartyTime);
//		DateTime EndTime = Convert.ToDateTime (eTime);


		if (PrivateParty)
			jsonElement ["party_type"] = "2";
		else
			jsonElement ["party_type"] = "1";
		var PEndTime = DateTime.Now.AddSeconds (PartyTime);
		DateTime EndTime = PEndTime;

		jsonElement ["party_end_time"] = EndTime.ToString ();
		jsonElement ["no_of_present_member"] = "0";
		jsonElement ["no_of_rooms"] = RoomPurchaseManager.Instance.flats.Count.ToString ();


		for (int i = 0; i < DecorController.Instance.PlacedDecors.Count; i++) {
			
			GameItemPropties tempObj = new GameItemPropties (DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().decorInfo.Id,
				                           "Decor" + "/" + DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().decorInfo.Name.Trim ('/').Trim ('"'),
				                           SerializeVector3Array (DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().transform.position).Trim ('|') +
				                           "/" + DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().direction
			                           );
			DecorItemPropties.Add (tempObj);
		}
		for (int j = 0; j < RoomPurchaseManager.Instance.Addedflats.Count; j++) {
			GameItemPropties tempRomm = new GameItemPropties (0, RoomPurchaseManager.Instance.flats [j].x.ToString () + ":"
			                            + RoomPurchaseManager.Instance.flats [j].y.ToString () + "/" + RoomPurchaseManager.Instance.Addedflats [j].GetComponent<Flat3D> ().WallColourNames + "/" +
			                            RoomPurchaseManager.Instance.Addedflats [j].GetComponent<Flat3D> ().GroundTextureName,
				                            SerializeVector3Array (RoomPurchaseManager.Instance.Addedflats [j].GetComponent<Flat3D> ().transform.position));
			RoomPropties.Add (tempRomm);
		}

		for (int a = 0; a < DecorItemPropties.Count; a++) {
			var jsonarray = new JSONClass ();
			jsonarray.Add ("item_id", DecorItemPropties [a].id.ToString ());
			jsonarray.Add ("item_type", DecorItemPropties [a].types.ToString ());
			jsonarray.Add ("properties", DecorItemPropties [a].propties.ToString ());

			jsonElement ["items"].Add (jsonarray);
		}
	 
//		foreach (var item in DecorItemPropties) {			
//			var jsonarray = new JSONClass ();
//			jsonarray.Add ("item_id", item.id.ToString ());
//			jsonarray.Add ("item_type", item.types.ToString ());
//			jsonarray.Add ("properties", item.propties.ToString ());
//
//			jsonElement ["items"].Add (jsonarray);
//		}

		for (int b = 0; b < RoomPropties.Count; b++) {
			var jsonarray = new JSONClass ();
			jsonarray.Add ("item_id", RoomPropties [b].id.ToString () + "Room");
			jsonarray.Add ("item_type", RoomPropties [b].types.ToString ());
			jsonarray.Add ("properties", RoomPropties [b].propties.ToString ());

			jsonElement ["items"].Add (jsonarray);
		}
//		foreach (var item in RoomPropties) {			
//			var jsonarray = new JSONClass ();
//			jsonarray.Add ("item_id", item.id.ToString () + "Room");
//			jsonarray.Add ("item_type", item.types.ToString ());
//			jsonarray.Add ("properties", item.propties.ToString ());
//
//			jsonElement ["items"].Add (jsonarray);
//		}


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (HostPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print (" flat party jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (isPaid) {
//				if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Your flat party has been updated successfully")) {
//					int HostedPartyId = 0;
//					int.TryParse (_jsnode ["party_id"], out HostedPartyId);
//					PlayerPrefs.SetInt ("HostedPartyId", HostedPartyId);
//					print (HostedPartyId.ToString () + " this is party id");
////					if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
////						AchievementsManager.Instance.CheckAchievementsToUpdate ("hostFlatParties");
//
//					OnMyFlatPartySubmited (EndTime, HostedPartyId);
//				} else 
				if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Your flat party has saved successfully")) {
					int HostedPartyId = 0;
					int.TryParse (_jsnode ["party_id"], out HostedPartyId);
					PlayerPrefs.SetInt ("HostedPartyId", HostedPartyId); 
					print (HostedPartyId.ToString () + " this is party id");
					OnMyFlatPartySubmited (EndTime, HostedPartyId);
					if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
						AchievementsManager.Instance.CheckAchievementsToUpdate ("hostFlatParties");
				} else if (_jsnode == null) {
					print ("Somthing went wrong!!!");
				} 
			}

		}

	}

	public void OfflineTutSubmitHostParty ()
	{
		DecorItemPropties.Clear ();
		RoomPropties.Clear ();
		var PEndTime = DateTime.Now.AddSeconds (PartyTime);
		DateTime EndTime = PEndTime;
		for (int i = 0; i < DecorController.Instance.PlacedDecors.Count; i++) {

			GameItemPropties tempObj = new GameItemPropties (DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().decorInfo.Id,
				                           "Decor" + "/" + DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().decorInfo.Name.Trim ('/').Trim ('"'),
				                           SerializeVector3Array (DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().transform.position).Trim ('|') +
				                           "/" + DecorController.Instance.PlacedDecors [i].GetComponent<Decor3DView> ().direction
			                           );
			DecorItemPropties.Add (tempObj);
		}
		for (int j = 0; j < RoomPurchaseManager.Instance.Addedflats.Count; j++) {
			GameItemPropties tempRomm = new GameItemPropties (0, RoomPurchaseManager.Instance.flats [j].x.ToString () + ":"
			                            + RoomPurchaseManager.Instance.flats [j].y.ToString () + "/" + RoomPurchaseManager.Instance.Addedflats [j].GetComponent<Flat3D> ().WallColourNames + "/" +
			                            RoomPurchaseManager.Instance.Addedflats [j].GetComponent<Flat3D> ().GroundTextureName,
				                            SerializeVector3Array (RoomPurchaseManager.Instance.Addedflats [j].GetComponent<Flat3D> ().transform.position));
			RoomPropties.Add (tempRomm);
		}
		if (isPaid) {
			if (DecorItemPropties != null && RoomPropties != null) {
				int HostedPartyId = 0;
				PlayerPrefs.SetInt ("HostedPartyId", HostedPartyId); 
				print (HostedPartyId.ToString () + " this is party id");
				OnMyFlatPartySubmited (EndTime, HostedPartyId);
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("hostFlatParties");
			} else {
				print ("Somthing went wrong!!!");
			} 
		}
		
	}

	public void UpdateFlatParty (int time)
	{
		if (time == 180f) {
			if (PlayerPrefs.GetInt ("Money") >= 500 && PlayerPrefs.GetInt ("Gems") >= 5) {
				ComfrimFlatPartyUpdate ("Are you sure you want to pay 500 coins & ", "5 gems to extend " + HostPartyManager.Instance.selectedFlatParty.Name + " Flat party? for 03:00 min.", time);
			} else {
				ShowLessAmmountMesg ("You need atleast 500 coins & 5 Gems");
			}
		} else if (time == 300f) {
			if (PlayerPrefs.GetInt ("Money") >= 1000 && PlayerPrefs.GetInt ("Gems") >= 8) {
				ComfrimFlatPartyUpdate ("Are you sure you want to pay 1000 coins & ", "8 gems to extend " + HostPartyManager.Instance.selectedFlatParty.Name + " Flat party? for 05:00 min.", time);
			} else {
				ShowLessAmmountMesg ("You need atleast 1000 coins & 8 Gems");
			}
		} else if (time == 900) {
			if (PlayerPrefs.GetInt ("Money") >= 1500 && PlayerPrefs.GetInt ("Gems") >= 10) {
				ComfrimFlatPartyUpdate ("Are you sure you want to pay 1500 coins & ", "10 gems to extend " + HostPartyManager.Instance.selectedFlatParty.Name + " Flat party? for 15:00 min.", time);
			} else {
				ShowLessAmmountMesg ("You need atleast 1500 coins & 10 Gems");
			}
		}			
	
	}


	public IEnumerator IeUpdateParty (int updateTime)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["party_name"] = HostPartyManager.Instance.selectedFlatParty.Name;
		jsonElement ["party_desc"] = HostPartyManager.Instance.selectedFlatParty.Description;
		jsonElement ["max_no_of_guests"] = HostPartyManager.Instance.selectedFlatParty.TotleMember.ToString ();

		//		string eTime = ExtensionMethods.GetTimeStringFromFloat (PartyTime);
		//		DateTime EndTime = Convert.ToDateTime (eTime);

//		var PEndTime = HostPartyManager.Instance.selectedFlatParty.PartyEndTime.AddSeconds (updateTime);

//		DateTime EndTime = PEndTime;
		DateTime EndTime = HostPartyManager.Instance.selectedFlatParty.PartyEndTime.AddSeconds (updateTime);
	
		print (EndTime);
		jsonElement ["party_end_time"] = EndTime.ToString ();
		if (HostPartyManager.Instance.selectedFlatParty.PartyType)
			jsonElement ["party_type"] = "2";
		else
			jsonElement ["party_type"] = "1";
		jsonElement ["no_of_present_member"] = "1";

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (HostPartyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (isPaid) {
				if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Your flat party has been updated successfully")) {
					TimeExtended = true;
					UpdateMyRunningParty (updateTime);
				} 
			}

		}
	}


	public void OnMyFlatPartySubmited (DateTime eTime, int partyId)
	{
		var FlatPartys = new List<HostPartyManager.HostParty> ();
		tempFlatParty = new HostPartyManager.HostParty (PlayerPrefs.GetInt ("PlayerId"), partyId, partyName, partyDiscription, TotelGuest, PrivateParty, eTime, 0, RoomPurchaseManager.Instance.flats.Count, DecorItemPropties, RoomPropties, DateTime.Now);
		FlatPartys.Add (tempFlatParty);
		HostPartyManager.Instance._myFlatParty = FlatPartys;
		CreateMyFlatPartyList (HostPartyManager.Instance._myFlatParty, HostPartyManager.Instance.MyFlatPartyContainer);
		ScreenManager.Instance.RunningParty.gameObject.GetComponent<FlatPartyScreenControle> ().enabled = true;
		PartyHosted = true;
		HostPartyManager.Instance.AttendingParty = true;
		clearFeild ();
//		PlayerPrefs.SetFloat ("HostPartyCooldownTime", HostPartyCoolDown);
//		DownloadContent.Instance.StartCoroutine (DownloadContent.Instance.UpdateData ());
	}

	public void UpdateMyRunningParty (int eTime)
	{
		print (HostPartyManager.Instance.selectedFlatParty.PartyEndTime.ToString ());
		HostPartyManager.Instance.selectedFlatParty.PartyEndTime.AddSeconds (eTime);
		var tt = HostPartyManager.Instance.selectedFlatParty.PartyEndTime.AddSeconds (eTime);
		HostPartyManager.Instance.selectedFlatParty.PartyEndTime = tt;
		print (tt);
		print (HostPartyManager.Instance.selectedFlatParty.PartyEndTime.ToString ());
	}

	public void clearFeild ()
	{
		partyName = "";
		HostPartyName.text = "";
		partyDiscription = "";
		HostPartyDiscription.text = "";
		PartyTimeDropDown.captionText.text = "Select";
		PartyTimeDropDown.value = 0;
		TotelGuestDropDown.captionText.text = "Select";
		TotelGuestDropDown.value = 0;
		PartyTypeToggel.isOn = true;
		PartyTypePrivate.isOn = false;
		PrivateParty = false;

	}

	public void CreateMyFlatPartyList (List<HostPartyManager.HostParty> MyFlatParty, GameObject Flatcontainer)
	{
		HostPartyManager.Instance.DeleteGameObjects (Flatcontainer);
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();

		MyFlatParty.ForEach (flatParty => {
			GameObject Go = GameObject.Instantiate (HostPartyManager.Instance.FlatPartyPrefabsList, Flatcontainer.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.GetComponent <HostPartyUI> ().HostParty = flatParty;
			Go.GetComponent<HostPartyUI> ().Start ();
			if (Tut.HostPartyCreated)
				Go.GetComponent<HostPartyUI> ().OpenFlatParty ();
			else
				Go.GetComponent<HostPartyUI> ().OpenOfflineFlatParty ();
				
		});
	}

    public void FlatPartyHostingFee ()
    {
        var Tut = GameManager.Instance.GetComponent<Tutorial> ();
        if (TotelGuest == 3) {
            ComfrimFlatPartySubmited ("Are you sure you want to host free party ", "");

        } else if (TotelGuest == 8) {
            if (!Tut.HostPartyCreated ) {
                ComfrimFlatPartySubmited ("Are you sure you want to host free party ", "");
                isPaid = false;
            } else {
                if (PlayerPrefs.GetInt ("Money") >= 500 && PlayerPrefs.GetInt ("Gems") >= 10) {
                    ComfrimFlatPartySubmited ("Are you sure you want to pay 500 coins & ", "10 gems to ");
                    isPaid = false;
                } else {
                    ShowLessAmmountMesg ("You need atleast 500 coins & 10 Gems");
                    isPaid = false;
                }
            }

        } else if (TotelGuest == 15) {
            if (!Tut.HostPartyCreated) {
                ComfrimFlatPartySubmited ("Are you sure you want to host free party ", "");
                isPaid = false;
            } else {
                if (PlayerPrefs.GetInt ("Money") >= 800 && PlayerPrefs.GetInt ("Gems") >= 30) {
                    ComfrimFlatPartySubmited ("Are you sure you want to pay 800 coins & ", "30 gems to ");

                } else {
                    ShowLessAmmountMesg ("You need atleast 800 coins & 30 Gems");
                    isPaid = false;
                }
            }
        } else if (TotelGuest == 25) {
            if (!Tut.HostPartyCreated) {
                ComfrimFlatPartySubmited ("Are you sure you want to host free party ", "");
                isPaid = false;
            } else {
                if (VipSubscriptionManager.Instance.VipSubscribed) {
                    if (PlayerPrefs.GetInt ("Money") >= 1000 && PlayerPrefs.GetInt ("Gems") >= 50) {
                        ComfrimFlatPartySubmited ("Are you sure you want to pay 1000 coins & ", "50 gems to ");
                    } else {
                        ShowLessAmmountMesg ("You need atleast 1000 coins and 50 gems");    
                    }
                } else {
                    ShowLessAmmountMesg ("You must have VIP subscription, 1000 coins and 50 gems"); 
                    isPaid = false;
                }
            }
        }
    }

	public void ComfrimFlatPartySubmited (string CoinMsg, string GemsMsg)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = CoinMsg + GemsMsg + partyName + " flat party?";	
		if (TotelGuest == 3) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				isPaid = true;
				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 10) {
					OfflineTutSubmitHostParty ();
					Tut.HostPartyTutorial ();
				} else
					StartCoroutine (IeHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();

			});
		} else if (TotelGuest == 8) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				GameManager.Instance.SubtractCoins (500);
				GameManager.Instance.SubtractGems (10);
				isPaid = true;
				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 10) {
					Tut.HostPartyTutorial ();
					OfflineTutSubmitHostParty ();
				} else
					StartCoroutine (IeHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();
			});
			
		} else if (TotelGuest == 15) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				GameManager.Instance.SubtractCoins (800);
				GameManager.Instance.SubtractGems (30);
				isPaid = true;
				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 10) {
					Tut.HostPartyTutorial ();
					OfflineTutSubmitHostParty ();
				} else
					StartCoroutine (IeHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();
			});
			
		} else if (TotelGuest == 25) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 					
				GameManager.Instance.SubtractCoins (1000);
				GameManager.Instance.SubtractGems (50);
				isPaid = true;
				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 10) {
					Tut.HostPartyTutorial ();
					OfflineTutSubmitHostParty ();
				} else
					StartCoroutine (IeHostPartySubmit ());
				ScreenManager.Instance.ClosePopup ();
			});
			
		}


		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}

	public void ComfrimFlatPartyUpdate (string CoinMsg, string GemsMsg, int time)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = CoinMsg + GemsMsg + partyName + " flat party?";	
		if (time == 180f) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				GameManager.Instance.SubtractCoins (500);
				GameManager.Instance.SubtractGems (5);
				StartCoroutine (IeUpdateParty (time));
				ScreenManager.Instance.ClosePopup ();
				TimeExtendPanel.SetActive (false);
			});
		} else if (time == 300f) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				GameManager.Instance.SubtractCoins (1000);
				GameManager.Instance.SubtractGems (8);
				StartCoroutine (IeUpdateParty (time));
				ScreenManager.Instance.ClosePopup ();
				TimeExtendPanel.SetActive (false);
			});

		} else if (time == 900) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 				
				GameManager.Instance.SubtractCoins (1500);
				GameManager.Instance.SubtractGems (10);
				StartCoroutine (IeUpdateParty (time));
				ScreenManager.Instance.ClosePopup ();
				TimeExtendPanel.SetActive (false);
			});

		} else if (time == 1500f) {
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 					
				GameManager.Instance.SubtractCoins (2000);
				GameManager.Instance.SubtractGems (20);
				StartCoroutine (IeUpdateParty (time));
				ScreenManager.Instance.ClosePopup ();
				TimeExtendPanel.SetActive (false);
			});

		}

		ScreenManager.Instance.News.transform.FindChild ("closeH").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			TimeExtendPanel.SetActive (false);
		});
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


	public void TimeExtendAlart (string msg, bool extend)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			TimeExtendPanel.SetActive (true);
			ScreenManager.Instance.ClosePopup ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			TimeExtendPanel.SetActive (false);
			ScreenManager.Instance.ClosePopup ();
		});
	}

	public void AlreadyHostedParty ()
	{
		if (!PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.IsBusy) {
			
			if (PartyHosted) {
				ShowLessAmmountMesg ("Your host party is on cooldown");
			} else {
				ScreenAndPopupCall.Instance.CreateHostPartyScreen ();
			}
		} else {
			FlatPartyHostingControler.Instance.ShowLessAmmountMesg ("Player is busy please try after some time.");
		}
	
	}

	public void PartyOnCoolDown ()
	{
		ShowLessAmmountMesg ("Your host party is on cooldown");
	}

	public void DeleteFlatPartyTime ()
	{
//		PlayerPrefs.DeleteKey ("FlatPartyEndTime");
	}

	public void PartyHostCoolDown ()
	{
		HostPartyCoolDown = 180f;
		if (myPartyEnding) {
			var endTime = DateTime.Now.AddSeconds (HostPartyCoolDown);
			HostparyCoolDownEnd = endTime;
			print (HostparyCoolDownEnd);
			PlayerPrefs.SetString ("HostPartyCooldownTime", endTime.ToBinary ().ToString ());
		}
	}

	public void PartyHostCoolDownIfEnded (float time)
	{
		HostPartyCoolDown = 180f;
		if (myPartyEnding) {
			var endTime = DateTime.Now.AddSeconds (time);
			HostparyCoolDownEnd = endTime;
			print (time);
			PlayerPrefs.SetString ("HostPartyCooldownTime", endTime.ToBinary ().ToString ());

		}
	}

	void Update ()
	{
		if (HostparyCoolDownEnd > DateTime.Now) {
			var Diff = HostparyCoolDownEnd - DateTime.Now;
			float TimeRemain = (float)Diff.TotalSeconds;
			HostPartyCoolDown = TimeRemain;
			CooldownText.text = ExtensionMethods.GetTimeStringFromFloat (HostPartyCoolDown);
			PartyHosted = true;
			isPaid = true;
			FlatPartyHostingControler.Instance.CooldownShow.SetActive (true);
			FlatPartyHostingControler.Instance.CooldownShow.transform.GetChild (0).GetComponent<Text> ().text = ExtensionMethods.GetTimeStringFromFloat (HostPartyCoolDown);
			if (HostPartyCoolDown < 0.5f) {
				PlayerPrefs.DeleteKey ("HostPartyCooldownTime");
				PartyHosted = false;
				isPaid = false;
				TimeExtended = false;
				FlatPartyHostingControler.Instance.CooldownShow.SetActive (false);
				CooldownText.text = "";
				if (HostPartyCoolDown < 0.08f) {
					HostPartyManager.Instance.StartCoroutine (HostPartyManager.Instance.IDeleteSelectedFlatParty (PlayerPrefs.GetInt ("HostedPartyId")));
					PlayerPrefs.DeleteKey ("HostedPartyId");
					HostPartyManager.Instance.CreateButton.SetActive (true);	
				}
				return;
			} 
		
		}
	}

	public static string SerializeVector3Array (Vector3 aVectors)
	{
		StringBuilder sb = new StringBuilder ();

		sb.Append (aVectors.x).Append (" ").Append (aVectors.y).Append (" ").Append (aVectors.z).Append ("|");

		if (sb.Length > 0) // remove last "|"
			sb.Remove (sb.Length - 1, 1);
		return sb.ToString ();
	}

	[Serializable]
	public class GameItemPropties
	{
		public int id;
		public string types;
		public string propties;

		public GameItemPropties (int _id, string _type, string _properties)
		{
			id = _id;
			types = _type;
			propties = _properties;
		}
	}



}
