using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_Notification : Window
    {
        public Window_Notification(string message)
        {
            this.message = message;
            this.timePosted = DateTime.Now;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (TimeHelper.SecondsElapsed(timePosted) > seconds) Close();
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);
            listing.Label(message);
            listing.End();
        }

        public override Vector2 InitialSize => new Vector2(500f, 500f);

        DateTime timePosted;
        public int seconds = 5;
        string message = "";
    }
}
