using UnityEngine;
using System.Collections;

public class Discard : MonoBehaviour
{

	void OnMouseDown ()
	{
        this.transform.GetComponentInParent <Decor3DView> ().Cross ();
	}
}
