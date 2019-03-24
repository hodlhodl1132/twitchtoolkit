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
        static GameComponentPawns pawnComponent = Current.Game.GetComponent<GameComponentPawns>();
        static Pawn currentPawn = null;
        static string currentViewer = null;
        static List<string> usernamesPool = null;
        static bool activeChoice = false;

        public QueueWindow()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Widgets.Label(inRect, "Unamed Colonists: " + GetUnamedColonists().Count);
            inRect.y += 24;
            if (currentPawn != null)
                Widgets.Label(inRect, "Next Colonist: " + currentPawn.Name.ToString());
            inRect.y += 24;
            if (currentViewer != null)
                Widgets.Label(inRect, "Selected viewer: " + currentViewer);
            Rect button = new Rect(0, 72, 240, 24);
            if (Widgets.ButtonText(button, "Name next colonist"))
                NextColonist();


            if (activeChoice)
                ConfirmPawn(100);
        }

        public static void NextColonist()
        {
            List<Pawn> colonistsUnnamed = GetUnamedColonists();
            if (colonistsUnnamed.NullOrEmpty())
                    return;
            List<string> usernames = Viewers.ParseViewersFromJson();
            if (usernames != null)
            {
                //decide which usersnames have not been used
                if (usernamesPool == null || usernamesPool.Count <= 0)
                {
                    usernamesPool = new List<string>();
                    foreach (string user in usernames)
                    {
                        if (!pawnComponent.HasUserBeenNamed(user) && !pawnComponent.HasUserBeenBanned(user))
                        {
                            usernamesPool.Add(user);
                        }
                    }
                }


                if (usernamesPool.Count() <= 0)
                {
                    Helper.Log("No eligible viewers");
                    return;
                }

                System.Random rnd = new System.Random();
                int winner = rnd.Next(0, usernamesPool.Count - 1);

                if (currentPawn == null)
                    currentPawn = colonistsUnnamed[0];
                activeChoice = true;

                activeChoice = true;
                currentViewer = usernamesPool[winner];

                    
                Helper.Log("Named colonist after " + usernamesPool[winner]);
            }
            else
            {
                //no viewers detected in chat yet
                Helper.Log("No viewers in chat");
            }
        }

        public static List<Pawn> GetUnamedColonists()
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

        public static void ConfirmPawn(int start)
        {
            Rect button = new Rect(0, start, 150, 24);
            if (Widgets.ButtonText(button, "Confirm"))
            {
                //confirm
                NamePawn();
            }

            button.y += 28;
            if (Widgets.ButtonText(button, "Skip"))
            {
                //skip
                usernamesPool = usernamesPool.Where(k => k != currentViewer).ToList();
                currentViewer = null;
                activeChoice = false;
                NextColonist();
            }

            button.y += 28;
            if (Widgets.ButtonText(button, "Ban"))
            {
                //ban
                usernamesPool = usernamesPool.Where(k => k != currentViewer).ToList();
                pawnComponent.namesBanned.Add(currentViewer);
                currentViewer = null;
                activeChoice = false;
                NextColonist();
            }
        }

        public static void NamePawn()
        {
            NameTriple oldName = currentPawn.Name as NameTriple;
            currentPawn.Name = new NameTriple(oldName.First, currentViewer, oldName.Last);
            currentPawn.GetComp<CompPawnNamed>().PropsName.isNamed = true;
            pawnComponent.pawnHistory.Add(currentViewer, currentPawn);
            
            currentViewer = null;
            currentPawn = null;
            usernamesPool = null;
            activeChoice = false;
        }
    }
}
