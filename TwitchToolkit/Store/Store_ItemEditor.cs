using RimWorld;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TwitchToolkit.Incidents;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Store
{
    [StaticConstructorOnStartup]
    public static class Store_ItemEditor
    {
        static Store_ItemEditor()
        {
            LoadStoreItemList();
        }

        public static IEnumerable<ThingDef> GetDefaultItems()
        {
            return from t in DefDatabase<ThingDef>.AllDefs
                where (t.tradeability.TraderCanSell() || ThingSetMakerUtility.CanGenerate(t)) &&
                (t.building == null || t.Minifiable || ToolkitSettings.MinifiableBuildings) &&
                (t.BaseMarketValue > 0) &&
                (t.race == null)
                orderby t.LabelCap
                select t;
        }

        public static void ResetItemsToDefault()
        {
            IEnumerable<ThingDef> tradeableItems = GetDefaultItems();

            List<Item> allItems = new List<Item>();

            foreach (ThingDef def in tradeableItems)
            {
                allItems.Add( new Item(Convert.ToInt32(def.BaseMarketValue * 10 / 6), string.Join("", def.label.Split(' ')).ToLower().Replace("\"", ""), def.defName) );
            }

            StoreInventory.items = allItems;
        }

        public static void FindItemsNotInList()
        {
            IEnumerable<ThingDef> tradeableItems = GetDefaultItems();

            foreach (ThingDef def in tradeableItems)
            {
                Item item = StoreInventory.items.Find(s => s.defname == def.defName);

                if (item == null)
                {
                    StoreInventory.items.Add( new Item( Convert.ToInt32(def.BaseMarketValue * 10 / 6), string.Join("", def.label.Split(' ')).ToLower().Replace("\"", ""), def.defName ) );
                }
            }

            UpdateStoreItemList();
        }

        public static void UpdateStoreItems(List<ThingDef> allItems, List<int> itemPrices)
        {
            for (int i = 0; i < allItems.Count; i++)
            {

                Item item = StoreInventory.items.Find(s => s.defname == allItems[i].defName);
                if (item != null)
                {
                    item.price = itemPrices[i];
                }
                else
                {
                    StoreInventory.items.Add(new Item( itemPrices[i], string.Join("", allItems[i].LabelCap.ToLower().Split(' ')), allItems[i].defName ));
                }
            }

            UpdateStoreItemList();
        }

        public static void UpdateStoreItemList()
        {
            StringBuilder json = new StringBuilder();

            bool dataPathExists = Directory.Exists(dataPath);
            
            if(!dataPathExists)
                Directory.CreateDirectory(dataPath);
            
            json.AppendLine("{");
            json.AppendLine("\t\"items\" : [");

            if (StoreInventory.items == null)
            {
                return;
            }

            List<Item> allItems = StoreInventory.items;

            int finalCount = allItems.Count;

            for (int i = 0; i < finalCount; i++)
            {
                ThingDef thing;
                IEnumerable<ThingDef> search = DefDatabase<ThingDef>.AllDefs.Where(s => s.defName == allItems[i].defname);

                if (search == null || search.Count() < 1)
                {
                    finalCount--;
                    continue;
                }
                else
                {
                    thing = search.ElementAt(0);
                }

                string category = thing.FirstThingCategory != null ? thing.FirstThingCategory.LabelCap : "Uncategorized";

                json.AppendLine("\t{");
                json.AppendLine("\t\t\"abr\": \"" + allItems[i].abr + "\",");
                json.AppendLine("\t\t\"price\": " + allItems[i].price + ",");
                json.AppendLine("\t\t\"category\": \"" + category + "\",");
                json.AppendLine("\t\t\"defname\": \"" + allItems[i].defname + "\"");
                json.AppendLine("\t}" + (i != allItems.Count - 1 ? "," : ""));
            }

            json.AppendLine("\t],");
            json.AppendLine("\t\"total\": " + finalCount);
            json.AppendLine("}");

            using (StreamWriter streamWriter = File.CreateText (Path.Combine(dataPath, "StoreItems.json")))
            {
                streamWriter.Write (json.ToString());
            }
        }

        public static void LoadStoreItemList()
        {
            string filePath = Path.Combine(dataPath, "StoreItems.json");

            try
            {
                if (!File.Exists(filePath))
                    return;

                using (StreamReader streamReader = File.OpenText (filePath))
                {
                    string jsonString = streamReader.ReadToEnd();
                    var node = JSON.Parse(jsonString);
                    Helper.Log(node.ToString());

                    if (StoreInventory.items == null)
                    {
                        StoreInventory.items = new List<Item>();
                    }

                    for (int i = 0; i < node["total"]; i++)
                    {
                        Item item = StoreInventory.items.Find(x => x.defname == node["items"][i]["defname"] );
                        if (item != null)
                        {
                            item.price = node["items"][i]["price"].AsInt;
                        }
                        else
                        {
                            StoreInventory.items.Add( new Item(node["items"][i]["price"].AsInt, node["items"][i]["abr"], node["items"][i]["defname"]) );    
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Warning(e.Message);
            }

            FindItemsNotInList();
        }

        public static string dataPath = SaveHelper.dataPath;
    }
}
