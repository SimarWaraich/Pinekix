using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SocietyUi : MonoBehaviour
{

	public SocietyManager.Society Society;
	public Text TitleText;
	public Text TagsText;
	public Image EmblemImage;

	void Start ()
	{
		TitleText.text = Society.Name;
		string tagsString = "";
		for (int i = 0; i < Society.Tags.Count; i++) {
			if (i == Society.Tags.Count - 1)
				tagsString += Society.Tags [i];
			else
				tagsString += Society.Tags [i] + ","; 
		}
		TagsText.text = tagsString;
//		TagsText.text = tagsString.Remove (tagsString.Length - 1);
		if (SocietyManager.Instance.EmblemList.Count <= Society.EmblemType || Society.EmblemType < 0) {
			Society.EmblemType = Random.Range (0, SocietyManager.Instance.EmblemList.Count);
		}
        var Tut = GameManager.Instance.GetComponent<Tutorial>();

        if (!Tut._SocietyCreated)
            transform.FindChild("Open").GetComponent<Button>().interactable = false;
        else          
            transform.FindChild("Open").GetComponent<Button>().interactable = true;
        
		EmblemImage.sprite = SocietyManager.Instance.EmblemList [Society.EmblemType];
	}

	public void OpenSociety ()
	{
        //		transform.FindChild ("Open").GetComponent <Button>().interactable = false; 
        var Tut = GameManager.Instance.GetComponent<Tutorial>();
        if (!Tut._SocietyCreated)
            return;
		SocietyManager.Instance.SelectedSociety = Society;
		GetMyRole ();
		///Get Society Party Status 
		SocietyPartyManager.Instance.GetAllSocietyParty ();
        IndicationManager.Instance.IncrementIndicationFor("Society", 5);
	}


	public void GetMyRole ()
	{
		SocietyManager.Instance.GetMyRole (Society);
	}
}