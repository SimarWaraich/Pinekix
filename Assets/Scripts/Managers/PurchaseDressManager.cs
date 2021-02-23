using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

[Serializable]
public class DressItem
{
	public ShoppingCategories Catergory;
	public string Name;
	public int Id;
	public Sprite DisplayIcon;
	public bool IsBusy;
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
    public Sprite[] DressesImages;
//	public Sprite[] Brown_Images;
//	public Sprite[] Black_Images;

	public DressItem (int category, string name, int id, Sprite icon, bool unlocked, bool purchased, GenderEnum gender, int level, int price, int gems, string[] _partname, Sprite[] white_Images/*, Sprite[] brown_Images, Sprite[] black_Images*/, bool vip)
	{
		Catergory = (ShoppingCategories)category;
		Name = name;
		Id = id;
		DisplayIcon = icon;
		Unlocked = unlocked;
		Purchased = purchased;
		Gender = gender;
		Level = level;
		Price = price;
		Gems = gems;
		PartName = _partname;
        VipSubscription = vip;
		DressesImages = white_Images;
//		Brown_Images = brown_Images;
//		Black_Images = black_Images;


	}
}

public enum ShoppingCategories
{
	Tops = 0,
	Pants = 1,
    Shorts = 2,
    Shoes = 3,
    Skirts = 4,
    Dresses = 5,
    Jackets = 6,
    Misc = 7,
    SeasonalClothes= 8
}

public class PurchaseDressManager : Singleton <PurchaseDressManager>
{
	public GameObject WardrobeContainer;
	public GameObject ShoppingContainer;
	public GameObject FashionEventContainer;
	public GameObject CoOpEventContainer;

	public GameObject DressItemsPrefab;
	public List<DressItem> AllDresses;

	public Button ConfirmationButton;

	public DressView selectedDress;

