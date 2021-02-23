using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;


public class WardrobeCharcterSelection : MonoBehaviour
{
	[HideInInspector] 
	public GameObject selection;
	[HideInInspector]
	public CharacterSelectionController thisController;
	[HideInInspector] 
	public string Name;
//	[HideInInspector] 
	public bool IsActive;

	public bool IsOnCoolDown;
	public bool IsBusy;
	public float CoolDownTimer;

	public Text TimerText;

    public Color NormalColor;
    public Color DisabledColor;

	void Start ()
	{
		GetComponentInChildren <Text> ().text = Name;

		GetComponent <Button> ().onClick.AddListener (OnClickCharacter);	
        GetComponent <Image>().color = IsActive ? NormalColor : DisabledColor;

		if (IsOnCoolDown) {
			TimerText.gameObject.SetActive (true);
			TimerText.text = ExtensionMethods.GetShortTimeStringFromFloat (CoolDownTimer);
		}else{
			TimerText.gameObject.SetActive (false);
		}		
	}

	void Update(){

		if (IsOnCoolDown || IsBusy) {
			if(CoolDownTimer >= 0)
			{
				CoolDownTimer -= Time.deltaTime;
				TimerText.gameObject.SetActive (true);
				TimerText.text = ExtensionMethods.GetShortTimeStringFromFloat (CoolDownTimer);
			}else {
				IsOnCoolDown = false;
				IsBusy = false;
				CoolDownTimer = -1;
                IsActive = true;
                GetComponent <Image>().color = IsActive ? NormalColor : DisabledColor;
				TimerText.gameObject.SetActive (false);
			}
		}
	}

	public void OnClickCharacter ()
    {     
        GetFlatmateAndmakeitSelected();
        if(ScreenManager.Instance.OpenedCustomizationScreen.Contains ("CatWalkEventDressUp"))
        {
            if (IsBusy || IsOnCoolDown)
                return;

            if(IsActive && EventManagment.Instance.SelectedRoommates.Count < 3)
            {           
                IsActive = false;                
                EventManagment.Instance.SelectedRoommates.Add(RoommateManager.Instance.SelectedRoommate);

            }else
            {
                IsActive = true;                
                EventManagment.Instance.SelectedRoommates.Remove(RoommateManager.Instance.SelectedRoommate);
            }

            GetComponent <Image>().color = IsActive ? NormalColor : DisabledColor;          
            return;
        }       
        else if(IsActive) 
        { 
            DressManager.Instance.SelectedCharacter = selection;
                             
            if (ScreenManager.Instance.OpenedCustomizationScreen.Contains("WardRobe"))
            {
                
                ScreenAndPopupCall.Instance.ShowWardrobeForCharacter();
                PurchaseDressManager.Instance.IntializeDressesforWardrobe(0);
                var Tut = GameManager.Instance.GetComponent <Tutorial>();
                if (Tut.enabled && !Tut._DressPurchased)
                    Tut.DressPurchasing();

            }
            else if (ScreenManager.Instance.OpenedCustomizationScreen.Contains("Boutique"))
            {
                ScreenAndPopupCall.Instance.Boutique();
                PurchaseSaloonManager.Instance.IntializeItemsforBoutique(0);
                var Tut = GameManager.Instance.GetComponent <Tutorial>();
                if (Tut.enabled && !Tut._SaloonPurchased)
                    Tut.SaloonPurchasing();
            }
            else if (ScreenManager.Instance.OpenedCustomizationScreen.Contains("FashionEventDressUp") || ScreenManager.Instance.OpenedCustomizationScreen.Contains("SocietyEventDressUp"))
            {
                ScreenAndPopupCall.Instance.CloseCharacterCamera();
                ScreenAndPopupCall.Instance.CloseScreen();
                ScreenAndPopupCall.Instance.FashionEventScreenSelection();
                PurchaseDressManager.Instance.IntializeDressesforFashionAndCatwalk(0);
                var Tut = GameManager.Instance.GetComponent <Tutorial>();
                if (!Tut._FashionEventCompleate && Tut._SaloonPurchased)
                    Tut.FashionEventStart();
            }
            else if (ScreenManager.Instance.OpenedCustomizationScreen.Contains("CoOpEvent"))
            {
//			ScreenAndPopupCall.Instance.CloseCharacterCamera ();
//			ScreenAndPopupCall.Instance.ShowCoOpPanel ();
//			ScreenAndPopupCall.Instance.ShowWardRobeForCoOp ();
//			PurchaseDressManager.Instance.IntializeDressesforFashionEvent (0, PurchaseDressManager.Instance.CoOpEventContainer.transform);


                if (MultiplayerManager.Instance._isReciever)
                {            
                    ScreenAndPopupCall.Instance.ShowCoOpPanel();                
                    MultiplayerManager.Instance.JoinorCreateRoomForCoOp(MultiplayerManager.Instance.RoomName);
                }
                else
                {          
                    ScreenAndPopupCall.Instance.ShowCoOpPanel();
                
                    if (CoOpEventController.Instance.playerCount == 2)
                    {
                        ScreenAndPopupCall.Instance.ShowReadyScreen(false);/*Done Here*/
                        CoOpEventController.Instance.StartTimer(10f);
                    }
                    else
                    {
//                ScreenAndPopupCall.Instance.ShowCoOpWaiting();
                        CoOpEventController.Instance.StartTimer(5f);
                    }
                }
            }
            GetComponent <Image>().color = IsActive ? NormalColor : DisabledColor;          

            thisController.ThisCamera.enabled = false;
            thisController.DeleteAllChars ();
        }
	}

	void GetFlatmateAndmakeitSelected ()
	{
		foreach (var flatmate in RoommateManager.Instance.RoommatesHired) {
			if (flatmate.GetComponent <Flatmate> ().data.Name == this.Name) {
				RoommateManager.Instance.SelectedRoommate = flatmate;
			}
		}
	}
}

