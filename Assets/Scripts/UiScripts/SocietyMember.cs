using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Simple_JSON;

public class SocietyMember : MonoBehaviour
{

	public SocietyManager.SocietyMembers _thisMember;

	PlayerRole _role;
	public Text RoleText;

	public void Start ()
	{
		switch (_thisMember.role_id) {
		case 0:
		default:
			_role = PlayerRole.President;

			switch (SocietyManager.Instance.myRole) {
			case 0:
				transform.GetChild (2).gameObject.SetActive (true);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (false);
				break;
			case 1:
			case 2:
			case 3:
			default:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (false);
				break;
			}
			RoleText.text = "P"; // simar changes
			break;
		case 1:
			_role = PlayerRole.committee_member;
			RoleText.text = "C";// simar changes
			
		
			switch (SocietyManager.Instance.myRole) {
			case 0:
				transform.GetChild (2).gameObject.SetActive (true);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (true);
				break;
			case 1:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (false);
				if (PlayerPrefs.GetInt ("PlayerId") == _thisMember.player_id)
					transform.GetChild (2).gameObject.SetActive (true);
				break;
			case 2:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (false);
				break;
			case 3:
			default:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (false);
				break;
			}

			break;
		case 2:
			_role = PlayerRole.normal_member;

			RoleText.text = "M";// simar changes
			switch (SocietyManager.Instance.myRole) {
			case 0:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (true);
				transform.GetChild (4).gameObject.SetActive (true);
				break;
			case 1:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (true);
				transform.GetChild (4).gameObject.SetActive (true);
				break;
			case 2:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (false);
				if (PlayerPrefs.GetInt ("PlayerId") == _thisMember.player_id)
					transform.GetChild (4).gameObject.SetActive (true);
				break;
			case 3:
			default:
				transform.GetChild (2).gameObject.SetActive (false);
				transform.GetChild (3).gameObject.SetActive (false);
				transform.GetChild (4).gameObject.SetActive (false);
				break;
			}

			break;
		case 3:
			_role = PlayerRole.visitor;
			break;
		}

		transform.GetChild (5).GetComponent <Text> ().text = _thisMember.player_name;
		transform.GetChild (6).GetComponent <Text> ().text = "User Level: " +_thisMember.level.ToString ();


		transform.GetChild (2).GetComponent <Button> ().onClick.AddListener (() => Demote ());
		transform.GetChild (3).GetComponent <Button> ().onClick.AddListener (() => Promote ());
		transform.GetChild (4).GetComponent <Button> ().onClick.AddListener (() => Remove ());


		if (SocietyManager.Instance._allMembers.Count <= 1) {
			transform.GetChild (2).gameObject.SetActive (false);
			transform.GetChild (3).gameObject.SetActive (false);
			transform.GetChild (4).gameObject.SetActive (false);
		}


	}

	public void Promote ()
	{
		int committeeCount = 0;

		foreach (var member in SocietyManager.Instance._allMembers) {
			if (member.role_id == 1)
				committeeCount++;
		}

		if (committeeCount < 10)
            SocietyManager.Instance.ShowPopUp ("Do you want to promote " + _thisMember.player_name, () => StartCoroutine (IePromote ()), ()=>SocietyDescriptionController.Instance.SocietyMemberList (true));
		else if (committeeCount >= 10) {
			SocietyManager.Instance.ShowPopUp ("You have reached the maximum number of committee members", ()=>SocietyDescriptionController.Instance.SocietyMemberList (true));
		}
			
	}

