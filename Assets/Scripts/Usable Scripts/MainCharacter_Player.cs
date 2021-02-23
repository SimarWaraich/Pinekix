using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCharacter_Player : MonoBehaviour
{

	// Use this for initialization
	private GameObject MainCharacter_GamePlayer;


	public void Awake ()
    {
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;

    }

}
