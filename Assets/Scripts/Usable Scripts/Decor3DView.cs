using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using System.Linq;

public class Decor3DView : MonoBehaviour
{
	public DecorData decorInfo;
	public List<Sprite> sprites = new List<Sprite> ();
	public int direction = 0;
	public bool Placed = false;
    public bool isForSociety = false;
	int mouseDown;

	/// <summary>
	/// the direction always describe the direction of the object is facing...
	/// 0 means front,
	/// 1 means right,
	/// 2 means back,
	/// 3 means left...
	/// going anticlockwise and all directions are according to the user or we can say the programmer
	/// </summary>
	/// 


	public Vector3 objPosition;

	Tutorial tutorial;

	public void Start ()
	{         
//		transform.FindChild ("Check").GetComponent<SpriteRenderer> ().sprite = DecorController.Instance.DecorBelowImage;
		transform.FindChild ("Check").GetComponent<SpriteRenderer> ().sortingLayerName = "Walls";

		transform.FindChild ("Check").GetComponent<SpriteRenderer> ().color = Color.green;
//		transform.GetChild (0).position = new Vector3 (transform.GetChild (1).position.x, transform.GetChild (1).position.y, -1);
		tutorial = GameObject.Find ("GameManager").GetComponent <Tutorial> ();

		ScreenAndPopupCall.Instance.placementbutton.interactable = false;

        if (isForSociety)
        {
            
        }
        else
        {
            if (!PlayerPrefs.HasKey("Direction" + decorInfo.Name))
                PlayerPrefs.SetInt("Direction" + decorInfo.Name, 0);

            objPosition = this.transform.position;
            if (!PlayerPrefs.HasKey("ObjPosition" + decorInfo.Name))
            {
                PlayerPrefs.SetString("ObjPosition" + decorInfo.Name, SerializeVector3Array(objPosition));
            }
            
            //		sprites = Resources.LoadAll<Sprite> ("PlayableObjects/" + this.gameObject.name + "/Sprites").ToList ();
            if (EventManagment.Instance.EventType == eType.Voting)
                direction = direction;
            else if (HostPartyManager.Instance.AttendingParty)
                direction = direction;
            else
                direction = PlayerPrefs.GetInt("Direction" + decorInfo.Name);

            objPosition = DeserializeVector3Array(PlayerPrefs.GetString("ObjPosition" + decorInfo.Name));
        }
        this.gameObject.GetComponent <SpriteRenderer>().sprite = sprites[direction];
		if (!HostPartyManager.Instance.AttendingParty) {
			if (!Placed)
				Invoke ("MovementEnable", 0.6f);
		}
	}

	void OnMouseDown ()
	{
		mouseDown++;
		if (mouseDown == 2) {
			mouseDown = 0;
		}
	

		if (!this.gameObject.GetComponent <DragSnap> ().enabled) {	

			if (mouseDown == 1) {
				EnablePlacement ();
			} else {
				DesablePlacement ();
			}		
		}
	
	}

	public void DesablePlacement ()
	{
        if (ScreenManager.Instance.OpenedCustomizationScreen != "DecorEvent" || ScreenManager.Instance.ScreenMoved || ScreenManager.Instance.PopupShowed || ScreenAndPopupCall.Instance.placementEnabled)
            return;
        
		print ("Placement enabled");
		DecorController.Instance.SelectedDecor = this.gameObject;
        if(isForSociety)
        {
            var placementForSociety = ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().PlacementButton;
            placementForSociety.gameObject.SetActive(false);
        }
		ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (false);
		ScreenAndPopupCall.Instance.placementbutton.interactable = false;	
        ChangeFlatmateTouch(true);
	}

