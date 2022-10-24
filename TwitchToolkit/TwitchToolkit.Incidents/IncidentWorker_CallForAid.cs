using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace TwitchToolkit.Incidents;

public class IncidentWorker_CallForAid : IncidentWorker_RaidFriendly
{
	protected override bool CanFireNowSub(IncidentParms parms)
	{
		return true;
	}

	protected override bool TryResolveRaidFaction(IncidentParms parms)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Expected O, but got Unknown
		Map map = (Map)parms.target;
		if (parms.faction != null)
		{
			return true;
		}
		if (!CandidateFactions(map, desperate: true).Any())
		{
			return false;
		}
		parms.faction = GenCollection.RandomElementByWeight<Faction>(CandidateFactions(map, desperate: true), (Func<Faction, float>)((Faction fac) => (float)fac.PlayerGoodwill + 120.000008f));
		return true;
	}

	protected IEnumerable<Faction> CandidateFactions(Map map, bool desperate = false)
	{
		return from f in Find.FactionManager.AllFactions
			where FactionCanBeGroupSource(f, map, desperate)
			select f;
	}

	protected override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = true)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002f: Invalid comparison between Unknown and I4
		return f.def != FactionDefOf.PlayerColony && f.def != FactionDefOf.PlayerTribe && !f.def.hidden && (int)f.PlayerRelationKind >= 1;
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		//IL_01aa: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01af: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing erences)
		//IL_023b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0283: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing erences)
		ResolveRaidPoints(parms);
		if (!TryResolveRaidFaction(parms))
		{
			return false;
		}
		PawnGroupKindDef combat = PawnGroupKindDefOf.Combat;
		ResolveRaidStrategy(parms, combat);
		ResolveRaidArriveMode(parms);
		parms.raidStrategy.Worker.TryGenerateThreats(parms);
		if (!parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
		{
			return false;
		}
		parms.points = IncidentWorker_Raid.AdjustedRaidPoints(parms.points, parms.raidArrivalMode, parms.raidStrategy, parms.faction, combat);
		List<Pawn> list = parms.raidStrategy.Worker.SpawnThreats(parms);
		if (list == null)
		{
			list = PawnGroupMakerUtility.GeneratePawns(IncidentParmsUtility.GetDefaultPawnGroupMakerParms(combat, parms, false), true).ToList();
			if (list.Count == 0)
			{
				Log.Error("Got no pawns spawning raid from parms " + parms, false);
				return false;
			}
			parms.raidArrivalMode.Worker.Arrive(list, parms);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Points = " + parms.points.ToString("F0"));
		foreach (Pawn pawn in list)
		{
			string str = ((pawn.equipment != null && pawn.equipment.Primary != null) ? ((Entity)pawn.equipment.Primary).LabelCap : "unarmed");
			stringBuilder.AppendLine(pawn.KindLabel + " - " + str);
		}
		TaggedString baseLetterLabel = (TaggedString)(GetLetterLabel(parms));
		TaggedString baseLetterText = (TaggedString)(GetLetterText(parms, list));
		PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref baseLetterLabel, ref baseLetterText, GetRelatedPawnsInfoLetterText(parms), true, true);
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
			foreach (Pawn t in list)
			{
				list2.Add((TargetInfo)((Thing)(object)t));
			}
		}
		SendStandardLetter(baseLetterLabel, baseLetterText, GetLetterDef(), parms, (LookTargets)(list2), Array.Empty<NamedArgument>());
		parms.raidStrategy.Worker.MakeLords(parms, list);
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, (OpportunityType)2);
		if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts))
		{
			for (int j = 0; j < list.Count; j++)
			{
				if (GenCollection.Any<Apparel>(list[j].apparel.WornApparel, (Predicate<Apparel>)((Apparel ap) => ap is CompShield)))
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, (OpportunityType)2);
					break;
				}
			}
		}
		return true;
	}
}
