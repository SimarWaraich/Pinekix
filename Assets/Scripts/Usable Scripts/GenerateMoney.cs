using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GenerateMoney : MonoBehaviour
{

	public int MoneyToBeGiven;
	public int MaxMoney = 50;
	public GameObject MoneyIcon;
	public GameObject MovableMoneyIcon;

	public DateTime current;
	public DateTime Previous;
	Flatmate ThisFlatmate;

	void Start ()
	{
		current = DateTime.Now;
		Previous = DateTime.Now;
		MoneyIcon.SetActive (false);
	}

	void Update ()
	{
		if (ThisFlatmate)
			Check ();
		else
			ThisFlatmate = GetComponent<Flatmate> ();
	}

	public void Check ()
	{
		TimeSpan ts = CheckForDifferenceofTime ();
//		print (ts.TotalSeconds);
	    
		if (ts.TotalSeconds >= 30) {
			Generate ();
			SaveLastPlayingTime ();
		}
	}

	void Generate ()
	{
		MoneyIcon.SetActive (true);
		MoneyToBeGiven += (MaxMoney / 3);
		if (MoneyToBeGiven >= MaxMoney)
			MoneyToBeGiven = MaxMoney;
		float bonus = GetComponent <Flatmate> ().GetPerkValueCorrespondingToString ("Coin");
		bonus = bonus * 1.5f;
		float moneyGiven = MoneyToBeGiven * this.GetComponent<Flatmate> ().data.education_level / 3.5f + bonus;
		MoneyToBeGiven = (int)moneyGiven * VipSubscriptionManager.Instance.VipSubcritionActived ();
		if (MoneyToBeGiven > 650)
			MoneyToBeGiven = 650;		
//		print (MoneyToBeGiven);
	}

	public void Moneycollection ()
	{
		if (MoneyToBeGiven <= 0) {
			MoneyToBeGiven = 0;
			return;
		}
		MoneyIcon.SetActive (false);
		StartCoroutine (GameManager.Instance.CoinsCollectionAnimation (MoneyToBeGiven, MoneyIcon.transform.position));
		GameManager.Instance.AddCoins (MoneyToBeGiven);
		MoneyToBeGiven = 0;
	}



	TimeSpan CheckForDifferenceofTime ()
	{
		current = DateTime.Now;
		TimeSpan ts = current - Previous;
		return ts;
	}

	void SaveLastPlayingTime ()
	{
		Previous = DateTime.Now;
	}
}
