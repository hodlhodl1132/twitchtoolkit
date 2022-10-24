using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class ReviveRandomPawn : IncidentHelper
{
	private Pawn pawn = null;

	public override bool IsPossible()
	{
		List<Pawn> allCorpses = (from s in Find.ColonistBar.GetColonistsInOrder()
			where s.Dead && ((Thing)s).SpawnedOrAnyParentSpawned && !PawnTracker.pawnsToRevive.Contains(s)
			select s).ToList();
		if (allCorpses == null || allCorpses.Count < 1)
		{
			return false;
		}
		allCorpses.Shuffle();
		pawn = allCorpses[0];
		PawnTracker.pawnsToRevive.Add(pawn);
		return true;
	}

	public override void TryExecute()
	{
		try
		{
            Thing thing;
            if (this.pawn.SpawnedParentOrMe != this.pawn.Corpse && this.pawn.SpawnedParentOrMe is Pawn spawnedParentOrMe && !spawnedParentOrMe.carryTracker.TryDropCarriedThing(spawnedParentOrMe.Position, ThingPlaceMode.Near, out thing, (Action<Thing, int>)null))
            {
                Log.Error(string.Format("Submit this bug to TwitchToolkit Discord: Could not drop {0} at {1} from {2}", (object)this.pawn, (object)spawnedParentOrMe.Position, (object)spawnedParentOrMe), false);
            }
            else
            {
                this.pawn.ClearAllReservations();
                ResurrectionUtility.ResurrectWithSideEffects(this.pawn);
                PawnTracker.pawnsToRevive.Remove(this.pawn);
                Find.LetterStack.ReceiveLetter("Pawn Revived", string.Format("{0} has been revived but is experiencing some side effects.", (object)this.pawn.Name), LetterDefOf.PositiveEvent, (LookTargets)(Thing)this.pawn, (Faction)null);
            }
        }
		catch (Exception e)
		{
			Log.Error("Submit this bug to TwitchToolkit Discord: " + e.Message);
		}
	}
}
