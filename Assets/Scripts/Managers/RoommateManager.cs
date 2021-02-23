/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 08 July 2k16
/// This script will be used to manage the room mate functinoality in the game
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

public class RoommateManager : Singleton<RoommateManager>
{
	#region RoommatesData

	public List<RoommateData> AllRoommatesData;

	public GameObject[] RoommatesAvailable;

	public GameObject[] RoommatesHired;
	public GameObject SelectedRoommate;
	public GameObject SelectedRoommateForEvent;
    public Button PurchaseButton;
	#endregion

	#region Perk Data

	public List<string> PerksName = new List<string> ();
	public List<Sprite> PerksImages = new List<Sprite> ();


	//	public List<string> AllPerksName = new List<string> ();
	//	public List<Sprite> AllPerksImages = new List<Sprite> ();
	//
	//	public List<String> TempPerkName = new List<string> ();
	//	public List<Sprite> TempPerkImage = new List<Sprite> ();

	#endregion

	#region Screen

	[Header ("For FlatMate Screen")]
	public GameObject UIPrefab;
	public GameObject AllFlatmatesContainer;
	public GameObject RemoveOption;
	//	public GameObject HiredFlatmatesContainer;

	//	public Dropdown ClassSelectionDropDown;


	[Header ("For Class Selections")]
	public GameObject ClassSelectionScreen;

	public GameObject TimerComponent;

	public Text ClassTimeText;
	public Text ExperienceText;
	public Text EducationLevelText;

	Button lastButtonClicked;

	#endregion

