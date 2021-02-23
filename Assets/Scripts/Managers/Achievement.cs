using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Achievement : MonoBehaviour
{

	public AchievementData ThisAchievement;
	// Use this for initialization
	public Text NameText;
	public Text DescriptionText;
	public Image MedalIcon;
	public Image SlideBar;
	public Text Count;
	public Button CompleteIcon;

	void Start ()
	{
		
		gameObject.name = ThisAchievement.Aname + ThisAchievement.AchievementName + ThisAchievement.Medal;
		NameText.text = ThisAchievement.AchievementName;
		DescriptionText.text = ThisAchievement.Description;
		if (ThisAchievement.IsCompleted) {
			Count.text = "COMPLETED";
			CompleteIcon.gameObject.SetActive (true);
			CompleteIcon.onClick.AddListener (OnClickSetAchievement);
			if (ThisAchievement.Aname + ThisAchievement.AchievementName + ThisAchievement.Medal == PlayerPrefs.GetString ("CurrentAchievementMedal")) {
				CompleteIcon.interactable = false;
			} else
				CompleteIcon.interactable = true;
		} else {
			Count.text = ThisAchievement.CurrentCount + "/" + ThisAchievement.MaxCount;
			CompleteIcon.gameObject.SetActive (false);
		}
		float Value = ThisAchievement.CurrentCount / ThisAchievement.MaxCount;
		SlideBar.fillAmount = Value;

//		if (ThisAchievement.Medal == AchievementMedals.GoldMedal) {
//			MedalIcon.sprite = AchievementsManager.Instance.Medals [0];
//		} else if (ThisAchievement.Medal == AchievementMedals.SilverMedal) {
//			MedalIcon.sprite = AchievementsManager.Instance.Medals [1];
//		} else if (ThisAchievement.Medal == AchievementMedals.BronzeMedal) {
//			MedalIcon.sprite = AchievementsManager.Instance.Medals [2];
//		}
		for (int i = 0; i < AchievementsManager.Instance.MedalList.Count; i++) {
			if (ThisAchievement.Aname == AchievementsManager.Instance.MedalList [i].MadelName) {
				if (ThisAchievement.Medal == AchievementMedals.GoldMedal)
					MedalIcon.sprite = AchievementsManager.Instance.MedalList [i].Gold;
				else if (ThisAchievement.Medal == AchievementMedals.SilverMedal)
					MedalIcon.sprite = AchievementsManager.Instance.MedalList [i].Silver;
				else if (ThisAchievement.Medal == AchievementMedals.BronzeMedal)
					MedalIcon.sprite = AchievementsManager.Instance.MedalList [i].Bronze;
			}
		}
	}

	void OnClickSetAchievement ()
	{
		CompleteIcon.interactable = false;
		string previousAchievement = PlayerPrefs.GetString ("CurrentAchievementMedal");
		if (previousAchievement != "") {
			var Go = AchievementsManager.Instance.AchievementContainer.FindChild (previousAchievement);
			if (Go != null) {
				Go.GetComponent<Achievement> ().CompleteIcon.interactable = true;
			}
		}
		ShowUserProfile.Instance.MedalIcon.sprite = MedalIcon.sprite;
		PlayerPrefs.SetString ("CurrentAchievementMedal", ThisAchievement.Aname + ThisAchievement.AchievementName + ThisAchievement.Medal);
		AchivementSeletedPopUp ("You have selected " + ThisAchievement.Medal + " of " + ThisAchievement.AchievementName + " achievement on your profile screen.");
		ScreenAndPopupCall.Instance.AchievementsScreenClose ();
		StartCoroutine (DownloadContent.Instance.UpdateData ());
	}

	public void AchivementSeletedPopUp (string message)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			ScreenAndPopupCall.Instance.AchievementsScreenClose ();
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			ScreenAndPopupCall.Instance.AchievementsScreenClose ();
		});
	}
}

public enum AchievementMedals
{
	BronzeMedal,
	SilverMedal,
	GoldMedal,
	SpecialMedal
}

[Serializable]
public class AchievementData
{
	public string Aname;
	public string AchievementName;
	public string Description;
	public AchievementMedals Medal;
	public float CurrentCount;
	public float MaxCount;
	public bool IsCompleted;

	public AchievementData (string AchiveName, string name, string description, AchievementMedals medal, int count, int maxCount, bool isCompleted)
	{
		Aname = AchiveName;
		AchievementName = name;
		Description = description;
		Medal = medal;
		CurrentCount = count;
		MaxCount = maxCount;
		IsCompleted = isCompleted;
	}
}

[Serializable]
public class AchievementMedalsSprite
{
	public String MadelName;
	public Sprite Gold;
	public Sprite Silver;
	public Sprite Bronze;

}
