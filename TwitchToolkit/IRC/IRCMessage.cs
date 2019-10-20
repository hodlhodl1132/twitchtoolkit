using System.Collections.Generic;
using TwitchToolkit.Viewers;
using Verse;

namespace TwitchToolkit.IRC
{
    public delegate void OnMessage(TwitchIRCMessage message);

    public class TwitchIRCMessage
    {
        public string User = "";
        public Viewer Viewer = null;
        public string Host = "";
        public string Cmd = "";
        public string Args = "";
        public string Channel = "";
        public string Message = "";
        public bool Whisper = false;
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        string chatboxStringCached = "";

        public string ChatBoxStringCached
        {
            get
            {
                if (chatboxStringCached == "")
                {
                    chatboxStringCached = "<color=#" + Viewers.Viewer.GetViewerColorCode(User) + ">" + User.CapitalizeFirst() + "</color>: " + Message;
                }

                return chatboxStringCached;
            }
        }
    }
}
