using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


[Serializable]
public struct QuestStruct
{

	public string QuestTitle;
	public string QuestDescription;

	public List<string> ConditionName;
	public List<string> ConditionValue;

	public bool IsTutorial;
	public bool IsCompleted;
	public float Completion;
	public int TotalTasks;
	public bool IsGoingOn;
	public List<TaskInformation> AllTasks;

	public List<string> RewardName;
	public List<int> RewardValue;

}


public class QuestManager : Singleton<QuestManager>
{

	public QuestStruct[] allQuestData;
	public QuestData[] AvailableQuest;
	public QuestData[] CompletedQuest;
	public QuestData CurrentQuest;
	public GameObject QuestPrefab;
	public GameObject QuestContainer;
	public GameObject TaskPrefab;
	public GameObject TaskContainer;

	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
		CheckEvents ();
	}

	// This function will work on start and for download serialization
	public void CheckEvents ()
	{

		List<QuestData> avlevent = new List<QuestData> ();
		List<QuestData> cmlevent = new List<QuestData> ();

		for (int i = 0; i < allQuestData.Length; i++) {
			var _event = new QuestData ();

			_event.QuestTitle = allQuestData [i].QuestTitle;
			_event.Completion = allQuestData [i].Completion;
			_event.QuestDescription = allQuestData [i].QuestDescription;
			_event.IsCompleted = allQuestData [i].IsCompleted;
			_event.IsTutorial = allQuestData [i].IsTutorial;
			_event.TotalTasks = allQuestData [i].TotalTasks;
			_event.IsGoingOn = allQuestData [i].IsGoingOn;

			for (int x = 0; x < Mathf.Min (allQuestData [i].ConditionName.Count, allQuestData [i].ConditionValue.Count); x++) {
				_event.conditions.Add (allQuestData [i].ConditionName [x], allQuestData [i].ConditionValue [x]);
			}
			for (int x = 0; x < Mathf.Min (allQuestData [i].RewardName.Count, allQuestData [i].RewardValue.Count); x++) {
				_event.Rewards.Add (allQuestData [i].RewardName [x], allQuestData [i].RewardValue [x]);
			}

			_event.Tasks = allQuestData [i].AllTasks;


			if (_event.IsCompleted) {
				cmlevent.Add (_event);
			} else {
				avlevent.Add (_event);
			}
		}

		AvailableQuest = avlevent.ToArray ();
		CompletedQuest = cmlevent.ToArray ();

		/// TODO: get the data of events from the server and then enable and disable them according to the time given and if completed then submit a completed icon.
	}

	// Check Event weather it is null
	public void CheckForEvent ()
	{
		if (CurrentQuest != null)
			return;
	}


	public void IncreaseCompletion (string NameofTask)
	{
		//		if (!CurrentEvent.Tasks.ContainsKey (NameofTask))
		//			return;
		//
		//
		//		CurrentEvent.Completion += (CurrentEvent.Completion / CurrentEvent.TotalTasks);
		//		if (CurrentEvent.Completion >= 100)
		//		ChangeStatus ();
	}

	// Change currunt Event and task status which are they holding
	public void ChangeStatus ()
	{		
		List<QuestData> avlEvents = new List<QuestData> ();
		List<QuestData> cplEvents = new List<QuestData> ();

		for (int i = 0; i < AvailableQuest.Length; i++) {
			avlEvents.Add (AvailableQuest [i]);
		}

		for (int i = 0; i < CompletedQuest.Length; i++) {
			cplEvents.Add (CompletedQuest [i]);
		}

		int cmptedTasksCount = 0;
		// Here this will check weather event is completed or not 
		int TaskCount = CurrentQuest.Tasks.Count;

		for (int i = 0; i < CurrentQuest.Tasks.Count; i++) {			

			if (CurrentQuest.Tasks [i].TaskCompleted) {
				cmptedTasksCount++;
			}
			if (cmptedTasksCount == TaskCount)
				CurrentQuest.IsCompleted = true;			
			CurrentQuest.IsGoingOn = false;

		}

		if (CurrentQuest.IsCompleted) {			
			var _event = QuestManager.Instance.QuestContainer.GetComponentInChildren<Events> ();
			_event.isAvailable = false;
			_event.IsGoingOn = false;

		}

		for (int i = 0; i < avlEvents.Count; i++) {
			if (avlEvents [i] == CurrentQuest) {
				cplEvents.Add (CurrentQuest);
				avlEvents.Remove (avlEvents [i]);
				break;
			}
		}

		AvailableQuest = avlEvents.ToArray ();
		CompletedQuest = cplEvents.ToArray ();

		// Give rewards for performed event 
		GiveRewards ();
	}

	public void GiveRewards ()
	{

		foreach (var Key in CurrentQuest.Rewards.Keys) {
			switch (Key) {
			case "ExperiencePoints":
//				PlayerPrefs.SetFloat ("ExperiencePoints" + GameManager.Instance.level, PlayerPrefs.GetFloat ("ExperiencePoints" + GameManager.Instance.level) + CurrentQuest.Rewards ["ExperiencePoints"]);
				GameManager.Instance.AddExperiencePoints (CurrentQuest.Rewards ["ExperiencePoints"]);
				break;
			case "Gems":
				PlayerPrefs.SetInt ("Gems", PlayerPrefs.GetInt ("Gems") + CurrentQuest.Rewards ["Gems"]);
				GameManager.Instance.AddGemsWithGemBonus (CurrentQuest.Rewards ["Gems"]);
				break;
			case  "Coins":
				GameManager.Instance.AddCoins (CurrentQuest.Rewards ["Coins"]);
				break;
			}
		}

		CurrentQuest = null;
	}

	public void SelectEvent (string EventName)
	{
		//		if (CurrentEvent != null)
		//			return;
		for (int i = 0; i < AvailableQuest.Length; i++) {
			if (AvailableQuest [i].QuestTitle == EventName) {
				AvailableQuest [i].IsGoingOn = true;
				for (int x = 0; x < AvailableQuest.Length; x++) {
					{
						if (allQuestData [x].QuestTitle == AvailableQuest [i].QuestTitle)
							allQuestData [x].IsGoingOn = true;
					}
					CurrentQuest = AvailableQuest [i];
					break;
				}
			}
		}
	}


	//	public void CreatTaskForEvent (EventsData _event)
	//	{
	//
	//	}

	void DeleteOldItems ()
	{
		for (int i = 0; i < QuestContainer.transform.childCount; i++) {
			Destroy (QuestContainer.transform.GetChild (i).gameObject);
		}
	}

	public void CreateEvent ()
	{
		DeleteOldItems ();
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (tut._PublicAreaAccessed && !tut._QuestAttended) {			

			for (int i = 0; i < AvailableQuest.Length; i++) {
				var EventButton = (GameObject)Instantiate (QuestPrefab, Vector2.zero, Quaternion.identity);


				EventButton.transform.SetParent (QuestContainer.transform);
				EventButton.transform.localScale = Vector3.one;
				var _Event = EventButton.GetComponent<Events> ();
				_Event.QuestTitle = AvailableQuest [i].QuestTitle;
				_Event.QuestDescription = AvailableQuest [i].QuestDescription;
				_Event.Completion = AvailableQuest [i].Completion;
				_Event.conditions = AvailableQuest [i].conditions;
				_Event.IsCompleted = AvailableQuest [i].IsCompleted;
				_Event.IsTutorial = AvailableQuest [i].IsTutorial;
				_Event.Tasks = AvailableQuest [i].Tasks; // t
				_Event.TotalTasks = AvailableQuest [i].TotalTasks;
				_Event.IsGoingOn = AvailableQuest [i].IsGoingOn;

				foreach (var keys in AvailableQuest[i].conditions.Keys) {
					if (keys.Contains ("Level")) {
						var Value = int.Parse (AvailableQuest [i].conditions ["Level"]);
						if (GameManager.Instance.level < Value)
							_Event.isAvailable = true;
						else
							_Event.isAvailable = true;
					}
				}

				_Event.Tasks = AvailableQuest [i].Tasks;

				foreach (var keys in AvailableQuest[i].conditions.Keys) {
					_Event.ConditionName.Add (keys);

				}
				foreach (var values in AvailableQuest[i].conditions.Values) {
					_Event.ConditionValue.Add (values);
				}

				foreach (var keys in AvailableQuest[i].Rewards.Keys) {
					_Event.RewardName.Add (keys);

				}
				foreach (var values in AvailableQuest[i].Rewards.Values) {
					_Event.RewardValue.Add (values);
				}
			}
		}

	}

	//	public void UpdateProgressBar (float Bar_value)
	//	{
	////		QuestContainer.transform.GetChild (0).GetComponent<Events> ().ProgressBar;
	////		float bar = GameObject.Find ("Progressing").GetComponent<Image> ().fillAmount;
	//		float bar = QuestContainer.transform.GetChild (0).GetComponent<Events> ().ProgressBar.GetComponent<Image> ().fillAmount;
	//
	//
	////		float i = CurrentEvent.TotalTasks + 1 + 
	//		bar = Bar_value / CurrentQuest.TotalTasks;
	////		newBar = Bar_value;
	//		QuestContainer.transform.GetChild (0).GetComponent<Events> ().ProgressBar.GetComponent<Image> ().fillAmount = Bar_value / CurrentQuest.TotalTasks;
	//
	//			
	//	}

	public void CheckifTaskisDone (string TaskType)
	{		// On 6th Steps
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!tut._QuestAttended) {
//			if(tut.questAttended ==5 ||tut.questAttended ==8 || tut.questAttended ==12)
//			tut.AttendQuest ();
		}

		foreach (var Event in AvailableQuest) {
			if (Event.IsGoingOn) {
				foreach (var task in Event.Tasks) {
					if (task.TaskName.Contains (TaskType)) {
						task.TaskCompleted = true;
						Event.Completion++;				
						//						tasks.DoneImageSprite.SetActive (task.TaskCompleted);
						//						tasks.GoButton.SetActive (!task.TaskCompleted);
						var AllEvents = QuestManager.Instance.QuestContainer.GetComponentsInChildren<Events> ();

						Events SelectedEvent = null;

						foreach (var eventGameObject in AllEvents) {
							if (eventGameObject.QuestTitle == Event.QuestTitle) {
								eventGameObject.UpdateTask (task);	
								SelectedEvent = eventGameObject;
								eventGameObject.UpdateProgressBar (CurrentQuest.Completion);
							}
						}

						if (Event.TotalTasks <= Event.Completion) {
							ChangeStatus ();

							SelectedEvent.IsCompleted = true;
							//TODO:
							SelectedEvent.transform.FindChild ("Start Button").GetComponentInChildren<Text> ().text = " COMPLETED";
							SelectedEvent.transform.FindChild ("Start Button").GetComponentInChildren<Text> ().fontSize = 15;
							SelectedEvent.transform.FindChild ("Start Button").GetComponent<Button> ().interactable = false;

						}					
					}						
				}
			}
		}		
	}
}