using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Coins
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0031: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00db: Unknown result type (might be due to invalid IL or missing erences)
		//IL_010f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_012b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0164: Unknown result type (might be due to invalid IL or missing erences)
		optionsListing.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitStartingBalance")),  ToolkitSettings.StartingBalance, 0.8f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitCoinInterval")),  ToolkitSettings.CoinInterval, Math.Round((float)ToolkitSettings.CoinInterval).ToString(), 1f, 15f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitCoinAmount")),  ToolkitSettings.CoinAmount, Math.Round((float)ToolkitSettings.CoinAmount).ToString());
		optionsListing.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMinimumPurchasePrice")),  ToolkitSettings.MinimumPurchasePrice, 0.8f);
		((Listing)optionsListing).Gap(12f);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitUnlimitedCoins")), ref ToolkitSettings.UnlimitedCoins, (string)null);
		((Listing)optionsListing).GapLine(12f);
		((Listing)optionsListing).Gap(12f);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitChatReqsForCoins")), ref ToolkitSettings.ChatReqsForCoins, (string)null);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitTimeBeforeHalfCoins")),  ToolkitSettings.TimeBeforeHalfCoins, Math.Round((double)ToolkitSettings.TimeBeforeHalfCoins).ToString(), 15f, 120f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitTimeBeforeNoCoins")),  ToolkitSettings.TimeBeforeNoCoins, Math.Round((double)ToolkitSettings.TimeBeforeNoCoins).ToString(), 30f, 240f);
	}
}
