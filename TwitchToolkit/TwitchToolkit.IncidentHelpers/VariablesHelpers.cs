using System;
using ToolkitCore;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers;

public static class VariablesHelpers
{
	public static void ViewerDidWrongSyntax(string username, string syntax, bool separateChannel = false)
	{
		TwitchWrapper.SendChatMessage("@" + username + " syntax is " + syntax);
	}

	public static bool PointsWagerIsValid(string wager, Viewer viewer,  int pointsWager,  StoreIncidentVariables incident, bool separateChannel = false, int quantity = 1, int maxPrice = 25000)
	{
		//IL_00ed: Unknown result type (might be due to invalid IL or missing erences)
		checked
		{
			try
			{
				if (!int.TryParse(wager, out pointsWager))
				{
					ViewerDidWrongSyntax(viewer.username, incident.syntax);
					return false;
				}
				pointsWager *= quantity;
			}
			catch (OverflowException e)
			{
				Helper.Log(e.Message);
				TwitchWrapper.SendChatMessage("@" + viewer.username + " points wager is invalid.");
				return false;
			}
			if (incident.maxWager > 0 && incident.maxWager > incident.cost && pointsWager > incident.maxWager)
			{
				TwitchWrapper.SendChatMessage($"@{viewer.username} you cannot spend more than {incident.maxWager} coins on {GenText.CapitalizeFirst(incident.abbreviation)}");
				return false;
			}
			if (pointsWager < incident.cost || pointsWager < incident.minPointsToFire)
			{
				TwitchWrapper.SendChatMessage(Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchToolkitMinPurchaseNotMet")), null, null, null, null, null, null, null, null, null, null, viewer: viewer.username, amount: pointsWager.ToString(), mod: null, newbalance: null, karma: null, first: incident.cost.ToString()));
				return false;
			}
			if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, pointsWager))
			{
				return false;
			}
			return true;
		}
	}

	public static void SendPurchaseMessage(string message, bool separateChannel = false)
	{
		if (ToolkitSettings.PurchaseConfirmations)
		{
			TwitchWrapper.SendChatMessage(message);
		}
	}
}
