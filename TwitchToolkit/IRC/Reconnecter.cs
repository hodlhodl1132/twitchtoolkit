using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using TwitchToolkit.Windows.Installation;
using Verse;

namespace TwitchToolkit.IRC
{
    public class Reconnecter : GameComponent
    {
        public Reconnecter(Game game)
        {
            if (ToolkitSettings.FirstTimeInstallation)
            {
                Window_Install window = new Window_Install();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }
        }

        public override void GameComponentTick()
        {
            if (!autoReconnect || Find.TickManager.TicksGame % reconnectTime != 0)
                return;

            reconnectTime = 2500;

            if (ToolkitSettings.AutoConnect && Toolkit.client == null)
            {
                ToolkitIRC.NewInstance();
            }
            else if (Toolkit.client != null && !Toolkit.client.Connected)
            {
                Helper.Log("Disconnect detected, attempting reconnect");
                ToolkitIRC.NewInstance();
            }
            else if (Ticker.LastIRCPong != 0 && TimeHelper.SecondsElapsed(DateTime.FromFileTime(Ticker.LastIRCPong)) > reconnectInterval )
            {
                Helper.Log($"Has been over {reconnectInterval} seconds since last message from server, reconnecting");
                ToolkitIRC.NewInstance();
            }
        }

        public static bool autoReconnect = true;

        public static int reconnectInterval = 300;

        int reconnectTime = 50;        
    }
}
