using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.IRC;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Store
{
    public static class Commands
    {
        static int resetWarning;
        static TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();

        public static void CheckCommand(IRCMessage msg)
        {

            if (msg == null)
            {
                return;
            }

            if (msg.Message == null)
            {
                return;
            }

            string message = msg.Message;
            string user = msg.User;
            if (message.Split(' ')[0] == "/w")
            {
                List<string> messagewhisper = message.Split(' ').ToList();
                messagewhisper.RemoveAt(0);
                message = string.Join(" ", messagewhisper.ToArray());
                Helper.Log(message);
            }

            Viewer viewer = Viewers.GetViewer(user);
            viewer.last_seen = DateTime.Now;

            if (viewer.IsBanned)
            {
                return;
            }
            
            //admin commands
            if (user.ToLower() == ToolkitSettings.Channel.ToLower())
            {
                if (message.StartsWith("!resetviewers"))
                {
                    if (ToolkitSettings.SyncStreamLabs)
                    {
                        WarningWindow window = new WarningWindow
                        {
                            warning = "You must reset viewers in Streamlabs chatbot and then restart the game."
                        };
                        Find.WindowStack.Add(window);
                    }
                    else if (resetWarning == 0)
                    {
                        Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningOne".Translate(), SendToChatroom(msg));
                        resetWarning = 1;
                    }
                    else if (resetWarning == 1)
                    {
                        Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningTwo".Translate(), SendToChatroom(msg));
                        resetWarning = 2;
                    }
                    else if (resetWarning == 2)
                    {
                        Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningThree".Translate(), SendToChatroom(msg));
                        Viewers.ResetViewers();
                        resetWarning = 0;
                    }
                }

                if (message.StartsWith("!addtoolkitmod"))
                {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }
                        
                        string mod = command[1].Replace("@", "").ToLower();

                        if (Viewer.IsModerator(mod))
                        {
                            Toolkit.client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitAlreadyMod".Translate(), mod: mod), SendToChatroom(msg));
                            return;
                        }

                        Viewer modviewer = Viewers.GetViewer(mod);

                        modviewer.SetAsModerator();
                        Toolkit.client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitAddedMod".Translate(), mod: mod), SendToChatroom(msg));
                }

                if (message.StartsWith("!removetoolkitmod"))
                {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }
                        
                        string mod = command[1].Replace("@", "").ToLower();

                        if (!Viewer.IsModerator(mod))
                        {
                            return;
                        }

                        Viewer modviewer = Viewers.GetViewer(mod);

                        modviewer.RemoveAsModerator();
                        Toolkit.client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitRemovedMod".Translate(), mod: mod), SendToChatroom(msg));
                }
            }

            //moderator commands
            if (user.ToLower() == ToolkitSettings.Channel.ToLower() || Viewer.IsModerator(user))
            {
                if (message.StartsWith("!refreshviewers"))
                {
                    TwitchToolkitDev.WebRequest_BeginGetResponse.Main("https://tmi.twitch.tv/group/user/" + ToolkitSettings.Channel.ToLower() + "/chatters", new Func<TwitchToolkitDev.RequestState, bool>(Viewers.SaveUsernamesFromJsonResponse));
                }

                if (message.StartsWith("!karmaround"))
                {
                    Viewers.AwardViewersCoins();
                }

                if (message.StartsWith("!giveallcoins"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }

                        bool isNumeric = int.TryParse(command[1], out int amount);

                        if (isNumeric)
                        {
                            foreach(Viewer vwr in Viewers.All)
                            {
                                vwr.GiveViewerCoins(amount);
                            }
                            Toolkit.client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitGiveAllCoins".Translate(), amount: amount.ToString()), SendToChatroom(msg));
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Give All Coins Syntax Error " + e.Message);
                    }
                }

                if (message.StartsWith("!givecoins"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

                        if (command.Length < 3)
                        {
                            return;
                        }
                        
                        string receiver = command[1].Replace("@", "");

                        if (user.ToLower() != ToolkitSettings.Channel.ToLower() && receiver.ToLower() == user.ToLower())
                        {
                            Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitModCannotGiveCoins".Translate());
                            return;
                        }

                        int amount;
                        bool isNumeric = int.TryParse(command[2], out amount);
                        if (isNumeric)
                        {
                            Viewer giftee = Viewers.GetViewer(receiver);

                            Helper.Log($"Giving viewer {giftee.username} {amount} coins");
                            giftee.GiveViewerCoins(amount);
                            Toolkit.client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitGivingCoins".Translate(), viewer: giftee.username, amount: amount.ToString(), newbalance: giftee.coins.ToString()), SendToChatroom(msg));
                            Store_Logger.LogGiveCoins(user, giftee.username, amount);
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Give Viewer Coins Command " + e.Message);
                    }
                }

                if (message.StartsWith("!checkuser"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

                        if (command.Length < 2)
                        {
                            return;
                        }

                        string target = command[1].Replace("@", "");

                        Viewer targeted = Viewers.GetViewer(target);
                        Toolkit.client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitCheckUser".Translate(), viewer: targeted.username, amount: targeted.coins.ToString(), karma: targeted.GetViewerKarma().ToString()), SendToChatroom(msg));

                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Check User Command " + e.Message);
                    }
                }

                if (message.StartsWith("!setkarma"))
                {
                    try
                    {
                        string[] command = message.Split(' ');

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
                            Toolkit.client.SendMessage($"@{user}" + Helper.ReplacePlaceholder("TwitchToolkitSetKarma".Translate(), viewer: targeted.username, karma: amount.ToString()), SendToChatroom(msg));
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Check User Command " + e.Message);
                    }
                }

                if (message.StartsWith("!togglecoins"))
                {
                    if (ToolkitSettings.EarningCoins)
                    {
                        ToolkitSettings.EarningCoins = false;
                        Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOff".Translate(), SendToChatroom(msg));
                    }
                    else
                    {
                        ToolkitSettings.EarningCoins = true;
                        Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOn".Translate(), SendToChatroom(msg));
                    }
                }

                if (message.StartsWith("!togglestore"))
                {
                    if (ToolkitSettings.StoreOpen)
                    {
                        ToolkitSettings.StoreOpen = false;
                        Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + " " + "TwitchToolkitOn".Translate(), SendToChatroom(msg));
                    }
                    else
                    {
                        ToolkitSettings.StoreOpen = true;
                        Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + " " + "TwitchToolkitOn".Translate(), SendToChatroom(msg));
                    }
                }
            }

            // commands are suppressed when not earning coins
            if (ToolkitSettings.EarningCoins)
            {    
                if (message.StartsWith(ToolkitSettings.BalanceCmd) && AllowCommand(msg))
                {
                    Toolkit.client.SendMessage($"@{viewer.username} " + Helper.ReplacePlaceholder("TwitchToolkitBalanceMessage".Translate(), amount: viewer.GetViewerCoins().ToString(), karma: viewer.GetViewerKarma().ToString()), SendToChatroom(msg));
                }

                if (message.StartsWith(ToolkitSettings.KarmaCmd) && !message.Contains("!karmaround"))
                {
                    Toolkit.client.SendMessage($"@{viewer.username} " + "TwitchToolkitWhatIsKarma".Translate() +  $" { viewer.GetViewerKarma()}%", SendToChatroom(msg));
                }

                if (message.StartsWith(ToolkitSettings.InstructionsCmd))
                {
                    Toolkit.client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitInstructions".Translate(), first: ToolkitSettings.BuyeventCmd, second: ToolkitSettings.BuyitemCmd, third: ToolkitSettings.CustomPricingSheetLink), SendToChatroom(msg));
                }

                if (message.StartsWith(ToolkitSettings.PurchaselistCmd))
                {
                    Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitPurchaseList".Translate() + $" {ToolkitSettings.CustomPricingSheetLink}", SendToChatroom(msg));
                }

                if (message.StartsWith(ToolkitSettings.GiftCmd) && ToolkitSettings.GiftingCoins && AllowCommand(msg))
                {
                    string[] command = message.Split(' ');

                    if (command.Count() < 3)
                    {
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
                            Toolkit.client.SendMessage($"@{giftee.username} " + Helper.ReplacePlaceholder("TwitchToolkitGiftCoins".Translate(), amount: amount.ToString(), from: viewer.username), SendToChatroom(msg));
                            Store_Logger.LogGiftCoins(viewer.username, giftee.username, amount);
                        }
                    }
                }

                if (message.StartsWith("!buy ticket") && ToolkitSettings.EnableViewerQueue && AllowCommand(msg))
                {
                    GameComponentPawns pawnComponent = Current.Game.GetComponent<GameComponentPawns>();

                    if (pawnComponent.HasUserBeenNamed(user) || pawnComponent.UserInViewerQueue(user))
                    {
                        return;
                    }

                    if (ToolkitSettings.ChargeViewersForQueue)
                    {
                        if (viewer.GetViewerCoins() < ToolkitSettings.CostToJoinQueue)
                        {
                            Toolkit.client.SendMessage($"@{user} you do not have enough coins to purchase a ticket, it costs {ToolkitSettings.CostToJoinQueue} and you have {viewer.GetViewerCoins()}.", SendToChatroom(msg));
                            return;
                        }

                        viewer.TakeViewerCoins(ToolkitSettings.CostToJoinQueue);
                    }

                    pawnComponent.AddViewerToViewerQueue(user);
                    Toolkit.client.SendMessage($"@{user} you have purchased a ticket and are in the queue!", SendToChatroom(msg));
                }
            }

            if (message.StartsWith(ToolkitSettings.ModinfoCmd))
            {
                Toolkit.client.SendMessage($"@{user} " + "TwitchToolkitModInfo".Translate() + " https://discord.gg/qrtg224 !", SendToChatroom(msg));
            }

            if (ToolkitSettings.StoreOpen)
            {

                if ((message.StartsWith("!buy") || message.StartsWith("!gambleskill")) && AllowCommand(msg))
                {
                    if (message.Split(' ').Count() < 2) return;
                    Purchase_Handler.ResolvePurchase(viewer, msg, SendToChatroom(msg));
                }
            }

            if (message.StartsWith(ToolkitSettings.ModsettingsCmd))
            {
                string minutess = ToolkitSettings.CoinInterval > 1 ? "s" : "";
                string storeon = ToolkitSettings.StoreOpen ? "TwitchToolkitOn".Translate() : "TwitchToolkitOff".Translate();
                string earningcoins = ToolkitSettings.EarningCoins ? "TwitchToolkitOn".Translate() : "TwitchToolkitOff".Translate();
                
                string stats_message = Helper.ReplacePlaceholder("TwitchToolkitModSettings".Translate(),
                    amount: ToolkitSettings.CoinAmount.ToString(),
                    first: ToolkitSettings.CoinInterval.ToString(),
                    second: storeon,
                    third: earningcoins,
                    karma: ToolkitSettings.KarmaCap.ToString()
                    );

                Toolkit.client.SendMessage(stats_message, SendToChatroom(msg));
            }

            if (message.StartsWith(ToolkitSettings.CommandHelpCmd))
            {
                string commands = " " +
                ToolkitSettings.BalanceCmd + ", " +
                ToolkitSettings.BuyeventCmd + ", " +
                ToolkitSettings.BuyitemCmd + ", " +
                ToolkitSettings.InstructionsCmd + ", " +
                ToolkitSettings.PurchaselistCmd + ", " +
                ToolkitSettings.ModinfoCmd + ", " +
                ToolkitSettings.ModsettingsCmd + ", " +
                ToolkitSettings.KarmaCmd;

                if (ToolkitSettings.GiftingCoins)
                    commands += ", " + ToolkitSettings.GiftCmd;

                Toolkit.client.SendMessage("TwitchToolkitUserCommands".Translate() + commands, SendToChatroom(msg));
            }


            if (ToolkitSettings.CommandsModsEnabled && message.StartsWith("!installedmods") && (DateTime.Now - modsCommandCooldown).TotalSeconds >= 10)
            {
                modsCommandCooldown = DateTime.Now;
                string modmsg = "Version: " + _mod.Version + ", Mods: ";
                string[] mods = LoadedModManager.RunningMods.Select((m) => m.Name).ToArray();

                for (int i = 0; i < mods.Length; i++)
                {
                    modmsg += mods[i] + ", ";

                    if (i == (mods.Length - 1) || modmsg.Length > 256)
                    {
                        modmsg = modmsg.Substring(0, modmsg.Length - 2);
                        Toolkit.client.SendMessage(modmsg, SendToChatroom(msg));
                        modmsg = "";
                    }
                }
                return;
            }

            List<TwitchInterfaceBase> modExtensions = Current.Game.components.OfType<TwitchInterfaceBase>().ToList();

            if (modExtensions == null)
            {
                return;
            }

            foreach(TwitchInterfaceBase parser in modExtensions)
            {
                parser.ParseCommand(msg);
            }
        }

        public static bool AllowCommand(IRCMessage msg)
        {
            if (!ToolkitSettings.UseSeparateChatRoom && (msg.Whisper || ToolkitSettings.AllowBothChatRooms || msg.Channel == "#" + ToolkitSettings.Channel)) return true;
            if (msg.Channel == "#chatrooms:" + ToolkitSettings.ChannelID + ":" + ToolkitSettings.ChatroomUUID) return true;
            if (ToolkitSettings.AllowBothChatRooms && ToolkitSettings.UseSeparateChatRoom || (msg.Whisper)) return true;
            return false;
        }

        public static bool SendToChatroom(IRCMessage msg)
        {
            if (msg.Whisper && ToolkitSettings.WhispersGoToChatRoom)
            {
                return true;
            }
            else if (msg.Whisper)
            {
                return false;
            }

            if (msg.Channel == "#" + ToolkitSettings.Channel) return false;
            if (ToolkitSettings.UseSeparateChatRoom && !ToolkitSettings.AllowBothChatRooms) return true;
            if (msg.Channel == "#chatrooms:" + ToolkitSettings.ChannelID + ":" + ToolkitSettings.ChatroomUUID && ToolkitSettings.UseSeparateChatRoom) return true;
            return false;
        }

        static DateTime modsCommandCooldown = DateTime.MinValue;
        static DateTime aliveCommandCooldown = DateTime.MinValue;
    }
}
