using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SocietyTag : MonoBehaviour {

	public string Tag;
	public bool isSuggestion;
	public Sprite SuggestionImage;
	public Sprite AddedImage;


	void Start () {
		if (isSuggestion) 
		{
			GetComponent <Image> ().sprite = SuggestionImage;
			transform.FindChild ("Close").GetComponent <Button>().gameObject.SetActive (false);

			GetComponent <Button> ().onClick.AddListener (() => {
				if(SocietyManager.Instance.societyCreationController.TagsArray.Count < 5){
					SocietyManager.Instance.societyCreationController.AddTagsToList (Tag,true,gameObject);

                    var Tut = GameManager.Instance.GetComponent<Tutorial>();
                    if (!Tut._SocietyCreated && SocietyManager.Instance.societyCreationController.TagsArray.Count <= 4 )
                    {
                        Tut.societyTutorial = 8;
                        Tut.SocietyTutorial();
                    } 
                    else if (!Tut._SocietyCreated && Tut.societyTutorial == 10)
                    {   
                        Tut.SocietyTutorial();
                    }
					Destroy (gameObject);
				}
				else
					SocietyManager.Instance.ShowPopUp ("You can not add more than 5 tags", null);

			});
		}
		else
		{
			GetComponent <Image> ().sprite = AddedImage;
			GetComponent <Button> ().interactable = false;
			transform.FindChild ("Close").GetComponent <Button>().gameObject.SetActive (true);

			transform.FindChild ("Close").GetComponent <Button> ().onClick.AddListener (() => {
				SocietyManager.Instance.societyCreationController.TagsArray.Remove (Tag);
				Destroy (gameObject);

                var Tut = GameManager.Instance.GetComponent<Tutorial>();
                if (!Tut._SocietyCreated && SocietyManager.Instance.societyCreationController.TagsArray.Count <= 4 )
                {
                    Tut.societyTutorial = 8;
                    Tut.SocietyTutorial();
                } 
				if(SocietyManager.Instance.societyCreationController.TagsArray.Count < 5)
				{
					SocietyManager.Instance.societyCreationController.NextButton.interactable = false;
					SocietyManager.Instance.societyCreationController.TagsSubmitScreen.transform.FindChild ("Open Tag List").gameObject.SetActive (true);
				}
				else
				{
					SocietyManager.Instance.societyCreationController.NextButton.interactable = true;
					SocietyManager.Instance.societyCreationController.TagsSubmitScreen.transform.FindChild ("Open Tag List").gameObject.SetActive (false);
				}
			});
		}
		GetComponentInChildren <Text> ().text = Tag;
		gameObject.name = Tag;
	}
}
