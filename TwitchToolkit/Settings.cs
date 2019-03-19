using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using TwitchToolkitDev;
using TwitchToolkit.Store;
using TwitchToolkit.Utilities;

namespace TwitchToolkit
{
    public class Settings : ModSettings
    {
        public static string Channel = "";
        public static string Username = "";
        public static string OAuth = "";
        public static string ChatroomUUID = "";
        public static string ChannelID = "";

        public static int VoteInterval = 5;
        public static int VoteTime = 2;
        public static int VoteOptions = 3;
        public static bool VoteEnabled = true;
        public static bool AutoConnect = true;
        public static bool OtherStorytellersEnabled = true;
        public static bool CommandsModsEnabled = true;
        public static bool CommandsAliveEnabled = true;
        public static bool QuotesEnabled = true;

        public static int CoinInterval = 2;
        public static int CoinAmount = 30;
        public static int MinimumPurchasePrice = 60;
        public static int KarmaCap = 140;
        public static int EventCooldownInterval;
        public static int StartingBalance = 150;
        public static int StartingKarma = 100;

        public static float VotingWindowx = -1;
        public static float VotingWindowy;
        public static bool LargeVotingWindow;
        public static bool VotingWindow;
        public static bool VotingChatMsgs;

        public static string CustomPricingSheetLink = "https://bit.ly/2C7bls0";

        public static bool EarningCoins = true;
        public static bool StoreOpen = false;
        public static bool GiftingCoins = false;

        public static bool WhisperCmdsOnly = false;
        public static bool WhisperCmdsAllowed = true;
        public static bool PurchaseConfirmations = true;
        public static bool EventsHaveCooldowns = true;
        public static bool RepeatViewerNames = false;
        public static bool ViewerNamedColonistQueue = false;

        public static string JWTToken;
        public static string AccountID;

        public static bool UnlimitedCoins = false;
        public static bool MinifiableBuildings = false;

        public static string BalanceCmd;
        public static string BuyeventCmd;
        public static string BuyitemCmd;
        public static string InstructionsCmd;
        public static string PurchaselistCmd;
        public static string ModinfoCmd;
        public static string ModsettingsCmd;
        public static string KarmaCmd;
        public static string GiftCmd;
        public static string CommandHelpCmd;

        public static float TierOneGoodBonus;
        public static float TierOneNeutralBonus;
        public static float TierOneBadBonus;

        public static float TierTwoGoodBonus;
        public static float TierTwoNeutralBonus;
        public static float TierTwoBadBonus;

        public static float TierThreeGoodBonus;
        public static float TierThreeNeutralBonus;
        public static float TierThreeBadBonus;

        public static float TierFourGoodBonus;
        public static float TierFourNeutralBonus;
        public static float TierFourBadBonus;

        public static float DoomBonus;

        public static bool BanViewersWhoPurchaseAlwaysBad;
        public static bool KarmaReqsForGifting;
        public static bool ChatReqsForCoins;

        public static int MinimumKarmaToRecieveGifts;
        public static int MinimumKarmaToSendGifts;
        public static int TimeBeforeHalfCoins;
        public static int TimeBeforeNoCoins;

        public static bool KarmaDecay;
        public static int KarmaDecayPeriod;
        public static bool MaxEvents;
        public static int MaxEventsPeriod;

        public static int MaxBadEventsBeforeDecay;
        public static int MaxGoodEventsBeforeDecay;
        public static int MaxNeutralEventsBeforeDecay;

        public static int MaxBadEventsPerInterval;
        public static int MaxGoodEventsPerInterval;
        public static int MaxNeutralEventsPerInterval;
        public static int MaxCarePackagesPerInterval;

        // viewer storage
        public static Dictionary<string, int> ViewerIds = null;
        public static Dictionary<int, int> ViewerCoins = new Dictionary<int, int>();
        public static Dictionary<int, int> ViewerKarma = new Dictionary<int, int>();

        public static Dictionary<string, string> ViewerColorCodes = new Dictionary<string, string>();

        public static Dictionary<string, bool> ViewerModerators = new Dictionary<string, bool>();

        public static List<Viewer> listOfViewers;
        public static Viewers viewers = new Viewers();

        // product storage
        public static Dictionary<string, int> ProductIds = null;
        public static Dictionary<int, int> ProductTypes = new Dictionary<int, int>();
        public static Dictionary<int, string> ProductNames = new Dictionary<int, string>();
        public static Dictionary<int, int> ProductKarmaTypes = new Dictionary<int, int>();
        public static Dictionary<int, int> ProductAmounts = new Dictionary<int, int>();
        public static Dictionary<int, int> ProductEventIds = new Dictionary<int, int>();
        public static Dictionary<int, int> ProductMaxEvents = new Dictionary<int, int>();

        public static List<Product> products = null;

        // item storage
        public static List<Item> items = null;

        private static List<string> _Categories = Enum.GetNames(typeof(EventCategory)).ToList();
        public static List<int> CategoryWeights = Enumerable.Repeat<int>(100, _Categories.Count).ToList();

        public static Scheduled JobManager = new Scheduled();

        public static double CategoryWeight(EventCategory category)
        {
            var index = _Categories.IndexOf(Enum.GetName(typeof(EventCategory), category));
            if (index < 0 || index >= CategoryWeights.Count)
            {
                return 1;
            }

            return CategoryWeights[index] / 100.0;
        }

        private static Dictionary<int, string> _Events = Events.GetEvents().ToDictionary(e => e.Id, e => e.Description);
        public static Dictionary<int, int> EventWeights = Events.GetEvents().ToDictionary(e => e.Id, e => 100);

        public static double EventWeight(int id)
        {
            if (!EventWeights.ContainsKey(id))
            {
                return 1;
            }

            return EventWeights[id] / 100.0;
        }

        public void Save()
        {
            LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Channel, "Channel", "", true);
            Scribe_Values.Look(ref Username, "Username", "", true);
            Scribe_Values.Look(ref OAuth, "OAuth", "", true);
            Scribe_Values.Look(ref ChannelID, "ChannelID", "", true);
            Scribe_Values.Look(ref ChatroomUUID, "ChatroomUUID", "", true);

