using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flat3D : MonoBehaviour
{

	public FlatData data;

	public Walls3D Walls;

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 currPos;

	public string WallColourNames;
	public string GroundTextureName;

	void Start ()
	{
		this.gameObject.AddComponent<BoxCollider2D> ().isTrigger = true;
		this.gameObject.AddComponent<Rigidbody2D> ().isKinematic = true;

		GroundTextureName = this.gameObject.GetComponent<SpriteRenderer> ().sprite.name.ToString ();
	}

	//	public void CreateThisFlat (NewFlat _data)
	//	{
	//		data = _data;
	//		List<GameObject> temp = new List<GameObject> ();
	//		for (int i = 0; i < FlatManager.Instance.AddedFlats.Length; i++) {
	//			temp.Add (FlatManager.Instance.AddedFlats [i]);
	//		}
	//		temp.Add (this.gameObject);
	//		FlatManager.Instance.AddedFlats = temp.ToArray ();
	//	}

	public void ChangeGroungColor (Sprite color)
	{
		if (color == null)
			return;
		
		GetComponent<SpriteRenderer> ().sprite = color;
	}



	public void ChangeChangeGroundTextureName (string ground)
	{
		GroundTextureName = ground;
	}

	public void ChangeChangeWallTextureName (string wall)
	{
		WallColourNames = wall;
	}

	
	void Update ()
	{
//		if (GroundsUI.ApplyRoomTexture) {
//	
//			if (Input.GetMouseButtonDown (0)) {
//				var cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
//				Ray ray = cam.ScreenPointToRay (Input.mousePosition);
//				RaycastHit hit;
//				this.gameObject.GetComponent<Flat3D> ().ChangeGroungColor (RoomPurchaseManager.Instance.SeletedGroundTexture.Texture);
//				print ("here" + this.gameObject.name);
//				GroundsUI.ApplyRoomTexture = false;
//				if (Physics.Raycast (ray, out hit)) {
//					print ("here1");
//	
//				}
//			}
//		}
	
	}
}
