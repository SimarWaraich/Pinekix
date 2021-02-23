using UnityEngine;
using System.Collections;

public class FlatMateInfo : ScriptableObject
{

	public Sprite Icon;
	public GameObject Prefab;
    [Header ("Gender - 0 for Female, 1 for Male")]
	public int gender;
}

