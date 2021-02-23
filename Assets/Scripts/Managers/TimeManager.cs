/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 12 July 2k16
/// This script will be used to manage the time status of the game
/// This script will also be used to submit the time of last game played so that we can manage the money earned and daily rewards...
/// </summary>

using UnityEngine;
using System.Collections;
using System.Globalization;
using System;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{
	const string dataFmt = "{0,-30}{1}";
	const string timeFmt = "{0,-30}{1:yyyy-MM-dd HH:mm}";


	public TimeZone localZone;
	public DateTime current;
	public DateTime Previous;
	public int currentYear;
	public Text text;

	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
//		PlayerPrefs.DeleteAll ();
		localZone = TimeZone.CurrentTimeZone;
		current = DateTime.Now;
		Previous = DateTime.Now;
	}


	public TimeSpan CheckForDifferenceofTime ()
	{
		current = DateTime.Now;
		TimeSpan ts = current - Previous;
		return ts;
	}


	public void SaveLastPlayingTime ()
	{
		Previous = DateTime.Now;
	}

}

