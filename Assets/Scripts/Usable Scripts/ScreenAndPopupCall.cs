using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class ScreenAndPopupCall : Singleton<ScreenAndPopupCall>
{
	public GameObject ClassAttendScreen;
	public Camera CharacterCamera;
	public Camera CharacterCameraForvoting1, CharacterCameraForvoting2;
	public Camera RoomCamera;


	public GameObject RoomCameraContainer;

	[Header ("CellPhone")]
	public Transform CellPhoneFrontPosition;
	public bool _CellPhoneCalled;


	[Header ("Screen Panels")]
	public Transform ScreenFrontPosition;

	public Transform BigScreenPosition;

	public Transform HalfScreenPosition;

	public Transform EventScreenPosition;
	public Transform FriendScreenPosition;
	public Transform VotingScreenPosition;
	public Transform ChatInFlatPartyPositon;
	public Transform bonusScreenPosition;

	public GameObject ResultButton;

	public Button ReCenterButton;
	public bool _EventCalled;
	public bool _RecruitCalled;


	[Header ("Menu Panel")]
	public bool _MenuCalled;
	public Sprite MenuOpen;
	public Sprite MenuClose;
	//	public Sprite PhoneOpen, PhoneClose;
	public Transform MenuPosition;
	public bool _Catalogue;


	[Header ("Player and FlatMate")]
	public bool _PlayerMenu;
	public Transform PlayerMenuPos;

	[Header ("Decore PlacementButton")]
	public Button placementbutton;
	public bool placementEnabled;

	private float _buttonDownPhaseStart;
	private float _doubleClickPhaseStart = 0.3f;


	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
//		Debug.Log ("In Start of Screen and Pop Up");
		CharacterCameraForvoting1.enabled = false;
		CharacterCameraForvoting2.enabled = false;
		RoomCamera.enabled = false;
	}

	public void DisplayClassScreenForSomeTime ()
	{
//		StartCoroutine (ShowAndDisable (ClassAttendScreen));
	}

	IEnumerator ShowAndDisable (GameObject Go)
	{
		//		yield return new WaitForSeconds (0.5f);

		CharacterCamera.enabled = false;
		iTween.ScaleTo (Go, Vector3.one, 0.01f);

		//		iTween.FadeTo (Go, 1, 0.1f);

		yield return new WaitForSeconds (2f);
		iTween.ScaleTo (Go, Vector3.zero, 0.01f);
		CharacterCamera.enabled = true;

		//		iTween.FadeTo (Go,0, 0.1f);
	}

	//	public void GoToAttendClass (float time)
	//	{
	//		Invoke ("GoforDress", time + 0.2f);
	//	}

	public void GoforDress ()
	{
		Tutorial Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (Tut.enabled && Tut._ClassAttended) {
			Tut.PopupGoForShoping ();
			//			Tut.UpdateTutorial ();
		} else
			return;
	}

	public void EventCalled ()
	{
		StartCoroutine (IeEventCalled ());
	}

	IEnumerator IeEventCalled ()
	{
		_RecruitCalled = false;
		_CellPhoneCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		if (!_EventCalled)
			ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.Events, ScreenFrontPosition.position);
		else
			ScreenManager.Instance.MoveScreenToBack ();
		_EventCalled = !_EventCalled;

	}

	public void RecruitCalled ()
	{
		StartCoroutine (IeRecruitCalled ());
	}

	IEnumerator IeRecruitCalled ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		if (!_RecruitCalled)
			ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.Recruit, BigScreenPosition.position);
		else
			ScreenManager.Instance.MoveScreenToBack ();

		_RecruitCalled = !_RecruitCalled;

	}

	public void DesablePhone ()
	{
		StartCoroutine (IeCellPhoneCalled1 ());
	}

	IEnumerator IeCellPhoneCalled1 ()
	{
		yield return new WaitForSeconds (0.5f);
		ScreenManager.Instance.CellPhone.transform.GetChild (0).GetComponent<Button> ().interactable = true;
		ScreenAndPopupCall.Instance._CellPhoneCalled = false;
		ScreenManager.Instance.CellPhoneOpen = false;
	}

	public void CellPhoneCalled ()
	{		
		StartCoroutine (IeCellPhoneCalled ());
	}

	IEnumerator IeCellPhoneCalled ()
	{	

		_EventCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.0f);
		if (!_CellPhoneCalled) {
			ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.CellPhone, CellPhoneFrontPosition.position);
			ScreenManager.Instance.CellPhoneOpen = true;
			IndicationManager.Instance.IncrementIndicationFor ("Society_Event_Request_Notification_Invitation", 2);
			//			ScreenManager.Instance.CellPhone.transform.GetChild (0).GetComponent<Image> ().sprite = PhoneClose;
