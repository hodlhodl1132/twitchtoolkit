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
            optionsListing.CheckboxLabeled("TwitchToolkitMaxEventsLimit".Translate(), ref ToolkitSettings.MaxEvents);
            optionsListing.SliderLabeled("TwitchToolkitMaxEventsPeriod".Translate(), ref ToolkitSettings.MaxEventsPeriod, Math.Round((double)ToolkitSettings.MaxEventsPeriod).ToString(), 1f, 15f);
            optionsListing.SliderLabeled("TwitchToolkitMaxBadEvents".Translate(), ref ToolkitSettings.MaxBadEventsPerInterval, Math.Round((double)ToolkitSettings.MaxBadEventsPerInterval).ToString(), 1f, 15f);
            optionsListing.SliderLabeled("TwitchToolkitMaxGoodEvents".Translate(), ref ToolkitSettings.MaxGoodEventsPerInterval, Math.Round((double)ToolkitSettings.MaxGoodEventsPerInterval).ToString(), 1f, 30f);
            optionsListing.SliderLabeled("TwitchToolkitMaxNeutralEvents".Translate(), ref ToolkitSettings.MaxNeutralEventsPerInterval, Math.Round((double)ToolkitSettings.MaxNeutralEventsPerInterval).ToString(), 1f, 30f);
            optionsListing.SliderLabeled("TwitchToolkitMaxItemEvents".Translate(), ref ToolkitSettings.MaxCarePackagesPerInterval, Math.Round((double)ToolkitSettings.MaxCarePackagesPerInterval).ToString(), 1f, 60f);

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("TwitchToolkitEventsHaveCooldowns".Translate(), ref ToolkitSettings.EventsHaveCooldowns);
            optionsListing.SliderLabeled("TwitchToolkitEventCooldownInterval".Translate(), ref ToolkitSettings.EventCooldownInterval, Math.Round((double)ToolkitSettings.EventCooldownInterval).ToString(), 1f, 15f);
        }
    }
}
