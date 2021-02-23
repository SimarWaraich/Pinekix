using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simple_JSON;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VotingPairManager : Singleton<VotingPairManager>
{
	public List <VotingPair> AllPairInEevnt = new List<VotingPair> ();

	public List<VotingPair> AllPlayerPair = new List<VotingPair> ();


	public List<CatWalkVotingPair> AllCatWalkPair = new List<CatWalkVotingPair> ();
	public List<CatWalkVotingPair> AllPlayerCatWalkPair = new List<CatWalkVotingPair> ();

	public List<VotingPairForDecor> AllPairOfDecorEvent = new List<VotingPairForDecor> ();
	public List<VotingPairForDecor> AllPlayerPairOfDecorEvent = new List<VotingPairForDecor> ();


	public List<VotingPair> FriendsInFashionEvent = new List<VotingPair> ();
	public List <CatWalkVotingPair> FriendsInCatwalkEvent = new List<CatWalkVotingPair> ();
	public List <VotingPairForDecor> FriendsInDecorEvent = new List<VotingPairForDecor> ();
	public List<CoOpVotingPair> FriendsInCoOp = new List<CoOpVotingPair> ();
	public List<SocietyVotingPair> FriendsInSociety = new List<SocietyVotingPair> ();

	public List<VotingBonus> allBonuses;

	SocietyVotingPair SelectedPair_SocietyEvent;
	VotingPair SelectedPair_Fashion;
	VotingPairForDecor SelectedPair_Decor;
	CatWalkVotingPair SelectedPair_CatWalk;
	CoOpVotingPair SelectedPair_CoOp;

	public bool PrizeClaimed = false;

	public GameObject MalePrefab;
	public GameObject FemalePrefab;

	public GameObject RoomPrefeb;

	public int pairIndex = 1;

	public Text TitleName;
	public Text ThemeText;

	public Text player1Name;
	public Text player2Name;
	public Text player1voteCount;
	public Text player2voteCount;

	public Text player1FriendBonus;
	public Text player2FriendBonus;

	public Text player1votingBonus;
	public Text player2votingBonus;

	public Text player1ScoreText;
	public Text player2ScoreText;

	public bool viewFriends = false;
	public bool viewStatus = false;

	public string SelectedEventType;


	public Button Player1Voting_Button;
	public Button Player2Voting_Button;

	public GameObject ApplybonusContainer;
	public GameObject ApplybonusButton;



	public Sprite _fashionShowBackGround;
	public Sprite _coOpBackGround;
	public Sprite _catWalkBackGround;
	public Sprite _decorBackGround;
	public Sprite _societyBackGround;


	void Awake ()
	{

		this.Reload ();
	}

	void Start ()
	{
		InitBonuses ();
//		GenrateVotingBonus (54, eventType.CoOp_Event);
//		PlayerPrefs.SetString ("VotingRefreshTime" + 54 + PlayerPrefs.GetInt ("PlayerId"), DateTime.UtcNow.AddMinutes (20).ToBinary ().ToString ());
////
//		PlayerPrefs.SetInt ("VotedCount" + 54 + PlayerPrefs.GetInt ("PlayerId"), 10);
	}

	#region GetPairDataofFashionEvent


	public void EnableAllButtons ()
	{
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
	}

	public void InitBonuses ()
	{
		var intBonuses = GameObject.FindObjectsOfType <VotingBonusTimer> ();

		foreach (var bonus in intBonuses) {
			Destroy (bonus.gameObject);
		}
		
		allBonuses = PersistanceManager.Instance.GetSavedVotingBonuses ();
		foreach (var bonus in allBonuses) {
			InstantiateBonuses (bonus);	
		}
	}

	//	void GetDataOfPair ()
	//	{
	//		if (AllPairInEevnt.Count > 0) {
	//			if (AllPairInEevnt.Count == 1)
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
	//			else
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
	//
	//			pairIndex = 1;
	//			VotingPair Pair = AllPairInEevnt [pairIndex];
	//			ShowOnePairOnScreen (Pair);
	//			ScreenAndPopupCall.Instance.CloseScreen ();
	//			ScreenAndPopupCall.Instance.VotingScreenSelection ();
	//		} else {
	//			ShowPopOfDescription ("Currently no pair have registered for this event", false);
	//			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
	//		}
	//	}

	//	void GetDataOfMyPair ()
	//	{
	//		if (AllPlayerPair.Count > 0) {
	//			if (AllPlayerPair.Count == 1)
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
	//			else
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
	//
	//			pairIndex = 1;
	//			VotingPair Pair = AllPlayerPair [pairIndex];
	//			ShowMyPairOnScreen (Pair);
	//
	//			ScreenAndPopupCall.Instance.CloseScreen ();
	//			ScreenAndPopupCall.Instance.VotingScreenSelection ();
	//		} else {
	//			ShowPopOfDescription ("You are not the participant of this event", false);
	//			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
	//		}
	//	}



	public void NextPairCheck ()
	{
		viewStatus = false;
	}

	public void NextPair ()
	{
		if (viewFriends) {
			if (SelectedEventType == "Fashion") {
				if (FriendsInFashionEvent.Count > 0) {		
					pairIndex = FriendsInFashionEvent.IndexOf (SelectedPair_Fashion);

					if (pairIndex >= FriendsInFashionEvent.Count - 1)
						ShowPopOfDescription ("No new pair to display.", () => {

							pairIndex = 1;
							VotingPair Pair = FriendsInFashionEvent [pairIndex - 1];
                            StartCoroutine(ShowOnePairOnScreen (Pair));
						});
					else {
						pairIndex++;
						VotingPair Pair = FriendsInFashionEvent [pairIndex];
                        StartCoroutine(ShowOnePairOnScreen (Pair));
					}

					if (FriendsInFashionEvent.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}
			} else if (SelectedEventType == "SocietyEvent") {
				pairIndex = FriendsInSociety.IndexOf (SelectedPair_SocietyEvent);


				if (FriendsInSociety.Count > 0) {
					if (pairIndex >= FriendsInSociety.Count - 1)
						ShowPopOfDescription ("No new pair to display.", () => {
							pairIndex = 1;
							SocietyVotingPair Pair = FriendsInSociety [pairIndex - 1];
                            StartCoroutine(ShowPairSocietyEvent (Pair));
						});
					else {
						pairIndex++;
						var Pair = FriendsInSociety [pairIndex];
                        StartCoroutine(ShowPairSocietyEvent (Pair));
					}

					if (FriendsInSociety.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}
			} else if (SelectedEventType == "CatWalk") {
				pairIndex = FriendsInCatwalkEvent.IndexOf (SelectedPair_CatWalk);

				if (FriendsInCatwalkEvent.Count > 0) {
					if (pairIndex >= FriendsInCatwalkEvent.Count - 1)
						ShowPopOfDescription ("No new pair to display.", () => {
							pairIndex = 1;
							CatWalkVotingPair Pair = FriendsInCatwalkEvent [pairIndex - 1];
                            StartCoroutine(ShowCatwalkPair (Pair));
						});
					else {
						pairIndex++;
						var Pair = FriendsInCatwalkEvent [pairIndex];
                        StartCoroutine(ShowCatwalkPair (Pair));
					}

					if (FriendsInCatwalkEvent.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}


			} else if (SelectedEventType == "CoOp") {
				pairIndex = FriendsInCoOp.IndexOf (SelectedPair_CoOp);
				
				if (FriendsInCoOp.Count > 0) {
					if (pairIndex >= FriendsInCoOp.Count - 1)
						ShowPopOfDescription ("No new pair to display.", () => {
							pairIndex = 1;
							CoOpVotingPair Pair = FriendsInCoOp [pairIndex - 1];
                            StartCoroutine(ShowPairCoOp (Pair));
						});
					else {
						pairIndex++;
						var Pair = FriendsInCoOp [pairIndex];
                        StartCoroutine(ShowPairCoOp (Pair));
					}

					if (FriendsInCoOp.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				}

			} else if (SelectedEventType == "Decor") {
				if (FriendsInDecorEvent.Count > 0) {
					if (pairIndex >= FriendsInDecorEvent.Count - 1)
						ShowPopOfDescription ("No new pair to display.", () => {
							pairIndex = 1;
							VotingPairForDecor Pair = FriendsInDecorEvent [pairIndex - 1];
							ShowMyPairDecorEvent (Pair);
							if (FriendsInDecorEvent.Count == 1)
								ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
							else
								ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
						});
					else {
						pairIndex++;
						var Pair = FriendsInDecorEvent [pairIndex];
						ShowMyPairDecorEvent (Pair);
					}

					if (FriendsInDecorEvent.Count == 1)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
				
				}
			}
		} else {
			if (SelectedEventType == "Fashion") {
				//				var tut = GameManager.Instance.GetComponent<Tutorial> ();
				//				if (!tut._FashionEventCompleate || tut.enabled)
				//					return;
				//				
				//				if (pairIndex == FriendsInFashionEvent.Count - 1)
				//					pairIndex = 1;
				//				else
				pairIndex++;
				GetVotingPair_Fashion (true);

			} else if (SelectedEventType == "CatWalk") {
			
				GetCatWalkPairs (true);
			} else if (SelectedEventType == "CoOp") {
				pairIndex++;
				GetAllVotingPair_coOp (true);
		

			} else if (SelectedEventType == "Decor") {
				ShowOnePairOnScreenOfDecor (true);


			} else if (SelectedEventType == "SocietyEvent") {
				StartCoroutine (IeGetPairSocietyEvent (true));
			}
		}
	}

	#endregion

	#region GetPairDataofDecorEvent

	void GetDataOfPairForDecor ()
	{
		if (AllPairOfDecorEvent.Count > 0) {
			pairIndex = 1;
			VotingPairForDecor Pair = AllPairOfDecorEvent [pairIndex];

			ScreenAndPopupCall.Instance.CloseScreen ();
			ScreenAndPopupCall.Instance.VotingScreenSelection ();
			StartCoroutine (ShowDecorPairOnScreenDelay (Pair));
		} else {
			ShowPopOfDescription ("No Pair Found");
		}

	}

	IEnumerator ShowDecorPairOnScreenDelay (VotingPairForDecor pair)
	{
		yield return new WaitForSeconds (0.5f);
		ShowOnePairOnScreenOfDecor (false);
	}

	void GetDataOfMYForDecor ()
	{
		ShowMyPairOnScreenOfDecor (false);
			
	}

	public void NextPairForDecor ()
	{
		if (viewStatus) {

			ShowMyPairOnScreenOfDecor (false);
			return;
		}


		if (!viewFriends) {

			//			var tut = GameManager.Instance.GetComponent<Tutorial> ();
			//			if (!tut._FashionEventCompleate || tut.enabled)
			//				return;
			//			if (AllPairOfDecorEvent.Count == 1)
			//				return;
			//			if (pairIndex == AllPairOfDecorEvent.Count - 1)
			//				pairIndex = 1;
			//			else
			//				pairIndex++;
			//			VotingPairForDecor Pair = AllPairOfDecorEvent [pairIndex];
			//			ShowOnePairOnScreenOfDecor (Pair);
		} else {
			//			var tut = GameManager.Instance.GetComponent<Tutorial> ();
			//			if (!tut._FashionEventCompleate || tut.enabled)
			//				return;
			//			if (FriendsInDecorEvent.Count == 1)
			//				return;
			//			if (pairIndex == FriendsInDecorEvent.Count - 1)
			//				pairIndex = 1;
			//			else
			//				pairIndex++;
			//			VotingPairForDecor Pair = FriendsInDecorEvent [pairIndex];
//			ShowOnePairOnScreenOfDecor ();
		}
	}




	#endregion

	#region ShowPairOfFashionEvent

    public IEnumerator ShowOnePairOnScreen (VotingPair pair)
	{
		if (SelectedEventType == "Fashion") {

			Player1Voting_Button.interactable = true;
			Player2Voting_Button.interactable = true;

			SelectedPair_Fashion = pair;
			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
			GetVotingResult_Fashion (PlayerPrefs.GetInt ("PlayerId"), pair.player1_flatmate_id, pair.pair_id);

			player1Name.text = pair.player1Name;
			player2Name.text = pair.player2Name;

			GameObject Char1 = null;
			GameObject Char2 = null;
			var previous = DressManager.Instance.dummyCharacter;

            var Oppo1 = FindFlatMateAvtar (pair.player1_flatmate_id);
            var Oppo2 = FindFlatMateAvtar (pair.player2_flatmate_id);

            if (Oppo1 != null) {
                Char1 = (GameObject)Instantiate (Oppo1, Vector3.zero, Quaternion.identity);
                Char1.GetComponent <CharacterProperties> ().PlayerType = Oppo1.GetComponent <CharacterProperties> ().PlayerType;
            } else {
                if (pair.Gender.Contains("Male"))
                {
                    Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
                }
                yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char1, pair.player1_id));
            }


            if (Oppo2 != null) {
                Char2 = (GameObject)Instantiate (Oppo2, Vector3.zero, Quaternion.identity);
                Char2.GetComponent <CharacterProperties> ().PlayerType = Oppo2.GetComponent <CharacterProperties> ().PlayerType;
            } else {
                if (pair.Gender.Contains("Male"))
                {            
                    Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);    
                }
                else
                {
                    Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
                }         
                yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char2, pair.player2_id));
            }

			Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
			Char1.transform.localScale = Vector3.one * 0.3f;
			Char1.transform.localPosition = Vector3.zero;

			Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
			Char2.transform.localScale = Vector3.one * 0.3f;
			Char2.transform.localPosition = Vector3.zero;

			foreach (var key in pair.player1_dressData.Keys) {	
                if (pair.player1_dressData [key].Contains ("Hair")) {
                    var hair =  FindSaloonWithId (key);
                    if (hair != null) {
                        DressManager.Instance.dummyCharacter = Char1;
                        DressManager.Instance.ChangeHairsForDummyCharacter (hair.PartName, hair.HairImages);
                    }
                }
                else {
					var dress =	FindDressWithId (key);
					if (dress != null) {
						DressManager.Instance.dummyCharacter = Char1;
//						if (pair.player1_SkinColor.Contains ("White"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
//						else if (pair.player1_SkinColor.Contains ("Brown"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						else if (pair.player1_SkinColor.Contains ("Black"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
//						else
							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
					}
				}
			}

			foreach (var key in pair.player2_dressData.Keys) {
                if (pair.player2_dressData [key].Contains ("Hair")) {
                    var hair =  FindSaloonWithId (key);
                    if (hair != null) {
                        DressManager.Instance.dummyCharacter = Char2;
                        DressManager.Instance.ChangeHairsForDummyCharacter (hair.PartName, hair.HairImages);
                    }
                }
                else {
					var dress =	FindDressWithId (key);
					if (dress != null) {
						DressManager.Instance.dummyCharacter = Char2;
//						if (pair.player2_SkinColor.Contains ("White"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
//						else if (pair.player2_SkinColor.Contains ("Brown"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						else if (pair.player2_SkinColor.Contains ("Black"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
//						else
							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
					}
				}  
			}

			//		foreach (var key in pair.player2_dressData.Keys) {
			//			var dress =	FindDressWithName (key.ToString (), pair.player2_dressData [key].ToString ());
			//			if (dress != null) {
			//				DressManager.Instance.dummyCharacter = Char2;
			//
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.White_Images);
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
			int Layer = LayerMask.NameToLayer ("UI3D");

			Char1.SetLayerRecursively (Layer);
			Char2.SetLayerRecursively (Layer);

			DressManager.Instance.dummyCharacter = previous;

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);

			ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (true);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (true);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "WHO'S GOT THE BETTER OUTFIT?";

			ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (true);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").GetComponentInChildren<Text> ().text = "VOTES LEFT \n" + (10 - GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id));
		}
	}

    public IEnumerator ShowMyPairOnScreen (VotingPair pair)
	{
		if (SelectedEventType == "Fashion") {


			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
			SelectedPair_Fashion = pair;
			GetVotingResult_Fashion (PlayerPrefs.GetInt ("PlayerId"), pair.player1_flatmate_id, pair.pair_id);

			player1Name.text = pair.player1Name;
			player2Name.text = pair.player2Name;

			GameObject Char1 = null;
			GameObject Char2 = null;
			var previous = DressManager.Instance.dummyCharacter;

            var Oppo1 = FindFlatMateAvtar (pair.player1_flatmate_id);
            var Oppo2 = FindFlatMateAvtar (pair.player2_flatmate_id);

            if (Oppo1 != null) {
                Char1 = (GameObject)Instantiate (Oppo1, Vector3.zero, Quaternion.identity);
                Char1.GetComponent <CharacterProperties> ().PlayerType = Oppo1.GetComponent <CharacterProperties> ().PlayerType;
            } else {
                if (pair.Gender.Contains("Male"))
                {
                    Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                }
                else
                {
                    Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
                }
                yield return  StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char1, pair.player1_id));
            }


            if (Oppo2 != null) {
                Char2 = (GameObject)Instantiate (Oppo2, Vector3.zero, Quaternion.identity);
                Char2.GetComponent <CharacterProperties> ().PlayerType = Oppo2.GetComponent <CharacterProperties> ().PlayerType;
            } else {
                if (pair.Gender.Contains("Male"))
                {            
                    Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);    
                }
                else
                {
                    Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
                }         
                yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char2, pair.player2_id));
            }

			Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
			Char1.transform.localScale = Vector3.one * 0.3f;
			Char1.transform.localPosition = Vector3.zero;

			Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
			Char2.transform.localScale = Vector3.one * 0.3f;
			Char2.transform.localPosition = Vector3.zero;

			foreach (var key in pair.player1_dressData.Keys) {	
                if (pair.player1_dressData[key].Contains("Hair"))
                {
                    var hair = FindSaloonWithId(key);
                    if (hair != null)
                    {
                        DressManager.Instance.dummyCharacter = Char1;
                        DressManager.Instance.ChangeHairsForDummyCharacter(hair.PartName, hair.HairImages);
                    }
                }
                else
                {
                    var dress =	FindDressWithId(key);
                    if (dress != null)
                    {
                        DressManager.Instance.dummyCharacter = Char1;
//						if (pair.player1_SkinColor.Contains ("White"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
//						else if (pair.player1_SkinColor.Contains ("Brown"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						else if (pair.player1_SkinColor.Contains ("Black"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
//						else
                        DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
                    }
                }				
			}

			foreach (var key in pair.player2_dressData.Keys) {

                if (pair.player2_dressData[key].Contains("Hair"))
                {
                    var hair = FindSaloonWithId(key);
                    if (hair != null)
                    {
                        DressManager.Instance.dummyCharacter = Char2;
                        DressManager.Instance.ChangeHairsForDummyCharacter(hair.PartName, hair.HairImages);
                    }
                }else{
					var dress =	FindDressWithId (key);
					if (dress != null) {
						DressManager.Instance.dummyCharacter = Char2;
//						if (pair.player2_SkinColor.Contains ("White"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
//						else if (pair.player2_SkinColor.Contains ("Brown"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						else if (pair.player2_SkinColor.Contains ("Black"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
//						else
							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
					}
				}  
			}

			//		foreach (var key in pair.player2_dressData.Keys) {
			//			var dress =	FindDressWithName (key.ToString (), pair.player2_dressData [key].ToString ());
			//			if (dress != null) {
			//				DressManager.Instance.dummyCharacter = Char2;
			//
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.White_Images);
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
			int Layer = LayerMask.NameToLayer ("UI3D");

			Char1.SetLayerRecursively (Layer);
			Char2.SetLayerRecursively (Layer);
			//			Char1.transform.localPosition = new Vector3 (-0.31f, -0.62f, 0);
			//			Char2.transform.localPosition = new Vector3 (0.34f, -0.59f, 0);


			DressManager.Instance.dummyCharacter = previous;

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;


			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);

			ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);

			ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (false);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (false);
			//false);

			if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
				ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
			else
				ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);

			ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "YOUR STATUS";
			ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (false);
            Char2.transform.localPosition = new Vector3(0,0,0);
            Char1.transform.localPosition = new Vector3(0,0,0);
		}
	}

	#endregion

	#region ShowPairOfDecorEvent


	public void Create3dUIForSubmission ()
	{
		
		EventSystem _system = GameObject.Find ("EventSystem").GetComponent<EventSystem> ();
		DecorData decor = _system.currentSelectedGameObject.GetComponent<DecorView> ().Item;
		//		var pos = GameObject.Find ("RoomForDecorEvent").transform.position;
		var dir = 0;
		if (!decor._isBusy && !decor._isCoolingDown)
			Create3DDecoreForUi (decor, Vector3.zero, dir, GameObject.Find ("RoomForDecorEvent"));
		else {
			if (decor._isBusy)
				ShowPopOfDescription ("You can't use this decor as it is being used in another event.");
			else if (decor._isCoolingDown)
				ShowPopOfDescription ("You can't use this decor as it was used in another event and is now cooling down.");
		}
		// To be Confirmed taht this fires after Start of Decor3D 
	}


	public void Create3DDecoreForUi (DecorData decor, Vector3 Pos, int dir, GameObject Parent)
	{
		if (decor == null)
			return;

		var asset = Resources.Load<Decor3DView> ("Decors/" + decor.Name.Trim ('"'));
		if (asset == null) {
			//			List<string> downloadednames = new List<string> ();
			for (int i = 0; i < DecorController.Instance.DownloadedDecors.Count; i++) {
				if (DecorController.Instance.DownloadedDecors [i].GetComponent <Decor3DView> ().decorInfo.Id == decor.Id || DecorController.Instance.DownloadedDecors [i].GetComponent <Decor3DView> ().name == DecorController.FirstCharToUpper (decor.Name.Trim ('"')))
					asset = DecorController.Instance.DownloadedDecors [i].GetComponent<Decor3DView> ();

				if (EventManagment.Instance.EventType == eType.Event) {
					ScreenAndPopupCall.Instance.SetCameraActiveForRoom ();
					ScreenAndPopupCall.Instance.DecorEventRoomScreenSelection ();
				}
			}
		}
		var _Layer = LayerMask.NameToLayer ("UI3D");
		GameObject Go = (GameObject)Instantiate (asset.gameObject, Pos, Quaternion.identity);	
		//		var 
		if (EventManagment.Instance.EventType == eType.Event) {
			Go.transform.parent = Parent.transform;
		} else {
			Go.transform.parent = Parent.transform.GetChild (0).transform;
		}
		Go.transform.localPosition = Pos;
		Destroy (Go.GetComponent<DragSnap> ());
	
		if (EventManagment.Instance.EventType == eType.Event) {
			var drs = Go.AddComponent<DragSnap> ();
			drs.grid = Parent.transform.GetChild (5).gameObject;
		} else {
			var drs = Go.AddComponent<DragSnap> ();
			drs.grid = Parent.transform.GetChild (0).transform.GetChild (5).gameObject;
		}
		Go.SetLayerRecursively (_Layer);
		Go.SetMaterialRecursively ();
		Go.GetComponent<Decor3DView> ().direction = dir; // To be Confirmed taht this fires after Start of Decor3D 
		Go.GetComponent<Decor3DView> ().CreateDecore (decor);
		Go.GetComponent<Decor3DView> ().Start ();
		if (EventManagment.Instance.EventType == eType.Voting || ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.ResultScreen) {			
			Destroy (Go.GetComponent<Decor3DView> ());
			Go.transform.GetChild (0).gameObject.SetActive (false);
		}	
		Go.transform.GetChild (1).localPosition = Vector3.zero;
	}


	public void ShowOnePairOnScreenOfDecor (bool isForNextPair)
	{
		SelectedEventType = "Decor";
		pairIndex++;
		StartCoroutine (IeShowOnePairOnScreenOfDecor (pairIndex, isForNextPair));
	}




	IEnumerator IeShowOnePairOnScreenOfDecor (int count, bool isForNextPair)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var Flat = ScreenAndPopupCall.Instance.RoomCamera.transform.GetComponentInChildren<Flat3D> ();
		var json = new JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		json ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		json ["page_no"] = count.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/decor_getvotingpair", encoding.GetBytes (json.ToString ()), postHeader);
	
		yield return www;
		if (www.error == null) {
			
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				
				var dataArray = _jsnode ["data"];
				VotingPairForDecor Vp = new VotingPairForDecor ();
				/// this data fatching for player 2
				Vp.pair_id = int.Parse (dataArray [0] ["pair_id"]);
				Vp.event_id = int.Parse (dataArray [0] ["event_id"]);

				Vp.player1_id = int.Parse (dataArray [0] ["player1_id"]);
				Vp.player1Name = dataArray [0] ["player1_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player1_GroundTexture_Name = dataArray [0] ["player1_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player1_WallTexture_Name = dataArray [0] ["player1_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());

				List <int> ObjIds = new List<int> ();
				var DecoreObjectIds = dataArray [0] ["player1_object_id"];
				for (int x = 0; x < DecoreObjectIds.Count; x++) {
					ObjIds.Add (int.Parse (DecoreObjectIds [x] [0].ToString ().Trim ('"')));
				}

				List <string> TransfomArray = new List<string> ();
				var ObjPosition = dataArray [0] ["player1_object_position"];
				for (int x = 0; x < ObjPosition.Count; x++) {
					TransfomArray.Add (ObjPosition [x] [0].ToString ().Trim ('"'));
				}

				for (int x = 0; x < Mathf.Min (TransfomArray.Count, ObjIds.Count); x++) {
					DecorObject Decor = new DecorObject ();
					Decor.Id = ObjIds [x];
					string[] Values = TransfomArray [x].Split ('/');
					if (Values.Length == 3) {
						Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (Values [0].Trim ('|'));
						Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
						//							Decor.Rotation = ExtensionMethods.DeserializeVector3ArrayExtented (Values [2]);
						Decor.Direction = int.Parse (Values [2]);
					} else {
						Debug.LogError (" Transform of player 1 not in correct format");
					}

					Vp.Player1AllDecorOnFlat.Add (Decor);
				}


				/// this data fatching for player 2
				Vp.player2_id = int.Parse (dataArray [0] ["player2_id"]);
				Vp.player2Name = dataArray [0] ["player2_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player2_WallTexture_Name = dataArray [0] ["player2_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player2_GroundTexture_Name = dataArray [0] ["player2_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());

				List <int> Player2ObjIds = new List<int> ();
				var Player2DecoreObjectIds = dataArray [0] ["player2_object_id"];
				for (int x = 0; x < Player2DecoreObjectIds.Count; x++) {
					Player2ObjIds.Add (int.Parse (Player2DecoreObjectIds [x] [0].ToString ().Trim ('"')));
				}


				List <string> Player2TransfomArray = new List<string> ();
				var Player2ObjPosition = dataArray [0] ["player2_object_position"];
				for (int x = 0; x < Player2ObjPosition.Count; x++) {
					Player2TransfomArray.Add (Player2ObjPosition [x] [0].ToString ().Trim ('"'));
				}

				for (int x = 0; x < Mathf.Min (Player2TransfomArray.Count, Player2ObjIds.Count); x++) {
					DecorObject Player2Decor = new DecorObject ();
					Player2Decor.Id = Player2ObjIds [x];
					string[] Values = Player2TransfomArray [x].Split ('/');
					if (Values.Length == 3) {
						Player2Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (Values [0].Trim ('|'));
						Player2Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
						//							Player2Decor.Rotation = ExtensionMethods.DeserializeVector3ArrayExtented (Values [2]);
						Player2Decor.Direction = int.Parse (Values [2]);
					} else {
						Debug.LogError (" Transform of player 2 not in correct format");
					}

					Vp.Player2AllDecorOnFlat.Add (Player2Decor);
				}


				SelectedPair_Decor = Vp;
				//2nd player
				if (ScreenManager.Instance.ScreenMoved == null || ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.VotingScreen) {
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();

					TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
					ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;
//					ScreenManager.Instance.VotingScreen.transform.GetChild (2).GetChild (1).gameObject.SetActive (true);



					ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
					ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (true);
					ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (true);
					ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);

					ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (true);
					ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").GetComponentInChildren<Text> ().text = "VOTES LEFT \n" + (10 - GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id));

					ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "WHO'S GOT THE BETTER FLAT?";
				}		
							
				player1Name.text = Vp.player1Name;
				player2Name.text = Vp.player2Name;

				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();				
				StartCoroutine (GetPairDecor (Vp));

			} else {
				if (isForNextPair) {
					pairIndex = 0;
					ShowPopOfDescription ("No new pair to display", () => NextPair ());
				} else
					ShowPopOfDescription ("Currently no pair have registered for this Event");	
			}
		} else {

		}
	}

	IEnumerator GetPairDecor (VotingPairForDecor Vp)
	{
		yield return new WaitForSeconds (0.0f);
		GameObject Player1Room = null;
		///TODO:
		//			var previous = RoomPurchaseManager.Instance.RoomTypePrefeb [0];

		Player1Room = (GameObject)Instantiate (VotingPairManager.Instance.RoomPrefeb, Vector3.zero, Quaternion.identity);


		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		Player1Room.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 16;
		Player1Room.transform.localScale = Vector3.one;
		Player1Room.transform.localPosition = new Vector3 (-1, 0, 0);
		Player1Room.name = "Player1Room";

		int Layer = LayerMask.NameToLayer ("UI3D");
		Player1Room.SetLayerRecursively (Layer);

        if (!string.IsNullOrEmpty (Vp.player1_WallTexture_Name))
            Player1Room.GetComponent<Flat3D> ().Walls.ChangeWallColorsNew (FindWallTextures (Vp.player1_WallTexture_Name));

		if (!string.IsNullOrEmpty (Vp.player1_GroundTexture_Name))
			Player1Room.GetComponent<Flat3D> ().ChangeGroungColor (FindGroundTexture (Vp.player1_GroundTexture_Name));

		foreach (var decor in Vp.Player1AllDecorOnFlat) {
			var _dec = VotingPairManager.Instance.FindDecore (decor.Id);
			var parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0).gameObject;
			Create3DDecoreForUi (_dec, decor.Position, decor.Direction, parent);

		}
		// 1st player

		GameObject Player2Room = null;
		///TODO:
		//			var previous = RoomPurchaseManager.Instance.RoomTypePrefeb [0];

		Player2Room = (GameObject)Instantiate (VotingPairManager.Instance.RoomPrefeb, Vector3.zero, Quaternion.identity);


		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
		Player2Room.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 16;
		Player2Room.transform.localScale = Vector3.one;
		Player2Room.transform.localPosition = new Vector3 (2.5f, 0, 0);
		Player2Room.name = "Player2Room";
		//

		Player2Room.SetLayerRecursively (Layer);

        if (!string.IsNullOrEmpty (Vp.player2_WallTexture_Name))
            Player2Room.GetComponent<Flat3D> ().Walls.ChangeWallColorsNew (FindWallTextures (Vp.player2_WallTexture_Name));
        
		if (!string.IsNullOrEmpty (Vp.player2_GroundTexture_Name))
			Player2Room.GetComponent<Flat3D> ().ChangeGroungColor (FindGroundTexture (Vp.player2_GroundTexture_Name));

		foreach (var decor in Vp.Player2AllDecorOnFlat) {
			var _dec = VotingPairManager.Instance.FindDecore (decor.Id);
			var parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0).gameObject;
			Create3DDecoreForUi (_dec, decor.Position, decor.Direction, parent);
		}
		GetVotingResult_Decor (PlayerPrefs.GetInt ("PlayerId"), Vp.event_id, Vp.pair_id);



	}



	public void ShowMyPairOnScreenOfDecor (bool isReward)
	{
		StartCoroutine (IeShowMyPairOnScreenOfDecor (1, isReward));
	}




	public IEnumerator IeShowMyPairOnScreenOfDecor (int count, bool isReward)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var Flat = ScreenAndPopupCall.Instance.RoomCamera.transform.GetComponentInChildren<Flat3D> ();
		var json = new JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		json ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		json ["count"] = count.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/decor_getSinglePair", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {

				var dataArray = _jsnode ["data"];
				VotingPairForDecor Vp = new VotingPairForDecor ();
				/// this data fatching for player 2
				Vp.pair_id = int.Parse (dataArray ["pair_id"]);
				Vp.event_id = int.Parse (dataArray ["event_id"]);

				Vp.player1_id = int.Parse (dataArray ["player1_id"]);
				Vp.player1Name = dataArray ["player1_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player1_GroundTexture_Name = dataArray ["player1_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player1_WallTexture_Name = dataArray ["player1_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());

				List <int> ObjIds = new List<int> ();
				var DecoreObjectIds = dataArray ["player1_decor_object_data"];
				for (int x = 0; x < DecoreObjectIds.Count; x++) {
					ObjIds.Add (int.Parse (DecoreObjectIds [x] ["id"].ToString ().Trim ('"')));
				}

				List <string> TransfomArray = new List<string> ();
				var ObjPosition = dataArray ["player1_decor_object_position"];
				for (int x = 0; x < ObjPosition.Count; x++) {
					TransfomArray.Add (ObjPosition [x].ToString ().Trim ('"'));
				}

				for (int x = 0; x < Mathf.Min (TransfomArray.Count, ObjIds.Count); x++) {
					DecorObject Decor = new DecorObject ();
					Decor.Id = ObjIds [x];
					string[] Values = TransfomArray [x].Split ('/');

					if (Values.Length == 3) {
						var rawpos = Values [0].Replace ('\\', ',');
						var pos = rawpos.Split ('"');
						var rawdir = Values [2].Split ('"');
						Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (pos [pos.Length - 1].Trim ('|'));
						Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
						//							Decor.Rotation = ExtensionMethods.DeserializeVector3ArrayExtented (Values [2]);
						Decor.Direction = int.Parse (rawdir [0].Trim ('"'));
					} else {
						Debug.LogError (" Transform of player 1 not in correct format");
					}

					Vp.Player1AllDecorOnFlat.Add (Decor);
				}


				/// this data fatching for player 2
				Vp.player2_id = int.Parse (dataArray ["player2_id"]);
				Vp.player2Name = dataArray ["player2_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player2_WallTexture_Name = dataArray ["player2_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());
				Vp.player2_GroundTexture_Name = dataArray ["player2_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());

				List <int> Player2ObjIds = new List<int> ();
				var Player2DecoreObjectIds = dataArray ["player2_decor_object_data"];
				for (int y = 0; y < Player2DecoreObjectIds.Count; y++) {
					Player2ObjIds.Add (int.Parse (Player2DecoreObjectIds [y] ["id"].ToString ().Trim ('"')));
				}


				List <string> Player2TransfomArray = new List<string> ();
				var Player2ObjPosition = dataArray ["player2_decor_object_position"];
				for (int x = 0; x < Player2ObjPosition.Count; x++) {
					Player2TransfomArray.Add (Player2ObjPosition [x].ToString ().Trim ('"'));
				}

				for (int x = 0; x < Mathf.Min (Player2TransfomArray.Count, Player2ObjIds.Count); x++) {
					DecorObject Player2Decor = new DecorObject ();
					Player2Decor.Id = Player2ObjIds [x];
					string[] Values = Player2TransfomArray [x].Split ('/');
					if (Values.Length == 3) {

						var rawpos = Values [0].Replace ('\\', ',');
						var pos = rawpos.Split ('"');
						var rawdir = Values [2].Split ('"');

						Player2Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (pos [pos.Length - 1].Trim ('|'));
						Player2Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
						Player2Decor.Direction = int.Parse (rawdir [0].Trim ('"'));
					} else {
						Debug.LogError (" Transform of player 2 not in correct format");
					}

					Vp.Player2AllDecorOnFlat.Add (Player2Decor);
				}


				SelectedPair_Decor = Vp;
				//2nd player
//				ScreenAndPopupCall.Instance.CloseScreen ();
				if (isReward) {
					ScreenAndPopupCall.Instance.RewardScreenSelection ();
					ScreenAndPopupCall.Instance.ResultPanelClose ();
				} else {
					ScreenAndPopupCall.Instance.CloseScreen ();	

					ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (false);
					ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (false);
					ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
					TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
					ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

//					ScreenManager.Instance.VotingScreen.transform.GetChild (2).GetChild (1).gameObject.SetActive (false);//
					ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "YOUR STATUS";//

					if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);		


					ScreenAndPopupCall.Instance.VotingScreenSelection ();
				}
				ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (false);
				ShowMyShinglePair (Vp);

				yield return Vp;

			} else {
				if (viewStatus)
					ShowPopOfDescription ("No pair found");
				yield return null;
			}
		} else {
			yield return null;
		}
	}

	public void ShowVotingPair (VotingPairForDecor Vp)
	{
		EventManagment.Instance.EventType = eType.Voting;

		player1Name.text = Vp.player1Name;
		player2Name.text = Vp.player2Name;

		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();	

		SelectedPair_Decor = Vp;
		StartCoroutine (GetPairDecor (Vp));

		ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "CHOOSE YOUR FAVORITE FLAT?";
//		ScreenManager.Instance.VotingScreen.transform.GetChild (2).GetChild (1).GetChild (0).GetComponent <Text> ().text = "FAVORITE FLAT";

		ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);

		ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (true);

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").GetComponentInChildren<Text> ().text = "VOTES LEFT \n" + (10 - GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id));
//		ScreenManager.Instance.VotingScreen.transform.GetChild (2).GetChild (1).gameObject.SetActive (true);
	}

	/// <summary>
	///  Show Single Pair from view status
	/// </summary>
	/// <param name="Vp">Vp.</param>
	public void ShowMyShinglePair (VotingPairForDecor Vp)
	{
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();

		EventManagment.Instance.EventType = eType.Voting;

		player1Name.text = Vp.player1Name;
		player2Name.text = Vp.player2Name;
		if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
		else
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
		
		SelectedPair_Decor = Vp;
		StartCoroutine (GetPairDecor (Vp));
	}

	public void ShowMyPairDecorEvent (VotingPairForDecor Vp)
	{
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();

		EventManagment.Instance.EventType = eType.Voting;

		player1Name.text = Vp.player1Name;
		player2Name.text = Vp.player2Name;


		SelectedPair_Decor = Vp;
		StartCoroutine (GetPairDecor (Vp));

		ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "CHOOSE YOUR FAVORITE FLAT?";

//		if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
//			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
//		else
//			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
		
		ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (true);

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").GetComponentInChildren<Text> ().text = "VOTES LEFT \n" + (10 - GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id));
//		ScreenManager.Instance.VotingScreen.transform.GetChild (2).GetChild (1).gameObject.SetActive (true);
	}


	//	void ApplyColorToRoom(GameObject room, )

	public DecorData FindDecore (int Id)
	{
		foreach (var Item in DecorController.Instance.AllDecores) {
			if (Item.Id == Id)
				return Item;
		}
		return null;
	}

    WallsColor FindWallTextures (string Name)
	{
		foreach (var Item in ReModelShopController.Instance.WallsColors) {
			if (Item.Name == Name) {
				return Item;
			}
		}

		//TODO change to null
        return null;

	}

	public Sprite FindGroundTexture (string Name)
	{
		foreach (var Item in ReModelShopController.Instance.Grounds) {
			if (Item.Name == Name)
				return Item.Texture;
		}


		//TODO Change to null
        return null;
	}

	#endregion




	public void GetCatWalkPairs (bool IsForNextPair)
	{
		SelectedEventType = "CatWalk";
		pairIndex++;
		StartCoroutine (IeGetCatWalkPairs (pairIndex, IsForNextPair));
	}


	IEnumerator IeGetCatWalkPairs (int count, bool isForNextPair)
	{

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["page_no"] = count.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		//		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/getvotingpair", encoding.GetBytes (jsonElement.ToString ()), postHeader);
		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/catwalkGetVotingPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode data is  ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");


//				JSONNode dataArray = _jsnode ["data"];
//
//				CatWalkVotingPair vp = new CatWalkVotingPair ();

				CatWalkVotingPair vp = new CatWalkVotingPair ();
				for (int i = 0; i < _jsnode ["data"].Count; i++) {
					JSONNode dataArray = _jsnode ["data"] [i];

					int.TryParse (dataArray ["event_id"], out vp.event_id);
					int.TryParse (dataArray ["pair_id"], out vp.pair_id);

					int.TryParse (dataArray ["player1_id"], out vp.player1_id);
					int.TryParse (dataArray ["player2_id"], out vp.player2_id);
					vp.player1Name = dataArray ["player1_name"].ToString ().Trim ('"');
					vp.player2Name = dataArray ["player2_name"].ToString ().Trim ('"');

					var player1Items = dataArray ["player1_item_data"];

                    var player1Flatmate1data = new CatwalkFlatmateData();
                    var player1Flatmate2data = new CatwalkFlatmateData();
                    var player1Flatmate3data = new CatwalkFlatmateData();

					for (int j = 0; j < player1Items.Count; j++) {
						int Id = 0;
						int.TryParse (player1Items [j] ["item_id"], out Id);

//						var jhgdkhgfd = player1Items [j] ["item_type"].ToString ();
						if (player1Items [j] ["item_type"].ToString ().Contains ("Flatmates")) {
                            var FlatmateIdenifier =int.Parse(player1Items [j] ["item_type"].Value.ToString ().Split('_')[1]);

                            if(FlatmateIdenifier == 0)
                            {
                                player1Flatmate1data.FlatmateId = Id;

                            }else if(FlatmateIdenifier ==1)
                            {
                                player1Flatmate2data.FlatmateId = Id;
                            }else if(FlatmateIdenifier == 2)
                            {
                                player1Flatmate3data.FlatmateId = Id;
                            }

//							vp.Player1_flatmate_id.Add (Id);
						}
						if (player1Items [j] ["item_type"].ToString ().Contains ("Hair")) {
                            var FlatmateIdenifier =int.Parse(player1Items [j] ["item_type"].Value.ToString ().Split('_')[1]);

                            if(FlatmateIdenifier == 0)
                            {
                                player1Flatmate1data.HairsId = Id;

                            }else if(FlatmateIdenifier ==1)
                            {
                                player1Flatmate2data.HairsId = Id;
                            }else if(FlatmateIdenifier == 2)
                            {
                                player1Flatmate3data.HairsId = Id;
                            }
//							vp.Player1_hair_id.Add (Id);
						}
						if (player1Items [j] ["item_type"].ToString ().Contains ("Clothes")) 
                        {
                            var FlatmateIdenifier =int.Parse(player1Items [j] ["item_type"].Value.ToString ().Split('_')[1]);

                            if(FlatmateIdenifier == 0)
                            {
                                player1Flatmate1data.Dress_ids.Add(Id);

                            }else if(FlatmateIdenifier ==1)
                            {
                                player1Flatmate2data.Dress_ids.Add(Id);
                            }else if(FlatmateIdenifier == 2)
                            {
                                player1Flatmate3data.Dress_ids.Add(Id);
                            }
//							vp.Player1_dress_id.Add (Id);
						}
//						if (player1Items [j] ["item_type"].ToString ().Contains ("Shoes")) {
//							vp.Player1_shoes_id.Add (Id);
//						}
						if (player1Items [j] ["item_type"].ToString ().Contains ("Gender")) {

							if (Id == 1) {
								vp.player1_Gender = "Male";
							} else {
								vp.player1_Gender = "Female";
							}
						}
					}

                    vp.Player1_flatmate1_data = player1Flatmate1data;
                    vp.Player1_flatmate2_data = player1Flatmate2data;
                    vp.Player1_flatmate3_data = player1Flatmate3data;

					var player2Items = dataArray ["player2_item_data"];
                    var player2Flatmate1data = new CatwalkFlatmateData();
                    var player2Flatmate2data = new CatwalkFlatmateData();
                    var player2Flatmate3data = new CatwalkFlatmateData();

                    for (int j = 0; j < player2Items.Count; j++)
                    {
                        int Id = 0;
                        int.TryParse(player2Items[j]["item_id"], out Id);


                        if (player2Items[j]["item_type"].ToString().Contains("Flatmates"))
                        {
                            var FlatmateIdenifier = int.Parse(player2Items[j]["item_type"].Value.ToString().Split('_')[1]);
                            
                            if (FlatmateIdenifier == 0)
                            {
                                player2Flatmate1data.FlatmateId = Id;

                            }
                            else if (FlatmateIdenifier == 1)
                            {
                                player2Flatmate2data.FlatmateId = Id;
                            }
                            else if (FlatmateIdenifier == 2)
                            {
                                player2Flatmate3data.FlatmateId = Id;
                            }
//							vp.Player2_flatmate_id.Add (Id);
                        }
                        if (player2Items[j]["item_type"].ToString().Contains("Hair"))
                        {            
                            var FlatmateIdenifier = int.Parse(player2Items[j]["item_type"].Value.ToString().Split('_')[1]);
                            
                            if (FlatmateIdenifier == 0)
                            {
                                player2Flatmate1data.HairsId = Id;

                            }
                            else if (FlatmateIdenifier == 1)
                            {
                                player2Flatmate2data.HairsId = Id;
                            }
                            else if (FlatmateIdenifier == 2)
                            {
                                player2Flatmate3data.HairsId = Id;
                            }
//							vp.Player2_hair_id.Add (Id);
                        }						
                        if (player2Items[j]["item_type"].ToString().Contains("Clothes"))
                        {
                            var FlatmateIdenifier = int.Parse(player2Items[j]["item_type"].Value.ToString().Split('_')[1]);

                            if (FlatmateIdenifier == 0)
                            {
                                player2Flatmate1data.Dress_ids.Add(Id);

                            }
                            else if (FlatmateIdenifier == 1)
                            {
                                player2Flatmate2data.Dress_ids.Add(Id);
                            }
                            else if (FlatmateIdenifier == 2)
                            {
                                player2Flatmate3data.Dress_ids.Add(Id);
                            }
//							vp.Player2_dress_id.Add (Id);
                        }
//						if (player2Items [j] ["item_type"].ToString ().Contains ("Shoes")) {
//							vp.Player2_shoes_id.Add (Id);
//						}
                        if (player2Items[j]["item_type"].ToString().Contains("Gender"))
                        {
                            if (Id == 1)
                            {
                                vp.player2_Gender = "Male";
                            }
                            else
                            {
                                vp.player2_Gender = "Female";
                            }
                        }
                    }

                    vp.Player2_flatmate1_data = player2Flatmate1data;
                    vp.Player2_flatmate2_data = player2Flatmate2data;
                    vp.Player2_flatmate3_data = player2Flatmate3data;					
				}
			
                StartCoroutine(ShowCatwalkPair (vp));
				if (!isForNextPair) {			
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();
				}

				yield return vp;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				if (isForNextPair) {
					pairIndex = 0;
					ShowPopOfDescription ("No pairs to display", () => NextPair ());
				} else
					ShowPopOfDescription ("Currently no pair have registered for this Event");	
				yield return null;

			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return null;
			}
		} else {			
			yield return null;
		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
	}

    public IEnumerator ShowCatwalkPair (CatWalkVotingPair pair)
    {

        Player1Voting_Button.interactable = true;
        Player2Voting_Button.interactable = true;

        SelectedPair_CatWalk = pair;

        ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents();
        GetVotingResult_CatWalk(pair.player1_id, pair.event_id, pair.pair_id);

        player1Name.text = pair.player1Name;
        player2Name.text = pair.player2Name;

        GameObject Player1_Char1 = null;
        GameObject Player1_Char2 = null;
        GameObject Player1_Char3 = null;
        GameObject Player2_Char1 = null;
        GameObject Player2_Char2 = null;
        GameObject Player2_Char3 = null;

        var previous = DressManager.Instance.dummyCharacter;

        {
            var P1F1 = FindFlatMateAvtar(pair.Player1_flatmate1_data.FlatmateId);
            if (P1F1 != null)
            {
                Player1_Char1 = (GameObject)Instantiate(P1F1, Vector3.zero, Quaternion.identity);                
            }
            else
            {
                if (pair.player1_Gender == "Male")
                    Player1_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                else
                    Player1_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
        
                yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char1, pair.player1_id));
            }
            DressManager.Instance.dummyCharacter = Player1_Char1;

            foreach (var id in pair.Player1_flatmate1_data.Dress_ids)
            {
                DressItem dress = FindDressWithId(id);
                if (dress != null)
                    DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
            }
            SaloonItem saloon = FindSaloonWithId(pair.Player1_flatmate1_data.HairsId);
            if (saloon != null)
            {     
                DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
            }
        }

    
        {
         var P1F2 = FindFlatMateAvtar(pair.Player1_flatmate2_data.FlatmateId);
        if(P1F2 != null){
            Player1_Char2 = (GameObject)Instantiate (P1F2, Vector3.zero, Quaternion.identity);                
        }else
        {
            if (pair.player1_Gender == "Male") 
                Player1_Char2 = (GameObject)Instantiate (MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player1_Char2 = (GameObject)Instantiate (FemalePrefab, Vector3.zero, Quaternion.identity);

            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char2, pair.player1_id)); 
       
        }
        DressManager.Instance.dummyCharacter = Player1_Char2;

        foreach(var id in pair.Player1_flatmate2_data.Dress_ids)
        {
            DressItem dress = FindDressWithId(id);
            if(dress != null)
                DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
        }
        SaloonItem saloon = FindSaloonWithId(pair.Player1_flatmate2_data.HairsId);
        if(saloon != null)
        {     
            DressManager.Instance.ChangeDressForDummyCharacter (saloon.PartName, saloon.HairImages);                
        }
        }

        {
         var P1F3 = FindFlatMateAvtar(pair.Player1_flatmate3_data.FlatmateId);
        if(P1F3 != null){
            Player1_Char3 = (GameObject)Instantiate (P1F3, Vector3.zero, Quaternion.identity);                
        }else
        {
            if (pair.player1_Gender == "Male") 
                Player1_Char3 = (GameObject)Instantiate (MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player1_Char3 = (GameObject)Instantiate (FemalePrefab, Vector3.zero, Quaternion.identity);

            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char3, pair.player1_id)); 
            DressManager.Instance.dummyCharacter = Player1_Char3;       
        } 
        foreach(var id in pair.Player1_flatmate3_data.Dress_ids)
        {
            DressItem dress = FindDressWithId(id);
            if(dress != null)
                DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
        }
        SaloonItem saloon = FindSaloonWithId(pair.Player1_flatmate3_data.HairsId);
        if(saloon != null)
        {     
            DressManager.Instance.ChangeDressForDummyCharacter (saloon.PartName, saloon.HairImages);                
        }
        }

        {
            var P2F1 = FindFlatMateAvtar(pair.Player2_flatmate1_data.FlatmateId);
        if (P2F1 != null)
        {
            Player2_Char1 = (GameObject)Instantiate(P2F1, Vector3.zero, Quaternion.identity);                
        }
        else
        {
            if (pair.player2_Gender == "Male")
                Player2_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player2_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);

            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char1, pair.player2_id));        
        }
        DressManager.Instance.dummyCharacter = Player2_Char1;

        foreach (var id in pair.Player2_flatmate1_data.Dress_ids)
        {
            DressItem dress = FindDressWithId(id);
            if (dress != null)
                DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
        }
        SaloonItem saloon = FindSaloonWithId(pair.Player2_flatmate1_data.HairsId);
        if (saloon != null)
        {     
            DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
        }}


        {
            var P2F2 = FindFlatMateAvtar(pair.Player2_flatmate2_data.FlatmateId);
        if(P2F2 != null){
            Player2_Char2 = (GameObject)Instantiate (P2F2, Vector3.zero, Quaternion.identity);                
        }else
        {
            if (pair.player2_Gender == "Male") 
                Player2_Char2 = (GameObject)Instantiate (MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player2_Char2 = (GameObject)Instantiate (FemalePrefab, Vector3.zero, Quaternion.identity);

            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char2, pair.player2_id));       
        }

        DressManager.Instance.dummyCharacter = Player2_Char2;

        foreach(var id in pair.Player2_flatmate2_data.Dress_ids)
        {
            DressItem dress = FindDressWithId(id);
            if(dress != null)
                DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
        }
        SaloonItem saloon = FindSaloonWithId(pair.Player2_flatmate2_data.HairsId);
        if(saloon != null)
        {     
            DressManager.Instance.ChangeDressForDummyCharacter (saloon.PartName, saloon.HairImages);                
        }}

        {
            var P2F3 = FindFlatMateAvtar(pair.Player2_flatmate3_data.FlatmateId);
        if(P2F3 != null){
            Player2_Char3 = (GameObject)Instantiate (P2F3, Vector3.zero, Quaternion.identity);                
        }else
        {
            if (pair.player2_Gender == "Male") 
                Player2_Char3 = (GameObject)Instantiate (MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player2_Char3 = (GameObject)Instantiate (FemalePrefab, Vector3.zero, Quaternion.identity);

            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char3, pair.player2_id)); 
        
        }
        DressManager.Instance.dummyCharacter = Player2_Char3;

        foreach(var id in pair.Player2_flatmate3_data.Dress_ids)
        {
            DressItem dress = FindDressWithId(id);
            if(dress != null)
                DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
        }
        SaloonItem saloon = FindSaloonWithId(pair.Player2_flatmate3_data.HairsId);
        if(saloon != null)
        {     
            DressManager.Instance.ChangeDressForDummyCharacter (saloon.PartName, saloon.HairImages);                
        }}

        Player1_Char1.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
        Player1_Char1.transform.localScale = Vector3.one * 0.3f;
        Player1_Char1.transform.localPosition = new Vector3 (0, 0.5f, 0f);

        Player1_Char2.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
        Player1_Char2.transform.localScale = Vector3.one * 0.25f;
        Player1_Char2.transform.localPosition = new Vector3 (0.85f, 1f, 0f);


        Player1_Char3.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
        Player1_Char3.transform.localScale = Vector3.one * 0.35f;
        Player1_Char3.transform.localPosition = new Vector3 (-1, 0f, 0f);


        Player2_Char1.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0));
        Player2_Char1.transform.localScale = Vector3.one * 0.3f;            
        Player2_Char1.transform.localPosition = new Vector3 (0, 0.5f, 0f);

        Player2_Char2.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0));
        Player2_Char2.transform.localScale = Vector3.one * 0.35f;             
        Player2_Char2.transform.localPosition = new Vector3 (1, 0f, 0f);  



        Player2_Char3.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0));
        Player2_Char3.transform.localScale = Vector3.one * 0.25f;           
        Player2_Char3.transform.localPosition = new Vector3 (-0.85f, 1f, 0f);




