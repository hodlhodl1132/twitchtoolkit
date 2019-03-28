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
    public class SaveHelper
    {
        static string modFolder = "TwitchToolkit";
        static string dataPath = Application.persistentDataPath + $"/{modFolder}/";
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
            SaveListOfItemsAsJson();
            SaveListOfIncItemsAsJson();
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

        public static void SaveListOfItemsAsJson()
        {
            List<Item> allItems = StoreInventory.items;
            var itemslisttemplate = JSON.Parse("{\"items\":[],\"total\":0}");
            string itemtemplate = "{\"id\":0,\"abr\":\"string\",\"defname\":\"string\",\"price\":0}";
            foreach (Item item in allItems)
            {
                var v = JSON.Parse(itemtemplate);
                v["id"] = item.id;
                v["abr"] = item.abr;
                v["defname"] = item.defname;
                v["price"] = item.price;
                itemslisttemplate["items"].Add(item.id.ToString(), v);
            }
            itemslisttemplate["total"] = allItems.Count;

            SaveJsonToDataPath(itemslisttemplate.ToString(), itemDataPath);
        }

        public static void LoadListOfItems()
        {
            try
            {
                if (!File.Exists(itemDataPath))
                    return;

                using (StreamReader streamReader = File.OpenText (itemDataPath))
                {
                    string jsonString = streamReader.ReadToEnd ();
                    var node = JSON.Parse(jsonString);
                    Helper.Log(node.ToString());
                    List<Item> listOfItems = new List<Item>();
                    for (int i = 0; i < node["total"]; i++)
                    {
                        Item item = new Item(node["items"][i]["price"].AsInt, node["items"][i]["abr"], node["items"][i]["defname"], node["items"][i]["id"].AsInt);
                        listOfItems.Add(item);
                    }

                    StoreInventory.items = listOfItems;
                }
            }
            catch (InvalidDataException e)
            {
                Helper.Log("Invalid " + e.Message);
            }
        }

        public static void SaveListOfIncItemsAsJson()
        {
            List<IncItem> allIncItems = StoreInventory.incItems;
            var incItemslisttemplate = JSON.Parse("{\"incitems\":[],\"total\":0}");
            string itemtemplate = "{\"id\":0,\"type\":\"string\",\"name\":\"string\",\"abr\":\"string\",\"karmatype\":\"string\",\"price\":0,\"evtid\":0,\"maxevents\":0}";
            foreach (IncItem incItem in allIncItems)
            {
                var v = JSON.Parse(itemtemplate);
                v["id"] = incItem.id;
                v["type"] = incItem.type;
                v["name"] = incItem.name;
                v["abr"] = incItem.abr;
                v["karmatype"] = incItem.karmatype.ToString();
                v["price"] = incItem.price;
                v["evtid"] = incItem.evtId;
                v["maxevents"] = incItem.maxEvents;
                incItemslisttemplate["incitems"].Add(incItem.id.ToString(), v);
            }
            incItemslisttemplate["total"] = allIncItems.Count;

            SaveJsonToDataPath(incItemslisttemplate.ToString(), incItemsDataPath);
        }

        public static void LoadListOfIncItems()
        {
            try
            {
                if (!File.Exists(incItemsDataPath))
                    return;

                using (StreamReader streamReader = File.OpenText (incItemsDataPath))
                {
                    string jsonString = streamReader.ReadToEnd ();
                    var node = JSON.Parse(jsonString);
                    Helper.Log(node.ToString());
                    List<IncItem> listOfIncItems = new List<IncItem>();
                    for (int i = 0; i < node["total"]; i++)
                    {
                        KarmaType karmaValue = (KarmaType) Enum.Parse(typeof(KarmaType), node["incitems"][i]["karmatype"].ToString().Replace("\"", ""), true);
                        
                        IncItem incItem = new IncItem(
                            node["incitems"][i]["id"].AsInt,
                            node["incitems"][i]["type"],
                            node["incitems"][i]["name"],
                            node["incitems"][i]["abr"],
                            karmaValue,
                            node["incitems"][i]["price"].AsInt,
                            node["incitems"][i]["evtid"].AsInt,
                            node["incitems"][i]["maxevents"].AsInt);
                        listOfIncItems.Add(incItem);
                    }

                    StoreInventory.incItems = listOfIncItems;
                }
            }
            catch (InvalidDataException e)
            {
                Helper.Log("Invalid " + e.Message);
            }
        }

        public static void CreateStorePricesCSV()
        {
            int linecount = 3 + StoreInventory.incItems.Count() + StoreInventory.items.Count();
            string[] lines = new string[linecount];
            lines[0] = "\"Events\", \"Price\", \"Type\", \"Code\"";
            int currentline = 1;
            foreach(IncItem product in StoreInventory.incItems)
            {
                string type = "malformed type";
                if (Enum.IsDefined(typeof(KarmaType), product.karmatype))
                {
                    type = product.karmatype.ToString();
                }
                
                if (product.price > 0)
                {
                    lines[currentline] = $"\"{product.name}\", \"{product.price}\", \"{type}\", \"{product.abr}\"";
                    currentline++;
                }
            }
            lines[currentline] = "";
            currentline++;
            lines[currentline] = "\n\"Items\", \"Price\"";
            currentline++;

            foreach(Item item in StoreInventory.items)
            {
                if (item.price > 0)
                {
                    lines[currentline] = $"\"{item.abr}\", \"{item.price}\"";
                    currentline++;
                }
            }
            System.IO.File.WriteAllLines(@"" + storePricesDataPath, lines, Helper.LanguageEncoding());
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