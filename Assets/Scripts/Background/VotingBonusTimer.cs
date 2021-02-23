using UnityEngine;
using System.Collections;
using System;

public class VotingBonusTimer : MonoBehaviour {

	public VotingBonus Bonus;
	public float TimerInFloat;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		var Diff = Bonus.DestroyTime - DateTime.UtcNow;
		TimerInFloat = (float)Diff.TotalSeconds;
		if(Bonus.DestroyTime < DateTime.UtcNow)
		{
			PersistanceManager.Instance.DeleteThisVotingBonus (Bonus.Id);
			PlayerPrefs.DeleteKey ("VotingRefreshTime" + Bonus.Id + PlayerPrefs.GetInt ("PlayerId"));
			PlayerPrefs.DeleteKey ("VotedCount"+Bonus.Id + PlayerPrefs.GetInt ("PlayerId"));
			Destroy (gameObject);			
		}
	}
}
