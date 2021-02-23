using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TestForDressChange : MonoBehaviour {


    public List<GameObject> AllDresses;
    public List<DressInfo> Pants;
    public List<DressInfo> Tops;

    public int CurrentPant = 0;
    public int CurrentTops = 0;
    void Start()
    {
        foreach(var dress in AllDresses)
        {
            if (dress.name.Contains("Pent") || dress.name.Contains("Short"))
                Pants.Add(dress.GetComponent<DressInfo>());
            else if (dress.name.Contains("Top"))
                Tops.Add(dress.GetComponent<DressInfo>());
        }
    }

    public void ChangeDressNext()
    {
        ChangeDress(Pants, CurrentPant);

        if (CurrentPant == Pants.Count-1)
            CurrentPant = 0;
        else
            CurrentPant++;
        
    }

    public void ChangeTops()
    {
//        ChangeDress(Tops, CurrentTops);
//
//        if (CurrentTops == Tops.Count-1)
//            CurrentTops = 0;
//        else
//            CurrentTops++;
        capture = true;
    }
    bool capture = false;
//    void OnPostRender() 
//    {
//        if (capture)
//        {
//            Texture2D sshot = new Texture2D(Screen.width, Screen.height);
//            sshot.ReadPixels(new Rect(0, 0,(float) Screen.width, (float) Screen.height), 0, 0);
//            sshot.Apply();
//            byte[] pngShot = sshot.EncodeToPNG();
//            Destroy(sshot);
//            File.WriteAllBytes(Application.dataPath + "/../screenshot_" + 1 + "_" + Random.Range(0, 1024).ToString() + ".png", pngShot);
//            capture = false;
//        }
//    }

    void ChangeDress(List<DressInfo> allDresses, int index)
    {
        var Current = allDresses[index];

        var _xyz = CharacterCustomizationAtStart.Instance.SelectedCharacter.GetComponentsInChildren<DressParts> (true);
        for (int i = 0; i < Mathf.Min (Current.BodyPartName.Count, Current.DressesSprites.Count); i++) {
            foreach (var _x in _xyz) {
                if (_x.name == Current.BodyPartName [i]) {
                    _x.GetComponent<SpriteRenderer> ().sprite = Current.DressesSprites [i];
                }
            }
        }   
    }

    [System.Serializable]

    public class DressTest
    {
        public string Name;
        public string[] PartName;
        public Sprite[] White_Images;
    }
}
