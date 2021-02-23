using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingGameAssets : MonoBehaviour
{

	public Slider slide;
	public float time = 1f;
	public int Ttime = 0;
	public GameObject LoginPanel;
	public GameObject RegistrationPanel;
	public GameObject ForgotPasswordPanel;
	public GameObject Character_SelectionPanel;
	public GameObject LodingScreen;

	public bool loading_On_Registration;
	public bool loading_On_CharSelection;
	public static bool CharSelectionsConfrim;
	public Text loadingValue;
	public GameObject MainBg;


	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (this.transform.root.gameObject);
	}

	// Update is called once per frame
	void Update ()
	{	
		if (loading_On_Registration) {
			if (Ttime <= 100) {
				Ttime++;
				slide.value = Ttime;
				loadingValue.text = "Loading..." + slide.value.ToString () + "%";

			} 
		} else {
			
//			UpdateAccordingToDownloading ();
//			UpdateAccordingToDownloading1 ();
		}
		if (loading_On_Registration) {

			if (slide.value >= 99) {
				slide.value = 0;
				Ttime = 0;
				loadingValue.text = "Loading..." + slide.value.ToString () + "%";
				RegistrationPanel.SetActive (false);
				LoginPanel.SetActive (false);
				ForgotPasswordPanel.SetActive (false);
				//PinekixRegistration.MessageText.gameObject.SetActive (false);
				Camera.main.GetComponent<PinekixRegistration> ().ShowCharacterCustomizationPanel ();
//				CharacterCustomizationAtStart.Instance.ShowConfirmationPopUpParentel (true);
				Character_SelectionPanel.SetActive (true);
				LodingScreen.SetActive (false);
				//				MainBg.SetActive (false);

			}
		}

//		if (loading_On_CharSelection) {
//
//			if (slide.value >= 99) {				
//				RegistrationPanel.SetActive (false);
//				LoginPanel.SetActive (false);
//				ForgotPasswordPanel.SetActive (false);
//				//				PinekixRegistration.MessageText.gameObject.SetActive (false);
//				Character_SelectionPanel.SetActive (false);
//				this.gameObject.SetActive (false);
//				Destroy (this.transform.root.gameObject);
//
//
//			}
//		}
	}



	void UpdateAccordingToDownloading ()
	{
		if (Ttime <= 60) {
			Ttime++;
			slide.value = Ttime;
			loadingValue.text = "Loading..." + slide.value.ToString () + "%";

		} else {
			CharSelectionsConfrim = true;


			while (SceneManager.GetActiveScene ().buildIndex != 1)
				return;

			if (Ttime <= 99 && DownloadContent.Instance.downloadcompleted) {
				Ttime++;
				slide.value = Ttime;
				loadingValue.text = "Loading..." + slide.value.ToString () + "%";
//				MainBg.SetActive (false);

			}
		}
	}

	void UpdateAccordingToDownloading1 ()
	{
//		slide.value = DownloadContent.Instance.percentege;
//		loadingValue.text = Mathf.Round (slide.value).ToString ().Trim ('.') + "%";
		if (loading_On_CharSelection) {
			
			if (slide.value <= 99) {				
				slide.value = DownloadContent.Instance.percentege;
				loadingValue.text = "Loading..." + Mathf.Round (slide.value).ToString ().Trim ('.') + "%";
			
			} else {
				CharSelectionsConfrim = true;

			}
			while (SceneManager.GetActiveScene ().buildIndex != 1)
				return;

			if (slide.value >= 99 && DownloadContent.Instance.downloadcompleted) {
				RegistrationPanel.SetActive (false);
				LoginPanel.SetActive (false);
				ForgotPasswordPanel.SetActive (false);
				//				PinekixRegistration.MessageText.gameObject.SetActive (false);
				Character_SelectionPanel.SetActive (false);
				this.gameObject.SetActive (false);
				Destroy (this.transform.root.gameObject);	
//				MainBg.SetActive (false);

			}
		}
	}

	public void ActiveLoading_OnRegistration ()
	{
		loading_On_Registration = true;
		loading_On_CharSelection = false;
	}

	public void ActiveLoading_OnCharSelectionConfrim ()
	{
		loading_On_Registration = false;
		loading_On_CharSelection = true;
	}
}