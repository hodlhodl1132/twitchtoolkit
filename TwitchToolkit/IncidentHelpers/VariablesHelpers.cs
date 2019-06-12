using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers
{
    public static class VariablesHelpers
    {
        public static void ViewerDidWrongSyntax(string username, string syntax, bool separateChannel = false)
        {
            Toolkit.client.SendMessage($"@{username} syntax is {syntax}", separateChannel);
        }

        public static bool PointsWagerIsValid(string wager, Viewer viewer, ref int pointsWager, ref StoreIncidentVariables incident, bool separateChannel = false, int quantity = 1, int maxPrice = 25000)
        {
            try
            {
                if (! int.TryParse( wager, out checked(pointsWager) ) )
                {
                    ViewerDidWrongSyntax(viewer.username, incident.syntax, separateChannel);
                    return false;
                }
                pointsWager = checked(pointsWager * quantity);
            }
            catch (OverflowException e)
            {
                Helper.Log(e.Message);
                Toolkit.client.SendMessage($"@{viewer.username} points wager is invalid.", separateChannel);
                return false;
            }

            if (incident.maxWager > 0 && incident.maxWager > incident.cost && pointsWager > incident.maxWager)
            {
                Toolkit.client.SendMessage($"@{viewer.username} you cannot spend more than {incident.maxWager} coins on {incident.abbreviation.CapitalizeFirst()}", separateChannel);
                return false;
            }

            //|| (incident.minPointsToFire > 0 && pointsWager < incident.minPointsToFire)
            if (pointsWager < incident.cost || pointsWager < incident.minPointsToFire)
            {
                Toolkit.client.SendMessage(Helper.ReplacePlaceholder(
                    "TwitchToolkitMinPurchaseNotMet".Translate(), 
                    viewer: viewer.username, 
                    amount: pointsWager.ToString(), 
                    first: incident.cost.ToString()
                ), separateChannel);
                return false;
            }

            if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, pointsWager, separateChannel)) return false;

            return true;
        }

        public static void SendPurchaseMessage(string message, bool separateChannel = false)
        {
            if (ToolkitSettings.PurchaseConfirmations)
            {
                Toolkit.client.SendMessage(message, separateChannel);
            }
        }
    }
}
