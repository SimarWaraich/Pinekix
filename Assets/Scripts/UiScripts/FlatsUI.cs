using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;

public class FlatsUI : MonoBehaviour
{
	public FlatData data;

	public Sprite Icon;

	void Start ()
	{
		InitDecor ();
	}

	void InitDecor ()
	{
		transform.GetChild (1).gameObject.SetActive (false);
		transform.GetChild (2).gameObject.SetActive (false);

		transform.GetChild (0).GetComponent<Image> ().sprite = Icon;
		ApplyListners ();
	}

	void ApplyListners ()
	{ 
		var Button = GetComponent<Button> ();

		Button.onClick.RemoveAllListeners ();
		Button.onClick.AddListener (() => ShowPopUpConfirmation ());
	}

	void ShowPopUpConfirmation ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Yes";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "No";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = " Do you want to build this room ?";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (true);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseConfirm ());

		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());		
	}

	void ShowPopUpBuyLand ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = " Please buy a land before you build this room";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());		
	}

	void ShowPopUpError ()
	{
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().interactable = true;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = "Some Error Occured... Please Try Again!!!";
		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => PurchaseCancle ());		
	}

	void PurchaseConfirm ()
	{
		RoomPurchaseManager.Instance._selectedFlat = data;
		RoomPurchaseManager.Instance.ShowMessage ();
//		var Tut = GameManager.Instance.GetComponent <Tutorial>();
//		if(!Tut._LandPurchased){
//			Tut.LandPurchasing ();
//		}
	}




	void PurchaseCancle ()
	{
		ScreenManager.Instance.ClosePopup ();
	}


	public static string SerializeVector3Array (Vector3 aVectors)
	{
		StringBuilder sb = new StringBuilder ();

		sb.Append (aVectors.x).Append (" ").Append (aVectors.y).Append (" ").Append (aVectors.z).Append ("|");

		if (sb.Length > 0) // remove last "|"
			sb.Remove (sb.Length - 1, 1);
		return sb.ToString ();
	}



}
