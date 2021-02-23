using UnityEngine;
using System.Collections;

public class DressManager : Singleton<DressManager>
{

	public GameObject SelectedCharacter;
//	GameObject[] CharacterParts;


	[Header ("For Character Representation in UI")]
	public GameObject dummyCharacter;
//	GameObject[] dummyParts;

//	int SkinColor = 0;

	void Awake ()
	{
		this.Reload ();
	}

//	void SelectCharacter (GameObject Character)
//	{
//		var SpriteParts = Character.transform.GetChild (0).GetComponentsInChildren<CharacterProperties> ();
//
//		CharacterParts = new GameObject[SpriteParts.Length];
//
//		for (int i = 0; i < SpriteParts.Length; i++) {
//			CharacterParts [i] = SpriteParts [i].gameObject;
//		}
//	}		
	//	public void ChangeFlatMateDress (int itemtype, string type)
	//	{
	//		if (SelectedCharacter)
	//			SelectCharacter (SelectedCharacter);
	//		else
	//			SelectCharacter (PlayerManager.Instance.MainCharacter);
	//
	//
	//		for (int i = 0; i < CharacterParts.Length; i++) {
	//			if (CharacterParts [i].GetComponent<CharacterProperties> ().sprites [itemtype].Name == type)
	//				CharacterParts [i].GetComponent<CharacterProperties> ().ChangeSprite (itemtype, 0);
	//		}
	//	}


    public void ChangeFlatMateDress (GameObject Avatar,string[] DressParts, Sprite[] Images)
    {   
        SelectedCharacter = Avatar;
        ChangeFlatMateDress(DressParts, Images);
    }

	public void ChangeFlatMateDress (string[] DressParts, Sprite[] Images)
	{	
        if (!SelectedCharacter)
            return;

        var _xyz = SelectedCharacter.GetComponentsInChildren<DressParts> (true);
		for (int i = 0; i < Mathf.Min (DressParts.Length, Images.Length); i++) {
			foreach (var _x in _xyz) {
				if (_x.name == DressParts [i]) {
					_x.GetComponent<SpriteRenderer> ().sprite = Images [i];
				}
			}
		}
	} 

	//
	//	public void ChangeDressForDummyCharacter (int itemtype, string type)
	//	{
	//		if (dummyCharacter)
	//			SelectDummyCharacter (dummyCharacter);
	//
	//		for (int i = 0; i < dummyParts.Length; i++) {
	//			if (dummyParts [i].GetComponent<CharacterProperties> ().sprites [itemtype].Name == type)
	//				dummyParts [i].GetComponent<CharacterProperties> ().ChangeSprite (itemtype, 0);
	//		}
	//	}

	public void ChangeDressForDummyCharacter (string[] DressParts, Sprite[] Images)
	{	
        if (!dummyCharacter)
            return;

        var _xyz = dummyCharacter.GetComponentsInChildren<DressParts> (true);
		for (int i = 0; i < Mathf.Min (DressParts.Length, Images.Length); i++) {
			foreach (var _x in _xyz) {
				if (_x.name == DressParts [i]) {
					_x.GetComponent<SpriteRenderer> ().sprite = Images [i];
				}
			}
		}
    }

//	void SelectDummyCharacter (GameObject Char)
//	{
//
//		var SpriteParts = Char.transform.GetChild (0).GetComponentsInChildren<CharacterProperties> ();
//
//		dummyParts = new GameObject[SpriteParts.Length];
//
//		for (int i = 0; i < SpriteParts.Length; i++) {
//			dummyParts [i] = SpriteParts [i].gameObject;
//		}
//	}


    public void ChangeHairsOfCharacter (GameObject Avatar,string[] BodyParts, Sprite[] Images)
    {   
        SelectedCharacter = Avatar;
        ChangeFlatMateDress(BodyParts, Images);
    }

    public void ChangeHairsOfCharacter (string[] BodyParts, Sprite[] Images)
    {   
        if (!SelectedCharacter)
            return;

        var _xyz = SelectedCharacter.GetComponentsInChildren<BodyParts> (true);
        for (int i = 0; i < Mathf.Min (BodyParts.Length, Images.Length); i++) {
            foreach (var _x in _xyz) {
                if (_x.name == BodyParts [i]) {
                    _x.GetComponent<SpriteRenderer> ().sprite = Images [i];
                }
            }
        }
    }

    public void ChangeHairsForDummyCharacter (string[] BodyParts, Sprite[] Images)
    {   
        if (!dummyCharacter)
            return;

        var _xyz = dummyCharacter.GetComponentsInChildren<BodyParts> (true);
        for (int i = 0; i < Mathf.Min (BodyParts.Length, Images.Length); i++) {
            foreach (var _x in _xyz) {
                if (_x.name == BodyParts [i]) {
                    _x.GetComponent<SpriteRenderer> ().sprite = Images [i];
                }
            }
        }
    }
}

