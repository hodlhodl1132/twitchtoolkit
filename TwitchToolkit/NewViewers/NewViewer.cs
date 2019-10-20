using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Viewers
{
    public abstract class Viewer
    {
        private string globalIdentifier = "";

        public string GlobalIdentifier { get => GlobalIdentifier; }

        private string username = "";

        public Viewer(string globalIdentifier, string username)
        {
            this.globalIdentifier = globalIdentifier;
            this.username = username;

            Helper.Log("New viewer created " + this.GlobalIdentifier);

            ViewerModel.All.Add(this);
        }

        public static Viewer GetViewer(string username)
        {
            return ViewerModel.All.Find(s => s.ViewerType == ViewerType.Base && s.username == username);
        }

        // Username

        public string Username
        {
            get
            {
                return username;
            }
        }

        public string UsernameLower
        {
            get
            {
                return username.ToLower();
            }
        }

        public string UsernameCap
        {
            get
            {
                return username.CapitalizeFirst();
            }
        }

        // Viewer Type

        public ViewerType ViewerType { get; set; } = ViewerType.Base;

        // Coins & Karma

        public int Coins { get; set; } = ToolkitSettings.StartingBalance;

        public int Karma { get; set; } = ToolkitSettings.StartingKarma;

        public void GiveCoins(int amount)
        {
            if (amount < 1)
            {
                throw new Exception("Negative or zero value passed for give coins");
            }

            this.Coins += amount;
        }

        public void TakeCoins(int amount)
        {
            if (amount > -1)
            {
                throw new Exception("Positive or zero value passed for take coins");
            }

            this.Coins -= amount;
        }

        public void GiveKarma(int amount)
        {
            if (amount < 1)
            {
                throw new Exception("Negative or zero value passed for give karma");
            }

            this.Karma += amount;
        }

        public void TakeKarma(int amount)
        {
            if (amount > -1)
            {
                throw new Exception("Positive or zero value passed for take karma");
            }

            this.Karma -= amount;
        }

        // Active tracker

        DateTime? lastPing = null;

        public int MinutesAgoSinceLastAction
        {
            get
            {
                if (lastPing == null)
                {
                    return 999;
                }

                return TimeHelper.MinutesElapsed((DateTime)lastPing);
            }
        }

        public void WasSeen()
        {
            lastPing = DateTime.Now;
        }


        // Role checks

        public bool IsBanned
        {
            get
            {
                if (ToolkitSettings.GloballyBannedViewers == null)
                {
                    return false;
                }

                return ToolkitSettings.GloballyBannedViewers.Contains(GlobalIdentifier);
            }
        }

        public bool Mod
        {
            get
            {
                return ToolkitSettings.GlobalToolkitMods.Contains(GlobalIdentifier);
            }
        }

        public bool Subscriber { get; set; } = false;

        public bool VIP { get; set; } = false;

        // Role changes

        public void ToggleBan()
        {
            if (ToolkitSettings.GloballyBannedViewers.Contains(GlobalIdentifier))
            {
                ToolkitSettings.GloballyBannedViewers.Remove(GlobalIdentifier);
            }
            else
            {
                ToolkitSettings.GloballyBannedViewers.Add(GlobalIdentifier);
            }
        }

        public void ToggleMod()
        {
            if (ToolkitSettings.GlobalToolkitMods.Contains(GlobalIdentifier))
            {
                ToolkitSettings.GlobalToolkitMods.Remove(GlobalIdentifier);
            }
            else
            {
                ToolkitSettings.GlobalToolkitMods.Add(GlobalIdentifier);
            }
        }
    }

    public enum ViewerType
    {
        Base,
        Twitch
    }
}
