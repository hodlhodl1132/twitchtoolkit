using System.Collections.Generic;
using ToolkitCore;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.Votes;

public class Vote_ToryTalker : Vote_VotingIncident
{
	private StorytellerPack pack = null;

	public Vote_ToryTalker(Dictionary<int, VotingIncident> incidents, StorytellerPack pack, string title = null)
		: base(incidents, title)
	{
		base.title = title;
		this.pack = pack;
	}

	public override void StartVote()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0080: Unknown result type (might be due to invalid IL or missing erences)
		if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
		{
			VoteWindow window = new VoteWindow(this, "<color=#6441A4>" + title + "</color>");
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

	public override void EndVote()
	{
		Current.Game.GetComponent<StoryTellerVoteTracker>().LogStorytellerCompVote(pack);
		base.EndVote();
	}
}
