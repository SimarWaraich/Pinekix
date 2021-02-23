using UnityEngine;
using System.Collections;
using System;

public class FlatmateCoolTimer : MonoBehaviour
{

	public float coolDownTimer;
	public Flatmate flatmate;
	bool _isTimerRunnig = false;
	DateTime EndTime;
	// Use this for initialization
	void Start ()
	{
//		if(PlayerPrefs.HasKey ("FlatMateCoolDownTime" +flatmate.data.Name))
//		{
//			var Temp = Convert.ToInt64 (PlayerPrefs.GetString ("FlatMateCoolDownTime" +flatmate.data.Name));
//			EndTime = DateTime.FromBinary (Temp);
//			_isTimerRunnig = true;
//		}
	}

	public void StartCoolDownTimeForFlatMate (Flatmate _flatmate)
	{
		flatmate = _flatmate;
		EndTime = flatmate.data.CooldownEndTime;
		_isTimerRunnig = true;

//		if (!PlayerPrefs.HasKey ("FlatMateCoolDownTime" + flatmate.data.Name)) {
//			SaveEndTime ();
//		}
	}

//	void SaveEndTime()
//	{
//		var endTime = DateTime.UtcNow.AddSeconds (coolDownTimer);
//		EndTime = flatmate.data.CooldownTime;
//		PlayerPrefs.SetString ("FlatMateCoolDownTime"+flatmate.data.Name, endTime.ToBinary ().ToString ());
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

	void Update()
	{
		if (_isTimerRunnig) 
		{
			flatmate.data.CooldownEndTime = EndTime;
			coolDownTimer = (float)(EndTime - DateTime.UtcNow).TotalMinutes;
			flatmate.data.IsCoolingDown = true;
			if(EndTime < DateTime.UtcNow)
			{
				RoommateManager.Instance.OnCoolDownComplete (flatmate);
				_isTimerRunnig = false;
				Destroy (gameObject);
				return;
			}
		}
	}
}