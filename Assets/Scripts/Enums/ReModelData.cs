using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class NewFlat : ReModelData
{
	
	ReModelCategories Category = ReModelCategories.Flats;

	public GameObject Prefab;
	public bool isCreated;
}

[Serializable]
public class WallsColor : ReModelData
{
	
	ReModelCategories Category = ReModelCategories.WallTexture;

	public Sprite[] Textures;
	public List<WallsParts> WallTexture;
}

[Serializable]
public class WallsParts
{
	public Sprite[] WallTexturePart;
}

[Serializable]
public class GroundTexture : ReModelData
{
	
	ReModelCategories Category = ReModelCategories.GroundTexture;

	public Sprite Texture;
}

[Serializable]
public class ReModelData
{
	[Header ("Properties")]
	public string Name;
	public Sprite DisplayIcon;
	public bool Unlocked;
	public bool Purchased;

	[Header ("Unlocking Conditions")]
	public int Level;
	public int Gems;

	[Header ("Purchasing Conditions")]
	public int Price;
	//	bool GetisTextureType (ReModelData data)
	//	{
	//		if (data.Category == ReModelCategories.Flats)
	//			return false;
	//		else
	//			return true;
	//	}


}

public enum ReModelCategories
{
	Flats = 0,
	WallTexture = 1,
	GroundTexture = 2,
}
