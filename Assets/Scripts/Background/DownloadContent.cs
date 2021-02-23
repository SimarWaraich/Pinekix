/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 07 July 2k16
/// This script will be used to download the last status and  of the game from the server
/// </summary>


using UnityEngine;
using System.Collections;
using System;
using Simple_JSON;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using SimpleJSON;


[Serializable]
public class DownloadedItems
{
	public string Name;
	public int Item_id;
	public string Category;
	public string SubCategory;
}

public class DownloadContent : Singleton<DownloadContent>
{
	public GameObject LoadingScreen;
	public GameObject LoginScreenODownload;

	public static float TotelDownloadItem;
	public float totelItem;
	public float FlatdataCount;
	public float percentege = 0.0f;
	float DownloadedItemCount;
	public List<DownloadedItems> downloaded_items = new List<DownloadedItems> ();
	public Dictionary<string, string> link_list = new Dictionary<string, string> ();

	public bool downloadcompleted;
	public Text reporttext;
	public float downloading_progress;

	//	GameObject internetinfo;

	void Awake ()
	{
		this.Reload ();
	}

	void Start ()
	{
//		internetinfo = GameObject.Find ("InternetReachability_BackGround");
		LoadingScreen = GameObject.Find ("LoadingScreen");
		LoginScreenODownload = GameObject.Find ("LoginPanel");
		InvokeRepeating ("CoroutineStartingCheck", 1f, 1f);
	}


	void CoroutineStartingCheck ()
	{
		ConnectionController.Instance.CheckServices ();
//		Debug.LogError ("Starting Download all data");
		if (ConnectionController.Instance._internetOutput && ConnectionController.Instance._serverOutput) {
			StartCoroutines ();
			CancelInvoke ("CoroutineStartingCheck");
		}

	}

	public void StartCoroutines ()
	{
		StopAllCoroutines ();
		StartCoroutine (StartCoroutineInSequence ());
	}

	/// <summary>
	/// Starts all coroutines in sequence.
	/// </summary>
	/// <returns>The coroutine in sequence.</returns>
	public IEnumerator StartCoroutineInSequence ()
	{
//		AssetBundleManager.UnloadAll ();  
		downloaded_items.Clear ();
		link_list.Clear ();

		yield return new WaitForSeconds (1f);
		yield return StartCoroutine (GetFData ());
		yield return StartCoroutine (DownloadAllData ());
        yield return StartCoroutine (ClaimedRewards ());
        yield return StartCoroutine(SeasonalClothesManager.Instance.IGetSavedSeasonalClothes());
		yield return StartCoroutine (GetFlat ());
		yield return StartCoroutine (PlayerManager.Instance.GetFlatmateDataOfRealPlayer ());
		yield return StartCoroutine (GetPurchasedData ());
//		yield return RetainClaimedRewards ();

		downloadcompleted = true;
		LoadingGameAssets.CharSelectionsConfrim = true;
		if (percentege > 98) {				

			if (LoginScreenODownload != null) {
				LoginScreenODownload.SetActive (false);
			}
			//			GameObject.Find ("RegistrationPanel").SetActive (false);
			//			GameObject.Find ("ForgotPasswordPanel").SetActive (false);
			//			GameObject.Find ("CharacterCustomizationPanel").SetActive (false);
			LoadingScreen.SetActive (false);
			Destroy (LoadingScreen.gameObject);			

		}
		GameManager.Instance.gameObject.GetComponent<Tutorial> ().Invoke ("TutorialStart", 0.5f);		
		GameManager.Instance.gameObject.GetComponent<Tutorial> ().Invoke ("UpdateTutorial", 1f);


	}

	AssetBundle bundle;

	/// <summary>
	///Downloads all data from server using webservice
	/// </summary>
	/// <returns>The all data.</returns>
	IEnumerator DownloadAllData ()
	{

		while (!Caching.ready)
			yield return null;

//		Caching.CleanCache ();

		yield return ConnectionController.Instance.IeCheckServices ();

		if (!ConnectionController.Instance._internetOutput || !ConnectionController.Instance._serverOutput) {
			StartCoroutines ();
		} else {
			yield return null;
		}

		Caching.CleanCache ();

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		int Id = PlayerPrefs.GetInt ("PlayerId");
		PlayerData pd = new PlayerData ();
		pd.player_id = Id;
		string json = JsonUtility.ToJson (pd);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());
		
		WWW www = new WWW ("http://pinekix.ignivastaging.com/items/getalldata", encoding.GetBytes (json), postHeader);

		yield return www;
//			print ("Done----"+ www.text);

		List<string> datakeys = new List<string> ();

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			foreach (var key in _jsnode["data"].Keys) {
				datakeys.Add (key);
			}
//			print ("Json Parse --------" + _jsnode ["data"].Count);

			TotelDownloadItem = _jsnode ["data"] ["Decor"].Count + _jsnode ["data"] ["Clothing"].Count + _jsnode ["data"] ["Expansion"].Count + _jsnode ["data"] ["Saloon"].Count + _jsnode ["data"] ["Flatmates"] ["Flatmates"].Count + FlatdataCount;
			totelItem = TotelDownloadItem;

