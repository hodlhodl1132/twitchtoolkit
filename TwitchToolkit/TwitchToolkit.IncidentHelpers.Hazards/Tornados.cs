using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Hazards;

public class Tornados : Tornado
{
	public override void TryExecute()
	{
        int count = 0;
		List<Thing> tornados = new List<Thing>();
		while (CellFinder.TryFindRandomCellInsideWith(cellRect, (Predicate<IntVec3>)((IntVec3 x) => CanSpawnTornadoAt(x, map)), out loc) && count < 3)
		{
			count++;
			Tornado tornado = (Tornado)GenSpawn.Spawn(ThingDefOf.Tornado, loc, map);
			tornados.Add((Thing)(object)tornado);
		}
		string text = "A  mobile, destructive vortex of violently rotating winds have appeard. Seek safe shelter!";
		Find.LetterStack.ReceiveLetter((TaggedString)("Tornados"), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)(tornados));
	}
}
