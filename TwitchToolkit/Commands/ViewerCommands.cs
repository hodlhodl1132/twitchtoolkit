using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using TwitchToolkit.Viewers;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Commands.ViewerCommands
{
    public class CheckBalance : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            Helper.Log("running balance command");

            Toolkit.client.SendMessage(
                $"@{message.Viewer.UsernameCap} " + Helper.ReplacePlaceholder("TwitchToolkitBalanceMessage".Translate(), 
                amount: message.Viewer.Coins.ToString(), karma: message.Viewer.Karma.ToString()), 
                CommandsHandler.SendToChatroom(message)
            );
        }
    }

    public class WhatIsKarma : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            Toolkit.client.SendMessage($"@{message.Viewer.UsernameCap} " + "TwitchToolkitWhatIsKarma".Translate() + $" { message.Viewer.Karma}%", CommandsHandler.SendToChatroom(message));
        }
    }

    public class PurchaseList : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            Toolkit.client.SendMessage($"@{message.User} " + "TwitchToolkitPurchaseList".Translate() + $" {ToolkitSettings.CustomPricingSheetLink}", CommandsHandler.SendToChatroom(message));
        }
    }

    public class GiftCoins : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            string[] command = message.Message.Split(' ');

            if (command.Count() < 3)
            {
                return;
            }

            string target = command[1].Replace("@", "");

            bool isNumeric = int.TryParse(command[2], out int amount);
            if (isNumeric && amount > 0)
            {

                Viewer giftee = Viewers.NewViewers.GetViewerByTypeAndUsername(target, message.Viewer.ViewerType);

                if (giftee == null)
                {
                    Toolkit.client.SendMessage($"@{message.Viewer.UsernameCap} viewer not found.");
                    return;
                }

                if (ToolkitSettings.KarmaReqsForGifting)
                {
                    if (giftee.Karma < ToolkitSettings.MinimumKarmaToRecieveGifts || message.Viewer.Karma < ToolkitSettings.MinimumKarmaToSendGifts)
                    {
                        return;
                    }
                }

                if (message.Viewer.Coins >= amount)
                {
                    message.Viewer.GiveCoins(amount);
                    giftee.GiveCoins(amount);
                    Toolkit.client.SendMessage($"@{giftee.UsernameCap} " + Helper.ReplacePlaceholder("TwitchToolkitGiftCoins".Translate(), amount: amount.ToString(), from: message.Viewer.UsernameCap), CommandsHandler.SendToChatroom(message));
                }
            }
        }
    }

    public class JoinQueue : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            GameComponentPawns pawnComponent = Current.Game.GetComponent<GameComponentPawns>();

            if (pawnComponent.HasUserBeenNamed(message.User) || pawnComponent.UserInViewerQueue(message.User))
            {
                return;
            }

            if (ToolkitSettings.ChargeViewersForQueue)
            {
                if (message.Viewer.Coins < ToolkitSettings.CostToJoinQueue)
                {
                    Toolkit.client.SendMessage($"@{message.User} you do not have enough coins to purchase a ticket, it costs {ToolkitSettings.CostToJoinQueue} and you have {message.Viewer.Username}.", CommandsHandler.SendToChatroom(message));
                    return;
                }

                message.Viewer.TakeCoins(ToolkitSettings.CostToJoinQueue);
            }

            pawnComponent.AddViewerToViewerQueue(message.User);
            Toolkit.client.SendMessage($"@{message.User} you have purchased a ticket and are in the queue!", CommandsHandler.SendToChatroom(message));
        }
    }

    public class ModInfo : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            Toolkit.client.SendMessage($"@{message.User} " + "TwitchToolkitModInfo".Translate() + " https://discord.gg/qrtg224 !", CommandsHandler.SendToChatroom(message));
        }
    }

    public class Buy : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            if (message.Message.Split(' ').Count() < 2) return;
            Purchase_Handler.ResolvePurchase(message.Viewer, message, CommandsHandler.SendToChatroom(message));
        }
    }

    public class ModSettings : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
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

            Toolkit.client.SendMessage(stats_message, CommandsHandler.SendToChatroom(message));
        }
    }

    public class Instructions : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            Command allCommandsCommand = DefDatabase<Command>.GetNamed("AvailableCommands");

            Toolkit.client.SendMessage($"@{message.User} the toolkit is a mod where you earn coins while you watch. Check out the bit.ly/toolkit-guide  or use !" + allCommandsCommand.command + " for a short list. " + ToolkitSettings.Channel.CapitalizeFirst() + " has a list of items/events to purchase at " + ToolkitSettings.CustomPricingSheetLink, CommandsHandler.SendToChatroom(message));
        }
    }

    public class AvailableCommands : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
        {
            List<Command> commands = DefDatabase<Command>.AllDefs.Where(s => !s.requiresAdmin && !s.requiresMod && s.enabled).ToList();

            string output = "@" + message.User + " viewer commands: ";


            for (int i = 0; i < commands.Count; i++)
            {
                output += "!" + commands[i].command;

                if (i < commands.Count - 1)
                {
                    output += ", ";
                }
            }

            Toolkit.client.SendMessage(output, CommandsHandler.SendToChatroom(message));
        }
    }

    public class InstalledMods : CommandDriver
    {
        public override void RunCommand(TwitchIRCMessage message)
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
                    Toolkit.client.SendMessage(modmsg, CommandsHandler.SendToChatroom(message));
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
