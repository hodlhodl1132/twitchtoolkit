using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Utilities;
using Verse;
using UnityEngine;

namespace TwitchToolkit.Store
{
    [StaticConstructorOnStartup]
    public static class Store_IncidentEditor
    {
        static Store_IncidentEditor()
        {
            List<StoreIncidentSimple> simpleIncidents = DefDatabase<StoreIncidentSimple>.AllDefs.ToList();
            List<StoreIncidentVariables> variableIncidents = DefDatabase<StoreIncidentVariables>.AllDefs.ToList();

            foreach (StoreIncidentSimple inc in simpleIncidents)
            {
                StoreIncidentSimple backup = new StoreIncidentSimple();
                backup.defName = inc.defName;
                backup.abbreviation = inc.abbreviation;
                backup.cost = inc.cost;
                backup.eventCap = inc.eventCap;
                backup.karmaType = inc.karmaType;
                backup.incidentHelper = inc.incidentHelper;
                simpleIncidentsBackup.Add(backup);
            }

            foreach (StoreIncidentVariables inc in variableIncidents)
            {
                StoreIncidentVariables backup = new StoreIncidentVariables();
                backup.defName = inc.defName;
                backup.abbreviation = inc.abbreviation;
                backup.cost = inc.cost;
                backup.eventCap = inc.eventCap;
                backup.karmaType = inc.karmaType;
                backup.incidentHelper = inc.incidentHelper;
                backup.minPointsToFire = inc.minPointsToFire;
                backup.variables = inc.variables;
                backup.maxWager = inc.maxWager;
                backup.syntax = inc.syntax;
                variableIncidentsBackup.Add(backup);
            }

            LoadCopies();

            Store_IncidentEditor.UpdatePriceSheet();
        }

        public static void SaveCopy(StoreIncident incident)
        {
            StoreIncidentSimple incidentSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList().Find(s => s.defName == incident.defName);
            
            if (incidentSimple != null)
            {
                SaveCopy(incidentSimple);
                return;
            }

            StoreIncidentVariables incidentVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList().Find(s => s.defName == incident.defName);

            if (incidentVariables != null)
            {
                SaveCopy(incidentVariables);
                return;
            }
        }

        public static void SaveCopy(StoreIncidentSimple incident)
        {
            if (!EditorPathExists())
            {
                throw new DirectoryNotFoundException();
            }

            string filePath = incident.defName + ".json";
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine($"\t\"defName\":\"" + incident.defName + "\",");
            json.AppendLine($"\t\"abbreviation\":\"" + incident.abbreviation + "\",");
            json.AppendLine($"\t\"cost\":\"" + incident.cost + "\",");
            json.AppendLine($"\t\"eventCap\":\"" + incident.eventCap + "\",");
            json.AppendLine($"\t\"karmaType\":\"" + incident.karmaType + "\"");
            json.AppendLine("}");

            Helper.Log(json.ToString());

            using (StreamWriter streamWriter = File.CreateText(editorPath + filePath))
            {
                streamWriter.Write(json.ToString());
            }

            Helper.Log("Backup created");
        }

        public static void SaveCopy(StoreIncidentVariables incident)
        {
            if (!EditorPathExists())
            {
                throw new DirectoryNotFoundException();
            }

            string filePath = incident.defName + ".json";
            StringBuilder json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine($"\t\"defName\":\"" + incident.defName + "\",");
            json.AppendLine($"\t\"abbreviation\":\"" + incident.abbreviation + "\",");
            json.AppendLine($"\t\"cost\":\"" + incident.cost + "\",");
            json.AppendLine($"\t\"eventCap\":\"" + incident.eventCap + "\",");
            json.AppendLine($"\t\"karmaType\":\"" + incident.karmaType + "\",");
            json.AppendLine($"\t\"minPointsToFire\":\"" + incident.minPointsToFire + "\",");
            json.AppendLine($"\t\"maxWager\":\"" + incident.maxWager + "\",");
            json.AppendLine("}");

            Helper.Log(json.ToString());

            using (StreamWriter streamWriter = File.CreateText(editorPath + filePath))
            {
                streamWriter.Write(json.ToString());
            }

            Helper.Log("Backup created");
        }

        public static void LoadCopies()
        {
            if (!EditorPathExists())
            {
                Helper.Log("Path for custom store incidents does not exist, creating");
                return;
            }

            List<StoreIncidentSimple> simpleIncidents = DefDatabase<StoreIncidentSimple>.AllDefs.ToList();
            List<StoreIncidentVariables> variableIncidents = DefDatabase<StoreIncidentVariables>.AllDefs.ToList();

            foreach (StoreIncidentSimple incident in simpleIncidents)
            {
                if (CopyExists(incident))
                {
                    LoadCopy(incident);
                }
            }

            foreach (StoreIncidentVariables incident in variableIncidents)
            {
                if (CopyExists(incident))
                {
                    LoadCopy(incident);
                }
            }
        }

