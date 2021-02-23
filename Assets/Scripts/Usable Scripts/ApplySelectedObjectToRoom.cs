using UnityEngine;
using System.Collections;

public class ApplySelectedObjectToRoom : MonoBehaviour
{
	private int tempIndex;

	void OnMouseDown ()
	{
		print (this.gameObject.name);
		if (GroundsUI.ApplyRoomTexture) {	
				
			this.gameObject.transform.parent.GetComponent<Flat3D> ().ChangeGroungColor (RoomPurchaseManager.Instance.SeletedGroundTexture.Texture);
			this.gameObject.transform.parent.GetComponent<Flat3D> ().ChangeChangeGroundTextureName (RoomPurchaseManager.Instance.SeletedGroundTexture.Name);
			GroundsUI.ApplyRoomTexture = false;	
//			var Tut = GameManager.Instance.GetComponent <Tutorial> ();
//			if (!Tut._GroundTextureChanged)
//				Tut.GroundTextureChanging ();
			var Roomname = this.gameObject.transform.parent;

			for (int i = 0; i < RoomPurchaseManager.Instance.Addedflats.Count; i++) {
				if (Roomname.name == RoomPurchaseManager.Instance.Addedflats [i].name) {
					tempIndex = i;
					print (tempIndex);
				}
			}
			RoomPurchaseManager.Instance.SelectedFlatData.position = RoomPurchaseManager.Instance.Addedflats [tempIndex].name;
			StartCoroutine (UpdateFlatData (RoomPurchaseManager.Instance.SelectedFlatData));

			var TempObj = GameObject.FindGameObjectsWithTag ("SelectRoom");
			GameObject[] tempObj = TempObj;
			foreach (GameObject DelObj in tempObj) {
				Destroy (DelObj);
			}
		}
		if (WallsColorUI.ApplyRoomWallTexture) {
//			this.gameObject.transform.parent.GetComponent<Flat3D> ().Walls.ChangeWallColors (RoomPurchaseManager.Instance.SelectedWallTexture.Textures);
			this.gameObject.transform.parent.GetComponent<Flat3D> ().Walls.ChangeWallColorsNew (RoomPurchaseManager.Instance.SelectedWallTexture);
			this.gameObject.transform.parent.GetComponent<Flat3D> ().ChangeChangeWallTextureName (RoomPurchaseManager.Instance.SelectedWallTexture.Name);
			WallsColorUI.ApplyRoomWallTexture = false;

			var Roomname = this.gameObject.transform.parent;
//			var Tut = GameManager.Instance.GetComponent <Tutorial> ();
//			if (!Tut._WallTextureChanged)
//				Tut.WallTextureChanging ();
			for (int i = 0; i < RoomPurchaseManager.Instance.Addedflats.Count; i++) {
				if (Roomname.name == RoomPurchaseManager.Instance.Addedflats [i].name) {
					tempIndex = i;
					print (tempIndex);
				}
			}
			RoomPurchaseManager.Instance.SelectedFlatData.position = RoomPurchaseManager.Instance.Addedflats [tempIndex].name;
			StartCoroutine (UpdateFlatData (RoomPurchaseManager.Instance.SelectedFlatData));

			var TempObj = GameObject.FindGameObjectsWithTag ("SelectRoom");
			GameObject[] tempObj = TempObj;
			foreach (GameObject DelObj in tempObj) {
				Destroy (DelObj);
			}
		}

	}

	IEnumerator UpdateFlatData (FlatUpdateData updateData)
	{
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.UpdateFlat (updateData));
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			print ("Success");
		}
	}
			

}
