/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 07 July 2k16
/// This script will be used to manage the player functionality of the game
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simple_JSON;

public class PlayerManager : Singleton<PlayerManager>
{
	public PlayerData playerInfo;
	public CustomCharacter custChar = new CustomCharacter ();
	public GameObject MainCharacter;

	public Sprite IconMale;
	public Sprite IconFemale;

	// Use this for initialization

	void Awake ()
	{
		this.Reload ();
	}

	public void OnEnable ()
	{
		DontDestroyOnLoad (this.gameObject);
	}


	public void UpdateData ()
	{
		playerInfo.Name = PlayerPrefs.GetString ("UserName");
		playerInfo.Username = PlayerPrefs.GetString ("UserName");
		playerInfo.EmailId = PlayerPrefs.GetString ("UserEmail");
		playerInfo.player_id = PlayerPrefs.GetInt ("PlayerId");
	}


	public IEnumerator GetCharacterCustomisations ()
	{
		playerInfo.Name = PlayerPrefs.GetString ("UserName");
		playerInfo.Username = PlayerPrefs.GetString ("UserName");
		playerInfo.EmailId = PlayerPrefs.GetString ("UserEmail");
		playerInfo.player_id = PlayerPrefs.GetInt ("PlayerId");


        CoroutineWithData cd = new CoroutineWithData (this, GetCharacterData (PlayerPrefs.GetInt ("PlayerId")));
		yield return cd.coroutine;

        if (cd.result != null) {
            
            if (PlayerPrefs.GetString ("CharacterType") == "Male") {
                PlayerManager.Instance.MainCharacter = CharacterCustomizationAtStart.Instance.MaleCharacter;
            } else {
                PlayerManager.Instance.MainCharacter = CharacterCustomizationAtStart.Instance.FemaleCharacter;
            }   

            CharacterCustomizationAtStart.Instance.SelectedCharacter = PlayerManager.Instance.MainCharacter;

			StartCoroutine (UpdateCharacter ());

		} else {
			StartCoroutine (GetCharacterCustomisations ());	
		}
	}