			for (int i = 0; i < _jsnode ["data"].Count; i++) {


				
//				print (datakeys [i]);
				if (datakeys [i] == "Decor") {
					for (int x = 0; x < _jsnode ["data"] ["Decor"].Count; x++) {
						for (int y = 0; y < _jsnode ["data"] ["Decor"] [x].Count; y++) {
							//Here is the info and the link to the asset package
							while (!Caching.ready)
								yield return null;

//							print (_jsnode ["data"] ["Decor"] [x] [y] ["name"].ToString () + "   url ----" + _jsnode ["data"] ["Decor"] [x] [y] ["image"].ToString ());

							string s = _jsnode ["data"] ["Decor"] [x] [y] ["image"].ToString ();
							string myString = s.Trim ('"');


							if (myString.Contains ("null")) {
//								print ("empty string error");
							} else {

								if (link_list.ContainsKey (_jsnode ["data"] ["Decor"] [x] [y] ["name"].ToString ()))
									link_list [_jsnode ["data"] ["Decor"] [x] [y] ["name"].ToString ()] = myString;
								else
									link_list.Add (_jsnode ["data"] ["Decor"] [x] [y] ["name"].ToString (), myString);



//								yield return StartCoroutine (AssetBundleManager.downloadAssetBundle (myString, 1));
//
//								AssetBundle bundle = AssetBundleManager.getAssetBundle (myString, 1);

								string Name = _jsnode ["data"] ["Decor"] [x] [y] ["name"].ToString ().Trim ('"');

								List<string> subcatlist = new List<string> ();

								foreach (var _subcat in _jsnode ["data"] ["Decor"].Keys) {
									subcatlist.Add (_subcat);
								}

								string subCategory = subcatlist [x];// for each key/values

								var path = AssetsPaths.DecorResourcespath + "/" + subCategory + "/" + Name;

								var target = Resources.Load<Decor3DView> (path);

								if (target != null) {
//									var _x = bundle.GetAllAssetNames ();
//									GameObject target = null;
//									foreach (var _y in _x) {
//										if (_y.Contains (".prefab"))
//											target = (GameObject)bundle.LoadAsset (_y);
//									}
//                                  print (_jsnode ["data"] ["Decor"] [x] [y] ["coins"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Decor"] [x] [y] ["level_no"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Decor"] [x] [y] ["gems"].ToString ().Trim ('"'));

									int coin = int.Parse (_jsnode ["data"] ["Decor"] [x] [y] ["coins"].ToString ().Trim ('"'));
									int level = int.Parse (_jsnode ["data"] ["Decor"] [x] [y] ["level_no"].ToString ().Trim ('"'));
									int gems = int.Parse (_jsnode ["data"] ["Decor"] [x] [y] ["gems"].ToString ().Trim ('"'));
                                    int VipSub = int.Parse (_jsnode ["data"] ["Decor"] [x] [y] ["vip_subscription"].Value);

									int key = int.Parse (_jsnode ["data"] ["Decor"] [x] [y] ["id"].ToString ().Trim ('"'));
									DownloadedItems item = new DownloadedItems ();
									item.Name = Name;
									item.Category = "Decor";
									item.SubCategory = subCategory;
									item.Item_id = key;
									downloaded_items.Add (item);

                                    DecorController.Instance.AddDecorToList (key, target, Name, subCategory, level, gems, coin, VipSub);
								}
							}
						}
						///percentege calculater
						DownloadedItemCount++;

						percentege = DownloadedItemCount * 100 / TotelDownloadItem;
						LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value = percentege;
						LoadingScreen.GetComponent<LoadingGameAssets> ().loadingValue.text = "Loading..." + Mathf.Round (LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value).ToString () + "%";
//						print (percentege + "%");
					}
				}
				if (datakeys [i] == "Clothing") {
					for (int x = 0; x < _jsnode ["data"] ["Clothing"].Count; x++) {
						for (int y = 0; y < _jsnode ["data"] ["Clothing"] [x].Count; y++) {
							//Here is the info and the link to the asset package


							// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
//							print (_jsnode ["data"] ["Clothing"] [x] [y] ["name"].ToString () + "   url ----" + _jsnode ["data"] ["Clothing"] [x] [y] ["image"].ToString ());

							string s = _jsnode ["data"] ["Clothing"] [x] [y] ["image"].ToString ();
							string myString = s.Trim ('"');




							if (myString.Contains ("null") || string.IsNullOrEmpty (myString)) {
//								print ("empty string error");
							} else {
								if (link_list.ContainsKey (_jsnode ["data"] ["Clothing"] [x] [y] ["name"].ToString ()))
									link_list [_jsnode ["data"] ["Clothing"] [x] [y] ["name"].ToString ()] = myString;
								else
									link_list.Add (_jsnode ["data"] ["Clothing"] [x] [y] ["name"].ToString (), myString);


//								yield return StartCoroutine (AssetBundleManager.downloadAssetBundle (myString, 1));
//
//								AssetBundle bundle = AssetBundleManager.getAssetBundle (myString, 1);

								string Name = _jsnode ["data"] ["Clothing"] [x] [y] ["name"].ToString ().Trim ('"');

								var path = AssetsPaths.DressesResourcespath + "/" + Name;

								var target = Resources.Load<DressInfo> (path);

//								if (bundle != null)
								if (target) {
//									var _x = bundle.GetAllAssetNames ();
//									GameObject target = null;
//									foreach (var _y in _x) {
//										if (_y.Contains (".prefab"))
//											target = (GameObject)bundle.LoadAsset (_y);
//									}
										

			

									List<string> subcatlist = new List<string> ();

									foreach (var _subcat in _jsnode ["data"] ["Clothing"].Keys) {
										subcatlist.Add (_subcat);
									}

									string subCategory = subcatlist [x];// for each key/values

//									print (_jsnode ["data"] ["Clothing"] [x] [y] ["coins"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Clothing"] [x] [y] ["level_no"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Clothing"] [x] [y] ["gems"].ToString ().Trim ('"'));
									int coin = int.Parse (_jsnode ["data"] ["Clothing"] [x] [y] ["coins"].ToString ().Trim ('"'));
									int level = int.Parse (_jsnode ["data"] ["Clothing"] [x] [y] ["level_no"].ToString ().Trim ('"'));
									int gems = int.Parse (_jsnode ["data"] ["Clothing"] [x] [y] ["gems"].ToString ().Trim ('"'));
                                    int VipSub = int.Parse (_jsnode ["data"] ["Clothing"] [x] [y] ["vip_subscription"].Value);


									int key = int.Parse (_jsnode ["data"] ["Clothing"] [x] [y] ["id"].ToString ().Trim ('"'));
									DownloadedItems item = new DownloadedItems ();
									item.Name = target.GetComponent<DressInfo> ().name;
									item.Category = "Clothing";
									item.SubCategory = subCategory;
									item.Item_id = key;
									downloaded_items.Add (item);

                                    PurchaseDressManager.Instance.UpdateDress (target, Name, subCategory, level, coin, gems, key,VipSub);

								}
							}								
						}
						DownloadedItemCount++;
						percentege = DownloadedItemCount * 100 / TotelDownloadItem;
						LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value = percentege;
						LoadingScreen.GetComponent<LoadingGameAssets> ().loadingValue.text = "Loading..." + Mathf.Round (LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value).ToString () + "%";
//						print (percentege + "%");
					}
				}
				if (datakeys [i] == "Expansion") {
					for (int x = 0; x < _jsnode ["data"] ["Expansion"].Count; x++) {
						for (int y = 0; y < _jsnode ["data"] ["Expansion"] [x].Count; y++) {
							//Here is the info and the link to the asset package
							while (!Caching.ready)
								yield return null;

//							print (_jsnode ["data"] ["Expansion"] [x] [y] ["name"].ToString () + "   url ----" + _jsnode ["data"] ["Expansion"] [x] [y] ["image"].ToString ());

							string s = _jsnode ["data"] ["Expansion"] [x] [y] ["image"].ToString ();
							string myString = s.Trim ('"');

							if (myString.Contains ("null")) {
//								print ("empty string error");
							} else {


								if (link_list.ContainsKey (_jsnode ["data"] ["Expansion"] [x] [y] ["name"].ToString ()))
									link_list [_jsnode ["data"] ["Expansion"] [x] [y] ["name"].ToString ()] = myString;
								else
									link_list.Add (_jsnode ["data"] ["Expansion"] [x] [y] ["name"].ToString (), myString);




								yield return StartCoroutine (AssetBundleManager.downloadAssetBundle (myString, 1));

								AssetBundle bundle = AssetBundleManager.getAssetBundle (myString, 1);
											

								if (bundle != null) {
									var _x = bundle.GetAllAssetNames ();
									GameObject target = null;
									foreach (var _y in _x) {
										if (_y.Contains (".prefab"))
											target = (GameObject)bundle.LoadAsset (_y);
									}


									string Name = _jsnode ["data"] ["Expansion"] [x] [y] ["name"].ToString ();

									List<string> subcatlist = new List<string> ();

									foreach (var _subcat in _jsnode ["data"] ["Expansion"].Keys) {
										subcatlist.Add (_subcat);
									}

									string subCategory = subcatlist [x];// for each key/values

//									print (_jsnode ["data"] ["Expansion"] [x] [y] ["coins"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Expansion"] [x] [y] ["level_no"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Expansion"] [x] [y] ["gems"].ToString ().Trim ('"'));


									int key = int.Parse (_jsnode ["data"] ["Expansion"] [x] [y] ["id"].ToString ().Trim ('"'));
									DownloadedItems item = new DownloadedItems ();
									item.Name = Name;
									item.Category = "Expansion";
									item.SubCategory = subCategory;
									item.Item_id = key;
									downloaded_items.Add (item);

									RoomPurchaseManager.Instance.AddFlatInfo (target);

								}
							}
						}
						DownloadedItemCount++;
						percentege = DownloadedItemCount * 100 / TotelDownloadItem;
						LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value = percentege;
						LoadingScreen.GetComponent<LoadingGameAssets> ().loadingValue.text = "Loading..." + Mathf.Round (LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value).ToString () + "%";
//						print (percentege + "%");
					}
				}
				if (datakeys [i] == "Saloon") {
					for (int x = 0; x < _jsnode ["data"] ["Saloon"].Count; x++) {
						for (int y = 0; y < _jsnode ["data"] ["Saloon"] [x].Count; y++) {

							// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
//							print (_jsnode ["data"] ["Saloon"] [x] [y] ["name"].ToString () + "   url ----" + _jsnode ["data"] ["Saloon"] [x] [y] ["image"].ToString ());

							string s = _jsnode ["data"] ["Saloon"] [x] [y] ["image"].ToString ();
							string myString = s.Trim ('"');




							if (myString.Contains ("null")) {
//								print ("empty string error");
							} else {



								if (link_list.ContainsKey (_jsnode ["data"] ["Saloon"] [x] [y] ["name"].ToString ()))
									link_list [_jsnode ["data"] ["Saloon"] [x] [y] ["name"].ToString ()] = myString;
								else
									link_list.Add (_jsnode ["data"] ["Saloon"] [x] [y] ["name"].ToString (), myString);

//								yield return StartCoroutine (AssetBundleManager.downloadAssetBundle (myString, 1));//
//								AssetBundle bundle = AssetBundleManager.getAssetBundle (myString, 1);
//								if (bundle != null) 
//                                {
//									var _x = bundle.GetAllAssetNames ();
//									GameObject target = null;
//									foreach (var _y in _x) {
//										if (_y.Contains (".prefab"))
//											target = (GameObject)bundle.LoadAsset (_y);
//									}
								var hairName = _jsnode ["data"] ["Saloon"] [x] [y] ["name"].ToString ().Trim ('"');

								var hairspath = AssetsPaths.SaloonResourcespath + "/" + hairName;
								var saloon = Resources.Load<SaloonInfo> (hairspath);

								if (saloon) {
									List<string> subcatlist = new List<string> ();

									foreach (var _subcat in _jsnode ["data"] ["Saloon"].Keys) {
										subcatlist.Add (_subcat);
									}

									string subCategory = subcatlist [x];// for each key/values

//									print (_jsnode ["data"] ["Saloon"] [x] [y] ["coins"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Saloon"] [x] [y] ["level_no"].ToString ().Trim ('"'));
//									print (_jsnode ["data"] ["Saloon"] [x] [y] ["gems"].ToString ().Trim ('"'));
									int coin = int.Parse (_jsnode ["data"] ["Saloon"] [x] [y] ["coins"].ToString ().Trim ('"'));
									int level = int.Parse (_jsnode ["data"] ["Saloon"] [x] [y] ["level_no"].ToString ().Trim ('"'));
									int gems = int.Parse (_jsnode ["data"] ["Saloon"] [x] [y] ["gems"].ToString ().Trim ('"'));
                                    int VipSub = int.Parse (_jsnode ["data"] ["Saloon"] [x] [y] ["vip_subscription"].Value);
                                    int id = int.Parse (_jsnode ["data"] ["Saloon"] [x] [y] ["id"].ToString ().Trim ('"'));

									DownloadedItems item = new DownloadedItems ();
									item.Name = saloon.GetComponent<SaloonInfo> ().name;
									item.Category = "Saloon";
									item.SubCategory = subCategory;
									item.Item_id = id;
									downloaded_items.Add (item);

                                    PurchaseSaloonManager.Instance.UpdateSaloon (saloon, name, subCategory, level, coin, gems, id, VipSub);


								}
							}
						}
						DownloadedItemCount++;
						percentege = DownloadedItemCount * 100 / TotelDownloadItem;
						LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value = percentege;
						LoadingScreen.GetComponent<LoadingGameAssets> ().loadingValue.text = "Loading..." + Mathf.Round (LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value).ToString () + "%";
//						print (percentege + "%");
					}
				}
				if (datakeys [i] == "Flatmates") {


					for (int z = 0; z < _jsnode ["data"] ["Flatmates"] ["Flatmates"].Count; z++) {
//						print (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["name"].ToString () + "   url ----" + _jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["image"].ToString ());

						string string_link = _jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["image"].ToString ();
						string myString = string_link.Trim ('"');



						if (myString.Contains ("null") || string.IsNullOrEmpty (myString)) {
//							print ("empty string error");
						} else {


							if (link_list.ContainsKey (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["name"].ToString ()))
								link_list [_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["name"].ToString ()] = myString;
							else
								link_list.Add (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["name"].ToString (), myString);


//
//							yield return StartCoroutine (AssetBundleManager.downloadAssetBundle (myString, 1));
//
//							AssetBundle bundle = AssetBundleManager.getAssetBundle (myString, 1);
//
//							if (bundle != null) {
//								var _x = bundle.GetAllAssetNames ();
//								GameObject target = null;
//								foreach (var _y in _x) {
//									if (_y.Contains (".prefab"))
//										target = (GameObject)bundle.LoadAsset (_y);
//								}


							string Name = _jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["name"].ToString ().Trim ('"');

							List<string> subcatlist = new List<string> ();

							foreach (var _subcat in _jsnode ["data"] ["Flatmate"]["Flatmates"].Keys) {
								subcatlist.Add (_subcat);
							}

							string subCategory = "Flatmates";// for each key/values

//								print (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["coins"].ToString ().Trim ('"'));
//								print (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["level_no"].ToString ().Trim ('"'));
//								print (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["gems"].ToString ().Trim ('"'));
							int coin = int.Parse (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["coins"].ToString ().Trim ('"'));
							int level = int.Parse (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["level_no"].ToString ().Trim ('"'));
							int gems = int.Parse (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["gems"].ToString ().Trim ('"'));
                            int VipSub = int.Parse (_jsnode ["data"] ["Flatmates"]["Flatmates"] [z] ["vip_subscription"].Value);

							int key = int.Parse (_jsnode ["data"] ["Flatmates"] ["Flatmates"] [z] ["id"].ToString ().Trim ('"'));
							DownloadedItems item = new DownloadedItems ();
							item.Name = Name;
							item.Category = "Flatmates";
							item.SubCategory = subCategory;
							item.Item_id = key;
							downloaded_items.Add (item);


                            RoommateManager.Instance.UpdateRoommates (Name, key, level, coin, gems, VipSub);
//							}
						}

						DownloadedItemCount++;
						percentege = DownloadedItemCount * 100 / TotelDownloadItem;
						LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value = percentege;
						LoadingScreen.GetComponent<LoadingGameAssets> ().loadingValue.text = "Loading..." + Mathf.Round (LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value).ToString () + "%";
						print (percentege + "%");
					}
				}
			}
		} else {
			yield return false;
		}
	}


	/// <summary>
	/// Gets the purchased data of downloaded objects
	/// </summary>
	/// <returns>The purchased data.</returns>
	IEnumerator GetPurchasedData ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		int Id = PlayerPrefs.GetInt ("PlayerId");
		PlayerData pd = new PlayerData ();
		pd.player_id = Id;
		string json = JsonUtility.ToJson (pd);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/items/viewpurchaseditem", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			for (int i = 0; i < _jsnode ["data"].Count; i++) {

				int id = 0;
				int.TryParse (_jsnode ["data"] [i] ["item_id"].ToString ().Trim ('"'), out id);

				for (int j = 0; j < downloaded_items.Count; j++) {
					if (id == downloaded_items [j].Item_id) {

						Dictionary<int, DecorData> datalist = new Dictionary<int, DecorData> ();
						switch (downloaded_items [j].Category) {
						case "Clothing":
							foreach (var dressitem in PurchaseDressManager.Instance.AllDresses) {
								if (dressitem.Name == downloaded_items [j].Name) {
									dressitem.Unlocked = true;
									dressitem.Purchased = true;
								}
							}
							break;
						case "Decor":
							
							foreach (var decoritem in DecorController.Instance.AllDecores) {
								if (decoritem.Name == downloaded_items [j].Name) {
									decoritem.Purchased = true;
									decoritem.Unlocked = true;
									datalist.Add (downloaded_items [j].Item_id, decoritem);
								}
							}

							break;
						case "Expansion":
							break;
						case "Saloon":

							foreach (var saloonitem in PurchaseSaloonManager.Instance.AllItems) {
								if (saloonitem.Name == downloaded_items [j].Name) {
									saloonitem.Unlocked = true;
									saloonitem.Purchased = true;
								}
							}
							break;
						case "Flatmates":
							print (downloaded_items [j].Name);
							foreach (var flatmate in RoommateManager.Instance.AllRoommatesData) {
								if (flatmate.Name.Trim ('"') == downloaded_items [j].Name.Trim ('"')) {
									flatmate.Unlocked = true;
									flatmate.Hired = true;
									RoommateGetData _rmgdata = new RoommateGetData (PlayerPrefs.GetInt ("PlayerId"), downloaded_items [j].Item_id);

									yield return StartCoroutine (GetFlatmateData (_rmgdata, flatmate));
								}
							}
							break;
						}

						yield return StartCoroutine (GetPositions (datalist));
					}
				}
			}
		}
	}





	public IEnumerator GetFlatmateData (RoommateGetData _rmgdata, RoommateData flatmate)
	{
		CoroutineWithData cd = new CoroutineWithData (this, DownloadFlatmateData (_rmgdata, flatmate));
		yield return cd.coroutine;

		if (cd.result != null) {


			foreach (var Checkdata in RoommateManager.Instance.AllRoommatesData) {
				if (Checkdata == flatmate)
					Checkdata.Hired = true;
			}
				
			var target = GameManager.Instance.WayPoints [UnityEngine.Random.Range (0, GameManager.Instance.WayPoints.Count)];

//			GameObject parent = new GameObject ();

			var flatmate_Go = (GameObject)Instantiate (flatmate.Prefab, Vector2.zero, Quaternion.identity);
			flatmate_Go.transform.position = target.transform.position;

			flatmate_Go.name = flatmate.Name;
//			Destroy (flatmate_Go.GetComponent<ManageOrderInLayer> ());
//			flatmate_Go.AddComponent<ManageOrderInLayer> ();
			flatmate_Go.transform.eulerAngles = new Vector3 (0, 0, 0);
			flatmate_Go.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
//			flatmate_Go.transform.SetParent (parent.transform, false);
//			flatmate_Go.transform.localPosition = new Vector3 (0, 3, 0);

			flatmate_Go.AddComponent<RoomMateMovement> ().currentWaypoint = target.GetComponent<WayPoint> ();
			flatmate_Go.GetComponent<Flatmate> ().data = flatmate;

			Destroy (flatmate_Go.GetComponent<GenerateMoney> ());
			flatmate_Go.AddComponent<GenerateMoney> ().MoneyIcon = flatmate_Go.transform.FindChild ("low money").gameObject;
			flatmate_Go.GetComponent <GenerateMoney> ().MoneyIcon.transform.localPosition = new Vector3 (flatmate_Go.GetComponent <GenerateMoney> ().MoneyIcon.transform.localPosition.x,
				flatmate_Go.GetComponent <GenerateMoney> ().MoneyIcon.transform.localPosition.y, -1);

			flatmate_Go.GetComponent<Flatmate> ().HireThisRoommate ();

			flatmate_Go.SetMaterialRecursively ();

			var _xyz = flatmate_Go.GetComponentsInChildren<DressParts> (true);

			foreach (var dress in PurchaseDressManager.Instance.AllDresses) {
				foreach (var flatmate_dress in flatmate.Dress) {
					if (dress.Id == flatmate_dress.Value) {
//						if (flatmate_Go.GetComponent<CharacterProperties> ().PlayerType == "White")
						for (int i = 0; i < Mathf.Min (dress.PartName.Length, dress.DressesImages.Length); i++) {
							foreach (var _x in _xyz) {
								if (_x.name == dress.PartName [i]) {
									_x.GetComponent<SpriteRenderer> ().sprite = dress.DressesImages [i];

								}
							}
						}
					}

//					if (dress.id == flatmate_dress.Value) {
//						if (flatmate_Go.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//							for (int i = 0; i < Mathf.Min (dress.PartName.Length, dress.Brown_Images.Length); i++) {
//								foreach (var _x in _xyz) {
//									if (_x.name == dress.PartName [i]) {
//										_x.GetComponent<SpriteRenderer> ().sprite = dress.Brown_Images [i];
//
//									}
//								}
//							}
//					}

//					if (dress.id == flatmate_dress.Value) {
//						if (flatmate_Go.GetComponent<CharacterProperties> ().PlayerType == "Black")
//							for (int i = 0; i < Mathf.Min (dress.PartName.Length, dress.Black_Images.Length); i++) {
//								foreach (var _x in _xyz) {
//									if (_x.name == dress.PartName [i]) {
//										_x.GetComponent<SpriteRenderer> ().sprite = dress.Black_Images [i];
//
//									}
//								}
//							}
//					}
				
				}
				int Id = 0;
				int.TryParse (flatmate.Hair_style, out Id);
				if (Id != 0) {
					var Hair = PurchaseSaloonManager.Instance.FindSaloonWithId (Id);
//					if (flatmate_Go.GetComponent<CharacterProperties> ().PlayerType == "White")
					DressManager.Instance.ChangeHairsOfCharacter (flatmate_Go, Hair.PartName, Hair.HairImages);

//					else if (flatmate_Go.GetComponent<CharacterProperties> ().PlayerType == "Brown")
//						DressManager.Instance.ChangeFlatMateDress (flatmate_Go, Hair.PartName, Hair.Brown_Images);
//					else if (flatmate_Go.GetComponent<CharacterProperties> ().PlayerType == "Black")
//						DressManager.Instance.ChangeFlatMateDress (flatmate_Go, Hair.PartName, Hair.Black_Images);
				}
			
				RoommateManager.Instance.SelectedRoommate = flatmate_Go;

//			RoommateManager.Instance.MakeFlatMateBusyForTimeRemainingAfterReturn (obj.GetComponent<Flatmate> ().data);


				//		TODO: change the dress of the flatmate generated
			}
		} else {

			StartCoroutine (GetFlatmateData (_rmgdata, flatmate));
		}
	}

	public IEnumerator SetPurchasingData (int item_id, string cat, string subcat)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		int Id = PlayerPrefs.GetInt ("PlayerId");
		PurchaseData pd = new PurchaseData ();
		pd.player_id = Id;
		pd.item_id = item_id;
		pd.cat = cat;
		pd.sub_cat = subcat;
		string json = JsonUtility.ToJson (pd);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/items/savepurchaseditem", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["description"].ToString ().Contains ("successfully")) {
				print ("Success");
				yield return true;
			} else {
				print ("error");
				yield return false;
			}
		} else {
			yield return false;
		}
	}



	public IEnumerator UpdateFlatmate (RoommateSaveData data)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

