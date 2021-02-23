using UnityEngine;
using System.Collections;

public class ArrowPingPong : MonoBehaviour
{	Vector2 transformPosition;
	Transform myRectTrans;

	public bool horizontal = true;
	public bool vertical;

	float speed = 0.8f;
	float movementOffset = 0.3f;

	void Start () 
	{
		myRectTrans = transform;
		transformPosition = myRectTrans.localPosition;
	}


	void Update () 
	{
		if(horizontal)
		{
			myRectTrans.localPosition = new Vector3(transformPosition.x + Mathf.PingPong(Time.time * speed, movementOffset), myRectTrans.localPosition.y);
		}
		if(vertical)
		{
			myRectTrans.localPosition= new Vector3 (myRectTrans.localPosition.x, transformPosition.y + Mathf.PingPong (Time.time * speed, movementOffset));			
		}
	}
}

