using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using TwitchToolkit.Utilities;

namespace TwitchToolkit
{
    public static class Viewers
    {
        public static string jsonallviewers;
        public static List<Viewer> All = new List<Viewer>();

        public static void AwardViewersCoins(int setamount = 0)
        {
            List<string> usernames = ParseViewersFromJsonAndFindActiveViewers();
            if (usernames != null)
            {
                foreach (string username in usernames)
                {
                    Viewer viewer = GetViewer(username);

                    if (viewer.IsBanned)
                    {
                        continue;
                    }

                    if (setamount > 0)
                    {
                        viewer.GiveViewerCoins(setamount);
                    }
                    else
                    {
                        // to earn full coins you either need to half talked within timebeforehalfcoins limit or chatreqs is turned off
                        if( TimeHelper.MinutesElapsed(viewer.last_seen) < ToolkitSettings.TimeBeforeHalfCoins || !ToolkitSettings.ChatReqsForCoins || true)
                        { 
                            double karmabonus = ((double)viewer.GetViewerKarma() / 100d) * (double)ToolkitSettings.CoinAmount;
                            viewer.GiveViewerCoins(Convert.ToInt32(karmabonus));
                        }
                        // otherwise you earn half if you have talked withing timebefore no coins
                        else if (TimeHelper.MinutesElapsed(viewer.last_seen) < ToolkitSettings.TimeBeforeNoCoins || !ToolkitSettings.ChatReqsForCoins)
                        {
                            double karmabonus = (((double)viewer.GetViewerKarma() / 100d) * (double)ToolkitSettings.CoinAmount) / 2;
                            viewer.GiveViewerCoins(Convert.ToInt32(karmabonus));
                        }
                        // else you get nothing for lurking
                    }
                }
            }
        }

        public static void GiveAllViewersCoins(int amount, List<Viewer> viewers = null)
        {
            if (viewers != null)
            {
                foreach (Viewer viewer in viewers)
                {
                    viewer.GiveViewerCoins(amount);
                }

                return;
            }

            List<string> usernames = ParseViewersFromJsonAndFindActiveViewers();
            if (usernames != null)
            {
                foreach (string username in usernames)
                {
                    Viewer viewer = Viewers.GetViewer(username);
                    if (viewer != null && viewer.GetViewerKarma() > 1)
                    {
                        viewer.GiveViewerCoins(amount);
                    }
                }
            }
        }

        public static void SetAllViewersCoins(int amount, List<Viewer> viewers = null)
        {
            if (viewers != null)
            {
                foreach (Viewer viewer in viewers)
                {
                    viewer.SetViewerCoins(amount);
                }

                return;
            }

            if (All != null)
            {
                foreach (Viewer viewer in All)
                {
                    if (viewer != null)
                    {
                        viewer.SetViewerCoins(amount);
                    }
                }
            }
        }

        public static void GiveAllViewersKarma(int amount, List<Viewer> viewers = null)
        {
            if (viewers != null)
            {
                foreach (Viewer viewer in viewers)
                {
                    viewer.SetViewerKarma(Math.Min(ToolkitSettings.KarmaCap, viewer.GetViewerKarma() + amount));
                }

                return;
            }

            List<string> usernames = ParseViewersFromJsonAndFindActiveViewers();
            if (usernames != null)
            {
                foreach (string username in usernames)
                {
                    Viewer viewer = Viewers.GetViewer(username);
                    if (viewer != null && viewer.GetViewerKarma() > 1)
                    {
                        viewer.SetViewerKarma( Math.Min(ToolkitSettings.KarmaCap, viewer.GetViewerKarma() + amount) );
                    }
                }
            }
        }

        public static void TakeAllViewersKarma(int amount, List<Viewer> viewers = null)
        {
            if (viewers != null)
            {
                foreach (Viewer viewer in viewers)
                {
                    viewer.SetViewerKarma(Math.Max(0, viewer.GetViewerKarma() - amount));
                }

                return;
            }

            if (All != null)
            {
                foreach (Viewer viewer in All)
                {
                    if (viewer != null)
                    {
                        viewer.SetViewerKarma( Math.Max(0, viewer.GetViewerKarma() - amount) );
                    }
                }
            }
        }

        public static void SetAllViewersKarma(int amount, List<Viewer> viewers = null)
        {
            if (viewers != null)
            {
                foreach (Viewer viewer in viewers)
                {
                    viewer.SetViewerKarma(amount);
                }

                return;
            }

            if (All != null)
            {
                foreach (Viewer viewer in All)
                {
                    if (viewer != null)
                    {
                        viewer.SetViewerKarma( amount );
                    }
                }
            }
        }

        public static List<string> ParseViewersFromJsonAndFindActiveViewers()
        {
            List<string> usernames = new List<string>();

            string json = jsonallviewers;

            if (json.NullOrEmpty())
            {
                return null;
            }

            var parsed = JSON.Parse(json);
            List<JSONArray> groups = new List<JSONArray>();
            groups.Add(parsed["chatters"]["moderators"].AsArray);
            groups.Add(parsed["chatters"]["staff"].AsArray);
            groups.Add(parsed["chatters"]["admins"].AsArray);
            groups.Add(parsed["chatters"]["global_mods"].AsArray);
            groups.Add(parsed["chatters"]["viewers"].AsArray);
            groups.Add(parsed["chatters"]["vips"].AsArray);
            foreach (JSONArray group in groups)
            {
                foreach (JSONNode username in group)
                {
                    string usernameconvert = username.ToString();
                    usernameconvert = usernameconvert.Remove(0, 1);
                    usernameconvert = usernameconvert.Remove(usernameconvert.Length - 1, 1);
                    usernames.Add(usernameconvert);
                }
            }

            // for bigger streams, the chatter api can get buggy. Therefore we add viewers active in chat within last 30 minutes just in case.

            foreach (Viewer viewer in All.Where(s => s.last_seen != null && TimeHelper.MinutesElapsed(s.last_seen) <= ToolkitSettings.TimeBeforeHalfCoins))
            {
                if (!usernames.Contains(viewer.username))
                {
                    Log.Warning("Viewer " + viewer.username + " added to active viewers through chat participation but not in chatter list.");
                    usernames.Add(viewer.username);
                }
            }

            return usernames;
        }

        public static bool SaveUsernamesFromJsonResponse(TwitchToolkitDev.RequestState request)
        {
            jsonallviewers = request.jsonString;
            return true;
        }

        public static void ResetViewers()
        {
            All = new List<Viewer>();
        }

        public static Viewer GetViewer(string user)
        {
            Viewer viewer = All.Find(x => x.username == user.ToLower());
            if (viewer == null)
            {
                viewer = new Viewer(user);
                viewer.SetViewerCoins((int)ToolkitSettings.StartingBalance);
                viewer.karma = ToolkitSettings.StartingKarma;
            }
            return viewer;
        }

        public static void RefreshViewers()
        {
            TwitchToolkitDev.WebRequest_BeginGetResponse.Main(
                "https://tmi.twitch.tv/group/user/" +
                ToolkitSettings.Channel.ToLower() +
                "/chatters", new Func<TwitchToolkitDev.RequestState, bool>(Viewers.SaveUsernamesFromJsonResponse)
                );
        }

        public static void ResetViewersCoins()
        {
            foreach(Viewer viewer in All) viewer.coins = (int)ToolkitSettings.StartingBalance;
        }

        public static void ResetViewersKarma()
        {
            foreach (Viewer viewer in All) viewer.karma = (int)ToolkitSettings.StartingKarma;
        }
    }
}
