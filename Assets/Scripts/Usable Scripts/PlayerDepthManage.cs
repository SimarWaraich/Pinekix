using UnityEngine;
using System.Collections;

public class PlayerDepthManage : MonoBehaviour
{


	SpriteRenderer[] sprites;
	Vector3 dir;
	// Use this for initialization
	void Start ()
	{
		sprites = gameObject.GetComponentsInChildren<SpriteRenderer> ();
		dir = -(transform.position - transform.GetChild (0).position) / Vector3.Magnitude (transform.position - transform.GetChild (0).position);
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		RaycastHit2D hit = Physics2D.Raycast (transform.position, dir);


		if (hit.collider.gameObject.name.Contains ("Wall")) {
			
		} else {
			
			for (int i = 0; i < sprites.Length; i++) {
				int x = (int)Camera.main.WorldToScreenPoint (gameObject.GetComponent<SpriteRenderer> ().bounds.min).y * -1;
				sprites [i].sortingOrder = x;
			}
		}


	}
}