//			var tut = GameManager.Instance.GetComponent <Tutorial> ();
//			if (!tut.enabled) {
//				ScreenManager.Instance.CellPhone.transform.GetChild (0).GetComponent<Button> ().interactable = false;
//			}
		} else {
			ScreenManager.Instance.CellPhoneOpen = false;
			ScreenManager.Instance.MoveScreenToBack ();
			IndicationManager.Instance.IncrementIndicationFor ("Society_Event_Request_Notification_Invitation", 1);
		}
		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (_CellPhoneCalled);
		_CellPhoneCalled = !_CellPhoneCalled;
		//		ScreenManager.Instance.CellPhone.transform.GetChild (0).GetComponent<Image> ().sprite = _CellPhoneCalled ? PhoneClose : PhoneOpen;

	}

	public void UniRepPopup ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
	}

	public void PurchasePopup ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.PurchaseLand);
	}

	public void CloseAllScreenIfAny ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
	}

	public void MenuCalled ()
	{
		DragCamera1.mousedowntime = 0;
		StartCoroutine (IeMenuCalled ());
	}

	IEnumerator IeMenuCalled ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		_PlayerMenu = false;

		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		if (!_MenuCalled) {

			ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.MenuScreen, MenuPosition.position);
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuClose;
			ScreenManager.Instance.MenuScreenOpen = true;
		} else {
			ScreenManager.Instance.MoveScreenToBack ();
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
			if (_Catalogue)
				ScreenManager.Instance.ClosePopup ();
			ScreenManager.Instance.CatalogOpen = false;
			ScreenManager.Instance.MenuScreenOpen = false;
			_Catalogue = false;
		}

		_MenuCalled = !_MenuCalled;

	}

	public void StorageScreenCalled ()
	{
		StartCoroutine (IeStorageScreenCalled ());	
	}

	IEnumerator IeStorageScreenCalled ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.Storage, BigScreenPosition.position);

	}

	public void DecorEventStorageScreenCalled ()
	{
		StartCoroutine (IeDecorEventStorageScreenCalled ());	
	}

	IEnumerator IeDecorEventStorageScreenCalled ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.DecorEventStorage, BigScreenPosition.position);

	}

	public void DecorScreenCalled ()
	{
		StartCoroutine (IeDecorScreenCalled ());
	}

	IEnumerator IeDecorScreenCalled ()
	{		
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.DecorShop, BigScreenPosition.position);
	}

	public void ClothsScreenCalled ()
	{
		StartCoroutine (IeClothsScreenCalled ());
	}

	IEnumerator IeClothsScreenCalled ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.ClothsShop, BigScreenPosition.position);
		PurchaseDressManager.Instance.IntializeDressesforShopping (0);
		StartCoroutine (ActiveCamera (PlayerManager.Instance.MainCharacter));
	}

	public void ShowCharacterSelectionForWarbrobe ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "WardRobe";
		ScreenManager.Instance.MenuScreenOpen = false;
		CharacterSelectionScreen ();
	}

	public void ShowCharacterSelectionForBotique ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "Boutique";
		ScreenManager.Instance.MenuScreenOpen = false;
		CharacterSelectionScreen ();
	}

	public void ShowCharacterSelectionForEvent ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "FashionEventDressUp";
		ScreenManager.Instance.MenuScreenOpen = false;
		CharacterSelectionScreen ();
	}

	public void ShowCharacterSelectionForSocietyEvent ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "SocietyEventDressUp";
		ScreenManager.Instance.MenuScreenOpen = false;
		CharacterSelectionScreen ();
	}

	public void ShowCharacterSelectionForCatWalk ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "CatWalkEventDressUp";
		ScreenManager.Instance.MenuScreenOpen = false;
		CharacterSelectionScreen ();
	}

	public void ShowCharacterSelectionForCoOp ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "CoOpEvent";
		ScreenManager.Instance.MenuScreenOpen = false;
		CharacterSelectionScreen ();
	}

	public void CharacterSelectionScreen ()
	{
		StartCoroutine (IeCharacterSelectionScreen ());
		ScreenManager.Instance.MenuScreenOpen = false;
	}


	public void OnClickBAckFromCharacterSelection ()
	{
		switch (ScreenManager.Instance.OpenedCustomizationScreen) {
		case "WardRobe":
			MenuCalled ();
			break;
		case "Boutique":
			MenuCalled ();
			break;
		case "FashionEventDressUp":
			EventsIntroScreenCalled ();
			break;
		case "SocietyEventDressUp":
			EventsIntroScreenCalled ();
			break;
		case "CatWalkEventDressUp": 
			CoOpEventController.Instance.CloseScreen ();
			EventsIntroScreenCalled ();
			break;
		case "CoOpEvent":
			CoOpEventController.Instance.CloseScreen ();
			EventsIntroScreenCalled ();
			break;
		default: 	
			break;
		}

		CloseScreen ();
		ScreenManager.Instance.CharacterSelection.GetComponentInChildren <Camera> ().enabled = false;
		ScreenManager.Instance.CharacterSelection.GetComponent <CharacterSelectionController> ().DeleteAllChars ();
		GameManager.Instance.GetComponent <Tutorial> ().DressLevelDecrease ();
		GameManager.Instance.GetComponent <Tutorial> ().DecreaseSaloon ();
	}



	#region CoOp Event


	public void ShowCoOpPanel ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		ScreenManager.Instance.OpenedCustomizationScreen = "CoOpEvent";
		StartCoroutine (IeShowSlotsforCoOp ());
		ShowFriendOrRandom ();
		CoOpEventController.Instance.TitleText.text = EventManagment.Instance.CurrentEvent.EventName;
		CoOpEventController.Instance.ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;
		CoOpEventController.Instance.ChatBagdeCountText.gameObject.SetActive (false);
	}

	public void ShowFriendOrRandom ()
	{		
		ScreenManager.Instance.CoOpFriendOption.SetActive (true);
		ScreenManager.Instance.CoOpReadyScreen.SetActive (false);
		ScreenManager.Instance.CoOpChatScreen.SetActive (false);
		ScreenManager.Instance.CoOpWardRobeScreen.SetActive (false);
		ScreenManager.Instance.CoOpWaitingScreen.SetActive (false);
		ScreenManager.Instance.CoOpFriendList.SetActive (false);

		StartCoroutine (ActiveCameraForVoting (RoommateManager.Instance.SelectedRoommate));
	}


	public void ShowCoOpFriendList ()
	{
		ScreenManager.Instance.CoOpFriendList.SetActive (true);
		ScreenManager.Instance.CoOpFriendOption.SetActive (false);
		ScreenManager.Instance.CoOpReadyScreen.SetActive (false);
		ScreenManager.Instance.CoOpChatScreen.SetActive (false);
		ScreenManager.Instance.CoOpWardRobeScreen.SetActive (false);
		FriendsManager.Instance.CreateAllFriendsForCoOpUi ();
		ScreenManager.Instance.CoOpWaitingScreen.SetActive (false);
		ScreenManager.Instance.CoOpFriendList.transform.GetComponentInChildren <InputField> ().text = "";
	}

	public void ShowCoOpWaiting ()
	{
		ScreenManager.Instance.CoOpFriendList.SetActive (false);
		ScreenManager.Instance.CoOpFriendOption.SetActive (false);
		ScreenManager.Instance.CoOpReadyScreen.SetActive (false);
		ScreenManager.Instance.CoOpChatScreen.SetActive (false);
		ScreenManager.Instance.CoOpWardRobeScreen.SetActive (false);
		ScreenManager.Instance.CoOpWaitingScreen.SetActive (true);
		CoOpEventController.Instance.Timer = 0f;
	}


	public void ShowReadyScreen (bool isAfterDressed)
	{
		ScreenManager.Instance.CoOpFriendList.SetActive (false);
		ScreenManager.Instance.CoOpFriendOption.SetActive (false);
		ScreenManager.Instance.CoOpReadyScreen.SetActive (true);
		ScreenManager.Instance.CoOpChatScreen.SetActive (true);
		ScreenManager.Instance.CoOpWardRobeScreen.SetActive (false);
		ScreenManager.Instance.CoOpWaitingScreen.SetActive (false);
		CoOpEventController.Instance.SubmitButton.gameObject.SetActive (false);
		ScreenManager.Instance.CoOpReadyScreen.transform.FindChild ("DressUp").gameObject.SetActive (true);

		if (!isAfterDressed) {
			CloseCharacterCamerasForEvents ();
			string NameString = RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ().data.Name;
			CoOpEventController.Instance.SelectPlayerToSpwan ();
		}
	}

	public void ShowWardRobeForCoOp ()
	{
		ScreenManager.Instance.CoOpFriendList.SetActive (false);
		ScreenManager.Instance.CoOpFriendOption.SetActive (false);
		ScreenManager.Instance.CoOpReadyScreen.SetActive (false);
		ScreenManager.Instance.CoOpChatScreen.SetActive (true);
		ScreenManager.Instance.CoOpWardRobeScreen.SetActive (true);
		ScreenManager.Instance.CoOpWaitingScreen.SetActive (false);	
		ScreenManager.Instance.CoOpWardRobeScreen.transform.FindChild ("RegisterButton").GetComponent <Button> ().interactable = false;
		PurchaseDressManager.Instance.IntializeDressesforCoop (0);


        PurchaseDressManager.Instance.selectedDresses = new System.Collections.Generic.Dictionary<string, DressItem>();
        PurchaseDressManager.Instance.TargetTempDresses = new System.Collections.Generic.List<string>();
        var Clothes = DressManager.Instance.SelectedCharacter.GetComponent<Flatmate>().data.Dress;
        foreach (var item in Clothes.Keys)
        {
            switch (item)
            {
                case "Dresses":
                case "SeasonalClothes":
                    PurchaseDressManager.Instance.TargetTempDresses.Add("Dresses");
                    break;
                case"Tops":
                case "Jackets":
                    PurchaseDressManager.Instance.TargetTempDresses.Add("Upper");
                    break;
                case"Pants":
                case"Shorts":
                case"Skirts":
                    PurchaseDressManager.Instance.TargetTempDresses.Add("Lower");
                    break;
                default:
                    PurchaseDressManager.Instance.TargetTempDresses.Add(item);
                    break;
            }
        }

		CharacterCameraForvoting1.enabled = false;
		CharacterCameraForvoting2.enabled = false;

		StartCoroutine (ActiveCamera (DressManager.Instance.SelectedCharacter));
	}

	Vector3 OriginalPosition = Vector3.zero;
	public bool ChatInCoopIsShown = false;

	public void ChatScreenInCoOpStatus ()
	{
		if (!ChatInCoopIsShown && OriginalPosition == Vector3.zero) {
			CharacterCamera.enabled = false;
			OriginalPosition = ScreenManager.Instance.CoOpChatScreen.transform.position;
			iTween.MoveTo (ScreenManager.Instance.CoOpChatScreen, ChatInFlatPartyPositon.position, 0.1f);
			ChatInCoopIsShown = true;
			ScreenManager.Instance.CoOpChatScreen.transform.FindChild ("chat button").GetComponent <Image> ().sprite = MenuClose;
			CoOpEventController.Instance.ChatBagdeCountText.gameObject.SetActive (false);

		} else if (ChatInCoopIsShown && OriginalPosition != Vector3.zero) {
			iTween.MoveTo (ScreenManager.Instance.CoOpChatScreen, OriginalPosition, 0.1f);
			OriginalPosition = Vector3.zero;
			ChatInCoopIsShown = false;
			ScreenManager.Instance.CoOpChatScreen.transform.FindChild ("chat button").GetComponent <Image> ().sprite = MenuOpen;
			CharacterCamera.enabled = true;
		}
	}

	public bool ChatInFlatPartyIsShown = false;

	public void ChatScreenInFlatPartyStatus ()
	{
		if (!ChatInFlatPartyIsShown && OriginalPosition == Vector3.zero) {
			CharacterCamera.enabled = false;
			OriginalPosition = ScreenManager.Instance.FlatPartyChatScreen.transform.position;
			iTween.MoveTo (ScreenManager.Instance.FlatPartyChatScreen, ChatInFlatPartyPositon.position, 0.1f);

			ChatInFlatPartyIsShown = true;
			ScreenManager.Instance.FlatPartyChatScreen.transform.FindChild ("chat button").GetComponent <Image> ().sprite = FlatPartyScreenControle.Instance.ChatButton [0];
			FlatPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (false);

		} else if (ChatInFlatPartyIsShown && OriginalPosition != Vector3.zero) {
			iTween.MoveTo (ScreenManager.Instance.FlatPartyChatScreen, OriginalPosition, 0.1f);
			OriginalPosition = Vector3.zero;
			ChatInFlatPartyIsShown = false;
			ChatManager.Instance.MessageCountForFlatParty = 0;
			ScreenManager.Instance.FlatPartyChatScreen.transform.FindChild ("chat button").GetComponent <Image> ().sprite = FlatPartyScreenControle.Instance.ChatButton [1];
			;
			CharacterCamera.enabled = true;
		}
	}

	public bool ChatInSocietyPartyIsShown = false;

	public void ChatScreenInSocietyPartyStatus ()
	{
		if (!ChatInSocietyPartyIsShown && OriginalPosition == Vector3.zero) {
			CharacterCamera.enabled = false;
			OriginalPosition = ScreenManager.Instance.SocietyPartyChatScreen.transform.position;
			iTween.MoveTo (ScreenManager.Instance.SocietyPartyChatScreen, ChatInFlatPartyPositon.position, 0.1f);
			ChatInSocietyPartyIsShown = true;
			ScreenManager.Instance.SocietyPartyChatScreen.transform.FindChild ("chat button").GetComponent <Image> ().sprite = SocietyPartyScreenControle.Instance.ChatButton [0];
			SocietyPartyScreenControle.Instance.ChatBagdeCountText.gameObject.SetActive (false);

		} else if (ChatInSocietyPartyIsShown && OriginalPosition != Vector3.zero) {
			iTween.MoveTo (ScreenManager.Instance.SocietyPartyChatScreen, OriginalPosition, 0.1f);
			OriginalPosition = Vector3.zero;
			ChatInSocietyPartyIsShown = false;
			ChatManager.Instance.MessageCountForSocietyParty = 0;
			ScreenManager.Instance.SocietyPartyChatScreen.transform.FindChild ("chat button").GetComponent <Image> ().sprite = SocietyPartyScreenControle.Instance.ChatButton [1];
			CharacterCamera.enabled = true;
		}
	}

	IEnumerator IeShowSlotsforCoOp ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;

		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.18f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.CoOpEventScreen, BigScreenPosition.position);
	}

	#endregion


	IEnumerator IeCharacterSelectionScreen ()
	{	
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		StartCoroutine (ScreenManager.Instance.CharacterSelection.GetComponent<CharacterSelectionController> ().InitChar ());
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.CharacterSelection, BigScreenPosition.position);
		ScreenManager.Instance.CharacterSelection.transform.FindChild ("Submit").gameObject.SetActive (false);

		if (!GameManager.Instance.GetComponent <Tutorial> ()._DressPurchased || !GameManager.Instance.GetComponent <Tutorial> ()._SaloonPurchased || !GameManager.Instance.GetComponent <Tutorial> ()._FashionEventCompleate) {
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("close").gameObject.SetActive (true);
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("Back").gameObject.SetActive (false);
		} else if (ScreenManager.Instance.OpenedCustomizationScreen == "CoOpEvent") {
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("close").gameObject.SetActive (false);
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("Back").gameObject.SetActive (true);

		} else if (ScreenManager.Instance.OpenedCustomizationScreen == "CatWalkEventDressUp") {
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("close").gameObject.SetActive (false);
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("Back").gameObject.SetActive (true);
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("Submit").gameObject.SetActive (true);
		} else {
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("close").gameObject.SetActive (true);
			ScreenManager.Instance.CharacterSelection.transform.FindChild ("Back").gameObject.SetActive (true);
		}
	}

	public void ShowWardrobeForCharacter ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "WardRobe";
		StartCoroutine (IeShowWardrobeForCharacter ());
	}

	IEnumerator IeShowWardrobeForCharacter ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.35f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.MyCloset, BigScreenPosition.position);

        PurchaseDressManager.Instance.selectedDresses = new System.Collections.Generic.Dictionary<string, DressItem>();
        PurchaseDressManager.Instance.TargetTempDresses = new System.Collections.Generic.List<string>();
        var Clothes = DressManager.Instance.SelectedCharacter.GetComponent<Flatmate>().data.Dress;
        foreach (var item in Clothes.Keys)
        {
            switch (item)
            {
                case "Dresses":
                case "SeasonalClothes":
                    PurchaseDressManager.Instance.TargetTempDresses.Add("Dresses");
                    break;
                    case"Tops":
                    case "Jackets":
                    PurchaseDressManager.Instance.TargetTempDresses.Add("Upper");
                    break;
                case"Pants":
                case"Shorts":
                case"Skirts":
                    PurchaseDressManager.Instance.TargetTempDresses.Add("Lower");
                    break;
                default:
                    PurchaseDressManager.Instance.TargetTempDresses.Add(item);
                    break;
            }            
		}
		PurchaseDressManager.Instance.ConfirmationButton.interactable = false;
		StartCoroutine (ActiveCamera (DressManager.Instance.SelectedCharacter));
	}


	public void ShowLeaderboard ()
	{

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.Leaderboard);
//		StartCoroutine (IeShowLeaderboard ());
	}

	IEnumerator IeShowLeaderboard ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.35f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.Leaderboard, BigScreenPosition.position);

	}

	public void DeactivateProfileCamera ()
	{
		StartCoroutine (ShowMyProfile (null, false, 0));
		if (MultiplayerManager.Instance._flatParty) {
			HostPartyManager.Instance.OnPartyLeaveFunction ();
		} else if (MultiplayerManager.Instance._societyParty) {
			SocietyPartyManager.Instance.OnSocietyPartyLeaveFunction ();
		} else if (MultiplayerManager.Instance.RoomName == "Public Park" || MultiplayerManager.Instance.RoomName == "Lounge" || MultiplayerManager.Instance.RoomName == "Library" || MultiplayerManager.Instance.RoomName == "Activity Area" || MultiplayerManager.Instance.RoomName == "Party Area") {
			ScreenAndPopupCall.Instance.CloseScreen ();
			MultiplayerManager.Instance.LeaveRoom ();
			MultiplayerManager.Instance.MoveOutOfPublicArea ();
		}
	}

	public IEnumerator ShowMyProfile (GameObject Char, bool status, int pId)
	{
		yield return new WaitForSeconds (0.45f);
		ActiveCameraforProfileView (Char, status, pId);

	}

	public void ActiveCameraforProfileView (GameObject Character, bool status, int PlayerId)
	{
		for (int x = 0; x < CharacterCamera.transform.GetChild (0).childCount; x++) {
			Destroy (CharacterCamera.transform.GetChild (0).GetChild (x).gameObject);
		}	
		CharacterCamera.gameObject.SetActive (status);
		CharacterCamera.gameObject.GetComponent<Camera> ().enabled = status;
		if (status) {
			GameObject GO = Instantiate (Character, Vector3.zero, Quaternion.identity)as GameObject;

			GO.transform.parent = CharacterCamera.transform.GetChild (0);
			if (GO.GetComponent<Flatmate> ()) {
				Destroy (GO.GetComponent<Flatmate> ());
				Destroy (GO.GetComponent<RoomMateMovement> ());
			}
			GO.transform.localPosition = Vector3.zero;
			GO.transform.localScale = Vector3.one;
			if (GO.GetComponent <GenerateMoney> ())
				GO.GetComponent <GenerateMoney> ().enabled = false;
			var moneyIcon = GO.transform.FindChild ("low money");
			GameObject.Destroy (moneyIcon.gameObject);
			int Layer = LayerMask.NameToLayer ("UI3D");
			GO.SetLayerRecursively (Layer);
			DressManager.Instance.dummyCharacter = GO;

			if (Character != PlayerManager.Instance.MainCharacter) {
				StartCoroutine (PlayerManager.Instance.ApplyCustomisationOfRealFlatmate (GO, PlayerId));
			}
		}
	}

	IEnumerator ActiveCamera (GameObject Char)
	{
		if (ScreenManager.Instance.ScreenMoved.gameObject.name == "Player Profile Screen")
			yield return new WaitForSeconds (5f);
		else
			yield return new WaitForSeconds (.45f);
		SetCameraActiveFor (Char);
//		Char.GetComponent<RoomMateMovement> ().animatorFront.SetBool ("Girl Front Select Dress", true);
	}

	public void SetCameraActiveFor (GameObject Character)
	{

		for (int x = 0; x < CharacterCamera.transform.GetChild (0).childCount; x++) {
			Destroy (CharacterCamera.transform.GetChild (0).GetChild (x).gameObject);
		}	
		
		GameObject GO = Instantiate (Character, Vector3.zero, Quaternion.identity)as GameObject;

		GO.transform.parent = CharacterCamera.transform.GetChild (0);
		if (GO.GetComponent<Flatmate> ()) {
			Destroy (GO.GetComponent<Flatmate> ());
			Destroy (GO.GetComponent<RoomMateMovement> ());
		}
		GO.transform.localPosition = Vector3.zero;
		GO.transform.localScale = Vector2.one;
        if (GO.GetComponent<GenerateMoney> ())
		GO.GetComponent <GenerateMoney> ().enabled = false;
		var moneyIcon = GO.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon.gameObject);
		int Layer = LayerMask.NameToLayer ("UI3D");
		GO.SetLayerRecursively (Layer);
		DressManager.Instance.dummyCharacter = GO;

		CharacterCamera.gameObject.SetActive (true);
        GO.SetActive(true);
		CharacterCamera.rect = new Rect (0.74f, 0.1f, 0.25f, 0.74f);
	}

	public IEnumerator ActiveCameraForVoting (GameObject Char)
	{
		yield return new WaitForSeconds (.6f);
		SetCameraActiveForVotingChar (Char);
	}

	public void SetCameraActiveForVotingChar (GameObject Character)
	{
		GameObject GO = Instantiate (Character, Vector3.zero, Quaternion.identity)as GameObject;

		GO.transform.parent = CharacterCameraForvoting1.transform.GetChild (0);
		if (GO.GetComponent<Flatmate> ())
			Destroy (GO.GetComponent<Flatmate> ());

		if (GO.GetComponent<RoomMateMovement> ())
			Destroy (GO.GetComponent<RoomMateMovement> ());
		GO.transform.localPosition = Vector3.zero;
		GO.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
		GO.GetComponent <GenerateMoney> ().enabled = false;
		var moneyIcon = GO.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon.gameObject);
		int Layer = LayerMask.NameToLayer ("UI3D");
		GO.SetLayerRecursively (Layer);
		DressManager.Instance.dummyCharacter = GO;
		CloseCharacterCamera ();
		//		CharacterCameraForvoting1.gameObject.SetActive (true);
		CharacterCameraForvoting1.enabled = true;
	}

	public	IEnumerator ActiveCameraForAIVoting (GameObject Char)
	{
		yield return new WaitForSeconds (.6f);
		SetCameraActiveForVotingAIChar (Char);
	}

	public void SetCameraActiveForVotingAIChar (GameObject Character)
	{
		GameObject GO = Instantiate (Character, Vector3.zero, Quaternion.identity)as GameObject;

		GO.transform.parent = CharacterCameraForvoting2.transform.GetChild (0);
		if (GO.GetComponent<Flatmate> ())
			Destroy (GO.GetComponent<Flatmate> ());

		if (GO.GetComponent<RoomMateMovement> ())
			Destroy (GO.GetComponent<RoomMateMovement> ());
		GO.transform.localPosition = Vector3.zero;
		GO.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
		GO.GetComponent <GenerateMoney> ().enabled = false;
		var moneyIcon = GO.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon.gameObject);
		int Layer = LayerMask.NameToLayer ("UI3D");
		GO.SetLayerRecursively (Layer);
		DressManager.Instance.dummyCharacter = GO;
		CloseCharacterCamera ();
		//		CharacterCameraForvoting2.gameObject.SetActive (true);
		CharacterCameraForvoting2.enabled = true;
		CharacterCameraForvoting2.transform.GetChild (0).GetChild (0).localPosition = Vector3.zero;
	}


	public void SetCameraActiveForRoom ()
	{	
		if (RoomPurchaseManager.OndecorEvent) {		
			StartCoroutine (RoomCameraEnable ());
			if (ScreenManager.Instance.OpenedCustomizationScreen.Contains ("DecorEvent"))
				ReModelShopController.Instance.isForEvent = false;
		}
	}

	IEnumerator RoomCameraEnable ()
	{		
		yield return new WaitForSeconds (0.6f);
		CloseCharacterCamera ();
		RoomCamera.enabled = true;
		RoomCamera.gameObject.SetActive (true);		
	}

	public void CloseRoomCamera ()
	{	
		StartCoroutine (CloseDecorCamera ());

	}

	IEnumerator CloseDecorCamera ()
	{
		yield return new WaitForSeconds (0.2f);
		CloseCharacterCamera ();
		RoomCamera.enabled = false;	
		//		RoomCamera.gameObject.SetActive (false);
	}

	public void DeleteRoom ()
	{
		StartCoroutine (DeleteRoomForDecorEvent ());
	}

	IEnumerator DeleteRoomForDecorEvent ()
	{
		yield return new WaitForSeconds (0.6f);
		iTween.MoveTo (ScreenManager.Instance.DecorEventRoomScreen, new Vector3 (460f, -260f, 0), 0.1f);

		for (int i = 0; i < RoomCamera.transform.GetChild (0).childCount; i++) {
			Destroy (RoomCamera.transform.GetChild (0).GetChild (i).gameObject);
		}
		RoomPurchaseManager.OndecorEvent = false;
		ReModelShopController.Instance.isForEvent = false;
		ScreenManager.Instance.OpenedCustomizationScreen = null;
		if (RoomCamera.transform.GetComponentInChildren<Decor3DView> ())
			RoomCamera.transform.GetComponentInChildren<Decor3DView> ().DesablePlacement ();
	}


	public void CloseCharacterCamera ()
	{
		CharacterCamera.gameObject.SetActive (false);

		if (CharacterCamera.transform.GetChild (0).childCount == 0)
			return;
		var DummObj = CharacterCamera.transform.GetChild (0).GetChild (0);
		if (DummObj) {
			GameObject.Destroy (DummObj.gameObject);
		}
//		RoommateManager.Instance.RoommatesHired[0].GetComponent<RoomMateMovement> ().animatorFront.SetBool ("Girl Front Select Dress", false);
	}

	public void CloseCharacterCamerasForEvents ()
	{
		CharacterCameraForvoting1.enabled = false;
		CharacterCameraForvoting2.enabled = false;

		if (CharacterCameraForvoting1.transform.GetChild (0).childCount != 0) {
			for (int x = 0; x < CharacterCameraForvoting1.transform.GetChild (0).childCount; x++) {
				Destroy (CharacterCameraForvoting1.transform.GetChild (0).GetChild (x).gameObject);
			}
		}

		if (CharacterCameraForvoting2.transform.GetChild (0).childCount != 0) {

			for (int x = 0; x < CharacterCameraForvoting2.transform.GetChild (0).childCount; x++) {
				Destroy (CharacterCameraForvoting2.transform.GetChild (0).GetChild (x).gameObject);
			}
		}	
	}

	public void FlatmateProfile ()
	{
		StartCoroutine (IeFlatmateProfile ());
	}

	IEnumerator IeFlatmateProfile ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.FlatMateProfile, BigScreenPosition.position);
		StartCoroutine (ActiveCamera (RoommateManager.Instance.SelectedRoommate));
	}

	public void ShowProfileScreen ()
	{
		StartCoroutine (IeSShowProfileScreen ());
	}

	IEnumerator IeSShowProfileScreen ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.ProfileScreen, BigScreenPosition.position);