	IEnumerator IePromote ()
	{
		int presidentId = 0;
		foreach (var _member in SocietyManager.Instance._allMembers) {
			if (_member.role_id == 0)
				presidentId = _member.player_id;
		}

		var cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.IeUpdateRole (_thisMember.role_id - 1, _thisMember.player_id, _thisMember.level));
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
			NotificationManager.Instance.SendNotificationToUser (_thisMember.player_id, string.Format ("You have been promoted in \"{0}\" society", SocietyManager.Instance.SelectedSociety.Name));
			NotificationManager.Instance.SendNotificationToUser (presidentId, string.Format (_thisMember.player_name + " has been promoted in \"{0}\" society", SocietyManager.Instance.SelectedSociety.Name));
		} else {
			SocietyManager.Instance.ShowPopUp ("Failed to promote the player. Please try again.", () => SocietyDescriptionController.Instance.SocietyMemberList (true));
		}

		yield return null;


	}


	IEnumerator IeDemote ()
	{
		int presidentId = 0;
		foreach (var _member in SocietyManager.Instance._allMembers) {
			if (_member.role_id == 0)
				presidentId = _member.player_id;
		}
		var cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.IeUpdateRole (_thisMember.role_id + 1, _thisMember.player_id, _thisMember.level));
		yield return cd.coroutine;

		if (cd.result.ToString () == "true" || cd.result.ToString () == "True") {
			NotificationManager.Instance.SendNotificationToUser (_thisMember.player_id, string.Format ("You have been demoted in \"{0}\" society", SocietyManager.Instance.SelectedSociety.Name));
			NotificationManager.Instance.SendNotificationToUser (presidentId, string.Format (_thisMember.player_name + " has been demoted in \"{0}\" society", SocietyManager.Instance.SelectedSociety.Name));
		} else {
			SocietyManager.Instance.ShowPopUp ("Failed to demote the player. Please try again.", () => SocietyDescriptionController.Instance.SocietyMemberList (true));
		}
		yield return null;
	}

	public void Demote ()
	{
		switch (_thisMember.role_id) {
		case 0:
			if (_thisMember.player_id == PlayerPrefs.GetInt ("PlayerId") && SocietyManager.Instance.myRole == 0)
				SocietyManager.Instance.ShowPopUp ("Do you really want to demote yourself? If yes, then please select new President.", () => SelectPresident (), () => SocietyDescriptionController.Instance.SocietyMemberList (true));
			break;
		case 1:
			if (_thisMember.player_id == PlayerPrefs.GetInt ("PlayerId"))
				SocietyManager.Instance.ShowPopUp ("Do you really want to demote yourself?", () => StartCoroutine(IeDemote ()), () => SocietyDescriptionController.Instance.SocietyMemberList (true));
			else if (SocietyManager.Instance.myRole == 0)
				SocietyManager.Instance.ShowPopUp ("Do you really want to demote " + _thisMember.player_name , () => StartCoroutine(IeDemote ()), () => SocietyDescriptionController.Instance.SocietyMemberList (true));
			break;
		case 2:
		case 3:
		default:
			break;

		}

	}

	public void Remove ()
	{
		switch (_thisMember.role_id) {
		case 0:
			if (_thisMember.player_id == PlayerPrefs.GetInt ("PlayerId") && SocietyManager.Instance.myRole == 0)
					SocietyManager.Instance.ShowPopUp ("Do you really want to demote yourself? If yes, then please select new President.", () => SelectPresident (), () => SocietyDescriptionController.Instance.SocietyMemberList (true));
					break;
		case 1:
			if (_thisMember.player_id == PlayerPrefs.GetInt ("PlayerId"))		
				SocietyManager.Instance.ShowPopUp ("Do you really want to remove yourself?", () => StartCoroutine(SocietyManager.Instance.IeDeletePlayer (_thisMember.player_id)), () => SocietyDescriptionController.Instance.SocietyMemberList (true));

//				SocietyManager.Instance.ShowPopUp ("Do you really want to demote yourself?", () => StartCoroutine(IeDemote ()), () => SocietyDescriptionController.Instance.SocietyMemberList (true)); // changed Because Committe member can remove himself now....
			else if (SocietyManager.Instance.myRole == 0)
				SocietyManager.Instance.ShowPopUp ("Do you really want to remove " + _thisMember.player_name , () => SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.IeDeletePlayer (_thisMember.player_id)), () => SocietyDescriptionController.Instance.SocietyMemberList (true));

			break;
		case 2:
			if (_thisMember.player_id == PlayerPrefs.GetInt ("PlayerId"))
				SocietyManager.Instance.ShowPopUp ("Do you really want to remove yourself?", () => SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.IeDeletePlayer (_thisMember.player_id)), () => SocietyDescriptionController.Instance.SocietyMemberList (true));
			else if (SocietyManager.Instance.myRole == 0 || SocietyManager.Instance.myRole == 1)
				SocietyManager.Instance.ShowPopUp ("Do you really want to remove " + _thisMember.player_name, () => SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.IeDeletePlayer (_thisMember.player_id)), () => SocietyDescriptionController.Instance.SocietyMemberList (true));
			break;

		case 3:
		default:
			break;
		}
	}


	//	public void ShowPopupofCommitteeMember ()
	//	{
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.ClosePopup ();
	//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	//
	//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Demote";
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Cancel";
	//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Do you really want demote?";
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (IeDemote ()));
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//	}
	//
	//
	//	public void ShowPopupofMemberDemote ()
	//	{
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.ClosePopup ();
	//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	//
	//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Demote";
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Cancel";
	//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Do you really want to demote the player?";
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (IeDemote ()));
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//	}
	//
	//
	//
	//	public void ShowPopupofPresident ()
	//	{
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.ClosePopup ();
	//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	//
	//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Demote";
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Cancel";
	//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Do you really want to demote from the president?";
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => SelectPresident ());
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//	}



	public void SelectPresident ()
	{
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.NewPresidentPopup);

		for(int i = 0; i < SocietyManager.Instance.NewPresidentContainer.transform.childCount; i++)
		{
			Destroy (SocietyManager.Instance.NewPresidentContainer.transform.GetChild (i).gameObject);
		}

		foreach (var list in SocietyManager.Instance._allMembers) {
			if (list.role_id == 1) {
				var go = Instantiate (SocietyManager.Instance.SocietyFriendPrefab, Vector3.one, Quaternion.identity) as GameObject;
				go.transform.SetParent (SocietyManager.Instance.NewPresidentContainer.transform);
				go.transform.localScale = Vector3.one;
				go.transform.GetChild (0).GetComponent <Text> ().text = list.player_name;
				go.transform.GetChild (1).GetComponent <Text> ().text = "" + list.level.ToString ();
				go.name = list.player_id.ToString ();
				go.transform.GetChild (2).GetComponent <Button> ().onClick.AddListener (() => StartCoroutine (ChangePresident (go)));
			}
		}
	}

	public IEnumerator ChangePresident (GameObject go)
	{
		

		CoroutineWithData cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.IeSwapPresident (go.name));
		yield return cd.coroutine;

		if(cd.result.ToString () == "true" || cd.result.ToString () == "True")
		{
			SocietyManager.Instance.ShowPopUp ("President changed successfully.", () => {
				SocietyDescriptionController.Instance.SocietyMemberList (true);
			});

			yield return SocietyManager.Instance.IGetAllPlayers (SocietyManager.Instance.SelectedSociety.Id);
			SocietyManager.Instance._allMembers.Sort ();
			foreach (var obj in SocietyManager.Instance._allMembers) {
				yield return NotificationManager.Instance.StartCoroutine(NotificationManager.Instance.ISendNotificationtoUser (obj.player_id, 
					string.Format ("New president of society \"{0}\" is \"{1}\"", SocietyManager.Instance.SelectedSociety.Name, SocietyManager.Instance._allMembers[0].player_name)));
			}
		}

		else
		{
			SocietyManager.Instance.ShowPopUp ("Failed to change the president. Please try again later.", () => SocietyDescriptionController.Instance.SocietyMemberList (true));
		}


	}


	//
	//	public void ShowPopupofLeaveSociety ()
	//	{
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.ClosePopup ();
	//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	//
	//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Leave";
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Cancel";
	//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Do you really want to Leave the Society?";
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => SocietyManager.Instance.RemovePlayer (PlayerPrefs.GetInt ("PlayerId")));
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//	}
	//
	//
	//	public void ShowPopupofRemovePlayer ()
	//	{
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.ClosePopup ();
	//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	//
	//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Leave";
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Cancel";
	//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Do you Really want to remove the Player?";
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => SocietyManager.Instance.RemovePlayer (_thisMember.player_id));
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//	}
	//


}
