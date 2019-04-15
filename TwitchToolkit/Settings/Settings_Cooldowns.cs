using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Cooldowns
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.SliderLabeled("Days per cooldown period", ref ToolkitSettings.EventCooldownInterval, Math.Round((double)ToolkitSettings.EventCooldownInterval).ToString(), 1f, 15f);

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("TwitchToolkitMaxEventsLimit".Translate(), ref ToolkitSettings.MaxEvents);

            optionsListing.Gap();

            optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxEventsPeriod".Translate(), ref ToolkitSettings.MaxEventsPeriod, 0.8f);
            optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxBadEvents".Translate(), ref ToolkitSettings.MaxBadEventsPerInterval, 0.8f);
            optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxGoodEvents".Translate(), ref ToolkitSettings.MaxGoodEventsPerInterval, 0.8f);
            optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxNeutralEvents".Translate(), ref ToolkitSettings.MaxNeutralEventsPerInterval, 0.8f);
            optionsListing.AddLabeledNumericalTextField("TwitchToolkitMaxItemEvents".Translate(), ref ToolkitSettings.MaxCarePackagesPerInterval, 0.8f);

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("TwitchToolkitEventsHaveCooldowns".Translate(), ref ToolkitSettings.EventsHaveCooldowns);
            //optionsListing.SliderLabeled("TwitchToolkitEventCooldownInterval".Translate(), ref ToolkitSettings.EventCooldownInterval, Math.Round((double)ToolkitSettings.EventCooldownInterval).ToString(), 1f, 15f);
        }
    }
}
