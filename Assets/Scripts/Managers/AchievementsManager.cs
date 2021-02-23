using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simple_JSON;
using System.Linq;
using UnityEngine.UI;

public class AchievementsManager : Singleton<AchievementsManager>
{

	public GameObject AchievementPrefab;
	public Transform AchievementContainer;

	public int flatmateRecruited = 0;
	public int completeUniversityClasses = 0;
	public int realPlayerLevel = 0;
	public int flatsUpgrade = 0;
	public int furnitureAcquisition = 0;
	public int clothesAcquisition = 0;
	public int enterUniversityEvents = 0;
	public int winUniversityEvents = 0;
	public int voteForFriends = 0;
	public int placementRank_CatwalkEvent = 0;
	public int placementRank_SocietyEvent = 0;
	public int completeQuests = 0;
	public int visitMultiplayerAreas = 0;
	public int hostFlatParties = 0;
	public int attendSocietyParties = 0;

	public Sprite[] Medals;
	public List<AchievementMedalsSprite> MedalList = new List<AchievementMedalsSprite> ();

	public const string LinkToUpdateAchievements = "http://pinekix.ignivastaging.com/achievement/achievementsCount";
	public const string LinkToGetUpdatedAchievements = "http://pinekix.ignivastaging.com/achievement/getachievementsCount";
	public List<AchievementData> AllAchievements = new List<AchievementData> ();

