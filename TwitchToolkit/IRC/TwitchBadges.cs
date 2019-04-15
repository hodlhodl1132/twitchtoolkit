using SimpleJSON;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using TwitchToolkit.Utilities;
using TwitchToolkitDev;
using Verse;

namespace TwitchToolkit.IRC
{
    public static class TwitchBadges
    {
        public static void GetBadgeInfo()
        {
            if (ToolkitSettings.ChannelID == null || ToolkitSettings.ChannelID == "")
            {
                return;
            }

            TwitchToolkitDev.WebRequest_BeginGetResponse.Main(
                "https://badges.twitch.tv/v1/badges/channels/" +
                ToolkitSettings.ChannelID +
                "/display?language=en", new Func<TwitchToolkitDev.RequestState, bool>(ParseChannelBadgeImages)
                );
        }

        public static bool ParseChannelBadgeImages(RequestState request)
        {
            var node = JSON.Parse(request.jsonString);

            if (node["badge_sets"]["subscriber"] != null)
            {
               var subNode = node["badge_sets"]["subscriber"]["versions"];

                if (subNode["0"] != null)
                {
                    subBadges.Add(0, subNode["0"]["image_url_2x"]);
                }

                if (subNode["3"] != null)
                {
                    subBadges.Add(3, subNode["3"]["image_url_2x"]);
                }

                if (subNode["6"] != null)
                {
                    subBadges.Add(6, subNode["6"]["image_url_2x"]);
                }

                if (subNode["12"] != null)
                {
                    subBadges.Add(12, subNode["12"]["image_url_2x"]);
                }
            }

            Log.Warning("badge count " + subBadges.Count);

            CreateWebClient();

            return true;
        }

        public static void CreateWebClient()
        {
            client = new WebClient();
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadNextFile);
            DownloadChannelBadgeImages();
        }

        public static void DownloadNextFile(object sender, AsyncCompletedEventArgs e)
        {
            NextBadge();
        }


        public static void DownloadChannelBadgeImages()
        {
            if (subBadges.Count < 0 || !BadgePathExists())
            {
                client = null;
                CreateBadges();
                return;
            }

            KeyValuePair<int, string> badge = subBadges.First();

            Log.Warning("Downloading badge for " + badge.Key);
            client.DownloadFileAsync( new Uri(badge.Value), Path.Combine( dataPath , badge.Key.ToString() + badgeExt ));
        }

        public static void NextBadge()
        {
            KeyValuePair<int, string> firstPair = subBadges.First();
            subBadges.Remove(firstPair.Key);

            DownloadChannelBadgeImages();
        }

        public static bool BadgePathExists()
        {
            bool dataPathExists = Directory.Exists(dataPath);

            if (!dataPathExists)
                Directory.CreateDirectory(dataPath);

            dataPathExists = Directory.Exists(dataPath);

            return dataPathExists;
        }

        public static void CreateBadges()
        {
            if (File.Exists(Path.Combine(dataPath, "0" + badgeExt)))
            {
                Badges.All.Add( new Badge("subscriber/0", Path.Combine(dataPath, "0" + badgeExt)) );
            }
        }

        public static string badgeExt = "_sub_badge.png";

        public static WebClient client = null;

        public static Dictionary<int, string> subBadges = new Dictionary<int, string>();

        public static string dataPath = Path.Combine(SaveHelper.dataPath, "Badges/");

        public static KeyValuePair<int, string> defaultPair = new KeyValuePair<int, string>(0, "");
    }

    public class Badge
    {
        public string badgeName;
        public string fileName;

        public Badge(string badgeName, string fileName)
        {
            this.badgeName = badgeName;
            this.fileName = fileName;
        }
    }

    public static class Badges
    {
        public static List<Badge> All = new List<Badge>();
    }

}
