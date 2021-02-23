using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragSnap : MonoBehaviour
{
	private Vector3 screenPoint;
	public Vector3 offset;
	private Vector3 currPos;
	public GameObject grid;
	public GameObject OnCurruntObject;
//	public List<GameObject> NearestObj;
	public float x = 170;
	public float y = 150;
	public Vector3 _newoffset;

	public bool CanBePlacedHere = true;
    public float Radius = 0.4f;
	public Sprite check_GreenCorrect;
	public Sprite check_RedWrong;
    public List<GameObject> ObjectsUnder =new List<GameObject>();

	void Start ()
	{
        transform.FindChild("Check").GetComponent<SpriteRenderer>().color = Color.white;
	}

	void OnMouseDown ()
	{  
		if (ScreenManager.Instance.OpenedCustomizationScreen != "DecorEvent") {
			if (ScreenManager.Instance.ScreenMoved)
				return;
			if (!enabled)
				return;


			Camera.main.GetComponent<DragCamera1> ().enabled = false;
			ScreenAndPopupCall.Instance.CloseScreen ();

			screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
			
			offset = gameObject.transform.localPosition - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		} else {
			CanBePlacedHere = false;
			screenPoint = transform.root.GetComponent<Camera> ().WorldToScreenPoint (gameObject.transform.localPosition);

			offset = gameObject.transform.localPosition - transform.root.GetComponent<Camera> ().ScreenToViewportPoint (new Vector3 (Input.mousePosition.x - x, Input.mousePosition.y - y, screenPoint.z));

		}
			
	}

	void OnMouseDrag ()
	{
		if (!enabled)
			return;

		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        if (GetComponent<Decor3DView>().isForSociety)
        {
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currPos = curPosition;
            GridSnapbyDistance_DecorEvent(currPos);
        }
        else
        {
            if (ScreenManager.Instance.OpenedCustomizationScreen != "DecorEvent")
            {
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                currPos = curPosition;
                if(!IsCalculating)
                GridSnapByDistance(currPos);
          
            }
            else
            {
                _newoffset = new Vector3(1000 + offset.x, 1000 + offset.y, 1000 + offset.z);
                Vector3 curPosition = gameObject.transform.root.GetComponent<Camera>().ScreenToWorldPoint(curScreenPoint);
                currPos = curPosition;
                GridSnapbyDistance_DecorEvent(currPos);
                print(currPos);
            }
        }
	}

	void OnMouseUp ()
	{
		Camera.main.GetComponent<DragCamera1> ().enabled = true;
    }

	public GameObject nearestFinal = null;

    public bool IsCalculating = false;
    public void GridSnapByDistance (Vector3 Position)
	{
        IsCalculating = true;
        transform.FindChild("Check").GetComponent<SpriteRenderer>().color = Color.white;

		GameObject nearestobject = GameManager.Instance.WayPoints [0];

		float shortestDistance = Vector2.Distance (this.transform.position, nearestobject.transform.position);
		float tempshortdistance = shortestDistance;

		for (int i = 0; i < GameManager.Instance.WayPoints.Count; i++) {
			tempshortdistance = Vector2.Distance (Position, GameManager.Instance.WayPoints [i].transform.position);

			if (tempshortdistance < shortestDistance) {
				nearestobject = GameManager.Instance.WayPoints [i];
				shortestDistance = tempshortdistance;
             
			} else if (nearestobject == GameManager.Instance.WayPoints [0]) {
				nearestobject = GameManager.Instance.WayPoints [i];
			}

            if(tempshortdistance <= Radius)
            {
                if (!ObjectsUnder.Contains(GameManager.Instance.WayPoints[i]))
                    ObjectsUnder.Add(GameManager.Instance.WayPoints [i]);
            }else
            {
                if (ObjectsUnder.Contains(GameManager.Instance.WayPoints[i]))
                    ObjectsUnder.Remove(GameManager.Instance.WayPoints[i]);
            }
		}         
        OnCurruntObject = nearestobject;

		this.transform.position = nearestobject.transform.position;
       
        if (CanBePlaced())
        {          
            nearestFinal = nearestobject;
            transform.FindChild("Check").GetComponent<SpriteRenderer>().sprite = check_GreenCorrect; 
            this.transform.position = nearestobject.transform.position;
            CanBePlacedHere = true;
        }
        else
        {    
            CanBePlacedHere = false;
            transform.FindChild("Check").GetComponent<SpriteRenderer>().sprite = check_RedWrong;
        }

        IsCalculating = false;
	}

    bool CanBePlaced ()
    {        
        bool CanBeplaced = false;
        
        foreach (var obj in ObjectsUnder)
        {
            if (!obj.GetComponent<WayPoint>()._CanBeUsed)
            {
                return false;
            }
        }
        if(!OnCurruntObject.GetComponent<WayPoint>()._CanBeUsed)
        {
            return false;
        }
        return true;
    }

	public void GridSnapbyDistance_DecorEvent (Vector3 Position)
	{
        transform.FindChild("Check").GetComponent<SpriteRenderer>().color = Color.white;

		GameObject nearestobject = grid.transform.GetChild (0).gameObject;
		float shortestDistance = Vector2.Distance (Position, nearestobject.transform.position);
		float tempshortdistance = shortestDistance;

		for (int i = 0; i < grid.transform.childCount; i++) {
			tempshortdistance = Vector2.Distance (Position, grid.transform.GetChild (i).position);

			if (tempshortdistance < shortestDistance) {
				nearestobject = grid.transform.GetChild (i).gameObject;
				shortestDistance = tempshortdistance;
			}
		}

		this.transform.position = nearestobject.transform.position;
		OnCurruntObject = nearestobject;

        if (nearestobject.GetComponent<WayPoint>()._CanBeUsed)
        {          
            nearestFinal = nearestobject;
            transform.FindChild("Check").GetComponent<SpriteRenderer>().sprite = check_GreenCorrect; 
            this.transform.position = nearestobject.transform.position;
            CanBePlacedHere = true;

        }else
        {    
            CanBePlacedHere = false;
            transform.FindChild("Check").GetComponent<SpriteRenderer>().sprite = check_RedWrong;
        }
	}


	/// <summary>
	/// Finds the near game object final. Updated on 16/11/2016
	/// for Decore Placement
	/// </summary>
	/// <param name="target_check">Target check.</param>
	public void FindNearGameObjectFinal (Transform target_check)
	{
		if (ScreenManager.Instance.OpenedCustomizationScreen != "DecorEvent") {
            this.transform.FindChild ("New Sprite").GetComponent<Collider2D> ().isTrigger = true;
	
			for (int i = 0; i < GameManager.Instance.WayPoints.Count; i++) {
				for (int x = 0; x < DecorController.Instance.PlacedDecors.Count; x++) {
					target_check = DecorController.Instance.PlacedDecors [x].transform.FindChild ("New Sprite");
                    if (target_check.gameObject.GetComponent<Collider2D> ().bounds.Contains (GameManager.Instance.WayPoints [i].transform.position)) {

						GameManager.Instance.WayPoints [i].GetComponent<WayPoint> ()._CanBeUsed = false;
//				NearestObj.Add (grid.transform.GetChild (i).gameObject);
						GameManager.Instance.WayPoints [i].gameObject.SetActive (false);
           
					}
				}
			}
		}
	}

    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, Radius);
    }

	public void FindNearGameObjectFinalToEnable (Transform target_check)
	{
		if (ScreenManager.Instance.OpenedCustomizationScreen != "DecorEvent") {
            this.transform.FindChild ("New Sprite").GetComponent<Collider2D> ().isTrigger = true;
			for (int i = 0; i < GameManager.Instance.WayPoints.Count; i++) {
                if (target_check.gameObject.GetComponent<Collider2D> ().bounds.Contains (GameManager.Instance.WayPoints [i].transform.position)) {
					GameManager.Instance.WayPoints [i].GetComponent<WayPoint> ()._CanBeUsed = true;
//				NearestObj.Add (grid.transform.GetChild (i).gameObject);
					GameManager.Instance.WayPoints [i].gameObject.SetActive (true);
				}			
			}
		}
	}

    public void FindNearTilesInSociety (Transform target_check, bool Enabled)
    {
        var WayPointsInGrid = transform.parent.GetComponentsInChildren<WayPoint>(true);
        target_check.GetComponent<Collider2D> ().isTrigger = true;
        for (int i = 0; i < WayPointsInGrid.Length; i++) {
            if (target_check.gameObject.GetComponent<Collider2D> ().bounds.Contains (WayPointsInGrid [i].transform.position)) {
                WayPointsInGrid [i].GetComponent<WayPoint> ()._CanBeUsed = Enabled;
                WayPointsInGrid [i].gameObject.SetActive (Enabled);
            }           
        }
    }
}