//		SetCameraActiveFor (RoommateManager.Instance.RoommatesHired [0]);
	}




	public void ShowOtherAndFriendProfileScreen ()
	{
		StartCoroutine (IeShowOtherAndFriendProfileScreen ());
	}

	IEnumerator IeShowOtherAndFriendProfileScreen ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.3f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.OtherAndFriendProfile, BigScreenPosition.position);
//		SetCameraActiveFor (RoommateManager.Instance.RoommatesHired [0]);
	}

	public void FlatMateMenu ()
	{
		StartCoroutine (IeFlatMateMenu ());
	}

	IEnumerator IeFlatMateMenu ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;

		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		if (!_PlayerMenu)
			ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.PlayerMenu, PlayerMenuPos.position);
		else
			ScreenManager.Instance.MoveScreenToBack ();
		ScreenManager.Instance.MenuScreen.transform.FindChild ("OpenCloseSlider").gameObject.SetActive (_PlayerMenu);
		_PlayerMenu = !_PlayerMenu;
	}

	public void RoomPurchaseScreenCalled ()
	{
		StartCoroutine (IeRoomPurchaseScreenCalled ());
	}

	IEnumerator IeRoomPurchaseScreenCalled ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		_PlayerMenu = false;

		if (!RoomPurchaseManager.OndecorEvent) {
			ScreenManager.Instance.MoveScreenToBack ();
		}
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.ReModelShop, BigScreenPosition.position);

	}


	public void ShowCatalogueMenu ()
	{
		ScreenManager.Instance.CatalogOpen = true;
		StartCoroutine (IeShowCatalogueMenu ());
	}

	IEnumerator IeShowCatalogueMenu ()
	{
		yield return new WaitForSeconds (0.0f);
		if (!_Catalogue) {
			
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.Catalogue);

		} else {
//			Debug.Log ("Close Pop Up Call");
			ScreenManager.Instance.ClosePopup ();
			ScreenManager.Instance.CatalogOpen = !ScreenManager.Instance.CatalogOpen;
		}
		_Catalogue = !_Catalogue;

	}


	public void CloseScreen ()
	{
		ScreenManager.Instance.MoveScreenToBack ();
		IndicationManager.Instance.IncrementIndicationFor ("Society_Event_Request_Notification_Invitation", 1);
	}

	public void BoutiqueShop ()
	{
		StartCoroutine (IeBoutiqueShop ());
	}

	IEnumerator IeBoutiqueShop ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;

		//For  Click on Play Area
		ScreenAndPopupCall.Instance._MenuCalled = false;
		ScreenManager.Instance.MenuScreenOpen = false;

		_PlayerMenu = false;

		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.35f);

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.BoutiqueShop, BigScreenPosition.position);
		StartCoroutine (ActiveCamera (PlayerManager.Instance.MainCharacter));

	}

	public void Boutique ()
	{
		StartCoroutine (IeBoutique ());
	}

	IEnumerator IeBoutique ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.35f);	
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.Boutique, BigScreenPosition.position);
		StartCoroutine (ActiveCamera (DressManager.Instance.SelectedCharacter));
	}

	public void PublicAreaSelection ()
	{
		StartCoroutine (IePublicAreaSelection ());
	}

	IEnumerator IePublicAreaSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.PublicAreaList, BigScreenPosition.position);

		ScreenManager.Instance.PublicAreaList.GetComponentInChildren<ScrollRect> ().horizontalNormalizedPosition = 0f;
		ScreenManager.Instance.PublicAreaList.GetComponentInChildren<ScrollRect> ().enabled = GameManager.Instance.GetComponent<Tutorial> ()._PublicAreaAccessed;
	}

	public void ClosePublicAreaList ()
	{
		ScreenAndPopupCall.Instance.CloseScreen ();
		GameManager.Instance.GetComponent<Tutorial> ().DecreasePublicAreaAccess ();
		MultiplayerManager.Instance.DisconnectPublicArea ();

		Invoke ("CellPhoneCalled", 0.1f);
	}

	public void OpenAnd_MemberDiscriptionPanel ()
	{
		StartCoroutine (IeOpenAnd_MemberDiscriptionPanel ());
	}

	IEnumerator IeOpenAnd_MemberDiscriptionPanel ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.Admin_MemberDiscriptionPanel, BigScreenPosition.position);
		StartCoroutine (ActiveCamera (PlayerManager.Instance.MainCharacter));
		CharacterCamera.rect = new Rect (0.74f, 0.03f, 0.25f, 0.74f);
