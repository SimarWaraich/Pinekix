using UnityEngine;
using System.Collections;

public static class PinekixConstatnts 
{
	
}


public static class AssetsPaths 
{
    public const string FlatmatesAssetspath = "Assets/Resources/Flatmates";
    public const string FlatmatesResourcespath = "Flatmates";
    public const string DressesResourcespath = "Dresses";
    public const string SaloonResourcespath = "Hairs";

    public const string DecorResourcespath = "Decors";

}

public static class PinekixUrls {
	public const string signUpUrl = "http://pinekix.ignivastaging.com/players/register";
	public const string signInUrl = "http://pinekix.ignivastaging.com/players/login";
	public const string resetPasswordUrl = "http://pinekix.ignivastaging.com/players/forgotPassword";


	public const string InsertCustCharUrl = "http://pinekix.ignivastaging.com/players/insertcustomcharacter";
	public const string GetCustCharUrl = "http://pinekix.ignivastaging.com/players/getcustomcharacter";

	public const string GetallEventsUrl = "http://pinekix.ignivastaging.com/events/getevents";

	public static string EventRegistrationUrl (string _eventType)
	{
		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkRegistration";
			break;
		case "CoOp_Event":
			link = "http://pinekix.ignivastaging.com/events/coopRegistration";
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decor_eventregister";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshow_eventregister";
			break;
		case "Society_Event":
			link = "http://pinekix.ignivastaging.com/societyEvents/socialEventRegister";

			break;
		}
		return link;
	}

	public static string EventGetVotingPairUrl(string _eventType){		
	
		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkGetVotingPair";
			break;
		case "CoOp_Event":
			link = "http://pinekix.ignivastaging.com/events/coopGetVotingPair";
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decor_getvotingpair";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshow_getvotingpair";

			break;
		}
		return link;
	}

	public static string EventUpdateVotingUrl(string _eventType){		

		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkRegistration"; // UpdateVotedResult Link Confirmed
			break;
		case "CoOp_Event":
			link = "http://pinekix.ignivastaging.com/events/coopUpdateVotedResult";
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decor_updatevotedresult";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshow_updatevotedresult";

			break;
		}
		return link;
	}

	public static string EventGetVotingResultUrl(string _eventType)
	{		

		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkGetVotingResult";
			break;
		case "CoOp_Event":
			link = "http://pinekix.ignivastaging.com/events/coopGetVotingResult";
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decor_getvotedresult";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshow_getvotedresult";

			break;
		}
		return link;
	}

	public static string EventLeaderBoardUrl(string _eventType){
		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkGetVoteData";
			break;
		case "CoOp_Event":
			link = "http://pinekixdev.ignivastaging.com/events/coopGetVotingResult";
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decorGetVoteData";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshowGetVoteData";

			break;
		}
		return link;
	}

	public static string EventResultsUrl(string _eventType){

		var link = "";

		switch (_eventType) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkGetVoteData";
			break;
		case "CoOp_Event":
			link = ""; //"http://pinekix.ignivastaging.com/events/coopGetVoteData";
			Debug.LogError ("UnImplemented Error");
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decorGetVoteData";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshowGetVoteData";
			break;
		}
		return link;
	}

	public static string GetEventRegistration(string category){	
	
		var link = "";

		switch (category) {
		case "CatWalk_Event":
			link = "http://pinekix.ignivastaging.com/events/catwalkGetEventRegistration";
			break;
		case "CoOp_Event":
			link = "";
			Debug.LogError ("UnImplemented Error");
			break;
		case "Decor_Event":
			link = "http://pinekix.ignivastaging.com/events/decor_getregisterevent";
			break;
		case "Fashion_Event":
			link = "http://pinekix.ignivastaging.com/events/fashionshow_getregisterevent";
			break;
		}
		return link;
	}
	public const string ViewFriendsLink = "http://pinekix.ignivastaging.com/unclaimedResults/friendsPair";

	public const string FriendActionUrl = "http://pinekix.ignivastaging.com/friends/friendAction";
	public const string NotificationUrl = "http://pinekix.ignivastaging.com/notifications/notification";
	public const string InvitationUrl = "http://pinekix.ignivastaging.com/invitations/invitation";

	public const string SocietyLink = "http://pinekix.ignivastaging.com/societies/society";

	public const string IndicationUrl = "http://pinekix.ignivastaging.com/societyParties/indication";

    public const string SeasonalClothsListing = "http://pinekix.ignivastaging.com/seasonalClothes/getSeasonalClothsListing";
    public const string GetSavedSeasonalClothsLink = "http://pinekix.ignivastaging.com/seasonalClothes/getPurchasedSeasonalClothsListing";
    public const string SaveSeasonalClothsLink = "http://pinekix.ignivastaging.com/seasonalClothes/saveGiftSeasonalData";

}