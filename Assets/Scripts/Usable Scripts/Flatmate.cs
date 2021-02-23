using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class Flatmate : MonoBehaviour
{

	public RoommateData data;
	public int PerkValue;
	//	public float Education_Level_Points;
	//	public int Education_Level = 0;
	public int DressId;

	Tutorial tutorial;

	void Start ()
	{
		/// this line is for to make character intractble with mouse 
		this.gameObject.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -1f);
		tutorial = GameManager.Instance.GetComponent<Tutorial> ();
	}

	public void HireThisRoommate ()
	{
		///NOTE: here we are describing the dress and perk
		data.Hired = true;
		List<GameObject> temp = new List<GameObject> ();
		for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
			temp.Add (RoommateManager.Instance.RoommatesHired [i]);
		}
		temp.Add (this.gameObject);
		RoommateManager.Instance.RoommatesHired = temp.ToArray ();

		if (PlayerPrefs.HasKey ("FlatMateCountTime" + data.Name + PlayerPrefs.GetInt ("PlayerId")) || data.IsBusy) {
//			if (PlayerPrefs.HasKey ("FlatMateCountTime" + data.Name+ PlayerPrefs.GetInt ("PlayerId"))) {
//				var Temp = Convert.ToInt64 (PlayerPrefs.GetString ("FlatMateCountTime" + data.Name + PlayerPrefs.GetInt ("PlayerId")));
//				RoommateManager.Instance.SelectedRoommate = this.gameObject;
//				var EndTime = DateTime.FromBinary (Temp);
//				var Diff = EndTime - DateTime.UtcNow;
//				RoommateManager.Instance.StartBusyTimerForSelectedRoomMate ((float)Diff.TotalSeconds);
//
//			} else {
			RoommateManager.Instance.SelectedRoommate = this.gameObject;
			var TotalMinutes = (float)(data.BusyTimeRemaining - DateTime.UtcNow).TotalSeconds;
			RoommateManager.Instance.StartBusyTimerForSelectedRoomMate (TotalMinutes);
//			}
		} else if (data.IsCoolingDown) {
			//			var Temp = Convert.ToInt64 (PlayerPrefs.GetString ("FlatMateCoolDownTime" + data.Name));
			//			RoommateManager.Instance.SelectedRoommate = this.gameObject;
			//			var EndTime = DateTime.FromBinary (Temp);
			//			if(data.CooldownTime != new DateTime() && data.CooldownTime > DateTime.UtcNow){
			var Diff = data.CooldownEndTime - DateTime.UtcNow;
			RoommateManager.Instance.SelectedRoommate = this.gameObject;
			RoommateManager.Instance.StartCooldownTimerForFlatmate ((float)Diff.TotalMinutes, this);
		}

		//		}
		this.gameObject.SetMaterialRecursively ();

		UpdateFlatmateOnServer ();
		if (GameManager.Instance.gameObject.GetComponent<Tutorial> ()._SofaPurchased)
			GameManager.Instance.gameObject.GetComponent<Tutorial> ().UpdateTutorial ();
	}



	public void UpdateFlatmateOnServer ()
	{
		RoommateSaveData _savedata = new RoommateSaveData ();

		_savedata.player_id = PlayerPrefs.GetInt ("PlayerId");
//		foreach (var downloaded_item in DownloadContent.Instance.downloaded_items) {
//			if (downloaded_item.Category.Trim ('"') == "Flatmates" && downloaded_item.SubCategory.Trim ('"') == "Flatmates" && downloaded_item.Name.Trim ('"') == data.Name)
//				_savedata.item_id = downloaded_item.Item_id;
//		}
		_savedata.item_id = data.Id;
		_savedata.name = data.Name;
		_savedata.gender = data.Gender.ToString ();
		_savedata.is_busy = data.IsBusy;

        if(data.IsBusy)
		_savedata.busy_time = data.BusyTimeRemaining.ToBinary ().ToString ();
        else
            _savedata.busy_time = "";        
		_savedata.education_point = data.education_level;
		_savedata.education_point_level = data.education_point;
		_savedata.perk = data.Perk;
		_savedata.perk_value = data.Perk_value;
		_savedata.hair_style = data.Hair_style;
        if(data.IsCoolingDown)
		_savedata.cooldown_time = data.CooldownEndTime.ToBinary ().ToString ();
        else
            _savedata.cooldown_time = "";   
        
		_savedata.busy_type = data.BusyType.ToString ();
		_savedata.cooldown_time_event_id = data.EventBusyId;

		var value = "";
		foreach (var dress in data.Dress) {
//          string category = "";
			if (dress.Key == "Hair")
//              category = "Clothes";
                _savedata.hair_style = dress.Value.ToString ();
			else
//              category = dress.Key;

//          var iD = FindDressId (category, dress.Value);
                value += "," + dress.Value;
		}
		_savedata.dress = value;
		StartCoroutine (UpdateData (_savedata));
	}

	IEnumerator UpdateData (RoommateSaveData _savedata)
	{
		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.UpdateFlatmate (_savedata));
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			print ("data saved");
		} else {
//			StartCoroutine (UpdateData (_savedata));
		}
		yield return null;
	}


	public void GivePerk ()
	{		
		int num = UnityEngine.Random.Range (0, RoommateManager.Instance.PerksName.Count);

		data.Perk = RoommateManager.Instance.PerksName [num];
		data.Perk_value = 1;
		PerkValue = 1;

	}

	public void UpgradePerk ()
	{
		if (data.Perk_value >= 6) {
			return;
		}

//		var num = ++PerkValue;
		data.Perk_value++;

		UpdateFlatmateOnServer ();
	}

	void OnMouseDown ()
	{
		if (ScreenAndPopupCall.Instance.placementEnabled)
			return;
		if (ScreenManager.Instance.PopupShowed && ScreenManager.Instance.PopupShowed != ScreenManager.Instance.Catalogue)
			return;
//		
//		if (ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.MenuScreen) {
//			if (ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.CellPhone) {
//				if (ScreenManager.Instance.ScreenMoved) {
//					if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.PlayerMenu) {
//						ScreenAndPopupCall.Instance.FlatMateMenu ();
//						if (GameManager.Instance.level <= 1) {
//							tutorial.DecreaseAttendClass ();
//						}
//					}
//					return;
//				}
//			}
//		}
//
//
//		//
//
//		if (this.gameObject.GetComponent<GenerateMoney> ().MoneyToBeGiven > 0) {
//			this.gameObject.GetComponent<GenerateMoney> ().Moneycollection ();
//		} else {		
//
//			if (GameManager.Instance.IsTutorialActive)
//				return;	
//			
//			if (!tutorial._ClassAttended) {
//				if (tutorial.attendClass == 1) {
//					//					tutorial.UpdateTutorial ();
//					tutorial.ClassAttend ();
//					tutorial.lasteventgameobject = null;
//				} else
//					tutorial.DecreaseAttendClass ();
//
//			}	
//	
//			ScreenAndPopupCall.Instance.CloseScreen ();
//			if (ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.MenuScreen) {
//				if (ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.CellPhone) {
//					ScreenAndPopupCall.Instance.FlatMateMenu ();
//				}
//			}
//			DressManager.Instance.SelectedCharacter = RoommateManager.Instance.SelectedRoommate = this.gameObject;
//		}
//	
		if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.MenuScreen || ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.CellPhone) {

			if (this.gameObject.GetComponent<GenerateMoney> ().MoneyToBeGiven > 0) {
				this.gameObject.GetComponent<GenerateMoney> ().Moneycollection ();
			}
		} else if (ScreenManager.Instance.ScreenMoved == null) {

			if (this.gameObject.GetComponent<GenerateMoney> ().MoneyToBeGiven > 0) {
				this.gameObject.GetComponent<GenerateMoney> ().Moneycollection ();
			} else {
				if (GameManager.Instance.IsTutorialActive)
					return;
				if (!tutorial._ClassAttended) {
					if (tutorial.attendClass == 1) {
						tutorial.ClassAttend ();
						tutorial.lasteventgameobject = null;
					} else
						tutorial.DecreaseAttendClass ();
			
				}	
				
				ScreenAndPopupCall.Instance.CloseScreen ();
				ScreenAndPopupCall.Instance.FlatMateMenu ();
				

				DressManager.Instance.SelectedCharacter = RoommateManager.Instance.SelectedRoommate = this.gameObject;
			}

		} else if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.PlayerMenu) {

			if (this.gameObject.GetComponent<GenerateMoney> ().MoneyToBeGiven > 0) {
				this.gameObject.GetComponent<GenerateMoney> ().Moneycollection ();
			} else {
				ScreenAndPopupCall.Instance.FlatMateMenu ();
				tutorial.DecreaseAttendClass ();
			}
		}
		

	}

	public void IncreaseEducationLevel ()
	{
//		Education_Level = PlayerPrefs.GetInt ("Education_Level" + data.Name);
//		Education_Level += 1;
//		PlayerPrefs.SetInt ("Education_Level" + data.Name, Education_Level);
		data.education_point = 0;
		data.education_level++; //Education_Level;
//		GameManager.Instance.AddExperiencePoints (10f);
//		UpdateFlatmateOnServer ();
	}

	public void AddEducationPoints (float points)
	{
//		Education_Level = PlayerPrefs.GetInt ("Education_Level" + data.Name);
//
//		float periviosEduPoint = PlayerPrefs.GetFloat ("Education_Level_Points" + Education_Level + data.Name);
//		float MaxEduPoint = GetMaxLevePointperLevel ();
//
//		if ((periviosEduPoint + points) >= MaxEduPoint) {
//			float diff = MaxEduPoint - periviosEduPoint;
//			PlayerPrefs.SetFloat ("Education_Level_Points" + Education_Level + data.Name, periviosEduPoint + diff);
//			IncreaseEducationLevel ();
//			Education_Level = GetCurrentEducationLevel ();
//			points = points - diff;
//		}
//
//		PlayerPrefs.SetFloat ("Education_Level_Points" + Education_Level + data.Name, PlayerPrefs.GetFloat ("Education_Level_Points" + Education_Level + data.Name) + points);
//
//		data.education_point = PlayerPrefs.GetFloat ("Education_Level_Points" + Education_Level + data.Name);


		float periviosEduPoint = data.education_point;
		float MaxEduPoint = GetMaxLevelPointperLevel ();

		if ((periviosEduPoint + points) >= MaxEduPoint) {
			float diff = MaxEduPoint - periviosEduPoint;
			IncreaseEducationLevel ();
			points = points - diff;
		}
		data.education_point += points;
		UpdateFlatmateOnServer ();

	}

	public int GetCurrentEducationLevel ()
	{
		
		return data.education_level;//= Education_Level = PlayerPrefs.GetInt ("Education_Level" + data.Name);			
	}

	public string GetEducationPointString ()
	{
		float Edu_Point = data.education_point;		//= PlayerPrefs.GetFloat ("Education_Level_Points" + Education_Level + data.Name);
		return string.Format ("{0}/ {1}", Edu_Point, GetMaxLevelPointperLevel ());		
	}

	float GetMaxLevelPointperLevel ()
	{	
		if (data.education_level == 0)
			return 80.0f;
		else {
			return 100.0f * data.education_level;
		}
	}

	public int GetPerkValueCorrespondingToString (string Perk)
	{
        if (data.Perk != null) {
			if (data.Perk.Contains (Perk)) {
				return data.Perk_value;
			}
		}
		return 0;
	}



}

