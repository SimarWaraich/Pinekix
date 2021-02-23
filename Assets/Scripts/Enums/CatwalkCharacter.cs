using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CatwalkCharacter : MonoBehaviour {


//    [HideInInspector] 
    public GameObject selection;
//    [HideInInspector]
    public CatwalkCharacterDressUp thisController;
    public string Name;
//    public Button DressUpButton;

    [SerializeField]
    Color NormalColor;

    [SerializeField]
    Color DisabledColor;

	void Start () {
        GetComponentInChildren <Text> ().text = Name;
        GetComponent<Button>().onClick.AddListener (()=>OnClickCharacter());
       
        if (thisController.ChangedDressChars.Contains(Name))
            GetComponent <Image>().color = DisabledColor;
        else
            GetComponent <Image>().color = NormalColor;        
	}
	
    void OnClickCharacter()
    {
        GetFlatmateAndmakeitSelected();
//        if( DressForString.Contains("CatwalkEvent"))
//        {
            ScreenAndPopupCall.Instance.CloseCharacterCamera ();
            ScreenAndPopupCall.Instance.CloseScreen ();
            ScreenAndPopupCall.Instance.FashionEventScreenSelection ();
        PurchaseDressManager.Instance.IntializeDressesforFashionAndCatwalk (0);
        thisController.ThisCamera.enabled = false;
        thisController.DeleteAllChars();

//        }else if(DressForString.Contains("CoOpEvent"))
//        {
//            
//        }
    }

    void GetFlatmateAndmakeitSelected ()
    {
        foreach (var flatmate in RoommateManager.Instance.RoommatesHired) {
            if (flatmate.GetComponent <Flatmate> ().data.Name == this.Name) {
                RoommateManager.Instance.SelectedRoommate = flatmate;
                DressManager.Instance.SelectedCharacter = flatmate;
            }
        }
    }
}
