using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SimpleJSON;
using TwitchToolkit.Store;
using UnityEngine;

namespace TwitchToolkit.Utilities
{
    public static class SaveHelper
    {
        public static string modFolder = "TwitchToolkit";
        public static string dataPath = Application.persistentDataPath + $"/{modFolder}/";
        public static string viewerDataPath = Path.Combine(dataPath, "ViewerData.json");
        public static string itemDataPath = Path.Combine(dataPath, "ItemData.json");
        public static string incItemsDataPath = Path.Combine(dataPath, "IncItemsData.json");
        public static string storePricesDataPath = Path.Combine(dataPath, "StorePrices.csv");

        private static void SaveJsonToDataPath(string json, string savePath)
        {
            bool dataPathExists = Directory.Exists(dataPath);
            
            if(!dataPathExists)
                Directory.CreateDirectory(dataPath);

            using (StreamWriter streamWriter = File.CreateText (savePath))
            {
                streamWriter.Write (json.ToString());
            }
        }

        public static void SaveAllModData()
        {
            SaveListOfViewersAsJson();
        }

        public static void SaveListOfViewersAsJson()
        {
            List<Utilities.ViewerSaveable> newViewers = new List<ViewerSaveable>();
            if (Viewers.All == null)
                return;
            foreach (Viewer vwr in Viewers.All)
            {
                Utilities.ViewerSaveable nwvwr = new Utilities.ViewerSaveable();
                nwvwr.id = vwr.id;
                nwvwr.username = vwr.username;
                nwvwr.coins = vwr.coins;
                nwvwr.karma = vwr.GetViewerKarma();
                newViewers.Add(nwvwr);
            }

            if (newViewers.Count <= 0)
                return;
            var viewerslisttemplate = JSON.Parse("{\"viewers\":[],\"total\":0}");
            string viewertemplate = "{\"id\":0,\"username\":\"string\",\"karma\":0,\"coins\":0}";
            foreach (ViewerSaveable vwr in newViewers)
            {
                var v = JSON.Parse(viewertemplate);
                v["id"] = vwr.id;
                v["username"] = vwr.username;
                v["karma"] = vwr.karma;
                v["coins"] = vwr.coins;
                viewerslisttemplate["viewers"].Add(vwr.id.ToString(), v);
            }
            viewerslisttemplate["total"] = newViewers.Count;

            SaveJsonToDataPath(viewerslisttemplate.ToString(), viewerDataPath);
        }

        public static void LoadListOfViewers()
        {
            try
            {
                if (!File.Exists(viewerDataPath))
                    return;

                using (StreamReader streamReader = File.OpenText (viewerDataPath))
                {
                    string jsonString = streamReader.ReadToEnd ();
                    var node = JSON.Parse(jsonString);
                    Helper.Log(node.ToString());
                    List<Viewer> listOfViewers = new List<Viewer>();
                    for (int i = 0; i < node["total"]; i++)
                    {
                        Viewer viewer = new Viewer(node["viewers"][i]["username"]);
                        if (ToolkitSettings.SyncStreamLabs)
                        {
                            viewer.SetViewerCoins(StreamLabs.GetViewerPoints(viewer));
                        }
                        else
                        {
                            viewer.SetViewerCoins(node["viewers"][i]["coins"].AsInt);
                        }
                        viewer.SetViewerKarma(node["viewers"][i]["karma"].AsInt);
                        listOfViewers.Add(viewer);
                    }

                    Viewers.All = listOfViewers;
                }
            }
            catch (InvalidDataException e)
            {
                Helper.Log("Invalid " + e.Message);
            }

        }
    }

    public class ViewerSaveable
    {
        public int id;
        public string username;
        public int karma;
        public int coins;
    }
}