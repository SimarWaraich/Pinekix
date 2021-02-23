using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour
{


	public int row_number, column_number;

	public bool _CanBeUsed = true;

	public GameObject upward, downward, right, left;


	public void UpdateGameObjects ()
	{
		int upward_count = row_number + 1;
		int down_count = row_number - 1;
		int right_count = column_number + 1;
		int left_count = column_number - 1;

		if (transform.parent.FindChild (row_number.ToString () + "_" + right_count.ToString ()))
			upward = transform.parent.FindChild ("" + row_number + "_" + right_count).gameObject;

		if (transform.parent.FindChild ( row_number.ToString () + "_" + left_count.ToString ()))
			downward = transform.parent.FindChild ("" + row_number + "_" + left_count).gameObject;

		if (transform.parent.FindChild (upward_count.ToString () + "_" + column_number.ToString ()))
			right = transform.parent.FindChild ("" + upward_count + "_" + column_number).gameObject;
	
		if (transform.parent.FindChild (down_count.ToString () + "_" + column_number.ToString ()))
			left = transform.parent.FindChild ("" + down_count + "_" + column_number).gameObject;


		if (upward == null) {

			var target_x = transform.parent.parent.GetComponent<Flat3D> ().data.x;
			var target_y = transform.parent.parent.GetComponent<Flat3D> ().data.y + 1;

			if (transform.root.FindChild ("" + target_x + "-" + target_y)) {
				var hit = Physics2D.Raycast (transform.position, Vector2.up);
				if (hit.collider.name == "" + target_x + "-" + target_y) {
					upward = hit.collider.gameObject.transform.FindChild ("grid Container").FindChild ("" + row_number + "_0").gameObject;
				}
			}
		}


		if (downward == null) {

			var target_x = transform.parent.parent.GetComponent<Flat3D> ().data.x;
			var target_y = transform.parent.parent.GetComponent<Flat3D> ().data.y - 1;

			if (transform.root.FindChild ("" + target_x + "-" + target_y)) {
				var hit = Physics2D.Raycast (transform.position, Vector2.up);
				if (hit.collider.name == "" + target_x + "-" + target_y) {
					upward = hit.collider.gameObject.transform.FindChild ("grid Container").FindChild ("" + row_number + "-" + (transform.root.FindChild ("" + target_x + "-" + target_y).GetComponent<ResizeGrid> ().gridHeight - 1)).gameObject;
				}
			}
		}


		if (right == null) {

			var target_x = transform.parent.parent.GetComponent<Flat3D> ().data.x + 1;
			var target_y = transform.parent.parent.GetComponent<Flat3D> ().data.y;

			if (transform.root.FindChild ("" + target_x + "-" + target_y)) {
				var hit = Physics2D.Raycast (transform.position, Vector2.up);
				if (hit.collider.name == "" + target_x + "-" + target_y) {
					upward = hit.collider.gameObject.transform.FindChild ("grid Container").FindChild ("0_" + column_number).gameObject;
				}
			}
		}


		if (left == null) {

			var target_x = transform.parent.parent.GetComponent<Flat3D> ().data.x - 1;
			var target_y = transform.parent.parent.GetComponent<Flat3D> ().data.y;

			if (transform.root.FindChild ("" + target_x + "-" + target_y)) {
				var hit = Physics2D.Raycast (transform.position, Vector2.up);
				if (hit.collider.name == "" + target_x + "-" + target_y) {
					upward = hit.collider.gameObject.transform.FindChild ("grid Container").FindChild ("" + (transform.root.FindChild ("" + target_x + "-" + target_y).GetComponent<ResizeGrid> ().gridWidth - 1) + "-" + column_number).gameObject;
				}
			}
		}
	}
}