//        for (int i = 0; i < pair.Player1_flatmate1_data; i++) {
//            
//			var dress = FindDressWithId (pair.Player1_dress_id [i]);
//            if (dress != null) {//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
////					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
////				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
////					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
////				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//                DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
//			}
//		}
//
//		for (int i = 0; i < Mathf.Min (pair.Player2_flatmate_id.Count, pair.Player2_dress_id.Count); i++) {
//			var dress = FindDressWithId (pair.Player2_dress_id [i]);
//			if (dress != null) {
//				if (i == 0)
//					DressManager.Instance.dummyCharacter = Player2_Char1;
//				if (i == 1)
//					DressManager.Instance.dummyCharacter = Player2_Char2;
//				if (i == 2)
//					DressManager.Instance.dummyCharacter = Player2_Char3;
//
////				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
////				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
////					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
////				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
////					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//			}
//		}

		int Layer = LayerMask.NameToLayer ("UI3D");

		Player1_Char1.SetLayerRecursively (Layer);
		Player1_Char2.SetLayerRecursively (Layer);
		Player1_Char3.SetLayerRecursively (Layer);
		Player2_Char1.SetLayerRecursively (Layer);
		Player2_Char2.SetLayerRecursively (Layer);
		Player2_Char3.SetLayerRecursively (Layer);

		Player1_Char1.SetOrderInLayerRecursively (100);
		Player1_Char2.SetOrderInLayerRecursively (200);
		Player1_Char3.SetOrderInLayerRecursively (300);
		Player2_Char1.SetOrderInLayerRecursively (100);
		Player2_Char2.SetOrderInLayerRecursively (200);
		Player2_Char3.SetOrderInLayerRecursively (300);


		DressManager.Instance.dummyCharacter = previous;

		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;
        ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.08f, 0.0f, 0.41f, 0.8f);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);

		ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (true);
		//true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "WHO'S GOT THE BETTER OUTFIT?";

		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").GetComponentInChildren<Text> ().text = "VOTES LEFT \n" + (10 - GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id));
	}



	public void CatWalk_ShowMyPair ()
	{
		SelectedEventType = "CatWalk";
		StartCoroutine (IeCatWalkShowMyPair (1, EventManagment.Instance.CurrentEvent.Event_id, false));
	}


	public IEnumerator IeCatWalkShowMyPair (int count, int eventId, bool isResult)
	{

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = eventId.ToString ();
		jsonElement ["count"] = count.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/catwalkGetSinglePair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");
				CatWalkVotingPair vp = new CatWalkVotingPair ();

				JSONNode dataArray = _jsnode ["data"];
				int.TryParse (dataArray ["event_id"], out vp.event_id);
				int.TryParse (dataArray ["pair_id"], out vp.pair_id);

				int.TryParse (dataArray ["player1_id"], out vp.player1_id);
				int.TryParse (dataArray ["player2_id"], out vp.player2_id);
				vp.player1Name = dataArray ["player1_name"].ToString ().Trim ('"');
				vp.player2Name = dataArray ["player2_name"].ToString ().Trim ('"');

                var player1Items = dataArray ["player1_catwalk_item"];

                var player1Flatmate1data = new CatwalkFlatmateData();
                var player1Flatmate2data = new CatwalkFlatmateData();
                var player1Flatmate3data = new CatwalkFlatmateData();

                for (int j = 0; j < player1Items.Count; j++) {
                    int Id = 0;
                    int.TryParse (player1Items [j] ["item_id"], out Id);
                    var jhgdkhgfd = player1Items [j] ["item_type"].ToString ();


                    if (player1Items [j] ["item_type"].ToString ().Contains ("Flatmates")) {
                        var FlatmateIdenifier =int.Parse(player1Items [j] ["item_type"].Value.ToString ().Split('_')[1]);

                        if(FlatmateIdenifier == 0)
                        {
                            player1Flatmate1data.FlatmateId = Id;

                        }else if(FlatmateIdenifier ==1)
                        {
                            player1Flatmate2data.FlatmateId = Id;
                        }else if(FlatmateIdenifier == 2)
                        {
                            player1Flatmate3data.FlatmateId = Id;
                        }

                        //                          vp.Player1_flatmate_id.Add (Id);
                    }
                    if (player1Items [j] ["item_type"].ToString ().Contains ("Hair")) {
                        var FlatmateIdenifier =int.Parse(player1Items [j] ["item_type"].Value.ToString ().Split('_')[1]);

                        if(FlatmateIdenifier == 0)
                        {
                            player1Flatmate1data.HairsId = Id;

                        }else if(FlatmateIdenifier ==1)
                        {
                            player1Flatmate2data.HairsId = Id;
                        }else if(FlatmateIdenifier == 2)
                        {
                            player1Flatmate3data.HairsId = Id;
                        }
                        //                          vp.Player1_hair_id.Add (Id);
                    }
                    if (player1Items [j] ["item_type"].ToString ().Contains ("Clothes")) {
                        var FlatmateIdenifier =int.Parse(player1Items [j] ["item_type"].Value.ToString ().Split('_')[1]);

                        if(FlatmateIdenifier == 0)
                        {
                            player1Flatmate1data.Dress_ids.Add(Id);

                        }else if(FlatmateIdenifier ==1)
                        {
                            player1Flatmate2data.Dress_ids.Add(Id);
                        }else if(FlatmateIdenifier == 2)
                        {
                            player1Flatmate3data.Dress_ids.Add(Id);
                        }
                        //                          vp.Player1_dress_id.Add (Id);
                    }
                    //                      if (player1Items [j] ["item_type"].ToString ().Contains ("Shoes")) {
                    //                          vp.Player1_shoes_id.Add (Id);
                    //                      }
                    if (player1Items [j] ["item_type"].ToString ().Contains ("Gender")) {

                        if (Id == 1) {
                            vp.player1_Gender = "Male";
                        } else {
                            vp.player1_Gender = "Female";
                        }
                    }
                }

                vp.Player1_flatmate1_data = player1Flatmate1data;
                vp.Player1_flatmate2_data = player1Flatmate2data;
                vp.Player1_flatmate3_data = player1Flatmate3data;

                var player2Items = dataArray ["player2_catwalk_item"];
                var player2Flatmate1data = new CatwalkFlatmateData();
                var player2Flatmate2data = new CatwalkFlatmateData();
                var player2Flatmate3data = new CatwalkFlatmateData();

                for (int j = 0; j < player2Items.Count; j++)
                {
                    int Id = 0;
                    int.TryParse(player2Items[j]["item_id"], out Id);


                    if (player2Items[j]["item_type"].ToString().Contains("Flatmates"))
                    {
                        var FlatmateIdenifier = int.Parse(player2Items[j]["item_type"].Value.ToString().Split('_')[1]);

                        if (FlatmateIdenifier == 0)
                        {
                            player2Flatmate1data.FlatmateId = Id;

                        }
                        else if (FlatmateIdenifier == 1)
                        {
                            player2Flatmate2data.FlatmateId = Id;
                        }
                        else if (FlatmateIdenifier == 2)
                        {
                            player2Flatmate3data.FlatmateId = Id;
                        }
                        //                          vp.Player2_flatmate_id.Add (Id);
                    }
                    if (player2Items[j]["item_type"].ToString().Contains("Hair"))
                    {                 
                        var FlatmateIdenifier = int.Parse(player2Items[j]["item_type"].Value.ToString().Split('_')[1]);
                        
                        if (FlatmateIdenifier == 0)
                        {
                            player2Flatmate1data.HairsId = Id;

                        }
                        else if (FlatmateIdenifier == 1)
                        {
                            player2Flatmate2data.HairsId = Id;
                        }
                        else if (FlatmateIdenifier == 2)
                        {
                            player2Flatmate3data.HairsId = Id;
                        }
                        //                          vp.Player2_hair_id.Add (Id);
                    }                       
                    if (player2Items[j]["item_type"].ToString().Contains("Clothes"))
                    {
                        var FlatmateIdenifier = int.Parse(player2Items[j]["item_type"].Value.ToString().Split('_')[1]);

                        if (FlatmateIdenifier == 0)
                        {
                            player2Flatmate1data.Dress_ids.Add(Id);

                        }
                        else if (FlatmateIdenifier == 1)
                        {
                            player2Flatmate2data.Dress_ids.Add(Id);
                        }
                        else if (FlatmateIdenifier == 2)
                        {
                            player2Flatmate3data.Dress_ids.Add(Id);
                        }
                        //                          vp.Player2_dress_id.Add (Id);
                    }
                    //                      if (player2Items [j] ["item_type"].ToString ().Contains ("Shoes")) {
                    //                          vp.Player2_shoes_id.Add (Id);
                    //                      }
                    if (player2Items[j]["item_type"].ToString().Contains("Gender"))
                    {
                        if (Id == 1)
                        {
                            vp.player2_Gender = "Male";
                        }
                        else
                        {
                            vp.player2_Gender = "Female";
                        }
                    }
                }

                vp.Player2_flatmate1_data = player2Flatmate1data;
                vp.Player2_flatmate2_data = player2Flatmate2data;
                vp.Player2_flatmate3_data = player2Flatmate3data;



				SelectedPair_CatWalk = vp;

				Player1Voting_Button.interactable = true;
				Player2Voting_Button.interactable = true;


				ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
				GetVotingResult_CatWalk (vp.player1_id, vp.event_id, vp.pair_id);

				player1Name.text = vp.player1Name;
				player2Name.text = vp.player2Name;

                GameObject Player1_Char1 = null;
                GameObject Player1_Char2 = null;
                GameObject Player1_Char3 = null;
                GameObject Player2_Char1 = null;
                GameObject Player2_Char2 = null;
                GameObject Player2_Char3 = null;

                var previous = DressManager.Instance.dummyCharacter;

                {
                    var P1F1 = FindFlatMateAvtar(vp.Player1_flatmate1_data.FlatmateId);
                    if (P1F1 != null)
                    {
                        Player1_Char1 = (GameObject)Instantiate(P1F1, Vector3.zero, Quaternion.identity);                
                    }
                    else
                    {
                        if (vp.player1_Gender == "Male")
                            Player1_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                        else
                            Player1_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);

                        yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char1, vp.player1_id));                       
                    }
                    DressManager.Instance.dummyCharacter = Player1_Char1;
                    foreach (var id in vp.Player1_flatmate1_data.Dress_ids)
                    {
                        DressItem dress = FindDressWithId(id);
                        if (dress != null)
                            DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
                    }
                    SaloonItem saloon = FindSaloonWithId(vp.Player1_flatmate1_data.HairsId);
                    if (saloon != null)
                    {     
                        DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
                    }
                }

                {
                    var P1F2 = FindFlatMateAvtar(vp.Player1_flatmate2_data.FlatmateId);
                    if (P1F2 != null)
                    {
                        Player1_Char2 = (GameObject)Instantiate(P1F2, Vector3.zero, Quaternion.identity);                
                    }
                    else
                    {
                        if (vp.player1_Gender == "Male")
                            Player1_Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                        else
                            Player1_Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);

                        yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char2, vp.player1_id));                     
                    }
                    DressManager.Instance.dummyCharacter = Player1_Char2;
                    foreach (var id in vp.Player1_flatmate2_data.Dress_ids)
                    {
                        DressItem dress = FindDressWithId(id);
                        if (dress != null)
                            DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
                    }
                    SaloonItem saloon = FindSaloonWithId(vp.Player1_flatmate2_data.HairsId);
                    if (saloon != null)
                    {     
                        DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
                    }
                }
                {
                    var P1F3 = FindFlatMateAvtar(vp.Player1_flatmate3_data.FlatmateId);
                    if (P1F3 != null)
                    {
                        Player1_Char3 = (GameObject)Instantiate(P1F3, Vector3.zero, Quaternion.identity);                
                    }
                    else
                    {
                        if (vp.player1_Gender == "Male")
                            Player1_Char3 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                        else
                            Player1_Char3 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);

                        yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char3, vp.player1_id));
                    } 
                    DressManager.Instance.dummyCharacter = Player1_Char3;
                    foreach (var id in vp.Player1_flatmate3_data.Dress_ids)
                    {
                        DressItem dress = FindDressWithId(id);
                        if (dress != null)
                            DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
                    }
                    SaloonItem saloon = FindSaloonWithId(vp.Player1_flatmate3_data.HairsId);
                    if (saloon != null)
                    {     
                        DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
                    }
                }

                {
                    var P2F1 = FindFlatMateAvtar(vp.Player2_flatmate1_data.FlatmateId);
                    if (P2F1 != null)
                    {
                        Player2_Char1 = (GameObject)Instantiate(P2F1, Vector3.zero, Quaternion.identity);                
                    }
                    else
                    {
                        if (vp.player2_Gender == "Male")
                            Player2_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                        else
                            Player2_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);

                        yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char1, vp.player2_id));                       
                    }
                    DressManager.Instance.dummyCharacter = Player2_Char1;
                    foreach (var id in vp.Player2_flatmate1_data.Dress_ids)
                    {
                        DressItem dress = FindDressWithId(id);
                        if (dress != null)
                            DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
                    }
                    SaloonItem saloon = FindSaloonWithId(vp.Player2_flatmate1_data.HairsId);
                    if (saloon != null)
                    {     
                        DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
                    }
                }

                {
                    var P2F2 = FindFlatMateAvtar(vp.Player2_flatmate2_data.FlatmateId);
                    if (P2F2 != null)
                    {
                        Player2_Char2 = (GameObject)Instantiate(P2F2, Vector3.zero, Quaternion.identity);                
                    }
                    else
                    {
                        if (vp.player2_Gender == "Male")
                            Player2_Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                        else
                            Player2_Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);

                        yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char2, vp.player2_id));                       
                    }
                    DressManager.Instance.dummyCharacter = Player2_Char2;

                    foreach (var id in vp.Player2_flatmate2_data.Dress_ids)
                    {
                        DressItem dress = FindDressWithId(id);
                        if (dress != null)
                            DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
                    }
                    SaloonItem saloon = FindSaloonWithId(vp.Player2_flatmate2_data.HairsId);
                    if (saloon != null)
                    {     
                        DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
                    }
                }
                {
                    var P2F3 = FindFlatMateAvtar(vp.Player2_flatmate3_data.FlatmateId);
                    if (P2F3 != null)
                    {
                        Player2_Char3 = (GameObject)Instantiate(P2F3, Vector3.zero, Quaternion.identity);                
                    }
                    else
                    {
                        if (vp.player2_Gender == "Male")
                            Player2_Char3 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
                        else
                            Player2_Char3 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);

                        yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char3, vp.player2_id));                       
                    }
                    DressManager.Instance.dummyCharacter = Player2_Char3;
                    foreach (var id in vp.Player2_flatmate3_data.Dress_ids)
                    {
                        DressItem dress = FindDressWithId(id);
                        if (dress != null)
                            DressManager.Instance.ChangeDressForDummyCharacter(dress.PartName, dress.DressesImages);
                    }
                    SaloonItem saloon = FindSaloonWithId(vp.Player2_flatmate3_data.HairsId);
                    if (saloon != null)
                    {     
                        DressManager.Instance.ChangeDressForDummyCharacter(saloon.PartName, saloon.HairImages);                
                    }
                }

                Player1_Char1.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
				Player1_Char1.transform.localScale = Vector3.one * 0.3f;
				Player1_Char1.transform.localPosition = new Vector3 (0, 0.5f, 0f);

                Player1_Char2.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
				Player1_Char2.transform.localScale = Vector3.one * 0.25f;
				Player1_Char2.transform.localPosition = new Vector3 (0.85f, 1f, 0f);


                Player1_Char3.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0));
				Player1_Char3.transform.localScale = Vector3.one * 0.35f;
				Player1_Char3.transform.localPosition = new Vector3 (-1, 0f, 0f);


                Player2_Char1.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0));
				Player2_Char1.transform.localScale = Vector3.one * 0.3f;			
                Player2_Char1.transform.localPosition = new Vector3 (0, 0.5f, 0f);
               
                Player2_Char2.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0));
				Player2_Char2.transform.localScale = Vector3.one * 0.35f;             
                Player2_Char2.transform.localPosition = new Vector3 (1, 0f, 0f);  
               


                Player2_Char3.transform.SetParent(ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0));
				Player2_Char3.transform.localScale = Vector3.one * 0.25f;			
                Player2_Char3.transform.localPosition = new Vector3 (-0.85f, 1f, 0f);


