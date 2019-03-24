using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Store
{
    public static class StoreInventory
    {
        public static TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();
        public static List<IncItem> incItems = new List<IncItem>();
        public static List<Item> items = new List<Item>();

        public static void ResetIncItemData()
        {
            // if no previous save data create new products
            incItems = IncidentItems.GenerateDefaultProducts().ToList();
            SaveHelper.SaveListOfIncItemsAsJson();
        }

        public static void ResetItemData()
        {
            items = new List<Item>();
            Item.TryMakeAllItems();
            SaveHelper.SaveListOfItemsAsJson();
        }

        public static void LoadItemsIfNotLoaded()
        {
            if (items == null) ResetItemData();
        }

        public static void LoadIncItemsIfNotLoaded()
        {
            Log.Message("Checking if inc items are loaded?");
            if (incItems == null) ResetIncItemData();
            if (incItems == null) Log.Message("incitems null");
        }
    }
}
