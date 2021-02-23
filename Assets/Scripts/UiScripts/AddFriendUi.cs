using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class AddFriendUi : MonoBehaviour
{

	public Button AddButton;
	public FriendData thisData;
	public string ViewFriendInString;

	public Sprite PinkBg;
	public Sprite WhiteBg;


	void Start ()
	{
		if (!FriendProfileManager.Instance.ShowBlockList)
			AddButton.onClick.RemoveAllListeners ();
		transform.GetChild (0).GetComponent <Text> ().text = "Name : " + thisData.Username;
//		transform.GetChild (1).GetComponent <Text> ().text = "Id : " + thisData.Id;

		if (thisData.Type == FriendData.FriendType.Unknown) {
//			AddButton.GetComponentInChildren <Text> ().text = "Add";
			//Changed by rehan
			AddButton.GetComponentInChildren <Text> ().text = "Profile";
			AddButton.gameObject.SetActive (true);
			AddButton.onClick.AddListener (() => { 
//				AddButton.interactable = false;
				ShowUserProfile.Instance.WaitingScreenState (true);
				FriendProfileManager.Instance.seletedPlayerData = thisData;
				FriendProfileManager.Instance.AddFriendButton.SetActive (true);
				FriendProfileManager.Instance.BlockButton.SetActive (true);
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				ShowUserProfile.Instance.ShowThisPlayerData (thisData.Id); 
//				FriendsManager.Instance.SendFreindRequestToUser (gameObject, thisData.Username, thisData.Id);
			});
		} else if (thisData.Type == FriendData.FriendType.Request) {
			AddButton.GetComponentInChildren <Text> ().text = "Profile";
			AddButton.onClick.AddListener (() => {
				ShowUserProfile.Instance.WaitingScreenState (true);
				FriendProfileManager.Instance.seletedPlayerData = thisData;
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (false);
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (true);
				FriendProfileManager.Instance.Decline.SetActive (true);
				ShowUserProfile.Instance.ShowThisPlayerData (thisData.Id); 
//				OnClickView ();
			});
			AddButton.gameObject.SetActive (true);
		} else if (thisData.Type == FriendData.FriendType.Friend) {
			AddButton.GetComponentInChildren <Text> ().text = "Profile";
			AddButton.onClick.AddListener (() => {
				ShowUserProfile.Instance.WaitingScreenState (true);
				FriendProfileManager.Instance.seletedPlayerData = thisData;
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (true);
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				ShowUserProfile.Instance.ShowThisPlayerData (thisData.Id); 

			});
			AddButton.gameObject.SetActive (true);
		} else if (thisData.Type == FriendData.FriendType.Invite) {
			AddButton.gameObject.SetActive (true);
			AddButton.GetComponentInChildren <Text> ().text = "Invite";
			AddButton.onClick.AddListener (() => OnClickInvite ());

			//			AddButton.onClick.AddListener (() => FriendsManager.Instance.SendCoOpRequestToUser (gameObject, thisData.Username, thisData.Id, thisData.Username));
		} else if (thisData.Type == FriendData.FriendType.ViewFriend) {
			AddButton.gameObject.SetActive (true);
			AddButton.GetComponentInChildren <Text> ().text = "Profile";
			AddButton.onClick.AddListener (() => OnClickViewFriend ());
		} else if (thisData.Type == FriendData.FriendType.Pending) {
			AddButton.gameObject.SetActive (true);
			AddButton.GetComponentInChildren <Text> ().text = "Profile";
			AddButton.onClick.AddListener (() => {
				ShowUserProfile.Instance.WaitingScreenState (true);
				FriendProfileManager.Instance.seletedPlayerData = thisData;
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (true);
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (true);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				ShowUserProfile.Instance.ShowThisPlayerData (thisData.Id);

			});
		} else if (thisData.Type == FriendData.FriendType.SocietyJoinCall) {
			AddButton.gameObject.SetActive (true);
			AddButton.GetComponentInChildren <Text> ().text = "Add";
			AddButton.onClick.AddListener (() => {
//				SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.ISendNotificationsToAuth (thisData.Id));
				string message = string.Format ("Hey! {0} want you to join his society - {1}", PlayerPrefs.GetString ("UserName"), SocietyManager.Instance.SelectedSociety.Name);
				PushScript.Instance.SendPushToUser (thisData.Username, message);
				NotificationManager.Instance.SendInvitationToUser (thisData.Id, message, SocietyManager.Instance.SelectedSociety.Id);
			});
		}
		//this changes is made by Rehan for maintain the block list
		else if (thisData.Type == FriendData.FriendType.Blocked) {
			AddButton.gameObject.SetActive (true);
			AddButton.GetComponentInChildren <Text> ().text = "Profile";
			AddButton.onClick.AddListener (() => {
				ShowUserProfile.Instance.WaitingScreenState (true);
				FriendProfileManager.Instance.seletedPlayerData = thisData;
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (false);
				FriendProfileManager.Instance.UnblockButton.SetActive (true);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				ShowUserProfile.Instance.ShowThisPlayerData (thisData.Id);

			});
		} else if (thisData.Type == FriendData.FriendType.BlockedBy) {
			AddButton.gameObject.SetActive (true);
			AddButton.GetComponentInChildren <Text> ().text = "Profile";
			AddButton.onClick.AddListener (() => {
				ShowUserProfile.Instance.WaitingScreenState (true);
				FriendProfileManager.Instance.seletedPlayerData = thisData;
				FriendProfileManager.Instance.AddFriendButton.SetActive (false);
				FriendProfileManager.Instance.BlockButton.SetActive (true);
				FriendProfileManager.Instance.UnblockButton.SetActive (false);
				FriendProfileManager.Instance.PendingRequest.SetActive (false);
				FriendProfileManager.Instance.Accept.SetActive (false);
				FriendProfileManager.Instance.Decline.SetActive (false);
				ShowUserProfile.Instance.ShowThisPlayerData (thisData.Id);
			
			});
		}

	}

	void OnClickView ()
	{
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Accept";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Decline";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = thisData.Username + ":-  " + thisData.Message;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			FriendsManager.Instance.AllAddedFriends.Add (thisData);
			FriendsManager.Instance.AcceptRequest (thisData.Username, thisData.Id, gameObject);
		});	

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => FriendsManager.Instance.RejectRequest (thisData.Username, thisData.Id, gameObject));
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	}


	void OnClickInvite ()
	{		
		MultiplayerManager.Instance.FriendId = thisData.Id;
		MultiplayerManager.Instance.JoinorCreateRoomForCoOp ("CoOpEventRoom_" + PlayerPrefs.GetString ("UserName") + "_" + thisData.Username);
	}

	void OnClickViewFriend ()
	{
		if (ViewFriendInString.Contains ("Decor")) {
			foreach (var pair in VotingPairManager.Instance.FriendsInDecorEvent) {
				if (thisData.Id == pair.player1_id || thisData.Id == pair.player2_id) {
					VotingPairManager.Instance.ShowVotingPair (pair);
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenManager.Instance.ClosePopup ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();

					if (VotingPairManager.Instance.FriendsInDecorEvent.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}					
			}
		} else if (ViewFriendInString.Contains ("Fashion")) {
			foreach (var pair in VotingPairManager.Instance.FriendsInFashionEvent) {
				if (thisData.Id == pair.player1_id || thisData.Id == pair.player2_id) {
                    StartCoroutine(VotingPairManager.Instance.ShowOnePairOnScreen (pair));				
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenManager.Instance.ClosePopup ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();

					if (VotingPairManager.Instance.FriendsInFashionEvent.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}				
			}
		} else if (ViewFriendInString.Contains ("CatWalk")) {
			foreach (var pair in VotingPairManager.Instance.FriendsInCatwalkEvent) {
				if (thisData.Id == pair.player1_id || thisData.Id == pair.player2_id) {
                    StartCoroutine(VotingPairManager.Instance.ShowCatwalkPair (pair));
					ScreenAndPopupCall.Instance.CloseScreen ();			
					ScreenManager.Instance.ClosePopup ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();

					if (VotingPairManager.Instance.FriendsInCatwalkEvent.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}					
			}
		} else if (ViewFriendInString.Contains ("CoOp")) {
			foreach (var pair in VotingPairManager.Instance.FriendsInCoOp) {
				if (thisData.Id == pair.set1_player1_id || thisData.Id == pair.set1_player2_id || thisData.Id == pair.set2_player1_id || thisData.Id == pair.set2_player2_id) {
                    StartCoroutine(VotingPairManager.Instance.ShowPairCoOp (pair));
					ScreenAndPopupCall.Instance.CloseScreen ();			
					ScreenManager.Instance.ClosePopup ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();

					if (VotingPairManager.Instance.FriendsInCoOp.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}					
			}
		} else if (ViewFriendInString.Contains ("Society")) {
			foreach (var pair in VotingPairManager.Instance.FriendsInSociety) {
				if (thisData.Id == pair.player1_id || thisData.Id == pair.player2_id) {
                    StartCoroutine(VotingPairManager.Instance.ShowPairSocietyEvent (pair));
					ScreenAndPopupCall.Instance.CloseScreen ();			
					ScreenManager.Instance.ClosePopup ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();

					if (VotingPairManager.Instance.FriendsInSociety.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}					
			}
		}
	}

	public void AddThisAsFriend ()
	{
		//		if (thisData.Type == FriendData.FriendType.Unknown)
		//			FriendsManager.Instance.SendFreindRequestToUser (thisData.Id);
		//		else if (thisData.Type == FriendData.FriendType.Request)
		//			FriendsManager.Instance.AcceptRequest (thisData.Id);
		//		else if(thisData.Type == FriendData.FriendType.Friend)
		//		{
		//			
		//		}
		//		else if(thisData.Type == FriendData.FriendType.Rejected)
		//		{
		//			
		//		}
	}
}


[Serializable]
public class FriendData :IComparable<FriendData>
{
	public string Username;
	public int Id;
	public int Status;
	public string Message;
	public FriendType Type;

	public enum FriendType
	{
		Unknown,
		Request,
		Friend,
		Invite,
		ViewFriend,
		Pending,
		SocietyJoinCall,
		Blocked,
		BlockedBy
	}

	public int CompareTo (FriendData other)
	{
		return this.Username.CompareTo (other.Username);
	}
}