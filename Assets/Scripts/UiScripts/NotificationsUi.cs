using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationsUi : MonoBehaviour {

	public NotificationManager.Notifications Data; 

	void Start () {
		transform.FindChild ("UserName").GetComponent <Text>().text = Data.Title;
		transform.FindChild ("Message").GetComponent <Text>().text = Data.Message;
//		transform.FindChild ("Accept").gameObject.SetActive (false);
//		transform.FindChild ("Decline").gameObject.SetActive (false);
	}
}
