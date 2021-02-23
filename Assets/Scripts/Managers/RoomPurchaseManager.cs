using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class RoomPurchaseManager : Singleton<RoomPurchaseManager>
{
	#region PUBLIC_DATA_FOR_GAME_FLAT_MANAGEMENT

	public GameData Gamedata;
	public List<FlatData> flats = new List<FlatData> ();
	public List<GameObject> Addedflats;
	public FlatProductData[] flatProducts;
	public GameObject ConstructionArea;
	public GameObject SaleBanner;
	public float pricebuffer;

	public string TextureData;
	public FlatUpdateData SelectedFlatData;
	public GroundTexture SeletedGroundTexture;
	public WallsColor SelectedWallTexture;
	public GameObject RoomSelectable;
	public List<GameObject> purchaseLands = new List<GameObject> ();
	public List <PurchasedLand> PurchasedLands;

	#endregion

	public List<GameObject> RoomTypePrefeb;
	public GameObject ConstructionPrefeb;

	public GameObject FlatContainer;

	public GameObject StorageContainer;
	public int PurchasedArea;

	private GameObject FlatPurchasedAreaContainer;

	public List<FlatData> FlatInfoData = new List<FlatData> ();

	//	public List <SaleBanner> SaleBanners;

	//	public SaleBanner selectedBanner;
	public static bool OndecorEvent = false;

	public Sprite FlatIcon;
	// this is temporary
	public GameObject purchaseLandPrefab;

	void Awake ()
	{
		this.Reload ();
	}


	public void AddFlatInfo (GameObject flat)
	{
		flat.SetMaterialRecursively ();
		RoomTypePrefeb.Add (flat.GetComponent<ExpansionInfo> ().Prefab);
		FlatPurchasedAreaContainer = GameObject.Find ("FlatPurchasedAreaContainer");
		CreateDefaultRoom ();
	}

	public void AddFlatData (string data, string ground_texture, string wall_texture)
	{
		if (!data.Contains ("0-0")) {
			foreach (var land in PurchasedLands) {
				if ("" + land.x + "-" + land.y == data)
					StartCoroutine (CreateConstructionFlatOnStart (wall_texture, ground_texture, land));				
			}

//			selectedBanner.isPurchased = true;
//			StartCoroutine (CreateConstructionFlatOnStart (wall_texture, ground_texture));
		} else {
			foreach (var land in PurchasedLands) {
				if ("" + land.x + "-" + land.y == data) {
					PurchasedLands.Remove (land);
					Destroy (land.gameObject);
				}
			}

			ApplyTextures (data, wall_texture, ground_texture);

//			SaleBanners.Remove (selectedBanner);
//			Destroy (selectedBanner);
		}
	}



	//	public void AddSpaceData (string data)
	//	{
	//		foreach (var banner in SaleBanners) {
	//			if ("" + banner.x + "-" + banner.y == data)
	//				selectedBanner = banner;
	//		}
	//		if (selectedBanner) {
	//			selectedBanner.isPurchased = true;
	//			GameObject Go = Instantiate (purchaseLandPrefab, selectedBanner.transform.position, Quaternion.identity)as GameObject;
	//			Go.GetComponent <PurchasedLand> ()._thisBanner = selectedBanner;
	//			purchaseLands.Add (Go);
	//			selectedBanner.gameObject.SetActive (false);
	////			ShowBannerofPurchase (selectedBanner);
	//		}
	//	}

	public void IntializeInventoryForFlats ()
	{
		if (!ReModelShopController.Instance.isForEvent) {
			ReModelShopController.Instance.DeleteAllItems ();

			FlatInfoData.ForEach (flat => {	
				GameObject Go = Instantiate (ReModelShopController.Instance.UiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				Go.transform.parent = ReModelShopController.Instance.Container.transform;
				Go.transform.localScale = Vector3.one;

				var newFlat = Go.AddComponent<FlatsUI> ();
				newFlat.Icon = FlatIcon;
				newFlat.data = flat;
			});
		}
	}

	/// <summary>
	/// Creates the default room.
	/// </summary>
	void CreateDefaultRoom ()
	{
		PurchasedArea++;
		GameObject flatArea = (GameObject)Instantiate (RoomTypePrefeb [0], new Vector3 (0, 0, 0), Quaternion.identity)as GameObject;
		GameObject pLand = (GameObject)Instantiate (purchaseLandPrefab, flatArea.transform.position, Quaternion.identity);
		purchaseLands.Add (pLand);
		flatArea.SetMaterialRecursively ();
		flatArea.GetComponent<SpriteRenderer> ().sortingLayerName = "Ground";
//		Go.transform.eulerAngles = new Vector3 (0f,0f,45f);	
		//			flateArea.name = "Room " + PurchasedArea;
		flatArea.transform.parent = FlatPurchasedAreaContainer.transform;
		flatArea.name = FlatInfoData [0].AreaName;
//		flatArea.GetComponent <SpriteRenderer> ().sprite = ReModelShopController.Instance.Grounds [1].Texture;
		// This function is for default room............


		Addflat (FlatInfoData [0].x, FlatInfoData [0].y, flatArea.transform);
		Addedflats.Add (flatArea);

	}

	/// <summary>
	/// Creates the room for decor event.
	/// </summary>
	public void CreateRoomForDecorEvent ()
	{		
		OndecorEvent = true;
		ScreenAndPopupCall.Instance.SetCameraActiveForRoom ();
		EventManagment.Instance.DecorEventList.Clear ();
		DecorController.AddItemEnable = true;
		GameObject flatArea = (GameObject)Instantiate (RoomTypePrefeb [0], new Vector3 (0, 0, 0), Quaternion.identity)as GameObject;
		Instantiate (purchaseLandPrefab, flatArea.transform.position, Quaternion.identity);
		flatArea.transform.parent = ScreenAndPopupCall.Instance.RoomCameraContainer.transform;
		flatArea.transform.localPosition = new Vector3 (0.62f, -2.78f, 0);
		flatArea.name = "RoomForDecorEvent";
		flatArea.GetComponent <SpriteRenderer> ().sprite = ReModelShopController.Instance.Grounds [4].Texture;
		flatArea.SetMaterialRecursively ();
		flatArea.GetComponent<SpriteRenderer> ().sortingLayerName = "Ground";
		flatArea.SetLayerRecursively (LayerMask.NameToLayer ("Ignore Raycast"));
		StartCoroutine (ActionDecorEventButtonOnCameraEnable ());
	}

	/// <summary>
	/// Decor event button action.
	/// </summary>
	IEnumerator ActionDecorEventButtonOnCameraEnable ()
	{
		yield return new WaitForSeconds (0.5f);
		if (ScreenAndPopupCall.Instance.RoomCamera.enabled) {
			EventManagment.Instance._registerButton.interactable = false;
			EventManagment.Instance._createButton.interactable = false;
			EventManagment.Instance._decorButton.interactable = true;
			EventManagment.Instance._groundButton.interactable = true;
			EventManagment.Instance._wallButton.interactable = true;
		} else {
			DecorEventButonAction ();
		}
	}

	public void DecorEventButonAction ()
	{		
		EventManagment.Instance._createButton.interactable = true;
		EventManagment.Instance._decorButton.interactable = false;
		EventManagment.Instance._groundButton.interactable = false;
		EventManagment.Instance._registerButton.interactable = false;
		EventManagment.Instance._wallButton.interactable = false;
	}

	public void CreateConstructionFlat (PurchasedLand Land)
	{	
		PurchasedArea++;
	

		GameObject flateArea = (GameObject)Instantiate (ConstructionPrefeb, Land.transform.position, Quaternion.identity);
//			flateArea.name = "Room " + PurchasedArea;
		flateArea.transform.parent = FlatPurchasedAreaContainer.transform;
		flateArea.name = "" + Land.x + "-" + Land.y;

		flateArea.GetComponent<ConstructionTimer> ().StartCountDownTimer (30f, flateArea.name, Land.x, Land.y);

		//GameManager.Instance.flats.Add (FlatInfoData [flateNo]);

		PurchasedLands.Remove (Land);
		Destroy (Land.gameObject);
		if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
			AchievementsManager.Instance.CheckAchievementsToUpdate ("flatsUpgrade");
		

	}

	//	public IEnumerator CreateConstructionFlatOnStart (string wall_texture, string ground_texture)
	//	{
	//		PurchasedArea++;
	//		if (selectedBanner && selectedBanner.isPurchased) {
	//
	//			GameObject flateArea = (GameObject)Instantiate (ConstructionPrefeb, selectedBanner.transform.position, Quaternion.identity);
	//			flateArea.transform.parent = FlatPurchasedAreaContainer.transform;
	//			flateArea.name = "" + selectedBanner.x + "-" + selectedBanner.y;
	//			flateArea.GetComponent<ConstructionTimer> ().StartCountDownTimer (0f, flateArea.name);
	//
	//			GameObject Go = Instantiate (purchaseLandPrefab, selectedBanner.transform.position, Quaternion.identity)as GameObject;
	//			purchaseLands.Add (Go);
	////			flateArea.GetComponent<ConstructionTimer> ().StartCountDownTimer (0f, flateArea.name);
	//
	//			yield return new WaitForSeconds (0.1f);
	//			//GameManager.Instance.flats.Add (FlatInfoData [flateNo]);
	//
	//			SaleBanners.Remove (selectedBanner);
	//			Destroy (selectedBanner.gameObject);
	//			yield return new WaitForSeconds (0.1f);
	//			ApplyTextures (wall_texture, ground_texture);
	//		}
	//	}

	public IEnumerator CreateConstructionFlatOnStart (string wall_texture, string ground_texture, PurchasedLand Land)
	{	
		PurchasedArea++;
		if (Land) {

			GameObject flateArea = (GameObject)Instantiate (ConstructionPrefeb, Land.transform.position, Quaternion.identity);
			flateArea.transform.parent = FlatPurchasedAreaContainer.transform;
			flateArea.name = "" + Land.x + "-" + Land.y;
			string flatName = flateArea.name;
			flateArea.GetComponent<ConstructionTimer> ().StartCountDownTimer (0f, flateArea.name, Land.x, Land.y);

//			GameObject Go = Instantiate (purchaseLandPrefab, Land.transform.position, Quaternion.identity)as GameObject;
//			purchaseLands.Add (Go);
		
			yield return new WaitForSeconds (0.1f);
			//GameManager.Instance.flats.Add (FlatInfoData [flateNo]);

			PurchasedLands.Remove (Land);
			Destroy (Land.gameObject);
			yield return new WaitForSeconds (0.1f);
			ApplyTextures (flatName, wall_texture, ground_texture);
		}
	}


	public void ApplyTextures (string flatName, string wall_texture, string ground_texture)
	{
		// this is for temp unless we find another wayout
		var Flat = RoomPurchaseManager.Instance.Addedflats [RoomPurchaseManager.Instance.Addedflats.Count - 1].GetComponent<Flat3D> ();

		for (int i = 0; i < RoomPurchaseManager.Instance.Addedflats.Count; i++) {
			if (RoomPurchaseManager.Instance.Addedflats [i].name == flatName)
				Flat = RoomPurchaseManager.Instance.Addedflats [i].GetComponent<Flat3D> ();
		}

		foreach (var obj in ReModelShopController.Instance.Grounds) {
			if (obj.Name == ground_texture) {
				Flat.ChangeGroungColor (obj.Texture);
				Flat.GroundTextureName = obj.Name;
			}
		}

		foreach (var obj in ReModelShopController.Instance.WallsColors) {
			if (obj.Name == wall_texture) {
//				Flat.Walls.ChangeWallColors (obj.Textures);
				Flat.Walls.ChangeWallColorsNew (obj);
				Flat.WallColourNames = obj.Name;
			}
		}
	}


	public void CreateRoom (GameObject Go, int x, int y)
	{
		GameObject flatArea = (GameObject)Instantiate (RoomTypePrefeb [0], Go.transform.position, Quaternion.identity);

		flatArea.SetMaterialRecursively ();
		flatArea.GetComponent<SpriteRenderer> ().sortingLayerName = "Ground";

		flatArea.transform.parent = FlatPurchasedAreaContainer.transform;
		flatArea.name = Go.name;
		Addedflats.Add (flatArea);
		Addflat (x, y, flatArea.transform);

		Invoke ("DisableWalls", 0.5f);


	}


	//	public void CreateRoomOnStart (GameObject Go, string wall_texture, string ground_texture)
	//	{
	//
	//		GameObject flatArea = (GameObject)Instantiate (RoomTypePrefeb [0], Go.transform.position, Quaternion.identity);
	//
	//		flatArea.transform.parent = GameObject.Find ("FlatPurchasedAreaContainer").transform;
	//		flatArea.name = selectedBanner.x + "-" + selectedBanner.y;
	//		Addedflats.Add (flatArea);
	//		Addflat (selectedBanner.x, selectedBanner.y, flatArea.transform);
	//
	//		Destroy (Go);
	//
	//		WallsColor color = null;
	//		GroundTexture ground = null;
	//
	//		foreach (var obj in ReModelShopController.Instance.WallsColors) {
	//			if (obj.Name == wall_texture)
	//				color = obj;
	//		}
	//
	//
	//		foreach (var obj in ReModelShopController.Instance.Grounds) {
	//			if (obj.Name == ground_texture)
	//				ground = obj;
	//		}
	//
	//
	////		List<Sprite> Temp = new List<Sprite> (); 
	////
	////		if (color != null) {
	////			foreach (var texture in color.Textures) {
	////				Temp.Add (texture);
	////			}
	////		}
	//			
	//		if (color != null)
	//			flatArea.GetComponent <Flat3D> ().Walls.ChangeWallColors (color.Textures);
	//
	//		// TODO this is only temporary.. this is to be done  based on selected flat......
	//		if (ground != null)
	//			flatArea.GetComponent <Flat3D> ().ChangeGroungColor (ground.Texture);
	//
	//		SaleBanners.Remove (selectedBanner);
	//		Destroy (selectedBanner.gameObject);
	//	}

	public void RemoveRoom (int room)
	{
		room++;
		for (int i = 0; i < FlatPurchasedAreaContainer.transform.childCount; i++) {
			GameObject.Destroy (FlatPurchasedAreaContainer.transform.GetChild (room).gameObject);
		}
	}

	void CkeckFlatPurchaseAvailable ()
	{
//		if (GameManager.Instance.level == 0) {
//			ShowBannerofPurchase ();
//		}
	}



	public void DisableWalls ()
	{
		for (int i = 0; i < flats.Count; i++) {

			if (FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x + 1).ToString () + "-" + (flats [i].y).ToString ())
			    && !FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x + 1).ToString () + "-" + (flats [i].y).ToString ()).GetComponent <ConstructionTimer> ()) {
				if (FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y).ToString ()).GetChild (0).name.Contains ("Walls")) {
					FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y).ToString ()).GetChild (0).GetChild (2).gameObject.SetActive (false);
					FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y).ToString ()).GetChild (0).GetChild (3).gameObject.SetActive (false);
				}
			}
			if (FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y + 1).ToString ())
			    && !FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y + 1).ToString ()).GetComponent <ConstructionTimer> ()) {
				print (FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y).ToString ()).gameObject.name);
				if (FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y).ToString ()).GetChild (0).name.Contains ("Walls")) {
					FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y).ToString ()).GetChild (0).GetChild (4).gameObject.SetActive (false);
					FlatPurchasedAreaContainer.transform.FindChild ((flats [i].x).ToString () + "-" + (flats [i].y).ToString ()).GetChild (0).GetChild (5).gameObject.SetActive (false);
				}
			}
		}
	}

	//	public void StartFlatConstruction (int ConstructionAreaNo)
	//	{
	//		GameObject flateArea = (GameObject)Instantiate (RoomTypePrefeb [0], PurchaseAreaPositon [ConstructionAreaNo].localPosition, Quaternion.identity)as GameObject;
	//		flateArea.name = "ConstructionAreaNo ";
	//	}

	public void Addflat (int x, int y, Transform t)
	{
		FlatData flat = new FlatData ();
		flat.x = x;
		flat.y = y;
		flat.AreaName = "" + x + "-" + y;
		flat.Purchased = true;
		flat.TransformDetails = new Dictionary<string, float> ();
		flat.TransformDetails.Add ("PositionX", t.position.x);
		flat.TransformDetails.Add ("PositionY", t.position.y);
		flat.TransformDetails.Add ("PositionZ", t.position.z);
		flat.TransformDetails.Add ("RotationX", t.eulerAngles.x);
		flat.TransformDetails.Add ("RotationY", t.eulerAngles.y);
		flat.TransformDetails.Add ("RotationZ", t.eulerAngles.z);
		flat.TransformDetails.Add ("ScaleX", t.localScale.x);
		flat.TransformDetails.Add ("ScaleY", t.localScale.y);
		flat.TransformDetails.Add ("ScaleZ", t.localScale.z);
		flat.AreaPrice = 0;
		flats.Add (flat);
		AddPurchasedLandNextToRoom (flat.x, flat.y, t);
//		ShowBannerofPurchase (flat.x, flat.y, t); TODO Add Purchase Land adjacent to flat....
		for (int i = 0; i < flats.Count; i++) {
			flats [i].AreaPrice = flats [0].AreaPrice + pricebuffer;
		
		}
		CheckForExtraTimers ();

		List<GameObject> tempPland = new List<GameObject> ();
		for (int i = 0; i < purchaseLands.Count; i++) {
			for (int j = 0; j < FlatPurchasedAreaContainer.transform.childCount; j++) {
				if (purchaseLands [i] != null && purchaseLands [i].GetComponent <PurchasedLand> ()
				    && purchaseLands [i].transform.position.x == FlatPurchasedAreaContainer.transform.GetChild (j).position.x && purchaseLands [i].transform.position.y == FlatPurchasedAreaContainer.transform.GetChild (j).position.y) {
					PurchasedLands.Remove (purchaseLands [i].GetComponent <PurchasedLand> ());
					Destroy (purchaseLands [i]);
				}
			}


//			for (int j = i + 1; j < purchaseLands.Count; j++) {
//				if (purchaseLands [i] != null && purchaseLands [j] != null) {
//					if (purchaseLands [i].transform.position.x == purchaseLands [j].transform.position.x && purchaseLands [i].transform.position.y == purchaseLands [j].transform.position.y)
//						Destroy (purchaseLands [j]);
//					else
//						tempPland.Add (purchaseLands [i]);
//				}
//			}
		}
		purchaseLands = tempPland;


	}

	public void AddPurchasedLandNextToRoom (int x, int y, Transform t)
	{	
		int _x = x;
		int _y = y;


		if (PurchasedLands.Find (obj => (obj.x == x + 1 && obj.y == y)) == null) {
			//TODO: Create a banner over the area at this place;
//			Vector2 position = new Vector2 (t.position.x + 7.28f, t.position.y + 4.23f);
            Vector2 position = new Vector2 (t.position.x + 11.7f, t.position.y + 6.8f);
			CreateLand (++_x, _y, position);
		}	
		_x = x;
		_y = y;

		if (PurchasedLands.Find (obj => (obj.x == x - 1 && obj.y == y)) == null) {
			//			//TODO: Create a banner over the area at this place;
            Vector2 position = new Vector2 (t.position.x - 11.7f, t.position.y - 6.8f);
			CreateLand (--_x, _y, position);


		}	
		_x = x;
		_y = y;

		if (PurchasedLands.Find (obj => (obj.x == x && obj.y == y + 1)) == null) {
			//TODO: Create a banner over the area at this place;
            Vector2 position = new Vector2 (t.position.x - 11.7f, t.position.y + 6.8f);
			CreateLand (_x, ++_y, position);

		}	

		_x = x;
		_y = y;


		if (PurchasedLands.Find (obj => (obj.x == x && obj.y == y - 1)) == null) {
			//TODO: Create a banner over the area at this place;
            Vector2 position = new Vector2 (t.position.x + 11.7f, t.position.y - 6.8f);
			CreateLand (_x, --_y, position);                                
		}	
	}

	void CreateLand (int x, int y, Vector2 position)
	{

		GameObject Go = Instantiate (RoomPurchaseManager.Instance.purchaseLandPrefab, transform.position, Quaternion.identity) as GameObject;
		RoomPurchaseManager.Instance.purchaseLands.Add (Go);

		print (x + " - " + y);
		Go.GetComponent <PurchasedLand> ().x = x;
		Go.GetComponent <PurchasedLand> ().y = y;
		Go.transform.position = position;
		Go.transform.parent = ConstructionArea.transform;

		PurchasedLands.Add (Go.GetComponent <PurchasedLand> ());
	}





	//
	//	public void ShowBannerofPurchase (int x, int y, Transform t)
	//	{
	//
	//
	//		int _x = x;
	//		int _y = y;
	//
	//
	//		if (SaleBanners.Find (obj => (obj.x == x + 1 && obj.y == y)) == null) {
	//			//TODO: Create a banner over the area at this place;
	//			Vector2 position = new Vector2 (t.position.x + 7.27f, t.position.y + 4.08f);
	//			ShowBanner (++_x, _y, position);
	//		}
	//		_x = x;
	//		_y = y;
	//
	//		if (SaleBanners.Find (obj => (obj.x == x - 1 && obj.y == y)) == null) {
	////			//TODO: Create a banner over the area at this place;
	//			Vector2 position = new Vector2 (t.position.x - 7.27f, t.position.y - 4.08f);
	//			ShowBanner (--_x, _y, position);
	//
	//
	//		}	
	//		_x = x;
	//		_y = y;
	//
	//		if (SaleBanners.Find (obj => (obj.x == x && obj.y == y + 1)) == null) {
	//			//TODO: Create a banner over the area at this place;
	//			Vector2 position = new Vector2 (t.position.x - 7.27f, t.position.y + 4.08f);
	//			ShowBanner (_x, ++_y, position);
	//
	//		}	
	//
	//		_x = x;
	//		_y = y;
	//
	//
	//		if (SaleBanners.Find (obj => (obj.x == x && obj.y == y - 1)) == null) {
	//			//TODO: Create a banner over the area at this place;
	//			Vector2 position = new Vector2 (t.position.x + 7.27f, t.position.y - 4.08f);
	//			ShowBanner (_x, --_y, position);
	//		}	
	//	}



	//	public void ShowBannerofPurchase (SaleBanner banner)
	//	{		
	//
	//		int _x = banner.x;
	//		int _y = banner.y;
	//
	//
	//		if (SaleBanners.Find (obj => (obj.x == banner.x + 1 && obj.y == banner.y)) == null) {
	//			//TODO: Create a banner over the area at this place;
	//			Vector2 position = new Vector2 (banner.gameObject.transform.position.x + 7.27f, banner.gameObject.transform.position.y + 4.08f);
	//			ShowBanner (++_x, _y, position);
	//		}	
	//		_x = banner.x;
	//		_y = banner.y;
	//
	//		if (SaleBanners.Find (obj => (obj.x == banner.x - 1 && obj.y == banner.y)) == null) {
	//			//			//TODO: Create a banner over the area at this place;
	//			Vector2 position = new Vector2 (banner.gameObject.transform.position.x - 7.27f, banner.gameObject.transform.position.y - 4.08f);
	//			ShowBanner (--_x, _y, position);
	//
	//
	//		}	
	//		_x = banner.x;
	//		_y = banner.y;
	//
	//		if (SaleBanners.Find (obj => (obj.x == banner.x && obj.y == banner.y + 1)) == null) {
	//			//TODO: Create a banner over the area at this place;
	//
	//
	//			Vector2 position = new Vector2 (banner.gameObject.transform.position.x - 7.27f, banner.gameObject.transform.position.y + 4.08f);
	//			ShowBanner (_x, ++_y, position);
	//
	//		}	
	//
	//		_x = banner.x;
	//		_y = banner.y;
	//
	//
	//		if (SaleBanners.Find (obj => (obj.x == banner.x + 1 && obj.y == banner.y - 1)) == null) {
	//			//TODO: Create a banner over the area at this place;
	//			Vector2 position = new Vector2 (banner.gameObject.transform.position.x + 7.27f, banner.gameObject.transform.position.y - 4.08f);
	//			ShowBanner (_x, --_y, position);
	//		}	
	//	}

	//	bool isfirst = true;

	//	void ShowBanner (int x, int y, Vector2 position)
	//	{
	//		print (x + " - " + y);
	//		var obj = (GameObject)Instantiate (SaleBanner, Vector2.zero, Quaternion.identity);
	//		obj.GetComponent <SaleBanner> ().x = x;
	//		obj.GetComponent <SaleBanner> ().y = y;
	//		obj.transform.position = position;
	//		if (PlayerPrefs.GetInt ("Tutorial_Progress") > 18) {
	//			obj.GetComponent<SaleBanner> ().isAceptingMouse = true;
	////			isfirst = false;
	//		} else {
	//			obj.GetComponent<SaleBanner> ().isAceptingMouse = false;
	//		}
	//
	//		obj.transform.parent = ConstructionArea.transform;
	//
	//		SaleBanners.Add (obj.GetComponent <SaleBanner> ());
	//		//		obj.transform.eulerAngles = new Vector3 (0, 0, 45f);
	//	}




	public void CheckForExtraTimers ()
	{
		for (int i = 0; i < FlatPurchasedAreaContainer.transform.childCount; i++) {
			if (FlatPurchasedAreaContainer.transform.GetChild (i).GetComponent <ConstructionTimer> () && !FlatPurchasedAreaContainer.transform.GetChild (i).GetComponent <ConstructionTimer> ()._isTimerRunnig) {
				Destroy (FlatPurchasedAreaContainer.transform.GetChild (i).gameObject);
			}
		}

	}





	#region room generation

	public GameObject _messageText;
	public bool _selectionEnabled;
	public FlatData _selectedFlat;

	public void ShowMessage ()
	{
		ScreenManager.Instance.ClosePopup ();
		ScreenAndPopupCall.Instance.CloseScreen ();
		_messageText.SetActive (true);
		_selectionEnabled = true;
	}



	public void RemoveMessage ()
	{
		_messageText.SetActive (false);
		_selectionEnabled = false;
	}


	public void PurchaseWhenClick (PurchasedLand Land)
	{
		RemoveMessage ();

//		if (RoomPurchaseManager.Instance.selectedBanner && RoomPurchaseManager.Instance.selectedBanner.isPurchased) {
		var updatedata = new FlatUpdateData ();
		updatedata.player_id = PlayerPrefs.GetInt ("PlayerId");
		updatedata.position = Land.x.ToString () + "-" + Land.y.ToString ();
		updatedata.wall_texture = "Default";
		updatedata.ground_texture = "Default";
		
		foreach (var item in DownloadContent.Instance.downloaded_items) {
			if (item.Category == "Expansion" && item.SubCategory == "Rooms") {
				updatedata.item_id = item.Item_id;
			}
		}
		StartCoroutine (Purchase (updatedata, Land));
//		}
	}


	IEnumerator Purchase (FlatUpdateData updatedata, PurchasedLand Land)
	{
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.UpdateFlat (updatedata));
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			ScreenAndPopupCall.Instance.CloseScreen ();
			ScreenManager.Instance.ClosePopup ();
			CreateConstructionFlat (Land);
			GameManager.Instance.GetComponent <Tutorial> ().LandPurchasing ();

		