//				for (int i = 0; i < Mathf.Min (vp.Player1_flatmate_id.Count, vp.Player1_dress_id.Count); i++) {
//					var dress = FindDressWithId (vp.Player1_dress_id [i]);
//					if (dress != null) {
//						if (i == 0)
//							DressManager.Instance.dummyCharacter = Player1_Char1;
//						if (i == 1)
//							DressManager.Instance.dummyCharacter = Player1_Char2;
//						if (i == 2)
//							DressManager.Instance.dummyCharacter = Player1_Char3;
//
////						if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
////						if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
////							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
////						if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
////							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//					}
//				}
//
//				for (int i = 0; i < Mathf.Min (vp.Player2_flatmate_id.Count, vp.Player2_dress_id.Count); i++) {
//					var dress = FindDressWithId (vp.Player2_dress_id [i]);
//					if (dress != null) {
//						if (i == 0)
//							DressManager.Instance.dummyCharacter = Player2_Char1;
//						if (i == 1)
//							DressManager.Instance.dummyCharacter = Player2_Char2;
//						if (i == 2)
//							DressManager.Instance.dummyCharacter = Player2_Char3;
//
////						if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
////						if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
////							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
////						if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
////							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//					}
//				}

				int Layer = LayerMask.NameToLayer ("UI3D");

				Player1_Char1.SetLayerRecursively (Layer);
				Player1_Char2.SetLayerRecursively (Layer);
				Player1_Char3.SetLayerRecursively (Layer);
				Player2_Char1.SetLayerRecursively (Layer);
				Player2_Char2.SetLayerRecursively (Layer);
				Player2_Char3.SetLayerRecursively (Layer);

				Player1_Char1.SetOrderInLayerRecursively (100);
				Player1_Char2.SetOrderInLayerRecursively (200);
				Player1_Char3.SetOrderInLayerRecursively (300);
				Player2_Char1.SetOrderInLayerRecursively (100);
				Player2_Char2.SetOrderInLayerRecursively (200);
				Player2_Char3.SetOrderInLayerRecursively (300);


				DressManager.Instance.dummyCharacter = previous;

				ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
				ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
				ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
				ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;
                ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.08f, 0.0f, 0.41f, 0.8f);
				ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);
				ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (false);


				if (!isResult) {
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();
					ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);

					ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (false);
					ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (false);
					//false);

					if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);

					ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "YOUR STATUS";

				} else {
					ScreenAndPopupCall.Instance.RewardScreenSelection ();
					ScreenAndPopupCall.Instance.ResultPanelClose ();
				}

				yield return vp;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				if (viewStatus)
					ShowPopOfDescription ("Currently no pair have registered for this Event");	
				yield return null;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return null;
			}
		} else {			
			yield return null;
		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
	}

	DressItem FindDressWithName (string Cat, string Name)
	{
		foreach (var Item in PurchaseDressManager.Instance.AllDresses) {
			if (Cat.Contains (Item.Catergory.ToString ()) && Name == Item.Name) {				
				return Item;				
			} //else
			//return null;
		}
		return null;
	}


	DressItem FindDressWithItemId (int itemId)
	{
		string cat = "";
		string name = "";
		foreach (var item in DownloadContent.Instance.downloaded_items) {
			if (item.Item_id == itemId) {
				cat = item.Category.Trim ('"');
				name = item.Name.Trim ('"');
			}
		}

		var dress = FindDressWithName (cat, name);
		return dress;
	}

	public DressItem FindDressWithId (int id)
	{
		foreach (var Item in PurchaseDressManager.Instance.AllDresses) {
			if (Item.Id == id) {				
				return Item;				
			} //else
			//return null;
		}
		return null;
	}

	SaloonItem FindSaloonWithId (int id)
	{
		foreach (var Item in PurchaseSaloonManager.Instance.AllItems) {
			if (Item.item_id == id) {				
				return Item;				
			} //else
			//return null;
		}
		return null;
	}





	#region GetVotingPairForFashion

	public void GetVotingPair_Fashion (bool isForNextPair)
	{
		SelectedEventType = "Fashion";
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = false;
		StartCoroutine (IeGetVotingPair_Fashion (isForNextPair));
	}

	IEnumerator IeGetVotingPair_Fashion (bool isForNextPair)
	{
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["page_no"] = pairIndex.ToString ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/fashionshow_getvotingpair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");


				JSONNode dataArray = _jsnode ["data"];
				VotingPair Vp = new VotingPair ();

				for (int i = 0; i < dataArray.Count; i++) {

					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					Vp.Gender = dataArray [i] ["gender"].ToString ();

					Vp.player1_id = int.Parse (dataArray [i] ["player1_id"]);
					Vp.player1_flatmate_id = int.Parse (dataArray [i] ["player1_flatmate_id"]);
					Vp.player1Name = dataArray [i] ["player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode p1DressData = dataArray [i] ["player1_item_data"];

					var P1_SkinColor = "";

					for (int a = 0; a < p1DressData.Count; a++) {						
						var itemType = p1DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P1_SkinColor = p1DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p1DressData [a] ["item_id"], out itemId);

							if (Vp.player1_dressData.ContainsKey (itemId))
								Vp.player1_dressData [itemId] = itemType;
							else
								Vp.player1_dressData.Add (itemId, itemType);
						}
					}
					Vp.player1_SkinColor = P1_SkinColor;

					Vp.player2_id = int.Parse (dataArray [i] ["player2_id"]);
					Vp.player2_flatmate_id = int.Parse (dataArray [i] ["player2_flatmate_id"]);
					Vp.player2Name = dataArray [i] ["player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode p2DressData = dataArray [i] ["player2_item_data"];

					var P2_SkinColor = "";

					for (int a = 0; a < p2DressData.Count; a++) {
						var itemType = p2DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P2_SkinColor = p2DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p2DressData [a] ["item_id"], out itemId);

							if (Vp.player2_dressData.ContainsKey (itemId))
								Vp.player2_dressData [itemId] = itemType;
							else
								Vp.player2_dressData.Add (itemId, itemType);
						}
					}

					Vp.player2_SkinColor = P2_SkinColor;
				}

				//				var TempList =	FindPairInFriendList (AllPairInEevnt);
				//
				//				foreach (var pair in TempList) {
				//					if (!isAlReadyAddedFashion (pair.pair_id))
				//						FriendsInFashionEvent.Add (pair);
				//				}
				//
				//
				//				if (viewFriends) {
				//					if (FriendsInFashionEvent.Count != 0) {
				////						ScreenAndPopupCall.Instance.CloseScreen ();
				//						ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
				//						ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
				//						var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();
				//						ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";
				//						for (int i = 0; i < container.transform.childCount; i++) {
				//							Destroy (container.transform.GetChild (i).gameObject);
				//						}
				//
				//						FriendsInFashionEvent.ForEach (pair => {	
				//							foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
				//								if (friend.Id == pair.player1_id || friend.Id == pair.player2_id) {
				//									GameObject Go = Instantiate (FriendsManager.Instance.FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
				////										ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsScreen);
				//									Go.transform.parent = container.transform;
				//									Go.transform.localScale = Vector3.one;
				//									Go.transform.localPosition = Vector3.zero;
				//									Go.GetComponent <AddFriendUi> ().thisData = friend;
				//									Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.ViewFriend;
				//									Go.name = friend.Username;
				//									Go.GetComponent <AddFriendUi> ().ViewFriendInString = "Fashion";
				//								}
				//							}
				//						});
				//					} else
				//						ShowPopOfDescription ("You do not have any of your friends participating in this event.", false);
				//				} else {	

                StartCoroutine(ShowOnePairOnScreen (Vp));

				if (!isForNextPair) {			
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();
				}

				//				}

				yield return true;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
				if (isForNextPair) {
					pairIndex = 0;
					ShowPopOfDescription ("No pairs to display", () => NextPair ());
				} else
					ShowPopOfDescription ("Currently no pair have registered for this Event");	
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return false;			
			}
		} else {			
			yield return false;
		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
	}

	public void VoteForPlayer1 ()
	{
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!tut._FashionEventCompleate && tut.enabled)
			return;

		var votes = GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id);

		if (SelectedEventType == "Fashion") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {
				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				VotingPair Pair = SelectedPair_Fashion;
				var IsFriend = HasPairInFriendList (Pair.player1_id);
				StartCoroutine (VoteForPlayer (Pair.player1_id, Pair.player1_flatmate_id, Pair.pair_id, IsFriend));
			}

		} else if (SelectedEventType == "CatWalk") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {

				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				CatWalkVotingPair Pair = SelectedPair_CatWalk;
				var IsFriend = HasPairInFriendList (Pair.player1_id);
				StartCoroutine (VoteForPlayer_CatWalk (Pair.player1_id, Pair.event_id, Pair.pair_id, IsFriend));
			}
		} else if (SelectedEventType == "Decor") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {
				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				VotingPairForDecor pair = SelectedPair_Decor;
				var IsFriend = HasPairInFriendList (pair.player1_id);
				StartCoroutine (VoteForPlayer_Decore (pair.player1_id, pair.event_id, pair.pair_id, IsFriend));
			}
		} else if (SelectedEventType == "CoOp") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {
				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				CoOpVotingPair pair = SelectedPair_CoOp;
				var IsFriend = HasPairInFriendList (pair.set1_player1_id, pair.set1_player2_id);
				StartCoroutine (VoteForPlayerCoOp (pair.set1_player1_id, pair.set1_player2_id, pair.pair_id, IsFriend));
			}
		} else if (SelectedEventType == "SocietyEvent") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {	

				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				SocietyVotingPair Pair = SelectedPair_SocietyEvent;
				var IsFriend = HasPairInFriendList (Pair.player1_id);
				StartCoroutine (VoteInSocietyEvent (Pair.player1_id, Pair.player1societyId, Pair.player1_flatmate_id, Pair.pair_id, IsFriend));
			}
		}
	}

	public IEnumerator GetAllVotingPairs_Fashion_ForCheck ()
	{

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/fashionshow_getvotingpair", encoding.GetBytes (jsonElement.ToString ()), postHeader);


		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");


				JSONNode dataArray = _jsnode ["data"];

				for (int i = 0; i < dataArray.Count; i++) {
					VotingPair Vp = new VotingPair ();
					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					Vp.Gender = dataArray [i] ["gender"].ToString ();

					Vp.player1_id = int.Parse (dataArray [i] ["player1_id"]);
					Vp.player1_flatmate_id = int.Parse (dataArray [i] ["player1_flatmate_id"]);
					Vp.player1Name = dataArray [i] ["player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode p1DressData = dataArray [i] ["player1_item_data"];

					var P1_SkinColor = "";

					for (int a = 0; a < p1DressData.Count; a++) {						
						var itemType = p1DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P1_SkinColor = p1DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p1DressData [a] ["item_id"], out itemId);

							if (Vp.player1_dressData.ContainsKey (itemId))
								Vp.player1_dressData [itemId] = itemType;
							else
								Vp.player1_dressData.Add (itemId, itemType);
						}
					}
					Vp.player1_SkinColor = P1_SkinColor;

					Vp.player2_id = int.Parse (dataArray [i] ["player2_id"]);
					Vp.player2_flatmate_id = int.Parse (dataArray [i] ["player2_flatmate_id"]);
					Vp.player2Name = dataArray [i] ["player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode p2DressData = dataArray [i] ["player2_item_data"];

					var P2_SkinColor = "";

					for (int a = 0; a < p2DressData.Count; a++) {
						var itemType = p2DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P2_SkinColor = p2DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p2DressData [a] ["item_id"], out itemId);

							if (Vp.player2_dressData.ContainsKey (itemId))
								Vp.player2_dressData [itemId] = itemType;
							else
								Vp.player2_dressData.Add (itemId, itemType);
						}
					}

					Vp.player2_SkinColor = P2_SkinColor;

					if (Vp.player1_id == PlayerPrefs.GetInt ("PlayerId") || Vp.player2_id == PlayerPrefs.GetInt ("PlayerId"))
						AllPlayerPair.Add (Vp);
					else
						AllPairInEevnt.Add (Vp);
				}
				SelectedEventType = "Fashion";
