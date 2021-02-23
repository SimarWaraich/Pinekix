using UnityEngine;
using System.Collections;

public class UiDecoreEventCalls : MonoBehaviour
{

	public void OnClickWalls ()
	{
		ScreenAndPopupCall.Instance.CloseRoomCamera ();
		ScreenAndPopupCall.Instance.RoomPurchaseScreenCalled ();
		ReModelShopController.Instance.isForEvent = true;
		ReModelShopController.Instance.IntializeInventoryForWalls ();

	}

	public void OnClickGrounds ()
	{
		ScreenAndPopupCall.Instance.CloseRoomCamera ();
		ScreenAndPopupCall.Instance.RoomPurchaseScreenCalled ();
		ReModelShopController.Instance.isForEvent = true;
		ReModelShopController.Instance.IntializeInventoryForGrounds ();
	}
}
