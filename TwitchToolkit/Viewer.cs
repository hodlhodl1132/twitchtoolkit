using System;
using System.Collections.Generic;
using System.Linq;

namespace TwitchToolkit
{
    public class Viewer
    {
        public string username;
        public int id;

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
                Helper.Log("Creating new user " + user.ToLower());
                viewer = new Viewer(user, Settings.ViewerIds.Count());
                Settings.ViewerIds.Add(viewer.username.ToLower(), viewer.id);
                Settings.ViewerCoins.Add(viewer.id, 150);
                Settings.ViewerKarma.Add(viewer.id, 100);
                Settings.listOfViewers.Add(viewer);
            }
            Helper.Log(viewer.username);
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
            return Settings.ViewerCoins[this.id];
        }

        public int GetViewerKarma()
        {
            return Settings.ViewerKarma[this.id];
        }

        public void SetViewerKarma(int karma)
        {
            Settings.ViewerKarma[this.id] = karma;
        }

        public int GiveViewerKarma(int karma)
        {
            Settings.ViewerKarma[this.id] = this.GetViewerKarma() + karma;
            return this.GetViewerKarma();
        }

        public int TakeViewerKarma(int karma)
        {
            Settings.ViewerKarma[this.id] = this.GetViewerKarma() - karma;
            return this.GetViewerKarma();
        }
        public void SetViewerCoins(int coins)
        {
            Settings.ViewerCoins[this.id] = coins;
        }

        public int GiveViewerCoins(int coins)
        {
            // do not let user go below 0 coins
            if (this.GetViewerCoins() + coins < 0)
            {
                Settings.ViewerCoins[this.id] = 0;
            }
            else
            {
                Settings.ViewerCoins[this.id] = this.GetViewerCoins() + coins;
            }

            return this.GetViewerCoins();
        }

        public int TakeViewerCoins(int coins)
        {
            Settings.ViewerCoins[this.id] = this.GetViewerCoins() - coins;
            return this.GetViewerCoins();
        }
    }
}