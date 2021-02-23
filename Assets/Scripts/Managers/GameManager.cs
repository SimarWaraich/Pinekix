/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 07 July 2k16
﻿/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 07 July 2k16
/// This script will be used to manage the game play in the device locally
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GenderEnum
{
	Female = 0,
	Male = 1
}

public class GameManager : Singleton<GameManager>
{

	#region PUBLIC_DATA

	public List<GameObject> WayPoints = new List<GameObject> ();
	public List<GameObject> FlatPartyWayPoints = new List<GameObject> ();
	public List<GameObject> SocietyPartyWayPoints = new List<GameObject> ();

	public int level;
	public float XpsbarLevel;
	//Test
	public float exPoint;
	public float maxlevelVal;
	//
	public int Gems;

	public Text CoinCountText;
	public Text GemsCountText;
	public Text UsernameText;
	public Text LevelNumberText;
	public Image ExperienceBar;
	public Text ExperienceBarText;

    public Sprite MaleIcon;
    public Sprite FemaleIcon;
    public Image UserIconImage;
	public bool IsTutorialActive;
	public GameObject MovableMoneyIcon;

	#endregion

	void Awake ()
	{
		this.Reload ();
//		InvokeRepeating ("CheckConnection", 1f, 1f);
//		AddGems (1000);

//		if (!PlayerPrefs.HasKey ("Gems"))
        UserIconImage.sprite = GetGender() == GenderEnum.Male ? MaleIcon : FemaleIcon;
	}

	void Start ()
	{
		UsernameText.text = PlayerPrefs.GetString ("UserName");
		Gems = PlayerPrefs.GetInt ("Gems");
		GemsCountText.text = Gems.ToString ();
		var playermoney = PlayerManager.Instance.MainCharacter.AddComponent<GenerateMoney> ();
		playermoney.MoneyIcon = playermoney.gameObject.transform.FindChild ("low money").gameObject;
		playermoney.MaxMoney = 40;
		if (PlayerPrefs.HasKey ("Tutorial_Progress")) {
			if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 25)
				this.gameObject.GetComponent <Tutorial> ().enabled = false;
		}
		if (!PlayerPrefs.HasKey ("Level")) {
			PlayerPrefs.SetInt ("Level", 0);
//			AddCoins (1000);
		}
		level = PlayerPrefs.GetInt ("Level");
		XpsbarLevel = PlayerPrefs.GetFloat ("ExperiencePoints");

