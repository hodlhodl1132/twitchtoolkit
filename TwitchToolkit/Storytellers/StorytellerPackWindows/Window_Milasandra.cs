using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows
{
    public class Window_Milasandra : Window
    {
        public Window_Milasandra()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Font = GameFont.Medium;
            listing.Label("<color=#1482CB>Milasandra</color> Settings");
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.GapLine();

            listing.Label("Milasandra uses an on and off cycle to bring votes in waves, similar to Cassandra.");

            listing.Gap();

            listing.Label("This pack is the closest to the base games basic forced raid cycle. You will experience lag generating these votes.");

            listing.Gap();

            listing.Label("There are no settings to change because Milasandra will generate votes on the same timeline as Cassandra.");

            listing.End();
        }

        public override Vector2 InitialSize => new Vector2(500f, 700f);
    }
}
