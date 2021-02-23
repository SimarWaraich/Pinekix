using UnityEngine;
using System.Collections;

public class NetworkPlayerforCoOp : Photon.MonoBehaviour
{
	Vector3 correctPlayerPos;
	Vector3 correctPlayerScale;
	public string ReceiverFlatmateName;
	string Parent;

	public CoOpPlayerData Data;
	Flatmate SelectedFlatmateForCoOp;

	// Use this for initialization
	void Start ()
	{
		if (photonView.isMine) 
		{
			SelectedFlatmateForCoOp = RoommateManager.Instance.SelectedRoommate.GetComponent <Flatmate> ();

			Data.UserName = PlayerManager.Instance.playerInfo.Username;
			Data.PlayerId = PlayerManager.Instance.playerInfo.player_id;
			Data.Level = GameManager.Instance.level;
			Data.FlatmateId = SelectedFlatmateForCoOp.data.Id;

			if(SelectedFlatmateForCoOp.gameObject.GetComponent<CharacterProperties> ().PlayerType == "White")
				Data.SkinColor = 1;
			else if(SelectedFlatmateForCoOp.gameObject.GetComponent<CharacterProperties> ().PlayerType == "Brown")
				Data.SkinColor = 2;
			else if(SelectedFlatmateForCoOp.gameObject.GetComponent<CharacterProperties> ().PlayerType == "Black")
				Data.SkinColor = 3;

			Data.Dresses = SelectedFlatmateForCoOp.data.Dress;
			Data.Gender = GameManager.GetGender ();
//			Data.CharacterType = SelectedFlatmateForCoOp.GetComponent <CharacterProperties> ().PlayerType;
			CoOpEventController.Instance.Player1Data = Data;

            ApplyDress ();
            InstantiateSelectedFlatmate();
		}
//		else if (!photonView.isMine) {
//			GameObject ParentGO = GameObject.Find (Parent);
//			if (ParentGO) {
//				this.transform.parent = ParentGO.transform.GetChild (0);
//				transform.localPosition = correctPlayerPos;
//				transform.localScale = correctPlayerScale;
//				CoOpEventController.Instance.Player2Data = Data;
//
//				ApplyDress ();
//			}
//		}
	}
    bool Instantiated = false;

    void InstantiateSelectedFlatmate()
    {
        if (Instantiated)
            return;
        if (Data.FlatmateId == 0)
            return;
        GameObject Avatar = null;

        foreach (var flatmate in RoommateManager.Instance.AllRoommatesData)
        {
            if (flatmate.Id == Data.FlatmateId)
            {             
                Avatar = GameObject.Instantiate(flatmate.Prefab, transform) as GameObject;
            }
        }
        if(Avatar == null)
        {
            Avatar = GameObject.Instantiate(PlayerManager.Instance.MainCharacter, transform) as GameObject;
            StartCoroutine( PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Avatar, Data.PlayerId));
        }

        Avatar.transform.localPosition = Vector3.zero;
        Avatar.transform.localScale = Vector3.one;
        Avatar.SetLayerRecursively(LayerMask.NameToLayer ("UI3D"));
        Destroy(Avatar.GetComponent<Flatmate>());
        Destroy(Avatar.GetComponent<GenerateMoney>());
        Destroy(Avatar.transform.FindChild("low money").gameObject);

