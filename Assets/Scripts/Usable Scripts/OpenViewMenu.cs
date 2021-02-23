using UnityEngine;
using System.Collections;

public class OpenViewMenu : MonoBehaviour
{
	public bool ObjState;
	// Use this for initialization


	void OnMouseDown ()
	{
        if (this.gameObject.GetComponent<PublicAreaPlayer> ().PlayerData.player_id != 0) {
			ObjState = !ObjState;
			if (ObjState)
				this.transform.FindChild ("MultiPlayer Option").gameObject.transform.localScale = new Vector3 (4.5f, 4.5f, 4.5f);
			else
				this.transform.FindChild ("MultiPlayer Option").gameObject.transform.localScale = Vector3.zero;
			print (ObjState);
		}
	}
}