//		CharacterCamera. ;
	}

	public void MoveToPublicAreaFromProfile ()
	{
		if (MultiplayerManager.Instance.RoomName == "Garden") {
			MoveToPublicArea (0);
		} else if (MultiplayerManager.Instance.RoomName == "Cafe") {
			MoveToPublicArea (1);
		} else
			MoveToPublicArea (0);
		FriendProfileManager.Instance.AcitveNormalPackButton ();
		FriendProfileManager.Instance.ShowBlockList = false;
		if (FriendsManager.Instance.ShowFriendInPublicArea) {
			FriendsManager.Instance.ShowAllPlayerToAddAsFreinds ();
		}
		CloseCharacterCamera ();
		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (false);
//		FriendsManager.Instance.ShowFriendInPublicArea = false;
	}

	public void MoveToPublicArea (int i)
	{
		StartCoroutine (IeMoveToPublicArea (i));
	}

	IEnumerator IeMoveToPublicArea (int publicArea)
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MenuScreen.transform.GetChild (5).gameObject.SetActive (false);
		ScreenManager.Instance.MenuScreen.gameObject.SetActive (false);
		ScreenManager.Instance.HudPanel.SetActive (false);

		ScreenManager.Instance.MoveScreenToBack ();     
//		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (false);
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.PublicAreaMenu, BigScreenPosition.position);
		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (false);
		//		ChatManager.Instance.MoveToPublicArea (publicArea);
		//		ChatManager.Instance.ChatCalled ();
	}


	public void BackFromChat ()
	{
		StartCoroutine (IeBackFromChat ());
	}

	IEnumerator IeBackFromChat ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.PublicAreaMenu, BigScreenPosition.position);

	}

	public bool IsChatOpen;
	public bool IsAddFriendOpen;

	public void ChatInPublicArea ()
	{
		if (IsAddFriendOpen) {
			//            iTween.ScaleTo (ScreenManager.Instance.AddFriendScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.05f, "easeType", "easeInCirc"));
			ScreenManager.Instance.AddFriendScreen.transform.localScale = Vector3.zero;
			IsAddFriendOpen = false;
		}

		if (!IsChatOpen) {
			iTween.ScaleTo (ScreenManager.Instance.ChatScreen, iTween.Hash ("scale", Vector3.one, "time", 0.1f, "easeType", "easeInCirc"));
			IsChatOpen = true;

		} else {
			iTween.ScaleTo (ScreenManager.Instance.ChatScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.1f, "easeType", "easeInCirc"));
			IsChatOpen = false;            

		}
