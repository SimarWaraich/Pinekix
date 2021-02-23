using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;
using Simple_JSON;


public class DecorController : Singleton<DecorController>
{
	public DecoreCategories currentcategory;
	public GameObject DecorUIPrefab;
	public GameObject Container;

	public GameObject StorageContainer;
	public GameObject DecorEventStorageContainer;
    public GameObject StorageContainerForSociety;

	public List<DecorData> AllDecores;
	public List<DecorData> TempDecorForEvent;

	public List<Decor3DView> PlacedDecors;
	public List<string> PlacedDecorsName;

	public GameObject SelectedDecor;
	public static bool AddItemEnable = false;
	public List<GameObject> DownloadedDecors = new List<GameObject> ();
	public Button lastButtonClicked;

	public List<Button> CategoryButtons;

	public List<GameObject> _timers = new List<GameObject> ();
    public bool isForSociety = false;
//	public Sprite DecorBelowImage;

	void Awake ()
	{
		this.Reload ();
	}

    public void AddDecorToList (int  Id, Decor3DView info, string Name, string SubCat, int level, int gems, int price, int vipsub)
	{
//		DecorInfo info = target.GetComponent<DecorInfo> ();
		int subcat = 0;

		/*
		 * 
		 * Sofa = 0,
	Chairs = 1,
	Tables = 2,
	Beds = 3,
	Techs = 4,
	Plants = 5,
	Appliance = 6,
	Plumbing = 7,
	Cabinets = 8,
	Misc = 9
		 */
		switch (SubCat.Trim ('"')) {
		case "Sofas":
			subcat = 0;
			break;
		case "Chairs":
			subcat = 1;
			break;
		case "Tables":
			subcat = 2;
			break;
		case "Beds":
			subcat = 3;
			break;	
		case "Plants":
			subcat = 4;
			break;	
		case "Electronics":
			subcat = 5;
			break;
            case "Living room": // Almirah
            subcat = 6;
            break;
            case "Bedroom"://"Cabinets":
			subcat = 7;
			break;
            case "Kitchen"://"Fridges":
			subcat = 8;
			break;
            case "Patio"://"Radio":
                subcat = 9;
                break;


//                Sofa = 0,
//                Chairs = 1,
//                Tables = 2,
//                Beds = 3,
//                Plants = 4,
//                Electronics = 5,
//                Almirah = 6,
//                Cabinets = 7,
//                Fridges = 8,
//                Radio= 9
		}

        DecorData data = new DecorData (Name, Id, subcat, info.decorInfo.DisplayIcon, false, false, level, gems, price, vipsub == 1? true:false);

		AllDecores.Add (data);

        DownloadedDecors.Add (info.gameObject);

	}

	public void AddPrizeDecorList (int Id, DecorInfo target, string Name, string SubCat, int level, int gems, int price)
	{
		
		int subcat = 0;

		switch (SubCat.Trim ('"')) {
            case "Sofas":
                subcat = 0;
                break;
            case "Chairs":
                subcat = 1;
                break;
            case "Tables":
                subcat = 2;
                break;
            case "Beds":
                subcat = 3;
                break;  
            case "Plants":
                subcat = 4;
                break;  
            case "Electronics":
                subcat = 5;
                break;
            case "Cabinets":
                subcat = 7;
                break;
            case "Misc":
            default:
                subcat = 6;
                break;
		}

        DecorData data = new DecorData (Name, Id, subcat, target.Icon, true, true, level, gems, price, false);

		AllDecores.Add (data);

		DownloadedDecors.Add (target.Prefab);

	}

	void Start ()
	{
		CheckUnlockConditions ();
	}

	void CheckUnlockConditions ()
	{
		for (int i = 0; i < AllDecores.Count; i++) {
			if (GameManager.Instance.level >= AllDecores [i].Level && GameManager.Instance.Gems >= AllDecores [i].Gems)
				AllDecores [i].Unlocked = true;
			else
				AllDecores [i].Unlocked = false;
		}
    }

