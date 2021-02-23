using UnityEngine;
using System.Collections;

public class DressParts : MonoBehaviour {

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

    void Update()
    {
        Bounds = CharRep.Bounds;
        Layer = ((int)(Camera.main.WorldToScreenPoint(Bounds.min).y) * -1);    
        finalLayer = Layer + LayerOffset + 5 ;

        if(!isActive)
            Sr.sortingOrder = Layer + LayerOffset;  
        else
            Sr.sortingOrder = (int) SpriteOrder;
        Sr.sortingOrder = finalLayer;
    }
}
