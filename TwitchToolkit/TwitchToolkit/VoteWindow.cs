using System;
using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Utilities;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit;

public class VoteWindow : Window
{
	private static TwitchToolkit mod = Toolkit.Mod;

	private Vote vote = null;

	private List<int> optionsKeys = null;

	private string title = "What should happen next?";

	public override Vector2 InitialSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
			//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
			//IL_0035: Unknown result type (might be due to invalid IL or missing erences)
			//IL_005b: Unknown result type (might be due to invalid IL or missing erences)
			//IL_0060: Unknown result type (might be due to invalid IL or missing erences)
			//IL_0082: Unknown result type (might be due to invalid IL or missing erences)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing erences)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing erences)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing erences)
			GameFont old = Text.Font;
			float titleHeight;
			if (ToolkitSettings.LargeVotingWindow)
			{
				Text.Font =((GameFont)2);
				titleHeight = Text.CalcHeight(title, 400f) * 3f + 3f;
				Text.Font =(old);
				return new Vector2(400f, titleHeight + (float)optionsKeys.Count * 48f + 36f);
			}
			Text.Font =((GameFont)1);
			titleHeight = Text.CalcHeight(title, 300f) * 3f;
			Text.Font =(old);
			return new Vector2(300f, titleHeight + (float)optionsKeys.Count * 28f + 32f);
		}
	}

	public VoteWindow(Vote vote, string title = null)
	{
		if (title != null)
		{
			this.title = title;
		}
		base.preventCameraMotion = false;
		base.closeOnCancel = false;
		base.closeOnAccept = false;
		base.closeOnClickedOutside = false;
		base.doCloseX = false;
		base.doCloseButton = false;
		base.soundAppear = SoundDefOf.DialogBoxAppear;
		base.draggable = true;
		try
		{
			this.vote = vote;
			optionsKeys = vote.optionsKeys;
		}
		catch (InvalidCastException e)
		{
			Log.Error("Invalid vote window. " + e.Message);
		}
	}

	public override void DoWindowContents(Rect inRect)
	{
		Rect exitVote = new Rect(((Rect)(inRect)).width - 25f, 0f, 20f, 20f);
        if (Widgets.ButtonTextSubtle(exitVote, "X", 0f, -1f, (SoundDef)null, default(Vector2), (Color?)null, false))
		{
			VoteHandler.ForceEnd();
		}
		GameFont old = Text.Font;
		Text.Font =((GameFont)((!ToolkitSettings.LargeVotingWindow) ? 1 : 2));
		float lineheight = (ToolkitSettings.LargeVotingWindow ? 50 : 30);
		string titleLabel = "<b>" + title + "</b>";
		float titleHeight = Text.CalcHeight(titleLabel, ((Rect)( inRect)).width);
		Widgets.Label(inRect, titleLabel);
		inRect.y =(((Rect)( inRect)).y + (titleHeight + 10f));
		for (int i = 0; i < optionsKeys.Count; i++)
		{
			string msg = "[" + (i + 1) + "] ";
			msg = msg + vote.VoteKeyLabel(i) + $": {vote.voteCounts[i]}";
			Widgets.Label(inRect, msg);
			inRect.y =(((Rect)( inRect)).y + lineheight);
		}
		int secondsElapsed = TimeHelper.SecondsElapsed(VoteHandler.voteStartedAt);
		Rect bar = new Rect(((Rect)(inRect)).x, ((Rect)(inRect)).y, 225f, 20f);
        Widgets.FillableBar(bar, ((float)ToolkitSettings.VoteTime * 60f - (float)secondsElapsed) / ((float)ToolkitSettings.VoteTime * 60f));
		Text.Font =(old);
	}

	protected override void SetInitialSizeAndPosition()
	{
		if (ToolkitSettings.VotingWindowx == -1f)
		{
			base.windowRect = new Rect(((float)UI.screenWidth - ((Window)this).InitialSize.x) / 2f, (float)UI.screenHeight - ((Window)this).InitialSize.y - 60f, ((Window)this).InitialSize.x, ((Window)this).InitialSize.y);
		}
		else
		{
			base.windowRect = new Rect(ToolkitSettings.VotingWindowx, ToolkitSettings.VotingWindowy, ((Window)this).InitialSize.x, ((Window)this).InitialSize.y);
		}
		base.windowRect = GenUI.Rounded(base.windowRect);
	}

	public override void PreClose()
	{
		((Window)this).PreClose();
		ToolkitSettings.VotingWindowx = ((Rect)( base.windowRect)).x;
		ToolkitSettings.VotingWindowy = ((Rect)( base.windowRect)).y;
	}
}