//				ShowRewardScreen ();

				yield return true;



			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				ShowPopOfDescription ("You are not the participant of this event");
				ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
			} else {
				print ("error" + _jsnode ["description"].ToString ());

				yield return false;

			}
		} else {
			yield return false;
		}
	}




	//	void ShowRewardScreen ()
	//	{
	//		pairIndex = 1;
	//		if (SelectedEventType == "Fashion") {
	//			if (AllPlayerPair.Count > 0) {
	//				VotingPair Pair = AllPlayerPair [pairIndex];
	//				ShowMyPairOnScreen (Pair);
	//			}
	//		} else if (SelectedEventType == "CatWalk") {
	//			CatWalk_ShowMyPair ();
	//		} else if (SelectedEventType == "Decor") {
	//			ShowMyPairOnScreenOfDecor (false);
	//		} else if (SelectedEventType == "CoOp") {
	//			if (MyPlayersInCoOp.Count > 0) {
	//				CoOpVotingPair pair = MyPlayersInCoOp [pairIndex];
	//				ShowMyPlayerCoOp (pair);
	//			}
	//		}
	//
	//		if (pairIndex < result.Count) {
	//			player2voteCount.GetComponent<Text> ().text = result [pairIndex].player2_voteCount.ToString ();
	//			player2votingBonus.GetComponent<Text> ().text = result [pairIndex].player2_votingBonus.ToString ();
	//			player2FriendBonus.GetComponent<Text> ().text = result [pairIndex].player2_friendBonus.ToString ();
	//
	//			player1voteCount.GetComponent<Text> ().text = result [pairIndex].player1_voteCount.ToString ();
	//			player1votingBonus.GetComponent<Text> ().text = result [pairIndex].player1_votingBonus.ToString ();
	//			player1FriendBonus.GetComponent<Text> ().text = result [pairIndex].player1_friendBonus.ToString ();
	//
	//			if (AllPlayerPair.Count > 1) {
	//				GameObject.Find ("Canvas/Result Screen/NextPairButton").SetActive (true);
	//				GameObject.Find ("Canvas/Result Screen/NextPairButton").GetComponent <Button> ().onClick.AddListener (() => ShowNextPairForReward ());
	//			} else {
	//				GameObject.Find ("Canvas/Result Screen/NextPairButton").SetActive (false);
	//			}
	//		} else {
	//			player2voteCount.GetComponent<Text> ().text = "0";
	//			player2votingBonus.GetComponent<Text> ().text = "0";
	//			player2FriendBonus.GetComponent<Text> ().text = "0";
	//
	//			player1voteCount.GetComponent<Text> ().text = "0";
	//			player1votingBonus.GetComponent<Text> ().text = "0";
	//			player1FriendBonus.GetComponent<Text> ().text = "0";
	//
	//			if (AllPlayerPair.Count > 1) {
	//				GameObject.Find ("Canvas/Result Screen/NextPairButton").SetActive (true);
	//				GameObject.Find ("Canvas/Result Screen/NextPairButton").GetComponent <Button> ().onClick.AddListener (() => ShowNextPairForReward ());
	//			} else {
	//				GameObject.Find ("Canvas/Result Screen/NextPairButton").SetActive (false);
	//			}
	//		}
	//
	//
	//	}



	void ShowNextPairForReward ()
	{
		pairIndex++;

		if (pairIndex >= AllPlayerPair.Count)
			pairIndex = 1;

		VotingPair Pair = AllPlayerPair [pairIndex];
        StartCoroutine(ShowMyPairOnScreen (Pair));

		GameObject.Find ("Canvas/Result Screen/Player 2/Vote_Count").GetComponent<Text> ().text = result [pairIndex].player2_voteCount.ToString ();
		GameObject.Find ("Canvas/Result Screen/Player 2/Voting_Bonus").GetComponent<Text> ().text = result [pairIndex].player2_votingBonus.ToString ();
		GameObject.Find ("Canvas/Result Screen/Player 2/Friend_Bonus").GetComponent<Text> ().text = result [pairIndex].player2_friendBonus.ToString ();

		GameObject.Find ("Canvas/Result Screen/Player 1/Vote_Count").GetComponent<Text> ().text = result [pairIndex].player1_voteCount.ToString ();
		GameObject.Find ("Canvas/Result Screen/Player 1/Voting_Bonus").GetComponent<Text> ().text = result [pairIndex].player1_votingBonus.ToString ();
		GameObject.Find ("Canvas/Result Screen/Player 1/Friend_Bonus").GetComponent<Text> ().text = result [pairIndex].player1_friendBonus.ToString ();


	}



	public List<ResultObject> result = new List<ResultObject> ();

	/// <summary>
	/// It's the Ienumerator of the leaderboard for downloading the data from the server
	/// </summary>
	/// <returns>The leaderboard.</returns>
	/// <param name="_eventType">Event type.</param>
	/// <param name="_eventId">Event identifier.</param>
	public IEnumerator ResultShow (string _eventType, int _eventId)
	{

		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkGetVoteData";
			break;
		case "CoOp_Event":
			link = ""; //"http://pinekix.ignivastaging.com/events/coopGetVoteData";
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decorGetVoteData";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshowGetVoteData";
			break;
		}

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var json = new JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		json ["event_id"] = _eventId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW (link, encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			for (int i = 0; i < _jsnode ["data"].Count; i++) {
				int player1_id = 0;
				string player1_name = _jsnode ["data"] [i] ["player1_name"].ToString ().Trim ('"');
				int player1_voteCount = 0;
				int player1_votescore = 0;
				int player1_votebonus = 0;
				int player1_friendbonus = 0;

				int player2_id = 0;
				string player2_name = _jsnode ["data"] [i] ["player2_name"].ToString ().Trim ('"');
				int player2_voteCount = 0;
				int player2_votescore = 0;
				int player2_votebonus = 0;
				int player2_friendbonus = 0;




				int.TryParse (_jsnode ["data"] [i] ["player1"].ToString ().Trim ('"'), out player1_id);
				int.TryParse (_jsnode ["data"] [i] ["player1_votecount"].ToString ().Trim ('"'), out player1_voteCount);
				int.TryParse (_jsnode ["data"] [i] ["player1_votescore"].ToString ().Trim ('"'), out player1_votescore);
				int.TryParse (_jsnode ["data"] [i] ["player1_vote_bonus"].ToString ().Trim ('"'), out player1_votebonus);
				int.TryParse (_jsnode ["data"] [i] ["player1_friend_bonus"].ToString ().Trim ('"'), out player1_friendbonus);

				int.TryParse (_jsnode ["data"] [i] ["player2"].ToString ().Trim ('"'), out player2_id);
				int.TryParse (_jsnode ["data"] [i] ["player2_votecount"].ToString ().Trim ('"'), out player2_voteCount);
				int.TryParse (_jsnode ["data"] [i] ["player2_votescore"].ToString ().Trim ('"'), out player2_votescore);
				int.TryParse (_jsnode ["data"] [i] ["player2_vote_bonus"].ToString ().Trim ('"'), out player2_votebonus);
				int.TryParse (_jsnode ["data"] [i] ["player2_friend_bonus"].ToString ().Trim ('"'), out player2_friendbonus);




				if (player1_id == PlayerPrefs.GetInt ("PlayerId") || player2_id == PlayerPrefs.GetInt ("PlayerId")) {
					ResultObject obj = new ResultObject ();

					obj.player1_id = player1_id;
					obj.player1_name = player1_name;
					obj.player1_voteCount = player1_voteCount;
					obj.player1_votingBonus = player1_votebonus;
					obj.player1_friendBonus = player1_friendbonus;

					obj.player2_id = player2_id;
					obj.player2_name = player2_name;
					obj.player2_voteCount = player2_voteCount;
					obj.player2_votingBonus = player2_votebonus;
					obj.player2_friendBonus = player2_friendbonus;

					result.Add (obj);

					print (obj); 
				}
			}
		} else {
			print ("you are not registered for this event"); 
			ShowPopOfDescription ("You are not registered for this event");
		}
	}



	public void GetMyPair_Fashion ()
	{
		SelectedEventType = "Fashion";
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = false;
		StartCoroutine (IeGetMyPair_Fashion (1, EventManagment.Instance.CurrentEvent.Event_id, false));
	}

	public IEnumerator IeGetMyPair_Fashion (int count, int EventId, bool IsRewards)
	{
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		jsonElement ["event_id"] = EventId.ToString ();
		jsonElement ["count"] = count.ToString ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/fashionshow_getSinglePair", encoding.GetBytes (jsonElement.ToString ()), postHeader);


		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode dataArray = _jsnode ["data"];
				var Vp = new VotingPair ();

				int.TryParse (dataArray ["pair_id"], out Vp.pair_id);
				int.TryParse (dataArray ["event_id"], out Vp.event_id);

				Vp.Gender = dataArray ["gender"].ToString ();

				int.TryParse (dataArray ["player1_id"], out Vp.player1_id);
				int.TryParse (dataArray ["player1_flatmate_id"], out Vp.player1_flatmate_id);
				Vp.player1Name = dataArray ["player1_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode p1DressData = dataArray ["player1_item_data"];

				var P1_SkinColor = "";

				for (int a = 0; a < p1DressData.Count; a++) {						
					var itemType = p1DressData [a] ["item_type"];

					if (itemType.ToString ().Contains ("CharacterType"))
						P1_SkinColor = p1DressData [a] ["item_id"];
					else {
						var itemId = 0;
						int.TryParse (p1DressData [a] ["item_id"], out itemId);

						if (Vp.player1_dressData.ContainsKey (itemId))
							Vp.player1_dressData [itemId] = itemType;
						else
							Vp.player1_dressData.Add (itemId, itemType);
					}
				}
				Vp.player1_SkinColor = P1_SkinColor;

				Vp.player2_id = int.Parse (dataArray ["player2_id"]);
				Vp.player2_flatmate_id = int.Parse (dataArray ["player2_flatmate_id"]);
				Vp.player2Name = dataArray ["player2_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode p2DressData = dataArray ["player2_item_data"];

				var P2_SkinColor = "";

				for (int a = 0; a < p2DressData.Count; a++) {
					var itemType = p2DressData [a] ["item_type"];

					if (itemType.ToString ().Contains ("CharacterType"))
						P2_SkinColor = p2DressData [a] ["item_id"];
					else {
						var itemId = 0;
						int.TryParse (p2DressData [a] ["item_id"], out itemId);

						if (Vp.player2_dressData.ContainsKey (itemId))
							Vp.player2_dressData [itemId] = itemType;
						else
							Vp.player2_dressData.Add (itemId, itemType);
					}
				}

				Vp.player2_SkinColor = P2_SkinColor;

				SelectedEventType = "Fashion";

				ScreenAndPopupCall.Instance.CloseScreen ();

				yield return new WaitForSeconds (0.1f);

				if (IsRewards) {
					ScreenAndPopupCall.Instance.RewardScreenSelection ();
					ScreenAndPopupCall.Instance.ResultPanelClose ();
				} else {
					ScreenAndPopupCall.Instance.VotingScreenSelection ();					
				}
                StartCoroutine(ShowMyPairOnScreen (Vp));

				yield return Vp;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {	

				if (viewStatus)
					ShowPopOfDescription ("No pair found");
				yield return null;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return null;

			}
		} else {			
			yield return null;
		}

		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = true;
	}

	public void VoteForPlayer2 ()
	{
		var tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!tut._FashionEventCompleate && tut.enabled)
			return;

		if (SelectedEventType == "Fashion") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {	
				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				VotingPair Pair = SelectedPair_Fashion;
				var IsFriend = HasPairInFriendList (Pair.player2_id);
				StartCoroutine (VoteForPlayer (Pair.player2_id, Pair.player2_flatmate_id, Pair.pair_id, IsFriend));
			}

		} else if (SelectedEventType == "CatWalk") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {

				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				CatWalkVotingPair Pair = SelectedPair_CatWalk;
				var IsFriend = HasPairInFriendList (Pair.player2_id);
				StartCoroutine (VoteForPlayer_CatWalk (Pair.player2_id, Pair.event_id, Pair.pair_id, IsFriend));
			}
		} else if (SelectedEventType == "Decor") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {
				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				VotingPairForDecor pair = SelectedPair_Decor;
				var IsFriend = HasPairInFriendList (pair.player2_id);
				StartCoroutine (VoteForPlayer_Decore (pair.player2_id, pair.event_id, pair.pair_id, IsFriend));
			}
		} else if (SelectedEventType == "CoOp") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {
				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				CoOpVotingPair pair = SelectedPair_CoOp;
				var IsFriend = HasPairInFriendList (pair.set2_player1_id, pair.set2_player2_id);
				StartCoroutine (VoteForPlayerCoOp (pair.set2_player1_id, pair.set2_player2_id, pair.pair_id, IsFriend));
			}
		} else if (SelectedEventType == "SocietyEvent") {
			if (!CheckVotingBonus (EventManagment.Instance.CurrentEvent.Event_id)) {	

				ShowPopOfDescription ("You have used your all 10 votes for this Event.");
			} else {
				SocietyVotingPair Pair = SelectedPair_SocietyEvent;
				var IsFriend = HasPairInFriendList (Pair.player2_id);
				StartCoroutine (VoteInSocietyEvent (Pair.player2_id, Pair.player2societyId, Pair.player2_flatmate_id, Pair.pair_id, IsFriend));
			}
		}
	}

	#endregion



	#region GetAllVotingPair_Catwalk


	/// <summary>
	/// Gets all voting pairs catwalk.
	/// TODO: change complete data fetching for 3 roommates
	/// </summary>
	public IEnumerator GetVotesResult_CatWalk (int playerId, int pairId, int EventId, bool isShowingResult)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = playerId.ToString ();
		jsonElement ["event_id"] = EventId.ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/catwalkGetVotingResult", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting result data is following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode data = _jsnode ["data"];

				//				{
				//					"total_pair_vote": 1,
				//					"first_player_id": "81",
				//					"first_player_vote": 0,
				//					"first_player_vote_data": {
				//						"votecount": "",
				//						"votescore": "",
				//						"votebonus": "",
				//						"friendbonus": ""
				//					},
				//					"second_player_id": "71",
				//					"second_player_vote": 1,
				//					"second_player_vote_data": {
				//						"votecount": "1",
				//						"votescore": "0",
				//						"votebonus": "0",
				//						"friendbonus": "0"
				//					}
				//				}
				float totalvotes = 0.0f;
