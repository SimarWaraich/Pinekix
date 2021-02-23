using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Simple_JSON;

public class SocietyCreationController : MonoBehaviour
{
	public string NameString;
	public string DescriptionString;
	public List<string> TagsArray;
	public int EmblemIndex;
	public int RoomIndex;

	public Button NextButton;
	public Button DoneButton;
	public Button BackButton;

	public Image StepImage;

	public InputField NameField;
	public InputField DescriptionField;
	public InputField TagsSearchField;

	public List<string> NamesOfAllSocieties;

	[Header ("CreateSocietyScreens")]
	public GameObject NameSubmitScreen;
	public GameObject DescriptionSubmitScreen;
	public GameObject TagsSubmitScreen;
	public GameObject EmblemSelectionScreen;
	public GameObject RoomSelectionScreen;

	[Header ("CreateSociety Containers")]
	public GameObject TagSuggestionContainer;
	public GameObject TagsContainer;
	public GameObject EmblemListContainer;
	public GameObject SelectedEmblem;
	public GameObject RoomListContainer;


	[Header ("CreateSociety Prefabs")]
	public GameObject TagsPrefab;
	public GameObject FlatPrefab;

	void Start ()
	{
      
	}

	public void IntializeCreateSociety ()
	{
		StartCoroutine (GetMySocietyForCheck ("total"));
		NameString = "";
		DescriptionString = "";
		TagsArray = new List<string> ();
		EmblemIndex = -1;
		RoomIndex = -1;
		NextButton.interactable = false;
		BackButton.interactable = false;
		NextButton.gameObject.SetActive (true);
		DoneButton.gameObject.SetActive (false);

		for (int i = 0; i < StepImage.transform.childCount; i++) {
			StepImage.transform.GetChild (i).GetComponent<Text> ().color = BlueColor;
		}

		NameScreen ();
	}

	public Color WhiteColor;
	public Color BlueColor;

	void NameScreen ()
	{
		BackButton.gameObject.SetActive (false);
		if (NameString == "" || string.IsNullOrEmpty (NameString))
			NextButton.interactable = false;
		else
			NextButton.interactable = true;
		
		NameField.text = NameString;

		StepImage.fillAmount = 0.0305f;
		StepImage.transform.GetChild (0).GetComponent<Text> ().color = WhiteColor;
		NameSubmitScreen.SetActive (true);
		DescriptionSubmitScreen.SetActive (false);
		TagsSubmitScreen.SetActive (false);
		EmblemSelectionScreen.SetActive (false);
		RoomSelectionScreen.SetActive (false);

		NextButton.onClick.RemoveAllListeners ();
		BackButton.onClick.RemoveAllListeners ();

		NextButton.onClick.AddListener (() => {   
			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (!Tut._SocietyCreated && Tut.societyTutorial == 6)
				Tut.SocietyTutorial ();
			DescriptionScreen ();
		});
//		BackButton.onClick.AddListener (()=>
//			{
//				ScreenAndPopupCall.Instance.ShowSocietyList ();
//			});
	}

	public void OnSubmitName (InputField inputField)
	{
		if (CheckIfNameIsUnique (inputField.text)) {
			if (inputField.text.Length <= 60) {
				if (inputField.text != "" && !Regex.IsMatch (inputField.text, "^[ \t\r\n\u0200]*$")) {
					NameString = inputField.text;
					NextButton.interactable = true;

					var Tut = GameManager.Instance.GetComponent<Tutorial> ();
					if (!Tut._SocietyCreated && Tut.societyTutorial == 5)
						Tut.SocietyTutorial ();

				} else {
					NextButton.interactable = false;
					BackButton.interactable = false;
				}
			} else {
				NextButton.interactable = false;
				ShowPopUp ("Maximum 60 characters allowed", null);
			}
		} else {
			NextButton.interactable = false;
			ShowPopUp ("This name already exists. Please choose another name.", null);
		}
	}