	void Awake ()
	{
		this.Reload ();
	}
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (GetUpdatedAchievements ());
		AchievementsManager.Instance.GenerateAllAchivementsList ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void CheckAchievementsToUpdate (string achievementType)
	{
		switch (achievementType) {
		case "flatmateRecruited":
			if (flatmateRecruited <= 25) {
				flatmateRecruited += 1;
				if (flatmateRecruited == 10)
					AchivementCompleatePopUpMsg ("You have won bronze medal for flatmate recruited achievement.");
				else if (flatmateRecruited == 15)
					AchivementCompleatePopUpMsg ("You have won silver medal for flatmate recruited achievement.");
				else if (flatmateRecruited == 25)
					AchivementCompleatePopUpMsg ("You have won gold medal for flatmate recruited achievement");
			}			
			break;
		case "completeUniversityClasses":
			if (completeUniversityClasses <= 150) {
				completeUniversityClasses += 1;
				if (completeUniversityClasses == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for complete university classes achievement.");
				else if (completeUniversityClasses == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for complete university classes achievement.");
				else if (completeUniversityClasses == 150)
					AchivementCompleatePopUpMsg ("You have won gold medal for complete university classes achievement");
			}
			break;
		case "realPlayerLevel":
			if (realPlayerLevel <= 20) {
				realPlayerLevel = GameManager.Instance.level;
				if (realPlayerLevel == 5)
					AchivementCompleatePopUpMsg ("You have won bronze medal for real player level achievement.");
				else if (realPlayerLevel == 10)
					AchivementCompleatePopUpMsg ("You have won silver medal for real player level achievement.");
				else if (realPlayerLevel == 20)
					AchivementCompleatePopUpMsg ("You have won gold medal for real player level achievement");
			}
			break;
		case "flatsUpgrade":
			if (flatsUpgrade <= 10) {
				flatsUpgrade += 1;
				if (flatsUpgrade == 2)
					AchivementCompleatePopUpMsg ("You have won bronze medal for flats upgrade achievement.");
				else if (flatsUpgrade == 5)
					AchivementCompleatePopUpMsg ("You have won silver medal for flats upgrade achievement.");
				else if (flatsUpgrade == 10)
					AchivementCompleatePopUpMsg ("You have won gold medal for flats upgrade achievement");
			}
			break;
		case "furnitureAcquisition":
			if (furnitureAcquisition <= 100) {
				furnitureAcquisition += 1;
				if (furnitureAcquisition == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for furniture acquisition achievement.");
				else if (furnitureAcquisition == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for furniture acquisition achievement.");
				else if (furnitureAcquisition == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for furniture acquisition achievement");
			}
			break;
		case "clothesAcquisition":
			if (clothesAcquisition <= 100) {
				clothesAcquisition += 1;
				if (clothesAcquisition == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for clothes acquisition achievement.");
				else if (clothesAcquisition == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for clothes acquisition achievement.");
				else if (clothesAcquisition == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for clothes acquisition achievement");
			}
			break;
		case "enterUniversityEvents":
			if (enterUniversityEvents <= 100) {
				enterUniversityEvents += 1;
				if (enterUniversityEvents == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for enter university events achievement.");
				else if (enterUniversityEvents == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for enter university events achievement.");
				else if (enterUniversityEvents == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for enter university events achievement");
			}
			break;
		case "winUniversityEvents":
			if (winUniversityEvents <= 100) {
				winUniversityEvents += 1;
				if (winUniversityEvents == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for win university events achievement.");
				else if (winUniversityEvents == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for win university events achievement.");
				else if (winUniversityEvents == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for win university events achievement.");
			}
			break;
		case "voteForFriends":
			if (voteForFriends <= 100) {
				voteForFriends += 1;
				if (voteForFriends == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for vote for friends achievement.");
				else if (voteForFriends == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for vote for friends achievement.");
				else if (voteForFriends == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for vote for friends achievement.");
			}
			break;
		case "placementRank_CatwalkEvent":
			///TODO: after client confirmation 
			if (placementRank_CatwalkEvent <= 1) {
				placementRank_CatwalkEvent += 1;
				if (completeUniversityClasses >= 1)
					AchivementCompleatePopUpMsg ("You have won special medal for placement rank catwalk event achievement.");				
			}
			break;
		case "placementRank_SocietyEvent":
			///TODO: after client confirmation 
			if (placementRank_SocietyEvent <= 1) {
				placementRank_SocietyEvent += 1;
				if (completeUniversityClasses >= 1)
					AchivementCompleatePopUpMsg ("You have won special medal for placement rank society event achievement.");
				
			}
			break;
		case "completeQuests":
			if (completeQuests <= 100) {
				completeQuests += 1;
				if (completeQuests == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for complete quests achievement.");
				else if (completeQuests == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for complete quests achievement.");
				else if (completeQuests == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for complete quests achievement");
			}
			break;
		case "visitMultiplayerAreas":
			if (visitMultiplayerAreas <= 100) {
				visitMultiplayerAreas += 1;
				if (visitMultiplayerAreas == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for visit multiplayer areas achievement.");
				else if (visitMultiplayerAreas == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for visit multiplayer areas achievement.");
				else if (visitMultiplayerAreas == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for visit multiplayer areas achievement");
			}
			break;
		case "hostFlatParties":
			if (hostFlatParties <= 100) {
				hostFlatParties += 1;
				if (hostFlatParties == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for host flat parties achievement.");
				else if (hostFlatParties == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for host flat parties achievement.");
				else if (hostFlatParties == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for host flat parties achievement");
			}
			break;
		case "attendSocietyParties":
			if (attendSocietyParties <= 100) {
				attendSocietyParties += 1;
				if (completeUniversityClasses == 20)
					AchivementCompleatePopUpMsg ("You have won bronze medal for attend society parties achievement.");
				else if (attendSocietyParties == 50)
					AchivementCompleatePopUpMsg ("You have won silver medal for attend society parties achievement.");
				else if (attendSocietyParties == 100)
					AchivementCompleatePopUpMsg ("You have won gold medal for attend society parties achievement");
			}
			break;
		}
		StartCoroutine (AddAchievementsToTheServer ());
	}

	public void GetAllAchivmentList ()
	{
		StartCoroutine (GetUpdatedAchievements ());
	}

	IEnumerator AddAchievementsToTheServer ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["flatmate_recruit"] = flatmateRecruited.ToString ();
		jsonElement ["complete_university_class"] = completeUniversityClasses.ToString ();
		jsonElement ["real_player_level"] = realPlayerLevel.ToString ();
		jsonElement ["flat_upgrades"] = flatsUpgrade.ToString ();
		jsonElement ["furniture_acquisition"] = furnitureAcquisition.ToString ();
		jsonElement ["clothing_acquisition"] = clothesAcquisition.ToString ();
		jsonElement ["enter_university_events"] = enterUniversityEvents.ToString ();
		jsonElement ["win_university_events"] = winUniversityEvents.ToString ();
		jsonElement ["vote_for_friends"] = voteForFriends.ToString ();
		jsonElement ["placement_rank_catwalk"] = placementRank_CatwalkEvent.ToString ();
		jsonElement ["placement_rank_society"] = placementRank_SocietyEvent.ToString ();
		jsonElement ["complete_quests"] = completeQuests.ToString ();
		jsonElement ["visit_multiplayer_area"] = visitMultiplayerAreas.ToString ();
		jsonElement ["host_flat_parties"] = hostFlatParties.ToString ();
		jsonElement ["attend_society_parties"] = attendSocietyParties.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (LinkToUpdateAchievements, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Your achievement data has updated successfully.")) {
//				var data = _jsnode["data"] [0];
//				int.TryParse(data["flatmate_recruit"], out flatmateRecruited);
//				int.TryParse(data["complete_university_class"], out completeUniversityClasses);
//				int.TryParse(data["real_player_level"], out realPlayerLevel);
//				int.TryParse(data["flat_upgrades"], out flatsUpgrade);
//				int.TryParse(data["furniture_acquisition"], out furnitureAcquisition);
//				int.TryParse(data["clothing_acquisition"], out clothesAcquisition);
//				int.TryParse(data["enter_university_events"], out enterUniversityEvents);
//				int.TryParse(data["win_university_events"], out winUniversityEvents);
//				int.TryParse(data["vote_for_friends"], out voteForFriends);
//				int.TryParse(data["complete_quests"], out completeQuests);
//				int.TryParse(data["visit_multiplayer_area"], out visitMultiplayerAreas);
//				int.TryParse(data["host_flat_parties"], out hostFlatParties);
//				int.TryParse(data["attend_society_parties"], out attendSocietyParties);
//				int.TryParse(data["placement_rank_catwalk"], out placementRank_CatwalkEvent);
//				int.TryParse(data["placement_rank_society"], out placementRank_SocietyEvent);
			} else {
				
			}
		}
	}

	IEnumerator GetUpdatedAchievements ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (LinkToGetUpdatedAchievements, encoding.GetBytes (jsonElement.ToString ()), postHeader);

//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200") && _jsnode ["description"].ToString ().Contains ("Achievements count are following.")) {
//				AchievementData data = new AchievementData ("Ankush", 1, 1, 1, 1);
				var data = _jsnode ["data"] [0];
				int.TryParse (data ["flatmate_recruit"], out flatmateRecruited);
				int.TryParse (data ["complete_university_class"], out completeUniversityClasses);
				int.TryParse (data ["real_player_level"], out realPlayerLevel);
				int.TryParse (data ["flat_upgrades"], out flatsUpgrade);
				int.TryParse (data ["furniture_acquisition"], out furnitureAcquisition);
				int.TryParse (data ["clothing_acquisition"], out clothesAcquisition);
				int.TryParse (data ["enter_university_events"], out enterUniversityEvents);
				int.TryParse (data ["win_university_events"], out winUniversityEvents);
				int.TryParse (data ["vote_for_friends"], out voteForFriends);
				int.TryParse (data ["complete_quests"], out completeQuests);
				int.TryParse (data ["visit_multiplayer_area"], out visitMultiplayerAreas);
				int.TryParse (data ["host_flat_parties"], out hostFlatParties);
				int.TryParse (data ["attend_society_parties"], out attendSocietyParties);
				int.TryParse (data ["placement_rank_catwalk"], out placementRank_CatwalkEvent);
				int.TryParse (data ["placement_rank_society"], out placementRank_SocietyEvent);

//				CheckAchievementsToUpdate ("flatmateRecruited");
			} else {

			}
		}
	}

	public void GenerateAllAchivementsList ()
	{
		for (int i = 0; i < AchievementContainer.childCount; i++) {
			Destroy (AchievementContainer.GetChild (i).gameObject);
		}
		InstantiateAchievement ("flatmateRecruited", "The more the merrier!", "Recruit 25 Flatmates to Get Gold Coin", AchievementMedals.GoldMedal, flatmateRecruited, 25, flatmateRecruited >= 25);
		InstantiateAchievement ("flatmateRecruited", "The more the merrier!", "Recruit 15 Flatmates to Get Silver Medal", AchievementMedals.SilverMedal, flatmateRecruited, 15, flatmateRecruited >= 15);
		InstantiateAchievement ("flatmateRecruited", "The more the merrier!", "Recruit 10 Flatmates to Get Bronze Medal", AchievementMedals.BronzeMedal, flatmateRecruited, 10, flatmateRecruited >= 10);
		InstantiateAchievement ("completeUniversityClasses", "Knowledge seeker", "Complete 150 University Classes to Get Gold Medal", AchievementMedals.GoldMedal, completeUniversityClasses, 150, completeUniversityClasses >= 150);
		InstantiateAchievement ("completeUniversityClasses", "Knowledge seeker", "Complete 50 University Classes to Get Silver Medal", AchievementMedals.SilverMedal, completeUniversityClasses, 50, completeUniversityClasses >= 50);
		InstantiateAchievement ("completeUniversityClasses", "Knowledge seeker", "Complete 20 University Classes to Get Bronze Medal", AchievementMedals.BronzeMedal, completeUniversityClasses, 20, completeUniversityClasses >= 20);
		InstantiateAchievement ("realPlayerLevel", "Level up!", "Reach XP 20 to Get Gold Medal", AchievementMedals.GoldMedal, realPlayerLevel, 20, realPlayerLevel >= 20);
		InstantiateAchievement ("realPlayerLevel", "Level up!", "Reach XP 10 to Get Silver Medal", AchievementMedals.SilverMedal, realPlayerLevel, 10, realPlayerLevel >= 10);
		InstantiateAchievement ("realPlayerLevel", "Level up!", "Reach XP 5 to Get Bronze Medal", AchievementMedals.BronzeMedal, realPlayerLevel, 5, realPlayerLevel >= 5);
		InstantiateAchievement ("flatsUpgrade", "Architect", "Build 10 Additional Rooms to Get Gold Medal", AchievementMedals.GoldMedal, flatsUpgrade, 10, flatsUpgrade >= 10);
		InstantiateAchievement ("flatsUpgrade", "Architect", "Build 5 Additional Rooms to Get Silver Medal", AchievementMedals.SilverMedal, flatsUpgrade, 5, flatsUpgrade >= 5);
		InstantiateAchievement ("flatsUpgrade", "Architect", "Build 2 Additional Rooms to Get Bronze Medal", AchievementMedals.BronzeMedal, flatsUpgrade, 2, flatsUpgrade >= 2);
		InstantiateAchievement ("furnitureAcquisition", "Acquisition", "Acquisition of 100 pieces of Furniture to Get Gold Medal", AchievementMedals.GoldMedal, furnitureAcquisition, 100, furnitureAcquisition >= 100);
		InstantiateAchievement ("furnitureAcquisition", "Acquisition", "Acquisition of 50 pieces of Furniture to Get Silver Medal", AchievementMedals.SilverMedal, furnitureAcquisition, 50, furnitureAcquisition >= 50);
		InstantiateAchievement ("furnitureAcquisition", "Acquisition", "Acquisition of 20 pieces of Furniture to Get Bronze Medal", AchievementMedals.BronzeMedal, furnitureAcquisition, 20, furnitureAcquisition >= 20);
		InstantiateAchievement ("clothesAcquisition", "Fashionista", "Acquisition of 100 pieces of Clothes to Get Gold Medal", AchievementMedals.GoldMedal, clothesAcquisition, 100, clothesAcquisition >= 100);
		InstantiateAchievement ("clothesAcquisition", "Fashionista", "Acquisition of 50 pieces of Clothes to Get Silver Medal", AchievementMedals.SilverMedal, clothesAcquisition, 50, clothesAcquisition >= 50);
		InstantiateAchievement ("clothesAcquisition", "Fashionista", "Acquisition of 20 pieces of Clothes to Get Bronze Medal", AchievementMedals.BronzeMedal, clothesAcquisition, 20, clothesAcquisition >= 20);
		InstantiateAchievement ("enterUniversityEvents", "Competitor", "Enter 100 University events to Get Gold Medal", AchievementMedals.GoldMedal, enterUniversityEvents, 100, enterUniversityEvents >= 100);
		InstantiateAchievement ("enterUniversityEvents", "Competitor", "Enter 50 University events to Get Silver Medal", AchievementMedals.SilverMedal, enterUniversityEvents, 50, enterUniversityEvents >= 50);
		InstantiateAchievement ("enterUniversityEvents", "Competitor", "Enter 20 University events to Get Bronze Medal", AchievementMedals.BronzeMedal, enterUniversityEvents, 20, enterUniversityEvents >= 20);
		InstantiateAchievement ("winUniversityEvents", "Challenger", "Win 100 University events to Get Gold Medal", AchievementMedals.GoldMedal, winUniversityEvents, 100, winUniversityEvents >= 100);
		InstantiateAchievement ("winUniversityEvents", "Challenger", "Win 50 University events to Get Silver Medal", AchievementMedals.SilverMedal, winUniversityEvents, 50, winUniversityEvents >= 50);
		InstantiateAchievement ("winUniversityEvents", "Challenger", "Win 20 University events to Get Bronze Medal", AchievementMedals.BronzeMedal, winUniversityEvents, 20, winUniversityEvents >= 20);
		InstantiateAchievement ("voteForFriends", "Friend in Need", "Vote for 100 friends to Get Gold Medal", AchievementMedals.GoldMedal, voteForFriends, 100, voteForFriends >= 100);
		InstantiateAchievement ("voteForFriends", "Friend in Need", "Vote for 50 friends to Get Silver Medal", AchievementMedals.SilverMedal, voteForFriends, 50, voteForFriends >= 50);
		InstantiateAchievement ("voteForFriends", "Friend in Need", "Vote for 20 friends to Get Bronze Medal", AchievementMedals.BronzeMedal, voteForFriends, 20, voteForFriends >= 20);
		InstantiateAchievement ("placementRank_CatwalkEvent", "Rank in Special cat-walk event", "Placement Rank in Special Cat-Walk Event", AchievementMedals.GoldMedal, placementRank_CatwalkEvent, 1, placementRank_CatwalkEvent >= 1);
		InstantiateAchievement ("placementRank_SocietyEvent", "Rank in Society event", "Placement Rank in Society Event", AchievementMedals.GoldMedal, placementRank_SocietyEvent, 1, placementRank_SocietyEvent >= 1);
		InstantiateAchievement ("visitMultiplayerAreas", "Traveller", "Visit 100 Multiplayer Areas to Get Gold Medal", AchievementMedals.GoldMedal, visitMultiplayerAreas, 100, visitMultiplayerAreas >= 100);
		InstantiateAchievement ("visitMultiplayerAreas", "Traveller", "Visit 50 Multiplayer Areas to Get Silver Medal", AchievementMedals.SilverMedal, visitMultiplayerAreas, 50, visitMultiplayerAreas >= 50);
		InstantiateAchievement ("visitMultiplayerAreas", "Traveller", "Visit 20 Multiplayer Areas to Get Bronze Medal", AchievementMedals.BronzeMedal, visitMultiplayerAreas, 20, visitMultiplayerAreas >= 20);
		InstantiateAchievement ("hostFlatParties", "Party Animal", "Host 100 Flat Parties to Get Gold Medal", AchievementMedals.GoldMedal, hostFlatParties, 100, hostFlatParties >= 100);
		InstantiateAchievement ("hostFlatParties", "Party Animal", "Host 50 Flat Parties to Get Silver Medal", AchievementMedals.SilverMedal, hostFlatParties, 50, hostFlatParties >= 50);
		InstantiateAchievement ("hostFlatParties", "Party Animal", "Host 20 Flat Parties to Get Bronze Medal", AchievementMedals.BronzeMedal, hostFlatParties, 20, hostFlatParties >= 20);
		InstantiateAchievement ("attendSocietyParties", "Socialite", "Attend 100 Society Parties to Get Gold Medal", AchievementMedals.GoldMedal, attendSocietyParties, 100, attendSocietyParties >= 100);
		InstantiateAchievement ("attendSocietyParties", "Socialite", "Attend 50 Society Parties to Get Silver Medal", AchievementMedals.SilverMedal, attendSocietyParties, 50, attendSocietyParties >= 50);
		InstantiateAchievement ("attendSocietyParties", "Socialite", "Attend 20 Society Parties to Get Bronze Medal", AchievementMedals.BronzeMedal, attendSocietyParties, 20, attendSocietyParties >= 20);


	}

	void InstantiateAchievement (string achivementName, string name, string description, AchievementMedals medal, int count, int maxCount, bool isCompleted)
	{
		GameObject Achievement_GameObject = (GameObject)Instantiate (AchievementPrefab, AchievementContainer);
		AchievementData data = new AchievementData (achivementName, name, description, medal, count, maxCount, isCompleted);
		AllAchievements.Add (data);
		Achievement_GameObject.GetComponent<Achievement> ().ThisAchievement = data;
		Achievement_GameObject.transform.localScale = Vector3.one;
		Achievement_GameObject.transform.localPosition = Vector3.zero;

	}

	List<AchievementData> GetCompletedAchieveMent ()
	{
		return	AllAchievements.Where (achievment => achievment.IsCompleted).ToList ();

	}

	public void AchivementCompleatePopUpMsg (string message)
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
		});
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
		});
	}
}
 