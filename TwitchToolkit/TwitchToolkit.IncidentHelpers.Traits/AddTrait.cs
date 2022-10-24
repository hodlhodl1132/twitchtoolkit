using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Traits;

public class AddTrait : IncidentHelperVariables
{
	private bool separateChannel = false;

	private Pawn pawn = null;

	private TraitDef traitDef = null;

	private Trait trait = null;

	private BuyableTrait buyableTrait = null;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		//IL_01de: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01e8: Expected O, but got Unknown
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
		float customMaxTraits = ((AddTraitSettings.maxTraits > 0) ? AddTraitSettings.maxTraits : 4);
		if (pawn.story.traits.allTraits != null && (float)pawn.story.traits.allTraits.Count >= customMaxTraits)
		{
			TwitchWrapper.SendChatMessage($"@{viewer.username} your pawn already has max {customMaxTraits} traits.");
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
		traitDef = buyableTrait.def;
		trait = new Trait(traitDef, buyableTrait.degree, false);
		foreach (Trait tr in pawn.story.traits.allTraits)
		{
			if (tr.def.ConflictsWith(trait) || traitDef.ConflictsWith(tr))
			{
				TwitchWrapper.SendChatMessage("@" + viewer.username + " " + ((Def)traitDef).defName + " conflicts with your pawn's trait " + tr.LabelCap + ".");
				return false;
			}
		}
		if (pawn.story.traits.allTraits != null && pawn.story.traits.allTraits.Find((Trait s) => ((Def)s.def).defName == ((Def)search.def).defName) != null)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " you already have this trait of this type.");
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		//IL_01ab: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing erences)
		pawn.story.traits.GainTrait(trait);
		TraitDegreeData traitDegreeData = traitDef.DataAtDegree(buyableTrait.degree);
		if (traitDegreeData != null && traitDegreeData.skillGains != null)
		{
			foreach (KeyValuePair<SkillDef, int> pair in traitDegreeData.skillGains)
			{
				SkillRecord skill = pawn.skills.GetSkill(pair.Key);
				int num = TraitHelpers.FinalLevelOfSkill(pawn, pair.Key);
				skill.Level = (num);
			}
		}
		Viewer.TakeViewerCoins(storeIncident.cost);
		Viewer.CalculateNewKarma(storeIncident.karmaType, storeIncident.cost);
		VariablesHelpers.SendPurchaseMessage(string.Concat("@", Viewer.username, " just added the trait ", trait.Label, " to ", pawn.Name, "."));
		string text = string.Concat(Viewer.username, " has purchased ", trait.LabelCap, " for ", pawn.Name, ".");
		Current.Game.letterStack.ReceiveLetter((TaggedString)("Trait"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
