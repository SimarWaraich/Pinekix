using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[Serializable]
public class SaloonItem
{
	public SaloonCategories Catergory;
	public string Name;
	public int item_id;
	public Sprite DisplayIcon;
	public bool Unlocked;
	public bool Purchased;
	public GenderEnum Gender;
	[Header ("Unlocking Conditions")]
	public int Level;
	public int Price;
	public int Gems;
    public bool VipSubscription;
	[Header ("Index for selecting clothes")]
	public string[] PartName;
	public Sprite[] HairImages;
//	public Sprite[] Brown_Images;
//	public Sprite[] Black_Images;

    public SaloonItem (int category, string name, int Id, Sprite icon, bool unlocked, bool purchased, GenderEnum gender, int level, int price, int gems, string[] _partname, Sprite[] white_Images/*, Sprite[] brown_Images, Sprite[] black_Images*/, bool vip)
	{
		Catergory = (SaloonCategories)category;
		Name = name;
		item_id = Id;
		DisplayIcon = icon;
		Unlocked = unlocked;
		Purchased = purchased;
		Gender = gender;
		Level = level;
		Price = price;
		Gems = gems;
		PartName = _partname;
		HairImages = white_Images;
        VipSubscription = vip;
//		Brown_Images = brown_Images;
//		Black_Images = black_Images;
	}
}

public enum SaloonCategories
{
	Hair = 0
}

public class PurchaseSaloonManager : Singleton <PurchaseSaloonManager>
{
	public GameObject PurchasedHairContainer;
	public GameObject HairContainer;

	public GameObject ItemsPrefab;
	public List<SaloonItem> AllItems;

	SaloonView selectedItem;

	public Button ConfirmationButton;

	List<SaloonItem> TempAllItem = new List<SaloonItem> ();

	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{			
		GetAllDressesStatus ();
	}

	void GetAllDressesStatus ()
	{
		CheckUnlockConditions ();	

//		if( PersistanceManager.Instance.GetSavedDresses ()!= null)
//		{
//			TempAllDresses = PersistanceManager.Instance.GetSavedDresses ();
//		}
//		else
//		{
		for (int i = 0; i < AllItems.Count; i++) {
			TempAllItem.Add (AllItems [i]);
		}
//			PersistanceManager.Instance.SaveAllDresses (TempAllDresses);
//		}
	}



    public void UpdateSaloon (SaloonInfo info, string Name, string SubCategory, int level, int Coins, int Gems, int Id, int vipsub)
	{
//		SaloonInfo info = SaloonPrefab.GetComponent<SaloonInfo> ();
		int subcat = 0;

		switch (SubCategory) {
		case "Hair":
			subcat = 0;
			break;
		default:
			subcat = 0;
			break;
		}

		var gender = (GenderEnum)info.Gender;

		SaloonItem ItemData = new SaloonItem (subcat,
			                      info.name,
			                      Id,
			                      info.Icon,
			                      false,
			                      false,
			                      gender,
			                      level,
			                      Coins,
			                      Gems,
			                      info.BodyPartName.ToArray (),
			                      info.BodyPartSprites.ToArray ()/*,
			                      info.BodyPartSprites_Brown.ToArray (),
			                      info.BodyPartSprites_Black.ToArray ()*/
            ,vipsub ==1?true:false);

		AllItems.Add (ItemData);

		List<GameObject> Targets = new List<GameObject> ();
		//TODO: create a prefab variable in roommates available data so that we can change the data there and also here we can add


		//		foreach (GameObject go in RoommateManager.Instance.RoommatesAvailable) {
		//			Targets.Add (go);
		//		}
		foreach (GameObject go in RoommateManager.Instance.RoommatesHired) {
			Targets.Add (go);
		}
		Targets.Add (PlayerManager.Instance.MainCharacter);

	}



