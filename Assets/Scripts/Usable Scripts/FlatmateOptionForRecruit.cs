using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FlatmateOptionForRecruit : MonoBehaviour
{

	public RoommateData data;

	void Start ()
	{
		InitFlatmateUi ();
	}

	void InitFlatmateUi ()
	{
		if (!data.Unlocked) {
			data.Hired = false;
		}
		transform.GetChild (0).GetComponent <Image> ().sprite = data.Icon; // Icon

		transform.GetChild (1).gameObject.SetActive (!data.Hired && data.Unlocked); // Coins Container
		transform.GetChild (1).GetComponentInChildren <Text> ().text = data.Price.ToString (); // Coin Text

		transform.GetChild (2).gameObject.SetActive (!data.Unlocked); // Locked
		transform.GetChild (3).GetComponent <Text> ().text = data.Name; // Name

		transform.GetChild (4).gameObject.SetActive (data.IsBusy);//|| data.IsCoolingDown);
		if (data.IsBusy)
			transform.GetChild (4).GetComponentInChildren <Text> ().text = ExtensionMethods.GetTimeStringFromFloat (data.busyTimeRemaining);
//		else if (data.IsCoolingDown) {
//			var Diff = data.CooldownEndTime - System.DateTime.UtcNow;
//			float coolDownTimer = (float)Diff.TotalSeconds;
//			transform.GetChild (4).GetComponentInChildren <Text> ().text = ExtensionMethods.GetTimeStringFromFloat (coolDownTimer);
//		}
		if (data.IsBusy)// || data.IsCoolingDown)
			GetComponent<Button> ().interactable = false;
		else
			GetComponent<Button> ().interactable = true;
		
		GetComponent<Button> ().onClick.RemoveAllListeners ();

		ApplyListeners ();
	}

	void ApplyListeners ()
	{
		var Button = GetComponent<Button> ();
		Button.onClick.RemoveAllListeners ();

//     if (data.Hired && data.Unlocked) {
//			Button.onClick.AddListener (() => ShowProfile ());
//        }else{
            Button.onClick.AddListener (() => SetReadyForPurchase());
//        }
	}

    void SetReadyForPurchase()
    {
        ScreenAndPopupCall.Instance.SetCameraActiveFor(data.Prefab);
        RoommateManager.Instance.PurchaseButton.interactable = true;

        if (!data.Hired && !data.Unlocked) 
        {
            RoommateManager.Instance.PurchaseButton.GetComponentInChildren<Text>().text = "Hire";
            RoommateManager.Instance.PurchaseButton.onClick.AddListener (() => ShowInfoPopUp ());
        } 
        else if (!data.Hired && data.Unlocked) {        
            RoommateManager.Instance.PurchaseButton.GetComponentInChildren<Text>().text = "Hire";  
            RoommateManager.Instance.PurchaseButton.onClick.AddListener (() =>{ 
               PurchaseThisdata ();
            });
        }   
        else if (data.Hired && data.Unlocked) {
            RoommateManager.Instance.PurchaseButton.GetComponentInChildren<Text>().text = "ShowProfile";
            RoommateManager.Instance.PurchaseButton.onClick.AddListener (() => ShowProfile ());
        }  
    }
 
	void ShowProfile ()
	{

		ScreenManager.Instance.MoveScreenToBack ();
		foreach (var roommate in RoommateManager.Instance.RoommatesHired) {
			var _data = roommate.GetComponent<Flatmate> ().data;
			if (data == _data) {
				RoommateManager.Instance.SelectedRoommate = roommate;
				break;
			}
		}
        ScreenAndPopupCall.Instance.CloseCharacterCamera();
		ScreenAndPopupCall.Instance.FlatmateProfile ();
		GameObject.Find ("FlatMatesProfile").GetComponent <ShowFlatMateProfile> ().ShowProfile ();
//		GameObject.Find ("FlatMatesProfile").GetComponent <ShowFlatMateProfile> ().setAllPerkToListAndShowProfile ();
	}

	void PurchaseThisdata ()
	{

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = " Do you want to hire this flatmate for " + data.Price + " Coins ?";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseConfirm ());

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());		
	}

	void PurchaseConfirm ()
	{
		StartCoroutine (Purchase ());
	}


	IEnumerator Purchase ()
	{
		if (data.Price < PlayerPrefs.GetInt ("Money")) {

			var button = GameObject.Find ("EventSystem").GetComponent<EventSystem> ().currentSelectedGameObject;
			button.GetComponent<Button> ().interactable = false;


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
				HireRoommate ();
//				GameManager.Instance.AddExperiencePoints (10.0f);

				GameObject.Destroy (this.gameObject);

				ScreenManager.Instance.ClosePopup ();
				ScreenManager.Instance.MoveScreenToBack ();
				InitFlatmateUi ();
//				GameManager.Instance.AddExperiencePoints (5.0f);
				GameManager.Instance.SubtractCoins (data.Price);
				var Tut = GameManager.Instance.GetComponent <Tutorial> ();
				if (Tut.recruitFlatmate == 6)
					Tut.RecruitFlatmate ();
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("flatmateRecruited");
                
                ScreenAndPopupCall.Instance.CloseCharacterCamera();

			} else {
				button.GetComponent<Button> ().interactable = true;
				ShowPopUpError ();
			}


		} else {
			ShowPopUpLessCoins ();
		}
	}


	void ShowPopUpError ()
	{
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("Sorry, Purchase Insuccessful....Internet Not Connected, Please check your connection!!!");
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void PurchaseCancle ()
	{
		ScreenManager.Instance.ClosePopup ();
	}

	void ShowInfoPopUp ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("To Unlock this flatmate you need  Level {0} and {1} Gems", data.Level, data.Gems);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void ShowPopUpLessCoins ()
	{
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("You do not have sufficient coins to hire {0}. \n " +
		"You need {1} more coins to buy this.", data.Name, data.Price - PlayerPrefs.GetInt ("Money"));
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	public void HireRoommate ()
	{
		foreach (var Checkdata in RoommateManager.Instance.AllRoommatesData) {
			if (Checkdata == data)
				Checkdata.Hired = true;
		}
		data.Hired = true;
		RoommateManager.Instance.ShowAllFlatMatesScreen ();

		var target = GameManager.Instance.WayPoints [UnityEngine.Random.Range (0, GameManager.Instance.WayPoints.Count)];

//		GameObject parent = new GameObject ();
		

		var obj = (GameObject)Instantiate (data.Prefab, Vector2.zero, Quaternion.identity);
        obj.transform.position = target.transform.position;
//		Destroy (obj.GetComponent<ManageOrderInLayer> ());
//		obj.AddComponent<ManageOrderInLayer> ();

		obj.name = data.Name;
		obj.transform.eulerAngles = new Vector3 (0, 0, 0);
		obj.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		obj.AddComponent<RoomMateMovement> ().currentWaypoint = target.GetComponent<WayPoint> ();
		obj.GetComponent<Flatmate> ().data = data;
		obj.GetComponent<Flatmate> ().GivePerk ();
		obj.GetComponent<Flatmate> ().HireThisRoommate ();
        obj.AddComponent<GenerateMoney>().MoneyIcon = obj.transform.FindChild ("low money").gameObject;
//		obj.transform.SetParent (parent.transform, false);
//		obj.transform.localPosition = new Vector3 (0, 3, 0);

	}

	void Update ()
	{
		if (data.busyTimeRemaining > 0)
			transform.GetChild (4).GetComponentInChildren <Text> ().text = ExtensionMethods.GetTimeStringFromFloat (data.busyTimeRemaining);
		else if (data.CooldownEndTime > System.DateTime.Now) {
			data.busyTimeRemaining = 0;
			var Diff = data.CooldownEndTime - System.DateTime.UtcNow;
			float coolDownTimer = (float)Diff.TotalSeconds;
			transform.GetChild (4).GetComponentInChildren <Text> ().text = ExtensionMethods.GetTimeStringFromFloat (coolDownTimer);
		} else {
			data.busyTimeRemaining = 0;
//			data.CooldownEndTime = new System.DateTime();
			InitFlatmateUi ();
		}
	}
}

