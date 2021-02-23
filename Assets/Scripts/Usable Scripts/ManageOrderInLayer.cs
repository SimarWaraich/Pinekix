using System.Collections;
using UnityEngine;

public class ManageOrderInLayer : MonoBehaviour
{
	public int x;
    int Offset = 25;
    public bool Increasing;
    BoxCollider2D Collider;
    Vector2 Size;
    Vector3 colOff;
    Vector2 sizeOff;

    void Start()
    {
        Collider = GetComponent<BoxCollider2D>();
        Size = Collider.size;
        colOff = Collider.offset * 0.5f;
    }

    void Update ()
    {
        sizeOff = transform.localScale;
		DepthManagement ();
	}


	void DepthManagement ()
	{	
        Vector2 finalSize = new Vector2(Size.x * sizeOff.x, Size.y * sizeOff.y);
        Bounds Bounds = new Bounds(transform.position + colOff, finalSize);

        x = ((int)(Camera.main.WorldToScreenPoint (Bounds.min).y) * -1);
        if(Increasing)
            gameObject.SetOrderInLayerRecursivelyIncreasing (x + Offset);
        else
            gameObject.SetOrderInLayerRecursively (x + Offset);
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Vector2 finalSize = new Vector2(Size.x * sizeOff.x, Size.y * sizeOff.y);
        Gizmos.DrawWireCube(transform.position + colOff , finalSize);
    }
}