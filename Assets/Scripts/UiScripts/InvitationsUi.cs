using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InvitationsUi : MonoBehaviour {
	public NotificationManager.Invitations Data; 

	// Use this for initialization
	void Start () {
		transform.FindChild ("UserName").GetComponent <Text>().text = Data.Title;
		transform.FindChild ("Message").GetComponent <Text>().text = Data.Message;
		transform.FindChild ("Accept").gameObject.SetActive (true);
		transform.FindChild ("Decline").gameObject.SetActive (true);

		transform.FindChild ("Accept").GetComponent <Button>().onClick.AddListener (()=> OnClickAccept());
		transform.FindChild ("Decline").GetComponent <Button>().onClick.AddListener (()=> OnClickDecline());
	}


	void OnClickInvitation () {

	}

	void OnClickAccept () {

		if(Data.Title.Contains ("Society"))
		{
//			if(Data.Message.Contains ("wants to join your society"))// Non Member's request
//				SocietyManager.Instance.AddMemberToSociety (Data.SenderId,Data.EventId, Data.Id, gameObject);
//			else
				if(Data.Message.Contains ("want you to join his society"))// President's request
				SocietyManager.Instance.AddMemberToSociety (PlayerPrefs.GetInt ("PlayerId"),Data.EventId, Data.Id, gameObject);
		}
		else if(Data.Title.Contains ("CoOp"))
		{
			NotificationManager.Instance.lastIvitationClicked_Id = Data.Id;
			MultiplayerManager.Instance._isReciever = true;
			MultiplayerManager.Instance.RoomName = Data.RoomName;
			MultiplayerManager.Instance.CoOpEventId = Data.EventId;
			MultiplayerManager.Instance.ConnectToServerforCoOp ();	
			ScreenManager.Instance.ClosePopup ();	
		}	
	}

	void OnClickDecline () 
	{		
		StartCoroutine (IeDecline ());
	}

	IEnumerator IeDecline()
	{
		var cd = new CoroutineWithData (NotificationManager.Instance, NotificationManager.Instance.IDeleteInvitations (Data.Id, null));
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
			NotificationManager.Instance.ShowPopUp ("Invitation declined successfully",null);
//			if(Data.Message.Contains ("wants to join your society"))// Non Member's request
//			{
//				NotificationManager.Instance.SendNotificationToUser (Data.SenderId, "Your request to join society has been declined"); // send notification to presidnet/ commitee member
//			}
//			else
			if(Data.Title.Contains ("Society")){
				if(Data.Message.Contains ("want you to join his society"))// President's request
				{
					NotificationManager.Instance.SendNotificationToUser (Data.SenderId, string.Format ("{0} has declined your request to join your society", 
						PlayerPrefs.GetString ("UserName")));// send notification to Non Member
				}
			}
			else if(Data.Title.Contains ("CoOp"))
			{
				NotificationManager.Instance.SendNotificationToUser (Data.SenderId, string.Format ("{0} has declined your request to join you in CoOp Event",
					PlayerPrefs.GetString ("UserName")/*, EventManagment.Instance.CurrentEvent.EventName*/));
			}

			Destroy (gameObject);
		}
	}
}
