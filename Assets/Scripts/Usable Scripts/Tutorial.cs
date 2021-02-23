/// <summary>
/// Made by Mandeep Yadav, Dated... 3 Aug 2k16
/// Tutorial ==> This script is used when the player is on level 0 and we have to teach him how to use the functionality of the game.
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tutorial : MonoBehaviour
{
	public int purchaseSofa = 0;
	public int downloadFlatmate = 0;
	public int attendClass = 0;
	public int purchaseDress = 0;
	public int purchaseSaloon = 0;
	public int fashionEvent = 0;
	public int recruitFlatmate = 0;
	public int purchaseLand = 0;
	//	public int groundChange = 0;
	//	public int walltextureChange = 0;
	public int questAttended = 0;

	public int publicAreaAccess = 0;
	public int societyTutorial = 0;
	public int hostPartyTutorial = 0;

	public bool _IntroDone;
	public bool _SofaPurchased;
	public bool _SofaPlaced;
	public bool _FlatmateDownloaded;
	public bool _ClassAttended;
	public bool _DressPurchased;
	public bool _SaloonPurchased;
	public bool _FashionEventCompleate;
	public bool _LandPurchased;
	//	public bool _GroundTextureChanged;
	//	public bool _WallTextureChanged;
	public bool _FlatmateRecruited;
	public bool _PublicAreaAccessed;
	public bool _QuestAttended;
	public bool _CatWalkEventCompleate;


	public bool _SocietyCreated;
	public bool HostPartyCreated;

	public bool Registered = false;
	public EventSystem eventsystem;
	public GameObject lasteventgameobject;

	public Button[] AllButtons;

	public Sprite FlatmateImageFemale;
	public Sprite FlatmateImageMale;

	[Header ("Sofa Purchase Arrow")]
	public GameObject arrowSofa1, arrowSofa2, arrowSofa3, arrowSofa4, arrowSofa5;


	[Header ("Dress Purchase Arrow")]
	public GameObject arrowDress1, arrowDress2, arrowDress3, arrowDress4, arrowDress5, wardrobeArrow1, wardrobeArrow2, wardrobeArrow3, wardrobeArrow4;


	[Header ("Land Purchase Arrow")]
	public GameObject arrowLand1, arrowLand2, arrowLand3, arrowLand4, arrowLand5;


	[Header ("Recruit Arrow")]
	public GameObject arrowRecruit1, arrowRecruit2, arrowRecruit3, arrowRecruit4;

	[Header ("Attend Class Arrow")]
	public GameObject arrowClass1, arrowClass2, arrowClass3, arrowClass4, arrowClass5, arrowClass6, arrowClass7;

	[Header ("Saloon Arrows")]
	public GameObject arrowSaloon1, arrowSaloon2, arrowSaloon3, arrowSaloon4, arrowSaloon5, arrowSaloon6, arrowSaloon7, arrowSaloon8;

	[Header ("Public Area Arrows")]
	public GameObject arrowPubArea1, arrowPubArea2, arrowPubArea3, arrowPubArea4, arrowPubArea5, arrowPubArea6, arrowPubArea7;


	[Header ("Quest Attending Arrows")]
	public GameObject questAttended1, questAttended2, questAttended3, questAttended4, questAttended5, questAttended6, questAttended7, questAttended8, questAttended9, questAttended10, questAttended11, questAttended12, questAttended13;


	[Header ("Fashion Event Arrows")]
	public GameObject fashionEvent1, fashionEvent2, fashionEvent3, fashionEvent4, fashionEvent5, fashionEvent6, fashionEvent7, fashionEvent8, fashionEvent9, fashionEvent10, fashionEvent11, fashionEvent12, fashionEvent13, fashionEvent14, fashionEvent15;

	[Header ("Society Tutorial Arrows")]
	public GameObject arrowSociety1, arrowSociety2, arrowSociety3, arrowSociety4, arrowSociety5, arrowSociety6, arrowSociety7, arrowSociety8;

	[Header ("Flat party Tutorial Arrows")]
	public GameObject arrowFlatParty1, arrowFlatParty2, arrowFlatParty3, arrowFlatParty4, arrowFlatParty5, arrowFlatParty6, arrowFlatParty7, arrowFlatParty8, arrowFlatParty9, arrowFlatParty10;

	public void Start ()
	{

		AllButtonsState (false);
//		PlayerPrefs.SetInt ("Tutorial_Progress", 24);
		//InvokeRepeating ("CheckStatus", 0.1f, 0.5f);
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 26) {
			///for Temp 
//			PlayerPrefs.DeleteKey ("Gems");
//			PlayerPrefs.DeleteKey ("Money");
//			GameManager.Instance.AddGems (1000);
//			GameManager.Instance.AddCoins (1000);
			//
			var pStatus = PlayerPrefs.GetString ("activateParentel");
			if (pStatus.Contains ("True")) {
				ParenteralController.Instance.activateParentel = true;
			} else {
				ParenteralController.Instance.activateParentel = false;
			}
		}
	
	}


	/// <summary>
	/// setup the tutorial according to the part completed.
	/// It's the function which will we called only once when the data will be downloaded
	/// </summary>
	public void TutorialStart ()
	{
		
		CloseToolTip ();
		int x = PlayerPrefs.GetInt ("Tutorial_Progress");
//        PlayerPrefs.SetInt ("Tutorial_Progress",10);
		print (x);
		if (!PlayerPrefs.HasKey ("Tutorial_Progress")) {
			PlayerPrefs.SetInt ("Tutorial_Progress", 0);
		}
		if (PlayerPrefs.GetInt ("Purchaseland") > 4)
			purchaseLand = PlayerPrefs.GetInt ("Purchaseland");

		//Revert all the previous settings of the tutorial

		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 2) {
			_SofaPlaced = true;
			_SofaPurchased = true;
			purchaseSofa = 7;
		}

		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 4) {
			//			RoommateManager.Instance.RoommatesAvailable [0].GetComponent<FlatmateOptionForRecruit> ().HireRoommate ();
			_FlatmateDownloaded = true;
			downloadFlatmate = 1;
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 6) {
			_ClassAttended = true;
			attendClass = 8;
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 9) {
			_DressPurchased = true;
			purchaseDress = 12;

		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 12) {
			_SaloonPurchased = true;
			purchaseSaloon = 11;
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 14) {
			_FashionEventCompleate = true;
			fashionEvent = 16;		
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 15) {
			_FlatmateRecruited = true;
			recruitFlatmate = 8;
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 17) {
//			_LandPurchased = true;
			purchaseLand = 5;
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 18) {
			_LandPurchased = true;
			purchaseLand = 6;
		}
