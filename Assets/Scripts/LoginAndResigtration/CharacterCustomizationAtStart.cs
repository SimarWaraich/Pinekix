using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;



public enum ButtonType
{
	simple = 0,
	color = 1,
	category = 2,
	onlycolor = 3,
}

public class CharacterCustomizationAtStart : Singleton<CharacterCustomizationAtStart>
{

	public bool _deleteAllPlayerprefs;
	public GameObject secondaryCanvas;
	public Camera secondaryCamera;

	public Button currentSelectedButton;

	public GameObject ConfirmationPopup;
	public GameObject ParentelConfirmction;
	public GameObject ParentleRegistrationScreen;
	public GameObject OtherBG;
	//Character Highlight
	public GameObject MaleCharacter, FemaleCharacter, CustomButtonPrefab, CustomColorButtonPrefab, MainPanel, ColorButtonPanel, SelectedCharacter;


	public string SelectedColor;
	public string SelectedCategory;

	public Dictionary<string, CustomCharacterSaving> EditedCharDict = new Dictionary<string, CustomCharacterSaving> ();

//	public CustomShoes[] CustomShoes;
//    public MyCustomCharacter[] MyChars;
//
    CustomisationAllData AllCustomData;


	void Awake ()
	{
		Reload ();
		if (_deleteAllPlayerprefs)
			PlayerPrefs.DeleteAll ();
//        DontDestroyOnLoad (this);
		AllCustomData = Resources.Load<CustomisationAllData> ("CustomisationAllData");
	}


	public void EnableSection ()
	{
		EditedCharDict.Clear ();
		CameraEnebleDesable (true);
		if (!PlayerPrefs.HasKey ("CharacterType")) {
			PlayerPrefs.SetString ("CharacterType", "Male");
			OnMaleSelection ();
		} else {
			if (PlayerPrefs.GetString ("CharacterType") == "Male") {
				OnMaleSelection ();
			} else {
				OnFemaleSelection ();
			}
		}

		PlayerPrefs.SetInt ("SkinToneColor", 1);
		SelectedColor = SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType;
		SelectedCategory = "SkinTone";
		_GenerateColorButtons ();
//		_GenerateButtons ();
	}

	public void OnMaleSelection ()
	{
		EditedCharDict.Clear ();
		PlayerPrefs.SetString ("CharacterType", "Male");
//			FemaleCharacter.SetActive (false);
		iTween.MoveTo (FemaleCharacter, new Vector2 (10, -3), 0.9f);// iTween.Hash ("position", new Vector2(20,-3), "time", 0.8f, "easeType", "easeInOutCubic"));
		iTween.MoveTo (MaleCharacter, new Vector2 (20.6f, -3), 0.8f);

//			MaleCharacter.SetActive (true);
		DeleteOldButtons ();
		SelectedCategory = "SkinTone";
		ZoomCameraToCategories ();
		_GenerateColorButtons ();
		SelectedCharacter = MaleCharacter;
		
	}

	public void RotateCharacter ()
	{
		if (PlayerPrefs.GetString ("CharacterType") == "Male") {
			MaleCharacter.GetComponent<CharacterProperties> ().ChangeFacingFront ();
		} else if (PlayerPrefs.GetString ("CharacterType") == "Female") {
			FemaleCharacter.GetComponent<CharacterProperties> ().ChangeFacingFront ();
		}
        
	}

	public void OnFemaleSelection ()
	{
		EditedCharDict.Clear ();
		PlayerPrefs.SetString ("CharacterType", "Female");
//			FemaleCharacter.SetActive (true);
//			MaleCharacter.SetActive (false);

		iTween.MoveTo (FemaleCharacter, new Vector2 (20.6f, -3), 0.8f);// iTween.Hash ("position", new Vector2(20,-3), "time", 0.8f, "easeType", "easeInOutCubic"));
		iTween.MoveTo (MaleCharacter, new Vector2 (30, -3), 0.9f); //iTween.Hash ("position", new Vector2(30,-3), "time", 0.8f, "easeType", "easeInOutCubic"));
		DeleteOldButtons ();
/*            SelectedColor = "default";
*/     
		SelectedCategory = "SkinTone";
		ZoomCameraToCategories ();
		_GenerateColorButtons ();           
		SelectedCharacter = FemaleCharacter;
	}

