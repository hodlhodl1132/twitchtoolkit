using System;
using System.Collections.Generic;
using System.Linq;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Storytellers.StorytellerPackWindows;

public class Window_GlobalVoteWeights : Window
{
	private Vector2 scrollPosition;

	private int totalWeights = 1;

	public override Vector2 InitialSize => new Vector2(450f, 560f);

	public Window_GlobalVoteWeights()
	{
		base.doCloseButton = true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008c: Unknown result type (might be due to invalid IL or missing erences)
		Listing_Standard listing = new Listing_Standard();
		List<VotingIncident> allVotes = DefDatabase<VotingIncident>.AllDefs.ToList();
		Rect outRect = new Rect(0f, 0f, ((Rect)(inRect)).width, ((Rect)(inRect)).height - 50f);
		Rect viewRect = new Rect(0f, 0f, ((Rect)(outRect)).width - 20f, (float)allVotes.Count * 31f);
		((Listing)listing).Begin(inRect);
		listing.BeginSection(450f, 4f, 4f);
		listing.Label("Change the weights given to votes. Setting to 0% disables it.", -1f, (string)null);
		((Listing)listing).Gap(12f);
		if (ToolkitSettings.VoteWeights != null)
		{
			int newWeights = 0;
			foreach (VotingIncident vote in allVotes)
			{
				int index = ((Def)vote).index;
				float percentage = (float)Math.Round((float)vote.voteWeight / (float)totalWeights * 100f, 2);
				listing.SliderLabeled(((Def)vote).defName + " - " + percentage + "%",  vote.voteWeight, vote.voteWeight.ToString());
				ToolkitSettings.VoteWeights[((Def)vote).defName] = vote.voteWeight;
				newWeights += vote.voteWeight;
				((Listing)listing).Gap(6f);
			}
			totalWeights = newWeights;
		}
		listing.EndSection(listing);
		((Listing)listing).End();
	}
}
