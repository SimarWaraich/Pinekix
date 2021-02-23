using UnityEngine;
using System.Collections;

public class SelectPartyIcon : MonoBehaviour
{
	public string name;
	// Use this for initialization
	public void SelectIcon ()
	{
		var objname = this.gameObject.name;
		name = objname.ToString ();
		for (int i = 0; i <= HostPartyManager.Instance.PartyIconList.Count; i++) {
			if (i.ToString () == name) {
				FlatPartyHostingControler.Instance.PartyIcon.sprite = HostPartyManager.Instance.PartyIconList [i];
				FlatPartyHostingControler.Instance.PartyIcon.name = name;
				FlatPartyHostingControler.Instance.OpenAndCloseIconScreen (false);
			}
		}
	}

}
