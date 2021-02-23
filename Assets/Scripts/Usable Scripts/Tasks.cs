/// <summary>
/// Created by Mandeep Yadav... Dated 18th August 2k16
/// This script initialise the 
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tasks : MonoBehaviour
{
	public TaskInformation task;
	public  Sprite DoneImage;

	public GameObject DoneImageSprite, GoButton;

	public void SelectTask ()
	{
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!tut._QuestAttended) {
//			tut.AttendQuest ();
		}
		if (task.TaskName.Contains ("Dress") || task.TaskName.Contains ("Cloth")) {
			ScreenAndPopupCall.Instance.CloseScreen ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();

			PurchaseDressManager.Instance.IntializeDressesforShopping (0);		
			ScreenAndPopupCall.Instance.ClothsScreenCalled ();

			/// TODO: Open dress purchasing screen;
		} else if (task.TaskName.Contains ("Flatmate")) {
			/// TODO: Open flatmate hiring screen;
		} else if (task.TaskName.Contains ("Hair") || task.TaskName.Contains ("Buy a Hair")) {
			ScreenAndPopupCall.Instance.CloseScreen ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
			PurchaseSaloonManager.Instance.IntializeItemsforShopping (0);
			ScreenAndPopupCall.Instance.BoutiqueShop ();
			/// TODO: Open item purchasing screen;
		} else if (task.TaskName.Contains ("Party")) {
			/// TODO: Open party starting screen;
		} else if (task.TaskName.Contains ("Exchange")) {
			/// TODO: Open Exchange screen;
		} else if (task.TaskName.Contains ("Decor")) {
			ScreenAndPopupCall.Instance.CloseScreen ();
			ScreenAndPopupCall.Instance.CloseCharacterCamera ();		
			DecorController.Instance.IntializedecorItemesforDecor (3);
			ScreenAndPopupCall.Instance.DecorScreenCalled ();
			/// TODO: Open Exchange screen;
		}
//		 DoneImageSprite.SetActive (task.TaskCompleted);
//		//						GoButton.SetActive (!task.TaskCompleted);
	}

	void Start ()
	{
		transform.GetChild (0).GetComponent<Text> ().text = task.TaskName;
		transform.GetChild (1).GetChild (0).GetComponent<Image> ().sprite = task.TaskIcon;
		transform.GetChild (4).GetComponent<Image> ().sprite = DoneImage;

		DoneImageSprite.SetActive (task.TaskCompleted);
		GoButton.SetActive (!task.TaskCompleted);
		GoButton.GetComponent<Button> ().onClick.AddListener (() => SelectTask ());
	}

	public void CompleteTask ()
	{
		if (task.TaskCompleted) {
			DoneImageSprite.SetActive (task.TaskCompleted);
			GoButton.SetActive (!task.TaskCompleted);
			QuestManager.Instance.ChangeStatus ();

		}				
	}

	void OpenScreen (GameObject Screen)
	{
		ScreenAndPopupCall.Instance.CloseScreen ();


	}
}

