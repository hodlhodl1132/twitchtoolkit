using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
using Verse;

namespace TwitchToolkit.Windows.Installation
{
    public class ChatSettingsTest : WindowContentsDriver
    {
        public override void DoWindowContents(Listing_Standard listing)
        {
            listing.Label("Let's make sure your connection details are correct.");

            if (Toolkit.client != null)
            {
                if (Toolkit.client.Connected)
                {
                    listing.Label("<color=#21D80E>" + "TwitchToolkitConnected".Translate() + "</color>");
                    if (listing.CenteredButton("TwitchToolkitDisconnect".Translate())) Toolkit.client.Disconnect();
                    if (listing.CenteredButton("TwitchToolkitReconnect".Translate())) Toolkit.client.Reconnect();
                }
                else
                {
                    if (listing.CenteredButton("TwitchToolkitConnect".Translate()))
                    {
                        ToolkitIRC.NewInstance();
                    }
                }
            }
            else
            {
                listing.Label("Need new connection");
                if (listing.CenteredButton("TwitchToolkitNewConnection".Translate()))
                {
                    ToolkitIRC.NewInstance();
                }
            }

            listing.Gap();

            if (Toolkit.client != null && Toolkit.client.Connected)
            {
                listing.TextEntry(string.Join("\r\n", Toolkit.client.MessageLog), 6);
            }
        }
    }
}
