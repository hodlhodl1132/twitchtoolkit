using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using TwitchToolkit.Utilities;
using TwitchToolkit.PawnQueue;

namespace TwitchToolkit
{
    public class TwitchToolkit_MainTabWindow : MainTabWindow
    {
        readonly TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();

        public override MainTabWindowAnchor Anchor
        {
            get
            {
                return MainTabWindowAnchor.Right; ;
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
            if (Widgets.ButtonText(rectBtn, "Settings"))
            {
                Window mobileSettings = new Dialog_CustomModSettings
                {
                    draggable = true,
                    preventCameraMotion = false,
                    forcePause = false
                };
                Find.WindowStack.Add(mobileSettings);
                
            }

            rectBtn.x += rectBtn.width + padding;
            if (Widgets.ButtonText(rectBtn, "Chat Window") && !Find.WindowStack.TryRemove(typeof(ChatWindow), true))
            {
                ChatWindow chatwnd = new ChatWindow();
                _mod.activeChatWindow = chatwnd;
                Find.WindowStack.Add(chatwnd);
            }

            rectBtn.x += rectBtn.width + padding;
            if (Widgets.ButtonText(rectBtn, "Queue Window") && !Find.WindowStack.TryRemove(typeof(QueueWindow), true))
            {
                QueueWindow quewnd = new QueueWindow();
                Find.WindowStack.Add(quewnd);
            }


            var rectMessages = new Rect(padding, rectBtn.height + padding, inRect.width - (padding * 2), 180f);
            Widgets.TextArea(rectMessages, string.Join("\r\n", _mod.MessageLog), true);

            /*var rectInput = new Rect(padding, inRect.height - btnHeight, 20, btnHeight);
            Widgets.TextFieldNumeric<int>(rectInput, ref inputValue, ref inputBuffer);*/

            rectBtn = new Rect(padding, rectMessages.y + rectMessages.height, btnWidth, btnHeight);
            if (Widgets.ButtonText(rectBtn, $"Store Toggle: {Settings.StoreOpen}"))
            {
                if (Settings.StoreOpen)
                {
                    Settings.StoreOpen = false;
                }
                else
                {
                    Settings.StoreOpen = true;
                }
            }

            rectBtn.y += btnHeight + padding;
            if (Widgets.ButtonText(rectBtn, $"Earning Coins: {Settings.EarningCoins}"))
            {
                if (Settings.EarningCoins)
                {
                    Settings.EarningCoins = false;
                }
                else
                {
                    Settings.EarningCoins = true;
                }
            }

            rectBtn.y += btnHeight + padding;
            if (Widgets.ButtonText(rectBtn, "Price Spreadsheet"))
            {
                Helper.PastePricesToOutputLog();
            }

            rectBtn.x += btnWidth + padding;
            rectBtn.y -= (btnHeight + padding) * 2;
            if (Widgets.ButtonText(rectBtn, "TwitchStoriesReconnect".Translate()))
            {
                _mod.Reconnect();
            }

            rectBtn.y += btnHeight + padding;
            if (Widgets.ButtonText(rectBtn, "TwitchStoriesDisconnect".Translate()))
            {
                _mod.Disconnect();
            }

            rectBtn.y += btnHeight + padding;
            if (Widgets.ButtonText(rectBtn, ResetAdminWarning))
            {
                if (Settings.SyncStreamLabs)
                {
                    WarningWindow window = new WarningWindow
                    {
                        warning = "You must reset viewers in Streamlabs chatbot and then restart the game."
                    };
                    Find.WindowStack.Add(window);
                }
                else if (Settings.ResetViewerStage == 0)
                {
                    ResetAdminWarning = "Are you sure?";
                    Settings.ResetViewerStage = 1;
                }
                else if (Settings.ResetViewerStage == 1)
                {
                    ResetAdminWarning = "One more time";
                    Settings.ResetViewerStage = 2;
                }
                else if (Settings.ResetViewerStage == 2)
                {
                    ResetAdminWarning = "Reset Viewers";
                    Settings.ResetViewerStage = 0;
                    Viewers.ResetViewers();
                }
            }
        }
    }
}