//        ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("ChatButton").GetComponent <Button> ().interactable = !IsChatOpen;
//        ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("AddFriendButton").GetComponent <Button> ().interactable = !IsAddFriendOpen;

		ScreenManager.Instance.AddFriendScreen.transform.FindChild ("NoFoundText").GetComponent <Text> ().text = " ";
        

//		iTween.ScaleTo (ScreenManager.Instance.AddFriendScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.1f, "easeType", "easeInCirc"));
		//		ScreenManager.Instance.AddFriendScreen.transform.FindChild ("NoFoundText").GetComponent <Text> ().text = " ";
//		ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("ChatButton").GetComponent <Button> ().interactable = false;
		FriendsManager.Instance.ShowFriendInPublicArea = false;

		if (GameManager.Instance.GetComponent <Tutorial> ()._PublicAreaAccessed)
			ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("AddFriendButton").GetComponent <Button> ().interactable = true;
		else
			ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("AddFriendButton").GetComponent <Button> ().interactable = false;

	}

	public void AddFriendInPublicPlace ()
	{
		if (IsChatOpen) {
//            iTween.ScaleTo (ScreenManager.Instance.ChatScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.05f, "easeType", "easeInCirc"));
			ScreenManager.Instance.ChatScreen.transform.localScale = Vector3.zero;
			IsChatOpen = false;
		}

		if (!IsAddFriendOpen) {
			iTween.ScaleTo (ScreenManager.Instance.AddFriendScreen, iTween.Hash ("scale", Vector3.one, "time", 0.1f, "easeType", "easeInCirc"));
			IsAddFriendOpen = true;
//            ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("AddFriendButton").GetComponent <Button> ().interactable = false;

		} else {
			iTween.ScaleTo (ScreenManager.Instance.AddFriendScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.1f, "easeType", "easeInCirc"));
			IsAddFriendOpen = false;
		}
//Done by Rehan
		FriendProfileManager.Instance.BackToFriendList.SetActive (false);
		FriendProfileManager.Instance.BackToPublicArea.SetActive (true);
		FriendProfileManager.Instance.BackToFlatParty.SetActive (false);
		FriendProfileManager.Instance.BackToSocietyParty.SetActive (false);
		FriendProfileManager.Instance.BackToMyProfile.SetActive (false);
    
//        ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("AddFriendButton").GetComponent <Button> ().interactable = !IsAddFriendOpen;
//        ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("ChatButton").GetComponent <Button> ().interactable = !IsChatOpen;


//		iTween.ScaleTo (ScreenManager.Instance.ChatScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.1f, "easeType", "easeInCirc"));
//		iTween.ScaleTo (ScreenManager.Instance.AddFriendScreen, iTween.Hash ("scale", Vector3.one, "time", 0.1f, "easeType", "easeInCirc"));
//
//		ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("ChatButton").GetComponent <Button> ().interactable = true;
//		ScreenManager.Instance.PublicAreaMenu.transform.FindChild ("SelectorImage").FindChild ("AddFriendButton").GetComponent <Button> ().interactable = false;

	}

	public void MoveOutOfPublicArea ()
	{
		StartCoroutine (IeMoveOutOfPublicArea ());
	}

	IEnumerator IeMoveOutOfPublicArea ()
	{
		yield return new WaitForSeconds (0.1f);
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MenuScreen.transform.GetChild (5).gameObject.SetActive (true);
		ScreenManager.Instance.MenuScreen.gameObject.SetActive (true);
		ScreenManager.Instance.HudPanel.SetActive (true);

		ScreenManager.Instance.MoveScreenToBack ();      
//		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (true);
		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (!_CellPhoneCalled);
        
		MultiplayerManager.Instance.MoveOutOfPublicArea ();
	}

	public void QuestScreenSelection ()
	{
		if (!ParenteralController.Instance.activateParentel) {
			StartCoroutine (IeQuestScreenSelection ());
		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();

		}

	}

	IEnumerator IeQuestScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.QuestScreen, EventScreenPosition.position);

	}

	public void TaskScreenSelection ()
	{
		StartCoroutine (IeTaskScreenSelection ());
	}

	IEnumerator IeTaskScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.TaskScreen, EventScreenPosition.position);
		SetCameraActiveForEvent (PlayerManager.Instance.MainCharacter);
	}

	public void FashionEventListScreenSelection ()
	{
		StartCoroutine (IeEventScreenSelection ());
	}

	IEnumerator IeEventScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.FashionEventScreen, BigScreenPosition.position);
        SetCameraActiveFor(PlayerManager.Instance.MainCharacter);
	}

    public void BackToEventsListFromSeasonal()
    {
        CloseScreen();
        CloseCharacterCamera();
        FashionEventListScreenSelection();
    }

    public void SeasonalClothingScreenForEvents ()
    {
        StartCoroutine (IeSeaonalClothesListing ());
    }

    IEnumerator IeSeaonalClothesListing ()
    {

        _EventCalled = false;
        _CellPhoneCalled = false;
        _RecruitCalled = false;
        if (_MenuCalled)
            ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
        if (_Catalogue)
            ScreenManager.Instance.ClosePopup ();
        _Catalogue = false;
        _MenuCalled = false;
        _PlayerMenu = false;
        ScreenManager.Instance.MenuScreenOpen = false;
        ScreenManager.Instance.MoveScreenToBack ();
        yield return new WaitForSeconds (0.1f);
        ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.SeasonalClothingForEvents, BigScreenPosition.position);
        SetCameraActiveFor(PlayerManager.Instance.MainCharacter);
    }

	public void DecorEventListScreenSelection ()
	{

		ScreenManager.Instance.OpenedCustomizationScreen = "DecorEvent";
		StartCoroutine (IeDecorEventListScreenSelection ());
	}

	IEnumerator IeDecorEventListScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.DecorEventRoomScreen, EventScreenPosition.position);

	}

	public void FashionEventScreenSelection ()
	{
		StartCoroutine (IeFashionEventScreenSelection ());

	}

	IEnumerator IeFashionEventScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		PurchaseDressManager.Instance.selectedDresses = new System.Collections.Generic.Dictionary<string, DressItem> ();

		if (ScreenManager.Instance.OpenedCustomizationScreen == "CatWalkEventDressUp") {
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().interactable = true;
//			if (EventManagment.Instance.SelectedRoommates.Count < 2) {
//				ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetChild (1).GetComponent<Text> ().text = "Next";
//				ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().onClick.AddListener (() => ShowCharacterSelectionForCatWalk ());
//			} else {
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetChild (1).GetComponent<Text> ().text = "Done";
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().onClick.AddListener (() => {
                
				ScreenManager.Instance.CatWalkCharacterDressUp.GetComponent<CatwalkCharacterDressUp> ().ChangedDressChars.Add (DressManager.Instance.SelectedCharacter.GetComponent<Flatmate> ().data.Name);

				foreach (var dress in PurchaseDressManager.Instance.selectedDresses) {
					if (DressManager.Instance.SelectedCharacter.GetComponent<Flatmate> ().data.Dress.ContainsKey ("Dresses")) {
						DressManager.Instance.SelectedCharacter.GetComponent<Flatmate> ().data.Dress.Remove ("Dresses");

						var allCustom = Resources.Load<CustomisationAllData> ("CustomisationAllData");
						if (GameManager.GetGender () == GenderEnum.Female) {
							DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllGirl.BodyPartName.ToArray (), allCustom.EmptyAllGirl.DressesSprites.ToArray ());
						} else {
							DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllBoy.BodyPartName.ToArray (), allCustom.EmptyAllBoy.DressesSprites.ToArray ());
						}
					}
					DressManager.Instance.ChangeFlatMateDress (dress.Value.PartName, dress.Value.DressesImages);        
				}

				PurchaseDressManager.Instance.UpdateDressofAllCharacter ();
				PurchaseSaloonManager.Instance.UpdateHairofAllCharacter ();

				ScreenAndPopupCall.Instance.CloseScreen ();
				ScreenAndPopupCall.Instance.CloseCharacterCamera ();
				ScreenAndPopupCall.Instance.CatWalkCharacterDressUp ();
			});

