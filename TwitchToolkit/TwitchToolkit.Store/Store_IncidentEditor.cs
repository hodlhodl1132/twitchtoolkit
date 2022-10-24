using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SimpleJSON;
using TwitchToolkit.Incidents;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Store;

[StaticConstructorOnStartup]
public static class Store_IncidentEditor
{
	public static readonly List<StoreIncidentSimple> simpleIncidentsBackup;

	public static readonly List<StoreIncidentVariables> variableIncidentsBackup;

	public static string dataPath;

	public static string editorPath;

	static Store_IncidentEditor()
	{
		simpleIncidentsBackup = new List<StoreIncidentSimple>();
		variableIncidentsBackup = new List<StoreIncidentVariables>();
		dataPath = SaveHelper.dataPath;
		editorPath = dataPath + "Editor/StoreIncidents/";
		List<StoreIncidentSimple> simpleIncidents = DefDatabase<StoreIncidentSimple>.AllDefs.ToList();
		List<StoreIncidentVariables> variableIncidents = DefDatabase<StoreIncidentVariables>.AllDefs.ToList();
		foreach (StoreIncidentSimple inc2 in simpleIncidents)
		{
			StoreIncidentSimple backup2 = new StoreIncidentSimple
			{
				defName = ((Def)inc2).defName,
				abbreviation = inc2.abbreviation,
				cost = inc2.cost,
				eventCap = inc2.eventCap,
				karmaType = inc2.karmaType,
				incidentHelper = inc2.incidentHelper
			};
			simpleIncidentsBackup.Add(backup2);
		}
		foreach (StoreIncidentVariables inc in variableIncidents)
		{
			StoreIncidentVariables backup = new StoreIncidentVariables
			{
				defName = ((Def)inc).defName,
				abbreviation = inc.abbreviation,
				cost = inc.cost,
				eventCap = inc.eventCap,
				karmaType = inc.karmaType,
				incidentHelper = inc.incidentHelper,
				minPointsToFire = inc.minPointsToFire,
				variables = inc.variables,
				maxWager = inc.maxWager,
				syntax = inc.syntax
			};
			variableIncidentsBackup.Add(backup);
		}
		LoadCopies();
		UpdatePriceSheet();
	}

	public static void SaveCopy(StoreIncident incident)
	{
		StoreIncidentSimple incidentSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList().Find((StoreIncidentSimple s) => ((Def)s).defName == ((Def)incident).defName);
		if (incidentSimple != null)
		{
			SaveCopy(incidentSimple);
			return;
		}
		StoreIncidentVariables incidentVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList().Find((StoreIncidentVariables s) => ((Def)s).defName == ((Def)incident).defName);
		if (incidentVariables != null)
		{
			SaveCopy(incidentVariables);
		}
	}

