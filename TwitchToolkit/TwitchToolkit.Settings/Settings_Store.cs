using System;
using TwitchToolkit.Windows;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Store
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0023: Unknown result type (might be due to invalid IL or missing erences)
		//IL_010f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_012b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0147: Unknown result type (might be due to invalid IL or missing erences)
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitEarningCoins")), ref ToolkitSettings.EarningCoins, (string)null);
		optionsListing.AddLabeledTextField((TaggedString)(Translator.Translate("TwitchToolkitCustomPricingLink")),  ToolkitSettings.CustomPricingSheetLink);
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		if (optionsListing.ButtonTextLabeled("Items Edit", "Open"))
		{
			Type type2 = typeof(StoreItemsWindow);
			Find.WindowStack.TryRemove(type2, true);
			Window window2 = (Window)(object)new StoreItemsWindow();
			Find.WindowStack.Add(window2);
		}
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		if (optionsListing.ButtonTextLabeled("Events Edit", "Open"))
		{
			Type type = typeof(StoreIncidentsWindow);
			Find.WindowStack.TryRemove(type, true);
			Window window = (Window)(object)new StoreIncidentsWindow();
			Find.WindowStack.Add(window);
		}
		((Listing)optionsListing).Gap(12f);
		((Listing)optionsListing).GapLine(12f);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitPurchaseConfirmations")), ref ToolkitSettings.PurchaseConfirmations, (string)null);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitRepeatViewerNames")), ref ToolkitSettings.RepeatViewerNames, (string)null);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitMinifiableBuildings")), ref ToolkitSettings.MinifiableBuildings, (string)null);
	}
}
