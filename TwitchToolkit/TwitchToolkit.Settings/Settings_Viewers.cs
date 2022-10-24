using System;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Viewers
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0085: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0139: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing erences)
		optionsListing.CheckboxLabeled("Allow viewers to !joinqueue to join name queue?", ref ToolkitSettings.EnableViewerQueue, (string)null);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitViewerColonistQueue")), ref ToolkitSettings.ViewerNamedColonistQueue, (string)null);
		optionsListing.CheckboxLabeled("Charge viewers to join queue?", ref ToolkitSettings.ChargeViewersForQueue, (string)null);
		optionsListing.AddLabeledNumericalTextField("Cost to join queue:",  ToolkitSettings.CostToJoinQueue, 0.8f);
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		optionsListing.Label("Special Viewers", -1f, (string)null);
		((Listing)optionsListing).Gap(12f);
		optionsListing.Label("<color=#D9BB25>Subscribers</color>", -1f, (string)null);
		string subExtraCoinBuffer = ToolkitSettings.SubscriberExtraCoins.ToString();
		string subCoinMultiplierBuffer = ToolkitSettings.SubscriberCoinMultiplier.ToString();
		string subExtraVotesBuffer = ToolkitSettings.SubscriberExtraVotes.ToString();
		optionsListing.TextFieldNumericLabeled<int>("Extra coins", ref ToolkitSettings.SubscriberExtraCoins, ref subExtraCoinBuffer, 0f, 100f);
		optionsListing.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref ToolkitSettings.SubscriberCoinMultiplier, ref subCoinMultiplierBuffer, 1f, 5f);
		optionsListing.TextFieldNumericLabeled<int>("Extra votes", ref ToolkitSettings.SubscriberExtraVotes, ref subExtraVotesBuffer, 0f, 100f);
		((Listing)optionsListing).Gap(12f);
		optionsListing.Label("<color=#5F49F2>VIPs</color>", -1f, (string)null);
		string vipExtraCoinBuffer = ToolkitSettings.VIPExtraCoins.ToString();
		string vipCoinMultiplierBuffer = ToolkitSettings.VIPCoinMultiplier.ToString();
		string vipExtraVotesBuffer = ToolkitSettings.VIPExtraVotes.ToString();
		optionsListing.TextFieldNumericLabeled<int>("Extra coins", ref ToolkitSettings.VIPExtraCoins, ref vipExtraCoinBuffer, 0f, 100f);
		optionsListing.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref ToolkitSettings.VIPCoinMultiplier, ref vipCoinMultiplierBuffer, 1f, 5f);
		optionsListing.TextFieldNumericLabeled<int>("Extra votes", ref ToolkitSettings.VIPExtraVotes, ref vipExtraVotesBuffer, 0f, 100f);
		((Listing)optionsListing).Gap(12f);
		optionsListing.Label("<color=#238C48>Mods</color>", -1f, (string)null);
		string modExtraCoinBuffer = ToolkitSettings.ModExtraCoins.ToString();
		string modCoinMultiplierBuffer = ToolkitSettings.ModCoinMultiplier.ToString();
		string modExtraVotesBuffer = ToolkitSettings.ModExtraVotes.ToString();
		optionsListing.TextFieldNumericLabeled<int>("Extra coins", ref ToolkitSettings.ModExtraCoins, ref modExtraCoinBuffer, 0f, 100f);
		optionsListing.TextFieldNumericLabeled<float>("Coin bonus multiplier", ref ToolkitSettings.ModCoinMultiplier, ref modCoinMultiplierBuffer, 1f, 5f);
		optionsListing.TextFieldNumericLabeled<int>("Extra votes", ref ToolkitSettings.ModExtraVotes, ref modExtraVotesBuffer, 0f, 100f);
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		if (optionsListing.CenteredButton("Edit Viewers"))
		{
			Type type = typeof(Window_Viewers);
			Find.WindowStack.TryRemove(type, true);
			Window window = (Window)(object)new Window_Viewers();
			Find.WindowStack.Add(window);
		}
	}
}
