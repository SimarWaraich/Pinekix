using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Simple_JSON;

public class SocietyHomeRoom : MonoBehaviour 
{

    public Button ReCenterButton;
    public GameObject StorageScreen;
    public Button PlacementButton;
    public List<SocietyDecor> PlacedDecorInRoom = new List<SocietyDecor>();
    public bool IsStorageOpen;
    public void OpenStorageInSocietyRoom ()
    {
        IsStorageOpen = true;
        if (SocietyManager.Instance.myRole == 0 || SocietyManager.Instance.myRole == 1)
        {
            iTween.ScaleTo(StorageScreen, Vector3.one, 0.02f);
            DecorController.Instance.IntializedecorForSocietyRoom(0);
        }
        else 
            SocietyManager.Instance.ShowPopUp("You do not have authority to edit home room. Only a President or a Committee Member can do so.");
    }

    public void BackFromSocietyStorage()
    {      
        IsStorageOpen = false;        
        iTween.ScaleTo(StorageScreen, Vector3.zero, 0.02f);     
    }

    public void MoveCameraToRoom()
    {
        Vector3 Pos = new Vector3 (-1000, 0, Camera.main.transform.position.z); 

        iTween.MoveTo (Camera.main.gameObject, iTween.Hash ("position", Pos, "time", 3f));
    }

    public void GetAllDecorInSocietyRoom()
    {
        DecorController.Instance.isForSociety =true;
        StartCoroutine(IGetAllDecorInSocietyRoom(SocietyManager.Instance.SelectedSociety.Id));
    }
    IEnumerator IGetAllDecorInSocietyRoom( int societyId)
    {
        PlacedDecorInRoom.Clear();

        var link = "http://pinekix.ignivastaging.com/decors/getDecorSocietyData";
        var encoding = new System.Text.UTF8Encoding ();

        Dictionary<string,string> postHeader = new Dictionary<string,string> ();
        var jsonElement = new Simple_JSON.JSONClass ();
        jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
        jsonElement ["society_id"] = societyId.ToString ();

        postHeader.Add ("Content-Type", "application/json");
        postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

        WWW www = new WWW (link, encoding.GetBytes (jsonElement.ToString()), postHeader);  
        yield return www;

        if (www.error == null)
        {
            JSONNode _jsnode = Simple_JSON.JSON.Parse(www.text);
            print("_jsnode ==>> " + _jsnode.ToString());
            //TODO
            if (_jsnode["status"].ToString().Contains("200") || _jsnode["description"].ToString().Contains("Data is following."))
            {
//                "data": [
//                        {
//                            "id": "12",
//                            "player_id": "101",
//                            "society_id": "56",
//                            "item_id": "39",
//                            "position": "",
//                            "rotation": ""
//                        }
//                ]
                var data = _jsnode["data"];
                for(int i = 0; i< data.Count; i++)
                {
                    var item = data[i];
                    var sd = new SocietyDecor();
                    int id = 0;
                    int.TryParse(item["item_id"], out id);

                    string posString = item["position"].Value;
                    int rotation = 0;
                    int.TryParse(item["rotation"], out rotation);

                    sd.Id = id;
                    sd.Position = Decor3DView.DeserializeVector3Array(posString);
                    sd.Rotation = rotation;
//                    sd.Prefab = FindDecorWithId(id);

                    PlacedDecorInRoom.Add(sd);
                }
                CreatePlacedDecor();
            }
        }
    }


    public IEnumerator SavePositionInSocietyRoom( int itemid, string position, int rotation)
    { 
        var encoding = new System.Text.UTF8Encoding();

        Dictionary<string,string> postHeader = new Dictionary<string,string>();
        var jsonElement = new Simple_JSON.JSONClass();

        jsonElement["data_type"] = "save";
        jsonElement["player_id"] = PlayerPrefs.GetInt("PlayerId").ToString();
        jsonElement["society_id"] = SocietyManager.Instance.SelectedSociety.Id.ToString();
        jsonElement["item_id"] = itemid.ToString();
        jsonElement["position"] = position;
        jsonElement["rotation"] = rotation.ToString();


        postHeader.Add("Content-Type", "application/json");
        postHeader.Add("Content-Length", jsonElement.Count.ToString());

        WWW www = new WWW("http://pinekix.ignivastaging.com/decors/saveDecorItemData", encoding.GetBytes(jsonElement.ToString()), postHeader);  
        yield return www;

        if (www.error == null)
        {
            JSONNode _jsnode = Simple_JSON.JSON.Parse(www.text);
            print("_jsnode ==>> " + _jsnode.ToString());
            //TODO
            if (_jsnode["status"].ToString().Contains("200") || _jsnode["description"].ToString().Contains("Data has updated successfully"))
            {
                yield return true;
            }
            else
                yield return false;
        }
        else
            yield return false;
    }
    public IEnumerator RemoveDecorFromSociety(int itemid)
    { 
        var encoding = new System.Text.UTF8Encoding();

        Dictionary<string,string> postHeader = new Dictionary<string,string>();
        var jsonElement = new Simple_JSON.JSONClass();

        jsonElement["player_id"] = PlayerPrefs.GetInt("PlayerId").ToString();
        jsonElement["society_id"] = SocietyManager.Instance.SelectedSociety.Id.ToString();
        jsonElement["item_id"] = itemid.ToString();


        postHeader.Add("Content-Type", "application/json");
        postHeader.Add("Content-Length", jsonElement.Count.ToString());

        WWW www = new WWW("http://pinekix.ignivastaging.com/decors/deleteDecorSocietyItem", encoding.GetBytes(jsonElement.ToString()), postHeader);  
        yield return www;

        if (www.error == null)
        {
            JSONNode _jsnode = Simple_JSON.JSON.Parse(www.text);
            print("_jsnode ==>> " + _jsnode.ToString());
            //TODO
            if (_jsnode["status"].ToString().Contains("200") || _jsnode["description"].ToString().Contains("Data has updated successfully"))
            {
                for (int i = 0; i < PlacedDecorInRoom.Count; i++)
                {
                    if(PlacedDecorInRoom[i].Id == itemid)
                        PlacedDecorInRoom.Remove(PlacedDecorInRoom[i]);                    
                }
                yield return true;
            }
            else
                yield return false;
        }
        else
            yield return false;
    }

    void CreatePlacedDecor()
    {
        foreach(var decor in PlacedDecorInRoom)
        {
            DecorData dec = FindDecorWithId(decor.Id);
            if(dec != null)
                DecorController.Instance.Create3DDecoreForSociety(dec, true, decor.Position, decor.Rotation);
        }
    }

    GameObject FindDecorGameobjectWithId(int id)
    {
        foreach(var decor in DecorController.Instance.DownloadedDecors)
        {
            int decorId = decor.GetComponent<Decor3DView>().decorInfo.Id;

            if (decorId == id)
                return decor;
        }
        return null;
    }
    DecorData FindDecorWithId(int id)
    {
        foreach(var decor in DecorController.Instance.AllDecores)
        {
            if (decor.Id == id)
                return decor;
        }
        return null;
    }
}

public class SocietyDecor
{
    public int Id;
    public Vector3 Position;
    public int Rotation;
//    public GameObject Prefab;
}
