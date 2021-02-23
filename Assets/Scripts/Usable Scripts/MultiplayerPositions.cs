using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerPositions : MonoBehaviour 
{

	public Transform GetEmptyPosition()
	{
		for(int i =0; i< transform.childCount; i++)
		{
			if (transform.FindChild (i.ToString ()).gameObject.activeInHierarchy) {
				Transform Object = transform.FindChild (i.ToString ());
				Object.gameObject.SetActive (false);
				return Object;
			} 
		}

			return null;
	}

	public void RemovePointAtIndex(int index)
	{
		transform.FindChild (index.ToString ()).gameObject.SetActive (false);
	}

	public void ResetAllPoints()
	{
		StartCoroutine (IResetAllPoints ());
	}
	IEnumerator IResetAllPoints()
	{
		yield return new WaitForSeconds (0.2f);

		for(int i =0; i< transform.childCount; i++)
		{
			transform.GetChild (i).gameObject.SetActive (true);
		}

        var AllPlayers = FindObjectsOfType <PublicAreaPlayer>();

		foreach(var playr in AllPlayers){
			RemovePointAtIndex (playr.PositionIndexInMp);
		}
	}
}