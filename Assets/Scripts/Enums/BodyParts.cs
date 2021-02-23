using UnityEngine;
using System.Collections;

public class BodyParts : MonoBehaviour {

    public int Layer;
    public int LayerOffset;
    public int finalLayer;
    Bounds Bounds; 
    SpriteRenderer Sr;
    public CharacterProperties CharRep;

    public bool isActive;
    public float SpriteOrder;

    void Start () 
    {
        CharRep = GetComponentInParent<CharacterProperties> ();
        Sr = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        Bounds = CharRep.Bounds;
        Layer = ((int)(Camera.main.WorldToScreenPoint (Bounds.min).y) * -1);       
        if(!isActive)
                 Sr.sortingOrder = Layer + LayerOffset;	
        else
                Sr.sortingOrder = (int) SpriteOrder;

        finalLayer = Layer + LayerOffset;
    }
}
