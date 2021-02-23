
/// <summary>
/// Show user profile. script is created by Rehan  on 24/05/2017
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Simple_JSON;

public class ShowUserProfile : Singleton <ShowUserProfile>
{
	public const string ShowProfileLink = "http://pinekix.ignivastaging.com/players/players_profile";
	public const string GetCurruntSocietyName = "http://pinekix.ignivastaging.com/societies/society";


	public GameObject WaitingScreen;
	public Text PlayerName;
	public InputField StatusField;
	public Text UpdatedStatus;
	public Text CurruntSocietyName;
	public Text VIPSubscribed;
	public Image MedalIcon;
	public Text CurrentGameLevel;
	public string PlayerGender;
	public string AchievementName;
	DateTime VipEndTime;
	[Header ("Player Prefab With Gender")]
	public GameObject[] PlayerType;


	void Awake ()
	{
		this.Reload ();
	}

	public void ShowMyProfile ()
	{
		ShowThisPlayerData (PlayerPrefs.GetInt ("PlayerId"));		
	}

	public void ShowThisPlayerData (int thisPlayerId)
	{
		StartCoroutine (ShowFriendProfile (thisPlayerId));

	}

	IEnumerator ShowFriendProfile (int thisPlayerId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = thisPlayerId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (ShowProfileLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Player profile data")) {
				var playerData = _jsnode ["data"];
				int playerId = 0;
				int.TryParse (playerData ["id"], out playerId);
				if (_jsnode ["data"] ["id"].ToString ().Trim ('\"') == PlayerPrefs.GetInt ("PlayerId").ToString ()) {
					// Show My Profile Details
					PlayerName.text = playerData ["username"].ToString ().Trim ('\"');
					CurrentGameLevel.text = playerData ["level_no"].ToString ().Trim ('\"');
					PlayerGender = playerData ["gender"].ToString ().Trim ('\"');

					UpdatedStatus.text = playerData ["player_profile_status"].ToString ().Trim ('\"');
				
					if (UpdatedStatus.text == "") {
						ScreenManager.Instance.ProfileScreen.transform.GetChild (1).GetChild (5).GetChild (0).GetComponent<Text> ().text = "Save";
					} else
						ScreenManager.Instance.ProfileScreen.transform.GetChild (1).GetChild (5).GetChild (0).GetComponent<Text> ().text = "Update";
					
					StartCoroutine (GetPlayerSocietyName (playerId));
					if (playerData ["vip_subscription"].ToString ().Contains ("True"))
						VIPSubscribed.text = "Yes";
					else
						VIPSubscribed.text = "No";
					AchievementName = playerData ["current_achievement_medal"].ToString ().Trim ('\"');
					PlayerPrefs.SetString ("CurrentAchievementMedal", AchievementName);
					if (AchievementName != "") {
						for (int i = 0; i < AchievementsManager.Instance.AchievementContainer.childCount; i++) {
							var achivmentName = AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().ThisAchievement.Aname +
							                    AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().ThisAchievement.AchievementName +
							                    AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().ThisAchievement.Medal;
							if (achivmentName == AchievementName) {
								MedalIcon.sprite = AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().MedalIcon.sprite;
								AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().CompleteIcon.interactable = false;
							}
						}
					} else
						MedalIcon.sprite = AchievementsManager.Instance.Medals [0];			
					StartCoroutine (ScreenAndPopupCall.Instance.ShowMyProfile (PlayerManager.Instance.MainCharacter, true, thisPlayerId));
					//ToDo

					ShowUserProfile.Instance.WaitingScreenState (false);
				} else {
					//Show Friend Profile Details
					FriendProfileManager.Instance.UserName.text = playerData ["username"].ToString ().Trim ('\"');
					FriendProfileManager.Instance.GameLevel.text = playerData ["level_no"].ToString ().Trim ('\"');
					FriendProfileManager.Instance.PlayerGender = playerData ["gender"].ToString ().Trim ('\"');
					FriendProfileManager.Instance.PlayerStatus.text = playerData ["player_profile_status"].ToString ().Trim ('\"');
					if (FriendProfileManager.Instance.PlayerStatus.text == "") {
						FriendProfileManager.Instance.PlayerStatus.text = " - ";
					}
					StartCoroutine (GetPlayerSocietyName (playerId));
					if (playerData ["vip_subscription"].ToString ().Contains ("True"))
						FriendProfileManager.Instance.VIPSubsctiption.text = "Yes";
					else
						FriendProfileManager.Instance.VIPSubsctiption.text = "No";	

					string achivmentMedalName = playerData ["current_achievement_medal"].ToString ().Trim ('\"');
					if (achivmentMedalName != "") {
						for (int i = 0; i < AchievementsManager.Instance.AchievementContainer.childCount; i++) {
							var OtherPlayerachivmentName = AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().ThisAchievement.Aname +
							                               AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().ThisAchievement.AchievementName +
							                               AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().ThisAchievement.Medal;
							if (OtherPlayerachivmentName == achivmentMedalName) {
								FriendProfileManager.Instance.MedalIcon.sprite = AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().MedalIcon.sprite;
								AchievementsManager.Instance.AchievementContainer.GetChild (i).GetComponent<Achievement> ().CompleteIcon.interactable = false;
							}
						}
					} else
						FriendProfileManager.Instance.MedalIcon.sprite = AchievementsManager.Instance.Medals [0];

					ScreenAndPopupCall.Instance.ShowOtherAndFriendProfileScreen ();
					//TODO: friends real profile avtar has to be display on the screen
					if (FriendProfileManager.Instance.PlayerGender == "Male") {
						StartCoroutine (ScreenAndPopupCall.Instance.ShowMyProfile (PlayerType [0], true, thisPlayerId));
					} else {
						StartCoroutine (ScreenAndPopupCall.Instance.ShowMyProfile (PlayerType [1], true, thisPlayerId));
					}
					ScreenManager.Instance.ClosePopup ();

					ShowUserProfile.Instance.WaitingScreenState (false);
				}
				ScreenAndPopupCall.Instance.CharacterCamera.rect = new Rect (0.74f, 0.06f, 0.25f, 0.74f);

			} else
				print ("Somthing went wrong!!!");
			ShowUserProfile.Instance.WaitingScreenState (false);
		}

	}


	IEnumerator GetPlayerSocietyName (int thisPlayerId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "search";
		jsonElement ["search_type"] = "mine";
		jsonElement ["keyword"] = "";
		jsonElement ["player_id"] = thisPlayerId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (GetCurruntSocietyName, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Societies are following according your search")) {
				var playerData = _jsnode ["data"];
				if (thisPlayerId == PlayerPrefs.GetInt ("PlayerId")) {
					CurruntSocietyName.text = playerData [0] ["society_name"].ToString ().Trim ('\"');				
				} else {
					FriendProfileManager.Instance.CurruntSociety.text = playerData [0] ["society_name"].ToString ().Trim ('\"');
				}
			} else if (_jsnode ["status"].ToString ().Contains ("400") && _jsnode ["description"].ToString ().Contains ("Your society list has empty"))
			if (thisPlayerId == PlayerPrefs.GetInt ("PlayerId"))
				CurruntSocietyName.text = "  -  ";
			else
				FriendProfileManager.Instance.CurruntSociety.text = "  -  ";
		}

	}

	public void StatusUpdate ()
	{
		if (StatusField.text == "") {
			ShowConfirmationPopUp ("Please enter status");
		} else {
			PlayerPrefs.SetString ("PlayerProfileStatus", StatusField.text);
			StartCoroutine (DownloadContent.Instance.UpdateData ());
			ShowConfirmationPopUp ("Your status updated successfully");
		}
	}

	public void ShowConfirmationPopUp (string message)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
//		if (message.Contains ("Your status updated successfully"))
//			ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
//		else
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (message.Contains ("Your status updated successfully")) {
				ScreenManager.Instance.ProfileScreen.transform.GetChild (1).GetChild (5).GetChild (0).GetComponent<Text> ().text = "Update";
				ShowUserProfile.Instance.UpdatedStatus.text = PlayerPrefs.GetString ("PlayerProfileStatus");
				ShowUserProfile.Instance.StatusField.text = "";
			}
		});
	}

	public void WaitingScreenState (bool isActive)
	{
		Vector3 scale = isActive ? Vector3.one : Vector3.zero;
		iTween.ScaleTo (WaitingScreen, scale, 0.1f);
	}

}
	
