using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class Window_UristBotSettings : Window
    {
        public Window_UristBotSettings()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Font = GameFont.Medium;
            listing.Label("<color=#CF0E0F>UristBot</color> Settings");
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.GapLine();

            listing.Label("UristBot is still being developed. At the moment, it will make a small raid and let the viewers choose the raid strategy.");

            listing.Gap();

            string uristbotMTBDays = Math.Truncate(((double)ToolkitSettings.UristBotMTBDays * 100) / 100).ToString();
            listing.TextFieldNumericLabeled<float>("Average Days Between Events", ref ToolkitSettings.UristBotMTBDays, ref uristbotMTBDays, 0.5f, 10f);

            listing.End();
        }
    }
}
