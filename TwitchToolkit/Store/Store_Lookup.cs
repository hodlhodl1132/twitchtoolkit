using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.IRC;
using Verse;

namespace TwitchToolkit.Store
{
    public class Store_Lookup : TwitchInterfaceBase
    {
        public Store_Lookup(Game game)
        {

        }

        public override void ParseCommand(IRCMessage msg)
        {
            if (msg.Message.StartsWith("!lookup") && Commands.AllowCommand(msg.Channel))
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

                FindLookup(searchObject, searchQuery);
            }
        }

        public void FindLookup(string searchObject, string searchQuery)
        {
            List<string> results = new List<string>();
            switch(searchObject)
            {
                case "disease":
                    FindLookup("diseases", searchQuery);
                    break;
                case "skill":
                    FindLookup("skills", searchQuery);
                    break;
                case "event":
                    FindLookup("events", searchQuery);
                    break;
                case "item":
                    FindLookup("items", searchQuery);
                    break;
                case "animal":
                    FindLookup("animals", searchQuery);
                    break;
                case "diseases":
                    IncidentDef[] allDiseases = DefDatabase<IncidentDef>.AllDefs.Where(s => 
                        s.category == IncidentCategoryDefOf.DiseaseHuman &&
                        (string.Join("", s.LabelCap.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.LabelCap.Split(' ')).ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (IncidentDef disease in allDiseases)
                        results.Add(string.Join("", disease.LabelCap.Split(' ')).ToLower());
                    SendTenResults(searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
                case "skills":
                    SkillDef[] allSkills = DefDatabase<SkillDef>.AllDefs.Where(s => 
                        (string.Join("", s.LabelCap.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.LabelCap.Split(' ')).ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (SkillDef skill in allSkills)
                        results.Add(skill.defName.ToLower());
                    SendTenResults(searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
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
                    SendTenResults(searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
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
                    SendTenResults(searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
                case "animals":
                    PawnKindDef[] allAnimals = DefDatabase<PawnKindDef>.AllDefs.Where(s =>
                        s.RaceProps.Animal &&
                        (string.Join("", s.LabelCap.Split(' ')).ToLower().Contains(searchQuery) ||
                        string.Join("", s.LabelCap.Split(' ')).ToLower() == searchQuery ||
                        s.defName.ToLower().Contains(searchQuery) ||
                        s.defName.ToLower() == searchQuery)
                    ).Take(10).ToArray();

                    foreach (PawnKindDef animal in allAnimals)
                        results.Add(animal.defName.ToLower());
                    SendTenResults(searchObject.CapitalizeFirst(), searchQuery, results.ToArray());
                    break;
            }
        }

        public void SendTenResults(string searchObject, string searchQuery, string[] results)
        {
            if (results.Count() < 1) return;

            string output = "Lookup for " + searchObject + " \"" + searchQuery + "\": ";

            for (int i = 0; i < results.Count(); i++)
            {
                output += results[i].CapitalizeFirst();
                if (i != results.Count() - 1)
                    output += ", ";
            }

            Toolkit.client.SendMessage(output, true);
        }
    }
}
