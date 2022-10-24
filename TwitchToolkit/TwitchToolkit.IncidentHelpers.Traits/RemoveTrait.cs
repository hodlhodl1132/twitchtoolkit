using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Traits;

public class RemoveTrait : IncidentHelperVariables
{
	private bool separateChannel = false;

	private Pawn pawn = null;

	private BuyableTrait buyableTrait = null;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		this.separateChannel = separateChannel;
		Viewer = viewer;
		string[] command = message.Split(' ');
		if (command.Length < 3)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " syntax is " + storeIncident.syntax);
			return false;
		}
		GameComponentPawns gameComponent = Current.Game.GetComponent<GameComponentPawns>();
		if (!gameComponent.HasUserBeenNamed(viewer.username))
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " you must be in the colony to use this command.");
			return false;
		}
		pawn = gameComponent.PawnAssignedToUser(viewer.username);
		if (pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Count <= 0)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " your pawn doesn't have any traits.");
			return false;
		}
		string traitKind = command[2].ToLower();
		BuyableTrait search = AllTraits.buyableTraits.Find((BuyableTrait s) => traitKind == s.label);
		if (search == null)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " trait " + traitKind + " not found.");
			return false;
		}
		buyableTrait = search;
		return true;
	}

	public override void TryExecute()
	{
		//IL_01ed: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing erences)
		Trait traitToRemove = pawn.story.traits.allTraits.Find((Trait s) => ((Def)s.def).defName == ((Def)buyableTrait.def).defName);
		if (traitToRemove == null)
		{
			return;
		}
		pawn.story.traits.allTraits.Remove(traitToRemove);
		TraitDegreeData traitDegreeData = traitToRemove.def.DataAtDegree(buyableTrait.degree);
		if (traitDegreeData != null && traitDegreeData.skillGains != null)
		{
			foreach (KeyValuePair<SkillDef, int> pair in traitDegreeData.skillGains)
			{
				SkillRecord skill = pawn.skills.GetSkill(pair.Key);
				skill.Level = (skill.Level - pair.Value);
			}
		}
		Viewer.TakeViewerCoins(storeIncident.cost);
		Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
		VariablesHelpers.SendPurchaseMessage(string.Concat("@", Viewer.username, " just removed the trait ", GenText.CapitalizeFirst(buyableTrait.label), " from ", pawn.Name, "."));
		string text = string.Concat(Viewer.username, " has purchased trait removal of ", GenText.CapitalizeFirst(buyableTrait.label), " from ", pawn.Name, ".");
		Current.Game.letterStack.ReceiveLetter((TaggedString)("Trait"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
