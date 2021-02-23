/// <summary>
/// Created by ::==>> Mandeep Yadav.. Dated 08 July 2k16
/// Asset bundle exporter.
/// </summary>

using UnityEditor;

public class AssetBundleExporter
{

	[MenuItem ("Assets Exporter/Build AssetBundle For Mac OSX")]
	static void BuildAssetBundleForMac ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/OSX", BuildAssetBundleOptions.None, BuildTarget.StandaloneOSXUniversal);
	}

	[MenuItem ("Assets Exporter/Build AssetBundle For Android")]
	static void BuildAssetBundleForAndroid ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
	}

	[MenuItem ("Assets Exporter/Build AssetBundle For IOS")]
	static void BuildAssetBundleForIos ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/IOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
	}

	[MenuItem ("Assets Exporter/Build AssetBundle For Windows")]
	static void BuildAssetBundleForWindows ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/Windows", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
	}

	[MenuItem ("Assets Exporter/Build AssetBundle For Windows64")]
	static void BuildAssetBundleForWindows64 ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/Windows64", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
	}

	[MenuItem ("Assets Exporter/Build AssetBundle For OnlineGames")]
	static void BuildAssetBundleForWeb ()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles/Web", BuildAssetBundleOptions.None, BuildTarget.WebGL);
	}
}
