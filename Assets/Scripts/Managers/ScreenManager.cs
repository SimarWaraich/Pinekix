/// <summary>
/// Created By ::==>> Mandeep Yadav.. Date 11 July 2k16..
/// Shop Manager... This script will manage all the ingame shops including the inapp purchasing and in game currency purchased things...
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenManager : Singleton<ScreenManager>
{
	
	[Header ("Menu Screens")]
	public GameObject HudPanel;
	public GameObject MenuScreen;
	public GameObject CellPhone;
	public GameObject Catalogue;

	[Header ("InGame Shops for Inapp Purchasing and other things")]
	public GameObject CurrencyShop;
	public GameObject ClothsShop;
	public GameObject BoutiqueShop;
	public GameObject DecorShop;
	public GameObject ReModelShop;

	[Header ("InGame Screens other then Shops used")]
	public GameObject CharacterSelection;
	public GameObject MyCloset;
	public GameObject Storage;
	public GameObject DecorEventStorage;
	public GameObject Careers;
	public GameObject Clubs;
	public GameObject Studio;
	public GameObject Settings;
	public GameObject Recruit;
	public GameObject Goals;
	public GameObject Events;
	public GameObject FlatMateProfile;
	public GameObject PlayerMenu;
	public GameObject Boutique;
	public GameObject Expansion;
	public GameObject PublicAreaList;
	public GameObject PublicAreaMenu;
	public GameObject ChatScreen;
	public GameObject AddFriendScreen;
	public GameObject FriendsScreen;
	public GameObject Leaderboard;
	public GameObject ResultScreen;
	public GameObject RewardScreen;
	public GameObject BonusScreen;
	public GameObject ResultPanel;
	public GameObject Notifications;
    public GameObject Loading_Screen;
    public GameObject CatWalkCharacterDressUp;

	public GameObject WaitingForOpponentScreen;

	[Header ("Society related Screens")]

	public GameObject SocietyListScreen;
	public GameObject CreateSocietyPanel;
	public GameObject Admin_MemberDiscriptionPanel;
    public GameObject HomeRoomSociety;

	[Header ("In Game Event List Screen")]
	public GameObject EventsIntroScreen;
	public GameObject QuestScreen;
	public GameObject TaskScreen;
	public GameObject FashionEventScreen;
	public GameObject FashionEvent;
	public GameObject VotingScreen;
	public GameObject EventListScreen;
	public GameObject DecorEventRoomScreen;
    public GameObject VotingScreenBackground;
    public GameObject SeasonalClothingForEvents;



	[Header ("Profile Screen")]
	public GameObject ProfileScreen;
	public GameObject OtherAndFriendProfile;
	public GameObject ProfileReportScreen;
	public GameObject MyProfileBlockList;

	[Header ("CoOp Screen")]
	public GameObject CoOpEventScreen;
	public GameObject CoOpFriendOption;
	public GameObject CoOpWaitingScreen;
	public GameObject CoOpFriendList;
	public GameObject CoOpReadyScreen;
	public GameObject CoOpWardRobeScreen;

	public GameObject CoOpChatScreen;
	public GameObject FlatPartyChatScreen;
	public GameObject SocietyPartyChatScreen;
	public GameObject SettingScreen;
	public GameObject InAppPurchseScreen;


	[Header ("InGame Popups")]
	public GameObject PurchaseLand;
	public GameObject RefillEnergy;
	public GameObject News;
	public GameObject UniPopup;
	public GameObject FriendsInvitePopUp;
	public GameObject InternetPopup;

	[Header ("Society Popups")]
	public GameObject ReportPopScreen;
	public GameObject MemberListScreen;
	public GameObject AchievementListScreen;
	public GameObject SocietyLeaderBoard;

	public GameObject NewPresidentPopup;

	[Header ("VIP Subscriber")]
	public GameObject VipPalnel;

	[Header ("Host Party & Society Party Screens")]
	public GameObject CreateHostParty;
	public GameObject HostPartyListScreen;
	public GameObject RunningParty;
	public GameObject SocietyPartyScreen;

	[Header ("Society Host Party Screens")]
	public GameObject CreateSocietyHostParty;
	public GameObject SoietyHostPartyListScreen;

	[Header ("ParenteralController Screens")]
	public GameObject ParenteralScreen;
	public GameObject AchievementScreen;

	public CanvasScaler canvasScaler;
	Vector3 OriginalPosition;

	public GameObject ScreenMoved;
	public GameObject PopupShowed;

	public string OpenedCustomizationScreen;
	// To check if Screen Cell Phone or Menu screen Opend
	public bool CellPhoneOpen;
	public bool MenuScreenOpen;
	public bool CatalogOpen;

	void Awake ()
	{
		this.Reload ();
	}

	public void MoveScreenToFront (GameObject Screen, Vector3 TargetPosition)
	{
		RoomPurchaseManager.Instance._selectionEnabled = false;
		RoomPurchaseManager.Instance.RemoveMessage ();

		/// ////////////////////////////////
		/// if any popup is enabled then we are not going to show any screen or popup
		/// ////////////////////////////////
//		if (PopupShowed != null)
//			return;


		/// ///////////////////////////////////
		/// if any other screen is opened then we will first close that screen and then we will open the target screen
		/// //////////////////////////////////

		OriginalPosition = Screen.transform.position;
		iTween.MoveTo (Screen, iTween.Hash ("position", TargetPosition, "time", 0.5f, "easeType", "easeInOutCubic"));
//		iTween.MoveTo (Screen, TargetPosition, 0.1f);
		ScreenMoved = Screen;
        CellPhone.transform.FindChild("CellButton").gameObject.SetActive(!ScreenAndPopupCall.Instance._CellPhoneCalled);
        MenuScreen.transform.FindChild("OpenCloseSlider").gameObject.SetActive(!ScreenAndPopupCall.Instance._PlayerMenu);
	}



	public void MoveScreenToFront (GameObject TargetScreen, Rect RectTransform)
	{ 

		RoomPurchaseManager.Instance._selectionEnabled = false;
		RoomPurchaseManager.Instance.RemoveMessage ();

		/// ////////////////////////////////
		/// if any popup is enabled then we are not going to show any screen or popup
		/// ////////////////////////////////
//		Debug.Log ("Panel is Moving towards Front");
		if (PopupShowed != null)
			return;


		/// /////////////////////////////////////////////////////////////////
		/// Changing the recttransform of the screens to the world point 
		/// so that when changing the position of the ui objects we can directly change them to the position
		/// ////////////////////////////////////////////////////////////////


		Vector2 rectposition = RectTransform.position;
		float scaleFactorX = canvasScaler.referenceResolution.x / Screen.width;
		float scaleFactorY = canvasScaler.referenceResolution.y / Screen.height;
		rectposition = new Vector3 (rectposition.x / scaleFactorX, rectposition.y / scaleFactorY, 0);

		Vector3 TargetPosition = Camera.main.ScreenToWorldPoint (rectposition);

		/// ////////////////////////////////////////////////////////////////////////////////////////
		/// ////////////////////////////////////////////////////////////////////////////////////////
		/// ////////////////////////////////////////////////////////////////////////////////////////


		/// ///////////////////////////////////
		/// if any other screen is opened then we will first close that screen and then we will open the target screen
		/// //////////////////////////////////


		OriginalPosition = TargetScreen.transform.position;
//		HudPanel.SetActive (false);
		iTween.MoveTo (TargetScreen, iTween.Hash ("position", TargetPosition, "time", 0.5f, "easeType", "easeInOutCubic"));

//		iTween.MoveTo (TargetScreen, TargetPosition, 0.1f);
		ScreenMoved = TargetScreen;
	}

	public void MoveScreenToBack ()
	{
		if (ScreenMoved == null)
			return;
        
		iTween.MoveTo (ScreenMoved, iTween.Hash ("position", OriginalPosition, "time", 0.5f, "easeType", "easeInOutCubic"));
       
        if (ScreenMoved == VotingScreen)
            iTween.MoveTo (VotingScreenBackground, iTween.Hash ("position", OriginalPosition, "time", 0.5f, "easeType", "easeInOutCubic"));
//		iTween.MoveTo (ScreenMoved, OriginalPosition, 0.1f);
//		HudPanel.SetActive (true);
		ScreenMoved = null;
//        ScreenManager.Instance.MenuScreen.transform.FindChild("OpenCloseSlider").gameObject.SetActive(_PlayerMenu);
	}

	public void ShowPopup (GameObject Popup)
	{
		RoomPurchaseManager.Instance._selectionEnabled = false;
		RoomPurchaseManager.Instance.RemoveMessage ();
		if (PopupShowed != null)
			ClosePopup ();
		iTween.ScaleTo (Popup, iTween.Hash ("scale", Vector3.one, "time", 0.5f, "easeType", "easeInCirc"));

//		iTween.ScaleTo (Popup, new Vector3 (1, 1, 1), 0.1f);
		PopupShowed = Popup;
	}

	public void ClosePopup ()
	{
		if (PopupShowed != null) {
			iTween.ScaleTo (PopupShowed, iTween.Hash ("scale", Vector3.zero, "time", 0.3f, "easeType", "easeInCirc"));

//			iTween.ScaleTo (PopupShowed, new Vector3 (0, 0, 0), 0.1f);
			PopupShowed = null;

			// For Click on play Area
			ScreenManager.Instance.CatalogOpen = false;
			ScreenAndPopupCall.Instance._Catalogue = false;
		}
	}
}