            Scribe_Values.Look(ref VoteTime, "VoteTime", 1, true);
            Scribe_Values.Look(ref VoteOptions, "VoteOptions", 3, true);
            Scribe_Values.Look(ref VoteEnabled, "VoteEnabled", false, true);
            Scribe_Values.Look(ref AutoConnect, "AutoConnect", false, true);
            Scribe_Values.Look(ref OtherStorytellersEnabled, "OtherStorytellersEnabled", false, true);
            Scribe_Values.Look(ref CommandsModsEnabled, "CommandsModsEnabled", true, true);
            Scribe_Values.Look(ref CommandsAliveEnabled, "CommandsAliveEnabled", true, true);
            Scribe_Values.Look(ref QuotesEnabled, "QuotesEnabled", true, true);

            Scribe_Values.Look(ref CoinInterval, "CoinInterval", 2, true);
            Scribe_Values.Look(ref CoinAmount, "CoinAmount", 30, true);
            Scribe_Values.Look(ref KarmaCap, "KarmaCap", 140, true);
            Scribe_Values.Look(ref MinimumPurchasePrice, "MinimumPurchasePrice", 60, true);
            Scribe_Values.Look(ref EventCooldownInterval, "EventCooldownInterval", 15, true);
            Scribe_Values.Look(ref StartingBalance, "StartingBalance", 150, true);
            Scribe_Values.Look(ref StartingKarma, "StartingKarma", 100, true);

            Scribe_Values.Look(ref CustomPricingSheetLink, "CustomPricingSheetLink", "https://bit.ly/2GT5daR", true);

            Scribe_Values.Look(ref EarningCoins, "EarningCoins", true, true);
            Scribe_Values.Look(ref StoreOpen, "StoreOpen", false, true);
            Scribe_Values.Look(ref GiftingCoins, "GiftingCoins", false, true);
            Scribe_Values.Look(ref WhisperCmdsAllowed, "WhisperCmdsAllowed", true, true);
            Scribe_Values.Look(ref WhisperCmdsOnly, "WhisperCmdsOnly", false, true);
            Scribe_Values.Look(ref PurchaseConfirmations, "PurchaseConfirmations", true, true);
            Scribe_Values.Look(ref VotingChatMsgs, "VotingChatMsgs", false, true);
            Scribe_Values.Look(ref VotingWindow, "VotingWindow", true, true);
            Scribe_Values.Look(ref ViewerNamedColonistQueue, "ViewerNamedColonistQueue", true, true);

            Scribe_Values.Look(ref MinimumKarmaToRecieveGifts, "MinimumKarmaToRecieveGifts", 40, true);
            Scribe_Values.Look(ref MinimumKarmaToSendGifts, "MinimumKarmaToSendGifts", 100, true);
            Scribe_Values.Look(ref TimeBeforeHalfCoins, "TimeBeforeHalfCoins", 30, true);
            Scribe_Values.Look(ref TimeBeforeNoCoins, "TimeBeforeNoCoins", 60, true);

            Scribe_Values.Look(ref JWTToken, "JWTToken", "", true);
            Scribe_Values.Look(ref AccountID, "AccountID", "", true);

            Scribe_Values.Look(ref MinifiableBuildings, "MinifiableBuildings", false, true);
            Scribe_Values.Look(ref UnlimitedCoins, "UnlimitedCoins", false, true);
            Scribe_Values.Look(ref EventsHaveCooldowns, "EventsHaveCooldowns", true, true);
            Scribe_Values.Look(ref RepeatViewerNames, "RepeatViewerNames", false, true);
            Scribe_Values.Look(ref LargeVotingWindow, "LargeVotingWindow", false, true);

            Scribe_Values.Look(ref BalanceCmd, "BalanceCmd", "!bal", true);
            Scribe_Values.Look(ref BuyeventCmd, "BuyeventCmd", "!buyevent", true);
            Scribe_Values.Look(ref BuyitemCmd, "BuyitemCmd", "!buyitem", true);
            Scribe_Values.Look(ref InstructionsCmd, "InstructionsCmd", "!instructions", true);
            Scribe_Values.Look(ref PurchaselistCmd, "PurchaselistCmd", "!purchaselist", true);
            Scribe_Values.Look(ref ModinfoCmd, "ModinfoCmd", "!modinfo", true);
            Scribe_Values.Look(ref ModsettingsCmd, "ModsettingsCmd", "!modsettings", true);
            Scribe_Values.Look(ref KarmaCmd, "KarmaCmd", "!whatiskarma", true);
            Scribe_Values.Look(ref GiftCmd, "GiftCmd", "!giftcoins", true);
            Scribe_Values.Look(ref CommandHelpCmd, "CommandHelpCmd", "!toolkitcmds", true);

            Scribe_Values.Look(ref TierOneGoodBonus, "TierOneGoodBonus", 16, true);
            Scribe_Values.Look(ref TierOneNeutralBonus, "TierOneNeutralBonus", 36, true);
            Scribe_Values.Look(ref TierOneBadBonus, "TierOneBadBonus", 24, true);

            Scribe_Values.Look(ref TierTwoGoodBonus, "TierTwoGoodBonus", 10, true);
            Scribe_Values.Look(ref TierTwoNeutralBonus, "TierTwoNeutralBonus", 30, true);
            Scribe_Values.Look(ref TierTwoBadBonus, "TierTwoBadBonus", 20, true);

            Scribe_Values.Look(ref TierThreeGoodBonus, "TierThreeGoodBonus", 10, true);
            Scribe_Values.Look(ref TierThreeNeutralBonus, "TierThreeNeutralBonus", 24, true);
            Scribe_Values.Look(ref TierThreeBadBonus, "TierThreeBadBonus", 18, true);

            Scribe_Values.Look(ref TierFourGoodBonus, "TierFourGoodBonus", 6, true);
            Scribe_Values.Look(ref TierFourNeutralBonus, "TierFourNeutralBonus", 18, true);
            Scribe_Values.Look(ref TierFourBadBonus, "TierFourBadBonus", 12, true);

            Scribe_Values.Look(ref DoomBonus, "DoomBonus", 67, true);

            Scribe_Values.Look(ref ChatReqsForCoins, "ChatReqsForCoins", true, true);
            Scribe_Values.Look(ref KarmaReqsForGifting, "KarmaReqsForGifting", true, true);
            Scribe_Values.Look(ref BanViewersWhoPurchaseAlwaysBad, "BanViewersWhoPurchaseAlwaysBad", true, true);

