using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DecorView : MonoBehaviour
{
	public Sprite Locked;
	public bool isForEvent = false;
    public bool isForSociety = false;

	public DecorData Item;

	Button Button;
	TimeSpan ts;

	void Start ()
	{
		Button = GetComponent<Button> ();
		InitDecor ();
	}


	void Update ()
	{
		if (Item._isBusy) {
			if (Item._isBusyEndTime <= DateTime.Now) {
				// finish busy time
			} else {
				// is busy
				Button.interactable = false;
				ts = Item._isBusyEndTime - DateTime.Now;
				Button.transform.GetChild (1).GetChild (1).gameObject.SetActive (false);
				Button.transform.GetChild (1).GetChild (0).GetComponent <Text> ().text = string.Format ("{0}:{1}", ts.Hours, ts.Minutes);


			}
		}

		if (Item._isCoolingDown) {
			if (Item._coolDownEndTime <= DateTime.Now) {
				//finish cooldown
			} else {
				// is on cooldown
				Button.interactable = false;
				ts = Item._coolDownEndTime - DateTime.Now;
				Button.transform.GetChild (1).GetChild (1).gameObject.SetActive (false);
				Button.transform.GetChild (1).GetChild (0).GetComponent <Text> ().text = string.Format ("{0}:{1}", ts.Hours, ts.Minutes);
			}
		}
	}

	void InitDecor ()
	{
		transform.GetChild (0).gameObject.SetActive (!Item.Purchased && Item.Unlocked);
		transform.GetChild (1).gameObject.SetActive (Item.Purchased && Item.Unlocked);

		transform.GetChild (0).GetComponentInChildren <Text> ().text = Item.Price.ToString ();
//		transform.GetChild ().gameObject.SetActive (Item.Unlocked);
		GetComponent <Image> ().sprite = Item.DisplayIcon;
		transform.GetChild (2).gameObject.SetActive (!Item.Unlocked);

		ApplyListners ();
	}

	void ApplyListners ()
	{ 
		Button.onClick.RemoveAllListeners ();
		if (!Item.Purchased && !Item.Unlocked) {
			Button.onClick.AddListener (() => ShowInfoPopUp ());
		} else if (!Item.Purchased && Item.Unlocked) { 
				Button.onClick.AddListener (() => PurchaseThisItem ());
		} else if (Item.Purchased && Item.Unlocked) {
			
//			Button.onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().AttendQuest ());
			if (isForEvent) {
				Button.onClick.AddListener (() => {					
					VotingPairManager.Instance.Create3dUIForSubmission ();//					Destroy (this.gameObject);
                    ScreenManager.Instance.MoveScreenToBack ();
                    GameManager.Instance.GetComponent <Tutorial> ().SofaPurchasing ();
                });
            }
            else if(isForSociety)
            {
                Button.onClick.AddListener(() =>{DecorController.Instance.Create3DDecoreForSociety(Item, false, Vector3.zero);});
            } 
            else {
				Button.onClick.AddListener (() => {
					DecorController.Instance.Create3DDecore (Item);
                     ScreenManager.Instance.MoveScreenToBack ();
                    GameManager.Instance.GetComponent <Tutorial> ().SofaPurchasing ();
				});
			}
		} 
//		else if (Item.Purchased && Item.Unlocked && isForEvent) {
//
//		
//		
//		}		
	}

	void PurchaseThisItem ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
        string message = " Do you want to purchase this item for " + Item.Price + " Coins";
       
        if(Item.Gems > 0)
        {
            message +=  " and "+ Item.Gems +" Gems";
        }

        ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message + "?";
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
		

        if (Item.Price <= PlayerPrefs.GetInt ("Money") && GameManager.Instance.Gems >= Item.Gems) {

			var button = GameObject.Find ("EventSystem").GetComponent<EventSystem> ().currentSelectedGameObject;
			button.GetComponent<Button> ().interactable = false;
			int item_id = 0;
			string cat = "";
			string sub_cat = "";
			foreach (var downloadeditem in DownloadContent.Instance.downloaded_items) {
				if (downloadeditem.Name == Item.Name) {
					item_id = downloadeditem.Item_id;
					cat = downloadeditem.Category;
					sub_cat = Item.decoreCategory.ToString ();
				}
			}

			CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.SetPurchasingData (item_id, cat, sub_cat));
			yield return cd.coroutine;


			print (cd.result);

			if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
				button.GetComponent<Button> ().interactable = true;

				Item.Purchased = true;
				ScreenManager.Instance.ClosePopup ();

//				GameManager.Instance.AddExperiencePoints (10.0f);
				GameManager.Instance.SubtractCoins (Item.Price);
				InitDecor ();	
				var Tut = GameManager.Instance.GetComponent <Tutorial> ();
				if (Tut.purchaseSofa == 4)
					Tut.SofaPurchasing ();
				QuestManager.Instance.CheckifTaskisDone ("Decor");
//			} else {
//				ShowPopUpError ();
//			}
				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("furnitureAcquisition");
			} else {
				button.GetComponent<Button> ().interactable = true;

				ShowPopUpError ();
			}

		} else {
			ShowPopUpLessCoins ();
		}

	}

	void ShowInfoPopUp ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("To Unlock this item you need  Level {0}", Item.Level);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}

	void ShowPopUpLessCoins ()
	{
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
        string Message = "You do not have sufficient";
        string Message2 ="You need ";
        bool written = false;

        if (Item.Price > PlayerPrefs.GetInt ("Money"))
        {
            Message += " coin";
            Message2 += Item.Price - PlayerPrefs.GetInt ("Money") + " more coins";
        }else
        {
            if(Item.Gems > GameManager.Instance.Gems)
            {
                Message += " gems";
                Message2 += Item.Gems - GameManager.Instance.Gems + " more gems";written = true;
            }
        }

        if(Item.Gems > GameManager.Instance.Gems && !written)
            {
                Message += " and gems";
            Message2 += " and " + (Item.Gems - GameManager.Instance.Gems).ToString() + " more gems";
            }
          
        Message += string.Format(" to buy this {0}. \n ", Item.decoreCategory.ToString ());
        Message2 +=   " to buy this";

//        ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("You do not have sufficient coins to buy this {0}. \n " +
//            "You need {1} more coins to buy this.", Item.decoreCategory.ToString (), Item.Price - PlayerPrefs.GetInt ("Money"));
        
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
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("Sorry, Purchase Unsuccessful....Internet Not Connected, Please check your connection!!!");
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());
	}


	void PurchaseCancle ()
	{
		ScreenManager.Instance.ClosePopup ();
	}
}


[Serializable]
public class DecorData
{
	[Header ("Properties")]
	public string Name;
	public int Id;
	public DecoreCategories decoreCategory;
	public Sprite DisplayIcon;
	public bool Unlocked;
	public bool Purchased;

	[Header ("Unlocking Conditions")]
	public int Level;
	public int Gems;

	[Header ("Purchasing Conditions")]
	public int Price;
    public bool VipSubscription = false;

	public bool _isBusy, _isCoolingDown;
	public DateTime _coolDownEndTime, _isBusyEndTime;

	//	public GameObject Prefab3D;

    public DecorData (string name, int id, int category, Sprite icon, bool unlocked, bool purchased, int level, int gems, int price, bool vip)
	{
		Name = name;
		Id = id;
		decoreCategory = (DecoreCategories)category;
		DisplayIcon = icon;
		Unlocked = unlocked;
		Purchased = purchased;
		Level = level;
		Gems = gems;
		Price = price;
        VipSubscription = vip;
	}
}
