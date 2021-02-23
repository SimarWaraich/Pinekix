using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{

	void OnMouseDown ()
	{
		print ("rotating");
		this.transform.GetComponentInParent <Decor3DView> ().ChangeDirection ();
	}
}
