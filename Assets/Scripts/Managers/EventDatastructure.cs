using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class EventDataStructure
{
	public string EventName;
	public eventType category;
	public string EventDetails;
	public string EventTheme;
	public int Event_id;
	public Dictionary<string, string> conditions = new Dictionary<string, string> ();
	//	public bool IsGoingOn;
    public bool VipSubscription;
	public bool IsRegisterd;
	public bool isForRewards;
	public DateTime RegistrationTime;
	public DateTime StartTime;
	public DateTime EndTime;

	//	public float Completion;
	//	public eType Etype;
	public Dictionary<string, string> Rewards = new Dictionary<string, string> ();

	//	public string EventName;
	//	public string EventDetails;
	//	public int Event_id;
	//	public Dictionary<string, string> conditions = new Dictionary<string, string> ();
	//	public bool IsGoingOn;
	//	public bool IsCompleted;
	//
	//	public DateTime RegistrationTime;
	//	public DateTime StartTime;
	//	public DateTime EndTime;
	//
	//	//	public eType Etype;
	//	public float Completion;
	//	public Dictionary<string, int> Rewards = new Dictionary<string, int> ();
}
