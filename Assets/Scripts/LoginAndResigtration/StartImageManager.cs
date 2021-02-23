
/// <summary>
/// Created by Mandeep Yadav Dated 14 July 2016
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

[Serializable]
public struct SpriteList
{
	public string name;
	public Sprite[] Images;
}

[Serializable]
public class StartImageManager : MonoBehaviour
{
	public SpriteList[] ListofSprites;

    void Start()
    {
        var lkjhd = gameObject.name;
    }
	public void ChangeImage (int color, int count)
	{
		gameObject.GetComponent<SpriteRenderer>().sprite = ListofSprites [count].Images [color];
	}
}