//				float.TryParse (data ["total_pair_vote"], out totalvotes);


				int Player1Id;
				int.TryParse (data ["first_player_id"], out Player1Id);
				int Player2Id;
				int.TryParse (data ["second_player_id"], out Player2Id);

				float p1VotesCount = 0.0f;
				float.TryParse (data ["first_player_vote_data"] ["votecount"], out p1VotesCount);

				float p2VotesCount = 0.0f;
				float.TryParse (data ["second_player_vote_data"] ["votecount"], out p2VotesCount);

				int P1VoteBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["votebonus"], out P1VoteBonus);
				int P1FriendBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["friendbonus"], out P1FriendBonus);


				int P2VoteBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["votebonus"], out P2VoteBonus);
				int P2FriendBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["friendbonus"], out P2FriendBonus);


				if (totalvotes != 0f) {

					player1voteCount.text = p1VotesCount.ToString ();
					player2voteCount.text = p2VotesCount.ToString ();

				} else {
					player1voteCount.text = 0 + "";
					player2voteCount.text = 0 + "";
				}		

				player1votingBonus.text = P1VoteBonus.ToString ();
				player2votingBonus.text = P2VoteBonus.ToString ();

				player1FriendBonus.text = P1FriendBonus.ToString ();
				player2FriendBonus.text = P2FriendBonus.ToString ();

				player1ScoreText.text = "" + (p1VotesCount * 10 + P1VoteBonus + P1FriendBonus);
				player2ScoreText.text = "" + (p2VotesCount * 10 + P2VoteBonus + P2FriendBonus);



				if (isShowingResult) { 			
					ScreenAndPopupCall.Instance.ResultPanelClose ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (true);
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.RemoveAllListeners ();
					int P1Total = Mathf.RoundToInt (p1VotesCount) + P1VoteBonus + P1FriendBonus;
					int P2Total = Mathf.RoundToInt (p2VotesCount) + P2VoteBonus + P2FriendBonus;

					if (PlayerPrefs.GetInt ("PlayerId") == Player1Id) { // If i am player 1 and I won
						if (P1Total > P2Total) {
							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));
						} else if (P1Total < P2Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, EventId, 0)));
						} else if (P1Total == P2Total) {
							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));		
						}
					} else if (PlayerPrefs.GetInt ("PlayerId") == Player2Id) {// If i am player 2 and I won
						if (P2Total > P1Total) {
							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));
						} else if (P2Total < P1Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, EventId, 0)));
						} else if (P2Total == P1Total) {
							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));	
						}
					}
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Vote_Count").GetComponent<Text> ().text = P1Total.ToString ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Vote_Count").GetComponent<Text> ().text = P2Total.ToString ();

				}
				yield return true;
			} else {
				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	IEnumerator VoteForPlayer (int votedToId, int flatmateId, int pairId, bool isfriend)
	{

		Player1Voting_Button.interactable = false;
		Player2Voting_Button.interactable = false;
		///TODO: change for all events
		int vote = 1;
		int friendBous = 0;
		int votingBonus = 0;

		if (isfriend) {
			friendBous = 1;
			votingBonus = 0;
		} else if (votedToId == PlayerPrefs.GetInt ("PlayerId")) {
			vote = 0;
			friendBous = 0;
			votingBonus = 1;
		}

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();	
		jsonElement ["player_id"] = votedToId.ToString ();
		jsonElement ["votedby_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["flatmate_id"] = flatmateId.ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();
		jsonElement ["vote_count"] = vote.ToString ();
		jsonElement ["vote_score"] = "0";
		///TODO: apply vote bonus and friend bonus
		jsonElement ["vote_bonus"] = votingBonus.ToString ();
		jsonElement ["friend_bonus"] = friendBous.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/fashionshow_updatevotedresult", encoding.GetBytes (jsonElement.ToString ()), postHeader);
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				if (votingBonus > 0) {
					ShowPopOfDescription ("Voting bonus applied", () => GetVotingResult_Fashion (PlayerPrefs.GetInt ("PlayerId"), SelectedPair_Fashion.player1_flatmate_id, SelectedPair_Fashion.pair_id));
					PersistanceManager.Instance.DeleteThisVotingBonus (EventManagment.Instance.CurrentEvent.Event_id);
				} else {
					if (GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id) == 9) {	
						GenrateVotingBonus (EventManagment.Instance.CurrentEvent.Event_id, eventType.Fashion_Event);
						ShowPopOfDescription ("Your vote saved successfully.\n You have gained a new Voting Bonus. You have used your all 10 votes for this Event. You can use this bonus to boost your scores in any event within 2 hours.", () => NextPair ());
					} else {				
						ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()), () => NextPair ());
					}
					IncreamentVoteCount (EventManagment.Instance.CurrentEvent.Event_id);
					if (isfriend) {
						if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
							AchievementsManager.Instance.CheckAchievementsToUpdate ("voteForFriends");
					}
				}


				yield return true;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				if (votingBonus > 0) {
					ShowPopOfDescription ("Already applied a voting bonus.");
				} else {
					ShowPopOfDescription ("Already voted for this pair.", () => NextPair ());
				}
				Player1Voting_Button.interactable = true;
				Player2Voting_Button.interactable = true;
				yield return false;
			}
		} else {
			Player1Voting_Button.interactable = true;
			Player2Voting_Button.interactable = true;
			yield return false;
		}
	}

	#endregion

	IEnumerator VoteForPlayer_CatWalk (int votedToId, int eventId, int pairId, bool isfriend)
	{
		///TODO: change for all events
		int vote = 1;
		int friendBous = 0;
		int votingBonus = 0;

		if (isfriend) {
			friendBous = 1;
			votingBonus = 0;
		} else if (votedToId == PlayerPrefs.GetInt ("PlayerId")) {
			vote = 0;
			friendBous = 0;
			votingBonus = 1;
		}

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();
		jsonElement ["data_type"] = "save_voted_result";
		jsonElement ["player_id"] = votedToId.ToString ();
		jsonElement ["event_id"] = eventId.ToString ();
		jsonElement ["votedby_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();
		jsonElement ["vote_count"] = vote.ToString ();
		jsonElement ["vote_score"] = "0";

		///TODO: apply the vote bonus and friend bonus for the game

		jsonElement ["vote_bonus"] = votingBonus.ToString ();
		jsonElement ["friend_bonus"] = friendBous.ToString ();


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/catwalkRegistration", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");
				if (votingBonus > 0) {
					ShowPopOfDescription ("Voting bonus applied", () => StartCoroutine (GetVotesResult_CatWalk (PlayerPrefs.GetInt ("PlayerId"), eventId, SelectedPair_CatWalk.pair_id)));
					PersistanceManager.Instance.DeleteThisVotingBonus (eventId);
				} else {
					
					if (GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id) == 9) {	
						GenrateVotingBonus (EventManagment.Instance.CurrentEvent.Event_id, eventType.CatWalk_Event);
						ShowPopOfDescription ("Your vote saved successfully.\n You have gained a new Voting Bonus. You have used your all 10 votes for this Event. You can use this bonus to boost your scores in any event within 2 hours.", () => NextPair ());
					} else {				
						ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()), () => NextPair ());
					}

					IncreamentVoteCount (EventManagment.Instance.CurrentEvent.Event_id);
					if (isfriend) {
						if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
							AchievementsManager.Instance.CheckAchievementsToUpdate ("voteForFriends");
					}
				}
				//				Invoke ("NextPair", 0.6f);

				yield return true;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				if (votingBonus > 0) {
					ShowPopOfDescription ("Already applied a voting bonus.");
				} else {
					ShowPopOfDescription ("Already voted for this pair.", () => NextPair ());
				}
			
				yield return false;
			}
		} else {			
			yield return false;
		}
		Player1Voting_Button.interactable = true;
		Player2Voting_Button.interactable = true;
	}

	/// <summary>
	/// Votes for player in decore decor Event. "isVoting" is only to be send "true" if player is voting else "false".
	/// </summary>
	/// <returns>The for player decore.</returns>
	/// <param name="isVoting">If set to <c>true</c> is voting.</param>
	/// <param name="FriendBonus">Friend bonus.</param>
	/// <param name="VotingBonus">Voting bonus.</param> 
	IEnumerator VoteForPlayer_Decore (int votedToId, int eventId, int pairId, bool isfriend)
	{
		int vote = 1;
		int friendBous = 0;
		int votingBonus = 0;

		if (isfriend) {
			friendBous = 1;
			votingBonus = 0;
		} else if (votedToId == PlayerPrefs.GetInt ("PlayerId")) {
			vote = 0;
			friendBous = 0;
			votingBonus = 1;
		}

		var encoding = new System.Text.UTF8Encoding ();
		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = votedToId.ToString ();
		jsonElement ["event_id"] = eventId.ToString ();
		jsonElement ["votedby_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();
		jsonElement ["vote_count"] = vote.ToString ();
		jsonElement ["vote_score"] = "0";
		jsonElement ["vote_bonus"] = votingBonus.ToString ();
		jsonElement ["friend_bonus"] = friendBous.ToString ();


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/decor_updatevotedresult", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");
				if (votingBonus > 0) {
					ShowPopOfDescription ("Voting bonus applied", () => GetVotingResult_Decor (PlayerPrefs.GetInt ("PLayerId"), SelectedPair_Decor.event_id, SelectedPair_Decor.pair_id));
					PersistanceManager.Instance.DeleteThisVotingBonus (eventId);
				} else {
					if (GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id) == 9) {	
						GenrateVotingBonus (EventManagment.Instance.CurrentEvent.Event_id, eventType.Decor_Event);
						ShowPopOfDescription ("Your vote saved successfully.\n You have gained a new Voting Bonus. You have used your all 10 votes for this Event. You can use this bonus to boost your scores in any event within 2 hours.", () => NextPair ());
					} else {				
						ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()), () => NextPair ());
					}
					//Check if This condiction Work for display Voting
					GetVotingResult_Decor (PlayerPrefs.GetInt ("PlayerId"), eventId, pairId);
					IncreamentVoteCount (EventManagment.Instance.CurrentEvent.Event_id);
					if (isfriend) {
						if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
							AchievementsManager.Instance.CheckAchievementsToUpdate ("voteForFriends");
					}
				}

				yield return true;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				if (votingBonus > 0) {
					ShowPopOfDescription ("Already applied a voting bonus.");
				} else {
					ShowPopOfDescription ("Already voted for this pair.", () => NextPair ());
				}
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	#region Voting Result for FashionEvent

	void GetVotingResult_Fashion (int playerId, int flatmateId, int pairId)
	{
		StartCoroutine (GetVotesResult_Fashion (playerId, flatmateId, pairId, EventManagment.Instance.CurrentEvent.Event_id, false));
	}

	public IEnumerator GetVotesResult_Fashion (int playerId, int flatmateId, int pairId, int EventId, bool isShowingResult)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = playerId.ToString ();
		jsonElement ["event_id"] = EventId.ToString ();
		jsonElement ["flatmate_id"] = flatmateId.ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/fashionshow_getvotedresult", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting result data is following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode data = _jsnode ["data"];

				float totalvotes = 0.0f;
				float.TryParse (data ["total_pair_vote"], out totalvotes);


				int Player1Id;
				int.TryParse (data ["first_player_id"], out Player1Id);
				int Player2Id;
				int.TryParse (data ["second_player_id"], out Player2Id);

				float p1VotesCount = 0.0f;
				float.TryParse (data ["first_player_vote_data"] ["votecount"], out p1VotesCount);

				float p2VotesCount = 0.0f;
				float.TryParse (data ["second_player_vote_data"] ["votecount"], out p2VotesCount);

				int P1VoteBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["votebonus"], out P1VoteBonus);
				int P1FriendBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["friendbonus"], out P1FriendBonus);


				int P2VoteBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["votebonus"], out P2VoteBonus);
				int P2FriendBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["friendbonus"], out P2FriendBonus);


				if (totalvotes != 0f) {

					player1voteCount.text = p1VotesCount.ToString ();
					player2voteCount.text = p2VotesCount.ToString ();

				} else {
					player1voteCount.text = 0 + "";
					player2voteCount.text = 0 + "";
				}		

				player1votingBonus.text = P1VoteBonus.ToString ();
				player2votingBonus.text = P2VoteBonus.ToString ();

				player1FriendBonus.text = P1FriendBonus.ToString ();
				player2FriendBonus.text = P2FriendBonus.ToString ();

				player1ScoreText.text = "" + (p1VotesCount * 10 + P1VoteBonus + P1FriendBonus);
				player2ScoreText.text = "" + (p2VotesCount * 10 + P2VoteBonus + P2FriendBonus);



				if (isShowingResult) { 			
					ScreenAndPopupCall.Instance.ResultPanelClose ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (true);
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.RemoveAllListeners ();
					int P1Total = Mathf.RoundToInt (p1VotesCount * 10) + P1VoteBonus + P1FriendBonus;
					int P2Total = Mathf.RoundToInt (p2VotesCount * 10) + P2VoteBonus + P2FriendBonus;




					if (PlayerPrefs.GetInt ("PlayerId") == Player1Id) { // If i am player 1 and I won
						if (P1Total > P2Total) {
							/// Player Get Mex chance of wining the prize on fashion Event
							GameObject FlatMate = null;
							for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
								if (RoommateManager.Instance.RoommatesHired [i].gameObject.GetComponent<Flatmate> ().data.Id == flatmateId) {
									FlatMate = RoommateManager.Instance.RoommatesHired [i].gameObject;
								}
							}
							float randomMaxValue = 10f;
							if (FlatMate.GetComponent<Flatmate> ().data.Perk == "Prize Bonus") {	

								randomMaxValue = randomMaxValue - FlatMate.GetComponent<Flatmate> ().PerkValue;
							}

							float Value = UnityEngine.Random.Range (0.0f, randomMaxValue);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));

                      

						} else if (P1Total < P2Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, EventId, 0)));
						} else {

							float Value = UnityEngine.Random.Range (1.0f, 10f);

							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));		
						}
					} else if (PlayerPrefs.GetInt ("PlayerId") == Player2Id) {// If i am player 2 and I won
						if (P2Total > P1Total) {

							/// Player Get Mex chance of wining the prize on fashion Event
							GameObject FlatMate = null;
							for (int i = 0; i < RoommateManager.Instance.RoommatesHired.Length; i++) {
								if (RoommateManager.Instance.RoommatesHired [i].gameObject.GetComponent<Flatmate> ().data.Id == flatmateId) {
									FlatMate = RoommateManager.Instance.RoommatesHired [i].gameObject;
								}
							}
							float randomMaxValue = 10f;
							if (FlatMate.GetComponent<Flatmate> ().data.Perk == "Prize Bonus") {	

								randomMaxValue = randomMaxValue - FlatMate.GetComponent<Flatmate> ().PerkValue;
							}								

							float Value = UnityEngine.Random.Range (0.0f, randomMaxValue);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));
						} else if (P2Total < P1Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, EventId, 0)));
						} else {

							float Value = UnityEngine.Random.Range (1.0f, 10f);

							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));	
						}
					}
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Vote_Count").GetComponent<Text> ().text = P1Total.ToString ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Vote_Count").GetComponent<Text> ().text = P2Total.ToString ();

				}
				yield return true;
			} else {
				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	IEnumerator GainRewards (bool Won, int EventId, float Value)
	{
		int Clicked = 0;
		if (Won) {
            if (Value < 10f)
            {
                var cd = new CoroutineWithData(SeasonalClothesManager.Instance, SeasonalClothesManager.Instance.GetRandomSeasonalClothesAndSaveIt());

                yield return cd.coroutine;
                if (cd.result != null)
                {
                    var boolean = (bool)cd.result;
                    if (boolean)
                    {
                        ShowPopUpWithOnClickOkFunction("You gained a new Seasonal Cloth", () =>
                            {
                                Clicked = 1;
                            });
                    }
                    else
                        Clicked = 1;                
            }

                yield return new WaitUntil (() =>
                        Clicked == 1);
            }
			if (Value < 0f) {

				yield return StartCoroutine (EventManagment.Instance.GetAllEventsForCheck ());
				yield return new WaitForSeconds (0.1f);
				var link = GetLinkOfRewards (EventId);

				ShowPopUpWithOnClickOkFunction ("You gained a reward", () => {
					EventManagment.Instance.ClaimReward (EventId, link, EventManagment.Instance.CurrentEvent.EventName);
					if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
						AchievementsManager.Instance.CheckAchievementsToUpdate ("winUniversityEvents");
					Clicked = 2;
				});		
				yield return new WaitUntil (() =>
					Clicked == 2);
			}

			if (Value < 5f) {
				yield return new WaitForSeconds (0.2f);
				ShowPopUpWithOnClickOkFunction ("You Gained 5 Gems", () => {
					GameManager.Instance.AddGemsWithGemBonus (5);
					Clicked = 3;
				});		
				yield return new WaitUntil (() => Clicked == 3);
			}
		}
		yield return new WaitForSeconds (0.2f);
		ShowPopUpWithOnClickOkFunction ("You Gained 500 coins", () => { 
			GameManager.Instance.AddCoins (500);
			Clicked = 4;
		});		
      
		PrizeClaimed = true;
		ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (false);
	}

	string GetLinkOfRewards (int eventId)
	{
		foreach (var _event in EventManagment.Instance.AllEventList) {
			if (_event.Event_id == eventId)
				return _event.RewardValue [0];
		}
		return string.Empty;
	}

	#endregion



	#region Voting Result for DecorEvent

	//
	void GetVotingResult_Decor (int playerId, int event_id, int pairId)
	{
		StartCoroutine (VoteForPlayer_Decor (playerId, event_id, pairId, false));
	}

	public IEnumerator VoteForPlayer_Decor (int playerId, int event_id, int pairId, bool isShowingResult)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = playerId.ToString ();

		jsonElement ["event_id"] = event_id.ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();
		//		jsonElement ["votedby_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		//		jsonElement ["vote_count"] = "1";
		//		jsonElement ["vote_score"] = "0";
		//		jsonElement ["vote_bonus"] = "0";
		//		jsonElement ["friend_bonus"] = "0";


		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/decor_getvotedresult", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());

			if (_jsnode ["description"].ToString ().Contains ("Voting result data is following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode data = _jsnode ["data"];

				//				{
				//					"total_pair_vote": 1,
				//					"first_player_id": "81",
				//					"first_player_vote": 0,
				//					"first_player_vote_data": {
				//						"votecount": "",
				//						"votescore": "",
				//						"votebonus": "",
				//						"friendbonus": ""
				//					},
				//					"second_player_id": "71",
				//					"second_player_vote": 1,
				//					"second_player_vote_data": {
				//						"votecount": "1",
				//						"votescore": "0",
				//						"votebonus": "0",
				//						"friendbonus": "0"
				//					}
				//				}
				float totalvotes = 0.0f;
				float.TryParse (data ["total_pair_vote"], out totalvotes);


				int Player1Id;
				int.TryParse (data ["first_player_id"], out Player1Id);
				int Player2Id;
				int.TryParse (data ["second_player_id"], out Player2Id);

				float p1VotesCount = 0.0f;
				float.TryParse (data ["first_player_vote_data"] ["votecount"], out p1VotesCount);

				float p2VotesCount = 0.0f;
				float.TryParse (data ["second_player_vote_data"] ["votecount"], out p2VotesCount);

				int P1VoteBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["votebonus"], out P1VoteBonus);
				int P1FriendBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["friendbonus"], out P1FriendBonus);


				int P2VoteBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["votebonus"], out P2VoteBonus);
				int P2FriendBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["friendbonus"], out P2FriendBonus);


				if (totalvotes != 0f) {

					player1voteCount.text = p1VotesCount.ToString ();
					player2voteCount.text = p2VotesCount.ToString ();

				} else {
					player1voteCount.text = 0 + "";
					player2voteCount.text = 0 + "";
				}		

				player1votingBonus.text = P1VoteBonus.ToString ();
				player2votingBonus.text = P2VoteBonus.ToString ();

				player1FriendBonus.text = P1FriendBonus.ToString ();
				player2FriendBonus.text = P2FriendBonus.ToString ();

				player1ScoreText.text = "" + (p1VotesCount * 10 + P1VoteBonus + P1FriendBonus);
				player2ScoreText.text = "" + (p2VotesCount * 10 + P2VoteBonus + P2FriendBonus);



				if (isShowingResult) { 		
					ScreenAndPopupCall.Instance.ResultPanelClose ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.RemoveAllListeners ();
					int P1Total = Mathf.RoundToInt (p1VotesCount) + P1VoteBonus + P1FriendBonus;
					int P2Total = Mathf.RoundToInt (p2VotesCount) + P2VoteBonus + P2FriendBonus;

					if (PlayerPrefs.GetInt ("PlayerId") == Player1Id) { // If i am player 1 and I won
						if (P1Total > P2Total) {
							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));
						} else if (P1Total < P2Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, event_id, 0)));
						} else if (P1Total == P2Total) {
							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));		
						}
					} else if (PlayerPrefs.GetInt ("PlayerId") == Player2Id) {// If i am player 2 and I won
						if (P2Total > P1Total) {
							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));
						} else if (P2Total < P1Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, event_id, 0)));
						} else if (P2Total == P1Total) {
							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));	
						}
					}
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Vote_Count").GetComponent<Text> ().text = P1Total.ToString ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Vote_Count").GetComponent<Text> ().text = P2Total.ToString ();

				}
				yield return true;
			} else {
				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}



	void GetVotingResult_CatWalk (int playerId, int event_id, int pairId)
	{
		StartCoroutine (GetVotesResult_CatWalk (playerId, event_id, pairId));
	}

	IEnumerator GetVotesResult_CatWalk (int playerId, int event_id, int pairId)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = playerId.ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();

		jsonElement ["pair_id"] = pairId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/catwalkGetVotingResult", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting result data is following") || _jsnode ["status"].ToString ().Contains ("200")) {

				print ("Success");

				JSONNode data = _jsnode ["data"];
				float p1VotesCount = 0.0f;
				float.TryParse (data ["first_player_vote_data"] ["votecount"], out p1VotesCount);

				float p2VotesCount = 0.0f;
				float.TryParse (data ["second_player_vote_data"] ["votecount"], out p2VotesCount);


				int P1VoteBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["votebonus"], out P1VoteBonus);
				int P1FriendBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["friendbonus"], out P1FriendBonus);


				float totalvotes = 0.0f;
				float.TryParse (data ["total_pair_vote"], out totalvotes);



				int P2VoteBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["votebonus"], out P2VoteBonus);
				int P2FriendBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["friendbonus"], out P2FriendBonus);

				if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.RewardScreen) {
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 1").FindChild ("Friend_bonus").GetComponent<Text> ().text = P1FriendBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 2").FindChild ("Friend_bonus").GetComponent<Text> ().text = P2FriendBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 1").FindChild ("Voting_bonus").GetComponent<Text> ().text = P1VoteBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 2").FindChild ("Voting_bonus").GetComponent<Text> ().text = P2VoteBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 1").FindChild ("Vote_Count").GetComponent<Text> ().text = p1VotesCount.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 2").FindChild ("Vote_Count").GetComponent<Text> ().text = p2VotesCount.ToString ();
				} else {
					if (totalvotes != 0f) {

						player1voteCount.text = p1VotesCount.ToString ();
						player2voteCount.text = p2VotesCount.ToString ();

						player1votingBonus.text = P1VoteBonus.ToString ();
						player2votingBonus.text = P2VoteBonus.ToString ();

						player1FriendBonus.text = P1FriendBonus.ToString ();
						player2FriendBonus.text = P2FriendBonus.ToString ();

					} else {
						player1voteCount.text = 0 + "";
						player2voteCount.text = 0 + "";
					}

					player1ScoreText.text = "" + (p1VotesCount * 10 + P1VoteBonus + P1FriendBonus);
					player2ScoreText.text = "" + (p2VotesCount * 10 + P2VoteBonus + P2FriendBonus);

				}
				yield return true;
			} else {
				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	#endregion


	void ShowPopOfDescription (string Message, UnityEngine.Events.UnityAction OnClickOkAction = null)
	{
		ScreenManager.Instance.ClosePopup ();

		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);

		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();

		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";

		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = Message;
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => ScreenManager.Instance.ClosePopup ());	
		if (OnClickOkAction != null)
			ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => OnClickOkAction ());	
	}

	void IncreamentVoteCount (int EventId)
	{
		if (PlayerPrefs.HasKey ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId"))) {
			var Temptime = Convert.ToInt64 (PlayerPrefs.GetString ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId")));
			DateTime RefreshTime = DateTime.FromBinary (Temptime);

			if (RefreshTime > DateTime.UtcNow) {
				var Value = GetVoteCountsForCurrentEvent (EventId);
				PlayerPrefs.SetInt ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId"), Value + 1);
			} else if (RefreshTime < DateTime.UtcNow) {
				PlayerPrefs.SetInt ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId"), 0);
			} else if (RefreshTime < DateTime.UtcNow) {
				PlayerPrefs.SetInt ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId"), 0);
			}
		} else {
			var Value = GetVoteCountsForCurrentEvent (EventId);
			PlayerPrefs.SetInt ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId"), Value + 1);
		}

		// delete old timer..
		PlayerPrefs.DeleteKey ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId"));

		PlayerPrefs.SetString ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId"), DateTime.UtcNow.AddMinutes (20).ToBinary ().ToString ());//AddHours (2).ToBinary ().ToString ());

		//run timer of votes refreshing

		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").GetComponentInChildren<Text> ().text = "VOTES LEFT \n" + (10 - GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id));
	}

	public bool CheckVotingBonus (int EventId)
	{
		if (GetVoteCountsForCurrentEvent (EventId) < 10) {
			return true;
		} else {
			if (PlayerPrefs.HasKey ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId"))) {
				var Temptime = Convert.ToInt64 (PlayerPrefs.GetString ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId")));
				DateTime RefreshTime = DateTime.FromBinary (Temptime);

				if (RefreshTime > DateTime.UtcNow) {
					return false;
				} else {
					return true;
				}
			} else {
				return true;
			}
		}
	}

	int GetVoteCountsForCurrentEvent (int EventId)
	{

		if (PlayerPrefs.HasKey ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId"))) {
			var Temptime = Convert.ToInt64 (PlayerPrefs.GetString ("VotingRefreshTime" + EventId + PlayerPrefs.GetInt ("PlayerId")));
			DateTime RefreshTime = DateTime.FromBinary (Temptime);

			if (RefreshTime > DateTime.UtcNow) {
				return  PlayerPrefs.GetInt ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId"), 0);
			} else if (RefreshTime < DateTime.UtcNow) {
				return 0;
			}
		} else {
			if (PlayerPrefs.HasKey ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId")))
				return 	PlayerPrefs.GetInt ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId"), 0);
			else {
				PlayerPrefs.SetInt ("VotedCount" + EventId + PlayerPrefs.GetInt ("PlayerId"), 0);
				return 0;
			}
		}
		return 0;
	}

	void GenrateVotingBonus (int id, eventType eventId)
	{
		VotingBonus Vb = new VotingBonus ();
		Vb.Id = id;
		Vb.Type = eventId;
		Vb.DestroyTime = DateTime.UtcNow.AddMinutes (20);//AddHours (2);
		allBonuses.Clear ();
		allBonuses = PersistanceManager.Instance.GetSavedVotingBonuses ();
		List <VotingBonus> Temp = PersistanceManager.Instance.GetSavedVotingBonuses ();
		Temp.Add (Vb);
		PersistanceManager.Instance.SaveAllVotingBonus (Temp);
		InstantiateBonuses (Vb);
	}


	public void ShowAllVotingBonus ()
	{
		allBonuses.Clear ();
		DeleteAllVotingBonus ();
		allBonuses = PersistanceManager.Instance.GetSavedVotingBonuses ();

		if (allBonuses.Count >= 1) {
			for (int i = 0; i < allBonuses.Count; i++) {
				GameObject obj = (GameObject)Instantiate (ApplybonusButton, Vector3.zero, Quaternion.identity);
				obj.transform.SetParent (ApplybonusContainer.transform);
				obj.transform.localScale = Vector3.one;
				obj.transform.GetChild (0).GetComponent<Text> ().text = allBonuses [i].Type + "_Bonus";
				var bonus = allBonuses [i];
				switch (SelectedEventType) {
				case "Fashion":
					if (PlayerPrefs.GetInt ("PlayerId") == SelectedPair_Fashion.player1_id) {
						obj.GetComponent <Button> ().onClick.AddListener (() => StartCoroutine (VoteForPlayer (PlayerPrefs.GetInt ("PlayerId"), SelectedPair_Fashion.player1_flatmate_id, SelectedPair_Fashion.pair_id, false)));
					} else if (PlayerPrefs.GetInt ("PlayerId") == SelectedPair_Fashion.player2_id) {
						obj.GetComponent <Button> ().onClick.AddListener (() => StartCoroutine (VoteForPlayer (PlayerPrefs.GetInt ("PlayerId"), SelectedPair_Fashion.player2_flatmate_id, SelectedPair_Fashion.pair_id, false)));
					}
					break;
				case "CatWalk":
					obj.GetComponent <Button> ().onClick.AddListener (() => StartCoroutine (VoteForPlayer_CatWalk (PlayerPrefs.GetInt ("PlayerId"), SelectedPair_CatWalk.event_id, SelectedPair_CatWalk.pair_id, false)));
					break;
				case "Decor":
					obj.GetComponent <Button> ().onClick.AddListener (() => StartCoroutine (VoteForPlayer_Decore (PlayerPrefs.GetInt ("PlayerId"), SelectedPair_Decor.event_id, SelectedPair_Decor.pair_id, false)));
					break;
				case "CoOp":
					obj.GetComponent <Button> ().onClick.AddListener (() => { 
						var myId = PlayerPrefs.GetInt ("PlayerId");
						if (SelectedPair_CoOp.set1_player1_id == myId || SelectedPair_CoOp.set1_player2_id == myId)
							StartCoroutine (VoteForPlayerCoOp (SelectedPair_CoOp.set1_player1_id, SelectedPair_CoOp.set1_player2_id, SelectedPair_CoOp.pair_id, false));
						else if (SelectedPair_CoOp.set2_player1_id == myId || SelectedPair_CoOp.set2_player2_id == myId)
							StartCoroutine (VoteForPlayerCoOp (SelectedPair_CoOp.set2_player1_id, SelectedPair_CoOp.set2_player2_id, SelectedPair_CoOp.pair_id, false));
					});
					break;
				case "SocietyEvent":
					obj.GetComponent <Button> ().onClick.AddListener (() => {
						var myId = PlayerPrefs.GetInt ("PlayerId");
						if (SelectedPair_SocietyEvent.player1_id == myId)
							StartCoroutine (VoteInSocietyEvent (myId, SelectedPair_SocietyEvent.player1societyId, SelectedPair_SocietyEvent.player1_flatmate_id, SelectedPair_SocietyEvent.pair_id, false));
						else if (SelectedPair_SocietyEvent.player2_id == myId)
							StartCoroutine (VoteInSocietyEvent (myId, SelectedPair_SocietyEvent.player2societyId, SelectedPair_SocietyEvent.player2_flatmate_id, SelectedPair_SocietyEvent.pair_id, false));
					});
					break;
				}
				obj.GetComponent <Button> ().onClick.AddListener (() => { 
					Destroy (obj);
					PersistanceManager.Instance.DeleteThisVotingBonus (EventManagment.Instance.CurrentEvent.Event_id);
					ScreenAndPopupCall.Instance.MoveBonusScreenBack ();

					if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
						ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
					else
						ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
				});
			}
		} else {
			GameObject obj = (GameObject)Instantiate (ApplybonusButton, Vector3.zero, Quaternion.identity);
			obj.transform.SetParent (ApplybonusContainer.transform);
			obj.transform.localScale = Vector3.one;	

			obj.transform.GetChild (0).GetComponent<Text> ().text = "No Bonus Available";
			obj.transform.GetChild (1).gameObject.SetActive (false);
		}

	}

	public void DeleteAllVotingBonus ()
	{
		for (int i = 0; i < ApplybonusContainer.transform.childCount; i++) {
			Destroy (ApplybonusContainer.transform.GetChild (i).gameObject);
		}
	}


	void InstantiateBonuses (VotingBonus Vb)
	{
		GameObject TimerGameObject = new GameObject ();
		TimerGameObject.AddComponent <VotingBonusTimer> ().Bonus = Vb;
	}

	bool HasPairInFriendList (int Id)
	{
		foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
			if (friend.Id == Id) {
				return true;
			} 
		}
		return false;
	}

	bool HasPairInFriendList (int Id, int Id2)
	{
		foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
			if (friend.Id == Id || friend.Id == Id2) {
				return true;
			} 
		}
		return false;
	}

	bool isAlReadyAddedCoOp (int pairId)
	{
		foreach (var friends in FriendsInCoOp) {
			if (friends.pair_id == pairId)
				return true;
		}
		return false;
	}

	bool isAlReadyAddedFashion (int PairId)
	{
		foreach (var friends in FriendsInFashionEvent) {
			if (friends.pair_id == PairId)
				return true;
		}
		return false;
	}

	bool isAlReadyAddedDecore (int PairId)
	{
		foreach (var friends in FriendsInDecorEvent) {
			if (friends.pair_id == PairId)
				return true;
		}
		return false;
	}

	bool isAlReadyAddedCatwalk (int PairId)
	{
		foreach (var friends in FriendsInCatwalkEvent) {
			if (friends.pair_id == PairId)
				return true;
		}
		return false;
	}

	void ShowPopUpWithOnClickOkFunction (string message, UnityEngine.Events.UnityAction OnClickActions = null)
	{				
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.News.transform.FindChild ("close").GetComponent<Button> ().onClick.RemoveAllListeners ();
		ScreenManager.Instance.ClosePopup ();
		ScreenManager.Instance.ShowPopup (ScreenManager.Instance.News);

		ScreenManager.Instance.News.transform.FindChild ("ok").gameObject.SetActive (true);
		ScreenManager.Instance.News.transform.FindChild ("close").gameObject.SetActive (false);
		ScreenManager.Instance.News.transform.FindChild ("ok").GetChild (0).GetComponent<Text> ().text = "Ok";
		ScreenManager.Instance.News.transform.FindChild ("close").GetChild (0).GetComponent<Text> ().text = "Close";
		ScreenManager.Instance.News.transform.FindChild ("Message").GetComponent<Text> ().text = message;	
		ScreenManager.Instance.News.transform.FindChild ("ok").GetComponent<Button> ().onClick.AddListener (() => {
			ScreenManager.Instance.ClosePopup ();
			if (OnClickActions != null)
				OnClickActions ();
		});
	}


    public void EventBackGround (GameObject Screen)
	{
        switch (EventManagment.Instance.category) {
		case eventType.Fashion_Event:
                Screen.GetComponent<Image> ().sprite = _fashionShowBackGround;
			break;
		case eventType.CoOp_Event:
                Screen.GetComponent<Image> ().sprite = _coOpBackGround;
			break;
		case eventType.CatWalk_Event:
                Screen.GetComponent<Image> ().sprite = _catWalkBackGround;
			break;
		case eventType.Decor_Event:
                Screen.GetComponent<Image> ().sprite = _decorBackGround;
			break;
		case eventType.Society_Event:
                Screen.GetComponent<Image> ().sprite = _societyBackGround;
			break;
		default:
                Screen.GetComponent<Image> ().sprite = _fashionShowBackGround;
			break;
		}
	}


	#region GetAllVotingPair_coOp

	public void GetAllVotingPair_coOp (bool isForNextPair)
	{
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = false;
		SelectedEventType = "CoOp";
		StartCoroutine (GetAllVotingPairs_CoOp (isForNextPair));
	}

	IEnumerator GetAllVotingPairs_CoOp (bool isForNextPair)
	{
		FriendsInCoOp.Clear ();
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;


		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["page_no"] = pairIndex.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/coopGetVotingPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode dataArray = _jsnode ["data"];
				CoOpVotingPair Vp = new CoOpVotingPair ();

				for (int i = 0; i < dataArray.Count; i++) {
					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					/// set 1 data 
					/// Player 1 data
					Vp.set1_player1_id = int.Parse (dataArray [i] ["set1_player1_id"]);
					Vp.set1_player1_flatmate_id = int.Parse (dataArray [i] ["set1_player1_flatmate_id"]);
					Vp.set1_player1Name = dataArray [i] ["set1_player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set1_P1DressItems = dataArray [i] ["set1_player1_items"];


					for (int a = 0; a < set1_P1DressItems.Count; a++) {
						var itemId = 0;
						int.TryParse (set1_P1DressItems [a] ["item_id"], out itemId);

						var itemCat = set1_P1DressItems [a] ["item_type"];

						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set1_player1Gender = "Male";
							else
								Vp.set1_player1Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set1_player1Skin = itemId;
						} else {
							if (Vp.set1_player1_dressData.ContainsKey (itemId))
								Vp.set1_player1_dressData [itemId] = itemCat;
							else
								Vp.set1_player1_dressData.Add (itemId, itemCat);
						}
					}
					///player2 data
					Vp.set1_player2_id = int.Parse (dataArray [i] ["set1_player2_id"]);
					Vp.set1_player2_flatmate_id = int.Parse (dataArray [i] ["set1_player2_flatmate_id"]);
					Vp.set1_player2Name = dataArray [i] ["set1_player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set1_P2DressItems = dataArray [i] ["set1_player2_items"];


					for (int a = 0; a < set1_P2DressItems.Count; a++) {
						var itemId = int.Parse (set1_P2DressItems [a] ["item_id"]);
						var itemCat = set1_P2DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set1_player2Gender = "Male";
							else
								Vp.set1_player2Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set1_player2Skin = itemId;
						} else {
							if (Vp.set1_player2_dressData.ContainsKey (itemId))
								Vp.set1_player2_dressData [itemId] = itemCat;
							else
								Vp.set1_player2_dressData.Add (itemId, itemCat);
						}
					}

					/// set 2 data 
					/// Player 1 data
					Vp.set2_player1_id = int.Parse (dataArray [i] ["set2_player1_id"]);
					Vp.set2_player1_flatmate_id = int.Parse (dataArray [i] ["set2_player1_flatmate_id"]);
					Vp.set2_player1Name = dataArray [i] ["set2_player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set2_P1DressItems = dataArray [i] ["set2_player1_items"];


					for (int a = 0; a < set2_P1DressItems.Count; a++) {
						var itemId = int.Parse (set2_P1DressItems [a] ["item_id"]);
						var itemCat = set2_P1DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set2_player1Gender = "Male";
							else
								Vp.set2_player1Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set2_player1Skin = itemId;
						} else {
							if (Vp.set2_player1_dressData.ContainsKey (itemId))
								Vp.set2_player1_dressData [itemId] = itemCat;
							else
								Vp.set2_player1_dressData.Add (itemId, itemCat);
						}
					}

					///player2 data
					Vp.set2_player2_id = int.Parse (dataArray [i] ["set2_player2_id"]);
					Vp.set2_player2_flatmate_id = int.Parse (dataArray [i] ["set2_player2_flatmate_id"]);
					Vp.set2_player2Name = dataArray [i] ["set2_player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set2_P2DressItems = dataArray [i] ["set2_player2_items"];


					for (int a = 0; a < set2_P2DressItems.Count; a++) {
						var itemId = int.Parse (set2_P2DressItems [a] ["item_id"]);
						var itemCat = set2_P2DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set2_player2Gender = "Male";
							else
								Vp.set2_player2Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set2_player2Skin = itemId;
						} else {
							if (Vp.set2_player2_dressData.ContainsKey (itemId))
								Vp.set2_player2_dressData [itemId] = itemCat;
							else
								Vp.set2_player2_dressData.Add (itemId, itemCat);
						}
					}
				}// for loop ends here


                StartCoroutine(ShowPairCoOp (Vp));

				if (!isForNextPair) {
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();
				}


//				var TempList = FindPairInFriendList (AllPairsInCoOp);
//
//				foreach (var pair in TempList) {
//					if (!isAlReadyAddedCoOp (pair.pair_id))
//						FriendsInCoOp.Add (pair);
//				}
//
//
//				if (viewFriends) {
//					if (FriendsInCoOp.Count != 0) {
////						ScreenAndPopupCall.Instance.CloseScreen ();
//						ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
//						ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
//						var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();
//						ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";
//
//						for (int i = 0; i < container.transform.childCount; i++) {
//							Destroy (container.transform.GetChild (i).gameObject);
//						}
//
//						FriendsInCoOp.ForEach (pair => {	
//							foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
//								if (friend.Id == pair.set1_player1_id || friend.Id == pair.set1_player2_id || friend.Id == pair.set2_player1_id || friend.Id == pair.set2_player2_id) {
//									GameObject Go = Instantiate (FriendsManager.Instance.FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
//									//										ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsScreen);
//									Go.transform.parent = container.transform;
//									Go.transform.localScale = Vector3.one;
//									Go.transform.localPosition = Vector3.zero;
//									Go.GetComponent <AddFriendUi> ().thisData = friend;
//									Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.ViewFriend;
//									Go.name = friend.Username;
//									Go.GetComponent <AddFriendUi> ().ViewFriendInString = "CoOp";
//								}
//							}
//						});
//					} else
//						ShowPopOfDescription ("You do not have any of your friends participating in this event.", false);
//				} else {				
//		
//					GetDataOfCoOpPair ();
//				}


				yield return true;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				if (isForNextPair) {
					pairIndex = 0;
					ShowPopOfDescription ("No pairs to display", () => NextPair ());
				} else
					ShowPopOfDescription ("Currently no pair have registered for this Event");	
			} else {
				print ("error" + _jsnode ["description"].ToString ());

				yield return false;

			}
		} else {
			yield return false;
		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
	}

	//	void GetDataOfCoOpPair ()
	//	{
	//		if (AllPairsInCoOp.Count != 0) {
	//
	//			if (AllPairsInCoOp.Count == 1)
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
	//			else
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
	//
	//			pairIndex = 1;
	//			CoOpVotingPair Pair = AllPairsInCoOp [pairIndex];
	//			ShowPairCoOp (Pair);
	//
	//			ScreenAndPopupCall.Instance.CloseScreen ();
	//			ScreenAndPopupCall.Instance.VotingScreenSelection ();
	//
	//		} else {
	//			ShowPopOfDescription ("Currently no pair have registered for this event", false);
	//			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
	//		}
	//	}


	//	void GetDataOfMyPlayerCoOp ()
	//	{
	//		if (MyPlayersInCoOp.Count != 0) {
	//			if (MyPlayersInCoOp.Count == 1)
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);
	//			else
	//				ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
	//
	//			pairIndex = 1;
	//			CoOpVotingPair Pair = MyPlayersInCoOp [pairIndex];
	//			ShowMyPlayerCoOp (Pair);
	//
	//			ScreenAndPopupCall.Instance.CloseScreen ();
	//			ScreenAndPopupCall.Instance.VotingScreenSelection ();
	//		} else {
	//			ShowPopOfDescription ("Currently no pair have registered for this event", false);
	//			ScreenAndPopupCall.Instance.ResultButton.SetActive (false);
	//		}
	//	}

    public IEnumerator ShowPairCoOp (CoOpVotingPair pair)
	{
		SelectedPair_CoOp = pair;
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
		GetVotesResults_CoOp (pair.event_id, pair.pair_id);

		player1Name.text = pair.set1_player1Name + " / " + pair.set1_player2Name;
		player2Name.text = pair.set2_player1Name + " / " + pair.set2_player2Name;

		GameObject Player1_Char1 = null;
		GameObject Player1_Char2 = null;
		GameObject Player2_Char1 = null;
		GameObject Player2_Char2 = null;

		var previous = DressManager.Instance.dummyCharacter;

    
        var flatmate1 = FindFlatMateAvtar(pair.set1_player1_flatmate_id);
        if (flatmate1 != null)
        {
            Player1_Char1 = (GameObject)Instantiate(flatmate1, Vector3.zero, Quaternion.identity);
            Player1_Char1.GetComponent <CharacterProperties>().PlayerType = flatmate1.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set1_player1Gender == "Male")
                Player1_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player1_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char1, pair.set1_player1_id));
        }

        var flatmate2 = FindFlatMateAvtar(pair.set1_player2_flatmate_id);
        if (flatmate2 != null)
        {
            Player1_Char2 = (GameObject)Instantiate(flatmate2, Vector3.zero, Quaternion.identity);
            Player1_Char2.GetComponent <CharacterProperties>().PlayerType = flatmate2.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set1_player2Gender == "Male")
                Player1_Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player1_Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char2, pair.set1_player2_id));
        }


        var flatmate3 = FindFlatMateAvtar(pair.set2_player1_flatmate_id);
        if (flatmate3 != null)
        {
            Player2_Char1 = (GameObject)Instantiate(flatmate3, Vector3.zero, Quaternion.identity);
            Player2_Char1.GetComponent <CharacterProperties>().PlayerType = flatmate3.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set2_player1Gender == "Male")
                Player2_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player2_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char1, pair.set2_player1_id));
        }

        var flatmate4 = FindFlatMateAvtar(pair.set2_player2_flatmate_id);
        if (flatmate4 != null)
        {
            Player2_Char2 = (GameObject)Instantiate(flatmate4, Vector3.zero, Quaternion.identity);
            Player2_Char2.GetComponent <CharacterProperties>().PlayerType = flatmate4.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set2_player2Gender == "Male")
                Player2_Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player2_Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char2, pair.set2_player2_id));
        }

		Player1_Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Player1_Char1.transform.localScale = Vector3.one * 0.3f;
		Player1_Char1.transform.localPosition = new Vector3 (-0.5f, 0f, 0);

		foreach (var dress in pair.set1_player1_dressData) {
			int id = dress.Key;
			string cat = dress.Value;
			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char1;
//					if (pair.set1_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set1_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {
				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char1;
//					if (pair.set1_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set1_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}

		Player1_Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Player1_Char2.transform.localScale = Vector3.one * 0.3f;
		Player1_Char2.transform.localPosition = new Vector3 (0.5f, 0f, 0);


		foreach (var dress in pair.set1_player2_dressData) {
			int id = dress.Key;
			string cat = dress.Value;
			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char2;
//					if (pair.set1_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set1_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {
					
				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char2;
//					if (pair.set1_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set1_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}

		Player2_Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
		Player2_Char1.transform.localScale = Vector3.one * 0.3f;
		Player2_Char1.transform.localPosition = new Vector3 (-0.5f, 0f, 0);


		foreach (var dress in pair.set2_player1_dressData) {
			int id = dress.Key;
			string cat = dress.Value;

			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char1;
//					if (pair.set2_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set2_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {

				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char1;
//					if (pair.set2_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set2_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}

		Player2_Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
		Player2_Char2.transform.localScale = Vector3.one * 0.3f;
		Player2_Char2.transform.localPosition = new Vector3 (0.5f, 0f, 0);


		foreach (var dress in pair.set2_player2_dressData) {
			int id = dress.Key;
			string cat = dress.Value;
			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char2;
//					if (pair.set2_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set2_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {

				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char2;
//					if (pair.set2_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set2_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}


		int Layer = LayerMask.NameToLayer ("UI3D");

		Player1_Char1.SetLayerRecursively (Layer);
		Player1_Char2.SetLayerRecursively (Layer);
		Player2_Char1.SetLayerRecursively (Layer);
		Player2_Char2.SetLayerRecursively (Layer);

		DressManager.Instance.dummyCharacter = previous;

		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;

		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.565f, 0.0f, 0.41f, 0.8f);

		ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);

		ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (true);

		ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "WHO'S GOT THE BETTER OUTFIT?";

		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").GetComponentInChildren<Text> ().text = "VOTES LEFT \n" + (10 - GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id));
	}

    public IEnumerator ShowMyPlayerCoOp (CoOpVotingPair pair)
	{
		SelectedPair_CoOp = pair;
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
		GetVotesResults_CoOp (pair.event_id, pair.pair_id);

		player1Name.text = pair.set1_player1Name + " / " + pair.set1_player2Name;
		player2Name.text = pair.set2_player1Name + " / " + pair.set2_player2Name;

		GameObject Player1_Char1 = null;
		GameObject Player1_Char2 = null;
		GameObject Player2_Char1 = null;
		GameObject Player2_Char2 = null;

		var previous = DressManager.Instance.dummyCharacter;

        var flatmate1 = FindFlatMateAvtar(pair.set1_player1_flatmate_id);
        if (flatmate1 != null)
        {
            Player1_Char1 = (GameObject)Instantiate(flatmate1, Vector3.zero, Quaternion.identity);
            Player1_Char1.GetComponent <CharacterProperties>().PlayerType = flatmate1.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set1_player1Gender == "Male")
                Player1_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player1_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char1, pair.set1_player1_id));
        }

        var flatmate2 = FindFlatMateAvtar(pair.set1_player2_flatmate_id);
        if (flatmate2 != null)
        {
            Player1_Char2 = (GameObject)Instantiate(flatmate2, Vector3.zero, Quaternion.identity);
            Player1_Char2.GetComponent <CharacterProperties>().PlayerType = flatmate2.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set1_player2Gender == "Male")
                Player1_Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player1_Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player1_Char2, pair.set1_player2_id));
        }


        var flatmate3 = FindFlatMateAvtar(pair.set2_player1_flatmate_id);
        if (flatmate3 != null)
        {
            Player2_Char1 = (GameObject)Instantiate(flatmate3, Vector3.zero, Quaternion.identity);
            Player2_Char1.GetComponent <CharacterProperties>().PlayerType = flatmate3.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set2_player1Gender == "Male")
                Player2_Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player2_Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char1, pair.set2_player1_id));
        }

        var flatmate4 = FindFlatMateAvtar(pair.set2_player2_flatmate_id);
        if (flatmate4 != null)
        {
            Player2_Char2 = (GameObject)Instantiate(flatmate4, Vector3.zero, Quaternion.identity);
            Player2_Char2.GetComponent <CharacterProperties>().PlayerType = flatmate4.GetComponent <CharacterProperties>().PlayerType;
        }
        else
        {
            if (pair.set2_player2Gender == "Male")
                Player2_Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            else
                Player2_Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Player2_Char2, pair.set2_player2_id));
        }

		Player1_Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Player1_Char1.transform.localScale = Vector3.one * 0.3f;
		Player1_Char1.transform.localPosition = new Vector3 (-0.5f, 0f, 0);

		foreach (var dress in pair.set1_player1_dressData) {
			int id = dress.Key;
			string cat = dress.Value;
			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char1;
//					if (pair.set1_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set1_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {
				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char1;
//					if (pair.set1_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set1_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}

		Player1_Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Player1_Char2.transform.localScale = Vector3.one * 0.3f;
		Player1_Char2.transform.localPosition = new Vector3 (0.5f, 0f, 0);


		foreach (var dress in pair.set1_player2_dressData) {
			int id = dress.Key;
			string cat = dress.Value;
			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char2;
//					if (pair.set1_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set1_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {

				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player1_Char2;
//					if (pair.set1_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set1_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set1_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}

		Player2_Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
		Player2_Char1.transform.localScale = Vector3.one * 0.3f;
		Player2_Char1.transform.localPosition = new Vector3 (-0.5f, 0f, 0);


		foreach (var dress in pair.set2_player1_dressData) {
			int id = dress.Key;
			string cat = dress.Value;

			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char1;
//					if (pair.set2_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set2_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {

				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char1;
//					if (pair.set2_player1Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set2_player1Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player1Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}

		Player2_Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
		Player2_Char2.transform.localScale = Vector3.one * 0.3f;
		Player2_Char2.transform.localPosition = new Vector3 (0.5f, 0f, 0);


		foreach (var dress in pair.set2_player2_dressData) {
			int id = dress.Key;
			string cat = dress.Value;
			if (cat.Contains ("Hair")) {
				var mydress = FindSaloonWithId (id);
				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char2;
//					if (pair.set2_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
//					else if (pair.set2_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.HairImages);
				}
			} else {

				var mydress = FindDressWithId (id);

				if (mydress != null) {
					DressManager.Instance.dummyCharacter = Player2_Char2;
//					if (pair.set2_player2Skin == 1)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
//					else if (pair.set2_player2Skin == 2)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Brown_Images);
//					else if (pair.set2_player2Skin == 3)
//						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.Black_Images);
//					else
						DressManager.Instance.ChangeDressForDummyCharacter (mydress.PartName, mydress.DressesImages);
				}
			}
		}

		int Layer = LayerMask.NameToLayer ("UI3D");

		Player1_Char1.SetLayerRecursively (Layer);
		Player1_Char2.SetLayerRecursively (Layer);
		Player2_Char1.SetLayerRecursively (Layer);
		Player2_Char2.SetLayerRecursively (Layer);

		DressManager.Instance.dummyCharacter = previous;

		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;

		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.565f, 0.0f, 0.41f, 0.8f);

		ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);

		if (ScreenManager.Instance.ScreenMoved != ScreenManager.Instance.RewardScreen) {
			ScreenManager.Instance.VotingScreen.transform.FindChild ("P2Voting Button").gameObject.SetActive (false);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("P1Voting Button").gameObject.SetActive (false);
			//false);
			if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
				ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
			else
				ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "YOUR STATUS";
			ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (false);

		}
	}

	void GetVotesResults_CoOp (int event_id, int pairId)
	{
		StartCoroutine (GetVotesResult_CoOp (event_id, pairId, false));
	}

	public IEnumerator GetVotesResult_CoOp (int event_id, int pairId, bool isForRewards)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
