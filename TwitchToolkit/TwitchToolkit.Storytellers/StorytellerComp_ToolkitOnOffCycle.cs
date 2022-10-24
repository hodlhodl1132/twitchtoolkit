using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerComp_ToolkitOnOffCycle : StorytellerComp
{
	private Stopwatch stopwatch;

	protected StorytellerCompProperties_ToolkitOnOffCycle Props => (StorytellerCompProperties_ToolkitOnOffCycle)(object)base.props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		if (!(VoteHandler.currentVote is Vote_Milasandra) && !((float)GenDate.DaysPassed <= Props.minDaysPassed))
		{
			float acceptFraction = 1f;
			if (Props.acceptFractionByDaysPassedCurve != null)
			{
				acceptFraction *= Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
			}
			if (Props.acceptPercentFactorPerThreatPointsCurve != null)
			{
				acceptFraction *= Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
			}
			int rand = Rand.Range(1, 25);
			int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, rand, Props.minDaysPassed, Props.onDays, Props.offDays, Props.minSpacingDays, Props.numIncidentsRange.min, Props.numIncidentsRange.max, acceptFraction);
			for (int i = 0; i < incCount; i++)
			{
				GenerateIncident(target);
			}
		}
		yield break;
	}

	private FiringIncident GenerateIncident(IIncidentTarget target)
	{
		IncidentParms parms = ((StorytellerComp)this).GenerateParms(Props.IncidentCategory, target);
		if ((float)GenDate.DaysPassed < Props.forceRaidEnemyBeforeDaysPassed)
		{
			if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
			{
				return null;
			}
			GenerateForcedIncidents(target, parms);
			return null;
		}
		GenerateIncidentsByWeight(target, parms);
		return null;
	}

	private void GenerateForcedIncidents(IIncidentTarget target, IncidentParms parms)
	{
		List<IncidentDef> defs = (from def in UsableIncidentsInCategory(Props.IncidentCategory, parms)
			where parms.points >= def.minThreatPoints
			select def).ToList();
		GenerateVotesFromDefs(defs, parms);
	}

	private void GenerateIncidentsByWeight(IIncidentTarget target, IncidentParms parms)
	{
		List<IncidentDef> defs = new List<IncidentDef>();
		for (int i = 0; i < 3; i++)
		{
			if ((from def in UsableIncidentsInCategory(Props.IncidentCategory, parms)
				where parms.points >= def.minThreatPoints && !defs.Contains(def)
				select def).TryChooseRandomElementByWeight((Func<IncidentDef, float>)base.IncidentChanceFinal, out IncidentDef def2))
			{
				defs.Add(def2);
			}
		}
		GenerateVotesFromDefs(defs, parms);
	}

	private void GenerateVotesFromDefs(List<IncidentDef> defs, IncidentParms parms)
	{
		if (defs != null && defs.Count() > 2)
		{
			defs.Shuffle();
			Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
			for (int i = 0; i < defs.Count; i++)
			{
				incidents.Add(i, defs[i]);
			}
			VoteHandler.QueueVote(new Vote_Milasandra(incidents, (StorytellerComp)(object)this, parms, "Which Big Threat should occur?"));
		}
		else
		{
			Log.Error("Only generated " + defs.Count + " incidents");
		}
	}

	public override string ToString()
	{
		return ((StorytellerComp)this).ToString() + " (" + ((Props.incident == null) ? ((Def)Props.IncidentCategory).defName : ((Def)Props.incident).defName) + ")";
	}
}
