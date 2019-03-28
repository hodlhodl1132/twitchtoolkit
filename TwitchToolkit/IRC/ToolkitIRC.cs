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

        void OnPrivMsg(IRCMessage message)
        {
            if (activeChatWindow != null && !message.Message.StartsWith("!") && message.User != ToolkitSettings.Username)
            {
                if ((_voteActive && !int.TryParse(message.Message[0].ToString(), out int result)) || !_voteActive)
                {
                    activeChatWindow.AddMessage(
                        message.Message,
                        message.User,
                        (message.Parameters.ContainsKey("color")) ? message.Parameters["color"].Remove(0, 1) : Viewer.GetViewerColorCode(message.User)
                    );
                }

            }

            if (Helper.ModActive) Commands.CheckCommand(message);

            if (VoteHandler.voteActive && int.TryParse(message.Message[0].ToString(), out int voteKey)) VoteHandler.currentVote.RecordVote(Viewers.GetViewer(message.User).id, voteKey - 1);
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

        public void SendMessage(string message, bool v = false)
        {
            if (client != null)
                client.SendMessage(message, v);
        }

        public ChatWindow activeChatWindow = null;
        bool _voteActive = false;
        public IRCClient client = null;

        static string _ircHost = "irc.twitch.tv";
        static short _ircPort = 443;
    }

}
