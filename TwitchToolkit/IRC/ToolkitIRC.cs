using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Store;
using TwitchToolkit.Utilities;
using TwitchToolkit.Votes;

namespace TwitchToolkit.IRC
{
    public class ToolkitIRC
    {
        public ToolkitIRC()
        {
            if (ToolkitSettings.AutoConnect)
                Connect();
        }

        public void Connect()
        {
            client = new IRCClient(_ircHost, _ircPort, ToolkitSettings.Username, ToolkitSettings.OAuth, ToolkitSettings.Channel.ToLower());
            client.OnPrivMsg += OnPrivMsg;
            client.Connect();
        }

        public void Disconnect()
        {
            if (client != null)
                client.Disconnect();
        }

        public void Reconnect()
        {
            if (client != null)
                client.Reconnect();
        }

        void OnPrivMsg(string channel, string user, string message)
        {
            if (activeChatWindow != null && !message.StartsWith("!") && user != ToolkitSettings.Username)
            {
                if ((_voteActive && !int.TryParse(message, out int result)) || !_voteActive)
                {
                    string colorcode = Viewer.GetViewerColorCode(user);
                    activeChatWindow.AddMessage(message, user, colorcode);
                }

            }

            if (Helper.ModActive) Commands.CheckCommand(message, user);

            if (VoteHandler.voteActive && int.TryParse(message[0].ToString(), out int voteKey)) VoteHandler.currentVote.RecordVote(Viewers.GetViewer(user).id, voteKey - 1);
        }

        public string[] MessageLog
        {
            get
            {
                if (client == null)
                    return new string[] { };
                return client.MessageLog;
            }
        }

        public void SendMessage(string message)
        {
            if (client != null)
                client.SendMessage(message);
        }

        public ChatWindow activeChatWindow = null;
        bool _voteActive = false;
        IRCClient client = null;

        static string _ircHost = "irc.twitch.tv";
        static short _ircPort = 443;
    }

}