	void DescriptionScreen ()
	{
		NameSubmitScreen.SetActive (false);
		DescriptionSubmitScreen.SetActive (true);	
		TagsSubmitScreen.SetActive (false);
		EmblemSelectionScreen.SetActive (false);
		RoomSelectionScreen.SetActive (false);

		if (DescriptionString == "" || string.IsNullOrEmpty (DescriptionString))
			NextButton.interactable = false;
		else
			NextButton.interactable = true;
		
		DescriptionField.text = DescriptionString;
		BackButton.interactable = true;
		BackButton.gameObject.SetActive (true);
		StepImage.fillAmount = 0.2722f;
		StepImage.transform.GetChild (1).GetComponent<Text> ().color = WhiteColor;
		NextButton.onClick.RemoveAllListeners ();
		BackButton.onClick.RemoveAllListeners ();
		NextButton.onClick.AddListener (() => {
			TagsScreen ();

			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (!Tut._SocietyCreated && Tut.societyTutorial == 8)
				Tut.SocietyTutorial ();
		});

		BackButton.onClick.AddListener (() => {
			NameScreen ();
		});
	}

	public void OnSubmitDescription (InputField inputField)
	{
		if (inputField.text.Length <= 150) {
			if (inputField.text != "" && !Regex.IsMatch (inputField.text, "^[ \t\r\n\u0200]*$")) {
				DescriptionString = inputField.text;
				NextButton.interactable = true;

				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (!Tut._SocietyCreated && Tut.societyTutorial == 7)
					Tut.SocietyTutorial ();
			} else {
				NextButton.interactable = false;
//			BackButton.interactable = false;
			}
			BackButton.interactable = true;
		} else {
			ShowPopUp ("Maximum 150 characters are allowed", null);
		} 
	}

	void TagsScreen ()
	{
		NameSubmitScreen.SetActive (false);
		DescriptionSubmitScreen.SetActive (false);
		TagsSubmitScreen.SetActive (true);
		EmblemSelectionScreen.SetActive (false);
		RoomSelectionScreen.SetActive (false);


		NextButton.interactable = false;
		BackButton.interactable = true;
		StepImage.fillAmount = 0.5145f;
		StepImage.transform.GetChild (2).GetComponent<Text> ().color = WhiteColor;

		for (int i = 0; i < TagsContainer.transform.childCount; i++) {
			GameObject.Destroy (TagsContainer.transform.GetChild (i).gameObject);
		}

		if (TagsArray.Count < 5)
			TagsSubmitScreen.transform.FindChild ("Open Tag List").gameObject.SetActive (true);
		else
			TagsSubmitScreen.transform.FindChild ("Open Tag List").gameObject.SetActive (false);
		
		TagsArray.ForEach (tag => {
			AddTagsToList (tag, false, null);          
		});
		NextButton.onClick.RemoveAllListeners ();
		BackButton.onClick.RemoveAllListeners ();
		NextButton.onClick.AddListener (() => { 
			EmblemScreen ();
			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (!Tut._SocietyCreated && Tut.societyTutorial == 11)
				Tut.SocietyTutorial ();
		});

		BackButton.onClick.AddListener (() => {
			DescriptionScreen ();
		});
	}

	public void OpentTagSuggestionList ()
	{
		transform.FindChild ("Society TagList").gameObject.SetActive (true);	
		TagsSearchField.text = ""; 

		for (int i = 0; i < TagSuggestionContainer.transform.childCount; i++) {
			GameObject.Destroy (TagSuggestionContainer.transform.GetChild (i).gameObject);
		}

		SocietyManager.Instance.AllTagsList.ForEach (tag => {
			if (isNotAlreadyAdded (tag)) {
				GameObject Go = GameObject.Instantiate (TagsPrefab, TagSuggestionContainer.transform)as GameObject;
				Go.transform.localPosition = Vector3.zero;
				Go.transform.localScale = Vector3.one;
				Go.transform.GetComponent <SocietyTag> ().Tag = tag;
				Go.transform.GetComponent <SocietyTag> ().isSuggestion = true;
			}
		});
	}

