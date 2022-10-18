using System;
using System.Collections.Generic;
using System.Linq;
using ToolkitCore;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands
{
    public class CheckBalance : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            Viewer viewer = Viewers.GetViewer(twitchMessage.Username);

            TwitchWrapper.SendChatMessage($"@{viewer.username} " + Helper.ReplacePlaceholder("TwitchToolkitBalanceMessage".Translate(), amount: viewer.GetViewerCoins().ToString(), karma: viewer.GetViewerKarma().ToString()));
        }
    }

    public class WhatIsKarma : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            Viewer viewer = Viewers.GetViewer(twitchMessage.Username);

            TwitchWrapper.SendChatMessage($"@{viewer.username} " + "TwitchToolkitWhatIsKarma".Translate() + $" { viewer.GetViewerKarma()}%");
        }
    }

    public class PurchaseList : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + "TwitchToolkitPurchaseList".Translate() + $" {ToolkitSettings.CustomPricingSheetLink}");
        }
    }

    public class GiftCoins : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            Viewer viewer = Viewers.GetViewer(twitchMessage.Username);

            string[] command = twitchMessage.Message.Split(' ');

            if (command.Count() < 3)
            {
                Log.Message("command not long enough");
                return;
            }

            string target = command[1].Replace("@", "");

            bool isNumeric = int.TryParse(command[2], out int amount);
            if (isNumeric && amount > 0)
            {
                Viewer giftee = Viewers.GetViewer(target);

                if (ToolkitSettings.KarmaReqsForGifting)
                {
                    if (giftee.GetViewerKarma() < ToolkitSettings.MinimumKarmaToRecieveGifts || viewer.GetViewerKarma() < ToolkitSettings.MinimumKarmaToSendGifts)
                    {
                        return;
                    }
                }

                if (viewer.GetViewerCoins() >= amount)
                {
                    viewer.TakeViewerCoins(amount);
                    giftee.GiveViewerCoins(amount);
                    TwitchWrapper.SendChatMessage($"@{giftee.username} " + Helper.ReplacePlaceholder("TwitchToolkitGiftCoins".Translate(), amount: amount.ToString(), from: viewer.username));
                    Store_Logger.LogGiftCoins(viewer.username, giftee.username, amount);
                }
            }
        }
    }

    public class JoinQueue : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            TwitchWrapper.SendChatMessage($"Pineapple");
            Viewer viewer = Viewers.GetViewer(twitchMessage.Username);
            GameComponentPawns pawnComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (pawnComponent.HasUserBeenNamed(twitchMessage.Username) || pawnComponent.UserInViewerQueue(twitchMessage.Username))
            {
                TwitchWrapper.SendChatMessage($"{pawnComponent.HasUserBeenNamed(twitchMessage.Username)},{pawnComponent.UserInViewerQueue(twitchMessage.Username)}");
                return;
            }

            if (ToolkitSettings.ChargeViewersForQueue)
            {
                if (viewer.GetViewerCoins() < ToolkitSettings.CostToJoinQueue)
                {
                    TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} you do not have enough coins to purchase a ticket, it costs {ToolkitSettings.CostToJoinQueue} and you have {viewer.GetViewerCoins()}.");
                    return;
                }

                viewer.TakeViewerCoins(ToolkitSettings.CostToJoinQueue);
            }

            pawnComponent.AddViewerToViewerQueue(twitchMessage.Username);
            TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} you have purchased a ticket and are in the queue!");
        }
    }

    public class ModInfo : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + "TwitchToolkitModInfo".Translate() + " https://discord.gg/qrtg224 !");
        }
    }

    public class Buy : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            Viewer viewer = Viewers.GetViewer(twitchMessage.Username);

            if (twitchMessage.Message.Split(' ').Count() < 2) return;
            Purchase_Handler.ResolvePurchase(viewer, twitchMessage);
        }
    }

    public class ModSettings : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            Command buyCommand = DefDatabase<Command>.GetNamed("Buy");

            string minutess = ToolkitSettings.CoinInterval > 1 ? "s" : "";
            string storeon = buyCommand.enabled ? "TwitchToolkitOn".Translate() : "TwitchToolkitOff".Translate();
            string earningcoins = ToolkitSettings.EarningCoins ? "TwitchToolkitOn".Translate() : "TwitchToolkitOff".Translate();

            string stats_message = Helper.ReplacePlaceholder("TwitchToolkitModSettings".Translate(),
                amount: ToolkitSettings.CoinAmount.ToString(),
                first: ToolkitSettings.CoinInterval.ToString(),
                second: storeon,
                third: earningcoins,
                karma: ToolkitSettings.KarmaCap.ToString()
                );

            TwitchWrapper.SendChatMessage(stats_message);
        }
    }

    public class Instructions : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            Command allCommandsCommand = DefDatabase<Command>.GetNamed("AvailableCommands");

            TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} the toolkit is a mod where you earn coins while you watch. Check out the bit.ly/toolkit-guide  or use !" + allCommandsCommand.command + " for a short list. " + ToolkitSettings.Channel.CapitalizeFirst() + " has a list of items/events to purchase at " + ToolkitSettings.CustomPricingSheetLink);
        }
    }

    public class AvailableCommands : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            List<Command> commands = DefDatabase<Command>.AllDefs.Where(s => !s.requiresAdmin && !s.requiresMod && s.enabled).ToList();

            string output = "@" + twitchMessage.Username + " viewer commands: ";


            for (int i = 0; i < commands.Count; i++)
            {
                output += "!" + commands[i].command;

                if (i < commands.Count - 1)
                {
                    output += ", ";
                }
            }

            TwitchWrapper.SendChatMessage(output);
        }
    }

    public class InstalledMods : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            if ((DateTime.Now - Cooldowns.modsCommandCooldown).TotalSeconds <= 15)
            {
                return;
            }

            Cooldowns.modsCommandCooldown = DateTime.Now;
            string modmsg = "Version: " + Toolkit.Mod.Version + ", Mods: ";
            string[] mods = LoadedModManager.RunningMods.Select((m) => m.Name).ToArray();

            for (int i = 0; i < mods.Length; i++)
            {
                modmsg += mods[i] + ", ";

                if (i == (mods.Length - 1) || modmsg.Length > 256)
                {
                    modmsg = modmsg.Substring(0, modmsg.Length - 2);
                    TwitchWrapper.SendChatMessage(modmsg);
                    modmsg = "";
                }
            }
            return;
        }
    }

    public static class Cooldowns
    {
        public static DateTime modsCommandCooldown = DateTime.Now;
    }
}
