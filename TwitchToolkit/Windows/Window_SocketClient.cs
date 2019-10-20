using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.SocketClient;
using TwitchToolkitDev;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_SocketClient : Window
    {
        public Window_SocketClient()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            if (listing.ButtonTextLabeled("Twitch Extension", "Start"))
            {
                TwitchExtensionClient.Instance.Connect();
            }

            if (TwitchExtensionClient.Instance.Connected &&
                listing.ButtonTextLabeled("Disconnect", "Disconnect"))
            {
                TwitchExtensionClient.Instance.Disconnect();
            }

            listing.End();

            Rect outputBox = new Rect(0f, 100f, inRect.width, 300f);

            if (TwitchExtensionClient.Instance.Connected)
            {
                string[] messages = TwitchExtensionClient.Instance.MessageLog;

                for (int i = 0; i < messages.Length; i++)
                    messages[i] = messages[i].Replace("\n", "");

                Widgets.TextArea(outputBox, string.Join("\r\n", messages), true);
            }
        }

        public override Vector2 InitialSize => new Vector2(800, 800);
    }
}
