using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SocietyInvitationsUi : MonoBehaviour {
	public SocietyInvitations Data; 

	// Use this for initialization
	void Start () {
		transform.FindChild ("UserName").GetComponent <Text>().text = Data.Title;
		transform.FindChild ("Message").GetComponent <Text>().text = Data.Message;
		transform.FindChild ("Accept").gameObject.SetActive (true);
		transform.FindChild ("Decline").gameObject.SetActive (true);

		transform.FindChild ("Accept").GetComponent <Button>().onClick.AddListener (()=> OnClickAccept());
		transform.FindChild ("Decline").GetComponent <Button>().onClick.AddListener (()=> OnClickDecline());
	}
		

	void OnClickAccept () 
	{
		StartCoroutine (IeAccept ());
	}

	IEnumerator IeAccept()
	{
		var cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.IeJoinSociety(Data.SenderId, Data.SocietyId, 0, null));
		// id is sent null because this id is used for other Invitations other than society. and gameObect is null to differentiate between society/ other invitations.
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
			yield return SocietyManager.Instance.DeleteSocietyInvitations (Data.InvitationId);
			Destroy (gameObject);
		}
	}

	void OnClickDecline () 
	{		
		StartCoroutine (IeDecline ());
	}

	IEnumerator IeDecline()
	{
		var cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.DeleteSocietyInvitations (Data.InvitationId));
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
			NotificationManager.Instance.ShowPopUp ("Invitation declined successfully",null);
			NotificationManager.Instance.SendNotificationToUser (Data.SenderId, string.Format ("Your request to join the \"{0}\" society has been declined.",SocietyManager.Instance.SelectedSociety.Name)); // send notification to presidnet/ commitee member
			Destroy (gameObject);
		}
	}
}
