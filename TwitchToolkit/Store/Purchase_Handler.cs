using ToolkitCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchLib.Client.Models;
using TwitchToolkit.Incidents;
using Verse;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models.Interfaces;

namespace TwitchToolkit.Store
{
    [StaticConstructorOnStartup]
    public static class Purchase_Handler
    {
        static Purchase_Handler()
        {
            allStoreIncidentsSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList();
            allStoreIncidentsVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList();

            Helper.Log("trying to load vars after def database loaded");

            Toolkit.Mod.GetSettings<ToolkitSettings>();

            viewerNamesDoingVariableCommands = new List<string>();
        }

        public static void ResolvePurchase(Viewer viewer, ITwitchMessage twitchMessage, bool separateChannel = false)
        {
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

            StoreIncidentVariables incidentVariables = allStoreIncidentsVariables.Find(s => productKey.ToLower() == s.abbreviation);

            if (incidentVariables != null)
            {
                ResolvePurchaseVariables(viewer, twitchMessage, incidentVariables, formattedMessage);
                return;
            }

            Item item = StoreInventory.items.Find(s => s.abr == productKey);

            Helper.Log($"abr: {productKey} ");

            if (item != null)
            {
                List<String> commandSplit = twitchMessage.Message.Split(' ').ToList();
                commandSplit.Insert(1, "item");

                if (commandSplit.Count < 4)
                {
                    commandSplit.Add("1");
                }

                if (!int.TryParse(commandSplit[3], out int quantity))
                {
                    commandSplit.Insert(3, "1");
                }

                formattedMessage = string.Join(" ", commandSplit.ToArray());

                ResolvePurchaseVariables(viewer, twitchMessage, StoreIncidentDefOf.Item, formattedMessage);
            }

            return;
        }

        public static void ResolvePurchaseSimple(Viewer viewer, ITwitchMessage twitchMessage, StoreIncidentSimple incident, string formattedMessage, bool separateChannel = false)
        {
            int cost = incident.cost;

            if (cost < 1) return;

            if (CheckIfViewerIsInVariableCommandList(viewer.username)) return;

            if (!CheckIfViewerHasEnoughCoins(viewer, cost)) return;

            if (CheckIfKarmaTypeIsMaxed(incident, viewer.username)) return;

            if (CheckIfIncidentIsOnCooldown(incident, viewer.username)) return;

            IncidentHelper helper = StoreIncidentMaker.MakeIncident(incident);
            
            if (helper == null)
            {
                Helper.Log("Missing helper for incident " + incident.defName);
                return;
            }

            if (!helper.IsPossible())
            {
                TwitchWrapper.SendChatMessage($"@{viewer.username} " + "TwitchToolkitEventNotPossible".Translate());
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
                TwitchWrapper.SendChatMessage(
                    Helper.ReplacePlaceholder(
                        "TwitchToolkitEventPurchaseConfirm".Translate(), 
                        first: incident.label.CapitalizeFirst(), 
                        viewer: viewer.username
                        ));
            }
        }

        public static void ResolvePurchaseVariables(Viewer viewer, ITwitchMessage twitchMessage, StoreIncidentVariables incident, string formattedMessage, bool separateChannel = false)
        {
            int cost = incident.cost;

            if (cost < 1 && incident.defName != "Item") return;

            if (CheckIfViewerIsInVariableCommandList(viewer.username)) return;

            if (!CheckIfViewerHasEnoughCoins(viewer, cost)) return;

            if (incident != DefDatabase<StoreIncidentVariables>.GetNamed("Item"))
            {
                if (CheckIfKarmaTypeIsMaxed(incident, viewer.username)) return;
            }
            else
            {
                if (CheckIfCarePackageIsOnCooldown(viewer.username)) return;
            }

            if (CheckIfIncidentIsOnCooldown(incident, viewer.username)) return;

            viewerNamesDoingVariableCommands.Add(viewer.username);

            IncidentHelperVariables helper = StoreIncidentMaker.MakeIncidentVariables(incident);
            
            if (helper == null)
            {
                Helper.Log("Missing helper for incident " + incident.defName);
                return;
            }

            if (!helper.IsPossible(formattedMessage, viewer))
            {
                if (viewerNamesDoingVariableCommands.Contains(viewer.username))
                    viewerNamesDoingVariableCommands.Remove(viewer.username);
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
                TwitchWrapper.SendChatMessage($"@{username} you must wait for the game to unpause to buy something else.");
                return true;
            }
            return false;
        }

