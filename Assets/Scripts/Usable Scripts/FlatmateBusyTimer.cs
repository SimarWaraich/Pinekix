using UnityEngine;
using System.Collections;
using System;

public class FlatmateBusyTimer : MonoBehaviour
{

	public float busyTimeRemaining;
	public Flatmate flatmate;
	bool _isTimerRunnig = false;
	DateTime EndTime;
	public int VIPPoint;
	// Use this for initialization
	void Start ()
	{
//		if(flatmate){
//			if(PlayerPrefs.HasKey ("FlatMateCountTime" +flatmate.data.Name + PlayerPrefs.GetInt ("PlayerId")))
//			{
//				var Temp = Convert.ToInt64 (PlayerPrefs.GetString ("FlatMateCountTime" +flatmate.data.Name + PlayerPrefs.GetInt ("PlayerId")));
//				EndTime = DateTime.FromBinary (Temp);
//				_isTimerRunnig = true;
//			}
//		}else{
//			Destroy (gameObject);
//		}
	}

	public void StartBusyTimeForFlatMate (Flatmate _flatmate)
	{
		flatmate = _flatmate;
		EndTime = flatmate.data.BusyTimeRemaining;
		_isTimerRunnig = true;

		VIPPoint = VipSubscriptionManager.Instance.VipSubcritionActived ();
	}

	//	void SaveEndTime()
	//	{
	//		var endTime = DateTime.UtcNow.AddSeconds (busyTimeRemaining);
	//		EndTime = endTime;
	//		PlayerPrefs.SetString ("FlatMateCountTime"+flatmate.data.Name + PlayerPrefs.GetInt ("PlayerId"), endTime.ToBinary ().ToString ());
	//	}
	//	IEnumerator Timer (float time)
	//	{
	//		for (int i = Mathf.FloorToInt (time); i > 0; i--) {
	//			busyTimeRemaining--;
	//			flatmate.data.busyTimeRemaining = busyTimeRemaining;
	//			yield return new WaitForSeconds (1f);
	//		}
	//		RoommateManager.Instance.UnbusyFlatmate (flatmate);
	//		Destroy (gameObject);
	//	}

	void Update ()
	{
		if (_isTimerRunnig) {
			var Diff = EndTime - DateTime.UtcNow;
			busyTimeRemaining = (float)Diff.TotalSeconds;
			flatmate.data.busyTimeRemaining = busyTimeRemaining;

			flatmate.data.BusyTimeRemaining = EndTime;
			flatmate.data.IsBusy = true;

			if (EndTime < DateTime.UtcNow) {
				RoommateManager.Instance.UnbusyFlatmateAfterClass (flatmate, VIPPoint);
				PlayerPrefs.DeleteKey ("FlatMateCountTime" + flatmate.data.Name + PlayerPrefs.GetInt ("PlayerId"));			
				_isTimerRunnig = false;			
				Destroy (gameObject);
				return;
			}
		}
	}
}