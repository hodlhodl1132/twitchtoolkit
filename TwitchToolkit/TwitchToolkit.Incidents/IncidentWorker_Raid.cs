using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Incidents;

public abstract class IncidentWorker_Raid : IncidentWorker_PawnsArrive
{
	public static TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();

	public bool disableEvent = false;

	protected abstract bool TryResolveRaidFaction(IncidentParms parms);

	protected abstract void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind);

	protected abstract string GetLetterLabel(IncidentParms parms);

	protected abstract string GetLetterText(IncidentParms parms, List<Pawn> pawns);

	protected abstract LetterDef GetLetterDef();

	protected abstract string GetRelatedPawnsInfoLetterText(IncidentParms parms);

	protected abstract void ResolveRaidPoints(IncidentParms parms);

	protected virtual void ResolveRaidArriveMode(IncidentParms parms)
	{
		if (parms.raidArrivalMode == null)
		{
			if (parms.raidArrivalModeForQuickMilitaryAid && !(from d in DefDatabase<PawnsArrivalModeDef>.AllDefs
				where d.forQuickMilitaryAid
				select d).Any((PawnsArrivalModeDef d) => d.Worker.GetSelectionWeight(parms) > 0f))
			{
				parms.raidArrivalMode = ((Rand.Value >= 0.6f) ? PawnsArrivalModeDefOf.CenterDrop : PawnsArrivalModeDefOf.EdgeDrop);
			}
			else if (!GenCollection.TryRandomElementByWeight<PawnsArrivalModeDef>(parms.raidStrategy.arriveModes.Where((PawnsArrivalModeDef x) => x.Worker.CanUseWith(parms)), (Func<PawnsArrivalModeDef, float>)((PawnsArrivalModeDef x) => x.Worker.GetSelectionWeight(parms)), out parms.raidArrivalMode))
			{
				Log.Error("Could not resolve arrival mode for raid. Defaulting to EdgeWalkIn. parms=" + parms, false);
				parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			}
		}
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		//IL_0149: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0150: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_028f: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0299: Unknown result type (might be due to invalid IL or missing erences)
		//IL_029e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_031c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_036b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing erences)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing erences)
		ResolveRaidPoints(parms);
		if (disableEvent)
		{
			return false;
		}
		if (!TryResolveRaidFaction(parms))
		{
			return false;
		}
		PawnGroupKindDef combat = PawnGroupKindDefOf.Combat;
		ResolveRaidStrategy(parms, combat);
		ResolveRaidArriveMode(parms);
		if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
		{
			return false;
		}
		parms.points = AdjustedRaidPoints(parms.points, parms.raidArrivalMode, parms.raidStrategy, parms.faction, combat);
		PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(combat, parms, false);
		List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList();
		List<string> viewernames = Viewers.ParseViewersFromJsonAndFindActiveViewers();
		if (list.Count > 0 && viewernames != null)
		{
			int count = 0;
			int totalviewers = viewernames.Count();
			foreach (Pawn pawn2 in list)
			{
				if (count != list.Count() && !GenList.NullOrEmpty<string>((IList<string>)viewernames) && !pawn2.RaceProps.IsMechanoid)
				{
					int thisviewer = Rand.Range(0, viewernames.Count());
					Name name2 = pawn2.Name;
					NameTriple name = (NameTriple)(object)((name2 is NameTriple) ? name2 : null);
					NameTriple newname = new NameTriple(name.First, viewernames[thisviewer], name.Last);
					if (!((Name)newname).UsedThisGame || ToolkitSettings.RepeatViewerNames)
					{
						pawn2.Name = ((Name)(object)newname);
					}
					viewernames.RemoveAt(thisviewer);
					count++;
				}
			}
		}
		if (list.Count == 0)
		{
			Log.Error("Got no pawns spawning raid from parms " + parms, false);
			return false;
		}
		parms.raidArrivalMode.Worker.Arrive(list, parms);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Points = " + parms.points.ToString("F0"));
		foreach (Pawn pawn in list)
		{
			string str = ((pawn.equipment != null && pawn.equipment.Primary != null) ? ((Entity)pawn.equipment.Primary).LabelCap : "unarmed");
			stringBuilder.AppendLine(pawn.KindLabel + " - " + str);
		}
		TaggedString letterLabel = (TaggedString)(GetLetterLabel(parms));
		TaggedString letterText = (TaggedString)(GetLetterText(parms, list));
		PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter((IEnumerable<Pawn>)list, ref letterLabel, ref letterText, GetRelatedPawnsInfoLetterText(parms), true, true);
		List<TargetInfo> list2 = new List<TargetInfo>();
		if (parms.pawnGroups != null)
		{
			List<List<Pawn>> list3 = IncidentParmsUtility.SplitIntoGroups(list, parms.pawnGroups);
			List<Pawn> list4 = GenCollection.MaxBy<List<Pawn>, int>((IEnumerable<List<Pawn>>)list3, (Func<List<Pawn>, int>)((List<Pawn> x) => x.Count));
			if (GenCollection.Any<Pawn>(list4))
			{
				list2.Add((TargetInfo)((Thing)(object)list4[0]));
			}
			for (int i = 0; i < list3.Count; i++)
			{
				if (list3[i] != list4 && GenCollection.Any<Pawn>(list3[i]))
				{
					list2.Add((TargetInfo)((Thing)(object)list3[i][0]));
				}
			}
		}
		else if (GenCollection.Any<Pawn>(list))
		{
			list2.Add((TargetInfo)((Thing)(object)list[0]));
		}
		SendStandardLetter(letterLabel, letterText, GetLetterDef(), parms, (LookTargets)(list2), Array.Empty<NamedArgument>());
		parms.raidStrategy.Worker.MakeLords(parms, list);
		return true;
	}

	public static float AdjustedRaidPoints(float points, PawnsArrivalModeDef raidArrivalMode, RaidStrategyDef raidStrategy, Faction faction, PawnGroupKindDef groupKind)
	{
		if (raidArrivalMode.pointsFactorCurve != null)
		{
			points *= raidArrivalMode.pointsFactorCurve.Evaluate(points);
		}
		if (raidStrategy.pointsFactorCurve != null)
		{
			points *= raidStrategy.pointsFactorCurve.Evaluate(points);
		}
		points = Mathf.Max(points, raidStrategy.Worker.MinimumPoints(faction, groupKind) * 1.05f);
		return points;
	}

	public void DoTable_RaidFactionSampled()
	{
		//IL_00cc: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d3: Expected O, but got Unknown
		int ticksGame = Find.TickManager.TicksGame;
		Find.TickManager.DebugSetTicksGame(36000000);
		List<TableDataGetter<Faction>> list = new List<TableDataGetter<Faction>>();
		list.Add(new TableDataGetter<Faction>("name", (Func<Faction, string>)((Faction f) => f.Name)));
		foreach (float points in DebugActionsUtility.PointsOptions(false))
		{
			Dictionary<Faction, int> factionCount = new Dictionary<Faction, int>();
			foreach (Faction current in Find.FactionManager.AllFactions)
			{
				factionCount.Add(current, 0);
			}
			for (int i = 0; i < 500; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = (IIncidentTarget)(object)Find.CurrentMap;
				incidentParms.points = points;
				if (TryResolveRaidFaction(incidentParms))
				{
					factionCount[incidentParms.faction]++;
				}
			}
			list.Add(new TableDataGetter<Faction>(points.ToString("F0"), (Func<Faction, string>)delegate(Faction str)
			{
				int num = factionCount[str];
				return GenText.ToStringPercent((float)num / 500f);
			}));
		}
		Find.TickManager.DebugSetTicksGame(ticksGame);
		DebugTables.MakeTablesDialog<Faction>(Find.FactionManager.AllFactions, list.ToArray());
	}

	public void DoTable_RaidStrategySampled(Faction fac)
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d1: Expected O, but got Unknown
		int ticksGame = Find.TickManager.TicksGame;
		Find.TickManager.DebugSetTicksGame(36000000);
		List<TableDataGetter<RaidStrategyDef>> list = new List<TableDataGetter<RaidStrategyDef>>();
		list.Add(new TableDataGetter<RaidStrategyDef>("defName", (Func<RaidStrategyDef, string>)((RaidStrategyDef d) => ((Def)d).defName)));
		foreach (float points in DebugActionsUtility.PointsOptions(false))
		{
			Dictionary<RaidStrategyDef, int> strats = new Dictionary<RaidStrategyDef, int>();
			foreach (RaidStrategyDef current in DefDatabase<RaidStrategyDef>.AllDefs)
			{
				strats.Add(current, 0);
			}
			for (int i = 0; i < 500; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = (IIncidentTarget)(object)Find.CurrentMap;
				incidentParms.points = points;
				incidentParms.faction = fac;
				if (TryResolveRaidFaction(incidentParms))
				{
					ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
					if (incidentParms.raidStrategy != null)
					{
						strats[incidentParms.raidStrategy]++;
					}
				}
			}
			list.Add(new TableDataGetter<RaidStrategyDef>(points.ToString("F0"), (Func<RaidStrategyDef, string>)delegate(RaidStrategyDef str)
			{
				int num = strats[str];
				return GenText.ToStringPercent((float)num / 500f);
			}));
		}
		Find.TickManager.DebugSetTicksGame(ticksGame);
		DebugTables.MakeTablesDialog<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs, list.ToArray());
	}

	public void DoTable_RaidArrivalModeSampled(Faction fac)
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00ce: Expected O, but got Unknown
		int ticksGame = Find.TickManager.TicksGame;
		Find.TickManager.DebugSetTicksGame(36000000);
		List<TableDataGetter<PawnsArrivalModeDef>> list = new List<TableDataGetter<PawnsArrivalModeDef>>();
		list.Add(new TableDataGetter<PawnsArrivalModeDef>("mode", (Func<PawnsArrivalModeDef, string>)((PawnsArrivalModeDef f) => ((Def)f).defName)));
		foreach (float points in DebugActionsUtility.PointsOptions(false))
		{
			Dictionary<PawnsArrivalModeDef, int> modeCount = new Dictionary<PawnsArrivalModeDef, int>();
			foreach (PawnsArrivalModeDef current in DefDatabase<PawnsArrivalModeDef>.AllDefs)
			{
				modeCount.Add(current, 0);
			}
			for (int i = 0; i < 500; i++)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = (IIncidentTarget)(object)Find.CurrentMap;
				incidentParms.points = points;
				incidentParms.faction = fac;
				if (TryResolveRaidFaction(incidentParms))
				{
					ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
					ResolveRaidArriveMode(incidentParms);
					modeCount[incidentParms.raidArrivalMode]++;
				}
			}
			list.Add(new TableDataGetter<PawnsArrivalModeDef>(points.ToString("F0"), (Func<PawnsArrivalModeDef, string>)delegate(PawnsArrivalModeDef str)
			{
				int num = modeCount[str];
				return GenText.ToStringPercent((float)num / 500f);
			}));
		}
		Find.TickManager.DebugSetTicksGame(ticksGame);
		DebugTables.MakeTablesDialog<PawnsArrivalModeDef>(DefDatabase<PawnsArrivalModeDef>.AllDefs, list.ToArray());
	}
}
