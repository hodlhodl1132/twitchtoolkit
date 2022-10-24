using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Karma
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0040: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0079: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0136: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0187: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0192: Unknown result type (might be due to invalid IL or missing erences)
		//IL_019e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0210: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0261: Unknown result type (might be due to invalid IL or missing erences)
		//IL_026c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0278: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing erences)
		//IL_033b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0346: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0352: Unknown result type (might be due to invalid IL or missing erences)
		//IL_038b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0415: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0420: Unknown result type (might be due to invalid IL or missing erences)
		//IL_042c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0465: Unknown result type (might be due to invalid IL or missing erences)
		//IL_049e: Unknown result type (might be due to invalid IL or missing erences)
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitStartingKarma")),  ToolkitSettings.StartingKarma, Math.Round((double)ToolkitSettings.StartingKarma).ToString(), 50f, 250f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitKarmaCap")),  ToolkitSettings.KarmaCap, Math.Round((double)ToolkitSettings.KarmaCap).ToString(), 150f, 600f);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitBanViewersWhoAreBad")), ref ToolkitSettings.BanViewersWhoPurchaseAlwaysBad, (string)null);
		((Listing)optionsListing).Gap(12f);
		string minKarmaBuffer = ToolkitSettings.KarmaMinimum.ToString();
		optionsListing.TextFieldNumericLabeled<int>("What is the minimum amount of karma viewers can reach?", ref ToolkitSettings.KarmaMinimum, ref minKarmaBuffer, -100f, 100f);
		((Listing)optionsListing).Gap(12f);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitKarmaReqsForGifting")), ref ToolkitSettings.KarmaReqsForGifting, (string)null);
		((Listing)optionsListing).Gap(12f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitMinKarmaForGifts")),  ToolkitSettings.MinimumKarmaToRecieveGifts, Math.Round((double)ToolkitSettings.MinimumKarmaToRecieveGifts).ToString(), 10f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitMinKarmaSendGifts")),  ToolkitSettings.MinimumKarmaToSendGifts, Math.Round((double)ToolkitSettings.MinimumKarmaToSendGifts).ToString(), 20f, 150f);
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		optionsListing.Label(Translator.Translate("TwitchToolkitGoodViewers"), -1f, (string)null);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  ToolkitSettings.TierOneGoodBonus, Math.Round((double)ToolkitSettings.TierOneGoodBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  ToolkitSettings.TierOneNeutralBonus, Math.Round((double)ToolkitSettings.TierOneNeutralBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  ToolkitSettings.TierOneBadBonus, Math.Round((double)ToolkitSettings.TierOneBadBonus).ToString(), 1f);
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		optionsListing.Label(Translator.Translate("TwitchToolkitNeutralViewers"), -1f, (string)null);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  ToolkitSettings.TierTwoGoodBonus, Math.Round((double)ToolkitSettings.TierTwoGoodBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  ToolkitSettings.TierTwoNeutralBonus, Math.Round((double)ToolkitSettings.TierTwoNeutralBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  ToolkitSettings.TierTwoBadBonus, Math.Round((double)ToolkitSettings.TierTwoBadBonus).ToString(), 1f);
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		optionsListing.Label(Translator.Translate("TwitchToolkitBadViewers"), -1f, (string)null);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  ToolkitSettings.TierThreeGoodBonus, Math.Round((double)ToolkitSettings.TierThreeGoodBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  ToolkitSettings.TierThreeNeutralBonus, Math.Round((double)ToolkitSettings.TierThreeNeutralBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  ToolkitSettings.TierThreeBadBonus, Math.Round((double)ToolkitSettings.TierThreeBadBonus).ToString(), 1f);
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		optionsListing.Label(Translator.Translate("TwitchToolkitDoomViewers"), -1f, (string)null);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitGoodKarma")),  ToolkitSettings.TierFourGoodBonus, Math.Round((double)ToolkitSettings.TierFourGoodBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitNeutralKarma")),  ToolkitSettings.TierFourNeutralBonus, Math.Round((double)ToolkitSettings.TierFourNeutralBonus).ToString(), 1f);
		optionsListing.SliderLabeled((TaggedString)(Translator.Translate("TwitchToolkitBadKarma")),  ToolkitSettings.TierFourBadBonus, Math.Round((double)ToolkitSettings.TierFourBadBonus).ToString(), 1f);
	}
}
