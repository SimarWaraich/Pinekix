using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Walls3D : MonoBehaviour
{

	public List<SpriteRenderer> AllWalls;
	public List<WallPart> WallParts;

	public void ChangeWallColors (Sprite[] allColors)
	{
		/// For Temp untill fix the wall
//		for (int i = 0; i < allColors.Length; i++) {
//			AllWalls [i].sprite = allColors [i];
//		}
		for (int j = 0; j < WallParts.Count; j++) {
			
			for (int k = 0; k < WallParts [j].WallPrat.Count; k++) {
				
			}
		}
	}

	public void ChangeWallColorsNew (WallsColor newWalls)
	{

		for (int j = 0; j < newWalls.WallTexture.Count; j++) {

			for (int k = 0; k < newWalls.WallTexture [j].WallTexturePart.Length; k++) {
				WallParts [j].WallPrat [k].sprite = newWalls.WallTexture [j].WallTexturePart [k];
			}
		}
	}

}

[Serializable]
public class WallPart
{
	public List<SpriteRenderer> WallPrat;
}


