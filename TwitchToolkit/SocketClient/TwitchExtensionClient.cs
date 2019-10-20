using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TwitchToolkit.Viewers;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.SocketClient
{
    public sealed class TwitchExtensionClient : Client
    {
        static string server = "localhost";
        static int port = 8082;

        private static TwitchExtensionClient instance = null;
        private static readonly object padlock = new object();

        public TwitchExtensionClient() : base(server, port)
        {
            Helper.Log("connectioning to socket");
        }

        public static TwitchExtensionClient Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new TwitchExtensionClient();
                        }
                    }
                }

                return instance;
            }
        }

        public override void PostAuthenticate()
        {
            Send($"{{ \"type\": \"authenticate\", \"channel_id\": {ToolkitSettings.ChannelID} }}");
        }

        public override void ParseMessage(string message)
        {
            Helper.Log("socket message: " + message);

            var json = JSON.Parse(message);

            if (json["type"] != null && json["type"] == "message")
            {
                string command = json["message"].ToString().Replace("\"", "");

                switch (command)
                {
                    case "register_viewer":
                        RegisterNewViewer(json["viewer_id"]);
                        break;

                    case "bal_lookup":
                        SendViewerBalance(json["viewer_id"]);
                        break;

                    default:
                        Helper.Log(command);
                        break;
                }
            }
        }

        void RegisterNewViewer(string viewerIdString)
        {
            new WebClientHelper().Get("https://api.twitch.tv/helix/users?id=" + viewerIdString.Replace("U", ""),
                new DownloadStringCompletedEventHandler(RegisterNewViewerCompleted),
                new Dictionary<string, string>
                {
                    { "Client-ID", "cvu7kfdjszvf4xgr6vgierwmw260bi" },
                    { "Accept", "application/vnd.twitchtv.v5+json" }
                }
            );
        }

        void RegisterNewViewerCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Helper.Log("Web Get Request: " + e.Error);
                throw new Exception("Viewer not found on TwitchAPI");
            }

            if (e.Result != null)

            Helper.Log("Web Get Request: " + e.Result);

            var jsonNode = JSON.Parse(e.Result);

            if (jsonNode["data"] != null)
            {
                if (jsonNode["data"][0] != null)
                {
                    int id = jsonNode["data"][0]["id"].AsInt;
                    string username = jsonNode["data"][0]["display_name"];

                    if (TwitchViewer.GetTwitchViewer(id, username) != null)
                    {
                        SendViewerBalance(id.ToString());
                    }
                }
                else
                {
                    Helper.Log("Viewer info not found");
                }
            }            
        }

        void SendViewerBalance(string viewerIdString)
        {
            int viewerId = int.Parse(viewerIdString.Replace("U", ""));

            TwitchViewer twitchViewer = TwitchViewer.GetTwitchViewer(viewerId);

            if (twitchViewer == null)
            {
                throw new Exception("Viewer with id " + viewerId + " not registered");
            }

            StringBuilder balance = new StringBuilder();

            balance.AppendLine("{");
            balance.AppendLine("\"type\": \"command\",");
            balance.AppendLine($"\"id\": {ToolkitSettings.ChannelID},");
            balance.AppendLine("\"message\": ");
            balance.AppendLine(" { ");
            balance.AppendLine($"   \"endpoint\": \"/viewer/{ToolkitSettings.ChannelID}/{viewerId}\",");
            balance.AppendLine($"   \"value\": {twitchViewer.Coins},");
            balance.AppendLine($"   \"key\": \"coin_balance\",");
            balance.AppendLine($"   \"karma\": {twitchViewer.Karma}");
            balance.AppendLine(" }");
            balance.AppendLine("}");

            Send(balance.ToString());
        }
    }
}
