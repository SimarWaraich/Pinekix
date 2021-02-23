using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
[CustomEditor (typeof(DressInfo))]
public class LevelScriptEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		DressInfo myScript = (DressInfo)target;
		if (GUILayout.Button ("Apply This Dress")) {
			myScript.ApplyDress ();
		}
	}
}

[CustomEditor (typeof(SaloonInfo))]
public class SaloonInfoEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();

        SaloonInfo myScript = (SaloonInfo)target;
        if (GUILayout.Button ("Apply this hairs")) {
            myScript.ChangeHairs ();
        }
    }
}

[CustomEditor (typeof(CharacterProperties))]
public class CharacterColor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		CharacterProperties myScript = (CharacterProperties)target;
		if (GUILayout.Button ("Change Color of skin")) {
			myScript.ChangeColor ();
		}
	}
}
[CustomEditor (typeof(ShoesInfo))]
public class ShoesInfoEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        DrawDefaultInspector ();

        ShoesInfo myScript = (ShoesInfo)target;
        if (GUILayout.Button ("Apply this Shoes")) {
            myScript.ChangeShoes ();
        }
    }
}

#endif
