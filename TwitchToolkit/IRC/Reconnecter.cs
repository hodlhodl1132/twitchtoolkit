using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using TwitchToolkit.Windows;
using TwitchToolkit.Windows.Installation;
using Verse;

namespace TwitchToolkit.IRC
{
    public class Reconnecter : GameComponent
    {
        public Reconnecter(Game game)
        {
        }

        bool openedInstall = false;

        public override void GameComponentTick()
        {
            if (!Helper.ModActive)
            {
                return;
            }

            if (!openedInstall && ToolkitSettings.FirstTimeInstallation)
            {
                openedInstall = true;
                Window_Install window = new Window_Install();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            if (!autoReconnect || Find.TickManager.TicksGame % reconnectTime != 0)
                return;

            reconnectTime = 2500;

            if (ToolkitSettings.AutoConnect && Toolkit.client == null)
            {
                ToolkitIRC.Reset();
            }
            else if (Toolkit.client != null && !Toolkit.client.Connected)
            {
                Helper.Log("Disconnect detected, attempting reconnect");
                ToolkitIRC.Reset();
            }
            else if (Ticker.LastIRCPong != 0 && TimeHelper.SecondsElapsed(DateTime.FromFileTime(Ticker.LastIRCPong)) > reconnectInterval )
            {
                Helper.Log($"Has been over {reconnectInterval} seconds since last message from server, reconnecting");
                ToolkitIRC.Reset();
            }
        }

        public static bool autoReconnect = true;

        public static int reconnectInterval = 300;

        int reconnectTime = 50;        
    }
}
