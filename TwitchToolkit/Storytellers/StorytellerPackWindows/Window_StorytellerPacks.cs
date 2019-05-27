using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows
{
    public class Window_StorytellerPacks : Window
    {
        public Window_StorytellerPacks()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Font = GameFont.Medium;
            listing.Label("Storyteller Packs");
            Text.Font = GameFont.Small;

            listing.GapLine();

            listing.Label("All storyteller packs will consider the global weights when choosing which votes to use. A weight of 0 would never be picked, meaning it is disabled.");

            listing.Gap();

            if (listing.ButtonTextLabeled("Edit Global Vote Weights", "Edit Weights"))
            {
                Window_GlobalVoteWeights window = new Window_GlobalVoteWeights();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.Gap();

            listing.GapLine();

            listing.Label("Enabled Storyteller Packs");

            listing.Gap(24);
            
            listing.CheckboxLabeled("<color=#6441A4>Torytalker</color> - Classic / Most Balanced", ref ToolkitSettings.ToryTalkerEnabled);

            listing.Gap();

            if (listing.ButtonTextLabeled("Edit ToryTalker Settings", "Settings"))
            {
                Window_ToryTalkerSettings window = new Window_ToryTalkerSettings();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.Gap(24);

            listing.CheckboxLabeled("<color=#4BB543>HodlBot</color> - Random by Category / Type", ref ToolkitSettings.HodlBotEnabled);

            listing.Gap();

            if (listing.ButtonTextLabeled("Edit HodlBot Settings", "Settings"))
            {
                Window_HodlBotSettings window = new Window_HodlBotSettings();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.Gap(24);

            listing.CheckboxLabeled("<color=#CF0E0F>UristBot</color> - Raids Strategies / Diseases", ref ToolkitSettings.UristBotEnabled);

            listing.Gap();

            if (listing.ButtonTextLabeled("Edit UristBot Settings", "Settings"))
            {
                Window_UristBotSettings window = new Window_UristBotSettings();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.End();
        }

        public override Vector2 InitialSize => new Vector2(500f, 560f);
    }
}
