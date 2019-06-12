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

            listing.Label("StorytellerPacks are packs of different types of votes. They each have their own settings, can be activated at the same time, and can be activated alongside normal storytellers.");

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

            if (listing.ButtonTextLabeled("ToryTalker Pack", "View"))
            {
                Window_ToryTalkerSettings window = new Window_ToryTalkerSettings();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.Gap(24);

            listing.CheckboxLabeled("<color=#4BB543>HodlBot</color> - Random by Category / Type", ref ToolkitSettings.HodlBotEnabled);

            listing.Gap();

            if (listing.ButtonTextLabeled("HodlBot Pack", "View"))
            {
                Window_HodlBotSettings window = new Window_HodlBotSettings();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.Gap(24);

            listing.CheckboxLabeled("<color=#CF0E0F>UristBot</color> - Raids Strategies / Diseases", ref ToolkitSettings.UristBotEnabled);

            listing.Gap();

            if (listing.ButtonTextLabeled("UristBot Pack", "View"))
            {
                Window_UristBotSettings window = new Window_UristBotSettings();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.Gap(24);

            listing.CheckboxLabeled("<color=#1482CB>Milasandra</color> - Threats OnOffCycle", ref ToolkitSettings.MilasandraEnabled);

            listing.Gap();

            if (listing.ButtonTextLabeled("Milasandra Pack", "View"))
            {
                Window_Milasandra window = new Window_Milasandra();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            listing.End();
        }

        public override Vector2 InitialSize => new Vector2(520f, 800f);
    }
}
