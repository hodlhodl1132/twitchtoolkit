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
    public class Window_AddTrait : Window
    {
        public Window_AddTrait()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Add Trait Settings");

            traitsBuffer = AddTraitSettings.maxTraits.ToString();
            listing.TextFieldNumericLabeled<int>("Maximum Traits", ref AddTraitSettings.maxTraits, ref traitsBuffer, 1f, 100f);

            listing.End();
        }

        private string traitsBuffer = "";
    }
}
