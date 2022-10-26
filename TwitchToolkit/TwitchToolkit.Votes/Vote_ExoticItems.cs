using System;
using System.Collections.Generic;
using RimWorld;
using ToolkitCore;
using Verse;

namespace TwitchToolkit.Votes;

public class Vote_ExoticItems : Vote
{
	private Dictionary<int, List<Thing>> thingsOptions = null;

	public Vote_ExoticItems(Dictionary<int, List<Thing>> thingsOptions)
		: base(new List<int>(thingsOptions.Keys))
	{
		try
		{
			this.thingsOptions = thingsOptions;
		}
		catch (InvalidCastException e)
		{
			Helper.Log(e.Message);
		}
	}

	public override void EndVote()
	{
        Map map = Helper.AnyPlayerMap;
		Find.WindowStack.TryRemove(typeof(VoteWindow), true);
		Messages.Message(new Message("Chat voted for: " + VoteKeyLabel(DecideWinner()), MessageTypeDefOf.PositiveEvent), true);
		IntVec3 intVec = DropCellFinder.RandomDropSpot(map, true);
		DropPodUtility.DropThingsNear(intVec, map, (IEnumerable<Thing>)thingsOptions[DecideWinner()], 110, false, true, true, true);
		Find.LetterStack.ReceiveLetter(Translator.Translate("LetterLabelCargoPodCrash"), Translator.Translate("CargoPodCrash"), LetterDefOf.PositiveEvent, (LookTargets)(new TargetInfo(intVec, map, false)), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}

	public override void StartVote()
	{
		if (ToolkitSettings.VotingWindow || (!ToolkitSettings.VotingWindow && !ToolkitSettings.VotingChatMsgs))
		{
			VoteWindow window = new VoteWindow(this, "Which exotic item should the colony receive?");
			Find.WindowStack.Add((Window)(object)window);
		}
		if (!ToolkitSettings.VotingChatMsgs)
		{
			return;
		}
		TwitchWrapper.SendChatMessage("Which exotic item should the colony receive?");
		foreach (KeyValuePair<int, List<Thing>> pair in thingsOptions)
		{
			TwitchWrapper.SendChatMessage($"[{pair.Key + 1}]  {VoteKeyLabel(pair.Key)}");
		}
	}

	public override string VoteKeyLabel(int id)
	{
		string msg = ((Entity)thingsOptions[id][0]).LabelCap;
		for (int i = 1; i < thingsOptions[id].Count; i++)
		{
			msg = msg + ", " + ((Entity)thingsOptions[id][i]).LabelCap;
		}
		return msg;
	}
}