	bool isNotAlreadyAdded (string tag)
	{
		foreach (var _tag in TagsArray) {
			if (_tag == tag)
				return false;
		}
		return true;
	}

	public void AddTagsToList (string _tag, bool checkTags, GameObject TagGO = null)
	{
		if (isNotAlreadyAdded (_tag) || !checkTags) {
			GameObject Go = GameObject.Instantiate (TagsPrefab, TagsContainer.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.transform.GetComponent <SocietyTag> ().Tag = _tag;
			Go.transform.GetComponent <SocietyTag> ().isSuggestion = false;
			if (checkTags)
				TagsArray.Add (_tag);
			if (TagGO)
				Destroy (TagGO);

			TagsSearchField.text = ""; 
		}

		if (TagsArray.Count < 5) {
			NextButton.interactable = false;
			transform.FindChild ("Society TagList").gameObject.SetActive (false);
			TagsSubmitScreen.transform.FindChild ("Open Tag List").gameObject.SetActive (true);
		} else {
			NextButton.interactable = true;
			transform.FindChild ("Society TagList").gameObject.SetActive (false);
			TagsSubmitScreen.transform.FindChild ("Open Tag List").gameObject.SetActive (false);      
		}
	}

	public void OnSearchTags (InputField inputField)
	{
		var SearchTransform = transform.FindChild ("Society TagList").GetComponentInChildren <GridLayoutGroup> ().transform;
		if (inputField.text != "" && !Regex.IsMatch (inputField.text, "^[ \t\r\n\u0200]*$")) {
			
			for (int i = 0; i < SearchTransform.childCount; i++) {
				if (SearchTransform.GetChild (i).name.ToLower ().Contains (inputField.text.ToLower ())) {
					SearchTransform.GetChild (i).gameObject.SetActive (true);
				} else if (string.IsNullOrEmpty (inputField.text.ToString ())) {
					SearchTransform.GetChild (i).gameObject.SetActive (true);
				} else {
					SearchTransform.GetChild (i).gameObject.SetActive (false);
				}
			}
		} else {
			for (int i = 0; i < SearchTransform.childCount; i++) {
				SearchTransform.GetChild (i).gameObject.SetActive (true);
			}
		}
	}

	public void OnSubmitTags (InputField inputField)
	{
		if (inputField.text != "" && !Regex.IsMatch (inputField.text, "^[ \t\r\n\u0200]*$")) {
			if (TagsArray.Count < 5) {
				AddTagsToList (inputField.text, true, null);
				transform.FindChild ("Society TagList").gameObject.SetActive (false);
				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (!Tut._SocietyCreated && TagsArray.Count < 4) {
					Tut.societyTutorial = 8;
					Tut.SocietyTutorial ();
				}
			} else
				ShowPopUp ("You can not add more than 5 tags", null);
			
		}
	}

	void EmblemScreen ()
	{
		NameSubmitScreen.SetActive (false);
		DescriptionSubmitScreen.SetActive (false);
		TagsSubmitScreen.SetActive (false);
		EmblemSelectionScreen.SetActive (true);
		RoomSelectionScreen.SetActive (false);

		if (EmblemIndex >= 0) {
			NextButton.interactable = true;
			EmblemSelectionScreen.transform.FindChild ("Open Emblem").gameObject.SetActive (false);
			EmblemSelectionScreen.transform.FindChild ("Society Emblem").gameObject.SetActive (true);                
		} else {
			NextButton.interactable = false;
			EmblemSelectionScreen.transform.FindChild ("Open Emblem").gameObject.SetActive (true);
			EmblemSelectionScreen.transform.FindChild ("Society Emblem").gameObject.SetActive (false);
		}

		NextButton.gameObject.SetActive (true);
		DoneButton.gameObject.SetActive (false);

		BackButton.interactable = true;

		StepImage.fillAmount = 0.758f;
		StepImage.transform.GetChild (3).GetComponent<Text> ().color = WhiteColor;
		NextButton.onClick.RemoveAllListeners ();
		BackButton.onClick.RemoveAllListeners ();
		NextButton.onClick.AddListener (() => { 
			RoomSelectScreen ();
			var Tut = GameManager.Instance.GetComponent<Tutorial> ();
			if (!Tut._SocietyCreated && Tut.societyTutorial == 14)
				Tut.SocietyTutorial ();
		});
		
		BackButton.onClick.AddListener (() => {
			TagsScreen ();
		});
	}

	public void OpenEmblemList ()
	{
		transform.FindChild ("Society Emblem Selection").gameObject.SetActive (true);

		for (int i = 0; i < EmblemListContainer.transform.childCount; i++) {
			GameObject.Destroy (EmblemListContainer.transform.GetChild (i).gameObject);
		}

		for (int i = 0; i < SocietyManager.Instance.EmblemList.Count; i++) {
			GameObject Go = new GameObject ();
			Go.transform.SetParent (EmblemListContainer.transform);
			Go.AddComponent <Button> ().onClick.AddListener (() => SelectEmblem (Go));
			Go.AddComponent <Image> ().sprite = SocietyManager.Instance.EmblemList [i];
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;		
			Go.name = i.ToString ();
		}
	}

	public void SelectEmblem (GameObject Go)
	{
		EmblemIndex = int.Parse (Go.name);
		EmblemSelectionScreen.transform.FindChild ("Open Emblem").gameObject.SetActive (false);
		EmblemSelectionScreen.transform.FindChild ("Society Emblem").gameObject.SetActive (true);
		EmblemSelectionScreen.transform.FindChild ("Society Emblem").GetComponent <Image> ().sprite = Go.GetComponent <Image> ().sprite;
		Go.GetComponent <Button> ().onClick.RemoveAllListeners ();
		transform.FindChild ("Society Emblem Selection").gameObject.SetActive (false);
		NextButton.interactable = true;

		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut._SocietyCreated && Tut.societyTutorial == 13)
			Tut.SocietyTutorial ();
	}

