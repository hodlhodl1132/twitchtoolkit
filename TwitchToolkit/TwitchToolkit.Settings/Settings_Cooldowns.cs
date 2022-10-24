using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings;

public static class Settings_Cooldowns
{
	public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0094: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00be: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing erences)
		//IL_011e: Unknown result type (might be due to invalid IL or missing erences)
		optionsListing.SliderLabeled("Days per cooldown period",  ToolkitSettings.EventCooldownInterval, Math.Round((double)ToolkitSettings.EventCooldownInterval).ToString(), 1f, 15f);
		((Listing)optionsListing).Gap(12f);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitMaxEventsLimit")), ref ToolkitSettings.MaxEvents, (string)null);
		((Listing)optionsListing).Gap(12f);
		optionsListing.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxBadEvents")),  ToolkitSettings.MaxBadEventsPerInterval, 0.8f);
		optionsListing.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxGoodEvents")),  ToolkitSettings.MaxGoodEventsPerInterval, 0.8f);
		optionsListing.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxNeutralEvents")),  ToolkitSettings.MaxNeutralEventsPerInterval, 0.8f);
		optionsListing.AddLabeledNumericalTextField((TaggedString)(Translator.Translate("TwitchToolkitMaxItemEvents")),  ToolkitSettings.MaxCarePackagesPerInterval, 0.8f);
		((Listing)optionsListing).Gap(12f);
		optionsListing.CheckboxLabeled((TaggedString)(Translator.Translate("TwitchToolkitEventsHaveCooldowns")), ref ToolkitSettings.EventsHaveCooldowns, (string)null);
	}
}
