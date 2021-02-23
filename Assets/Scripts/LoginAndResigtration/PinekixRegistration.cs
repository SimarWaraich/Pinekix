/// <summary>
/// Created By ::==>> SimarjitSingh... Dated 13 July 2k16
﻿/// <summary>
/// Created By ::==>> Simarjit Singh... Dated 14 July 2k16
/// This script will be used for player Registration, Login and Forgot Password
/// This script is used for player custom set-up
/// </summary>
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Simple_JSON;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections.Generic;
using System;

public class PinekixRegistration : MonoBehaviour
{
	const string signUpUrl = "http://pinekix.ignivastaging.com/players/register";
	const string signInUrl = "http://pinekix.ignivastaging.com/players/login";
	const string resetPasswordUrl = "http://pinekix.ignivastaging.com/players/forgotPassword";


	#region Registration Screen

	public InputField userNameField;
	public InputField emailField;
	public InputField passwordField;
	public InputField confirmPasswordField;
	public GameObject signUpButton;
	public GameObject InternetReachability_Check;

	public string userNameString;
	public string emailString;
	public string passwordString;

	#endregion

	#region Login Screen

	public InputField _login_EmailField;
	public InputField _login_passwordField;

	#endregion

	public InputField _forgotPassword_EmailField;

	public GameObject LoginPanel;
	public GameObject RegistrationPanel;
	public GameObject ForgotPasswordPanel;
	//	public GameObject Character_MaleFemaleSelectionPanel;
	public GameObject Character_CustomizationSelectionPanel;
	public GameObject LodingScreen;
	public Text MessageText;
	public Text UserName;

	void OnEnable ()
	{
//		PlayerPrefs.DeleteAll ();
		MessageText.gameObject.SetActive (false);
		signUpButton.GetComponent<Button> ().interactable = false;
//		PlayerPrefs.SetString ("UserName", "");
//		PlayerPrefs.SetInt ("Purchaseland", 0);

//		PlayerPrefs.SetInt ("Money", 10000);

	}

	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		if (string.IsNullOrEmpty (PlayerPrefs.GetString ("UserName"))) {
			
		} else if (Application.internetReachability == NetworkReachability.NotReachable) {
			LoadingAssetsPanel ();
			LodingScreen.GetComponent<LoadingGameAssets> ().enabled = false;
			InternetReachability_Check.SetActive (true);
			InternetReachability_Check.transform.FindChild ("Ok").gameObject.SetActive (false);
//			InvokeRepeating ("CheckConnection", 1f, 1f);
		} else if (!PlayerPrefs.HasKey ("CharacterRegistered")) {
			UserName.text = PlayerPrefs.GetString ("UserName");
			ShowCharacterCustomizationPanel ();
		} else {
			UserName.text = PlayerPrefs.GetString ("UserName");
			StartCoroutine (CoroutineForDirectLogin ());

		}

