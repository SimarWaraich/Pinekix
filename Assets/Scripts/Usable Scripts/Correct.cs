using UnityEngine;
using System.Collections;

public class Correct : MonoBehaviour
{

	void OnMouseDown ()
	{
		this.transform.GetComponentInParent<Decor3DView> ().Correct ();
	}
}
