using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;

namespace TwitchToolkit.Store
{
    public class PurchaseLogger
    {
        public static List<Purchase> purchases = new List<Purchase>();

        public static void LogPurchase(Purchase purchase)
        {
            try
            {
                Settings.JobManager.AddNewJob(new ScheduledJob(120, new Func<object, bool>(RemovePurchaseFromLog), purchase));
                purchases.Add(purchase);
            }
            catch (InvalidCastException e)
            {
                Helper.Log("Invalid cast " + e.Message);
            }
        }

        public static bool RemovePurchaseFromLog(object obj)
        {
            Purchase purchase = obj as Purchase;
            purchases = purchases.Where(x => x != purchase).ToList();
            return true;
        }

        public static int CountRecentEventsOfType(KarmaType karmatype, int minutes = 5)
        {
            return purchases.Where(x => x.karmatype == karmatype && TimeHelper.MinutesElapsed(x.time) < minutes).ToList().Count();
        }

        public static int CountRecentEvents(int minutes = 5)
        {
            return purchases.Where(x => TimeHelper.MinutesElapsed(x.time) < minutes).Count();
        }

        public static int CountRecentCarePackages(int minutes = 5)
        {
            return purchases.Where(x => x.logged.Contains("carepackage") && TimeHelper.MinutesElapsed(x.time) < minutes).ToList().Count();
        }

        public static int CountRecentEventsTotalCostOfType(KarmaType karmatype, int minutes = 5)
        {
            int totalcost = 0;
            foreach (Purchase purchase in purchases)
            {
                if (purchase.karmatype == karmatype && TimeHelper.MinutesElapsed(purchase.time) < minutes)
                {
                    totalcost += purchase.calculatedprice;
                }
            }
            return totalcost;
        }

        public static int CountRecentEventsTotalCost(int minutes = 5)
        {
            int totalcost = 0;
            foreach (Purchase purchase in purchases)
            {
                if (TimeHelper.MinutesElapsed(purchase.time) < minutes)
                {
                    totalcost += purchase.calculatedprice;
                }
            }
            return totalcost;
        }

        public static int CountRecentCarePackagesTotalCost(int minutes = 5)
        {
            int totalcost = 0;
            foreach (Purchase purchase in purchases)
            {
                if (purchase.type.Contains("carepackage") && TimeHelper.MinutesElapsed(purchase.time) < minutes)
                {
                    totalcost += purchase.calculatedprice;
                }
            }
            return totalcost;
        }
    }

    public class Purchase
    {
        public string username;
        public string type;
        public KarmaType karmatype;
        public int calculatedprice;
        public string logged;
        public DateTime time;

        public Purchase(string username, string type, KarmaType karmatype, int calculatedprice, string logged, DateTime time)
        {
            this.username = username;
            this.type = type;
            this.karmatype = karmatype;
            this.calculatedprice = calculatedprice;
            this.logged = logged;
            this.time = time;
        }
    }
}