    public void Create3DDecoreForSociety (DecorData decor, bool isPlaced,Vector3 position, int direction = 0)
    {
        ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().BackFromSocietyStorage();
        var asset = Resources.Load<Decor3DView> ("Decors/" + decor.Name.Trim ('"'));
        if (asset == null) {
            //          List<string> downloadednames = new List<string> ();
            for (int i = 0; i < DownloadedDecors.Count; i++) {
                if (DownloadedDecors [i].GetComponent <Decor3DView> ().name == decor.Name.Trim ('"') || DownloadedDecors [i].GetComponent <Decor3DView> ().name == FirstCharToUpper (decor.Name.Trim ('"')))
                    asset = DownloadedDecors [i].GetComponent<Decor3DView> ();
            }
        }
        GameObject Go = (GameObject)Instantiate (asset.gameObject, Vector3.zero, Quaternion.identity);
     
        var RoomforSociety = GameObject.Find("SocietyPartyRoom");
        Go.transform.SetParent(RoomforSociety.transform);
        Go.GetComponent<Decor3DView>().SetPositionofThisItem(position,direction);         

        Go.SetMaterialRecursively ();
        Go.GetComponent<Decor3DView> ().CreateDecore (decor);
        Go.GetComponent<DragSnap>().grid = Go.transform.parent.GetChild(5).gameObject;//grid Container
        Go.GetComponent<Decor3DView>().Placed = isPlaced;
        Go.GetComponent<Decor3DView>().isForSociety = true;

        if (isPlaced)
        {
            Go.transform.GetChild(1).gameObject.SetActive(false);
            Go.GetComponent <DragSnap> ().enabled = false;
            Go.GetComponent<Decor3DView>().Correct();
        }
        else
        {
            Go.transform.GetChild(0).gameObject.SetActive(true);
            Go.GetComponent <DragSnap> ().enabled = true;
        }
    }

	public void Create3DDecore (DecorData decor)
	{
		var asset = Resources.Load<Decor3DView> ("Decors/" + decor.Name.Trim ('"'));
		if (asset == null) {
			//			List<string> downloadednames = new List<string> ();
			for (int i = 0; i < DownloadedDecors.Count; i++) {
				if (DownloadedDecors [i].GetComponent <Decor3DView> ().name == decor.Name.Trim ('"') || DownloadedDecors [i].GetComponent <Decor3DView> ().name == FirstCharToUpper (decor.Name.Trim ('"')))
					asset = DownloadedDecors [i].GetComponent<Decor3DView> ();
				//				downloadednames.Add (DownloadedDecors [i].name);
			}

			//			var j = downloadednames.IndexOf (decor.Name);
			//			asset = DownloadedDecors [j].GetComponent<Decor3DView> ();
		}

		GameObject Go = (GameObject)Instantiate (asset.gameObject, Vector3.zero, Quaternion.identity);
    
		Go.SetMaterialRecursively ();

		Go.GetComponent<Decor3DView> ().CreateDecore (decor);

		/// Updated by rehan on 17/11/2016 for decor placement
//		Destroy (Go.GetComponent<DragSnap> ());
//		Go.AddComponent<DragSnap> ();

//		Destroy (Go.GetComponent<DecorOrderInLayer> ());
//		Go.AddComponent<DecorOrderInLayer> ();


		Go.transform.GetChild (0).gameObject.SetActive (true);
	}

	public static string FirstCharToUpper (string input)
	{
		if (String.IsNullOrEmpty (input))
			throw new ArgumentException ("ARGH!");
		return input.First ().ToString ().ToUpper () + input.Substring (1);
	}


