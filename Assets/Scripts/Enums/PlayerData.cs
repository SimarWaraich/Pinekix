/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 13 July 2k16
/// This script will be used to store the player data
/// </summary>

using System.Collections.Generic;
using System;


[Serializable]
public class PlayerData
{
	public bool IsLoggedIn;
	public string Name;
	public string Username;
	public string EmailId;
	public int player_id;
	public int NumberofRoommates;
    public Dictionary<string, int> DressWeared = new Dictionary<string,int > ();
	public Dictionary<string, string> RoomMatesId = new Dictionary<string, string> ();
}


public class PurchaseData
{
	public int player_id;
	public int item_id;
	public string cat;
	public string sub_cat;
}

[Serializable]
public class CustomCharacter
{
	public int player_id;
	public int gender;
	public int skin_tone;
	public string eyes;
	public string nose;
	public string lips;
	public string ears;
	public string hair;
	public string shoes;
	public string clothing;

}

public class PositionUpdate
{
	public int player_id;
	public int item_id;
	public string position;
    public string rotation;
	public string is_busy_time;
	public string cool_time;
	public int cool_down_time_event_id;
}

public class PositionGet
{
	public int player_id;
}


public class LogoutData
{
	public int player_id;
	public int coins;
	public int gems;
	public float expirence_points;
	public float expirence_level;
	public string tutorial_status;
	public string logout_time;
	public float flatPartyCoolDown;
}

[Serializable]
public class FlatUpdateData
{
	public int player_id;
	public int item_id;
	public string position;
	public string wall_texture;
	public string ground_texture;
}
