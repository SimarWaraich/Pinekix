using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DressView : MonoBehaviour
{
	public DressItem thisDress;

	void Start ()
	{
//		thisDress.Unlocked = !IsWornbyBusyPlayer ();
		InitDresses ();
	}


	bool IsWornbyBusyPlayer ()
	{
		foreach (var mates in RoommateManager.Instance.RoommatesHired) {
			var hired = mates.GetComponent <Flatmate> ().data;
			if (hired.EventBusyId != 0 && hired.IsBusy) {
				if (hired.Dress.ContainsValue (thisDress.Id))
					return true;
			}
		}
		return false;
	}



//	public void UpdateDressofFlatmate ()
//	{
//		var target = RoommateManager.Instance.SelectedRoommate.GetComponent<Flatmate> ();
//
//		if (thisDress.Catergory == ShoppingCategories.Tops) {			
//			if (target.data.Dress.ContainsKey ("Jeans"))
//				target.data.Dress.Remove ("Jeans");
//			if (target.data.Dress.ContainsKey ("Tops"))
//				target.data.Dress.Remove ("Tops");
//			if (target.data.Dress.ContainsKey ("Clothes"))
//				target.data.Dress.Remove ("Clothes");
//        } else if (thisDress.Catergory == ShoppingCategories.Pants || thisDress.Catergory == ShoppingCategories.Skirts/* ||thisDress.Catergory == ShoppingCategories.Tops*/) {
//			if (target.data.Dress.ContainsKey ("Clothes"))
//				target.data.Dress.Remove ("Clothes");
//			target.data.Dress.Remove (thisDress.Catergory.ToString ());
//		}
//		target.data.Dress.Add (thisDress.Catergory.ToString (), thisDress.Id);
//	}
//
//	public void UpdateDressofPlayer ()
//	{
//		var target = PlayerManager.Instance.playerInfo;
//		if (thisDress.Catergory == ShoppingCategories.Tops) {
//
//			if (target.DressWeared.ContainsKey ("Jeans"))
//				target.DressWeared.Remove ("Jeans");
//			if (target.DressWeared.ContainsKey ("Tops"))
//				target.DressWeared.Remove ("Tops");
//			if (target.DressWeared.ContainsKey ("Clothes"))
//				target.DressWeared.Remove ("Clothes");
//        } else if (thisDress.Catergory == ShoppingCategories.Pants || thisDress.Catergory == ShoppingCategories.Skirts/* ||thisDress.Catergory == ShoppingCategories.Tops*/) {
//			if (target.DressWeared.ContainsKey ("Clothes"))
//				target.DressWeared.Remove ("Clothes");
//			target.DressWeared.Remove (thisDress.Catergory.ToString ());
//		}
//		target.DressWeared.Add (thisDress.Catergory.ToString (), thisDress.Id);
//	}

	public void InitDresses ()
	{
		if (!thisDress.Unlocked) {
			thisDress.Purchased = false;
		}

		transform.GetChild (0).gameObject.SetActive (!thisDress.Purchased && thisDress.Unlocked);
		transform.GetChild (1).gameObject.SetActive (!thisDress.Unlocked);
		transform.GetChild (0).GetComponentInChildren <Text> ().text = thisDress.Price.ToString ();
		GetComponent <Image> ().sprite = thisDress.DisplayIcon;
		GetComponent<Button> ().onClick.RemoveAllListeners ();
			
		ApplyListeners ();
	}

	void PurchaseThisItem ()
	{
		
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";

        string message = " Do you want to purchase this item for " + thisDress.Price + " Coins";
        if(thisDress.Gems > 0)
        {
            message +=  " and "+ thisDress.Gems +" Gems";
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
		if (thisDress.Price < PlayerPrefs.GetInt ("Money")) {

			var button = GameObject.Find ("EventSystem").GetComponent<EventSystem> ().currentSelectedGameObject;
			button.GetComponent<Button> ().interactable = false;

			int item_id = 0;
			string cat = "";
			string sub_cat = "";
			foreach (var downloadeditem in DownloadContent.Instance.downloaded_items) {
				if (downloadeditem.Name.Trim ('"') == thisDress.Name) {
					item_id = downloadeditem.Item_id;
					cat = downloadeditem.Category;
					sub_cat = thisDress.Catergory.ToString ();
				}
			}

			CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.SetPurchasingData (item_id, cat, sub_cat));
			yield return cd.coroutine;

			if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
				button.GetComponent<Button> ().interactable = true;
				PurchaseDressManager.Instance.UpdateDresses (thisDress);
				thisDress.Purchased = true;
				GameObject.Destroy (this.gameObject);
				ScreenManager.Instance.ClosePopup ();
				GameManager.Instance.GetComponent <Tutorial> ().DressPurchasing ();

				QuestManager.Instance.CheckifTaskisDone ("Dress");

				InitDresses ();
//				GameManager.Instance.AddExperiencePoints (10.0f);
				GameManager.Instance.SubtractCoins (thisDress.Price);

				if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
					AchievementsManager.Instance.CheckAchievementsToUpdate ("clothesAcquisition");
				
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

		if (!thisDress.Purchased && !thisDress.Unlocked) {
			GetComponent<Button> ().onClick.AddListener (() => ShowInfoPopUp ());
		} else if (!thisDress.Purchased && thisDress.Unlocked) {            
				GetComponent<Button> ().onClick.AddListener (() => PurchaseThisItem ());
			
//			GetComponent<Button> ().onClick.AddListener (() => GameManager.Instance.GetComponent <Tutorial> ().DressPurchasing ());
		} else if (thisDress.Purchased && thisDress.Unlocked) {		

            GetComponent <Button> ().onClick.AddListener (()=> ChangeDress());

		} else {
			Debug.LogError ("No Condition Saticified in Dress items");
		}		
	}

	void ChecksForTutorial ()
	{
		var Tut = GameManager.Instance.GetComponent <Tutorial> ();
		if (!Tut._DressPurchased && Tut.purchaseDress == 10)
			Tut.DressPurchasing ();
		if (!Tut._FashionEventCompleate && Tut.fashionEvent == 6)
			Tut.FashionEventStart ();
	}


	void CheckFashionEvent ()
	{
		if (GameManager.Instance.GetComponent<Tutorial> ().fashionEvent == 7) {
			GameManager.Instance.GetComponent <Tutorial> ().FashionEventStart ();
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
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = string.Format ("To Unlock this item you need Level {0}", thisDress.Level);
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

        if (thisDress.Price > PlayerPrefs.GetInt ("Money"))
        {
            Message += " coin";
            Message2 += thisDress.Price - PlayerPrefs.GetInt ("Money") + " more coins";
        }else
        {
            if(thisDress.Gems > GameManager.Instance.Gems)
            {
                Message += " gems";
                Message2 += thisDress.Gems - GameManager.Instance.Gems + " more gems";
                written = true;
            }
        }

        if(thisDress.Gems > GameManager.Instance.Gems && !written)
        {
            Message += " and gems";
            Message2 += " and " + (thisDress.Gems - GameManager.Instance.Gems).ToString() + " more gems";
        }

        Message += string.Format(" to buy this dress. \n");
        Message2 +=   " to buy this";

        ScreenManager.Instance.News.transform.FindChild("Message").GetComponent<Text>().text = Message + Message2;
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

    void ChangeDress()
    {
          ChecksForTutorial ();
        //          GetComponent<Button> ().onClick.AddListener (() => DressManager.Instance.ChangeDressForDummyCharacter (thisDress.Index, thisDress.Catergory.ToString ()));
        if (DressManager.Instance.SelectedCharacter) {

//            var TargetChar = DressManager.Instance.SelectedCharacter.GetComponent<Flatmate>();

            bool Condition =PurchaseDressManager.Instance.TargetTempDresses.Contains("Dresses")|| PurchaseDressManager.Instance.selectedDresses.ContainsKey("Dresses");

            if (Condition)
            {
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Dresses");
                PurchaseDressManager.Instance.selectedDresses.Remove("Dresses");

                var allCustom = Resources.Load<CustomisationAllData>("CustomisationAllData");
                if(GameManager.GetGender()== GenderEnum.Female)
                {
                    DressManager.Instance.ChangeDressForDummyCharacter (allCustom.EmptyAllGirl.BodyPartName.ToArray(), allCustom.EmptyAllGirl.DressesSprites.ToArray());
                }else
                {
                    DressManager.Instance.ChangeDressForDummyCharacter (allCustom.EmptyAllBoy.BodyPartName.ToArray(), allCustom.EmptyAllBoy.DressesSprites.ToArray());
                }
            }


         
             DressManager.Instance.ChangeDressForDummyCharacter (thisDress.PartName, thisDress.DressesImages);

            //              if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
            //                  GetComponent<Button> ().onClick.AddListener (() => DressManager.Instance.ChangeDressForDummyCharacter (thisDress.PartName, thisDress.Brown_Images));
            //
            //              if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
            //                  GetComponent<Button> ().onClick.AddListener (() => DressManager.Instance.ChangeDressForDummyCharacter (thisDress.PartName, thisDress.Black_Images));
        }
        PurchaseDressManager.Instance.SelectDress (this);
    }
}
