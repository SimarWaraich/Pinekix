/// <summary>
/// Created by ::==>> Ankush Arora... dated 14 July 2016
/// 
/// This script is used to change the dress of the player
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Simple_JSON;

public class DressChange : MonoBehaviour
{
	private GameObject Target_1;
	private GameObject Target_2;
	private GameObject Target_3;
	private GameObject Target_4;

	private Button currentSelectedButton;
	private Button currentSelectedColorButton;

	private int spriteNumberSelected;

	public CustomCharacter customchar = new CustomCharacter ();


	void Start ()
	{
		customchar.clothing = "default";
		customchar.ears = "default";
		customchar.eyes = "default";
		customchar.gender = 0;
		customchar.hair = "default";
		customchar.lips = "default";
		customchar.nose = "default";
		customchar.shoes = "default";
		customchar.skin_tone = 0;
	}

	public void Buttons (int spritenumber)
	{
		
		if (currentSelectedButton) {
			currentSelectedButton.interactable = true;
		}
		currentSelectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		print (currentSelectedButton);
		currentSelectedButton.interactable = false;
		print (currentSelectedButton);

		if (currentSelectedButton.name.Contains ("Color_1") ||
		    currentSelectedButton.name.Contains ("Color_2") ||
		    currentSelectedButton.name.Contains ("Color_3")) {
		
			Target_1 = GameObject.Find ("lower1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("upper1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("right arm1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("right mid arm");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("right hand1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("left arm1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("left arm mid1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("left hand1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("face1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("right leg1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("right leg mid1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("right leg btm1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("left leg1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("left leg mid1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			Target_1 = GameObject.Find ("left leg btm1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);

			if (currentSelectedButton.name.Contains ("Color_1")) {
				PlayerPrefs.SetInt ("SkinToneColor", 0);
				customchar.skin_tone = 0;
			} else if (currentSelectedButton.name.Contains ("Color_2")) {

				PlayerPrefs.SetInt ("SkinToneColor", 1);
				customchar.skin_tone = 1;

			} else {
				PlayerPrefs.SetInt ("SkinToneColor", 2);
				customchar.skin_tone = 2;
			}



		} else if (currentSelectedButton.name == "Eyes_1" ||
		           currentSelectedButton.name == "Eyes_2" ||
		           currentSelectedButton.name == "Eyes_3" ||
		           currentSelectedButton.name == "Eyes_4") {

			spriteNumberSelected = spritenumber;
			Target_1 = GameObject.Find ("left eye");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (0, spritenumber);
			Target_1 = GameObject.Find ("right eye");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (0, spritenumber);
			customchar.eyes = spritenumber.ToString ();

		} else if (currentSelectedButton.name.Contains ("Nose_1") ||
		           currentSelectedButton.name.Contains ("Nose_2") ||
		           currentSelectedButton.name.Contains ("Nose_3")) {

			spriteNumberSelected = spritenumber;
			Target_1 = GameObject.Find ("nose");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (0, spritenumber);
			customchar.nose = spritenumber.ToString ();


		} else if (currentSelectedButton.name.Contains ("Lips_1") ||
		           currentSelectedButton.name.Contains ("Lips_2") ||
		           currentSelectedButton.name.Contains ("Lips_3") ||
		           currentSelectedButton.name.Contains ("Lips_4")) {
			spriteNumberSelected = spritenumber;
			Target_1 = GameObject.Find ("lips");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (0, spritenumber);
			customchar.lips = spritenumber.ToString ();

		} else if (currentSelectedButton.name.Contains ("Ears_1") ||
		           currentSelectedButton.name.Contains ("Ears_2") ||
		           currentSelectedButton.name.Contains ("Ears_3") ||
		           currentSelectedButton.name.Contains ("Ears_4")) {

			spriteNumberSelected = spritenumber;
			if (PlayerPrefs.GetString ("CharacterType") == "Male") {
				Target_1 = GameObject.Find ("ear");
				Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), spritenumber);
			} else {
				Target_1 = GameObject.Find ("ear");
				Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			}
			customchar.ears = spritenumber.ToString ();

		} else if (currentSelectedButton.name.Contains ("Hair_1") ||
		           currentSelectedButton.name.Contains ("Hair_2") ||
		           currentSelectedButton.name.Contains ("Hair_3") ||
		           currentSelectedButton.name.Contains ("Hair_4")) {

			if (PlayerPrefs.GetString ("CharacterType") == "Male") {
				Target_1 = GameObject.Find ("hair");
				Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			} else {
				Target_1 = GameObject.Find ("front hair");
				Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
				Target_1 = GameObject.Find ("back hair");
				Target_1.GetComponent<StartImageManager> ().ChangeImage (spritenumber, 0);
			}
			customchar.hair = spritenumber.ToString ();

		} else if (currentSelectedButton.name.Contains ("Shoes_1") ||
		           currentSelectedButton.name.Contains ("Shoes_2")) {

			Target_1 = GameObject.Find ("left leg btm1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), spritenumber);
			Target_1 = GameObject.Find ("right leg btm1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), spritenumber);
			customchar.shoes = spritenumber.ToString ();

		} else if (currentSelectedButton.name.Contains ("Clothing_1") ||
		           currentSelectedButton.name.Contains ("Clothing_2")) {

			Target_1 = GameObject.Find ("lower1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), spritenumber);
			Target_1 = GameObject.Find ("upper1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), spritenumber);
			Target_1 = GameObject.Find ("left leg1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), spritenumber);
			Target_1 = GameObject.Find ("right leg1");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), spritenumber);
			customchar.clothing = spritenumber.ToString ();

		}
	}

	public void ColorSelectorButton (int ColorNumber)
	{
		if (currentSelectedColorButton) {
			currentSelectedColorButton.interactable = true;
		}
		currentSelectedColorButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button> ();
		currentSelectedColorButton.interactable = false;
		if (currentSelectedColorButton.name == "EyesColor_1" ||
		    currentSelectedColorButton.name == "EyesColor_2" ||
		    currentSelectedColorButton.name == "EyesColor_3" ||
		    currentSelectedColorButton.name == "EyesColor_4" ||
		    currentSelectedColorButton.name == "EyesColor_5") {
			Target_1 = GameObject.Find ("left eye");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (ColorNumber, spriteNumberSelected);
			Target_2 = GameObject.Find ("right eye");
			Target_2.GetComponent<StartImageManager> ().ChangeImage (ColorNumber, spriteNumberSelected);
			customchar.eyes = ColorNumber.ToString () + "_" + spriteNumberSelected.ToString ();


		} else if (currentSelectedColorButton.name == "NoseColor_1" ||
		           currentSelectedColorButton.name == "NoseColor_2" ||
		           currentSelectedColorButton.name == "NoseColor_3" ||
		           currentSelectedColorButton.name == "NoseColor_4" ||
		           currentSelectedColorButton.name == "NoseColor_5") {
			Target_1 = GameObject.Find ("nose");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (ColorNumber, spriteNumberSelected);
			customchar.nose = ColorNumber.ToString () + "_" + spriteNumberSelected.ToString ();

		} else if (currentSelectedColorButton.name == "LipsColor_1" ||
		           currentSelectedColorButton.name == "LipsColor_2" ||
		           currentSelectedColorButton.name == "LipsColor_3" ||
		           currentSelectedColorButton.name == "LipsColor_4" ||
		           currentSelectedColorButton.name == "LipsColor_5") {
			Target_1 = GameObject.Find ("lips");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (ColorNumber, spriteNumberSelected);
			customchar.lips = ColorNumber.ToString () + "_" + spriteNumberSelected.ToString ();

		} else if (currentSelectedColorButton.name == "EarColor_1" ||
		           currentSelectedColorButton.name == "EarColor_2" ||
		           currentSelectedColorButton.name == "EarColor_3" ||
		           currentSelectedColorButton.name == "LipsColor_4" ||
		           currentSelectedColorButton.name == "LipsColor_5") {
			Target_1 = GameObject.Find ("ear");
			Target_1.GetComponent<StartImageManager> ().ChangeImage (ColorNumber, spriteNumberSelected);
			customchar.ears = ColorNumber.ToString () + "_" + spriteNumberSelected.ToString ();

		}
	}



	public void UpdateCharacter ()
	{
//		StartCoroutine (CharacterUpdate ());
	}
	
}


