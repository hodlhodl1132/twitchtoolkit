using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Diseases;

public abstract class Infection : VotingHelper
{
	public float percentAffected;

	private List<Pawn> candidates;

	public override bool IsPossible()
	{
		candidates = target.PlayerPawnsForStoryteller.ToList();
		candidates.Shuffle();
		int count = (int)Math.Round((float)candidates.Count * percentAffected);
		candidates = candidates.Take(count).ToList();
		if (candidates.Count > 0)
		{
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0040: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0060: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0071: Unknown result type (might be due to invalid IL or missing erences)
		//IL_007b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0104: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0109: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0114: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0119: Unknown result type (might be due to invalid IL or missing erences)
		//IL_011d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0161: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0177: Unknown result type (might be due to invalid IL or missing erences)
		//IL_017d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing erences)
		string label = "";
		string text = "";
		foreach (Pawn pawn in candidates)
		{
			BodyPartRecord part = pawn.health.hediffSet.GetRandomNotMissingPart(new DamageDef(), (BodyPartHeight)0, (BodyPartDepth)0, (BodyPartRecord)null);
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.WoundInfection, pawn, part);
			label = (TaggedString)(Translator.Translate("LetterLabelNewDisease") + " (" + ((Def)hediff.def).label + ")");
			HediffCompProperties_Discoverable d = hediff.def.CompProps<HediffCompProperties_Discoverable>();
			d.sendLetterWhenDiscovered = false;
			pawn.health.hediffSet.hediffs.Add(hediff);
			if (candidates.Count <= 1)
			{
				TaggedString val = TranslatorFormattedStringExtensions.Translate("NewPartDisease", NamedArgumentUtility.Named((object)pawn, "PAWN"), (NamedArgument)(part.Label), (NamedArgument)(GenText.LabelDefinite(pawn)), (NamedArgument)(((Def)hediff.def).label));
				val = ((TaggedString)(val)).AdjustedFor(pawn, "PAWN", true);
				text = (TaggedString)(((TaggedString)(val)).CapitalizeFirst());
			}
		}
		if (candidates.Count > 1)
		{
			text = (TaggedString)(Translator.Translate("TwitchStoriesDescription18"));
			Current.Game.letterStack.ReceiveLetter((TaggedString)(label), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)(candidates), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		}
		else
		{
			Current.Game.letterStack.ReceiveLetter((TaggedString)(label), (TaggedString)(text), LetterDefOf.NegativeEvent, (LookTargets)((Thing)(object)candidates[0]), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
		}
	}
}
