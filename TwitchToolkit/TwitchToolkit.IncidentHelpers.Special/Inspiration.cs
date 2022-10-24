using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class Inspiration : IncidentHelperVariables
{
	public bool separateChannel = false;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		this.separateChannel = separateChannel;
		string[] command = message.Split(' ');
		if (command.Length - 2 < storeIncident.variables)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " syntax is " + storeIncident.syntax);
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		List<Pawn> pawns = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned.ToList();
		pawns.Shuffle();
		bool successfulInspiration = false;
		foreach (Pawn pawn in pawns)
		{
			if (pawn.Inspired)
			{
				continue;
			}
			InspirationDef randomAvailableInspirationDef = GenCollection.RandomElementByWeightWithFallback<InspirationDef>(from x in DefDatabase<InspirationDef>.AllDefsListForReading
				where true
				select x, (Func<InspirationDef, float>)((InspirationDef x) => x.Worker.CommonalityFor(pawn)), (InspirationDef)null);
			if (randomAvailableInspirationDef != null)
			{
				successfulInspiration = pawn.mindState.inspirationHandler.TryStartInspiration(randomAvailableInspirationDef, (string)null, true);
				if (successfulInspiration)
				{
					break;
				}
			}
		}
		if (successfulInspiration)
		{
			Viewer.TakeViewerCoins(storeIncident.cost);
			Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
			VariablesHelpers.SendPurchaseMessage("@" + Viewer.username + " purchased a random inspiration.");
		}
		else
		{
			VariablesHelpers.SendPurchaseMessage("@" + Viewer.username + " attempted to inspired a pawn, but none were successful.");
		}
	}
}