            Scribe_Values.Look(ref KarmaDecay, "KarmaDecay", false, true);
            Scribe_Values.Look(ref MaxEvents, "MaxEvents", false, true);
            Scribe_Values.Look(ref KarmaDecayPeriod, "KarmaDecayPeriod", 5, true);
            Scribe_Values.Look(ref MaxEventsPeriod, "MaxEventsPeriod", 5, true);

            Scribe_Values.Look(ref MaxBadEventsBeforeDecay, "MaxBadEventsBeforeDecay", 3, true);
            Scribe_Values.Look(ref MaxGoodEventsBeforeDecay, "MaxGoodEventsBeforeDecay", 10, true);
            Scribe_Values.Look(ref MaxNeutralEventsBeforeDecay, "MaxNeutralEventsBeforeDecay", 10, true);
            
            Scribe_Values.Look(ref MaxBadEventsPerInterval, "MaxBadEventsPerInterval", 3, true);
            Scribe_Values.Look(ref MaxGoodEventsPerInterval, "MaxGoodEventsPerInterval", 10, true);
            Scribe_Values.Look(ref MaxNeutralEventsPerInterval, "MaxNeutralEventsPerInterval", 10, true);
            Scribe_Values.Look(ref MaxCarePackagesPerInterval, "MaxCarePackagesPerInterval", 10, true);

            Scribe_Values.Look(ref VoteInterval, "VoteInterval", 5, true);

            Scribe_Collections.Look(ref ViewerIds, "ViewerIds", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerCoins, "ViewerCoins", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerKarma, "ViewerKarma", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerModerators, "ViewerModerators", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerColorCodes, "ViewerColorCodes", LookMode.Value, LookMode.Value);

            Scribe_Collections.Look(ref ProductIds, "ProductIds", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductTypes, "ProductTypes", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductNames, "ProductNames", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductKarmaTypes, "ProductKarmaTypes", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductAmounts, "ProductAmounts", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductEventIds, "ProductEventIds", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductMaxEvents, "ProductMaxEvents", LookMode.Value, LookMode.Value);

            Scribe_Collections.Look(ref CategoryWeights, "CategoryWeights", LookMode.Value);


            if (ViewerIds == null)
            {
                ViewerIds = new Dictionary<string, int>();
                ViewerCoins = new Dictionary<int, int>();
                ViewerKarma = new Dictionary<int, int>();
            }


            if (CategoryWeights == null)
            {
                CategoryWeights = Enumerable.Repeat<int>(100, _Categories.Count).ToList();
            }

            if (ViewerColorCodes == null)
            {
                ViewerColorCodes = new Dictionary<string, string>();
            }

            Scribe_Collections.Look(ref EventWeights, "EventWeights", LookMode.Value);
            if (EventWeights == null)
            {
                EventWeights = Events.GetEvents().ToDictionary(e => e.Id, e => 100);
            }

            if (listOfViewers == null)
            {
                listOfViewers = new List<Viewer>();
                foreach (KeyValuePair<string, int> viewer in ViewerIds)
                {
                    int viewerkarma = ViewerKarma[viewer.Value];
                    int viewercoins = ViewerCoins[viewer.Value];
                    Viewer newviewer = new Viewer(viewer.Key, viewer.Value);
                    listOfViewers.Add(newviewer);
                }
                SaveHelper.SaveListOfViewersAsJson();
                SaveHelper.LoadListOfViewers();
            }

            if (products == null)
            {
                if (ProductIds == null || ProductMaxEvents == null)
                {
                    Helper.Log("Ressetting Products");
                    ResetProductData();
                    this.Write();
                }
                else
                {
                    Helper.Log("Loading Product Settings");
                    products = new List<Product>();
                    // load products from settings, then load them into settings class
                    foreach (KeyValuePair<string, int> product in ProductIds)
                    {
                        int id = product.Value;
                        string abr = product.Key;
                        int type = ProductTypes[id];
                        string name = ProductNames[id];
                        KarmaType karmatype = (KarmaType)ProductKarmaTypes[id];
                        int amount = ProductAmounts[id];
                        int evtId = ProductEventIds[id];
                        int maxEvents = ProductMaxEvents[id];
                        products.Add(new Product(id, type, name, abr, karmatype, amount, evtId, maxEvents));
                    }
                }
            }

            if (items == null)
            {
                SaveHelper.LoadListOfItems();
            }
        }

        private static int _showOAuth = 0;
        public static void HideOAuth()
        {
            _showOAuth = 0;
        }

