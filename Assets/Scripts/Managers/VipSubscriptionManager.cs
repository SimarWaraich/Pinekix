using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class VipSubscriptionManager : Singleton<VipSubscriptionManager>
{
	public string tt;
	DateTime EndTime;
	public Text Time;
	float testIndt = 5;
	public bool VipSubscribed = false;
	private float endtime;
	public float EndSubcriptionlTime;
	public int GemsPerSubscribe;
	public GameObject VIPButton;
	public GameObject TimeHolder;
	int Days;
	// Use this for initialization


	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
		if (PlayerPrefs.HasKey ("VIPSubcribedTime")) {
			var Temp = Convert.ToInt64 (PlayerPrefs.GetString ("VIPSubcribedTime"));
			EndTime = DateTime.FromBinary (Temp);
			if (EndTime > DateTime.UtcNow)
				VipSubscribed = true;
			else
				VipSubscribed = false;			
		} else
			VipSubscribed = false;	
		Time.text = "00:00:00";
	}

	public void VIPPurchasePopUp ()
	{
		ShowVIPPopUpMsgForPurchase ("Do you want to purchase VIP subscription for one month?");
	}

	public void VipSubscriptionProcess ()
	{ 
		
		if (GameManager.Instance.Gems >= GemsPerSubscribe) {
			if (!VipSubscribed) {
				SaveEndTimeForSubscription ();
				GameManager.Instance.SubtractGems (GemsPerSubscribe);	
				VipSubscribed = true;
				Days = EndTime.Day - DateTime.UtcNow.Day;
				var Diff = EndTime - DateTime.UtcNow;
				float TimeRemain = (float)Diff.TotalSeconds;
				EndSubcriptionlTime = TimeRemain;
				ShowVIPPopUpMsg ("Congratulations!! \n You have enabled VIP subscription till " + Days.ToString () + " days. " /*+ ExtensionMethods.GetTimeStringFromFloat (EndSubcriptionlTime)*/, "Ok", () => {
					ScreenManager.Instance.ClosePopup ();
				}, () => {
					ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
				});
				StartCoroutine (DownloadContent.Instance.UpdateData ());

			} else {				
				ShowVIPPopUpMsg ("You already have subscription of VIP", "Ok", () => {
					ScreenManager.Instance.ClosePopup ();
				}, () => {
					ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
				});
			}
		} else {
			float GemsRequird = GemsPerSubscribe - GameManager.Instance.Gems;
			ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());	
			ShowVIPPopUpMsg ("You Don't have enough gems to subscribe." + " You need "
			+ GemsRequird + " Gems to subscribe!", "Close", () => {
				ScreenManager.Instance.ClosePopup ();
			});
		}
	}

	void ShowVIPPopUpMsg (string msg, string closeButtonName, UnityEngine.Events.UnityAction OnClickOkAction = null, UnityEngine.Events.UnityAction OnClickOkActionClosePopUp = null)
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		//			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = false;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = closeButtonName;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		//			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ));
		//			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseConfirm ());
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			if (OnClickOkActionClosePopUp != null)
				OnClickOkActionClosePopUp ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => OnClickOkAction ());	

	}

	void ShowVIPPopUpMsgForPurchase (string msg)
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => VipSubscriptionProcess ());

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});	

	}

	public int VipSubcritionActived ()
	{
		if (VipSubscribed) {
			return 2;
		} else {
			return 1;
		}
	}


	void SaveEndTimeForSubscription ()
	{
//		var endTime = DateTime.UtcNow.AddSeconds (EndSubcriptionlTime);
		var endTime = DateTime.UtcNow.AddDays (EndSubcriptionlTime);
		EndTime = endTime;
		print (EndTime);
		PlayerPrefs.SetString ("VIPSubcribedTime", endTime.ToBinary ().ToString ());
	}

	void Update ()
	{
		if (VipSubscribed) {			
			var Diff = EndTime - DateTime.UtcNow;
			float TimeRemain = (float)Diff.TotalSeconds;
			EndSubcriptionlTime = TimeRemain;
			VIPButton.SetActive (false);
			TimeHolder.SetActive (true);
			Days = EndTime.Day - DateTime.UtcNow.Day;
			if (Days >= 1)
				Time.text = Days.ToString () + " days ";
			else
				Time.text = ExtensionMethods.GetTimeStringFromFloat (EndSubcriptionlTime);
			if (EndSubcriptionlTime < 0.6f) {
				VipSubscribed = false;	
				StartCoroutine (DownloadContent.Instance.UpdateData ());
				PlayerPrefs.DeleteKey ("VIPSubcribedTime");
				EndSubcriptionlTime = endtime;
				Time.text = "00:00:00";
				VIPButton.SetActive (true);
				TimeHolder.SetActive (false);
			}
			if (EndTime < DateTime.UtcNow) {
				PlayerPrefs.DeleteKey (" VIPSubcribedTime");
				EndSubcriptionlTime = endtime;
				Time.text = "00:00:00";
				VIPButton.SetActive (true);
				TimeHolder.SetActive (false);
			}
		}
	}

	public void ShowMsgOnVIPItems (string vipItem)
	{
		ShowVIPPopUpMsg (vipItem, "Close", () => {
			ScreenManager.Instance.ClosePopup ();
		});
	}
}


