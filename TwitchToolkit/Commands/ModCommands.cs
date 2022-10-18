using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolkitCore;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.Commands.ModCommands
{
    public class RefreshViewers : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            if (string.IsNullOrEmpty(ToolkitSettings.Channel))
            {
                TwitchToolkitDev.WebRequest_BeginGetResponse.Main("https://tmi.twitch.tv/group/user/cerisevt/chatters", new Func<TwitchToolkitDev.RequestState, bool>(Viewers.SaveUsernamesFromJsonResponse));
            }
            else
            {
                TwitchToolkitDev.WebRequest_BeginGetResponse.Main("https://tmi.twitch.tv/group/user/" + ToolkitSettings.Channel.ToLower() + "/chatters", new Func<TwitchToolkitDev.RequestState, bool>(Viewers.SaveUsernamesFromJsonResponse));
            }

            TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} viewers have been refreshed.");
        }
    }

    public class KarmaRound : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            Viewers.AwardViewersCoins();

            TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} rewarding all active viewers coins.");
        }
    }

    public class GiveAllCoins : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            try
            {
                string[] command = twitchMessage.Message.Split(' ');

                if (command.Length < 2)
                {
                    return;
                }

                bool isNumeric = int.TryParse(command[1], out int amount);

                if (isNumeric)
                {
                    foreach (Viewer vwr in Viewers.All)
                    {
                        vwr.GiveViewerCoins(amount);
                    }

                    TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + Helper.ReplacePlaceholder("TwitchToolkitGiveAllCoins".Translate(), amount: amount.ToString()));
                }
            }
            catch (InvalidCastException e)
            {
                Helper.Log("Give All Coins Syntax Error " + e.Message);
            }
        }
    }

    public class GiveCoins : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            try
            {
                string[] command = twitchMessage.Message.Split(' ');

                if (command.Length < 3)
                {
                    return;
                }

                string receiver = command[1].Replace("@", "");

                if (twitchMessage.Username.ToLower() != ToolkitSettings.Channel.ToLower() && receiver.ToLower() == twitchMessage.Username.ToLower())
                {
                    TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + "TwitchToolkitModCannotGiveCoins".Translate());
                    return;
                }

                int amount;
                bool isNumeric = int.TryParse(command[2], out amount);
                if (isNumeric)
                {
                    Viewer giftee = Viewers.GetViewer(receiver);

                    Helper.Log($"Giving viewer {giftee.username} {amount} coins");
                    giftee.GiveViewerCoins(amount);
                    TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + Helper.ReplacePlaceholder("TwitchToolkitGivingCoins".Translate(), viewer: giftee.username, amount: amount.ToString(), newbalance: giftee.coins.ToString()));
                    Store_Logger.LogGiveCoins(twitchMessage.Username, giftee.username, amount);
                }
            }
            catch (InvalidCastException e)
            {
                Helper.Log("Invalid Give Viewer Coins Command " + e.Message);
            }
        }
    }

    public class CheckUser : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            try
            {
                string[] command = twitchMessage.Message.Split(' ');

                if (command.Length < 2)
                {
                    return;
                }

                string target = command[1].Replace("@", "");

                Viewer targeted = Viewers.GetViewer(target);
                TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + Helper.ReplacePlaceholder("TwitchToolkitCheckUser".Translate(), viewer: targeted.username, amount: targeted.coins.ToString(), karma: targeted.GetViewerKarma().ToString()));

            }
            catch (InvalidCastException e)
            {
                Helper.Log("Invalid Check User Command " + e.Message);
            }
        }
    }

    public class SetKarma : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            try
            {
                string[] command = twitchMessage.Message.Split(' ');

                if (command.Length < 3)
                {
                    return;
                }

                string target = command[1].Replace("@", "");
                int amount;
                bool isNumeric = int.TryParse(command[2], out amount);
                if (isNumeric)
                {
                    Viewer targeted = Viewers.GetViewer(target);
                    targeted.SetViewerKarma(amount);
                    TwitchWrapper.SendChatMessage($"@{twitchMessage.Username}" + Helper.ReplacePlaceholder("TwitchToolkitSetKarma".Translate(), viewer: targeted.username, karma: amount.ToString()));
                }
            }
            catch (InvalidCastException e)
            {
                Helper.Log("Invalid Check User Command " + e.Message);
            }
        }
    }

    public class ToggleCoins : CommandDriver
    {
        public override void RunCommand(ITwitchMessage twitchMessage)
        {
            if (ToolkitSettings.EarningCoins)
            {
                ToolkitSettings.EarningCoins = false;
                TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOff".Translate());
            }
            else
            {
                ToolkitSettings.EarningCoins = true;
                TwitchWrapper.SendChatMessage($"@{twitchMessage.Username} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOn".Translate());
            }
        }
    }
}