	public void EnablePlacement ()
	{
        if (ScreenManager.Instance.OpenedCustomizationScreen == "DecorEvent") {         
            print ("Placement enabled");
            DecorController.Instance.SelectedDecor = this.gameObject;
            ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (true);
            ScreenAndPopupCall.Instance.placementbutton.interactable = true;

            ScreenAndPopupCall.Instance.placementbutton.onClick.RemoveAllListeners ();
            ScreenAndPopupCall.Instance.placementbutton.onClick.AddListener (() => MovementEnable ());          
            ScreenAndPopupCall.Instance.placementbutton.gameObject.transform.SetSiblingIndex(23);
            ScreenAndPopupCall.Instance.placementbutton.gameObject.transform.localPosition = new Vector2(-428f, -180f);
        }
        else if(isForSociety)
        {
            var societyHomeRoom = ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>();
            if ((SocietyManager.Instance.myRole == 0 || SocietyManager.Instance.myRole == 1) && !societyHomeRoom.IsStorageOpen)
            {
                var placementForSociety= societyHomeRoom.PlacementButton;
                placementForSociety.gameObject.SetActive(true);
                placementForSociety.onClick.RemoveAllListeners();
                placementForSociety.onClick.AddListener(() => MovementEnable());
            }
        } 
        else {
            if (ScreenManager.Instance.ScreenMoved || ScreenManager.Instance.PopupShowed)
                return;

            print ("Placement enabled");
            DecorController.Instance.SelectedDecor = this.gameObject;
            ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (true);
            ScreenAndPopupCall.Instance.placementbutton.interactable = true;
            ChangeFlatmateTouch(false);
            ScreenAndPopupCall.Instance.placementbutton.onClick.RemoveAllListeners ();
            ScreenAndPopupCall.Instance.placementbutton.onClick.AddListener (() => MovementEnable ());
            ScreenAndPopupCall.Instance.placementbutton.gameObject.transform.SetSiblingIndex (10);  
            ScreenAndPopupCall.Instance.placementbutton.gameObject.transform.localPosition = new Vector2 (-452f, -230f);
            print (ScreenAndPopupCall.Instance.placementbutton.gameObject.transform.localPosition.ToString ());
         }
	}

