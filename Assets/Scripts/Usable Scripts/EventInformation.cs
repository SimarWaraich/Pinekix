using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EventInformation : MonoBehaviour
{
	public string RewardInformation;
	// Use this for initialization
	void Start ()
	{

		transform.GetChild (3).GetChild (0).GetComponent<Text> ().text = PlayerPrefs.GetString ("UserName").ToUpper ();

		transform.GetChild (4).GetComponentInChildren<Text> ().text = RewardInformation;

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