//		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 19) {
//			_GroundTextureChanged = true;
//			groundChange = 6;
//		}
//		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 20) {
//			_WallTextureChanged = true;
//			walltextureChange = 6;
//		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 20) {
			_PublicAreaAccessed = true;
			publicAreaAccess = 6;
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 22) {
			_SocietyCreated = true;
			societyTutorial = 17;
		}
		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 24) {
			HostPartyCreated = true;
			hostPartyTutorial = 12;
		}
	

		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 26) {
			
			var pStatus = PlayerPrefs.GetString ("activateParentel");
			if (pStatus.Contains ("True")) {
				ParenteralController.Instance.activateParentel = true;
			} else {
				ParenteralController.Instance.activateParentel = false;
			}
		}
	}



	/// <summary>
	/// Updates the tutorial according to the completed level
	/// will be called from every changed level of the tutorial
	/// </summary>
	public void UpdateTutorial ()
	{
		CloseToolTip ();
		GameManager.Instance.IsTutorialActive = true;
		var dkghd = PlayerPrefs.GetInt ("Tutorial_Progress");
		//Choose from where the tutorial should progress
			
		switch (PlayerPrefs.GetInt ("Tutorial_Progress")) {
		case 0:
//			foreach (var banner in RoomPurchaseManager.Instance.SaleBanners) {
//				banner.isAceptingMouse = false;
//			}
			IntroductionByUniRep ();
			break;
		case 1:
			SofaPurchasing ();
			break;
		case 2:
			PlaceSofa ();
			break;
		case 3:
			Invoke ("ExcellentScreen", 0.6f);
			break;
		case 4:
			DownloadnewFlatmate ();
			break;
		case 5:
			Invoke ("ShowFlatMatePopUp", 0.6f);
			break;
		case 6:
			Invoke ("AttendClass", 0.5f);
			GameManager.Instance.IsTutorialActive = false;
			break;
		case 7:
			ShowPopUpWithMessage ("Want to Go Shopping?", () => ShowPopupOk ());
			break;
		case 8:		
			DressPurchasing ();
			break;
		case 9:
			purchaseDress = 5;
			DressPurchasing ();
			break;
		case 10:
			Invoke ("ShowPopUpForSaloon", 0.6f);
			break;
		case 11:
			SaloonPurchasing ();
			break;
		case 12:
			purchaseSaloon = 5;
			SaloonPurchasing ();
			ParenteralController.Instance.activateParentel = false;
			break;
		case 13:
//			if(!ParenteralController.Instance.activateParentel)
			CheckFashionShowEvent ();
//			else {
//				_FashionEventCompleate = true;
//				fashionEvent = 16;	
//				PlayerPrefs.SetInt ("Tutorial_Progress", 15);
//				UpdateTutorial ();
//			}			
			break;

		case 14:
//			if (!ParenteralController.Instance.activateParentel) {
			FashionEventStart ();			
//			}
//			else {
//				_FashionEventCompleate = true;
//				fashionEvent = 16;	
//				PlayerPrefs.SetInt ("Tutorial_Progress", 15);
//				UpdateTutorial ();
//			}
			break;
		case 15:
			RecruitFlatmate ();
			break;
		case 16:
			IntroductionToPurchaseLand ();
			break;
		case 17:
			LandPurchasing ();// Room build Code Here;
			break;
		case 18:
			purchaseLand = 5;
			LandPurchasing ();			
			break;
		case 19:
//			GroundTextureChanging ();
//			break;
//		case 20:
			if (!ParenteralController.Instance.activateParentel)
				PopUpForPublicArea ();
			else {
				_PublicAreaAccessed = true;
				publicAreaAccess = 6;
				PlayerPrefs.SetInt ("Tutorial_Progress", 21);
				UpdateTutorial ();
			}
			break;
		case 20:
			if (!ParenteralController.Instance.activateParentel)
				PublicAreaAccessing ();
			else {
				_PublicAreaAccessed = true;
				publicAreaAccess = 6;
				PlayerPrefs.SetInt ("Tutorial_Progress", 21);
				UpdateTutorial ();
			}
			break;

//			RoomPurchaseManager.Instance.SaleBanners [0].isAceptingMouse = true;
//			AllButtonsState (false);
//			LandPurchasing ();
//			ParenteralController.Instance.activateParentel = false;
//			break;

		case 21:
			if (!ParenteralController.Instance.activateParentel)
				ShowPopUpWithMessage ("Lets take a look at societies.", () => {                   
					PlayerPrefs.SetInt ("Tutorial_Progress", 22);
					UpdateTutorial (); 
                            
				});
			else { 
				_SocietyCreated = true;
				societyTutorial = 6;
				PlayerPrefs.SetInt ("Tutorial_Progress", 23);
				UpdateTutorial ();
			}
			break;
		case 22:
			if (!ParenteralController.Instance.activateParentel)
				SocietyTutorial ();
			else { 
				_SocietyCreated = true;
				societyTutorial = 6;
				PlayerPrefs.SetInt ("Tutorial_Progress", 23);
				UpdateTutorial ();
			}
			break;
//		case 16:
//			purchaseLand = 7;
//			LandPurchasing ();
//			ParenteralController.Instance.activateParentel = false;
//			break;
//		case 17:
//			PopUpForPublicArea ();
//			break;
//		case 18:
//			PublicAreaAccessing ();
//			ParenteralController.Instance.activateParentel = false;
//			break;
//		case 19:
////			GameManager.Instance.level = 4;
//
//			if (GameManager.Instance.level >= 1 && RoommateManager.Instance.RoommatesHired.Length >= 2) {
//				AllButtonsState (false);
//				CheckForQuest ();
//				ParenteralController.Instance.activateParentel = false;
//
//			} else {
////				if (!_shown)
////					Phase2EndPopup ();
//				EnablebuttonsAfterSecondPhase ();
//				GameManager.Instance.IsTutorialActive = false;
//				ParenteralController.Instance.activateParentel = false;
//			}
//			break;
//		case 20:
//			questAttended = 0;
//			QuestAttendingStart ();
//			ParenteralController.Instance.activateParentel = false;
//			break;
//		case 21:
//			questAttended = 7;
//			AttendQuest ();
//			ParenteralController.Instance.activateParentel = false;
//			break;
//		case 22:
//			questAttended = 10;
//			AttendQuest ();
//			ParenteralController.Instance.activateParentel = false;
//			break;
//		case 23:
//			CheckFashionShowEvent ();
//			ParenteralController.Instance.activateParentel = false;
//			break;
//		case 24:
//			FashionEventStart ();
//			ParenteralController.Instance.activateParentel = false;
//			break;
		case 23:

			if (!ParenteralController.Instance.activateParentel) {
				ShowPopUpWithMessage ("Lets take a look at host party.", () => {                   
					PlayerPrefs.SetInt ("Tutorial_Progress", 24);
					UpdateTutorial (); 

				});
			} else { 
				HostPartyCreated = true;
				hostPartyTutorial = 6;
				PlayerPrefs.SetInt ("Tutorial_Progress", 25);
				UpdateTutorial ();
			}
			break;
		case 24:
			if (!ParenteralController.Instance.activateParentel) {
				HostPartyTutorial ();
			} else { 
				HostPartyCreated = true;
				hostPartyTutorial = 12;
				PlayerPrefs.SetInt ("Tutorial_Progress", 25);
				UpdateTutorial ();
			}
			break;
		case 25:
			ShowPopUpWithMessage ("Well Done! Your Tutorial is completed. Now you can explore the University", () => {
				PlayerPrefs.SetInt ("Tutorial_Progress", 26);
				UpdateTutorial ();
				//Delete Flat Party
				LeaveTutorialFlatParty ();
				HostPartyManager.Instance.StartCoroutine (HostPartyManager.Instance.IDeleteSelectedFlatParty (PlayerPrefs.GetInt ("HostedPartyId")));
				PlayerPrefs.DeleteKey ("HostedPartyId");
			});
			break;
		case 26:
			AllButtonsState (true);
			this.enabled = false;
			GameManager.Instance.IsTutorialActive = false;
			var DragCamera = Camera.main.GetComponent <DragCamera1> ();
			DragCamera.SwipeLeft.SetActive (false);
			DragCamera.SwipeRight.SetActive (false);

			var pStatus = PlayerPrefs.GetString ("activateParentel");
			if (pStatus.Contains ("True")) {
				ParenteralController.Instance.activateParentel = true;
				ParenteralController.Instance.disableParentalControl.SetActive (true);
				ParenteralController.Instance.StatusForButton.text = "DISABLE";
				ParenteralController.Instance.ButtonText.text = ParenteralController.Instance.StatusForButton.text;
			} else {
				ParenteralController.Instance.activateParentel = false;
				//						enableParentalControl.SetActive (true);
//				ParenteralController.Instance.disableParentalControl.SetActive (true);
//				ParenteralController.Instance.StatusForButton.text = "ENABLE";
//				ParenteralController.Instance.ButtonText.text = ParenteralController.Instance.StatusForButton.text;
			}	
			// flat party atributes
			FlatPartyHostingControler.Instance.HostPartyName.interactable = true;
			FlatPartyHostingControler.Instance.HostPartyDiscription.interactable = true;
			FlatPartyHostingControler.Instance.PartyTimeDropDown.interactable = true;
			FlatPartyHostingControler.Instance.TotelGuestDropDown.interactable = true;
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = true;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = true;
			GameObject.Find ("PartyScreen").transform.GetChild (3).gameObject.SetActive (true);
			arrowFlatParty1.SetActive (false);
			arrowFlatParty2.SetActive (false);
			arrowFlatParty3.SetActive (false);
			arrowFlatParty4.SetActive (false);
			arrowFlatParty5.SetActive (false);
			arrowFlatParty6.SetActive (false);
			arrowFlatParty7.SetActive (false);
			arrowFlatParty8.SetActive (false);
			arrowFlatParty9.SetActive (false);
			arrowFlatParty10.SetActive (false);
			break;
		}
	}

	void UniversityExploring ()
	{
		PlayerPrefs.SetInt ("SwipeLeftRightSetUp", 0);
		var DragCamera = Camera.main.GetComponent <DragCamera1> ();
		DragCamera.SwipeLeft.SetActive (true);
		DragCamera.SwipeRight.SetActive (false);
	}

	public void EnablebuttonsAfterFirstPhase ()
	{
		AllButtonsState (false);
		AllButtons [0].interactable = true;
		AllButtons [1].interactable = true;
		AllButtons [2].interactable = true;
		AllButtons [3].interactable = true;
		AllButtons [4].interactable = true;
		AllButtons [7].interactable = true;
		AllButtons [8].interactable = true;
		AllButtons [9].interactable = true;
		AllButtons [10].interactable = true;
		AllButtons [13].interactable = true;
		AllButtons [14].interactable = true;
		AllButtons [15].interactable = true;
		AllButtons [16].interactable = true;
		AllButtons [26].interactable = true;
		AllButtons [27].interactable = true;
		AllButtons [32].interactable = true;
		AllButtons [34].interactable = true;
	}

	public void EnablebuttonsAfterSecondPhase ()
	{
		AllButtonsState (false);

		AllButtons [0].interactable = true;
		AllButtons [1].interactable = true;
		AllButtons [2].interactable = true;
		AllButtons [3].interactable = true;
		AllButtons [4].interactable = true;
		AllButtons [7].interactable = true;
		AllButtons [8].interactable = true;
		AllButtons [9].interactable = true;
		AllButtons [10].interactable = true;
		AllButtons [13].interactable = true;
		AllButtons [14].interactable = true;
		AllButtons [15].interactable = true;
		AllButtons [16].interactable = true;
		AllButtons [26].interactable = true;
		AllButtons [27].interactable = true;
		AllButtons [32].interactable = true;
		AllButtons [34].interactable = true;


		AllButtons [6].interactable = true;
		AllButtons [11].interactable = true;
		AllButtons [17].interactable = true;
		AllButtons [18].interactable = true;
		AllButtons [24].interactable = true;
		AllButtons [12].interactable = true;// Contacts Button...
		AllButtons [37].interactable = true; // Add Friend in public place button...

//		foreach (var banner in RoomPurchaseManager.Instance.SaleBanners) {
//			banner.isAceptingMouse = true;
//		}
	}

	public void AllButtonsState (bool isActive)
	{
		foreach (var button in AllButtons) {
			button.interactable = isActive;
		}
	}


	/// <summary>
	/// NOTE: this function describe that how we gonna use any popup multiple times like if we want to use this uni pop up again for another message then we just have to add the message and then 
	/// we can change the button text and listener because listener is not fixed in this button
	/// </summary>
	public void IntroductionByUniRep ()
	{        
		StartCoroutine (IEIntroductionByUniRep ());

	}

	IEnumerator IEIntroductionByUniRep ()
	{
		yield return new WaitForSeconds (1.5f);
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);

		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Welcome to the University! " +
		"This is your new home! Why don’t you get unpacked? \n \nFollow the steps to purchase Decore for you!!! ";
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => UniRepOK ());
	}

	public void UniRepOK ()
	{
		PlayerPrefs.SetInt ("Tutorial_Progress", 1);
		ScreenManager.Instance.ClosePopup ();
		_IntroDone = true;
		UpdateTutorial ();
		//		PurchaseSofa ();
	}

	public void IntroductionToPurchaseLand ()
	{
		PlayerPrefs.SetInt ("Tutorial_Progress", 17);

		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Welcome " + PlayerPrefs.GetString ("UserName") + " we need more space!";
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => LandPurchaseOK ());
	}

	public void LandPurchaseOK ()
	{
		ScreenManager.Instance.ClosePopup ();
		print ("Totorial to follow land purchase");
		UpdateTutorial ();
		//		LandPurchasing ();
	}

	public void LandPurchasing ()
	{
		if (_LandPurchased || !_DressPurchased || !_SaloonPurchased || !_FlatmateRecruited) {
			return;
		}

//		if (GameManager.Instance.level >= 2 && RoommateManager.Instance.RoommatesHired.Length >= 2)
		PurchaseLand ();
//		else
//			EnablebuttonsAfterFirstPhase ();

	}


	public void LandLevelDecrease ()
	{		
		if (_LandPurchased || !_DressPurchased) {
			return;		
		}

		if (purchaseLand == 1)
			purchaseLand--;
		else if (purchaseLand < 6)
			purchaseLand = 0;
		else if (purchaseLand >= 6)
			purchaseLand = 2;


		PurchaseLand ();
	}

	void PurchaseLand ()
	{
		CloseToolTip ();
		if (_LandPurchased)
			return;
		purchaseLand++;
		PlayerPrefs.SetInt ("Purchaseland", purchaseLand);

		if (purchaseLand == 1) {

//			AllButtonsState (false);
//			arrowLand1.SetActive (true);
//			arrowLand1.transform.position = RoomPurchaseManager.Instance.SaleBanners [0].transform.position + new Vector3 (0f, 1.8f, 0f);
//			arrowLand2.SetActive (false);
//			arrowLand3.SetActive (false);
//			arrowLand4.SetActive (false);
//			arrowLand5.SetActive (false);
//
////			var TempPos= GameObject.Find ("UICamera").GetComponent <Camera>().WorldToScreenPoint(arrowLand1.transform.position);
////
////			Vector2 pos = new Vector2 (TempPos.x, TempPos.y);
////			ShowToolTipWithArrow ("Click here to expand your land" , pos, TooltipSide.DownRight);
//
//		} else if (purchaseLand == 2) {
//			PlayerPrefs.SetInt ("Tutorial_Progress", 15);
//			AllButtonsState (false);// On Ok Click of pop up of sale banner
//			arrowLand1.SetActive (false);
//			arrowLand2.SetActive (false);
//			arrowLand3.SetActive (false);
//			arrowLand4.SetActive (false);
//			arrowLand5.SetActive (false);
//
//		} else if (purchaseLand == 3) {
			AllButtonsState (false);
			AllButtons [0].interactable = true; // Menu button
			ShowToolTipWithArrow ("Lets Build a room now", new Vector2 (-180, -100), TooltipSide.DownLeft, 0.5f);
			arrowLand1.SetActive (false);
			arrowLand2.SetActive (true);
			arrowLand3.SetActive (false);
			arrowLand4.SetActive (false);
			arrowLand5.SetActive (false);


		} else if (purchaseLand == 2) {
			AllButtonsState (false);
			AllButtons [3].interactable = true; // Cataloge button
			arrowLand1.SetActive (false);
			arrowLand2.SetActive (false);
			arrowLand3.SetActive (true);
			arrowLand4.SetActive (false);
			arrowLand5.SetActive (false);


		} else if (purchaseLand == 3) {
			AllButtonsState (false);
			AllButtons [11].interactable = true; // ReModel button
			ShowToolTipWithArrow ("This is Remodel shop,you can build a new room, change color of wall and flooring", new Vector2 (250, 60), TooltipSide.DownLeft, 0.5f);
			arrowLand1.SetActive (false);
			arrowLand2.SetActive (false);
			arrowLand3.SetActive (false);
			arrowLand4.SetActive (true);
			arrowLand5.SetActive (false);

		} else if (purchaseLand == 4) {
			// Flat Item 
			AllButtonsState (false);
			//			AllButtons [0].interactable = true; // Flat fisrt item
			arrowLand1.SetActive (false);
			arrowLand2.SetActive (false);
			arrowLand3.SetActive (false);
			arrowLand4.SetActive (false);
			arrowLand5.SetActive (true);

		} else if (purchaseLand == 5) {
			// Flat Item 
			AllButtonsState (false);
			arrowLand1.SetActive (false);
			arrowLand2.SetActive (false);
			arrowLand3.SetActive (false);
			arrowLand4.SetActive (false);
			arrowLand5.SetActive (true);
			PlayerPrefs.SetInt ("Tutorial_Progress", 18);

		} else {			
			arrowLand1.SetActive (false);
			arrowLand2.SetActive (false);
			arrowLand3.SetActive (false);
			arrowLand4.SetActive (false);
			arrowLand5.SetActive (false);
			_LandPurchased = true;
			PlayerPrefs.SetInt ("Tutorial_Progress", 19);

			UpdateTutorial ();
			EarnCoin ();
		}
	}

	//	public void GroundTextureChanging ()
	//	{
	//		if (_GroundTextureChanged || !_LandPurchased)
	//			return;
	//
	//			Invoke ("GroundChangeTutorial", 0.2f);
	//
	//	}
	//	public void GroundLevelDecrease ()
	//	{
	//		if (_GroundTextureChanged || !_LandPurchased) {
	//			return;
	//
	//			if (groundChange == 1)
	//				groundChange--;
	////			else if (groundChange < 6)
	////				groundChange = 0;
	////			else if (groundChange >= 6)
	////				groundChange = 2;
	//			GroundChangeTutorial ();
	//
	//		}
	//	}
	//	void GroundChangeTutorial(){
	//		CloseToolTip ();
	//		if (_GroundTextureChanged)
	//			return;
	//		groundChange++;
	////
	////
	////		if(groundChange == 1)
	////		{
	////			ShowPopUpWithMessage ("It’s getting lonely around here! We should increase our ranks.",()=> RecruitFlatmate());
	////		}
	//
	//		if (groundChange == 1) 
	//		{
	//			AllButtonsState (false);
	//			AllButtons [0].interactable = true; // Menu button
	//			ShowToolTipWithArrow ("Lets buy new flooring for new room", new Vector2 (-320, -100), TooltipSide.DownLeft);
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (true);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (false);
	//
	//
	//		} else if (groundChange == 2) {
	//			AllButtonsState (false);
	//			AllButtons [3].interactable = true; // Cataloge button
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (true);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (false);
	//
	//
	//		} else if (groundChange == 3) {
	//			AllButtonsState (false);
	//			AllButtons [11].interactable = true; // ReModel button
	////			ShowToolTipWithArrow ("This is Remodel shop,you can build a new room, change color of wall and flooring", new Vector2 (108, 86), TooltipSide.DownLeft);
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (true);
	//			arrowLand5.SetActive (false);
	//
	//		} else if (groundChange == 4) {
	//			// Flat Item 
	//			AllButtonsState (false);
	//			ReModelShopController.Instance.IntializeInventoryForGrounds ();
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (true);
	//
	////		} else if (groundChange == 5) {
	////			// Flat Item 
	////			AllButtonsState (false);
	////			arrowLand1.SetActive (false);
	////			arrowLand2.SetActive (false);
	////			arrowLand3.SetActive (false);
	////			arrowLand4.SetActive (false);
	////			arrowLand5.SetActive (true);
	////			PlayerPrefs.SetInt ("Tutorial_Progress", 20);
	//
	//		} else {			
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (false);
	//			_LandPurchased = true;
	//			PlayerPrefs.SetInt ("Tutorial_Progress", 21);
	//
	//			UpdateTutorial ();
	//			EarnCoin ();
	//		}
	//	}
	//
	//	public void WallTextureChanging ()
	//	{
	//		if (_WallTextureChanged|| !_GroundTextureChanged) {
	//			return;
	//			Invoke ("WallChangeTutorial", 0.2f);
	//		}
	//	}
	//	public void WallLevelDecrease ()
	//	{		
	//		if (_WallTextureChanged || !_GroundTextureChanged) {
	//			return;		
	//
	//			if (walltextureChange == 1)
	//				walltextureChange--;
	//			//			else if (groundChange < 6)
	//			//				groundChange = 0;
	//			//			else if (groundChange >= 6)
	//			//				groundChange = 2;
	//			WallChangeTutorial ();
	//
	//		}
	//	}
	//	void WallChangeTutorial(){
	//		CloseToolTip ();
	//		if (_WallTextureChanged)
	//			return;
	//		walltextureChange++;
	//
	//		if (walltextureChange == 1) 
	//		{
	//			AllButtonsState (false);
	//			AllButtons [0].interactable = true; // Menu button
	//			ShowToolTipWithArrow ("“I’m studying interior design, we should redecorate", new Vector2 (-320, -100), TooltipSide.DownLeft);
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (true);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (false);
	//
	//
	//		} else if (walltextureChange == 2) {
	//			AllButtonsState (false);
	//			AllButtons [3].interactable = true; // Cataloge button
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (true);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (false);
	//
	//
	//		} else if (walltextureChange == 3) {
	//			AllButtonsState (false);
	//			AllButtons [11].interactable = true; // ReModel button
	//			//			ShowToolTipWithArrow ("This is Remodel shop,you can build a new room, change color of wall and flooring", new Vector2 (108, 86), TooltipSide.DownLeft);
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (true);
	//			arrowLand5.SetActive (false);
	//
	//		} else if (walltextureChange == 4) {
	//			// Flat Item 
	//			AllButtonsState (false);
	//			ReModelShopController.Instance.IntializeInventoryForWalls ();
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (true);
	//
	////		} else if (walltextureChange == 5) {
	////			// Flat Item 
	////			AllButtonsState (false);
	////			arrowLand1.SetActive (false);
	////			arrowLand2.SetActive (false);
	////			arrowLand3.SetActive (false);
	////			arrowLand4.SetActive (false);
	////			arrowLand5.SetActive (true);
	////			PlayerPrefs.SetInt ("Tutorial_Progress", 22);
	//
	//		} else {			
	//			arrowLand1.SetActive (false);
	//			arrowLand2.SetActive (false);
	//			arrowLand3.SetActive (false);
	//			arrowLand4.SetActive (false);
	//			arrowLand5.SetActive (false);
	//			_LandPurchased = true;
	//			PlayerPrefs.SetInt ("Tutorial_Progress", 23);
	//
	//			UpdateTutorial ();
	//			EarnCoin ();
	//		}
	//	}


	public void ExcellentScreen ()
	{
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Excellent! You have a freshers introductory lecture tomorrow, I hope you enjoy your time here!";
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => OnClickExelllentOk ());
	}

	void OnClickExelllentOk ()
	{
		PlayerPrefs.SetInt ("Tutorial_Progress", 4);
		ScreenManager.Instance.ClosePopup ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "- THE NEXT DAY -";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		Invoke ("ClosePopUp", 1f);

		//		UpdateTutorial ();
		Invoke ("UpdateTutorial", 1.7f);
	}


	public void PopupGoForShoping ()
	{
		if (_DressPurchased)
			return;
		//		UpdateTutorial ();
		Invoke ("UpdateTutorial", 0.5f);
	}

	void ShowPopUpWithMessage (string Message, UnityEngine.Events.UnityAction OnClickOkAction)
	{
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = Message;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickOkAction != null) {
				OnClickOkAction ();
			}
		});
	}

	void ShowPopupOk ()
	{
		PlayerPrefs.SetInt ("Tutorial_Progress", 8);
		UpdateTutorial ();
	}

	void ClosePopUp ()
	{
		ScreenManager.Instance.ClosePopup ();
	}

	public void SofaPurchasing ()
	{
		if (purchaseSofa >= 6 && _SofaPurchased && _SofaPlaced)
			return;


		arrowSofa1.SetActive (false);
		arrowSofa2.SetActive (false);
		arrowSofa3.SetActive (false);
		arrowSofa4.SetActive (false);
		arrowSofa5.SetActive (false);


        if (eventsystem.currentSelectedGameObject != lasteventgameobject || lasteventgameobject == null) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			arrowSofa1.SetActive (false);
			arrowSofa2.SetActive (false);
			arrowSofa3.SetActive (false);

			Invoke ("PurchaseSofa", 0.2f);

		} else {
			SofalevelDecrease ();
			lasteventgameobject = null;
		}
	}

	public void DressPurchasing ()
	{
		if (purchaseDress >= 12 || !_SofaPurchased || !_ClassAttended || !_FlatmateDownloaded)
			return;

		if (eventsystem.currentSelectedGameObject != lasteventgameobject) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			PurchaseDress ();
		} else {
			DressLevelDecrease ();
			lasteventgameobject = null;
		}
	}

	public void DownloadnewFlatmate ()
	{	
		if (!_SofaPurchased)
			return;	
		
		Invoke ("DownloadFlatmate", 0.2f);

	}

	public void SofalevelDecrease ()
	{	

		if (_SofaPurchased && _SofaPlaced)
			return;
		else if (_SofaPurchased && !_SofaPlaced) {
			Invoke ("UpdateTutorial", 0.6f);
			purchaseSofa = 7;
			_SofaPlaced = true;
			return;
		}

		if (purchaseSofa == 1) {
			purchaseSofa--;
		} else
			purchaseSofa = 0;
		PurchaseSofa ();
	}

	public void DressLevelDecrease ()
	{
		if (_DressPurchased || !_ClassAttended)
			return;		

		if (purchaseDress == 6 || purchaseDress == 5) {
			PurchaseDress ();
			return;
		}

		if (purchaseDress == 1)
			purchaseDress--;
		else if (purchaseDress < 5)
			purchaseDress = 0;
		else if (purchaseDress > 5 && purchaseDress <= 11)
			purchaseDress = 6;
		PurchaseDress ();
	}

	public void DressBackToCharacterSelection ()
	{
		arrowSaloon1.SetActive (false);
		arrowSaloon2.SetActive (false);
		arrowSaloon3.SetActive (false);
		arrowSaloon4.SetActive (false);
		arrowSaloon5.SetActive (false);
		arrowSaloon6.SetActive (false);
		arrowSaloon7.SetActive (false);
		arrowSaloon8.SetActive (false);

		if (_DressPurchased && _SaloonPurchased)
			return;

		if (purchaseDress >= 10) {
			if (!_DressPurchased) {
				purchaseDress = 8;
				PurchaseDress ();
			}
		}

		if (purchaseSaloon >= 9) {
			if (!_SaloonPurchased) {
				purchaseSaloon = 7;
				PurchaseSaloon ();
			}
		}
	}





	public void PurchaseSofa ()
	{
		CloseToolTip ();

		if (_SofaPurchased && _SofaPlaced)
			return;

		print ("sofa purchasing on");

		purchaseSofa++;

		if (purchaseSofa == 1) {
			AllButtonsState (false);
			AllButtons [0].interactable = true; // OpenCloseSlider
			arrowSofa1.SetActive (true);
			ShowToolTipWithArrow ("This is the Main Menu", new Vector2 (-180, -105), TooltipSide.DownLeft);

			arrowSofa2.SetActive (false);
			arrowSofa3.SetActive (false);
			arrowSofa4.SetActive (false);
			arrowSofa5.SetActive (false);

		} else if (purchaseSofa == 2) {
			AllButtonsState (false);
			AllButtons [3].interactable = true; // Cataogue button..
			arrowSofa1.SetActive (false); 
			arrowSofa2.SetActive (true);
			ShowToolTipWithArrow ("You can buy goods here", new Vector2 (-85, 2), TooltipSide.DownLeft);
			arrowSofa3.SetActive (false);
			arrowSofa4.SetActive (false);
			arrowSofa5.SetActive (false);

		} else if (purchaseSofa == 3) {
			AllButtonsState (false);
			AllButtons [7].interactable = true; // Decore button
			arrowSofa1.SetActive (false);
			arrowSofa2.SetActive (false);
			arrowSofa3.SetActive (true);
			ShowToolTipWithArrow ("Decor Shop", new Vector2 (-90, 50), TooltipSide.DownLeft);
			arrowSofa4.SetActive (false);	
			arrowSofa5.SetActive (false);

		} else if (purchaseSofa == 4) {
			AllButtons [32].interactable = true;
			arrowSofa1.SetActive (false);
			arrowSofa2.SetActive (false);
			arrowSofa3.SetActive (false);
			arrowSofa4.SetActive (true);
			arrowSofa5.SetActive (false);
		} else if (purchaseSofa == 5) {
			PlayerPrefs.SetInt ("Tutorial_Progress", 2);
			arrowSofa1.SetActive (false);
			arrowSofa2.SetActive (false);
			arrowSofa3.SetActive (false);
			arrowSofa4.SetActive (true);
			arrowSofa5.SetActive (false);
			_SofaPurchased = true;
		} else if (purchaseSofa == 6) {
			arrowSofa1.SetActive (false);
			arrowSofa2.SetActive (false);
			arrowSofa3.SetActive (false);
			arrowSofa4.SetActive (false);
			arrowSofa5.SetActive (false);
		} else {
			arrowSofa1.SetActive (false);
			arrowSofa2.SetActive (false);
			arrowSofa3.SetActive (false);
			arrowSofa4.SetActive (false);
			arrowSofa5.SetActive (false);
			EarnCoin ();

			_SofaPlaced = true;
			PlayerPrefs.SetInt ("Tutorial_Progress", 3);
			UpdateTutorial ();
			//			Invoke ("ExcellentScreen", 0.6f);
		}
	}


	public void PlaceSofa ()
	{
        
		_SofaPlaced = true;
		PlayerPrefs.SetInt ("Tutorial_Progress", 3);
		UpdateTutorial ();
		//		Invoke ("ExcellentScreen", 0.6f);
	}



	public void DisableArrows ()
	{
		arrowSofa1.SetActive (false);
		arrowSofa2.SetActive (false);
		arrowSofa3.SetActive (false);
		arrowSofa4.SetActive (false);
		arrowSofa5.SetActive (false);
		arrowClass1.SetActive (false);
		arrowClass2.SetActive (false);
		arrowClass3.SetActive (false);
		arrowDress1.SetActive (false);
		arrowDress2.SetActive (false);
		arrowDress3.SetActive (false);
		arrowRecruit1.SetActive (false);
		arrowRecruit2.SetActive (false);
	}

	public void PurchaseDress ()
	{
		CloseToolTip ();
		if (_DressPurchased)
			return;
		purchaseDress++;

		if (purchaseDress == 1) {
			AllButtonsState (false);
			AllButtons [0].interactable = true; // Open Close Slider
			arrowDress1.SetActive (true);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);

		} else if (purchaseDress == 2) {
			AllButtonsState (false);
			AllButtons [3].interactable = true; // Catalouge button
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (true);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);

		} else if (purchaseDress == 3) {
			AllButtonsState (false);
			AllButtons [8].interactable = true; // Shopping button
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (true);
			ShowToolTipWithArrow ("You can buy new dresses here", new Vector2 (0, 70), TooltipSide.DownLeft, 0.5f);
			arrowDress4.SetActive (false);

		} else if (purchaseDress == 4) {
			AllButtonsState (false);
			AllButtons [26].interactable = true;
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (true);
			arrowDress5.SetActive (false);

		} else if (purchaseDress == 5) {
			AllButtons [26].interactable = true;
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			arrowDress5.SetActive (true);            
			PlayerPrefs.SetInt ("Tutorial_Progress", 9);

			// purchased
		} else if (purchaseDress == 6) {
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			arrowDress5.SetActive (false);
			wardrobeArrow1.SetActive (false);
			wardrobeArrow2.SetActive (false);
			wardrobeArrow3.SetActive (false);
			ShowPopUpWithMessage ("Lets try on our new clothes!", () => PurchaseDress ());

		} else if (purchaseDress == 7) {
			AllButtonsState (false);
			AllButtons [0].interactable = true; // Open Close Slider
			arrowDress1.SetActive (true);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			arrowDress5.SetActive (false);
		} else if (purchaseDress == 8) {
			AllButtonsState (false);
			AllButtons [2].interactable = true; // Wardrobe button
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			arrowDress5.SetActive (false);
			arrowDress4.SetActive (true);
			wardrobeArrow1.SetActive (true);
			ShowToolTipWithArrow ("Dresses you once bought goes here", new Vector2 (-100, 128), TooltipSide.DownLeft, 0.5f);	
			wardrobeArrow2.SetActive (false);
			wardrobeArrow3.SetActive (false);
		} else if (purchaseDress == 9) {
			AllButtonsState (false);
			AllButtons [34].interactable = true;
			arrowDress1.SetActive (false); // characterSelecetion
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			wardrobeArrow1.SetActive (false);		
			wardrobeArrow2.SetActive (true);
			ShowToolTipWithArrow ("Select a character for whom you want to change dress", new Vector2 (-160, 130), TooltipSide.DownLeft, 0.5f);	
			wardrobeArrow3.SetActive (false);
			_DressPurchased = false;

		} else if (purchaseDress == 10) {
			AllButtonsState (false);
			arrowDress1.SetActive (false); 
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			arrowDress5.SetActive (false);
			wardrobeArrow1.SetActive (false);		
			wardrobeArrow2.SetActive (false);
			wardrobeArrow3.SetActive (true);
			wardrobeArrow4.SetActive (false);
		} else if (purchaseDress == 11) {
			AllButtonsState (false);
			arrowDress1.SetActive (false); // On Click Wear Confirmation Ok But Arrow On Confrim Button
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			arrowDress5.SetActive (false);
			wardrobeArrow1.SetActive (false);		
			wardrobeArrow2.SetActive (false);
			wardrobeArrow3.SetActive (false);
			wardrobeArrow4.SetActive (true);
			ShowToolTipWithArrow ("Confirm dress change", new Vector2 (50, -130), TooltipSide.DownRight, 0.5f);
		} else {
			arrowDress1.SetActive (false);
			arrowDress2.SetActive (false);
			arrowDress3.SetActive (false);
			arrowDress4.SetActive (false);
			arrowDress5.SetActive (false);
			wardrobeArrow1.SetActive (false);		
			wardrobeArrow2.SetActive (false);
			wardrobeArrow3.SetActive (false);
			wardrobeArrow4.SetActive (false);
			PlayerPrefs.SetInt ("Tutorial_Progress", 10);
			UpdateTutorial ();
			//			Invoke ("ShowPopUpForSaloon", 0.6f);
			_DressPurchased = true;
			EarnCoin ();		


		}
	}

	public void DownloadFlatmate ()
	{
		if (_FlatmateDownloaded || !_SofaPurchased)
			return;
		downloadFlatmate++;

		if (downloadFlatmate == 1) {	
			if (RoommateManager.Instance.RoommatesHired.Length <= 1) {
				StartCoroutine (Purchase ());
			}

		}
	}


	IEnumerator Purchase ()
	{

		var data = RoommateManager.Instance.RoommatesAvailable [0].GetComponent<FlatmateOptionForRecruit> ().data;

		int item_id = 0;
		string cat = "";
		string sub_cat = "";
		foreach (var downloadeditem in DownloadContent.Instance.downloaded_items) {
			if (downloadeditem.Name.Trim ('"') == data.Name.Trim ('"')) {
				item_id = downloadeditem.Item_id;
				cat = downloadeditem.Category;
				sub_cat = downloadeditem.SubCategory;
			}
		}

		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.SetPurchasingData (item_id, cat, sub_cat));
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
			RoommateManager.Instance.RoommatesAvailable [0].GetComponent<FlatmateOptionForRecruit> ().HireRoommate ();
			PlayerPrefs.SetInt ("Tutorial_Progress", 5);
			UpdateTutorial ();
			_FlatmateDownloaded = true;
			EarnCoin ();
		} else {
			StartCoroutine (Purchase ());
		}

	}



	void ShowFlatMatePopUp ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		string FlatMateName = RoommateManager.Instance.RoommatesHired [1].GetComponent <Flatmate> ().data.Name;
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent <Text> ().text = "Hi I’m " + FlatMateName.Trim ('"') + ", your new flatmate! Do you have the freshers intro lecture today? So do I, lets go!";
		ScreenManager.Instance.UniPopup.transform.FindChild ("Image").GetComponent<Image> ().sprite = GameManager.GetGender () == GenderEnum.Male ? FlatmateImageMale : FlatmateImageFemale;
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => OnClickFlatmateOk ());
	}

	public void OnClickFlatmateOk ()
	{
		PlayerPrefs.SetInt ("Tutorial_Progress", 6);
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		UpdateTutorial ();
		//		Invoke ("AttendClass", 0.5f);
	}

	public void AttendClass ()
	{
		if (attendClass > 7)
			return;

		if (eventsystem.currentSelectedGameObject != lasteventgameobject) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			arrowClass1.SetActive (false);
			arrowClass2.SetActive (false);
			arrowClass3.SetActive (false);
			Invoke ("ClassAttend", 0.2f);
		} else {
			DecreaseAttendClass ();
			lasteventgameobject = null;
		}
	}

	public void DecreaseAttendClass ()
	{
		if (_ClassAttended && attendClass == 0)
			return;

		if (attendClass > 7)
			return;

		if (attendClass == 1)
			attendClass--;
		else if (attendClass >= 6)
			attendClass = 4;
		else
			attendClass = 0;

		ClassAttend ();

		lasteventgameobject = null;
	}

	public void ClassSelectionInTutorial ()
	{
		if (_ClassAttended || !_FlatmateDownloaded)
			return;

		attendClass = 6;
		ClassAttend ();
	}

	public void ClassAttend ()
	{
		CloseToolTip ();
		if (_ClassAttended && !_FlatmateDownloaded)
			return;

		attendClass++;

		arrowClass1.SetActive (false);
		arrowClass2.SetActive (false);
		arrowClass3.SetActive (false);
		arrowClass4.SetActive (false);
		arrowClass5.SetActive (false);
		arrowClass6.SetActive (false);
		arrowClass7.SetActive (false);
		if (attendClass == 1) {
			arrowClass1.SetActive (true);
			GameObject Go = RoommateManager.Instance.RoommatesHired [1];
			arrowClass1.transform.parent = Go.transform;
			arrowClass1.transform.localPosition = new Vector3 (0f, 7.5f, 0f);

			arrowClass2.SetActive (false);
			arrowClass3.SetActive (false);
		} else if (attendClass == 2) {
			AllButtonsState (false);
			arrowClass1.transform.parent = null;
			AllButtons [13].interactable = true; // Info Button after click on Flatmate..
			arrowClass2.SetActive (true);
			ShowToolTipWithArrow ("Here you can see flatmate's details", new Vector2 (-240, -75), TooltipSide.DownLeft);

		} else if (attendClass == 3) {
			AllButtonsState (false);
			arrowClass1.transform.parent = null;
			AllButtons [39].interactable = true; // Change Perk Button
			arrowClass6.SetActive (true);
			ShowToolTipWithArrow ("Perks help you gain points quickly, Change perk from here", new Vector2 (100, -65), TooltipSide.DownLeft, 0.4f);

		} else if (attendClass == 4) {
			AllButtonsState (false);
			arrowClass1.transform.parent = null;
			AllButtons [40].interactable = true; // Upgrade Perk Button
			arrowClass7.SetActive (true);
			ShowToolTipWithArrow ("You can also Upgrade your flatmate's perks", new Vector2 (-110, -206), TooltipSide.UpLeft, 0.4f);

		} else if (attendClass == 5) {
			AllButtonsState (false);
			AllButtons [14].interactable = true; // Attend Class Buttons
			arrowClass3.SetActive (true);
			ShowToolTipWithArrow ("Attend classes to earn Xps point and Level up", new Vector2 (150, -140), TooltipSide.DownRight, 0.4f);
		} else if (attendClass == 6) {
			AllButtonsState (false);// Arrow on essay class
			arrowClass4.SetActive (true);
			ShowToolTipWithArrow ("Each class have different time, xps and education level", new Vector2 (-83, 100), TooltipSide.UpLeft, 0.4f);
		} else if (attendClass == 7) {

			AllButtonsState (false);
			arrowClass5.SetActive (true);
//			_ClassAttended = true;
		} else {
			arrowClass1.SetActive (false);
			arrowClass2.SetActive (false);
			arrowClass3.SetActive (false);
			arrowClass4.SetActive (false);
			arrowClass5.SetActive (false);
			EarnCoin ();
			_ClassAttended = true;
			PlayerPrefs.SetInt ("Tutorial_Progress", 7);
		}
	}

	public void ShowPopUpForSaloon ()
	{
		PlayerPrefs.SetInt ("Tutorial_Progress", 11);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Let's get a new Hair cut?";
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => SaloonPurchasing ());
	}

	public void SaloonPurchasing ()
	{
		if (_SaloonPurchased || purchaseSaloon > 10 || !_DressPurchased)
			return;

        if (eventsystem.currentSelectedGameObject != lasteventgameobject || lasteventgameobject == null) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);

			Invoke ("PurchaseSaloon", 0.2f);
		} else {
			DecreaseSaloon ();

		}
	}

	public void DecreaseSaloon ()
	{		
		if (_SaloonPurchased || purchaseSaloon > 10)
			return;

		if (purchaseSaloon == 5) {
			PurchaseSaloon ();
			return;
		}

		if (purchaseSaloon == 1)
			purchaseSaloon--;
		else if (purchaseSaloon < 5)
			purchaseSaloon = 0;
		else if (purchaseSaloon > 5 && purchaseSaloon <= 10)
			purchaseSaloon = 5;

		PurchaseSaloon ();
		lasteventgameobject = null;
	}

	void PurchaseSaloon ()
	{
		CloseToolTip ();
		if (_SaloonPurchased && !_DressPurchased)
			return;

		purchaseSaloon++;

		if (purchaseSaloon == 1) {
			AllButtonsState (false);
			AllButtons [0].interactable = true; // OpenClose Slider 
			arrowSaloon1.SetActive (true);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
		} else if (purchaseSaloon == 2) {
			AllButtonsState (false);
			AllButtons [3].interactable = true; // Catalouge button
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (true);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
		} else if (purchaseSaloon == 3) {
			AllButtonsState (false);
			AllButtons [9].interactable = true; //Saloon Button
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (true);
			ShowToolTipWithArrow ("You can buy new Hairs here", new Vector2 (80, 60), TooltipSide.DownLeft, 0.5f);
			arrowSaloon4.SetActive (false);
		} else if (purchaseSaloon == 4) {
			PlayerPrefs.SetInt ("Tutorial_Progress", 12);
			AllButtonsState (false); // Saloon First Item.. Move Forward Ok Click on pop up
			AllButtons [27].interactable = true;
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (true);
			arrowSaloon5.SetActive (false);
		} else if (purchaseSaloon == 5) {
			AllButtonsState (false); // On Click back from Saloon 
			AllButtons [27].interactable = true;
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
			arrowSaloon5.SetActive (true);
		} else if (purchaseSaloon == 6) {
			AllButtonsState (false);
			AllButtons [0].interactable = true; // OpenClose Slider 
			arrowSaloon1.SetActive (true);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
			arrowSaloon5.SetActive (false);
			arrowSaloon6.SetActive (false);
			arrowSaloon7.SetActive (false);
			arrowSaloon8.SetActive (false);

		} else if (purchaseSaloon == 7) {
			AllButtonsState (false);
			AllButtons [1].interactable = true; // Saloon button
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
			arrowSaloon5.SetActive (false);
			arrowSaloon6.SetActive (true);
			ShowToolTipWithArrow ("Here are your bought saloon items", new Vector2 (-110, 10), TooltipSide.UpLeft);
			arrowSaloon7.SetActive (false);
			arrowSaloon8.SetActive (false);
		} else if (purchaseSaloon == 8) {
			AllButtonsState (false);
			AllButtons [34].interactable = true;
			arrowSaloon1.SetActive (false); // Character Selection
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
			wardrobeArrow2.SetActive (true);
			ShowToolTipWithArrow ("Select a character for whom you want to change hairs", new Vector2 (-160, 130), TooltipSide.DownLeft, 0.5f);
			arrowSaloon5.SetActive (false);
			arrowSaloon6.SetActive (false);
			arrowSaloon7.SetActive (false);
			arrowSaloon8.SetActive (false);
		} else if (purchaseSaloon == 9) {
			AllButtonsState (false);
			arrowSaloon1.SetActive (false); // First item
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
			wardrobeArrow2.SetActive (false);
			arrowSaloon5.SetActive (false);
			arrowSaloon6.SetActive (false);
			arrowSaloon7.SetActive (true);
			arrowSaloon8.SetActive (false);
		} else if (purchaseSaloon == 10) {
			AllButtonsState (false); // Confirm Button But Click On Ok 
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
			wardrobeArrow2.SetActive (false);
			arrowSaloon5.SetActive (false);
			arrowSaloon6.SetActive (false);
			arrowSaloon7.SetActive (false);
			arrowSaloon8.SetActive (true);
			ShowToolTipWithArrow ("Confirm hair change", new Vector2 (80, -135), TooltipSide.DownRight, 0.5f);
		} else {
			arrowSaloon1.SetActive (false);
			arrowSaloon2.SetActive (false);
			arrowSaloon3.SetActive (false);
			arrowSaloon4.SetActive (false);
			arrowSaloon5.SetActive (false);
			arrowSaloon6.SetActive (false);
			arrowSaloon7.SetActive (false);
			arrowSaloon8.SetActive (false);
			_SaloonPurchased = true;
			PlayerPrefs.SetInt ("Tutorial_Progress", 13);
			UpdateTutorial ();
			EarnCoin ();
		}
	}

	public void Phase1EndPopup ()
	{
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "You have to reach level 2 and must hire one more flatmate to expand your flat and explore the university.";
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => UpdateTutorial ());
	}

	//	bool _shown;

	public void Phase2EndPopup ()
	{
//		_shown = true;
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "You have to reach level 1 and must hire one more flatmate to participate in quests and events.";
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => UpdateTutorial ());
	}

	public void PopUpForPublicArea ()
	{
//		PlayerPrefs.SetInt ("Tutorial_Progress", 18);

		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Do you fancy exploring the University grounds today?";
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PlayerPrefs.SetInt ("Tutorial_Progress", 20));

		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PublicAreaAccessing ());

	}

