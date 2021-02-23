using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class ReModelShopController : Singleton<ReModelShopController>
{

	public GameObject UiPrefab;

	public GameObject Container;

	public List<WallsColor> WallsColors;

	public List<GroundTexture> Grounds;

	public Sprite Locked;

	public bool isForEvent = false;


	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
		AddToList ();
	}


	public void AddDynamicObjects (GameObject dataObj, string sub_cat)
	{
		if (sub_cat == "Walls")
			WallsColors.Add (dataObj.GetComponent<WallTextureInfo> ().colorObj);
		else if (sub_cat == "Ground")
			Grounds.Add (dataObj.AddComponent<GroundInfo> ().groundobj);
	}

	void AddToList ()
	{

		CheckUnlockConditions ();
	}

	void CheckUnlockConditions ()
	{
		for (int i = 0; i < WallsColors.Count; i++) {
			if (GameManager.Instance.level >= WallsColors [i].Level)
				WallsColors [i].Unlocked = true;
			else
				WallsColors [i].Unlocked = false;
		}
		for (int i = 0; i < Grounds.Count; i++) {
			if (GameManager.Instance.level >= Grounds [i].Level)
				Grounds [i].Unlocked = true;
			else
				Grounds [i].Unlocked = false;
		}
	}

	public void DeleteAllItems ()
	{
		for (int i = 0; i < Container.transform.childCount; i++) {
			GameObject.Destroy (Container.transform.GetChild (i).gameObject);			
		}
	}

	public void IntializeInventoryForWalls ()
	{
		DeleteAllItems ();
		WallsColors.ForEach (wall => {	
			if (GameManager.Instance.level >= wall.Level) {
//				int Index = AllItems.IndexOf (dress);
//				AllItems [Index].Unlocked = true;
				wall.Unlocked = true;		
			} else
				wall.Unlocked = false;

			GameObject Go = Instantiate (UiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
			Go.transform.parent = Container.transform;
			Go.transform.localScale = Vector3.one;

			var newWall = Go.AddComponent<WallsColorUI> ();
			if (isForEvent)
				newWall.isForEvent = true;
			newWall.data = wall;
		});
	}

	public void IntializeInventoryForGrounds ()
	{
		DeleteAllItems ();
		Grounds.ForEach (ground => {	
			if (GameManager.Instance.level >= ground.Level) {
				//				int Index = AllItems.IndexOf (dress);
				//				AllItems [Index].Unlocked = true;
				ground.Unlocked = true;		
			} else
				ground.Unlocked = false;

			GameObject Go = Instantiate (UiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
			Go.transform.parent = Container.transform;
			Go.transform.localScale = Vector3.one;

			var newGround = Go.AddComponent<GroundsUI> ();
			if (isForEvent)
				newGround.isForEvent = true;
			newGround.data = ground;
		});
	}
}