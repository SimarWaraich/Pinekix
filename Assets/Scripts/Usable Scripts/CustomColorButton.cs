using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomColorButton : MonoBehaviour 
{
    public ItemColors ThisColor;

    public List<string> PartName;

    public bool iscoloronly;
    BodyParts[] BoyCharacterParts;

    BodyParts[] GirlcharacterParts;
//    public int Index;

    void Start()
    {

        transform.GetChild(0).
        GetComponent<UnityEngine.UI.Image>().color = ThisColor.icon;
        gameObject.GetComponent<UnityEngine.UI.Button> ().onClick.AddListener (() => OnColor ());

        BoyCharacterParts = CharacterCustomizationAtStart.Instance.MaleCharacter.GetComponentsInChildren<BodyParts> (true);
        GirlcharacterParts = CharacterCustomizationAtStart.Instance.FemaleCharacter.GetComponentsInChildren<BodyParts> (true);
    }

    public void OnColor ()
    {
        CharacterCustomizationAtStart.Instance.SelectedColor = ThisColor.name;
        if (string.IsNullOrEmpty (CharacterCustomizationAtStart.Instance.SelectedCategory))
            CharacterCustomizationAtStart.Instance.SelectedCategory = "SkinTone";
//        switch(CharacterCustomizationAtStart.Instance.SelectedCategory)
//        {
//            case "SkinTone":
//                break;
//            case "Eyes":
//                CharacterCustomizationAtStart.Instance.Eyes = Index;
//                break;
//            case "Nose":
//                CharacterCustomizationAtStart.Instance.Nose = Index;
//                break;
//            case "Lips":
//                break;
//            case "Hair":
//                break;
//            case "Ears":
//                break;
//            case "Shoes":
//                break;
//        }

        // 

        var cs = new CustomCharacterSaving();
        cs.Color = ThisColor.name;
        if (CharacterCustomizationAtStart.Instance.EditedCharDict.ContainsKey(CharacterCustomizationAtStart.Instance.SelectedCategory))
            cs.IndexofItem = CharacterCustomizationAtStart.Instance.EditedCharDict[CharacterCustomizationAtStart.Instance.SelectedCategory].IndexofItem;
        else
            cs.IndexofItem = 0;

        if (CharacterCustomizationAtStart.Instance.EditedCharDict.ContainsKey(CharacterCustomizationAtStart.Instance.SelectedCategory))
            CharacterCustomizationAtStart.Instance.EditedCharDict.Remove(CharacterCustomizationAtStart.Instance.SelectedCategory);

        CharacterCustomizationAtStart.Instance.EditedCharDict.Add(CharacterCustomizationAtStart.Instance.SelectedCategory, cs);


        if (!iscoloronly)
        {     
            if( CharacterCustomizationAtStart.Instance.SelectedCategory == "Shoes")
                CharacterCustomizationAtStart.Instance.GenerateShoesButton();
            else
                CharacterCustomizationAtStart.Instance._GenerateButtons();
        }
        else
        {
//            switch(ThisColor.name){
//                case "White":
////                    PlayerPrefs.SetInt("SkinToneColor", 0);
                    CharacterCustomizationAtStart.Instance.SelectedCharacter.GetComponent<CharacterProperties>().PlayerType = ThisColor.name;
//                    break;
//                case "Brown":                    
//                    PlayerPrefs.SetInt("SkinToneColor", 1);
//                    break;
//                case "Black":
//                    PlayerPrefs.SetInt("SkinToneColor", 2);
//                    break;
//                default:
//                    PlayerPrefs.SetInt("SkinToneColor", 0);
//                    break;                  

//            }
//            ChangeDress(cs.IndexofItem);
        } 
        ChangeDress(cs.IndexofItem);
    }

    void ChangeDress(int index = 0)
    {
        for (int i = 0; i < Mathf.Min (PartName.Count, ThisColor.AllItems[index].images.Count); i++) {
            if (PlayerPrefs.GetString ("CharacterType") == "Male") {
                foreach(var part in BoyCharacterParts)
                {
                    if(part.name ==PartName [i])
                        part.GetComponent<SpriteRenderer>().sprite = ThisColor.AllItems[index].images[i];
                    
                }
            } else {
                foreach(var part in GirlcharacterParts)
                {
                    if(part.name ==PartName [i])
                        part.GetComponent<SpriteRenderer>().sprite = ThisColor.AllItems[index].images[i];

                }
            }
        }
        if(CharacterCustomizationAtStart.Instance.SelectedCategory == "SkinTone")
        {
            if (CharacterCustomizationAtStart.Instance.EditedCharDict.ContainsKey("Nose"))
            {
                var IndexofItem = CharacterCustomizationAtStart.Instance.EditedCharDict["Nose"].IndexofItem;
                if (PlayerPrefs.GetString("CharacterType") == "Male")
                    PlayerManager.Instance.ApplyDress(BoyCharacterParts, "Male", "Nose", ThisColor.name, IndexofItem);
                else
                    PlayerManager.Instance.ApplyDress(GirlcharacterParts,"Female", "Nose", ThisColor.name, IndexofItem);
                
                var cs = new CustomCharacterSaving();
                cs.Color = ThisColor.name;
                cs.IndexofItem = IndexofItem;
                CharacterCustomizationAtStart.Instance.EditedCharDict["Nose"] = cs;
            }

             if (CharacterCustomizationAtStart.Instance.EditedCharDict.ContainsKey("Ears"))
            {
                var IndexofItem = CharacterCustomizationAtStart.Instance.EditedCharDict["Ears"].IndexofItem;
                if (PlayerPrefs.GetString("CharacterType") == "Male")
                    PlayerManager.Instance.ApplyDress(BoyCharacterParts,"Male", "Ears",ThisColor.name,IndexofItem);
                else
                    PlayerManager.Instance.ApplyDress(GirlcharacterParts,"Female", "Ears",ThisColor.name,IndexofItem);
                var cs = new CustomCharacterSaving();
                cs.Color = ThisColor.name;
                cs.IndexofItem = IndexofItem;
                CharacterCustomizationAtStart.Instance.EditedCharDict["Ears"] = cs;
            }
             if (CharacterCustomizationAtStart.Instance.EditedCharDict.ContainsKey("Shoes"))
            {
                var IndexofItem = CharacterCustomizationAtStart.Instance.EditedCharDict["Shoes"].IndexofItem;
                var Color = CharacterCustomizationAtStart.Instance.EditedCharDict["Shoes"].Color;
                if (PlayerPrefs.GetString("CharacterType") == "Male")
                    CharacterCustomizationAtStart.Instance.ChangeShoesOnClickSkinTone(Color,IndexofItem);
                else
                    CharacterCustomizationAtStart.Instance.ChangeShoesOnClickSkinTone(Color,IndexofItem);
//                var cs = new CustomCharacterSaving();
//                cs.Color = ThisColor.name;
//                cs.IndexofItem = IndexofItem;
//                CharacterCustomizationAtStart.Instance.EditedCharDict["Shoes"] = cs;
            }
        }
    }

    public static string GetSkinNameToInt(int skintonNumber)
    {
        string Value = "";
        switch(skintonNumber)
        {
            case 0:
                Value = "White";
                break;
            case 1:
                Value = "Brown";
                break;
            case 2:
                Value = "Black";
                    break;
            default:
                Value = "Brown";
                break;
        }
        return Value;
        
    }

//    void ChangeNoseAccordingToSkin()
//    {
//
//         
//        for (int i = 0; i < Mathf.Min (PartName.Count, ThisColor.AllItems[0].images.Count); i++) {
//            if (PlayerPrefs.GetString ("CharacterType") == "Male") {
//                foreach(var part in BoyCharacterParts)
//                {
//                    if(part.name =="nose")
//                        part.GetComponent<SpriteRenderer>().sprite = ThisColor.AllItems[index].images[i];
//
//                }
//            } else {
//                foreach(var part in GirlcharacterParts)
//                {
//                    if(part.name == "nose"])
//                        part.GetComponent<SpriteRenderer>().sprite = ThisColor.AllItems[index].images[i];
//
//                }
//            } 
//        }
//    }
}
