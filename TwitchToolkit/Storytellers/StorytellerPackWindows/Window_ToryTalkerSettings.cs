using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows
{
    public class Window_ToryTalkerSettings : Window
    {
        public Window_ToryTalkerSettings()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Font = GameFont.Medium;
            listing.Label("<color=#6441A4>ToryTalker</color> Settings");
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.GapLine();

            listing.Label("Tory Talker uses the global weights and it's own weighting system based on events that have happened recently.");

            listing.Gap();

            if (listing.ButtonTextLabeled("Edit Global Vote Weights", "Edit Weights"))
            {
                Window_GlobalVoteWeights window = new Window_GlobalVoteWeights();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.Gap();

            string toryTalkerMTBDays = Math.Truncate(((double)ToolkitSettings.ToryTalkerMTBDays * 100) / 100).ToString();
            listing.TextFieldNumericLabeled<float>("Average Days Between Events", ref ToolkitSettings.ToryTalkerMTBDays, ref toryTalkerMTBDays, 0.5f, 10f);

            listing.End();
        }
    }
}
