using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_Viewers : Window
    {
        public Window_Viewers()
        {
            Viewers.RefreshViewers();
            this.doCloseButton = true;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(600f, 600f);
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            GameFont normal = Text.Font;

            Rect title = new Rect(10f, 0f, 300f, 26f);
            Text.Font = GameFont.Medium;
            Widgets.Label(title, "Viewer Tool");

            Text.Font = GameFont.Small;

            if (notificationFrames > 0)
            {
                title.width = 340f;
                title.x = inRect.width - 340f;
                Widgets.Label(title, notification);

                notificationFrames--;
            }

            Rect leftHalf = new Rect(10f, 38f, (inRect.width - 20f) / 2f, inRect.height - 40f);
            Rect rightHalf = new Rect(leftHalf);
            rightHalf.x = leftHalf.width + 10f;

            Rect coinAmountBox = new Rect(leftHalf.x, leftHalf.y, leftHalf.width, 28f);

            string coinBuffer = coinBoxAmount.ToString();
            Widgets.TextFieldNumericLabeled<int>(coinAmountBox, "Coins:", ref coinBoxAmount, ref coinBuffer);


            Rect smallBtn = new Rect(leftHalf.width / 2f, leftHalf.y + 38f, leftHalf.width / 2f, 28f);

            // left half
            if (Widgets.ButtonText(smallBtn, "Give All Coins"))
            {
                Viewers.GiveAllViewersCoins(coinBoxAmount);
                if (selectedViewer != null)
                {
                    viewersCoins += coinBoxAmount;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }
                
                CreateNotification("Gave all viewers " + coinBoxAmount + " coins.");
            }

            smallBtn.y += 38f;

            if (Widgets.ButtonText(smallBtn, "Take All Coins"))
            {
                Viewers.GiveAllViewersCoins(- (coinBoxAmount));
                if (selectedViewer != null)
                {
                    viewersCoins -= coinBoxAmount;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }

                CreateNotification("Took " + coinBoxAmount + " coins from every viewer.");
            }

            smallBtn.y += 38f;

            if (!setAllCoinsWarning && Widgets.ButtonText(smallBtn, "Set All Coins"))
            {
                setAllCoinsWarning = true;
            }

            if (setAllCoinsWarning && Widgets.ButtonText(smallBtn, "Are you sure?"))
            {
                Viewers.SetAllViewersCoins(coinBoxAmount);
                if (selectedViewer != null)
                {
                    viewersCoins = coinBoxAmount;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }

                CreateNotification("Set all viewers coins to " + coinBoxAmount);
                setAllCoinsWarning = false;
            }

            smallBtn.y += 38f;

            if (!resetAllCoinsWarning && Widgets.ButtonText(smallBtn, "Reset All Coins"))
            {
                resetAllCoinsWarning = true;
            }

            if (resetAllCoinsWarning && Widgets.ButtonText(smallBtn, "Are you sure?"))
            {
                Viewers.SetAllViewersCoins(ToolkitSettings.StartingBalance);
                if (selectedViewer != null)
                {
                    viewersCoins = ToolkitSettings.StartingBalance;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }

                CreateNotification("Set all viewers coins to " + ToolkitSettings.StartingBalance);
                resetAllCoinsWarning = false; 
            }

            Rect karmaAmountBox = new Rect(coinAmountBox);
            karmaAmountBox.y = smallBtn.y + 38f;

            string karmaBuffer = karmaBoxAmount.ToString();
            Widgets.TextFieldNumericLabeled<int>(karmaAmountBox, "Karma %:", ref karmaBoxAmount, ref karmaBuffer);

            smallBtn.y = karmaAmountBox.y + 38f;

            if (Widgets.ButtonText(smallBtn, "Give All Karma"))
            {
                Viewers.GiveAllViewersKarma(karmaBoxAmount);
                if (selectedViewer != null)
                {
                    viewersKarma = Math.Min(karmaBoxAmount + viewersKarma, ToolkitSettings.KarmaCap);
                    viewerKarmaBuffer = viewersKarma.ToString();
                    UpdateViewer();
                }
                
                CreateNotification("Gave all viewers " + karmaBoxAmount + " karma.");
            }

            smallBtn.y += 38f;

            if (Widgets.ButtonText(smallBtn, "Take All Karma"))
            {
                Viewers.TakeAllViewersKarma(karmaBoxAmount);
                if (selectedViewer != null)
                {
                    viewersKarma = Math.Max( viewersKarma - karmaBoxAmount, 0);
                    viewerKarmaBuffer = viewersKarma.ToString();
                    UpdateViewer();
                }

                CreateNotification("Took " + karmaBoxAmount + "% karma from every viewer.");
            }

            smallBtn.y += 38f;

            if (!setAllKarmaWarning && Widgets.ButtonText(smallBtn, "Set All Karma"))
            {
                setAllKarmaWarning = true;
            }

            if (setAllKarmaWarning && Widgets.ButtonText(smallBtn, "Are you sure?"))
            {
                Viewers.SetAllViewersKarma(karmaBoxAmount);
                if (selectedViewer != null)
                {
                    viewersKarma = karmaBoxAmount;
                    viewerKarmaBuffer = viewersKarma.ToString();
                    UpdateViewer();
                }

                CreateNotification("Set all viewers karma to " + karmaBoxAmount);
                setAllKarmaWarning = false;
            }
            
            smallBtn.y += 38f;

            if (!resetAllKarmaWarning && Widgets.ButtonText(smallBtn, "Reset All Karma"))
            {
                resetAllKarmaWarning = true;
            }

            if (resetAllKarmaWarning && Widgets.ButtonText(smallBtn, "Are you sure?"))
            {
                Viewers.SetAllViewersKarma(karmaBoxAmount);
                if (selectedViewer != null)
                {
                    viewersKarma = ToolkitSettings.StartingKarma;
                    viewerKarmaBuffer = viewersKarma.ToString();
                    UpdateViewer();
                }

                CreateNotification("Set all viewers karma to " + ToolkitSettings.StartingKarma);
                resetAllKarmaWarning = false;
            }

            // right half
            Rect viewerLabel = new Rect(rightHalf.x + 100, rightHalf.y, rightHalf.width, 28f);

            if (selectedViewer != null)
            {
                if (viewersCoins != viewersCoinsCached)
                {
                    UpdateViewer();
                }

                string colorCode = Viewer.GetViewerColorCode(selectedViewer.username);
                Widgets.Label(viewerLabel, $"<b>Viewer:</b> <color=#{colorCode}>{selectedViewer.username}</color>");

                viewerLabel.y += 98f;

                if (Viewer.IsModerator(selectedViewer.username))
                {
                    Widgets.Label(viewerLabel, "<b><color=#008000>Moderator</color></b>");

                    viewerLabel.y += 38f;
                }

                viewerLabel.x = rightHalf.x + 10;

                Widgets.TextFieldNumericLabeled<int>(viewerLabel, "Coins:", ref viewersCoins, ref viewerCoinBuffer);

                viewerLabel.y += 38f;

                Widgets.TextFieldNumericLabeled<int>(viewerLabel, "Karma %:", ref viewersKarma, ref viewerKarmaBuffer);

                viewerLabel.y += 38f;
                viewerLabel.width = viewerLabel.width / 2;
                viewerLabel.x = rightHalf.x + (rightHalf.width / 2f);

                if (Widgets.ButtonText(viewerLabel, "Give Coins"))
                {
                    viewersCoins += coinBoxAmount;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }

                viewerLabel.y += 38f;

                if (Widgets.ButtonText(viewerLabel, "Take Coins"))
                {
                    viewersCoins -= coinBoxAmount;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }

                viewerLabel.y += 38f;

                if (Widgets.ButtonText(viewerLabel, "Set Coins"))
                {
                    viewersCoins = coinBoxAmount;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }

                viewerLabel.y += 38f;

                if (Widgets.ButtonText(viewerLabel, "Reset Coins"))
                {
                    viewersCoins = ToolkitSettings.StartingBalance;
                    viewerCoinBuffer = viewersCoins.ToString();
                    UpdateViewer();
                }

                viewerLabel.y += 38f;

                if (Widgets.ButtonText(viewerLabel, "Make Moderator"))
                {
                    selectedViewer.SetAsModerator();
                }

                viewerLabel.y += 38f;

                if (Widgets.ButtonText(viewerLabel, "Remove Moderator"))
                {
                    selectedViewer.RemoveAsModerator();
                }
            }

            if (lastSearch != usernameSearch)
            {
                selectedViewer = null;
                FindViewers();
            }

            Rect viewerBar = new Rect(rightHalf.x, 70f, rightHalf.width, 28f);
            usernameSearch = Widgets.TextEntryLabeled(viewerBar, "Search:", usernameSearch);

            if (searchResults != null && searchResults.Count > 0)
            {
                Rect vwrBtn = new Rect((inRect.width - 20f) * 0.75f, viewerBar.y + 38f, rightHalf.width / 2f, 28f);

                foreach (Viewer viewer in searchResults)
                {
                    if (Widgets.ButtonText(vwrBtn, viewer.username))
                    {
                        SetViewer(viewer);
                    }
                    vwrBtn.y += 28f;
                }
            }

            Rect bottomBar = new Rect(10f, inRect.height - 78f, (inRect.width - 20f) / 2f, 28f);

            if (Widgets.ButtonText(bottomBar, "Give viewers coins by karma"))
            {
                selectedViewer = null;
                Viewers.AwardViewersCoins();
                CreateNotification("Gave all viewers coins based on their karma");
            }

            bottomBar.x += bottomBar.width + 5f;
            
            if (!resetAllViewersWarning && Widgets.ButtonText(bottomBar, "Reset All Viewers"))
            {
                resetAllViewersWarning = true;
            }

            if (resetAllViewersWarning && Widgets.ButtonText(bottomBar, "Are you sure?"))
            {
                selectedViewer = null;
                Viewers.ResetViewers();
                CreateNotification($"Reset all viewers to {ToolkitSettings.StartingBalance} coins and {ToolkitSettings.StartingKarma}% karma.");
                resetAllViewersWarning = false;
            }

        }

        public void CreateNotification(string message)
        {
            notification = message;
            notificationFrames = 240;
        }

        private void FindViewers()
        {
            lastSearch = usernameSearch;

            searchResults = Viewers.All.Where(s => 
                usernameSearch != "" &&
                (s.username.Contains(usernameSearch) ||
                s.username == usernameSearch)
            ).Take(6).ToList();
        }

        private void SetViewer(Viewer viewer)
        {
            selectedViewer = viewer;
            searchResults = new List<Viewer>();

            int coins = viewer.GetViewerCoins();
            int karma = viewer.GetViewerKarma();

            viewersCoins = coins;
            viewersCoinsCached = coins;

            viewersKarma = karma;
            viewersKarmaCached = karma;

            viewerCoinBuffer = coins.ToString();
            viewerKarmaBuffer = karma.ToString();
        }

        private void UpdateViewer()
        {
            if (selectedViewer == null)
            {
                return;
            }

            CreateNotification("Updated viewer");

            viewersCoinsCached = viewersCoins;
            viewersKarmaCached = viewersKarma;

            selectedViewer.SetViewerCoins(viewersCoins);
            selectedViewer.SetViewerKarma(viewersKarma);
        }

        private List<Viewer> searchResults = new List<Viewer>();

        private int coinBoxAmount = 0;

        private int karmaBoxAmount = 0;

        private int notificationFrames = 0;

        private string notification = "";

        private Viewer selectedViewer = null;

        private string usernameSearch = "";

        private string lastSearch = "";

        // viewer cached

        private int viewersCoinsCached;
        private int viewersKarmaCached;

        private int viewersCoins;
        private int viewersKarma;

        private string viewerCoinBuffer;
        private string viewerKarmaBuffer;

        // bool warnings
        private bool setAllCoinsWarning = false;
        private bool resetAllCoinsWarning = false;

        private bool setAllKarmaWarning = false;
        private bool resetAllKarmaWarning = false;

        private bool resetAllViewersWarning = false;
    }
}
