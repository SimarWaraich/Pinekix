using UnityEngine;
using System.Collections;

public class PingPongUI : MonoBehaviour
{
	Vector2 transformPosition;
	Vector2 InitialPosition;
	RectTransform myRectTrans;

	public bool horizontal = true;
	public bool vertical;

	float speed = 40f;
	float movementOffset = 20;
//	Vector3 childPos;

	void Awake ()
	{
//		childPos = this.gameObject.transform.GetChild (0).transform.position;
		myRectTrans = GetComponent <RectTransform> ();
		InitialPosition = myRectTrans.anchoredPosition;
	}
	void Start(){
		print ("Start"); 
	}
	void OnEnable ()
	{
		transformPosition = myRectTrans.anchoredPosition;
	}

	//	void SetTarget()
	//	{
	//		if(vertical)
	//		{
	//			Vector2 targetposition = new Vector2 (transformPosition.x, transformPosition.y + 10);
	//		}
	//		if(horizontal)
	//		{
	//			Vector2 targetposition = new Vector2 (transformPosition.x+10, transformPosition.y);
	//		}
	//
	//	}

	void Update ()
	{
		if (horizontal) {
			myRectTrans.anchoredPosition = new Vector3 (transformPosition.x + Mathf.PingPong (Time.time * speed, movementOffset), myRectTrans.anchoredPosition.y);

//			this.gameObject.transform.GetChild (0).transform.position = childPos;
		}
		if (vertical) {
			myRectTrans.anchoredPosition = new Vector3 (myRectTrans.anchoredPosition.x, transformPosition.y + Mathf.PingPong (Time.time * speed, movementOffset));	

//			this.gameObject.transform.GetChild (0).transform.position = childPos;
		}
	}

	void OnDisable()
	{
		myRectTrans.anchoredPosition = InitialPosition;
	}
}
