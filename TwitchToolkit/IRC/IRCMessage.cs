using System.Collections.Generic;

namespace TwitchToolkit.IRC
{
    public delegate void OnMessage(IRCMessage message);

    public class IRCMessage
    {
        public string User = "";
        public string Host = "";
        public string Cmd = "";
        public string Args = "";
        public string Channel = "";
        public string Message = "";
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
    }
}
