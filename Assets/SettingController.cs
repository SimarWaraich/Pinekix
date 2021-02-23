using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingController : Singleton<SettingController> {

	public Toggle SoundEffectToggle;
	public Toggle MusicToggle;
	public Toggle NotificationToggle;
	public GameObject ParentalButton;
	public Sprite[] ParentalImage;
	// Use this for initialization
	void Start () {
		ParentalButton.GetComponent<Image>().sprite = ParentalImage[0];
	}
	
	public void OpenParantelScreen()
	{
		ParentalButton.GetComponent<Image>().sprite = ParentalImage[1];
		ScreenAndPopupCall.Instance.OpenParenteralScreen ();
	}

	public void SoundToggleFunction()
	{
		if(SoundEffectToggle.isOn)
		{
			print ("SoundMusicOn");
		}else
			print ("SoundMusicOFF");
	}

	public void MusicToggleFunction()
	{
		if(MusicToggle.isOn)
		{
			print ("MusicOn");
		}else
			print ("MusicOFF");
	}

	public void nitificationToggleFunction()
	{
		if(NotificationToggle.isOn)
		{
			print ("NotificationMusicOn");
		}else
			print ("NotificationMusicOFF");
	}
}
