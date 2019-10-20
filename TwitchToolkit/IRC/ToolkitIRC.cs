using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Viewers;
using TwitchToolkit.Store;
using TwitchToolkit.Utilities;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.IRC
{
    public sealed class ToolkitIRC
    {
        private static ToolkitIRC instance = null;
        private static readonly object padlock = new object();

        public static ToolkitIRC Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new ToolkitIRC();
                        }
                    }
                }

                return instance;
            }
        }

        public static void Reset()
        {
            instance = null;

            Toolkit.client = Instance;
        }

        public ToolkitIRC()
        {
            Connect();
        }

        private void Connect()
        {
            StripUsernameAndChannel();

            Helper.Log("creating new connection");

            client = new IRCClient(_ircHost, _ircPort, ToolkitSettings.Username, ToolkitSettings.OAuth, ToolkitSettings.Channel.ToLower());
            client.OnPrivMsg += OnPrivMsg;
            client.Connect();
        }

        public void Disconnect()
        {
            if (client != null)
            {
                Helper.Log("Disconnecting client");
                client.Disconnect();
            }     
        }

        public void Reconnect()
        {
            client.Reconnect();
        }

        void OnPrivMsg(TwitchIRCMessage message)
        {
            //if (activeChatWindow != null && !message.Message.StartsWith("!") && message.User != ToolkitSettings.Username)
            //{
            //    if ((_voteActive && !int.TryParse(message.Message[0].ToString(), out int result)) || !_voteActive)
            //    {
            //        activeChatWindow.AddMessage(
            //            message.Message,
            //            message.User,
            //            (message.Parameters.ContainsKey("color")) ? message.Parameters["color"].Remove(0, 1) : Viewer.GetViewerColorCode(message.User)
            //        );
            //    }

            //}

            message.Viewer = TwitchViewer.GetTwitchViewer(message.User);

            if (message.Viewer == null)
            {
                Helper.Log(message.User.CapitalizeFirst() + " tried to run a command but is not registed.");
                return;
            }

            if (Helper.ModActive) CommandsHandler.CheckCommand(message);

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
            Helper.Log($"message: {message} bool: {v}");
            
            if (client == null)
            {
                Log.Error("Client null");
                return;
            }

            if (!client.Connected)
            {
                Log.Error("Internal client is disconnected");
            }

            client.SendMessage(message, v);
        }

        public bool Connected
        {
            get
            {
                return client.Connected;
            }
        }

        private void StripUsernameAndChannel()
        {
            ToolkitSettings.Channel = ToolkitSettings.Channel.Replace("https://www.twitch.tv/", "");
            ToolkitSettings.Channel = ToolkitSettings.Channel.Replace("www.twitch.tv/", "");
            ToolkitSettings.Channel = ToolkitSettings.Channel.Replace("twitch.tv/", "");

            ToolkitSettings.Username =  ToolkitSettings.Username.Replace("https://www.twitch.tv/", "");
            ToolkitSettings.Username = ToolkitSettings.Username.Replace("www.twitch.tv/", "");
            ToolkitSettings.Username = ToolkitSettings.Username.Replace("twitch.tv/", "");
        }

        public Window_ChatBoxSetup activeChatWindow = null;
        private IRCClient client = null;

        static string _ircHost = "irc.twitch.tv";
        static short _ircPort = 443;
    }

}
