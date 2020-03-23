using ToolkitCore;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using TwitchLib.Client.Models;
using TwitchToolkit.IncidentHelpers.Traits;
using TwitchToolkit.Incidents;
using Verse;

namespace TwitchToolkit.Store
{
    public class Store_Lookup : TwitchInterfaceBase
    {
        public Store_Lookup(Game game)
        {

        }

        public override void ParseCommand(ChatMessage msg)
        {
            if (msg.Message.StartsWith("!lookup") && CommandsHandler.AllowCommand(msg))
            {

                string[] command = msg.Message.Split(' ');
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
                
                if  (command.Length > 2)
                {
                    searchQuery = command[2].ToLower();
                }

                if (searchQuery == null)
                {
                    searchQuery = "";
                }

                FindLookup(msg, searchObject, searchQuery);
            }

            Store_Logger.LogString("Finished lookup parse");
        }

        public void FindLookup(ChatMessage msg, string searchObject, string searchQuery)
        {
            List<string> results = new List<string>();
            switch(searchObject)
            {
                case "disease":
                    FindLookup(msg, "diseases", searchQuery);
                    break;
                case "skill":
                    FindLookup(msg, "skills", searchQuery);
                    break;
                case "event":
                    FindLookup(msg, "events", searchQuery);
                    break;
                case "item":
                    FindLookup(msg, "items", searchQuery);
                    break;
                case "animal":
                    FindLookup(msg, "animals", searchQuery);
                    break;
                case "trait":
                    FindLookup(msg, "traits", searchQuery);
                    break;
                case "diseases":
                    IncidentDef[] allDiseases = DefDatabase<IncidentDef>.AllDefs.Where(s => 
                        s.category == IncidentCategoryDefOf.DiseaseHuman &&
                        (string.Join("", s.LabelCap.RawText.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.LabelCap.RawText.Split(' ')).ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (IncidentDef disease in allDiseases)
                        results.Add(string.Join("", disease.LabelCap.RawText.Split(' ')).ToLower());
                    SendTenResults(msg, searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
                case "skills":
                    SkillDef[] allSkills = DefDatabase<SkillDef>.AllDefs.Where(s => 
                        (string.Join("", s.LabelCap.RawText.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.LabelCap.RawText.Split(' ')).ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (SkillDef skill in allSkills)
                        results.Add(skill.defName.ToLower());
                    SendTenResults(msg, searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
                case "events":
                    StoreIncident[] allEvents = DefDatabase<StoreIncident>.AllDefs.Where(s => 
                        s.cost > 0 &&
                        (string.Join("", s.abbreviation.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.abbreviation.Split(' ')).ToLower() == searchQuery ||
                        s.defName.ToLower().Contains(searchQuery) ||
                        s.defName.ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (StoreIncident evt in allEvents)
                        results.Add(string.Join("", evt.abbreviation.Split(' ')).ToLower());
                    SendTenResults(msg, searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
                case "items":
                    Item[] allItems = StoreInventory.items.Where(s => 
                        s.price > 0 &&
                        (string.Join("", s.abr.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.abr.Split(' ')).ToLower() == searchQuery ||
                        s.defname.ToLower().Contains(searchQuery) ||
                        s.defname.ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (Item item in allItems)
                        results.Add(string.Join("", item.abr.Split(' ')).ToLower());
                    SendTenResults(msg, searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
                case "animals":
                    PawnKindDef[] allAnimals = DefDatabase<PawnKindDef>.AllDefs.Where(s =>
                        s.RaceProps.Animal &&
                        (string.Join("", s.LabelCap.RawText.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.LabelCap.RawText.Split(' ')).ToLower() == searchQuery ||
                        s.defName.ToLower().Contains(searchQuery) ||
                        s.defName.ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (PawnKindDef animal in allAnimals)
                        results.Add(animal.defName.ToLower());
                    SendTenResults(msg, searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
                case "traits":
                    BuyableTrait[] allTrait = AllTraits.buyableTraits.Where(s =>
                        s.label.Contains(searchQuery) ||
                        s.label == searchQuery ||
                        s.def.defName.ToLower().Contains(searchQuery) ||
                        s.def.defName == searchQuery
                    ).Take(10).ToArray();

                    foreach (BuyableTrait trait in allTrait)
                        results.Add(trait.label);
                    SendTenResults(msg, searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
            }
        }

        public void SendTenResults(ChatMessage msg, string searchObject, string searchQuery, string[] results)
        {
            if (results.Count() < 1) return;

            string output = "Lookup for " + searchObject + " \"" + searchQuery + "\": ";

            for (int i = 0; i < results.Count(); i++)
            {
                output += results[i].CapitalizeFirst();
                if (i != results.Count() - 1)
                    output += ", ";
            }

            TwitchWrapper.SendChatMessage(output);
        }
    }
}
