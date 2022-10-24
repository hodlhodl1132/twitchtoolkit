using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using TwitchToolkit.Incidents;
using TwitchToolkit.Settings;
using TwitchToolkit.Storytellers.StorytellerPackWindows;
using TwitchToolkit.Votes;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;
using System.Reflection;

namespace TwitchToolkit;

public class ToolkitSettings : ModSettings
{
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

	public static bool FirstTimeInstallation = true;

	public static string Channel = "";

	public static string Username = "";

	public static string OAuth = "";

	public static bool AutoConnect = true;

	public static bool UseSeparateChatRoom = false;

	public static bool AllowBothChatRooms = false;

	public static bool WhispersGoToChatRoom = true;

	public static string ChannelID = "";

	public static string ChatroomUUID = "";

	public static int VoteTime = 2;

	public static int VoteOptions = 3;

	public static bool TimedStorytelling = false;

	public static int TimeBetweenStorytellingEvents = 10;

	public static bool VotingChatMsgs = false;

	public static bool VotingWindow = true;

	public static float VotingWindowx = -1f;

	public static float VotingWindowy = -1f;

	public static bool LargeVotingWindow = true;

	public static bool CommandsModsEnabled = true;

	public static int StartingBalance = 150;

	public static int CoinInterval = 2;

	public static int CoinAmount = 30;

	public static int MinimumPurchasePrice = 60;

	public static bool UnlimitedCoins = false;

	public static bool EarningCoins = true;

	public static string CustomPricingSheetLink = "bit.ly/toolkit-list";

	public static bool WhisperCmdsAllowed = true;

	public static bool WhisperCmdsOnly = false;

	public static bool PurchaseConfirmations = true;

	public static bool RepeatViewerNames = false;

	public static bool MinifiableBuildings = false;

	public static bool SyncStreamElements = false;

	public static string AccountID = "";

	public static string JWTToken = "";

	public static bool SyncStreamLabs = false;

	public static int StartingKarma = 100;

	public static int KarmaCap = 140;

	public static bool BanViewersWhoPurchaseAlwaysBad = true;

	public static bool KarmaReqsForGifting = false;

	public static int MinimumKarmaToRecieveGifts = 33;

	public static int MinimumKarmaToSendGifts = 100;

	public static int KarmaMinimum = 10;

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

	public static bool ChatReqsForCoins = true;

	public static int TimeBeforeHalfCoins = 30;

	public static int TimeBeforeNoCoins = 60;

	public static bool KarmaDecay = false;

	public static int KarmaDecayPeriod = 5;

	public static int MaxBadEventsBeforeDecay = 0;

	public static int MaxGoodEventsBeforeDecay = 0;

	public static int MaxNeutralEventsBeforeDecay = 0;

	public static bool MaxEvents = false;

	public static int MaxEventsPeriod = 5;

	public static int MaxBadEventsPerInterval = 3;

	public static int MaxGoodEventsPerInterval = 10;

	public static int MaxNeutralEventsPerInterval = 10;

	public static int MaxCarePackagesPerInterval = 10;

	public static bool EventsHaveCooldowns = true;

	public static int EventCooldownInterval = 15;

	public static Dictionary<string, string> ViewerColorCodes = new Dictionary<string, string>();

	public static Dictionary<string, bool> ViewerModerators = new Dictionary<string, bool>();

	public static List<string> BannedViewers = new List<string>();

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

	public static Dictionary<string, int> VoteWeights = new Dictionary<string, int>();

	public static bool HodlBotEnabled = true;

	public static float HodlBotMTBDays = 1f;

	public static Dictionary<string, float> VoteTypeWeights = new Dictionary<string, float>();

	public static Dictionary<string, float> VoteCategoryWeights = new Dictionary<string, float>();

	public static bool ToryTalkerEnabled = false;

	public static float ToryTalkerMTBDays = 2f;

	public static bool UristBotEnabled = false;

	public static float UristBotMTBDays = 6f;

	public static bool MilasandraEnabled = false;

	public static bool MercuriusEnabled = false;

	public static List<string> CustomCommandDefs = new List<string>();

	public static bool NotifiedAboutUtils = false;

	private static Vector2 scrollVector2;

	public static SettingsTab currentTab = SettingsTab.Chat;

