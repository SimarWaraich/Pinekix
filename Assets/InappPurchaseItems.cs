using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class InappPurchaseItems : MonoBehaviour {

	public List<Gems> AllGemsPurchsesList;
	public List<Coins> AllCoinPurchsesList;
	public GameObject Container;
	public GameObject CoinPrefeb;
	public GameObject GemPrefeb;
	// Use this for initialization
	void Start () {
	
	}

	public void ShowAllGemPurchaseScreen()
	{
		for(int i= 0; i<Container.transform.childCount; i++)
		{
			Destroy (Container.transform.GetChild (i).gameObject) ;
		}
		ScreenAndPopupCall.Instance.OpenInAppPurchaseScreen ();
		InstantiateAllGemPurchase ();
	}

	public void ShowAllCoinPurchaseScreen()
	{
		for(int i= 0; i<Container.transform.childCount; i++)
		{
			Destroy (Container.transform.GetChild (i).gameObject) ;
		}
		ScreenAndPopupCall.Instance.OpenInAppPurchaseScreen ();
		InstantiateAllCoinPurchase ();
	}


	public void InstantiateAllGemPurchase()
	{


		for(int i = 0; i<AllGemsPurchsesList.Count; i++)
		{
			GameObject GemObj = GameObject.Instantiate (GemPrefeb, Vector3.zero, Quaternion.identity) as GameObject;
			GemObj.GetComponent<GemItems> ().GemAttribute = AllGemsPurchsesList [i];
			GemObj.transform.parent = Container.transform;
			GemObj.transform.localScale = Vector3.one;
		}		
	}

	public void InstantiateAllCoinPurchase()
	{


		for(int i = 0; i<AllCoinPurchsesList.Count; i++)
		{
			GameObject CoinObj = GameObject.Instantiate (CoinPrefeb, Vector3.zero, Quaternion.identity) as GameObject;
			CoinObj.GetComponent<CoinItems> ().CoinAttribute = AllCoinPurchsesList [i];
			CoinObj.transform.parent = Container.transform;
			CoinObj.transform.localScale = Vector3.one;
		}		
	}
}


[Serializable]
public class Gems
{
	public string Title;
	public string Price;
	public int PriceTobuy;


	public Gems (string title, string priceString, int priceint)
	{	
		Title = title;
		Price = priceString;
		PriceTobuy = priceint;

	}
}

[Serializable]
public class Coins
{
	public string Title;
	public string Price;
	public int PriceTobuy;


	public Coins (string title, string priceString, int priceint)
	{	
		Title = title;
		Price = priceString;
		PriceTobuy = priceint;

	}
}