//	bool messagesent = false;

	public void PublicAreaAccessing ()
	{
		if (_PublicAreaAccessed || publicAreaAccess > 5 || !_LandPurchased)
			return;

        if (eventsystem.currentSelectedGameObject != lasteventgameobject || lasteventgameobject == null) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			arrowPubArea1.SetActive (false);
			arrowPubArea2.SetActive (false);
			arrowPubArea3.SetActive (false);
			arrowPubArea4.SetActive (false);
			arrowPubArea5.SetActive (false);

			Invoke ("AccessPublicArea", 0.2f);
		} else {
			DecreasePublicAreaAccess ();
			lasteventgameobject = null;
		}
	}

	public void DecreasePublicAreaAccess ()
	{
		CloseToolTip ();

		if (_PublicAreaAccessed || publicAreaAccess > 5)
			return;		

		if (publicAreaAccess == 1)
			publicAreaAccess--;
		else if (publicAreaAccess == 4)
			publicAreaAccess = 2;
		else if (publicAreaAccess < 4)
			publicAreaAccess = 1;
		else if (publicAreaAccess >= 5)
			print ("");
		else
			publicAreaAccess = 0;
		lasteventgameobject = null;
		CancelInvoke ("AccessPublicArea");

		Invoke ("AccessPublicArea", 0.1f);
	}

	void AccessPublicArea ()
	{
		CloseToolTip ();
		if (_PublicAreaAccessed && !_LandPurchased)
			return;

		publicAreaAccess++;

		if (publicAreaAccess == 1) {
			AllButtonsState (false);
			AllButtons [6].interactable = true; // Cell Phone Button
			arrowPubArea1.SetActive (true);
			arrowPubArea2.SetActive (false);
			arrowPubArea3.SetActive (false);
			arrowPubArea4.SetActive (false);
			arrowPubArea5.SetActive (false);
			arrowPubArea6.SetActive (false);
			arrowPubArea7.SetActive (false);
//			ShowToolTipWithArrow ("Your personal mobile.", new Vector2 (340, -45), TooltipSide.DownRight);
		} else if (publicAreaAccess == 2) {
			AllButtonsState (false);
			AllButtons [24].interactable = true; // Public area button
			arrowPubArea1.SetActive (false);
			arrowPubArea2.SetActive (true);
			arrowPubArea3.SetActive (false);
			arrowPubArea4.SetActive (false);
			arrowPubArea5.SetActive (false);
			arrowPubArea6.SetActive (false);
			arrowPubArea7.SetActive (false);
			ShowToolTipWithArrow ("Public Areas of University, Meet new peoples here", new Vector2 (110, 20), TooltipSide.DownRight, 0.5f);

		} else if (publicAreaAccess == 3) {
			AllButtonsState (false);
			//			AllButtons [26].interactable = true; // Public area list  Button
			arrowPubArea1.SetActive (false);
			arrowPubArea2.SetActive (false);
			arrowPubArea3.SetActive (true);
			arrowPubArea4.SetActive (false);
			arrowPubArea5.SetActive (false);
			arrowPubArea6.SetActive (false);
			arrowPubArea7.SetActive (false);
			ShowToolTipWithArrow ("University Garden", new Vector2 (-290, 0), TooltipSide.DownRight, 0.5f);

		} else if (publicAreaAccess == 4) {
			AllButtonsState (false); //  arrow On Input Field 
			arrowPubArea1.SetActive (false);
			arrowPubArea2.SetActive (false);
			arrowPubArea3.SetActive (false);
			arrowPubArea4.SetActive (true);			
			arrowPubArea5.SetActive (false);
			arrowPubArea6.SetActive (false);
			arrowPubArea7.SetActive (false);
			ShowToolTipWithArrow ("Start a new chat with other players", new Vector2 (100, -60), TooltipSide.DownRight, 0.5f);
		} else if (publicAreaAccess == 5) {
			AllButtonsState (false); // On Click Send Button
			AllButtons [47].interactable = true;
			arrowPubArea1.SetActive (false);
			arrowPubArea2.SetActive (false);
			arrowPubArea3.SetActive (false);
			arrowPubArea4.SetActive (false);
			arrowPubArea5.SetActive (false);
			arrowPubArea6.SetActive (true);
			arrowPubArea7.SetActive (true);
			ShowToolTipWithArrow ("Lets go back for some more new cool stuff", new Vector2 (-200, -40), TooltipSide.DownLeft, 0.5f);
			PlayerPrefs.SetInt ("Tutorial_Progress", 21);
//			messagesent = true;
		
		} else {
			arrowPubArea1.SetActive (false);
			arrowPubArea2.SetActive (false);
			arrowPubArea3.SetActive (false);
			arrowPubArea4.SetActive (false);
			arrowPubArea5.SetActive (false);
			arrowPubArea6.SetActive (false);
			arrowPubArea7.SetActive (false);
			CloseToolTip ();
			EarnCoin ();
			_PublicAreaAccessed = true;
//			PlayerPrefs.SetInt ("Tutorial_Progress", 22);
			UpdateTutorial ();
		}
	}