        private static float _padding = 5f;
        private static float _height = 35f;
        private static int _menu = 0;
        public static void DoSettingsWindowContents(Rect rect)
        {
            float buttonWidth = 100f;

            var buttonRect = new Rect(rect.width - _padding - buttonWidth, _padding + _height, buttonWidth, 20f);
            if (Widgets.ButtonText(buttonRect, "Main"))
            {
                _menu = 0;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Coins"))
            {
                _menu = 3;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Events"))
            {
                _menu = 4;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Items"))
            {
                _menu = 1;
            }

            if (Prefs.DevMode)
            {
                buttonRect.y += _height;
                if (Widgets.ButtonText(buttonRect, "StreamElements"))
                {
                    _menu = 2;
                }
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Options"))
            {
                _menu = 5;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Commands"))
            {
                _menu = 6;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Balance"))
            {
                _menu = 7;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Karma"))
            {
                _menu = 8;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Stats"))
            {
                _menu = 9;
            }

            rect.width -= buttonWidth + _padding;
            switch (_menu)
            {
                case 0:
                default:
                    MainMenu(rect);
                    break;
                case 1:
                    LoadItemsIfNotLoaded();
                    ItemMenu(rect);
                    break;
                case 2:
                    StreamElementsMenu(rect);
                    break;
                case 3:
                    CoinMenu(rect);
                    break;
                case 4:
                    if (products == null)
                    {
                        Helper.Log("Ressetting Products");
                        ResetProductData();
                    }
                    EventMenu(rect);
                    break;
                case 5:
                    OptionsMenu(rect);
                    break;
                case 6:
                    CommandMenu(rect);
                    break;
                case 7:
                    BalanceMenu(rect);
                    break;
                case 8:
                    KarmaMenu(rect);
                    break;
                case 9:
                    StatsMenu(rect);
                    break;
            }
        }

        private static void MainMenu(Rect rect)
        {
            var labelRect = new Rect(_padding, _padding + _height, rect.width - (_padding * 2), rect.height - (_padding * 2));
            var inputRect = new Rect(_padding + 140f, _padding + _height, rect.width - (_padding * 2) - 140f, 20f);
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.Label(labelRect, "TwitchStoriesSettingsTwitchChannel".Translate() + ": ");
            Channel = Widgets.TextField(inputRect, Channel, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsTwitchUsername".Translate() + ": ");
            Username = Widgets.TextField(inputRect, Username, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsOAuth".Translate() + ": ");
            if (_showOAuth > 2)
            {
                OAuth = Widgets.TextField(inputRect, OAuth, 999, new Regex("^[a-z0-9:]*$", RegexOptions.IgnoreCase));
            }
            else
            {
                if (Widgets.ButtonText(inputRect, ("TwitchStoriesSettingsOAuthWarning" + _showOAuth).Translate()))
                {
                    _showOAuth++;
                }
            }

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsTwitchAutoConnect".Translate() + ": ");
            if (Widgets.ButtonText(inputRect, (AutoConnect ? "TwitchStoriesEnabled".Translate() : "TwitchStoriesDisabled".Translate())))
            {
                AutoConnect = !AutoConnect;
            }

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsVoteTime".Translate() + ": ");
            VoteTime = (int)Widgets.HorizontalSlider(inputRect, VoteTime, 1, 15, false, VoteTime.ToString() + " " + "TwitchStoriesMinutes".Translate(), null, null, 1);

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsVoteOptions".Translate() + ": ");
            VoteOptions = (int)Widgets.HorizontalSlider(inputRect, VoteOptions, 1, 5, false, VoteOptions.ToString() + " " + "TwitchStoriesOptions".Translate(), null, null, 1);

            labelRect.y += _height;
            inputRect.y += _height;
            inputRect.width = ((inputRect.width - _padding) / 2);
            Widgets.Label(labelRect, "TwitchStoriesSettingsOtherCommands".Translate() + ": ");
            if (Widgets.ButtonText(inputRect, "!installedmods " + (CommandsModsEnabled ? "TwitchStoriesEnabled".Translate() : "TwitchStoriesDisabled".Translate())))
            {
                CommandsModsEnabled = !CommandsModsEnabled;
            }

            inputRect.x += inputRect.width + _padding;
            if (Widgets.ButtonText(inputRect, "!alive " + (CommandsAliveEnabled ? "TwitchStoriesEnabled".Translate() : "TwitchStoriesDisabled".Translate())))
            {
                CommandsAliveEnabled = !CommandsAliveEnabled;
            }

            var mod = LoadedModManager.GetMod<TwitchToolkit>();
            labelRect.y += _height;
            labelRect.height = 30f;
            labelRect.width = 100f;
            if (Widgets.ButtonText(labelRect, "TwitchStoriesReconnect".Translate()))
            {
                mod.Reconnect();
            }

            labelRect.x += 100f + _padding;
            if (Widgets.ButtonText(labelRect, "TwitchStoriesDisconnect".Translate()))
            {
                mod.Disconnect();
            }

            labelRect.x = _padding;
            labelRect.y += _height;
            labelRect.height = rect.height - labelRect.y;
            labelRect.width = rect.width - (_padding * 2);
            Widgets.TextArea(labelRect, string.Join("\r\n", mod.MessageLog), true);

            inputRect.y += inputRect.height;
            if (Widgets.ButtonText(inputRect, "Reset Main Settings"))
            {
                VoteTime = 1;
                VoteOptions = 3;
                CommandsModsEnabled = true;
                CommandsAliveEnabled = true;
                AutoConnect = true;
                mod.WriteSettings();
            }
        }

        private static void CoinMenu(Rect rect)
        {
            var mod = LoadedModManager.GetMod<TwitchToolkit>();
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled("Reward Coins: ", ref EarningCoins, "Should viewers earn coins while watching?");
            listingStandard.CheckboxLabeled("Store Open: ", ref StoreOpen, "Enable purchasing of events and items");
            listingStandard.CheckboxLabeled("Viewers can gift other viewers coins: ", ref GiftingCoins, "Enable gifting");
            listingStandard.CheckboxLabeled("Should viewers have unlimited coins? ", ref UnlimitedCoins, "Unlimited coins?");
            listingStandard.Label("Coins Per Interval: " + CoinAmount);
            CoinAmount = listingStandard.Slider((float)CoinAmount, 1, 250);
            listingStandard.Label("Minimum Purchase Price: " + MinimumPurchasePrice);
            MinimumPurchasePrice = listingStandard.Slider((float)MinimumPurchasePrice, 10, 500);
            listingStandard.Label("Minutes between coin reward: " + CoinInterval);
            CoinInterval = listingStandard.Slider((float)CoinInterval, 1, 60);
            listingStandard.Label("Starting balance: " + StartingBalance);
            StartingBalance = listingStandard.Slider((float)StartingBalance, 0, 1000);
            listingStandard.Label("Link to custom pricing sheet: (check steamworkshop description for instructions)");
            CustomPricingSheetLink = listingStandard.TextEntry(CustomPricingSheetLink);
            if (listingStandard.ButtonText("Disable Events"))
            {
                foreach(Product product in products)
                {
                    product.amount = -10;
                    ProductAmounts[product.id] = -10;
                }
            }
            if (listingStandard.ButtonText("Enable Events"))
            {
                ResetProductData();
            }
            if (listingStandard.ButtonText("Reset Coin Settings"))
            {
                EarningCoins = true;
                StoreOpen = false;
                GiftingCoins = false;
                UnlimitedCoins = false;
                CoinAmount = 30;
                KarmaCap = 140;
                MinimumPurchasePrice = 60;
                CoinInterval = 2;
                CustomPricingSheetLink = "https://bit.ly/2C7bls0";
                StartingBalance = 150;
                StartingKarma = 100;
                mod.WriteSettings();
            }
            listingStandard.End();
        }

        public static void OptionsMenu(Rect rect)
        {
            var mod = LoadedModManager.GetMod<TwitchToolkit>();
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled("Should buildings unable to be uninstalled be included in the item list? ", ref MinifiableBuildings, "Non-Minifiable Buildings?");
            listingStandard.CheckboxLabeled("Should events have cooldowns? ", ref EventsHaveCooldowns, "Event cooldowns?");
            listingStandard.CheckboxLabeled("Should viewer names be repeated in raids/aid? ", ref RepeatViewerNames, "Repeat viewers?");
            listingStandard.CheckboxLabeled("Enable voting window? ", ref VotingWindow, "Large window?");
            if (VotingWindow)
            {
                listingStandard.CheckboxLabeled("Large voting window? ", ref LargeVotingWindow, "Large window?");
            }
            listingStandard.CheckboxLabeled("Send vote options to chat? ", ref VotingChatMsgs, "Vote msgs?");
            listingStandard.CheckboxLabeled("Enable the viewer named colonist queue? ", ref ViewerNamedColonistQueue, "Queue names?");
            listingStandard.Label("How many minutes in a cooldown period: " + EventCooldownInterval);
            EventCooldownInterval = listingStandard.Slider((float)EventCooldownInterval, 1, 120);
            listingStandard.Label("Seperate chatroom UUID:");
            ChatroomUUID = listingStandard.TextEntry(ChatroomUUID);
            listingStandard.Label("Seperate channel id:");
            ChannelID = listingStandard.TextEntry(ChannelID);
            if (listingStandard.ButtonText("Reset Options"))
            {
                MinifiableBuildings = false;
                UnlimitedCoins = false;
                EventsHaveCooldowns = true;
                EventCooldownInterval = 15;
                RepeatViewerNames = false;
                VotingWindow = true;
                LargeVotingWindow = false;
                VotingChatMsgs = false;
                mod.WriteSettings();
            }
            listingStandard.End();
        }

        public static void CommandMenu(Rect rect)
        {
            var mod = LoadedModManager.GetMod<TwitchToolkit>();
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled($"Commands can be whispered to {Username}: ", ref WhisperCmdsAllowed, "Allow whispers");
            listingStandard.CheckboxLabeled($"Commands must be whispered to {Username}: ", ref WhisperCmdsOnly, "Require whispers");  
            listingStandard.CheckboxLabeled("Should purchases be confirmed in chat: ", ref PurchaseConfirmations, "Purchase confirmations");
            listingStandard.Label("Check your coin balance");
            BalanceCmd = listingStandard.TextEntry(BalanceCmd);
            listingStandard.Label("Buy an event");
            BuyeventCmd = listingStandard.TextEntry(BuyeventCmd);
            listingStandard.Label("Buy an item");
            BuyitemCmd = listingStandard.TextEntry(BuyitemCmd);
            listingStandard.Label("Instructions");
            InstructionsCmd = listingStandard.TextEntry(InstructionsCmd);
            listingStandard.Label("Purchase list");
            PurchaselistCmd = listingStandard.TextEntry(PurchaselistCmd);
            listingStandard.Label("Mod info");
            ModinfoCmd = listingStandard.TextEntry(ModinfoCmd);
            listingStandard.Label("Mod settings");
            ModsettingsCmd = listingStandard.TextEntry(ModsettingsCmd);
            listingStandard.Label("Karma explanation");
            KarmaCmd = listingStandard.TextEntry(KarmaCmd);
            listingStandard.Label("Gift coins");
            GiftCmd = listingStandard.TextEntry(GiftCmd);
            listingStandard.Label("All toolkit commands");
            CommandHelpCmd = listingStandard.TextEntry(CommandHelpCmd);
            if (listingStandard.ButtonText("Reset Commands"))
            {
                WhisperCmdsAllowed = true;
                WhisperCmdsOnly = false;
                PurchaseConfirmations = true;

                BalanceCmd = "TwitchToolkitBalCmd".Translate();
                BuyeventCmd = "TwitchToolkitBuyEventCmd".Translate();
                BuyitemCmd = "TwitchTookitBuyItemCmd".Translate();
                InstructionsCmd = "TwitchToolkitInstructionsCmd".Translate();
                PurchaselistCmd = "TwitchToolkitPurchaseListCmd".Translate();
                ModinfoCmd = "TwitchToolkitModInfoCmd".Translate();
                ModsettingsCmd = "TwitchToolkitModSettingsCmd".Translate();
                KarmaCmd = "TwitchToolkitKarmaCmd".Translate();
                GiftCmd = "TwitchToolkitGiftCmd".Translate();
                CommandHelpCmd = "TwitchToolkitCmdHelpCmd".Translate();
                mod.WriteSettings();
            }
            listingStandard.End();
        }

        private static void StreamElementsMenu(Rect rect)
        {
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            JWTToken = listingStandard.TextEntry(JWTToken, 3);
            AccountID = listingStandard.TextEntry(AccountID);

            var inputRect = new Rect(_padding + 140f, _padding + _height * 3, rect.width - (_padding * 2) - 140f, 20f);

            if (Widgets.ButtonText(inputRect, "Import Points"))
            {
                StreamElements element = new StreamElements(AccountID, JWTToken);
                element.ImportPoints();
            }

            listingStandard.End();
        }

        static int BalanceTab = 0;
        private static void BalanceMenu(Rect rect)
        {
            
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            
            if ( listingStandard.ButtonText("Next Page") )
            {
                if (BalanceTab == 2)
                {
                    BalanceTab = 0;
                }
                else
                {
                    BalanceTab++;
                }
            }

            if (BalanceTab == 0)
            { 

                if (EarningCoins)
                {
                    listingStandard.CheckboxLabeled("Should viewers who don't participate stop earning coins?", ref ChatReqsForCoins, "Force chat?");
                }
                if (ChatReqsForCoins)
                {
                    listingStandard.Label("How many minutes without chatting until viewers earn half coins?: " + TimeBeforeHalfCoins);
                    TimeBeforeHalfCoins = listingStandard.Slider((float)TimeBeforeHalfCoins, 10, 60);
                    listingStandard.Label("How many minutes without chatting until viewers earn no coins?: " + TimeBeforeNoCoins);
                    TimeBeforeNoCoins = listingStandard.Slider((float)TimeBeforeNoCoins, TimeBeforeHalfCoins, TimeBeforeHalfCoins * 2);
                }

                if (GiftingCoins)
                {
                    listingStandard.CheckboxLabeled("Enforce karma requirements for gifting?", ref KarmaReqsForGifting, "Enable reqs?");
                }
                if (KarmaReqsForGifting)
                {
                    listingStandard.Label("Minimum karma required to recieve gifts?: " + MinimumKarmaToRecieveGifts);
                    MinimumKarmaToRecieveGifts = listingStandard.Slider((float)MinimumKarmaToRecieveGifts, 0, KarmaCap);
                    listingStandard.Label("Minimum karma required to send gifts?: " + MinimumKarmaToSendGifts);
                    MinimumKarmaToSendGifts = listingStandard.Slider((float)MinimumKarmaToSendGifts, MinimumKarmaToRecieveGifts, KarmaCap);
                }
                listingStandard.CheckboxLabeled("Should viewers go into negative karma?", ref BanViewersWhoPurchaseAlwaysBad);
                if (listingStandard.ButtonText("Reset"))
                {
                    ChatReqsForCoins = true;
                    TimeBeforeHalfCoins = 30;
                    TimeBeforeNoCoins = 45;
                    KarmaReqsForGifting = false;
                    MinimumKarmaToRecieveGifts = 33;
                    MinimumKarmaToSendGifts = 100;
                }
            }
            else if (BalanceTab == 1)
            {
                listingStandard.Label("Placehoder for karma decay, disabled in this update");
                //listingStandard.CheckboxLabeled("Karma Decay", ref KarmaDecay);
                //listingStandard.Label("Karma Decay Period: " + KarmaDecayPeriod + " minutes");
                //KarmaDecayPeriod = listingStandard.Slider(KarmaDecayPeriod, 1, 30);
                //listingStandard.Label("Max Bad Events Before Decay: " + MaxBadEventsBeforeDecay);
                //MaxBadEventsBeforeDecay = listingStandard.Slider(MaxBadEventsBeforeDecay, 1, 30);
                //listingStandard.Label("Max Good Events Before Decay: " + MaxGoodEventsBeforeDecay);
                //MaxGoodEventsBeforeDecay = listingStandard.Slider(MaxGoodEventsBeforeDecay, 1, 30);
                //listingStandard.Label("Max Neutral Events Before Decay: " + MaxNeutralEventsBeforeDecay);
                //MaxNeutralEventsBeforeDecay = listingStandard.Slider(MaxNeutralEventsBeforeDecay, 1, 30);
                //if (listingStandard.ButtonText("Reset"))
                //{
                //    KarmaDecay = false;
                //    KarmaDecayPeriod = 5;
                //    MaxBadEventsBeforeDecay = 3;
                //    MaxGoodEventsBeforeDecay = 10;
                //    MaxNeutralEventsBeforeDecay = 10;
                //}
            }
            else if (BalanceTab == 2)
            {
                listingStandard.CheckboxLabeled("Limit max amount of types of events?", ref MaxEvents);
                listingStandard.Label("Max Events Period: " + MaxEventsPeriod + " minutes");
                MaxEventsPeriod = listingStandard.Slider(MaxEventsPeriod, 1, 60);
                listingStandard.Label("Max Bad Events Per Interval: " + MaxBadEventsPerInterval);
                MaxBadEventsPerInterval = listingStandard.Slider(MaxBadEventsPerInterval, 0, 60);
                listingStandard.Label("Max Good Events Per Interval: " + MaxGoodEventsPerInterval);
                MaxGoodEventsPerInterval = listingStandard.Slider(MaxGoodEventsPerInterval, 0, 60);
                listingStandard.Label("Max Neutral Events Per Interval: " + MaxNeutralEventsPerInterval);
                MaxNeutralEventsPerInterval = listingStandard.Slider(MaxNeutralEventsPerInterval, 0, 60);
                listingStandard.Label("Max Care Packages Per Interval: " + MaxCarePackagesPerInterval);
                MaxCarePackagesPerInterval = listingStandard.Slider(MaxCarePackagesPerInterval, 0, 60);
                if (listingStandard.ButtonText("Reset"))
                {
                    MaxEvents = false;
                    MaxEventsPeriod = 5;
                    MaxBadEventsPerInterval = 3;
                    MaxGoodEventsPerInterval = 10;
                    MaxNeutralEventsPerInterval = 10;
                    MaxCarePackagesPerInterval = 10;
                }
            }

            listingStandard.End();
        }

        static int KarmaTab = 1;
        private static void KarmaMenu(Rect rect)
        {
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            if (KarmaTab == 0)
            { 
                var labelRect = new Rect(2, 35f, 150, 30);

                var listingRect = new Rect(152, 35f, 250, 400);

                Widgets.Label(labelRect, "T1 Good:" + TierOneGoodBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T1 Neutral:" + TierOneNeutralBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T1 Bad:" + TierOneBadBonus);
                labelRect.y += 24;

                Widgets.Label(labelRect, "T2 Good:" + TierTwoGoodBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T2 Neutral:" + TierTwoNeutralBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T2 Bad:" + TierTwoBadBonus);
                labelRect.y += 24;

                Widgets.Label(labelRect, "T3 Good:" + TierThreeGoodBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T3 Neutral:" + TierThreeNeutralBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T3 Bad:" + TierThreeBadBonus);
                labelRect.y += 24;

                Widgets.Label(labelRect, "T4 Good:" + TierFourGoodBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T4 Neutral:" + TierFourNeutralBonus);
                labelRect.y += 24;
                Widgets.Label(labelRect, "T4 Bad:" + TierFourBadBonus);
                labelRect.y += 24;

                Widgets.Label(labelRect, "Doom: " + DoomBonus);
                labelRect.y += 24;

                listingStandard.Begin(listingRect);

                TierOneGoodBonus = listingStandard.Slider(TierOneGoodBonus, 0, 100);
                TierOneNeutralBonus = listingStandard.Slider(TierOneNeutralBonus, 0, 100);
                TierOneBadBonus = listingStandard.Slider(TierOneBadBonus, 0, 100);

                TierTwoGoodBonus = listingStandard.Slider(TierTwoGoodBonus, 0, 100);
                TierTwoNeutralBonus = listingStandard.Slider(TierTwoNeutralBonus, 0, 100);
                TierTwoBadBonus = listingStandard.Slider(TierTwoBadBonus, 0, 100);

                TierThreeGoodBonus = listingStandard.Slider(TierThreeGoodBonus, 0, 100);
                TierThreeNeutralBonus = listingStandard.Slider(TierThreeNeutralBonus, 0, 100);
                TierThreeBadBonus = listingStandard.Slider(TierThreeBadBonus, 0, 100);

                TierFourGoodBonus = listingStandard.Slider(TierFourGoodBonus, 0, 100);
                TierFourNeutralBonus = listingStandard.Slider(TierFourNeutralBonus, 0, 100);
                TierFourBadBonus = listingStandard.Slider(TierFourBadBonus, 0, 100);

                DoomBonus = listingStandard.Slider(DoomBonus, 0, 100);
                if (listingStandard.ButtonText("Default Karma"))
                {
                    TierOneGoodBonus = 16;
                    TierOneNeutralBonus = 36;
                    TierOneBadBonus = 24;
                    TierTwoGoodBonus = 10;
                    TierTwoNeutralBonus = 30;
                    TierTwoBadBonus = 20;
                    TierThreeGoodBonus = 10;
                    TierThreeNeutralBonus = 24;
                    TierThreeBadBonus = 18;
                    TierFourGoodBonus = 6;
                    TierFourNeutralBonus = 18;
                    TierFourBadBonus = 12;
                    DoomBonus = 67;
                }
            }
            else
            {
                listingStandard.Begin(rect);
                listingStandard.Label("Max Karma: " + KarmaCap);
                KarmaCap = listingStandard.Slider((float)KarmaCap, 100, 1000);
                listingStandard.Label("Starting karma: " + StartingKarma);
                StartingKarma = listingStandard.Slider((float)StartingKarma, 10, Settings.KarmaCap);
                if (listingStandard.ButtonText("Default Karma"))
                {
                    StartingKarma = 100;
                    KarmaCap = 140;
                }
            }
            if ( listingStandard.ButtonText("Next Page") )
            {
                if (KarmaTab == 1)
                {
                    KarmaTab = 0;
                }
                else
                {
                    KarmaTab++;
                }
            }

            listingStandard.End();

        }

        static int StatsMinutes = 1;
        public static void StatsMenu(Rect rect)
        {
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            listingStandard.Label("In the last " + StatsMinutes + " minutes");
            StatsMinutes = listingStandard.Slider(StatsMinutes, 1, 120);
            listingStandard.Label("Recent Events: " + PurchaseLogger.CountRecentEvents(StatsMinutes));
            listingStandard.Label("Good: " + PurchaseLogger.CountRecentEventsOfType(KarmaType.Good, StatsMinutes));
            listingStandard.Label("Bad: " + PurchaseLogger.CountRecentEventsOfType(KarmaType.Bad, StatsMinutes));
            listingStandard.Label("Neutral: " + PurchaseLogger.CountRecentEventsOfType(KarmaType.Neutral, StatsMinutes) + "\n\n");

            listingStandard.Label("Recently spent: " + PurchaseLogger.CountRecentEventsTotalCost(StatsMinutes));
            listingStandard.Label("Good: " + PurchaseLogger.CountRecentEventsTotalCostOfType(KarmaType.Good, StatsMinutes));
            listingStandard.Label("Bad: " + PurchaseLogger.CountRecentEventsTotalCostOfType(KarmaType.Bad, StatsMinutes));
            listingStandard.Label("Neutral: " + PurchaseLogger.CountRecentEventsTotalCostOfType(KarmaType.Neutral, StatsMinutes));
            listingStandard.End();
        }

        public static int ProductScroll = 0;
        private static string searchquery;

        public static int ResetProductStage { get; private set; }
        public static string ResetAdminWarning { get; private set; }
        public static int ItemScroll { get; private set; }
        public static int ResetItemStage { get; private set; }
        public static int ResetViewerStage { get; internal set; }

        public static void EventMenu(Rect rect)
        {
            var mod = LoadedModManager.GetMod<TwitchToolkit>();

            var scrollRect = new Rect(_padding + 300f, _padding + _height, 60f, 20f);

            var searchRect = new Rect(_padding, _padding + _height, 300f, 20f);

            searchquery = Widgets.TextField(searchRect, searchquery, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));


            if (searchquery != "")
            {
                if (Widgets.ButtonText(scrollRect, "search"))
                {
                    ProductScroll = 0;
                }
            }
             
            scrollRect.width = 40f;
            scrollRect.x += 60f;
            if (ProductScroll > 0)
            {
                if (Widgets.ButtonText(scrollRect, "up"))
                {
                    ProductScroll = Math.Max(0, ProductScroll - 1);
                }
            }

            int count = 0;
            int scroll = 0;

            scrollRect.x += 40f;
            if (ProductScroll < (products.Count - count) - 8)
            {
                if (Widgets.ButtonText(scrollRect, "down"))
                {
                    ProductScroll++;
                }
            }

            scrollRect.x += 40f;
            if (Widgets.ButtonText(scrollRect, "x0.5"))
            {
                Products.MultiplyProductPrices(0.5);
            }

            scrollRect.x += 40f;
            if (Widgets.ButtonText(scrollRect, "x2"))
            {
                Products.MultiplyProductPrices(2);
            }

            scrollRect.x += 40f;
            if (Widgets.ButtonText(scrollRect, "x5"))
            {
                Products.MultiplyProductPrices(5);
            }

            scrollRect.x += 40f;
            if (Widgets.ButtonText(scrollRect, "x10"))
            {
                Products.MultiplyProductPrices(10);
            }

            scrollRect.x += 40f;
            scrollRect.width = (150f);

            if (ResetProductStage == 0)
            {
                ResetAdminWarning = "Reset to Default";
            }
            else if (ResetProductStage == 1)
            {
                ResetAdminWarning = "Are you sure?";
            }
            else if (ResetProductStage == 2)
            {
                ResetAdminWarning = "One more time";
            }
            else if (ResetProductStage == 3)
            {
                ResetProductStage = 0;
                ResetProductData();
                EventMenu(rect);
            }

            if (Widgets.ButtonText(scrollRect, ResetAdminWarning))
            {
                ResetProductStage += 1;
            }

            scrollRect.y += _height;
            
            Rect productline = new Rect(_padding, _padding + _height, 600f, 30f);
            
            List<Product> query = products.Where(a => (a.abr.Contains(searchquery))).ToList();
            foreach (Product product in query)
            {
                if (++scroll <= ProductScroll)
                {
                    continue;
                }

                productline.y += 30f;

                if (productline.y  > rect.height - 50f)
                {
                    continue;
                }

                Rect smallButton = new Rect(300f, productline.y, 40f, 30f);

                string pricelabel = (product.amount) < 0 ? "Disabled" : product.amount.ToString();
                Widgets.Label(productline, $"{ProductScroll + count + 1} - {product.name}: {pricelabel}");
                
                int newprice = product.amount;

                if (Widgets.ButtonText(smallButton, "-" + 500))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 500 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 50)
                    {
                        newprice = 50;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 50))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 50 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 50)
                    {
                        newprice = 50;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 10))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 10 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 50)
                    {
                        newprice = 50;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 10))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 10 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 50))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 50 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 500))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 500 * GenUI.CurrentAdjustmentMultiplier();
                }

                string karmabutton = product.karmatype.ToString();

                smallButton.x += 60f;
                smallButton.width = 60f;
                if (Widgets.ButtonText(smallButton, karmabutton))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    if (product.karmatype == KarmaType.Doom)
                    {
                        product.karmatype = 0;
                        ProductKarmaTypes[product.id] = 0;
                    }
                    else
                    {
                        product.karmatype = product.karmatype + 1;
                        ProductKarmaTypes[product.id] = ProductKarmaTypes[product.id] + 1;
                    }
                }

