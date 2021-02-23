using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Simple_JSON;

public class KickOutThisPlayer : MonoBehaviour
{
	public const string KickOutPlayerData = "http://pinekix.ignivastaging.com/flats/updateKickedPlayer";
	public int id;
	public int masterId;
	// Use this for initialization
	void Start ()
	{
		id = this.gameObject.transform.parent.transform.parent.GetComponent<PhotonView> ().ownerId;	
	}

	void OnMouseDown ()
	{	
		if (HostPartyManager.Instance.selectedFlatParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) {	
			if (id != PhotonNetwork.masterClient.ID) {
				ShowLessAmmountMesg ("Are you sure you want to kick out " + this.transform.parent.parent.GetComponent<PlayerNetworkForFlatParty> ().PlayerDataForFlatParty.Username + " ?", "No", true);	
			} else
				ShowLessAmmountMesg ("You cannot kick out yourself.", "Ok", false);

		} else {
			print ("you are not the owner! You can not remove or kick off any player");
			this.transform.parent.gameObject.transform.localScale = Vector3.zero;
			this.transform.parent.parent.GetComponent<RemoveThisPlayer> ().ObjState = false;
			FlatPartyHostingControler.Instance.ShowLessAmmountMesg ("You are not admin of " + HostPartyManager.Instance.selectedFlatParty.Name + " flat party!");
		}

	}

	public void ShowLessAmmountMesg (string msg, string butonName, bool active)
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (active);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = butonName;
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			StartCoroutine (KickOutPlayer (id));
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			this.transform.parent.gameObject.transform.localScale = Vector3.zero;
			this.transform.parent.parent.GetComponent<RemoveThisPlayer> ().ObjState = false;
			ScreenManager.Instance.ClosePopup ();
		});
	}

	public IEnumerator KickOutPlayer (int Id)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
		int thisPlayerId = this.transform.parent.parent.GetComponent<PlayerNetworkForFlatParty> ().PlayerDataForFlatParty.player_id;

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["party_id"] = HostPartyManager.Instance.selectedFlatParty.Id.ToString ();
		jsonElement ["party_type"] = "1";
		jsonElement ["kicked_out_player_id"] = thisPlayerId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (KickOutPlayerData, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Player has kicked out successfully")) {
				PhotonNetwork.CloseConnection (PhotonPlayer.Find (Id));
				this.transform.parent.gameObject.transform.localScale = Vector3.zero;
				this.transform.parent.parent.GetComponent<RemoveThisPlayer> ().ObjState = false;
				ScreenManager.Instance.ClosePopup ();
			} 
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("This party has not available")) {
				this.transform.parent.gameObject.transform.localScale = Vector3.zero;
				this.transform.parent.parent.GetComponent<RemoveThisPlayer> ().ObjState = false;
				ScreenManager.Instance.ClosePopup ();
			}
		}

	}

}
