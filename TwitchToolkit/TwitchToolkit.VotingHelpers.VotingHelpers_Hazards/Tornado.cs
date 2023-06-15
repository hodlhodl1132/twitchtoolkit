using System;
using System.Collections.Generic;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Hazards;

public class Tornado : VotingHelper
{
	internal IntVec3 loc;

	internal CellRect cellRect;

	internal Map map;

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
		if (((CellRect)( cellRect)).IsEmpty)
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
		Tornado tornado = (Tornado)GenSpawn.Spawn(ThingDefOf.Tornado, loc, map, (WipeMode)0);
		string text = "A  mobile, destructive vortex of violently rotating winds have appeard. Seek safe shelter!";
		Find.LetterStack.ReceiveLetter((TaggedString)("Tornado"), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)((Thing)(object)tornado), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}

	protected bool CanSpawnTornadoAt(IntVec3 c, Map map)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0024: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0030: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0035: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0037: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0047: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0060: Unknown result type (might be due to invalid IL or missing erences)
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
		//IL_0001: Unknown result type (might be due to invalid IL or missing erences)
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