using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.SocketClient;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_SocketClient : Window
    {
        Client client = null;

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            if (listing.ButtonTextLabeled("Start Socket", "Start"))
            {
                client = new Client("localhost", 8081);
            }

            listing.End();
        }
    }
}
