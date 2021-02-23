using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ConnectionController : Singleton<ConnectionController>
{
    private const bool allowCarrierDataNetwork = true;

	private string pingAddress = "8.8.8.8";
	// Google Public DNS server
	private const float waitingTime = 2.0f;

	private Ping ping;
	private float pingStartTime;

	public bool _internetOutput = true;
	public bool _serverOutput = true;

	public GameObject InternetPopup;

	public bool _checking = false;


	public void Awake ()
	{
		this.Reload ();
		DontDestroyOnLoad (this.gameObject);
	}

	public void CheckServices ()
	{
		if (_checking)
			return;
		StartCoroutine (IeCheckServices ());

	}

	public IEnumerator IeCheckServices ()
	{
		_checking = true;

		CheckInternet ();
		yield return new WaitUntil (() => ping == null);

		CheckServer ();
		yield return new WaitUntil (() => ping == null);

		if (!_internetOutput) {
			InternetPopup.SetActive (true);
//			Debug.Log ("showing popup of internet");
			InternetPopup.transform.GetChild (0).GetComponent <Text> ().text = "No Internet Available... Please check your connection....";
			yield return false;
			
		} else if (!_serverOutput) {
			InternetPopup.SetActive (true);
//			Debug.Log ("showing popup of server");
			InternetPopup.transform.GetChild (0).GetComponent <Text> ().text = "Server Not accessible... Please try again later....";
			yield return false;
		} else {
//			Debug.Log ("closing popup");
			InternetPopup.SetActive (false);
			yield return true;
		}
			
	}

	public void CheckInternet ()
	{
		bool internetPossiblyAvailable;
		switch (Application.internetReachability) {
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			internetPossiblyAvailable = true;
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			// allowCarrierDataNetwork = true;
			internetPossiblyAvailable = allowCarrierDataNetwork;
			break;
		default:
			internetPossiblyAvailable = false;
			break;
		}
		if (!internetPossiblyAvailable) {
			InternetIsNotAvailable ();
			return;
		}
		pingAddress = "8.8.8.8";
		ping = new Ping (pingAddress);
		pingStartTime = Time.time;
	}

	public void CheckServer ()
	{
		bool internetPossiblyAvailable;
		switch (Application.internetReachability) {
		case NetworkReachability.ReachableViaLocalAreaNetwork:
			internetPossiblyAvailable = true;
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			// allowCarrierDataNetwork = true;
			internetPossiblyAvailable = allowCarrierDataNetwork;
			break;
		default:
			internetPossiblyAvailable = false;
			break;
		}
		if (!internetPossiblyAvailable) {
			InternetIsNotAvailable ();
			return;
		}
		pingAddress = "202.164.59.44";
		ping = new Ping (pingAddress);
		pingStartTime = Time.time;
	}

	public void Update ()
	{
		if (ping != null) {
			bool stopCheck = true;
			if (ping.isDone) {
				if (ping.time >= 0) {
					if (pingAddress == "8.8.8.8")
						InternetAvailable ();
					else
						ServerAvailable ();
				} else {
					if (pingAddress == "8.8.8.8")
						InternetIsNotAvailable ();
					else
						ServerIsNotAvailable ();
				}
			} else if (Time.time - pingStartTime < waitingTime)
				stopCheck = false;
			else
				InternetIsNotAvailable ();
			if (stopCheck)
				ping = null;
		}
	}

	private void InternetIsNotAvailable ()
	{
//		Debug.Log ("No Internet :(");
		_internetOutput = false;
		_checking = false;
	}

	private void InternetAvailable ()
	{
//		Debug.Log ("Internet is available! ;)");
		_internetOutput = true;

	}


	private void ServerIsNotAvailable ()
	{
//		Debug.Log ("No Server Access :(");
		_serverOutput = false;
		_checking = false;
	}

	private void ServerAvailable ()
	{
//		Debug.Log ("Server is available! ;)");
//		ConnectionController.Instance.InternetPopup.SetActive (false);
		_serverOutput = true;
		_checking = false;
	}
}