//		jsonElement ["player2_id"] = player2Id.ToString ();
		jsonElement ["event_id"] = event_id.ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/coopGetVotingResult", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("voting result data is following") || _jsnode ["status"].ToString ().Contains ("200")) {

				print ("Success");

				JSONNode data = _jsnode ["data"];

				float totalvotes = 0.0f;
				float.TryParse (data ["total_pair_vote"], out totalvotes);

				float p1VotesCount = 0.0f;
				float.TryParse (data ["set1_vote"], out p1VotesCount);

				int P1VoteBonus = 0;
				int P1FriendBonus = 0;
//				if(data ["first_player_vote_data"]["votebonus"] =="")					
				int.TryParse (data ["set1_vote_data"] ["votebonus"], out P1VoteBonus);
				int.TryParse (data ["set1_vote_data"] ["friendbonus"], out P1FriendBonus);


				float p2VotesCount = 0.0f;
				float.TryParse (data ["set2_vote"], out p2VotesCount);

				int P2VoteBonus = 0;
				int P2FriendBonus = 0;
				int.TryParse (data ["set2_vote_data"] ["votebonus"], out P2VoteBonus);
				int.TryParse (data ["set2_vote_data"] ["friendbonus"], out P2FriendBonus);

				if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.RewardScreen) {
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 1").FindChild ("Friend_bonus").GetComponent<Text> ().text = P1FriendBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 2").FindChild ("Friend_bonus").GetComponent<Text> ().text = P2FriendBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 1").FindChild ("Voting_bonus").GetComponent<Text> ().text = P1VoteBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 2").FindChild ("Voting_bonus").GetComponent<Text> ().text = P2VoteBonus.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 1").FindChild ("Vote_Count").GetComponent<Text> ().text = p1VotesCount.ToString ();
					ScreenManager.Instance.RewardScreen.transform.FindChild ("Player 2").FindChild ("Vote_Count").GetComponent<Text> ().text = p2VotesCount.ToString ();
				} else {
					if (totalvotes != 0f) {

						player1voteCount.text = p1VotesCount.ToString ();
						player2voteCount.text = p2VotesCount.ToString ();

						player1votingBonus.text = P1VoteBonus.ToString ();
						player2votingBonus.text = P2VoteBonus.ToString ();

						player1FriendBonus.text = P1FriendBonus.ToString ();
						player2FriendBonus.text = P2FriendBonus.ToString ();

					} else {
						player1voteCount.text = 0 + "";
						player2voteCount.text = 0 + "";
					}

					player1ScoreText.text = "" + (p1VotesCount * 10 + P1VoteBonus + P1FriendBonus);
					player2ScoreText.text = "" + (p2VotesCount * 10 + P2VoteBonus + P2FriendBonus);
				}

				if (isForRewards) { 		
					ScreenAndPopupCall.Instance.ResultPanelClose ();

					int Player1Id;
					int.TryParse (data ["set1_player1_id"], out Player1Id);
					int Player2Id;
					int.TryParse (data ["set1_player2_id"], out Player2Id);

					int Player3Id;
					int.TryParse (data ["set2_player1_id"], out Player3Id);
					int Player4Id;
					int.TryParse (data ["set2_player2_id"], out Player4Id);
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (true);
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.RemoveAllListeners ();
					int P1Total = Mathf.RoundToInt (p1VotesCount * 10) + P1VoteBonus + P1FriendBonus;
					int P2Total = Mathf.RoundToInt (p2VotesCount * 10) + P2VoteBonus + P2FriendBonus;

					if (PlayerPrefs.GetInt ("PlayerId") == Player1Id || PlayerPrefs.GetInt ("PlayerId") == Player2Id) { // If i am player 1
						if (P1Total > P2Total) {
							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));
						} else if (P1Total < P2Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, event_id, 0)));
						} else {
							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));		
						}
					} else if (PlayerPrefs.GetInt ("PlayerId") == Player3Id || PlayerPrefs.GetInt ("PlayerId") == Player4Id) {// If i am player 2
						if (P2Total > P1Total) {
							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));
						} else if (P2Total < P1Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, event_id, 0)));
						} else {
							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, event_id, Value)));	
						}
					}
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Vote_Count").GetComponent<Text> ().text = P1Total.ToString ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Vote_Count").GetComponent<Text> ().text = P2Total.ToString ();

				}
				yield return true;
			} else {
				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	IEnumerator VoteForPlayerCoOp (int player1_id, int player2_id, int pairId, bool isfriend)
	{
		///TODO: change for all events
		int vote = 1;
		int friendBous = 0;
		int votingBonus = 0;

		if (isfriend) {
			friendBous = 1;
			votingBonus = 0;
		} else if (player1_id == PlayerPrefs.GetInt ("PlayerId")) {
			vote = 0;
			friendBous = 0;
			votingBonus = 1;
		}

		var encoding = new System.Text.UTF8Encoding ();
		//		{ 
		//			"player1_id": "73",
		//			"player2_id": "73","event_id": "67",
		//			"votedby_player_id": "125",
		//			"pair_id": "5",
		//			"vote_count": "1",
		//			"vote_score": "10",
		//			"vote_bonus": "10",
		//			"friend_bonus": "10"
		//		}
		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();	
		jsonElement ["player1_id"] = player1_id.ToString ();
		jsonElement ["player2_id"] = player2_id.ToString ();
		jsonElement ["votedby_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();

		jsonElement ["pair_id"] = pairId.ToString ();
		jsonElement ["vote_count"] = vote.ToString ();
		jsonElement ["vote_score"] = "0";
		jsonElement ["vote_bonus"] = votingBonus.ToString ();
		jsonElement ["friend_bonus"] = friendBous.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/coopUpdateVotedResult", encoding.GetBytes (jsonElement.ToString ()), postHeader);
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");
				if (votingBonus > 0) {
					ShowPopOfDescription ("Voting bonus applied", () => GetVotesResults_CoOp (EventManagment.Instance.CurrentEvent.Event_id, SelectedPair_CoOp.pair_id));
					PersistanceManager.Instance.DeleteThisVotingBonus (EventManagment.Instance.CurrentEvent.Event_id);
				} else {

					if (GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id) == 9) {	
						GenrateVotingBonus (EventManagment.Instance.CurrentEvent.Event_id, eventType.CoOp_Event);
						ShowPopOfDescription ("Your vote saved successfully.\n You have gained a new Voting Bonus. You have used your all 10 votes for this Event. You can use this bonus to boost your scores in any event within 2 hours.", () => NextPair ());
					} else {				
						ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()), () => NextPair ());
					}
					IncreamentVoteCount (EventManagment.Instance.CurrentEvent.Event_id);
					if (isfriend) {
						if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
							AchievementsManager.Instance.CheckAchievementsToUpdate ("voteForFriends");
					}
				}
				//				Invoke ("NextPair", 0.6f);

				yield return true;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				if (votingBonus > 0) {
					ShowPopOfDescription ("Already applied a voting bonus.");
				} else {
					ShowPopOfDescription ("Already voted for this pair.", () => NextPair ());
				}
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	public IEnumerator GetMyPair_CoOp (int Count, bool IsRewards)
	{
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = false;
		SelectedEventType = "CoOp";

		var cd = new CoroutineWithData (EventManagment.Instance, EventManagment.Instance.GetCoOpRegistration (EventManagment.Instance.CurrentEvent.Event_id));
		yield return cd.coroutine;

		if (cd.result != null) {
			var Registered = (CoopRegisterPlayers)cd.result;
			StartCoroutine (IeGetMyPair_CoOp (Registered.Player1Id, Registered.Player2Id, Count, IsRewards));
		} else {
			ShowPopUpWithOnClickOkFunction ("Sorry! No Pairs Found");
		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = true;
	}

	public IEnumerator IeGetMyPair_CoOp (int Player1Id, int Player2Id, int Count, bool IsRewards)
	{
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["main_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["player1_id"] = Player1Id.ToString ();
		jsonElement ["player2_id"] = Player2Id.ToString ();
		jsonElement ["count"] = Count.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/coopGetSinglePair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode dataArray = _jsnode ["data"];

			
				CoOpVotingPair Vp = new CoOpVotingPair ();
				Vp.pair_id = int.Parse (dataArray ["pair_id"]);
				Vp.event_id = int.Parse (dataArray ["event_id"]);
				/// set 1 data 
				/// Player 1 data
				Vp.set1_player1_id = int.Parse (dataArray ["old_player1_id"]);
//				Vp.set1_player1_flatmate_id = int.Parse (dataArray ["old_player1_flatmate_id"]);
				Vp.set1_player1Name = dataArray ["old_player1_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode set1_P1DressItems = dataArray ["old_player1_item_data"];


				for (int a = 0; a < set1_P1DressItems.Count; a++) {
					var itemId = 0;
					int.TryParse (set1_P1DressItems [a] ["item_id"], out itemId);

					var itemCat = set1_P1DressItems [a] ["item_type"];

					if (itemCat.ToString ().Contains ("Gender")) {
						if (itemId == 1)
							Vp.set1_player1Gender = "Male";
						else
							Vp.set1_player1Gender = "Female";
					} else if (itemCat.ToString ().Contains ("SkinColor")) {
						Vp.set1_player1Skin = itemId;
					} else {
						if (Vp.set1_player1_dressData.ContainsKey (itemId))
							Vp.set1_player1_dressData [itemId] = itemCat;
						else
							Vp.set1_player1_dressData.Add (itemId, itemCat);
					}
				}
				///player2 data
				Vp.set1_player2_id = int.Parse (dataArray ["old_player2_id"]);
//				Vp.set1_player2_flatmate_id = int.Parse (dataArray ["old_player2_flatmate_id"]);
				Vp.set1_player2Name = dataArray ["old_player2_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode set1_P2DressItems = dataArray ["old_player2_item_data"];


				for (int a = 0; a < set1_P2DressItems.Count; a++) {
					var itemId = int.Parse (set1_P2DressItems [a] ["item_id"]);
					var itemCat = set1_P2DressItems [a] ["item_type"];
					if (itemCat.ToString ().Contains ("Gender")) {
						if (itemId == 1)
							Vp.set1_player2Gender = "Male";
						else
							Vp.set1_player2Gender = "Female";
					} else if (itemCat.ToString ().Contains ("SkinColor")) {
						Vp.set1_player2Skin = itemId;
					} else {
						if (Vp.set1_player2_dressData.ContainsKey (itemId))
							Vp.set1_player2_dressData [itemId] = itemCat;
						else
							Vp.set1_player2_dressData.Add (itemId, itemCat);
					}
				}

				/// set 2 data 
				/// Player 1 data
				Vp.set2_player1_id = int.Parse (dataArray ["new_player1_id"]);
//				Vp.set2_player1_flatmate_id = int.Parse (dataArray ["new_player1_flatmate_id"]);
				Vp.set2_player1Name = dataArray ["new_player1_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode set2_P1DressItems = dataArray ["new_player1_item_data"];


				for (int a = 0; a < set2_P1DressItems.Count; a++) {
					var itemId = int.Parse (set2_P1DressItems [a] ["item_id"]);
					var itemCat = set2_P1DressItems [a] ["item_type"];
					if (itemCat.ToString ().Contains ("Gender")) {
						if (itemId == 1)
							Vp.set2_player1Gender = "Male";
						else
							Vp.set2_player1Gender = "Female";
					} else if (itemCat.ToString ().Contains ("SkinColor")) {
						Vp.set2_player1Skin = itemId;
					} else {
						if (Vp.set2_player1_dressData.ContainsKey (itemId))
							Vp.set2_player1_dressData [itemId] = itemCat;
						else
							Vp.set2_player1_dressData.Add (itemId, itemCat);
					}
				}

				///player2 data
				Vp.set2_player2_id = int.Parse (dataArray ["new_player2_id"]);
//					Vp.set2_player2_flatmate_id = int.Parse (dataArray ["new_player2_flatmate_id"]);
				Vp.set2_player2Name = dataArray ["new_player2_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode set2_P2DressItems = dataArray ["new_player2_item_data"];


				for (int a = 0; a < set2_P2DressItems.Count; a++) {
					var itemId = int.Parse (set2_P2DressItems [a] ["item_id"]);
					var itemCat = set2_P2DressItems [a] ["item_type"];
					if (itemCat.ToString ().Contains ("Gender")) {
						if (itemId == 1)
							Vp.set2_player2Gender = "Male";
						else
							Vp.set2_player2Gender = "Female";
					} else if (itemCat.ToString ().Contains ("SkinColor")) {
						Vp.set2_player2Skin = itemId;
					} else {
						if (Vp.set2_player2_dressData.ContainsKey (itemId))
							Vp.set2_player2_dressData [itemId] = itemCat;
						else
							Vp.set2_player2_dressData.Add (itemId, itemCat);
					}
				}						

				if (IsRewards) {
					ScreenAndPopupCall.Instance.RewardScreenSelection ();
					ScreenAndPopupCall.Instance.ResultPanelClose ();
				} else {
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();

				}
                StartCoroutine(ShowMyPlayerCoOp (Vp));

				yield return Vp;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				if (viewStatus)
					ShowPopOfDescription ("No pair found");
				yield return null;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return null;
			}
		} else {
			yield return null;
		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = true;
	}

	public IEnumerator GetAllVotingPairs_CoOpForCheck ()
	{
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["page_no"] = pairIndex.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		//		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/getvotingpair", encoding.GetBytes (jsonElement.ToString ()), postHeader);
		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/coopGetVotingPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode dataArray = _jsnode ["data"];

				for (int i = 0; i < dataArray.Count; i++) {
					CoOpVotingPair Vp = new CoOpVotingPair ();
					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					/// set 1 data 
					/// Player 1 data
					Vp.set1_player1_id = int.Parse (dataArray [i] ["set1_player1_id"]);
//					Vp.set1_player1_flatmate_id = int.Parse (dataArray [i] ["set1_player1_flatmate_id"]);
					Vp.set1_player1Name = dataArray [i] ["set1_player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set1_P1DressItems = dataArray [i] ["set1_player1_items"];


					for (int a = 0; a < set1_P1DressItems.Count; a++) {
						var itemId = int.Parse (set1_P1DressItems [a] ["item_id"]);
						var itemCat = set1_P1DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set1_player1Gender = "Male";
							else
								Vp.set1_player1Gender = "Female";
						} else {
							if (Vp.set1_player1_dressData.ContainsKey (itemId))
								Vp.set1_player1_dressData [itemId] = itemCat;
							else
								Vp.set1_player1_dressData.Add (itemId, itemCat);
						}
					}
					///player2 data
					Vp.set1_player2_id = int.Parse (dataArray [i] ["set1_player2_id"]);
//					Vp.set1_player2_flatmate_id = int.Parse (dataArray [i] ["set1_player2_flatmate_id"]);
					Vp.set1_player2Name = dataArray [i] ["set1_player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set1_P2DressItems = dataArray [i] ["set1_player2_items"];


					for (int a = 0; a < set1_P2DressItems.Count; a++) {
						var itemId = int.Parse (set1_P2DressItems [a] ["item_id"]);
						var itemCat = set1_P2DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set1_player2Gender = "Male";
							else
								Vp.set1_player2Gender = "Female";
						} else {
							if (Vp.set1_player2_dressData.ContainsKey (itemId))
								Vp.set1_player2_dressData [itemId] = itemCat;
							else
								Vp.set1_player2_dressData.Add (itemId, itemCat);
						}
					}

					/// set 2 data 
					/// Player 1 data
					Vp.set2_player1_id = int.Parse (dataArray [i] ["set2_player1_id"]);
//					Vp.set2_player1_flatmate_id = int.Parse (dataArray [i] ["set2_player1_flatmate_id"]);
					Vp.set2_player1Name = dataArray [i] ["set2_player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set2_P1DressItems = dataArray [i] ["set2_player1_items"];


					for (int a = 0; a < set2_P1DressItems.Count; a++) {
						var itemId = int.Parse (set2_P1DressItems [a] ["item_id"]);
						var itemCat = set2_P1DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set2_player1Gender = "Male";
							else
								Vp.set2_player1Gender = "Female";
						} else {
							if (Vp.set2_player1_dressData.ContainsKey (itemId))
								Vp.set2_player1_dressData [itemId] = itemCat;
							else
								Vp.set2_player1_dressData.Add (itemId, itemCat);
						}
					}

					///player2 data
					Vp.set2_player2_id = int.Parse (dataArray [i] ["set2_player2_id"]);
//					Vp.set2_player2_flatmate_id = int.Parse (dataArray [i] ["set2_player2_flatmate_id"]);
					Vp.set2_player2Name = dataArray [i] ["set2_player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set2_P2DressItems = dataArray [i] ["set2_player2_items"];


					for (int a = 0; a < set2_P2DressItems.Count; a++) {
						var itemId = int.Parse (set2_P2DressItems [a] ["item_id"]);
						var itemCat = set2_P2DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set2_player2Gender = "Male";
							else
								Vp.set2_player2Gender = "Female";
						} else {
							if (Vp.set2_player2_dressData.ContainsKey (itemId))
								Vp.set2_player2_dressData [itemId] = itemCat;
							else
								Vp.set2_player2_dressData.Add (itemId, itemCat);
						}
					}

//					if (Vp.set1_player1_id == PlayerPrefs.GetInt ("PlayerId") || Vp.set1_player2_id == PlayerPrefs.GetInt ("PlayerId") || Vp.set2_player1_id == PlayerPrefs.GetInt ("PlayerId") || Vp.set2_player2_id == PlayerPrefs.GetInt ("PlayerId"))
//						MyPlayersInCoOp.Add (Vp);
//					else
//						AllPairsInCoOp.Add (Vp);
				}

				SelectedEventType = "CoOp";
//				ShowRewardScreen ();

				yield return true;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				ShowPopOfDescription ("Currently no pair have registered for this Event");	
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	#endregion


	public IEnumerator DeleteMyPair (int EventId, int flatmateId)
	{

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventId.ToString ();
		jsonElement ["flatmate_id"] = flatmateId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/deleteFashionPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				yield return true;
			} else if (_jsnode ["status"].ToString ().Contains ("400"))
				yield return false;
		}
	}


	public IEnumerator DeleteMyPairDecor (int EventId)
	{

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventId.ToString ();
		jsonElement ["flatmate_id"] = "";

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/deleteDecorPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				yield return true;
			} else if (_jsnode ["status"].ToString ().Contains ("400"))
				yield return false;
		}
	}



	public IEnumerator DeleteMyPairCatWalk (int EventId)
	{

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/deleteCatWalkPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				yield return true;
			} else if (_jsnode ["status"].ToString ().Contains ("400"))
				yield return false;
		}
	}

	public IEnumerator DeleteMyPair_Coop (int eventId, int player1Id, int player2Id)
	{

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["event_id"] = eventId.ToString ();
		jsonElement ["player1_id"] = player1Id.ToString ();
		jsonElement ["player2_id"] = player2Id.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/events/deleteCoopPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				yield return true;
			} else if (_jsnode ["status"].ToString ().Contains ("400"))
				yield return false;
		}
	}

	public IEnumerator DeleteMyPair_Society (int EventId)
	{

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/societyEvents/deleteSocialFashionPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {
				yield return true;
			} else if (_jsnode ["status"].ToString ().Contains ("400"))
				yield return false;
		}
	}

	public IEnumerator IeGetPairSocietyEvent (bool isForNextPair)
	{
		SelectedEventType = "SocietyEvent";
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent <Button> ().interactable = false;
		pairIndex++;
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["page_no"] = pairIndex.ToString ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/societyEvents/getSocietyEventPair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");


				JSONNode dataArray = _jsnode ["data"];
				SocietyVotingPair Vp = new SocietyVotingPair ();

				for (int i = 0; i < dataArray.Count; i++) {

					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					Vp.Gender = dataArray [i] ["gender"].ToString ();

					Vp.player1_id = int.Parse (dataArray [i] ["player1_id"]);
					Vp.player1_flatmate_id = int.Parse (dataArray [i] ["player1_flatmate_id"]);
					Vp.player1Name = dataArray [i] ["player1_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player1societyId = int.Parse (dataArray [i] ["player1_society_id"]);
					JSONNode p1DressData = dataArray [i] ["player1_item_data"];

					var P1_SkinColor = "";

					for (int a = 0; a < p1DressData.Count; a++) {						
						var itemType = p1DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P1_SkinColor = p1DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p1DressData [a] ["item_id"], out itemId);

							if (Vp.player1_dressData.ContainsKey (itemId))
								Vp.player1_dressData [itemId] = itemType;
							else
								Vp.player1_dressData.Add (itemId, itemType);
						}
					}
					Vp.player1_SkinColor = P1_SkinColor;

					Vp.player2_id = int.Parse (dataArray [i] ["player2_id"]);
					Vp.player2_flatmate_id = int.Parse (dataArray [i] ["player2_flatmate_id"]);
					Vp.player2Name = dataArray [i] ["player2_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player2societyId = int.Parse (dataArray [i] ["player2_society_id"]);

					JSONNode p2DressData = dataArray [i] ["player2_item_data"];

					var P2_SkinColor = "";

					for (int a = 0; a < p2DressData.Count; a++) {
						var itemType = p2DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P2_SkinColor = p2DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p2DressData [a] ["item_id"], out itemId);

							if (Vp.player2_dressData.ContainsKey (itemId))
								Vp.player2_dressData [itemId] = itemType;
							else
								Vp.player2_dressData.Add (itemId, itemType);
						}
					}

					Vp.player2_SkinColor = P2_SkinColor;
				}

                StartCoroutine(ShowPairSocietyEvent (Vp));

				if (!isForNextPair) {			
					ScreenAndPopupCall.Instance.CloseScreen ();
					ScreenAndPopupCall.Instance.VotingScreenSelection ();
				}

				//				}

				yield return true;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
				if (isForNextPair) {
					pairIndex = 0;
					ShowPopOfDescription ("No pairs to display", () => NextPair ());
				} else
					ShowPopOfDescription ("Currently no pair have registered for this Event");	
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return false;			
			}
		} else {			
			yield return false;
		}
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("Vote").GetComponent<Button> ().interactable = true;
	}

	public IEnumerator GetMyPair_SocietyEvent ()
	{
		SelectedEventType = "SocietyEvent";

		if (SocietyManager.Instance._mySociety != null && SocietyManager.Instance._mySociety.Id != 0) {

			ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = false;
			StartCoroutine (IeGetMyPair_SocietyEvent (1, EventManagment.Instance.CurrentEvent.Event_id, SocietyManager.Instance._mySociety.Id, false));
		} else {
			var cd = new CoroutineWithData (SocietyManager.Instance, SocietyManager.Instance.IGetSocieties (SocietyManager.SeachType.mine, "", true));
			yield return cd.coroutine;
			if (cd.result != null) {
				ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = false;
				StartCoroutine (IeGetMyPair_SocietyEvent (1, EventManagment.Instance.CurrentEvent.Event_id, SocietyManager.Instance._mySociety.Id, false));
			} else {
				ShowPopOfDescription ("You are not a member of any Society, Please join or create a new society");
			}
		}
	}

	public IEnumerator IeGetMyPair_SocietyEvent (int count, int EventId, int SocietyId, bool IsRewards)
	{
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["society_id"] = SocietyId.ToString ();
		jsonElement ["event_id"] = EventId.ToString ();
		jsonElement ["count"] = count.ToString ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/societyEvents/getSocialEventSinglePair", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode dataArray = _jsnode ["data"];
				var Vp = new SocietyVotingPair ();

				int.TryParse (dataArray ["pair_id"], out Vp.pair_id);
				int.TryParse (dataArray ["event_id"], out Vp.event_id);
				Vp.Gender = dataArray ["gender"].ToString ();

				int.TryParse (dataArray ["player1_society_id"], out Vp.player1societyId);
				int.TryParse (dataArray ["player1_id"], out Vp.player1_id);
				int.TryParse (dataArray ["player1_flatmate_id"], out Vp.player1_flatmate_id);
				Vp.player1Name = dataArray ["player1_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode p1DressData = dataArray ["player1_item_data"];

				var P1_SkinColor = "";

				for (int a = 0; a < p1DressData.Count; a++) {						
					var itemType = p1DressData [a] ["item_type"];

					if (itemType.ToString ().Contains ("CharacterType"))
						P1_SkinColor = p1DressData [a] ["item_id"];
					else {
						var itemId = 0;
						int.TryParse (p1DressData [a] ["item_id"], out itemId);

						if (Vp.player1_dressData.ContainsKey (itemId))
							Vp.player1_dressData [itemId] = itemType;
						else
							Vp.player1_dressData.Add (itemId, itemType);
					}
				}
				Vp.player1_SkinColor = P1_SkinColor;

				int.TryParse (dataArray ["player2_id"], out Vp.player2_id);
				int.TryParse (dataArray ["player2_flatmate_id"], out Vp.player2_flatmate_id);
				int.TryParse (dataArray ["player2_society_id"], out Vp.player2societyId);
				Vp.player2Name = dataArray ["player2_name"].ToString ().Trim ("\"".ToCharArray ());

				JSONNode p2DressData = dataArray ["player2_item_data"];

				var P2_SkinColor = "";

				for (int a = 0; a < p2DressData.Count; a++) {
					var itemType = p2DressData [a] ["item_type"];

					if (itemType.ToString ().Contains ("CharacterType"))
						P2_SkinColor = p2DressData [a] ["item_id"];
					else {
						var itemId = 0;
						int.TryParse (p2DressData [a] ["item_id"], out itemId);

						if (Vp.player2_dressData.ContainsKey (itemId))
							Vp.player2_dressData [itemId] = itemType;
						else
							Vp.player2_dressData.Add (itemId, itemType);
					}
				}

				Vp.player2_SkinColor = P2_SkinColor;

				SelectedEventType = "SocietyEvent";

				ScreenAndPopupCall.Instance.CloseScreen ();

				yield return new WaitForSeconds (0.1f);

				if (IsRewards) {
					ScreenAndPopupCall.Instance.RewardScreenSelection ();
					ScreenAndPopupCall.Instance.ResultPanelClose ();
				} else {
					ScreenAndPopupCall.Instance.VotingScreenSelection ();					
				}
                StartCoroutine(ShowMyPairForScoiety (Vp));

				yield return Vp;
			} else if (_jsnode ["description"].ToString ().Contains ("Voting pair data has empty") || _jsnode ["status"].ToString ().Contains ("400")) {	

				if (viewStatus)
					ShowPopOfDescription ("No pair found");
				yield return null;
			} else {
				print ("error" + _jsnode ["description"].ToString ());
				yield return null;

			}
		} else {			
			yield return null;
		}

		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Status").GetComponent <Button> ().interactable = true;
	}

    public IEnumerator ShowPairSocietyEvent (SocietyVotingPair pair)
	{
		Player1Voting_Button.interactable = true;
		Player2Voting_Button.interactable = true;

		SelectedPair_SocietyEvent = pair;
		ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
		StartCoroutine (GetVotesResultSocietyEvent (PlayerPrefs.GetInt ("PlayerId"), pair.pair_id, pair.event_id, false));

		player1Name.text = pair.player1Name;
		player2Name.text = pair.player2Name;

		GameObject Char1 = null;
		GameObject Char2 = null;
		var previous = DressManager.Instance.dummyCharacter;

		//			if (pair.Gender.Contains ("Male")) {
        var Oppo1 = FindFlatMateAvtar (pair.player1_flatmate_id);
        var Oppo2 = FindFlatMateAvtar (pair.player2_flatmate_id);

		if (Oppo1 != null) {
			Char1 = (GameObject)Instantiate (Oppo1, Vector3.zero, Quaternion.identity);
			Char1.GetComponent <CharacterProperties> ().PlayerType = Oppo1.GetComponent <CharacterProperties> ().PlayerType;
		} else {
            if (pair.Gender.Contains("Male"))
            {
                Char1 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Char1 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            }
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char1, pair.player1_id));
		}


		if (Oppo2 != null) {
			Char2 = (GameObject)Instantiate (Oppo2, Vector3.zero, Quaternion.identity);
			Char2.GetComponent <CharacterProperties> ().PlayerType = Oppo2.GetComponent <CharacterProperties> ().PlayerType;
		} else {
            if (pair.Gender.Contains("Male"))
            {            
                Char2 = (GameObject)Instantiate(MalePrefab, Vector3.zero, Quaternion.identity);    
            }
            else
            {
                Char2 = (GameObject)Instantiate(FemalePrefab, Vector3.zero, Quaternion.identity);
            }         
            yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char2, pair.player2_id));
		}

		Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
		Char1.transform.localScale = Vector3.one * 0.3f;
		Char1.transform.localPosition = Vector3.zero;
		if (Char1.GetComponent<Flatmate> ())
			Destroy (Char1.GetComponent<Flatmate> ());

		if (Char1.GetComponent<RoomMateMovement> ())
			Destroy (Char1.GetComponent<RoomMateMovement> ());

		var moneyIcon = Char1.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon.gameObject);
		Destroy (Char1.GetComponent<GenerateMoney> ());

		Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
		Char2.transform.localScale = Vector3.one * 0.3f;
		Char2.transform.localPosition = Vector3.zero;

		if (Char2.GetComponent<Flatmate> ())
			Destroy (Char2.GetComponent<Flatmate> ());

		if (Char2.GetComponent<RoomMateMovement> ())
			Destroy (Char2.GetComponent<RoomMateMovement> ());
		Destroy (Char2.GetComponent<GenerateMoney> ());
		var moneyIcon2 = Char2.transform.FindChild ("low money");
		GameObject.Destroy (moneyIcon2.gameObject);

		foreach (var key in pair.player1_dressData.Keys) {				
            if (pair.player1_dressData [key].Contains ("Hair")) {
                var hair =  FindSaloonWithId (key);
                if (hair != null) {
                    DressManager.Instance.dummyCharacter = Char1;
                    DressManager.Instance.ChangeHairsForDummyCharacter (hair.PartName, hair.HairImages);
                }
            }else {
				var dress =	FindDressWithId (key);
				if (dress != null) {
					DressManager.Instance.dummyCharacter = Char1;
						DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
				}
			} 
		}

		foreach (var key in pair.player2_dressData.Keys) {
            if (pair.player2_dressData [key].Contains ("Hair")) {
                var hair =  FindSaloonWithId (key);
                if (hair != null) {
                    DressManager.Instance.dummyCharacter = Char2;
                    DressManager.Instance.ChangeHairsForDummyCharacter (hair.PartName, hair.HairImages);
                }
            }else {
				var dress =	FindDressWithId (key);
				if (dress != null) {
					DressManager.Instance.dummyCharacter = Char2;

						DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
				}
			}  
		}
            
		int Layer = LayerMask.NameToLayer ("UI3D");

		Char1.SetLayerRecursively (Layer);
		Char2.SetLayerRecursively (Layer);
		Char1.SetMaterialRecursively ();
		Char2.SetMaterialRecursively ();
		DressManager.Instance.dummyCharacter = previous;

		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;

		ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
		ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);

		ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (true);
