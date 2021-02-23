using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScaleCenterContent : MonoBehaviour {
//    ScrollRect MyScroll;
//    Image CenterImage;
    Vector2 OriginalSize;
    Vector2 SmallSize ;

    RectTransform ScaleableObj;
//    Rect mRect;

    float XNeg = -4f;
    float Xpos = 4f;
    bool isClickable;
	void Start () 
    {
//        MyScroll = GetComponentInParent<ScrollRect>();
        OriginalSize = new Vector2(298.4f, 402.4f);//GetComponentInParent<GridLayoutGroup>().cellSize;
        SmallSize = OriginalSize * 0.8f;    
        ScaleableObj = transform.FindChild("BackGround").GetComponent<RectTransform>();
	}
	
	void Update () {

        if (ScreenManager.Instance.ScreenMoved == ScreenManager.Instance.PublicAreaList)
        {
            if (transform.position.x < XNeg || transform.position.x > Xpos)
            {
                ScaleableObj.sizeDelta = Vector2.Lerp(ScaleableObj.sizeDelta, SmallSize, 6f * Time.deltaTime);
                isClickable = false;
            }
            else
            {
                ScaleableObj.sizeDelta = Vector2.Lerp(ScaleableObj.sizeDelta, OriginalSize, 6f * Time.deltaTime);
                isClickable = true;
            }  
        }
	}

    public void OnClickPublicAreas(string PublicAreaName)
    {
        if(isClickable)
        {
            MultiplayerManager.Instance.SelectPublicAreaToJoin(PublicAreaName);
        }else
        {
            GetComponentInParent<CenterOnClickUI>().CenterOnItem(GetComponent<RectTransform>());    
        }
    }
}
