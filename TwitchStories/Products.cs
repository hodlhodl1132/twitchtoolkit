using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchStories
{
    public class Products
    {
        public static Event[] defaultEvents = Events.GetEvents();

        public static Product[] GenerateDefaultProducts()
        {
            Product[] defaultProducts = 
            {
                new Product(0, "Small raid", "smallraid", 0, 450, defaultEvents[0]),
                new Product(0, "Medium raid", "mediumraid", 0, 500, defaultEvents[1]),
                new Product(0, "Big raid", "bigraid", 0, 950, defaultEvents[2]),
                new Product(0, "Medium raid drop", "mediumraiddrop", 0, 1050, defaultEvents[3]),
                new Product(0, "Big raid drop", "bigraiddrop", 0, 1050, defaultEvents[4]),
                new Product(0, "Big siege", "bigsiege", 0, 1300, defaultEvents[5]),
                new Product(0, "Big mechanoid raid", "bigmechanoidraid", 0, 1300, defaultEvents[6]),
                new Product(0, "Infestation", "infestation", 0, 1050, defaultEvents[7]),

                new Product(0, "Heart attack", "heartattack", 0, 600, defaultEvents[8]),
                new Product(0, "Plague", "plague", 0, 350, defaultEvents[9]),
                new Product(0, "Flu", "flu", 0, 400, defaultEvents[10]),
                new Product(0, "Infection", "infection", 0, 200, defaultEvents[11]),
                new Product(0, "Malaria", "malaria", 0, 400, defaultEvents[12]),
                new Product(0, "Gut worms", "gutworms", 0, 400, defaultEvents[13]),
                new Product(0, "Muscle parasites", "muscleparasites", 0, 400, defaultEvents[14]),

                new Product(0, "Plague medium", "plaguemedium", 0, 900, defaultEvents[15]),
                new Product(0, "Flu medium", "flumedium", 0, 900, defaultEvents[16]),
                new Product(0, "Infection medium", "infectionmedium", 0, 1100, defaultEvents[17]),
                new Product(0, "Malaria medium", "malariamedium", 0, 1100, defaultEvents[18]),
                new Product(0, "Gut worms medium", "gutwormsmedium", 0, 1100, defaultEvents[19]),
                new Product(0, "Muscle parasites medium", "muscleparasitesmedium", 0, 1100, defaultEvents[20]),

                new Product(0, "Plague hard", "plaguehard", 0, 1500, defaultEvents[21]),
                new Product(0, "Flu hard", "fluhard", 0, 1500, defaultEvents[22]),
                new Product(0, "Infection hard", "infectionhard", 0, 1500, defaultEvents[23]),
                new Product(0, "Malaria hard", "malariahard", 0, 1500, defaultEvents[24]),
                new Product(0, "Gut worms hard", "gutwormshard", 0, 1500, defaultEvents[25]),
                new Product(0, "Muscle parasites hard", "muscleparasiteshard", 0, 1500, defaultEvents[26]),
                new Product(0, "Blindness", "blindness", 0, 1500, defaultEvents[27]),

                new Product(0, "Clear weather", "clearweather", 1, 300, defaultEvents[28]),
                new Product(0, "Rain", "rain", 1, 300, defaultEvents[29]),
                new Product(0, "Rainy thunderstorm", "rainythunderstorm", 2, 200, defaultEvents[30]),
                new Product(0, "Dry thunderstorm", "drythunderstorm", 0, 250, defaultEvents[31]),
                new Product(0, "Snow gentle", "snowgentle", 2, 300, defaultEvents[32]),
                new Product(0, "Snow hard", "snowhard", 0, 400, defaultEvents[33]),
                new Product(0, "Fog", "fog", 2, 150, defaultEvents[34]),
                new Product(0, "Foggy rain", "foggyrain", 2, 250, defaultEvents[35]),
                new Product(0, "Flash storm", "flashstorm", 0, 300, defaultEvents[36]),

                new Product(0, "Eclipse", "eclipse", 0, 400, defaultEvents[37]),
                new Product(0, "Aurora", "aurora", 1, 150, defaultEvents[38]),
                new Product(0, "Vomit rain", "vomitrain", 0, 150, defaultEvents[39]),
                new Product(0, "Ambrosia sprout", "ambrosiasprout", 1, 150, defaultEvents[40]),
                new Product(0, "Meteorite", "meteorite", 2, 150, defaultEvents[41]),
                new Product(0, "Meteorite shower", "meteoriteshower", 2, 600, defaultEvents[42]),

                new Product(0, "Blight", "blight", 0, 500, defaultEvents[43]),
                new Product(0, "Solar flare", "solarflare", 0, 300, defaultEvents[44]),
                new Product(0, "Volcanic winter", "volcanicwinter", 3, 900, defaultEvents[45]),
                new Product(0, "Toxic fallout", "toxicfallout", 3, 1500, defaultEvents[46]),
                new Product(0, "Heat wave", "heatwave", 0, 500, defaultEvents[47]),
                new Product(0, "Cold snap", "coldsnap", 0, 550, defaultEvents[48]),
                new Product(0, "Tornado", "tornado", 0, 500, defaultEvents[49]),
                new Product(0, "Tornados", "tornados", 3, 1100, defaultEvents[50]),

                new Product(0, "Wild human", "wildhuman", 1, 200, defaultEvents[51]),
                new Product(0, "Wanderer joins", "wandererjoins", 2, 250, defaultEvents[52]),
                new Product(0, "Gender swap", "genderswap", 2, 150, defaultEvents[53]),
                new Product(0, "Skill increase", "skillincrease", 1, 150, defaultEvents[54]),
                new Product(0, "Party", "party", 1, 300, defaultEvents[55]),
                new Product(0, "Cannibal", "cannibal", 0, 450, defaultEvents[56]),
                new Product(0, "Luciferium", "luciferium", 3, 5000, defaultEvents[57]),

                new Product(0, "Mad animal", "mananimal", 0, 600, defaultEvents[58]),
                new Product(0, "Herd migration", "herdmigration", 1, 400, defaultEvents[59]),
                new Product(0, "Animals wander in", "animalswanderin", 1, 300, defaultEvents[60]),
                new Product(0, "Rare thrumbos", "thrumbos", 1, 600, defaultEvents[61]),
                new Product(0, "Farm animals", "farmanimals", 1, 200, defaultEvents[62]),
                new Product(0, "Animal self-tamed", "animalselftame", 1, 200, defaultEvents[63]),
                new Product(0, "Yorkshire terriers", "yorkshireterriers", 1, 300, defaultEvents[64]),
                new Product(0, "Manhunter pack", "manhunterpack", 0, 400, defaultEvents[65]),
                new Product(0, "Predators", "predators", 3, 1500, defaultEvents[66]),

                new Product(0, "Inspiration", "inspiration", 1, 150, defaultEvents[67]),
                new Product(0, "Psychic wave", "psychicwave", 0, 300, defaultEvents[68]),
                new Product(0, "Psychic drone", "psychicdrone", 0, 400, defaultEvents[69]),
                new Product(0, "Psychic soothe", "psychicsoothe", 1, 250, defaultEvents[70]),
                new Product(0, "Minor mental break", "minormentalbreak", 0, 200, defaultEvents[71]),
                new Product(0, "Major mental break", "majormentalbreak", 0, 200, defaultEvents[72]),
                new Product(0, "Extreme mental break", "extremementalbreak", 3, 1800, defaultEvents[73]),
                new Product(0, "Berserk mental break", "berserkmentalbreak", 3, 10000, defaultEvents[74]),

                new Product(0, "Ship chunk drop", "shipchunkdrop", 1, 300, defaultEvents[75]),
                new Product(0, "Cargo pod dropped", "cargopoddropped", 1, 300, defaultEvents[76]),
                new Product(0, "Transport pod dropped", "transportpoddropped", 1, 600, defaultEvents[77]),
                new Product(0, "Ship part poison", "shippartpoison", 0, 900, defaultEvents[78]),
                new Product(0, "Ship part psychic", "shippartpsychic", 0, 1000, defaultEvents[79]),
                new Product(1, "Care package", "carepackage", 1, 0, defaultEvents[80]),

                new Product(0, "Man in black", "maninblack", 1, 250, defaultEvents[81]),
                new Product(0, "Refugee chased", "refugeechased", 2, 450, defaultEvents[82]),
                new Product(0, "Traveler", "traveler", 1, 300, defaultEvents[83]),
                new Product(0, "Visitor", "visitor", 1, 150, defaultEvents[84]),
                new Product(0, "Trader visting", "tradervisiting", 1, 225, defaultEvents[85]),
                new Product(0, "Trader ship", "tradership", 1, 300, defaultEvents[86]),
            };

            return defaultProducts;
        }

        public static Product GetProduct(string abr)
        {
            try
            {
                Product product = Settings.products.Find(x => x.abr == abr);
                return product;
            }
            catch (InvalidCastException e)
            {
                Helper.Log("Invalid Product");
                Helper.Log($"Source: {e.Source} - Message: {e.Message} - Trace: {e.StackTrace}");
                return null;
            }
        }
    }

    public class Product
    {
        public int type;
        public string name;
        public string abr;
        public int karmatype; //0 bad - 1 good - 2 neutral - 3 doom
        public int amount;
        public Event evt;


        public Product(int type, string name, string abr, int karmatype, int amount, Event evt)
        {
            this.type = type;
            this.name = name;
            this.abr = abr;
            this.karmatype = karmatype;
            this.amount = amount;
            this.evt = evt;
        }        
    }

    public class ShopCommand
    {
        public string message;
        public Viewer viewer;
        public Product product;
        public int calculatedprice = 0;
        public string errormessage = null;
        public string successmessage = null;
        private string item = null;
        private int quantity = 0;
        private string craftedmessage;

        public ShopCommand(string message, Viewer viewer)
        {
            string[] command = message.Split(' ');
            string productabr = command[1];
            this.message = message;
            this.viewer = viewer;

            this.product = Products.GetProduct(productabr);

            if (this.product == null)
            {
                Helper.Log("Product is null");
                return;
            }

            Helper.Log("Configuring purchase");
            if (product.type == 0)
            { //event
                Helper.Log("Calculating price for event");
                this.calculatedprice = this.product.amount;
                string[] chatmessage = command;
                craftedmessage = $"{this.viewer.username}: ";
                for (int i = 2; i < chatmessage.Length; i++)
                {
                    craftedmessage += chatmessage[i] + " ";
                }
                this.product.evt.chatmessage = craftedmessage;
            }
            else if (product.type == 1)
            { //item
                try
                {
                    Helper.Log("Trying ItemEvent Checks");
                    int quantity;
                    this.item = command[0];
                    int itemPrice = CarePackage.ItemPriceResolver(this.item);
                    bool isNumeric = int.TryParse(command[2], out quantity);
                    if (isNumeric && itemPrice > 0)
                    { 
                        Helper.Log($"item: {this.item} - price: {itemPrice} - isnumeric: {isNumeric} - quantity{quantity}");
                        this.calculatedprice = quantity * itemPrice;
                        Helper.Log("ItemEvent Checks passed, buying item");
                    }
                }
                catch (InvalidCastException e)
                {
                    Helper.Log("Invalid product or quantity - " + e.Message);
                }
                finally
                {
                    string[] chatmessage = command;
                    craftedmessage = $"{this.viewer.username}: ";
                    for (int i = 3; i < chatmessage.Length; i++)
                    {
                        craftedmessage += chatmessage[i] + " ";
                    }
                }
            }

            if (this.calculatedprice <= 0)
            {               
                // invalid price
                Helper.Log("Invalid price detected?");
            }
            else if (viewer.GetViewerCoins() < this.calculatedprice)
            {
                // send message not enough coins
               this.errormessage = $"@{this.viewer.username} you do not have enough coins. Your selected item(s) price is {this.calculatedprice} coins. You have {viewer.GetViewerCoins()} coins.";
            }
            else if (calculatedprice < Settings.MinimumPurchasePrice)
            {
                // does not meet minimum purchase price
                this.errormessage = $"@{this.viewer.username} purchase does not meet minimum amount. Your selected purchase price is {this.calculatedprice} coins and you have {this.viewer.GetViewerCoins()}";
            }
            else
            {
                this.ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {
            // take user coins
            this.viewer.TakeViewerCoins(this.calculatedprice);
            // create success message
            if (this.product.type == 0)
            {
                // normal event
                this.successmessage = $"Event {this.product.name} purchased by @{this.viewer.username}";
            }
            else if (this.product.type == 1)
            {
                // care package
                this.product.evt = CarePackage.CarePackageGenerate(this.item, this.quantity);

                this.product.evt.chatmessage = craftedmessage;
            }
            
            // create purchase event
            Ticker.Events.Enqueue(this.product.evt);
        }
    }

    public class CarePackage
    {
        public static Event CarePackageGenerate(string item, int amount)
        {
            Event evt = new Event(80, EventType.Good, EventCategory.Drop, 3, "Care Package", () => true, (quote) => Helper.CargoDropItem(quote, amount, item));

            return evt;
        }

        public static int ItemPriceResolver(string itemname)
        {
            int price = 0;
            switch (itemname)
            {
                case "silver":
                    price = 5;
                break;
                case "uranium":
                        price = 30;
                break;
                case "survivalmeal":
                        price = 30;
                break;
                case "pastemeal":
                        price = 15;
                break;
                case "simplemeal":
                        price = 45;
                break;
                case "finemeal":
                        price = 60;
                break;
                case "kibble":
                        price = 5;
                break;
                case "hay":
                        price = 2;
                break;
                case "humanmeat":
                        price = 4;
                break;
                case "luciferium":
                        price = 300;
                break;
                case "pemmican":
                        price = 4;
                break;
                case "techprofsubpersonacore":
                        price = 5000;
                break;
                case "wort":
                        price = 12;
                break;
                case "gold":
                        price = 50;
                break;
                case "steel":
                        price = 8;
                break;
                case "wood":
                        price = 2;
                break;
                case "herbalmedicine":
                        price = 25;
                break;
                case "industrialmedicine":
                        price = 80;
                break;
                case "glitterworldmedicine":
                        price = 200;
                break;
                case "graniteblocks":
                        price = 4;
                break;
                case "plasteel":
                        price = 45;
                break;
                case "beer":
                        price = 40;
                break;
                case "aipersonacore":
                        price = 8000;
                break;
                case "smokeleafjoint":
                        price = 55;
                break;
                case "industrialcomponent":
                        price = 60;
                break;
                case "advcomponent":
                        price = 1000;
                break;
                case "insectjelly":
                        price = 40;
                break;
                case "cloth":
                        price = 40;
                break;
                case "plainleather":
                        price = 10;
                break;
                case "hyperweave":
                        price = 45;
                break;
                case "chocolate":
                        price = 15;
                break;
                case "elephanttusk":
                        price = 400;
                break;
                case "potatoes":
                        price = 5;
                break;
                case "berries":
                        price = 5;
                break;
                case "heart":
                        price = 2500;
                break;

                // weapons
                case "chargerifle":
                        price = 5000;
                break;
                case "revolver":
                        price = 100;
                break;
                case "boltactionrifle":
                        price = 250;
                break;
                case "chainshotgun":
                        price = 350;
                break;
                case "doomsdaylauncher":
                        price = 450;
                break;

                // apparel
                case "advancedhelmet":
                        price = 450;
                break;
                case "marinehelmet":
                        price = 1100;
                break;
                case "duster":
                        price = 300;
                break;
                case "tribalwear":
                        price = 50;
                break;
                case "tshirt":
                        price = 55;
                break;
                case "pants":
                        price = 55;
                break;
                case "cowboyhat":
                        price = 25;
                break;
                default:
                        price = 0;
                break;
            }
            
            return price;
        }

    }
}

