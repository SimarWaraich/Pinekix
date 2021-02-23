using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaloonInfo : MonoBehaviour
{

	public Sprite Icon;
    [Header ("Gender - 0 for Female, 1 for Male")]
	public int Gender;
	public List<string> BodyPartName = new List<string> ();
	public List<Sprite> BodyPartSprites = new List<Sprite> ();

    public void ChangeHairs()
    {
        GameObject selected = null;

        if(Gender ==0)
            selected = GameObject.Find("Girl Character");
        else
            selected = GameObject.Find("Male Character");

        var _xyz = selected.GetComponentsInChildren<BodyParts>(true);

        for (int i = 0; i < Mathf.Min(BodyPartName.Count, BodyPartSprites.Count); i++)
        {
            foreach (var _x in _xyz)
            {
                if (_x.name == BodyPartName[i])
                {
                    _x.GetComponent<SpriteRenderer>().sprite = BodyPartSprites[i];
                }
            }
        }
    }
}