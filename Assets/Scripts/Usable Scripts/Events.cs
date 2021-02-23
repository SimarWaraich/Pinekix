using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Events : MonoBehaviour
{
	public string QuestTitle;
	public string QuestDescription;

	public List<string> ConditionName;
	public List<string> ConditionValue;


	public Dictionary<string, string> conditions;
	public bool IsTutorial;
	public bool IsCompleted;
	public float Completion;
	public int TotalTasks;
	public GameObject ProgressBar;
	public List<string> TaskName;
	public List<int> TaskValue;

	public List<string> RewardName;
	public List<int> RewardValue;

	public List<TaskInformation> Tasks;
	public bool isAvailable;
	public bool IsGoingOn;

	public Dictionary<string, int> TaskList;
	public Dictionary<string, int> Rewards;

	void Start ()
	{	
		transform.FindChild ("Start Button").GetComponent<Button> ().onClick.RemoveAllListeners ();

		transform.FindChild ("Start Button").GetComponent<Button> ().interactable = isAvailable && !IsCompleted;
		transform.FindChild ("Start Button").GetComponent<Button> ().onClick.AddListener (() => this.OpenTasks ());
		transform.FindChild ("Event Information").GetComponent<Text> ().text = QuestDescription;
//		float taskComplitonValue = ProgressBar.GetComponent<Image> ().fillAmount;
//		float newTaskValue = taskComplitonValue / TotalTasks;
//		ProgressBar.GetComponent<Image> ().fillAmount = Completion;

	}


	void OpenTasks ()
	{
		ScreenAndPopupCall.Instance.CloseScreen ();
		ScreenAndPopupCall.Instance.TaskScreenSelection ();
		SelectEvent ();
		InitializeTasks ();
		IsGoingOn = true;
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!tut._QuestAttended) {
//			tut.AttendQuest ();
		}
		transform.FindChild ("Start Button").GetComponent<Button> ().onClick.RemoveAllListeners ();

	}


	public void OnBeforeSerialize ()
	{
		conditions.Clear ();
		TaskList.Clear ();
		Rewards.Clear ();
		for (int i = 0; i < Mathf.Min (ConditionName.Count, ConditionValue.Count); i++) {
			conditions.Add (ConditionName [i], ConditionValue [i]);
		}

		for (int i = 0; i < Mathf.Min (TaskName.Count, TaskValue.Count); i++) {
			TaskList.Add (TaskName [i], TaskValue [i]);
		}

		for (int i = 0; i < Mathf.Min (RewardName.Count, RewardValue.Count); i++) {
			Rewards.Add (RewardName [i], RewardValue [i]);
		}
	}

	public void SelectEvent ()
	{

		QuestManager.Instance.SelectEvent (QuestTitle);

	}

	public void DeleteOldItems ()
	{
		for (int i = 0; i < QuestManager.Instance.TaskContainer.transform.childCount; i++) {
			Destroy (QuestManager.Instance.TaskContainer.transform.GetChild (i).gameObject);
		}
	}

	public void InitializeTasks ()
	{
		DeleteOldItems ();
//		List<string> keylist = new List<string> (TaskList.Keys);

		for (int i = 0; i < Tasks.Count; i++) {

			var Taskbutton = (GameObject)Instantiate (QuestManager.Instance.TaskPrefab, Vector2.zero, Quaternion.identity);
			Taskbutton.transform.SetParent (QuestManager.Instance.TaskContainer.transform);
			Taskbutton.transform.localScale = Vector3.one;

//			Taskbutton.transform.parent.GetComponent <RectTransform> ().sizeDelta =
//				new Vector2 (Taskbutton.transform.parent.GetComponent <RectTransform> ().rect.width,
//				Taskbutton.transform.parent.GetComponent <RectTransform> ().rect.height + 95);

			Taskbutton.GetComponent<Tasks> ().task = Tasks [i];
		}
	}

	public void UpdateProgressBar (float Bar_value)
	{
		float bar = ProgressBar.GetComponent<Image> ().fillAmount;	
		bar = Bar_value / QuestManager.Instance.CurrentQuest.TotalTasks;
		ProgressBar.GetComponent<Image> ().fillAmount = Bar_value / QuestManager.Instance.CurrentQuest.TotalTasks;
		if (ProgressBar.GetComponent<Image> ().fillAmount >= 1)
			DeleteOldItems ();

	}

	public void UpdateTask (TaskInformation _task)
	{
		var AllTasks = QuestManager.Instance.TaskContainer.GetComponentsInChildren <Tasks> ();
		foreach (var taskGameObject in AllTasks) {
			if (_task == taskGameObject.task) {
				taskGameObject.DoneImageSprite.SetActive (_task.TaskCompleted);
				taskGameObject.GoButton.SetActive (!_task.TaskCompleted);
			}
		}
	}
}