	int ClassTypeSelectionInt = 0;
	//	float EarnedEducationPoints = 0f;
	//	float EarnedXps = 0;

	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
//		ShowAllFlatMatesScreen ();
	}

	public void SelectRoommateForEvent ()
	{
		SelectedRoommateForEvent = SelectedRoommate;
	}

    public void UpdateRoommates (string name, int id, int level, int price, int gems, int vip)
	{
		var path = AssetsPaths.FlatmatesResourcespath + "/" + name;
		var info = Resources.Load<FlatMateInfo> (path);
        RoommateData data = new RoommateData (name, id, info.gender, info.Icon, level, gems, price, false, false, info.Prefab, vip ==1 ?true:false);
		AllRoommatesData.Add (data);

		DeleteAllRoommate ();
		RoommatesAvailable = new GameObject[0];
		AllRoommatesData.ForEach (roommate => { 
            
			/// this is to update the unlock condition of all the decorItem, accoroding to level or gems in gamemanager 
			if (GameManager.Instance.level >= roommate.Level && GameManager.Instance.Gems >= roommate.Gems) {
				int Index = AllRoommatesData.IndexOf (roommate);
				AllRoommatesData [Index].Unlocked = true;
				roommate.Unlocked = true;
			} else {
				roommate.Unlocked = false;
			}
            if(roommate.VipSubscription)
            {   int Index = AllRoommatesData.IndexOf (roommate);
                AllRoommatesData [Index].Unlocked = true;
                if(VipSubscriptionManager.Instance.VipSubscribed)
                    roommate.Unlocked = true;
                else
                    roommate.Unlocked = false;
            }

			if (roommate.Gender == GameManager.GetGender () && !roommate.Hired) {
				GameObject Go = Instantiate (UIPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = AllFlatmatesContainer.transform;
				Go.transform.localScale = Vector3.one;
				Go.name = roommate.Name;
				Go.GetComponent <FlatmateOptionForRecruit> ().data = roommate;	
				AddRoommate (Go);
			}	
		});
	}

	void CheckUnlockConditions ()
	{
		for (int i = 0; i < AllRoommatesData.Count; i++) {
			if (GameManager.Instance.level >= AllRoommatesData [i].Level && GameManager.Instance.Gems >= AllRoommatesData [i].Gems)
				AllRoommatesData [i].Unlocked = true;
			else
				AllRoommatesData [i].Unlocked = false;
		}
	}


	public void ShowHiredFlatmates ()
	{
		StartCoroutine (IeShowHiredFlatmates ());
        RoommateManager.Instance.PurchaseButton.interactable = false;
	}


	public IEnumerator IeShowHiredFlatmates ()
	{
//		yield return DownloadContent.Instance.IeCheckForDownloadList ();
		yield return null;

		DeleteAllRoommate ();
		foreach (var hired in RoommatesHired) {	
			var roommate = hired.GetComponent<Flatmate> ().data;

			if (roommate.Gender == GameManager.GetGender () && roommate.Hired) {
				GameObject Go = Instantiate (UIPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = AllFlatmatesContainer.transform;
				Go.transform.localScale = Vector3.one;
				Go.name = roommate.Name;
				Go.GetComponent <FlatmateOptionForRecruit> ().data = roommate;
			}
		}	
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}



	public void ShowAllFlatMatesScreen ()
	{
        RoommateManager.Instance.PurchaseButton.interactable = false;
		StartCoroutine (IeShowAllFlatmates ());	
	}



	public IEnumerator IeShowAllFlatmates ()
	{
//		yield return DownloadContent.Instance.IeCheckForDownloadList ();
		yield return null;

		DeleteAllRoommate ();
		RoommatesAvailable = new GameObject[0];
		AllRoommatesData.Sort ();

		AllRoommatesData.ForEach (roommate => {	
			/// this is to update the unlock condition of all the decorItem, accoroding to level or gems in gamemanager 
			if (GameManager.Instance.level >= roommate.Level && GameManager.Instance.Gems >= roommate.Gems) {
				int Index = AllRoommatesData.IndexOf (roommate);
				AllRoommatesData [Index].Unlocked = true;
				roommate.Unlocked = true;
			} else {
				roommate.Unlocked = false;
			}

            if(roommate.VipSubscription)
            {   int Index = AllRoommatesData.IndexOf (roommate);
                AllRoommatesData [Index].Unlocked = true;
                if(VipSubscriptionManager.Instance.VipSubscribed)
                    roommate.Unlocked = true;
                else
                    roommate.Unlocked = false;
            }

			if (roommate.Gender == GameManager.GetGender () && !roommate.Hired) {
				GameObject Go = Instantiate (UIPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = AllFlatmatesContainer.transform;
				Go.transform.localScale = Vector3.one;
				Go.name = roommate.Name;
				Go.GetComponent <FlatmateOptionForRecruit> ().data = roommate;	
				AddRoommate (Go);
			}
		});	

		ScreenAndPopupCall.Instance.LoadingScreenClose ();
		var Tut = GameManager.Instance.GetComponent <Tutorial> ();
		if (!Tut._FlatmateRecruited)
			Tut.RecruitFlatmate ();
	}

	void DeleteAllRoommate ()
	{
		for (int i = 0; i < AllFlatmatesContainer.transform.childCount; i++) {
			GameObject.Destroy (AllFlatmatesContainer.transform.GetChild (i).gameObject);
		}
	}

	public void AddRoommate (GameObject target)
	{
		List<GameObject> temp = new List<GameObject> ();

		foreach (GameObject go in RoommatesAvailable) {
			temp.Add (go);
		}

		temp.Add (target);
		RoommatesAvailable = temp.ToArray ();
	}


	public void MakeFlatMateBusyForTime (int typeofClass)
	{
		float busyTime = GetBusyTimeForClassTypeInMinutes ((ClassTypes)typeofClass);

//		screenAndPopupCall.GoToAttendClass (busyTime + 0.1f);
		///Study Bonus For seleted flatmate
		var selectedRoomate = SelectedRoommate.GetComponent <Flatmate> ();
		float bonuspercentage = selectedRoomate.PerkValue;
		int TempPerkValue = 0;
		if (selectedRoomate.data.Perk == "Study Bonus") {
			if (bonuspercentage == 1)
				TempPerkValue = 0;
			else if (bonuspercentage == 2)
				TempPerkValue = 3;
			else if (bonuspercentage == 3)
				TempPerkValue = 5;
			else if (bonuspercentage == 4)
				TempPerkValue = 9;
			else if (bonuspercentage == 5)
				TempPerkValue = 15;
			else if (bonuspercentage == 6)
				TempPerkValue = 20;
			
			float bonusTimeLess = busyTime * TempPerkValue / 100;
			busyTime = busyTime - bonusTimeLess;
		}
		busyTime = Mathf.RoundToInt (busyTime);

		PlayerPrefs.SetInt ("Class_" + PlayerPrefs.GetInt ("PlayerId") + "_" + SelectedRoommate.GetComponent<Flatmate> ().data.Name, typeofClass);
//		selectedRoomate.data.BusyType = BusyType.Class;
//		selectedRoomate.data.
		StartBusyTimerForSelectedRoomMate (busyTime);
	}


	//	public void MakeFlatMateBusyForTimeRemainingAfterReturn (RoommateData data)
	//	{
	//		var typeofClass = PlayerPrefs.GetInt ("Class_" + PlayerPrefs.GetInt ("PlayerId") + "_" + data.Name);
	//		string _logoutTime = PlayerPrefs.GetString ("logoutTime_" + PlayerPrefs.GetInt ("PlayerId"));
	//
	//		long temp = Convert.ToInt64 (_logoutTime);
	//
	//		//Convert the old time from binary to a DataTime variable
	//		DateTime oldDate = DateTime.FromBinary (temp);
	//		print ("oldDate: " + oldDate);
	//
	//		//Use the Subtract method and store the result as a timespan variable
	//		float difference = DateTime.Now.Subtract (oldDate).Seconds;
	//
	//		if (GetBusyTimeForClassTypeInMinutes ((ClassTypes)typeofClass) - difference > 0) {
	//			float busyTime = GetBusyTimeForClassTypeInMinutes ((ClassTypes)typeofClass) - difference;
	//			EarnedXps = GetXpsForClassType ((ClassTypes)typeofClass);
	//			EarnedEducationPoints = GetEducationPointsForClassType ((ClassTypes)typeofClass);
	//			StartTimerForSelectedRoomMate (busyTime);
	//		} else {
	//			UnbusyFlatmate (SelectedRoommate.GetComponent<Flatmate> ());
	//		}
	//	}

	/// <summary>
	/// Starts the busy timer for selected room mate in Seconds.
	/// TODO Change Seconds to Minutes at FlatmateBusyTimer.cs <Line no. 41>
	/// </summary>
	/// <param name="busyTime">Busy time in Seconds for now but this will be chaged to Minutes.</param>
	public void StartBusyTimerForSelectedRoomMate (float busyTime)
	{
		var selectedRoomate = SelectedRoommate.GetComponent <Flatmate> ();
        selectedRoomate.data.BusyTimeRemaining = DateTime.UtcNow.AddSeconds (busyTime);
		selectedRoomate.data.IsBusy = true;


		GameObject Go = new GameObject ();

		if (selectedRoomate.data.BusyType == BusyType.Class)
			Go.AddComponent <FlatmateBusyTimer> ().StartBusyTimeForFlatMate (selectedRoomate);
		else
			Go.AddComponent <FlatmateBusyTimer> ().StartBusyTimeForFlatMate (selectedRoomate);

		selectedRoomate.gameObject.SetActive (false);


//		RoommateSaveData _savedata = new RoommateSaveData ();
//
//		_savedata.player_id = PlayerPrefs.GetInt ("PlayerId");
//		foreach (var downloaded_item in DownloadContent.Instance.downloaded_items) {
//			if (downloaded_item.Category.Trim ('"') == "Flatmates" && downloaded_item.SubCategory.Trim ('"') == "Flatmates" && downloaded_item.Name.Trim ('"') == selectedRoomate.data.Name)
//				_savedata.item_id = downloaded_item.Item_id;
//		}
//
//		_savedata.name = selectedRoomate.data.Name;
//		_savedata.gender = selectedRoomate.data.Gender.ToString ();
//		_savedata.is_busy = selectedRoomate.data.IsBusy;
//		_savedata.busy_time = selectedRoomate.data.busyTimeRemaining;
//		_savedata.education_point = selectedRoomate.data.education_level;
//		_savedata.education_point_level = selectedRoomate.data.education_point;
//		_savedata.perk = selectedRoomate.data.Perk;
//		_savedata.perk_value = selectedRoomate.data.Perk_value;
//		_savedata.hair_style = selectedRoomate.data.Hair_style;
////		_savedata.dress = selectedRoomate.data.Dress.Select (kvp => kvp.Value).ToList ();
//		var value = "";
//		foreach(var dress in selectedRoomate.data.Dress){
//			var iD = FindDressId (dress.Key, dress.Value);
//			value += "," + iD;
//		}
//		_savedata.dress = value;
//		StartCoroutine (UpdateData (_savedata, selectedRoomate));
		UpdateData ();
	}



	public void UpdateData ()
	{
		var selectedRoomate = SelectedRoommate.GetComponent <Flatmate> ();

		RoommateSaveData _savedata = new RoommateSaveData ();

//		foreach (var downloaded_item in DownloadContent.Instance.downloaded_items) {
//			if (downloaded_item.Category.Trim ('"') == "Flatmates" && downloaded_item.SubCategory.Trim ('"') == "Flatmates" && downloaded_item.Name.Trim ('"') == selectedRoomate.data.Name)
//				_savedata.item_id = downloaded_item.Item_id;
//		}
		_savedata.item_id = selectedRoomate.data.Id;
		_savedata.player_id = PlayerPrefs.GetInt ("PlayerId");
		_savedata.name = selectedRoomate.data.Name;
		_savedata.gender = selectedRoomate.data.Gender.ToString ();
		_savedata.is_busy = selectedRoomate.data.IsBusy;
        if(selectedRoomate.data.IsBusy)
            _savedata.busy_time = selectedRoomate.data.BusyTimeRemaining.ToBinary ().ToString ();
        else        
            _savedata.busy_time ="";
        
		_savedata.education_point = selectedRoomate.data.education_level;
		_savedata.education_point_level = selectedRoomate.data.education_point;
		_savedata.perk = selectedRoomate.data.Perk;
		_savedata.perk_value = selectedRoomate.data.Perk_value;
		_savedata.hair_style = selectedRoomate.data.Hair_style;
        if(selectedRoomate.data.IsCoolingDown)
            _savedata.cooldown_time = selectedRoomate.data.CooldownEndTime.ToBinary ().ToString ();
        else 
            _savedata.cooldown_time = "";
		_savedata.busy_type = selectedRoomate.data.BusyType.ToString ();
		_savedata.cooldown_time_event_id = selectedRoomate.data.EventBusyId;

		var value = "";
		foreach (var dress in selectedRoomate.data.Dress) {
//			string category = "";
			if (dress.Key == "Hair")
//				category = "Clothes";
                _savedata.hair_style = dress.Value.ToString ();
			else
//				category = dress.Key;
			
//			var iD = FindDressId (category, dress.Value);
            value += "," + dress.Value;
		}
	
		_savedata.dress = value;
		StartCoroutine (UpdateData (_savedata, selectedRoomate));
	}

	public int FindDressId (string Cat, string Name)
	{
		foreach (var dress in PurchaseDressManager.Instance.AllDresses) {
			if (dress.Catergory.ToString () == Cat && dress.Name == Name)
				return dress.Id;
		}
		return 0;
	}

	IEnumerator UpdateData (RoommateSaveData _savedata, Flatmate selectedRoomate)
	{
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.UpdateFlatmate (_savedata));
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			print ("data saved");

		} else {
//			StartCoroutine (UpdateData (_savedata, selectedRoomate));
		}
		yield return null;

	}

	public void UnbusyFlatmateAfterClass (Flatmate flatmate, int vip_point)
	{
		flatmate.data.IsBusy = false;
		flatmate.gameObject.SetActive (true);

		var typeofClass = (ClassTypes)PlayerPrefs.GetInt ("Class_" + PlayerPrefs.GetInt ("PlayerId") + "_" + flatmate.data.Name);

		float EarnedXps = GetXpsForClassType ((ClassTypes)typeofClass);

		float EarnedEducationPoints = GetEducationPointsForClassType ((ClassTypes)typeofClass);

		int bonus = flatmate.GetPerkValueCorrespondingToString ("Study");
		EarnedXps = EarnedXps + bonus;
		GameManager.Instance.AddExperiencePoints (EarnedXps * vip_point);
		var Tut = GameManager.Instance.GetComponent <Tutorial> ();		

		int edubonus = flatmate.GetPerkValueCorrespondingToString ("Education");
		flatmate.AddEducationPoints (EarnedEducationPoints + edubonus);

		if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.FlatMateProfile) { 
			// if FlatmateProfile is still opened while in tutorial
			if (!Tut._DressPurchased) {
				Tut.AttendClass ();
				ScreenAndPopupCall.Instance.CloseCharacterCamera ();
				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();

				ScreenAndPopupCall.Instance.CloseScreen ();
			} else {
				GameObject.Find ("FlatMatesProfile").GetComponent <ShowFlatMateProfile> ().ShowProfile ();
			}
		}

		if (!Tut._DressPurchased)
			ScreenAndPopupCall.Instance.GoforDress ();

		PlayerPrefs.DeleteKey ("Class_" + PlayerPrefs.GetInt ("PlayerId") + "_" + flatmate.data.Name);


		if (flatmate.data.EventBusyId != 0) {

			var Event_Id = flatmate.data.EventBusyId;
			float cooldownTime = 15; // 6 hours == 6x60 = 360 minutes
			float PerkValue = flatmate.PerkValue;
			int TempPerkValue = 0;
			if (flatmate.data.Perk == "Fashion Bonus") {
				if (PerkValue == 1)
					TempPerkValue = 0;
				else if (PerkValue == 2)
					TempPerkValue = 3;
				else if (PerkValue == 3)
					TempPerkValue = 5;
				else if (PerkValue == 4)
					TempPerkValue = 9;
				else if (PerkValue == 5)
					TempPerkValue = 15;
				else if (PerkValue == 6)
					TempPerkValue = 20;
				
				float CoolDownTimeLess = cooldownTime * TempPerkValue / 100;
				cooldownTime = cooldownTime - CoolDownTimeLess;
			}

			cooldownTime = Mathf.RoundToInt (cooldownTime);
			if (flatmate.data.BusyType == BusyType.CatwalkEvents) {
//				if (ScreenManager.Instance.PopupShowed == null && ScreenManager.Instance.PopupShowed != ScreenManager.Instance.News) {
//					ShowPopUp ("Your Flatmates have returned", () => {	
				var Previous = PersistanceManager.Instance.GetEventToClaimRewards ();
				Previous.Add (Event_Id);
				PersistanceManager.Instance.SaveClaimRewardsEvents (Previous);
//					});
//				}
			} else {
//				ShowPopUp ("Your Flatmate have returned", () => {	
				var Previous = PersistanceManager.Instance.GetEventToClaimRewards ();
				Previous.Add (Event_Id);
				PersistanceManager.Instance.SaveClaimRewardsEvents (Previous);
//				});
			}
			StartCooldownTimerForFlatmate (cooldownTime, flatmate);
		}
		UpdateData ();
		if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
			AchievementsManager.Instance.CheckAchievementsToUpdate ("completeUniversityClasses");
	}

	void ShowPopUp (string message, UnityEngine.Events.UnityAction OnClickActions)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent <Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickActions != null)
				OnClickActions ();
		});
	}

	public void ShowClassSelectionScreen ()
	{
		ClassSelectionScreen.SetActive (true);
		//		SelectClassAndShowClassInfo (1); // show Info of Essay Class Type.... 
		if (lastButtonClicked)
			lastButtonClicked.interactable = true;
		lastButtonClicked = null; 
		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("TextPanel").gameObject.SetActive (true);
		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("Info_Panel").gameObject.SetActive (false);
		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("Go").GetComponent <Button> ().interactable = false; // This should be last line... 
	}

	public void CloseClassSelection ()
	{
		ClassSelectionScreen.SetActive (false);

		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("Go").GetComponent <Button> ().interactable = false;
		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("TextPanel").gameObject.SetActive (true);
		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("Info_Panel").gameObject.SetActive (false);


		if (lastButtonClicked)
			lastButtonClicked.interactable = true;
		lastButtonClicked = null; 
	}

	public void SelectClassAndShowClassInfo (int ClassType)
	{
		ClassTypeSelectionInt = ClassType;

		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("Info_Panel").gameObject.SetActive (true);
		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("TextPanel").gameObject.SetActive (false);
		ClassSelectionScreen.transform.FindChild ("InfoPanel").FindChild ("Go").GetComponent <Button> ().interactable = true;

		ClassTimeText.text = string.Format ("{0} Minutes", GetBusyTimeForClassTypeInMinutes ((ClassTypes)ClassType));
		EducationLevelText.text = string.Format ("{0} Points", GetEducationPointsForClassType ((ClassTypes)ClassType));
		ExperienceText.text = string.Format ("{0} Xps", GetXpsForClassType ((ClassTypes)ClassType));
	}

	public void ConfirmClassSelection ()
	{
		if (ClassTypeSelectionInt == 0)
			return;
		else {
			MakeFlatMateBusyForTime (ClassTypeSelectionInt);
			ClassSelectionScreen.SetActive (false);
//			screenAndPopupCall.CloseCharacterCamera ();
			ScreenAndPopupCall.Instance.DisplayClassScreenForSomeTime ();
		}
	}

	public void SetButtonInterChangeable (Button Go)
	{		
		if (lastButtonClicked)
			lastButtonClicked.interactable = true;

		Go.interactable = false;
		lastButtonClicked = Go;
	}

	public void StartCooldownTimerForFlatmate (float cooldowntime, Flatmate flatmate)
	{
		flatmate.data.CooldownEndTime = DateTime.UtcNow.AddMinutes (cooldowntime);
//		flatmate.data.BusyType = BusyType.FashionEvents;
		var generateMoney = flatmate.gameObject.GetComponent <GenerateMoney> ();
		generateMoney.enabled = false;
		generateMoney.MoneyIcon.SetActive (false);
		flatmate.data.IsCoolingDown = true;
		flatmate.data.EventBusyId = 0;

		GameObject Go = new GameObject ();
		Go.AddComponent <FlatmateCoolTimer> ().StartCoolDownTimeForFlatMate (flatmate);
		Go.name = "FlatmateCoolTimer";

		UpdateData ();
	}

	public void OnCoolDownComplete (Flatmate flatmate)
	{
		flatmate.data.IsCoolingDown = false;
		flatmate.data.IsBusy = false;
		flatmate.data.EventBusyId = 0;
//		flatmate.data.BusyType = BusyType.Class;

		var generateMoney = flatmate.gameObject.GetComponent <GenerateMoney> ();
		generateMoney.enabled = true;
		if (generateMoney.MoneyToBeGiven > 0)
			generateMoney.MoneyIcon.SetActive (true);

		PlayerPrefs.DeleteKey ("FlatMateCoolDownTime" + flatmate.data.Name);			
		UpdateData ();
	}

	// Return The XPS value to selected Flatemet
	float GetXpsForClassType (ClassTypes classtype)
	{
		switch (classtype) {
		case ClassTypes.Essay:
			return 2f;
		case ClassTypes.SeminarPrepration:
			return 5f;
		case ClassTypes.Tutorial:
			return 10f;
		case ClassTypes.Lecture:
			return 20f;
		case ClassTypes.Seminar:
			return 30f;
		case ClassTypes.Coursework:
			return 40f;
		default:
			return 0f;
		}
	}

	/// <summary>
	/// Returns the busy time of a Flatmate on basis of type of class attended in minutes.
	/// </summary>
	/// <param name="classtype">Classtype.</param>
	int GetBusyTimeForClassTypeInMinutes (ClassTypes classtype)
	{
		switch (classtype) {
		case ClassTypes.Essay:
			return 5;//return 30;
		case ClassTypes.SeminarPrepration:
			return 60;
		case ClassTypes.Tutorial:
			return 120;
		case ClassTypes.Lecture:
			return 240;
		case ClassTypes.Seminar:
			return 360;
		case ClassTypes.Coursework:
			return 10;
		default:
			return 0;
		}
	}

	float GetEducationPointsForClassType (ClassTypes classtype)
	{
		switch (classtype) {
		case ClassTypes.Essay:
			return 5f;
		case ClassTypes.SeminarPrepration:
			return 10f;
		case ClassTypes.Tutorial:
			return 20f;
		case ClassTypes.Lecture:
			return 40f;
		case ClassTypes.Seminar:
			return 80f;
		case ClassTypes.Coursework:
			return 160f;
		default:
			return 0f;
		}
	}


	int GetCoolDownTimeForClassType (ClassTypes classtype)
	{
		return Mathf.RoundToInt (GetBusyTimeForClassTypeInMinutes (classtype) * 0.4f);
	}
}



/// <summary>
/// Enum for specifying type of Class user want to send a flatmate.
/// </summary>
public enum ClassTypes
{
	None = 0,
	Essay = 1,
	SeminarPrepration = 2,
	Tutorial = 3,
	Lecture = 4,
	Seminar = 5,
	Coursework = 6
}