using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomisationAllData : ScriptableObject
{
    public MyCustomCharacter[] MyChars;

    public CustomShoes[] CustomShoes;

    public EmptyDress EmptyAllBoy;
    public EmptyDress EmptyAllGirl;


    [System.Serializable]
    public class EmptyDress
    {
        public List<string> BodyPartName = new List<string> ();
        public List<Sprite> DressesSprites = new List<Sprite> ();
    }
}
