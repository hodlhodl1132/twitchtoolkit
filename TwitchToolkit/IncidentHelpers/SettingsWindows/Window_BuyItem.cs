using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using TwitchToolkit.Incidents;
using UnityEngine;
using Verse;

namespace TwitchToolkit.IncidentHelpers.SettingsWindows
{
    public class Window_BuyItem : Window
    {
        public Window_BuyItem()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Buy Item Settings");

            traitsBuffer = AddTraitSettings.maxTraits.ToString();
            listing.CheckboxLabeled("Should items be researched before being buyable?", ref BuyItemSettings.mustResearchFirst);

            listing.End();
        }

        private string traitsBuffer = "";
    }
}
