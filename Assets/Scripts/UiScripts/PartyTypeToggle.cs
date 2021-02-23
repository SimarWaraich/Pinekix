using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PartyTypeToggle : MonoBehaviour, IPointerDownHandler
{

	public void OnPointerDown (PointerEventData data)
	{
		FlatPartyHostingControler.Instance.PartyTypeCommon ();
		var Tut = GameManager.Instance.GetComponent<Tutorial> ();
		if (!Tut.HostPartyCreated && Tut.hostPartyTutorial == 9)
			Tut.HostPartyTutorial ();
	}
}