        Instantiated = true;
    }

    void ApplyDress ()
	{      
		var Originaldumyy = DressManager.Instance.dummyCharacter;

		foreach (var dresskey in Data.Dresses.Keys) {
			var DressCat = dresskey;
			var DressId = Data.Dresses [dresskey];
			if(DressCat.Contains ("Hair")){
                SaloonItem hairItem = FindSaloon (DressId);
				if (hairItem != null) {
                    DressManager.Instance.dummyCharacter = transform.gameObject;
//					if (Data.SkinColor == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.HairImages);
//					else if (Data.SkinColor == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.Brown_Images);
//					else if (Data.SkinColor == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (hairItem.PartName, hairItem.Black_Images);
//					else
                    DressManager.Instance.ChangeHairsForDummyCharacter (hairItem.PartName, hairItem.HairImages);
					
				}
			}
			else
			{
				DressItem mydress = FindDress (DressId);		
				if (mydress != null) {
                    DressManager.Instance.dummyCharacter = transform.gameObject;
//					if (Data.SkinColor == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (Data.SkinColor == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (Data.SkinColor == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}}
		}
		DressManager.Instance.dummyCharacter = Originaldumyy;       
	}

	DressItem FindDress (int DressId)
	{
		foreach (var dress in PurchaseDressManager.Instance.AllDresses) {
            if (dress.Id == DressId) {
				return dress;
			}
		}
		return null;
	}

    SaloonItem FindSaloon (int HairId)
	{
		foreach (var hair in PurchaseSaloonManager.Instance.AllItems) {
            if (hair.item_id == HairId) {
				return hair;
			}
		}
		return null;
	}

	
	// Update is called once per frame
	void Update ()
	{
		if (!photonView.isMine) {
			GameObject ParentGO = GameObject.Find (Parent);
			if (ParentGO) {
				this.transform.parent = ParentGO.transform.GetChild (0);
				transform.localPosition = correctPlayerPos;
				transform.localScale = correctPlayerScale;
				CoOpEventController.Instance.Player2Data = Data;
//				this.transform.parent.parent.GetComponent <Camera>().enabled = true;
                ApplyDress ();
                InstantiateSelectedFlatmate();
//				CoOpEventController.Instance.hasOtherPlayerEntered = true;
			}
        }else
        {
            if (Data != null)
            {
                ApplyDress();
                InstantiateSelectedFlatmate();
            }
        }
	}
    GameObject FindFlatMateAvtar (int Id)
    {
        foreach (var flatmate in RoommateManager.Instance.AllRoommatesData)
        {
            if (flatmate.Id == Id)
            {
                return flatmate.Prefab;
            }
        }
        return null;
    }
	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {

			stream.SendNext (transform.parent.parent.gameObject.name); // Parent at 1

			stream.SendNext (transform.localPosition); //position at 2
			stream.SendNext (transform.localScale); // scale at 3

			stream.SendNext (Data.UserName); // UserName at 4
			stream.SendNext (Data.PlayerId); // PlayerId at 5
			stream.SendNext (Data.Level); // Level at 6

			stream.SendNext (Data.SkinColor); // flatmatename at 7
			stream.SendNext (Data.FlatmateId);// flatmate Id at 8




			string KeyString = "";
			string ValueString = "";
			foreach (var key in Data.Dresses.Keys) {
				KeyString += key + ",";
				ValueString += (Data.Dresses [key]) + ",";
			}

			stream.SendNext (KeyString); // Dress category at 9
			stream.SendNext (ValueString);// Dress Name at 10
		
			stream.SendNext ((int)Data.Gender);
			int Status = 0;
			if (!MultiplayerManager.Instance._isReciever) {
				Status = CoOpEventController.Instance.hasSubmitCompleted;			
			} else
				Status = 0;
			stream.SendNext (Status);

            bool ClothesSubmittedByMe = CoOpEventController.Instance.hasMyPlayerEntered; 
            stream.SendNext (ClothesSubmittedByMe);     

		} 

		else {
			
			Parent = (string)stream.ReceiveNext ();// Parent at 1

			this.correctPlayerPos = (Vector3)stream.ReceiveNext (); //position at 2
			this.correctPlayerScale = (Vector3)stream.ReceiveNext ();// scale at 3

			Data.UserName = (string)stream.ReceiveNext ();// UserName at 4
			Data.PlayerId = (int)stream.ReceiveNext ();// PlayerId at 5
			Data.Level = (int) stream.ReceiveNext (); // Level at 6

			Data.SkinColor = (int)stream.ReceiveNext (); // Flatmate name at 7
			Data.FlatmateId = (int) stream.ReceiveNext ();// Flatmate Id at 8

			string[] Keys = stream.ReceiveNext ().ToString ().Split (','); // Dress category at 9
			string[] Values = stream.ReceiveNext ().ToString ().Split (',');// Dress Name at 10

			for (int i = 0; i < Mathf.Min (Keys.Length -1, Values.Length - 1); i++) {
                int Id = 0;
                int.TryParse(Values[i], out Id);
				if (Data.Dresses.ContainsKey (Keys [i]))
                    Data.Dresses [Keys [i]] = Id;
				else
                    Data.Dresses.Add (Keys [i], Id);
			}
			int GenderInt = (int)stream.ReceiveNext ();
			Data.Gender = (GenderEnum)GenderInt;
			int Status = 0;

			Status = (int)stream.ReceiveNext ();
            CoOpEventController.Instance.hasSubmitCompleted = Status;	

            CoOpEventController.Instance.hasOtherPlayerEntered = (bool)stream.ReceiveNext ();   
		}
	}

	void OnPhotonInstantiate(PhotonMessageInfo info) 
	{
//		if(info.photonView.isMine)
//		{
//
//		}
	}
}