		LevelNumberText.text = level.ToString ();
		if (level == 0) {			
			Invoke ("EnableTutorial", 0.5f);
		} else {
			this.GetComponent<Tutorial> ().Start ();
//			this.gameObject.GetComponent <Tutorial> ().enabled = false;
		}
		ExperienceBar.fillAmount = GetExperienceBasedOnLevel ();
		ExperienceBarText.text = GetExperienceBasedOnLevelString ();
//		if (ExperienceBar.fillAmount == 1f) {
//			ExperienceBar.fillAmount = 0.0f;
//		}
        CoinCountText.text = PlayerPrefs.GetInt ("Money").ToString ();

	}


	public void EnableTutorial ()
	{
		this.gameObject.GetComponent<Tutorial> ().enabled = true;
	}

	public void AddCoins (int Coins)
	{
		StartCoroutine (SetLabelAnimation (CoinCountText, PlayerPrefs.GetInt ("Money"), PlayerPrefs.GetInt ("Money") + Coins));

		PlayerPrefs.SetInt ("Money", PlayerPrefs.GetInt ("Money") + Coins);
//		CoinCountText.text = PlayerPrefs.GetInt ("Money").ToString ();
		DownloadContent.Instance.StartCoroutine (DownloadContent.Instance.UpdateData ());
	}

	IEnumerator SetLabelAnimation (Text Label, int From, int To)
	{
		float dist = To - From;
//        var x = 0.0f;
//        float strt = Time.time;
//
//        for(int i = From; i< To; i++)
//        {
//            Label.text = i.ToString();
//            yield return new WaitForSeconds(time);
//            x += time;
//        }
//        float end = Time.time;
		Label.text = Mathf.CeilToInt (iTween.FloatUpdate (From, To, 0.1f)).ToString ();
		yield return null;
		Label.text = PlayerPrefs.GetInt ("Money").ToString ();
//        Debug.LogError("Total Time.time - " + (end - strt).ToString());
//        Debug.LogError("Total Time taken was - "+ x);
	}

	int exampleInt = 0;

	void tweenOnUpdateCallBack (int newValue)
	{
		exampleInt = newValue;
		Debug.Log (exampleInt);
	}


	public IEnumerator CoinsCollectionAnimation (int Coins, Vector3 position)
	{
		for (int i = 0; i < 4; i++) {
			var TargetPos = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 2, Screen.height, 0));
			var Go = Instantiate (MovableMoneyIcon, position, Quaternion.identity) as GameObject;

			iTween.MoveTo (Go, iTween.Hash ("position", TargetPos, "time", 0.8f, "easeType", "easeInOutCubic"));
			StartCoroutine (DestroyCoins (Go, 0.5f));
			yield return new WaitForSeconds (0.1f);
		}
	}

	IEnumerator DestroyCoins (GameObject Go, float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		Destroy (Go);
	}

	public void SubtractCoins (int Coins)
	{
		PlayerPrefs.SetInt ("Money", PlayerPrefs.GetInt ("Money") - Coins);
		CoinCountText.text = PlayerPrefs.GetInt ("Money").ToString ();
		DownloadContent.Instance.StartCoroutine (DownloadContent.Instance.UpdateData ());
	}

	bool popUpShowed = false;

	public void ChangeLevel ()
	{
//		if (level == 0) {
//			GetComponent <Tutorial> ().enabled = false;
//		}

		level = PlayerPrefs.GetInt ("Level");
		level += 1;
		PlayerPrefs.SetInt ("Level", level);
		LevelNumberText.text = level.ToString ();
//		ExperienceBar.fillAmount = GetExperienceBasedOnLevel ();
//		ExperienceBarText.text = GetExperienceBasedOnLevelString ();


//		if (ExperienceBar.fillAmount == 1f) {
//			ExperienceBar.fillAmount = 0.0f;
//		}
//			Instance.GetComponent<Tutorial> ().UpdateTutorial ();
		DownloadContent.Instance.StartCoroutine (DownloadContent.Instance.UpdateData ());
		if (!popUpShowed)
			ShowLevelUpPopup ();
	}

	/// <summary>
	/// This function is for Gem update, substract & addition
	/// </summary>

	public void AddGems (int GemsAdd)
	{
		Gems = PlayerPrefs.GetInt ("Gems") + GemsAdd;
		PlayerPrefs.SetInt ("Gems", Gems);
		GemsCountText.text = PlayerPrefs.GetInt ("Gems").ToString ();
		DownloadContent.Instance.StartCoroutine (DownloadContent.Instance.UpdateData ());
	}

	public void SubtractGems (int GemsSubtract)
	{		
		Gems = PlayerPrefs.GetInt ("Gems") - GemsSubtract;
		PlayerPrefs.SetInt ("Gems", Gems);
		GemsCountText.text = PlayerPrefs.GetInt ("Gems").ToString ();
		DownloadContent.Instance.StartCoroutine (DownloadContent.Instance.UpdateData ());
	}

	public void AddGemsWithGemBonus (int gempoint)
	{
		var seletedFlatmate = RoommateManager.Instance.SelectedRoommate.GetComponent<Flatmate> ();
		int perkValue = seletedFlatmate.PerkValue;
		float Newgempoint = 0f;
		int TempPerkValue = 0;
		if (VipSubscriptionManager.Instance.VipSubscribed) {
			if (seletedFlatmate.data.Perk == "Gem Bonus") {
				if (perkValue == 1)
					TempPerkValue = 0;
				else if (perkValue == 2)
					TempPerkValue = 3;
				else if (perkValue == 3)
					TempPerkValue = 5;
				else if (perkValue == 4)
					TempPerkValue = 9;
				else if (perkValue == 5)
					TempPerkValue = 15;
				else if (perkValue == 6)
					TempPerkValue = 20;
						
				Newgempoint = gempoint * TempPerkValue / 100;
			}
		}
		gempoint = Mathf.RoundToInt (Newgempoint) + gempoint;
		GameManager.Instance.AddGems (gempoint);
	}

	public void ShowLevelUpPopup ()
	{        
		popUpShowed = true;
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.UniPopup);

		ScreenManager.Instance.UniPopup.transform.FindChild ("Message").GetComponent<Text> ().text = "Wooohoooo!!! \n Level Up!!!";
		ScreenManager.Instance.UniPopup.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.UniPopup.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => { 
			ScreenManager.Instance.ClosePopup ();		
			if (PlayerPrefs.GetInt ("Tutorial_Progress") < 26)
				GetComponent<Tutorial> ().UpdateTutorial ();
			popUpShowed = false;
			if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
				AchievementsManager.Instance.CheckAchievementsToUpdate ("realPlayerLevel");
		});
	}

	public static GenderEnum GetGender ()
	{		
		if (PlayerPrefs.GetString ("CharacterType") == "Male")
			return GenderEnum.Male;
		else
			return GenderEnum.Female;
	}

	float GetMaxXPperLevel ()
	{
		if (level == 0)
			return 80;
		else if (level == 1)
			return 100;
		else {
			return 100.0f * level * 0.7f;
		}
		return 100f;
	}

	public void AddExperiencePoints (float points)
	{	
//		PlayerPrefs.SetInt ("Level", 0);
		level = PlayerPrefs.GetInt ("Level");
//		points = points * VipSubscriptionManager.Instance.VipSubcritionActived ();
//		float periviosXp = PlayerPrefs.GetFloat ("ExperiencePoints" + level);
		float periviosXp = XpsbarLevel;
		exPoint = points;
		float MaxXp = GetMaxXPperLevel ();
		maxlevelVal = MaxXp;
		if ((periviosXp + points) >= MaxXp) {
			float diff = MaxXp - periviosXp;
			PlayerPrefs.SetFloat ("ExperiencePoints" /*+ level*/, diff);
			XpsbarLevel = 0;
			ChangeLevel ();
			points = points - diff;
//			print (points);
			AddExperiencePoints (points);
			exPoint = points;
			maxlevelVal = MaxXp;
		} else {
//			print (points);
			XpsbarLevel += points;
			PlayerPrefs.SetFloat ("ExperiencePoints", XpsbarLevel);
//		PlayerPrefs.SetFloat ("ExperiencePoints" + level, PlayerPrefs.GetFloat ("ExperiencePoints" + level) + points);
//		var last = ExperienceBar.fillAmount;
			ExperienceBar.fillAmount = GetExperienceBasedOnLevel ();
			ExperienceBarText.text = GetExperienceBasedOnLevelString ();		
			DownloadContent.Instance.StartCoroutine (DownloadContent.Instance.UpdateData ());			
		}
	}


	float GetExperienceBasedOnLevel ()
	{
		level = PlayerPrefs.GetInt ("Level");
//		if (PlayerPrefs.HasKey ("ExperiencePoints" + level)) {
//			float Xp = PlayerPrefs.GetFloat ("ExperiencePoints" + level);
//			return Xp / GetMaxXPperLevel ();
//		} else {
//			PlayerPrefs.SetFloat ("ExperiencePoints" + level, 0);
//			return 0;
//		}
		return XpsbarLevel / GetMaxXPperLevel ();

	}

	string GetExperienceBasedOnLevelString ()
	{
		level = PlayerPrefs.GetInt ("Level");
		if (PlayerPrefs.HasKey ("ExperiencePoints" /*+ level*/)) {
//			float Xp = PlayerPrefs.GetFloat ("ExperiencePoints" /*+ level*/);

			return string.Format ("{0}/ {1}", XpsbarLevel, GetMaxXPperLevel ());
		} else {
			PlayerPrefs.SetFloat ("ExperiencePoints"/*+ level*/, 0);
			return "0";
		}
	}
}