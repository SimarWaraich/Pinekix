using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CoinItems : MonoBehaviour {

	public Coins CoinAttribute;
	public Text Title;
	public Text ButtonPrice;
	public int Price;
	public Button BuyButton;
	// Use this for initialization
	void Start () {

		Title.text = CoinAttribute.Title;
		ButtonPrice.text = CoinAttribute.Price;
		Price = CoinAttribute.PriceTobuy;
		BuyButton.onClick.RemoveAllListeners ();
		BuyButton.onClick.AddListener (BuyThisProduct);
	}

	// Update is called once per frame

	void BuyThisProduct()
	{
		if(Title.text.Contains ("100 Coins"))
		{
			InAppPurchaseManager.Instance.BuyCoins (InAppPurchaseManager.PRODUCT_100_Coins);
		}else if(Title.text.Contains ("200 Coins"))
		{
			InAppPurchaseManager.Instance.BuyCoins (InAppPurchaseManager.PRODUCT_100_Coins);
		}
	}

}