//		ScreenManager.Instance.VotingScreen.transform.FindChild ("Player 1").GetChild (1).gameObject.SetActive (true);
//		ScreenManager.Instance.VotingScreen.transform.FindChild ("Player 2").GetChild (1).gameObject.SetActive (true);
		//true);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
		ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "WHO'S GOT THE BETTER OUTFIT?";
	}

    public IEnumerator ShowMyPairForScoiety (SocietyVotingPair pair)
	{
		if (SelectedEventType == "SocietyEvent") {


			ScreenAndPopupCall.Instance.CloseCharacterCamerasForEvents ();
			SelectedPair_SocietyEvent = pair;
			StartCoroutine (GetVotesResultSocietyEvent (PlayerPrefs.GetInt ("PlayerId"), pair.pair_id, pair.event_id, false));

			player1Name.text = pair.player1Name;
			player2Name.text = pair.player2Name;

			GameObject Char1 = null;
			GameObject Char2 = null;
			var previous = DressManager.Instance.dummyCharacter;

//			if (pair.Gender.Contains ("Male")) {
			var Oppo1 = FindFlatMateAvtar (pair.player1_flatmate_id);
			var Oppo2 = FindFlatMateAvtar (pair.player2_flatmate_id);

			if (Oppo1 != null) {
				Char1 = (GameObject)Instantiate (Oppo1, Vector3.zero, Quaternion.identity);
				Char1.GetComponent <CharacterProperties> ().PlayerType = Oppo1.GetComponent <CharacterProperties> ().PlayerType;
			} else {
				if (pair.Gender.Contains ("Male"))
					Char1 = (GameObject)Instantiate (MalePrefab, Vector3.zero, Quaternion.identity);
				else
					Char1 = (GameObject)Instantiate (FemalePrefab, Vector3.zero, Quaternion.identity);
                yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char1, pair.player1_id));
			}


			if (Oppo2 != null) {
				Char2 = (GameObject)Instantiate (Oppo2, Vector3.zero, Quaternion.identity);
				Char2.GetComponent <CharacterProperties> ().PlayerType = Oppo2.GetComponent <CharacterProperties> ().PlayerType;
			} else {
				if (pair.Gender.Contains ("Male"))
					Char2 = (GameObject)Instantiate (MalePrefab, Vector3.zero, Quaternion.identity);
				else
					Char2 = (GameObject)Instantiate (FemalePrefab, Vector3.zero, Quaternion.identity);
                yield return StartCoroutine(PlayerManager.Instance.ApplyCustomisationOfRealFlatmate(Char2, pair.player2_id));
			}


			Char1.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting1.transform.GetChild (0);
			Char1.transform.localScale = Vector3.one * 0.3f;
			Char1.transform.localPosition = Vector3.zero;

			if (Char1.GetComponent<Flatmate> ())
				Destroy (Char1.GetComponent<Flatmate> ());

			if (Char1.GetComponent<RoomMateMovement> ())
				Destroy (Char1.GetComponent<RoomMateMovement> ());

			var moneyIcon = Char1.transform.FindChild ("low money");
			if (moneyIcon != null)
				GameObject.Destroy (moneyIcon.gameObject);
			if (Char1.GetComponent<GenerateMoney> ())
				Destroy (Char1.GetComponent<GenerateMoney> ());

			Char2.transform.parent = ScreenAndPopupCall.Instance.CharacterCameraForvoting2.transform.GetChild (0);
			Char2.transform.localScale = Vector3.one * 0.3f;
			Char2.transform.localPosition = Vector3.zero;

			if (Char2.GetComponent<Flatmate> ())
				Destroy (Char2.GetComponent<Flatmate> ());

			if (Char2.GetComponent<RoomMateMovement> ())
				Destroy (Char2.GetComponent<RoomMateMovement> ());
			if (Char2.GetComponent<GenerateMoney> ())
				Destroy (Char2.GetComponent<GenerateMoney> ());
		
			var moneyIcon2 = Char2.transform.FindChild ("low money");
			if (moneyIcon2 != null)
				GameObject.Destroy (moneyIcon2.gameObject);

			foreach (var key in pair.player1_dressData.Keys) {	
                if (pair.player1_dressData [key].Contains ("Hair")) {
                    var hair =  FindSaloonWithId (key);
                    if (hair != null) {
                        DressManager.Instance.dummyCharacter = Char1;
                        DressManager.Instance.ChangeHairsForDummyCharacter (hair.PartName, hair.HairImages);
                    }
                } else{				
					var dress =	FindDressWithId (key);
					if (dress != null) {
						DressManager.Instance.dummyCharacter = Char1;
//						if (Char1.GetComponent <CharacterProperties> ().PlayerType.Contains ("White"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
//						else if (Char1.GetComponent <CharacterProperties> ().PlayerType.Contains ("Brown"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						else if (Char1.GetComponent <CharacterProperties> ().PlayerType.Contains ("Black"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
//						else
							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
					}
				} 
			}

			foreach (var key in pair.player2_dressData.Keys) {
                if (pair.player2_dressData [key].Contains ("Hair")) {
                    var hair =  FindSaloonWithId (key);
                    if (hair != null) {
                        DressManager.Instance.dummyCharacter = Char2;
                        DressManager.Instance.ChangeHairsForDummyCharacter (hair.PartName, hair.HairImages);
                    }
                }else {
					var dress =	FindDressWithId (key);
					if (dress != null) {
						DressManager.Instance.dummyCharacter = Char2;
//						if (Char2.GetComponent <CharacterProperties> ().PlayerType.Contains ("White"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
//						else if (Char2.GetComponent <CharacterProperties> ().PlayerType.Contains ("Brown"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
//						else if (Char2.GetComponent <CharacterProperties> ().PlayerType.Contains ("Black"))
//							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
//						else
							DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.DressesImages);
					}
				} 
			}

			//		foreach (var key in pair.player2_dressData.Keys) {
			//			var dress =	FindDressWithName (key.ToString (), pair.player2_dressData [key].ToString ());
			//			if (dress != null) {
			//				DressManager.Instance.dummyCharacter = Char2;
			//
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "White")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.White_Images);
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Black")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Black_Images);
			//				if (DressManager.Instance.dummyCharacter.GetComponent<CharacterProperties> ().PlayerType == "Brown")
			//					DressManager.Instance.ChangeDressForDummyCharacter (dress.PartName, dress.Brown_Images);
			int Layer = LayerMask.NameToLayer ("UI3D");

			Char1.SetLayerRecursively (Layer);
			Char2.SetLayerRecursively (Layer);
			Char1.SetMaterialRecursively ();
			Char2.SetMaterialRecursively ();


			DressManager.Instance.dummyCharacter = previous;

			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.enabled = true;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.enabled = true;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.orthographicSize = 2.2f;
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.orthographicSize = 2.2f;


			ScreenAndPopupCall.Instance.CharacterCameraForvoting1.rect = new Rect (0.03f, 0.0f, 0.41f, 0.8f);
			ScreenAndPopupCall.Instance.CharacterCameraForvoting2.rect = new Rect (0.53f, 0.0f, 0.41f, 0.8f);

			ScreenManager.Instance.VotingScreen.transform.FindChild ("NextPairButton").gameObject.SetActive (false);

