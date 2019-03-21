using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitchToolkit
{
    public class Viewer
    {
        public string username;
        public int id;
        public int coins { get; set; }
        public int karma { get; set; }

        public DateTime last_seen;

        public Viewer(string username, int id)
        {
            this.username = username;
            this.id = id;
        }

        public static Viewer GetViewer(string user)
        {
            Viewer viewer = Settings.listOfViewers.Find(x => x.username == user.ToLower());
            if (viewer == null)
            {
                viewer = new Viewer(user, Settings.ViewerIds.Count());
                Settings.ViewerIds.Add(viewer.username.ToLower(), viewer.id);
                viewer.SetViewerCoins(Settings.StartingBalance);
                viewer.karma = Settings.StartingKarma;
                Settings.listOfViewers.Add(viewer);
            }
            return viewer;
        }

        public static bool IsModerator(string user)
        {
            if (Settings.ViewerModerators == null)
            {
                return false;
            }
            return Settings.ViewerModerators.ContainsKey(user);
        }

        public void SetAsModerator()
        {
            if (Settings.ViewerModerators == null)
            {
                Settings.ViewerModerators = new Dictionary<string, bool>();
            }

            if (!IsModerator(this.username))
            {
                Settings.ViewerModerators.Add(this.username, true);
            }
        }

        public void RemoveAsModerator()
        {
            if (IsModerator(this.username))
            {
                Settings.ViewerModerators.Remove(this.username);
            }
        }

        public int GetViewerCoins()
        {
            if (Settings.SyncStreamLabs)
                return StreamLabs.GetViewerPoints(this);
            return coins;
        }

        public int GetViewerKarma()
        {
            return karma;
        }

        public void SetViewerKarma(int karma)
        {
            this.karma = karma;
        }

        public int GiveViewerKarma(int karma)
        {
            this.karma = this.GetViewerKarma() + karma;
            return this.GetViewerKarma();
        }

        public int TakeViewerKarma(int karma)
        {
            this.karma = this.GetViewerKarma() - karma;
            return this.GetViewerKarma();
        }
        public void SetViewerCoins(int coins)
        {
            this.coins = coins;
            if (Settings.SyncStreamLabs)
                StreamLabs.SetViewerPoints(this);
        }

        public void GiveViewerCoins(int coins)
        {
            // do not let user go below 0 coins
            if (this.coins + coins < 0)
            {
                this.coins = 0;
                SetViewerCoins(0);
            }
            else
            {
                SetViewerCoins(this.coins + coins);
            }
        }

        public void TakeViewerCoins(int coins)
        {
            SetViewerCoins(this.coins - coins);
        }

        public static string GetViewerColorCode(string username)
        {
            if (Settings.ViewerColorCodes == null)
            {
                return "FF0000";
            }

            if (!Settings.ViewerColorCodes.ContainsKey(username))
            {
                SetViewerColorCode(Helper.GetRandomColorCode(), username);
            }
            return Settings.ViewerColorCodes[username];
        }

        public static void SetViewerColorCode(string colorcode, string username)
        {
            Settings.ViewerColorCodes[username] = colorcode;
        }
    }
}