using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using TwitchToolkit.Utilities;

namespace TwitchToolkit
{
    public class Viewers
    {
        public string jsonallviewers;

        public void AwardViewersCoins(int setamount = 0)
        {
            List<string> usernames = ParseViewersFromJson();
            if (usernames != null)
            {
                foreach (string username in usernames)
                {
                    Viewer viewer = Viewer.GetViewer(username);
                    if (setamount > 0)
                    {
                        viewer.GiveViewerCoins(setamount);
                    }
                    else
                    {
                        // to earn full coins you either need to half talked within timebeforehalfcoins limit or chatreqs is turned off
                        if( TimeHelper.MinutesElapsed(viewer.last_seen) < Settings.TimeBeforeHalfCoins || !Settings.ChatReqsForCoins )
                        { 
                            double karmabonus = ((double)viewer.GetViewerKarma() / 100d) * (double)Settings.CoinAmount;
                            viewer.GiveViewerCoins(Convert.ToInt32(karmabonus));
                        }
                        // otherwise you earn half if you have talked withing timebefore no coins
                        else if (TimeHelper.MinutesElapsed(viewer.last_seen) < Settings.TimeBeforeNoCoins )
                        {
                            double karmabonus = (((double)viewer.GetViewerKarma() / 100d) * (double)Settings.CoinAmount) / 2;
                            viewer.GiveViewerCoins(Convert.ToInt32(karmabonus));
                        }
                        // else you get nothing for lurking
                    }
                }
            }
        }

        public List<string> ParseViewersFromJson()
        {
            List<string> usernames = new List<string>();

            string json = this.jsonallviewers;

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
            return usernames;
        }

        public bool SaveUsernamesFromJsonResponse(TwitchToolkitDev.RequestState request)
        {
            this.jsonallviewers = request.jsonString;
            return true;
        }

        public static void ResetViewers()
        {
            Settings.ViewerIds = new Dictionary<string, int>();
            Settings.listOfViewers = new List<Viewer>();
        }
    }
}
