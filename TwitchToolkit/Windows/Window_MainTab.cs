using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using TwitchToolkit.Utilities;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.IRC;

namespace TwitchToolkit
{
    public class TwitchToolkit_MainTabWindow : MainTabWindow
    {
        static TwitchToolkit _mod = Toolkit.Mod;

        public TwitchToolkit_MainTabWindow()
        {
            _mod = Toolkit.Mod;
        }

        public override MainTabWindowAnchor Anchor
        {
            get
            {
                return MainTabWindowAnchor.Right;
            }
        }

        public override Vector2 RequestedTabSize
        {
            get
            {
                return new Vector2(450f, 355f);
            }
        }

        public string ResetAdminWarning = "Reset Viewers";

        public static int inputValue = 0;
        public static string inputBuffer = "00";

        public override void DoWindowContents(Rect inRect)
        {

            base.DoWindowContents(inRect);
            
            float padding = 5f;
            float btnWidth = ((inRect.width - (padding * 3)) / 2);
            float btnHeight = 30f;

            var rectDifficulty = new Rect(padding, padding, btnWidth, 20);


            var rectBtn = new Rect(padding, rectDifficulty.y, (inRect.width - rectDifficulty.width - (padding * 3)) / 2, btnHeight);
            if (Widgets.ButtonText(rectBtn, "Chat Window") && !Find.WindowStack.TryRemove(typeof(ChatWindow), true))
            {
                ChatWindow chatwnd = new ChatWindow();
                Toolkit.client.activeChatWindow = chatwnd;
                Find.WindowStack.Add(chatwnd);
            }

            var rectMessages = new Rect(padding, rectBtn.height + padding, inRect.width - (padding * 2), 180f);
            if (Toolkit.client != null) Widgets.TextArea(rectMessages, string.Join("\r\n", Toolkit.client.MessageLog), true);

            btnWidth = btnWidth * 2;
            rectBtn = new Rect(padding, rectMessages.y + rectMessages.height, btnWidth, btnHeight);
            Widgets.CheckboxLabeled(rectBtn, "TwitchToolkitStoreOpen".Translate(), ref ToolkitSettings.StoreOpen);

            rectBtn.y += btnHeight + padding;
            Widgets.CheckboxLabeled(rectBtn, "TwitchToolkitEarningCoins".Translate(), ref ToolkitSettings.EarningCoins);
        }
    }
}
