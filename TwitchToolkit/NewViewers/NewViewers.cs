using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TwitchToolkit.Utilities;
using TwitchToolkit.Viewers;

namespace TwitchToolkit.Viewers
{
    public static class ViewerModel
    {
        public static Viewer GetViewer(string globalIdentifier)
        {
            return All.Find(s => s.GlobalIdentifier == globalIdentifier);
        }

        public static Viewer GetViewerByTypeAndUsername(string username, ViewerType viewerType)
        {
            return All.Find(s => s.ViewerType == viewerType && s.Username == username);
        }

        // Manage viewers that are active

        public static void RefreshActiveViewers()
        {
            if (state != RefreshViewerState.FINISHED) return;

            // Reset Active Viewer List
            Active = new List<Viewer>();

            // First retrieve active viewers internally
                // hold in temporary variable, so that it can be accessed after web call

            // Add viewers who have been actively chatting as large streams will not list every chatter in the API call
            Active = GetViewersActivelyChatting();

            // Call Twitch API to pick up any viewers who have been lurking
            GetChattersFromTwitch();
        }

        // Web call to Twitch API to get active lurkers/chatters
        static void GetChattersFromTwitch()
        {
            state = RefreshViewerState.WAITING_FOR_TWITCH;

            new WebClientHelper()
                .Get($"https://tmi.twitch.tv/group/user/{ToolkitSettings.Channel.ToLower()}/chatters", new DownloadStringCompletedEventHandler(ParseActiveViewers));
        }

        // Add viewers who have been actively chatting as large streams will not list every chatter in the API call
        static List<Viewer> GetViewersActivelyChatting()
        {
            // Only return viewers that have been active within the TimeBeforeHalfCoins setting

            return All.Where(s => s.MinutesAgoSinceLastAction <= ToolkitSettings.TimeBeforeHalfCoins).ToList();
        }

        // Callback Parse for Twitch API call
        static void ParseActiveViewers(object sender, DownloadStringCompletedEventArgs eventArgs)
        {
            state = RefreshViewerState.PARSING;

            if (eventArgs.Error != null)
                throw new Exception("No chatters received from tmi.twitch.tv API - This can be ignored most of the time");

            if (eventArgs.Result != null)
                Helper.Log("Twitch Active Chatters API: " + eventArgs.Result);

            List<string> viewers = new List<string>();

            // Parse chatters from list

            JSONNode resultNode = JSON.Parse(eventArgs.Result);

            if (resultNode["chatters"] != null)
            {
                JSONNode chatters = resultNode["chatters"];

                string[] viewerTypes = { "broadcaster", "vips", "moderators", "staff", "admins", "global_mods", "viewers" };

                foreach (string type in viewerTypes)
                {
                    for (int i = 0; i < chatters[type].Count; i++)
                        Active.Add(TwitchViewer.GetViewer(chatters[type][i]));
                }
            }

            state = RefreshViewerState.FINISHED;
        }

        public static void AwardViewersCoins()
        {
            if (state != RefreshViewerState.FINISHED) return;

            // Calculate amount and reward each active viewer
            foreach (Viewer viewer in Active)
            {
                int baseCoins = ToolkitSettings.CoinAmount;
                float baseMultiplier = (float)viewer.Karma / 100f;

                if (viewer.Subscriber)
                {
                    baseCoins += ToolkitSettings.SubscriberExtraCoins;
                    baseMultiplier += ToolkitSettings.SubscriberCoinMultiplier;
                }
                else if (viewer.VIP)
                {
                    baseCoins += ToolkitSettings.VIPExtraCoins;
                    baseMultiplier += ToolkitSettings.VIPCoinMultiplier;
                }
                else if (viewer.Mod)
                {
                    baseCoins += ToolkitSettings.ModExtraCoins;
                    baseMultiplier += ToolkitSettings.ModCoinMultiplier;
                }

                // Lets round up so viewers don't get stuck earning less than 1 coin
                double coinsToReward = (double)baseCoins * baseMultiplier;

                viewer.GiveCoins((int)Math.Ceiling(coinsToReward));
            }
        }

        public static void ResetAllViewersCoins()
        {
            foreach (Viewer viewer in All)
                viewer.Coins = ToolkitSettings.StartingBalance;
        }

        public static void ResetAllViewersKarma()
        {
            foreach (Viewer viewer in All)
                viewer.Karma = ToolkitSettings.StartingKarma;
        }

        public static void ResetAllViewers()
        {
            foreach (Viewer viewer in All)
            {
                viewer.Coins = ToolkitSettings.StartingBalance;
                viewer.Karma = ToolkitSettings.StartingKarma;
            }
        }

        static void DestroyAllViewers()
        {
            All = new List<Viewer>();
        }

        public static List<Viewer> All { get; private set; } = new List<Viewer>();

        public static List<Viewer> Active = new List<Viewer>();

        static RefreshViewerState state = RefreshViewerState.FINISHED;

        enum RefreshViewerState
        {
            RESETING,
            WAITING_FOR_TWITCH,
            PARSING,
            FINISHED
        }
    }
}
