using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PublicAreaPlayer : Photon.MonoBehaviour
{

	Vector3 correctPlayerPos = Vector3.zero;
	Vector3 correctPlayerScale = Vector3.one;
	// Received datas
    public PlayerData PlayerData = new PlayerData();
    public CustomCharacter customCharacter = new CustomCharacter();


	public int PositionIndexInMp;
	//	public CustomCharacter receivedCustChar;

	bool DressApplied = false;

	void Start ()
	{
		if (photonView.isMine) {

			PlayerData.DressWeared = PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.Dress;
            customCharacter = PlayerManager.Instance.custChar;
			ApplyDataToThisView ();
		}
	
	}

	void Update ()
	{
		
		if (!photonView.isMine) {
			transform.position = this.correctPlayerPos;
			transform.localScale = this.correctPlayerScale;
			//			PlayerManager.Instance.playerInfo = receivedData;
			//			player.customchar = receivedCustChar;
		}	
	}

	public void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.localScale);

			stream.SendNext (PlayerManager.Instance.playerInfo.EmailId);
			stream.SendNext (PlayerManager.Instance.playerInfo.Username);
			stream.SendNext (PlayerManager.Instance.playerInfo.player_id);

            stream.SendNext(customCharacter.skin_tone);
            stream.SendNext(customCharacter.gender);

            stream.SendNext(customCharacter.eyes);
            stream.SendNext(customCharacter.nose);
            stream.SendNext(customCharacter.lips);
            stream.SendNext(customCharacter.ears);
            stream.SendNext(customCharacter.hair);
            stream.SendNext(customCharacter.shoes);

			string KeyString = "";
			string ValueString = "";
			foreach (var key in PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.Dress.Keys) {
				KeyString += key + ",";
				ValueString += (PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.Dress [key]) + ",";
			}

			stream.SendNext (KeyString);

			stream.SendNext (ValueString);
			stream.SendNext (PositionIndexInMp);
		} else {
			this.correctPlayerPos = (Vector3)stream.ReceiveNext ();

			this.correctPlayerScale = (Vector3)stream.ReceiveNext ();
//
//			Player player = GetComponent <Player> ();

			PlayerData.EmailId = (string)stream.ReceiveNext ();
			PlayerData.Username = (string)stream.ReceiveNext ();
			PlayerData.player_id = (int)stream.ReceiveNext ();

            customCharacter.skin_tone =(int)stream.ReceiveNext();
            customCharacter.gender =(int)stream.ReceiveNext();

            customCharacter.eyes = (string) stream.ReceiveNext();
            customCharacter.nose = (string) stream.ReceiveNext();
            customCharacter.lips = (string) stream.ReceiveNext();
            customCharacter.ears = (string) stream.ReceiveNext();
            customCharacter.hair = (string) stream.ReceiveNext();
            customCharacter.shoes = (string) stream.ReceiveNext();

			string[] Keys = stream.ReceiveNext ().ToString ().Split (',');

			string[] Values = stream.ReceiveNext ().ToString ().Split (',');

			for (int i = 0; i < Mathf.Min (Keys.Length, Values.Length); i++) {
                int Id = 0;
                int.TryParse(Values [i],out Id);

				if (PlayerData.DressWeared.ContainsKey (Keys [i]))
                    PlayerData.DressWeared [Keys [i]] = Id;
				else
                    PlayerData.DressWeared.Add (Keys [i], Id);
			}

			PositionIndexInMp = (int)stream.ReceiveNext ();

			ApplyDataToThisView ();
		}
	}


	void ApplyDataToThisView ()
	{		
		if (!DressApplied) {
			var Originaldumyy = DressManager.Instance.dummyCharacter;
            ChangeCustomisations();
       
                
			foreach (var dresskey in PlayerData.DressWeared.Keys) {
				var DressId = PlayerData.DressWeared [dresskey];

                DressItem mydress = FindDress (DressId);		
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = this.gameObject;
//					if (this.GetComponent<CharacterProperties> ().PlayerType == "White")
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);

//					if (this.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//
//					if (this.GetComponent<CharacterProperties> ().PlayerType == "Black")
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
				}
			}
			DressManager.Instance.dummyCharacter = Originaldumyy;

			MultiplayerManager.Instance.CurrentPublicArea.RemovePointAtIndex (PositionIndexInMp);
		}
		DressApplied = true;
	}

    DressItem FindDress (int Id)
	{
		foreach (var dress in PurchaseDressManager.Instance.AllDresses) {
            if (dress.Id == Id) {
				return dress;
			}
		}
		return null;
	}

    void ChangeCustomisations()
    {
        var bodyParts = this.GetComponentsInChildren<BodyParts>(true);

        string skintone = "";

        switch (customCharacter.skin_tone) 
        {
            case 0:
                skintone = "White";
                break;
            case 1:
                skintone = "Brown";
                break;
            case 2:
                skintone = "Black";
                break;
        }

        var eyes = customCharacter.eyes.Split('_');
        var nose = customCharacter.nose.Split('_');
        var lips = customCharacter.lips.Split('_');
        var ears = customCharacter.ears.Split('_');
        var hair = customCharacter.hair.Split('_');
        var shoes = customCharacter.shoes.Split('_');

        int eye_number = 0;
        int nose_number = 0;
        int lips_number = 0;
        int ears_number = 0;
        int hair_number = 0;        
        int shoes_number = 0;

        int.TryParse(eyes[1],out eye_number);
        int.TryParse(nose[1],out nose_number);
        int.TryParse(lips[1],out lips_number);
        int.TryParse(ears[1],out ears_number);
        int.TryParse(hair[1],out hair_number);
        int.TryParse(shoes[1],out shoes_number);


        var eye_color = eyes[0];
        var nose_color = nose[0];
        var ear_color = ears[0];
        var lips_color = lips[0];
        var hairs_color = hair[0];
        var shoes_color = shoes[0];    

        string gender ="";
        if (customCharacter.gender == 0)
            gender = "Female";
        else
            gender = "Male";
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "SkinTone",skintone,0);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Eyes", eye_color, eye_number);
        PlayerManager.Instance.ApplyDress(bodyParts,gender,"Nose", nose_color, nose_number);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Lips", lips_color, lips_number);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Ears", ear_color, ears_number);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Hairs", hairs_color, hair_number);

        PlayerManager.Instance.ChangeShoesOfCharacter(bodyParts, gender,skintone, shoes_color,shoes_number);
    }

	void OnPhotonInstantiate (PhotonMessageInfo info)
	{
//		if(MultiplayerManager.Instance._forCoOp)
//		{
//
//		}
//		info.photonView.transform.localScale= new Vector3 (0.4f,0.4f,1f);

//		if(info.photonView.isMine)
//		{
//			info.photonView.GetComponent <Player>().data = PlayerManager.Instance.MainCharacter.GetComponent <Player>().data;
//			info.photonView.GetComponent <Player>().customchar = PlayerManager.Instance.MainCharacter.GetComponent <Player>().customchar;
//		print (" OnPhotonInstantiate calledd  ======>>>>>>>>>>" + info.sender.name);
//		}
	}
}