//	bool Checked = false;

	public void CheckForQuest ()
	{
//		if (_PublicAreaAccessed && !_QuestAttended) {
//			if(!Checked)				
//				Invoke ("IntroductionToAttendQuest", 0.3f);
//			Checked = true;
//		}
	}

	public void IntroductionToAttendQuest ()
	{
//		PlayerPrefs.SetInt ("Tutorial_Progress", 20);
//		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
//		ScreenManager.Instance.ClosePopup ();
//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
//		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
//		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Welcome " + PlayerPrefs.GetString ("UserName") + " let's participate in the quest!";
//		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => AttendQuest ());
//		arrowPubArea6.SetActive (false);
//		arrowPubArea7.SetActive (false);
	}


	public void AttendQuest ()
	{		
		//		GameManager.Instance.ChangeLevel ();

//		if (GameManager.Instance.level >= 1 && RoommateManager.Instance.RoommatesHired.Length >= 2 && !_QuestAttended && _PublicAreaAccessed) {
//			ScreenManager.Instance.ClosePopup ();
//			print ("Totorial to follow to attend in the Quest");
//
//			if (eventsystem.currentSelectedGameObject != lasteventgameobject) {
//				lasteventgameobject = eventsystem.currentSelectedGameObject;
//				questAttended1.SetActive (false);
//				questAttended2.SetActive (false);
//				questAttended3.SetActive (false);
//				questAttended4.SetActive (false);
//				questAttended5.SetActive (false);
//				questAttended6.SetActive (false);
//				questAttended7.SetActive (false);
//				questAttended8.SetActive (false);
//				questAttended9.SetActive (false);
//				questAttended10.SetActive (false);
//				questAttended11.SetActive (false);
//				questAttended12.SetActive (false);
//				questAttended13.SetActive (false);
//
//				Invoke ("QuestAttendingStart", 0.2f);	
//			} else {
//				DecreaseQuestAttended ();
//				lasteventgameobject = null;
//			}
//		}
	}

	public void DecreaseQuestAttended ()
	{
//		if (_QuestAttended && questAttended > 13)
//			return;
//
//		if (questAttended == 2)
//			questAttended--;
//		else if (questAttended <= 5)
//			questAttended = 2;
//		else if (questAttended > 5 && questAttended <= 13)
//			questAttended = 5;
//		else
//			questAttended = 0;
//		QuestAttendingStart ();
	}

	public void QuestAttendingStart ()
	{
//		CloseToolTip ();
//		if (_QuestAttended && !_PublicAreaAccessed)
//			return;
//
//		questAttended++;
//
//		if (questAttended == 1) {
//			AllButtonsState (false);
//			AllButtons [6].interactable = true; // Cell Phone Button
//			questAttended1.SetActive (true);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//			ShowToolTipWithArrow ("A new Quest Unlocked, lets take a look", new Vector2 (340, -45), TooltipSide.DownRight);
//
//		} else if (questAttended == 2) {
//			AllButtonsState (false);
//			AllButtons [21].interactable = true; // Event Create Button
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (true);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//			ShowToolTipWithArrow ("Here are new quests, complete them to earn rewards", new Vector2 (145, 100), TooltipSide.DownRight);
//
//
//		} else if (questAttended == 3) {
//			AllButtonsState (false);
//			AllButtons [28].interactable = false;
//			EventManager.Instance.QuestContainer.transform.GetChild (1).gameObject.SetActive (false);
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (true);// Event Selection on start
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//			ShowToolTipWithArrow ("Each Quest have number of tasks", new Vector2 (230, -140), TooltipSide.UpRight);
//
//
//		} else if (questAttended == 4) {
//			AllButtonsState (false); 
//			AllButtons [29].interactable = false;
//			EventManager.Instance.TaskContainer.transform.GetChild (1).gameObject.SetActive (false);
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (true);// Select Task Inside Event			
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//			ShowToolTipWithArrow ("Complete these tasks to complete a Quest", new Vector2 (60, 90), TooltipSide.DownRight);
//
//		} else if (questAttended == 5) {
//			AllButtonsState (false); // On Click Send Button
//			AllButtons [26].interactable = false;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (true); // DressPurchse Screen
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//
//		} else if (questAttended == 6) {
//			PlayerPrefs.SetInt ("Tutorial_Progress", 21);
//			AllButtonsState (false); // 
//			AllButtons [26].interactable = true;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (true); // Back To task Screen
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//
//		} else if (questAttended == 7) {
//			AllButtonsState (false); // On Click Send Button
//			ScreenAndPopupCall.Instance.TaskScreenSelection ();
//			EventManager.Instance.QuestContainer.transform.GetChild (0).GetComponent<Events> ().InitializeTasks ();
//			EventManager.Instance.TaskContainer.transform.GetChild (1).gameObject.SetActive (true);
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (true); // 
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//
//		} else if (questAttended == 8) {
//			AllButtonsState (false); // On Click Send Button
//			AllButtons [27].interactable = false;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (true); // buy hair
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//
//		} else if (questAttended == 9) {
//			PlayerPrefs.SetInt ("Tutorial_Progress", 22);
//			AllButtonsState (false); // On Click back Button
//			AllButtons [27].interactable = true;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (true);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//
//		} else if (questAttended == 10) {
//			AllButtonsState (false); // On Click Send Button
//			ScreenAndPopupCall.Instance.QuestScreenSelection ();
//			EventManager.Instance.QuestContainer.transform.GetChild (1).gameObject.SetActive (true);
//			AllButtons [28].interactable = false;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (true);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//			ShowToolTipWithArrow ("Lets see next task", new Vector2 (260, -80), TooltipSide.DownRight);
//
//		} else if (questAttended == 11) {
//			AllButtonsState (false); // On Click Send Button
//			AllButtons [28].interactable = false;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (true);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (false);
//
//		} else if (questAttended == 12) {
//			AllButtonsState (false); // On Click Send Button
//			AllButtons [28].interactable = true;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (true);
//			questAttended13.SetActive (false);
//
//		} else if (questAttended == 13) {
//
//			AllButtonsState (false); // On Click Send Button
//			ScreenAndPopupCall.Instance.CloseScreen ();
//			ScreenAndPopupCall.Instance.QuestScreenSelection ();
//			AllButtons [28].interactable = true;
//			questAttended1.SetActive (false);
//			questAttended2.SetActive (false);
//			questAttended3.SetActive (false);
//			questAttended4.SetActive (false);
//			questAttended5.SetActive (false);
//			questAttended6.SetActive (false);
//			questAttended7.SetActive (false);
//			questAttended8.SetActive (false);
//			questAttended9.SetActive (false);
//			questAttended10.SetActive (false);
//			questAttended11.SetActive (false);
//			questAttended12.SetActive (false);
//			questAttended13.SetActive (true);
//
//			EarnCoin ();
//			_QuestAttended = true;
//			//			GameManager.Instance.ChangeLevel ();
//		} 

	}

	public void CheckFashionShowEvent ()
	{
		if (_SaloonPurchased && !_FashionEventCompleate) {
//			questAttended13.SetActive (false);
			//			UpdateTutorial ();
			Invoke ("IntroductionToFashionEvent", 0.2f);
		}
	}

	public void IntroductionToFashionEvent ()
	{
//		PlayerPrefs.SetInt ("Tutorial_Progress", 24);
		PlayerPrefs.SetInt ("Tutorial_Progress", 14);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Have you seen all of the events going on around the University for Freshers Week?" + "\n \nThis looks like fun, lets give it a go!";
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => FashionEventStart ());
		arrowPubArea6.SetActive (false);
		arrowPubArea7.SetActive (false);
	}



	public void FashionEventStart ()
	{
		//		GameManager.Instance.ChangeLevel ();
		if (_FashionEventCompleate || !_SaloonPurchased)
			return;
		ScreenManager.Instance.ClosePopup ();
		print ("Totorial to follow to particapate in fashion show");

		if (eventsystem.currentSelectedGameObject != lasteventgameobject || lasteventgameobject == null) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent14.SetActive (false);

			Invoke ("FashionShowEventTutorial", 0.2f);
		} else {
			DecreaseFashionEvent ();
			lasteventgameobject = null;
		}
	}

	public void DecreaseFashionEvent ()
	{
		if (_FashionEventCompleate || fashionEvent > 15)
			return;
		lasteventgameobject = eventsystem.currentSelectedGameObject;
		if (fashionEvent == 3)
			fashionEvent -= 2;
		else if (fashionEvent <= 5)
			fashionEvent -= 2;
		else if (fashionEvent > 5 && fashionEvent <= 15)
			fashionEvent = 9;
		else
			fashionEvent = 0;
		FashionShowEventTutorial ();
	}

	void FashionShowEventTutorial ()
	{
		CloseToolTip ();
		if (_FashionEventCompleate && !_SaloonPurchased)
			return;
		fashionEvent++;

		if (fashionEvent == 1) {
			AllButtonsState (false);
			AllButtons [6].interactable = true; // Cell Phone Button
			fashionEvent1.SetActive (true);  // Cell Phone Button
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);
			ShowToolTipWithArrow ("New University event launched", new Vector2 (240, -60), TooltipSide.DownRight, 0.5f);


		} else if (fashionEvent == 2) {
			AllButtonsState (false);
			AllButtons [23].interactable = true; // Fashion Event Create Button
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (true); // FashionEvent Button
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);
			ShowToolTipWithArrow ("University events rests here", new Vector2 (260, -65), TooltipSide.UpRight, 0.5f);
		} else if (fashionEvent == 3) {
//			// Popup massage for register and vote
//			AllButtonsState (false);
//			AllButtons [28].interactable = false;
//			fashionEvent1.SetActive (false);
//			fashionEvent2.SetActive (false);
//			fashionEvent3.SetActive (true);
//			fashionEvent4.SetActive (false);
//			fashionEvent5.SetActive (false);
//			fashionEvent6.SetActive (false);
//			fashionEvent7.SetActive (false);
//			fashionEvent8.SetActive (false);
//			fashionEvent9.SetActive (false);
//			fashionEvent10.SetActive (false);
//			fashionEvent11.SetActive (false);
//			fashionEvent12.SetActive (false);
//			fashionEvent13.SetActive (false);
//			fashionEvent14.SetActive (false);
//			fashionEvent15.SetActive (false);
//
//		} else if (fashionEvent == 4) {
			AllButtonsState (false); 
			AllButtons [33].interactable = false;
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (true); // Event List in Event Screen
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);
		} else if (fashionEvent == 4) {
			AllButtonsState (false);
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (true);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);
			ShowToolTipWithArrow ("Click here to Enter into this event", new Vector2 (60, -120), TooltipSide.UpLeft, 0.5f);	

		} else if (fashionEvent == 5) {
			AllButtonsState (false); // 
			AllButtons [34].interactable = false;
			AllButtons [36].interactable = false;
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (true); 
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);
			ShowToolTipWithArrow ("Select a character to participate in the event", new Vector2 (260, 70), TooltipSide.DownLeft, 0.5f);	

		} else if (fashionEvent == 6) {
			AllButtonsState (false); 
			AllButtons [35].interactable = false;
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (true);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);
		} else if (fashionEvent == 7) {
			AllButtonsState (false); 
			AllButtons [36].interactable = true;
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (true); 
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);
			ShowToolTipWithArrow ("Confirm and Enter", new Vector2 (80, -130), TooltipSide.DownRight, 0.5f);

		} else if (fashionEvent == 8) {
			AllButtonsState (false);
			AllButtons [6].interactable = false; // Register button true
			// On click ok of popup
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false); 
			fashionEvent9.SetActive (false); 
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);

		} else if (fashionEvent == 9) {
			Invoke ("EventVoting", 0.2f);
			AllButtonsState (false); 
//			AllButtons [23].interactable = true;//to check
//			AllButtons [6].interactable = true;//to check
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (true);
			fashionEvent15.SetActive (false);


		} else if (fashionEvent == 10) {
//			AllButtonsState (false);
//			AllButtons [23].interactable = true;
//			fashionEvent1.SetActive (false);
//			fashionEvent2.SetActive (false);
//			fashionEvent3.SetActive (false);
//			fashionEvent4.SetActive (false);
//			fashionEvent5.SetActive (false);
//			fashionEvent6.SetActive (false);
//			fashionEvent7.SetActive (false);
//			fashionEvent8.SetActive (false);
//			fashionEvent9.SetActive (false);
//			fashionEvent10.SetActive (false);// Voting Screen
//			fashionEvent11.SetActive (true); 
//			fashionEvent12.SetActive (false);
//			fashionEvent13.SetActive (false);
//			fashionEvent14.SetActive (false);
//			fashionEvent15.SetActive (false);
//		} else if (fashionEvent == 12) {
//			AllButtonsState (false);
//			fashionEvent1.SetActive (false);
//			fashionEvent2.SetActive (false);
//			fashionEvent3.SetActive (false);
//			fashionEvent4.SetActive (false);
//			fashionEvent5.SetActive (false);
//			fashionEvent6.SetActive (false);
//			fashionEvent7.SetActive (false);
//			fashionEvent8.SetActive (false);
//			fashionEvent9.SetActive (false);
//			fashionEvent10.SetActive (false);
//			fashionEvent11.SetActive (false); // Voting Screen
//			fashionEvent12.SetActive (true);
//			fashionEvent13.SetActive (false);
//			fashionEvent14.SetActive (false);
//			fashionEvent15.SetActive (false);
//		} else if (fashionEvent == 13) {
//			AllButtonsState (false); 
//			AllButtons [29].interactable = true;
//			fashionEvent1.SetActive (false);
//			fashionEvent2.SetActive (false);
//			fashionEvent3.SetActive (false);
//			fashionEvent4.SetActive (false);
//			fashionEvent5.SetActive (false);
//			fashionEvent6.SetActive (false);
//			fashionEvent7.SetActive (false);
//			fashionEvent8.SetActive (false);
//			fashionEvent9.SetActive (false);
//			fashionEvent10.SetActive (false);
//			fashionEvent11.SetActive (false);
//			fashionEvent12.SetActive (false);
//			fashionEvent13.SetActive (true);
//			fashionEvent14.SetActive (false);
//			fashionEvent15.SetActive (false);
//
//		} else if (fashionEvent == 14) {
//			AllButtonsState (false); 
//			AllButtons [29].interactable = true;
//			fashionEvent1.SetActive (false);
//			fashionEvent2.SetActive (false);
//			fashionEvent3.SetActive (false);
//			fashionEvent4.SetActive (false);
//			fashionEvent5.SetActive (false);
//			fashionEvent6.SetActive (false);
//			fashionEvent7.SetActive (false);
//			fashionEvent8.SetActive (false);
//			fashionEvent9.SetActive (false);
//			fashionEvent10.SetActive (false);
//			fashionEvent11.SetActive (false);
//			fashionEvent12.SetActive (false);
//			fashionEvent13.SetActive (false);
//			fashionEvent14.SetActive (true);
//			fashionEvent15.SetActive (false);
//			ShowToolTipWithArrow ("Click here to vote other players who are participating in this event", new Vector2 (-120, -30), TooltipSide.DownLeft);	
//
//		} else if (fashionEvent == 15) {
			AllButtonsState (false); 
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (true);
            ShowToolTipWithArrow ("Tap here to vote for player with nice outfit", new Vector2 (-270, -200), TooltipSide.UpLeft, 0.4f);	

		} else {
			AllButtonsState (false); 
			AllButtons [29].interactable = true;
			fashionEvent1.SetActive (false);
			fashionEvent2.SetActive (false);
			fashionEvent3.SetActive (false);
			fashionEvent4.SetActive (false);
			fashionEvent5.SetActive (false);
			fashionEvent6.SetActive (false);
			fashionEvent7.SetActive (false);
			fashionEvent8.SetActive (false);
			fashionEvent9.SetActive (false);
			fashionEvent10.SetActive (false);
			fashionEvent11.SetActive (false);
			fashionEvent12.SetActive (false);
			fashionEvent13.SetActive (false);
			fashionEvent14.SetActive (false);
			fashionEvent15.SetActive (false);

			EarnCoin ();
			_FashionEventCompleate = true;
			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
			VotingManager.Instance._Player2Voting.interactable = true;
			VotingManager.Instance._Player1Voting.interactable = true;
		} 

	}


	void EventVoting ()
	{
		ShowToolTipWithArrow (PlayerPrefs.GetString ("UserName") + ", we can vote for others while we wait!", new Vector2 (60, -120), TooltipSide.UpLeft, 0.5f);

		ScreenAndPopupCall.Instance.EventsIntroScreenCalled ();
//		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Back").GetComponent <Button> ().interactable = false;
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Discription").GetComponent<Text> ().text = "Welcome to Fashion Show Event. Let's participate in Fashion Show Event";
//		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Event Logo").GetComponent<Image> ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").gameObject.SetActive (false);
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").gameObject.SetActive (true);
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponentInChildren<Text> ().text = "VOTE";
		ScreenManager.Instance.EventsIntroScreen.transform.GetChild (0).FindChild ("Back").GetComponent<Button> ().interactable = false;
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Register").GetComponent<Button> ().onClick.AddListener (() => { 
			EventManagment.Instance.EventType = eType.Voting;
			ScreenManager.Instance.ClosePopup (); 
			GameManager.Instance.GetComponent<Tutorial> ().FashionEventStart ();
			StartCoroutine (ScreenAndPopupCall.Instance.ActiveCameraForVoting (RoommateManager.Instance.SelectedRoommate));
			StartCoroutine (ScreenAndPopupCall.Instance.ActiveCameraForAIVoting (RoommateManager.Instance.RoommatesHired [1]));
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);	
			ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("CloseButton").gameObject.SetActive (false);	

