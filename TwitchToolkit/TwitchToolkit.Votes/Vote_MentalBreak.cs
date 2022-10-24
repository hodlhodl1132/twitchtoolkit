using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using ToolkitCore;
using Verse;
using Verse.AI;

namespace TwitchToolkit.Votes;

public class Vote_MentalBreak : Vote
{
	private Dictionary<int, Pawn> pawnOptions = null;

	public Vote_MentalBreak(Dictionary<int, Pawn> pawnOptions)
		: base(new List<int>(pawnOptions.Keys))
	{
		try
		{
			this.pawnOptions = pawnOptions;
		}
		catch (InvalidCastException e)
		{
			Helper.Log(e.Message);
		}
	}

	public override void EndVote()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_008d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0092: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0097: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0114: Unknown result type (might be due to invalid IL or missing erences)
		//IL_011f: Expected O, but got Unknown
		Pawn pawn = pawnOptions[DecideWinner()];
		float minorBreak = pawn.mindState.mentalBreaker.BreakThresholdMinor - 0.05f;
		IEnumerable<MentalBreakDef> breaks = from d in DefDatabase<MentalBreakDef>.AllDefsListForReading
			where (int)d.intensity == 1 && d.Worker.BreakCanOccur(pawn)
			select d;
		MentalBreakDef mentalBreakDef = default(MentalBreakDef);
		bool breakPawn = GenCollection.TryRandomElementByWeight<MentalBreakDef>(breaks, (Func<MentalBreakDef, float>)((MentalBreakDef d) => d.Worker.CommonalityFor(pawn, false)),out  mentalBreakDef);
		string text = (TaggedString)(Translator.Translate("MentalStateReason_Mood"));
		text = (TaggedString)(text + "\n\n" + TranslatorFormattedStringExtensions.Translate("FinalStraw", (NamedArgument)("Chat said something that upset them.")));
		if (!RestUtility.Awake(pawn))
		{
			pawn.jobs.EndCurrentJob((JobCondition)5, true, true);
		}
		if (breakPawn && mentalBreakDef.Worker.TryStart(pawn, text, false))
		{
			Messages.Message(new Message("Chat caused a mental break for: " + ((Entity)pawnOptions[DecideWinner()]).LabelCap, MessageTypeDefOf.NegativeEvent), true);
		}
		Find.WindowStack.TryRemove(typeof(VoteWindow), true);
	}

	public override void StartVote()
	{
		if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
		{
			VoteWindow window = new VoteWindow(this, "Which colonist should experience a mental break?");
			Find.WindowStack.Add((Window)(object)window);
		}
		if (!ToolkitSettings.VotingChatMsgs)
		{
			return;
		}
		TwitchWrapper.SendChatMessage("Which colonist should experience a mental break?");
		foreach (KeyValuePair<int, Pawn> pair in pawnOptions)
		{
			TwitchWrapper.SendChatMessage($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
		}
	}

	public override string VoteKeyLabel(int id)
	{
		Name name = pawnOptions[id].Name;
		string nick = ((NameTriple)((name is NameTriple) ? name : null)).Nick;
		if (nick != null)
		{
			return nick;
		}
		return ((object)pawnOptions[id].Name).ToString();
	}
}
