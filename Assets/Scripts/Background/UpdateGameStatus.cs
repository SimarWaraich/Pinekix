/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 07 July 2k16
/// This script will be used to update the status of the game on the server
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UpdateGameStatus : Singleton<UpdateGameStatus>
{
	void Awake ()
	{
		this.Reload ();
	}

	public void UpdateStatus (string link, string datatype, object data)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW (link, encoding.GetBytes (json), postHeader);
	}


	IEnumerator WaitForReply (WWW www)
	{
		yield return www;

		if (www.error != null) {
			print ("error while updating: " + www.error);
		} else {
			print ("updated successfully");
		}
	}
}

