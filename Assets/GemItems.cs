using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GemItems : MonoBehaviour {

	public Gems GemAttribute;
	public Text Title;
	public Text ButtonPrice;
	public int Price;
	public Button BuyButton;
	// Use this for initialization
	void Start () {
	
		Title.text = GemAttribute.Title;
		ButtonPrice.text = GemAttribute.Price;
		Price = GemAttribute.PriceTobuy;
		BuyButton.onClick.RemoveAllListeners ();
		BuyButton.onClick.AddListener (BuyThisProduct);
	}
	
	// Update is called once per frame

	void BuyThisProduct()
	{
		if(Title.text.Contains ("10 Diamonds"))
		{
			InAppPurchaseManager.Instance.BuyGems  (InAppPurchaseManager.PRODUCT_10_Gmes);
		}else if(Title.text.Contains ("20 Diamonds"))
		{
			InAppPurchaseManager.Instance.BuyGems (InAppPurchaseManager.PRODUCT_10_Gmes);
		}
	}
}
