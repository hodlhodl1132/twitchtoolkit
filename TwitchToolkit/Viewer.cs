using System;
using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Store;

namespace TwitchToolkit
{
    public class Viewer
    {
        public string username;
        public int id;
        public int coins { get; set; }
        public int karma { get; set; }

        public bool mod = false;
        public bool subscriber = false;
        public bool vip = false;

        public DateTime last_seen;

        public Viewer(string username)
        {
            this.username = username;
            this.id = Viewers.All.Count;
            Viewers.All.Add(this);
        }

        public static bool IsModerator(string user)
        {
            if (Viewers.GetViewer(user).mod)
            {
                return true;
            }

            if (ToolkitSettings.ViewerModerators == null)
            {
                return false;
            }
            return ToolkitSettings.ViewerModerators.ContainsKey(user);
        }

        public void SetAsModerator()
        {
            if (ToolkitSettings.ViewerModerators == null)
            {
                ToolkitSettings.ViewerModerators = new Dictionary<string, bool>();
            }

            if (!IsModerator(this.username))
            {
                ToolkitSettings.ViewerModerators.Add(this.username, true);
            }
        }

        public void RemoveAsModerator()
        {
            if (IsModerator(this.username))
            {
                ToolkitSettings.ViewerModerators.Remove(this.username);
            }
        }

        public int GetViewerCoins()
        {
            if (ToolkitSettings.SyncStreamLabs)
                coins = StreamLabs.GetViewerPoints(this);
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

        public void CalculateNewKarma(KarmaType karmaType, int price)
        {
            int old = this.GetViewerKarma();
            int newKarma = Karma.CalculateNewKarma(old, karmaType, price);
            
            SetViewerKarma(newKarma);
            Store_Logger.LogKarmaChange(username, old, newKarma);
        }

        public void SetViewerCoins(int coins)
        {
            this.coins = coins;
            if (ToolkitSettings.SyncStreamLabs)
                StreamLabs.SetViewerPoints(this);
        }

        public void GiveViewerCoins(int coins)
        {
            if (ToolkitSettings.SyncStreamLabs)
                this.coins = StreamLabs.GetViewerPoints(this);
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
            if (ToolkitSettings.SyncStreamLabs)
                this.coins = StreamLabs.GetViewerPoints(this);

            SetViewerCoins(this.coins - coins);
        }

        public bool IsBanned
        {
            get { return ToolkitSettings.BannedViewers.Contains(username); }
        }

        public void BanViewer()
        {
            if (!IsBanned)
            {
                ToolkitSettings.BannedViewers.Add(username);
            }
        }

        public void UnBanViewer()
        {
            if (IsBanned)
            {
                ToolkitSettings.BannedViewers.Remove(username);
            }
        }

        public static string GetViewerColorCode(string username)
        {
            if (ToolkitSettings.ViewerColorCodes == null)
            {
                ToolkitSettings.ViewerColorCodes = new Dictionary<string, string>();
            }

            if (!ToolkitSettings.ViewerColorCodes.ContainsKey(username))
            {
                SetViewerColorCode(Helper.GetRandomColorCode(), username);
            }
            
            return ToolkitSettings.ViewerColorCodes[username];
        }

        public static void SetViewerColorCode(string colorcode, string username)
        {
            ToolkitSettings.ViewerColorCodes[username] = colorcode;
        }
    }
}