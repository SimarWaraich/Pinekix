using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class IndicationManager : Singleton<IndicationManager>
{
	void Awake ()
	{
		this.Reload ();

//		Indications.Add ("Event");
//		Indications.Add ("Society");
		StartCoroutine (GetIndications (false));
	}

	void Start ()
	{
		InvokeRepeating ("GetIndicationByInvoke", 60f, 60f);

	}

    void GetIndicationByInvoke()
    {
        StartCoroutine(IeGetIndication());
    }

    IEnumerator IeGetIndication()
    {
        if(ScreenManager.Instance.ScreenMoved)
               yield return new WaitUntil(() => ScreenManager.Instance.ScreenMoved == null);
        StartCoroutine(GetIndications(ScreenManager.Instance.ScreenMoved != null));        
    }
        

	public List <Indication> Indications = new List<Indication> ();

	public Button PhoneMenu;
	public Button EventsButton;

	public Button SocietyButton;
	public Button MySocietyButton;
	public Button JoinPartyButton;
	public Button JoinPartyButton2;

	public Button ContactsButton;
	public Button RequestButton;

	public Button NotificationButton;
	public Button InvitationButton;

	public UnityEngine.EventSystems.EventSystem eventsystem;
	GameObject lasteventgameobject;

	//	int Step = 0;
	public void InItIndications ()
	{
		foreach (var Badge in Indications) { 
			if (Badge.Name == "Society") {                
				StartCoroutine (IsPartyGoingOn (Badge));
			} else
				ShowBadges (Badge, 1);
		}		
	}

	IEnumerator IsPartyGoingOn (Indication Indication)
	{
		var cd = new CoroutineWithData (SocietyPartyManager.Instance, SocietyPartyManager.Instance.IGetSocietyPartyForCheck (Indication.IdOfIdicatorObject));
		yield return cd.coroutine;
		bool boolean = false;
		if (cd.result != null) {
			if ((bool)cd.result)
				ShowBadges (Indication, 1);
			else
				DeleteIndication (Indication.IdOfIdicatorObject, Indication.Name);
            
		}
	}

	public void ShowBadges (Indication indication, int Step)
	{
		if (indication.Name.Contains ("Event")) {
			switch (Step) {
			case 1:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (true);			
				EventsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);	
				break;
			case 2:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				EventsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);
				break;
			case 3:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				EventsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				var Childs = EventManagment.Instance.EventContainer.GetComponentsInChildren<EventInfo> ();
				FindBadgeWithId (Childs, indication).ShowBagdeOnThisButton (true);                        
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				EventsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			case 4:               
				Childs = EventManagment.Instance.EventContainer.GetComponentsInChildren<EventInfo> ();                    
				FindBadgeWithId (Childs, indication).ShowBagdeOnThisButton (false);
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				EventsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			}
		}
		if (indication.Name.Contains ("Society")) {
			switch (Step) {
			case 1:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (true);			
				SocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);	
				MySocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				JoinPartyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			case 2:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);
				MySocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				JoinPartyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				JoinPartyButton2.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			case 3:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				MySocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);
				JoinPartyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);             
				JoinPartyButton2.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
                    
				break;
			case 4:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				MySocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyManager.Instance.MySocietyContainer.GetComponentInChildren<Badge> ().ShowBagdeOnThisButton (true);
				JoinPartyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				JoinPartyButton2.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;	
			case 5:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				MySocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyManager.Instance.MySocietyContainer.GetComponentInChildren<Badge> ().ShowBagdeOnThisButton (false);
				JoinPartyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);
				JoinPartyButton2.GetComponent <Badge> ().ShowBagdeOnThisButton (true);
				break;
			case 6:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				MySocietyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				SocietyManager.Instance.MySocietyContainer.GetComponentInChildren<Badge> ().ShowBagdeOnThisButton (false);
				JoinPartyButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false); 
				JoinPartyButton2.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				DeleteIndication (indication.IdOfIdicatorObject, indication.Name);
				break;
			}
		}
		if (indication.Name.Contains ("Request")) {
			switch (Step) {
			case 1:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (true);         
				ContactsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);    
				RequestButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			case 2:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);         
				ContactsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);    
				RequestButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			case 3:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);         
				ContactsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);    
				RequestButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);
				break;
			case 4:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);         
				ContactsButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);    
				RequestButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				DeleteIndication (1, "Request");

				Step = 0;
				InItIndications ();

				break;  
			}
		}
		if (indication.Name.Contains ("Notification") || indication.Name.Contains ("Invitation")) {
			switch (Step) {
			case 1:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (true);         
				NotificationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);    
				InvitationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			case 2:
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);         
				NotificationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);    
				InvitationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
				break;
			case 3:                   
				PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);                   
				if (indication.Name.Contains ("Notification")) {
					NotificationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
					InvitationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
					DeleteIndication (1, "Notification");
				} else if (indication.Name.Contains ("Invitation")) {
					NotificationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
					InvitationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (true);
				}
				break;
			case 4:
				if (indication.Name.Contains ("Invitation")) {
					PhoneMenu.GetComponent <Badge> ().ShowBagdeOnThisButton (false);         
					NotificationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);    
					InvitationButton.GetComponent <Badge> ().ShowBagdeOnThisButton (false);
					DeleteIndication (1, "Invitation");
				}
				break;  
			}
		}

	}

	public void IncrementIndicationFor (string ButtonClicked, int Step)
	{
		foreach (var Badge in Indications) { 
			if (ButtonClicked.Contains (Badge.Name)) {
				ShowBadges (Badge, Step);
			}
		}
	}

	public void IncrementIndicationForRequest (int Step)
	{
		foreach (var Badge in Indications) { 
			if (Badge.Name.Contains ("Request")) {
				ShowBadges (Badge, Step);
			}
		}
	}

	public void IncrementIndicationForNotification (int Step)
	{
		foreach (var Badge in Indications) { 
			if (Badge.Name.Contains ("Notification") || Badge.Name.Contains ("Invitation")) {
				ShowBadges (Badge, Step);
			}
		}
	}

	IEnumerator GetIndications (bool IsAnyScreenOpen)
	{
		Indications.Clear ();
		var encoding = new System.Text.UTF8Encoding ();


		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
//		{
//			"data_type": "view",
//			"player_id": 10
		//		}	

		jsonElement ["data_type"] = "view";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (PinekixUrls.IndicationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);
//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

//		{"status":"200","description":"Data has following.",
//		"data":[
//		{"player_id":"10","message":"hkjsflkasd alksndflaksd alsjkdaksd asd","message_type":"test"},
//		{"player_id":"10","message":"hkjsflkasd alksndflaksd alsjkdaksd asd","message_type":"test"}
//		]
//	}

		if (www.error == null) {
			Simple_JSON.JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			if (_jsnode ["description"].ToString ().Contains ("Data has following") || _jsnode ["status"].ToString ().Contains ("200")) {

				for (int i = 0; i < _jsnode ["data"].Count; i++) {
					var data = _jsnode ["data"] [i];
					var id = 0;
					int.TryParse (data ["indication_id"], out id);
                   
					var eventId = 0;
					int.TryParse (data ["message"], out eventId);

					Indications.Add (new Indication (id, data ["message_type"], eventId));
				}
			} else {
				
			}
		} else {
			
		}
		if (!IsAnyScreenOpen)
			InItIndications ();
	}

	public void SendIndicationToUsers (int[] UserIds, string Indication, int IdOfIndicator = 1)
	{
		StartCoroutine (ISendIndicationToUsers (UserIds, Indication, IdOfIndicator.ToString ()));
	}

	public void SendIndicationToUsers (int[] UserIds, string Indication, string message)
	{
		StartCoroutine (ISendIndicationToUsers (UserIds, Indication, message));
	}

	IEnumerator ISendIndicationToUsers (int[] UserIds, string Indication, string message)
	{
		var encoding = new System.Text.UTF8Encoding ();


		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();
//		{
//			"data_type": "save",
//			"players": [{"id" : 10}, { "id" : 11}],
//			"message":"hkjsflkasd alksndflaksd alsjkdaksd asd",
//			"message_type":"test"
//		}
		jsonElement ["data_type"] = "save";

		var jsonarray = new Simple_JSON.JSONArray ();

		foreach (int id in UserIds) {
			var jsonItem = new Simple_JSON.JSONClass ();
			jsonItem ["id"] = id.ToString ();

			jsonarray.Add (jsonItem);
		}

		jsonElement ["players"] = jsonarray;
		jsonElement ["message"] = message;
		jsonElement ["message_type"] = Indication;

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (PinekixUrls.IndicationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);
//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		if (www.error == null) {
			Simple_JSON.JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			if (_jsnode ["description"].ToString ().Contains ("Data has following") || _jsnode ["status"].ToString ().Contains ("200")) {

			} else {

			}
		} else {

		}
	}

	public void DeleteIndication (int eventId, string Name)
	{

		var Temp = new List<Indication> ();

		for (int i = 0; i < Indications.Count; i++) {
			if (Indications [i].IdOfIdicatorObject == eventId && Indications [i].Name == Name) {              
				StartCoroutine (IDeleteIndication (Indications [i].Id));
			} else {
				Temp.Add (Indications [i]);
			}
		}
		Indications = Temp;
	}

	public IEnumerator IDeleteIndication (int IndicationId)
	{
		var encoding = new System.Text.UTF8Encoding ();


		Dictionary<string,string> postHeader = new Dictionary<string,string> ();
		var jsonElement = new Simple_JSON.JSONClass ();


		jsonElement ["data_type"] = "delete";
		jsonElement ["player_id"] = PlayerPrefs.GetInt ("PlayerId").ToString ();
		jsonElement ["indication_id"] = IndicationId.ToString ();

		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonElement.Count.ToString ());

		WWW www = new WWW (PinekixUrls.IndicationUrl, encoding.GetBytes (jsonElement.ToString ()), postHeader);
