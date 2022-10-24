using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.Votes;

public class Vote_Mercurius : VoteIncidentDef
{
	private StorytellerPack pack;

	public Vote_Mercurius(Dictionary<int, IncidentDef> incidents, StorytellerComp source, IncidentParms parms = null, string title = null)
		: base(incidents, source, parms)
	{
		pack = DefDatabase<StorytellerPack>.GetNamed("Mercurius", true);
		base.title = title;
	}

	public override void StartVote()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0080: Unknown result type (might be due to invalid IL or missing erences)
		if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
		{
			VoteWindow window = new VoteWindow(this, "<color=#BF0030>" + title + "</color>");
			Find.WindowStack.Add((Window)(object)window);
		}
		if (!ToolkitSettings.VotingChatMsgs)
		{
			return;
		}
		TwitchWrapper.SendChatMessage(title ?? (TaggedString)(Translator.Translate("TwitchStoriesChatMessageNewVote") + ": " + Translator.Translate("TwitchToolKitVoteInstructions")));
		foreach (KeyValuePair<int, IncidentDef> pair in incidents)
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
