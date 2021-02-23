using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterCustomButton : MonoBehaviour
{
    public ItemBodyParts _characteritem;
    public List<string> PartName;
    public string category;
	List<Transform> BoyCharacterParts = new List<Transform> ();
	List<Transform> GirlcharacterParts = new List<Transform> ();

    public ButtonType Mytype;
    public int Index;
//	public string _colorName;

	void Start ()
	{
        var _BoyCharacterParts = CharacterCustomizationAtStart.Instance.MaleCharacter.GetComponentsInChildren<BodyParts> (true);
        var _GirlcharacterParts = CharacterCustomizationAtStart.Instance.FemaleCharacter.GetComponentsInChildren<BodyParts> (true);
		foreach (var obj in _BoyCharacterParts) {
            BoyCharacterParts.Add (obj.transform);
		}
		foreach (var obj in _GirlcharacterParts) {
            GirlcharacterParts.Add (obj.transform);
		}
        if (Mytype == ButtonType.simple)
			gameObject.GetComponent<Button> ().onClick.AddListener (() => OnClick ());
		else
			gameObject.GetComponent<Button> ().onClick.AddListener (() => OnCategory ());

	}

	public void OnClick ()
	{
		int index = 0;

		for (int i = 0; i < Mathf.Min (PartName.Count, _characteritem.images.Count); i++) {
			if (PlayerPrefs.GetString ("CharacterType") == "Male") {
				index = BoyCharacterParts.FindIndex (obj => obj.name == PartName [i]);
                BoyCharacterParts[index].GetComponent<SpriteRenderer>().sprite = _characteritem.images[i];
			} else {
				index = GirlcharacterParts.FindIndex (obj => obj.name == PartName [i]);
				GirlcharacterParts [index].GetComponent<SpriteRenderer> ().sprite = _characteritem.images [i];
			}
		}

        var cs = new CustomCharacterSaving();
        cs.IndexofItem = Index;
        cs.Color = CharacterCustomizationAtStart.Instance.SelectedColor;

        if (CharacterCustomizationAtStart.Instance.EditedCharDict.ContainsKey(CharacterCustomizationAtStart.Instance.SelectedCategory))
            CharacterCustomizationAtStart.Instance.EditedCharDict.Remove(CharacterCustomizationAtStart.Instance.SelectedCategory);
        
        CharacterCustomizationAtStart.Instance.EditedCharDict.Add(CharacterCustomizationAtStart.Instance.SelectedCategory, cs);
  
	}

	public void OnCategory ()
	{
//		var category = _characteritem._category;
		CharacterCustomizationAtStart.Instance.SelectedCategory = category;
        CharacterCustomizationAtStart.Instance.SelectedColor = "";     
        if(category.Contains("Shoes"))
            CharacterCustomizationAtStart.Instance.GenerateShoesColor ();
        else
            CharacterCustomizationAtStart.Instance._GenerateColorButtons ();
        CharacterCustomizationAtStart.Instance.DeleteOldButtons();

        CharacterCustomizationAtStart.Instance.ZoomCameraToCategories();

        if (CharacterCustomizationAtStart.Instance.SelectedCategory == "SkinTone")
        {
            CharacterCustomizationAtStart.Instance.SelectedColor = CharacterCustomizationAtStart.Instance.SelectedCharacter.GetComponent<CharacterProperties>().PlayerType;
            return;
        }
        else if (CharacterCustomizationAtStart.Instance.SelectedCategory == "Nose")
        {
            CharacterCustomizationAtStart.Instance.SelectedColor = CharacterCustomizationAtStart.Instance.SelectedCharacter.GetComponent<CharacterProperties>().PlayerType;
//            var Skin = PlayerPrefs.GetInt("SkinToneColor");
//            switch (Skin)
//            {
//                case 0:
//                    CharacterCustomizationAtStart.Instance.SelectedColor = "White";       
//                    break;
//                case 1:
//                    CharacterCustomizationAtStart.Instance.SelectedColor = "Brown";
//                    break;
//                case 2:                    
//                    CharacterCustomizationAtStart.Instance.SelectedColor = "Black";
//                    break;
//            }
        }
        else if (CharacterCustomizationAtStart.Instance.SelectedCategory == "Ears")
        {
            CharacterCustomizationAtStart.Instance.SelectedColor = CharacterCustomizationAtStart.Instance.SelectedCharacter.GetComponent<CharacterProperties>().PlayerType;

//            var Skin = PlayerPrefs.GetInt("SkinToneColor");
//            switch (Skin)
//            {
//                case 0:
//                    CharacterCustomizationAtStart.Instance.SelectedColor = "White";       
//                    break;
//                case 1:
//                    CharacterCustomizationAtStart.Instance.SelectedColor = "Brown";
//                    break;
//                case 2:                    
//                    CharacterCustomizationAtStart.Instance.SelectedColor = "Black";
//                    break;
//            }
        }
        else if (CharacterCustomizationAtStart.Instance.SelectedCategory == "Shoes")
        {
            CharacterCustomizationAtStart.Instance.GenerateShoesButton();
            return;
        }

        CharacterCustomizationAtStart.Instance._GenerateButtons();
    }
}
