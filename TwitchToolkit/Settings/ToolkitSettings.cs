using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Settings;
using TwitchToolkit.Store;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit
{
    public class ToolkitSettings : ModSettings
    {
        public static bool FirstTimeInstallation = true;

        #region IRCAuthentication
        public static string Channel = "";
        public static string Username = "";
        public static string OAuth = "";
        public static bool AutoConnect = true;
        #endregion

        #region TwitchChatRooms
        public static bool UseSeparateChatRoom = false;
        public static bool AllowBothChatRooms = false;
        public static bool WhispersGoToChatRoom = true;
        public static string ChannelID = "";
        public static string ChatroomUUID = "";
        #endregion

        #region VoteOptions
        public static int VoteTime = 2;
        public static int VoteOptions = 3;
        public static bool TimedStorytelling = false;
        public static int TimeBetweenStorytellingEvents = 10;
        public static bool VotingChatMsgs = false;
        public static bool VotingWindow = true;
        public static float VotingWindowx = -1;
        public static float VotingWindowy = -1;
        public static bool LargeVotingWindow = true;
        #endregion

        #region StatCommands
        public static bool CommandsModsEnabled = true;
        #endregion

        #region CoinSettings
        public static int StartingBalance = 150;
        public static int CoinInterval = 2;
        public static int CoinAmount = 30;
        public static int MinimumPurchasePrice = 60;
        public static bool UnlimitedCoins = false;
        #endregion

        #region StoreSettings
        public static bool EarningCoins = true;
        public static string CustomPricingSheetLink = "bit.ly/toolkit-list";
        #endregion

        #region Options
        public static bool WhisperCmdsAllowed = true;
        public static bool WhisperCmdsOnly = false; 
        public static bool PurchaseConfirmations = true;
        public static bool RepeatViewerNames = false;
        public static bool MinifiableBuildings = false; //
        #endregion

        #region StreamElements
        public static bool SyncStreamElements = false;
        public static string AccountID = "";
        public static string JWTToken = "";
        #endregion

        #region StreamLabs
        public static bool SyncStreamLabs = false;
        #endregion

        #region KarmaSettings
        public static int StartingKarma = 100;
        public static int KarmaCap = 140;
        public static bool BanViewersWhoPurchaseAlwaysBad = true;
        public static bool KarmaReqsForGifting = false;
        public static int MinimumKarmaToRecieveGifts = 33;
        public static int MinimumKarmaToSendGifts = 100;
        public static int KarmaMinimum = 10;
        #endregion

        #region KarmaBonusValues
        public static int TierOneGoodBonus = 16;
        public static int TierOneNeutralBonus = 36;
        public static int TierOneBadBonus = 24;

        public static int TierTwoGoodBonus = 10;
        public static int TierTwoNeutralBonus = 30;
        public static int TierTwoBadBonus = 20;

        public static int TierThreeGoodBonus = 10;
        public static int TierThreeNeutralBonus = 24;
        public static int TierThreeBadBonus = 18;

        public static int TierFourGoodBonus = 6;
        public static int TierFourNeutralBonus = 18;
        public static int TierFourBadBonus = 12;

        public static int DoomBonus = 67;
        #endregion

        #region LurkerSettings
        public static bool ChatReqsForCoins = true;
        public static int TimeBeforeHalfCoins = 30;
        public static int TimeBeforeNoCoins = 60;
        #endregion

        #region KarmaDecay
        public static bool KarmaDecay = false;
        public static int KarmaDecayPeriod = 5;
        public static int MaxBadEventsBeforeDecay = 0;
        public static int MaxGoodEventsBeforeDecay = 0;
        public static int MaxNeutralEventsBeforeDecay = 0;
        #endregion

        #region MaxEvents
        public static bool MaxEvents = false;
        public static int MaxEventsPeriod = 5;
        public static int MaxBadEventsPerInterval = 3;
        public static int MaxGoodEventsPerInterval = 10;
        public static int MaxNeutralEventsPerInterval = 10;
        public static int MaxCarePackagesPerInterval = 10;

        public static bool EventsHaveCooldowns = true;
        public static int EventCooldownInterval = 15;
        #endregion

        #region ViewerData
        public static Dictionary<string, string> ViewerColorCodes = new Dictionary<string, string>();
        public static Dictionary<string, bool> ViewerModerators = new Dictionary<string, bool>();
        public static List<string> BannedViewers = new List<string>();
        #endregion

        #region ViewerSettings
        public static bool EnableViewerQueue = true;
        public static bool ViewerNamedColonistQueue = true;
        public static bool ChargeViewersForQueue = false;
        public static int CostToJoinQueue = 0;

        public static int SubscriberExtraCoins = 10;
        public static float SubscriberCoinMultiplier = 1.25f;
        public static int SubscriberExtraVotes = 1;

        public static int VIPExtraCoins = 5;
        public static float VIPCoinMultiplier = 1.15f;
        public static int VIPExtraVotes = 0;

        public static int ModExtraCoins = 3;
        public static float ModCoinMultiplier = 1.05f;
        public static int ModExtraVotes = 0;
        #endregion

        #region VoteSettings
        public static Dictionary<string, int> VoteWeights = new Dictionary<string, int>();
        #endregion

        #region Storyteller

        #region Hodlbot
        public static bool HodlBotEnabled = true;
        public static float HodlBotMTBDays = 1;

        public static Dictionary<string, float> VoteTypeWeights = new Dictionary<string, float>();
        public static Dictionary<string, float> VoteCategoryWeights = new Dictionary<string, float>();
        #endregion

        #region Torytalker
        public static bool ToryTalkerEnabled = false;
        public static float ToryTalkerMTBDays = 2;
        #endregion

        #region UristBot
        public static bool UristBotEnabled = false;
        public static float UristBotMTBDays = 6;
        #endregion

        #region Milasandra
        public static bool MilasandraEnabled = false;
        #endregion

        #region Mercurius
        public static bool MercuriusEnabled = false;
        #endregion

        #endregion

        #region CustomDefs

        public static List<string> CustomCommandDefs = new List<string>();

        #endregion

        private static Vector2 scrollVector2;

        public void DoWindowContents(Rect rect)
        {
            Listing_Standard options = new Listing_Standard();
            Color defaultColor = new Color(100, 65, 164);
            options.Begin(rect);

            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleCenter;
            GUI.color = Color.magenta;
            options.Label("TwitchToolkitSettingsTitle".Translate());
            GUI.color = defaultColor;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
            options.GapLine();
            options.Gap();

            options.ColumnWidth = (rect.width / 3) - 20f;

            // Left column
            if (options.ButtonText("TwitchToolkitChat".Translate()))
                currentTab = SettingsTab.Chat;

            if (options.ButtonText("TwitchToolkitCoins".Translate()))
                currentTab = SettingsTab.Coins;

            if (options.ButtonText("Storyteller"))
                currentTab = SettingsTab.Storyteller;

            if (options.ButtonText("Patches"))
                currentTab = SettingsTab.Patches;

            options.Gap();

            if (options.ButtonText("Wiki"))
                Application.OpenURL("https://github.com/hodldeeznuts/twitchtoolkit/wiki");

            // Middle column
            options.NewColumn();
            options.Gap(53f);

            if (options.ButtonText("TwitchToolkitStore".Translate()))
                currentTab = SettingsTab.Store;

            if (options.ButtonText("TwitchToolkitKarma".Translate()))
                currentTab = SettingsTab.Karma;

            if (options.ButtonText(""))
                currentTab = SettingsTab.Chat;

            if (options.ButtonText("TwitchToolkitCooldowns".Translate()))
                currentTab = SettingsTab.Cooldowns;

            // Right Column
            options.NewColumn();
            options.Gap(53f);

            if (options.ButtonText(""))
                currentTab = SettingsTab.Options;

            if (options.ButtonText("TwitchToolkitViewers".Translate()))
                currentTab = SettingsTab.Viewers;
            
            if (options.ButtonText(""))
                currentTab = SettingsTab.Integrations;

            if (options.ButtonText(""))
                currentTab = SettingsTab.Votes;

            options.End();

            Listing_Standard gapline = new Listing_Standard();
            Rect gapliRect = new Rect(rect.x, rect.y, rect.width, rect.height);
            gapline.Begin(gapliRect);
            gapline.Gap();
            gapline.End();
        
            Rect optionsRect = rect;
            optionsRect.y = 145;
            optionsRect.height = 620f;
            optionsRect.yMax = 765f;

            Rect scrollViewer = new Rect(optionsRect);
            scrollViewer.height -= 145f;
            scrollViewer.yMax -= 145f;
            
            Rect viewRect = new Rect(0, 0, rect.width - 125f, 430f);

            if (currentTab == SettingsTab.Chat) viewRect.height += 400f;
            if (currentTab == SettingsTab.Storyteller) viewRect.height += 400f;
            if (currentTab == SettingsTab.Karma) viewRect.height += 250f;
            if (currentTab == SettingsTab.Viewers) viewRect.height += 120f;
            
            Listing_Standard optionsListing = new Listing_Standard();

            optionsListing.Begin(optionsRect);
            optionsListing.BeginScrollView(scrollViewer, ref scrollVector2, ref viewRect);

            switch(currentTab)
            {
                case SettingsTab.Coins:
                    Settings_Coins.DoWindowContents(viewRect, optionsListing);
                    break;
                case SettingsTab.Storyteller:
                    Settings_Storyteller.DoWindowContents(viewRect, optionsListing);
                    break;
                case SettingsTab.Patches:
                    Settings_Patches.DoWindowContents(viewRect, optionsListing);
                    break;
                case SettingsTab.Store:
                    Settings_Store.DoWindowContents(viewRect, optionsListing);
                    break;
                case SettingsTab.Karma:
                    Settings_Karma.DoWindowContents(viewRect, optionsListing);
                    break;
                //case SettingsTab.Commands:
                //    Settings_Commands.DoWindowContents(viewRect, optionsListing);
                //    break;
                case SettingsTab.Cooldowns:
                    Settings_Cooldowns.DoWindowContents(viewRect, optionsListing);
                    break;
                //case SettingsTab.Options:
                //    Settings_Options.DoWindowContents(viewRect, optionsListing);
                //    break;
                case SettingsTab.Viewers:
                    Settings_Viewers.DoWindowContents(viewRect, optionsListing);
                    break;
                //case SettingsTab.Integrations:
                //    Settings_Integrations.DoWindowContents(viewRect, optionsListing);
                //    break;
                //case SettingsTab.Votes:
                //    Settings_VoteWeights.DoWindowContents(viewRect, optionsListing);
                //    break;
                default:
                    Settings_Chat.DoWindowContents(viewRect, optionsListing);
                    break;
            }

            optionsListing.EndScrollView(ref viewRect);
            optionsListing.End();         
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref FirstTimeInstallation, "FirstTimeInstallation", true);

            Scribe_Values.Look(ref Channel, "Channel", "");
            Scribe_Values.Look(ref Username, "Username", "");
            Scribe_Values.Look(ref OAuth, "OAuth", "");
            Scribe_Values.Look(ref AutoConnect, "AutoConnect", true);

            Scribe_Values.Look(ref UseSeparateChatRoom, "UseSeparateChatRoom", false);
            Scribe_Values.Look(ref AllowBothChatRooms, "AllowBothChatRooms", false);
            Scribe_Values.Look(ref WhispersGoToChatRoom, "WhispersGoToChatRoom", true);
            Scribe_Values.Look(ref ChannelID, "ChannelID", "");
            Scribe_Values.Look(ref ChatroomUUID, "ChatroomUUID", "");

            Scribe_Values.Look(ref VoteTime, "VoteTime", 2);
            Scribe_Values.Look(ref VoteOptions, "VoteOptions", 3);
            Scribe_Values.Look(ref TimedStorytelling, "TimedStorytelling", false);
            Scribe_Values.Look(ref TimeBetweenStorytellingEvents, "TimeBetweenStorytellingEvents", 10);
            Scribe_Values.Look(ref VotingChatMsgs, "VotingChatMsgs", false);
            Scribe_Values.Look(ref VotingWindow, "VotingWindow", true);
            Scribe_Values.Look(ref VotingWindowx, "VotingWindowx", -1);
            Scribe_Values.Look(ref VotingWindowy, "VotingWindowy", -1);
            Scribe_Values.Look(ref LargeVotingWindow, "LargeVotingWindow", true);

            Scribe_Values.Look(ref StartingBalance, "StartingBalance", 150);
            Scribe_Values.Look(ref CoinInterval, "CoinInterval", 2);
            Scribe_Values.Look(ref CoinAmount, "CoinAmount", 30);
            Scribe_Values.Look(ref MinimumPurchasePrice, "MinimumPurchasePrice", 60);
            Scribe_Values.Look(ref UnlimitedCoins, "UnlimitedCoins", false);

            Scribe_Values.Look(ref EarningCoins, "EarningCoins", true);
            Scribe_Values.Look(ref CustomPricingSheetLink, "CustomPricingSheetLink", "bit.ly/toolkit-list");

            Scribe_Values.Look(ref WhisperCmdsAllowed, "WhisperCmdsAllowed", true);
            Scribe_Values.Look(ref WhisperCmdsOnly, "WhisperCmdsOnly", false);
            Scribe_Values.Look(ref PurchaseConfirmations, "PurchaseConfirmations", true);
            Scribe_Values.Look(ref RepeatViewerNames, "RepeatViewerNames", false);
            Scribe_Values.Look(ref MinifiableBuildings, "MinifiableBuildings", false);

            Scribe_Values.Look(ref SyncStreamElements, "SyncStreamElements", false);
            Scribe_Values.Look(ref AccountID, "AccountID", "");
            Scribe_Values.Look(ref JWTToken, "JWTToken", "");

            Scribe_Values.Look(ref SyncStreamLabs, "SyncStreamLabs", false);

            Scribe_Values.Look(ref StartingKarma, "StartingKarma", 100);
            Scribe_Values.Look(ref KarmaCap, "KarmaCap", 140);
            Scribe_Values.Look(ref BanViewersWhoPurchaseAlwaysBad, "BanViewersWhoPurchaseAlwaysBad", true);
            Scribe_Values.Look(ref KarmaReqsForGifting, "KarmaReqsForGifting", false);
            Scribe_Values.Look(ref MinimumKarmaToRecieveGifts, "MinimumKarmaToRecieveGifts", 33);
            Scribe_Values.Look(ref MinimumKarmaToSendGifts, "MinimumKarmaToSendGifts", 100);
            Scribe_Values.Look(ref KarmaMinimum, "KarmaMinimum", 10);

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

            Scribe_Values.Look(ref ChatReqsForCoins, "ChatReqsForCoins", true);
            Scribe_Values.Look(ref TimeBeforeHalfCoins, "TimeBeforeHalfCoins", 30);
            Scribe_Values.Look(ref TimeBeforeNoCoins, "TimeBeforeNoCoins", 60);
            
            Scribe_Values.Look(ref KarmaDecay, "KarmaDecay", false);
            Scribe_Values.Look(ref KarmaDecayPeriod, "KarmaDecayPeriod", 5);
            Scribe_Values.Look(ref MaxBadEventsBeforeDecay, "MaxBadEventsBeforeDecay", 0);
            Scribe_Values.Look(ref MaxGoodEventsBeforeDecay, "MaxGoodEventsBeforeDecay", 0);
            Scribe_Values.Look(ref MaxNeutralEventsBeforeDecay, "MaxNeutralEventsBeforeDecay", 0);

            Scribe_Values.Look(ref MaxEvents, "MaxEvents", false);
            Scribe_Values.Look(ref MaxEventsPeriod, "MaxEventsPeriod", 5);
            Scribe_Values.Look(ref MaxBadEventsPerInterval, "MaxBadEventsPerInterval", 3);
            Scribe_Values.Look(ref MaxGoodEventsPerInterval, "MaxGoodEventsPerInterval", 10);
            Scribe_Values.Look(ref MaxNeutralEventsPerInterval, "MaxNeutralEventsPerInterval", 10);
            Scribe_Values.Look(ref MaxCarePackagesPerInterval, "MaxCarePackagesPerInterval", 10);

            Scribe_Values.Look(ref EventsHaveCooldowns, "EventsHaveCooldowns", true);
            Scribe_Values.Look(ref EventCooldownInterval, "EventCooldownInterval", 15);

            Scribe_Collections.Look(ref ViewerColorCodes, "ViewerColorCodes", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerModerators, "ViewerModerators", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref BannedViewers, "BannedViewers", LookMode.Value);

            Scribe_Values.Look(ref EnableViewerQueue, "EnableViewerQueue", true);
            Scribe_Values.Look(ref ViewerNamedColonistQueue, "ViewerNamedColonistQueue", true);
            Scribe_Values.Look(ref ChargeViewersForQueue, "ChargeViewersForQueue", false);
            Scribe_Values.Look(ref CostToJoinQueue, "CostToJoinQueue", 0);

            Scribe_Values.Look(ref SubscriberExtraCoins, "SubscriberExtraCoins", 10);
            Scribe_Values.Look(ref SubscriberCoinMultiplier, "SubscriberCoinMultiplier", 1.25f);
            Scribe_Values.Look(ref SubscriberExtraVotes, "SubscriberExtraVotes", 1);

            Scribe_Values.Look(ref VIPExtraCoins, "VIPExtraCoins", 5);
            Scribe_Values.Look(ref VIPCoinMultiplier, "VIPCoinMultiplier", 1.15f);
            Scribe_Values.Look(ref VIPExtraVotes, "VIPExtraVotes", 0);

            Scribe_Values.Look(ref ModExtraCoins, "ModExtraCoins", 3);
            Scribe_Values.Look(ref ModCoinMultiplier, "ModCoinMultiplier", 1.15f);
            Scribe_Values.Look(ref ModExtraVotes, "ModExtraVotes", 0);

            Scribe_Collections.Look(ref VoteWeights, "VoteWeights", LookMode.Value, LookMode.Value);

            Helper.Log("exposing vote weights");

            Scribe_Values.Look(ref ToryTalkerEnabled, "ToryTalkerEnabled", false);
            Scribe_Values.Look(ref ToryTalkerMTBDays, "ToryTalkerMTBDays", 2);

            Scribe_Values.Look(ref HodlBotEnabled, "HodlBotEnabled", true);
            Scribe_Values.Look(ref HodlBotMTBDays, "HodlBotMTBDays", 1);

            Scribe_Collections.Look(ref VoteTypeWeights, "VoteTypeWeights", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref VoteCategoryWeights, "VoteCategoryWeights", LookMode.Value, LookMode.Value);

            Scribe_Values.Look(ref UristBotEnabled, "UristBotEnabled", false);
            Scribe_Values.Look(ref UristBotMTBDays, "UristBotMTBDays", 6);

            Scribe_Values.Look(ref MilasandraEnabled, "MilasandraEnabled", false);

            Scribe_Values.Look(ref MercuriusEnabled, "MercuriusEnabled", false);

            Scribe_Values.Look(ref CommandsModsEnabled, "CommandsModsEnabled", true);

            Scribe_Collections.Look(ref CustomCommandDefs, "CustomCommandDefs", LookMode.Value);

            List<StoreIncidentVariables> variableIncidents = DefDatabase<StoreIncidentVariables>.AllDefs.Where(s => s.customSettings).ToList();
            
            foreach (StoreIncidentVariables incident in variableIncidents)
            {
                incident.RegisterCustomSettings();
                incident.settings.ExposeData();
            }

            if (BannedViewers == null || BannedViewers.Count < 1)
            {
                BannedViewers = new List<string>(PubliclyKnownBots);
            }

            if (VoteWeights == null || VoteWeights.Count < 1)
            {
                ToolkitSettings.VoteWeights = new Dictionary<string, int>();

                foreach (VotingIncident vote in DefDatabase<VotingIncident>.AllDefs)
                {
                    ToolkitSettings.VoteWeights.Add(vote.defName, 100);
                }
            }
            else
            {
                Settings_VoteWeights.Load_Votewieghts();
            }
        }

        public static SettingsTab currentTab = SettingsTab.Chat;

        public enum SettingsTab
        {
            Chat,
            Coins,
            Storyteller,
            Patches,
            Store,
            Karma,
            Commands,
            Cooldowns,
            Options,
            Viewers,
            Integrations,
            Votes
        }

        static string[] PubliclyKnownBots = new string[69] {
            "0_applebadapple_0",
            "activeenergy",
            "Anotherttvviewer",
            "apricotdrupefruit",
            "avocadobadado",
            "bananennanen",
            "benutzer",
            "BloodLustr",
            "Chloescookieworld",
            "cleverusernameduh",
            "cogwhistle",
            "commanderroot",
            "commanderrott",
            "communityshowcase",
            "cutehealgirl",
            "danCry",
            "decafsmurf",
            "djcozby",
            "dosrev",
            "electricallongboard",
            "electricalskateboard",
            "faegwent",
            "freast",
            "freddyybot",
            "himekoelectric",
            "host_giveaway",
            "hostgiveaway",
            "jade_elephant_association",
            "laf21",
            "lanfusion",
            "llorx_falso",
            "luki4fun_bot_master",
            "M0psy",
            "mattmongaming",
            "mwmwmwmwmwmwmwmmwmwmwmwmw",
            "n0tahacker",
            "n0tahacker_",
            "n3td3v",
            "norkdorf",
            "nosebleedgg",
            "not47y",
            "ogqp",
            "p0sitivitybot",
            "philderbeast",
            "royalestreamers",
            "shoutgamers",
            "sickfold",
            "skinnyseahorse",
            "SkumShop",
            "slocool",
            "smallstreamersconnect",
            "spectre_807",
            "Stay_hydrated_bot",
            "Stockholm_Sweden",
            "StreamElixir",
            "StreamPromoteBot",
            "Subcentraldotnet",
            "Texastryhard",
            "teyd",
            "thatsprettyokay",
            "thelurkertv",
            "thronezilla",
            "tj_target",
            "twitchprimereminder",
            "uehebot",
            "v_and_k",
            "virgoproz",
            "woppes",
            "zanekyber"
        };
    }
}