//			CheckForDuplicateBanners ();
		}
	}



	//	public void CheckForDuplicateBanners ()
	//	{
	//		var temp = new List<SaleBanner> ();
	//
	//		foreach (var obj in SaleBanners) {
	//			if (FlatPurchasedAreaContainer.transform.FindChild (obj.x.ToString () + "-" + obj.y.ToString ())) {
	//				Destroy (obj.gameObject);
	//			} else
	//				temp.Add (obj);
	//		}
	//
	//		SaleBanners = temp;
	//	}

	#endregion

	public static Vector3 DeserializeVector3Array (string aData)
	{
		Vector3 result = new Vector3 (0, 0, 0);

		string[] values = aData.Split (' ');
		if (values.Length != 3)
			throw new System.FormatException ("component count mismatch. Expected 3 components but got " + values.Length);
		result = new Vector3 (float.Parse (values [0]), float.Parse (values [1]), float.Parse (values [2]));
		return result;
	}
}


[Serializable]
public class AreaFlatData
{
	[Header ("Info of the Area")]
	public string AreaName;
	public float AreaPrice;
	public bool Purchased;
	[Header ("TransformDetails")]
	public Dictionary<string, float> TransformDetails;
	public int x;
	public int y;

	public AreaFlatData (string areaName, float areaPrice, bool purchased, Dictionary<string, float> transformPos, int xPos, int yPos)
	{
		AreaName = areaName;
		AreaPrice = areaPrice;
		Purchased = purchased;
		TransformDetails = transformPos;
		x = xPos;
		y = yPos;
	}
}

[Serializable]
public class AreaFlatProductData
{
	[Header ("Name of the Product")]
	public string Name;
	[Header ("TransformDetails")]
	public Dictionary<string, string> TransformDetails;

	public AreaFlatProductData (string aName, Dictionary<string, string> aTransformPos)
	{
		Name = aName;
		TransformDetails = aTransformPos;
	}
}
