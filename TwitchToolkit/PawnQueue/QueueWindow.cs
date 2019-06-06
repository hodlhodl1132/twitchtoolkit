using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.PawnQueue
{
    class QueueWindow : Window
    {
        GameComponentPawns pawnComponent;

        public QueueWindow()
        {
            doCloseButton = true;
            pawnComponent = Current.Game.GetComponent<GameComponentPawns>();
            if (pawnComponent == null)
            {
                Log.Error("component null");
                Close();
            }
            GetPawn(PawnQueueSelector.FirstDefault);
        }

        public override void DoWindowContents(Rect inRect)
        {
            Rect unnamedCounter = new Rect(inRect.x + 10, 0,  300, 52);
            Widgets.Label(unnamedCounter, "Unnamed Colonists: " + unnamedColonists.Count);

            Rect colonistPortrait = new Rect(inRect.x + 10, 60, 100, 140);
            DrawColonist(colonistPortrait, selectedPawn);

            Rect rightSide = new Rect(130, 60, 300, 26);
            selectedUsername = Widgets.TextEntryLabeled(rightSide, "Username:", selectedUsername);

            rightSide.width = 120;
            rightSide.y += 26;
            rightSide.x += 150;
            if (Widgets.ButtonText(rightSide, "Assign"))
            {
                NameColonist(selectedUsername, selectedPawn);
            }         
            
            Rect pawnSelectors = new Rect(26, 210, 40, 26);

            if (Widgets.ButtonText(pawnSelectors, "<"))
            {
                GetPawn(PawnQueueSelector.Back);
            }

            pawnSelectors.x += 42;
            if (Widgets.ButtonText(pawnSelectors, ">"))
            {
                GetPawn(PawnQueueSelector.Next);
            }

            Rect namedStatus = new Rect(0, 236, 300, 26);
            bool viewerNamed = pawnComponent.HasPawnBeenNamed(selectedPawn);

            Widgets.Label(namedStatus, "Name: " + selectedPawn.Name);

            namedStatus.y += 26;
            Widgets.Label(
                namedStatus,
                "Colonist " + (
                        viewerNamed ? 
                        "Named after <color=#4BB543>" + pawnComponent.UserAssignedToPawn(selectedPawn) + "</color>" : 
                        "<color=#B2180E>Unnamed</color>"
                    )
                );

            Rect queueButtons = new Rect(0, 300, 200, 26);

            Widgets.Label(queueButtons, "Viewers in Queue: " + pawnComponent.ViewersInQueue());

            queueButtons.y += 26;
            if (Widgets.ButtonText(queueButtons, "Next Viewer from Queue"))
            {
                selectedUsername = pawnComponent.GetNextViewerFromQueue();
            }

            queueButtons.y += 26;
            if (Widgets.ButtonText(queueButtons, "Random Viewer from Queue"))
            {
                selectedUsername = pawnComponent.GetRandomViewerFromQueue();
            }

            queueButtons.y += 26;
            if (Widgets.ButtonText(queueButtons, "Ban Viewer from Queue"))
            {
                Viewer viewer = Viewers.GetViewer(selectedUsername);
                viewer.BanViewer();
            }
        }

        public void GetPawn(PawnQueueSelector method)
        {
            switch (method)
            {
                case PawnQueueSelector.Next:
                    if (pawnIndex + 1 == allColonists.Count)
                    {
                        selectedPawn = allColonists[0];
                        pawnIndex = 0;
                    }
                    else
                    {
                        pawnIndex++;
                        selectedPawn = allColonists[pawnIndex];
                    }
                    break;
                case PawnQueueSelector.Back:
                    if (pawnIndex - 1 < 0)
                    {
                        selectedPawn = allColonists[allColonists.Count - 1];
                        pawnIndex = allColonists.FindIndex(s => s == selectedPawn);
                    }
                    else
                    {
                        pawnIndex--;
                        selectedPawn = allColonists[pawnIndex];
                    }
                    break;
                case PawnQueueSelector.FirstDefault:
                    Log.Warning("first or default");
                    allColonists = Find.ColonistBar.GetColonistsInOrder();
                    unnamedColonists = GetUnamedColonists();
                    selectedUsername = "";
                    GetPawn(PawnQueueSelector.New);
                    break;
                case PawnQueueSelector.New:
                    if (unnamedColonists.Count > 0)
                    {
                        selectedPawn = unnamedColonists[0];
                    }
                    else
                    {
                        selectedPawn = allColonists[0];
                    }
                    pawnIndex = allColonists.FindIndex(s => s == selectedPawn);
                    break;
            }
        }

        public void NextColonist()
        {
            List<Pawn> colonistsUnnamed = GetUnamedColonists();
            if (colonistsUnnamed.NullOrEmpty())
                    return;

        }

        public List<Pawn> GetUnamedColonists()
        {
            List<Pawn> allColonists = Find.ColonistBar.GetColonistsInOrder();
            List<Pawn> colonistsUnnamed = new List<Pawn>();
            foreach (Pawn pawn in allColonists)
            {
                if (!pawnComponent.pawnHistory.ContainsValue(pawn))
                    colonistsUnnamed.Add(pawn);
            }
            return colonistsUnnamed;
        }

        public void DrawColonist(Rect rect, Pawn colonist)
        {
            GUI.DrawTexture(
                rect, 
                PortraitsCache.Get(
                    colonist, 
                    ColonistBarColonistDrawer.PawnTextureSize, 
                    ColonistBarColonistDrawer.PawnTextureCameraOffset, 
                    1.28205f
                    )
                );
        }

        public void NameColonist(string username, Pawn pawn)
        {
            if (pawnComponent.HasPawnBeenNamed(pawn))
            {
                if (pawnComponent.pawnHistory.ContainsValue(pawn))
                {
                    string key = null;
                    foreach (KeyValuePair<string, Pawn> pair in pawnComponent.pawnHistory)
                    {
                        if (pair.Value == pawn)
                        {
                            key = pair.Key;
                            continue;
                        }
                    }

                    if (key != null)
                    {
                        pawnComponent.pawnHistory.Remove(key);
                    }
                }
            }

            NameTriple currentName = pawn.Name as NameTriple;
            pawn.Name = new NameTriple(currentName.First, username, currentName.Last);
            pawnComponent.AssignUserToPawn(selectedUsername.ToLower(), selectedPawn);
            GetPawn(PawnQueueSelector.FirstDefault);
        }

        public string selectedUsername = "";
        public Pawn selectedPawn = null;
        public int pawnIndex = -1;
        public List<Pawn> allColonists = new List<Pawn>();
        public List<Pawn> unnamedColonists = new List<Pawn>();
        public override Vector2 InitialSize => new Vector2(500f, 500f);
    }

    public enum PawnQueueSelector
    {
        FirstDefault,
        Next,
        Back,
        New
    }
}
