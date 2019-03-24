using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Store
{
    public static class Commands
    {
        static int resetWarning;
        static TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();
        static readonly ToolkitIRC _client = Toolkit.client;

        public static void CheckCommand(string message, string user)
        {

            if (message.Split(' ')[0] == "/w")
            {
                List<string> messagewhisper = message.Split(' ').ToList();
                messagewhisper.RemoveAt(0);
                message = string.Join(" ", messagewhisper.ToArray());
                Helper.Log(message);
        
            }

            Viewer viewer = Viewers.GetViewer(user);
            viewer.last_seen = DateTime.Now;
            
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
                        _client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningOne".Translate());
                        resetWarning = 1;
                    }
                    else if (resetWarning == 1)
                    {
                        _client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningTwo".Translate());
                        resetWarning = 2;
                    }
                    else if (resetWarning == 2)
                    {
                        _client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningThree".Translate());
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
                            _client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitAlreadyMod".Translate(), mod: mod));
                            return;
                        }

                        Viewer modviewer = Viewers.GetViewer(mod);

                        modviewer.SetAsModerator();
                        _client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitAddedMod".Translate(), mod: mod));
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
                        _client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitRemovedMod".Translate(), mod: mod));
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
                            _client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitGiveAllCoins".Translate(), amount: amount.ToString()));
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
                            _client.SendMessage($"@{user} " + "TwitchToolkitModCannotGiveCoins".Translate());
                            return;
                        }

                        int amount;
                        bool isNumeric = int.TryParse(command[2], out amount);
                        if (isNumeric)
                        {
                            Viewer giftee = Viewers.GetViewer(receiver);

                            Helper.Log($"Giving viewer {giftee.username} {amount} coins");
                            giftee.GiveViewerCoins(amount);
                            _client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitGivingCoins".Translate(), viewer: giftee.username, amount: amount.ToString(), newbalance: giftee.coins.ToString()));
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
                        _client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitCheckUser".Translate(), viewer: targeted.username, amount: targeted.coins.ToString(), karma: targeted.GetViewerKarma().ToString()));

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
                            _client.SendMessage($"@{user}" + Helper.ReplacePlaceholder("TwitchToolkitSetKarma".Translate(), viewer: targeted.username, karma: amount.ToString()));
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
                        _client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOff".Translate());
                    }
                    else
                    {
                        ToolkitSettings.EarningCoins = true;
                        _client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOn".Translate());
                    }
                }

                if (message.StartsWith("!togglestore"))
                {
                    if (ToolkitSettings.StoreOpen)
                    {
                        ToolkitSettings.StoreOpen = false;
                        _client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + " " + "TwitchToolkitOn".Translate());
                    }
                    else
                    {
                        ToolkitSettings.StoreOpen = true;
                        _client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + " " + "TwitchToolkitOn".Translate());
                    }
                }
            }

            // commands are suppressed when not earning coins
            if (ToolkitSettings.EarningCoins)
            {    
                if (message.StartsWith(ToolkitSettings.BalanceCmd))
                {
                    Helper.Log("Trying to find User");
                    _client.SendMessage($"@{viewer.username} " + Helper.ReplacePlaceholder("TwitchToolkitBalanceMessage".Translate(), amount: viewer.coins.ToString(), karma: viewer.GetViewerKarma().ToString()));
                }

                if (message.StartsWith(ToolkitSettings.KarmaCmd) && !message.Contains("!karmaround"))
                {
                    _client.SendMessage($"@{viewer.username} " + "TwitchToolkitWhatIsKarma".Translate() +  $" { viewer.GetViewerKarma()}%");
                }

                if (message.StartsWith(ToolkitSettings.InstructionsCmd))
                {
                    _client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitInstructions".Translate(), first: ToolkitSettings.BuyeventCmd, second: ToolkitSettings.BuyitemCmd, third: ToolkitSettings.CustomPricingSheetLink));
                }

                if (message.StartsWith(ToolkitSettings.PurchaselistCmd))
                {
                    _client.SendMessage($"@{user} " + "TwitchToolkitPurchaseList".Translate() + $" {ToolkitSettings.CustomPricingSheetLink}");
                }

                if (message.StartsWith(ToolkitSettings.GiftCmd) && ToolkitSettings.GiftingCoins)
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


                        if (viewer.coins >= amount)
                        {
                            viewer.TakeViewerCoins(amount);
                            giftee.GiveViewerCoins(amount);
                            _client.SendMessage($"@{giftee.username} " + Helper.ReplacePlaceholder("TwitchToolkitGiftCoins".Translate(), amount: amount.ToString(), from: viewer.username));
                        }
                    }
                }
            }

            if (message.StartsWith(ToolkitSettings.ModinfoCmd))
            {
                _client.SendMessage($"@{user} " + "TwitchToolkitModInfo".Translate() + " https://discord.gg/qrtg224 !");
            }

            if (ToolkitSettings.StoreOpen)
            {
                if (message.StartsWith(ToolkitSettings.BuyeventCmd))
                {
                    if (message.Split(' ').Count() < 2 || (message.Split(' ')[1] == "carepackage"))
                    {
                        return;
                    }
                    
                    StoreCommands command = new StoreCommands(message, Viewers.GetViewer(user));
                    if (command.errormessage != null)
                    {
                        _client.SendMessage(command.errormessage);
                    }
                    else if (command.successmessage != null && ToolkitSettings.PurchaseConfirmations)
                    {
                        _client.SendMessage(command.successmessage);
                    }
                }

                if (message.StartsWith(ToolkitSettings.BuyitemCmd))
                {
                    string[] command = message.Split(' ');

                    if (command.Length < 2)
                    {
                        return;
                    }

                    string item = command[1];
                    command[1] = "TwitchToolkitCarepackage".Translate();
                    command[0] = item.ToLower();

                    string newcommand = string.Join(" ", command);
                    Helper.Log("Attemping item purchase " + newcommand);
                    StoreCommands command2 = new StoreCommands(newcommand, Viewers.GetViewer(user));
                    if (command2.errormessage != null)
                    {
                        _client.SendMessage(command2.errormessage);
                    }
                    else if (command2.successmessage != null && ToolkitSettings.PurchaseConfirmations)
                    {
                        _client.SendMessage(command2.successmessage);
                    }
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

                _client.SendMessage(stats_message);
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
                ToolkitSettings.KarmaCmd + ", " +
                ToolkitSettings.GiftCmd;

                _client.SendMessage("TwitchToolkitUserCommands".Translate() + commands);
            }


            if (ToolkitSettings.CommandsModsEnabled && message.Contains("!installedmods") && (DateTime.Now - modsCommandCooldown).TotalSeconds >= 10)
            {
                modsCommandCooldown = DateTime.Now;
                string msg = "Version: " + _mod.Version + ", Mods: ";
                string[] mods = LoadedModManager.RunningMods.Select((m) => m.Name).ToArray();

                for (int i = 0; i < mods.Length; i++)
                {
                    msg += mods[i] + ", ";

                    if (i == (mods.Length - 1) || msg.Length > 256)
                    {
                        msg = msg.Substring(0, msg.Length - 2);
                        Toolkit.client.SendMessage(msg);
                        msg = "";
                    }
                }
                return;
            }
        }

        static DateTime modsCommandCooldown = DateTime.MinValue;
        static DateTime aliveCommandCooldown = DateTime.MinValue;
    }
}