//			}
		} else {
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().interactable = false;
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().onClick.RemoveAllListeners ();
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().interactable = true;
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetChild (1).GetComponent<Text> ().text = "Done";
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().onClick.AddListener (() => {
				EventManagment.Instance.OnRegistration ();
			});

            PurchaseDressManager.Instance.selectedDresses = new System.Collections.Generic.Dictionary<string, DressItem>();
            PurchaseDressManager.Instance.TargetTempDresses = new System.Collections.Generic.List<string>();
            var Clothes = DressManager.Instance.SelectedCharacter.GetComponent<Flatmate>().data.Dress;
            foreach (var item in Clothes.Keys)
            {
                switch (item)
                {
                    case "Dresses":
                    case "SeasonalClothes":
                        PurchaseDressManager.Instance.TargetTempDresses.Add("Dresses");
                        break;
                    case"Tops":
                    case "Jackets":
                        PurchaseDressManager.Instance.TargetTempDresses.Add("Upper");
                        break;
                    case"Pants":
                    case"Shorts":
                    case"Skirts":
                        PurchaseDressManager.Instance.TargetTempDresses.Add("Lower");
                        break;
                    default:
                        PurchaseDressManager.Instance.TargetTempDresses.Add(item);
                        break;
                }
            }
		}

		if (!GameManager.Instance.GetComponent <Tutorial> ()._FashionEventCompleate)
			ScreenManager.Instance.FashionEvent.transform.FindChild ("Back").gameObject.SetActive (false);
		else
			ScreenManager.Instance.FashionEvent.transform.FindChild ("Back").gameObject.SetActive (true);
		
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.FashionEvent, BigScreenPosition.position);
		StartCoroutine (ActiveCamera (DressManager.Instance.SelectedCharacter));
	}

	public void VotingScreenSelection ()
	{
		StartCoroutine (IeVotingScreenSelection ());
	}

	IEnumerator IeVotingScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.VotingScreen, VotingScreenPosition.position);

		iTween.MoveTo (ScreenManager.Instance.VotingScreenBackground, iTween.Hash ("position", BigScreenPosition.position, "time", 0.1f, "easeType", "easeInOutCubic"));


		yield return new WaitForSeconds (0.3f);
		ResultButton.SetActive (true);
        VotingPairManager.Instance.EventBackGround(ScreenManager.Instance.VotingScreenBackground);
	}

	public void ShowRegisteredPairForWaiting ()
	{
		StartCoroutine (IeShowRegisteredPairFor ());
	}

	IEnumerator IeShowRegisteredPairFor ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.WaitingForOpponentScreen, FriendScreenPosition.position);	
//        VotingPairManager.Instance.EventBackGround(ScreenManager.Instance.WaitingForOpponentScreen);
	}

	public void RewardScreenSelection ()
	{
		StartCoroutine (IeRewardScreenSelection ());
	}

	IEnumerator IeRewardScreenSelection ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.ResultScreen, BigScreenPosition.position);
//		ResultButton.SetActive (true);
	}

	public void EventLsitScreenSelection ()
	{
		if (!ParenteralController.Instance.activateParentel) {
			StartCoroutine (IeEventLsitScreenSelection ());
		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();
		}
	}

	IEnumerator IeEventLsitScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.EventListScreen, EventScreenPosition.position);
		var position = ScreenManager.Instance.EventListScreen.GetComponentInChildren<GridLayoutGroup> ().GetComponent<RectTransform> ().anchoredPosition;
		position.y = 0;
		ScreenManager.Instance.EventListScreen.GetComponentInChildren<GridLayoutGroup> ().GetComponent<RectTransform> ().anchoredPosition = position;
	
	}

	public void ShowHostPartyListandCreate ()
	{	
		
		if (!PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.IsBusy) {
			if (!ParenteralController.Instance.activateParentel) {
				StartCoroutine (IeShowHostPartyListandCreate ());
				ScreenManager.Instance.HostPartyListScreen.transform.GetChild (0).FindChild ("Host Party List").GetComponent<Button> ().interactable = false;
				ScreenManager.Instance.HostPartyListScreen.transform.GetChild (0).FindChild ("Host Flat Party").GetComponent<Button> ().interactable = true;
			
				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (Tut.HostPartyCreated) {
					MultiplayerManager.Instance.ConnectToServerforFlatParty ();
					HostPartyManager.Instance.GetFlatParty (1);
				} else
					Tut.HostPartyTutorial ();
			} else {
				ParenteralController.Instance.ShowPopUpMessageForParentel ();
			}
		} else {	
			SocietyManager.Instance.ShowPopUp ("Your Player is currently busy");
		}
	}



	IEnumerator IeShowHostPartyListandCreate ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.HostPartyListScreen, FriendScreenPosition.position);	
	}

	public void ShowRunnuingPartyFromProfile ()
	{
		StartCoroutine (IeShowRunningPartyScreenFromProfile ());
	}

	IEnumerator IeShowRunningPartyScreenFromProfile ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;

		yield return new WaitForSeconds (0.2f);
		ScreenManager.Instance.MoveScreenToBack ();
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.RunningParty, FriendScreenPosition.position);	
//		FlatPartyScreenControle.Instance.SetTotelTime ();
		FriendProfileManager.Instance.BackFormMultiplayerArea ();
		FriendProfileManager.Instance.AcitveNormalPackButton ();

		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (false);
	}

	public void ShowRunningPartyScreen ()
	{

		if (!ParenteralController.Instance.activateParentel) {
			StartCoroutine (IeShowRunningPartyScreen ());


		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();
		}

	}

	IEnumerator IeShowRunningPartyScreen ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;

		yield return new WaitForSeconds (1.6f);
		ScreenManager.Instance.MoveScreenToBack ();
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.RunningParty, FriendScreenPosition.position);	
		FlatPartyScreenControle.Instance.SetTotelTime ();
		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (false);
		CloseCharacterCamera ();

	}

	public void ShowSocietyPartyRunningScreenFromProfile ()
	{
		StartCoroutine (IeShowSocietyPartyRunningScreenFromProfile ());
	}

	IEnumerator IeShowSocietyPartyRunningScreenFromProfile ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;

		yield return new WaitForSeconds (0.2f);
		ScreenManager.Instance.MoveScreenToBack ();
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.SocietyPartyScreen, FriendScreenPosition.position);
//		SocietyPartyScreenControle.Instance.SetTotelTime ();
		FriendProfileManager.Instance.BackFormMultiplayerArea ();
		FriendProfileManager.Instance.AcitveNormalPackButton ();
	}

	public void ShowSocietyRunningPartyScreen ()
	{

		if (!ParenteralController.Instance.activateParentel) {
			StartCoroutine (IeShowSocietyRunningPartyScreen ());
		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();
		}

	}

	IEnumerator IeShowSocietyRunningPartyScreen ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
	
		yield return new WaitForSeconds (1.6f);
		ScreenManager.Instance.MoveScreenToBack ();
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.SocietyPartyScreen, FriendScreenPosition.position);
		SocietyPartyScreenControle.Instance.SetTotelTime ();
		ScreenAndPopupCall.Instance.CloseCharacterCamera ();
	}

	public void ShowSocietyHostPartyListandCreate ()
	{

		if (!ParenteralController.Instance.activateParentel) {
			StartCoroutine (IeShowSocietyHostPartyListandCreate ());
		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();
		}

	}

	IEnumerator IeShowSocietyHostPartyListandCreate ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.SoietyHostPartyListScreen, FriendScreenPosition.position);	

		//		SocietyManager.Instance.GetRecentSocieties ();
	}

	public void DecorEventRoomScreenSelection ()
	{
		ScreenManager.Instance.OpenedCustomizationScreen = "DecorEvent";
		StartCoroutine (IeDecorEventRoomScreenSelection ());
	}

	IEnumerator IeDecorEventRoomScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.DecorEventRoomScreen, BigScreenPosition.position);
	}



	public void EventsIntroScreenCalled ()
	{
		StartCoroutine (IeEventsIntroScreenCalled ());
	}

	IEnumerator IeEventsIntroScreenCalled ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.EventsIntroScreen, BigScreenPosition.position);
	}


	void SetCameraActiveForEvent (GameObject Character)
	{
		Vector3 playerPos = new Vector3 (-0.16f, -0.85f, 0);
		GameObject GO = Instantiate (Character, playerPos, Quaternion.identity)as GameObject;
		GO.transform.SetParent (CharacterCamera.transform.GetChild (0), true);
		GO.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);


		if (GO.GetType () == typeof(Flatmate))
			Destroy (GO.GetComponent<Flatmate> ());
		GO.transform.localPosition = playerPos;
		GO.GetComponent <GenerateMoney> ().enabled = false;
		var moneyIcon = GO.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon.gameObject);
		int Layer = LayerMask.NameToLayer ("UI3D");
		GO.SetLayerRecursively (Layer);
		DressManager.Instance.dummyCharacter = GO;

		CharacterCamera.gameObject.SetActive (true);
	}


	public void Logout ()
	{
		StartCoroutine (OnLogOutClick ());
	}

	IEnumerator OnLogOutClick ()
	{
		yield return StartCoroutine (DownloadContent.Instance.UpdateData ());
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.Logout ());
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			PlayerPrefs.SetString ("logoutTime_" + PlayerPrefs.GetInt ("PlayerId"), DateTime.Now.ToBinary ().ToString ());
			PlayerPrefs.SetString ("UserName", "");
			PlayerPrefs.SetInt ("PlayerId", 0);
			PlayerPrefs.SetInt ("Money", 0);
			PlayerPrefs.SetInt ("Experience_level", 0);
			PlayerPrefs.SetInt ("ExperiencePoints", 0);
			ParenteralController.Instance.activateParentel = false;
			PlayerPrefs.DeleteKey ("activateParentel"); //, ParenteralController.Instance.activateParentel.ToString ());