	void CheckUnlockConditions ()
	{
		for (int i = 0; i < AllItems.Count; i++) {
			if (GameManager.Instance.level >= AllItems [i].Level && GameManager.Instance.Gems >= AllItems [i].Gems)
				AllItems [i].Unlocked = true;
			else
				AllItems [i].Unlocked = false;
		}
	}

	public void IntializeItemsforShopping (int shoppingCategories)
	{
		StartCoroutine (IeInitializeItemsForShopping (shoppingCategories));
	}


	public IEnumerator IeInitializeItemsForShopping (int shoppingCategories)
	{
//		yield return DownloadContent.Instance.IeCheckForDownloadList ();
		yield return null;

		DeleteAllItems ();
		var ListCategoryWise = AllItems.Where (item => item.Catergory == (SaloonCategories)shoppingCategories).ToList ();
		ListCategoryWise.ForEach (item => {	
			/// this is to update the unlock condition of all the dress, accoroding to level or gems in gamemanager 
			if (GameManager.Instance.level >= item.Level /*&& GameManager.Instance.Gems >= item.Gems*/) {
				item.Unlocked = true;
			} else {
				item.Unlocked = false;
			}

            if(item.VipSubscription)
            {  
//                int Index = AllItems.IndexOf (item);
//                AllItems [Index].Unlocked = true;
                if(VipSubscriptionManager.Instance.VipSubscribed)
                    item.Unlocked = true;
                else
                    item.Unlocked = false;
            }

			if (GameManager.GetGender () == item.Gender && !item.Purchased) {
				GameObject Go = Instantiate (ItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = HairContainer.transform;
				Go.transform.localScale = Vector3.one;

				Init (Go, item);
			}
		});
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	public void IntializeItemsforBoutique (int shoppingCategories)
	{
//		ScreenManager.Instance.OpenedCustomizationScreen = "Boutique";
		StartCoroutine (IeInitializeItemsForBoutique (shoppingCategories));
	}


	public IEnumerator IeInitializeItemsForBoutique (int shoppingCategories)
	{
//		yield return DownloadContent.Instance.IeCheckForDownloadList ();
		yield return null;

		DeleteAllItems ();
		var ListCategoryWise = AllItems.Where (item => item.Catergory == (SaloonCategories)shoppingCategories).ToList ();

		ListCategoryWise.ForEach (item => {					
			if (item.Gender == GameManager.GetGender () && item.Purchased) {
				GameObject Go = Instantiate (ItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Init (Go, item);
				Go.transform.parent = PurchasedHairContainer.transform;
				Go.transform.localScale = Vector3.one;
			}
		});
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}


	public void Init (GameObject target, SaloonItem item)
	{
		var dresstarget = target.GetComponent<SaloonView> ();

		dresstarget.thisItem = item;
	}

	public void	UpdateItem (SaloonItem item)
	{		
		TempAllItem.Remove (item);
		item.Purchased = true;
		TempAllItem.Add (item);
	}


    public void UpdateDress (GameObject dressPrefab, string Name, string SubCategory, int level, int Coins, int Gems, int vipsub)
	{
		DressInfo info = dressPrefab.GetComponent<DressInfo> ();
		int subcat = 0;


		var gender = (GenderEnum)info.Gender;

        AddSaloonItem (subcat, info.name, info.Icon, false, false, gender, level, Coins, Gems, info.BodyPartName.ToArray (), info.DressesSprites.ToArray ()/*, info.BodyPartSprites_Brown.ToArray (), info.BodyPartSprites_Black.ToArray ()*/, vipsub == 1?true:false);

	}

	/// <summary>
	/// Create and Instantiate a new the dress in Shopping Screen.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="icon">Icon.</param>
	/// <param name="unlocked">If set to <c>true</c> unlocked.</param>
	/// <param name="purchased">If set to <c>true</c> purchased.</param>
	/// <param name="gender">Gender.</param>
	/// <param name="level">Level.</param>
	/// <param name="price">Price.</param>
	/// <param name="gems">Gems.</param>
	/// <param name="index">Index.</param>
	public void AddSaloonItem (int category, string name, Sprite icon, bool unlocked, bool purchased, GenderEnum gender, int level, int price, int gems, string[] bodyparts, Sprite[] White_Images/*, Sprite[] Brown_Images, Sprite[] Black_Images*/,bool vip)
	{
        SaloonItem itemData = new SaloonItem (category, name, 0, icon, unlocked, purchased, gender, level, price, gems, bodyparts, White_Images/*, Brown_Images, Black_Images*/,vip);
		
		GameObject Go = Instantiate (ItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
		Go.transform.parent = HairContainer.transform;
		Go.transform.localScale = Vector3.one;

		Init (Go, itemData);
	}

	/// <summary>
	/// Deletes all dresses after closed button in clicked to prevent duplicating items.
	/// </summary>
	public void DeleteAllItems ()
	{
		selectedItem = null;
		ConfirmationButton.interactable = false;

		for (int i = 0; i < HairContainer.transform.childCount; i++) {
			GameObject.Destroy (HairContainer.transform.GetChild (i).gameObject);
		}

		for (int i = 0; i < PurchasedHairContainer.transform.childCount; i++) {
			GameObject.Destroy (PurchasedHairContainer.transform.GetChild (i).gameObject);
		}
	}

	public void ShowConfirmationPopUp ()
	{
		if (selectedItem != null) {
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
			ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
			ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";

			var Ok = ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ();
			var Close = ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ();
			Ok.interactable = true;
			Ok.onClick.RemoveAllListeners ();
			Close.onClick.RemoveAllListeners ();

			ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Are you sure you want to confirm ?";
			Ok.gameObject.SetActive (true);
			Close.gameObject.SetActive (true);
			AddListnertoConfirmButton (Ok);
			Close.onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		}
	}

	void AddListnertoConfirmButton (Button btn)
	{
		if (selectedItem != null && DressManager.Instance.SelectedCharacter) {
//			if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
            btn.onClick.AddListener (() => DressManager.Instance.ChangeHairsOfCharacter (selectedItem.thisItem.PartName, selectedItem.thisItem.HairImages));
			
//			if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//				btn.onClick.AddListener (() => DressManager.Instance.ChangeFlatMateDress (selectedItem.thisItem.PartName, selectedItem.thisItem.Brown_Images));
//			
//			if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
//				btn.onClick.AddListener (() => DressManager.Instance.ChangeFlatMateDress (selectedItem.thisItem.PartName, selectedItem.thisItem.Black_Images));
			btn.onClick.AddListener (() => UpdateHairofAllCharacter ());
		}
		btn.onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		btn.onClick.AddListener (() => ScreenManager.Instance.MoveScreenToBack ());
		btn.onClick.AddListener (() => ScreenAndPopupCall.Instance.CloseCharacterCamera ());
		btn.onClick.AddListener (() => ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ());
		btn.onClick.AddListener (() => GameManager.Instance.GetComponent<Tutorial> ().SaloonPurchasing ());
	}

	public void SelectThisItem (SaloonView item)
	{
		if (selectedItem) {
			selectedItem.GetComponent <Button> ().interactable = true;
		}	

		selectedItem = item;
		selectedItem.GetComponent <Button> ().interactable = false;
		ConfirmationButton.interactable = true;
	}


	public void UpdateHairofAllCharacter ()
	{
		if (DressManager.Instance.SelectedCharacter == null || selectedItem == null)
			return;
		
		if (DressManager.Instance.SelectedCharacter.GetComponent <Flatmate> ()) {
			selectedItem.UpdateHairsofFlatmate ();
			RoommateManager.Instance.UpdateData ();
		}
//		else
//			selectedItem.UpdateHairsofPlayer ();
	}

	public SaloonItem FindSaloonWithId (int Id)
	{
		foreach (var hair in AllItems) {
			if (hair.item_id == Id)
				return hair;
		}
		return null;
	}
}