using UnityEngine;
using System.Collections;

public class PurchasedLand : MonoBehaviour
{

//	public SaleBanner _thisBanner;

	public int x;
	public int y;

	void Awake ()
	{
//		foreach (var banner in RoomPurchaseManager.Instance.SaleBanners) {
//			if (transform.position.x == banner.transform.position.x && transform.position.y == banner.transform.position.y)
//				_thisBanner = banner;
//		}
	}

	void OnMouseDown ()
	{
		if (ScreenManager.Instance.ScreenMoved == null) {			
			if (!RoomPurchaseManager.Instance._selectionEnabled)
				return;
		
			if (RoomPurchaseManager.Instance._selectionEnabled && ScreenManager.Instance.ScreenMoved == null && ScreenManager.Instance.PopupShowed == null) {
//			RoomPurchaseManager.Instance.selectedBanner = _thisBanner;
				RoomPurchaseManager.Instance.PurchaseWhenClick (this);
			}
		}
	}

	void Update (){

		if (RoomPurchaseManager.Instance._selectionEnabled) 
		{
			GetComponent <SpriteRenderer> ().color = Color.white;
			transform.GetChild (0).gameObject.SetActive (true);
		}else{
			GetComponent <SpriteRenderer> ().color = Color.black;
			transform.GetChild (0).gameObject.SetActive (false);
		}
	}
}
