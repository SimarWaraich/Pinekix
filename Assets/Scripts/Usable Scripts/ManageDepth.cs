using UnityEngine;
using System.Collections;

public class ManageDepth : MonoBehaviour
{
	public int Layer;
	public int LayerOffset;

	void OnEnable ()
	{
		transform.tag = "Wall";
	}

	void Start ()
	{
		DepthManagement ();

	}

	void Update ()
	{
		DepthManagement ();
	}

	void DepthManagement ()
	{
		for (int i = 0; i < transform.childCount; i++) {
			Layer = ((int)(Camera.main.WorldToScreenPoint (transform.GetChild (i).GetComponent<SpriteRenderer> ().bounds.min).y) * -1) + LayerOffset;
            transform.GetChild (i).GetComponent<SpriteRenderer> ().sortingOrder = Layer-i;
		}

	}

}