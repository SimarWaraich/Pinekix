using UnityEngine;
using System.Collections;

public class DecorOrderInLayer : MonoBehaviour
{

	public int x;
    public int offset;

	// Use this for initialization
	void Start ()
	{
	}

	// Update is called once per frame
	void Update ()
	{
		DepthManagement ();

	}

	void DepthManagement ()
	{	
        int layer = (int)Camera.main.WorldToScreenPoint (gameObject.GetComponent<SpriteRenderer> ().bounds.min).y * -1;
        x = layer + offset;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = (x);
        gameObject.GetComponentInChildren<Canvas>(true).sortingOrder = (x);
        transform.GetChild (2).GetComponent<SpriteRenderer> ().sortingOrder = x;
	} 
}
