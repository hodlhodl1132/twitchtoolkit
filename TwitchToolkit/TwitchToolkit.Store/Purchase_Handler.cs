using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store;

[StaticConstructorOnStartup]
public static class Purchase_Handler
{
	public static List<StoreIncidentSimple> allStoreIncidentsSimple;

	public static List<StoreIncidentVariables> allStoreIncidentsVariables;

	public static List<string> viewerNamesDoingVariableCommands;

	static Purchase_Handler()
	{
		allStoreIncidentsSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList();
		allStoreIncidentsVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList();
		Helper.Log("trying to load vars after def database loaded");
		((Mod)Toolkit.Mod).GetSettings<ToolkitSettings>();
		viewerNamesDoingVariableCommands = new List<string>();
	}

	public static void ResolvePurchase(Viewer viewer, ITwitchMessage twitchMessage, bool separateChannel = false)
	{
		Log.Warning("reached purchase resolver");
		List<string> command = twitchMessage.Message.Split(' ').ToList();
		if (command.Count < 2)
		{
			return;
		}
		if (command[0] == "!levelskill")
		{
			command[0] = "levelskill";
			command.Insert(0, "!buy");
		}
		string productKey = command[1].ToLower();
		string formattedMessage = string.Join(" ", command.ToArray());
		StoreIncidentSimple incident = allStoreIncidentsSimple.Find(s => productKey.ToLower() == s.abbreviation);
		if (incident != null)
		{
			ResolvePurchaseSimple(viewer, twitchMessage, incident, formattedMessage);
			return;
		}
		StoreIncidentVariables incidentVariables = allStoreIncidentsVariables.Find((StoreIncidentVariables s) => productKey.ToLower() == s.abbreviation);
		if (incidentVariables != null)
		{
			ResolvePurchaseVariables(viewer, twitchMessage, incidentVariables, formattedMessage);
			return;
		}
		Item item = StoreInventory.items.Find((Item s) => s.abr == productKey);
		Helper.Log("abr: " + productKey + " ");
		if (item != null)
		{
			List<string> commandSplit = twitchMessage.Message.Split(' ').ToList();
			commandSplit.Insert(1, "item");
			if (commandSplit.Count < 4)
			{
				commandSplit.Add("1");
			}
			if (!int.TryParse(commandSplit[3], out var _))
			{
				commandSplit.Insert(3, "1");
			}
			formattedMessage = string.Join(" ", commandSplit.ToArray());
			ResolvePurchaseVariables(viewer, twitchMessage, StoreIncidentDefOf.Item, formattedMessage);
		}
	}

	public static void ResolvePurchaseSimple(Viewer viewer, ITwitchMessage twitchMessage, StoreIncidentSimple incident, string formattedMessage, bool separateChannel = false)
	{
        int cost = incident.cost;
		if (cost < 1 || CheckIfViewerIsInVariableCommandList(viewer.username) || !CheckIfViewerHasEnoughCoins(viewer, cost) || CheckIfKarmaTypeIsMaxed(incident, viewer.username) || CheckIfIncidentIsOnCooldown(incident, viewer.username))
		{
			return;
		}
		IncidentHelper helper = StoreIncidentMaker.MakeIncident(incident);
		if (helper == null)
		{
			Helper.Log("Missing helper for incident " + incident.defName);
			return;
		}
		if (!helper.IsPossible())
		{
			TwitchWrapper.SendChatMessage(("@" + viewer.username + " " + Translator.Translate("TwitchToolkitEventNotPossible")));
			return;
		}
		if (!ToolkitSettings.UnlimitedCoins)
		{
			viewer.TakeViewerCoins(cost);
		}
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		helper.Viewer = viewer;
		helper.message = formattedMessage;
		Ticker.IncidentHelpers.Enqueue(helper);
		Store_Logger.LogPurchase(viewer.username, formattedMessage);
		component.LogIncident(incident);
		viewer.CalculateNewKarma(incident.karmaType, cost);
		if (ToolkitSettings.PurchaseConfirmations)
		{
			TwitchWrapper.SendChatMessage(Helper.ReplacePlaceholder((Translator.Translate("TwitchToolkitEventPurchaseConfirm")), null, null, null, null, null, null, null, null, null, null, null, null, first: GenText.CapitalizeFirst(((Def)incident).label), viewer: viewer.username));
		}
	}

	public static void ResolvePurchaseVariables(Viewer viewer, ITwitchMessage twitchMessage, StoreIncidentVariables incident, string formattedMessage, bool separateChannel = false)
	{
		int cost = incident.cost;
		if ((cost < 1 && ((Def)incident).defName != "Item") || CheckIfViewerIsInVariableCommandList(viewer.username) || !CheckIfViewerHasEnoughCoins(viewer, cost))
		{
			return;
		}
		if (incident != DefDatabase<StoreIncidentVariables>.GetNamed("Item", true))
		{
			if (CheckIfKarmaTypeIsMaxed(incident, viewer.username))
			{
				return;
			}
		}
		else if (CheckIfCarePackageIsOnCooldown(viewer.username))
		{
			return;
		}
		if (CheckIfIncidentIsOnCooldown(incident, viewer.username))
		{
			return;
		}
		viewerNamesDoingVariableCommands.Add(viewer.username);
		IncidentHelperVariables helper = StoreIncidentMaker.MakeIncidentVariables(incident);
		if (helper == null)
		{
			Helper.Log("Missing helper for incident " + ((Def)incident).defName);
			return;
		}
		if (!helper.IsPossible(formattedMessage, viewer))
		{
			if (viewerNamesDoingVariableCommands.Contains(viewer.username))
			{
				viewerNamesDoingVariableCommands.Remove(viewer.username);
			}
			return;
		}
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		helper.Viewer = viewer;
		helper.message = formattedMessage;
		Ticker.IncidentHelperVariables.Enqueue(helper);
		Store_Logger.LogPurchase(viewer.username, twitchMessage.Message);
		component.LogIncident(incident);
	}

