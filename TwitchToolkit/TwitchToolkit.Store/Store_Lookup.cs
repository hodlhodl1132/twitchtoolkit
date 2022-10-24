using System.Collections.Generic;
using System.Linq;
using RimWorld;
using ToolkitCore;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.IncidentHelpers.Traits;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store;

public class Store_Lookup : TwitchInterfaceBase
{
	public Store_Lookup(Game game)
	{
	}

	public override void ParseMessage(ITwitchMessage twitchMessage)
	{
		if (twitchMessage.Message.StartsWith("!lookup"))
		{
			string[] command = twitchMessage.Message.Split(' ');
			if (command.Length < 2)
			{
				return;
			}
			string searchObject = command[1].ToLower();
			if (searchObject == null || searchObject == "")
			{
				return;
			}
			string searchQuery = null;
			if (command.Length > 2)
			{
				searchQuery = command[2].ToLower();
			}
			if (searchQuery == null)
			{
				searchQuery = "";
			}
			FindLookup(twitchMessage, searchObject, searchQuery);
		}
		Store_Logger.LogString("Finished lookup parse");
	}

	public void FindLookup(ITwitchMessage twitchMessage, string searchObject, string searchQuery)
	{
		//IL_0297: Unknown result type (might be due to invalid IL or missing erences)
		//IL_029c: Unknown result type (might be due to invalid IL or missing erences)
		List<string> results = new List<string>();
		switch (searchObject)
		{
		case "disease":
			FindLookup(twitchMessage, "diseases", searchQuery);
			break;
		case "skill":
			FindLookup(twitchMessage, "skills", searchQuery);
			break;
		case "event":
			FindLookup(twitchMessage, "events", searchQuery);
			break;
		case "item":
			FindLookup(twitchMessage, "items", searchQuery);
			break;
		case "animal":
			FindLookup(twitchMessage, "animals", searchQuery);
			break;
		case "trait":
			FindLookup(twitchMessage, "traits", searchQuery);
			break;
		case "diseases":
		{
			IncidentDef[] allDiseases = DefDatabase<IncidentDef>.AllDefs.Where(delegate(IncidentDef s)
			{
				//IL_0013: Unknown result type (might be due to invalid IL or missing erences)
				//IL_0018: Unknown result type (might be due to invalid IL or missing erences)
				//IL_004d: Unknown result type (might be due to invalid IL or missing erences)
				//IL_0052: Unknown result type (might be due to invalid IL or missing erences)
				int result;
				if (s.category == IncidentCategoryDefOf.DiseaseHuman)
				{
					TaggedString labelCap2 = ((Def)s).LabelCap;
					if (!string.Join("", ((TaggedString)( labelCap2)).RawText.Split(' ')).ToLower().Contains(searchQuery))
					{
						labelCap2 = ((Def)s).LabelCap;
						result = ((string.Join("", ((TaggedString)( labelCap2)).RawText.Split(' ')).ToLower() == searchQuery) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}).Take(10)
				.ToArray();
			IncidentDef[] array4 = allDiseases;
			foreach (IncidentDef disease in array4)
			{
				TaggedString labelCap = ((Def)disease).LabelCap;
				results.Add(string.Join("", ((TaggedString)( labelCap)).RawText.Split(' ')).ToLower());
			}
			SendTenResults(twitchMessage, GenText.CapitalizeFirst(searchObject), searchQuery, results.ToArray());
			break;
		}
		case "skills":
		{
			SkillDef[] allSkills = DefDatabase<SkillDef>.AllDefs.Where(delegate(SkillDef s)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing erences)
				//IL_000b: Unknown result type (might be due to invalid IL or missing erences)
				//IL_0040: Unknown result type (might be due to invalid IL or missing erences)
				//IL_0045: Unknown result type (might be due to invalid IL or missing erences)
				TaggedString labelCap4 = ((Def)s).LabelCap;
				int result3;
				if (!string.Join("", ((TaggedString)( labelCap4)).RawText.Split(' ')).ToLower().Contains(searchQuery))
				{
					labelCap4 = ((Def)s).LabelCap;
					result3 = ((string.Join("", ((TaggedString)( labelCap4)).RawText.Split(' ')).ToLower() == searchQuery) ? 1 : 0);
				}
				else
				{
					result3 = 1;
				}
				return (byte)result3 != 0;
			}).Take(10)
				.ToArray();
			SkillDef[] array2 = allSkills;
			foreach (SkillDef skill in array2)
			{
				results.Add(((Def)skill).defName.ToLower());
			}
			SendTenResults(twitchMessage, GenText.CapitalizeFirst(searchObject), searchQuery, results.ToArray());
			break;
		}
		case "events":
		{
			StoreIncident[] allEvents = (from s in DefDatabase<StoreIncident>.AllDefs
				where s.cost > 0 && (string.Join("", s.abbreviation.Split(' ')).ToLower().Contains(searchQuery) || string.Join("", s.abbreviation.Split(' ')).ToLower() == searchQuery || ((Def)s).defName.ToLower().Contains(searchQuery) || ((Def)s).defName.ToLower() == searchQuery)
				select s).Take(10).ToArray();
			StoreIncident[] array6 = allEvents;
			foreach (StoreIncident evt in array6)
			{
				results.Add(string.Join("", evt.abbreviation.Split(' ')).ToLower());
			}
			SendTenResults(twitchMessage, GenText.CapitalizeFirst(searchObject), searchQuery, results.ToArray());
			break;
		}
		case "items":
		{
			Item[] allItems = StoreInventory.items.Where((Item s) => s.price > 0 && (string.Join("", s.abr.Split(' ')).ToLower().Contains(searchQuery) || string.Join("", s.abr.Split(' ')).ToLower() == searchQuery || s.defname.ToLower().Contains(searchQuery) || s.defname.ToLower() == searchQuery)).Take(10).ToArray();
			Item[] array5 = allItems;
			foreach (Item item in array5)
			{
				results.Add(string.Join("", item.abr.Split(' ')).ToLower());
			}
			SendTenResults(twitchMessage, GenText.CapitalizeFirst(searchObject), searchQuery, results.ToArray());
			break;
		}
		case "animals":
		{
			PawnKindDef[] allAnimals = DefDatabase<PawnKindDef>.AllDefs.Where(delegate(PawnKindDef s)
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing erences)
				//IL_001b: Unknown result type (might be due to invalid IL or missing erences)
				//IL_0050: Unknown result type (might be due to invalid IL or missing erences)
				//IL_0055: Unknown result type (might be due to invalid IL or missing erences)
				int result2;
				if (s.RaceProps.Animal)
				{
					TaggedString labelCap3 = ((Def)s).LabelCap;
					if (!string.Join("", ((TaggedString)( labelCap3)).RawText.Split(' ')).ToLower().Contains(searchQuery))
					{
						labelCap3 = ((Def)s).LabelCap;
						if (!(string.Join("", ((TaggedString)( labelCap3)).RawText.Split(' ')).ToLower() == searchQuery) && !((Def)s).defName.ToLower().Contains(searchQuery))
						{
							result2 = ((((Def)s).defName.ToLower() == searchQuery) ? 1 : 0);
							goto IL_00b8;
						}
					}
					result2 = 1;
				}
				else
				{
					result2 = 0;
				}
				goto IL_00b8;
				IL_00b8:
				return (byte)result2 != 0;
			}).Take(10)
				.ToArray();
			PawnKindDef[] array3 = allAnimals;
			foreach (PawnKindDef animal in array3)
			{
				results.Add(((Def)animal).defName.ToLower());
			}
			SendTenResults(twitchMessage, GenText.CapitalizeFirst(searchObject), searchQuery, results.ToArray());
			break;
		}
		case "traits":
		{
			BuyableTrait[] allTrait = AllTraits.buyableTraits.Where((BuyableTrait s) => s.label.Contains(searchQuery) || s.label == searchQuery || ((Def)s.def).defName.ToLower().Contains(searchQuery) || ((Def)s.def).defName == searchQuery).Take(10).ToArray();
			BuyableTrait[] array = allTrait;
			foreach (BuyableTrait trait in array)
			{
				results.Add(trait.label);
			}
			SendTenResults(twitchMessage, GenText.CapitalizeFirst(searchObject), searchQuery, results.ToArray());
			break;
		}
		}
	}

	public void SendTenResults(ITwitchMessage twitchMessage, string searchObject, string searchQuery, string[] results)
	{
		if (results.Count() < 1)
		{
			return;
		}
		string output = "Lookup for " + searchObject + " \"" + searchQuery + "\": ";
		for (int i = 0; i < results.Count(); i++)
		{
			output += GenText.CapitalizeFirst(results[i]);
			if (i != results.Count() - 1)
			{
				output += ", ";
			}
		}
		TwitchWrapper.SendChatMessage(output);
	}
}
