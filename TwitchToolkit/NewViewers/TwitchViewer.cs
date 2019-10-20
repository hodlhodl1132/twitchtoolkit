using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Viewers
{
    internal class TwitchViewer : Viewer
    {
        int twitchId = 0;

        static List<string> viewersBeingRegistered = new List<string>();

        static void RegisterTwitchUser(string username)
        {
            // Prevent viewers from being registered multiple times
            
            if (viewersBeingRegistered.Contains(username.ToLower()))
            {
                return;
            }

            viewersBeingRegistered.Add(username.ToLower());

            new WebClientHelper().Get("https://twitch.honest.chat/list-chat-rooms/json-request.php?username=" + ToolkitSettings.Username.ToLower(),
                    new DownloadStringCompletedEventHandler(RegisterTwitchViewerCallback)
                );
        }

        static void RegisterTwitchViewerCallback(object sender, DownloadStringCompletedEventArgs eventArgs)
        {
            if (eventArgs.Error != null)
                throw new Exception("Viewers twitch id could not be retrieved");

            if (eventArgs.Result != null)
                Helper.Log("Twitch Viewer Info: " + eventArgs.Result);

            JSONNode resultNode = JSON.Parse(eventArgs.Result);

            if (resultNode != null)
            {
                int channel_id = resultNode["channel_id"].AsInt;
                string username = resultNode["user_name"];

                TwitchViewer newViewer = new TwitchViewer(channel_id, username);

                if (viewersBeingRegistered.Contains(newViewer.Username.ToLower()))
                {
                    viewersBeingRegistered.Remove(newViewer.Username.ToLower());
                }
                else
                {
                    throw new Exception(newViewer.Username.CapitalizeFirst() + " was registered as a viewer but was not in the viewers being registered list");
                }

                Helper.Log(newViewer.Username.CapitalizeFirst() + " has been registed as a twitch viewer");
            }
        }

        public TwitchViewer(int twitchId, string username) : base("TWITCH" + twitchId + username, username)
        {
            if (twitchId == 0)
            {
                throw new Exception("Twitch Viewer cannot have id of 0");
            }

            this.twitchId = twitchId;
            this.ViewerType = ViewerType.Twitch;
        }

        public int TwitchId => twitchId;

        public static List<TwitchViewer> AllTwitchViewers
        {
            get
            {
                return Viewers.All.Where(s => s is TwitchViewer).Cast<TwitchViewer>().ToList();
            }
        }

        public static TwitchViewer GetTwitchViewer(int twitchId, string username = "")
        {
            TwitchViewer findViewer = AllTwitchViewers.Find(s => s.TwitchId == twitchId);

            if (findViewer == null && username != "")
            {
                if (username == "")
                    throw new Exception("Twitch Viewer not found with id and no username provided to create new Twitch Viewer");

                return new TwitchViewer(twitchId, username);
            }

            return findViewer;
        }

        public static TwitchViewer GetTwitchViewer(string username)
        {
            TwitchViewer viewer = AllTwitchViewers.Find(s => s.Username == username);

            if (viewer == null)
            {
                RegisterTwitchUser(username);
            }

            return viewer;
        }

        public new static Viewer GetViewer(string username)
        {
            return GetTwitchViewer(username);
        }
    }
}