    void ChangeFlatmateTouch(bool _enable)
    {
        for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) 
        {
            RoommateManager.Instance.RoommatesHired [i].gameObject.GetComponent<BoxCollider2D> ().enabled = _enable;
        }
    }

	public static string SerializeVector3Array (Vector3 aVectors)
	{
		StringBuilder sb = new StringBuilder ();

		sb.Append (aVectors.x).Append (" ").Append (aVectors.y).Append (" ").Append (aVectors.z).Append ("|");

		if (sb.Length > 0) // remove last "|"
			sb.Remove (sb.Length - 1, 1);
		return sb.ToString ();
	}

	public static Vector3 DeserializeVector3Array (string aData)
	{
		Vector3 result = new Vector3 (0, 0, 0);

		string[] values = aData.Split (' ');
		if (values.Length != 3)
			throw new System.FormatException ("component count mismatch. Expected 3 components but got " + values.Length);
		result = new Vector3 (float.Parse (values [0]), float.Parse (values [1]), float.Parse (values [2]));
		return result;
	}

	public void ChangeDirection ()
	{
		direction++;
		if (direction == sprites.Count)
			direction = 0;
		this.gameObject.GetComponent <SpriteRenderer> ().sprite = sprites [direction];
	}

	void MovementEnable ()
	{
		print ("Movement enabled");
		this.transform.GetChild (1).gameObject.SetActive (true);
//		var posCorrector = transform.GetChild (0).localPosition;
//		posCorrector = new Vector3 (0, posCorrector.y, 0);
//		transform.GetChild (1).localPosition = posCorrector;

		this.gameObject.GetComponent <DragSnap> ().enabled = true;
		if (ScreenManager.Instance.OpenedCustomizationScreen != "DecorEvent") {
			ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (false);
			ScreenManager.Instance.MenuScreen.transform.FindChild ("OpenCloseSlider").gameObject.SetActive (false);		
		}
        if(isForSociety)
            this.GetComponent<DragSnap> ().FindNearTilesInSociety (this.transform.FindChild ("New Sprite"),true);
        else
            this.GetComponent<DragSnap> ().FindNearGameObjectFinalToEnable (this.transform.FindChild ("New Sprite"));
		transform.FindChild ("Check").gameObject.SetActive (true);
		transform.FindChild ("Check").GetComponent<SpriteRenderer> ().color = Color.green;
		ScreenAndPopupCall.Instance.placementEnabled = true;
//			print (i);
//		}
	}

	public void Correct ()
	{
        if (ScreenManager.Instance.OpenedCustomizationScreen == "DecorEvent") {
			EventManagment.Instance.DecorEventList.Clear ();
			StartCoroutine (SetPositionForEvent ());
			EventManagment.Instance._registerButton.interactable = true;
			for (int i = 0; i < DecorController.Instance.TempDecorForEvent.Count; i++) {
				if (decorInfo.Name == DecorController.Instance.TempDecorForEvent [i].Name) {

					DecorController.Instance.TempDecorForEvent.Remove (DecorController.Instance.TempDecorForEvent [i]);
					if (DecorController.Instance.TempDecorForEvent.Count == 0) {
						DecorController.AddItemEnable = false;
					}
				}
			}
        }else if(isForSociety)
        {
            StartCoroutine(SetPositionForSociety());
        } 
        else {  
			ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (true);
			ScreenManager.Instance.MenuScreen.transform.FindChild ("OpenCloseSlider").gameObject.SetActive (true);
            ChangeFlatmateTouch(true);
			DragSnap dragSnap = GetComponent<DragSnap> ();
			if (!dragSnap.CanBePlacedHere) {
				dragSnap.CanBePlacedHere = true;
				if (dragSnap.nearestFinal) {
					this.transform.position = dragSnap.nearestFinal.transform.position;                   
				} else {
					var objPosition = Decor3DView.DeserializeVector3Array (PlayerPrefs.GetString ("ObjPosition" + decorInfo.Name));
					this.transform.position = objPosition;
				}
			}   

            if (!Placed) {
                DecorController.Instance.PlacedDecors.Add (this);
                DecorController.Instance.PlacedDecorsName.Add (decorInfo.Name);
                Placed = true;
            }
			StartCoroutine (SetPosition ());   
            ScreenAndPopupCall.Instance.placementEnabled = false;
		}
	}
    public IEnumerator SetPositionForSociety ()
    {
        var societyHomeRoom =  ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>();
        var postionString = SerializeVector3Array(transform.localPosition);
        CoroutineWithData cd = new CoroutineWithData (societyHomeRoom, societyHomeRoom.SavePositionInSocietyRoom(decorInfo.Id, postionString, direction));
        yield return cd.coroutine;

        if (cd.result.ToString() == "True" || cd.result.ToString() == "true")
        {      
            DragSnap dragSnap = GetComponent<DragSnap> ();
            if (!dragSnap.CanBePlacedHere) {
                dragSnap.CanBePlacedHere = true;
                if (dragSnap.nearestFinal) {
                    this.transform.localPosition = dragSnap.nearestFinal.transform.position;                   
                } 
            }   
            objPosition = transform.localPosition;
            if(!Placed)
            {
                var Pd = new SocietyDecor();
                Pd.Id = decorInfo.Id;
                Pd.Position = transform.localPosition;
                Pd.Rotation = direction;
                societyHomeRoom.PlacedDecorInRoom.Add(Pd);
            }
            Placed = true;
            ScreenAndPopupCall.Instance.placementEnabled = false;
            transform.FindChild ("Check").gameObject.SetActive (false);
            this.transform.GetChild (1).gameObject.SetActive (false);
            this.gameObject.GetComponent <DragSnap> ().enabled = false;

            var placementForSociety = ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().PlacementButton;
            placementForSociety.gameObject.SetActive(false);

            var AllDecorInSociety = transform.parent.GetComponentsInChildren<Decor3DView>();

            for (int i = 0; i < AllDecorInSociety.Length; i++) {   
                this.GetComponent<DragSnap> ().FindNearTilesInSociety (AllDecorInSociety [i].transform.FindChild ("New Sprite"), false);
            }

        }else
        {
            CrossSociety();
        }     
    }

    IEnumerator RemoveDecorFromSocietyRoom()
    {
        var societyHomeRoom =  ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>();
        var postionString = SerializeVector3Array(transform.localPosition);
        CoroutineWithData cd = new CoroutineWithData (societyHomeRoom, societyHomeRoom.RemoveDecorFromSociety(decorInfo.Id));
        yield return cd.coroutine;

        if (cd.result.ToString() == "True" || cd.result.ToString() == "true")
        {   
            ScreenAndPopupCall.Instance.placementEnabled = false;

            Destroy(gameObject);
            var placementForSociety = ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().PlacementButton;
            placementForSociety.gameObject.SetActive(false);

        }else
        {
            CrossSociety();
        }
    }

	public IEnumerator SetPositionForEvent ()
	{
		yield return null;
		foreach (var item in DownloadContent.Instance.downloaded_items) {
			
			if (decorInfo.Name.Trim ('"') == item.Name.Trim ('"')) {
				
//				for (int i = 0; i <= EventManagment.Instance.DecorEventList.Count; i++) {
//					if (EventManagment.Instance.DecorEventList.ContainsKey (i))
//						EventManagment.Instance.DecorEventList.Remove (i);					
//				}
				EventManagment.Instance.DecorEventList.Add (item.Item_id, SerializeVector3Array (transform.localPosition).Trim ('|') + "/" + SerializeVector3Array (transform.localScale).Trim ('|') + "/"
				+ direction);
			}
            ScreenAndPopupCall.Instance.placementbutton.onClick.RemoveAllListeners ();
            ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (false);
            ScreenAndPopupCall.Instance.placementbutton.interactable = false;
		}

		this.transform.GetChild (1).gameObject.SetActive (false);
		this.gameObject.GetComponent <DragSnap> ().enabled = false;
		ScreenAndPopupCall.Instance.placementbutton.onClick.RemoveAllListeners ();
		ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (false);
		ScreenAndPopupCall.Instance.placementbutton.interactable = false;
	}

	public IEnumerator SetPosition ()
	{
		PositionUpdate data = new PositionUpdate ();
		data.player_id = PlayerPrefs.GetInt ("PlayerId");
		foreach (var downloaded_item in DownloadContent.Instance.downloaded_items) {
			if (downloaded_item.Name.Trim ('"') == decorInfo.Name.Trim ('"')) {
				data.item_id = downloaded_item.Item_id;
			}
		}
   
		data.position = SerializeVector3Array (this.transform.position);
        data.rotation = sprites.FindIndex (x => x == gameObject.GetComponent<SpriteRenderer> ().sprite)+"_"+ Placed;
		data.cool_down_time_event_id = 0;
		data.cool_time = "";
		data.is_busy_time = "";

		CoroutineWithData cd = new CoroutineWithData (DownloadContent.Instance, DownloadContent.Instance.SendPositions (data));
		yield return cd.coroutine;

		if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
			this.transform.GetChild (1).gameObject.SetActive (false);
			this.gameObject.GetComponent <DragSnap> ().enabled = false;
			PlayerPrefs.SetInt ("Direction" + decorInfo.Name, direction);
			objPosition = this.transform.position;
			PlayerPrefs.SetString ("ObjPosition" + decorInfo.Name, SerializeVector3Array (objPosition));
			ScreenAndPopupCall.Instance.placementbutton.onClick.RemoveAllListeners ();
			ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (false);
			ScreenAndPopupCall.Instance.placementbutton.interactable = false;

			if (mouseDown != 0)
				OnMouseDown ();		

			//		GameManager.Instance.AddExperiencePoints (2f);
			if (!tutorial._SofaPlaced && tutorial.purchaseSofa > 1)
				tutorial.PurchaseSofa ();

			if (!tutorial._QuestAttended && tutorial._PublicAreaAccessed && tutorial.questAttended > 8) {
//				tutorial.QuestAttendingStart ();			

			}
//			transform.GetComponent<DragSnap> ().FindNeartGameObject ();
			/// Bug fixed of Sprint 6 on Date 16/11/2016
//			for (int i = 0; i < DecorController.Instance.PlacedDecors.Count; i++) {
			this.GetComponent<DragSnap> ().FindNearGameObjectFinal (this.transform.FindChild ("New Sprite"));
			transform.FindChild ("Check").GetComponent<SpriteRenderer> ().color = Color.white;
//				print (i);
//			}
		}
		transform.FindChild ("Check").gameObject.SetActive (false);

	}


	public void Cross ()
	{
        if(isForSociety)
        {
            CrossSociety();
            return;
        }
		ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (true);
		ScreenManager.Instance.MenuScreen.transform.FindChild ("OpenCloseSlider").gameObject.SetActive (true);
        ChangeFlatmateTouch(true);	
		this.transform.GetChild (1).gameObject.SetActive (false);
		this.gameObject.GetComponent <DragSnap> ().enabled = false;
		direction = PlayerPrefs.GetInt ("Direction" + decorInfo.Name);
		this.gameObject.GetComponent <SpriteRenderer> ().sprite = sprites [direction];
		objPosition = DeserializeVector3Array (PlayerPrefs.GetString ("ObjPosition" + decorInfo.Name));
		this.transform.position = objPosition;
		ScreenAndPopupCall.Instance.placementbutton.onClick.RemoveAllListeners ();
		ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (false);
		ScreenAndPopupCall.Instance.placementbutton.interactable = false;

		DecorController.Instance.SelectedDecor = null;

		if (!tutorial._SofaPlaced)
			tutorial.SofalevelDecrease ();

		if (!Placed) {
			GameObject.Destroy (this.gameObject);
			if (tutorial._PublicAreaAccessed && !tutorial._QuestAttended) {
//				tutorial.QuestAttendingStart ();
			}
		} else {
			print ("Already placed");
		}

		for (int i = 0; i < DecorController.Instance.PlacedDecors.Count; i++) {
			this.GetComponent<DragSnap> ().FindNearGameObjectFinal (DecorController.Instance.PlacedDecors [i].transform.FindChild ("New Sprite"));
			print (i);
		}
		transform.FindChild ("Check").gameObject.SetActive (false);

		if (ScreenManager.Instance.OpenedCustomizationScreen == "DecorEvent") {
			DecorController.Instance.IntializedecorItemesforDecorEventStorage ();
		}
        ScreenAndPopupCall.Instance.placementEnabled = false;

	}
    public void CrossSociety ()
    {
        this.transform.GetChild (1).gameObject.SetActive (false);
        this.gameObject.GetComponent <DragSnap> ().enabled = false;
        this.gameObject.GetComponent <SpriteRenderer> ().sprite = sprites [direction];
//        objPosition = DeserializeVector3Array (PlayerPrefs.GetString ("ObjPosition" + decorInfo.Name));
        this.transform.localPosition = objPosition;

        var placementForSociety = ScreenManager.Instance.HomeRoomSociety.GetComponent<SocietyHomeRoom>().PlacementButton;
        placementForSociety.gameObject.SetActive(false);

        DecorController.Instance.SelectedDecor = null;

        if (!Placed) {
            GameObject.Destroy (this.gameObject);           
        } else {
            print ("Already placed");
        }
        var AllDecorInSociety = transform.parent.GetComponentsInChildren<Decor3DView>();

        for (int i = 0; i < AllDecorInSociety.Length; i++) {   
            this.GetComponent<DragSnap> ().FindNearTilesInSociety (AllDecorInSociety [i].transform.FindChild ("New Sprite"), false);
//            print (i);
        }
        transform.FindChild ("Check").gameObject.SetActive (false);       
//        DecorController.Instance.IntializedecorForSocietyRoom (0);
        ScreenAndPopupCall.Instance.placementEnabled = false;
    }


	public void CreateDecore (DecorData decoredata)
	{
		decorInfo = decoredata;
	}

	public void SetPositionofThisItem (string positionString, int rotation)
	{
		gameObject.GetComponent<Transform> ().position = DeserializeVector3Array (positionString);
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [rotation];
	}

    public void SetPositionofThisItem (Vector3 Localposition, int rotation)
    {
        objPosition = Localposition;
        direction = rotation;
        gameObject.transform.localPosition = objPosition;
        gameObject.GetComponent<SpriteRenderer> ().sprite = sprites [direction];
    }

    public void SaveToStorage()
    {
        if(isForSociety)
        {
            StartCoroutine(RemoveDecorFromSocietyRoom());
            return;  
        }
        //All screen set ups
        ScreenManager.Instance.CellPhone.transform.FindChild ("CellButton").gameObject.SetActive (true);
        ScreenManager.Instance.MenuScreen.transform.FindChild ("OpenCloseSlider").gameObject.SetActive (true);
        ChangeFlatmateTouch(true);  
        this.transform.GetChild (1).gameObject.SetActive (false);
        this.gameObject.GetComponent <DragSnap> ().enabled = false;

        ScreenAndPopupCall.Instance.placementbutton.onClick.RemoveAllListeners ();
        ScreenAndPopupCall.Instance.placementbutton.gameObject.SetActive (false);
        ScreenAndPopupCall.Instance.placementbutton.interactable = false;

        DecorController.Instance.SelectedDecor = null;
        ScreenAndPopupCall.Instance.placementEnabled = false;

        // Saving to storage code below

        Destroy(gameObject);
        Placed = false;
        DecorController.Instance.PlacedDecors.Remove (this);
        DecorController.Instance.PlacedDecorsName.Remove (decorInfo.Name);
        StartCoroutine (SetPosition ());  
    }
}