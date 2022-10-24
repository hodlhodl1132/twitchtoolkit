using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Hazards;

public class Tornados : Tornado
{
	public Tornados()
	{
		map = Helper.AnyPlayerMap;
	}

	public override void TryExecute()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0027: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002d: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0072: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0078: Unknown result type (might be due to invalid IL or missing erences)
		int count = 0;
		List<Thing> tornados = new List<Thing>();
		while (CellFinder.TryFindRandomCellInsideWith(cellRect, (Predicate<IntVec3>)((IntVec3 x) => CanSpawnTornadoAt(x, map)), out loc) && count < 3)
		{
			count++;
			Tornado tornado = (Tornado)GenSpawn.Spawn(ThingDefOf.Tornado, loc, map, (WipeMode)0);
			tornados.Add((Thing)(object)tornado);
		}
		string text = "A  mobile, destructive vortex of violently rotating winds have appeard. Seek safe shelter!";
		Find.LetterStack.ReceiveLetter((TaggedString)("Tornados"), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)(tornados), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
