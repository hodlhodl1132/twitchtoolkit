using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using TwitchToolkit.Utilities;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Windows;
using TwitchToolkit.Store;
using TwitchLib.Client.Models;

namespace TwitchToolkit
{
    public class TwitchToolkit_MainTabWindow : MainTabWindow
    {
        static TwitchToolkit _mod = Toolkit.Mod;
        static List<ChatMessage> lastFiveChatMessages = new List<ChatMessage>();

        public static void LogChatMessage(ChatMessage message)
        {
            if (lastFiveChatMessages.Count >= 5)
            {
                lastFiveChatMessages = lastFiveChatMessages.FindAll((s) => s != lastFiveChatMessages[0]);
            }

            lastFiveChatMessages.Add(message);
        }

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
                return new Vector2(560f, 370f);
            }
        }

        public string ResetAdminWarning = "Reset Viewers";

        public static int inputValue = 0;
        public static string inputBuffer = "00";

        public override void DoWindowContents(Rect inRect)
        {

            base.DoWindowContents(inRect);
            
            float padding = 5f;
            float btnWidth = 100f;
            float btnHeight = 30f;

            var rectBtn = new Rect(padding, 0, btnWidth, btnHeight);
            //if (Widgets.ButtonText(rectBtn, "Chat Window") && !Find.WindowStack.TryRemove(typeof(ChatWindow), true))
            //{
            //    ChatWindow chatwnd = new ChatWindow();
            //    Toolkit.client.activeChatWindow = chatwnd;
            //    Find.WindowStack.Add(chatwnd);
            //}

            //rectBtn.x += btnWidth + padding;
            if (Widgets.ButtonText(rectBtn, "Events"))
            {
                Type type = typeof(StoreIncidentsWindow);
                Find.WindowStack.TryRemove(type);
                
                Window window = new StoreIncidentsWindow();
                Find.WindowStack.Add(window);
            }

            rectBtn.x += btnWidth + padding;
            if (Widgets.ButtonText(rectBtn, "Items"))
            {
                Type type = typeof(StoreItemsWindow);
                Find.WindowStack.TryRemove(type);
                
                Window window = new StoreItemsWindow();
                Find.WindowStack.Add(window);
            }

            rectBtn.x += btnWidth + padding;
            if (Widgets.ButtonText(rectBtn, "Settings"))
            {
                Mod mod = LoadedModManager.GetMod(typeof(TwitchToolkit));
                Type type = typeof(SettingsWindow);
                Find.WindowStack.TryRemove(type);
                
                Window window = new SettingsWindow(mod);
                Find.WindowStack.Add(window);
            }

            rectBtn.x += btnWidth + padding;
            if (Widgets.ButtonText(rectBtn, "Name Queue"))
            {
                Type type = typeof(QueueWindow);
                Find.WindowStack.TryRemove(type);
                
                Window window = new QueueWindow();
                Find.WindowStack.Add(window);
            }

            rectBtn.x += btnWidth + padding;
            if (Widgets.ButtonText(rectBtn, "Viewers"))
            {
                Type type = typeof(Window_Viewers);
                Find.WindowStack.TryRemove(type);
                
                Window window = new Window_Viewers();
                Find.WindowStack.Add(window);
            }

            rectBtn.x = padding;
            rectBtn.y += padding + 28f;
            if (Widgets.ButtonText(rectBtn, "Debug Fix"))
            {
                Helper.playerMessages = new List<string>();
                Purchase_Handler.viewerNamesDoingVariableCommands = new List<string>();
            }

            rectBtn.x += btnWidth + padding;
            if (Widgets.ButtonText(rectBtn, "Tracker"))
            {
                Window_Trackers window = new Window_Trackers();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            rectBtn.x += btnWidth + padding;
            if (Widgets.ButtonText(rectBtn, "Commands"))
            {
                Window_Commands window = new Window_Commands();
                Find.WindowStack.TryRemove(window.GetType());
                Find.WindowStack.Add(window);
            }

            btnWidth = inRect.width - (padding / 2);
            rectBtn = new Rect(padding, rectBtn.y + rectBtn.height, btnWidth, btnHeight);
            Widgets.CheckboxLabeled(rectBtn, "TwitchToolkitEarningCoins".Translate(), ref ToolkitSettings.EarningCoins);

            Rect textBox = new Rect(rectBtn.x, rectBtn.y + rectBtn.height + padding, rectBtn.width, rectBtn.height * 10);
            string outputText = "";
            foreach (ChatMessage message in lastFiveChatMessages)
            {
                outputText += "\n" + message.Username + ": " + message.Message;
            }

            Widgets.TextArea(textBox, outputText, true);
        }

    }
}
