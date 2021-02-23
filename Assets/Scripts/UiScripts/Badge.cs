using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Badge : MonoBehaviour 
{
	public Sprite NormalSprite;
	public Sprite HighLightedSprite;

    public Indication thisIndication;

	RectTransform myRectTrans;
	float speed = 0.2f;
	float maxScale = 0.1f;
	Vector3 MyScale;
	bool IsActive = false;
	Transform BadgeObj;

	void Start(){
		BadgeObj = transform.FindChild ("Badge");
		myRectTrans = BadgeObj.GetComponent <RectTransform> ();
		MyScale = myRectTrans.localScale;
	}

	public void ShowBagdeOnThisButton(bool isActive)
	{
		IsActive = isActive;
		GetComponent <Button>().image.sprite = isActive ? HighLightedSprite : NormalSprite ;
		transform.FindChild ("Badge").gameObject.SetActive (isActive);
	}

	void Update ()
	{
		if(IsActive)
			myRectTrans.localScale = new Vector2 ( MyScale.x + Mathf.PingPong (Time.time * speed, maxScale), MyScale.y + Mathf.PingPong (Time.time * speed, maxScale));	
	}
}
