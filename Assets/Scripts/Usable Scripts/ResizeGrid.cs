using UnityEngine;
using System.Collections;

public class ResizeGrid : MonoBehaviour
{


	public Transform pointA;
	public Transform pointB;
	public Transform pointC;
	public Transform pointD;
	public GameObject spritePfb;
	private GameObject gridRotation;
	public float gridWidth = 5;
	public float gridHeight = 5;

	public GameObject[,] Grid;

	void Awake ()
	{
		//print ("Resize Grid");
		//GenerateGrid (pointA.position, pointB.position, pointC.position);
		GenerateGrid4Points (pointA.position, pointB.position, pointC.position, pointD.position);
		gridRotation = this.gameObject;
		gridRotation.transform.Rotate (0, 0, 45);
		var container = transform.FindChild ("grid Container");
		for (int i = 0; i < container.childCount; i++) {
			container.transform.GetChild (i).GetComponent<WayPoint> ().UpdateGameObjects ();
		}
	
	}

	void OnMouseDown ()
	{
		print ("Click On Ground");
//		if (ScreenManager.Instance.ScreenMoved) {
//			ScreenManager.Instance.MoveScreenToBack ();
//		}
//
//		DecorController.Instance.PlacedDecors.ForEach (decor => {
//			decor.EnablePlacement ();
//		});
	}

	//	void GenerateGrid (Vector3 a, Vector3 b, Vector3 c)
	//	{
	//
	//		GameObject container = new GameObject ();
	//		container.name = "Grid Container";
	//		container.transform.position = c;
	//		float spriteWidth = Vector3.Distance (a, b) / gridWidth;
	//		float spriteHeight = Vector3.Distance (a, c) / gridHeight;
	//
	//		for (int x = 0; x < gridWidth; x++) {
	//			for (int y = 0; y < gridHeight; y++) {
	//				GameObject sprite = (GameObject)Instantiate (spritePfb, container.transform.position, Quaternion.identity);
	//				sprite.transform.parent = container.transform;
	//				sprite.transform.localPosition = new Vector3 (x * spriteWidth, y * spriteHeight, 0);
	//				sprite.transform.localScale = new Vector3 (spriteWidth, spriteHeight, 1);
	//
	//			}
	//		}
	//		container.transform.SetParent (pointA.parent, false);
	//	}

	void GenerateGrid4Points (Vector3 a, Vector3 b, Vector3 c, Vector3 d)
	{		
		Grid = new GameObject[(int)gridWidth, (int)gridHeight];

		GameObject container = new GameObject ();
		Vector3 _c = new Vector3 (a.x, d.y, c.z);



		container.name = "grid Container";
		container.transform.position = _c;

		float spriteWidth = Vector3.Distance (a, b) / gridWidth;
		float spriteHeight = Vector3.Distance (a, c) / gridHeight;


		for (int x = 0; x < gridWidth; x++) {
			for (int y = 0; y < gridHeight; y++) {
				GameObject GenObj = (GameObject)Instantiate (spritePfb, container.transform.position, Quaternion.identity);
				GenObj.transform.parent = container.transform;
				GenObj.name = "" + x + "_" + y;
				GenObj.GetComponent<WayPoint> ().row_number = x;
				GenObj.GetComponent<WayPoint> ().column_number = y;
				float xpos = (x * spriteWidth) + (((gridHeight - y) / gridHeight) * (c.x - _c.x));
				float ypos = (y * spriteHeight) + (((gridWidth - x) / gridWidth) * (c.y - _c.y));
//				//print ("" + xpos + "-" + ypos);
				GenObj.transform.localPosition = new Vector3 (xpos, ypos, 0);
				GenObj.transform.localScale = new Vector3 (spriteWidth, spriteHeight, 1);
				/// NOTE: To be removed from both here and gamemanager
				if (SocietyPartyManager.Instance.AttendingParty) {					
					GameManager.Instance.SocietyPartyWayPoints.Add (GenObj);
				} 
				else if(HostPartyManager.Instance.AttendingParty)
				{
					GameManager.Instance.FlatPartyWayPoints.Add (GenObj);
				}else 
					GameManager.Instance.WayPoints.Add (GenObj);

				
				
				Grid [x, y] = GenObj;
//				if (x == 0 || x == gridWidth - 1 || y == 0 || y == gridHeight - 1)
//					GenObj.GetComponent<WayPoint> ()._CanBeUsed = false;

				if (x == (gridWidth / 2) || y == (gridHeight / 2))
					GenObj.GetComponent<WayPoint> ()._CanBeUsed = true;

				//print (Grid [x, y].name);
			}
		}

		container.transform.SetParent (pointA.parent, true);


	}
}