using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DressInfo : MonoBehaviour
{
	public Sprite Icon;
    [Header ("Gender - 0 for Female, 1 for Male")]
	public int Gender;

	public List<string> BodyPartName = new List<string> ();
	public List<Sprite> DressesSprites = new List<Sprite> ();
//	public List<Sprite> BodyPartSprites_Brown = new List<Sprite> ();
//	public List<Sprite> BodyPartSprites_Black = new List<Sprite> ();

    /// <summary>
    /// Only for test purposes
    /// </summary>
    public void ApplyDress()
    {
        GameObject selected = null;

        if(Gender ==0)
            selected = GameObject.Find("Girl Character");
        else
            selected = GameObject.Find("Male Character");
        
            var _xyz = selected.GetComponentsInChildren<DressParts>(true);
       
            for (int i = 0; i < Mathf.Min(BodyPartName.Count, DressesSprites.Count); i++)
            {
                foreach (var _x in _xyz)
                {
                    if (_x.name == BodyPartName[i])
                    {
                        _x.GetComponent<SpriteRenderer>().sprite = DressesSprites[i];
                    }
                }
            }
         
    }
}

