using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers
{
    public class Window_Mercurius : Window
    {
        public Window_Mercurius()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            Text.Font = GameFont.Medium;
            listing.Label("<color=#BF0030>Mercurius</color> Settings");
            Text.Font = GameFont.Small;

            listing.Gap();

            listing.GapLine();

            listing.Label("Mercurius generates events in intervals through a cycle generator.");

            listing.Gap();

            listing.Label("You will increasingly get more events the more days that pass.");            

            listing.End();
        }

        public override Vector2 InitialSize => new Vector2(500f, 700f);
    }
}
