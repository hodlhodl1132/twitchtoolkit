using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using TwitchToolkit.Viewers;
using TwitchToolkitDev;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities
{
    public class StreamLabsLoadingWindow : Window
    {
        public StreamLabsLoadingWindow()
        {
            this.doCloseButton = true;
            viewerCache = Viewers.All;
            numOfViewers = viewerCache.Count;
        }

        public override void DoWindowContents(Rect inRect)
        {
            GameFont old = Text.Font;
            Text.Font = GameFont.Medium;
            Widgets.Label(inRect, "<b><color=#FF4500>" + Header + "</color></b>");
            Text.Font = GameFont.Small;
            inRect.y += 30;
            Widgets.Label(inRect, CurrentMessage);
            inRect.y += 30;
            Rect button = new Rect(0, inRect.y, 120, 24);
            if (!syncStarted && Widgets.ButtonText(button, "Start Sync"))
            {
                syncStarted = true;
                lastCall = DateTime.Now;
                DeleteViewerData();
            }
            if (syncStarted && viewerCache.Count > 0 && lastCall != null && TimeHelper.SecondsElapsed(lastCall) >= 1)
            {
                //next viewer
                Viewer next = viewerCache[0];
                //delete old viewer
                viewerCache = viewerCache.Where(k => k != next).ToList();
                lastCall = DateTime.Now;
                CurrentMessage = "Syncing viewer " + next.username + " with " + next.coins + " coins";
                Progress = (float)viewerCache.Count / numOfViewers;
                SyncViewers(next);           
            }
            inRect.y += 30;
            Rect bar = new Rect(0, inRect.y, 500, 30);
            Widgets.FillableBar(bar, Progress);
            Text.Font = old;
        }

        static string Token = "Bearer " + ToolkitSettings.JWTToken;
        public static void DeleteViewerData()
        {
            WebHeaderCollection deleteHeaders = new WebHeaderCollection();
            deleteHeaders.Add(HttpRequestHeader.Authorization, Token);
            deleteHeaders.Set(HttpRequestHeader.Authorization, Token);
            WebRequest_BeginGetResponse.Delete($"https://api.streamelements.com/kappa/v2/points/{ToolkitSettings.AccountID}/reset/current", null, deleteHeaders);
        }

        public void SyncViewers(Viewer viewer)
        {
            WebHeaderCollection syncHeaders = new WebHeaderCollection();
            syncHeaders.Add(HttpRequestHeader.Authorization, Token);
            syncHeaders.Set(HttpRequestHeader.Authorization, Token); 
            WebRequest_BeginGetResponse.Put($"https://api.streamelements.com/kappa/v2/points/{ToolkitSettings.AccountID}/{viewer.username}/{viewer.coins}", null, syncHeaders);
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(500f, 300f);
            }
        }

        public static string Header = "";
        public static string CurrentMessage = "";
        public static float Progress = 0f;
        public static bool syncStarted = false;

        public List<Viewer> viewerCache = new List<Viewer>();
        public DateTime lastCall;
        public float numOfViewers;
    }
}