    public IEnumerator GetCharacterData (int PlayerId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		CustomCharacter customchar = new CustomCharacter ();
        customchar.player_id = PlayerId;
		string json = JsonUtility.ToJson (customchar);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/players/getcustomcharacter", encoding.GetBytes (json), postHeader);

//		print ("jsonDtat is ==>> " + json); 
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
//			print ("www.text ==>> " + www.text);
//			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("inserted") || _jsnode ["status"].ToString ().Contains ("200")) {
//				print ("Success");

				JSONNode data = _jsnode ["data"];

				int playerId = 0;
				int.TryParse (data ["player_id"], out playerId);
				int gender = 0;
				int.TryParse (data ["gender"], out gender);
				int skinTone = 0;
				int.TryParse (data ["skin_tone"], out skinTone);
                var cust_Char = new CustomCharacter();
				cust_Char.player_id = playerId;               
                cust_Char.gender = gender;              
                cust_Char.skin_tone = skinTone;
                cust_Char.eyes = data ["eyes"];
                cust_Char.nose = data ["nose"];
                cust_Char.lips = data ["lips"];
                cust_Char.ears = data ["ears"];
                cust_Char.hair = data ["hair"];
                cust_Char.shoes = data ["shoes"];
                cust_Char.clothing = data ["clothing"];

                if (PlayerId == PlayerPrefs.GetInt("PlayerId"))
                {
                    custChar = cust_Char;

                    PlayerPrefs.SetInt("SkinToneColor", custChar.skin_tone);

                    if (custChar.gender == 0)
                        PlayerPrefs.SetString("CharacterType", "Female");
                    else
                        PlayerPrefs.SetString("CharacterType", "Male");
                }
                else
                {
                    
                }

                yield return cust_Char;
			} else {
//				print ("error" + www.error);
                yield return null;
			}
		} else {
            yield return null;
		}
	}


	public bool Character_Dress_Changed = false;

	public IEnumerator UpdateCharacter ()
	{
        switch (PlayerPrefs.GetInt ("SkinToneColor")) {
		case 0:
                MainCharacter.GetComponent<CharacterProperties> ().PlayerType = "White";
			break;
		case 1:
                MainCharacter.GetComponent<CharacterProperties> ().PlayerType = "Brown";
			break;
		case 2:
                MainCharacter.GetComponent<CharacterProperties> ().PlayerType = "Black";
			break;
		}
        string Skin = MainCharacter.GetComponent<CharacterProperties> ().PlayerType;


//		Dictionary<string, GameObject> child_dict = new Dictionary<string, GameObject> ();
        var bodyParts = MainCharacter.GetComponentsInChildren<BodyParts>(true);

		var skintone = custChar.skin_tone;
		var eye_number = int.Parse (custChar.eyes.Substring (custChar.eyes.LastIndexOf ('_') + 1));
		var nose_number = int.Parse (custChar.nose.Substring (custChar.nose.LastIndexOf ('_') + 1));
		var lips_number = int.Parse (custChar.lips.Substring (custChar.lips.LastIndexOf ('_') + 1));
		var ears_number = int.Parse (custChar.ears.Substring (custChar.ears.LastIndexOf ('_') + 1));
		var hair_number = int.Parse (custChar.hair.Substring (custChar.hair.LastIndexOf ('_') + 1));
        var shoes_number = int.Parse (custChar.shoes.Substring (custChar.shoes.LastIndexOf ('_') + 1));
//		var dress_number = int.Parse (custChar.clothing.Substring (custChar.clothing.LastIndexOf ('_') + 1));


		var eye_color = "";
		var nose_color = "";
		var ear_color = "";
		var lips_color = "";
        var hairs_color = "";
        var shoes_color = "";
		foreach (var c in custChar.eyes) {
			if (c == '_') {
				break;
			} else
				eye_color += c.ToString ();
		}
		foreach (var c in custChar.nose) {
			if (c == '_') {
				break;
			} else
				nose_color += c.ToString ();
		}
		foreach (var c in custChar.ears) {
			if (c == '_') {
				break;
			} else
				ear_color += c.ToString ();
		}
		foreach (var c in custChar.lips) {
			if (c == '_') {
				break;
			} else
				lips_color += c.ToString ();
		}
        foreach (var c in custChar.hair) {
            if (c == '_') {
                break;
            } else
                hairs_color += c.ToString ();
        }  
        foreach (var c in custChar.shoes) {
            if (c == '_') {
                break;
            } else
                shoes_color += c.ToString ();
        }
        var Gender = PlayerPrefs.GetString("CharacterType");

//		var Target_1 = child_dict ["lower1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["upper1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["right arm1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["right mid arm"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["right hand1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["left arm1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["left arm mid1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["left hand1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["face1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["right leg1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["right leg mid1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["right leg btm1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["left leg1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["left leg mid1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//		Target_1 = child_dict ["left leg btm1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (skintone, 0);
//
//
//
//
//		Target_1 = child_dict ["left eye"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (0, eye_number);
//		Target_1 = child_dict ["right eye"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (0, eye_number);
//
//
//
//		Target_1 = child_dict ["nose"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (0, nose_number);
//
//
//
//		Target_1 = child_dict ["lips"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (0, lips_number);
//
//
//		if (PlayerPrefs.GetString ("CharacterType") == "Male") {
//			Target_1 = child_dict ["ear"];
//			Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), ears_number);
//		} else {
//			Target_1 = child_dict ["ear"];
//			Target_1.GetComponent<StartImageManager> ().ChangeImage (ears_number, 0);
//		}
//
//
//		if (PlayerPrefs.GetString ("CharacterType") == "Male") {
//			Target_1 = child_dict ["hair"];
//			Target_1.GetComponent<StartImageManager> ().ChangeImage (hair_number, 0);
//		} else {
//			Target_1 = child_dict ["front hair"];
//			Target_1.GetComponent<StartImageManager> ().ChangeImage (hair_number, 0);
//			Target_1 = child_dict ["back hair"];
//			Target_1.GetComponent<StartImageManager> ().ChangeImage (hair_number, 0);
//		}
//
//
//
//		//shoes
//		Target_1 = child_dict ["left leg btm1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), shoes_number);
//		Target_1 = child_dict ["right leg btm1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), shoes_number);
//
//
//		Target_1 = child_dict ["lower1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), dress_number);
//		Target_1 = child_dict ["upper1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), dress_number);
//		Target_1 = child_dict ["left leg1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), dress_number);
//		Target_1 = child_dict ["right leg1"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (PlayerPrefs.GetInt ("SkinToneColor"), dress_number);
//
//
//
//
//		Target_1 = child_dict ["left eye"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (int.Parse (eye_color), eye_number);
//		var Target_2 = child_dict ["right eye"];
//		Target_2.GetComponent<StartImageManager> ().ChangeImage (int.Parse (eye_color), eye_number);
//
//
//		Target_1 = child_dict ["nose"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (int.Parse (nose_color), nose_number);
//
//
//		Target_1 = child_dict ["lips"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (int.Parse (lips_color), lips_number);
//
//
//
//		Target_1 = child_dict ["ear"];
//		Target_1.GetComponent<StartImageManager> ().ChangeImage (int.Parse (ear_color), ears_number);
        ApplyDress(bodyParts,Gender, "SkinTone", Skin, 0);
        ApplyDress(bodyParts,Gender, "Eyes", eye_color, eye_number);
        ApplyDress(bodyParts,Gender, "Nose", nose_color, nose_number);
        ApplyDress(bodyParts, Gender,"Lips", lips_color, lips_number);
        ApplyDress(bodyParts,Gender, "Ears", ear_color, ears_number);
        ApplyDress(bodyParts,Gender, "Hairs", hairs_color, hair_number);
        CharacterCustomizationAtStart.Instance.ChangeShoesOnClickSkinTone(shoes_color,shoes_number);
		Character_Dress_Changed = true;
        yield return null;
    }

    public void ApplyDress(BodyParts[] parts, string Gender,string Category, string Color, int index)
    {
        var AllCustomData = Resources.Load<CustomisationAllData>("CustomisationAllData");

        foreach (var _char in AllCustomData.MyChars)
        {          
            if (_char._category.Contains (Category) && _char._gender == Gender) 
            {   
//                if (string.IsNullOrEmpty(Color))
//                {
                    int _index = _char.AllColors.FindIndex(colorobj => colorobj.name == Color);
                    var PartName = _char.PartName;
                    var item = _char.AllColors[_index].AllItems[index];

                    foreach (var part in parts)
                    {
                        for (int i = 0; i < Mathf.Min(PartName.Count, item.images.Count); i++)
                        {
                            if (part.name == PartName[i])
                                part.GetComponent<SpriteRenderer>().sprite = item.images[i];
                        }
                    }
                return;
//                }                
            }
        }
    }

    public void ChangeShoesOfCharacter(BodyParts[] parts,string Gender,string SkinColor, string ShoesColor, int index)
    { 
        var AllCustomData = Resources.Load<CustomisationAllData>("CustomisationAllData");

        foreach (var _char in AllCustomData.CustomShoes)
        {          
            if (_char._gender == Gender) 
            {         
                int _index = _char.AllColors.FindIndex(colorobj => colorobj.name == ShoesColor);
                var PartName = _char.PartName;
                ItemBodyParts item = new ItemBodyParts();
                if(SkinColor == "Brown")
                    item = _char.AllColors[_index].BrownShoes[index];                
                else if(SkinColor == "White")
                    item = _char.AllColors[_index].WhiteShoes[index];
                else if(SkinColor =="Black")
                    item =_char.AllColors[_index].BlackShoes[index];                    

                foreach (var part in parts)
                {
                    for (int i = 0; i < Mathf.Min(PartName.Count, item.images.Count); i++)
                    {
                        if (part.name == PartName[i])
                            part.GetComponent<SpriteRenderer>().sprite = item.images[i];
                    }
                }
                return;
            }
        }
    }

    public IEnumerator ApplyCustomisationOfRealFlatmate(GameObject Char, int IdofPlayer)
    {
        CoroutineWithData cd = new CoroutineWithData (PlayerManager.Instance,PlayerManager.Instance.GetCharacterData (IdofPlayer));
        yield return cd.coroutine;

        if (cd.result != null) 
        {
            var _custChar = (CustomCharacter)cd.result;

            var bodyParts = Char.GetComponentsInChildren<BodyParts>(true);

            string skintone = "";

            switch (_custChar.skin_tone) 
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

            var eyes = _custChar.eyes.Split('_');
            var nose = _custChar.nose.Split('_');
            var lips = _custChar.lips.Split('_');
            var ears = _custChar.ears.Split('_');
            var hair = _custChar.hair.Split('_');
            var shoes = _custChar.shoes.Split('_');

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
            if (_custChar.gender == 0)
                gender = "Female";
            else
                gender = "Male";

            ApplyDress(bodyParts,gender, "SkinTone",skintone,0);
            ApplyDress(bodyParts,gender, "Eyes", eye_color, eye_number);
            ApplyDress(bodyParts,gender, "Nose", nose_color, nose_number);
            ApplyDress(bodyParts,gender, "Lips", lips_color, lips_number);
            ApplyDress(bodyParts,gender, "Ears", ear_color, ears_number);
            ApplyDress(bodyParts,gender, "Hairs", hairs_color, hair_number);
            ChangeShoesOfCharacter(bodyParts, gender,skintone, shoes_color,shoes_number);
        }
    }


	public IEnumerator GetFlatmateDataOfRealPlayer()
	{
		var flatemate = MainCharacter.AddComponent <Flatmate>();
		var target = GameManager.Instance.WayPoints [UnityEngine.Random.Range (0, GameManager.Instance.WayPoints.Count)];

		flatemate.transform.position = target.transform.position;

		flatemate.gameObject.AddComponent<RoomMateMovement> ().currentWaypoint = target.GetComponent<WayPoint> ();
		Sprite icon;
		if (GameManager.GetGender () == GenderEnum.Female)
			icon = IconFemale;
		else
			icon = IconMale;

		var rd = new RoommateGetData (PlayerPrefs.GetInt ("PlayerId"), 1);

        var RData = new RoommateData (PlayerPrefs.GetString ("UserName"), 1, (int)GameManager.GetGender (), icon, 0, 0, 0, true, true, MainCharacter, false);
		flatemate.data = RData;

		CoroutineWithData cd = new CoroutineWithData (this, DownloadContent.Instance.DownloadFlatmateData (rd, RData));
		yield return cd.coroutine;

		if (cd.result != null) {
			
            var _xyz = flatemate.GetComponentsInChildren<DressParts> (true);
			foreach (var dress in PurchaseDressManager.Instance.AllDresses) {
			foreach (var flatmate_dress in RData.Dress) {
                  if (dress.Id == flatmate_dress.Value) {
//					if (flatemate.GetComponent<CharacterProperties> ().PlayerType == "White")
						for (int i = 0; i < Mathf.Min (dress.PartName.Length, dress.DressesImages.Length); i++) {
							foreach (var _x in _xyz) {
								if (_x.name == dress.PartName [i]) {
									_x.GetComponent<SpriteRenderer> ().sprite = dress.DressesImages [i];
								}
							}
						}
					}
//                  if (dress.id == flatmate_dress.Value) {
//					if (flatemate.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//						for (int i = 0; i < Mathf.Min (dress.PartName.Length, dress.Brown_Images.Length); i++) {
//							foreach (var _x in _xyz) {
//								if (_x.name == dress.PartName [i]) {
//									_x.GetComponent<SpriteRenderer> ().sprite = dress.Brown_Images [i];
//								}
//							}
//						}
//					}
//
//                 if (dress.id == flatmate_dress.Value) {
//					if (flatemate.GetComponent<CharacterProperties> ().PlayerType == "Black")
//						for (int i = 0; i < Mathf.Min (dress.PartName.Length, dress.Black_Images.Length); i++) {
//							foreach (var _x in _xyz) {
//								if (_x.name == dress.PartName [i]) {
//									_x.GetComponent<SpriteRenderer> ().sprite = dress.Black_Images [i];
//								}
//							}
//						}
//					}
				}
			}

            int Id = 0;
            int.TryParse(RData.Hair_style, out Id);
            var Hair = PurchaseSaloonManager.Instance.FindSaloonWithId(Id);

            if(Hair != null){
//                if (flatemate.GetComponent<CharacterProperties> ().PlayerType == "White")
                DressManager.Instance.ChangeHairsOfCharacter(MainCharacter,Hair.PartName,Hair.HairImages);
//                else if (flatemate.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//                    DressManager.Instance.ChangeFlatMateDress(MainCharacter,Hair.PartName,Hair.Brown_Images); 
//                else if (flatemate.GetComponent<CharacterProperties> ().PlayerType == "Black")
//                    DressManager.Instance.ChangeFlatMateDress(MainCharacter,Hair.PartName,Hair.Black_Images);
            }

			flatemate.data = RData;
			flatemate.HireThisRoommate ();
		}else
		{
			flatemate.data = RData;
			flatemate.GivePerk ();
			flatemate.HireThisRoommate ();
		}	
	}
}