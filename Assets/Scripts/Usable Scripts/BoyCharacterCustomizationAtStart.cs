/// <summary>
/// Modified By ::==>> Rehan Manandhar... Dated 08 Aug 2k16
﻿/// <summary>
/// Modified By ::==>> Rehan Manandhar... Dated 08 Aug 2k16
/// This script will be used for Panel Customization
/// This script is used for Panel switching
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class BoyCharacterCustomizationAtStart : MonoBehaviour
{
	private GameObject CurrentActive_Panel;
	public GameObject Panel_Character_MaleFemaleSelection;
	public GameObject Panel_CharacterCustomization;
	public GameObject Panel_SkinToneSelection, Boy_PanelSkinToneSelection;
	public GameObject Panel_EyesSelection, Boy_PanelEyesSelection;
	public GameObject Panel_NoseSelection, Boy_PanelNoseSelection;
	public GameObject Panel_LipsSelection, Boy_PanelLipsSelection;
	public GameObject Panel_EarsSelection, Boy_PanelEarsSelection;
	public GameObject Panel_HairSelection, Boy_PanelHairSelection;
	public GameObject Panel_ShoesSelection, Boy_PanelShoseSelection;
	public GameObject Panel_ClothingSelection, Boy_PanelClothingSelection;
	public GameObject secondaryCamera;

	public GameObject SecondaryCamera_Player;
	public GameObject MainGame_Player;

	public Button currentSelectedButton;

	public Dropdown myDropdown;

	void Start ()
	{
		myDropdown.value = 0;
		////< HEAD
		currentSelectedButton.interactable = false;

		//		Panel_SkinToneSelection.SetActive (true);
		//		CurrentActive_Panel = Panel_SkinToneSelection;
		//===
		//		Panel_SkinToneSelection.SetActive (true);
		//		CurrentActive_Panel = Panel_SkinToneSelection;
		////> 3d28f842ae0c1547bf696fce654d71865f0adab1
	}

	public void SkinToneSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_SkinToneSelection.SetActive (true);
		CurrentActive_Panel = Panel_SkinToneSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, 2.5f, 10), 5.1f);
	}

	public void EyesSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_EyesSelection.SetActive (true);
		CurrentActive_Panel = Panel_EyesSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, -1, 10), 2);
	}

	public void NoseSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_NoseSelection.SetActive (true);
		CurrentActive_Panel = Panel_NoseSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, -1, 10), 2);
	}

	public void LipsSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_LipsSelection.SetActive (true);
		CurrentActive_Panel = Panel_LipsSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, -1, 10), 2);
	}

	public void EarsSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_EarsSelection.SetActive (true);
		CurrentActive_Panel = Panel_EarsSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, -1, 10), 2);
	}

	public void HairSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_HairSelection.SetActive (true);
		CurrentActive_Panel = Panel_HairSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, -1, 10), 2);
	}

	public void ShoesSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_ShoesSelection.SetActive (true);
		CurrentActive_Panel = Panel_ShoesSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, 6, 10), 2);
	}

	public void ClothingSelection_Panel ()
	{
		CurrentActive_Panel.SetActive (false);
		Panel_ClothingSelection.SetActive (true);
		CurrentActive_Panel = Panel_ClothingSelection;
		currentSelectedButton.interactable = true;
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedButton.interactable = false;
		ZoomCameraOnViewingParts (new Vector3 (0, 2.5f, 10), 5.1f);
	}

	public void SetDropdownIndex ()
	{
		print ("Value of Drop Down List is :---->>" + myDropdown.value);
		if (myDropdown.value == 0) {
			PlayerPrefs.SetString ("CharacterType", "Male");
			print ("Male Character Selected :----//>" + myDropdown.value);
		} else {
			PlayerPrefs.SetString ("CharacterType", "Female");
			print ("Female Character Selected :----//>" + myDropdown.value);
		}
	}

	public void MaleFemaleSelection_Button ()
	{
		GetComponent<CharacterCustomizationAtStart> ().enabled = false;
		Panel_Character_MaleFemaleSelection.SetActive (false);
		Panel_CharacterCustomization.SetActive (true);
		Panel_SkinToneSelection.SetActive (true);
		CurrentActive_Panel = Panel_SkinToneSelection;
		secondaryCamera.SetActive (true);
	}

	//	public void RandomCharacterSelectionButton ()
	//	{
	//		GameObject Child_Object = secondaryCamera.transform.GetChild (0).gameObject;
	//		Child_Object.transform.parent = null;
	//		DontDestroyOnLoad (Child_Object);
	//		SceneManager.LoadScene ("GamePlay");
	//	}

	public void ConfirmedCharacterSelectionButton ()
	{
		GameObject Child_Object = secondaryCamera.transform.GetChild (0).gameObject;
		Child_Object.transform.parent = null;
		DontDestroyOnLoad (Child_Object);
		SceneManager.LoadScene ("GamePlay");
//        AsyncOperation async = Application.LoadLevelAsync("GamePlay");
//        yield return async;
	}

	void ZoomCameraOnViewingParts (Vector3 playerPosition, float cameraZoomInOutSize)
	{
		SecondaryCamera_Player.GetComponent<Camera> ().orthographicSize = cameraZoomInOutSize;
		MainGame_Player.transform.localPosition = playerPosition;
	}
}