	public void IntializedecorItemesforDecor (int Categories)
	{
		EmptyAllDecors ();
		for (int i = 0; i < CategoryButtons.Count; i++) {
			CategoryButtons [i].interactable = true;
		}
		CategoryButtons [Categories].interactable = false;

		AllDecores.ForEach (decorItem => {	
			/// this is to update the unlock condition of all the decorItem, accoroding to level or gems in gamemanager 
			if (GameManager.Instance.level >= decorItem.Level/* && GameManager.Instance.Gems >= decorItem.Gems*/) {
				int Index = AllDecores.IndexOf (decorItem);
				AllDecores [Index].Unlocked = true;
				decorItem.Unlocked = true;
			} else {
				decorItem.Unlocked = false;
			}

            if(decorItem.VipSubscription)
            {   int Index = AllDecores.IndexOf (decorItem);
                AllDecores [Index].Unlocked = true;
                if(VipSubscriptionManager.Instance.VipSubscribed)
                    decorItem.Unlocked = true;
                else
                    decorItem.Unlocked = false;
            }

			if (!decorItem.Purchased && decorItem.decoreCategory == (DecoreCategories)Categories) {
				GameObject Go = Instantiate (DecorUIPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = Container.transform;
				Go.transform.localScale = Vector3.one;
                isForSociety = false;
				Init (Go, decorItem);
			}
		});
	}

	public void IntializedecorItemesforStorage (int Categories)
	{
		EmptyAllDecors ();
		AllDecores.ForEach (decorItem => {	
			/// this is to update the unlock condition of all the decorItem, accoroding to level or gems in gamemanager 
            if (GameManager.Instance.level >= decorItem.Level) {
				int Index = AllDecores.IndexOf (decorItem);
				AllDecores [Index].Unlocked = true;
				decorItem.Unlocked = true;
			} else {
				decorItem.Unlocked = false;
			}       

			bool placed = false;
			foreach (var placeddecor in PlacedDecors) {
				if (placeddecor.name == decorItem.Name) {
					placed = true;
				}
			}

			if (decorItem.Purchased && decorItem.Unlocked && decorItem.decoreCategory == (DecoreCategories)Categories && !PlacedDecorsName.Contains (decorItem.Name) && !placed) {
				GameObject Go = Instantiate (DecorUIPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = StorageContainer.transform;
				Go.transform.localScale = Vector3.one;
                isForSociety = false;
				Init (Go, decorItem);
			}
		});
	}

	public void IntializedecorItemesforDecorEventStorage ()
	{
		EmptyAllDecorsEvent ();

		if (TempDecorForEvent.Count == 0 && AddItemEnable) {
			for (int i = 0; i < AllDecores.Count; i++) {
				if (AllDecores [i].Purchased) {
					TempDecorForEvent.Add (AllDecores [i]);
				}
			}
		}
		if (TempDecorForEvent.Count != 0) {
			for (int j = 0; j < TempDecorForEvent.Count; j++) {
				
				GameObject Go = Instantiate (DecorUIPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = DecorEventStorageContainer.transform;
				Go.transform.localScale = Vector3.one;
				Go.GetComponent<DecorView> ().Item = TempDecorForEvent [j];
				Go.GetComponent<DecorView> ().isForEvent = true;
                isForSociety = false;
            }
		}
	}


    public void IntializedecorForSocietyRoom (int Categories)
    {
        EmptyAllDecors ();

        var purchasedDecor = AllDecores.Where(dec => dec.Purchased).ToList();
        var tempPurchaseddecor = new List<DecorData>(); 
        tempPurchaseddecor.AddRange(purchasedDecor);
        for (int i = 0; i < tempPurchaseddecor.Count; i++)
        {            
            var placedInSociety = ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().PlacedDecorInRoom;

            foreach(var pdc in placedInSociety)
            {
                if (tempPurchaseddecor[i].Id == pdc.Id)
                {
                    if(purchasedDecor.Contains(tempPurchaseddecor[i]))
                    purchasedDecor.Remove(tempPurchaseddecor[i]);
                }
            }
        }

        purchasedDecor.ForEach(decor=>{
            
            if (decor.decoreCategory == (DecoreCategories)Categories)
            {
                GameObject Go = Instantiate(DecorUIPrefab, Vector3.zero, Quaternion.identity)as GameObject;
                Go.transform.parent = StorageContainerForSociety.transform;
                Go.transform.localScale = Vector3.one;
                Go.GetComponent<DecorView>().Item = decor;
                Go.GetComponent<DecorView> ().isForSociety = true;
                isForSociety = true;
            }
        });
    }

	void Init (GameObject go, DecorData decor)
	{
		go.GetComponent <DecorView> ().Item = decor;
	}

	void EmptyAllDecors ()
	{
		for (int i = 0; i < Container.transform.childCount; i++) {
			GameObject.Destroy (Container.transform.GetChild (i).gameObject);
		}
		for (int i = 0; i < StorageContainer.transform.childCount; i++) {
			GameObject.Destroy (StorageContainer.transform.GetChild (i).gameObject);
		}
        for (int i = 0; i < StorageContainerForSociety.transform.childCount; i++) {
            GameObject.Destroy (StorageContainerForSociety.transform.GetChild (i).gameObject);
        }
	}

	void EmptyAllDecorsEvent ()
	{
		for (int i = 0; i < Container.transform.childCount; i++) {
			GameObject.Destroy (Container.transform.GetChild (i).gameObject);
		}
		for (int i = 0; i < DecorEventStorageContainer.transform.childCount; i++) {
			GameObject.Destroy (DecorEventStorageContainer.transform.GetChild (i).gameObject);
		}
	}


	public void SetButtonInterChangeable (Button Go)
	{		
		if (lastButtonClicked)
			lastButtonClicked.interactable = true;

		Go.interactable = false;
		lastButtonClicked = Go;
	}


	public void BusyDecor (int id, int eventId)
	{
		for (int i = 0; i < AllDecores.Count; i++) {
			if (id == AllDecores [i].Id) {
				var time = DateTime.Now;
				time.AddHours (2);
				AllDecores [i]._isBusyEndTime = time;
				AllDecores [i]._isBusy = true;
				GameObject obj = new GameObject ();
				obj.AddComponent <DecorTimer> ()._type = "Busy";
				obj.GetComponent <DecorTimer> ()._targetDecor = AllDecores [i];
				obj.GetComponent <DecorTimer> ()._endTime = time;
				obj.GetComponent <DecorTimer> ()._eventId = eventId;
				_timers.Add (obj);
				UpdateTime (time, "busy", AllDecores [i].Id, eventId);
			}
		}
	}

	public void CoolDownOfDecor (int id, int eventId)
	{
		for (int i = 0; i < AllDecores.Count; i++) {
			if (id == AllDecores [i].Id) {
				var time = DateTime.Now;
				time.AddHours (6);
				AllDecores [i]._coolDownEndTime = time;
				AllDecores [i]._isBusy = false;
				AllDecores [i]._isCoolingDown = true;
				GameObject obj = new GameObject ();
				obj.AddComponent <DecorTimer> ()._type = "CoolDown";
				obj.GetComponent <DecorTimer> ()._targetDecor = AllDecores [i];
				obj.GetComponent <DecorTimer> ()._endTime = time;
				obj.GetComponent <DecorTimer> ()._eventId = eventId;
				_timers.Add (obj);
				StartCoroutine (UpdateTime (time, "cooldown", AllDecores [i].Id, eventId));
			}
		}
	}



	public IEnumerator UpdateTime (DateTime time, string type, int decorId, int eventId)
	{
		PositionUpdate data = new PositionUpdate ();
		data.player_id = PlayerPrefs.GetInt ("PlayerId");
		data.item_id = decorId;
//        data.rotation = 
		data.position = "";
		data.cool_down_time_event_id = eventId;
		if (type.ToLower ().Contains ("busy")) {
			data.cool_time = "";
			data.is_busy_time = time.ToString ();
		} else if (type.ToLower ().Contains ("cool")) {
			data.cool_time = time.ToString ();
			data.is_busy_time = "";
		}
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.SendPositions (data));
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			print ("updated");
		}
	}



	public void ShowRewards (int eventid)
	{
		ShowPopUp ("Your submission result is declared", () => {	

			var Previous = PersistanceManager.Instance.GetEventToClaimRewards ();
			Previous.Add (eventid);
			PersistanceManager.Instance.SaveClaimRewardsEvents (Previous);
		});
	}

	//	IEnumerator GetMyPairToShowRewards (int Event_Id)
	//	{
	//		var cd = new CoroutineWithData (VotingPairManager.Instance, VotingPairManager.Instance.IeShowMyPairOnScreenOfDecor (1, true));
	//		yield return cd.coroutine;
	//
	//		if (cd.result != null) {
	//
	//			var vp = (VotingPairForDecor)cd.result;
	//			yield return StartCoroutine (VotingPairManager.Instance.VoteForPlayer_Decor (PlayerPrefs.GetInt ("PlayerId"), vp.pair_id, vp.event_id, true));
	//		} else {
	//
	//		}
	//		yield return StartCoroutine (VotingPairManager.Instance.DeleteMyPairDecor (Event_Id));
	//
	//	}

	void ShowPopUp (string message, UnityEngine.Events.UnityAction OnClickActions)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickActions != null)
				OnClickActions ();
		});
	}

}

public enum DecoreCategories
{
	Sofa = 0,
	Chairs = 1,
	Tables = 2,
	Beds = 3,
	Plants = 4,
	Electronics = 5,
    Almirah = 6,
    Cabinets = 7,
    Fridges = 8,
    Radio= 9
}
//WallColor = 4,Floor = 5,
