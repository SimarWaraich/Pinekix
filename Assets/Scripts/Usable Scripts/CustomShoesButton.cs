using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomShoesButton : MonoBehaviour 
{
    public List<string> PartName;
    List<GameObject> BoyCharacterParts = new List<GameObject>();
    List<GameObject> GirlcharacterParts = new List<GameObject>();
    public int Index;
    public List<Sprite> BodyParts;

    void Start ()
    {
        var _BoyCharacterParts = CharacterCustomizationAtStart.Instance.MaleCharacter.GetComponentsInChildren<BodyParts> (true);
        var _GirlcharacterParts = CharacterCustomizationAtStart.Instance.FemaleCharacter.GetComponentsInChildren<BodyParts> (true);

        foreach(var go in _BoyCharacterParts)
        {
            BoyCharacterParts.Add(go.gameObject);
        }

        foreach(var go in _GirlcharacterParts)
        {
            GirlcharacterParts.Add(go.gameObject);
        }
        gameObject.GetComponent<Button> ().onClick.AddListener (() => OnClick ());
	}

    void OnClick ()
    {
        int index = 0;
        for (int i = 0; i < Mathf.Min (PartName.Count, BodyParts.Count); i++) {
            if (PlayerPrefs.GetString ("CharacterType") == "Male") {
                index = BoyCharacterParts.FindIndex (obj => obj.name== PartName [i]);
                BoyCharacterParts[index].GetComponent<SpriteRenderer>().sprite = BodyParts[i];
            } else {
                index = GirlcharacterParts.FindIndex (obj => obj.name == PartName [i]);
                GirlcharacterParts [index].GetComponent<SpriteRenderer> ().sprite = BodyParts[i];
            }
        }

        var cs = new CustomCharacterSaving();
        cs.IndexofItem = Index;
        cs.Color = CharacterCustomizationAtStart.Instance.SelectedColor;

        if (CharacterCustomizationAtStart.Instance.EditedCharDict.ContainsKey(CharacterCustomizationAtStart.Instance.SelectedCategory))
            CharacterCustomizationAtStart.Instance.EditedCharDict.Remove(CharacterCustomizationAtStart.Instance.SelectedCategory);

        CharacterCustomizationAtStart.Instance.EditedCharDict.Add(CharacterCustomizationAtStart.Instance.SelectedCategory, cs);
    }
}
