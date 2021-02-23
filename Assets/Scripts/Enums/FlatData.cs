/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 08 July 2k16
/// This script will be used to store the data of the flat including the items used in flat with the placements
/// </summary>

using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class FlatData
{
	[Header ("Info of the Area")]
	public string AreaName;
	public float AreaPrice;
	public bool Purchased;
	[Header ("TransformDetails")]
	public Dictionary<string, float> TransformDetails;
	public int x;
	public int y;
}

[Serializable]
public class FlatProductData
{
	[Header ("Name of the Product")]
	public string Name;
	[Header ("TransformDetails")]
	public Dictionary<string, string> TransformDetails;
}
