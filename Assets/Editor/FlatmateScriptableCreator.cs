using UnityEngine;
using System.Collections;
using UnityEditor;

public class FlatmateScriptableCreator : MonoBehaviour {


    public static T CreateScriptacbleObject<T>(string path) where T: ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance <T> ();
        string assetpath = path + "Flatmates";


        if (AssetDatabase.IsValidFolder (assetpath))
        {
            assetpath = assetpath+"/"+asset.GetType ()+".asset";
        }
        else
        {
            string Guid  =AssetDatabase.CreateFolder (path, "Flatmates");
            assetpath = AssetDatabase.GUIDToAssetPath (Guid)+ "/"+asset.GetType ()+".asset";
        }

        AssetDatabase.CreateAsset (asset, assetpath);
        AssetDatabase.SaveAssets ();

        EditorUtility.FocusProjectWindow ();

        Selection.activeObject = asset;

        return asset;
    }

    [MenuItem("Pinekix/Create New Flatmate")]
    public static void OpenLoginScene()
    {
        CreateScriptacbleObject<FlatMateInfo>("Assets/Resources/");
    }

//    [MenuItem("Pinekix/Create Shoes")]
//    public static void CreateShoePrefab()
//    {
//        var allCustom = Resources.Load<CustomisationAllData>("CustomisationAllData");
//
//
//        foreach(var shoe in allCustom.CustomShoes)
//        {
//            foreach (var color in shoe.AllColors)
//            {
//                for(int x = 0; x < color.BrownShoes.Count; x++)
//                {
//                    GameObject tempGO = new GameObject(shoe._gender+color.name+"_"+ color.BrownShoes[x].Name);
//           
//                    var tempComponent = tempGO.AddComponent<ShoesInfo>();
//
//                    tempComponent.Icon = color.BrownShoes[x].icon;
//
//                    if (shoe._gender == "Female")
//                        tempComponent.Gender = 0;
//                    else
//                        tempComponent.Gender = 1;
//
//                    tempComponent.BodyPartName = shoe.PartName;
//
//                    tempComponent.BrownShoes =color.BrownShoes[x].images;
//                    tempComponent.BlackShoes =color.BlackShoes[x].images;
//                    tempComponent.WhiteShoes =color.WhiteShoes[x].images;
//
//                    string assetpath = "Assets/Resources/" + "Shoes";
//
//
//                    if (AssetDatabase.IsValidFolder (assetpath))
//                    {
//                        assetpath = assetpath+"/"+ shoe._gender+color.name+"_"+ color.BrownShoes[x].Name+".prefab";
//                    }
//                    else
//                    {
//                        string Guid = AssetDatabase.CreateFolder("Assets/Resources/", "Shoes");
////                        assetpath+"/"+asset.GetType ()+".asset";
//                        assetpath = AssetDatabase.GUIDToAssetPath (Guid)+ "/"+shoe._gender+color.name+"_"+ color.BrownShoes[x].Name+".prefab";
//                    }
//
//                    var emptyPrefab = PrefabUtility.CreateEmptyPrefab(assetpath);
//                    PrefabUtility.ReplacePrefab(tempGO, emptyPrefab);
//                }
//            }          
//        }
//    }


//    [MenuItem("Pinekix/Hairs Style")]
//    public static void CreateShoePrefab()
//    {
//        var allCustom = Resources.Load<CustomisationAllData>("CustomisationAllData");
//
//
//        foreach(var _char in allCustom.MyChars)
//        {  
//            if (_char._category.Contains ("Hairs")) 
//             { 
//
//                foreach(var color in _char.AllColors)
//                {
//
//                    for (int x = 0; x < color.AllItems.Count; x++)
//                    {
//                        GameObject tempGO = new GameObject(_char._gender+"_"+ color.name +x);
//                                   
//                        var tempComponent = tempGO.AddComponent<SaloonInfo>();
//    
//                        tempComponent.Icon = color.AllItems[x].icon;
//    
//                        if (_char._gender == "Female")
//                            tempComponent.Gender = 0;
//                        else
//                            tempComponent.Gender = 1;
//    
//                        tempComponent.BodyPartName = _char.PartName;    
//                        tempComponent.BodyPartSprites =color.AllItems[x].images;
//                    
//                        string assetpath = "Assets/Resources/" + "Shoes";
//
//                        if (AssetDatabase.IsValidFolder(assetpath))
//                        {
//                            assetpath = assetpath + "/" +_char._gender+"_"+ color.name +x + ".prefab";
//                        }
//                        else
//                        {
//                            string Guid = AssetDatabase.CreateFolder("Assets/Resources/", "Shoes");
////                        assetpath+"/"+asset.GetType ()+".asset";
//                            assetpath = AssetDatabase.GUIDToAssetPath(Guid) + "/" + _char._gender+"_"+ color.name +x + ".prefab";
//                        }
//
//                        var emptyPrefab = PrefabUtility.CreateEmptyPrefab(assetpath);
//                        PrefabUtility.ReplacePrefab(tempGO, emptyPrefab);
//                    }
//                }
//            }          
//        }
//    }
}
