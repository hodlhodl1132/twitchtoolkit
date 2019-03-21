using RimWorld;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using TwitchToolkit;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkitDev
{
    public class StreamElements
    {
        public static void ImportPoints()
        {
            WebRequest_BeginGetResponse.Main($"https://api.streamelements.com/kappa/v2/points/{Settings.AccountID}/alltime?offset=0&page=1", new Func<RequestState, bool>(ParseJsonResponse));
        }

        public void SavePoints()
        {

        }

        private static bool ParseJsonResponse(RequestState Request)
        {
            string json = Request.jsonString;
            string offsetVar = Regex.Match(Request.urlCalled, "\\A?offset=[^&]*").ToString();
            int offset = Convert.ToInt32(offsetVar.Replace("offset=", ""));
            Helper.Log("Streamlabs Request: " + json + " offset: " + offset);
            var v = JSON.Parse(json);
            for (int i = 0; i < 25; i++)
            {
                if (i > v["_total"].AsInt - offset - 1)
                    continue;

                Viewer viewer = Viewer.GetViewer(v["users"][i]["username"]);
                viewer.SetViewerCoins(v["users"][i]["points"].AsInt);
            }
            if (offset + 25 < v["_total"].AsInt)
            {
                offset += 25;
                WebRequest_BeginGetResponse.Main($"https://api.streamelements.com/kappa/v2/points/{Settings.AccountID}/alltime?offset={offset}&page=1", new Func<RequestState, bool>(ParseJsonResponse));
            }
            return true;
        }

        public static void SynceUserToWeb(string user)
        {

        }

        public static void SyncViewerStatsToWeb()
        {
            Find.WindowStack.TryRemove(typeof(Dialog_CustomModSettings));
            Find.WindowStack.TryRemove(typeof(Dialog_ModSettings));
            Find.WindowStack.TryRemove(typeof(StreamLabsLoadingWindow));
            StreamLabsLoadingWindow window = new StreamLabsLoadingWindow();
            StreamLabsLoadingWindow.Header = "Syncing viewers to StreamElement";
            StreamLabsLoadingWindow.CurrentMessage = "Resetting viewers";
            StreamLabsLoadingWindow.Progress = 0f;
            Find.WindowStack.Add(window);
        }
    }
}