                smallButton.x += 60f;
                if (Widgets.ButtonText(smallButton, "Disable"))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice = -10;
                }

                smallButton.x += 60f;
                if (Widgets.ButtonText(smallButton, "Max: " + ProductMaxEvents[product.id]))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    if (ProductMaxEvents[product.id] < 10)
                    {
                        ProductMaxEvents[product.id]++;
                    }
                    else
                    {
                        ProductMaxEvents[product.id] = 1;
                    }
                }

                ProductAmounts[product.id] = newprice;
                product.amount = newprice;

                count++;
            }
        }


        public static void ItemMenu(Rect rect)
        {
            var mod = LoadedModManager.GetMod<TwitchToolkit>();

            var scrollRect = new Rect(_padding + 300f, _padding + _height, 60f, 20f);

            var searchRect = new Rect(_padding, _padding + _height, 300f, 20f);

            searchquery = Widgets.TextField(searchRect, searchquery, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));

            if (searchquery != "")
            {
                if (Widgets.ButtonText(scrollRect, "search"))
                {
                    ItemScroll = 0;
                }
            }

            scrollRect.width = 40f;
            scrollRect.x += 60f;

            if (ItemScroll > 0)
            {
                if (Widgets.ButtonText(scrollRect, "up"))
                {
                    ItemScroll = Math.Max(0, ItemScroll - 1);
                }
            }

            int count = 0;
            int scroll = 0;

            if (ItemScroll < (items.Count - count) - 8)
            {
                scrollRect.x += 40f;
                if (Widgets.ButtonText(scrollRect, "down"))
                {
                    ItemScroll++;
                }
            }

            scrollRect.x = _padding + (rect.width - (_padding * 2)) / 2 + (rect.width - (_padding * 2)) / 4;
            scrollRect.width = (rect.width - (_padding * 2)) / 4;

            if (ResetItemStage == 0)
            {
                ResetAdminWarning = "Reset to Default";
            }
            else if (ResetItemStage == 1)
            {
                ResetAdminWarning = "Are you sure?";
            }
            else if (ResetItemStage == 2)
            {
                ResetAdminWarning = "One more time";
            }
            else if (ResetItemStage == 3)
            {
                ResetItemStage = 0;
                ResetItemData();
                mod.WriteSettings();
                ItemMenu(rect);
            }

            if (Widgets.ButtonText(scrollRect, ResetAdminWarning))
            {
                ResetItemStage += 1;
            }

            scrollRect.y += _height;
            
            Rect itemline = new Rect(_padding, _padding + _height, 600f, 30f);
            
            List<Item> query = items.Where(a => (a.abr.Contains(searchquery.ToLower()))).ToList();
            foreach (Item item in query)
            {
                if (++scroll <= ItemScroll)
                {
                    continue;
                }

                itemline.y += 30f;

                if (itemline.y  > rect.height - 50f)
                {
                    continue;
                }

                Rect smallButton = new Rect(300f, itemline.y, 40f, 30f);

                string pricelabel = (item.price) < 0 ? "Disabled" : item.price.ToString();
                Widgets.Label(itemline, $"{ItemScroll + count + 1} - {item.abr}: {pricelabel}");

                int newprice = item.price;
                if (Widgets.ButtonText(smallButton, "-" + 100))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 100 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 1)
                    {
                        newprice = 1;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 10))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 10 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 1)
                    {
                        newprice = 1;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 1))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 1 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 1)
                    {
                        newprice = 1;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 1))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 1 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 10))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 10 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 100))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 100 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                smallButton.width = 60f;
                if (Widgets.ButtonText(smallButton, "Disable"))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice = -10;
                }

                item.price = newprice;

                count++;
            }
        }

        public static void ResetProductData()
        {
            products = new List<Product>();
            ProductIds = new Dictionary<string, int>();
            ProductTypes = new Dictionary<int, int>();
            ProductNames = new Dictionary<int, string>();
            ProductKarmaTypes = new Dictionary<int, int>();
            ProductAmounts = new Dictionary<int, int>();
            ProductEventIds = new Dictionary<int, int>();
            ProductMaxEvents = new Dictionary<int, int>();
            // if no previous save data create new products
            List<Product> defaultProducts = Products.GenerateDefaultProducts().ToList();
            foreach (Product product in defaultProducts)
            {
                int id = product.id;
                ProductIds.Add(product.abr, id);
                ProductTypes.Add(id, product.type);
                ProductNames.Add(id, product.name);
                ProductKarmaTypes.Add(id, (int)product.karmatype);
                ProductAmounts.Add(id, product.amount);
                ProductEventIds.Add(id, product.evtId);
                ProductMaxEvents.Add(id, product.maxEvents);
                products.Add(product);
            }
        }

        public static void ResetItemData()
        {
            items = new List<Item>();
            Item.TryMakeAllItems();
            SaveHelper.SaveListOfItemsAsJson();
        }

        public static void LoadItemsIfNotLoaded()
        {
            if (items == null)
            {
                ResetItemData();
            }
        }

    }
}