//		Debug.LogError ("Save data is - " + json.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/items/saveflatematesdata", encoding.GetBytes (json), postHeader);
		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["description"].ToString ().Contains ("successfully")) {
				print ("success");
				yield return true;
			} else {
				print ("error");
				yield return false;
			}
		} else {
			yield return false;
		}
	}


	//TODO: get link of the download flatmate data
	public IEnumerator DownloadFlatmateData (RoommateGetData data, RoommateData _roommatedata)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/items/viewflatmateattributes", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

//			"player_id": "71",
//			"item_id":"22",
//			"name":"asd",
//			"gender":"asd",
//			"perk":"asd",
//			"perk_value":"asd",
//			"dress":"asd",
//			"hair_style":"asd",
//			"education_point":"asd",
//			"education_point_level":"asd",
//			"busy_type":"asd",
//			"cooldown_time":"sdfsdf",
//			"cooldown_time_event_id":"adsfasdsa",
//			"busy_time":"asd

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");
//				Debug.LogError ("Faltmate data is - " + _jsnode.ToString ());
				_roommatedata.Name = _jsnode ["data"] ["name"].ToString ().Trim ('"');
//				int gender = 0;

				if (_jsnode ["data"] ["gender"].ToString ().Trim ('"').Contains ("Female"))
					_roommatedata.Gender = GenderEnum.Female;
				else
					_roommatedata.Gender = GenderEnum.Male;
				
				_roommatedata.Perk = _jsnode ["data"] ["perk"].ToString ().Trim ('"');
				int.TryParse (_jsnode ["data"] ["perk_value"].ToString ().Trim ('"'), out _roommatedata.Perk_value);