//		print ("jsonDtat is ==>> " + jsonElement.ToString ()); 
		yield return www;

		//		{"status":"200","description":"Data has following.",
		//		"data":[
		//		{"player_id":"10","message":"hkjsflkasd alksndflaksd alsjkdaksd asd","message_type":"test"},
		//		{"player_id":"10","message":"hkjsflkasd alksndflaksd alsjkdaksd asd","message_type":"test"}
		//		]
		//	}

		if (www.error == null) {
			Simple_JSON.JSONNode _jsnode = Simple_JSON.JSON.Parse (www.text);
			if (_jsnode ["description"].ToString ().Contains ("Data has following") || _jsnode ["status"].ToString ().Contains ("200")) {

			} else {

			}
		} else {

		}
	}

	Badge FindBadgeWithId (EventInfo[] List, Indication indication)
	{
		foreach (var go in List) {
			if (go.Event_id == indication.IdOfIdicatorObject) {
				var badge = go.GetComponent<Badge> ();
				badge.thisIndication = indication;
				return go.GetComponent<Badge> ();
			}
		}
		return null;

	}
}

[Serializable]
public class Indication
{
	public int Id;
	public string Name;
	public int IdOfIdicatorObject;

	public Indication (int id, string name, int eventId)
	{
		Id = id;
		Name = name;
		IdOfIdicatorObject = eventId;
	}
}
