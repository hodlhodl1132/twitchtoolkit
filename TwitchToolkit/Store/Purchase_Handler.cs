using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.IRC;
using Verse;

namespace TwitchToolkit.Store
{
    [StaticConstructorOnStartup]
    public static class Purchase_Handler
    {
        static Purchase_Handler()
        {
            allStoreIncidentsSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList();
            allStoreIncidentsVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList();

            Log.Warning("trying to load vars after def database loaded");

            Toolkit.Mod.GetSettings<ToolkitSettings>();

            viewerNamesDoingVariableCommands = new List<string>();
        }

        public static void ResolvePurchase(Viewer viewer, IRCMessage message, bool separateChannel = false)
        {
            List<string> command = message.Message.Split(' ').ToList();

            if (command.Count < 2)
            {
                return;
            }

            if (command[0] == "!gambleskill")
            {
                command[0] = "gambleskill";
                command.Insert(0, "!buy");
            }

            string productKey = command[1].ToLower();

            Helper.Log(productKey);
            message.Message = string.Join(" ", command.ToArray());

            StoreIncidentSimple incident = allStoreIncidentsSimple.Find(s => productKey.ToLower() == s.abbreviation);
            
            if (incident != null)
            {
                ResolvePurchaseSimple(viewer, message, incident, separateChannel);
                return;
            }

            StoreIncidentVariables incidentVariables = allStoreIncidentsVariables.Find(s => productKey.ToLower() == s.abbreviation);

            if (incidentVariables != null)
            {
                ResolvePurchaseVariables(viewer, message, incidentVariables, separateChannel);
                return;
            }

            Item item = StoreInventory.items.Find(s => s.abr == productKey);

            Log.Warning($"abr: {productKey} ");

            if (item != null)
            {
                List<String> commandSplit = message.Message.Split(' ').ToList();
                commandSplit.Insert(1, "item");

                if (commandSplit.Count < 4)
                {
                    commandSplit.Add("1");
                }

                if (!int.TryParse(commandSplit[3], out int quantity))
                {
                    commandSplit.Insert(3, "1");
                }

                message.Message = string.Join(" ", commandSplit.ToArray());

                ResolvePurchaseVariables(viewer, message, StoreIncidentDefOf.Item, separateChannel);
            }

            return;
        }

        public static void ResolvePurchaseSimple(Viewer viewer, IRCMessage message, StoreIncidentSimple incident, bool separateChannel = false)
        {
            int cost = incident.cost;

            if (cost < 1) return;

            if (CheckIfViewerIsInVariableCommandList(viewer.username, separateChannel)) return;

            if (!CheckIfViewerHasEnoughCoins(viewer, cost, separateChannel)) return;

            if (CheckIfKarmaTypeIsMaxed(incident.karmaType, viewer.username, separateChannel)) return;

            if (CheckIfIncidentIsOnCooldown(incident, viewer.username, separateChannel)) return;

            IncidentHelper helper = StoreIncidentMaker.MakeIncident(incident);
            
            if (helper == null)
            {
                Log.Warning("Missing helper for incident " + incident.defName);
                return;
            }

            if (!helper.IsPossible())
            {
                Toolkit.client.SendMessage($"@{viewer.username} " + "TwitchToolkitEventNotPossible".Translate(), separateChannel);
                return;
            }

            if (!ToolkitSettings.UnlimitedCoins)
            {
                viewer.TakeViewerCoins(cost);
            }

            Store_Component component = Current.Game.GetComponent<Store_Component>();

            QueuePlayerMessage(viewer, message.Message);
            Ticker.IncidentHelpers.Enqueue(helper);
            Store_Logger.LogPurchase(viewer.username, message.Message);
            component.LogIncident(incident);
            viewer.CalculateNewKarma(incident.karmaType, cost);    

            if (ToolkitSettings.PurchaseConfirmations)
            {
                Toolkit.client.SendMessage(
                    Helper.ReplacePlaceholder(
                        "TwitchToolkitEventPurchaseConfirm".Translate(), 
                        first: incident.label.CapitalizeFirst(), 
                        viewer: viewer.username
                        ),
                        separateChannel
                    );
            }
        }

