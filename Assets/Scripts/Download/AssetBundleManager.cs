using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

static public class AssetBundleManager
{

	// A dictionary to hold the AssetBundle references
	static private Dictionary<string, AssetBundleRef> dictAssetBundleRefs;

	static AssetBundleManager ()
	{
		dictAssetBundleRefs = new Dictionary<string, AssetBundleRef> ();
	}
	// Class with the AssetBundle reference, url and version
	private class AssetBundleRef
	{
		public AssetBundle assetBundle = null;
		public int version;
		public string url;

		public AssetBundleRef (string strUrlIn, int intVersionIn)
		{
			url = strUrlIn;
			version = intVersionIn;
		}
	};
	// Get an AssetBundle
	public static AssetBundle getAssetBundle (string url, int version)
	{
		string keyName = url + version.ToString ();
		AssetBundleRef abRef;
		if (dictAssetBundleRefs.TryGetValue (keyName, out abRef))
			return abRef.assetBundle;
		else
			return null;
	}


	// Download an AssetBundle
	public static IEnumerator downloadAssetBundle (string url, int version)
	{

		string keyName = url + version.ToString ();
		bool InternetError = false;
		AssetBundleManager.Unload (url, version, false);

//		float time = 0;

		using (WWW www = WWW.LoadFromCacheOrDownload (url, version)) {
			while (!www.isDone) {

				CoroutineWithData cd = new CoroutineWithData (ConnectionController.Instance, ConnectionController.Instance.IeCheckServices ());
				yield return cd.coroutine;

				if (cd.result.ToString () == "True" || cd.result.ToString () == "true") {
					ConnectionController.Instance.InternetPopup.SetActive (false);
				} else {
//					InternetError = true;
					yield return null;
				}
				yield return null;
			}
			if (www.error != null || InternetError) {
				///will remove the old file from cashe which cause the problem 
				AssetBundleManager.Unload (url, version, false);
//				DownloadContent.Instance.reporttext.text = "Download error please retry " + www.error;
				Debug.Log ("Error : " + www.error);
				DownloadContent.Instance.StartCoroutines ();

			} else {
				AssetBundleRef abRef = new AssetBundleRef (url, version);
				abRef.assetBundle = www.assetBundle;

				if (!dictAssetBundleRefs.ContainsKey (keyName)) {
					dictAssetBundleRefs.Add (keyName, abRef);
				} else {
					//				DownloadContent.Instance.reporttext.text = "Asset is unloading plz wait";
					Debug.Log ("This is Just Test that how we can unload asset which is in cache");
					AssetBundleManager.Unload (url, version, false);
				}
			}
		}
	}
	// Unload an AssetBundle
	public static void Unload (string url, int version, bool allObjects)
	{
		string keyName = url + version.ToString ();
		AssetBundleRef abRef;
		if (dictAssetBundleRefs.TryGetValue (keyName, out abRef)) {
			abRef.assetBundle.Unload (allObjects);
			abRef.assetBundle = null;
			dictAssetBundleRefs.Remove (keyName);
		}
	}


	public static void UnloadAll ()
	{
		foreach (var kvp in dictAssetBundleRefs) {
			if (kvp.Value.assetBundle != null)
				kvp.Value.assetBundle.Unload (true);

		}	
        dictAssetBundleRefs.Clear();

//		Debug.Log ("data cleared");
	}


	public static bool CheckForData (string link)
	{
		if (dictAssetBundleRefs.ContainsKey (link)) {
			return true;
		} else {
			return false;
		}
	}


	public static IEnumerator RetryDownload (string link)
	{
		string keyName = link + "1";
		bool InternetError = false;
		AssetBundleManager.Unload (link, 1, false);

		using (WWW www = WWW.LoadFromCacheOrDownload (link, 1)) {
			while (!www.isDone) {
				yield return ConnectionController.Instance.StartCoroutine (ConnectionController.Instance.IeCheckServices ());
				if (!ConnectionController.Instance._checking && (!ConnectionController.Instance._internetOutput || !ConnectionController.Instance._serverOutput)) {
					InternetError = true;
					break;
				} else if (!ConnectionController.Instance._checking) {
					yield return new WaitForSeconds (1f);
				} else if (ConnectionController.Instance._internetOutput || ConnectionController.Instance._serverOutput) {
					ConnectionController.Instance.InternetPopup.SetActive (false);
				}
				yield return null;
			}
			if (www.error != null || InternetError) {
				///will remove the old file from cashe which cause the problem 
				AssetBundleManager.Unload (link, 1, false);
				//				DownloadContent.Instance.reporttext.text = "Download error please retry " + www.error;
				Debug.Log ("Error : " + www.error);
				DownloadContent.Instance.StopAllCoroutines ();
				DownloadContent.Instance.IeCheckForDownloadList ();

			} else {
				AssetBundleRef abRef = new AssetBundleRef (link, 1);
				abRef.assetBundle = www.assetBundle;

				if (!dictAssetBundleRefs.ContainsKey (keyName)) {
					dictAssetBundleRefs.Add (keyName, abRef);
				} else {
					//				DownloadContent.Instance.reporttext.text = "Asset is unloading plz wait";
					Debug.Log ("This is Just Test that how we can unload asset which is in cache");
					AssetBundleManager.Unload (link, 1, false);
				}
			}
		}
	}


}
