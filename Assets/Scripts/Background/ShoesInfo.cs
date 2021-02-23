using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShoesInfo : MonoBehaviour 
{
    public Sprite Icon;
    [Header ("Gender - 0 for Female, 1 for Male")]
    public int Gender;

    public List<string> BodyPartName = new List<string> ();

    public List<Sprite> BrownShoes = new List<Sprite> ();
    public List<Sprite> BlackShoes = new List<Sprite> ();
    public List<Sprite> WhiteShoes = new List<Sprite> ();




    public void ChangeShoes ()
    {
        GameObject selected = null;

        if(Gender ==0)
            selected = GameObject.Find("Girl Character");
        else
            selected = GameObject.Find("Male Character");

        var _xyz = selected.GetComponentsInChildren<BodyParts>(true);
        string SkinColor = selected.GetComponent<CharacterProperties>().PlayerType;

        if (SkinColor == "Brown")
        {

            for (int i = 0; i < Mathf.Min(BodyPartName.Count, BrownShoes.Count); i++)
            {
                foreach (var _x in _xyz)
                {
                    if (_x.name == BodyPartName[i])
                    {
                        _x.GetComponent<SpriteRenderer>().sprite = BrownShoes[i];
                    }
                }
            }
        }else if(SkinColor == "Black")
        {
            for (int i = 0; i < Mathf.Min(BodyPartName.Count, BlackShoes.Count); i++)
            {
                foreach (var _x in _xyz)
                {
                    if (_x.name == BodyPartName[i])
                    {
                        _x.GetComponent<SpriteRenderer>().sprite = BlackShoes[i];
                    }
                }
            }
        }else
        {
            for (int i = 0; i < Mathf.Min(BodyPartName.Count, WhiteShoes.Count); i++)
            {
                foreach (var _x in _xyz)
                {
                    if (_x.name == BodyPartName[i])
                    {
                        _x.GetComponent<SpriteRenderer>().sprite = WhiteShoes[i];
                    }
                }
            }
        }
    }
}
