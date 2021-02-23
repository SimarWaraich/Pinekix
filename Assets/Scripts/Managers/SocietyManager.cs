using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simple_JSON;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class SocietyManager : Singleton<SocietyManager>
{
	public SocietyCreationController societyCreationController;
	public GameObject AllSocietyContainer;
	public GameObject RecentSocietyContainer;
	public GameObject MySocietyContainer;
	public GameObject NewPresidentContainer;

	public GameObject CreateButton;
	public GameObject RecentSocietyPanel;
	public GameObject AllSocietyPanel;
	public GameObject MySocietyPanel;

	public List<Sprite> EmblemList;
	public List<string> AllTagsList;
	public List<GameObject> RoomPrefabsList;

	public GameObject SocietyPrefab;
	public GameObject SocietyFriendPrefab;
	public GameObject SocietyMemberPrefab;
	public GameObject SocietyAchievementPrefab;
	public GameObject SocietyLBPrefab;

	public GameObject FriendsContainer;
	public GameObject NotificationPopUp;
	public Text _memberCountText;


	public Transform MemberContainer;
	public Transform AcheivementContainer;
	public Transform LeaderBoardContainer;
    /// <summary>
    /// My role. role id - 2(Normal Member), 1(Committee member), 0(President), 3(Unknown)
    /// </summary>
    public int myRole = 0;
	public Society _mySociety;

	public List<SocietyMembers> _allMembers = new List<SocietyMembers> ();
	public List<SocietyAchievements> _allachievements = new List<SocietyAchievements> ();

	public Society SelectedSociety;

	public const string SocietyLink = "http://pinekix.ignivastaging.com/societies/society";

	public bool isChatInSocietyOn = false;


	public Text _totalPlayers;

	void Awake ()
	{
		Reload ();
	}

	public void CreateSocietyList (List<Society> societies, GameObject container)
	{
		DeleteGameObjects (container);
		if (container == MySocietyContainer) {	
			
			AllSocietyPanel.SetActive (false);
			RecentSocietyPanel.SetActive (false);
			MySocietyPanel.SetActive (true);

			ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("Message").GetComponent <Text> ().text = "";
			if (societies.Count == 0)
				CreateButton.SetActive (true);
			else
				CreateButton.SetActive (false);			
		} else if (container == RecentSocietyContainer) {
			RecentSocietyPanel.SetActive (true);
			AllSocietyPanel.SetActive (false);
			MySocietyPanel.SetActive (false);
			CreateButton.SetActive (false);		
			if (societies.Count == 0)
				ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("Message").GetComponent <Text> ().text = "No societies found";
			else
				ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("Message").GetComponent <Text> ().text = "";	
			
		} else {			
			AllSocietyPanel.SetActive (true);
			RecentSocietyPanel.SetActive (false);
			MySocietyPanel.SetActive (false);
			CreateButton.SetActive (false);		
			if (societies.Count == 0)
				ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("Message").GetComponent <Text> ().text = "No societies found";
			else
				ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("Message").GetComponent <Text> ().text = "";		
		}
		societies.ForEach (society => {
			GameObject Go = GameObject.Instantiate (SocietyPrefab, container.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.GetComponent <SocietyUi> ().Society = society;
		});
	}

	void DeleteGameObjects (GameObject container)
	{
		for (int i = 0; i < container.transform.childCount; i++) {
			GameObject.Destroy (container.transform.GetChild (i).gameObject);
		}
	}

	public enum SeachType
	{
		recent,
		name,
		tag,
		mine

	}

	public void GetRecentSocieties ()
	{
		if (!ParenteralController.Instance.activateParentel) {
			ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("SearchByName").GetComponent <InputField> ().text = "";
			ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("SearchByTag").GetComponent <InputField> ().text = "";
			StartCoroutine (IGetSocieties (SeachType.recent, "", false));
		}
	}

	public void GetMySocieties ()
	{
		ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("SearchByName").GetComponent <InputField> ().text = "";
		ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("SearchByTag").GetComponent <InputField> ().text = "";
		StartCoroutine (IGetSocieties (SeachType.mine, "", false));
	}

	public void SearchSocietiesByTag (InputField field)
	{
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("SearchByName").GetComponent <InputField> ().text = "";

		if (!Tut._SocietyCreated)
			return; 

		if (field.text != string.Empty)
			StartCoroutine (IGetSocieties (SeachType.tag, field.text, false));
		else
			GetRecentSocieties ();
	}

	public void SearchSocietiesByName (InputField field)
	{ 
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		ScreenManager.Instance.SocietyListScreen.transform.FindChild ("SocietySearch").FindChild ("SearchByTag").GetComponent <InputField> ().text = "";

		if (!Tut._SocietyCreated)
			return; 
        
		if (field.text != string.Empty)
			StartCoroutine (IGetSocieties (SeachType.name, field.text, false));
		else
			GetRecentSocieties ();
	}

	public void GetAllPlayers (int society_id)
	{
		StartCoroutine (IGetAllPlayers (society_id));
	}

	public void RemovePlayer (int target_id)
	{
		StartCoroutine (IeDeletePlayer (target_id));
	}



	public void UpdateRole (int target_id, int target_role, int target_level)
	{
		StartCoroutine (IeUpdateRole (target_role, target_id, target_level));
	}

	public void GetAllAchievements (int society_id)
	{
		StartCoroutine (IeGetSocietyAchievements (society_id));
	}

	public void GetMyRole (Society society)
	{
		StartCoroutine (IeGetMyRole (society));
	}

	public void LeaveSociety ()
	{
		RemovePlayer (PlayerPrefs.GetInt ("PlayerId"));
	}

	public void QuitSociety ()
	{
		ShowPopUp ("Do you really want to quit?", () => RemovePlayer (PlayerPrefs.GetInt ("PlayerId")), null);
	}

	public IEnumerator IGetSocieties (SeachType type, string SeachKeyword, bool isForCheck)
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "search";
		jsonElement ["search_type"] = type.ToString ();
		jsonElement ["keyword"] = SeachKeyword;
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("400") || _jsnode ["status"].ToString ().Contains ("200")) {
		
				JSONNode data = _jsnode ["data"];
				var Societies = new List<Society> ();
				for (int i = 0; i < data.Count; i++) {
					string name = data [i] ["society_name"];
					string description = data [i] ["society_description"];
					int emblem = 0;
					int.TryParse (data [i] ["emblem_index"], out emblem);
					int room = 0;
					int.TryParse (data [i] ["room_index"], out room);
					int _id = 0;
					int.TryParse (data [i] ["society_id"], out _id);

					JSONNode tags = data [i] ["tags"];
					var listofTags = new List<string> ();

					for (int x = 0; x < tags.Count; x++) {
						listofTags.Add (tags [x] ["tag_title"]);
					}

					Society sc = new Society (name, _id, description, emblem - 1, room - 1, listofTags);
					Societies.Add (sc);
				}
				switch (type) {
				case SeachType.mine:
					if (Societies.Count > 0)
						_mySociety = Societies [0];
					else
						_mySociety = null;

					if (!isForCheck) {
						CreateSocietyList (Societies, MySocietyContainer);
						ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MySociety").GetComponent <Button> ().interactable = false;
						if (!GameManager.Instance.gameObject.GetComponent<Tutorial> ()._SocietyCreated)
							ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MostRecent").GetComponent <Button> ().interactable = false;
						else
							ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MostRecent").GetComponent <Button> ().interactable = true;
						IndicationManager.Instance.IncrementIndicationFor ("Society", 4);
					}
					break;
				case SeachType.recent:
					CreateSocietyList (Societies, RecentSocietyContainer);
					ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MySociety").GetComponent <Button> ().interactable = true;
					ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MostRecent").GetComponent <Button> ().interactable = false;
					break;
				case SeachType.name:
					CreateSocietyList (Societies, AllSocietyContainer);
					ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MySociety").GetComponent <Button> ().interactable = true;
					ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MostRecent").GetComponent <Button> ().interactable = true;
					break;
				case SeachType.tag:
					CreateSocietyList (Societies, AllSocietyContainer);
					ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MySociety").GetComponent <Button> ().interactable = true;
					ScreenManager.Instance.SocietyListScreen.transform.FindChild ("MostRecent").GetComponent <Button> ().interactable = true;
					break;
				default:
					Debug.LogError ("Search type not specified");
					break;
				}
				yield return _mySociety;
			} 	
		}
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	public IEnumerator IGetAllPlayers (int society_id)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "get_member";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = society_id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("400") || _jsnode ["status"].ToString ().Contains ("200")) {

				JSONNode data = _jsnode ["data"];
				_allMembers.Clear ();
				int _id = 0;
				int _role = 0;
				int _society_id = 0;
				int _level = 0;
				string _name = "";

				for (int i = 0; i < data.Count; i++) {
					int.TryParse (data [i] ["player_id"].ToString ().Trim ('"'), out _id);
					int.TryParse (data [i] ["role_id"].ToString ().Trim ('"'), out _role);
					int.TryParse (data [i] ["society_id"].ToString ().Trim ('"'), out _society_id);
					int.TryParse (data [i] ["level_number"].ToString ().Trim ('"'), out _level);
					_name = data [i] ["player_name"].ToString ().Trim ('"');
					var newMember = new SocietyMembers (_id, _name, _role, _society_id, _level);
					_allMembers.Add (newMember);
				}

				_totalPlayers.text = _allMembers.Count.ToString () + "/50";

			}
		} else {
			print (www.error.ToString ());
		}
	}


	public IEnumerator IeUpdateRole (int role, int player_id, int level)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "update";
		jsonElement ["role_id"] = role.ToString ();
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["updated_player_id"] = player_id.ToString ();
		jsonElement ["society_id"] = SelectedSociety.Id.ToString ();
		jsonElement ["level_number"] = level.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");
				yield return true;
			} else {
				yield return false;
			}
			SocietyManager.Instance.StartCoroutine (SocietyManager.Instance.IeGetMyRole (SocietyManager.Instance.SelectedSociety));
			SocietyDescriptionController.Instance.SocietyMemberList (true);
		} else {
			yield return false;
		}

	}




	public IEnumerator IeDeletePlayer (int player_id)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "delete_member";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["delete_player_id"] = player_id.ToString ();
		jsonElement ["society_id"] = SelectedSociety.Id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");
				if (!_reported)
					ShowPopUp ("Player has been removed from society", () => {

						StartCoroutine (IeGetMyRole (SelectedSociety));
						if (player_id == PlayerPrefs.GetInt ("PlayerId")) {
						} else {
							
							SocietyDescriptionController.Instance.SocietyMemberList (true);
						}
					});
				else {
					ShowPopUp ("Your report has been sent to the Admin he will take action shortly", () => StartCoroutine (IeGetMyRole (SelectedSociety)));
					_reported = false;
				}
				NotificationManager.Instance.SendNotificationToUser (player_id, string.Format ("You have been removed out of \"{0}\" society", SelectedSociety.Name));
			}


		} 	
	}


	public IEnumerator IeGetMyRole (Society society)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
		ScreenAndPopupCall.Instance.LoadingScreen ();
