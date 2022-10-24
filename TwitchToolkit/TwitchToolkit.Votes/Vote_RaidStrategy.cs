using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.Storytellers;
using Verse;

namespace TwitchToolkit.Votes;

public class Vote_RaidStrategy : Vote
{
	private readonly Dictionary<int, RaidStrategyDef> allStrategies;

	private readonly StorytellerPack pack;

	private readonly StorytellerComp comp;

	private IncidentWorker worker = null;

	private IncidentParms parms = null;

	private string title;

	public Vote_RaidStrategy(Dictionary<int, RaidStrategyDef> allStrategies, StorytellerPack pack, IncidentWorker worker, StorytellerComp comp, IncidentParms parms, string title = null)
		: base(new List<int>(allStrategies.Keys))
	{
		this.title = title;
		this.allStrategies = allStrategies;
		this.pack = pack;
		this.worker = worker;
		this.parms = parms;
		this.comp = comp;
	}

	public override void EndVote()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0059: Expected O, but got Unknown
		parms.raidStrategy = allStrategies[DecideWinner()];
		Find.WindowStack.TryRemove(typeof(VoteWindow), true);
		Ticker.FiringIncidents.Enqueue(new FiringIncident(worker.def, comp, parms));
		Current.Game.GetComponent<StoryTellerVoteTracker>().LogStorytellerCompVote(pack);
	}

	public override void StartVote()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0080: Unknown result type (might be due to invalid IL or missing erences)
		if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
		{
			VoteWindow window = new VoteWindow(this, "<color=#CF0E0F>" + title + "</color>");
			Find.WindowStack.Add((Window)(object)window);
		}
		if (!ToolkitSettings.VotingChatMsgs)
		{
			return;
		}
		TwitchWrapper.SendChatMessage(title ?? (TaggedString)(Translator.Translate("TwitchStoriesChatMessageNewVote") + ": " + Translator.Translate("TwitchToolKitVoteInstructions")));
		foreach (KeyValuePair<int, RaidStrategyDef> pair in allStrategies)
		{
			TwitchWrapper.SendChatMessage($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
		}
	}

	public override string VoteKeyLabel(int id)
	{
		return ((Def)allStrategies[id]).defName switch
		{
			"ImmediateAttack" => "Immediate Attack", 
			"StageThenAttack" => "Wait, Then Attack", 
			"ImmediateAttackSmart" => "Avoid Traps & Turrets", 
			"ImmediateAttackSappers" => "Sappers", 
			"Siege" => "Siege", 
			_ => ((Def)allStrategies[id]).defName, 
		};
	}
}