	public static bool CheckIfViewerIsInVariableCommandList(string username, bool separateChannel = false)
	{
		if (viewerNamesDoingVariableCommands.Contains(username))
		{
			TwitchWrapper.SendChatMessage("@" + username + " you must wait for the game to unpause to buy something else.");
			return true;
		}
		return false;
	}

	public static bool CheckIfViewerHasEnoughCoins(Viewer viewer, int finalPrice, bool separateChannel = false)
	{
		if (!ToolkitSettings.UnlimitedCoins && viewer.GetViewerCoins() < finalPrice)
		{
			TwitchWrapper.SendChatMessage(Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitNotEnoughCoins")), null, null, null, null, null, null, null, null, null, null, viewer: viewer.username, amount: finalPrice.ToString(), mod: null, newbalance: null, karma: null, first: viewer.GetViewerCoins().ToString()));
			return false;
		}
		return true;
	}

	public static bool CheckIfKarmaTypeIsMaxed(StoreIncident incident, string username, bool separateChannel = false)
	{
		bool maxed = CheckTimesKarmaTypeHasBeenUsedRecently(incident);
		if (maxed)
		{
			Store_Component component = Current.Game.GetComponent<Store_Component>();
			TwitchWrapper.SendChatMessage("@" + username + " " + GenText.CapitalizeFirst(((Def)incident).label) + " is maxed from karmatype, wait " + component.DaysTillIncidentIsPurchaseable(incident) + " days to purchase.");
		}
		return maxed;
	}

	public static bool CheckTimesKarmaTypeHasBeenUsedRecently(StoreIncident incident)
	{
		if (!ToolkitSettings.MaxEvents)
		{
			return false;
		}
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		return incident.karmaType switch
		{
			KarmaType.Bad => component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxBadEventsPerInterval, 
			KarmaType.Good => component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxGoodEventsPerInterval, 
			KarmaType.Neutral => component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxNeutralEventsPerInterval, 
			KarmaType.Doom => component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxBadEventsPerInterval, 
			_ => false, 
		};
	}

	public static bool CheckIfCarePackageIsOnCooldown(string username, bool separateChannel = false)
	{
		if (!ToolkitSettings.MaxEvents)
		{
			return false;
		}
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		StoreIncidentVariables incident = DefDatabase<StoreIncidentVariables>.GetNamed("Item", true);
		if (component.IncidentsInLogOf(incident.abbreviation) >= ToolkitSettings.MaxCarePackagesPerInterval)
		{
			float daysTill = component.DaysTillIncidentIsPurchaseable(incident);
			TwitchWrapper.SendChatMessage("@" + username + " care packages are on cooldown, wait " + daysTill + " day" + ((daysTill != 1f) ? "s" : "") + ".");
			return true;
		}
		return false;
	}

	public static bool CheckIfIncidentIsOnCooldown(StoreIncident incident, string username, bool separateChannel = false)
	{
		if (!ToolkitSettings.EventsHaveCooldowns)
		{
			return false;
		}
		Store_Component component = Current.Game.GetComponent<Store_Component>();
		bool maxed = component.IncidentsInLogOf(incident.abbreviation) >= incident.eventCap;
		if (maxed)
		{
			float days = component.DaysTillIncidentIsPurchaseable(incident);
			TwitchWrapper.SendChatMessage("@" + username + " " + GenText.CapitalizeFirst(((Def)incident).label) + " is maxed, wait " + days + " day" + ((days != 1f) ? "s" : "") + " to purchase.");
		}
		return maxed;
	}

	public static void QueuePlayerMessage(Viewer viewer, string message, int variables = 0)
	{
		string colorCode = Viewer.GetViewerColorCode(viewer.username);
		string userNameTag = "<color=#" + colorCode + ">" + viewer.username + "</color>";
		string[] command = message.Split(' ');
		string output = "\n\n";
		if (command.Length - 2 == variables)
		{
			output = output + "<i>from</i> " + userNameTag;
		}
		else
		{
			output = output + userNameTag + ":";
			for (int i = 2 + variables; i < command.Length; i++)
			{
				output = ((!(viewer.username.ToLower() == "hodlhodl") && !(viewer.username.ToLower() == "sirrandoo") && !(viewer.username.ToLower() == "saschahi")) ? ((!viewer.IsSub) ? ((!viewer.IsVIP) ? ((!viewer.mod) ? (output + " " + command[i]) : (output + " " + ModText(command[i]))) : (output + " " + VIPText(command[i]))) : (output + " " + SubText(command[i]))) : (output + " " + AdminText(command[i])));
			}
		}
		Helper.playerMessages.Add(output);
	}

	private static string AdminText(string input)
	{
		char[] chars = input.ToCharArray();
		StringBuilder output = new StringBuilder();
		char[] array = chars;
		foreach (char str in array)
		{
			output.Append($"<color=#{Helper.GetRandomColorCode()}>{str}</color>");
		}
		return output.ToString();
	}

	private static string SubText(string input)
	{
		return "<color=#D9BB25>" + input + "</color>";
	}

	private static string VIPText(string input)
	{
		return "<color=#5F49F2>" + input + "</color>";
	}

	private static string ModText(string input)
	{
		return "<color=#238C48>" + input + "</color>";
	}
}
