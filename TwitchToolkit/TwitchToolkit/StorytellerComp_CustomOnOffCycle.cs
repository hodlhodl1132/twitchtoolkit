using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit;

public class StorytellerComp_CustomOnOffCycle : StorytellerComp
{
	private IEnumerable<IncidentDef> options;

	protected StorytellerCompProperties_CustomOnOffCycle Props => (StorytellerCompProperties_CustomOnOffCycle)(object)base.props;

	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		if (VoteHandler.voteActive)
		{
			yield break;
		}
		float acceptFraction = 1f;
		if (Props.acceptFractionByDaysPassedCurve != null)
		{
			acceptFraction *= Props.acceptFractionByDaysPassedCurve.Evaluate(GenDate.DaysPassedFloat);
		}
		if (Props.acceptPercentFactorPerThreatPointsCurve != null)
		{
			acceptFraction *= Props.acceptPercentFactorPerThreatPointsCurve.Evaluate(StorytellerUtility.DefaultThreatPointsNow(target));
		}
		int incCount = IncidentCycleUtility.IncidentCountThisInterval(target, Find.Storyteller.storytellerComps.IndexOf((StorytellerComp)(object)this), ((StorytellerCompProperties)Props).minDaysPassed, Props.onDays, Props.offDays, Props.minSpacingDays, Props.numIncidentsRange.min, Props.numIncidentsRange.max, acceptFraction);
		for (int i = 0; i < incCount; i++)
		{
			Helper.Log("Trying to gen OFC Inc");
			FiringIncident fi = GenerateIncident(target);
			if (fi != null)
			{
				yield return fi;
			}
		}
	}

	private FiringIncident GenerateIncident(IIncidentTarget target)
	{
		//IL_02ae: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02b5: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing erences)
		//IL_02d1: Expected O, but got Unknown
		Helper.Log("Trying OnOffCycle Incident");
		List<IncidentDef> pickedoptions = new List<IncidentDef>();
		IncidentParms parms = ((StorytellerComp)this).GenerateParms(Props.IncidentCategory, target);
		IncidentDef def2 = default(IncidentDef);
		if ((float)GenDate.DaysPassed < Props.forceRaidEnemyBeforeDaysPassed)
		{
			if (!IncidentDefOf.RaidEnemy.Worker.CanFireNow(parms))
			{
				return null;
			}
			def2 = IncidentDefOf.RaidEnemy;
		}
		else
		{
			if (Props.incident == null)
			{
				options = from def in UsableIncidentsInCategory(Props.IncidentCategory, parms)
					where parms.points >= def.minThreatPoints
					select def;
				Helper.Log($"Trying OFC Category: ${Props.IncidentCategory}");
				if (GenCollection.TryRandomElementByWeight<IncidentDef>(options, (Func<IncidentDef, float>)base.IncidentChanceFinal, out def2))
				{
					if (options.Count() > 1)
					{
						options = options.Where((IncidentDef k) => k != def2);
						pickedoptions.Add(def2);
						IncidentDef picked = default(IncidentDef);
						for (int x = 0; x < ToolkitSettings.VoteOptions - 1 && x < options.Count(); x++)
						{
							GenCollection.TryRandomElementByWeight<IncidentDef>(options, (Func<IncidentDef, float>)base.IncidentChanceFinal, out picked);
							if (picked != null)
							{
								options = options.Where((IncidentDef k) => k != picked);
								pickedoptions.Add(picked);
							}
						}
						Dictionary<int, IncidentDef> incidents = new Dictionary<int, IncidentDef>();
						for (int i = 0; i < pickedoptions.Count(); i++)
						{
							incidents.Add(i, pickedoptions.ToList()[i]);
						}
						VoteHandler.QueueVote(new VoteIncidentDef(incidents, (StorytellerComp)(object)this, parms));
						Helper.Log("Events created");
						return null;
					}
					if (options.Count() == 1)
					{
						Helper.Log("Firing one incident OFC");
						return new FiringIncident(def2, (StorytellerComp)(object)this, parms);
					}
				}
				return null;
			}
			if (!Props.incident.Worker.CanFireNow(parms))
			{
				return null;
			}
			def2 = Props.incident;
		}
		return new FiringIncident(def2, (StorytellerComp)(object)this, parms);
	}

	public override string ToString()
	{
		return ((StorytellerComp)this).ToString() + " (" + ((Props.incident == null) ? ((Def)Props.IncidentCategory).defName : ((Def)Props.incident).defName) + ")";
	}
}
