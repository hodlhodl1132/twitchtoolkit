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
    public class Window_LevelPawn : Window
    {
        public Window_LevelPawn()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Level Pawn Settings");

            xpBuffer = LevelPawnSettings.xpMultiplier.ToString();
            listing.TextFieldNumericLabeled<float>("XP Multiplier", ref LevelPawnSettings.xpMultiplier, ref xpBuffer, 0.5f, 5f);

            listing.End();
        }

        private string xpBuffer = "";
    }
}
