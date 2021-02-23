using UnityEngine;
using System.Collections;

public class PlayerNetworkForFlatParty : Photon.MonoBehaviour
{

	Vector3 realPlayerPosition = Vector3.zero;
	Vector3 realPlayerScale = Vector3.one;
	// Received datas
	public PlayerData PlayerDataForFlatParty;

	public int PositionIndexInMp;
	//	public CustomCharacter receivedCustChar;

	bool apliedDress = false;
	public int playerId;

    CustomCharacter _customChar = new CustomCharacter();

	// Use this for initialization
	void Start ()
	{
		if (photonView.isMine) {
			PlayerDataForFlatParty.DressWeared = PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.Dress;
            _customChar = PlayerManager.Instance.custChar;
			ApplyDataToThisView ();
			playerId = this.gameObject.GetComponent<PhotonView> ().ownerId;
			if (HostPartyManager.Instance.selectedFlatParty != null) {
				if (HostPartyManager.Instance.selectedFlatParty.PlayerId == PlayerPrefs.GetInt ("PlayerId")) {		
					if (!PhotonPlayer.Find (playerId).isMasterClient) {
						PhotonNetwork.SetMasterClient (PhotonPlayer.Find (playerId));
					}
				}
			}
		}
		playerId = this.gameObject.GetComponent<PhotonView> ().ownerId;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!photonView.isMine) {
			transform.position = this.realPlayerPosition;
			transform.localScale = this.realPlayerScale;
			apliedDress = false;
			ApplyDataToThisView ();
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

			string KeyString = "";
			string ValueString = "";
			foreach (var key in PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.Dress.Keys) {
				KeyString += key + ",";
				ValueString += (PlayerManager.Instance.MainCharacter.GetComponent <Flatmate> ().data.Dress [key]) + ",";
			}

			stream.SendNext (KeyString);

			stream.SendNext (ValueString);
			stream.SendNext (PositionIndexInMp);

            stream.SendNext(_customChar.skin_tone);

            stream.SendNext(_customChar.gender);

            stream.SendNext(_customChar.eyes);
            stream.SendNext(_customChar.nose);
            stream.SendNext(_customChar.lips);
            stream.SendNext(_customChar.ears);
            stream.SendNext(_customChar.hair);
            stream.SendNext(_customChar.shoes);

		} else {
			this.realPlayerPosition = (Vector3)stream.ReceiveNext ();

			this.realPlayerScale = (Vector3)stream.ReceiveNext ();
			//
			//			Player player = GetComponent <Player> ();

			PlayerDataForFlatParty.EmailId = (string)stream.ReceiveNext ();
			PlayerDataForFlatParty.Username = (string)stream.ReceiveNext ();
			PlayerDataForFlatParty.player_id = (int)stream.ReceiveNext ();

			string[] Keys = stream.ReceiveNext ().ToString ().Split (',');

			string[] Values = stream.ReceiveNext ().ToString ().Split (',');

			for (int i = 0; i < Mathf.Min (Keys.Length, Values.Length); i++) {
                int Id = 0;
                int.TryParse(Values[i], out Id);
				if (PlayerDataForFlatParty.DressWeared.ContainsKey (Keys [i]))
                    PlayerDataForFlatParty.DressWeared [Keys [i]] = Id;
				else
                    PlayerDataForFlatParty.DressWeared.Add (Keys [i], Id);
			}

			PositionIndexInMp = (int)stream.ReceiveNext ();

            _customChar.skin_tone =(int)stream.ReceiveNext();
            _customChar.gender =(int)stream.ReceiveNext();

            _customChar.eyes = (string) stream.ReceiveNext();
            _customChar.nose = (string) stream.ReceiveNext();
            _customChar.lips = (string) stream.ReceiveNext();
            _customChar.ears = (string) stream.ReceiveNext();
            _customChar.hair = (string) stream.ReceiveNext();
            _customChar.shoes = (string) stream.ReceiveNext();		}
	}

	void ApplyDataToThisView ()
	{		
		if (!apliedDress) {
			var Originaldumyy = DressManager.Instance.dummyCharacter;
            ChangeCustomisations();
			foreach (var dresskey in PlayerDataForFlatParty.DressWeared.Keys) {
//				var DressCat = dresskey;
                var DressId = PlayerDataForFlatParty.DressWeared [dresskey];
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
		apliedDress = true;
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

        switch (_customChar.skin_tone) 
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

        var eyes = _customChar.eyes.Split('_');
        var nose = _customChar.nose.Split('_');
        var lips = _customChar.lips.Split('_');
        var ears = _customChar.ears.Split('_');
        var hair = _customChar.hair.Split('_');
        var shoes = _customChar.shoes.Split('_');

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
        if (_customChar.gender == 0)
            gender = "Female";
        else
            gender = "Male";

        PlayerManager.Instance.ApplyDress(bodyParts,gender, "SkinTone",skintone,0);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Eyes", eye_color, eye_number);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Nose", nose_color, nose_number);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Lips", lips_color, lips_number);
        PlayerManager.Instance.ApplyDress(bodyParts, gender,"Ears", ear_color, ears_number);
        PlayerManager.Instance.ApplyDress(bodyParts,gender, "Hairs", hairs_color, hair_number);

        PlayerManager.Instance.ChangeShoesOfCharacter(bodyParts, gender,skintone, shoes_color,shoes_number);
    }
}