	void RoomSelectScreen ()
	{
		NameSubmitScreen.SetActive (false);
		DescriptionSubmitScreen.SetActive (false);
		TagsSubmitScreen.SetActive (false);
		EmblemSelectionScreen.SetActive (false);
		RoomSelectionScreen.SetActive (true);

		NextButton.gameObject.SetActive (true);
		DoneButton.gameObject.SetActive (false);

		if (RoomIndex < 0) {
			NextButton.interactable = false;
			NextButton.gameObject.SetActive (false);
			DoneButton.gameObject.SetActive (false);
			RoomSelectionScreen.transform.FindChild ("Open And Select Room").gameObject.SetActive (true);
			RoomSelectionScreen.transform.FindChild ("Show Society Room").gameObject.SetActive (false);
			RoomSelectionScreen.transform.FindChild ("Create Emblem").gameObject.SetActive (false);
		} else {			
			NextButton.gameObject.SetActive (false);
			DoneButton.gameObject.SetActive (true);
			RoomSelectionScreen.transform.FindChild ("Open And Select Room").gameObject.SetActive (false);
			RoomSelectionScreen.transform.FindChild ("Show Society Room").gameObject.SetActive (true);
			RoomSelectionScreen.transform.FindChild ("Create Emblem").gameObject.SetActive (true);
		}
		
		BackButton.interactable = true;
		StepImage.fillAmount = 1f;
		StepImage.transform.GetChild (4).GetComponent<Text> ().color = WhiteColor;
//        StepImage.transform.GetChild(5).GetComponent<Text>().color = BlueColor;		
		NextButton.onClick.RemoveAllListeners ();
		BackButton.onClick.RemoveAllListeners ();
		NextButton.onClick.AddListener (() => { 
//				SumbitMyScoietyToServer();
		});

		BackButton.onClick.AddListener (() => {
			EmblemScreen ();
		});
	}

	public void CloseRoomSelection ()
	{
		transform.FindChild ("Society Room Selection").gameObject.SetActive (false);
		if (RoomIndex < 0)
			RoomSelectionScreen.transform.FindChild ("Show Society Room").gameObject.SetActive (false);
		else
			RoomSelectionScreen.transform.FindChild ("Show Society Room").gameObject.SetActive (true);
	}

