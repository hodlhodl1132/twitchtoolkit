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
        public VoteWindow(Vote vote, string title = null)
        {
            if (title != null)
            {
                this.title = title;
            }

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
            Rect exitVote = new Rect(inRect.width - 25f, 0f, 20f, 20f);
            if (Widgets.ButtonTextSubtle(exitVote, "X"))
            {
                VoteHandler.ForceEnd();
            }

            GameFont old = Text.Font;
            Text.Font = ToolkitSettings.LargeVotingWindow ? GameFont.Medium : GameFont.Small;
            float lineheight = ToolkitSettings.LargeVotingWindow ? 50 : 30;

            string titleLabel = "<b>" + title + "</b>";
            float titleHeight = Text.CalcHeight(titleLabel, inRect.width);
            Widgets.Label(inRect, titleLabel);
            inRect.y += titleHeight + 10;
            for (int i = 0; i < optionsKeys.Count; i++)
            {
                string msg = "[" + (i + 1) + "] ";
                msg += (vote.VoteKeyLabel(i)) + $": {vote.voteCounts[i]}";
                Widgets.Label(inRect, msg);
                inRect.y += lineheight;
            }
            int secondsElapsed = TimeHelper.SecondsElapsed(VoteHandler.voteStartedAt);

            Rect bar = new Rect(inRect.x, inRect.y, 225, 20);
            Widgets.FillableBar(bar, ((float)ToolkitSettings.VoteTime * 60f - (float)secondsElapsed) / ((float)ToolkitSettings.VoteTime * 60f));
            
            Text.Font = old;
        }

        public override Vector2 InitialSize
        {
            get
            {
                // return ToolkitSettings.LargeVotingWindow ? new Vector2(400, 80 + (optionsKeys.Count * 60f) + Text.CalcHeight(title, 400)) : new Vector2(300, 50 + (optionsKeys.Count * 30f) + Text.CalcHeight(title, 300));

                GameFont old = Text.Font;
                float titleHeight;
                if (ToolkitSettings.LargeVotingWindow)
                {
                    Text.Font = GameFont.Medium;
                    titleHeight = Text.CalcHeight(title, 400f) * 3 + 3;
                    Text.Font = old;

                    return new Vector2(400, titleHeight + (optionsKeys.Count * 48f) + 36f);
                }
                else
                {
                    Text.Font = GameFont.Small;
                    titleHeight = Text.CalcHeight(title, 300f) * 3;
                    Text.Font = old;
                    return new Vector2(300, titleHeight + (optionsKeys.Count * 28f) + 32f);
                }
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

        private string title = "What should happen next?";
    }
}
