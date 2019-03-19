using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SimpleJSON;
using UnityEngine;

namespace TwitchToolkit.Utilities
{
    public class SaveHelper
    {
        static string modFolder = "TwitchToolkit";
        static string dataPath = Application.persistentDataPath + $"/{modFolder}/";
        public static string viewerDataPath = Path.Combine(dataPath, "ViewerData.json");
        public static string itemDataPath = Path.Combine(dataPath, "ItemData.json");

        public static void SaveListOfViewersAsJson()
        {
            List<Utilities.ViewerSaveable> newViewers = new List<ViewerSaveable>();
            foreach (Viewer vwr in Settings.listOfViewers)
            {
                Utilities.ViewerSaveable nwvwr = new Utilities.ViewerSaveable();
                nwvwr.id = vwr.id;
                nwvwr.username = vwr.username;
                nwvwr.coins = vwr.GetViewerCoins();
                nwvwr.karma = vwr.GetViewerKarma();
                newViewers.Add(nwvwr);
            }

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

            bool dataPathExists = Directory.Exists(dataPath);
            
            if(!dataPathExists)
                Directory.CreateDirectory(dataPath);

            using (StreamWriter streamWriter = File.CreateText (viewerDataPath))
            {
                streamWriter.Write (viewerslisttemplate.ToString());
            }
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
                        Viewer viewer = new Viewer(node["viewers"][i]["username"], node["viewers"][i]["id"].AsInt);
                        viewer.SetViewerCoins(node["viewers"][i]["coins"].AsInt);
                        viewer.SetViewerKarma(node["viewers"][i]["karma"].AsInt);
                        listOfViewers.Add(viewer);
                    }

                    Settings.listOfViewers = listOfViewers;
                }
            }
            catch (InvalidDataException e)
            {
                Helper.Log("Invalid " + e.Message);
            }

        }

        public static void SaveListOfItemsAsJson()
        {
            List<Item> allItems = Settings.items;
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

            bool dataPathExists = Directory.Exists(dataPath);
            
            if(!dataPathExists)
                Directory.CreateDirectory(dataPath);

            using (StreamWriter streamWriter = File.CreateText (itemDataPath))
            {
                streamWriter.Write (itemslisttemplate.ToString());
            }
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

                    Settings.items = listOfItems;
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