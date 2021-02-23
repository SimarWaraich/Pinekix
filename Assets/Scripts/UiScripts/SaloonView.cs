using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SaloonView : MonoBehaviour
{
	public SaloonItem thisItem;

	void Start ()
	{
		InitDresses ();
	}

	public void InitDresses ()
	{
		if (!thisItem.Unlocked) {
			thisItem.Purchased = false;
		}

		transform.GetChild (0).gameObject.SetActive (!thisItem.Purchased && thisItem.Unlocked);
		transform.GetChild (1).gameObject.SetActive (!thisItem.Unlocked);
		transform.GetChild (0).GetComponentInChildren <Text> ().text = thisItem.Price.ToString ();
		GetComponent <Image> ().sprite = thisItem.DisplayIcon;
		GetComponent<Button> ().onClick.RemoveAllListeners ();
			
		ApplyListeners ();
	}
	//

	void PurchaseThisItem ()
	{
		
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
       
        string message = " Do you want to purchase this item for " + thisItem.Price + " Coins";
        if(thisItem.Gems > 0)
        {
            message +=  " and "+ thisItem.Gems +" Gems";
        }
        ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message +"?";
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

		if (thisItem.Price < PlayerPrefs.GetInt ("Money")) {

			var button = GameObject.Find ("EventSystem").GetComponent<EventSystem> ().currentSelectedGameObject;
			button.GetComponent<Button> ().interactable = false;


			int item_id = 0;
			string cat = "";
			string sub_cat = "";
			foreach (var downloadeditem in DownloadContent.Instance.downloaded_items) {
				if (downloadeditem.Name.Trim ('"') == thisItem.Name) {
					item_id = downloadeditem.Item_id;
					cat = downloadeditem.Category;
					sub_cat = thisItem.Catergory.ToString ();
				}
			}

			CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.SetPurchasingData (item_id, cat, sub_cat));
			yield return cd.coroutine;

			if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
				button.GetComponent<Button> ().interactable = true;
				PurchaseSaloonManager.Instance.UpdateItem (thisItem);
				thisItem.Purchased = true;
				GameObject.Destroy (this.gameObject);
				ScreenManager.Instance.ClosePopup ();
				InitDresses ();
//				GameManager.Instance.AddExperiencePoints (10.0f);
				GameManager.Instance.SubtractCoins (thisItem.Price);

				GameManager.Instance.GetComponent<Tutorial> ().SaloonPurchasing ();
				QuestManager.Instance.CheckifTaskisDone ("Hair");
			} else {
				button.GetComponent<Button> ().interactable = true;
				ShowPopUpError ();
			}


		} else {
			ShowPopUpLessCoins ();
		}
	}



	public void ApplyListeners ()
	{
		if (!thisItem.Purchased && !thisItem.Unlocked) {
			GetComponent<Button> ().onClick.AddListener (() => ShowInfoPopUp ());
		} else if (!thisItem.Purchased && thisItem.Unlocked) {	
			GetComponent<Button> ().onClick.AddListener (() => PurchaseThisItem ());			
		} else if (thisItem.Purchased && thisItem.Unlocked) {
			
            GetComponent<Button> ().onClick.AddListener (() => DressManager.Instance.ChangeHairsForDummyCharacter (thisItem.PartName, thisItem.HairImages));
			GetComponent<Button> ().onClick.AddListener (() => PurchaseSaloonManager.Instance.SelectThisItem (this));
			GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().SaloonPurchasing ());		

		} else {
			Debug.LogError ("No Condition Saticified in Dress items");
		}		
	}

	void PurchaseCancle ()
	{
		ScreenManager.Instance.ClosePopup ();
	}

	void ShowInfoPopUp ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("To Unlock this item you need  Level {0}s", thisItem.Level);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void ShowPopUpLessCoins ()
	{
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";

        string Message = "You do not have sufficient";
        string Message2 ="You need ";
        bool written = false;
        if (thisItem.Price > PlayerPrefs.GetInt ("Money"))
        {
            Message += " coin";
            Message2 += thisItem.Price - PlayerPrefs.GetInt ("Money") + " more coins";
        }else
        {
            if(thisItem.Gems > GameManager.Instance.Gems)
            {
                Message += " gems";
                Message2 += thisItem.Gems - GameManager.Instance.Gems + " more gems";
                written = true;
            }
        }

        if(thisItem.Gems > GameManager.Instance.Gems && !written)
        {
            Message += " and gems";
            Message2 += " and " + (thisItem.Gems - GameManager.Instance.Gems).ToString() + " more gems";
        }

        Message += string.Format(" to buy this dress. \n");
        Message2 +=   " to buy this";

        ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = Message + Message2;
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void ShowPopUpError ()
	{
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("Sorry, Purchase Insuccessful....Internet Not Connected, Please check your connection!!!");
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void OnDisable ()
	{
		GetComponent<Button> ().onClick.RemoveAllListeners ();
	}

	public void UpdateHairsofFlatmate ()
	{
		var target = RoommateManager.Instance.SelectedRoommate.GetComponent<Flatmate> ();
//		target.data.Hair_style = thisItem.item_id.ToString ();  
		if (thisItem.Catergory == SaloonCategories.Hair) {
			if (target.data.Dress.ContainsKey ("Hair"))
				target.data.Dress.Remove ("Hair");		
		}

		target.data.Dress.Add (thisItem.Catergory.ToString (), thisItem.item_id);

	}

	public void UpdateHairsofPlayer ()
	{
		var target = PlayerManager.Instance.playerInfo;
		if (thisItem.Catergory == SaloonCategories.Hair) {

			if (target.DressWeared.ContainsKey ("Hair"))
				target.DressWeared.Remove ("Hair");
		}
		target.DressWeared.Add (thisItem.Catergory.ToString (), thisItem.item_id);
	}
}
