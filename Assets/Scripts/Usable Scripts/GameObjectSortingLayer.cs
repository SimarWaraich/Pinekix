using UnityEngine;
using System.Collections;

public class GameObjectSortingLayer : MonoBehaviour {

	public string sortingLayer;
	public int orderInLayer;
	int layer;
	// Use this for initialization
	void OnEnable ()
	{
		layer = SortingLayer.GetLayerValueFromName(sortingLayer);
		GetComponent <MeshRenderer>().sortingLayerName = sortingLayer;
		GetComponent <MeshRenderer> ().sortingOrder = orderInLayer;
	}
}