    public Dictionary <string ,DressItem> selectedDresses = new Dictionary<string, DressItem>();
    public List <string> TargetTempDresses = new List<string>();
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
	}

	void CheckUnlockConditions ()
	{
		for (int i = 0; i < AllDresses.Count; i++) {
			if (GameManager.Instance.level >= AllDresses [i].Level)
				AllDresses [i].Unlocked = true;
			else
				AllDresses [i].Unlocked = false;
		}
	}

	public void IntializeDressesforShopping (int shoppingCategories)
	{
		StartCoroutine (IeInitializeDressesForShopping (shoppingCategories));
	}


	public IEnumerator IeInitializeDressesForShopping (int shoppingCategories)
	{
//		yield return DownloadContent.Instance.IeCheckForDownloadList ();
		yield return null;

		DeleteAllDresses ();
        CreateCategoryButtons(ScreenManager.Instance.ClothsShop);

		var ListCategoryWise = AllDresses.Where (dress => dress.Catergory == (ShoppingCategories)shoppingCategories).ToList ();
		ListCategoryWise.ForEach (dress => {	
			/// this is to update the unlock condition of all the dress, accoroding to level or gems in gamemanager 
			if (GameManager.Instance.level >= dress.Level /*&& GameManager.Instance.Gems >= dress.Gems*/) {
				int Index = AllDresses.IndexOf (dress);
				AllDresses [Index].Unlocked = true;
				dress.Unlocked = true;		
			} else {
				dress.Unlocked = false;
			}

            if(dress.VipSubscription)
            {   
//                int Index = AllDresses.IndexOf (dress);
//                AllDresses [Index].Unlocked = true;
                if(VipSubscriptionManager.Instance.VipSubscribed)
                    dress.Unlocked = true;
                else
                    dress.Unlocked = false;
            }

			if (GameManager.GetGender () == dress.Gender && !dress.Purchased) {
				GameObject Go = Instantiate (DressItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = ShoppingContainer.transform;
				Go.transform.localScale = Vector3.one;

				Init (Go, dress);
			}
		});
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	public void IntializeDressesforWardrobe (int shoppingCategories)
	{
//		ScreenManager.Instance.OpenedCustomizationScreen = "WardRobe";
		StartCoroutine (IeInitializeDressesForWardRobe (shoppingCategories));
	}


	public IEnumerator IeInitializeDressesForWardRobe (int shoppingCategories)
	{
//		yield return DownloadContent.Instance.IeCheckForDownloadList ();
		yield return null;

		DeleteAllDresses ();
        CreateCategoryButtons(ScreenManager.Instance.MyCloset);
        var ListCategoryWise = AllDresses.Where (dress => dress.Catergory == (ShoppingCategories)shoppingCategories).ToList ();

		ListCategoryWise.ForEach (dress => {					
			if (dress.Gender == GameManager.GetGender () && dress.Purchased) {
				GameObject Go = Instantiate (DressItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Init (Go, dress);
				Go.transform.parent = WardrobeContainer.transform;
				Go.transform.localScale = Vector3.one;
			}
		});
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	public void IntializeDressesforFashionAndCatwalk (int shoppingCategories)
	{
		IntializeDressesforFashionEvent (shoppingCategories, PurchaseDressManager.Instance.FashionEventContainer.transform);
	}

	public void IntializeDressesforCoop (int shoppingCategories)
	{
		IntializeDressesforFashionEvent (shoppingCategories, PurchaseDressManager.Instance.CoOpEventContainer.transform);
	}

	public void IntializeDressesforFashionEvent (int shoppingCategories, Transform Container)
	{
		StartCoroutine (IeInitializeDressesForEvent (shoppingCategories, Container));
	}


	public IEnumerator IeInitializeDressesForEvent (int shoppingCategories, Transform Container)
	{
//		yield return DownloadContent.Instance.IeCheckForDownloadList ();
		yield return null;

		for (int i = 0; i < Container.transform.childCount; i++) {
			GameObject.Destroy (Container.transform.GetChild (i).gameObject);
		}	

		var ListCategoryWise = AllDresses.Where (dress => dress.Catergory == (ShoppingCategories)shoppingCategories).ToList ();

		ListCategoryWise.ForEach (dress => {					
			if (dress.Gender == GameManager.GetGender () && dress.Purchased) {
				GameObject Go = Instantiate (DressItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Init (Go, dress);
				Go.transform.parent = Container;
				Go.transform.localScale = Vector3.one;
			}
		});
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	public void Init (GameObject target, DressItem dress)
	{
		var dresstarget = target.GetComponent<DressView> ();

		dresstarget.thisDress = dress;
	}

	public void	UpdateDresses (DressItem Dress)
	{		
		int Index = AllDresses.IndexOf (Dress);
		AllDresses [Index].Unlocked = true;
		Dress.Unlocked = true;
	}


    public void UpdateDress (DressInfo info, string Name, string SubCategory, int level, int Coins, int Gems, int Key, int Vip)
	{
//		DressInfo info = dressPrefab.GetComponent<DressInfo> ();
		int subcat = 0;

		switch (SubCategory) 
        {                
            case "Tops":
    			subcat = 0;
    			break;
            case "Pants":
    			subcat = 1;
    			break;
            case "Shots":// To be changed to Shorts
    			subcat = 2;
    			break;
            case "Shoes":
    			subcat = 3;
    			break;
            case "Skirts":
                subcat = 4;
                break;         
            case "Dresses":
                subcat = 5;
                break;
            case "Jackets":
                subcat = 6;
                break;
            case "Misc":
            default:                
                subcat = 7;
                break;
            case "SeasonalClothes":
                subcat = 8;
                break;
		}

		var gender = (GenderEnum)info.Gender;

		DressItem DressData = new DressItem (subcat,
			                      info.name,
			                      Key,
			                      info.Icon,
			                      false,
			                      false,
			                      gender,
			                      level,
			                      Coins,
			                      Gems,
			                      info.BodyPartName.ToArray (),
			                      info.DressesSprites.ToArray ()
//			                      ,info.BodyPartSprites_Brown.ToArray (),
//			                      info.BodyPartSprites_Black.ToArray ()
            , Vip == 1?true:false
        );

		AllDresses.Add (DressData);
    }



	public void UpdatePrizeDress (DressInfo dress, string Name, string SubCategory, int level, int Coins, int Gems, int key)
	{		
		int subcat = 0;

		switch (SubCategory) {
            case "Tops":
                subcat = 0;
                break;
            case "Pants":
                subcat = 1;
                break;
            case "Shots":// To be changed to Shorts
                subcat = 2;
                break;
            case "Shoes":
                subcat = 3;
                break;
            case "Skirts":
                subcat = 4;
                break;         
            case "Dresses":
                subcat = 5;
                break;
            case "Jackets":
                subcat = 6;
                break;
            case "Misc":
            default:                
                subcat = 7;
                break;
            case "SeasonalClothes":
                subcat = 8;
                break;
		}

		var gender = (GenderEnum)dress.Gender;

		var charGender = (GenderEnum)dress.Gender;

		if (PlayerPrefs.GetString ("CharacterType") == "Male")
			charGender = GenderEnum.Male;
		else
			charGender = GenderEnum.Female;

		if (gender != charGender)
			return;

		DressItem DressData = new DressItem (subcat,
			                      dress.name,
			                      key,
			                      dress.Icon,
			                      true,
			                      true,
			                      gender,
			                      level,
			                      Coins,
			                      Gems,
			                      dress.BodyPartName.ToArray (),
			                      dress.DressesSprites.ToArray ()
//                                  ,dress.BodyPartSprites_Brown.ToArray (),
//			                      dress.BodyPartSprites_Black.ToArray ()
            , false
        );
		

		AllDresses.Add (DressData);
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
//	public void AddDress (int category, string name, Sprite icon, bool unlocked, bool purchased, GenderEnum gender, int level, int price, int gems, string[] bodyparts, Sprite[] White_Images/*, Sprite[] Brown_Images, Sprite[] Black_Images*/)
//	{
////        DressItem DressData = new DressItem (category, name, 0, icon, unlocked, purchased, gender, level, price, gems, bodyparts, White_Images/*, Brown_Images, Black_Images*/,);
//
//
//		GameObject Go = Instantiate (DressItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
//		Go.transform.parent = ShoppingContainer.transform;
//		Go.transform.localScale = Vector3.one;
//
//		Init (Go, DressData);
//	}

	/// <summary>
	/// Deletes all dresses after closed button in clicked to prevent duplicating items.
	/// </summary>
	public void DeleteAllDresses ()
	{
		selectedDress = null;

		for (int i = 0; i < ShoppingContainer.transform.childCount; i++) {
			GameObject.Destroy (ShoppingContainer.transform.GetChild (i).gameObject);
		}

		for (int i = 0; i < WardrobeContainer.transform.childCount; i++) {
			GameObject.Destroy (WardrobeContainer.transform.GetChild (i).gameObject);
		}
		for (int i = 0; i < FashionEventContainer.transform.childCount; i++) {
			GameObject.Destroy (FashionEventContainer.transform.GetChild (i).gameObject);
		}
	}

    bool IsCategoryGenderWise(ShoppingCategories Catergory)
    {

        if(GameManager.GetGender()== GenderEnum.Male)
            return Catergory == ShoppingCategories.Tops || Catergory == ShoppingCategories.Pants || Catergory == ShoppingCategories.Shorts || Catergory == ShoppingCategories.Shoes 
                || Catergory == ShoppingCategories.Jackets || Catergory == ShoppingCategories.Misc;
        else 
            return Catergory == ShoppingCategories.Tops || Catergory == ShoppingCategories.Pants || Catergory == ShoppingCategories.Shorts 
                || Catergory == ShoppingCategories.Shoes || Catergory == ShoppingCategories.Skirts || Catergory== ShoppingCategories.Dresses|| Catergory == ShoppingCategories.Misc;

    }
//    ScreenManager.Instance.ClothsShop
    void CreateCategoryButtons(GameObject Screen)
    {
        //        for (int i = 0; i < Screen.transform.FindChild("CategoryButtons").transform.childCount; i++)  

        var Categories = Enum.GetValues(typeof(ShoppingCategories));

        for (int i = 0; i < Categories.Length; i++)
        {
            var StringName =  ((ShoppingCategories)Categories.GetValue(i)).ToString();

            if (IsCategoryGenderWise((ShoppingCategories)Categories.GetValue(i)))
                Screen.transform.FindChild("CategoryButtons").FindChild(StringName).gameObject.SetActive(true);
            else if(StringName == "SeasonalClothes")
            {
                if(Screen == ScreenManager.Instance.MyCloset)
                    Screen.transform.FindChild("CategoryButtons").FindChild(StringName).gameObject.SetActive(true);
                else
                {
                    
                }
            }
            else
                Screen.transform.FindChild("CategoryButtons").FindChild(StringName).gameObject.SetActive(false);           
        }
    }

	public void ShowConfirmationPopUp ()
	{
        var tut = GameManager.Instance.GetComponent<Tutorial>();

        if (tut._DressPurchased)
        {
            if (TargetTempDresses.Contains("Dresses") || (TargetTempDresses.Contains("Upper") && TargetTempDresses.Contains("Lower")))
            {

            }
            else
            {
                ScreenAndPopupCall.Instance.ShowPopOfDescription("Some of Garments of this Character are visible. Please wear a full dress");            
                return;
            }
        }

        if (selectedDresses.Count !=0) {
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
			ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
			ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";

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
        if (selectedDresses.Count != 0 && DressManager.Instance.SelectedCharacter) {
//			if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
            var TargetChar = DressManager.Instance.SelectedCharacter.GetComponent<Flatmate>();
//            temp.AddRange(TargetChar.data.Dress.Select(kvp=> kvp.Key).ToList());      

            btn.onClick.AddListener (() =>
                {
                    foreach(var dress in selectedDresses)
                    {

                        if (TargetChar.data.Dress.ContainsKey("Dresses"))
                        {
                            TargetChar.data.Dress.Remove("Dresses");

                            var allCustom = Resources.Load<CustomisationAllData>("CustomisationAllData");
                            if(GameManager.GetGender()== GenderEnum.Female)
                            {
                                DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllGirl.BodyPartName.ToArray(), allCustom.EmptyAllGirl.DressesSprites.ToArray());
                            }else
                            {
                                DressManager.Instance.ChangeFlatMateDress (allCustom.EmptyAllBoy.BodyPartName.ToArray(), allCustom.EmptyAllBoy.DressesSprites.ToArray());
                            }
                        }
                        DressManager.Instance.ChangeFlatMateDress (dress.Value.PartName, dress.Value.DressesImages);
                    }
            });

//			if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//				btn.onClick.AddListener (() => DressManager.Instance.ChangeFlatMateDress (selectedDress.thisDress.PartName, selectedDress.thisDress.Brown_Images));
//
//			if (DressManager.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
//				btn.onClick.AddListener (() => DressManager.Instance.ChangeFlatMateDress (selectedDress.thisDress.PartName, selectedDress.thisDress.Black_Images));
		
			btn.onClick.AddListener (() => UpdateDressofAllCharacter ());
		}

		btn.onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
		btn.onClick.AddListener (() => ScreenManager.Instance.MoveScreenToBack ());
		btn.onClick.AddListener (() => ScreenAndPopupCall.Instance.CloseCharacterCamera ());
		btn.onClick.AddListener (() => GameManager.Instance.GetComponent <Tutorial> ().DressPurchasing ());
	}

	public void SelectDress (DressView dress)
	{
		if (selectedDress) 
			selectedDress.GetComponent <Button> ().interactable = true;
		

		selectedDress = dress;

        //If dress is of Dresses Category Remove all Dresses
        if(selectedDress.thisDress.Catergory == ShoppingCategories.Dresses ||selectedDress.thisDress.Catergory == ShoppingCategories.SeasonalClothes )
        {
            if (selectedDresses.ContainsKey(ShoppingCategories.Pants.ToString()))
                selectedDresses.Remove(ShoppingCategories.Pants.ToString());

            if (selectedDresses.ContainsKey(ShoppingCategories.Shorts.ToString()))
                selectedDresses.Remove(ShoppingCategories.Shorts.ToString());

            if (selectedDresses.ContainsKey(ShoppingCategories.Skirts.ToString()))
                selectedDresses.Remove(ShoppingCategories.Skirts.ToString());

            if (selectedDresses.ContainsKey(ShoppingCategories.Dresses.ToString()))
                selectedDresses.Remove(ShoppingCategories.Dresses.ToString());   
            if (selectedDresses.ContainsKey(ShoppingCategories.Tops.ToString()))
                selectedDresses.Remove(ShoppingCategories.Tops.ToString());

            if (selectedDresses.ContainsKey(ShoppingCategories.Jackets.ToString()))
                selectedDresses.Remove(ShoppingCategories.Jackets.ToString());       
           
            if (PurchaseDressManager.Instance.TargetTempDresses.Contains("Dresses"))
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Dresses");
            if (PurchaseDressManager.Instance.TargetTempDresses.Contains("Upper"))
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Upper");
            if (PurchaseDressManager.Instance.TargetTempDresses.Contains("Lower"))
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Lower");

            PurchaseDressManager.Instance.TargetTempDresses.Add("Dresses");
        }
        //If dress is in Legs then reomve all legs dresses 
        else if(selectedDress.thisDress.Catergory == ShoppingCategories.Pants ||selectedDress.thisDress.Catergory == ShoppingCategories.Shorts||
            selectedDress.thisDress.Catergory == ShoppingCategories.Skirts)
        {
            if (selectedDresses.ContainsKey(ShoppingCategories.Pants.ToString()))
                selectedDresses.Remove(ShoppingCategories.Pants.ToString());
            
            if (selectedDresses.ContainsKey(ShoppingCategories.Shorts.ToString()))
                selectedDresses.Remove(ShoppingCategories.Shorts.ToString());
            
            if (selectedDresses.ContainsKey(ShoppingCategories.Skirts.ToString()))
                selectedDresses.Remove(ShoppingCategories.Skirts.ToString());
            
            if (selectedDresses.ContainsKey(ShoppingCategories.Dresses.ToString()))
                selectedDresses.Remove(ShoppingCategories.Dresses.ToString());     

            if (PurchaseDressManager.Instance.TargetTempDresses.Contains("Lower"))
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Lower");
            if (PurchaseDressManager.Instance.TargetTempDresses.Contains("Dresses"))
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Dresses");
            
            PurchaseDressManager.Instance.TargetTempDresses.Add("Lower");
        }
        //If dress is in Torso then reomve all Torso dresses 
        else if(selectedDress.thisDress.Catergory == ShoppingCategories.Tops ||selectedDress.thisDress.Catergory == ShoppingCategories.Jackets)
        {
            if (selectedDresses.ContainsKey(ShoppingCategories.Tops.ToString()))
                selectedDresses.Remove(ShoppingCategories.Tops.ToString());
            
            if (selectedDresses.ContainsKey(ShoppingCategories.Jackets.ToString()))
                selectedDresses.Remove(ShoppingCategories.Jackets.ToString());

            if (PurchaseDressManager.Instance.TargetTempDresses.Contains("Upper"))
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Upper");
            if (PurchaseDressManager.Instance.TargetTempDresses.Contains("Dresses"))
                PurchaseDressManager.Instance.TargetTempDresses.Remove("Dresses");
            
            PurchaseDressManager.Instance.TargetTempDresses.Add("Upper");
        }    

        if(selectedDresses.ContainsKey(dress.thisDress.Catergory.ToString()))
        {
            selectedDresses[dress.thisDress.Catergory.ToString()] = dress.thisDress;
        }else             
            selectedDresses.Add(dress.thisDress.Catergory.ToString(),dress.thisDress);
        
		selectedDress.GetComponent <Button> ().interactable = false;
		ConfirmationButton.interactable = true;
		if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.CoOpEventScreen)
			ScreenManager.Instance.CoOpWardRobeScreen.transform.FindChild ("RegisterButton").GetComponent <Button> ().interactable = true;
		else if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.FashionEvent)
			ScreenManager.Instance.FashionEvent.transform.FindChild ("RegisterButton").GetComponent<Button> ().interactable = true;
	}


	public void UpdateDressofAllCharacter ()
	{
        if (DressManager.Instance.SelectedCharacter == null)
			return;
        if (DressManager.Instance.SelectedCharacter.GetComponent <Flatmate>())
        {

            foreach (var dress in selectedDresses)
            {
            
                var target = DressManager.Instance.SelectedCharacter.GetComponent<Flatmate>();

                if (dress.Value.Catergory == ShoppingCategories.Dresses || dress.Value.Catergory == ShoppingCategories.SeasonalClothes)
                {
                    if (target.data.Dress.ContainsKey("Tops"))
                        target.data.Dress.Remove("Tops");
                    if (target.data.Dress.ContainsKey("Pants"))
                        target.data.Dress.Remove("Pants"); 
                    if (target.data.Dress.ContainsKey("Shorts"))
                        target.data.Dress.Remove("Shorts");
                    if (target.data.Dress.ContainsKey("Skirts"))
                        target.data.Dress.Remove("Skirts");
                }
                else if(dress.Value.Catergory  == ShoppingCategories.Pants ||dress.Value.Catergory  == ShoppingCategories.Shorts||
                    dress.Value.Catergory == ShoppingCategories.Skirts)
                {
                    if (target.data.Dress.ContainsKey(ShoppingCategories.Pants.ToString()))
                        target.data.Dress.Remove(ShoppingCategories.Pants.ToString());

                    if (target.data.Dress.ContainsKey(ShoppingCategories.Shorts.ToString()))
                        target.data.Dress.Remove(ShoppingCategories.Shorts.ToString());

                    if (target.data.Dress.ContainsKey(ShoppingCategories.Skirts.ToString()))
                        target.data.Dress.Remove(ShoppingCategories.Skirts.ToString());

                    if (target.data.Dress.ContainsKey(ShoppingCategories.Dresses.ToString()))
                        target.data.Dress.Remove(ShoppingCategories.Dresses.ToString());           
                }
                else if(dress.Value.Catergory == ShoppingCategories.Tops ||dress.Value.Catergory == ShoppingCategories.Jackets)
                {
                    if (target.data.Dress.ContainsKey(ShoppingCategories.Tops.ToString()))
                        target.data.Dress.Remove(ShoppingCategories.Tops.ToString());

                    if (target.data.Dress.ContainsKey(ShoppingCategories.Jackets.ToString()))
                        target.data.Dress.Remove(ShoppingCategories.Jackets.ToString());
                }


                if(target.data.Dress.ContainsKey(dress.Value.Catergory.ToString()))
                    target.data.Dress.Remove(dress.Value.Catergory.ToString());
                target.data.Dress.Add(dress.Value.Catergory.ToString(), dress.Value.Id);
            }
        }  

//		else
//			selectedDress.UpdateDressofPlayer ();

		RoommateManager.Instance.UpdateData ();

        selectedDresses = new Dictionary<string, DressItem>();

	}
}