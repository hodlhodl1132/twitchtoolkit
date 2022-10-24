using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI.Group;

namespace TwitchToolkit.Incidents;

public class IncidentWorker_VisitColony : IncidentWorker_NeutralGroup
{
	private const float TraderChance = 0.75f;

	private static readonly SimpleCurve PointsCurve;

	private Viewer viewer;

	public IncidentWorker_VisitColony()
	{
	}

	public IncidentWorker_VisitColony(Viewer viewer = null)
	{
		this.viewer = viewer;
	}

	protected override bool CanFireNowSub(IncidentParms parms)
	{
		if (viewer == null)
		{
			return false;
		}
		return CanFireNowSub(parms);
	}

	protected override bool TryExecuteWorker(IncidentParms parms)
	{
		Map map = (Map)parms.target;
		if (!TryResolveParms(parms))
		{
			return false;
		}
		List<Pawn> list = SpawnPawns(parms);
		if (list.Count == 0)
		{
			return false;
		}
		IntVec3 chillSpot = default(IntVec3);
		RCellFinder.TryFindRandomSpotJustOutsideColony(list[0],out chillSpot);
		LordJob_VisitColony lordJob = new LordJob_VisitColony(parms.faction, chillSpot, (int?)null);
		LordMaker.MakeNewLord(parms.faction, (LordJob)(object)lordJob, map, (IEnumerable<Pawn>)list);
		bool flag = false;
		if (Rand.Value < 0.75f)
		{
			flag = TryConvertOnePawnToSmallTrader(list, parms.faction, map);
		}
		Pawn pawn = list.Find((Pawn x) => parms.faction.leader == x);
		Pawn viewerPawn = list.Find((Pawn x) => parms.faction.leader != x);
		Name name = viewerPawn.Name;
		NameTriple oldName = (NameTriple)(object)((name is NameTriple) ? name : null);
		NameTriple newName = new NameTriple(oldName.First, GenText.CapitalizeFirst(viewer.username), oldName.Last);
		viewerPawn.Name = ((Name)(object)newName);
		TaggedString val;
		TaggedString label;
		TaggedString text;
		if (list.Count == 1)
		{
			string text2;
			if (flag)
			{
				val = TranslatorFormattedStringExtensions.Translate("SingleVisitorArrivesTraderInfo", NamedArgumentUtility.Named((object)list[0], "PAWN"));
				val = ((TaggedString)(val)).AdjustedFor(list[0], "PAWN", true);
				text2 = "\n\n" + ((object)(TaggedString)(val)).ToString();
			}
			else
			{
				text2 = string.Empty;
			}
			string value = text2;
			string text3;
			if (pawn != null)
			{
				val = TranslatorFormattedStringExtensions.Translate("SingleVisitorArrivesLeaderInfo", NamedArgumentUtility.Named((object)list[0], "PAWN"));
				val = "\n\n" + ((TaggedString)(val)).AdjustedFor(list[0], "PAWN", true);
				text3 = ((object)(TaggedString)(val)).ToString();
			}
			else
			{
				text3 = string.Empty;
			}
			string value2 = text3;
			label = Translator.Translate("LetterLabelSingleVisitorArrives");
			val = TranslatorFormattedStringExtensions.Translate("SingleVisitorArrives", (NamedArgument)(list[0].story.Title), (NamedArgument)(parms.faction.Name), (NamedArgument)(list[0].Name.ToStringFull), (NamedArgument)(value), (NamedArgument)(value2), NamedArgumentUtility.Named((object)list[0], "PAWN"));
			text = ((TaggedString)(val)).AdjustedFor(list[0], "PAWN", true);
		}
		else
		{
			string text4;
			if (flag)
			{
				val = "\n\n" + Translator.Translate("GroupVisitorsArriveTraderInfo");
				text4 = ((object)(TaggedString)(val)).ToString();
			}
			else
			{
				text4 = string.Empty;
			}
			string value3 = text4;
			string text5;
			if (pawn != null)
			{
				val = "\n\n" + TranslatorFormattedStringExtensions.Translate("GroupVisitorsArriveLeaderInfo", (NamedArgument)(((Entity)pawn).LabelShort), (NamedArgument)((Thing)(object)pawn));
				text5 = ((object)(TaggedString)(val)).ToString();
			}
			else
			{
				text5 = string.Empty;
			}
			string value4 = text5;
			label = Translator.Translate("LetterLabelGroupVisitorsArrive");
			text = TranslatorFormattedStringExtensions.Translate("GroupVisitorsArrive", (NamedArgument)(parms.faction.Name), (NamedArgument)(value3), (NamedArgument)(value4));
		}
		PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter((IEnumerable<Pawn>)list, ref label, ref text, (TaggedString)(TranslatorFormattedStringExtensions.Translate("LetterRelatedPawnsNeutralGroup", (NamedArgument)(Faction.OfPlayer.def.pawnsPlural))), true, true);
		Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent, (LookTargets)((Thing)(object)list[0]), parms.faction, (Quest)null, (List<ThingDef>)null, (string)null);
		return true;
	}

	private bool TryConvertOnePawnToSmallTrader(List<Pawn> pawns, Faction faction, Map map)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing erences)
		//IL_011b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0123: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0128: Unknown result type (might be due to invalid IL or missing erences)
		//IL_012c: Unknown result type (might be due to invalid IL or missing erences)
		if (GenList.NullOrEmpty<TraderKindDef>((IList<TraderKindDef>)faction.def.visitorTraderKinds))
		{
			return false;
		}
		Pawn pawn = GenCollection.RandomElement<Pawn>((IEnumerable<Pawn>)pawns);
		Lord lord = LordUtility.GetLord(pawn);
		pawn.mindState.wantsToTradeWithColony = true;
		PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
		TraderKindDef traderKindDef = GenCollection.RandomElementByWeight<TraderKindDef>((IEnumerable<TraderKindDef>)faction.def.visitorTraderKinds, (Func<TraderKindDef, float>)((TraderKindDef traderDef) => traderDef.CalculatedCommonality));
		pawn.trader.traderKind = traderKindDef;
		pawn.inventory.DestroyAll((DestroyMode)0);
		ThingSetMakerParams parms = default(ThingSetMakerParams);
		parms.traderDef = traderKindDef;
		parms.tile = map.Tile;
		parms.makingFaction = faction;
		foreach (Thing thing in ThingSetMakerDefOf.TraderStock.root.Generate(parms))
		{
			Pawn pawn2 = (Pawn)(object)((thing is Pawn) ? thing : null);
			if (pawn2 != null)
			{
				if (((Thing)pawn2).Faction != ((Thing)pawn).Faction)
				{
					((Thing)pawn2).SetFaction(((Thing)pawn).Faction, (Pawn)null);
				}
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(((Thing)pawn).Position, map, 5, (Predicate<IntVec3>)null);
				GenSpawn.Spawn((Thing)(object)pawn2, loc, map, (WipeMode)0);
				lord.AddPawn(pawn2);
			}
			else if (!((ThingOwner)pawn.inventory.innerContainer).TryAdd(thing, true))
			{
				thing.Destroy((DestroyMode)0);
			}
		}
		PawnInventoryGenerator.GiveRandomFood(pawn);
		return true;
	}

	protected override void ResolveParmsPoints(IncidentParms parms)
	{
		if (!(parms.points >= 0f))
		{
			parms.points = Rand.ByCurve(PointsCurve);
		}
	}

	static IncidentWorker_VisitColony()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0005: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0010: Unknown result type (might be due to invalid IL or missing erences)
		//IL_001c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0027: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0033: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0055: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0061: Unknown result type (might be due to invalid IL or missing erences)
		//IL_006c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0078: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0083: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0094: Expected O, but got Unknown
		SimpleCurve val = new SimpleCurve();
		val.Add(new CurvePoint(45f, 0f), true);
		val.Add(new CurvePoint(50f, 1f), true);
		val.Add(new CurvePoint(100f, 1f), true);
		val.Add(new CurvePoint(200f, 0.25f), true);
		val.Add(new CurvePoint(300f, 0.1f), true);
		val.Add(new CurvePoint(500f, 0f), true);
		PointsCurve = val;
	}
}
