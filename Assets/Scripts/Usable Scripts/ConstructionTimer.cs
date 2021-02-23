using UnityEngine;
using System.Collections;
using System;

public class ConstructionTimer : MonoBehaviour
{
	//	public float StartTimer;
	public TextMesh timerText;
	public bool _isTimerRunnig = false;
	DateTime EndTime;
	int X;
	int Y;

	//	bool stop = true;

	public void StartCountDownTimer (float value, string Name, int x, int y)
	{
		_isTimerRunnig = true;
//		stop = false;

		var endTime = DateTime.UtcNow.AddSeconds (value);
		EndTime = endTime;
//		PlayerPrefs.SetString ("ConstructionTime"+ Name, endTime.ToBinary ().ToString ());
		X = x; Y = y;
		gameObject.name = Name;
	}

	void Update ()
	{
		if (_isTimerRunnig) {
			var Diff = EndTime - DateTime.UtcNow;
			float TimeinFloats = (float)Diff.TotalSeconds;

			if (EndTime < DateTime.UtcNow) {
				RoomPurchaseManager.Instance.CreateRoom (gameObject, X, Y);
				//			GameManager.Instance.AddExperiencePoints (-25);
				var Tut = GameManager.Instance.GetComponent<Tutorial> ();
				if (Tut.enabled && !Tut._LandPurchased) {
					Tut.LandPurchasing ();
				}
				_isTimerRunnig = false;			
//				PlayerPrefs.DeleteKey ("ConstructionTime"+ gameObject.name);
				Destroy (gameObject);
				return;
			}
			timerText.text = ExtensionMethods.GetShortTimeStringFromFloat (TimeinFloats);
		} else
			timerText.text = "0:00";

	}



}
