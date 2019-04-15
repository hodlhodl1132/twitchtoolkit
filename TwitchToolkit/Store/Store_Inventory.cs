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
        public static List<Item> items = new List<Item>();
    }
}
