using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VotingManager : Singleton<VotingManager>
{

	public Text playerName1, playerName2;
	public Text flatMateName1, flateMateName2;
	public Text eventName;
	public Text eventTheme;
	public Image progressBarP1, progressBarP2;
	public int voteCount = 0;
	public Button _Player1Voting, _Player2Voting;

	// Use this for initialization
	void Awake ()
	{
		this.Reload ();
	}

	public void OnIntelazition ()
	{
		playerName1.text = PlayerPrefs.GetString ("UserName");
        playerName2.text = RoommateManager.Instance.RoommatesHired[1].GetComponent<Flatmate>().data.Name.Trim ('"');
//		flatMateName1.text = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0).GetChild (0).GetComponent<Flatmate> ().data.Name.Trim ('"');
//		flateMateName2.text = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0).GetChild (0).GetComponent<Flatmate> ().name.Trim ('"');
		eventName.text = EventManagment.Instance.CurrentEvent.EventName.ToString ();
		eventTheme.text = "Theme: " + EventManagment.Instance.CurrentEvent.EventTheme.ToString ();
		progressBarP1.fillAmount = voteCount;
		progressBarP2.fillAmount = voteCount;
	
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!tut._FashionEventCompleate)
			_Player2Voting.interactable = false;
		else
			_Player2Voting.interactable = true;
		
	}

	public void OnClickVoting ()
	{
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (tut._FashionEventCompleate && tut._SaloonPurchased)
			return;
		
		if (_Player1Voting) {
			voteCount++;
			progressBarP1.fillAmount = voteCount;
			if (progressBarP1.fillAmount == 1f) {
//				EventManagment.Instance.CurrentEvent.IsCompleted = true;
//				EventManagment.Instance.CurrentEvent.IsGoingOn = false;
//				EventManagment.Instance.ChangeCurruntEventStatus (); 

				Invoke ("CongoMassageToWinner", 1.5f);
			}
			_Player2Voting.interactable = false;
		}
		if (_Player2Voting) {
			voteCount = 0;
			voteCount++;
			progressBarP1.fillAmount = voteCount;
		}		
	}

	void CongoMassageToWinner ()
	{		
//		ScreenAndPopupCall.Instance.CloseScreen ();
//		ScreenAndPopupCall.Instance.CloseCharacterCameraForFashionEvent ();
//		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
//		ScreenManager.Instance.ClosePopup ();
//		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
//		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
//		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
//		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Conglaturation ! " + ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0).GetChild (0).GetComponent<Flatmate> ().name + " You have won the fashion show event!";
//		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());

		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenAndPopupCall.Instance.CloseScreen ();
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").gameObject.GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";

		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Congratulation! " + PlayerPrefs.GetString ("UserName") + ", that was fun! We need to look out for the rare special university events that happen every month or so. They are huge events where the whole university compete with each other in order to earn the most points over the time period! Only the top players earn the best prizes!";
		// Event Button Function
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() =>{ 
			ScreenManager.Instance.ClosePopup ();
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);	
			ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("CloseButton").gameObject.SetActive (true);
			PlayerPrefs.SetInt ("Tutorial_Progress", 15);	
			GameManager.Instance.GetComponent<Tutorial> ().UpdateTutorial ();
		});

	}



}
