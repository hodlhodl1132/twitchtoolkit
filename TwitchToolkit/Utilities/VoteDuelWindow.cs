using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit
{
    public class VoteDuelWindow : Window
    {
        IncidentDef[] _eventsPossibleChosen;
        TwitchToolkit mod;

        public VoteDuelWindow(IncidentDef[] eventsPossibleChosen, TwitchToolkit twitchToolkit)
        {
            this.preventCameraMotion = false;
            this.closeOnCancel = false;
            this.closeOnAccept = false;
            this.closeOnClickedOutside = false;
            this.doCloseX = false;
            this.doCloseButton = false;
            this.soundAppear = SoundDefOf.DialogBoxAppear;
            this._eventsPossibleChosen = eventsPossibleChosen;
            this.draggable = true;
            this.mod = twitchToolkit;
        }

        public override void DoWindowContents(Rect inRect)
        {
            System.Random rnd = new System.Random();
            int[] votekeys = new int[_eventsPossibleChosen.Count()];
            foreach (KeyValuePair<string, int> vote in mod._voteAnswers)
            {
                if (_eventsPossibleChosen.Length < vote.Value)
                {
                    continue;
                }
                votekeys[vote.Value - 1] += 1;
            }
            int evt = 0;
            int voteCount = 0;
            for (int i = 0; i < votekeys.Count(); i++)
            {
                if (votekeys[i] > votekeys[evt])
                {
                    evt = i;
                }
                else if (votekeys[i] == votekeys[evt] && rnd.Next(0, 2) == 1)
                {
                    evt = i;
                }
                voteCount += votekeys[i];
            }
            Widgets.Label(inRect, "<b><color=#76BA4E>" + "TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate() + "</color></b>");

            Rect left = new Rect(0, 0, 250, 100);
            left = left.Rounded();
            Widgets.Label(left, _eventsPossibleChosen[0].LabelCap);

            var barRect = new Rect(left.x, left.y + 25, 225, 20);
            Widgets.FillableBar(barRect, (float)votekeys[0] / (votekeys[0] + votekeys[1]) > 1 ? 0 : (float)votekeys[0] / (votekeys[0] + votekeys[1]));

            Rect right = new Rect(250, 0, 250, 100);
            right = right.Rounded();
            Widgets.Label(right, _eventsPossibleChosen[1].LabelCap);

            barRect.x += 250;
            Widgets.FillableBar(barRect, (float)votekeys[1] / (votekeys[0] + votekeys[1]) > 1 ? 0 : (float)votekeys[1] / (votekeys[0] + votekeys[1]));
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(500, 150);
            }
        }

        protected override void SetInitialSizeAndPosition()
		{
			this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) - 50f, this.InitialSize.x, this.InitialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}
    }
}
