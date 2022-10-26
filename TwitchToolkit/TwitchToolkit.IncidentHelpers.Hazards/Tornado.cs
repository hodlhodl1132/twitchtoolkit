using System;
using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Hazards;

public class Tornado : IncidentHelper
{
	internal IntVec3 loc;

	internal CellRect cellRect;

	internal readonly Map map;

	public Tornado()
	{
		map = Helper.AnyPlayerMap;
	}

	public Tornado(IntVec3 location, CellRect cell, Map Map)
	{
		loc = location;
		cellRect = cell;
		map = Map;
	}

    public static explicit operator Tornado(Thing source)
    {
		return new Tornado(location: source.Position,
			cell: (CellRect)source.CustomRectForSelector,
			Map: source.Map);
    }

    public override bool IsPossible()
	{
		CellRect val = CellRect.WholeMap(map);
		cellRect = ((CellRect)(val)).ContractedBy(30);
		if (((CellRect)(cellRect)).IsEmpty)
		{
			cellRect = CellRect.WholeMap(map);
		}
		if (!CellFinder.TryFindRandomCellInsideWith(cellRect, (Predicate<IntVec3>)((IntVec3 x) => CanSpawnTornadoAt(x, map)), out loc))
		{
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		Tornado tornado = (Tornado)GenSpawn.Spawn(ThingDefOf.Tornado, loc, map, 0);
		string text = "A  mobile, destructive vortex of violently rotating winds have appeard. Seek safe shelter!";
		Find.LetterStack.ReceiveLetter((TaggedString)("Tornado"), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)((Thing)(object)tornado));
	}

	protected bool CanSpawnTornadoAt(IntVec3 c, Map map)
	{
		if (GridsUtility.Fogged(c, map))
		{
			return false;
		}
		int num = GenRadial.NumCellsInRadius(7f);
		for (int i = 0; i < num; i++)
		{
			IntVec3 c2 = c + GenRadial.RadialPattern[i];
			if (GenGrid.InBounds(c2, map))
			{
				if (AnyPawnOfPlayerFactionAt(c2, map))
				{
					return false;
				}
				RoofDef roofDef = map.roofGrid.RoofAt(c2);
				if (roofDef != null && roofDef.isThickRoof)
				{
					return false;
				}
			}
		}
		return true;
	}

	protected bool AnyPawnOfPlayerFactionAt(IntVec3 c, Map map)
	{
		List<Thing> thingList = GridsUtility.GetThingList(c, map);
		for (int i = 0; i < thingList.Count; i++)
		{
			Thing obj = thingList[i];
			Pawn pawn = (Pawn)(object)((obj is Pawn) ? obj : null);
			if (pawn != null && ((Thing)pawn).Faction == Faction.OfPlayer)
			{
				return true;
			}
		}
		return false;
	}
}