        public static void LoadCopy(StoreIncident incident)
        {
            string filePath = incident.defName + ".json";

            try
            {
                using (StreamReader reader = File.OpenText(editorPath + filePath))
                {
                    string json = reader.ReadToEnd();
                    var node = JSON.Parse(json);

                    if (node["abbreviation"] == null)
                    {
                        Helper.Log("Copy of store incident file is missing critical info, delete file " + editorPath + filePath);
                    }
                    incident.abbreviation = node["abbreviation"];

                    if (node["cost"] == null)
                    {
                        Helper.Log("Copy of store incident file is missing critical info, delete file " + editorPath + filePath);
                    }
                    incident.cost = node["cost"].AsInt;

                    if (node["eventCap"] == null)
                    {
                    Helper.Log("Copy of store incident file is missing critical info, delete file " + editorPath + filePath);
                    }
                    incident.eventCap = node["eventCap"].AsInt;

                    if (node["karmaType"] == null)
                    {
                    Helper.Log("Copy of store incident file is missing critical info, delete file " + editorPath + filePath);
                    }
                    incident.karmaType = (KarmaType) Enum.Parse(typeof(KarmaType), node["karmaType"], true);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Helper.Log(e.Message);
            }

        }

        public static void LoadCopy(StoreIncidentSimple incident)
        {
            LoadCopy(incident as StoreIncident);
        }

        public static void LoadCopy(StoreIncidentVariables incident)
        {
            LoadCopy(incident as StoreIncident);

            string filePath = incident.defName + ".json";

            try
            {
                using (StreamReader reader = File.OpenText(editorPath + filePath))
                {
                    string json = reader.ReadToEnd();
                    var node = JSON.Parse(json);

                    if (node["minPointsToFire"] == null)
                    {
                        Helper.Log("Copy of store incident file is missing critical info, delete file " + editorPath + filePath);
                    }
                    incident.minPointsToFire = node["baseCost"].AsInt; 

                    if (node["maxWager"] == null)
                    {
                        Helper.Log("Copy of store incident file is missing critical info, delete file " + editorPath + filePath);
                    }
                    incident.maxWager = node["maxWager"].AsInt;
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Helper.Log(e.Message);
            }
        }

        public static bool CopyExists(StoreIncident incident)
        {
            if (!EditorPathExists())
            {
                throw new DirectoryNotFoundException();
            }

            string filePath = incident.defName + ".json";

            return File.Exists(editorPath + filePath);
        }

        public static void LoadBackups()
        {
            foreach(StoreIncident incident in DefDatabase<StoreIncident>.AllDefs)
            {
                LoadBackup(incident);
                SaveCopy(incident);
            }
        }

        public static void LoadBackup(StoreIncident incident)
        {
            StoreIncidentSimple incidentSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList().Find(s => s.defName == incident.defName);
            
            if (incidentSimple != null)
            {
                LoadBackupSimple(ref incidentSimple);
                return;
            }

            StoreIncidentVariables incidentVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList().Find(s => s.defName == incident.defName);

            if (incidentVariables != null)
            {
                LoadBackupVariables(ref incidentVariables);
                return;
            }
        }

        public static void LoadBackupSimple(ref StoreIncidentSimple incident)
        {
            string defName = incident.defName;
            StoreIncidentSimple incNew = simpleIncidentsBackup.ToList().Find(s => defName == s.defName);
            incident.abbreviation = incNew.abbreviation;
            incident.cost = incNew.cost;
            incident.eventCap = incNew.eventCap;
            incident.karmaType = incNew.karmaType;
            incident = incNew;
        }

        public static void LoadBackupVariables(ref StoreIncidentVariables incident)
        {
            string defName = incident.defName;
            StoreIncidentVariables incNew = variableIncidentsBackup.Find(s => defName == s.defName);
            incident.abbreviation = incNew.abbreviation;
            incident.cost = incNew.cost;
            incident.eventCap = incNew.eventCap;
            incident.karmaType = incNew.karmaType;
            incident.incidentHelper = incNew.incidentHelper;
            incident.minPointsToFire = incNew.minPointsToFire;
            incident.variables = incNew.variables;
            incident.maxWager = incNew.maxWager;
            incident.syntax = incNew.syntax;
        }

        public static bool EditorPathExists()
        {
            bool dataPathExists = Directory.Exists(editorPath);

            if (!dataPathExists)
                Directory.CreateDirectory(editorPath);

            return dataPathExists;
        }

        public static void UpdatePriceSheet()
        {
            StringBuilder json = new StringBuilder();

            bool dataPathExists = Directory.Exists(dataPath);
            
            if(!dataPathExists)
                Directory.CreateDirectory(dataPath);
            
            json.AppendLine("{");
            json.AppendLine("\t\"incitems\" : [");

            List<StoreIncident> allEvents = DefDatabase<StoreIncident>.AllDefs.Where(s => s.cost > 0).ToList();

            for (int i = 0; i < allEvents.Count; i++)
            {
                json.AppendLine("\t{");
                json.AppendLine("\t\t\"abr\": \"" + allEvents[i].abbreviation + "\",");
                json.AppendLine("\t\t\"price\": " + allEvents[i].cost + ",");
                json.AppendLine("\t\t\"karmatype\": \"" + allEvents[i].karmaType.ToString() + "\"");
                json.AppendLine("\t}" + (i != allEvents.Count - 1 ? "," : ""));
            }

            json.AppendLine("\t],");
            json.AppendLine("\t\"total\": " + allEvents.Count);
            json.AppendLine("}");

            using (StreamWriter streamWriter = File.CreateText (Path.Combine(dataPath, "StoreIncidents.json")))
            {
                streamWriter.Write (json.ToString());
            }

        }

        public static string dataPath = SaveHelper.dataPath;

        public static readonly List<StoreIncidentSimple> simpleIncidentsBackup = new List<StoreIncidentSimple>();

        public static readonly List<StoreIncidentVariables> variableIncidentsBackup = new List<StoreIncidentVariables>();

        public static string editorPath = dataPath + "Editor/StoreIncidents/";
    }
}
