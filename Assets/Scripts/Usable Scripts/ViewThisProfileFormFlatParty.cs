using UnityEngine;
using System.Collections;

public class ViewThisProfileFormFlatParty : MonoBehaviour
{

	void OnMouseDown ()
	{	
		ShowUserProfile.Instance.WaitingScreenState (true);
		FriendProfileManager.Instance.GetOtherBlockUserProfile ();
		FriendProfileManager.Instance.GetMyProfileBlockedUserList (false);
		FriendsManager.Instance.GetPendingRequestFor_Party ();
		if (ScreenAndPopupCall.Instance.ChatInFlatPartyIsShown)
			ScreenAndPopupCall.Instance.ChatScreenInFlatPartyStatus ();
		Invoke ("ShowThis", 2.5f);
	
	}

	void ShowThis ()
	{
		var thisPlayer = this.gameObject.transform.parent.parent.GetComponent<PlayerNetworkForFlatParty> ();
		int playerId = thisPlayer.PlayerDataForFlatParty.player_id; 
		string username = thisPlayer.PlayerDataForFlatParty.Username;

		FriendProfileManager.Instance.BackToFriendList.SetActive (false);
		FriendProfileManager.Instance.BackToPublicArea.SetActive (false);
		FriendProfileManager.Instance.BackToFlatParty.SetActive (true);
		FriendProfileManager.Instance.BackToSocietyParty.SetActive (false);
		FriendProfileManager.Instance.BackToMyProfile.SetActive (false);
		ScreenManager.Instance.FlatPartyChatScreen.SetActive (false);

		for (int i = 0; i < FriendProfileManager.Instance.BlockedByList.Count; i++) {
			if (FriendProfileManager.Instance.BlockedByList [i].PlayerId == playerId) {
				ShowUserProfile.Instance.ShowThisPlayerData (playerId);
				FriendProfileManager.Instance.seletedPlayerData.Id = playerId;
				FriendProfileManager.Instance.seletedPlayerData.Username = username;
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (true);
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				this.transform.parent.gameObject.transform.localScale = Vector3.zero;
				this.transform.parent.parent.transform.GetComponent<RemoveThisPlayer> ().ObjState = false;
				ShowUserProfile.Instance.WaitingScreenState (false);
				return;
			}
		}

		for (int j = 0; j < FriendProfileManager.Instance.MyBlockList.Count; j++) {
			if (FriendProfileManager.Instance.MyBlockList [j].PlayerId == playerId) {
				ShowUserProfile.Instance.ShowThisPlayerData (playerId);
				FriendProfileManager.Instance.seletedPlayerData.Id = playerId;
				FriendProfileManager.Instance.seletedPlayerData.Username = username;
				FriendProfileManager.Instance.UnblockButton.SetActive (true);
				FriendProfileManager.Instance.BlockButton.SetActive (false);
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				this.transform.parent.gameObject.transform.localScale = Vector3.zero;
				this.transform.parent.parent.transform.GetComponent<RemoveThisPlayer> ().ObjState = false;
				ShowUserProfile.Instance.WaitingScreenState (false);
				return;
			}
		}

		for (int k = 0; k < FriendsManager.Instance.AllAddedFriends.Count; k++) {
			if (FriendsManager.Instance.AllAddedFriends [k].Id == playerId) {
				ShowUserProfile.Instance.ShowThisPlayerData (playerId);
				FriendProfileManager.Instance.seletedPlayerData.Id = playerId;
				FriendProfileManager.Instance.seletedPlayerData.Username = username;
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (true);
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				this.transform.parent.gameObject.transform.localScale = Vector3.zero;
				this.transform.parent.parent.transform.GetComponent<RemoveThisPlayer> ().ObjState = false;
				ShowUserProfile.Instance.WaitingScreenState (false);
				return;
			}
		} 
		for (int a = 0; a < FriendsManager.Instance.PendingRequests.Count; a++) {
			if (FriendsManager.Instance.PendingRequests [a].Id == playerId) {
				ShowUserProfile.Instance.ShowThisPlayerData (playerId);
				FriendProfileManager.Instance.seletedPlayerData.Id = playerId;
				FriendProfileManager.Instance.seletedPlayerData.Username = username;
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (true);
				FriendProfileManager.Instance.PendingRequest.SetActive (true);
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);

				this.transform.parent.gameObject.transform.localScale = Vector3.zero;
				this.transform.parent.parent.transform.GetComponent<OpenViewMenu> ().ObjState = false;
				ShowUserProfile.Instance.WaitingScreenState (false);
				return;
			} 
		}

		ShowUserProfile.Instance.ShowThisPlayerData (playerId);
		FriendProfileManager.Instance.seletedPlayerData.Id = playerId;
		FriendProfileManager.Instance.seletedPlayerData.Username = username;
		FriendProfileManager.Instance.UnblockButton.SetActive (false);
		FriendProfileManager.Instance.BlockButton.SetActive (true);
		FriendProfileManager.Instance.AddFriendButton.SetActive (true);
		FriendProfileManager.Instance.Accept.SetActive (false);
		FriendProfileManager.Instance.Decline.SetActive (false);
		FriendProfileManager.Instance.PendingRequest.SetActive (false);


		this.transform.parent.gameObject.transform.localScale = Vector3.zero;
		this.transform.parent.parent.transform.GetComponent<RemoveThisPlayer> ().ObjState = false;
		ShowUserProfile.Instance.WaitingScreenState (false);

	}

}
