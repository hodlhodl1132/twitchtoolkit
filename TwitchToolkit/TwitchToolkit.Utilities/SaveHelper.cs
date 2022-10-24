using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using UnityEngine;

namespace TwitchToolkit.Utilities;

public static class SaveHelper
{
	public static string modFolder = "TwitchToolkit";

	public static string dataPath = Application.persistentDataPath + "/" + modFolder + "/";

	public static string viewerDataPath = Path.Combine(dataPath, "ViewerData.json");

	public static string itemDataPath = Path.Combine(dataPath, "ItemData.json");

	public static string incItemsDataPath = Path.Combine(dataPath, "IncItemsData.json");

	public static string storePricesDataPath = Path.Combine(dataPath, "StorePrices.csv");

	private static void SaveJsonToDataPath(string json, string savePath)
	{
		if (!Directory.Exists(dataPath))
		{
			Directory.CreateDirectory(dataPath);
		}
		using StreamWriter streamWriter = File.CreateText(savePath);
		streamWriter.Write(json.ToString());
	}

	public static void SaveAllModData()
	{
		Helper.Log("Saving data");
		SaveListOfViewersAsJson();
	}

	public static void SaveListOfViewersAsJson()
	{
		List<ViewerSaveable> newViewers = new List<ViewerSaveable>();
		if (Viewers.All == null)
		{
			return;
		}
		foreach (Viewer vwr2 in Viewers.All)
		{
			ViewerSaveable nwvwr = new ViewerSaveable();
			nwvwr.id = vwr2.id;
			nwvwr.username = vwr2.username;
			nwvwr.coins = vwr2.coins;
			nwvwr.karma = vwr2.GetViewerKarma();
			newViewers.Add(nwvwr);
		}
		if (newViewers.Count <= 0)
		{
			return;
		}
		JSONNode viewerslisttemplate = JSON.Parse("{\"viewers\":[],\"total\":0}");
		string viewertemplate = "{\"id\":0,\"username\":\"string\",\"karma\":0,\"coins\":0}";
		foreach (ViewerSaveable vwr in newViewers)
		{
			JSONNode v = JSON.Parse(viewertemplate);
			v["id"] = vwr.id;
			v["username"] = vwr.username;
			v["karma"] = vwr.karma;
			v["coins"] = vwr.coins;
			viewerslisttemplate["viewers"].Add(vwr.id.ToString(), v);
		}
		viewerslisttemplate["total"] = newViewers.Count;
		Helper.Log("Saving viewers file");
		SaveJsonToDataPath(viewerslisttemplate.ToString(), viewerDataPath);
	}

	public static void LoadListOfViewers()
	{
		try
		{
			if (!File.Exists(viewerDataPath))
			{
				return;
			}
			using StreamReader streamReader = File.OpenText(viewerDataPath);
			string jsonString = streamReader.ReadToEnd();
			JSONNode node = JSON.Parse(jsonString);
			Helper.Log(node.ToString());
			List<Viewer> listOfViewers = new List<Viewer>();
			for (int i = 0; i < (int)node["total"]; i++)
			{
				Viewer viewer = new Viewer(node["viewers"][i]["username"]);
				viewer.SetViewerCoins(node["viewers"][i]["coins"].AsInt);
				viewer.SetViewerKarma(node["viewers"][i]["karma"].AsInt);
				listOfViewers.Add(viewer);
			}
			Viewers.All = listOfViewers;
		}
		catch (InvalidDataException e)
		{
			Helper.Log("Invalid " + e.Message);
		}
	}
}