//			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Back").GetComponent <Button> ().interactable = true;
			ScreenAndPopupCall.Instance.VotingScreenSelection ();
			VotingManager.Instance.OnIntelazition ();
		});
	}

	public void RecruitFlatmate ()
	{
		//		GameManager.Instance.ChangeLevel ();
		if (_FlatmateRecruited || !_FashionEventCompleate)
			return;
		ScreenManager.Instance.ClosePopup ();

		if (eventsystem.currentSelectedGameObject != lasteventgameobject || lasteventgameobject == null) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;

			Invoke ("RecruitFlatmateTutorial", 0.2f);

		} else {
			RecruitLevelDecrease ();
			lasteventgameobject = null;
		}
	}

	public void RecruitLevelDecrease ()
	{
		if (_FlatmateRecruited)
			return;

		if (recruitFlatmate == 1)
			recruitFlatmate--;
		else if (recruitFlatmate > 4)
			recruitFlatmate = 1;
		else
			recruitFlatmate = 0;

		RecruitFlatmateTutorial ();
	}

	void RecruitFlatmateTutorial ()
	{
		CloseToolTip ();

		arrowRecruit1.SetActive (false);
		arrowRecruit2.SetActive (false);
		arrowRecruit3.SetActive (false);
		arrowRecruit4.SetActive (false);

		if (_FlatmateRecruited && !_FashionEventCompleate)
			return;
		recruitFlatmate++;
		if (recruitFlatmate == 1) {
			ShowPopUpWithMessage ("It’s getting lonely around here! We should increase our ranks.", () => RecruitFlatmate ());
		} else if (recruitFlatmate == 2) {
			AllButtonsState (false);
			AllButtons [0].interactable = true; // Main Menu Button
			arrowRecruit1.SetActive (true);  
			ShowToolTipWithArrow ("Let's Recruit a new flatmate", new Vector2 (-190, -100), TooltipSide.DownLeft, 0.5f);

		} else if (recruitFlatmate == 3) {
			AllButtonsState (false);
			AllButtons [3].interactable = true; // Catalouge Button
			arrowRecruit2.SetActive (true);
		} else if (recruitFlatmate == 4) {
			AllButtonsState (false);
			AllButtons [10].interactable = true; // Recruit Button
			arrowRecruit3.SetActive (true);
			ShowToolTipWithArrow ("New Flatmates", new Vector2 (160, 70), TooltipSide.DownLeft, 0.5f);
		} else if (recruitFlatmate == 5) {
			AllButtonsState (false);
//			AllButtons [3].interactable = true; // Catalouge Button
			var ButtonTrans = RoommateManager.Instance.AllFlatmatesContainer.transform.GetChild (0).GetComponent <RectTransform> ();
			arrowRecruit4.GetComponent <RectTransform> ().anchoredPosition = new Vector2 (ButtonTrans.anchoredPosition.x + ButtonTrans.rect.width / 1.5f, ButtonTrans.anchoredPosition.y);
			arrowRecruit4.SetActive (true);
		} else if (recruitFlatmate == 6) {
			AllButtonsState (false);
		} else {
			_FlatmateRecruited = true;
			PlayerPrefs.SetInt ("Tutorial_Progress", 16);
			UpdateTutorial ();
		}
	}


	public void EarnCoin ()
	{
//		GameManager.Instance.AddCoins (10);
//		GameManager.Instance.AddExperiencePoints (5f);
	}


	public RectTransform ToolTipDown;
	public RectTransform ToolTipUp;

	enum TooltipSide
	{
		DownLeft,
		DownRight,
		UpRight,
		UpLeft

	}



	void ShowToolTipWithArrow (string Message, Vector2 AnchoredPos, TooltipSide Side, float scaleFactor = 0.5f)
	{
		StartCoroutine (IShowToolTipWithArrow (Message, AnchoredPos, Side, new Vector2 (512f, 262f) * scaleFactor));
	}

	IEnumerator  IShowToolTipWithArrow (string _message, Vector2 anchoredPos, TooltipSide side, Vector2 scale)
	{
		yield return new WaitForSeconds (0.4f);

		if (side == TooltipSide.DownLeft) {
			ToolTipDown.gameObject.SetActive (true);
			ToolTipDown.GetComponentInChildren <Text> ().text = _message;
			ToolTipDown.transform.FindChild ("Image").localScale = new Vector2 (1, 1);
			ToolTipDown.anchoredPosition = anchoredPos;
			ToolTipDown.sizeDelta = scale;

		} else if (side == TooltipSide.DownRight) {
			ToolTipDown.gameObject.SetActive (true);
			ToolTipDown.GetComponentInChildren <Text> ().text = _message;
			ToolTipDown.transform.FindChild ("Image").localScale = new Vector2 (-1, 1);
			ToolTipDown.anchoredPosition = anchoredPos;
			ToolTipDown.sizeDelta = scale;

		} else if (side == TooltipSide.UpLeft) {
			ToolTipUp.gameObject.SetActive (true);
			ToolTipUp.GetComponentInChildren <Text> ().text = _message;
			ToolTipUp.transform.FindChild ("Image").localScale = new Vector2 (1, 1);
			ToolTipUp.anchoredPosition = anchoredPos;
			ToolTipUp.sizeDelta = scale;

		} else if (side == TooltipSide.UpRight) {
			ToolTipUp.gameObject.SetActive (true);
			ToolTipUp.GetComponentInChildren <Text> ().text = _message;
			ToolTipUp.transform.FindChild ("Image").localScale = new Vector2 (-1, 1);
			ToolTipUp.anchoredPosition = anchoredPos; 
			ToolTipUp.sizeDelta = scale;

		}
	}

	void CloseToolTip ()
	{
		ToolTipDown.gameObject.SetActive (false);
		ToolTipDown.transform.localPosition = Vector3.zero;
		ToolTipUp.gameObject.SetActive (false);
		ToolTipUp.transform.localPosition = Vector3.zero;
	}

	public void SocietyTutorial ()
	{
		if (_SocietyCreated || societyTutorial > 19 || !_PublicAreaAccessed)
			return;

		if (eventsystem.currentSelectedGameObject != lasteventgameobject || lasteventgameobject == null) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			arrowSociety1.SetActive (false);
			arrowSociety2.SetActive (false);
			arrowSociety3.SetActive (false);
			arrowSociety4.SetActive (false);
			arrowSociety5.SetActive (false);
			arrowSociety6.SetActive (false);
			arrowSociety7.SetActive (false);
			arrowSociety8.SetActive (false);

			CancelInvoke ("SocietyCreateTutorial");
			Invoke ("SocietyCreateTutorial", 0.2f);
		} else {
			DecreaseSocietyTutorial ();
			lasteventgameobject = null;
		}
	}

	public void DecreaseSocietyTutorial ()
	{
		CloseToolTip ();

		if (_SocietyCreated || societyTutorial > 17)
			return;     

		if (societyTutorial < 4)
			societyTutorial = 0;
		else if (societyTutorial == 4)
			societyTutorial = 2;
		else if (societyTutorial == 5)// doubt here......
            societyTutorial = 2;
//        else if (societyTutorial >= 5)
//            print ("");        
        else
			societyTutorial -= 2;
		lasteventgameobject = null;
		CancelInvoke ("SocietyCreateTutorial");

		Invoke ("SocietyCreateTutorial", 0.1f);
	}

	public void BackInCreateSociety ()
	{
		if (_SocietyCreated || societyTutorial > 19)
			return;   
		societyTutorial = 2;

		CancelInvoke ("SocietyCreateTutorial");

		Invoke ("SocietyCreateTutorial", 0.1f);
	}

	public void ResetSocietyTutorialToZero ()
	{
		if (_SocietyCreated || societyTutorial > 19)
			return;   
		societyTutorial = 0;

		CancelInvoke ("SocietyCreateTutorial");

		Invoke ("SocietyCreateTutorial", 0.1f);
        
	}

	void SocietyCreateTutorial ()
	{
		CloseToolTip ();
		if (_SocietyCreated && !_PublicAreaAccessed)
			return;

		arrowSociety1.SetActive (false);
		arrowSociety2.SetActive (false);
		arrowSociety3.SetActive (false);
		arrowSociety4.SetActive (false);
		arrowSociety5.SetActive (false);
		arrowSociety6.SetActive (false);
		arrowSociety7.SetActive (false);
		arrowSociety8.SetActive (false);

		AllButtonsState (false);

		societyTutorial++;  

		if (societyTutorial == 1) {
			AllButtons [6].interactable = true; // Cell Phone Button
			arrowSociety1.SetActive (true);
			selectTagArrowShowed = false;
			tagsArrowShowed = false;
    
		} else if (societyTutorial == 2) {
			AllButtons [25].interactable = true; // Societies button
			arrowSociety2.SetActive (true);         
			ShowToolTipWithArrow ("Societies!!", new Vector2 (180, 80), TooltipSide.DownRight, 0.5f);

		} else if (societyTutorial == 3) {
			AllButtons [41].interactable = true; // My society  Button
  
			arrowSociety3.SetActive (true);
        
			ShowToolTipWithArrow ("Your Society", new Vector2 (240, 25), TooltipSide.DownLeft, 0.5f);

		} else if (societyTutorial == 4) {

			arrowSociety4.SetActive (true);         
   
			ShowToolTipWithArrow ("Lets Create a new Society", new Vector2 (240, 25), TooltipSide.DownLeft, 0.5f);
		} else if (societyTutorial == 5) {
			arrowSociety5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -15);
			arrowSociety5.SetActive (true);
       
			ShowToolTipWithArrow ("Type name of your society.", new Vector2 (35, 55), TooltipSide.DownLeft);

		} else if (societyTutorial == 6) {
			ShowToolTipWithArrow ("Tap here to confirm.", new Vector2 (200, -110), TooltipSide.DownLeft);
			arrowSociety6.SetActive (true);// Next Button
		} else if (societyTutorial == 7) {
			arrowSociety5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-3, -10);
			arrowSociety5.SetActive (true); //Submit Description
			ShowToolTipWithArrow ("Write a short description of your society.", new Vector2 (40, 60), TooltipSide.DownLeft);

		} else if (societyTutorial == 8) {
			arrowSociety6.SetActive (true);// Next Button
			selectTagArrowShowed = false;
			tagsArrowShowed = false;
		} else if (societyTutorial == 9) {
			arrowSociety5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-15, 50);
			arrowSociety5.SetActive (true); // Select Tags
			if (!selectTagArrowShowed) {
				ShowToolTipWithArrow ("Tap here to select tags", new Vector2 (25, 120), TooltipSide.DownLeft);
				selectTagArrowShowed = true;
			}
		} else if (societyTutorial == 10) {
			// arrow on Tags List        
			arrowSociety7.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 135);            
			arrowSociety7.SetActive (true);   
			if (!tagsArrowShowed) {
				ShowToolTipWithArrow ("Choose Atleast 5 tags", new Vector2 (175, 190), TooltipSide.DownLeft);            
				tagsArrowShowed = true;
			}      
		} else if (societyTutorial == 11) {
			arrowSociety6.SetActive (true);// Next Button

		} else if (societyTutorial == 12) {          
			arrowSociety5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-15, 10);            
			arrowSociety5.SetActive (true); 
			ShowToolTipWithArrow ("Tap here to choose an emblem.", new Vector2 (30, 70), TooltipSide.DownLeft);
		} else if (societyTutorial == 13) {
			// arrow on Emblems            
			arrowSociety7.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-20, 160);          
			arrowSociety7.SetActive (true);
			ShowToolTipWithArrow ("Select an emblem to display.", new Vector2 (140, 190), TooltipSide.DownLeft);
		} else if (societyTutorial == 14) {
			arrowSociety6.SetActive (true);// Next Button
		} else if (societyTutorial == 15) {           
			arrowSociety5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-15, 5);   
			arrowSociety5.SetActive (true); // Select Room
			ShowToolTipWithArrow ("Tap here to select a default room of your society.", new Vector2 (25, 75), TooltipSide.DownLeft);
		} else if (societyTutorial == 16) {
			// arrow on Rooms            
			arrowSociety7.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-20, 160);   
            
			arrowSociety7.SetActive (true);

			ShowToolTipWithArrow ("Select a default room of your society.", new Vector2 (220, 160), TooltipSide.DownLeft);
		} else if (societyTutorial == 17) {
			arrowSociety6.SetActive (true);// Next Button
		}

