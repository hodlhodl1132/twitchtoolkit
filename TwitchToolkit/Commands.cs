using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Store
{
    public class Commands
    {
        public int resetWarning;
        public TwitchToolkit _mod;

        public Commands(TwitchToolkit mod)
        {
            _mod = mod;
        }

        public void CheckCommand(string message, string user)
        {

            if (message.Split(' ')[0] == "/w")
            {
                List<string> messagewhisper = message.Split(' ').ToList();
                messagewhisper.RemoveAt(0);
                message = string.Join(" ", messagewhisper.ToArray());
                Helper.Log(message);
        
            }

            Viewer viewer = Viewer.GetViewer(user);
            viewer.last_seen = DateTime.Now;
            
            //admin commands
            if (user.ToLower() == Settings.Channel.ToLower())
            {
                if (message.StartsWith("!resetviewers"))
                {
                    if (Settings.SyncStreamLabs)
                    {
                        WarningWindow window = new WarningWindow
                        {
                            warning = "You must reset viewers in Streamlabs chatbot and then restart the game."
                        };
                        Find.WindowStack.Add(window);
                    }
                    else if (resetWarning == 0)
                    {
                        _mod._client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningOne".Translate());
                        resetWarning = 1;
                    }
                    else if (resetWarning == 1)
                    {
                        _mod._client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningTwo".Translate());
                        resetWarning = 2;
                    }
                    else if (resetWarning == 2)
                    {
                        _mod._client.SendMessage($"@{user} " + "TwitchToolkitResetViewersWarningThree".Translate());
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
                            _mod._client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitAlreadyMod".Translate(), mod: mod));
                            return;
                        }

                        Viewer modviewer = Viewer.GetViewer(mod);

                        modviewer.SetAsModerator();
                        _mod._client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitAddedMod".Translate(), mod: mod));
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

                        Viewer modviewer = Viewer.GetViewer(mod);

                        modviewer.RemoveAsModerator();
                        _mod._client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitRemovedMod".Translate(), mod: mod));
                }
            }

            //moderator commands
            if (user.ToLower() == Settings.Channel.ToLower() || Viewer.IsModerator(user))
            {
                if (message.StartsWith("!refreshviewers"))
                {
                    TwitchToolkitDev.WebRequest_BeginGetResponse.Main("https://tmi.twitch.tv/group/user/" + Settings.Channel.ToLower() + "/chatters", new Func<TwitchToolkitDev.RequestState, bool>(Settings.viewers.SaveUsernamesFromJsonResponse));
                }

                if (message.StartsWith("!karmaround"))
                {
                    Settings.viewers.AwardViewersCoins();
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
                            foreach(Viewer vwr in Settings.listOfViewers)
                            {
                                vwr.GiveViewerCoins(amount);
                            }
                            _mod._client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitGiveAllCoins".Translate(), amount: amount.ToString()));
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

                        if (user.ToLower() != Settings.Channel.ToLower() && receiver.ToLower() == user.ToLower())
                        {
                            _mod._client.SendMessage($"@{user} " + "TwitchToolkitModCannotGiveCoins".Translate());
                            return;
                        }

                        int amount;
                        bool isNumeric = int.TryParse(command[2], out amount);
                        if (isNumeric)
                        {
                            Viewer giftee = Viewer.GetViewer(receiver);

                            Helper.Log($"Giving viewer {giftee.username} {amount} coins");
                            giftee.GiveViewerCoins(amount);
                            _mod._client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitGivingCoins".Translate(), viewer: giftee.username, amount: amount.ToString(), newbalance: giftee.coins.ToString()));
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

                        Viewer targeted = Viewer.GetViewer(target);
                        _mod._client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitCheckUser".Translate(), viewer: targeted.username, amount: targeted.coins.ToString(), karma: targeted.GetViewerKarma().ToString()));

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
                            Viewer targeted = Viewer.GetViewer(target);
                            targeted.SetViewerKarma(amount);
                            _mod._client.SendMessage($"@{user}" + Helper.ReplacePlaceholder("TwitchToolkitSetKarma".Translate(), viewer: targeted.username, karma: amount.ToString()));
                        }
                    }
                    catch (InvalidCastException e)
                    {
                        Helper.Log("Invalid Check User Command " + e.Message);
                    }
                }

                if (message.StartsWith("!togglecoins"))
                {
                    if (Settings.EarningCoins)
                    {
                        Settings.EarningCoins = false;
                        _mod._client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOff".Translate());
                    }
                    else
                    {
                        Settings.EarningCoins = true;
                        _mod._client.SendMessage($"@{user} " + "TwitchToolkitEarningCoinsMessage".Translate() + " " + "TwitchToolkitOn".Translate());
                    }
                }

                if (message.StartsWith("!togglestore"))
                {
                    if (Settings.StoreOpen)
                    {
                        Settings.StoreOpen = false;
                        _mod._client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + " " + "TwitchToolkitOn".Translate());
                    }
                    else
                    {
                        Settings.StoreOpen = true;
                        _mod._client.SendMessage($"@{user} " + "TwitchToolkitStorePurchasesMessage".Translate() + " " + "TwitchToolkitOn".Translate());
                    }
                }
            }

            // commands are suppressed when not earning coins
            if (Settings.EarningCoins)
            {    
                if (message.StartsWith(Settings.BalanceCmd))
                {
                    Helper.Log("Trying to find User");
                    _mod._client.SendMessage($"@{viewer.username} " + Helper.ReplacePlaceholder("TwitchToolkitBalanceMessage".Translate(), amount: viewer.coins.ToString(), karma: viewer.GetViewerKarma().ToString()));
                }

                if (message.StartsWith(Settings.KarmaCmd) && !message.Contains("!karmaround"))
                {
                    _mod._client.SendMessage($"@{viewer.username} " + "TwitchToolkitWhatIsKarma".Translate() +  $" { viewer.GetViewerKarma()}%");
                }

                if (message.StartsWith(Settings.InstructionsCmd))
                {
                    _mod._client.SendMessage($"@{user} " + Helper.ReplacePlaceholder("TwitchToolkitInstructions".Translate(), first: Settings.BuyeventCmd, second: Settings.BuyitemCmd, third: Settings.CustomPricingSheetLink));
                }

                if (message.StartsWith(Settings.PurchaselistCmd))
                {
                    _mod._client.SendMessage($"@{user} " + "TwitchToolkitPurchaseList".Translate() + $" {Settings.CustomPricingSheetLink}");
                }

                if (message.StartsWith(Settings.GiftCmd) && Settings.GiftingCoins)
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
                        Viewer giftee = Viewer.GetViewer(target);

                        if (Settings.KarmaReqsForGifting)
                        {
                            if (giftee.GetViewerKarma() < Settings.MinimumKarmaToRecieveGifts || viewer.GetViewerKarma() < Settings.MinimumKarmaToSendGifts)
                            {
                                return;
                            }
                        }


                        if (viewer.coins >= amount)
                        {
                            viewer.TakeViewerCoins(amount);
                            giftee.GiveViewerCoins(amount);
                            _mod._client.SendMessage($"@{giftee.username} " + Helper.ReplacePlaceholder("TwitchToolkitGiftCoins".Translate(), amount: amount.ToString(), from: viewer.username));
                        }
                    }
                }
            }

            if (message.StartsWith(Settings.ModinfoCmd))
            {
                _mod._client.SendMessage($"@{user} " + "TwitchToolkitModInfo".Translate() + " https://discord.gg/qrtg224 !");
            }

            if (Settings.StoreOpen)
            {
                if (message.StartsWith(Settings.BuyeventCmd))
                {
                    if (message.Split(' ').Count() < 2 || (message.Split(' ')[1] == "carepackage"))
                    {
                        return;
                    }
                    
                    ShopCommand command = new ShopCommand(message, Viewer.GetViewer(user));
                    if (command.errormessage != null)
                    {
                        _mod._client.SendMessage(command.errormessage);
                    }
                    else if (command.successmessage != null && Settings.PurchaseConfirmations)
                    {
                        _mod._client.SendMessage(command.successmessage);
                    }
                }

                if (message.StartsWith(Settings.BuyitemCmd))
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
                    ShopCommand command2 = new ShopCommand(newcommand, Viewer.GetViewer(user));
                    if (command2.errormessage != null)
                    {
                        _mod._client.SendMessage(command2.errormessage);
                    }
                    else if (command2.successmessage != null && Settings.PurchaseConfirmations)
                    {
                        _mod._client.SendMessage(command2.successmessage);
                    }
                }

            }

            if (message.StartsWith(Settings.ModsettingsCmd))
            {
                string minutess = Settings.CoinInterval > 1 ? "s" : "";
                string storeon = Settings.StoreOpen ? "TwitchToolkitOn".Translate() : "TwitchToolkitOff".Translate();
                string earningcoins = Settings.EarningCoins ? "TwitchToolkitOn".Translate() : "TwitchToolkitOff".Translate();
                
                string stats_message = Helper.ReplacePlaceholder("TwitchToolkitModSettings".Translate(),
                    amount: Settings.CoinAmount.ToString(),
                    first: Settings.CoinInterval.ToString(),
                    second: storeon,
                    third: earningcoins,
                    karma: Settings.KarmaCap.ToString()
                    );

                _mod._client.SendMessage(stats_message);
            }

            if (message.StartsWith(Settings.CommandHelpCmd))
            {
                string commands = " " +
                Settings.BalanceCmd + ", " + 
                Settings.BuyeventCmd + ", " + 
                Settings.BuyitemCmd + ", " + 
                Settings.InstructionsCmd + ", " + 
                Settings.PurchaselistCmd + ", " + 
                Settings.ModinfoCmd + ", " + 
                Settings.ModsettingsCmd + ", " + 
                Settings.KarmaCmd + ", " + 
                Settings.GiftCmd;

                _mod._client.SendMessage("TwitchToolkitUserCommands".Translate() + commands);
            }



            if (Settings.CommandsAliveEnabled && message.StartsWith("!alive", StringComparison.InvariantCultureIgnoreCase) && (DateTime.Now - _mod._aliveCommand).TotalSeconds >= 10)
            {
                _mod._aliveCommand = DateTime.Now;
                _mod._client.SendMessage(
                  "TwitchStoriesChatMessageColonyAlive".Translate() + " " + Current.Game.tickManager.TicksGame.ToReadableRimworldTimeString() +
                  ", " +
                  "TwitchStoriesChatMessageGamePlayed".Translate() + " " + Current.Game.Info.RealPlayTimeInteracting.ToReadableTimeString()
                );
                return;
            }

            if (Settings.CommandsModsEnabled && message.Contains("!installedmods") && (DateTime.Now - _mod._modsCommand).TotalSeconds >= 10)
            {
                _mod._modsCommand = DateTime.Now;
                string msg = "Version: " + _mod.Version + ", Mods: ";
                string[] mods = LoadedModManager.RunningMods.Select((m) => m.Name).ToArray();

                for (int i = 0; i < mods.Length; i++)
                {
                    msg += mods[i] + ", ";

                    if (i == (mods.Length - 1) || msg.Length > 256)
                    {
                        msg = msg.Substring(0, msg.Length - 2);
                        _mod._client.SendMessage(msg);
                        msg = "";
                    }
                }
                return;
            }
        }
    }
}
