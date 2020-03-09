using rim_twitch;
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
            MessageQueue.messageQueue.Enqueue($"@{username} syntax is {syntax}");
        }

        public static bool PointsWagerIsValid(string wager, Viewer viewer, ref int pointsWager, ref StoreIncidentVariables incident, bool separateChannel = false, int quantity = 1, int maxPrice = 25000)
        {
            try
            {
                if (! int.TryParse( wager, out checked(pointsWager) ) )
                {
                    ViewerDidWrongSyntax(viewer.username, incident.syntax);
                    return false;
                }
                pointsWager = checked(pointsWager * quantity);
            }
            catch (OverflowException e)
            {
                Helper.Log(e.Message);
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} points wager is invalid.");
                return false;
            }

            if (incident.maxWager > 0 && incident.maxWager > incident.cost && pointsWager > incident.maxWager)
            {
                MessageQueue.messageQueue.Enqueue($"@{viewer.username} you cannot spend more than {incident.maxWager} coins on {incident.abbreviation.CapitalizeFirst()}");
                return false;
            }

            //|| (incident.minPointsToFire > 0 && pointsWager < incident.minPointsToFire)
            if (pointsWager < incident.cost || pointsWager < incident.minPointsToFire)
            {
                MessageQueue.messageQueue.Enqueue(Helper.ReplacePlaceholder(
                    "TwitchToolkitMinPurchaseNotMet".Translate(), 
                    viewer: viewer.username, 
                    amount: pointsWager.ToString(), 
                    first: incident.cost.ToString()
                ));
                return false;
            }

            if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, pointsWager)) return false;

            return true;
        }

        public static void SendPurchaseMessage(string message, bool separateChannel = false)
        {
            if (ToolkitSettings.PurchaseConfirmations)
            {
                MessageQueue.messageQueue.Enqueue(message);
            }
        }
    }
}
