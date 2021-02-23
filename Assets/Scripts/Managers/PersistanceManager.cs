using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simple_JSON;
using System;


/// <summary>
/// Persistance manager is used for saving and getting data stored in PlayerPrefs.
/// 
/// created by- Simarjit Singh
/// </summary>
public class PersistanceManager
{

	private static PersistanceManager _instance = null;

	public static PersistanceManager Instance {
		get {
			if (_instance == null) {
				_instance = new PersistanceManager ();
			}
			return _instance;
		}
	}

	public List<VotingBonus> GetSavedVotingBonuses ()
	{
		string jsonString = PlayerPrefs.GetString ("VotingBonusSavedString" + PlayerPrefs.GetInt ("PlayerId"), "");
		var votingBonuses = new List< VotingBonus> ();
		if (jsonString != "") {
			var json = JSON.Parse (jsonString);
			var count = json.Count;

			for (int i = 0; i < count; i++) {
				int id = int.Parse (json [i] ["Id"].ToString ().Trim ("\"".ToCharArray ()));

				int type = int.Parse (json [i] ["EventType"].ToString ().Trim ("\"".ToCharArray ()));
				eventType typeinEnum = (eventType)type;
				string EndTimeString = json [i] ["Endtime"].ToString ().Trim ("\"".ToCharArray ());
				var temp = Convert.ToInt64 (EndTimeString);

				VotingBonus Vb = new VotingBonus ();
				Vb.Id = id;
				Vb.Type = typeinEnum;
				Vb.DestroyTime = DateTime.FromBinary (temp);

				votingBonuses.Add (Vb);
			}
		}
		return votingBonuses;
	}

	public void SaveAllVotingBonus (List<VotingBonus> Bonuses)
	{
		PlayerPrefs.DeleteKey ("VotingBonusSavedString" + PlayerPrefs.GetInt ("PlayerId"));

		JSONClass arrayJson = new JSONClass ();

		Bonuses.ForEach (bonus => {
			var jsonElement = new JSONClass ();
			jsonElement ["Id"] = bonus.Id.ToString ();
			jsonElement ["EventType"] = ((int)bonus.Type).ToString ();
			jsonElement ["Endtime"] = bonus.DestroyTime.ToBinary ().ToString ();
			arrayJson.Add (jsonElement);		
		});

		PlayerPrefs.SetString ("VotingBonusSavedString" + PlayerPrefs.GetInt ("PlayerId"), arrayJson.ToString (""));
	}

	public void DeleteThisVotingBonus (int bonusId)
	{
		var FinalList = new List<VotingBonus> ();

		var temp = GetSavedVotingBonuses ();

		foreach (var _bonus in temp) {
			if (_bonus.Id !=bonusId)
				FinalList.Add (_bonus);
		}

		SaveAllVotingBonus (FinalList);
	}

	const string RewardsString = "RewardSavedString";

	public void SaveClaimRewardsEvents (List<int> EventIds)
	{
		PlayerPrefs.DeleteKey (RewardsString+ PlayerPrefs.GetInt ("PlayerId"));

		JSONClass arrayJson = new JSONClass ();

		EventIds.ForEach (id => {
			var jsonElement = new JSONClass ();
			jsonElement ["Id"] = id.ToString ();
			arrayJson.Add (jsonElement);
		});			

		PlayerPrefs.SetString (RewardsString+ PlayerPrefs.GetInt ("PlayerId"), arrayJson.ToString (""));
	}

	public List<int> GetEventToClaimRewards()
	{
		var jsonString = PlayerPrefs.GetString (RewardsString + PlayerPrefs.GetInt ("PlayerId"));
		var IdsList = new List<int> ();

		if (jsonString != "") {
			var json = JSON.Parse (jsonString);

			for(int i = 0; i< json.Count;i++)
			{
				int Id = 0;
				int.TryParse (json [i] ["Id"], out Id);
				if (Id != 0)
					IdsList.Add (Id);
			}
		}

		return IdsList;
	}
	public void DeleteEventFromClaimedRewards (int deletedId)
	{
		var FinalList = new List<int> ();

		var temp = GetEventToClaimRewards ();

		foreach (var id in temp){
			if (id !=deletedId)
				FinalList.Add (id);
		}
		SaveClaimRewardsEvents (FinalList);
	}
}
