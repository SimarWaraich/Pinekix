using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class CharacterSelectionController : MonoBehaviour
{
	public GameObject Container;
	public GameObject Prefab;
	public Camera ThisCamera;


	public IEnumerator InitChar ()
	{
		yield return new WaitForSeconds (0.45f);
		InitializeCharacterSelection ();
	}

	public void InitializeCharacterSelection ()
	{
		DeleteAllChars ();
		GetList ();
	}

	void GetList ()
	{
		List<GameObject> AllCharacterInGame = new List<GameObject> ();

//		AllCharacterInGame.Add (PlayerManager.Instance.MainCharacter);

		foreach (var Char in RoommateManager.Instance.RoommatesHired) {
			AllCharacterInGame.Add (Char);
		}

		AddCharactersToScreen (AllCharacterInGame);
		ThisCamera.enabled = true;
	}

	void AddCharactersToScreen (List<GameObject> AllCharacterInGame)
	{
		AllCharacterInGame.ForEach (Char => {
			GameObject Go = Instantiate (Prefab.gameObject, Vector3.zero, Quaternion.identity)as GameObject;

			Go.transform.parent = Container.transform;
			Go.transform.localScale = Vector3.one;

			var Component = Go.GetComponent<WardrobeCharcterSelection> ();
			Component.selection = Char;
			Component.thisController = this;

			GameObject CharRep = Instantiate (Char, Vector3.zero, Quaternion.identity)as GameObject;
			CharRep.gameObject.SetActive (true);
			CharRep.transform.parent = Go.transform;
			CharRep.transform.localPosition = new Vector2 (0, -160);
			CharRep.transform.localScale = new Vector2 (40, 40);
			CharRep.transform.rotation = Quaternion.identity;
			CharRep.transform.localEulerAngles = new Vector3 (0, 0, 0);

			int Layer = LayerMask.NameToLayer ("UI3D");
			CharRep.SetLayerRecursively (Layer);

			if (CharRep.GetComponent <Flatmate> ()) {

				CharRep.GetComponent <Flatmate> ().data = Char.GetComponent <Flatmate> ().data;

				var _flatMate = CharRep.GetComponent <Flatmate> ();

				Component.Name = _flatMate.data.Name.Trim ('"');
				_flatMate.enabled = false;

				Component.IsOnCoolDown = false;
				var Tut = GameManager.Instance.GetComponent <Tutorial> ();
				if (Tut.enabled && !Tut._DressPurchased) {
					if (CharRep.GetComponent <Player> ())
						Component.IsActive = true;
					else
						Component.IsActive = false;
				} else if (Tut.enabled && !Tut._SaloonPurchased) {
					if (CharRep.GetComponent <Player> ())
						Component.IsActive = true;
					else
						Component.IsActive = false;
				} else if (ScreenManager.Instance.OpenedCustomizationScreen == "SocietyEventDressUp" || ScreenManager.Instance.OpenedCustomizationScreen == "FashionEventDressUp" || ScreenManager.Instance.OpenedCustomizationScreen == "CatWalkEventDressUp" || ScreenManager.Instance.OpenedCustomizationScreen == "CoOpEvent") {
					if (ScreenManager.Instance.OpenedCustomizationScreen == "CatWalkEventDressUp" && EventManagment.Instance.SelectedRoommates.Contains (Char)) {
						Component.IsActive = false;
					} else {
						if (_flatMate.data.IsBusy) {
							Component.IsActive = false;
							Component.IsOnCoolDown = false;
							Component.IsBusy = true;
							Component.CoolDownTimer = _flatMate.data.busyTimeRemaining;//(float) (_flatMate.data.BusyTimeRemaining -  System.DateTime.UtcNow).TotalSeconds;

						} else if (_flatMate.data.IsCoolingDown) {
							Component.IsActive = false;
							Component.IsOnCoolDown = true;
							Component.CoolDownTimer = (float)(_flatMate.data.CooldownEndTime - System.DateTime.UtcNow).TotalSeconds;
						} else {
							Component.IsActive = true;
							Component.IsOnCoolDown = false;
						}
					}
				} else {
					if (_flatMate.data.IsBusy) {
						Component.IsActive = false;		
						Component.IsBusy = true;
						Component.CoolDownTimer = _flatMate.data.busyTimeRemaining;//(float) (_flatMate.data.BusyTimeRemaining -  System.DateTime.UtcNow).TotalSeconds;
					} else {
						Component.IsActive = true;		
						Component.IsBusy = false;
					}
				}

				CharRep.GetComponent <GenerateMoney> ().enabled = false;
				if (CharRep.GetComponent<RoomMateMovement> ())
					Destroy (CharRep.GetComponent<RoomMateMovement> ());

				//				GameObject.Destroy (CharRep.transform.GetChild (12).gameObject);
				GameObject.Destroy (CharRep.transform.FindChild ("low money").gameObject);

			} 
			if (CharRep.GetComponent <Player> ()) {
				Component.Name = PlayerPrefs.GetString ("UserName");
			}
		});

		var _scrollRect = GetComponentInChildren<UnityEngine.UI.ScrollRect> ();
		_scrollRect.transform.FindChild ("Back").GetComponent <UnityEngine.UI.Button> ().interactable = false;
		_scrollRect.transform.FindChild ("Next").GetComponent <UnityEngine.UI.Button> ().interactable = _scrollRect.content.transform.childCount > 3;
	}
	//    [Range(0.0f, 1f)]
	//    public float offsetNext;
	//    [Range(0.0f, 1f)]
	//    public float offsetBack;
	public void DeleteAllChars ()
	{
		for (int i = 0; i < Container.transform.childCount; i++) {
			GameObject.Destroy (Container.transform.GetChild (i).gameObject);
		}
	}

	public void OnChangeValue ()
	{   
		var _scrollRect = GetComponentInChildren<ScrollRect> ();

		if (_scrollRect.content.transform.childCount > 3) {
			var totalObj = _scrollRect.content.transform.childCount;
			float stepSize = 1 / (float)(totalObj - 2);// -2 just because there are 3 characters in one screen
			_scrollRect.transform.FindChild ("Back").GetComponent <Button> ().interactable = _scrollRect.horizontalNormalizedPosition > stepSize;
			_scrollRect.transform.FindChild ("Next").GetComponent <Button> ().interactable = _scrollRect.horizontalNormalizedPosition < 1 - stepSize + 0.12f;
		}
	}

	//    public void OnClickNext()
	//    {
	//        var _scrollRect = GetComponentInChildren<ScrollRect>();
	//        var totalObj = _scrollRect.GetComponentInChildren<GridLayoutGroup>().transform.childCount;
	//        float stepSize = 1 / (float)(totalObj - 1);
	//        stepSize *= 2;
	////
	////        float lastvalue = _scrollRect.horizontalNormalizedPosition + stepSize;
	////        float t = 0;
	////
	////        while(_scrollRect.horizontalNormalizedPosition < lastvalue)
	////        {
	////            _scrollRect.horizontalNormalizedPosition =  Mathf.Lerp(_scrollRect.horizontalNormalizedPosition, lastvalue, t);
	////            t += 0.1f * Time.deltaTime;
	////        }
	////
	//        _scrollRect.horizontalNormalizedPosition += stepSize;
	//        _scrollRect.transform.FindChild ("Next").GetComponent <Button> ().interactable = _scrollRect.horizontalNormalizedPosition <= 1;
	//        _scrollRect.transform.FindChild ("Back").GetComponent <Button> ().interactable = _scrollRect.horizontalNormalizedPosition >= 0;
	//    }

	//    public void OnClickBack()
	//    {
	//        var _scrollRect = GetComponentInChildren<ScrollRect>();
	//        var totalObj = _scrollRect.GetComponentInChildren<GridLayoutGroup>().transform.childCount;
	//        float stepSize = 1 / (float)(totalObj - 1);
	//        stepSize *= 2;
	//        _scrollRect.horizontalNormalizedPosition -= stepSize;
	//        _scrollRect.transform.FindChild ("Next").GetComponent <Button> ().interactable = _scrollRect.horizontalNormalizedPosition <= 1;
	//        _scrollRect.transform.FindChild ("Back").GetComponent <Button> ().interactable = _scrollRect.horizontalNormalizedPosition >= 0;
	//    }

}