//        else if(societyTutorial == 15)
//        {
//            arrowSociety5.SetActive (true); // Select Tags
//        } 
//        else if(societyTutorial == 16)
//        {
//            arrowSociety6.SetActive (true);// Next Button
//        }  

        else if (societyTutorial == 18) {
			ShowPopUpWithMessage ("Your Society is created. This is only for to show you tutorial. You can create your own society any time in game.", () => {
				SocietyCreateTutorial ();
			});
		} else if (societyTutorial == 19) {
			arrowSociety8.SetActive (true);//On Back Button of Society Description page.
		} else {
       
			CloseToolTip ();
			EarnCoin ();
			_SocietyCreated = true;
			PlayerPrefs.SetInt ("Tutorial_Progress", 23);
			UpdateTutorial ();
		}
	}

	bool selectTagArrowShowed;
	bool tagsArrowShowed;

	public void OnCloseCondition (int Value)
	{
		if (societyTutorial == Value) {
			DecreaseSocietyTutorial ();
		}  
	}


	public void HostPartyTutorial ()
	{
		if (HostPartyCreated || hostPartyTutorial > 11 || !_SocietyCreated)
			return;

		if (eventsystem.currentSelectedGameObject != lasteventgameobject || lasteventgameobject == null) {
			lasteventgameobject = eventsystem.currentSelectedGameObject;
			arrowFlatParty1.SetActive (false);
			arrowFlatParty2.SetActive (false);
			arrowFlatParty3.SetActive (false);
			arrowFlatParty4.SetActive (false);
			arrowFlatParty5.SetActive (false);
			arrowFlatParty6.SetActive (false);
			arrowFlatParty7.SetActive (false);
			arrowFlatParty8.SetActive (false);
			arrowFlatParty9.SetActive (false);
			arrowFlatParty10.SetActive (false);

			CancelInvoke ("HostPartyCreateTutorial");
			Invoke ("HostPartyCreateTutorial", 0.2f);
		} else {
			DecreaseHostPartyTutorial ();
			lasteventgameobject = null;
		}
	}

	public void DecreaseHostPartyTutorial ()
	{
		CloseToolTip ();

		if (HostPartyCreated || hostPartyTutorial > 11)
			return;     

		if (hostPartyTutorial <= 10)
			hostPartyTutorial = 0;
		else
			hostPartyTutorial -= 2;
		lasteventgameobject = null;
		CancelInvoke ("HostPartyCreateTutorial");

		Invoke ("HostPartyCreateTutorial", 0.1f);
	}

	public void BackInCreateHostParty ()
	{
		if (HostPartyCreated || hostPartyTutorial > 11)
			return;   
		hostPartyTutorial = 2;

		CancelInvoke ("HostPartyCreateTutorial");

		Invoke ("HostPartyCreateTutorial", 0.1f);
	}

	public void ResetHostPartyTutorialToZero ()
	{
		if (HostPartyCreated || hostPartyTutorial > 19)
			return;   
		hostPartyTutorial = 0;

		CancelInvoke ("HostPartyCreateTutorial");

		Invoke ("HostPartyCreateTutorial", 0.1f);

	}




	void HostPartyCreateTutorial ()
	{
		CloseToolTip ();
		if (HostPartyCreated && !_SocietyCreated)
			return;

		arrowFlatParty1.SetActive (false);
		arrowFlatParty2.SetActive (false);
		arrowFlatParty3.SetActive (false);
		arrowFlatParty4.SetActive (false);
		arrowFlatParty5.SetActive (false);
		arrowFlatParty6.SetActive (false);
		arrowFlatParty7.SetActive (false);
		arrowFlatParty8.SetActive (false);
		arrowFlatParty9.SetActive (false);
		arrowFlatParty10.SetActive (false);

		AllButtonsState (false);

		hostPartyTutorial++;  

		if (hostPartyTutorial == 1) {
			AllButtons [6].interactable = true; // Cell Phone Button
			arrowFlatParty1.SetActive (true);

		} else if (hostPartyTutorial == 2) {
			AllButtons [22].interactable = true; // Host party button
			arrowFlatParty2.SetActive (true);         
			ShowToolTipWithArrow ("Host flat party!!", new Vector2 (180, 140), TooltipSide.DownRight);

		} else if (hostPartyTutorial == 3) {
			AllButtons [44].interactable = true; // My Host party  Button

			arrowFlatParty3.SetActive (true);

			ShowToolTipWithArrow ("My host party list.", new Vector2 (50, -30), TooltipSide.DownRight);

		} else if (hostPartyTutorial == 4) {
			AllButtons [44].interactable = true; // create host party button
			arrowFlatParty4.SetActive (true);       

			ShowToolTipWithArrow ("Let's host a new flat party.", new Vector2 (170, 140), TooltipSide.DownLeft);
		
		} else if (hostPartyTutorial == 5) {
			
//			arrowFlatParty5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-139, -36);// 
			FlatPartyHostingControler.Instance.HostPartyName.interactable = true;
			FlatPartyHostingControler.Instance.HostPartyDiscription.interactable = false;
			FlatPartyHostingControler.Instance.PartyTimeDropDown.interactable = false;
			FlatPartyHostingControler.Instance.TotelGuestDropDown.interactable = false;
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = false;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = false;
			arrowFlatParty5.SetActive (true);

			ShowToolTipWithArrow ("Type flat party name that you want to host.", new Vector2 (250, 20), TooltipSide.UpLeft);

		} else if (hostPartyTutorial == 6) {
			FlatPartyHostingControler.Instance.HostPartyName.interactable = false;
			FlatPartyHostingControler.Instance.HostPartyDiscription.interactable = true;
			FlatPartyHostingControler.Instance.PartyTimeDropDown.interactable = false;
			FlatPartyHostingControler.Instance.TotelGuestDropDown.interactable = false;
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = false;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = false;
			ShowToolTipWithArrow ("Write a short description of your flat party.", new Vector2 (250, -35), TooltipSide.UpLeft);
			arrowFlatParty6.SetActive (true);// Next Button
		} else if (hostPartyTutorial == 7) {
			FlatPartyHostingControler.Instance.HostPartyName.interactable = false;
			FlatPartyHostingControler.Instance.HostPartyDiscription.interactable = false;
			FlatPartyHostingControler.Instance.PartyTimeDropDown.interactable = false;
			FlatPartyHostingControler.Instance.TotelGuestDropDown.interactable = true;
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = false;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = false;
//			arrowFlatParty7.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-139, 25);
			arrowFlatParty7.SetActive (true); //Submit Description
			ShowToolTipWithArrow ("Select member limit for your flat party.", new Vector2 (80, 65), TooltipSide.DownLeft);

		} else if (hostPartyTutorial == 8) {
			FlatPartyHostingControler.Instance.HostPartyName.interactable = false;
			FlatPartyHostingControler.Instance.HostPartyDiscription.interactable = false;
			FlatPartyHostingControler.Instance.PartyTimeDropDown.interactable = true;
			FlatPartyHostingControler.Instance.TotelGuestDropDown.interactable = false;
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = false;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = false;
//			arrowSociety5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-139, 25);
			arrowFlatParty8.SetActive (true); //Submit Description
			ShowToolTipWithArrow ("Select the party duration till what time you want to have party.", new Vector2 (310, 65), TooltipSide.DownLeft);
		} else if (hostPartyTutorial == 9) {
			FlatPartyHostingControler.Instance.HostPartyName.interactable = false;
			FlatPartyHostingControler.Instance.HostPartyDiscription.interactable = false;
			FlatPartyHostingControler.Instance.PartyTimeDropDown.interactable = false;
			FlatPartyHostingControler.Instance.TotelGuestDropDown.interactable = false;
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = false;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = true;
//			arrowSociety5.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-139, 25);
			arrowFlatParty9.SetActive (true); //Submit Description
			ShowToolTipWithArrow ("Check the private toggle to host a private party.", new Vector2 (260, 15), TooltipSide.DownLeft);
		} else if (hostPartyTutorial == 10) {
			AllButtons [45].interactable = true; // Host party Button
			arrowFlatParty10.SetActive (true); 
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = false;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = false;
			ShowToolTipWithArrow ("Wow!! Let's host the flat party.", new Vector2 (230, -3), TooltipSide.DownLeft);
			GameObject.Find ("PartyScreen").transform.GetChild (3).gameObject.SetActive (false);
		} else if (hostPartyTutorial == 11) {
			StartCoroutine (ShowFinalTutIndication ());
		} 
	}

	IEnumerator ShowFinalTutIndication ()
	{
		yield return new WaitForSeconds (3f);
		ShowPopUpWithMessage ("Your flat party is hosted. This is only for to show you tutorial. You can host your own flat party any time in game.", () => {
			//				HostPartyTutorial ();

			CloseToolTip ();
			EarnCoin ();
			HostPartyCreated = true;
			PlayerPrefs.SetInt ("Tutorial_Progress", 25);
			StartCoroutine (ShowFlatPartyTutMsg ());
			FlatPartyHostingControler.Instance.HostPartyName.interactable = true;
			FlatPartyHostingControler.Instance.HostPartyDiscription.interactable = true;
			FlatPartyHostingControler.Instance.PartyTimeDropDown.interactable = true;
			FlatPartyHostingControler.Instance.TotelGuestDropDown.interactable = true;
			FlatPartyHostingControler.Instance.PartyTypeToggel.GetComponent<PartyTypeToggle> ().enabled = true;
			FlatPartyHostingControler.Instance.PartyTypePrivate.GetComponent<PartyTypeToggle> ().enabled = true;
		});  

	}

	IEnumerator ShowFlatPartyTutMsg ()
	{
		yield return new WaitForSeconds (5f);
		UpdateTutorial ();
	}

	void LeaveTutorialFlatParty ()
	{
		var Button = GameManager.Instance.GetComponent<Tutorial> ();
		HostPartyManager.Instance.selectedFlatParty = null;		

		Button.AllButtons [0].transform.parent.gameObject.SetActive (true);
		Button.AllButtons [6].gameObject.GetComponent<Image> ().enabled = true;
		Button.AllButtons [6].gameObject.SetActive (true);
		HostPartyManager.Instance.GameGUIControle (true);
		FlatPartyHostingControler.Instance.ScreenCanMove = false;
		ScreenAndPopupCall.Instance.CloseScreen ();
		HostPartyManager.Instance.AttendingParty = false;
		FlatPartyHostingControler.Instance.PartyHosted = false;
		FlatPartyHostingControler.Instance.isPaid = false;
		if (GameObject.Find ("FlatPartyPublicArea")) {
			Destroy (GameObject.Find ("FlatPartyRoom"));
			Destroy (GameObject.Find ("FlatPartyPublicArea"));
			Destroy (GameObject.Find ("OfflineChar"));
		}
		ScreenManager.Instance.RunningParty.transform.FindChild ("Name").GetChild (0).GetComponent<Text> ().text = "";
		ScreenManager.Instance.RunningParty.transform.FindChild ("Members").GetChild (0).GetComponent<Text> ().text = "";
		HostPartyManager.Instance._myFlatParty.Clear ();
		HostPartyManager.Instance.AllFlatParty.Clear ();
		MultiplayerManager.Instance.LeavRoomForFlatParty ();
		MultiplayerManager.Instance.MoveOutOfPublicAreaForTutFlatParty ();
	}

	//	public void OnCloseCondition (int Value)
	//	{
	//		if (societyTutorial == Value) {
	//			DecreaseSocietyTutorial ();
	//		}
	//	}

	public void OnBackCondition ()
	{

		CloseToolTip ();

		if (HostPartyCreated || hostPartyTutorial > 11)
			return;     

		if (hostPartyTutorial <= 10)
			hostPartyTutorial = 2;
		else
			hostPartyTutorial -= 2;
		lasteventgameobject = null;
		CancelInvoke ("HostPartyCreateTutorial");

		Invoke ("HostPartyCreateTutorial", 0.1f);
		 
	}
}

