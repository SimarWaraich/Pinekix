/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 07 July 2k16
/// This script will be used to store the event data which are going on
/// </summary>

using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class QuestData
{
	public string QuestTitle;
	public string QuestDescription;
	public Dictionary<string, string> conditions = new Dictionary<string, string> ();
	public bool IsTutorial;
	public bool IsGoingOn;
	public bool IsCompleted;
	public float Completion;
	public int TotalTasks;
	public List<TaskInformation> Tasks;
	public Dictionary<string, int> Rewards = new Dictionary<string, int> ();
}

