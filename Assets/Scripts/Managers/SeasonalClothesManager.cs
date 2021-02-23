using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simple_JSON;

public class SeasonalClothesManager : MonoBehaviour
{
    public static SeasonalClothesManager Instance = null;
    public List<SeasonalClothes> AllSeasonalClothes = new List<SeasonalClothes>();
    public GameObject DressItemsPrefab;
    public Transform DressContainer;

    void Awake ()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

    public void GetAllSeasonalClothesPerSeason()
    {
        StartCoroutine(IGetAllSeasonalClothesPerSeason(true));
    }
    IEnumerator IGetAllSeasonalClothesPerSeason(bool openScreen)
    {
        ScreenAndPopupCall.Instance.LoadingScreen();
        AllSeasonalClothes.Clear();
        var encoding = new System.Text.UTF8Encoding();
        var postHeader = new Dictionary<string,string>();

        var jsonElement = new JSONClass ();

        jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
//        "cat_id":1,
//        "level_no":100
        jsonElement["cat_id"] = GetSeasonCategory().ToString();
        jsonElement ["level_no"] = GameManager.Instance.level.ToString();

        postHeader.Add ("Content-Type", "application/json");
        postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

        WWW www = new WWW (PinekixUrls.SeasonalClothsListing, encoding.GetBytes(jsonElement.ToString ()), postHeader);       
        yield return www;
        if (www.error == null)
        {
            JSONNode _jsnode = Simple_JSON.JSON.Parse(www.text);
//            {
//                "status": "200",
//                "description": "Data has followed.",
//                "data": [
//                        {
//                            "season_id": "1",
//                            "item_id": "10",
//                            "item_name": "Test",
//                            "item_image_url": "http://pinekix.ignivastaging.com/admin/seasonalClothes/addclothes",
//                            "item_coins": "12",
//                            "item_level_no": "1",
//                            "item_gems": "122"
//                        }
//                ]
//            }
            if(_jsnode["status"].ToString().Contains("200"))
            {
                var data = _jsnode["data"];

                for (int x = 0; x < data.Count; x++)
                {
                    int id = 0;
                    int.TryParse(data [x]["item_id"], out id);
                    int coin = 0;
                    int.TryParse(data [x]["item_coins"], out coin);
                    int gem = 0;
                    int.TryParse(data[x]["item_gems"], out gem);
                    int level = 0;
                    int.TryParse(data[x]["item_level_no"], out level);

                    int season = 0;
                    int.TryParse(data [x]["season_id"], out season);          

                    string Name = data[x]["item_name"];
                    string path = data[x]["item_image_url"];

                    SeasonalClothes sc = new SeasonalClothes(Name,id,season -1 ,coin,gem,level,path);
                    AllSeasonalClothes.Add(sc);
                }                      
            }else
            {
                
            }
            if (openScreen)
            {
                ShowAllDressesForSeasonal();
            }
            else
            {
                List <SeasonalClothes> ListGenderwise = new List<SeasonalClothes>();
                AllSeasonalClothes.ForEach(dress =>
                    {
                        var target = Resources.Load<DressInfo>(dress.PathUrl);
                        if (GameManager.GetGender() == (GenderEnum)target.Gender)
                        {
                        
                        }
                    });
                AllSeasonalClothes.Clear();
                AllSeasonalClothes.AddRange(ListGenderwise);
            }            

        }else
        {
            
        }
        ScreenAndPopupCall.Instance.LoadingScreenClose();
    }

    public IEnumerator IGetSavedSeasonalClothes()
    {
        AllSeasonalClothes.Clear();
        var encoding = new System.Text.UTF8Encoding();
        var postHeader = new Dictionary<string,string>();

        var jsonElement = new JSONClass ();

        jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
        jsonElement["season_id"] = GetSeasonCategory().ToString();

        postHeader.Add ("Content-Type", "application/json");
        postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

        WWW www = new WWW (PinekixUrls.GetSavedSeasonalClothsLink, encoding.GetBytes(jsonElement.ToString ()), postHeader);       
        yield return www;
        if (www.error == null)
        {
            JSONNode _jsnode = Simple_JSON.JSON.Parse(www.text);

            if(_jsnode["status"].ToString().Contains("200"))
            {
                var data = _jsnode["data"];

                for (int x = 0; x < data.Count; x++)
                {
                    int id = 0;
                    int.TryParse(data [x]["item_id"], out id);
                    int coin = 0;
                    int.TryParse(data [x]["item_coins"], out coin);
                    int gem = 0;
                    int.TryParse(data[x]["item_gems"], out gem);
                    int level = 0;
                    int.TryParse(data[x]["item_level_no"], out level);

                    int season = 0;
                    int.TryParse(data [x]["season_id"], out season);

                    string Name = data[x]["item_name"];
                    string path = data[x]["item_image_url"];

                    var target = Resources.Load<DressInfo>(path);
                    if(target)
                    PurchaseDressManager.Instance.UpdatePrizeDress(target, Name, "SeasonalClothes",level,coin,gem,id);
                }
            }else
            {

            }
        }else
        {

        }
    }

