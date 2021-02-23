using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;


[Serializable]
public struct SpriteParts
{
	public string Name;
	public string Category;
	public string SubCategory;
	public Sprite[] items;
}

public class CharacterProperties : MonoBehaviour
{
	public string PlayerType;
    public string Gender;
    bool isFacingFront = false;
    public GameObject Front;
    public GameObject Back;

    BoxCollider2D Collider;
    Vector2 Size;
    Vector3 colOff;
    float sizeOff;

    public Bounds Bounds;

	void Start ()
	{
//		DontDestroyOnLoad (this.gameObject);
//        Invoke("Init",0.5f); 
        isFacingFront = false;
        ChangeFacingFront();

        Collider = GetComponent<BoxCollider2D>();
        Size = Collider.size;
        colOff = Collider.offset * 0.5f;
        
	}
//
//    void Init()
//    {
//        ChangeFacingFront(true);
//    }

    public void ChangeFacingFront()
    { 
        if (!Front || !Back)
            return;
        isFacingFront = !isFacingFront;
        Front.SetActive(isFacingFront);
        Back.SetActive(!isFacingFront);
//        isFacingFront = isFacingFront;
    }

	public void ChangeFacingBack()
	{
		if (!Front || !Back)
			return;
		isFacingFront = !isFacingFront;
		Front.SetActive(!isFacingFront);
		Back.SetActive(isFacingFront);
		//        isFacingFront = isFacingFront;
	}

    public bool GetIsFacingFront()
    {
        return isFacingFront;
    }

    void Update()
    {
        sizeOff = transform.localScale.x;
        Bounds = new Bounds(transform.position + colOff, (Size * sizeOff));
    }

    public void ChangeColor ()
    {
        BodyParts[] parts = GetComponentsInChildren<BodyParts>(true);
        var Cust = Resources.Load<CustomisationAllData>("CustomisationAllData");
        foreach (var _char in Cust.MyChars)
        {          
        if (_char._category.Contains ("SkinTone") && _char._gender == Gender) 
            {   
            int _index = _char.AllColors.FindIndex(colorobj => colorobj.name == PlayerType);
                var PartName = _char.PartName;
                var item = _char.AllColors[_index].AllItems[0];

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


}