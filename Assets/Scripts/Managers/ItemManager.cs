using UnityEngine;
using System.Collections;

public class ItemManager : Singleton<ItemManager>
{

	public ItemsData[] items;

	void Awake ()
	{
		this.Reload ();
	}

	public void ChangeItemData ()
	{
		
	}

}