    public IEnumerator GetRandomSeasonalClothesAndSaveIt()
    {
        ScreenAndPopupCall.Instance.LoadingScreen();
        yield return StartCoroutine(IGetAllSeasonalClothesPerSeason(false));
        var value = UnityEngine.Random.Range(0, AllSeasonalClothes.Count);
        var dress = AllSeasonalClothes[value];
        yield return StartCoroutine(ISaveSeasonalClothes(dress.Id));

        ScreenAndPopupCall.Instance.LoadingScreenClose();
    }

    IEnumerator ISaveSeasonalClothes(int ItemId)
    {
        AllSeasonalClothes.Clear();
        ScreenAndPopupCall.Instance.LoadingScreen();
        var encoding = new System.Text.UTF8Encoding();
        var postHeader = new Dictionary<string,string>();

        var jsonElement = new JSONClass ();

        jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();  
        jsonElement["item_id"] = ItemId.ToString();

        postHeader.Add ("Content-Type", "application/json");
        postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

        WWW www = new WWW (PinekixUrls.SaveSeasonalClothsLink, encoding.GetBytes(jsonElement.ToString ()), postHeader);       
        yield return www;
        if (www.error == null)
        {
            JSONNode _jsnode = Simple_JSON.JSON.Parse(www.text);

            if(_jsnode["status"].ToString().Contains("200"))
            {
                      
            }else
            {
                
            }
        }else
        {

        }
        ScreenAndPopupCall.Instance.LoadingScreenClose();
    }

    void ShowAllDressesForSeasonal()
    {
        for (int i = 0; i < DressContainer.transform.childCount; i++) {
            GameObject.Destroy (DressContainer.transform.GetChild (i).gameObject);
        }
        
        if (AllSeasonalClothes.Count == 0)
        {
            ScreenAndPopupCall.Instance.ShowPopOfDescription("There are currently no seasonal clothes for your level"
                , null);
        }
        ScreenAndPopupCall.Instance.SeasonalClothingScreenForEvents();

        AllSeasonalClothes.ForEach (dress => {  
                 
            var target = Resources.Load<DressInfo>(dress.PathUrl);
            if(GameManager.GetGender() == (GenderEnum)target.Gender)
            {

                DressItem DressData = new DressItem(8,
                    target.name,
                    dress.Id,
                    target.Icon,
                    true,
                    true,
                    (GenderEnum)target.Gender,
                    dress.Level,
                    dress.Coins,
                    dress.Gems,
                    target.BodyPartName.ToArray(),
                    target.DressesSprites.ToArray(), false);

                GameObject Go = Instantiate (DressItemsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
                Go.transform.parent = DressContainer.transform;
                Go.transform.localScale = Vector3.one;
                Go.GetComponent<DressView>().thisDress = DressData;
            }
        });
    }

    int GetSeasonCategory()
    {
        var now =System.DateTime.UtcNow.Month;
        switch(now)
        {
            case 3:
            case 4:
            case 5:
                return 1;
            case 6:
            case 7:
            case 8:
                return 2;
            case 9:
            case 10:
            case 11:
                return 3;
            case 12:
            case 1:
            case 2:
                return 4;
                
        }
        return 0;
    }
}

[System.Serializable]
public class SeasonalClothes
{
    public string Name;
    public int Id;
    public Seasons Season;
    public int Coins;
    public int Gems;
    public int Level;
    public string PathUrl;

    public SeasonalClothes(string name, int id, int season, int coin,int gem,int level, string path)
    {
        Name = name;
        Id = id;
        Season = (Seasons) season;
        Coins = coin;
        Gems = gem;
        Level = level;
        PathUrl = path;
    }
}

public enum Seasons
{
    Spring = 0,
    Summer = 1,
    Autumn = 2,
    Winter = 3
}