public static class ExtensionMethods
{

	public static Vector3 DeserializeVector3ArrayExtented (string aData)
	{
		Vector3 result = new Vector3 (0, 0, 0);

		string[] values = aData.Split (' ');
		if (values.Length != 3)
			throw new System.FormatException ("component count mismatch. Expected 3 components but got " + values.Length);
		result = new Vector3 (float.Parse (values [0]), float.Parse (values [1]), float.Parse (values [2]));
		return result;
	}

	public static void SetLayerRecursively (this GameObject obj, int newLayer)
	{
		if (null == obj) {
			return;
		}

		obj.layer = newLayer;

		foreach (Transform child in obj.transform) {
			if (null == child) {
				continue;
			}
			SetLayerRecursively (child.gameObject, newLayer);
		}
	}

	public static void SetMaterialRecursively (this GameObject obj)
	{
		if (null == obj) {
			return;
		}


		if (obj.GetComponent<SpriteRenderer> ()) {
			obj.GetComponent<SpriteRenderer> ().material = GameObject.Find ("Dummy").GetComponent<SpriteRenderer> ().material;
			if (!obj.name.Contains ("Check") || obj.name != "selectionmenu" || obj.transform.parent.name != "selectionmenu")
				obj.GetComponent<SpriteRenderer> ().sortingLayerName = GameObject.Find ("Dummy").GetComponent<SpriteRenderer> ().sortingLayerName;
			else if (!obj.name.Contains ("Check") && (obj.name == "selectionmenu" || obj.transform.parent.name == "selectionmenu"))
				obj.GetComponent<SpriteRenderer> ().sortingLayerName = GameObject.Find ("Dummy").transform.GetChild (0).GetComponent<SpriteRenderer> ().sortingLayerName;

			if (obj.name.Contains ("Check"))
				obj.SetActive (false);

			if (obj.transform.parent && (obj.transform.parent.name.Contains ("grid") || obj.transform.parent.name.Contains ("Grid")))
				obj.GetComponent<SpriteRenderer> ().enabled = false;
		}


		foreach (Transform child in obj.transform) {
			if (null == child) {
				continue;
			}
			SetMaterialRecursively (child.gameObject);
		}
	}




