using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class GroundsUI : MonoBehaviour
{

	public GroundTexture data;
	public bool isForEvent;

	public static bool ApplyRoomTexture = false;

	void Start ()
	{
		InitDecor ();
	}

	void InitDecor ()
	{
		transform.GetChild (1).gameObject.SetActive (data.Unlocked);
		transform.GetChild (2).gameObject.SetActive (false);

		transform.GetChild (1).GetComponentInChildren <Text> ().text = data.Price.ToString ();

		transform.GetChild (0).GetComponent <Image> ().sprite = data.Unlocked ? data.DisplayIcon : ReModelShopController.Instance.Locked;

	
		ApplyListners ();
	}

	void ApplyListners ()
	{ 
		var Button = GetComponent<Button> ();

		Button.onClick.RemoveAllListeners ();

		if (!data.Unlocked) {
			Button.onClick.AddListener (() => ShowInfoPopUp ());
		} else if (data.Unlocked) { 
			if (GameManager.Instance.Gems >= data.Gems) {
				Button.onClick.AddListener (() => PurchaseThisdata ());
			}
		}
			
	}

	void ChangeGroundColor ()
	{
//		var last = RoomPurchaseManager.Instance.Addedflats.Count - 1;

		// TODO this is only temporary.. this is to be done  based on selected flat......

		if (isForEvent) {
			var Flat = ScreenAndPopupCall.Instance.RoomCamera.transform.GetComponentInChildren<Flat3D> ();
			Flat.ChangeGroungColor (data.Texture);
			Flat.GroundTextureName = data.Name;
			ScreenAndPopupCall.Instance.DecorEventRoomScreenSelection ();
			ScreenAndPopupCall.Instance.SetCameraActiveForRoom ();
			EventManagment.Instance._registerButton.interactable = true;
			ReModelShopController.Instance.isForEvent = false;
		}
//		else
//		RoomPurchaseManager.Instance.Addedflats [last].GetComponent <Flat3D> ().ChangeGroungColor (data.Texture);

	}

	void PurchaseThisdata ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = " Do you want to purchase this item for " + data.Price + " Coins ?";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseConfirmForRoom ());
//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseConfirm ());

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());		
	}

	void PurchaseConfirm ()
	{
		if (data.Price <= PlayerPrefs.GetInt ("Money")) {
			var last = RoomPurchaseManager.Instance.Addedflats.Count - 1;

			var updatedata = new FlatUpdateData ();
			updatedata.player_id = PlayerPrefs.GetInt ("PlayerId");
			updatedata.position = RoomPurchaseManager.Instance.Addedflats [last].name;
			updatedata.ground_texture = data.Name;

			foreach (var item in DownloadContent.Instance.downloaded_items) {
				if (item.Category == "Expansion" && item.SubCategory == "Rooms") {             // && item.Name == data.AreaName) {
					updatedata.item_id = item.Item_id;
				}
			}

			StartCoroutine (ChangeGroundTexture (updatedata));

		} else {
			ShowPopUpLessCoins ();
		}
	}

	void PurchaseConfirmForRoom ()
	{
		if (data.Price <= PlayerPrefs.GetInt ("Money")) {

			var updatedata = new FlatUpdateData ();
			updatedata.player_id = PlayerPrefs.GetInt ("PlayerId");
			updatedata.position = null;
			updatedata.ground_texture = data.Name;

			foreach (var item in DownloadContent.Instance.downloaded_items) {
				if (item.Category == "Expansion" && item.SubCategory == "Rooms") {             // && item.Name == data.AreaName) {
					updatedata.item_id = item.Item_id;
				}
			}
			RoomPurchaseManager.Instance.SelectedFlatData = updatedata;
			StartCoroutine (ChangeGroundTexture (updatedata));

		} else {
			ShowPopUpLessCoins ();
		}
	}



	IEnumerator ChangeGroundTexture (FlatUpdateData updatedata)
	{
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.UpdateFlat (updatedata));
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			data.Purchased = true;
			ScreenManager.Instance.ClosePopup ();

//			GameManager.Instance.AddExperiencePoints (10.0f);
			GameManager.Instance.SubtractCoins (data.Price);
			InitDecor ();
			ChangeGroundColor ();
			ScreenAndPopupCall.Instance.CloseScreen ();
			RoomPurchaseManager.Instance.SeletedGroundTexture = data;

			var TempObj = GameObject.FindGameObjectsWithTag ("SelectRoom");
			GameObject[] tempObj = TempObj;
			foreach (GameObject DelObj in tempObj) {
				Destroy (DelObj);
			}
		
			for (int i = 0; i < RoomPurchaseManager.Instance.Addedflats.Count; i++) {
				GameObject FlatSelect = (GameObject)Instantiate (RoomPurchaseManager.Instance.RoomSelectable, new Vector3 (0, 0, 0), Quaternion.identity)as GameObject;
				FlatSelect.transform.parent = RoomPurchaseManager.Instance.Addedflats [i].transform;
				FlatSelect.name = "SelecteblRoom" + i;
				FlatSelect.transform.localPosition = Vector3.zero;
			}
			ApplyRoomTexture = true;
		} else {
			ShowPopUpError ();
		}
	}


	void ShowPopUpError ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Some Error Occured... Please Try Again!!!";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());		
	}

	void ShowInfoPopUp ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("To Unlock this item you need  Level {0} and {1} Gems", data.Level, data.Gems);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void ShowPopUpLessCoins ()
	{
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("You do not have sufficient coins to buy this item. \n " +
		"You need {0} more coins to buy this.", data.Price - PlayerPrefs.GetInt ("Money"));
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void PurchaseCancle ()
	{
		ScreenManager.Instance.ClosePopup ();
	}

	public static string SerializeVector3Array (Vector3 aVectors)
	{
		StringBuilder sb = new StringBuilder ();

		sb.Append (aVectors.x).Append (" ").Append (aVectors.y).Append (" ").Append (aVectors.z).Append ("|");

		if (sb.Length > 0) // remove last "|"
			sb.Remove (sb.Length - 1, 1);
		return sb.ToString ();
	}

}