//		"data_type" : "get_role",
		jsonElement ["data_type"] = "get_role";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = society.Id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
//			"description": "Player`s role is following.",
			if (_jsnode ["description"].ToString ().Contains ("Player`s role is following") || _jsnode ["status"].ToString ().Contains ("200")) {
				JSONNode data = _jsnode ["data"];
				int.TryParse (data ["role_id"].ToString ().Trim ('"'), out myRole);

				var societyDescriptionController = ScreenManager.Instance.Admin_MemberDiscriptionPanel.GetComponent <SocietyDescriptionController> ();
				societyDescriptionController.OpenSocietyDiscription (myRole, society);
//				switch(myRole)
//				{
//				case 0:
//					
//					break;
//				case 1:
//					break;
//				case 2:
//					break;
//				case 3:
//					break;
//				default:
//					break;
//				}
			} else if (_jsnode ["description"].ToString ().Contains ("This player is not found in this society") || _jsnode ["status"].ToString ().Contains ("400")) {
				myRole = 3;
				var societyDescriptionController = ScreenManager.Instance.Admin_MemberDiscriptionPanel.GetComponent <SocietyDescriptionController> ();
				societyDescriptionController.OpenSocietyDiscription (myRole, society);
			}
		} 	
	}



	public IEnumerator IeSwapPresident (string id)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
		ScreenAndPopupCall.Instance.LoadingScreen ();
		//		"data_type" : "get_role",
		jsonElement ["data_type"] = "update_president_role";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SelectedSociety.Id.ToString ();
		jsonElement ["president_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["commity_member_id"] = id;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			//			"description": "Player`s role is following.",
			if (_jsnode ["description"].ToString ().Contains ("Player`s role is following") || _jsnode ["status"].ToString ().Contains ("200")) {
				yield return true;
			} else if (_jsnode ["description"].ToString ().Contains ("This player is not found in this society") || _jsnode ["status"].ToString ().Contains ("400")) {
				yield return false;
			}
		} else
			yield return false;
	}



	public IEnumerator IeGetSocietyAchievements (int society_id)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "get_achievement";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = society_id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				JSONNode data = _jsnode ["data"];
				int.TryParse (data ["role_id"].ToString ().Trim ('"'), out myRole);
			}
		} 	
	}

	public void AddMemberToSociety (int PlayerId, int SocietyId, int invitationid, GameObject go = null)
	{
//			bool _alreadyadded = false;
//			for (int i = 0; i < _allMembers.Count; i++) {
//					if (_allMembers [i].player_id == PlayerId) {
//							_alreadyadded = true;
//							break;
//				} else
//							_alreadyadded = false;
//				}
//			if (!_alreadyadded)
		StartCoroutine (IeJoinSociety (PlayerId, SocietyId, invitationid, go));
//			else
//					ShowPopUp ("This Player is already Added.", null);
	}



	public void JoinSocietyOnRequest (int societyId)
	{
//		StartCoroutine (IeJoinSociety (societyId));
	}

	public IEnumerator IeJoinSociety (int playerId, int society_Id, int invitationid, GameObject go = null)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "join";
		jsonElement ["player_id"] = playerId.ToString ();
		jsonElement ["society_id"] = society_Id.ToString ();
		jsonElement ["level_number"] = "1";


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Your have joined society successfully.") || _jsnode ["status"].ToString ().Contains ("200")) {
				ShowPopUp ("Member added successfully.", null);
				if (go != null)
					NotificationManager.Instance.DeleteInvitation (invitationid, go);

				NotificationManager.Instance.SendNotificationToUser (playerId, string.Format ("Your request to join the \"{0}\" society is accepted", SelectedSociety.Name));
				yield return true;
			} else if (_jsnode ["description"].ToString ().Contains ("This player id has already join any society") && _jsnode ["status"].ToString ().Contains ("400")) {
				ShowPopUp ("Already a member of a society.", null);
				yield return false;
//				if (go != null)
//					NotificationManager.Instance.DeleteInvitation (invitationid, go);
			} else if (_jsnode ["description"].ToString ().Contains ("Members limit has not crossed")) {
				ShowPopUp ("Society has reached maximum limit of members", null);		
				yield return false;
			}			
		} 	
	}

	public void OnClickReportButton ()
	{
		if (myRole < 3 && myRole > 0)
			ShowPopUp ("If you want to report your own society then you would have to leave it.", () => ScreenAndPopupCall.Instance.ReportSocietyPopUp ());
		else if (myRole == 0)
			ShowPopUp ("You can't report your own society. If you want then you have to first demote yourself.", null);
		else
			ScreenAndPopupCall.Instance.ReportSocietyPopUp ();
	}

	public void ReportSociety (InputField inputField)
	{
		if (inputField.text != "" && !Regex.IsMatch (inputField.text, "^[ \t\r\n\u0200]*$")) {
			if (myRole == 3)
				StartCoroutine (IeReportSociety (SelectedSociety.Id, inputField.text));
			else if (myRole < 3 && myRole > 0)
				StartCoroutine (IeReportSociety (SelectedSociety.Id, inputField.text));
			else if (myRole == 0)
				ShowPopUp ("You can't report your own society. If you want then you have to first demote yourself.", null);
		} else
			ShowPopUp ("Report message can not be empty", () => {
				ScreenAndPopupCall.Instance.ReportSocietyPopUp ();
			});
	}

	bool _reported = false;

	IEnumerator IeReportSociety (int society_Id, string message)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["data_type"] = "save_report";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = society_Id.ToString ();
		jsonElement ["message"] = message;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
				_reported = true;
				if (myRole == 3) {
					ScreenManager.Instance.ClosePopup ();
					ShowPopUp ("Your report has been sent to the Admin he will take action shortly", null);
				} else if (myRole < 3) {
					yield return IeDeletePlayer (PlayerPrefs.GetInt ("PlayerId"));
					
				}
			} else if (_jsnode ["status"].ToString ().Contains ("400")) {
				ShowPopUp ("You already reported this society.", null);
			}
		}
	}


	public void ShowPopUp (string message, UnityEngine.Events.UnityAction OnClickActions = null)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickActions != null)
				OnClickActions ();
		});
	}

	public void ShowPopUp (string message, UnityEngine.Events.UnityAction OnClickActions, UnityEngine.Events.UnityAction OnClickCloseActions)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickActions != null)
				OnClickActions ();
		});

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickCloseActions != null)
				OnClickCloseActions ();
		});
	}

	[Serializable]
	public class Society
	{
		public string Name;
		public string Description;
		public int Id;
		public int EmblemType;
		public int RoomType;
		public List<string> Tags = new List<string> ();


		public Society (string name, int id, string descrptn, int emblem, int room, List<string> tags)
		{
			Name = name;
			Id = id;
			Description = descrptn;
			EmblemType = emblem;
			RoomType = room;
			Tags = tags;
		}
	}



	public class SocietyAchievements
	{
		public int achievement_id;
		public int status;
		public string achievement_title;
		public string achievement_url;
		public string achievement_description;

		public SocietyAchievements (int id, int _status, string title, string url, string desc)
		{
			achievement_id = id;
			status = _status;
			achievement_title = title;
			achievement_url = url;
			achievement_description = desc;
		}
	}

	public class SocietyMembers : IComparable<SocietyMembers>
	{
		public int player_id;
		public string player_name;
		public int role_id;
		public int society_id;
		public int level;


		public SocietyMembers (int id, string _name, int role, int soc_id, int lev)
		{
			player_id = id;
			player_name = _name;
			role_id = role;
			society_id = soc_id;
			level = lev;
		}

		public int CompareTo (SocietyMembers other)
		{
			if (this.role_id == other.role_id) {
				return this.player_name.CompareTo (other.player_name);
			}

			return this.role_id.CompareTo (other.role_id);
		}
	}



	public void AddMember ()
	{
		if (myRole == 0 || myRole == 1) {
			StartCoroutine (ListofFriends ());
		} else {
			ShowPopUp ("You don't have authority to add people", () => SocietyDescriptionController.Instance.SocietyMemberList (true));
		}
	}


	IEnumerator ListofFriends ()
	{
		yield return StartCoroutine (FriendsManager.Instance.GetAllFriendsList (false));
		ScreenManager.Instance.ClosePopup ();
//		ScreenAndPopupCall.Instance.ShowFriendList ();
		FriendsManager.Instance.CreateAllFriendsForJoin ();

	}

	//	public void ShowPopupofAddError ()
	//	{
	//		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
	//		ScreenManager.Instance.ClosePopup ();
	//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
	//
	//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
	//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Cancel";
	//		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "You don't have authority to add people.";
	//		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());
	//	}

	public IEnumerator GetSocietyInvitations ()
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string, string> postHeader = new Dictionary<string, string> ();


		var jsonElement = new JSONClass ();

		jsonElement ["data_type"] = "get_society_invitation";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyManager.Instance.SelectedSociety.Id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			List <SocietyInvitations> Invitations = new List<SocietyInvitations> ();

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");
				var data = _jsnode ["data"];
				for (int i = 0; i < data.Count; i++) {

					int invitation_id = 0;
					int.TryParse (data [i] ["invitation_id"], out invitation_id);

					int senderId = 0;
					int.TryParse (data [i] ["player_id"], out senderId);

					int societyId = 0;
					int.TryParse (data [i] ["society_id"], out societyId);

					string message = data [i] ["message"];


					SocietyInvitations Invite = new SocietyInvitations (invitation_id, societyId, senderId, message, "Invitation");
					Invitations.Add (Invite);
				}
			}
			CreateSocietyInvitations (Invitations);
		}
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	void DeleteGameObjects ()
	{
		var NotificationContainer = NotificationPopUp.GetComponentInChildren <GridLayoutGroup> ().transform;

		for (int i = 0; i < NotificationContainer.transform.childCount; i++) {
			GameObject.Destroy (NotificationContainer.transform.GetChild (i).gameObject);
		}
	}

	void CreateSocietyInvitations (List<SocietyInvitations> _List)
	{	
		DeleteGameObjects ();

		ScreenManager.Instance.ShowPopup (NotificationPopUp);

		var NotificationContainer = NotificationPopUp.GetComponentInChildren <GridLayoutGroup> ().transform;

		if (_List.Count == 0)
			NotificationPopUp.transform.FindChild ("NotFoundText").GetComponent <Text> ().text = "No Invitations to show";
		else
			NotificationPopUp.transform.FindChild ("NotFoundText").GetComponent <Text> ().text = "";

		_List.ForEach (invite => {
			GameObject Go = GameObject.Instantiate (NotificationManager.Instance.InvitationPrefab, NotificationContainer.transform)as GameObject;
			Go.transform.localPosition = Vector3.zero;
			Go.transform.localScale = Vector3.one;
			Go.AddComponent <SocietyInvitationsUi> ().Data = invite;
		});
	}

	public IEnumerator DeleteSocietyInvitations (int InvitationId)
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string, string> postHeader = new Dictionary<string, string> ();


		var jsonElement = new JSONClass ();

		jsonElement ["data_type"] = "delete_society";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["invitation_id"] = InvitationId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");		
				yield return true;
			} else
				yield return false;
				
		}
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}

	public IEnumerator SendSocietyInvitations (string message)
	{
		ScreenAndPopupCall.Instance.LoadingScreen ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string, string> postHeader = new Dictionary<string, string> ();


		var jsonElement = new JSONClass ();

		jsonElement ["data_type"] = "society_invitation";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyManager.Instance.SelectedSociety.Id.ToString ();
		jsonElement ["message"] = message;


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (SocietyLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {

				print ("Success");
				SocietyManager.Instance.ShowPopUp (string.Format ("Request to join \"{0}\" society sent successfully", SocietyManager.Instance.SelectedSociety.Name), null);

			} else if (_jsnode ["status"].ToString ().Contains ("400") || _jsnode ["description"].ToString ().Contains ("You has already sent invitation to this society"))
				SocietyManager.Instance.ShowPopUp (string.Format ("You have already sent request to this society"), null);
			else
				SocietyManager.Instance.ShowPopUp (string.Format ("Request to join \"{0}\" society failed", SocietyManager.Instance.SelectedSociety.Name), null);
		}
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}
}

public enum PlayerRole
{
	visitor = 0,
	normal_member = 1,
	committee_member = 2,
	President = 3
}

[Serializable]
public class SocietyInvitations
{

	public SocietyInvitations (int invitationid, int societyid, int senderId, string message, string title)
	{
		InvitationId = invitationid;
		SocietyId = societyid;
		SenderId = senderId;
		Message = message;
		Title = title;
	}

	public int InvitationId;
	public int SocietyId;
	public int SenderId;
	public string Message;
	public string Title;
}