/// <summary>
/// Created By ::==>> Mandeep Yadav... Dated 07 July 2k16
/// This script will be used to store the room mate data
/// </summary>

using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class RoommateData : IComparable <RoommateData>
{
	public string Name;
	public int Id;
	public GenderEnum Gender;

	public Sprite Icon;
	public int Level;
	public int Gems;
	public int Price;
	public bool Unlocked;

	public bool Hired;
	public float MoneyPerHour;
	public bool IsBusy;

	public bool IsCoolingDown;


	public DateTime CooldownEndTime;
	public DateTime BusyTimeRemaining;
	public float busyTimeRemaining = 0;
	public GameObject Prefab;

	public BusyType BusyType;
	public int EventBusyId;

	public string Hair_style;
    public Dictionary<string, int> Dress = new Dictionary<string, int>();
	public string Perk;
	public int Perk_value;
	public float education_point;
	public int education_level;
    public bool VipSubscription;

    public RoommateData (string name,int id, int gender, Sprite icon, int level, int gems, int price, bool unlocked, bool hired, GameObject prefab, bool Vip)
	{
		Name = name;
		Id = id;
		Gender = (GenderEnum)gender;
		Icon = icon;
		Level = level;
		Gems = gems;
		Price = price;
		Unlocked = unlocked;
		Hired = hired;
		Prefab = prefab;
		education_level = 1;
		education_point = 0;
        VipSubscription = Vip;
        Dress = new Dictionary<string, int> ();
	}

	public int CompareTo(RoommateData other)
	{
		return this.Price.CompareTo(other.Price);
	}

	public RoommateData ()
	{
		
	}
}



public class RoommateSaveData
{
	public int player_id;
	public int item_id;
	public string name;
	public string gender;
	public string perk;
	public int perk_value;
	public string dress;
	public string hair_style;
	public int education_point;
	public float education_point_level;
	public bool is_busy;
	public string busy_time;
	public string cooldown_time;
	public string busy_type;
	public int cooldown_time_event_id;
}

public class RoommateGetData
{
	public int player_id;
	public int item_id;

	public RoommateGetData (int _playerID, int _ItemID)
	{
		player_id = _playerID;
		item_id = _ItemID;
	}
}

public enum BusyType
{
	Class, 
	FashionEvents,
	CatwalkEvents,
	CoopEvent
}