//			ScreenManager.Instance.VotingScreen.transform.FindChild ("Player 1").GetChild (1).gameObject.SetActive (false);
//			ScreenManager.Instance.VotingScreen.transform.FindChild ("Player 2").GetChild (1).gameObject.SetActive (false);
			//false);

			if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
				ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
			else
				ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);

			ScreenManager.Instance.VotingScreen.transform.FindChild ("Help Text").GetComponent <Text> ().text = "YOUR STATUS";
			ScreenManager.Instance.VotingScreen.transform.FindChild ("VotesLeftCount").gameObject.SetActive (false);

		}
	}

	public IEnumerator GetVotesResultSocietyEvent (int playerId, int pairId, int EventId, bool isShowingResult)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = playerId.ToString ();
		jsonElement ["event_id"] = EventId.ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 


		WWW www = new WWW ("http://pinekix.ignivastaging.com/societyEvents/getSocietyEventVotedResult", encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return  www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("www.text ==>> " + www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("voting result data is following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode data = _jsnode ["data"];

				float totalvotes = 0.0f;
				float.TryParse (data ["total_pair_vote"], out totalvotes);


				int Player1Id;
				int.TryParse (data ["first_player_id"], out Player1Id);
				int Player2Id;
				int.TryParse (data ["second_player_id"], out Player2Id);

				float p1VotesCount = 0.0f;
				float.TryParse (data ["first_player_vote_data"] ["votecount"], out p1VotesCount);

				float p2VotesCount = 0.0f;
				float.TryParse (data ["second_player_vote_data"] ["votecount"], out p2VotesCount);

				int P1VoteBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["votebonus"], out P1VoteBonus);
				int P1FriendBonus = 0;
				int.TryParse (data ["first_player_vote_data"] ["friendbonus"], out P1FriendBonus);


				int P2VoteBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["votebonus"], out P2VoteBonus);
				int P2FriendBonus = 0;
				int.TryParse (data ["second_player_vote_data"] ["friendbonus"], out P2FriendBonus);


				if (totalvotes != 0f) {

					player1voteCount.text = p1VotesCount.ToString ();
					player2voteCount.text = p2VotesCount.ToString ();

				} else {
					player1voteCount.text = 0 + "";
					player2voteCount.text = 0 + "";
				}		

				player1votingBonus.text = P1VoteBonus.ToString ();
				player2votingBonus.text = P2VoteBonus.ToString ();

				player1FriendBonus.text = P1FriendBonus.ToString ();
				player2FriendBonus.text = P2FriendBonus.ToString ();

				player1ScoreText.text = "" + (p1VotesCount * 10 + P1VoteBonus + P1FriendBonus);
				player2ScoreText.text = "" + (p2VotesCount * 10 + P2VoteBonus + P2FriendBonus);



				if (isShowingResult) { 			
					ScreenAndPopupCall.Instance.ResultPanelClose ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").gameObject.SetActive (true);
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.RemoveAllListeners ();
					int P1Total = Mathf.RoundToInt (p1VotesCount * 10) + P1VoteBonus + P1FriendBonus;
					int P2Total = Mathf.RoundToInt (p2VotesCount * 10) + P2VoteBonus + P2FriendBonus;


					if (PlayerPrefs.GetInt ("PlayerId") == Player1Id) { // If i am player 1 and I won
						if (P1Total > P2Total) {
							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));
						} else if (P1Total < P2Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, EventId, 0)));
						} else {

							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));		
						}
					} else if (PlayerPrefs.GetInt ("PlayerId") == Player2Id) {// If i am player 2 and I won
						if (P2Total > P1Total) {

							float Value = UnityEngine.Random.Range (0.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Win!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));
						} else if (P2Total < P1Total) {
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Lost!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (false, EventId, 0)));
						} else {

							float Value = UnityEngine.Random.Range (1.0f, 10.0f);
							ScreenManager.Instance.ResultScreen.transform.FindChild ("YouWinText").GetComponent <Text> ().text = "You Tied!";
							PrizeClaimed = false;
							ScreenManager.Instance.ResultScreen.transform.FindChild ("Claim Reward").GetComponent<Button> ().onClick.AddListener (() => StartCoroutine (GainRewards (true, EventId, Value)));	
						}
					}
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 1").FindChild ("Vote_Count").GetComponent<Text> ().text = P1Total.ToString ();
					ScreenManager.Instance.ResultScreen.transform.FindChild ("Player 2").FindChild ("Vote_Count").GetComponent<Text> ().text = P2Total.ToString ();

				}
				yield return true;
			} else {
				print ("error" + www.error);
				yield return false;
			}
		} else {
			yield return false;
		}
	}

    GameObject FindFlatMateAvtar (int Id)
	{
        if(Id ==1)
            return null;
        foreach (var flatmate in RoommateManager.Instance.AllRoommatesData) {
			if (flatmate.Id == Id) {
				return flatmate.Prefab;
			}
		}
		return null;
	}

	IEnumerator VoteInSocietyEvent (int votedToId, int societyId, int flatmateId, int pairId, bool isfriend)
	{

		Player1Voting_Button.interactable = false;
		Player2Voting_Button.interactable = false;

		int vote = 1;
		int friendBous = 0;
		int votingBonus = 0;

		if (isfriend) {
			friendBous = 1;
			votingBonus = 0;
		} else if (votedToId == PlayerPrefs.GetInt ("PlayerId")) {
			vote = 0;
			friendBous = 0;
			votingBonus = 1;
		}

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var jsonElement = new JSONClass ();	
		jsonElement ["player_id"] = votedToId.ToString ();
		jsonElement ["votedby_player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["flatmate_id"] = flatmateId.ToString ();
		jsonElement ["society_id"] = societyId.ToString ();
		jsonElement ["pair_id"] = pairId.ToString ();
		jsonElement ["vote_count"] = vote.ToString ();
		jsonElement ["vote_score"] = "0";
		///TODO: apply vote bonus and friend bonus
		jsonElement ["vote_bonus"] = votingBonus.ToString ();
		jsonElement ["friend_bonus"] = friendBous.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW ("http://pinekix.ignivastaging.com/societyEvents/updateSocialEventVotedResult", encoding.GetBytes (jsonElement.ToString ()), postHeader);
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("successfully") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				if (votingBonus > 0) {

					ShowPopOfDescription ("Voting bonus applied", () => StartCoroutine (GetVotesResultSocietyEvent (PlayerPrefs.GetInt ("PlayerId"), SelectedPair_SocietyEvent.pair_id, SelectedPair_SocietyEvent.event_id, false)));

					PersistanceManager.Instance.DeleteThisVotingBonus (EventManagment.Instance.CurrentEvent.Event_id);
				} else {
					if (GetVoteCountsForCurrentEvent (EventManagment.Instance.CurrentEvent.Event_id) == 9) {	
						GenrateVotingBonus (EventManagment.Instance.CurrentEvent.Event_id, eventType.Society_Event);
						ShowPopOfDescription ("Your vote saved successfully.\n You have gained a new Voting Bonus. You have used your all 10 votes for this Event. You can use this bonus to boost your scores in any event within 2 hours.", () => NextPair ());
					} else {				
						ShowPopOfDescription (_jsnode ["description"].ToString ().Trim ("\"".ToCharArray ()), () => NextPair ());
					}
					IncreamentVoteCount (EventManagment.Instance.CurrentEvent.Event_id);
					if (isfriend) {
						if (PlayerPrefs.GetInt ("Tutorial_Progress") >= 26)
							AchievementsManager.Instance.CheckAchievementsToUpdate ("voteForFriends");
					}
				}


				yield return true;
			} else if (_jsnode ["description"].ToString ().Contains ("Already voted for this player") || _jsnode ["status"].ToString ().Contains ("400")) {
				print ("error" + _jsnode ["description"].ToString ());
				if (votingBonus > 0) {
					ShowPopOfDescription ("Already applied a voting bonus.");
				} else {
					ShowPopOfDescription ("Already voted for this pair.", () => NextPair ());
				}
				yield return false;
			}
		} else {			
			yield return false;
		}
		Player1Voting_Button.interactable = true;
		Player2Voting_Button.interactable = true;
	}

	public IEnumerator GetFriendInFashionEvent ()
	{
		yield return StartCoroutine (FriendsManager.Instance.GetAllFriendsList (false));
		FriendsInFashionEvent.Clear ();
		SelectedEventType = "Fashion";
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["data_type"] = "fashion_show";

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW (PinekixUrls.ViewFriendsLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");


				JSONNode dataArray = _jsnode ["data"];


				for (int i = 0; i < dataArray.Count; i++) {
					VotingPair Vp = new VotingPair ();

					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					Vp.Gender = dataArray [i] ["gender"].ToString ();

					Vp.player1_id = int.Parse (dataArray [i] ["player1_id"]);
					Vp.player1_flatmate_id = int.Parse (dataArray [i] ["player1_flatmate_id"]);
					Vp.player1Name = dataArray [i] ["player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode p1DressData = dataArray [i] ["player1_item_data"];

					var P1_SkinColor = "";

					for (int a = 0; a < p1DressData.Count; a++) {						
						var itemType = p1DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P1_SkinColor = p1DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p1DressData [a] ["item_id"], out itemId);

							if (Vp.player1_dressData.ContainsKey (itemId))
								Vp.player1_dressData [itemId] = itemType;
							else
								Vp.player1_dressData.Add (itemId, itemType);
						}
					}
					Vp.player1_SkinColor = P1_SkinColor;

					Vp.player2_id = int.Parse (dataArray [i] ["player2_id"]);
					Vp.player2_flatmate_id = int.Parse (dataArray [i] ["player2_flatmate_id"]);
					Vp.player2Name = dataArray [i] ["player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode p2DressData = dataArray [i] ["player2_item_data"];

					var P2_SkinColor = "";

					for (int a = 0; a < p2DressData.Count; a++) {
						var itemType = p2DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P2_SkinColor = p2DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p2DressData [a] ["item_id"], out itemId);

							if (Vp.player2_dressData.ContainsKey (itemId))
								Vp.player2_dressData [itemId] = itemType;
							else
								Vp.player2_dressData.Add (itemId, itemType);
						}
					}

					Vp.player2_SkinColor = P2_SkinColor;

					FriendsInFashionEvent.Add (Vp);
				}					

				yield return true;
			} else
				yield return false;
		} else
			yield return false;

		if (FriendsInFashionEvent.Count != 0) {

			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
			ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
			var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";

			for (int i = 0; i < container.transform.childCount; i++) {
				Destroy (container.transform.GetChild (i).gameObject);
			}

			FriendsInFashionEvent.ForEach (pair => {	
				foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
					if (friend.Id == pair.player1_id || friend.Id == pair.player2_id) {
						GameObject Go = Instantiate (FriendsManager.Instance.FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
						Go.transform.parent = container.transform;
						Go.transform.localScale = Vector3.one;
						Go.transform.localPosition = Vector3.zero;
						Go.GetComponent <AddFriendUi> ().thisData = friend;
						Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.ViewFriend;
						Go.name = friend.Username;
						Go.GetComponent <AddFriendUi> ().ViewFriendInString = "Fashion";
					}
				}
			});
		} else
			ShowPopOfDescription ("You do not have any of your friends participating in this event.");
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().interactable = true;
	}

	public IEnumerator GetFriendInCoOpEvent ()
	{
		yield return StartCoroutine (FriendsManager.Instance.GetAllFriendsList (false));

		FriendsInCoOp.Clear ();
		SelectedEventType = "CoOp";

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["data_type"] = "coop_show";

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW (PinekixUrls.ViewFriendsLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode dataArray = _jsnode ["data"];

				for (int i = 0; i < dataArray.Count; i++) {
					CoOpVotingPair Vp = new CoOpVotingPair ();

					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					/// set 1 data 
					/// Player 1 data
					Vp.set1_player1_id = int.Parse (dataArray [i] ["set1_player1_id"]);
					Vp.set1_player1_flatmate_id = int.Parse (dataArray [i] ["set1_player1_flatmate_id"]);
					Vp.set1_player1Name = dataArray [i] ["set1_player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set1_P1DressItems = dataArray [i] ["set1_player1_items"];


					for (int a = 0; a < set1_P1DressItems.Count; a++) {
						var itemId = 0;
						int.TryParse (set1_P1DressItems [a] ["item_id"], out itemId);

						var itemCat = set1_P1DressItems [a] ["item_type"];

						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set1_player1Gender = "Male";
							else
								Vp.set1_player1Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set1_player1Skin = itemId;
						} else {
							if (Vp.set1_player1_dressData.ContainsKey (itemId))
								Vp.set1_player1_dressData [itemId] = itemCat;
							else
								Vp.set1_player1_dressData.Add (itemId, itemCat);
						}
					}
					///player2 data
					Vp.set1_player2_id = int.Parse (dataArray [i] ["set1_player2_id"]);
					Vp.set1_player2_flatmate_id = int.Parse (dataArray [i] ["set1_player2_flatmate_id"]);
					Vp.set1_player2Name = dataArray [i] ["set1_player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set1_P2DressItems = dataArray [i] ["set1_player2_items"];


					for (int a = 0; a < set1_P2DressItems.Count; a++) {
						var itemId = int.Parse (set1_P2DressItems [a] ["item_id"]);
						var itemCat = set1_P2DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set1_player2Gender = "Male";
							else
								Vp.set1_player2Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set1_player2Skin = itemId;
						} else {
							if (Vp.set1_player2_dressData.ContainsKey (itemId))
								Vp.set1_player2_dressData [itemId] = itemCat;
							else
								Vp.set1_player2_dressData.Add (itemId, itemCat);
						}
					}

					/// set 2 data 
					/// Player 1 data
					Vp.set2_player1_id = int.Parse (dataArray [i] ["set2_player1_id"]);
					Vp.set2_player1_flatmate_id = int.Parse (dataArray [i] ["set2_player1_flatmate_id"]);
					Vp.set2_player1Name = dataArray [i] ["set2_player1_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set2_P1DressItems = dataArray [i] ["set2_player1_items"];


					for (int a = 0; a < set2_P1DressItems.Count; a++) {
						var itemId = int.Parse (set2_P1DressItems [a] ["item_id"]);
						var itemCat = set2_P1DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set2_player1Gender = "Male";
							else
								Vp.set2_player1Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set2_player1Skin = itemId;
						} else {
							if (Vp.set2_player1_dressData.ContainsKey (itemId))
								Vp.set2_player1_dressData [itemId] = itemCat;
							else
								Vp.set2_player1_dressData.Add (itemId, itemCat);
						}
					}

					///player2 data
					Vp.set2_player2_id = int.Parse (dataArray [i] ["set2_player2_id"]);
					Vp.set2_player2_flatmate_id = int.Parse (dataArray [i] ["set2_player2_flatmate_id"]);
					Vp.set2_player2Name = dataArray [i] ["set2_player2_name"].ToString ().Trim ("\"".ToCharArray ());

					JSONNode set2_P2DressItems = dataArray [i] ["set2_player2_items"];


					for (int a = 0; a < set2_P2DressItems.Count; a++) {
						var itemId = int.Parse (set2_P2DressItems [a] ["item_id"]);
						var itemCat = set2_P2DressItems [a] ["item_type"];
						if (itemCat.ToString ().Contains ("Gender")) {
							if (itemId == 1)
								Vp.set2_player2Gender = "Male";
							else
								Vp.set2_player2Gender = "Female";
						} else if (itemCat.ToString ().Contains ("SkinColor")) {
							Vp.set2_player2Skin = itemId;
						} else {
							if (Vp.set2_player2_dressData.ContainsKey (itemId))
								Vp.set2_player2_dressData [itemId] = itemCat;
							else
								Vp.set2_player2_dressData.Add (itemId, itemCat);
						}
					}

					FriendsInCoOp.Add (Vp);
				}					

				yield return true;
			} else
				yield return false;
		} else
			yield return false;

		if (FriendsInCoOp.Count != 0) {

			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
			ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
			var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";

			for (int i = 0; i < container.transform.childCount; i++) {
				Destroy (container.transform.GetChild (i).gameObject);
			}

			FriendsInCoOp.ForEach (pair => {	
				foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
					if (friend.Id == pair.set1_player1_id || friend.Id == pair.set1_player2_id || friend.Id == pair.set2_player1_id || friend.Id == pair.set2_player2_id) {
						GameObject Go = Instantiate (FriendsManager.Instance.FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
						Go.transform.parent = container.transform;
						Go.transform.localScale = Vector3.one;
						Go.transform.localPosition = Vector3.zero;
						Go.GetComponent <AddFriendUi> ().thisData = friend;
						Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.ViewFriend;
						Go.name = friend.Username;
						Go.GetComponent <AddFriendUi> ().ViewFriendInString = "CoOp";
					}
				}
			});
		} else
			ShowPopOfDescription ("You do not have any of your friends participating in this event.");
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().interactable = true;
	}

	public IEnumerator GetFriendInSocietyEvent ()
	{
		yield return StartCoroutine (FriendsManager.Instance.GetAllFriendsList (false));

		FriendsInSociety.Clear ();
		SelectedEventType = "SocietyEvent";

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["data_type"] = "society_show";

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW (PinekixUrls.ViewFriendsLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("Voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");

				JSONNode dataArray = _jsnode ["data"];

				for (int i = 0; i < dataArray.Count; i++) {
					SocietyVotingPair Vp = new SocietyVotingPair ();
					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);
					Vp.Gender = dataArray [i] ["gender"].ToString ();

					Vp.player1_id = int.Parse (dataArray [i] ["player1_id"]);
					Vp.player1_flatmate_id = int.Parse (dataArray [i] ["player1_flatmate_id"]);
					Vp.player1Name = dataArray [i] ["player1_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player1societyId = int.Parse (dataArray [i] ["player1_society_id"]);
					JSONNode p1DressData = dataArray [i] ["player1_item_data"];

					var P1_SkinColor = "";

					for (int a = 0; a < p1DressData.Count; a++) {						
						var itemType = p1DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P1_SkinColor = p1DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p1DressData [a] ["item_id"], out itemId);

							if (Vp.player1_dressData.ContainsKey (itemId))
								Vp.player1_dressData [itemId] = itemType;
							else
								Vp.player1_dressData.Add (itemId, itemType);
						}
					}
					Vp.player1_SkinColor = P1_SkinColor;

					Vp.player2_id = int.Parse (dataArray [i] ["player2_id"]);
					Vp.player2_flatmate_id = int.Parse (dataArray [i] ["player2_flatmate_id"]);
					Vp.player2Name = dataArray [i] ["player2_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player2societyId = int.Parse (dataArray [i] ["player2_society_id"]);

					JSONNode p2DressData = dataArray [i] ["player2_item_data"];

					var P2_SkinColor = "";

					for (int a = 0; a < p2DressData.Count; a++) {
						var itemType = p2DressData [a] ["item_type"];

						if (itemType.ToString ().Contains ("CharacterType"))
							P2_SkinColor = p2DressData [a] ["item_id"];
						else {
							var itemId = 0;
							int.TryParse (p2DressData [a] ["item_id"], out itemId);

							if (Vp.player2_dressData.ContainsKey (itemId))
								Vp.player2_dressData [itemId] = itemType;
							else
								Vp.player2_dressData.Add (itemId, itemType);
						}
					}

					Vp.player2_SkinColor = P2_SkinColor;

					FriendsInSociety.Add (Vp);
				}					

				yield return true;
			} else
				yield return false;
		} else
			yield return false;

		if (FriendsInSociety.Count != 0) {
			
			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
			ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
			var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";
				
			for (int i = 0; i < container.transform.childCount; i++) {
				Destroy (container.transform.GetChild (i).gameObject);
			}

			FriendsInSociety.ForEach (pair => {	
				foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
					if (friend.Id == pair.player1_id || friend.Id == pair.player2_id) {
						GameObject Go = Instantiate (FriendsManager.Instance.FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
						Go.transform.parent = container.transform;
						Go.transform.localScale = Vector3.one;
						Go.transform.localPosition = Vector3.zero;
						Go.GetComponent <AddFriendUi> ().thisData = friend;
						Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.ViewFriend;
						Go.name = friend.Username;
						Go.GetComponent <AddFriendUi> ().ViewFriendInString = "Society";
					}
				}
			});
		} else
			ShowPopOfDescription ("You do not have any of your friends participating in this event.");
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().interactable = true;
	}

	public IEnumerator GetFriendInCatWalkEvent ()
	{

		yield return StartCoroutine (FriendsManager.Instance.GetAllFriendsList (false));

		FriendsInCatwalkEvent.Clear ();
		SelectedEventType = "CatWalk";

		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["data_type"] = "catwalk_show";


		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW (PinekixUrls.ViewFriendsLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["description"].ToString ().Contains ("voting pair data are following") || _jsnode ["status"].ToString ().Contains ("200")) {
				print ("Success");	


				for (int i = 0; i < _jsnode ["data"].Count; i++) {
					CatWalkVotingPair vp = new CatWalkVotingPair ();
					JSONNode dataArray = _jsnode ["data"] [i];

					int.TryParse (dataArray ["event_id"], out vp.event_id);
					int.TryParse (dataArray ["pair_id"], out vp.pair_id);

					int.TryParse (dataArray ["player1_id"], out vp.player1_id);
					int.TryParse (dataArray ["player2_id"], out vp.player2_id);
					vp.player1Name = dataArray ["player1_name"].ToString ().Trim ('"');
					vp.player2Name = dataArray ["player2_name"].ToString ().Trim ('"');

                    var player1Items = dataArray ["player1_item_data"];

                    var player1Flatmate1data = new CatwalkFlatmateData();
                    var player1Flatmate2data = new CatwalkFlatmateData();
                    var player1Flatmate3data = new CatwalkFlatmateData();

                    for (int j = 0; j < player1Items.Count; j++) {
                        int Id = 0;
                        int.TryParse (player1Items [j] ["item_id"], out Id);
                        var FlatmateIdenifier =int.Parse(player1Items [j] ["item_type"].ToString ().Split('_')[1]);

                        var jhgdkhgfd = player1Items [j] ["item_type"].ToString ();
                        if (player1Items [j] ["item_type"].ToString ().Contains ("Flatmates")) {
                            if(FlatmateIdenifier == 0)
                            {
                                player1Flatmate1data.FlatmateId = Id;

                            }else if(FlatmateIdenifier ==1)
                            {
                                player1Flatmate2data.FlatmateId = Id;
                            }else if(FlatmateIdenifier == 2)
                            {
                                player1Flatmate3data.FlatmateId = Id;
                            }

                            //                          vp.Player1_flatmate_id.Add (Id);
                        }
                        if (player1Items [j] ["item_type"].ToString ().Contains ("Hair")) {

                            if(FlatmateIdenifier == 0)
                            {
                                player1Flatmate1data.HairsId = Id;

                            }else if(FlatmateIdenifier ==1)
                            {
                                player1Flatmate2data.HairsId = Id;
                            }else if(FlatmateIdenifier == 2)
                            {
                                player1Flatmate3data.HairsId = Id;
                            }
                            //                          vp.Player1_hair_id.Add (Id);
                        }
                        if (player1Items [j] ["item_type"].ToString ().Contains ("Clothes")) {
                            if(FlatmateIdenifier == 0)
                            {
                                player1Flatmate1data.Dress_ids.Add(Id);

                            }else if(FlatmateIdenifier ==1)
                            {
                                player1Flatmate2data.Dress_ids.Add(Id);
                            }else if(FlatmateIdenifier == 2)
                            {
                                player1Flatmate3data.Dress_ids.Add(Id);
                            }
                            //                          vp.Player1_dress_id.Add (Id);
                        }
                        //                      if (player1Items [j] ["item_type"].ToString ().Contains ("Shoes")) {
                        //                          vp.Player1_shoes_id.Add (Id);
                        //                      }
                        if (player1Items [j] ["item_type"].ToString ().Contains ("Gender")) {

                            if (Id == 1) {
                                vp.player1_Gender = "Male";
                            } else {
                                vp.player1_Gender = "Female";
                            }
                        }
                    }

                    vp.Player1_flatmate1_data = player1Flatmate1data;
                    vp.Player1_flatmate2_data = player1Flatmate2data;
                    vp.Player1_flatmate3_data = player1Flatmate3data;

                    var player2Items = dataArray ["player2_item_data"];
                    var player2Flatmate1data = new CatwalkFlatmateData();
                    var player2Flatmate2data = new CatwalkFlatmateData();
                    var player2Flatmate3data = new CatwalkFlatmateData();

                    for (int j = 0; j < player2Items.Count; j++)
                    {
                        int Id = 0;
                        int.TryParse(player2Items[j]["item_id"], out Id);

                        var FlatmateIdenifier = int.Parse(player2Items[j]["item_type"].ToString().Split('_')[1]);

                        if (player2Items[j]["item_type"].ToString().Contains("Flatmates"))
                        {

                            if (FlatmateIdenifier == 0)
                            {
                                player2Flatmate1data.FlatmateId = Id;

                            }
                            else if (FlatmateIdenifier == 1)
                            {
                                player2Flatmate2data.FlatmateId = Id;
                            }
                            else if (FlatmateIdenifier == 2)
                            {
                                player2Flatmate3data.FlatmateId = Id;
                            }
                            //                          vp.Player2_flatmate_id.Add (Id);
                        }
                        if (player2Items[j]["item_type"].ToString().Contains("Hair"))
                        {
                            if (FlatmateIdenifier == 0)
                            {
                                player2Flatmate1data.HairsId = Id;

                            }
                            else if (FlatmateIdenifier == 1)
                            {
                                player2Flatmate2data.HairsId = Id;
                            }
                            else if (FlatmateIdenifier == 2)
                            {
                                player2Flatmate3data.HairsId = Id;
                            }
                            //                          vp.Player2_hair_id.Add (Id);
                        }                       
                        if (player2Items[j]["item_type"].ToString().Contains("Clothes"))
                        {
                            if (FlatmateIdenifier == 0)
                            {
                                player2Flatmate1data.Dress_ids.Add(Id);

                            }
                            else if (FlatmateIdenifier == 1)
                            {
                                player2Flatmate2data.Dress_ids.Add(Id);
                            }
                            else if (FlatmateIdenifier == 2)
                            {
                                player2Flatmate3data.Dress_ids.Add(Id);
                            }
                            //                          vp.Player2_dress_id.Add (Id);
                        }
                        //                      if (player2Items [j] ["item_type"].ToString ().Contains ("Shoes")) {
                        //                          vp.Player2_shoes_id.Add (Id);
                        //                      }
                        if (player2Items[j]["item_type"].ToString().Contains("Gender"))
                        {
                            if (Id == 1)
                            {
                                vp.player2_Gender = "Male";
                            }
                            else
                            {
                                vp.player2_Gender = "Female";
                            }
                        }
                    }

                    vp.Player2_flatmate1_data = player2Flatmate1data;
                    vp.Player2_flatmate2_data = player2Flatmate2data;
                    vp.Player2_flatmate3_data = player2Flatmate3data;

					FriendsInCatwalkEvent.Add (vp);
				}

				yield return true;
			} else
				yield return false;
		} else
			yield return false;

		if (FriendsInCatwalkEvent.Count != 0) {

			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
			ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
			var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";

			for (int i = 0; i < container.transform.childCount; i++) {
				Destroy (container.transform.GetChild (i).gameObject);
			}

			FriendsInCatwalkEvent.ForEach (pair => {	
				foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
					if (friend.Id == pair.player1_id || friend.Id == pair.player2_id) {
						GameObject Go = Instantiate (FriendsManager.Instance.FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
						Go.transform.parent = container.transform;
						Go.transform.localScale = Vector3.one;
						Go.transform.localPosition = Vector3.zero;
						Go.GetComponent <AddFriendUi> ().thisData = friend;
						Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.ViewFriend;
						Go.name = friend.Username;
						Go.GetComponent <AddFriendUi> ().ViewFriendInString = "CatWalk";
					}
				}
			});
		} else
			ShowPopOfDescription ("You do not have any of your friends participating in this event.");
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().interactable = true;
	}

	public IEnumerator GetFriendInDecorEvent ()
	{
		yield return StartCoroutine (FriendsManager.Instance.GetAllFriendsList (false));

		FriendsInDecorEvent.Clear ();
		SelectedEventType = "Decor";
		TitleName.text = EventManagment.Instance.CurrentEvent.EventName;
		ThemeText.text = EventManagment.Instance.CurrentEvent.EventTheme;

		var jsonElement = new JSONClass ();

		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["event_id"] = EventManagment.Instance.CurrentEvent.Event_id.ToString ();
		jsonElement ["data_type"] = "decor_show";

//		{
//			"data_type" : "decor_show",
//			"player_id": 225,
//			"event_id" : 16
//		}

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());
		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 

		WWW www = new WWW (PinekixUrls.ViewFriendsLink, encoding.GetBytes (jsonElement.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = JSON.Parse (www.text);
			print ("_jsnode ==>> " + _jsnode.ToString ());
			if (_jsnode ["status"].ToString ().Contains ("200")) {

				var dataArray = _jsnode ["data"];

				/// this data fatching for player 2
				/// 
				for (int i = 0; i < dataArray.Count; i++) {
					
					VotingPairForDecor Vp = new VotingPairForDecor ();

					Vp.pair_id = int.Parse (dataArray [i] ["pair_id"]);
					Vp.event_id = int.Parse (dataArray [i] ["event_id"]);

					Vp.player1_id = int.Parse (dataArray [i] ["player1_id"]);
					Vp.player1Name = dataArray [i] ["player1_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player1_GroundTexture_Name = dataArray [i] ["player1_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player1_WallTexture_Name = dataArray [i] ["player1_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());

					List <int> ObjIds = new List<int> ();
					var DecoreObjectIds = dataArray [i] ["player1_object_id"];
					for (int x = 0; x < DecoreObjectIds.Count; x++) {
						ObjIds.Add (int.Parse (DecoreObjectIds [x] [0].ToString ().Trim ('"')));
					}

					List <string> TransfomArray = new List<string> ();
					var ObjPosition = dataArray [i] ["player1_object_position"];
					for (int x = 0; x < ObjPosition.Count; x++) {
						TransfomArray.Add (ObjPosition [x] [0].ToString ().Trim ('"'));
					}

					for (int x = 0; x < Mathf.Min (TransfomArray.Count, ObjIds.Count); x++) {
						DecorObject Decor = new DecorObject ();
						Decor.Id = ObjIds [x];
						string[] Values = TransfomArray [x].Split ('/');
						if (Values.Length == 3) {
							Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (Values [0].Trim ('|'));
							Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
							//							Decor.Rotation = ExtensionMethods.DeserializeVector3ArrayExtented (Values [2]);
							Decor.Direction = int.Parse (Values [2]);
						} else {
							Debug.LogError (" Transform of player 1 not in correct format");
						}

						Vp.Player1AllDecorOnFlat.Add (Decor);
					}


					/// this data fatching for player 2
					Vp.player2_id = int.Parse (dataArray [i] ["player2_id"]);
					Vp.player2Name = dataArray [i] ["player2_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player2_WallTexture_Name = dataArray [i] ["player2_wall_texture_name"].ToString ().Trim ("\"".ToCharArray ());
					Vp.player2_GroundTexture_Name = dataArray [i] ["player2_ground_texture_name"].ToString ().Trim ("\"".ToCharArray ());

					List <int> Player2ObjIds = new List<int> ();
					var Player2DecoreObjectIds = dataArray [i] ["player2_object_id"];
					for (int x = 0; x < Player2DecoreObjectIds.Count; x++) {
						Player2ObjIds.Add (int.Parse (Player2DecoreObjectIds [x] [0].ToString ().Trim ('"')));
					}


					List <string> Player2TransfomArray = new List<string> ();
					var Player2ObjPosition = dataArray [i] ["player2_object_position"];
					for (int x = 0; x < Player2ObjPosition.Count; x++) {
						Player2TransfomArray.Add (Player2ObjPosition [x] [0].ToString ().Trim ('"'));
					}

					for (int x = 0; x < Mathf.Min (Player2TransfomArray.Count, Player2ObjIds.Count); x++) {
						DecorObject Player2Decor = new DecorObject ();
						Player2Decor.Id = Player2ObjIds [x];
						string[] Values = Player2TransfomArray [x].Split ('/');
						if (Values.Length == 3) {
							Player2Decor.Position = ExtensionMethods.DeserializeVector3ArrayExtented (Values [0].Trim ('|'));
							Player2Decor.Scale = ExtensionMethods.DeserializeVector3ArrayExtented (Values [1].Trim ('|'));
							//							Player2Decor.Rotation = ExtensionMethods.DeserializeVector3ArrayExtented (Values [2]);
							Player2Decor.Direction = int.Parse (Values [2]);
						} else {
							Debug.LogError (" Transform of player 2 not in correct format");
						}

						Vp.Player2AllDecorOnFlat.Add (Player2Decor);
					}

					FriendsInDecorEvent.Add (Vp);
				}// for loop end


				yield return true;
			} else
				yield return false;
		} else
			yield return false;

		if (FriendsInDecorEvent.Count != 0) {

			ScreenManager.Instance.ShowPopup (ScreenManager.Instance.FriendsInvitePopUp);
			ScreenManager.Instance.FriendsInvitePopUp.transform.GetComponentInChildren <InputField> ().text = "";
			var container = ScreenManager.Instance.FriendsInvitePopUp.GetComponentInChildren <GridLayoutGroup> ();
			ScreenManager.Instance.FriendsInvitePopUp.transform.FindChild ("Message").GetComponent <Text> ().text =	"";

			for (int i = 0; i < container.transform.childCount; i++) {
				Destroy (container.transform.GetChild (i).gameObject);
			}

			FriendsInDecorEvent.ForEach (pair => {	
				foreach (var friend in FriendsManager.Instance.AllAddedFriends) {
					if (friend.Id == pair.player1_id || friend.Id == pair.player2_id) {
						GameObject Go = Instantiate (FriendsManager.Instance.FriendUiPrefab, Vector3.zero, Quaternion.identity)as GameObject;
						Go.transform.parent = container.transform;
						Go.transform.localScale = Vector3.one;
						Go.transform.localPosition = Vector3.zero;
						Go.GetComponent <AddFriendUi> ().thisData = friend;
						Go.GetComponent <AddFriendUi> ().thisData.Type = FriendData.FriendType.ViewFriend;
						Go.name = friend.Username;
						Go.GetComponent <AddFriendUi> ().ViewFriendInString = "Decor";
					}
				}
			});
		} else
			ShowPopOfDescription ("You do not have any of your friends participating in this event.");
		ScreenManager.Instance.EventsIntroScreen.transform.FindChild ("View Friend").GetComponent<Button> ().interactable = true;
	}
	//End line.............................................................................

	public void CheckVisibilityofBonusButton ()
	{
		if (PersistanceManager.Instance.GetSavedVotingBonuses ().Count > 0)
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (true);
		else
			ScreenManager.Instance.VotingScreen.transform.FindChild ("Bonus").gameObject.SetActive (false);
	}
}



[Serializable]
public class VotingPair
{
	public int pair_id;
	public int event_id;
	public string Gender;

	[Header ("Player_1 data")]
	public string player1Name;
	public int player1_id;
	public int player1_flatmate_id;
	public string player1_SkinColor;
	public Dictionary <int , string> player1_dressData = new Dictionary<int, string> ();

	[Header ("Player_2 data")]
	public string player2Name;
	public int player2_id;
	public int player2_flatmate_id;
	public string player2_SkinColor;
	public Dictionary <int , string> player2_dressData = new Dictionary<int, string> ();
}

public class SocietyVotingPair: VotingPair
{
	public int player1societyId;
	public int player2societyId;
}

[Serializable]
public class CoOpVotingPair
{
	public int pair_id;
	public int event_id;

	[Header ("Set_1 data")]
	public string set1_player1Name;
	public string set1_player1Gender;
	public int set1_player1_id;
	public int set1_player1_flatmate_id;
	public int set1_player1Skin;
	public Dictionary <int, string> set1_player1_dressData = new Dictionary<int, string> ();

	public string set1_player2Name;
	public string set1_player2Gender;
	public int set1_player2_id;
	public int set1_player2_flatmate_id;
	public int set1_player2Skin;
	public Dictionary <int , string> set1_player2_dressData = new Dictionary<int, string> ();

	[Header ("Set_2 data")]
	public string set2_player1Name;
	public string set2_player1Gender;
	public int set2_player1_id;
	public int set2_player1_flatmate_id;
	public int set2_player1Skin;
	public Dictionary <int , string> set2_player1_dressData = new Dictionary<int, string> ();

	public string set2_player2Name;
	public string set2_player2Gender;
	public int set2_player2_id;
	public int set2_player2_flatmate_id;
	public int set2_player2Skin;
	public Dictionary <int , string> set2_player2_dressData = new Dictionary<int, string> ();

}

[Serializable]
public class CatWalkVotingPair
{
	public int pair_id;
	public int event_id;

	[Header ("Player_1 data")]
	public string player1Name;
	public int player1_id;
	public string player1_Gender;

    public CatwalkFlatmateData Player1_flatmate1_data = new CatwalkFlatmateData();
    public CatwalkFlatmateData Player1_flatmate2_data = new CatwalkFlatmateData();
    public CatwalkFlatmateData Player1_flatmate3_data = new CatwalkFlatmateData();

//    public List<int> Player1_flatmate_id = new List<int> ();
//	public List<int> Player1_dress_id = new List<int> ();
//	public List<int> Player1_hair_id = new List<int> ();
//	public List<int> Player1_shoes_id = new List<int> ();

	[Header ("Player_2 data")]
	public string player2Name;
	public int player2_id;
	public string player2_Gender;

    public CatwalkFlatmateData Player2_flatmate1_data = new CatwalkFlatmateData();
    public CatwalkFlatmateData Player2_flatmate2_data = new CatwalkFlatmateData();
    public CatwalkFlatmateData Player2_flatmate3_data = new CatwalkFlatmateData();
//
//	public List<int> Player2_flatmate_id = new List<int> ();
//	public List<int> Player2_dress_id = new List<int> ();
//	public List<int> Player2_hair_id = new List<int> ();
//	public List<int> Player2_shoes_id = new List<int> ();

}
public class CatwalkFlatmateData
{
    public int FlatmateId;
    public List<int> Dress_ids = new List<int>();
    public int HairsId;
}


[Serializable]
public class VotingPairForDecor
{
	public int pair_id;
	public int event_id;


	[Header ("Player_1 data")]
	public string player1Name;
	public int player1_id;



	public string player1_GroundTexture_Name;
	public string player1_WallTexture_Name;

	public List<DecorObject> Player1AllDecorOnFlat = new List<DecorObject> ();
	//	public Dictionary <string , int> player1_object_id = new Dictionary<string, int> ();
	//	public Dictionary <string , int> player1_object_position = new Dictionary<string, int> ();


	[Header ("Player_2 data")]
	public string player2Name;
	public int player2_id;


	public string player2_GroundTexture_Name;
	public string player2_WallTexture_Name;

	public List<DecorObject> Player2AllDecorOnFlat = new List<DecorObject> ();

	//	public Dictionary <string , string> player2_object_id = new Dictionary<string, string> ();
	//	public Dictionary <string , string> player2_object_position = new Dictionary<string, string> ();


}

/// <summary>
/// Class for saving Voting bonus in PlayerPrefs.
/// </summary>
[Serializable]
public class VotingBonus
{
	public int Id;
	// Id of event in which this bonus is genrated, so that it can't be applied to same event.
	public eventType Type;
	// Type of events in which its genrated.
	public DateTime DestroyTime;
	//Timer at which this bonus will end.
}

[Serializable]
public class DecorObject
{
	public int Id;
	public Vector3 Position;
	public Vector3 Scale;
	public Vector3 Rotation;
	public int Direction;
}


public class ResultObject
{
	public int player1_id;
	public string player1_name;
	public int player1_voteCount;
	public int player1_votingBonus;
	public int player1_friendBonus;

	public int player2_id;
	public string player2_name;
	public int player2_voteCount;
	public int player2_votingBonus;
	public int player2_friendBonus;
}