//			for (int i = 0; i == GameManager.Instance.level; i++) {
			PlayerPrefs.DeleteKey ("ExperiencePoints" /*+ i*/);
//			}

			PlayerPrefs.SetInt ("Gems", 0);
			PlayerPrefs.DeleteKey ("Level");
			PlayerPrefs.DeleteKey ("SwipeLeftRightSetUp");
			PlayerPrefs.SetInt ("Tutorial_Progress", 0);
			///By Rehan
			PlayerPrefs.DeleteKey ("PlayerProfileStatus");
			PlayerPrefs.DeleteKey ("CurrentAchievementMedal");
			//			PlayerPrefs.DeleteKey ("CharacterType");
//			PlayerPrefs.DeleteKey ("VIPSubcribedTime");
			Destroy (PlayerManager.Instance.gameObject);
			Destroy (DressManager.Instance.gameObject);
			AssetBundleManager.UnloadAll ();
//			ExtensionMethods.cleardictionary ();

			Destroy (PlayerManager.Instance.MainCharacter);

			foreach (var obj in RoommateManager.Instance.RoommatesHired) {

				Destroy (obj);
			}
			foreach (var obj in PurchaseDressManager.Instance.AllDresses) {
				obj.Purchased = false;
				obj.Unlocked = false;
			}

			foreach (var obj in PurchaseSaloonManager.Instance.AllItems) {
				obj.Purchased = false;
				obj.Unlocked = false;
			}
			PlayerPrefs.SetInt ("Logout", 1);

			Destroy (CharacterCustomizationAtStart.Instance.gameObject);

			SceneManager.LoadScene (0);

		}
	}

	public void ShowFriendList ()
	{
		if (!ParenteralController.Instance.activateParentel) {
//			if (FriendsManager.Instance.FreindsButton.interactable && !FriendsManager.Instance.RequestsButton.interactable)
//				FriendsManager.Instance.OnClickRequests ();
//			else
			FriendsManager.Instance.OnClickFriendsInFriendList ();
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsScreen);
			ScreenManager.Instance.FriendsScreen.transform.GetChild (0).GetComponentInChildren <InputField> ().text = "";
			IndicationManager.Instance.IncrementIndicationForRequest (3);
		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();
		}
	}

	//	IEnumerator IeShowFriendList ()
	//	{
	//		FriendsManager.Instance.OnClickFriendsInFriendList ();
	//		_EventCalled = false;
	//		_CellPhoneCalled = false;
	//		_RecruitCalled = false;
	//		if (_MenuCalled)
	//			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
	//		if (_Catalogue)
	//			ScreenManager.Instance.ClosePopup ();
	//		_Catalogue = false;
	//		_MenuCalled = false;
	//		_PlayerMenu = false;
	//
	//		ScreenManager.Instance.MoveScreenToBack ();
	//		yield return new WaitForSeconds (0.1f);
	//		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.FriendsScreen, FriendScreenPosition.position);
	//		ScreenManager.Instance.FriendsScreen.transform.GetChild (0).GetComponentInChildren <InputField>().text = "";
	//		//		FriendsManager.Instance.CreateAllFriendsForUi ();
	//	}



	public void RewardScreen ()
	{
		if (!ParenteralController.Instance.activateParentel) {
			StartCoroutine (IeRewardScreen ());
		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();
		}
	}

	Vector3 bonusScreenPos;
	bool _bonusScreenOpen = false;

	IEnumerator IeRewardScreen ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.RewardScreen, BigScreenPosition.position);	
	}

	public void BonusApplyScreen ()
	{
		bonusScreenPos = ScreenManager.Instance.BonusScreen.transform.position;
		iTween.MoveTo (ScreenManager.Instance.BonusScreen, bonusScreenPosition.position, 0.1f);
		_bonusScreenOpen = true;
	}

	public void MoveBonusScreenBack ()
	{
		if (_bonusScreenOpen) {
			iTween.MoveTo (ScreenManager.Instance.BonusScreen, bonusScreenPos, 0.1f);
			_bonusScreenOpen = false;
		}
	}


	bool _resultOpen = false;
	Vector3 _resultScreenPos;
	public Transform _targetResultPos;

	public void ResultPanel ()
	{
		if (!_resultOpen) {
			_resultScreenPos = ScreenManager.Instance.ResultPanel.transform.position;
			iTween.MoveTo (ScreenManager.Instance.ResultPanel, _targetResultPos.position, 0.1f);
			_resultOpen = true;
		} else {
			iTween.MoveTo (ScreenManager.Instance.ResultPanel, _resultScreenPos, 0.1f);
			_resultOpen = false;
		}
	}

	public void ResultPanelClose ()
	{
		if (_resultOpen) {
			iTween.MoveTo (ScreenManager.Instance.ResultPanel, _resultScreenPos, 0.1f);
			_resultOpen = false;
		}
		ResultButton.SetActive (false);
	}

	public void ShowNotifications ()
	{
		if (!ParenteralController.Instance.activateParentel) {
			NotificationManager.Instance.GetNotifications ();
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.Notifications);
			IndicationManager.Instance.IncrementIndicationFor ("Notification_Invitation", 3);
		} else {
			ParenteralController.Instance.ShowPopUpMessageForParentel ();
		}
	}

	public void ShowInvitations ()
	{
		NotificationManager.Instance.GetInvitations ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.Notifications);
	}

	//	IEnumerator IeShowNotifications ()
	//	{
	//		_EventCalled = false;
	//		_CellPhoneCalled = false;
	//		_RecruitCalled = false;
	//		if (_MenuCalled)
	//			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
	//		if (_Catalogue)
	//			ScreenManager.Instance.ClosePopup ();
	//		_Catalogue = false;
	//		_MenuCalled = false;
	//		_PlayerMenu = false;
	//
	//		ScreenManager.Instance.MoveScreenToBack ();
	//		yield return new WaitForSeconds (0.1f);
	//		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.Notifications, FriendScreenPosition.position);
	//		NotificationManager.Instance.GetNotifications ();
	//	}

	//	Vector3 LoadingScreenPos;

	public void LoadingScreen ()
	{
//		LoadingScreenPos = ScreenManager.Instance.Loading_Screen.transform.position;
		iTween.ScaleTo (ScreenManager.Instance.Loading_Screen, Vector3.one, 0.1f);
	}

	public void LoadingScreenClose ()
	{
		iTween.ScaleTo (ScreenManager.Instance.Loading_Screen, Vector3.zero, 0.1f);
	}

	public void ShowSocietyList ()
	{
		if (!PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.IsBusy) {
			if (!ParenteralController.Instance.activateParentel) {                
				StartCoroutine (IeShowSociety ());
				IndicationManager.Instance.IncrementIndicationFor ("Society", 3);
			} else {
				ParenteralController.Instance.ShowPopUpMessageForParentel ();
	
			}
		} else {	
			SocietyManager.Instance.ShowPopUp ("Your Player is currently busy");
		}
	}

	IEnumerator IeShowSociety ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.SocietyListScreen, FriendScreenPosition.position);	
       
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (Tut._SocietyCreated)
			SocietyManager.Instance.GetRecentSocieties ();
		else {
			SocietyManager.Instance.GetMySocieties ();
			Tut.societyTutorial = 3;
			Tut.SocietyTutorial ();
		}
	}

	public void CreateSocietyScreen ()
	{
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut._SocietyCreated) {
			Tut.SocietyTutorial ();
			StartCoroutine (IeCreateSociety ());
			return;
		} else if (PlayerPrefs.GetInt ("Money") >= 1000) {
			StartCoroutine (IeCreateSociety ());
		} else
			SocietyManager.Instance.societyCreationController.ShowPopUp ("You need 1000 coins to create a new society", null);
	}

	IEnumerator IeCreateSociety ()
	{
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.CreateSocietyPanel, FriendScreenPosition.position);	
		SocietyManager.Instance.societyCreationController.IntializeCreateSociety ();
	}

	public void VipSubscriptionScreenSelection ()
	{
		StartCoroutine (IeVipSubscriptionScreenSelection ());
	}

	IEnumerator IeVipSubscriptionScreenSelection ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		ScreenManager.Instance.MoveScreenToBack ();

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.VipPalnel, BigScreenPosition.position);
		StartCoroutine (ActiveCamera (PlayerManager.Instance.MainCharacter));
		CharacterCamera.rect = new Rect (0.74f, 0.03f, 0.25f, 0.74f);
		yield return new WaitForSeconds (0.3f);
	
	}



	public void CreateHostPartyScreen ()
	{
		StartCoroutine (IeCreateHostPartyScreen ());
	}

	IEnumerator IeCreateHostPartyScreen ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		ScreenManager.Instance.MoveScreenToBack ();

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.CreateHostParty, BigScreenPosition.position);
		StartCoroutine (ActiveCamera (PlayerManager.Instance.MainCharacter));
		CharacterCamera.rect = new Rect (0.74f, 0.03f, 0.25f, 0.74f);
		yield return new WaitForSeconds (0.3f);

	}

	public void CreateSocietyHostPartyScreen ()
	{
		StartCoroutine (IeCreateSocietyHostPartyScreen ());
	}

	IEnumerator IeCreateSocietyHostPartyScreen ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.CreateSocietyHostParty, BigScreenPosition.position);
		ScreenAndPopupCall.Instance.StartCoroutine (ActiveCamera (PlayerManager.Instance.MainCharacter));
		ScreenAndPopupCall.Instance.CharacterCamera.rect = new Rect (0.74f, 0.03f, 0.25f, 0.74f);
		yield return new WaitForSeconds (0.3f);

	}

	public void OpenParenteralScreen ()
	{
		StartCoroutine (IeOpenParenteralScreen ());
	}

	IEnumerator IeOpenParenteralScreen ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.ParenteralScreen, BigScreenPosition.position);
		ParenteralController.Instance.ClearInputFeild ();
		yield return new WaitForSeconds (0.3f);

	}

	public void OpenSettingScreen ()
	{
		StartCoroutine (IeOpenSettingScreen ());
	}

	IEnumerator IeOpenSettingScreen ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.SettingScreen, BigScreenPosition.position);
		SettingController.Instance.ParentalButton.GetComponent<Image>().sprite = SettingController.Instance.ParentalImage[0];
		yield return new WaitForSeconds (0.3f);

	}

	public void OpenInAppPurchaseScreen ()
	{
		StartCoroutine (IeOpenInAppPurchaseScreen ());
	}

	IEnumerator IeOpenInAppPurchaseScreen ()
	{

		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		CloseCharacterCamera ();
		//		SetCameraActiveForVotingChar (RoommateManager.Instance.SelectedRoommate);
		//		SetCameraActiveForVotingAIChar (RoommateManager.Instance.AIPlayer);
		ScreenManager.Instance.MoveScreenToBack ();

		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.InAppPurchseScreen, BigScreenPosition.position);
		SettingController.Instance.ParentalButton.GetComponent<Image>().sprite = SettingController.Instance.ParentalImage[0];
		yield return new WaitForSeconds (0.3f);

	}

	public void ReportSocietyPopUp ()
	{
		ScreenManager.Instance.ReportPopScreen.GetComponentInChildren <InputField> ().text = "";
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.ReportPopScreen);
	}

	public void RportThisProfilePupUp ()
	{		
		ScreenManager.Instance.ProfileReportScreen.GetComponentInChildren <InputField> ().text = "";
		ScreenManager.Instance.ProfileReportScreen.transform.GetChild (0).GetChild (4).GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.ProfileReportScreen);
	}

	public void MyProfileBlockListPopup ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.MyProfileBlockList);
	}


	public void MemberListPopup ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.MemberListScreen);
	}

	public void AchievementPopup ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.AchievementListScreen);
	}

	public void SocietyLeaderBoard ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.SocietyLeaderBoard);
	}

	public void CloseVotingScreen ()
	{
		CloseScreen ();
		CloseCharacterCamerasForEvents ();
		ResultPanelClose ();
		MoveBonusScreenBack ();
		EventsIntroScreenCalled ();

		if (VotingPairManager.Instance.viewFriends) {
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
			ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";
		} else {
			
		}
	}

	public void CatWalkCharacterDressUp ()
	{
		StartCoroutine (IeCatWalkCharacterDressUp ());
		ScreenManager.Instance.MenuScreenOpen = false;
	}

	IEnumerator IeCatWalkCharacterDressUp ()
	{   
		_EventCalled = false;
		_CellPhoneCalled = false;
		_RecruitCalled = false;
		if (_MenuCalled)
			ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
		if (_Catalogue)
			ScreenManager.Instance.ClosePopup ();
		_Catalogue = false;
		_MenuCalled = false;
		_PlayerMenu = false;
		ScreenManager.Instance.MenuScreenOpen = false;
		ScreenManager.Instance.MoveScreenToBack ();
		yield return new WaitForSeconds (0.1f);
		ScreenManager.Instance.CatWalkCharacterDressUp.GetComponent<CatwalkCharacterDressUp> ().InitializeCatwalkDressUp ();
		ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.CatWalkCharacterDressUp, BigScreenPosition.position);

	}

	public void ShowPopOfDescription (string message, UnityEngine.Events.UnityAction OnClickOkAction = null)
	{

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickOkAction != null)
				OnClickOkAction ();     
		}); 
	}

	public void BackFromEventResultScreen ()
	{
		if (VotingPairManager.Instance.PrizeClaimed) {
			CloseScreen ();
			CloseCharacterCamerasForEvents ();
			ResultPanelClose ();
		} else
			SocietyManager.Instance.ShowPopUp ("You haven't claimed your rewards yet. Do you still want to exit?" +
			"\nDoing this will erase your rewards", () => {
				CloseScreen ();
				CloseCharacterCamerasForEvents ();
				ResultPanelClose ();
			}, null);
	}

	public void AchievementsScreen ()
	{
		AchievementsManager.Instance.GenerateAllAchivementsList ();
		iTween.ScaleTo (ScreenManager.Instance.AchievementScreen, iTween.Hash ("scale", Vector3.one, "time", 0.5f, "easeType", "easeInCirc"));
	}

	public void AchievementsScreenClose ()
	{
		iTween.ScaleTo (ScreenManager.Instance.AchievementScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.3f, "easeType", "easeInCirc"));		
		StartCoroutine (DeleteAllAchivement ());
	}

	IEnumerator DeleteAllAchivement ()
	{
		yield return new WaitForSeconds (0.3f);
		for (int i = 0; i < AchievementsManager.Instance.AchievementContainer.childCount; i++) {
			Destroy (AchievementsManager.Instance.AchievementContainer.GetChild (i).gameObject);
		}
		
	}

	void OnApplicationQuit ()
	{
		PlayerPrefs.DeleteKey ("Logout");
	}


    public void OpenSocietyHomeRoomScreen ()
    {
        StartCoroutine (IeSocietyHomeRoomScreen ());
    }

    IEnumerator IeSocietyHomeRoomScreen ()
    {
        _EventCalled = false;
        _CellPhoneCalled = false;
        _RecruitCalled = false;
        if (_MenuCalled)
            ScreenManager.Instance.MenuScreen.transform.GetChild (5).GetComponent<Image> ().sprite = MenuOpen;
        if (_Catalogue)
            ScreenManager.Instance.ClosePopup ();
        _Catalogue = false;
        _MenuCalled = false;
        _PlayerMenu = false;
        ScreenManager.Instance.MenuScreenOpen = false;
        ScreenManager.Instance.MenuScreen.transform.GetChild (5).gameObject.SetActive (false);
        ScreenManager.Instance.MenuScreen.gameObject.SetActive (false);
        ScreenManager.Instance.HudPanel.SetActive (false);

        ScreenManager.Instance.MoveScreenToBack ();     
        yield return new WaitForSeconds (0.1f);
        ScreenManager.Instance.MoveScreenToFront (ScreenManager.Instance.HomeRoomSociety, BigScreenPosition.position);        
        ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (false);
        
    }
}
