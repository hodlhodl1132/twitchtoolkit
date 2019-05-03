using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class Window_ViewerEditProp : Window
    {
        public Window_ViewerEditProp(EditPropsActions action, EditProp prop, Viewer viewer = null)
        {
            this.action = action;
            this.prop = prop;
            this.doCloseButton = true;

            Viewers.RefreshViewers();

            if (viewer == null)
            {
                allViewers = true;
                viewerBuffer = "<b>All Viewers</b>";
            }
            else
            {
                allViewers = false;
                this.viewer = viewer;
                viewerBuffer = "<b>" +  viewer.username + "</b>";
            }

            if (viewer != null && action == EditPropsActions.Set)
            {
                if (prop == EditProp.Coins)
                {
                    amount = viewer.GetViewerCoins();
                }
                else if (prop == EditProp.Karma)
                {
                    amount = viewer.GetViewerKarma();
                }
            }
        }

        public override void DoWindowContents(Rect inRect)
        {
            switch (action)
            {
                case EditPropsActions.Give:
                    this.actionBuffer = "Giving " + viewerBuffer + " " + amount + " " + prop.ToString();
                    break;
                case EditPropsActions.Set:
                    this.actionBuffer = "Setting " + viewerBuffer + "'s " + prop.ToString() + " to " + amount;
                    break;
                case EditPropsActions.Take:
                    this.actionBuffer = "Taking " + amount + " " + prop.ToString() + " from " + viewerBuffer;
                    break;
            }

            Rect label = new Rect(0, 0, inRect.width, 28f);
            Widgets.Label(label, actionBuffer);
            label.y += 28f;
            amountBuffer = amount.ToString();
            Widgets.TextFieldNumeric(label, ref amount, ref amountBuffer);
            label.y += 38f;
            if (Widgets.ButtonText(label, action.ToString()))
            {
                UpdateViewers();
            }
        }

        private void UpdateViewers()
        {
            List<Viewer> viewersToUpdate = new List<Viewer>();
            if (allViewers == true)
            {
                if (action == EditPropsActions.Give)
                {
                    foreach (string viewer in Viewers.ParseViewersFromJsonAndFindActiveViewers())
                    {
                        viewersToUpdate.Add(Viewers.GetViewer(viewer));
                    }
                }
                else
                {
                    viewersToUpdate = Viewers.All;
                }
            }
            else
            {
                viewersToUpdate.Add(viewer);
            }



            switch (action)
            {
                case EditPropsActions.Give:
                    if (prop == EditProp.Coins)
                    {
                        Viewers.GiveAllViewersCoins(amount, viewersToUpdate);
                    }
                    else if (prop == EditProp.Karma)
                    {
                        Viewers.GiveAllViewersKarma(amount, viewersToUpdate);
                    }
                    break;
                case EditPropsActions.Take:
                    if (prop == EditProp.Coins)
                    {
                        Viewers.GiveAllViewersCoins(-amount, viewersToUpdate);
                    }
                    else if (prop == EditProp.Karma)
                    {
                        Viewers.GiveAllViewersKarma(-amount, viewersToUpdate);
                    }
                    break;
                case EditPropsActions.Set:
                    if (prop == EditProp.Coins)
                    {
                        Viewers.SetAllViewersCoins(amount, viewersToUpdate);
                    }
                    else if (prop == EditProp.Karma)
                    {
                        Viewers.SetAllViewersKarma(amount, viewersToUpdate);
                    }
                    break;
            }

            Close();
        }

        public override Vector2 InitialSize => new Vector2(300f, 178f);

        private Viewer viewer = null;

        private EditPropsActions action;

        private EditProp prop;

        private bool allViewers = false;

        private int amount = 0;

        private string amountBuffer = "";

        private string actionBuffer = "";

        private string viewerBuffer = "";
    }

    public enum EditPropsActions
    {
        Give,
        Take,
        Set,
    }

    public enum EditProp
    {
        Coins,
        Karma
    }
}