	public void CameraEnebleDesable (bool enable)
	{		
		secondaryCanvas.SetActive (enable);
		secondaryCamera.enabled = enable;
	}

	public void GenerateShoesButton ()
	{

		DeleteOldButtons ();
		if (string.IsNullOrEmpty (SelectedColor))
			SelectedColor = "default";
		if (string.IsNullOrEmpty (SelectedCategory))
			SelectedCategory = "SkinTone";
		
		foreach (var obj in AllCustomData.CustomShoes) {
			if (obj._gender == PlayerPrefs.GetString ("CharacterType")) {
				var index = obj.AllColors.FindIndex (colorobj => colorobj.name == SelectedColor);

				if (index < 0)
					index = 0;
                
				string CharType = CharacterCustomizationAtStart.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType;
				ItemBodyParts[] BodyImages = new ItemBodyParts[0];
				if (CharType == "Brown")
					BodyImages = obj.AllColors [index].BrownShoes.ToArray ();
				else if (CharType == "White")
					BodyImages = obj.AllColors [index].WhiteShoes.ToArray ();
				else if (CharType == "Black")
					BodyImages = obj.AllColors [index].BlackShoes.ToArray ();
                    
				for (int i = 0; i < BodyImages.Length; i++) {
					var go = (GameObject)Instantiate (CustomButtonPrefab, Vector3.zero, Quaternion.identity);
					go.transform.SetParent (MainPanel.transform);
					go.transform.localScale = Vector3.one;

					go.transform/*.GetChild(0)*/.GetComponent<Image> ().sprite = BodyImages [i].icon;
					var Component = go.AddComponent<CustomShoesButton> ();        
					Component.BodyParts = BodyImages [i].images;
					Component.Index = i;
					Component.PartName = obj.PartName;
				}
			}
		}
	}

	public void DeleteOldButtons ()
	{
		for (int i = 0; i < MainPanel.transform.childCount; i++) {
			Destroy (MainPanel.transform.GetChild (i).gameObject);
		}       
	}

	public void GenerateShoesColor ()
	{
		for (int i = 0; i < ColorButtonPanel.transform.childCount; i++) {
			Destroy (ColorButtonPanel.transform.GetChild (i).gameObject);
		}

		foreach (var obj in AllCustomData.CustomShoes) {
			if (obj.type == ButtonType.color && obj._gender == PlayerPrefs.GetString ("CharacterType")) {

				foreach (var color in obj.AllColors) {
					var go = (GameObject)Instantiate (CustomColorButtonPrefab, Vector3.zero, Quaternion.identity);
					go.transform.SetParent (ColorButtonPanel.transform);
					go.transform.localScale = Vector3.one;

					go.GetComponent<Image> ().color = color.icon;
					var NewColor = new ItemColors ();
					var CharType = CharacterCustomizationAtStart.Instance.SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType;
					if (CharType == "Brown")
						NewColor.AllItems = color.BrownShoes;
					else if (CharType == "White")
						NewColor.AllItems = color.WhiteShoes;
					else if (CharType == "Black")
						NewColor.AllItems = color.BlackShoes;
					NewColor.icon = color.icon;
					NewColor.name = color.name;
                           
					var Component = go.AddComponent<CustomColorButton> ();
					Component.ThisColor = NewColor;
					Component.PartName = obj.PartName;

//                    go.GetComponent<CharacterCustomButton>().Mytype = ButtonType.color;
				}
			}
		}
	}

    public IEnumerator CharacterUpdate ()
    {
        PlayerManager.Instance.MainCharacter = SelectedCharacter;
        switch (SelectedCharacter.GetComponent<CharacterProperties>().PlayerType)
        {
            case "White":
                PlayerPrefs.SetInt("SkinToneColor", 0);
                break;
            case "Brown":
                PlayerPrefs.SetInt("SkinToneColor", 1);
                break;
            case "Black":
                PlayerPrefs.SetInt("SkinToneColor", 2);
                break;
            default:
                PlayerPrefs.SetInt("SkinToneColor", 0);
                break;
        }


        CoroutineWithData cd = new CoroutineWithData (this, SendCharacterData ());
        yield return cd.coroutine;

        if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
            Camera.main.GetComponent<PinekixRegistration> ().LoadingAssetsPanelOnCharConfrim ();
        }

