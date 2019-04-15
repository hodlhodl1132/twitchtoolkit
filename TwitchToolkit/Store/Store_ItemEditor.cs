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
            FindItemsNotInList();
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
                allItems.Add( new Item(Convert.ToInt32(def.BaseMarketValue * 10 / 6), string.Join("", def.label.Split(' ')).ToLower(), def.defName) );
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
                    StoreInventory.items.Add( new Item( Convert.ToInt32(def.BaseMarketValue * 10 / 6), string.Join("", def.label.Split(' ')).ToLower(), def.defName ) );
                }
            }

            UpdateStoreItemList();
        }

        public static void UpdateStoreItems(List<ThingDef> allItems, List<int> itemPrices)
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (itemPrices[i] < 1) continue;

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

            List<Item> allItems = StoreInventory.items;
            int itemsDiscluded = 0;

            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].price < 1)
                {
                    itemsDiscluded ++;
                    continue;
                }

                json.AppendLine("\t{");
                json.AppendLine("\t\t\"abr\": \"" + allItems[i].abr + "\",");
                json.AppendLine("\t\t\"price\": " + allItems[i].price + ",");
                json.AppendLine("\t\t\"category\": \"" + DefDatabase<ThingDef>.GetNamed(allItems[i].defname).FirstThingCategory.LabelCap + "\",");
                json.AppendLine("\t\t\"defname\": \"" + allItems[i].defname + "\"");
                json.AppendLine("\t}" + (i != allItems.Count - 1 ? "," : ""));
            }

            json.AppendLine("\t],");
            json.AppendLine("\t\"total\": " + (allItems.Count - itemsDiscluded));
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

                    for (int i = 0; i < node["total"]; i++)
                    {
                        Item item = StoreInventory.items.Find(x => x.abr == node["items"][i]["abr"] );
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
        }

        public static string dataPath = SaveHelper.dataPath;
    }
}
