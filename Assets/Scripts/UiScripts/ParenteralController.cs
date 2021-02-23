using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Simple_JSON;
using UnityEngine.SceneManagement;

public class ParenteralController : MonoBehaviour
{
    public static ParenteralController Instance = null;  

	public const string parentleActivateLink = "http://pinekix.ignivastaging.com/players/parentalControlRegister";
	public const string parentleLogInLink = "http://pinekix.ignivastaging.com/players/parentControllogin";
	public const string parentalControlUpdateOrChangeStatus = "http://pinekix.ignivastaging.com/players/saveParentControlView";

	public GameObject LoginPanel;
	public GameObject RegisterPanel;
	public GameObject BackToControlerButton;
	public GameObject BackButton;

	public GameObject enableParentalControl;
	public GameObject disableParentalControl;
	public bool activateParentel = false;

	[Header ("For Register Parentle")]
	public InputField UserEmail;
	public InputField Passowrd;
	public InputField ConfrimPassword;
	public Button Done;

	[Header ("For Login Parentel")]
	public InputField PUserEmail;
	public InputField PPassowrd;
	public Button loginButton;
	public Text StatusForButton;
	public Text WarningText;
	public Text ButtonText;

	public string _userEmail;
	public string _password;
	// Use this for initialization


	void Awake ()
	{
//		Reload ();

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

	void Start ()
	{
		ClearInputFeild ();
		Done.interactable = false;
		if (SceneManager.GetActiveScene ().name == "GamePlay") {
			if (PlayerPrefs.HasKey ("activateParentel")) {

				if (PlayerPrefs.GetString ("activateParentel").Contains ("true") || PlayerPrefs.GetString ("activateParentel").Contains ("True")) {
					activateParentel = true;
				}

				if (PlayerPrefs.GetInt ("Tutorial_Progress") > 24) {
					if (PlayerPrefs.GetString ("activateParentel").Contains ("true") || PlayerPrefs.GetString ("activateParentel").Contains ("True")) {
						activateParentel = true;
//						disableParentalControl.SetActive (true);
						PanelControlerLogin ();
						StatusForButton.text = "DISABLE";
						ButtonText.text = StatusForButton.text;
						print ("Disable Parental control button");
					} else {
						activateParentel = false;
//						enableParentalControl.SetActive (true);
//						disableParentalControl.SetActive (true);
						PanelControlerLogin ();
						StatusForButton.text = "ENABLE";
						ButtonText.text = StatusForButton.text;
						print ("Enable Parental control button");
					}
				}
			} else {
				activateParentel = false;
//				enableParentalControl.SetActive (true);
				PanelControlerRegister ();
//				disableParentalControl.SetActive (false);
				Debug.Log ("Enable Parental control button");
			}
		}
	}

	public void PanelControlerRegister ()
	{
		if (SceneManager.GetActiveScene ().name == "GamePlay") {
			LoginPanel.SetActive (false);
			BackButton.SetActive (true);
			BackToControlerButton.SetActive (false);
			RegisterPanel.SetActive (true);
			ClearInputFeild ();
		}
	}

	public void PanelControlerLogin ()
	{
		if (SceneManager.GetActiveScene ().name == "GamePlay") {
			LoginPanel.SetActive (true);
			BackButton.SetActive (true);
			BackToControlerButton.SetActive (false);
			RegisterPanel.SetActive (false);
			ClearInputFeild ();
		}
	}

	public void BackToPanelControler ()
	{
		if (SceneManager.GetActiveScene ().name == "GamePlay") {
			BackToControlerButton.SetActive (true);
			BackButton.SetActive (false);
			RegisterPanel.SetActive (false);
			LoginPanel.SetActive (false);
			WarningText.gameObject.SetActive (false);

		}
	}

	public void ClearInputFeild ()
	{
		UserEmail.text = "";
		Passowrd.text = "";
		ConfrimPassword.text = "";
		PUserEmail.text = "";
		PPassowrd.text = "";
		_userEmail = null;
		_password = null;
		WarningText.gameObject.SetActive (false);
		WarningText.text = "";
	}

	public void SubmitEmail ()
	{
		WarningText.text = "";
		_userEmail = UserEmail.text;
		if (_userEmail.Length <= 8) {
			WarningText.gameObject.SetActive (true);
			WarningText.text = "Email is too short Please enter again";
			UserEmail.text = "";
		}
		if (_userEmail.Length >= 30) {
			WarningText.gameObject.SetActive (true);
			WarningText.text = "Email is too long Please enter again";
			UserEmail.text = "";
		}
		if (_userEmail.Length >= 5 && _userEmail.Length <= 29) {
			WarningText.text = "";
			WarningText.gameObject.SetActive (false);
		}
		if (UserEmail.text.Length == 0) {
			WarningText.text = "";
		}
	}

	public void SubmitPassword ()
	{
		WarningText.text = "";
		_password = Passowrd.text;
		if (_password.Length == 0) {
			Passowrd.text = "";
		} else if (_password.Length <= 5) {
			WarningText.gameObject.SetActive (true);
			WarningText.text = "Password is too short Please enter again";
			Passowrd.text = "";
		} else if (_password.Length >= 20) {
			WarningText.gameObject.SetActive (true);
			WarningText.text = "Password is too long please enter again";
			Passowrd.text = "";
		} else if (_password.Length >= 6 && _userEmail.Length <= 19) {
			WarningText.text = "";
			WarningText.gameObject.SetActive (false);
		} else if (Passowrd.text.Length == 0) {
			WarningText.text = "";
		}

	}

	public void ConfrimEnteredPassword ()
	{
		WarningText.text = "";
		if (_password != ConfrimPassword.text) {
			WarningText.gameObject.SetActive (true);
			WarningText.text = "Password mismatched please enter again";
			Passowrd.text = "";
			ConfrimPassword.text = "";
		} else {
			WarningText.text = "";
			WarningText.gameObject.SetActive (false);
			if (_password == ConfrimPassword.text && _userEmail != "") {
				_password = ConfrimPassword.text;
				Done.interactable = true;
			} else {
				Done.interactable = false;
				if (_userEmail == "") {
					WarningText.text = " Please fill email address";
				} else if (_password == "") {
					WarningText.text = " Please fill password ";
				} else
					WarningText.text = "";
			}
		}
		if (_password == "" && _userEmail == "" && ConfrimPassword.text.Length == 0) {
			WarningText.text = "";
		}
		if (_password == "" && _userEmail == "") {
			WarningText.gameObject.SetActive (true);
			WarningText.text = "Plese enter email and password then confirm";
			Passowrd.text = "";
			ConfrimPassword.text = "";
		}

	}


	public void SubmitLoginEmail ()
	{
		if (SceneManager.GetActiveScene ().name == "GamePlay") {
			WarningText.text = "";
			_userEmail = PUserEmail.text;
		}
	}

	public void SubmitLoginPassword ()
	{
		if (SceneManager.GetActiveScene ().name == "GamePlay") {
			WarningText.text = "";
			_password = PPassowrd.text;
		}
	}

	public void OnCLickParentleSetup ()
	{
		WarningText.text = "";
		if (Application.internetReachability == NetworkReachability.NotReachable) {
			WarningText.gameObject.SetActive (true);
			WarningText.text = "Network is not available please try again!";
		} else {
			if (_userEmail == "" && _password == "") {
				WarningText.gameObject.SetActive (true);
				WarningText.text = "Pease enter email and password";
			} else
				StartCoroutine (ParentleRegister (_userEmail, _password));
		}

	}

	public IEnumerator ParentleRegister (string _email, string _password)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["player_email"] = PlayerPrefs.GetString ("UserEmail");
		jsonElement ["parental_email_id"] = _email;
		jsonElement ["parental_password"] = _password;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (parentleActivateLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Player`s parental control has saved successfully")) {
				activateParentel = true;
				PlayerPrefs.SetString ("activateParentel", activateParentel.ToString ());

				WarningText.text = WarningText.text = _jsnode ["description"].ToString ().Trim ('"');
				PlayerPrefs.SetString ("ParentelEmail", _userEmail);
				ClearInputFeild ();
				if (SceneManager.GetActiveScene ().name == "GamePlay") {
					StatusForButton.text = "Disable";
					ButtonText.text = StatusForButton.text;

//					disableParentalControl.SetActive (true);
					enableParentalControl.SetActive (false);
					PanelControlerLogin ();
					ShowPopUpMessage ("Parental Control Enabled");
				}
				if (SceneManager.GetActiveScene ().name == "00_LoginScene") {
					CharacterCustomizationAtStart.Instance.CameraEnebleDesable (false);
					CharacterCustomizationAtStart.Instance.ShowConfirmationPopUpParentelScreen (false);

				} 
			} else {
				ClearInputFeild ();
				WarningText.gameObject.SetActive (true);
				WarningText.text = _jsnode ["description"].ToString ().Trim ('"');
			}

		} 	
	}

	public void OnCLickParentleLogin ()
	{
		if (SceneManager.GetActiveScene ().name == "GamePlay") {
			WarningText.text = "";
			if (Application.internetReachability == NetworkReachability.NotReachable) {
				WarningText.gameObject.SetActive (true);
				WarningText.text = "Network is not available please try again!";
			} else {
				if (PlayerPrefs.HasKey ("activateParentel")) {
					if (_userEmail != null && _password != null)
						StartCoroutine (ParentleLogIn (_userEmail, _password));
					else {
						WarningText.gameObject.SetActive (true);
						WarningText.text = "Pease enter email and password";
					}
				} else {
					OnCLickParentleSetup ();
				}
//				if (enableParentalControl.activeInHierarchy)
//				{
//						
//					ShowPopUpMessage ("Parental Control Enabled");
//				} 
//				else if (disableParentalControl.activeInHierarchy) 
//				{
//					ShowPopUpMessage ("Parental Control Disabled");
//				}
			}
		}
	}

	public void HomeButtonClick ()
	{
		BackToPanelControler ();
		ScreenAndPopupCall.Instance.CloseScreen ();

	}

	public IEnumerator ParentleLogIn (string _email, string _password)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["parental_email_id"] = _email;
		jsonElement ["parental_password"] = _password;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (parentleLogInLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("_jsnode of Login or enable Parental -->> ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Player parental control details are following")) {		
				if (_jsnode ["data"] ["parental_status"].ToString ().Contains ("1")) {
					StartCoroutine (UpdateStatusOfParentalControl (2));
				} else if (_jsnode ["data"] ["parental_status"].ToString ().Contains ("2")) {
					StartCoroutine (UpdateStatusOfParentalControl (1));
				}
			
//				ShowPopUpMessage ("Parental Enabled");
			} else {
				ClearInputFeild ();
				WarningText.gameObject.SetActive (true);
				WarningText.text = _jsnode ["description"].ToString ().Trim ('"');
			}
		}
	}

	IEnumerator UpdateStatusOfParentalControl (int _status)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["status"] = _status.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (parentalControlUpdateOrChangeStatus, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("jsonDtat is ==>> " + _jsnode.ToString ()); 
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Player parental control status has updated")) {
				if (_status == 2) {
					WarningText.text = "Parental control successfully Disabled!";
					activateParentel = false;
					StatusForButton.text = "Enable";
					ButtonText.text = StatusForButton.text;
					ShowPopUpMessage ("Parental Control Disabled");
//					disableParentalControl.SetActive (true);
					PanelControlerLogin ();
//					enableParentalControl.SetActive(false);
//					BackToPanelControler ();
					PlayerPrefs.SetString ("activateParentel", activateParentel.ToString ());
				} else if (_status == 1) {
					WarningText.text = "Parental control successfully Enabled!";
					activateParentel = true;
					StatusForButton.text = "Disable";
					ButtonText.text = StatusForButton.text;
					ShowPopUpMessage ("Parental Control Enabled");
//					disableParentalControl.SetActive (true);
					PanelControlerLogin ();
//					enableParentalControl.SetActive(false);
//					BackToPanelControler ();
					PlayerPrefs.SetString ("activateParentel", activateParentel.ToString ());
				}
				ClearInputFeild ();
			} else {
				ClearInputFeild ();
				WarningText.gameObject.SetActive (true);
				WarningText.text = _jsnode ["description"].ToString ().Trim ('"');
			}
		}
	}

	public void ShowPopUpMessageForParentel ()
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Currently you are under parental control. To disable :- Go to settings and disable it.";	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();

		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
	}

	public void ShowPopUpMessage (string msg)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = msg;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();

		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
//			BackToPanelControler ();
		});
	}

}

