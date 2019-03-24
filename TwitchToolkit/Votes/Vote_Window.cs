using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;
using TwitchToolkit.Utilities;
using TwitchToolkit.Votes;

namespace TwitchToolkit
{
    public class VoteWindow : Window
    {
        static TwitchToolkit mod = Toolkit.Mod;
        Vote vote = null;
        List<int> optionsKeys = null;
        public VoteWindow(Vote vote)
        {
            this.preventCameraMotion = false;
            this.closeOnCancel = false;
            this.closeOnAccept = false;
            this.closeOnClickedOutside = false;
            this.doCloseX = false;
            this.doCloseButton = false;
            this.soundAppear = SoundDefOf.DialogBoxAppear;
            this.draggable = true;
            try
            {
                this.vote = vote;
                this.optionsKeys = vote.optionsKeys;
        }
            catch (InvalidCastException e)
            {
                Log.Error("Invalid vote window. " + e.Message);
            }  
        }

        public override void DoWindowContents(Rect inRect)
        {   
            GameFont old = Text.Font;
            Text.Font = ToolkitSettings.LargeVotingWindow ? GameFont.Medium : GameFont.Small;
            float lineheight = ToolkitSettings.LargeVotingWindow ? 50 : 30;


            Widgets.Label(inRect, "<b><color=#76BA4E>" + "TwitchStoriesChatMessageNewVote".Translate() + ": " + "TwitchToolKitVoteInstructions".Translate() + "</color></b>");
            inRect.y += lineheight / 2;
            for (int i = 0; i < optionsKeys.Count; i++)
            {
                inRect.y += lineheight;
                string msg = "[" + (i + 1) + "] ";
                msg += (vote.VoteKeyLabel(i)) + $": {vote.voteCounts[i]}";
                Widgets.Label(inRect, msg);
            }
            int secondsElapsed = TimeHelper.SecondsElapsed(VoteHandler.voteStartedAt);

            Rect bar = new Rect(inRect.x, inRect.y + lineheight, 225, 20);
            Widgets.FillableBar(bar, ((float)ToolkitSettings.VoteTime * 60f - (float)secondsElapsed) / ((float)ToolkitSettings.VoteTime * 60f));
            
            Text.Font = old;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return ToolkitSettings.LargeVotingWindow ? new Vector2(400, 140 + (optionsKeys.Count * 50f)) : new Vector2(300, 110 + (optionsKeys.Count * 30f));
            }
        }

        protected override void SetInitialSizeAndPosition()
		{
            if (ToolkitSettings.VotingWindowx == -1)
            {
                this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) - 60f, this.InitialSize.x, this.InitialSize.y);
            }
            else
            {
                this.windowRect = new Rect(ToolkitSettings.VotingWindowx, ToolkitSettings.VotingWindowy, this.InitialSize.x, this.InitialSize.y);
            }
			
			this.windowRect = this.windowRect.Rounded();
		}

        public override void PreClose()
        {
            base.PreClose();
            ToolkitSettings.VotingWindowx = this.windowRect.x;
            ToolkitSettings.VotingWindowy = this.windowRect.y;
        }


    }
}
