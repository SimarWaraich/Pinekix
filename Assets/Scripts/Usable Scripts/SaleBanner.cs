using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SaleBanner : MonoBehaviour
{

//	public int x;
//	public int y;
//
//	public bool isAceptingMouse;
//
//	public bool isPurchased;
//
//	public GameObject purchaseLandPrefeb;
//
//	Tutorial tut;
//
//	void Start ()
//	{
//		tut = GameManager.Instance.GetComponent<Tutorial> ();
//	}
//
//	void OnMouseDown ()
//	{
//		if (ScreenManager.Instance.ScreenMoved || !isAceptingMouse) {
//			return;
//		}
//
//		if (tut.enabled && !tut._DressPurchased) {
//			return;
//		}
////		ScreenAndPopupCall.Instance.RoomPurchaseScreenCalled ();
//		ShowPopUp ();
//
////		RoomPurchaseManager.Instance.selectedBanner = this;
//
////		if (tut.enabled) {
////			tut.LandPurchasing ();
////			tut.lasteventgameobject = null;
////		}
//	}
//
//
//
//	void ShowPopUp ()
//	{
//		ScreenManager.Instance.ClosePopup ();
//
//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
//
//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Do You Want To Purchase This Land For 500 Coins";
//
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => OnclickOk ());
//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
//
//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
//			ScreenManager.Instance.ClosePopup ();
////			if (tut.enabled) {
////				tut.LandLevelDecrease ();
////			}
//		});
//	}
//
//	void OnclickOk ()
//	{
//		if (500 <= PlayerPrefs.GetInt ("Money")) {
//
//			var updatedata = new FlatUpdateData ();
//			updatedata.player_id = PlayerPrefs.GetInt ("PlayerId");
//			updatedata.position = RoomPurchaseManager.Instance.selectedBanner.x.ToString () + "-" + RoomPurchaseManager.Instance.selectedBanner.y.ToString ();
//			updatedata.wall_texture = "null";
//			updatedata.ground_texture = "null";
//
//			updatedata.item_id = 0;
//			StartCoroutine (Purchase (updatedata));
//
//		} else {
//			ShowPopUpLessCoins ();
//		}
//	}
//
//
//	IEnumerator Purchase (FlatUpdateData updatedata)
//	{
//		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.UpdateFlat (updatedata));
//		yield return cd.coroutine;
//
//		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
//			GameObject Go = Instantiate (RoomPurchaseManager.Instance.purchaseLandPrefab, transform.position, Quaternion.identity) as GameObject;
//			RoomPurchaseManager.Instance.purchaseLands.Add (Go);
////			GameManager.Instance.AddExperiencePoints (25.0f);
////			Go.GetComponent <PurchasedLand> ()._thisBanner = RoomPurchaseManager.Instance.selectedBanner;
//			GameManager.Instance.SubtractCoins (500);
//			ScreenManager.Instance.ClosePopup ();
//			this.gameObject.SetActive (false);
//			isPurchased = true;
////			if (tut.enabled) {
////				tut.LandPurchasing ();
////			}
////			RoomPurchaseManager.Instance.ShowBannerofPurchase (RoomPurchaseManager.Instance.selectedBanner);
//		} else {
//			ShowPopUpError ();
//		}
//	}
//
//
//	void ShowPopUpError ()
//	{
//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Some Error Occured... Please Try Again!!!";
//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => CanclePurchase ());		
//	}
//
//
//	void ShowPopUpLessCoins ()
//	{
//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("You do not have sufficient coins to buy this land. \n " +
//		"You need {0} more coins to buy this.", 500 - PlayerPrefs.GetInt ("Money"));
//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => CanclePurchase ());
//	}
//
//	void CanclePurchase ()
//	{
//		ScreenManager.Instance.ClosePopup ();
////		if (tut.enabled) {
////			tut.LandLevelDecrease ();
////		}
//	}
}

