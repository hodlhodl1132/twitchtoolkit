using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit.SocketClient
{
    public sealed class TwitchClient : Client
    {
        static string server = "irc.twitch.tv";
        static int port = 443;

        public TwitchClient() : base(server, port)
        {

        }

        public override void PostAuthenticate()
        {
            string pass = ToolkitSettings.OAuth;
            if (!pass.StartsWith("oauth:", StringComparison.InvariantCultureIgnoreCase))
            {
                pass = "oauth:" + pass;
            }

            Log.Warning("PASS " + pass + "\nNICK " + ToolkitSettings.Username.ToLower() + "\n");

            Send("PASS " + pass + "\nNICK " + ToolkitSettings.Username.ToLower() + "\n");
        }

        public override void ParseMessage(string message)
        {
            
        }
    }
}
