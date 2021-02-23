using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowFlatMateProfile : MonoBehaviour
{

	public Text Name;
	public Text Gender;
	public Image PerkIcon;
	public Text PerkName;
	public Text PerkValue;
	public Text Education_Bonus_Point;
	public Text Education_LevelText;

	public Text TimerText;

	public Button AttendClassButton;

	public Button UpgradeButton;
	public Button ChangeButton;


	public void ShowProfile ()
	{
		Flatmate selectedFlatmate = RoommateManager.Instance.SelectedRoommate.GetComponent<Flatmate> ();
		Name.text = selectedFlatmate.data.Name.Trim ('"');
		Gender.text = selectedFlatmate.data.Gender.ToString ();
		if (selectedFlatmate.data.IsBusy) {
//			TimerText.transform.parent.gameObject.SetActive (true);
			TimerText.gameObject.SetActive (true);
			AttendClassButton.interactable = false;
			TimerText.text = ExtensionMethods.GetTimeStringFromFloat (selectedFlatmate.data.busyTimeRemaining);
		} else {
//			TimerText.transform.parent.gameObject.SetActive (false);

			TimerText.gameObject.SetActive (false);
			AttendClassButton.interactable = true;
		}


		PerkName.text = selectedFlatmate.data.Perk;


		for (int i = 0; i < RoommateManager.Instance.PerksName.Count; i++) {
			if (RoommateManager.Instance.PerksName [i] == selectedFlatmate.data.Perk) {
				PerkIcon.sprite = RoommateManager.Instance.PerksImages [i];
			}
			
		}
		PerkValue.text = selectedFlatmate.data.Perk_value.ToString ();
//		for (int i = 0; i < RoommateManager.Instance.AllPerksName.Count; i++) {
//			if (RoommateManager.Instance.AllPerksName [i] == selectedFlatmate.data.Perk) {
//				if (RoommateManager.Instance.TempPerkName.Count >= 1) {
//					for (int cout = 0; cout < RoommateManager.Instance.TempPerkName.Count; cout++) {
//
//						if (!RoommateManager.Instance.TempPerkName [cout].Contains (selectedFlatmate.data.Perk)) {
//							// store this perk in to this Obj
//							RoommateManager.Instance.TempPerkName.Add (RoommateManager.Instance.AllPerksName [i]);
//							RoommateManager.Instance.TempPerkImage.Add (RoommateManager.Instance.AllPerksImages [i]);
//							// delete currunt perk form list
//							RoommateManager.Instance.AllPerksImages.RemoveAt (i);
//							RoommateManager.Instance.AllPerksName.RemoveAt (i);
//						}
//					}
//				} else if (RoommateManager.Instance.TempPerkName.Count == 0) {
//					// store this perk in to this Obj
//					RoommateManager.Instance.TempPerkName.Add (selectedFlatmate.data.Perk);
//					RoommateManager.Instance.TempPerkImage.Add (RoommateManager.Instance.AllPerksImages [i]);
//					// delete currunt perk form list
//					RoommateManager.Instance.AllPerksImages.RemoveAt (i);
//					RoommateManager.Instance.AllPerksName.RemoveAt (i);
//				}
//				// add removed perk to Main perk lsit and remove from temp list
//				if (RoommateManager.Instance.TempPerkName.Count >= 2) {					
//					RoommateManager.Instance.AllPerksImages.Add (RoommateManager.Instance.TempPerkImage [0]);
//					RoommateManager.Instance.AllPerksName.Add (RoommateManager.Instance.TempPerkName [0]);
//					RoommateManager.Instance.TempPerkName.RemoveAt (0);
//					RoommateManager.Instance.TempPerkImage.RemoveAt (0);
//				}
//			}
//		}

		Education_Bonus_Point.text = selectedFlatmate.GetEducationPointString ();
		Education_LevelText.text = "" + selectedFlatmate.GetCurrentEducationLevel ();

		UpgradeButton.onClick.RemoveAllListeners ();
		UpgradeButton.onClick.AddListener (() => {
			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (Tut._ClassAttended) {
				if (PlayerPrefs.GetInt ("Money") >= CoinAndGemsdeductionForPerk ()) {
					if (selectedFlatmate.data.Perk_value <= 5)
						UpgradeThisPerkLevel (selectedFlatmate);
					else
						ShowPopUpLessCoins ("You can not upgrade perk level more then 6");
				} else {
					int remaingCoin = PlayerPrefs.GetInt ("Money") - CoinAndGemsdeductionForPerk ();
					ShowPopUpLessCoins ("You do not have sufficient coins to upgrade perk \n" +
					"You need " + remaingCoin.ToString ().Trim ('-') + " more coins to upgrade this perk.");
				}
			} else {
				selectedFlatmate.UpgradePerk ();
				ShowProfile ();
			}
		});
		ChangeButton.onClick.RemoveAllListeners ();
		ChangeButton.onClick.AddListener (() => {

			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (Tut._ClassAttended) {
				if (PlayerPrefs.GetInt ("Gems") >= CoinAndGemsdeductionForChangePerk ())
					ChangePerk (selectedFlatmate);
				else {
					int remaingCoin = PlayerPrefs.GetInt ("Gems") - CoinAndGemsdeductionForChangePerk ();
					ShowPopUpLessCoins ("You do not have sufficient gems to change the perk \n" +
					"You need " + remaingCoin.ToString ().Trim ('-') + " more gems to change the perk.");
				}
			} else {
				selectedFlatmate.GivePerk ();
				ShowProfile ();
			}

		});

	}

	//	public void setAllPerkToListAndShowProfile ()
	//	{
	//		RoommateManager.Instance.AllPerksName.Clear ();
	//		RoommateManager.Instance.AllPerksImages.Clear ();
	//		foreach (var allPerk in RoommateManager.Instance.PerksName) {
	//			RoommateManager.Instance.AllPerksName.Add (allPerk);
	//		}
	//		foreach (var allPerkImage in RoommateManager.Instance.PerksImages) {
	//			RoommateManager.Instance.AllPerksImages.Add (allPerkImage);
	//		}
	//		RoommateManager.Instance.TempPerkImage.Clear ();
	//		RoommateManager.Instance.TempPerkName.Clear ();
	//		ShowProfile ();
	//	}

	int CoinAndGemsdeductionForPerk ()
	{
		int coinTobeDeduct = 0;
		Flatmate selectedFlatmate = RoommateManager.Instance.SelectedRoommate.GetComponent<Flatmate> ();
		if (selectedFlatmate.data.Perk_value == 1) {
			coinTobeDeduct = 20 * GameManager.Instance.level;
			if (GameManager.Instance.level == 0)
				coinTobeDeduct = 20;
		} else if (selectedFlatmate.data.Perk_value == 2) {
			coinTobeDeduct = 40 * GameManager.Instance.level;
			if (GameManager.Instance.level == 0)
				coinTobeDeduct = 40;
		} else if (selectedFlatmate.data.Perk_value == 3) {
			coinTobeDeduct = 60 * GameManager.Instance.level;
			if (GameManager.Instance.level == 0)
				coinTobeDeduct = 60;
		} else if (selectedFlatmate.data.Perk_value == 4) {
			coinTobeDeduct = 80 * GameManager.Instance.level;
			if (GameManager.Instance.level == 0)
				coinTobeDeduct = 80;
		} else if (selectedFlatmate.data.Perk_value == 5) {
			coinTobeDeduct = 120 * GameManager.Instance.level;
			if (GameManager.Instance.level == 0)
				coinTobeDeduct = 120;
		} 
		return coinTobeDeduct;
	}

	void UpgradeThisPerkLevel (Flatmate slectedflatmet)
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = " Do you want to upgrade this perk level for " + CoinAndGemsdeductionForPerk () + " Coins ?";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			GameManager.Instance.SubtractCoins (CoinAndGemsdeductionForPerk ());
			slectedflatmet.UpgradePerk ();
			ShowProfile ();
			ScreenManager.Instance.ClosePopup ();
		});

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());		
	}

	void ShowPopUpLessCoins (string msg)
	{	

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	}


	void ChangePerk (Flatmate slectedflatmet)
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = " Do you want to change perk for " + CoinAndGemsdeductionForChangePerk () + " Gems ?";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			GameManager.Instance.SubtractGems (CoinAndGemsdeductionForChangePerk ());
			slectedflatmet.GivePerk ();
			ShowProfile ();
			ScreenManager.Instance.ClosePopup ();
		});

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());		
	}

	int CoinAndGemsdeductionForChangePerk ()
	{
		int coinTobeDeductForChangePerk = 0;
		coinTobeDeductForChangePerk = 2 * GameManager.Instance.level;
		if (GameManager.Instance.level == 0)
			return coinTobeDeductForChangePerk = 2;
		else
			return coinTobeDeductForChangePerk;
			
	}



	void Update ()
	{
		if (RoommateManager.Instance.SelectedRoommate && RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ().data.IsBusy)
			TimerText.text = ExtensionMethods.GetTimeStringFromFloat (RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ().data.busyTimeRemaining);
		
	}
}