        public static bool CheckIfViewerHasEnoughCoins(Viewer viewer, int finalPrice, bool separateChannel = false)
        {
            if (!ToolkitSettings.UnlimitedCoins && viewer.GetViewerCoins() < finalPrice)
            {
                TwitchWrapper.SendChatMessage(
                    Helper.ReplacePlaceholder(
                        "TwitchToolkitNotEnoughCoins".Translate(),
                        viewer: viewer.username,
                        amount: finalPrice.ToString(),
                        first: viewer.GetViewerCoins().ToString()
                    ));
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
                    TwitchWrapper.SendChatMessage($"@{username} {incident.label.CapitalizeFirst()} is maxed from karmatype, wait " + component.DaysTillIncidentIsPurchaseable(incident) + " days to purchase.");
                }

                return maxed;
        }

        public static bool CheckTimesKarmaTypeHasBeenUsedRecently(StoreIncident incident)
        {
            // if they have max event setting off always return false
            if (!ToolkitSettings.MaxEvents)
            {
                return false;
            }

            Store_Component component = Current.Game.GetComponent<Store_Component>();

            switch (incident.karmaType)
            {
                case KarmaType.Bad:
                    return component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxBadEventsPerInterval;
                case KarmaType.Good:
                    return component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxGoodEventsPerInterval;
                case KarmaType.Neutral:
                    return component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxNeutralEventsPerInterval;
                case KarmaType.Doom:
                    return component.KarmaTypesInLogOf(incident.karmaType) >= ToolkitSettings.MaxBadEventsPerInterval;
            }

            return false;
        }

        public static bool CheckIfCarePackageIsOnCooldown(string username, bool separateChannel = false)
        {
            if (!ToolkitSettings.MaxEvents)
            {
                return false;
            }

            Store_Component component = Current.Game.GetComponent<Store_Component>();
            StoreIncidentVariables incident = DefDatabase<StoreIncidentVariables>.GetNamed("Item");

            if (component.IncidentsInLogOf(incident.abbreviation) >= ToolkitSettings.MaxCarePackagesPerInterval)
            {
                float daysTill = component.DaysTillIncidentIsPurchaseable(incident);
                TwitchWrapper.SendChatMessage($"@{username} care packages are on cooldown, wait " + daysTill + $" day{(daysTill != 1 ? "s" : "")}.");
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
                TwitchWrapper.SendChatMessage($"@{username} {incident.label.CapitalizeFirst()} is maxed, wait " + days + $" day{(days != 1 ? "s" : "")} to purchase.");
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
                    else if (viewer.IsSub)
                    {
                        output += " " + SubText(command[i]);
                    }
                    else if (viewer.IsVIP)
                    {
                        output += " " + VIPText(command[i]);
                    }
                    else if (viewer.mod)
                    {
                        output += " " + ModText(command[i]);
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

            foreach (char str in chars)
            {
                output.Append($"<color=#{Helper.GetRandomColorCode()}>{str}</color>");
            }

            return output.ToString();
        }

        static string SubText(string input)
        {
            return "<color=#D9BB25>" + input + "</color>";
        }

        static string VIPText(string input)
        {
            return "<color=#5F49F2>" + input + "</color>";
        }

        static string ModText(string input)
        {
            return "<color=#238C48>" + input + "</color>";
        }

        public static List<StoreIncidentSimple> allStoreIncidentsSimple;
        public static List<StoreIncidentVariables> allStoreIncidentsVariables;

        public static List<string> viewerNamesDoingVariableCommands;
    }
}