//				var dresskeys = new List<string> ();
//				foreach (var key in _jsnode["data"]["dress"].Keys) {
//					dresskeys.Add (key.ToString ().Trim ('"'));
//				}
//
//				for (int i = 0; i < _jsnode ["data"] ["dress"].Count; i++) {
//					_roommatedata.Dress.Add (dresskeys [i], _jsnode ["data"] ["dress"] [i].ToString ().Trim ('"'));
//				}

				_roommatedata.Hair_style = _jsnode ["data"] ["hair_style"].ToString ().Trim ('"');
				int Id = 0;
				int.TryParse (_jsnode ["data"] ["hair_style"], out Id);
				if (Id != 0)
					_roommatedata.Dress.Add ("Hair", Id);
				string[] Ids = _jsnode ["data"] ["dress"].ToString ().Trim ('"').Split (',');
				foreach (var id in Ids) {
					int IntId = 0;
					int.TryParse (id, out IntId);
					if (IntId != 0) {
						var dress = FindDressWithId (IntId);
						if (dress != null) {

							string Cat = dress.Catergory.ToString ();
							    
							if (_roommatedata.Dress.ContainsKey (Cat))
								_roommatedata.Dress [Cat] = dress.Id;
							else
								_roommatedata.Dress.Add (Cat, dress.Id);
						}
					}
				}