	public void OpenRoomSelectionList ()
	{
		RoomSelectionScreen.transform.FindChild ("Show Society Room").gameObject.SetActive (false);
		transform.FindChild ("Society Room Selection").gameObject.SetActive (true);
		transform.FindChild ("Society Room Selection").GetComponentInChildren <ScrollRect> ().horizontalNormalizedPosition = 0f;
		for (int i = 0; i < RoomListContainer.transform.childCount; i++) {
			GameObject.Destroy (RoomListContainer.transform.GetChild (i).gameObject);
		}

		for (int i = 0; i < SocietyManager.Instance.RoomPrefabsList.Count; i++) {
			GameObject Go = GameObject.Instantiate (FlatPrefab, RoomListContainer.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.GetComponentInChildren<Text> ().text = "Room " + (i + 1);
			Go.name = i.ToString ();
			Go.GetComponent <Button> ().onClick.AddListener (() => SelectRoom (Go));

			GameObject FlatRepresent = Instantiate (SocietyManager.Instance.RoomPrefabsList [i], Vector3.zero, Quaternion.identity)as GameObject;
			FlatRepresent.gameObject.SetActive (true);
			FlatRepresent.transform.parent = Go.transform;
			FlatRepresent.transform.localPosition = Vector3.zero;
			FlatRepresent.transform.localScale = new Vector2 (12, 12);
			FlatRepresent.transform.rotation = Quaternion.identity;
			FlatRepresent.transform.localEulerAngles = new Vector3 (0, 0, 45f);
			FlatRepresent.transform.FindChild ("grid Container").gameObject.SetActive (false);

			int Layer = LayerMask.NameToLayer ("UI");
			FlatRepresent.SetLayerRecursively (Layer);
			if (FlatRepresent.GetComponent <ResizeGrid> ())
				Destroy (FlatRepresent.GetComponent <ResizeGrid> ());
			if (FlatRepresent.GetComponent <Flat3D> ())
				Destroy (FlatRepresent.GetComponent <Flat3D> ());
			if (FlatRepresent.GetComponent <PolygonCollider2D> ())
				Destroy (FlatRepresent.GetComponent <PolygonCollider2D> ());
		}
		transform.FindChild ("Society Room Selection").GetComponentInChildren <ScrollRect> ().transform.FindChild ("Back").GetComponent <Button> ().interactable = false;
		transform.FindChild ("Society Room Selection").GetComponentInChildren <ScrollRect> ().transform.FindChild ("Next").GetComponent <Button> ().interactable = true;
	}

	public void OnClickNextButton (ScrollRect _scrollRect)
	{
		_scrollRect.transform.FindChild ("Next").GetComponent <Button> ().interactable = false;

		var totalObj = _scrollRect.content.transform.childCount;
		float stepSize = 1 / (float)(totalObj - 1);     
		var lastPos = _scrollRect.horizontalNormalizedPosition;
        
		if (_scrollRect.horizontalNormalizedPosition.AlmostEquals (1f, 0.01f)) {
//            _scrollRect.horizontalNormalizedPosition = 0f;
			StartCoroutine (MoveTo (_scrollRect, 0f, 0.5f));
		} else {
			//            _scrollRect.horizontalNormalizedPosition += stepSize;
			lastPos += stepSize;
			StartCoroutine (MoveTo (_scrollRect, lastPos, 0.5f));
		}
	}

	public void OnClickPreviousButton (ScrollRect _scrollRect)
	{
		_scrollRect.transform.FindChild ("Back").GetComponent <Button> ().interactable = false;
		var totalObj = _scrollRect.transform.GetChild (0).GetChild (0).childCount;
		float stepSize = 1 / (float)(totalObj - 1);
		var lastPos = _scrollRect.horizontalNormalizedPosition;
//        if (_scrollRect.horizontalNormalizedPosition.AlmostEquals(0f, 0.01f))
//        {
////            _scrollRect.horizontalNormalizedPosition = 1f;
//        }
//        else
//        {
//            _scrollRect.horizontalNormalizedPosition -= stepSize;
		lastPos -= stepSize;
		StartCoroutine (MoveTo (_scrollRect, lastPos, 0.5f));
//        }
        
//		_scrollRect.horizontalNormalizedPosition 
//		_scrollRect.horizontalNormalizedPosition = iTween.FloatUpdate (_scrollRect.horizontalNormalizedPosition, stepSize, 0.01f);
		


	}

	IEnumerator MoveTo (ScrollRect mScrollRect, float position, float time)
	{
		float start = mScrollRect.horizontalNormalizedPosition;
		float end = position;
		float t = 0;

		while (t < 1) {
			yield return null;
			t += Time.deltaTime / time;
			mScrollRect.horizontalNormalizedPosition = Mathf.Lerp (start, end, t);
		}
		mScrollRect.horizontalNormalizedPosition = end;

		if (mScrollRect.horizontalNormalizedPosition.AlmostEquals (1f, 0.01f)) {
			mScrollRect.transform.FindChild ("Next").GetComponent <Button> ().interactable = false;
			mScrollRect.transform.FindChild ("Back").GetComponent <Button> ().interactable = true;
		} else if (mScrollRect.horizontalNormalizedPosition.AlmostEquals (0f, 0.01f)) {
			mScrollRect.transform.FindChild ("Next").GetComponent <Button> ().interactable = true;
			mScrollRect.transform.FindChild ("Back").GetComponent <Button> ().interactable = false;
		} else {
			mScrollRect.transform.FindChild ("Next").GetComponent <Button> ().interactable = true;
			mScrollRect.transform.FindChild ("Back").GetComponent <Button> ().interactable = true;
		}
	}

	public void SelectRoom (GameObject Go)
	{
		RoomIndex = int.Parse (Go.name);
		RoomSelectionScreen.transform.FindChild ("Open And Select Room").gameObject.SetActive (false);
		RoomSelectionScreen.transform.FindChild ("Create Emblem").gameObject.SetActive (true);
		RoomSelectionScreen.transform.FindChild ("Show Society Room").gameObject.SetActive (true);

		if (RoomSelectionScreen.transform.FindChild ("Show Society Room").childCount != 0) {
			Destroy (RoomSelectionScreen.transform.FindChild ("Show Society Room").GetChild (0).gameObject);
		}
		Go.transform.parent = RoomSelectionScreen.transform.FindChild ("Show Society Room");
		Go.GetComponent <Button> ().onClick.RemoveAllListeners ();
		transform.FindChild ("Society Room Selection").gameObject.SetActive (false);
		NextButton.gameObject.SetActive (false);
		DoneButton.gameObject.SetActive (true);

		DoneButton.onClick.RemoveAllListeners ();               
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
        
		DoneButton.onClick.AddListener (() => {     
			if (!Tut._SocietyCreated && Tut.societyTutorial == 17) {
				Tut.SocietyTutorial ();

				SocietyManager.Society sc = new SocietyManager.Society (NameString, 0, DescriptionString, EmblemIndex, RoomIndex, TagsArray);

				SocietyManager.Instance.SelectedSociety = SocietyManager.Instance._mySociety = sc;
				SocietyManager.Instance.myRole = 0;
				var societyDescriptionController = ScreenManager.Instance.Admin_MemberDiscriptionPanel.GetComponent <SocietyDescriptionController> ();
				ScreenAndPopupCall.Instance.CloseScreen ();
				societyDescriptionController.OpenSocietyDiscription (0, SocietyManager.Instance._mySociety);
				IntializeCreateSociety ();
			} else {
				DoneButton.interactable = false;
				SumbitMyScoietyToServer ();
			}
		});

		if (!Tut._SocietyCreated && Tut.societyTutorial == 16)
			Tut.SocietyTutorial ();
	}

	void SumbitMyScoietyToServer ()
	{
		
		NextButton.interactable = false;
		BackButton.interactable = false;
		if (PlayerPrefs.GetInt ("Money") >= 1000) {			
			StartCoroutine (CreateSociety ());
		} else {
			ShowPopUp ("You need 1000 coins to create a new society", () => {
				DoneButton.interactable = true;
				BackButton.interactable = true;
			});
		}		
	}

	IEnumerator CreateSociety ()
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new JSONClass ();

		jsonElement ["data_type"] = "create";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		jsonElement ["society_name"] = NameString;
		jsonElement ["description"] = DescriptionString;
		jsonElement ["emblem_index"] = (EmblemIndex + 1).ToString ();
		jsonElement ["room_index"] = (RoomIndex + 1).ToString ();
		jsonElement ["level_number"] = PlayerPrefs.GetInt ("Level").ToString ();
		var jsonarray = new JSONArray ();
		foreach (var item in TagsArray) {
			var jsonItem = new JSONClass ();		
			jsonItem ["title"] = item.ToString ();
			jsonarray.Add (jsonItem);
		}
		jsonElement ["tags"] = jsonarray;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/societies/society", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;
		ScreenAndPopupCall.Instance.LoadingScreenClose ();

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {//|| _jsnode ["status"].ToString ().Contains ("200"))
				//TODO get my society and show its description page.
				GameManager.Instance.SubtractCoins (1000);
				ShowPopUp ("Your society has been created successfully", () => {
					StartCoroutine (GetMySocietyForCheck ("mine"));
					IntializeCreateSociety ();
				});				

			} else if (_jsnode ["description"].ToString ().Contains ("You already has created a society")) {
				ShowPopUp ("You are already Created/Joined a society", () => {
					IntializeCreateSociety ();
					ScreenAndPopupCall.Instance.CloseScreen ();
				});	
			} else {
				ShowPopUp ("Some unknown problem occured", () => {
					IntializeCreateSociety ();
				});
			}
		}
		DoneButton.interactable = true;
		BackButton.interactable = true;
	}

	public void ShowPopUp (string message, UnityEngine.Events.UnityAction OnClickActions)
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


	IEnumerator GetMySocietyForCheck (string SearchType)
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "search";
		jsonElement ["search_type"] = SearchType;
		jsonElement ["keyword"] = "";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/societies/society", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {

				JSONNode data = _jsnode ["data"];
				for (int i = 0; i < data.Count; i++) {
					
					if (SearchType.Contains ("mine")) {
						
						string name = data [i] ["society_name"];
						string description = data [i] ["society_description"];
						int emblem = 0;
						int.TryParse (data [i] ["emblem_index"], out emblem);
						int room = 0;
						int.TryParse (data [i] ["room_index"], out room);
						int _id = 0;
						int.TryParse (data [i] ["society_id"], out _id);

						JSONNode tags = data [i] ["tags"];
						var listofTags = new List<string> ();

						for (int x = 0; x < tags.Count; x++) {
							listofTags.Add (tags [x] ["tag_title"]);
						}

						SocietyManager.Society sc = new SocietyManager.Society (name, _id, description, emblem - 1, room - 1, listofTags);

						SocietyManager.Instance.SelectedSociety = SocietyManager.Instance._mySociety = sc;
						SocietyManager.Instance.myRole = 0;
						var societyDescriptionController = ScreenManager.Instance.Admin_MemberDiscriptionPanel.GetComponent <SocietyDescriptionController> ();
						ScreenAndPopupCall.Instance.CloseScreen ();
						societyDescriptionController.OpenSocietyDiscription (0, SocietyManager.Instance._mySociety);
					} else if (SearchType.Contains ("total")) {
						string name = data [i] ["society_name"];
						NamesOfAllSocieties.Add (name);
					}
				}
			}
		}
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	bool CheckIfNameIsUnique (string Name)
	{
		foreach (var name in NamesOfAllSocieties) {
			if (Name.ToLower () == name.ToLower ())
				return false;
		}
		return true;
	}
}
