using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class ChangeGender : IncidentHelperVariables
{
	private bool separateChannel = false;

	private Pawn pawn = null;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		this.separateChannel = separateChannel;
		Viewer = viewer;
		GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();
		if (!gameComponent.HasUserBeenNamed(viewer.username))
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " you must be in the colony to use this command.");
			return false;
		}
		pawn = gameComponent.PawnAssignedToUser(viewer.username);
		Helper.Log("changing gneder");
		return true;
	}

	public override void TryExecute()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_002b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0047: Unknown result type (might be due to invalid IL or missing erences)
		//IL_005c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0061: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0101: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0107: Invalid comparison between Unknown and I4
		//IL_0180: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01db: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing erences)
		if ((int)pawn.gender == 2)
		{
			pawn.gender = (Gender)1;
		}
		else
		{
			pawn.gender = (Gender)2;
		}
		pawn.story.HairColor = PawnHairColors.RandomHairColor(pawn,pawn.story.SkinColor, pawn.ageTracker.AgeBiologicalYears);
		pawn.story.hairDef = HairDefOf.Bald;
		if (pawn.story.Adulthood != null)
		{
			pawn.story.bodyType = pawn.story.Adulthood.BodyTypeFor(pawn.gender);
		}
		else if (Rand.Value < 0.5f)
		{
			pawn.story.bodyType = BodyTypeDefOf.Thin;
		}
		else
		{
			pawn.story.bodyType = (((int)pawn.gender != 2) ? BodyTypeDefOf.Male : BodyTypeDefOf.Female);
		}
		Viewer.TakeViewerCoins(storeIncident.cost);
		Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
		VariablesHelpers.SendPurchaseMessage("@" + Viewer.username + " has just swapped genders to " + GenderUtility.GetLabel(pawn.gender, false) + ".");
		string text = Viewer.username + " has just swapped genders to " + GenderUtility.GetLabel(pawn.gender, false) + ".";
		Current.Game.letterStack.ReceiveLetter((TaggedString)("GenderSwap"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