        PlayerManager.Instance.MainCharacter.transform.position = new Vector3 (0.5f, 3f, -1);    
        PlayerManager.Instance.MainCharacter.transform.eulerAngles = new Vector3 (0, 0, 0);
        PlayerManager.Instance. MainCharacter.transform.localScale = Vector3.one * 0.5f;
        yield return PlayerManager.Instance.StartCoroutine (PlayerManager.Instance.GetCharacterCustomisations ());

        SelectedCharacter.transform.parent = null;
        DontDestroyOnLoad (SelectedCharacter);
        Camera.main.gameObject.GetComponent <PinekixRegistration> ().LodingScreen.SetActive (true);
        Camera.main.gameObject.GetComponent <PinekixRegistration> ().Character_CustomizationSelectionPanel.SetActive (false);
//        SceneManager.LoadScene ("GamePlay");
        StartCoroutine(LoadLevelAsynchronously());
    }

    string GetCustomizationPerCategory(string Category)
    {
        if(EditedCharDict.ContainsKey(Category))
        {
            var cs = EditedCharDict[Category];
            return string.Format("{0}_{1}", cs.Color, cs.IndexofItem);
        }
        if(Category == "SkinTone" || Category =="Ears" || Category == "Nose")
            return string.Format("{0}_{1}", SelectedCharacter.GetComponent<CharacterProperties>().PlayerType, 0);

        return "default_0";
    }

    public IEnumerator SendCharacterData ()
    {
        var encoding = new System.Text.UTF8Encoding ();

        Dictionary<string,string> postHeader = new Dictionary<string,string> ();

        //      customchar.player_id = PlayerPrefs.GetInt ("PlayerId");
        //      customchar.gender = (int)GameManager.GetGender ();
        var json = new Simple_JSON.JSONClass();

        json ["player_id"]= PlayerPrefs.GetInt ("PlayerId").ToString();
        json["gender"] = ((int)GameManager.GetGender()).ToString();


        json ["skin_tone"]= PlayerPrefs.GetInt("SkinToneColor").ToString(); 
        json ["eyes"]= GetCustomizationPerCategory("Eyes");
        json ["nose"]=GetCustomizationPerCategory("Nose");
        json ["lips"]= GetCustomizationPerCategory("Lips");
        json ["ears"]= GetCustomizationPerCategory("Ears");
        json ["hair"]= GetCustomizationPerCategory("Hairs");
        json ["shoes"]= GetCustomizationPerCategory("Shoes");
        json ["clothing"]= "";
        //        {
        //            "player_id": "1",
        //            "gender": "1",
        //            "skin_tone": "1",
        //            "eyes": "1",
        //            "nose": "1",
        //            "lips": "1",
        //            "ears": "1",
        //            "hair": "1",
        //            "shoes": "1",
        //            "clothing": "1"
        //        }


        postHeader.Add ("Content-Type", "application/json");
        postHeader.Add ("Content-Length", json.Count.ToString ());

        WWW www = new WWW ("http://pinekix.ignivastaging.com/players/insertcustomcharacter", encoding.GetBytes (json.ToString()), postHeader);

        print ("jsonDtat is ==>> " + json); 
        yield return www;

        if (www.error == null) {
            Simple_JSON.JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
            print ("www.text ==>> " + www.text);
            print ("_jsnode ==>> " + _jsnode.ToString ());
            if (_jsnode ["description"].ToString ().Contains ("inserted") || _jsnode ["status"].ToString ().Contains ("200")) {
                print ("Success");
                yield return true;
            } else {
                print ("error" + www.error);
                yield return false;
            }
        } else {
            yield return false;
        }
    }

	public void ConfirmedCharacterSelectionButton ()
	{
		StartCoroutine (CharacterUpdate ());
		PlayerPrefs.SetInt ("CharacterRegistered", 1);
//		Child_Object = secondaryCamera.transform.GetChild (0).gameObject;

	}

	//	GameObject Child_Object;

	//	public void ConfirmedCharacterSelectionButtonl ()
	//	{

	//		var SecondryCam = GameObject.Find ("SecondCamera");
	//		secondaryCamera = SecondryCam;
	//		if (PlayerPrefs.GetString ("CharacterType") == "Male") {
	//
	//
	//			switch (PlayerPrefs.GetInt ("SkinToneColor")) {
	//			case 0:
	//				MaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "White";
	//				break;
	//			case 1:
	//				MaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Brown";
	//				break;
	//			case 2:
	//			default:
	//				MaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Black";
	//				break;
	//
	//			}
	//		} else {
	//
	//			Child_Object = secondaryCamera.transform.GetChild (1).gameObject;
	//			switch (PlayerPrefs.GetInt ("SkinToneColor")) {
	//			case 0:
	//				FemaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "White";
	//				break;
	//			case 1:
	//				FemaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Brown";
	//				break;
	//			case 2:
	//			default:
	//				FemaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Black";
	//				break;
	//			}
	//		}

     

	//	}



	public IEnumerator DirectConfirm ()
	{//		var SecondryCam = GameObject.Find ("SecondCamera");
//		secondaryCamera = SecondryCam;
		yield return PlayerManager.Instance.StartCoroutine (PlayerManager.Instance.GetCharacterCustomisations ());

//        if (GameManager.GetGender () == GenderEnum.Male || PlayerPrefs.GetString ("CharacterType") == "Male") {
//            PlayerManager.Instance.MainCharacter = CharacterCustomizationAtStart.Instance.MaleCharacter;
//        } else {
//            PlayerManager.Instance.MainCharacter = CharacterCustomizationAtStart.Instance.FemaleCharacter;
//        }   

		PlayerManager.Instance.MainCharacter.transform.position = new Vector3 (0.5f, 3f, -1);    
		PlayerManager.Instance.MainCharacter.transform.eulerAngles = new Vector3 (0, 0, 0);
		PlayerManager.Instance.MainCharacter.transform.localScale = Vector3.one * 0.5f;


		if (PlayerPrefs.GetString ("CharacterType") == "Male") {
			SelectedCharacter = MaleCharacter;
			switch (PlayerPrefs.GetInt ("SkinToneColor")) {
			case 0:
				MaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "White";
				break;
			case 1:
				MaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Brown";	
				break;
			case 2:
			default:
				MaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Black";
				break;

			}
		} else {

			SelectedCharacter = FemaleCharacter;
			switch (PlayerPrefs.GetInt ("SkinToneColor")) {
			case 0:
				FemaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "White";
				break;
			case 1:
				FemaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Brown";	
				break;
			case 2:
			default:
				FemaleCharacter.GetComponent<CharacterProperties> ().PlayerType = "Black";
				break;
			}
		}

		SelectedCharacter.transform.parent = null;
		DontDestroyOnLoad (SelectedCharacter);
//		DontDestroyOnLoad (SecondryCam);
//		SceneManager.LoadScene ("GamePlay");
        StartCoroutine(LoadLevelAsynchronously());
	}

    IEnumerator LoadLevelAsynchronously()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("GamePlay");
        if (Application.isLoadingLevel) 
        {    
            Debug.Log ("Loading complete");
            yield return async;
        }
    }


	void ZoomCameraOnViewingParts (Vector3 playerPosition, float cameraZoomInOutSize)
	{
//		if (myDropdown.value == 0) {
//			SecondaryCamera_Player.GetComponent<Camera> ().orthographicSize = cameraZoomInOutSize;
//			MaleMainGame_Player.transform.localPosition = playerPosition;
//		} else {
//			SecondaryCamera_Player.GetComponent<Camera> ().orthographicSize = cameraZoomInOutSize;
//			FemaleMainGame_Player.transform.localPosition = playerPosition;
//		}
	}

	public void ShowConfirmationPopUp (bool isActive)
	{
		if (isActive) {
			iTween.ScaleTo (ConfirmationPopup, iTween.Hash ("scale", Vector3.one, "time", 0.1f, "easeType", "easeInCirc"));

//			iTween.ScaleTo (ConfirmationPopup, new Vector3 (1, 1, 1), 0.1f);
		} else {
			iTween.ScaleTo (ConfirmationPopup, iTween.Hash ("scale", Vector3.zero, "time", 0.1f, "easeType", "easeInCirc"));

//			iTween.ScaleTo (ConfirmationPopup, new Vector3 (0, 0, 0), 0.1f);
		}
	}

	public void ShowConfirmationPopUpParentel (bool isActive)
	{		
		if (isActive) {
			iTween.ScaleTo (ParentelConfirmction, iTween.Hash ("scale", Vector3.one, "time", 0.5f, "easeType", "easeInCirc"));
			OtherBG.SetActive (true);
			//			iTween.ScaleTo (ConfirmationPopup, new Vector3 (1, 1, 1), 0.1f);
		} else {
			iTween.ScaleTo (ParentelConfirmction, iTween.Hash ("scale", Vector3.zero, "time", 0.5f, "easeType", "easeInCirc"));
			OtherBG.SetActive (false);
			//			iTween.ScaleTo (ConfirmationPopup, new Vector3 (0, 0, 0), 0.1f);
		}

	}

	public void ShowConfirmationPopUpParentelScreen (bool isActive)
	{		
		if (isActive) {
//			iTween.ScaleTo (ParentleRegistrationScreen, iTween.Hash ("scale", Vector3.one, "time", 0.1f, "easeType", "easeInCirc"));
//			ParentleRegistrationScreen.SetActive (isActive);
			iTween.ScaleTo (ParentleRegistrationScreen, iTween.Hash ("scale", Vector3.one, "time", 0.5f, "easeType", "easeInCirc"));
			//			iTween.ScaleTo (ConfirmationPopup, new Vector3 (1, 1, 1), 0.1f);
		} else {
//			iTween.ScaleTo (ParentleRegistrationScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.1f, "easeType", "easeInCirc"));
//			ParentleRegistrationScreen.SetActive (isActive);
			iTween.ScaleTo (ParentleRegistrationScreen, iTween.Hash ("scale", Vector3.zero, "time", 0.5f, "easeType", "easeInCirc"));
			//			iTween.ScaleTo (ConfirmationPopup, new Vector3 (0, 0, 0), 0.1f);
			OtherBG.SetActive (false);
			Camera.main.gameObject.GetComponent <PinekixRegistration> ().LoadingAssetsPanel ();
		}

	}

	public void ShowParentelScreenClose ()
	{
		ParentleRegistrationScreen.SetActive (false);
		OtherBG.SetActive (false);

	}

	///======================= my changes======//////////////
	/// 

	public void _GenerateColorButtons ()
	{

		for (int i = 0; i < ColorButtonPanel.transform.childCount; i++) {
			Destroy (ColorButtonPanel.transform.GetChild (i).gameObject);
		}

		foreach (var obj in AllCustomData.MyChars) {
			if ((obj.type == ButtonType.color || obj.type == ButtonType.onlycolor) && obj._category.Contains (SelectedCategory) && obj._gender == PlayerPrefs.GetString ("CharacterType")) {
				foreach (var _color in obj.AllColors) {
					var go = (GameObject)Instantiate (CustomColorButtonPrefab, Vector3.zero, Quaternion.identity);
					go.transform.SetParent (ColorButtonPanel.transform);
					go.transform.localScale = Vector3.one;
					go.name = _color.name;
//                    go.GetComponent<Image>().sprite = color.icon;
					go.AddComponent<CustomColorButton> ().ThisColor = _color;
					go.GetComponent<CustomColorButton> ().PartName = obj.PartName;
					go.GetComponent<CustomColorButton> ().iscoloronly = obj.type == ButtonType.onlycolor ? true : false;
				}
			}
		}
	}

	public void _GenerateButtons ()
	{

		DeleteOldButtons ();
		if (string.IsNullOrEmpty (SelectedCategory))
			SelectedCategory = "SkinTone";

		foreach (var obj in AllCustomData.MyChars) {
			if (obj._category.Contains (SelectedCategory) && obj._gender == PlayerPrefs.GetString ("CharacterType")) {  
				if (string.IsNullOrEmpty (SelectedColor))
					SelectedColor = obj.AllColors [0].name;
                
				var index = obj.AllColors.FindIndex (colorobj => colorobj.name == SelectedColor);

				if (index >= 0) {
//                    foreach(var item in obj.AllColors[index].AllItems){
					var _allItmes = obj.AllColors [index].AllItems;
					for (int i = 0; i < _allItmes.Count; i++) {
						var go = (GameObject)Instantiate (CustomButtonPrefab, Vector3.zero, Quaternion.identity);
						go.transform.SetParent (MainPanel.transform);
						go.transform.localScale = Vector3.one;

						go.transform/*.GetChild(0)*/.GetComponent<Image> ().sprite = _allItmes [i].icon;
						var Component = go.AddComponent<CharacterCustomButton> ();
						Component._characteritem = _allItmes [i];
						Component.PartName = obj.PartName;
						Component.Mytype = ButtonType.simple;
						Component.category = obj._category;
						Component.Index = i;
					}
				}
			}
		}
	}

	public void ChangeShoesOnClickSkinTone (string Color, int index)
	{ 
		var parts = SelectedCharacter.GetComponentsInChildren<BodyParts> (true);
		foreach (var _char in AllCustomData.CustomShoes) {          
			if (_char._gender == PlayerPrefs.GetString ("CharacterType")) {         
				int _index = _char.AllColors.FindIndex (colorobj => colorobj.name == Color);
				var PartName = _char.PartName;
				var CharType = SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType;
				ItemBodyParts item = new ItemBodyParts ();
				if (CharType == "Brown")
					item = _char.AllColors [_index].BrownShoes [index];
				else if (CharType == "White")
					item = _char.AllColors [_index].WhiteShoes [index];
				else if (CharType == "Black")
					item = _char.AllColors [_index].BlackShoes [index];                    

				foreach (var part in parts) {
					for (int i = 0; i < Mathf.Min (PartName.Count, item.images.Count); i++) {
						if (part.name == PartName [i])
							part.GetComponent<SpriteRenderer> ().sprite = item.images [i];
					}
				}
				return;
			}
		}
	}

	public void ZoomCameraToCategories ()
	{
		switch (SelectedCategory) {
		case "SkinTone":
			StartCoroutine (ZoomCameraToPos (secondaryCamera.transform.localPosition, new Vector3 (20f, 0f, -10f), secondaryCamera.orthographicSize, 4.5f, 0.5f));
			break;
		case "Eyes":
		case "Nose":
		case "Lips":
		case "Hairs":
		case "Ears":
			StartCoroutine (ZoomCameraToPos (secondaryCamera.transform.localPosition, new Vector3 (20f, 2f, -10f), secondaryCamera.orthographicSize, 2.7f, 0.5f));
			break;
		case "Shoes":
			StartCoroutine (ZoomCameraToPos (secondaryCamera.transform.localPosition, new Vector3 (20f, -2f, -10f), secondaryCamera.orthographicSize, 2.7f, 0.5f));                
			break;    

		default:
                //                secondryCamera.orthographicSize = iTween.FloatUpdate(5.5f, 2f, 0.01f);
			break;
		}       
	}

	IEnumerator ZoomCameraToPos (Vector3 startPos, Vector3 endPos, float startZoom, float endZoom, float time)
	{ 
		float i = 1f;
		StopCoroutine ("ZoomCameraToPos");
//        yield return new WaitForSeconds(0.01f);

		i = 0.0f;
		float rate = 1.0f / time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			secondaryCamera.transform.localPosition = Vector3.Lerp (startPos, endPos, i);
			secondaryCamera.orthographicSize = Mathf.Lerp (startZoom, endZoom, i);
			yield return null;
		}
	}

	public void OnClickRandomButton ()
	{
		var playerManger = PlayerManager.Instance;
		var bodyParts = SelectedCharacter.GetComponentsInChildren<BodyParts> (true);
		string Skin = "";
		UnityEngine.Random.InitState ((int)System.DateTime.Now.Ticks);
		switch (UnityEngine.Random.Range (0, 3)) {
		case 0:
			Skin = "White";
			break;
		case 1:
			Skin = "Brown";
			break;
		case 2:
			Skin = "Black";
			break;
		}
		SelectedCharacter.GetComponent<CharacterProperties> ().PlayerType = Skin;

		ChangeCharacterRandomlyCategoryWise (bodyParts, "SkinTone", Skin);
		ChangeCharacterRandomlyCategoryWise (bodyParts, "Eyes");
		ChangeCharacterRandomlyCategoryWise (bodyParts, "Nose", Skin);
		ChangeCharacterRandomlyCategoryWise (bodyParts, "Lips");
		ChangeCharacterRandomlyCategoryWise (bodyParts, "Ears", Skin);
		ChangeCharacterRandomlyCategoryWise (bodyParts, "Hairs");
		ChangeShoesRandomly (Skin);

		StartCoroutine (ZoomCameraToPos (secondaryCamera.transform.localPosition, new Vector3 (20f, 0f, -10f), secondaryCamera.orthographicSize, 4.5f, 0.5f));
	}

	void ChangeCharacterRandomlyCategoryWise (BodyParts[] parts, string Category, string Color = null)
	{
		var cs = new CustomCharacterSaving ();

		var AllCustomData = Resources.Load<CustomisationAllData> ("CustomisationAllData");
		foreach (var _char in AllCustomData.MyChars) {          
			if (_char._category.Contains (Category) && _char._gender == PlayerPrefs.GetString ("CharacterType")) { 
				int RandomColor = 0;
				if (Color != null)
					RandomColor = _char.AllColors.FindIndex (colorobj => colorobj.name == Color);
				else
					RandomColor = UnityEngine.Random.Range (0, _char.AllColors.Count);
                
				int RandomIndex = UnityEngine.Random.Range (0, _char.AllColors [RandomColor].AllItems.Count);

				var PartName = _char.PartName;
				var item = _char.AllColors [RandomColor].AllItems [RandomIndex];

				cs.IndexofItem = RandomIndex;
				cs.Color = _char.AllColors [RandomColor].name;

				foreach (var part in parts) {
					for (int i = 0; i < Mathf.Min (PartName.Count, item.images.Count); i++) {
						if (part.name == PartName [i])
							part.GetComponent<SpriteRenderer> ().sprite = item.images [i];
					}
				}
			}
		}
		if (EditedCharDict.ContainsKey (Category))
			EditedCharDict [Category] = cs;
		else
			EditedCharDict.Add (Category, cs);
	}

	public void ChangeShoesRandomly (string SkinColor)
	{ 
		var parts = SelectedCharacter.GetComponentsInChildren<BodyParts> (true);
		foreach (var _char in AllCustomData.CustomShoes) {          
			if (_char._gender == PlayerPrefs.GetString ("CharacterType")) {  
				int _index = UnityEngine.Random.Range (0, _char.AllColors.Count);
				var PartName = _char.PartName;
				ItemBodyParts item = new ItemBodyParts ();
				int index = UnityEngine.Random.Range (0, _char.AllColors [_index].BrownShoes.Count);
				if (SkinColor == "Brown")
					item = _char.AllColors [_index].BrownShoes [index];
				else if (SkinColor == "White")
					item = _char.AllColors [_index].WhiteShoes [index];
				else if (SkinColor == "Black")
					item = _char.AllColors [_index].BlackShoes [index];


				foreach (var part in parts) {
					for (int i = 0; i < Mathf.Min (PartName.Count, item.images.Count); i++) {
						if (part.name == PartName [i])
							part.GetComponent<SpriteRenderer> ().sprite = item.images [i];
					}
				}
				return;
			}
		}
	}
}

[Serializable]
public class CustomShoes
{
	public string name;
	public string _gender;
	public ButtonType type = ButtonType.color;
	public List<string> PartName = new List<string> ();
	public List<ShoesColors> AllColors = new List<ShoesColors> ();
}

[Serializable]
public class ShoesColors
{
	public string name;
	public Color icon;
	public List<ItemBodyParts> BrownShoes = new List<ItemBodyParts> ();
	public List<ItemBodyParts> BlackShoes = new List<ItemBodyParts> ();
	public List<ItemBodyParts> WhiteShoes = new List<ItemBodyParts> ();
}

[Serializable]
public class MyCustomCharacter
{
	public string name;
	public string _category;
	public string _gender;
	public ButtonType type;
	public List<string> PartName = new List<string> ();
	public List<ItemColors> AllColors = new List<ItemColors> ();
}

[Serializable]
public class ItemColors
{
	public string name;
	public Color icon;
	public List<ItemBodyParts> AllItems = new List<ItemBodyParts> ();
}

[Serializable]
public class ItemBodyParts
{
	public string Name;
	public Sprite icon;
	public List<Sprite> images = new List<Sprite> ();
}

public class CustomCharacterSaving
{
	public string Color;
	public int IndexofItem;
}