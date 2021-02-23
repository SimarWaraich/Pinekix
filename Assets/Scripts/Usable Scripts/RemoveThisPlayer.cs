using UnityEngine;
using System.Collections;

public class RemoveThisPlayer : MonoBehaviour
{
	public bool ObjState;
	public int id;
	// Use this for initialization
	void Start ()
	{
		id = this.gameObject.GetComponent<PhotonView> ().ownerId;	
	}

	void OnMouseDown ()
	{
		ObjState = !ObjState;
		if (ObjState) {
			this.transform.FindChild ("MultiPlayer Option").gameObject.transform.localScale = new Vector3 (4.5f, 4.5f, 4.5f);
			if (id != PhotonNetwork.masterClient.ID) {
				if (this.gameObject.GetComponent<PlayerNetworkForFlatParty> ().PlayerDataForFlatParty.player_id != 0)
					this.transform.FindChild ("MultiPlayer Option").gameObject.transform.GetChild (1).gameObject.SetActive (true);				
			} else
				this.transform.FindChild ("MultiPlayer Option").gameObject.transform.GetChild (1).gameObject.SetActive (false);	
				
			if (this.gameObject.GetComponent<PlayerNetworkForFlatParty> ().PlayerDataForFlatParty.player_id != 0) {
				this.transform.FindChild ("MultiPlayer Option").gameObject.transform.GetChild (0).gameObject.SetActive (true);
			} else {
				this.transform.FindChild ("MultiPlayer Option").gameObject.transform.GetChild (0).gameObject.SetActive (false);
			}
		} else
			this.transform.FindChild ("MultiPlayer Option").gameObject.transform.localScale = Vector3.zero;
		print (ObjState);
	}
}
