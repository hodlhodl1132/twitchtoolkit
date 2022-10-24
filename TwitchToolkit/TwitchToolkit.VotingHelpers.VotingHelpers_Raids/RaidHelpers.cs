using System;
using RimWorld;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Raids;

public static class RaidHelpers
{
	public static bool RaidPossible(float points, PawnsArrivalModeDef arrival, RaidStrategyDef strategy = null, Faction faction = null)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0007: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0018: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0023: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0039: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0040: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0051: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005d: Expected O, but got Unknown
		IncidentWorker_RaidEnemy raidEnemy = new IncidentWorker_RaidEnemy();
		((IncidentWorker)raidEnemy).def = IncidentDefOf.RaidEnemy;
		return ((IncidentWorker)raidEnemy).CanFireNow(new IncidentParms
		{
			target = (IIncidentTarget)(object)Helper.AnyPlayerMap,
			points = Math.Max(StorytellerUtility.DefaultThreatPointsNow((IIncidentTarget)(object)Helper.AnyPlayerMap), points),
			raidArrivalMode = arrival,
			raidStrategy = ((strategy == null) ? RaidStrategyDefOf.ImmediateAttack : strategy),
			faction = faction
		});
	}

	public static void Raid(float points, PawnsArrivalModeDef arrival, RaidStrategyDef strategy = null, Faction faction = null)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0015: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0026: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0031: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0047: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006b: Expected O, but got Unknown
		if (!(points < 0f))
		{
			IncidentWorker_RaidEnemy raidEnemy = new IncidentWorker_RaidEnemy();
			((IncidentWorker)raidEnemy).def = IncidentDefOf.RaidEnemy;
			((IncidentWorker)raidEnemy).TryExecute(new IncidentParms
			{
				target = (IIncidentTarget)(object)Helper.AnyPlayerMap,
				points = Math.Max(StorytellerUtility.DefaultThreatPointsNow((IIncidentTarget)(object)Helper.AnyPlayerMap), points),
				raidArrivalMode = arrival,
				raidStrategy = ((strategy == null) ? RaidStrategyDefOf.ImmediateAttack : strategy),
				faction = faction
			});
		}
	}
}
