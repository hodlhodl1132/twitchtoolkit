using System;
using System.Collections.Generic;
using System.Linq;
using ToolkitCore;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.Votes;

public class Vote_VotingIncident : Vote
{
	public enum VoteLabelType
	{
		Label,
		Category,
		Type
	}

	public VoteLabelType labelType = VoteLabelType.Label;

	public Dictionary<int, VotingIncident> incidents = new Dictionary<int, VotingIncident>();

	public string title = null;

	public Vote_VotingIncident(Dictionary<int, VotingIncident> incidents, string title = null)
		: base(new List<int>(incidents.Keys))
	{
		List<VoteLabelType> voteLabels = Enum.GetValues(typeof(VoteLabelType)).Cast<VoteLabelType>().ToList();
		voteLabels.Shuffle();
		Helper.Log("Shuffled for vote");
		labelType = voteLabels[0];
		this.incidents = incidents;
		this.title = title;
	}

	public override void EndVote()
	{
		VotingIncident incident = incidents[DecideWinner()];
		Find.WindowStack.TryRemove(typeof(VoteWindow), true);
		Ticker.IncidentHelpers.Enqueue(incident.helper);
		StoryTellerVoteTracker tracker = Current.Game.GetComponent<StoryTellerVoteTracker>();
		tracker.LogVoteIncident(incident);
	}

	public override void StartVote()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0062: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
		{
			VoteWindow window = new VoteWindow(this, title);
			Find.WindowStack.Add((Window)(object)window);
		}
		if (!ToolkitSettings.VotingChatMsgs)
		{
			return;
		}
		TwitchWrapper.SendChatMessage(title ?? (TaggedString)(Translator.Translate("TwitchStoriesChatMessageNewVote") + ": " + Translator.Translate("TwitchToolKitVoteInstructions")));
		foreach (KeyValuePair<int, VotingIncident> pair in incidents)
		{
			TwitchWrapper.SendChatMessage($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
		}
	}

	public override string VoteKeyLabel(int id)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing erences)
		return labelType switch
		{
			VoteLabelType.Category => incidents[id].eventCategory.ToString(), 
			VoteLabelType.Type => incidents[id].eventType.ToString(), 
			_ => (TaggedString)(((Def)incidents[id]).LabelCap), 
		};
	}
}