	private static string[] PubliclyKnownBots = new string[69]
	{
		"0_applebadapple_0", "activeenergy", "Anotherttvviewer", "apricotdrupefruit", "avocadobadado", "bananennanen", "benutzer", "BloodLustr", "Chloescookieworld", "cleverusernameduh",
		"cogwhistle", "commanderroot", "commanderrott", "communityshowcase", "cutehealgirl", "danCry", "decafsmurf", "djcozby", "dosrev", "electricallongboard",
		"electricalskateboard", "faegwent", "freast", "freddyybot", "himekoelectric", "host_giveaway", "hostgiveaway", "jade_elephant_association", "laf21", "lanfusion",
		"llorx_falso", "luki4fun_bot_master", "M0psy", "mattmongaming", "mwmwmwmwmwmwmwmmwmwmwmwmw", "n0tahacker", "n0tahacker_", "n3td3v", "norkdorf", "nosebleedgg",
		"not47y", "ogqp", "p0sitivitybot", "philderbeast", "royalestreamers", "shoutgamers", "sickfold", "skinnyseahorse", "SkumShop", "slocool",
		"smallstreamersconnect", "spectre_807", "Stay_hydrated_bot", "Stockholm_Sweden", "StreamElixir", "StreamPromoteBot", "Subcentraldotnet", "Texastryhard", "teyd", "thatsprettyokay",
		"thelurkertv", "thronezilla", "tj_target", "twitchprimereminder", "uehebot", "v_and_k", "virgoproz", "woppes", "zanekyber"
	};