//			

				int.TryParse (_jsnode ["data"] ["education_point"].ToString ().Trim ('"'), out _roommatedata.education_level);
				float.TryParse (_jsnode ["data"] ["education_point_level"].ToString ().Trim ('"'), out _roommatedata.education_point);
//				var sdjhyf = _jsnode ["data"] ["is_busy"].ToString ().Trim ('"');

//				float.TryParse (_jsnode ["data"] ["busy_time"].ToString ().Trim ('"'), out _roommatedata.busyTimeRemaining);
				string busyTime = _jsnode ["data"] ["busy_time"].ToString ().Trim ('\"');

				if (busyTime != "0" && busyTime != "") {
					var EndTime = DateTime.FromBinary (Convert.ToInt64 (busyTime));
//					if (EndTime > DateTime.UtcNow) {
					_roommatedata.BusyTimeRemaining = EndTime;
					_roommatedata.IsBusy = true;
//					}
				} else {
					_roommatedata.BusyTimeRemaining = new DateTime ();
					_roommatedata.IsBusy = false;
				}

				int.TryParse (_jsnode ["data"] ["cooldown_time_event_id"].ToString ().Trim ('"'), out _roommatedata.EventBusyId);
				string CooldownTime = _jsnode ["data"] ["cooldown_time"].ToString ().Trim ('\"');
				if (CooldownTime != "0" && CooldownTime != "") {
					var EndTime = DateTime.FromBinary (Convert.ToInt64 (CooldownTime));
//					if (EndTime > DateTime.UtcNow) {
					_roommatedata.CooldownEndTime = EndTime;
					_roommatedata.IsCoolingDown = true;
//					} else {
//						_roommatedata.CooldownEndTime = new DateTime ();
//						_roommatedata.IsCoolingDown = false;
//					}
				} else {
					_roommatedata.CooldownEndTime = new DateTime ();
					_roommatedata.IsCoolingDown = false;
				}

				
				if (!string.IsNullOrEmpty (_jsnode ["data"] ["busy_type"].ToString ())) {
					if (_jsnode ["data"] ["busy_type"].ToString ().Contains ("Class"))
						_roommatedata.BusyType = BusyType.Class;
					else if (_jsnode ["data"] ["busy_type"].ToString ().Contains ("FashionEvents"))
						_roommatedata.BusyType = BusyType.FashionEvents;
					else if (_jsnode ["data"] ["busy_type"].ToString ().Contains ("CatwalkEvents"))
						_roommatedata.BusyType = BusyType.CatwalkEvents;
					else if (_jsnode ["data"] ["busy_type"].ToString ().Contains ("CoopEvent"))
						_roommatedata.BusyType = BusyType.CoopEvent;
				}
				
				yield return _roommatedata;
			} else {
				print ("error");
				yield return null;
			}
		} else {
			yield return null;
		}
	}

	DressItem FindDressWithId (int Id)
	{
		foreach (var item in PurchaseDressManager.Instance.AllDresses) {
			if (item.Id == Id)
				return item;
		}
		return null;
	}

	public IEnumerator SendPositions (PositionUpdate data)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/players/updateassetsposition", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");

				yield return true;
			} else {
				print ("error");
				yield return false;
			}
		} else {
			yield return false;
		}
	}


	public IEnumerator GetPositions (Dictionary<int, DecorData> item_dictionary)
	{

		PositionGet data = new PositionGet ();
		data.player_id = PlayerPrefs.GetInt ("PlayerId");

		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/players/getassetposition", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success" + _jsnode.ToString ());
				for (int i = 0; i < _jsnode ["data"].Count; i++) {
					foreach (var kvp in item_dictionary) {                        
						var xyz = _jsnode ["data"] [i] [1].ToString ();
						if (_jsnode ["data"] [i] ["item_id"].ToString ().Contains (kvp.Key.ToString ())) {
                            
							string[] Value = _jsnode ["data"] [i] ["rotation"].ToString ().Trim ('"').Split ('_');
							var rotation = 0;
							var IsPlaced = false;
							int.TryParse (Value [0], out rotation);
							if (Value.Length == 2) {
								bool.TryParse (Value [1], out IsPlaced);
							}

							if (IsPlaced) {
								var asset = Resources.Load<Decor3DView> ("Decors/" + kvp.Value.Name.Trim ('"'));
								if (asset == null) {
									//			List<string> downloadednames = new List<string> ();
									for (int x = 0; x < DecorController.Instance.DownloadedDecors.Count; x++) {

										if (DecorController.Instance.DownloadedDecors [x].gameObject.name == kvp.Value.Name.Trim ('"') || DecorController.Instance.DownloadedDecors [x].gameObject.name == FirstCharToUpper (kvp.Value.Name.Trim ('"'))) {
											asset = DecorController.Instance.DownloadedDecors [x].GetComponent<Decor3DView> ();
										}

									}

								}
								int eventid = 0;
								DateTime.TryParse (_jsnode ["data"] [i] ["is_busy_time"].ToString ().Trim ('"'), out kvp.Value._isBusyEndTime);
								DateTime.TryParse (_jsnode ["data"] [i] ["cool_time"].ToString ().Trim ('"'), out kvp.Value._coolDownEndTime);
								int.TryParse (_jsnode ["data"] [i] ["cool_down_time_event_id"], out eventid);



								if (DateTime.Compare (kvp.Value._isBusyEndTime, DateTime.Now) > 0) {
									kvp.Value._isBusy = true;
									DecorController.Instance.BusyDecor (kvp.Value.Id, eventid);
								} else {
									kvp.Value._isBusy = false;
								}
								if (DateTime.Compare (kvp.Value._coolDownEndTime, DateTime.Now) > 0) {
									kvp.Value._isCoolingDown = true;
									DecorController.Instance.CoolDownOfDecor (kvp.Value.Id, eventid);
								} else {
									kvp.Value._isCoolingDown = false;
								}



								GameObject Go = (GameObject)Instantiate (asset.gameObject, Vector2.zero, Quaternion.identity);

								Go.SetMaterialRecursively ();

								Go.GetComponent<Decor3DView> ().CreateDecore (kvp.Value);

								Go.GetComponent<Decor3DView> ().SetPositionofThisItem (_jsnode ["data"] [i] ["position"].ToString ().Trim ('"'), rotation);


								Go.GetComponent<Decor3DView> ().Placed = IsPlaced;
								DecorController.Instance.PlacedDecors.Add (Go.GetComponent<Decor3DView> ());
								DecorController.Instance.PlacedDecorsName.Add (Go.GetComponent<Decor3DView> ().decorInfo.Name);

//							Destroy (Go.GetComponent<DragSnap> ());
//							Go.AddComponent<DragSnap> ();

								Go.GetComponent<DragSnap> ().enabled = true;

//							Destroy (Go.GetComponent<DecorOrderInLayer> ());
//							Go.AddComponent<DecorOrderInLayer> ();
						

								Go.GetComponent<Decor3DView> ().Correct ();
							}
						}
					}
				}

			} else {
				print ("error");
			
			}
		} else {
			print ("error");

		}
	}


	public static string FirstCharToUpper (string input)
	{
		if (String.IsNullOrEmpty (input))
			throw new ArgumentException ("ARGH!");
		return input.First ().ToString ().ToUpper () + input.Substring (1);
	}




	public IEnumerator Logout ()
	{
		var Json = new Simple_JSON.JSONClass ();
		Json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

//		data.coins = PlayerPrefs.GetInt ("Money");
//		data.expirence_level = PlayerPrefs.GetInt ("Level");
//		data.expirence_points = PlayerPrefs.GetFloat ("ExperiencePoints" + PlayerPrefs.GetInt ("Level"));
//		data.gems = PlayerPrefs.GetInt ("Gems");
//		data.tutorial_status = PlayerPrefs.GetInt ("Tutorial_Progress") + "/" + PlayerPrefs.GetInt ("Purchaseland");
//		data.logout_time = DateTime.UtcNow.ToString ();
//		if (PlayerPrefs.GetString ("activateParentel").Contains("true") || PlayerPrefs.GetString ("activateParentel").Contains("True") ) 
//			data.parental_Status = "1";
//		else
//			data.parental_Status = "2";
		var encoding = new System.Text.UTF8Encoding ();
		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
	
//		Debug.LogError ("Json Data for logOut" + json.ToString ());
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", Json.Count.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/players/player_logout", encoding.GetBytes (Json.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");

				yield return true;
			} else {
				print ("error");
				yield return false;
			}
		} else {
			yield return false;
		}
	}




	public IEnumerator UpdateData ()
	{
//		var data = new LogoutData ();
//		data.player_id = PlayerPrefs.GetInt ("PlayerId");
//		data.coins = PlayerPrefs.GetInt ("Money");
//		data.expirence_level = PlayerPrefs.GetInt ("Level");
//		data.expirence_points = PlayerPrefs.GetFloat ("ExperiencePoints" + PlayerPrefs.GetInt ("Level"));
//		data.gems = PlayerPrefs.GetInt ("Gems");
//		data.tutorial_status = PlayerPrefs.GetInt ("Tutorial_Progress") + "/" + PlayerPrefs.GetInt ("Purchaseland");

		var Json = new Simple_JSON.JSONClass ();
		Json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		Json ["level_no"] = PlayerPrefs.GetInt ("Level").ToString ();
		Json ["gender"] = PlayerPrefs.GetString ("CharacterType");
		Json ["coins"] = PlayerPrefs.GetInt ("Money").ToString ();
		Json ["gems"] = PlayerPrefs.GetInt ("Gems").ToString ();
//		Json ["logout_time"] = 
		Json ["expirence_points"] = PlayerPrefs.GetFloat ("ExperiencePoints" /* + PlayerPrefs.GetInt ("Level")*/).ToString ();
		Json ["expirence_level"] = PlayerPrefs.GetInt ("Level").ToString ();
		Json ["tutorial_status"] = PlayerPrefs.GetInt ("Tutorial_Progress") + "/" + PlayerPrefs.GetInt ("Purchaseland");

		//		bool VIP_Subscription = !string.IsNullOrEmpty (PlayerPrefs.GetString ("VIPSubcribedTime"));
		Json ["vip_subscription"] = VipSubscriptionManager.Instance.VipSubscribed.ToString (); 

		Json ["end_time_vip_subscription"] = PlayerPrefs.GetString ("VIPSubcribedTime").Trim ('\"');
		Json ["parental_control_status"] = PlayerPrefs.GetString ("activateParentel");
		Json ["party_time_cooldown"] = PlayerPrefs.GetFloat ("HostPartyCooldownTime").ToString ();

		/// Addition by Rehan
		Json ["player_profile_status"] = PlayerPrefs.GetString ("PlayerProfileStatus");
		Json ["current_achievement_medal"] = PlayerPrefs.GetString ("CurrentAchievementMedal");
//		Json ["rank_last_special_event"] = 


//		{
//			"player_id": "94",
//			"level_no": "2",
//			"gender": "male",
//			"coins": "119",
//			"gems": "09",
//			"logout_time": "safs245dsf",
//			"expirence_points": "",
//			"expirence_level": "23",
//			"tutorial_status": "112",
//			"vip_subscription" : "asd",
//			"end_time_vip_subscription":"435",
//			"parental_control_status": "345",
//			"party_time_cooldown": "534"	
//		}

		var encoding = new System.Text.UTF8Encoding ();
		Dictionary<string, string> postHeader = new Dictionary<string, string> ();

		//		Debug.LogError ("Json Data for logOut" + json.ToString ());
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", Json.Count.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/players/playerUpdate", encoding.GetBytes (Json.ToString ()), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");

				yield return true;
			} else {
				print ("error");
				yield return false;
			}
		} else {
			yield return false;
		}
	}



	public IEnumerator UpdateFlat (FlatUpdateData data)
	{
		
		var encoding = new System.Text.UTF8Encoding ();
		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/flats/updateflat", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");

				yield return true;
			} else {
				print ("error");
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	public IEnumerator GetFData ()
	{

		var data = new PositionGet ();
		data.player_id = PlayerPrefs.GetInt ("PlayerId");


		var encoding = new System.Text.UTF8Encoding ();
		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/flats/getflat", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			FlatdataCount = _jsnode ["data"].Count;
			print (FlatdataCount);

		}
	}

	/// <summary>
	/// Gets the flat info from server.
	/// </summary>
	/// <returns>The flat</returns>
	public IEnumerator GetFlat ()
	{
		
		var data = new PositionGet ();
		data.player_id = PlayerPrefs.GetInt ("PlayerId");


		var encoding = new System.Text.UTF8Encoding ();
		Dictionary<string, string> postHeader = new Dictionary<string, string> ();
		string json = JsonUtility.ToJson (data);

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Length.ToString ());

		WWW www = new WWW ("http://pinekix.ignivastaging.com/flats/getflat", encoding.GetBytes (json), postHeader);

		yield return www;

		if (www.error == null) {
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");
				for (int i = 0; i < _jsnode ["data"].Count; i++) {
					var item_id = int.Parse (_jsnode ["data"] [i] ["item_id"].ToString ().Trim ('"'));
					var item_data = _jsnode ["data"] [i] ["position"].ToString ().Trim ('"');
					var ground_texture = _jsnode ["data"] [i] ["ground_texture"].ToString ().Trim ('"');
					var wall_texture = _jsnode ["data"] [i] ["wall_texture"].ToString ().Trim ('"');
				
//					if (item_id == 0) {
//						RoomPurchaseManager.Instance.AddSpaceData (item_data);

//					} else {
					RoomPurchaseManager.Instance.AddFlatData (item_data, ground_texture, wall_texture);

//					}
					DownloadedItemCount++;
					percentege = DownloadedItemCount * 100 / TotelDownloadItem;
					LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value = percentege;
					LoadingScreen.GetComponent<LoadingGameAssets> ().loadingValue.text = "Loading..." + Mathf.Round (LoadingScreen.GetComponent<LoadingGameAssets> ().slide.value).ToString () + "%";
					print (percentege + "%");
					yield return new WaitForSeconds (0.5f);

				}
//				RoomPurchaseManager.Instance.CheckForDuplicateBanners ();
				RoomPurchaseManager.Instance.CheckForExtraTimers ();
				RoomPurchaseManager.Instance.DisableWalls ();
				LoadingScreen.GetComponent<LoadingGameAssets> ().MainBg.SetActive (false);
//				if(GameObject.Find("ParantelScreenAfterRegistertion"))
//				Destroy (GameObject.Find ("ParantelScreenAfterRegistertion"));
				yield return true;
			} else {
				print ("error");
				yield return false;
			}
		} else {
			yield return false;
		}
	}





	public IEnumerator ClaimReward (string link)
	{

		yield return StartCoroutine (AssetBundleManager.downloadAssetBundle (link, 1));
		AssetBundle bundle = AssetBundleManager.getAssetBundle (link, 1);

		if (bundle != null) {
			var _x = bundle.GetAllAssetNames ();
			GameObject target = null;
			foreach (var _y in _x) {
				if (_y.Contains (".prefab"))
					target = (GameObject)bundle.LoadAsset (_y);
			}


			var prizes = target.GetComponent<WinnerPrizeForEvents> ();

//			PlayerPrefs.SetInt ("money", PlayerPrefs.GetInt ("money") + prizes._coin);
//			PlayerPrefs.SetInt ("gems", PlayerPrefs.GetInt ("gems") + prizes._gems);
			GameManager.Instance.AddCoins (prizes._coin);
			GameManager.Instance.AddGemsWithGemBonus (prizes._gems);


			var rewardobj = new RewardObject ();

			rewardobj.coinWin = prizes._coin;
			rewardobj.gemsWin = prizes._gems;



			foreach (var _dress in prizes._dresses) {
				string Name = _dress.name;


				string subCategory = "Dresses";// for each key/values

				int coin = 0;
				int level = 0;
				int gems = 0;
				int key = 0;

				PurchaseDressManager.Instance.UpdatePrizeDress (_dress, Name, subCategory, level, coin, gems, key);
				rewardobj.dressName.Add (Name);
				rewardobj.dressIcon.Add (_dress.Icon);
			}

			foreach (var _decor in prizes._decors) {
				string name = _decor.name;
				string subCategory = "Sofas";

				int coin = 0;
				int level = 0;
				int gems = 0;
				int key = 0;

				DecorController.Instance.AddPrizeDecorList (key, _decor, name, subCategory, level, gems, coin);
				rewardobj.decorName.Add (name);
				rewardobj.decorIcon.Add (_decor.Icon);
				EventManagment.Instance.Rewards.Add (rewardobj);
			}
				
			yield return true;

		} else
			yield return false;
	}



	public IEnumerator UpdateClaimedRewards (int eventid, string eventname, string link)
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var json = new Simple_JSON.JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		json ["event_id"] = eventid.ToString ();
		json ["eventname"] = eventname;
		json ["link"] = link;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekixdev.ignivastaging.com/events/updateClaimedReward", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {
			
			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");
				yield return true;
			} else {
				yield return false;
			}
		} else {
			yield return false;
		}
	}

	IEnumerator ClaimedRewards ()
	{
		var encoding = new System.Text.UTF8Encoding ();

		Dictionary<string,string> postHeader = new Dictionary<string,string> ();

		var json = new Simple_JSON.JSONClass ();

		json ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", json.Count.ToString ());
		print ("jsonDtat is ==>> " + json.ToString ()); 

		WWW www = new WWW ("http://pinekixdev.ignivastaging.com/events/getClaimedReward", encoding.GetBytes (json.ToString ()), postHeader);

		yield return www;
		if (www.error == null) {

			JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);

			if (_jsnode ["status"].ToString ().Contains ("200")) {
				print ("success");
				var eventname = _jsnode ["data"] [0] ["eventname"].ToString ().Trim ('"');
//				var event_id = _jsnode ["data"] [0] ["event_id"].ToString ().Trim ('"');
				var link = _jsnode ["data"] [0] ["link"].ToString ().Trim ('"');


				yield return StartCoroutine (AssetBundleManager.downloadAssetBundle (link, 1));
				AssetBundle bundle = AssetBundleManager.getAssetBundle (link, 1);

				if (bundle != null) {
					var _x = bundle.GetAllAssetNames ();
					GameObject target = null;
					foreach (var _y in _x) {
						if (_y.Contains (".prefab"))
							target = (GameObject)bundle.LoadAsset (_y);
					}


					var prizes = target.GetComponent<WinnerPrizeForEvents> ();


					var rewardobj = new RewardObject ();
					rewardobj.NameofEvent = eventname;
					rewardobj.coinWin = prizes._coin;
					rewardobj.gemsWin = prizes._gems;



					foreach (var _dress in prizes._dresses) {
						string Name = _dress.name;


						string subCategory = "Dresses";// for each key/values

						int coin = 0;
						int level = 0;
						int gems = 0;
						int key = 0;


						PurchaseDressManager.Instance.UpdatePrizeDress (_dress, Name, subCategory, level, coin, gems, key);
						rewardobj.dressName.Add (Name);
						rewardobj.dressIcon.Add (_dress.Icon);
					}

					foreach (var _decor in prizes._decors) {
						string name = _decor.name;
						string subCategory = "Sofas";

						int coin = 0;
						int level = 0;
						int gems = 0;
						int key = 0;

						DecorController.Instance.AddPrizeDecorList (key, _decor, name, subCategory, level, gems, coin);
						rewardobj.decorName.Add (name);
						rewardobj.decorIcon.Add (_decor.Icon);
						EventManagment.Instance.Rewards.Add (rewardobj);
					}

					yield return true;
				} else
					yield return false;
			} else {
				yield return false;
			}
		} else {
			yield return false;
		}
	}


	public IEnumerator IeCheckForDownloadList ()
	{

		ScreenAndPopupCall.Instance.LoadingScreen ();
		yield return new WaitForSeconds (0.1f);
		foreach (var kvp in link_list) {

			while (!AssetBundleManager.CheckForData (kvp.Value + "1")) {
				if (AssetBundleManager.CheckForData (kvp.Value + "1")) {
					break;
				} else {
					yield return AssetBundleManager.RetryDownload (kvp.Value);
					yield return null;
				}
			}
		}

		yield return new WaitForSeconds (0.1f);
		ScreenAndPopupCall.Instance.LoadingScreenClose ();
	}
}


[Serializable]
public class RewardObject
{
	public string NameofEvent;
	public int coinWin;
	public int gemsWin;
	public List<string> dressName = new List<string> ();
	public List<Sprite> dressIcon = new List<Sprite> ();
	public List<string> decorName = new List<string> ();
	public List<Sprite> decorIcon = new List<Sprite> ();
}
