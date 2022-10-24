using System;
using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class BuyPawn : IncidentHelperVariables
{
	private IntVec3 loc;

	private Map map = null;

	private IncidentParms parms = null;

	private bool separateChannel = false;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0098: Expected O, but got Unknown
		if (!Purchase_Handler.CheckIfViewerHasEnoughCoins(viewer, storeIncident.cost))
		{
			return false;
		}
		if (Current.Game.GetComponent<GameComponentPawns>().HasUserBeenNamed(viewer.username))
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " you are already in the colony.");
			return false;
		}
		this.separateChannel = separateChannel;
		Viewer = viewer;
		IIncidentTarget target = (IIncidentTarget)(object)Helper.AnyPlayerMap;
		parms = StorytellerUtility.DefaultParmsNow(IncidentCategoryDefOf.Misc, target);
		map = (Map)parms.target;
		if (!CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => map.reachability.CanReachColony(c) && !GridsUtility.Fogged(c, map)), map, CellFinder.EdgeRoadChance_Neutral, out loc))
		{
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00ba: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing erences)
		//IL_010c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_010e: Unknown result type (might be due to invalid IL or missing erences)
		PawnKindDef pawnKind = PawnKindDefOf.Colonist;
		Faction ofPlayer = Faction.OfPlayer;
		PawnGenerationRequest request = new PawnGenerationRequest(pawnKind, ofPlayer, (PawnGenerationContext)2, map.Tile, false, false, false, false, true, 1f, false, false, true, true, true, false, false, false, false, 0f, 0f, (Pawn)null, 1f);
		Pawn pawn = PawnGenerator.GeneratePawn(request);
		Name name = pawn.Name;
		NameTriple old = (NameTriple)(object)((name is NameTriple) ? name : null);
		pawn.Name = ((Name)new NameTriple(old.First, Viewer.username, old.Last));
		GenSpawn.Spawn((Thing)(object)pawn, loc, map, (WipeMode)0);
		TaggedString label = (TaggedString)("Viewer Joins");
		TaggedString text = (TaggedString)("A new pawn has been purchased by " + Viewer.username + ", let's welcome them to the colony.");
		PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
		Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		Current.Game.GetComponent<GameComponentPawns>().AssignUserToPawn(Viewer.username, pawn);
		Viewer.TakeViewerCoins(storeIncident.cost);
		Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
		VariablesHelpers.SendPurchaseMessage("@" + Viewer.username + " has purchased a pawn and is joining the colony.");
	}
}
