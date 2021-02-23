/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 08 July 2k16
/// This script will be used to store the data of the items owned by player
/// </summary>

using System;
using UnityEngine;
using System.Collections.Generic;



public class ItemsData
{
	[Header ("Info")]
	public string Name;
	public string Category;
	public string SubCategory;
	public int UniqueId;

	[Header ("Images")]
	public Sprite Icon;
	public Sprite Right_Image;
	public Sprite Left_Image;
	public Sprite Front_Image;
	public Sprite Back_Image;

	[Header ("Conditions")]
	public bool Purchased;
	public bool Unlocked;
	public Dictionary<string, string> UnlockConditions;

}