        public static void ResolvePurchaseVariables(Viewer viewer, IRCMessage message, StoreIncidentVariables incident, bool separateChannel = false)
        {
            int cost = incident.cost;

            if (cost < 1 && incident.defName != "Item") return;

            if (CheckIfViewerIsInVariableCommandList(viewer.username, separateChannel)) return;

            if (!CheckIfViewerHasEnoughCoins(viewer, cost, separateChannel)) return;

            if (CheckIfKarmaTypeIsMaxed(incident.karmaType, viewer.username, separateChannel)) return;

            if (CheckIfIncidentIsOnCooldown(incident, viewer.username, separateChannel)) return;

            viewerNamesDoingVariableCommands.Add(viewer.username);

            IncidentHelperVariables helper = StoreIncidentMaker.MakeIncidentVariables(incident);
            
            if (helper == null)
            {
                Log.Warning("Missing helper for incident " + incident.defName);
                return;
            }

            if (!helper.IsPossible(message.Message, viewer, separateChannel))
            {
                if (viewerNamesDoingVariableCommands.Contains(viewer.username))
                    viewerNamesDoingVariableCommands.Remove(viewer.username);
                return;
            }

            Store_Component component = Current.Game.GetComponent<Store_Component>();

            QueuePlayerMessage(viewer, message.Message, incident.variables);

            Ticker.IncidentHelperVariables.Enqueue(helper);
            Store_Logger.LogPurchase(viewer.username, message.Message);
            component.LogIncident(incident);
        }

        public static bool CheckIfViewerIsInVariableCommandList(string username, bool separateChannel = false)
        {
            if (viewerNamesDoingVariableCommands.Contains(username))
            {
                Toolkit.client.SendMessage($"@{username} you must wait for the game to unpause to buy something else.", separateChannel);
                return true;
            }
            return false;
        }

        public static bool CheckIfViewerHasEnoughCoins(Viewer viewer, int finalPrice, bool separateChannel = false)
        {
            if (!ToolkitSettings.UnlimitedCoins && viewer.GetViewerCoins() < finalPrice)
            {
                Toolkit.client.SendMessage(
                    Helper.ReplacePlaceholder(
                        "TwitchToolkitNotEnoughCoins".Translate(),
                        viewer: viewer.username,
                        amount: finalPrice.ToString(),
                        first: viewer.GetViewerCoins().ToString()
                    ),
                    separateChannel
                );
                return false;
            }
            return true;
        }

        public static bool CheckIfKarmaTypeIsMaxed(KarmaType karmaType, string username, bool separateChannel = false)
        {
                bool maxed = CheckTimesKarmaTypeHasBeenUsedRecently(karmaType);        

                if (maxed)
                {
                    Log.Message("KarmaType " + karmaType + " event cap has been reached");
                    Toolkit.client.SendMessage($"@{username} " + "TwitchToolkitMaxEvents".Translate(), separateChannel);
                }

                return maxed;
        }

        public static bool CheckTimesKarmaTypeHasBeenUsedRecently(KarmaType karmaType)
        {
            // if they have max event setting off always return false
            if (!ToolkitSettings.MaxEvents)
            {
                return false;
            }

            Store_Component component = Current.Game.GetComponent<Store_Component>();

            switch (karmaType)
            {
                case KarmaType.Bad:
                    return component.KarmaTypesInLogOf(karmaType) >= ToolkitSettings.MaxBadEventsPerInterval;
                case KarmaType.Good:
                    return component.KarmaTypesInLogOf(karmaType) >= ToolkitSettings.MaxGoodEventsPerInterval;
                case KarmaType.Neutral:
                    return component.KarmaTypesInLogOf(karmaType) >= ToolkitSettings.MaxNeutralEventsPerInterval;
                case KarmaType.Doom:
                    return component.KarmaTypesInLogOf(karmaType) >= ToolkitSettings.MaxBadEventsPerInterval;
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
                Log.Message("StoreIncident max per day reached for " + incident.label);
                Toolkit.client.SendMessage($"@{username} " + "TwitchToolkitEventOnCooldown".Translate(), separateChannel);
            }

            return maxed;
        }

        public static void QueuePlayerMessage(Viewer viewer, string message, int variables = 0)
        {
            string colorCode = Viewer.GetViewerColorCode(viewer.username);
            string userNameTag = $"<color=#{colorCode}>{viewer.username}</color>";
            string[] command = message.Split(' ');
            string output = "\n\n";

            if (command.Length  - 2 == variables)
            {
                output += "<i>from</i> " + userNameTag;
            }
            else
            {
                output += userNameTag + ":";

                for (int i = 2 + variables; i < command.Length; i++)
                {
                    if (viewer.username.ToLower() == "hodlhodl")
                    {
                        output += " " + AdminText(command[i]);
                    }
                    else
                    {
                        output += " " + command[i];
                    }
                }
            }

            Helper.playerMessages.Add(output);
        }

        static string AdminText(string input)
        {
            char[] chars = input.ToCharArray();
            StringBuilder output = new StringBuilder();
            output.Append("<size=24>");

            foreach (char str in chars)
            {
                output.Append($"<color=#{Helper.GetRandomColorCode()}>{str}</color>");
            }

            output.Append("</size>");

            return output.ToString();
        }

        public static List<StoreIncidentSimple> allStoreIncidentsSimple;
        public static List<StoreIncidentVariables> allStoreIncidentsVariables;

        public static List<string> viewerNamesDoingVariableCommands;
    }
}
