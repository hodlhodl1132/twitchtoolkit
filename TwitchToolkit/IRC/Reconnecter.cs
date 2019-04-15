using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.IRC
{
    public class Reconnecter : GameComponent
    {
        public Reconnecter(Game game)
        {

        }

        public override void GameComponentTick()
        {
            if (Find.TickManager.TicksGame % 5000 != 0)
                return;

            if (Toolkit.client != null && Toolkit.client.client != null)
            {
                if (!Toolkit.client.client.Connected)
                {
                    Log.Warning("Disconnect deteced, attempting reconnect");
                    Toolkit.client = new ToolkitIRC();
                    Toolkit.client.Connect();
                }
            }
        }

        public bool autoReconnect = false;
    }
}
