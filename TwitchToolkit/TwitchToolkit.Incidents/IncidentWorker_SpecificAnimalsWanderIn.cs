using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Incidents;

public class IncidentWorker_SpecificAnimalsWanderIn : IncidentWorker
{
	private string Label;

	private PawnKindDef PawnKindDef;

	private bool JoinColony;

	private int Count;

	private bool Manhunter;

	private bool DefaultText;

	private const float MaxWildness = 0.35f;

	private const float TotalBodySizeToSpawn = 2.5f;

	public IncidentWorker_SpecificAnimalsWanderIn(string label = null, PawnKindDef pawnKindDef = null, bool joinColony = false, int count = 0, bool manhunter = false, bool defaultText = false)
	{
		Label = label;
		PawnKindDef = pawnKindDef;
		JoinColony = joinColony;
		Count = count;
		Manhunter = manhunter;
		DefaultText = defaultText;
	}

	private bool TryFindAnimalKind(int tile, out PawnKindDef animalKind)
	{
		return GenCollection.TryRandomElementByWeight<PawnKindDef>(from k in DefDatabase<PawnKindDef>.AllDefs
			where k.RaceProps.Animal && k.RaceProps.wildness < 0.35f && Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, k.race)
			select k, (Func<PawnKindDef, float>)((PawnKindDef k) => 0.420000017f - k.RaceProps.wildness), out animalKind);
	}

	protected override bool CanFireNowSub(IncidentParms parms)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0020: Expected O, but got Unknown
		if (!CanFireNowSub(parms))
		{
			return false;
		}
		Map map = (Map)parms.target;
		IntVec3 intVec = default(IntVec3);
		return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, (Predicate<IntVec3>)null);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003f: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0143: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0148: Unknown result type (might be due to invalid IL or missing erences)
		//IL_014d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0162: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0167: Unknown result type (might be due to invalid IL or missing erences)
		//IL_017c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_017f: Unknown result type (might be due to invalid IL or missing erences)
		if (PawnKindDef == null && !TryFindAnimalKind(parms.target.Tile, out PawnKindDef))
		{
			return false;
		}
		Map map = (Map)parms.target;
		IntVec3 intVec = default(IntVec3);
		if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, (Predicate<IntVec3>)null))
		{
			return false;
		}
		if (Count <= 0)
		{
			Count = Mathf.Clamp(GenMath.RoundRandom(2.5f / PawnKindDef.RaceProps.baseBodySize), 2, 10);
		}
		for (int i = 0; i < Count; i++)
		{
			IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 12, (Predicate<IntVec3>)null);
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDef, (Faction)null);
			GenSpawn.Spawn((Thing)(object)pawn, loc, map, Rot4.Random, (WipeMode)0, false);
			if (JoinColony)
			{
				((Thing)pawn).SetFaction(Faction.OfPlayer, (Pawn)null);
			}
			if (Manhunter)
			{
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, (string)null, false, false, (Pawn)null, false, false, false);
			}
		}
		TaggedString text = TranslatorFormattedStringExtensions.Translate("LetterFarmAnimalsWanderIn", (NamedArgument)(PawnKindDef.GetLabelPlural(-1)));
		Find.LetterStack.ReceiveLetter((TaggedString)(Label ?? "LetterLabelFarmAnimalsWanderIn"), text, Manhunter ? LetterDefOf.NegativeEvent : LetterDefOf.PositiveEvent, (LookTargets)(new TargetInfo(intVec, map, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		return true;
	}
}