		if (PlayerPrefs.GetInt ("Logout") == 1) {
//			LoginPanel.SetActive (true);
			MoveToForntScreen (LoginPanel);
//			RegistrationPanel.SetActive (false);
			MoveToBackScreen (RegistrationPanel);
		} else if (PlayerPrefs.HasKey ("CharacterRegistered")) {
//			MoveToForntScreen (LoginPanel);
			UserName.text = PlayerPrefs.GetString ("UserName");
			MoveToBackScreen (RegistrationPanel);
		}	
	}

	void MoveToForntScreen (GameObject Screen)
	{
//		Screen.SetActive (true);
		iTween.ScaleTo (Screen, iTween.Hash ("scale", Vector3.one, "time", 0.5f, "easeType", "easeInCirc"));
	}

	void MoveToBackScreen (GameObject Screen)
	{
//		Screen.SetActive (false);
		iTween.ScaleTo (Screen, iTween.Hash ("scale", Vector3.zero, "time", 0.5f, "easeType", "easeInCirc"));
	}

	void CheckConnection ()
	{
		if (!ConnectionController.Instance._internetOutput || !ConnectionController.Instance._serverOutput) {
			Start ();
			CancelInvoke ();
		}
	}

	IEnumerator CoroutineForDirectLogin ()
	{
		LoadingAssetsPanelOnCharConfrim ();
		LodingScreen.GetComponent<LoadingGameAssets> ().ActiveLoading_OnCharSelectionConfrim ();

		yield return CharacterCustomizationAtStart.Instance.StartCoroutine (CharacterCustomizationAtStart.Instance.DirectConfirm ());
	}

	#region for Register

	public void SubmitName ()
	{
		userNameString = userNameField.text;
		if (userNameString.Length <= 3) {
			MessageText.gameObject.SetActive (true);
			MessageText.text = "User name is too short Please enter again!!";
			userNameField.text = "";
		}
		if (userNameString.Length >= 20) {
			MessageText.gameObject.SetActive (true);
			MessageText.text = "User name is too long Please enter again!!";
			userNameField.text = "";
		}
		if (userNameString.Length >= 4 && userNameString.Length <= 20) {
			MessageText.text = "";
		}
	}

	public void SubmitEmail ()
	{
		emailString = emailField.text;
	}

	public void SubmitConfirmPassword ()
	{
		if (confirmPasswordField.text.Length <= 5) {
			MessageText.gameObject.SetActive (true);
			MessageText.text = "Enterd password is too short Please enter again!!";
			passwordField.text = "";
			confirmPasswordField.text = "";
		} else {
			MessageText.text = "";
		}

		if (passwordField.text == confirmPasswordField.text && userNameString != "" && emailString != "" && userNameString.Length >= 3) {
			signUpButton.GetComponent<Button> ().interactable = true;
			passwordString = passwordField.text;
		} else {
			MessageText.gameObject.SetActive (true);
			if (userNameString == "") {
				MessageText.text = "Please enter username";
			} else if (emailString == "") {
				MessageText.text = "Please enter email";
			} else {
				MessageText.text = "Password does not match";
			}
			StartCoroutine ("EmptyTheMessageFieldAfterFewSeconds");
			confirmPasswordField.text = "";
		}
	}

	public void onClickSignUp ()
	{
//		print ("onClickSignUp");
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			InternetReachability_Check.SetActive (true);
		} else {
			SignUp (userNameString, emailString, passwordString);
		}
	}

	void SignUp (string _userName, string _email, string _password)
	{
		var encoding = new System.Text.UTF8Encoding ();
		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		UserInfo Info = new UserInfo ();
		Info.username = _userName;
		Info.email = _email;
		Info.password = _password;

		string json = JsonUtility.ToJson (Info);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW request = new WWW (signUpUrl, encoding.GetBytes (json), postHeader);

		StartCoroutine (WaitForRequest (request));
	}

	IEnumerator WaitForRequest (WWW www)
	{
//		print ("Waiting For Response for SignUP");
		yield return(www);

		if (www.error == null) {
			JSONNode _jsNode = JSON.Parse (www.text);
//			print ("JSON DATA IN RESPONSE IS -->>" + _jsNode.ToString () + "    " + _jsNode [2]);

			if (!_jsNode.ToString ().Contains ("data")) { // if no data is prsent means error 
				MessageText.gameObject.SetActive (true);
				MessageText.text = _jsNode [1].ToString ().Trim ("\"".ToCharArray ());
				StartCoroutine ("EmptyTheMessageFieldAfterFewSeconds");
				EmptyAllFields ();
			} else {
				PlayerPrefs.SetInt ("PlayerId", 0);
				PlayerPrefs.SetInt ("Money", 1000);

//				for (int i = 0; i == PlayerPrefs.GetInt ("Level"); i++) {
				PlayerPrefs.DeleteKey ("ExperiencePoints" /*+ i*/);
//				}

				PlayerPrefs.SetInt ("Gems", 0);
				PlayerPrefs.DeleteKey ("Level");
			
				PlayerPrefs.SetInt ("Tutorial_Progress", 0);
				PlayerPrefs.SetInt ("Purchaseland", 0);
				PlayerPrefs.DeleteKey ("CharacterRegistered");
				PlayerPrefs.SetString ("CharacterType", "Male");

//				print ("SignUp Sucess" + www.text);
				PlayerPrefs.SetString ("UserName", userNameString);
				UserName.text = PlayerPrefs.GetString ("UserName");
				PlayerPrefs.SetString ("UserEmail", emailString);
				PlayerPrefs.SetString ("UserPassword", passwordString);
				PlayerPrefs.SetInt ("PlayerId", int.Parse (_jsNode ["data"] ["id"].ToString ().Trim ("\"".ToCharArray ())));
				PlayerManager.Instance.UpdateData ();
//				ShowCharacterCustomizationPanel ();

				PushScript.CreateNewUserOnRegistration (PlayerPrefs.GetString ("UserName"), PlayerPrefs.GetString ("UserPassword"), PlayerPrefs.GetString ("UserEmail"));
//				LodingScreen.GetComponent<LoadingGameAssets> ().ActiveLoading_OnRegistration ();
				CloseOtherScreen ();
				CharacterCustomizationAtStart.Instance.ShowConfirmationPopUpParentel (true);
//				LoadingAssetsPanel ();
				MessageText.text = "";
//				SceneManager.LoadScene ("00_MenuScreen");

				yield return new WaitForSeconds (0.2f);
//				#if !UNITY_EDITOR
				print ("Push register is called here---->>>");
				PushScript.registerForRemoteNotifications ();
//				#endif

			}
		} else if (www.error != null) {
			print ("SignUp Error" + www.error);

		}
	}

	#endregion

	public void ShowLoginPanel ()
	{
		MoveToBackScreen (RegistrationPanel);
		MoveToForntScreen (LoginPanel);
		MoveToBackScreen (ForgotPasswordPanel);
		MessageText.gameObject.SetActive (false);
		EmptyAllFields ();
	}

	public void ShowSignUpPanel ()
	{
		MoveToForntScreen (RegistrationPanel);
		MoveToBackScreen (LoginPanel);
		MoveToBackScreen (ForgotPasswordPanel);
		MessageText.gameObject.SetActive (false);
		EmptyAllFields ();
	}

	public void ShowForgotPasswordPanel ()
	{
		MoveToBackScreen (RegistrationPanel);
		MoveToBackScreen (LoginPanel);
		MoveToForntScreen (ForgotPasswordPanel);
		MessageText.gameObject.SetActive (false);
		EmptyAllFields ();
	}

	#region for LOGIN

	public void SubmitLoginEmail ()
	{
		emailString = _login_EmailField.text;
	}

	public void SubmitLoginPassword ()
	{
		passwordString = _login_passwordField.text;

	}

	public void OnCLickSignIn ()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			InternetReachability_Check.SetActive (true);
		} else {
			SignIN (emailString, passwordString);
		}
	}

	float timer = 0;

	void SignIN (string _email, string _password)
	{	
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		UserInfo Info = new UserInfo ();

		Info.email = _email.Trim ('"');
		Info.password = _password.Trim ('\"');
		string json = JsonUtility.ToJson (Info);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW (signInUrl, encoding.GetBytes (json), postHeader);

		StartCoroutine (WaitForLoginRequest (www));
	}

	IEnumerator WaitForLoginRequest (WWW www)
	{
		print ("Waiting For Response for SignIN");

		while (!www.isDone) {
			timer += Time.deltaTime;
			if (timer >= 5f) {
				InternetReachability_Check.SetActive (true);
				www.Dispose ();
				StopAllCoroutines ();
			}
			yield return null;
		}
		if (www.error == null) {
			JSONNode _jsNode = JSON.Parse (www.text);
//			print ("JSON DATA IS -->>" + _jsNode.ToString());

			if (!_jsNode.ToString ().Contains ("data")) {
				MessageText.gameObject.SetActive (true);
				MessageText.text = _jsNode ["description"].ToString ().Trim ("\"".ToCharArray ());
				StartCoroutine ("EmptyTheMessageFieldAfterFewSeconds");
				EmptyAllFields ();
			} else {
				print ("SignIn Sucess" + www.text);
				//Display User Name


//				"id": "157",
//				"username": "Simar7",
//				"email": "simar7@gmail.com",
//				"level_no": "0",
//				"gender": "",
//				"total_coins": "48040",
//				"total_gems": "40",
//				"total_expirence_points": "29",
//				"total_expirence_level": "4",
//				"tutorial_status": "26/8",
//				"vip_subscription": "",
//				"end_time_vip_subscription": "",
//				"parental_control_status": "",
//				"party_time_cooldown": "",
//				"auth_token": "YXQZNSJZ",
//				"logout_time": "",
//				"role_id": "2"

				UserName.text = _jsNode ["data"] ["username"].ToString ().Trim ("\"".ToCharArray ());
				int Currentlevel = int.Parse (_jsNode ["data"] ["level_no"].ToString ().Trim ("\"".ToCharArray ()));

				if (Currentlevel != 0) {
					PlayerPrefs.SetInt ("LevelLocked", Currentlevel);
				} else {
					PlayerPrefs.SetInt ("LevelLocked", 1);
				}
				PlayerPrefs.SetInt ("PlayerId", int.Parse (_jsNode ["data"] ["id"].ToString ().Trim ("\"".ToCharArray ())));

				int level = 0;
				int.TryParse (_jsNode ["data"] ["level_no"], out level);
				PlayerPrefs.SetInt ("Level", level);
				int coins = 0;
				int.TryParse (_jsNode ["data"] ["total_coins"].ToString ().Trim ('"'), out coins);
				PlayerPrefs.SetInt ("Money", coins);
				int Gems = 0;
				int.TryParse (_jsNode ["data"] ["total_gems"].ToString ().Trim ('"'), out Gems);
				PlayerPrefs.SetInt ("Gems", Gems);
				int Xpslevel = 0;
				int.TryParse (_jsNode ["data"] ["total_expirence_level"].ToString ().Trim ('"'), out Xpslevel);
				PlayerPrefs.SetInt ("Level", Xpslevel);
				int XpsPoints = 0;
				int.TryParse (_jsNode ["data"] ["total_expirence_points"].ToString ().Trim ('"'), out XpsPoints);
				PlayerPrefs.SetFloat ("ExperiencePoints" /*+ Xpslevel*/, XpsPoints);

				PlayerPrefs.SetInt ("CharacterRegistered", 1);

				var tutorial = _jsNode ["data"] ["tutorial_status"].ToString ().Trim ('"');

				string temp = "";
				foreach (var c in tutorial) {
					if (c.ToString () == "/")
						break;
					temp += c.ToString ();
				}
				int Tut = 0;
				int.TryParse (temp, out Tut);
				PlayerPrefs.SetInt ("Tutorial_Progress", Tut);
				int PurchseLand = 0;
				int.TryParse (tutorial.Substring (tutorial.LastIndexOf ('/') + 1), out PurchseLand);
				PlayerPrefs.SetInt ("Purchaseland", PurchseLand);

				PlayerPrefs.SetString ("UserEmail", _jsNode ["data"] ["email"].ToString ().Trim ("\"".ToCharArray ()));
				PlayerPrefs.SetString ("UserPassword", passwordString);
				PlayerPrefs.SetString ("UserName", _jsNode ["data"] ["username"].ToString ().Trim ("\"".ToCharArray ()));

				if (_jsNode ["data"] ["end_time_vip_subscription"].ToString () != string.Empty)
					PlayerPrefs.SetString ("VIPSubcribedTime", _jsNode ["data"] ["end_time_vip_subscription"].ToString ().Trim ('\"'));
				else
					PlayerPrefs.DeleteKey ("VIPSubcribedTime");


				if (_jsNode ["data"] ["parental_control_status"].ToString () != string.Empty)
					PlayerPrefs.SetString ("activateParentel", _jsNode ["data"] ["parental_control_status"].ToString ().Trim ('\"'));
				else
					PlayerPrefs.DeleteKey ("activateParentel");

				if (_jsNode ["data"] ["party_time_cooldown"].ToString () != string.Empty)
					PlayerPrefs.SetString ("HostPartyCooldownTime", _jsNode ["data"] ["party_time_cooldown"].ToString ().Trim ('\"'));
				else
					PlayerPrefs.DeleteKey ("HostPartyCooldownTime");


				// will be changed according to data type
				PlayerPrefs.SetString ("CurrentAchievementMedal", _jsNode ["data"] ["current_achievement_meda"].ToString ().Trim ('\"'));
				PlayerPrefs.SetString ("RankLastSpecialEvent", _jsNode ["data"] ["rank_last_special_event"].ToString ().Trim ('\"'));

//				SceneManager.LoadScene ("00_MenuScreen");
				//Start Loading
				LoadingAssetsPanel ();
				MessageText.text = "";

				LoadingAssetsPanelOnCharConfrim ();
//                yield return PlayerManager.Instance.StartCoroutine (PlayerManager.Instance.GetCharacterCustomisations ());

				LodingScreen.GetComponent<LoadingGameAssets> ().ActiveLoading_OnCharSelectionConfrim ();

				yield return CharacterCustomizationAtStart.Instance.StartCoroutine (CharacterCustomizationAtStart.Instance.DirectConfirm ());
//				#if !UNITY_EDITOR
				PushScript.registerForRemoteNotifications ();
//				#endif

			}
		} else {
			print ("SignIN Error" + www.error);
			EmptyAllFields ();
		}
	}

	#endregion

	#region For Forgot Password

	public void onForgotSubmitEmail ()
	{
		emailString = _forgotPassword_EmailField.text;
	}

	public void OnClickResetMyPassword ()
	{
		ResetMyPassword (emailString);
	}

	void ResetMyPassword (string _email)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		UserInfo Info = new UserInfo ();

		Info.email = _email;

		string json = JsonUtility.ToJson (Info);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());
	
		WWW www = new WWW (resetPasswordUrl, encoding.GetBytes (json), postHeader);

		StartCoroutine (WaitForPassword (www));
	}

	IEnumerator WaitForPassword (WWW www)
	{
		print ("Waiting For Response for WaitForPassword");
		yield return(www);

		if (www.error == null) {
			print ("Password Sucess" + www.text);
			JSONNode _jsNode = JSON.Parse (www.text);

			MessageText.gameObject.SetActive (true);
			MessageText.text = MessageText.text = _jsNode ["description"].ToString ().Trim ("\"".ToCharArray ()) + " \n " + _jsNode ["error"] ["error_msg"].ToString ().Trim ("\"".ToCharArray ());
			StartCoroutine ("EmptyTheMessageFieldAfterFewSeconds");
			EmptyAllFields ();
		} else {
			print ("Password Error" + www.error);
		}
	}

	public void OnClickToInternetReachabilityButton ()
	{
		InternetReachability_Check.SetActive (false);
		EmptyAllFields ();
	}

	#endregion

	void EmptyAllFields ()
	{
		userNameField.text = "";
		emailField.text = "";
		passwordField.text = "";
		confirmPasswordField.text = "";
		_forgotPassword_EmailField.text = "";
		_login_passwordField.text = "";
		_login_EmailField.text = "";
		_forgotPassword_EmailField.text = "";
		userNameString = "";
		emailString = "";
		passwordString = "";
		signUpButton.GetComponent<Button> ().interactable = false;
	}

	IEnumerator EmptyTheMessageFieldAfterFewSeconds ()
	{
//		Debug.Log ("In Emptying the Field");
		yield return new WaitForSeconds (6);
		MessageText.text = "";
		MessageText.gameObject.SetActive (false);
	}

	#region for character customization

	public void LoadingAssetsPanel ()
	{
		MoveToBackScreen (RegistrationPanel);
		MoveToBackScreen (LoginPanel);
		MoveToBackScreen (ForgotPasswordPanel);
		MessageText.gameObject.SetActive (false);
		Character_CustomizationSelectionPanel.SetActive (false);
		LodingScreen.SetActive (true);
	}

	void CloseOtherScreen ()
	{
		MoveToBackScreen (RegistrationPanel);
		MoveToBackScreen (LoginPanel);
		LodingScreen.SetActive (false);
		MoveToBackScreen (ForgotPasswordPanel);
		MessageText.gameObject.SetActive (false);
		Character_CustomizationSelectionPanel.SetActive (false);
	}

	public void LoadingAssetsPanelOnCharConfrim ()
	{
		MoveToBackScreen (RegistrationPanel);
		MoveToBackScreen (LoginPanel);
		MoveToBackScreen (ForgotPasswordPanel);
		MessageText.gameObject.SetActive (false);
//		Character_MaleFemaleSelectionPanel.SetActive (false);
		Character_CustomizationSelectionPanel.SetActive (false);
		LodingScreen.SetActive (true);

		CharacterCustomizationAtStart.Instance.CameraEnebleDesable (false);

	}

	public void ShowCharacterCustomizationPanel ()
	{
		MoveToBackScreen (RegistrationPanel);
		MoveToBackScreen (LoginPanel);
		MoveToBackScreen (ForgotPasswordPanel);
		MessageText.gameObject.SetActive (false);
		Character_CustomizationSelectionPanel.SetActive (true);
		CharacterCustomizationAtStart.Instance.EnableSection ();
		LodingScreen.SetActive (false);

	}


	#endregion

	void OnApplicationQuit ()
	{
		PlayerPrefs.DeleteKey ("Logout");
	}
}


public class UserInfo
{
	public  string username;
	public string email;
	public string password;
	public int score;
	public int level_no;

}
