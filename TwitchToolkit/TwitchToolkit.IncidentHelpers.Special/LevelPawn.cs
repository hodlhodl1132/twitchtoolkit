using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using ToolkitCore;
using TwitchToolkit.IncidentHelpers.IncidentHelper_Settings;
using TwitchToolkit.PawnQueue;
using TwitchToolkit.Store;
using Verse;

namespace TwitchToolkit.IncidentHelpers.Special;

public class LevelPawn : IncidentHelperVariables
{
	private int pointsWager = 0;

	private bool separateChannel = false;

	private Pawn pawn = null;

	private SkillDef skill = null;

	public override Viewer Viewer { get; set; }

	public override bool IsPossible(string message, Viewer viewer, bool separateChannel = false)
	{
		this.separateChannel = separateChannel;
		Viewer = viewer;
		string[] command = message.Split(' ');
		if (command.Length < 4)
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
		if (!VariablesHelpers.PointsWagerIsValid(command[3], viewer,  pointsWager,  storeIncident, separateChannel))
		{
			return false;
		}
		string skillKind = command[2].ToLower();
		List<SkillDef> allSkills = (from s in DefDatabase<SkillDef>.AllDefs
			where string.Join("", ((Def)s).defName.Split(' ')).ToLower() == skillKind || string.Join("", ((Def)s).label.Split(' ')).ToLower() == skillKind
			select s).ToList();
		if (allSkills.Count < 1)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " skill " + skillKind + " not found.");
			return false;
		}
		skill = allSkills[0];
		pawn = gameComponent.PawnAssignedToUser(viewer.username);
		if (pawn.skills.GetSkill(skill).TotallyDisabled)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " skill " + skillKind + " disabled on your pawn.");
			return false;
		}
		if (pawn.skills.GetSkill(skill).levelInt >= 20)
		{
			TwitchWrapper.SendChatMessage("@" + viewer.username + " skill " + skillKind + " disabled on your pawn.");
			return false;
		}
		return true;
	}

	public override void TryExecute()
	{
		//IL_00f9: Unknown result type (might be due to invalid IL or missing erences)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0100: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0103: Invalid comparison between Unknown and I4
		//IL_011b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_011e: Invalid comparison between Unknown and I4
		//IL_014a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing erences)
		//IL_021b: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0222: Unknown result type (might be due to invalid IL or missing erences)
		float customMultiplier = ((LevelPawnSettings.xpMultiplier > 0f) ? LevelPawnSettings.xpMultiplier : 0.5f);
		float xpWon = pawn.skills.GetSkill(skill).XpRequiredForLevelUp * customMultiplier * Rand.Range(0.5f, 1.5f);
		xpWon = IncidentHelper_PointsHelper.RollProportionalGamePoints(storeIncident, pointsWager, xpWon);
		pawn.skills.Learn(skill, xpWon, true);
		Viewer.TakeViewerCoins(pointsWager);
		Viewer.CalculateNewKarma(storeIncident.karmaType, pointsWager);
		SkillRecord record = pawn.skills.GetSkill(skill);
		string increaseText = $" Level {record.levelInt}: {(int)record.xpSinceLastLevel} / {(int)record.XpRequiredForLevelUp}.";
		float percent = 35f;
		string passionPlus = "";
		Passion passion = record.passion;
		if ((int)passion == 1)
		{
			percent = 100f;
			passionPlus = "+";
		}
		if ((int)passion == 2)
		{
			percent = 150f;
			passionPlus = "++";
		}
		VariablesHelpers.SendPurchaseMessage($"Increasing skill {((Def)skill).LabelCap} for {((Entity)pawn).LabelCap} with {pointsWager} coins wagered and ({(int)xpWon} * {percent}%){passionPlus} {(float)(int)xpWon * (percent / 100f)} xp purchased by {Viewer.username}. {increaseText}");
		string text = Helper.ReplacePlaceholder((TaggedString)(Translator.Translate("TwitchStoriesDescription55")), ((object)pawn.Name).ToString(), null, null, null, ((Def)skill).defName, null, null, null, null, null, null, null, null, null, null, Math.Round(xpWon).ToString());
		Current.Game.letterStack.ReceiveLetter(Translator.Translate("TwitchToolkitIncreaseSkill"), (TaggedString)(text), LetterDefOf.PositiveEvent, (LookTargets)((Thing)(object)pawn), (Faction)null, (Quest)null, (List<ThingDef>)null, (string)null);
	}
}
