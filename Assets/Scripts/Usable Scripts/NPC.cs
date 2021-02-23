using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{

	public GameObject CurrentWayPoint;
	public GameObject TargetWayPoint;

	List<GameObject> Flats = new List<GameObject> ();




	public void Start ()
	{
		
	}



	/// <summary>
	/// Selects the direction of the movement randomly then after move to that direction for 5 steps
	/// and if at any step the next block is blocked with any of the object or obstacle then change the path
	/// by selecting the new direction.
	/// </summary>
	/// 

	//TODO: find elements at all the points of the selected point
	void SelectPath ()
	{
		int x = CurrentWayPoint.GetComponent <WayPoint> ().row_number;
		int y = CurrentWayPoint.GetComponent <WayPoint> ().column_number;
		int gridwidth = (int)CurrentWayPoint.transform.parent.GetComponent <ResizeGrid> ().gridWidth;
		int gridheight = (int)CurrentWayPoint.transform.parent.GetComponent <ResizeGrid> ().gridHeight;

		switch (Random.Range (0, 3)) {
		case 0:
			//right;

			if (x + 1 > gridwidth) {
				
			} else {
				int target_x = x + 1;

				TargetWayPoint = CurrentWayPoint.transform.parent.FindChild ("" + target_x + "-" + y).gameObject;
				transform.position = TargetWayPoint.transform.position;
				CurrentWayPoint = TargetWayPoint;
			}

			break;
		case 1:
			//left

			if (x - 1 < 0) {

			} else {
				int target_x = x - 1;

				TargetWayPoint = CurrentWayPoint.transform.parent.FindChild ("" + target_x + "-" + y).gameObject;
				transform.position = TargetWayPoint.transform.position;
				CurrentWayPoint = TargetWayPoint;
			}

			break;
		case 2:
			//up
			if (y + 1 > gridheight) {

			} else {
				int target_y = y + 1;

				TargetWayPoint = CurrentWayPoint.transform.parent.FindChild ("" + x + "-" + target_y).gameObject;
				transform.position = TargetWayPoint.transform.position;
				CurrentWayPoint = TargetWayPoint;
			}
			break;
		case 3:
			//down
			if (y - 1 < 0) {

			} else {
				int target_y = y - 1;

				TargetWayPoint = CurrentWayPoint.transform.parent.FindChild ("" + x + "-" + target_y).gameObject;
				transform.position = TargetWayPoint.transform.position;
				CurrentWayPoint = TargetWayPoint;
			}
			break;
		
		}
	}

	GameObject grid;

	void GridSnapByDistance ()
	{
		GameObject nearestObject = grid.transform.GetChild (0).gameObject;
		float shortestDistance = Vector2.Distance (this.transform.position, nearestObject.transform.position);
		float tempshortdistance = shortestDistance;

		for (int i = 0; i < grid.transform.childCount; i++) {
			tempshortdistance = Vector2.Distance (this.transform.position, grid.transform.GetChild (i).position);

			if (tempshortdistance < shortestDistance) {
				nearestObject = grid.transform.GetChild (i).gameObject;
				shortestDistance = tempshortdistance;
			}
		}

		CurrentWayPoint = nearestObject;
		//	print (nearestObject.name);
		this.transform.position = nearestObject.transform.position;

	}



	void UpdateGrid ()
	{
		var flatarray = GameObject.FindGameObjectsWithTag ("Room");
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject.tag == "Room")
			grid = collider.gameObject.transform.FindChild ("grid Container").gameObject;
		print (grid.transform.parent.name);
		//SnapToGrid (transform.position);
		GridSnapByDistance ();
	}

}
