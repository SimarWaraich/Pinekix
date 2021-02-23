using UnityEngine;
using System.Collections;
using System;

public class DecorTimer : MonoBehaviour
{

	public string _type;

	public DecorData _targetDecor;

	public DateTime _endTime;

	public int _eventId;

	TimeSpan _remainingTime;



	void Update ()
	{
		_remainingTime = _endTime - DateTime.Now;

		if (_remainingTime.TotalSeconds < 0) {
			//timer over
			if (_type.ToLower ().Contains ("busy")) {
				UnbusyDecor ();
			} else if (_type.ToLower ().Contains ("cooldown")) {
				EndCooldownDecor ();
			}
		} else {
			//timer running
		}
	}




	void UnbusyDecor ()
	{
		DecorController.Instance.CoolDownOfDecor (_targetDecor.Id, _eventId);
		DecorController.Instance._timers.Remove (this.gameObject);
//		DecorController.Instance.ShowRewards (_eventId);
		Destroy (this.gameObject);
	}


	void EndCooldownDecor ()
	{
		DecorController.Instance._timers.Remove (this.gameObject);
		foreach (var decor in DecorController.Instance.AllDecores) {
			if (decor.Id == _targetDecor.Id) {
				decor._isCoolingDown = false;
			}
		}
		Destroy (this.gameObject);
	}




}