	//	public static Dictionary<string, GameObject> ChildsOfGameobject = new Dictionary<string, GameObject> ();

	public static Dictionary<string,GameObject> GetAllChilds (this GameObject transformForSearch)
	{
		Dictionary<string,GameObject> getedChilds = new Dictionary<string,GameObject> ();

		foreach (var trans in transformForSearch.GetComponentsInChildren<BodyParts>()) {
			//Debug.Log (trans.name);
//			GetAllChilds (trans.gameObject);
			getedChilds.Add (trans.gameObject.name, trans.gameObject);            
		}        
		return getedChilds;
	}


	//	public static void cleardictionary ()
	//	{
	//		ChildsOfGameobject.Clear ();
	//	}

	public static void SetOrderInLayerRecursively (this GameObject obj, int newLayer)
	{
		if (null == obj) {
			return;
		}

		if (obj.GetComponent<SpriteRenderer> ()) {
			obj.GetComponent<SpriteRenderer> ().sortingOrder = newLayer;
		}

		foreach (Transform child in obj.transform) {
			if (null == child) {
				continue;
			}
			Transform parent = null;
			int parentLayer = newLayer;

			if (parent && parent != child) {
				parentLayer = newLayer;
			} else {
				parentLayer--;
			}

			parent = child;
			//			Debug.Log ("Parent == " + child.parent);

			//			if (child.GetComponent<SpriteRenderer> ())
			SetOrderInLayerRecursively (child.gameObject, parentLayer);
		}	
	}

	public static void SetOrderInLayerRecursivelyIncreasing (this GameObject obj, int newLayer)
	{
		if (null == obj) {
			return;
		}

		if (obj.GetComponent<SpriteRenderer> ()) {
			obj.GetComponent<SpriteRenderer> ().sortingOrder = newLayer;
		}

		foreach (Transform child in obj.transform) {
			if (null == child) {
				continue;
			}
			Transform parent = null;
			int parentLayer = newLayer;

			if (parent && parent != child) {
				parentLayer = newLayer;
			} else {
				parentLayer++;

			}

			parent = child;
			//			Debug.Log ("Parent == " + child.parent);

			//			if (child.GetComponent<SpriteRenderer> ())
			SetOrderInLayerRecursivelyIncreasing (child.gameObject, parentLayer);
		}	
	}

	public static string GetTimeStringFromFloat (float time)
	{
		System.TimeSpan t = System.TimeSpan.FromSeconds (time);
		return string.Format ("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
	}

	public static string GetShortTimeStringFromFloat (float time)
	{
		System.TimeSpan t = System.TimeSpan.FromSeconds (time);
		return string.Format ("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
	}
}