	public static void SaveCopy(StoreIncidentSimple incident)
	{
		if (!EditorPathExists())
		{
			throw new DirectoryNotFoundException();
		}
		string filePath = ((Def)incident).defName + ".json";
		StringBuilder json = new StringBuilder();
		json.AppendLine("{");
		json.AppendLine("\t\"defName\":\"" + ((Def)incident).defName + "\",");
		json.AppendLine("\t\"abbreviation\":\"" + incident.abbreviation + "\",");
		json.AppendLine("\t\"cost\":\"" + incident.cost + "\",");
		json.AppendLine("\t\"eventCap\":\"" + incident.eventCap + "\",");
		json.AppendLine(string.Concat("\t\"karmaType\":\"", incident.karmaType, "\""));
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
		string filePath = ((Def)incident).defName + ".json";
		StringBuilder json = new StringBuilder();
		json.AppendLine("{");
		json.AppendLine("\t\"defName\":\"" + ((Def)incident).defName + "\",");
		json.AppendLine("\t\"abbreviation\":\"" + incident.abbreviation + "\",");
		json.AppendLine("\t\"cost\":\"" + incident.cost + "\",");
		json.AppendLine("\t\"eventCap\":\"" + incident.eventCap + "\",");
		json.AppendLine(string.Concat("\t\"karmaType\":\"", incident.karmaType, "\","));
		json.AppendLine("\t\"minPointsToFire\":\"" + incident.minPointsToFire + "\",");
		json.AppendLine("\t\"maxWager\":\"" + incident.maxWager + "\",");
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
		foreach (StoreIncidentSimple incident2 in simpleIncidents)
		{
			if (CopyExists(incident2))
			{
				LoadCopy(incident2);
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
		string filePath = ((Def)incident).defName + ".json";
		try
		{
			using StreamReader reader = File.OpenText(editorPath + filePath);
			string json = reader.ReadToEnd();
			JSONNode node = JSON.Parse(json);
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
			incident.karmaType = (KarmaType)Enum.Parse(typeof(KarmaType), node["karmaType"], ignoreCase: true);
		}
		catch (UnauthorizedAccessException e)
		{
			Helper.Log(e.Message);
		}
	}

	public static void LoadCopy(StoreIncidentSimple incident)
	{
		LoadCopy((StoreIncident)incident);
	}

	public static void LoadCopy(StoreIncidentVariables incident)
	{
		LoadCopy((StoreIncident)incident);
		string filePath = ((Def)incident).defName + ".json";
		try
		{
			using StreamReader reader = File.OpenText(editorPath + filePath);
			string json = reader.ReadToEnd();
			JSONNode node = JSON.Parse(json);
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
		string filePath = ((Def)incident).defName + ".json";
		return File.Exists(editorPath + filePath);
	}

	public static void LoadBackups()
	{
		foreach (StoreIncident incident in DefDatabase<StoreIncident>.AllDefs)
		{
			LoadBackup(incident);
			SaveCopy(incident);
		}
	}

	public static void LoadBackup(StoreIncident incident)
	{
		StoreIncidentSimple incidentSimple = DefDatabase<StoreIncidentSimple>.AllDefs.ToList().Find((StoreIncidentSimple s) => ((Def)s).defName == ((Def)incident).defName);
		if (incidentSimple != null)
		{
			LoadBackupSimple( incidentSimple);
			return;
		}
		StoreIncidentVariables incidentVariables = DefDatabase<StoreIncidentVariables>.AllDefs.ToList().Find((StoreIncidentVariables s) => ((Def)s).defName == ((Def)incident).defName);
		if (incidentVariables != null)
		{
			LoadBackupVariables( incidentVariables);
		}
	}

	public static void LoadBackupSimple( StoreIncidentSimple incident)
	{
		string defName = ((Def)incident).defName;
		StoreIncidentSimple incNew = simpleIncidentsBackup.ToList().Find((StoreIncidentSimple s) => defName == ((Def)s).defName);
		incident.abbreviation = incNew.abbreviation;
		incident.cost = incNew.cost;
		incident.eventCap = incNew.eventCap;
		incident.karmaType = incNew.karmaType;
		incident = incNew;
	}

	public static void LoadBackupVariables( StoreIncidentVariables incident)
	{
		string defName = ((Def)incident).defName;
		StoreIncidentVariables incNew = variableIncidentsBackup.Find((StoreIncidentVariables s) => defName == ((Def)s).defName);
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
		{
			Directory.CreateDirectory(editorPath);
		}
		return dataPathExists;
	}

	public static void UpdatePriceSheet()
	{
		StringBuilder json = new StringBuilder();
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		json.AppendLine("{");
		json.AppendLine("\t\"incitems\" : [");
		List<StoreIncident> allEvents = (from s in DefDatabase<StoreIncident>.AllDefs
			where s.cost > 0
			select s).ToList();
		for (int i = 0; i < allEvents.Count; i++)
		{
			json.AppendLine("\t{");
			json.AppendLine("\t\t\"abr\": \"" + allEvents[i].abbreviation + "\",");
			json.AppendLine("\t\t\"price\": " + allEvents[i].cost + ",");
			json.AppendLine("\t\t\"karmatype\": \"" + allEvents[i].karmaType.ToString() + "\"");
			json.AppendLine("\t}" + ((i != allEvents.Count - 1) ? "," : ""));
		}
		json.AppendLine("\t],");
		json.AppendLine("\t\"total\": " + allEvents.Count);
		json.AppendLine("}");
		using StreamWriter streamWriter = File.CreateText(Path.Combine(dataPath, "StoreIncidents.json"));
		streamWriter.Write(json.ToString());
	}
}
