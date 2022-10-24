using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerComp_UristBot : StorytellerComp_ToryTalker
{
	protected new StorytellerCompProperties_UristBot Props => (StorytellerCompProperties_UristBot)(object)((StorytellerComp)this).props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		if (VoteHandler.voteActive || !Rand.MTBEventOccurs(ToolkitSettings.UristBotMTBDays, 60000f, 1000f))
		{
			yield break;
		}
		IncidentWorker worker = (IncidentWorker)new IncidentWorker_RaidEnemy();
		IncidentParms parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.ThreatSmall, target);
		worker.def = IncidentDefOf.RaidEnemy;
		parms.points = StorytellerUtility.DefaultThreatPointsNow(target);
		parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
		parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
		if (!worker.CanFireNow(parms) || Rand.Range(0, 0) != 0)
		{
			yield break;
		}
		parms.faction = Find.FactionManager.RandomEnemyFaction(false, false, false, (TechLevel)0);
		Dictionary<int, RaidStrategyDef> allStrategies = new Dictionary<int, RaidStrategyDef>();
		List<RaidStrategyDef> raidStrategyDefs = new List<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs);
		raidStrategyDefs.Shuffle();
		using (List<RaidStrategyDef>.Enumerator enumerator = raidStrategyDefs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				allStrategies.Add(value: enumerator.Current, key: allStrategies.Count);
			}
		}
		StorytellerPack named = DefDatabase<StorytellerPack>.GetNamed("UristBot", true);
		VoteHandler.QueueVote(new Vote_RaidStrategy(allStrategies, named, worker, (StorytellerComp)(object)this, parms, "How should the next raiders attack?"));
	}
}