	public void DoWindowContents(Rect rect)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard options = new Listing_Standard();
		((Listing)options).Begin(rect);
		Color defaultColor = new Color(100f, 65f, 164f);
		Text.Font =((GameFont)2);
		Text.Anchor =((TextAnchor)4);
		GUI.color = (Color.magenta);
		options.Label(Translator.Translate("TwitchToolkitSettingsTitle"), -1f, (string)null);
		GUI.color =  (defaultColor);
		Text.Font =((GameFont)1);
		Text.Anchor =((TextAnchor)0);
		((Listing)options).End();
		Rect mainRect = new Rect(0f, 40f, ((Rect)(rect)).width, ((Rect)(rect)).height - 80f);
		FillMainRect(mainRect);
	}

	public void FillMainRect(Rect mainRect)
	{
		Rect viewRect = new Rect(0f, 0f, ((Rect)(mainRect)).width - 16f, 2800f);
		Listing_Standard options = new Listing_Standard();
		Widgets.BeginScrollView(mainRect, ref scrollVector2, viewRect, true);
		((Listing)options).Begin(viewRect);
		Text.Font =((GameFont)2);
		options.Label("Coins", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)options).Gap(12f);
		options.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitStartingBalance")),  StartingBalance, 0.8f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitCoinInterval")),  CoinInterval, Math.Round((float)CoinInterval).ToString(), 1f, 15f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitCoinAmount")),  CoinAmount, Math.Round((float)CoinAmount).ToString());
		options.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMinimumPurchasePrice")),  MinimumPurchasePrice, 0.8f);
		((Listing)options).Gap(12f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitUnlimitedCoins")), ref UnlimitedCoins, (string)null);
		((Listing)options).GapLine(12f);
		((Listing)options).Gap(12f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitChatReqsForCoins")), ref ChatReqsForCoins, (string)null);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitTimeBeforeHalfCoins")),  TimeBeforeHalfCoins, Math.Round((double)TimeBeforeHalfCoins).ToString(), 15f, 120f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitTimeBeforeNoCoins")),  TimeBeforeNoCoins, Math.Round((double)TimeBeforeNoCoins).ToString(), 30f, 240f);
		((Listing)options).GapLine(12f);
		((Listing)options).Gap(12f);
		Text.Font =((GameFont)2);
		options.Label("Cooldowns", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)options).Gap(12f);
		options.SliderLabeled("Days per cooldown period",  EventCooldownInterval, Math.Round((double)EventCooldownInterval).ToString(), 1f, 15f);
		((Listing)options).Gap(12f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitMaxEventsLimit")), ref MaxEvents, (string)null);
		((Listing)options).Gap(12f);
		options.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxBadEvents")),  MaxBadEventsPerInterval, 0.8f);
		options.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxGoodEvents")),  MaxGoodEventsPerInterval, 0.8f);
		options.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxNeutralEvents")),  MaxNeutralEventsPerInterval, 0.8f);
		options.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxItemEvents")),  MaxCarePackagesPerInterval, 0.8f);
		((Listing)options).Gap(12f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitEventsHaveCooldowns")), ref EventsHaveCooldowns, (string)null);
		((Listing)options).GapLine(12f);
		((Listing)options).Gap(12f);
		Text.Font =((GameFont)2);
		options.Label("Karma", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)options).Gap(12f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitStartingKarma")),  StartingKarma, Math.Round((double)StartingKarma).ToString(), 50f, 250f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitKarmaCap")),  KarmaCap, Math.Round((double)KarmaCap).ToString(), 150f, 600f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitBanViewersWhoAreBad")), ref BanViewersWhoPurchaseAlwaysBad, (string)null);
		((Listing)options).Gap(12f);
		string minKarmaBuffer = KarmaMinimum.ToString();
		options.TextFieldNumericLabeled<int>("What is the minimum amount of karma viewers can reach?", ref KarmaMinimum, ref minKarmaBuffer, -100f, 100f);
		((Listing)options).Gap(12f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitKarmaReqsForGifting")), ref KarmaReqsForGifting, (string)null);
		((Listing)options).Gap(12f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitMinKarmaForGifts")),  MinimumKarmaToRecieveGifts, Math.Round((double)MinimumKarmaToRecieveGifts).ToString(), 10f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitMinKarmaSendGifts")),  MinimumKarmaToSendGifts, Math.Round((double)MinimumKarmaToSendGifts).ToString(), 20f, 150f);
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		options.Label(Translator.Translate("TwitchToolkitGoodViewers"), -1f, (string)null);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  TierOneGoodBonus, Math.Round((double)TierOneGoodBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  TierOneNeutralBonus, Math.Round((double)TierOneNeutralBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  TierOneBadBonus, Math.Round((double)TierOneBadBonus).ToString(), 1f);
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		options.Label(Translator.Translate("TwitchToolkitNeutralViewers"), -1f, (string)null);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  TierTwoGoodBonus, Math.Round((double)TierTwoGoodBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  TierTwoNeutralBonus, Math.Round((double)TierTwoNeutralBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  TierTwoBadBonus, Math.Round((double)TierTwoBadBonus).ToString(), 1f);
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		options.Label(Translator.Translate("TwitchToolkitBadViewers"), -1f, (string)null);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  TierThreeGoodBonus, Math.Round((double)TierThreeGoodBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  TierThreeNeutralBonus, Math.Round((double)TierThreeNeutralBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  TierThreeBadBonus, Math.Round((double)TierThreeBadBonus).ToString(), 1f);
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		options.Label(Translator.Translate("TwitchToolkitDoomViewers"), -1f, (string)null);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  TierFourGoodBonus, Math.Round((double)TierFourGoodBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  TierFourNeutralBonus, Math.Round((double)TierFourNeutralBonus).ToString(), 1f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  TierFourBadBonus, Math.Round((double)TierFourBadBonus).ToString(), 1f);
		((Listing)options).GapLine(12f);
		((Listing)options).Gap(12f);
		Text.Font =((GameFont)2);
		options.Label("Patches", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)options).Gap(12f);
		foreach (ToolkitExtension extension in Settings_ToolkitExtensions.GetExtensions)
		{
			if (options.ButtonTextLabeled(extension.mod.SettingsCategory(), "Settings"))
			{
				ConstructorInfo constructor = extension.windowType.GetConstructor(new Type[1] { typeof(Mod) });
				SettingsWindow window5 = constructor.Invoke(new object[1] { extension.mod }) as SettingsWindow;
				Type type4 = typeof(SettingsWindow);
				Find.WindowStack.TryRemove(type4, true);
				Find.WindowStack.Add((Window)(object)window5);
			}
		}
		((Listing)options).GapLine(12f);
		((Listing)options).Gap(12f);
		Text.Font =((GameFont)2);
		options.Label("Store", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)options).Gap(12f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitEarningCoins")), ref EarningCoins, (string)null);
		options.AddLabeledTextField((TaggedString)(Translator.Translate("TwitchToolkitCustomPricingLink")),  CustomPricingSheetLink);
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		if (options.ButtonTextLabeled("Items Edit", "Open"))
		{
			Type type3 = typeof(StoreItemsWindow);
			Find.WindowStack.TryRemove(type3, true);
			Window window4 = (Window)(object)new StoreItemsWindow();
			Find.WindowStack.Add(window4);
		}
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		if (options.ButtonTextLabeled("Events Edit", "Open"))
		{
			Type type2 = typeof(StoreIncidentsWindow);
			Find.WindowStack.TryRemove(type2, true);
			Window window3 = (Window)(object)new StoreIncidentsWindow();
			Find.WindowStack.Add(window3);
		}
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitPurchaseConfirmations")), ref PurchaseConfirmations, (string)null);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitRepeatViewerNames")), ref RepeatViewerNames, (string)null);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitMinifiableBuildings")),ref  MinifiableBuildings, (string)null);
		((Listing)options).GapLine(12f);
		((Listing)options).Gap(12f);
		Text.Font =((GameFont)2);
		options.Label("Storyteller", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)options).Gap(12f);
		options.Label("All", -1f, (string)null);
		((Listing)options).GapLine(12f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitVoteTime")),  VoteTime, Math.Round((double)VoteTime).ToString(), 1f, 15f);
		options.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitVoteOptions")),  VoteOptions, Math.Round((double)VoteOptions).ToString(), 2f, 5f);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitVotingChatMsgs")), ref VotingChatMsgs, (string)null);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitVotingWindow")), ref VotingWindow, (string)null);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitLargeVotingWindow")), ref LargeVotingWindow, (string)null);
		((Listing)options).Gap(12f);
		if (options.ButtonTextLabeled("Edit Storyteller Packs", "Storyteller Packs"))
		{
			Window_StorytellerPacks window2 = new Window_StorytellerPacks();
			Find.WindowStack.TryRemove(((object)window2).GetType(), true);
			Find.WindowStack.Add((Window)(object)window2);
		}
		((Listing)options).GapLine(12f);
		((Listing)options).Gap(12f);
		Text.Font =((GameFont)2);
		options.Label("Viewers", -1f, (string)null);
		Text.Font =((GameFont)1);
		((Listing)options).Gap(12f);
		options.CheckboxLabeled("Allow viewers to !joinqueue to join name queue?", ref EnableViewerQueue, (string)null);
		options.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitViewerColonistQueue")), ref ViewerNamedColonistQueue, (string)null);
		options.CheckboxLabeled("Charge viewers to join queue?", ref ChargeViewersForQueue, (string)null);
		options.AddLabeledNumericalTextField("Cost to join queue:",  CostToJoinQueue, 0.8f);
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		options.Label("Special Viewers", -1f, (string)null);
		((Listing)options).Gap(12f);
		options.Label("<color=#D9BB25>Subscribers</color>", -1f, (string)null);
		string subExtraCoinBuffer = SubscriberExtraCoins.ToString();
		string subCoinMultiplierBuffer = SubscriberCoinMultiplier.ToString();
		string subExtraVotesBuffer = SubscriberExtraVotes.ToString();
		options.TextFieldNumericLabeled<int>("Extra coins", ref SubscriberExtraCoins, ref subExtraCoinBuffer, 0f, 100f);
		options.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref SubscriberCoinMultiplier, ref subCoinMultiplierBuffer, 1f, 5f);
		options.TextFieldNumericLabeled<int>("Extra votes", ref SubscriberExtraVotes, ref subExtraVotesBuffer, 0f, 100f);
		((Listing)options).Gap(12f);
		options.Label("<color=#5F49F2>VIPs</color>", -1f, (string)null);
		string vipExtraCoinBuffer = VIPExtraCoins.ToString();
		string vipCoinMultiplierBuffer = VIPCoinMultiplier.ToString();
		string vipExtraVotesBuffer = VIPExtraVotes.ToString();
		options.TextFieldNumericLabeled<int>("Extra coins", ref VIPExtraCoins, ref vipExtraCoinBuffer, 0f, 100f);
		options.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref VIPCoinMultiplier, ref vipCoinMultiplierBuffer, 1f, 5f);
		options.TextFieldNumericLabeled<int>("Extra votes",ref  VIPExtraVotes, ref vipExtraVotesBuffer, 0f, 100f);
		((Listing)options).Gap(12f);
		options.Label("<color=#238C48>Mods</color>", -1f, (string)null);
		string modExtraCoinBuffer = ModExtraCoins.ToString();
		string modCoinMultiplierBuffer = ModCoinMultiplier.ToString();
		string modExtraVotesBuffer = ModExtraVotes.ToString();
		options.TextFieldNumericLabeled<int>("Extra coins", ref ModExtraCoins, ref modExtraCoinBuffer, 0f, 100f);
		options.TextFieldNumericLabeled<float>("Coin bonus multiplier",ref  ModCoinMultiplier, ref modCoinMultiplierBuffer, 1f, 5f);
		options.TextFieldNumericLabeled<int>("Extra votes", ref ModExtraVotes, ref modExtraVotesBuffer, 0f, 100f);
		((Listing)options).Gap(12f);
		((Listing)options).GapLine(12f);
		if (options.ButtonText("Edit Viewers", (string)null))
		{
			Type type = typeof(Window_Viewers);
			Find.WindowStack.TryRemove(type, true);
			Window window = (Window)(object)new Window_Viewers();
			Find.WindowStack.Add(window);
		}
		((Listing)options).End();
		Widgets.EndScrollView();
	}

	public override void ExposeData()
	{
		Scribe_Values.Look<bool>(ref FirstTimeInstallation, "FirstTimeInstallation", true, false);
		Scribe_Values.Look<string>(ref Channel, "Channel", "", false);
		Scribe_Values.Look<string>(ref Username, "Username", "", false);
		Scribe_Values.Look<string>(ref OAuth, "OAuth", "", false);
		Scribe_Values.Look<bool>(ref AutoConnect, "AutoConnect", true, false);
		Scribe_Values.Look<bool>(ref UseSeparateChatRoom, "UseSeparateChatRoom", false, false);
		Scribe_Values.Look<bool>(ref AllowBothChatRooms, "AllowBothChatRooms", false, false);
		Scribe_Values.Look<bool>(ref WhispersGoToChatRoom, "WhispersGoToChatRoom", true, false);
		Scribe_Values.Look<string>(ref ChannelID, "ChannelID", "", false);
		Scribe_Values.Look<string>(ref ChatroomUUID, "ChatroomUUID", "", false);
		Scribe_Values.Look<int>(ref VoteTime, "VoteTime", 2, false);
		Scribe_Values.Look<int>(ref VoteOptions, "VoteOptions", 3, false);
		Scribe_Values.Look<bool>(ref TimedStorytelling, "TimedStorytelling", false, false);
		Scribe_Values.Look<int>(ref TimeBetweenStorytellingEvents, "TimeBetweenStorytellingEvents", 10, false);
		Scribe_Values.Look<bool>(ref VotingChatMsgs, "VotingChatMsgs", false, false);
		Scribe_Values.Look<bool>(ref VotingWindow, "VotingWindow", true, false);
		Scribe_Values.Look<float>(ref VotingWindowx, "VotingWindowx", -1f, false);
		Scribe_Values.Look<float>(ref VotingWindowy, "VotingWindowy", -1f, false);
		Scribe_Values.Look<bool>(ref LargeVotingWindow, "LargeVotingWindow", true, false);
		Scribe_Values.Look<int>(ref StartingBalance, "StartingBalance", 150, false);
		Scribe_Values.Look<int>(ref CoinInterval, "CoinInterval", 2, false);
		Scribe_Values.Look<int>(ref CoinAmount, "CoinAmount", 30, false);
		Scribe_Values.Look<int>(ref MinimumPurchasePrice, "MinimumPurchasePrice", 60, false);
		Scribe_Values.Look<bool>(ref UnlimitedCoins, "UnlimitedCoins", false, false);
		Scribe_Values.Look<bool>(ref EarningCoins, "EarningCoins", true, false);
		Scribe_Values.Look<string>(ref CustomPricingSheetLink, "CustomPricingSheetLink", "bit.ly/toolkit-list", false);
		Scribe_Values.Look<bool>(ref WhisperCmdsAllowed, "WhisperCmdsAllowed", true, false);
		Scribe_Values.Look<bool>(ref WhisperCmdsOnly, "WhisperCmdsOnly", false, false);
		Scribe_Values.Look<bool>(ref PurchaseConfirmations, "PurchaseConfirmations", true, false);
		Scribe_Values.Look<bool>(ref RepeatViewerNames, "RepeatViewerNames", false, false);
		Scribe_Values.Look<bool>(ref MinifiableBuildings, "MinifiableBuildings", false, false);
		Scribe_Values.Look<bool>(ref SyncStreamElements, "SyncStreamElements", false, false);
		Scribe_Values.Look<string>(ref AccountID, "AccountID", "", false);
		Scribe_Values.Look<string>(ref JWTToken, "JWTToken", "", false);
		Scribe_Values.Look<bool>(ref SyncStreamLabs, "SyncStreamLabs", false, false);
		Scribe_Values.Look<int>(ref StartingKarma, "StartingKarma", 100, false);
		Scribe_Values.Look<int>(ref KarmaCap, "KarmaCap", 140, false);
		Scribe_Values.Look<bool>(ref BanViewersWhoPurchaseAlwaysBad, "BanViewersWhoPurchaseAlwaysBad", true, false);
		Scribe_Values.Look<bool>(ref KarmaReqsForGifting, "KarmaReqsForGifting", false, false);
		Scribe_Values.Look<int>(ref MinimumKarmaToRecieveGifts, "MinimumKarmaToRecieveGifts", 33, false);
		Scribe_Values.Look<int>(ref MinimumKarmaToSendGifts, "MinimumKarmaToSendGifts", 100, false);
		Scribe_Values.Look<int>(ref KarmaMinimum, "KarmaMinimum", 10, false);
		Scribe_Values.Look<int>(ref TierOneGoodBonus, "TierOneGoodBonus", 16, true);
		Scribe_Values.Look<int>(ref TierOneNeutralBonus, "TierOneNeutralBonus", 36, true);
		Scribe_Values.Look<int>(ref TierOneBadBonus, "TierOneBadBonus", 24, true);
		Scribe_Values.Look<int>(ref TierTwoGoodBonus, "TierTwoGoodBonus", 10, true);
		Scribe_Values.Look<int>(ref TierTwoNeutralBonus, "TierTwoNeutralBonus", 30, true);
		Scribe_Values.Look<int>(ref TierTwoBadBonus, "TierTwoBadBonus", 20, true);
		Scribe_Values.Look<int>(ref TierThreeGoodBonus, "TierThreeGoodBonus", 10, true);
		Scribe_Values.Look<int>(ref TierThreeNeutralBonus, "TierThreeNeutralBonus", 24, true);
		Scribe_Values.Look<int>(ref TierThreeBadBonus, "TierThreeBadBonus", 18, true);
		Scribe_Values.Look<int>(ref TierFourGoodBonus, "TierFourGoodBonus", 6, true);
		Scribe_Values.Look<int>(ref TierFourNeutralBonus, "TierFourNeutralBonus", 18, true);
		Scribe_Values.Look<int>(ref TierFourBadBonus, "TierFourBadBonus", 12, true);
		Scribe_Values.Look<int>(ref DoomBonus, "DoomBonus", 67, true);
		Scribe_Values.Look<bool>(ref ChatReqsForCoins, "ChatReqsForCoins", true, false);
		Scribe_Values.Look<int>(ref TimeBeforeHalfCoins, "TimeBeforeHalfCoins", 30, false);
		Scribe_Values.Look<int>(ref TimeBeforeNoCoins, "TimeBeforeNoCoins", 60, false);
		Scribe_Values.Look<bool>(ref KarmaDecay, "KarmaDecay", false, false);
		Scribe_Values.Look<int>(ref KarmaDecayPeriod, "KarmaDecayPeriod", 5, false);
		Scribe_Values.Look<int>(ref MaxBadEventsBeforeDecay, "MaxBadEventsBeforeDecay", 0, false);
		Scribe_Values.Look<int>(ref MaxGoodEventsBeforeDecay, "MaxGoodEventsBeforeDecay", 0, false);
		Scribe_Values.Look<int>(ref MaxNeutralEventsBeforeDecay, "MaxNeutralEventsBeforeDecay", 0, false);
		Scribe_Values.Look<bool>(ref MaxEvents, "MaxEvents", false, false);
		Scribe_Values.Look<int>(ref MaxEventsPeriod, "MaxEventsPeriod", 5, false);
		Scribe_Values.Look<int>(ref MaxBadEventsPerInterval, "MaxBadEventsPerInterval", 3, false);
		Scribe_Values.Look<int>(ref MaxGoodEventsPerInterval, "MaxGoodEventsPerInterval", 10, false);
		Scribe_Values.Look<int>(ref MaxNeutralEventsPerInterval, "MaxNeutralEventsPerInterval", 10, false);
		Scribe_Values.Look<int>(ref MaxCarePackagesPerInterval, "MaxCarePackagesPerInterval", 10, false);
		Scribe_Values.Look<bool>(ref EventsHaveCooldowns, "EventsHaveCooldowns", true, false);
		Scribe_Values.Look<int>(ref EventCooldownInterval, "EventCooldownInterval", 15, false);
		Scribe_Collections.Look<string, string>(ref ViewerColorCodes, "ViewerColorCodes", (LookMode)1, (LookMode)1);
		Scribe_Collections.Look<string, bool>(ref ViewerModerators, "ViewerModerators", (LookMode)1, (LookMode)1);
		Scribe_Collections.Look<string>(ref BannedViewers, "BannedViewers", (LookMode)1, Array.Empty<object>());
		Scribe_Values.Look<bool>(ref EnableViewerQueue, "EnableViewerQueue", true, false);
		Scribe_Values.Look<bool>(ref ViewerNamedColonistQueue, "ViewerNamedColonistQueue", true, false);
		Scribe_Values.Look<bool>(ref ChargeViewersForQueue, "ChargeViewersForQueue", false, false);
		Scribe_Values.Look<int>(ref CostToJoinQueue, "CostToJoinQueue", 0, false);
		Scribe_Values.Look<int>(ref SubscriberExtraCoins, "SubscriberExtraCoins", 10, false);
		Scribe_Values.Look<float>(ref SubscriberCoinMultiplier, "SubscriberCoinMultiplier", 1.25f, false);
		Scribe_Values.Look<int>(ref SubscriberExtraVotes, "SubscriberExtraVotes", 1, false);
		Scribe_Values.Look<int>(ref VIPExtraCoins, "VIPExtraCoins", 5, false);
		Scribe_Values.Look<float>(ref VIPCoinMultiplier, "VIPCoinMultiplier", 1.15f, false);
		Scribe_Values.Look<int>(ref VIPExtraVotes, "VIPExtraVotes", 0, false);
		Scribe_Values.Look<int>(ref ModExtraCoins, "ModExtraCoins", 3, false);
		Scribe_Values.Look<float>(ref ModCoinMultiplier, "ModCoinMultiplier", 1.15f, false);
		Scribe_Values.Look<int>(ref ModExtraVotes, "ModExtraVotes", 0, false);
		Scribe_Collections.Look<string, int>(ref VoteWeights, "VoteWeights", (LookMode)1, (LookMode)1);
		Helper.Log("exposing vote weights");
		Scribe_Values.Look<bool>(ref ToryTalkerEnabled, "ToryTalkerEnabled", false, false);
		Scribe_Values.Look<float>(ref ToryTalkerMTBDays, "ToryTalkerMTBDays", 2f, false);
		Scribe_Values.Look<bool>(ref HodlBotEnabled, "HodlBotEnabled", true, false);
		Scribe_Values.Look<float>(ref HodlBotMTBDays, "HodlBotMTBDays", 1f, false);
		Scribe_Collections.Look<string, float>(ref VoteTypeWeights, "VoteTypeWeights", (LookMode)1, (LookMode)1);
		Scribe_Collections.Look<string, float>(ref VoteCategoryWeights, "VoteCategoryWeights", (LookMode)1, (LookMode)1);
		Scribe_Values.Look<bool>(ref UristBotEnabled, "UristBotEnabled", false, false);
		Scribe_Values.Look<float>(ref UristBotMTBDays, "UristBotMTBDays", 6f, false);
		Scribe_Values.Look<bool>(ref MilasandraEnabled, "MilasandraEnabled", false, false);
		Scribe_Values.Look<bool>(ref MercuriusEnabled, "MercuriusEnabled", false, false);
		Scribe_Values.Look<bool>(ref CommandsModsEnabled, "CommandsModsEnabled", true, false);
		Scribe_Values.Look<bool>(ref NotifiedAboutUtils, "NotifiedAboutUtils", false, false);
		Scribe_Collections.Look<string>(ref CustomCommandDefs, "CustomCommandDefs", (LookMode)1, Array.Empty<object>());
		List<StoreIncidentVariables> variableIncidents = (from s in DefDatabase<StoreIncidentVariables>.AllDefs
			where s.customSettings
			select s).ToList();
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
			VoteWeights = new Dictionary<string, int>();
			{
				foreach (VotingIncident vote in DefDatabase<VotingIncident>.AllDefs)
				{
					VoteWeights.Add(((Def)vote).defName, 100);
				}
				return;
			}
		}
		Settings_VoteWeights.Load_Votewieghts();
	}
}
