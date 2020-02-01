using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.PawnQueue;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_Viewers : Window
    {
        public Window_Viewers()
        {
            Viewers.RefreshViewers();
            if (Current.Game != null)
            {
                this.component = Current.Game.GetComponent<GameComponentPawns>();
            }
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            float firstColumn = inRect.width * 0.67f;
            float cellHeight = 28f;

            Rect searchBar = new Rect(0, 0, firstColumn * 0.66F, cellHeight);
            viewerBuffer = searchQuery;
            searchQuery = Widgets.TextEntryLabeled(searchBar, "Search:", searchQuery);

            searchBar.x += searchBar.width;
            searchBar.width = 20f;

            if (Widgets.ButtonText(searchBar, "X") ||
                searchQuery == "" ||
                (searchQuery != viewerBuffer && (selectedViewer != null || allViewers))
                )
            {
                ClearViewer();
            }

            searchBar.x += searchBar.width;
            searchBar.width = firstColumn / 3;

            if (Widgets.ButtonText(searchBar, "All Viewers"))
            {
                SelectAllViewers();
            }

            if (selectedViewer == null && allViewers == false)
            {
                if (searchQuery == "")
                {
                    return;
                }

                List<Viewer> searchViewers = Viewers.All.Where(s =>
                        s.username.Contains(searchQuery.ToLower()) ||
                        s.username == searchQuery.ToLower()
                    ).Take(6).ToList();

                Rect viewerButton = new Rect(0, searchBar.y + cellHeight, 200f, cellHeight);

                foreach (Viewer viewer in searchViewers)
                {
                    if (Widgets.ButtonText(viewerButton, viewer.username))
                    {
                        SelectViewer(viewer);
                    }

                    viewerButton.y += viewerButton.height;
                }

                return;
            }

            Rect editLabel = new Rect(0, searchBar.y + cellHeight + 10f, firstColumn, cellHeight);
            Widgets.Label(editLabel, "Editing: " + viewerBuffer);

            // three rows
            Rect smallLabel = new Rect(0, editLabel.y + cellHeight, firstColumn * 0.33f, cellHeight);
            Rect smallButton = new Rect(0, smallLabel.y + cellHeight, firstColumn * 0.33f, cellHeight);

            // first row
            Widgets.Label(smallLabel, "Coins");
            
            if (Widgets.ButtonText(smallButton, "Give"))
            {
                OpenEditProp(EditPropsActions.Give, EditProp.Coins);
            }

            smallButton.y += cellHeight;

            if (Widgets.ButtonText(smallButton, "Take"))
            {
                OpenEditProp(EditPropsActions.Take, EditProp.Coins);
            }

            smallButton.y += cellHeight;

            if (Widgets.ButtonText(smallButton, "Set"))
            {
                OpenEditProp(EditPropsActions.Set, EditProp.Coins);
            }

            // second row

            smallButton.y = smallLabel.y + cellHeight;
            smallButton.x = firstColumn * 0.45f;
            smallLabel.x = smallButton.x;

            Widgets.Label(smallLabel, "Karma");

            if (Widgets.ButtonText(smallButton, "Give"))
            {
                OpenEditProp(EditPropsActions.Give, EditProp.Karma);
            }

            smallButton.y += cellHeight;

            if (Widgets.ButtonText(smallButton, "Take"))
            {
                OpenEditProp(EditPropsActions.Take, EditProp.Karma);
            }

            smallButton.y += cellHeight;

            if (Widgets.ButtonText(smallButton, "Set"))
            {
                OpenEditProp(EditPropsActions.Set, EditProp.Karma);
            }

            float viewerInfoHeight = smallButton.y + cellHeight;

            // third row

            if (allViewers)
            {
                smallButton.y = smallLabel.y + cellHeight;
                smallButton.x = firstColumn * 1f;
                smallLabel.x = smallButton.x;
                smallLabel.width = 400f;

                if (Widgets.ButtonText(smallButton, "Karma round"))
                {
                    Viewers.AwardViewersCoins();
                }

                smallButton.y += cellHeight;


                //// banned viewers
                //if (Widgets.ButtonText(smallButton, "placeholder"))
                //{

                //}

                //smallButton.y += cellHeight;

                if (Widgets.ButtonText(smallButton, (resetAllWarning ? "Are you sure?" : "Reset All")))
                {
                    if (resetAllWarning)
                    {
                        Viewers.ResetViewers();
                        resetAllWarning = false;
                    }
                    else
                    {
                        resetAllWarning = true;
                    }
                }

                smallButton.y += cellHeight;

                if (Widgets.ButtonText(smallButton, (resetCoinWarning ? "Are you sure?" : "Reset All Coins")))
                {
                    if (resetCoinWarning)
                    {
                        Viewers.ResetViewersCoins();
                        resetCoinWarning = false;
                    }
                    else
                    {
                        resetCoinWarning = true;
                    }
                }

                smallButton.y += cellHeight;

                if (Widgets.ButtonText(smallButton, (resetKarmaWarning ? "Are you sure?" : "Reset All Karma")))
                {
                    if (resetKarmaWarning)
                    {
                        Viewers.ResetViewersKarma();
                        resetKarmaWarning = false;
                    }
                    else
                    {
                        resetKarmaWarning = true;
                    }
                }

                smallButton.y += cellHeight;
            }

            if (selectedViewer == null)
            {
                return;
            }


            // viewer info

            smallLabel.x = 0f;
            smallLabel.width = 200f;
            smallLabel.y = viewerInfoHeight + 20f;

            string colorCode = Viewer.GetViewerColorCode(selectedViewer.username);
            Widgets.Label(smallLabel, $"<b>Viewer:</b> <color=#{colorCode}>{selectedViewer.username}</color>");

            smallLabel.y += cellHeight;
            smallButton.y = smallLabel.y;
            smallButton.x = 250f;

            Widgets.Label(smallLabel, "Banned: " + (selectedViewer.IsBanned ? "Yes" : "No"));
            if (Widgets.ButtonText(smallButton, (selectedViewer.IsBanned ? "Unban" : "Ban")))
            {
                if (selectedViewer.IsBanned)
                {
                    selectedViewer.UnBanViewer();
                }
                else
                {
                    selectedViewer.BanViewer();
                }
            }

            smallLabel.y += cellHeight;
            smallButton.y = smallLabel.y;

            Widgets.Label(smallLabel, "Toolkit Mod: " + (selectedViewer.mod ? "Yes" : "No"));
            if (Widgets.ButtonText(smallButton, (selectedViewer.mod ? "Unmod" : "Mod")))
            {
                selectedViewer.mod = !selectedViewer.mod;
            }

            if (component != null)
            {
                smallLabel.y += cellHeight;
                smallButton.y = smallLabel.y;

                Widgets.Label(smallLabel, "Colonist: " + (component.HasUserBeenNamed(selectedViewer.username) ? component.PawnAssignedToUser(selectedViewer.username).Name.ToStringShort : "None"));
                if (component.HasUserBeenNamed(selectedViewer.username) && Widgets.ButtonText(smallButton, "Unassign"))
                {
                    component.pawnHistory.Remove(selectedViewer.username);
                }
            }



            smallLabel.y += cellHeight;

            Widgets.Label(smallLabel, "Coins: " + selectedViewer.GetViewerCoins());

            smallLabel.y += cellHeight;

            Widgets.Label(smallLabel, "Karma: " + selectedViewer.GetViewerKarma() + "%");

            smallButton.y = smallLabel.y + cellHeight;
            smallButton.x = 0f;

            if (Widgets.ButtonText(smallButton, (resetWarning ? "Are you sure?" : "Reset Viewer")))
            {
                if (resetWarning)
                {
                    Viewers.All = Viewers.All.Where(s => s != selectedViewer).ToList();
                    string username = selectedViewer.username;
                    resetWarning = false;
                    SelectViewer(Viewers.GetViewer(username));
                }
                else
                {
                    resetWarning = true;
                }
            }
        }

        public void OpenEditProp(EditPropsActions action, EditProp prop)
        {
            Find.WindowStack.TryRemove(typeof(Window_ViewerEditProp));

            Window_ViewerEditProp window;

            if (selectedViewer != null)
            {
                window = new Window_ViewerEditProp(action, prop, selectedViewer);
            }
            else
            {
                window = new Window_ViewerEditProp(action, prop);
            }

            Find.WindowStack.Add(window);
        }

        public void SelectViewer(Viewer viewer)
        {
            this.selectedViewer = viewer;
            this.viewerBuffer = viewer.username;
            this.searchQuery = viewerBuffer;
            this.allViewers = false;
        }

        public void SelectAllViewers()
        {
            this.selectedViewer = null;
            this.allViewers = true;
            this.viewerBuffer = "All Viewers";
            this.searchQuery = viewerBuffer;
        }

        public void ClearViewer()
        {
            selectedViewer = null;
            allViewers = false;
            viewerBuffer = "";
            searchQuery = viewerBuffer;
        }

        private GameComponentPawns component = null;

        private Viewer selectedViewer = null;

        private string viewerBuffer = "";

        private bool allViewers = false;

        private string searchQuery = "";

        private bool resetWarning = false;

        private bool resetAllWarning = false;

        private bool resetCoinWarning = false;

        private bool resetKarmaWarning = false;
    }
}
