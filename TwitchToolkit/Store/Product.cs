using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitchToolkit.Store
{
    public class IncItem
    {
        public int id;
        public int type; //0 event - 1 care package
        public string name; //Small raid
        public string abr; //smallraid
        public KarmaType karmatype; //0 bad - 1 good - 2 neutral - 3 doom
        public int amount;
        public int evtId;
        public Event evt;
        public int maxEvents;


        public IncItem(int id, int type, string name, string abr, KarmaType karmatype, int amount, int evtId, int maxEvents)
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.abr = abr;
            this.karmatype = karmatype;
            this.amount = amount;
            this.evtId = evtId;
            this.evt = Products.defaultEvents[this.evtId];
            this.maxEvents = maxEvents;
        }

        public static int GetProductIdFromAbr(string abr)
        {
            return Settings.products.Find(x => x.abr == abr).id;
        }

        public static IncItem GetProductFromId(int id)
        {
            return Settings.products.Find(x => x.id == id);
        }

        public void SetProductAmount(int id, int amount)
        {
            Settings.ProductAmounts[id] = amount;
            this.amount = amount;
        }
    }
}
