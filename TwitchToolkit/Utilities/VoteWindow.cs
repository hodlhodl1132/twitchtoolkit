using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;
using TwitchToolkit.Utilities;

namespace TwitchToolkit
{
    public class VoteWindow : Window
    {
        IncidentDef[] _eventsPossibleChosen;
        TwitchToolkit mod;
        public VoteWindow(IncidentDef[] eventsPossibleChosen, TwitchToolkit toolkit)
        {
            this.preventCameraMotion = false;
            this.closeOnCancel = false;
            this.closeOnAccept = false;
            this.closeOnClickedOutside = false;
            this.doCloseX = false;
            this.doCloseButton = false;
            this.soundAppear = SoundDefOf.DialogBoxAppear;
            this._eventsPossibleChosen = eventsPossibleChosen;
            this.mod = toolkit;
            this.draggable = true;
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

            GameFont old = Text.Font;
            Text.Font = Settings.LargeVotingWindow ? GameFont.Medium : GameFont.Small;
            float lineheight = Settings.LargeVotingWindow ? 50 : 30;


            Widgets.Label(inRect, "<b><color=#76BA4E>" + "TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate() + "</color></b>");
            inRect.y += lineheight / 2;
            for (int i = 0; i < _eventsPossibleChosen.Count(); i++)
            {
                inRect.y += lineheight;
                string msg = "[" + (i + 1) + "] ";
                msg += (_eventsPossibleChosen[i].LabelCap) + $": {votekeys[i]}";
                Widgets.Label(inRect, msg);
            }
            int secondsElapsed = TimeHelper.SecondsElapsed(mod.StartTime);

            Rect bar = new Rect(inRect.x, inRect.y + lineheight, 225, 20);
            Widgets.FillableBar(bar, ((float)Settings.VoteTime * 60f - (float)secondsElapsed) / ((float)Settings.VoteTime * 60f));
            
            Text.Font = old;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return Settings.LargeVotingWindow ? new Vector2(400, 140 + (_eventsPossibleChosen.Count() * 50f)) : new Vector2(300, 110 + (_eventsPossibleChosen.Count() * 30f));
            }
        }

        protected override void SetInitialSizeAndPosition()
		{
            if (Settings.VotingWindowx == -1)
            {
                this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) - 60f, this.InitialSize.x, this.InitialSize.y);
            }
            else
            {
                this.windowRect = new Rect(Settings.VotingWindowx, Settings.VotingWindowy, this.InitialSize.x, this.InitialSize.y);
            }
			
			this.windowRect = this.windowRect.Rounded();
		}

        public override void PreClose()
        {
            base.PreClose();
            Settings.VotingWindowx = this.windowRect.x;
            Settings.VotingWindowy = this.windowRect.y;
        }


    }
}
