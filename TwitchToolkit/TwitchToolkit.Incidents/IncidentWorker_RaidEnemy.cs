using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace TwitchToolkit.Incidents;

public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
{
	protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
	{
		return FactionCanBeGroupSource(f, map, desperate) && FactionUtility.HostileTo(f, Faction.OfPlayer) && (desperate || (float)GenDate.DaysPassed >= f.def.earliestRaidDays);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		if (!base.TryExecuteWorker(parms))
		{
			return false;
		}
		Find.TickManager.slower.SignalForceNormalSpeedShort();
		StatsRecord statsRecord = Find.StoryWatcher.statsRecord;
		statsRecord.numRaidsEnemy++;
		return true;
	}

	protected override bool TryResolveRaidFaction(IncidentParms parms)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001f: Expected O, but got Unknown
		Map map = (Map)parms.target;
		if (parms.faction != null)
		{
			return true;
		}
		float num = parms.points;
		if (num <= 0f)
		{
			num = 999999f;
		}
		return PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Predicate<Faction>)((Faction f) => FactionCanBeGroupSource(f, map, false)), true, true, true, true) || PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroup(num, out parms.faction, (Predicate<Faction>)((Faction f) => FactionCanBeGroupSource(f, map, true)), true, true, true, true);
	}

	protected override void ResolveRaidPoints(IncidentParms parms)
	{
		if (parms.points <= 0f)
		{
			Log.Error("RaidEnemy is resolving raid points. They should always be set before initiating the incident.", false);
			parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
		}
	}

	protected override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0043: Expected O, but got Unknown
		if (parms.raidStrategy != null)
		{
			return;
		}
		Map map = (Map)parms.target;
		if (!GenCollection.TryRandomElementByWeight<RaidStrategyDef>(from d in DefDatabase<RaidStrategyDef>.AllDefs
			where d.Worker.CanUseWith(parms, groupKind) && (parms.raidArrivalMode != null || (d.arriveModes != null && GenCollection.Any<PawnsArrivalModeDef>(d.arriveModes, (Predicate<PawnsArrivalModeDef>)((PawnsArrivalModeDef x) => x.Worker.CanUseWith(parms)))))
			select d, (Func<RaidStrategyDef, float>)((RaidStrategyDef d) => d.Worker.SelectionWeight(map, parms.points)), out parms.raidStrategy))
		{
			Log.Error(string.Concat("No raid stategy for ", parms.faction, " with points ", parms.points, ", groupKind=", groupKind, "\nparms=", parms), false);
			if (!Prefs.DevMode)
			{
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
		}
	}

	protected override string GetLetterLabel(IncidentParms parms)
	{
		return parms.raidStrategy.letterLabelEnemy;
	}

	protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing erences)
		string text = string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural, parms.faction.Name);
		text += "\n\n";
		text += parms.raidStrategy.arrivalTextEnemy;
		Pawn pawn = pawns.Find((Pawn x) => ((Thing)x).Faction.leader == x);
		if (pawn != null)
		{
			text += "\n\n";
			text = (TaggedString)(text + TranslatorFormattedStringExtensions.Translate("EnemyRaidLeaderPresent", (NamedArgument)(((Thing)pawn).Faction.def.pawnsPlural), (NamedArgument)(((Entity)pawn).LabelShort), NamedArgumentUtility.Named((object)pawn, "LEADER")));
		}
		return text;
	}

	protected override LetterDef GetLetterDef()
	{
		return LetterDefOf.ThreatBig;
	}

	protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002f: Unknown result type (might be due to invalid IL or missing erences)
		return (TaggedString)(TranslatorFormattedStringExtensions.Translate("LetterRelatedPawnsRaidEnemy", (NamedArgument)(Faction.OfPlayer.def.pawnsPlural), (NamedArgument)(parms.faction.def.pawnsPlural)));
	}
}
