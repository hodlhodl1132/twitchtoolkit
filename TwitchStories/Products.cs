using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace TwitchToolkit
{
    public class Products
    {
        public static Event[] defaultEvents = Events.GetEvents();

        public static Product[] GenerateDefaultProducts()
        {
            Product[] defaultProducts = 
            {
                new Product(0, 0, "Small raid", "smallraid", 0, 450, 0),
                new Product(1, 0, "Medium raid", "mediumraid", 0, 500, 1),
                new Product(2, 0, "Big raid", "bigraid", 0, 950, 2),
                new Product(3, 0, "Medium raid drop", "mediumraiddrop", 0, 1050, 3),
                new Product(4, 0, "Big raid drop", "bigraiddrop", 0, 1050, 4),
                new Product(5, 0, "Big siege", "bigsiege", 0, 1300, 5),
                new Product(6, 0, "Big mechanoid raid", "bigmechanoidraid", 0, 1300, 6),
                new Product(7, 0, "Infestation", "infestation", 0, 1050, 7),

                new Product(8, 0, "Heart attack", "heartattack", 0, 600, 8),
                new Product(9, 0, "Plague", "plague", 0, 350, 9),
                new Product(10, 0, "Flu", "flu", 0, 400, 10),
                new Product(11, 0, "Infection", "infection", 0, 200, 11),
                new Product(12, 0, "Malaria", "malaria", 0, 400, 12),
                new Product(13, 0, "Gut worms", "gutworms", 0, 400, 13),
                new Product(14, 0, "Muscle parasites", "muscleparasites", 0, 400, 14),

                new Product(15, 0, "Plague medium", "plaguemedium", 0, 900, 15),
                new Product(16, 0, "Flu medium", "flumedium", 0, 900, 16),
                new Product(17, 0, "Infection medium", "infectionmedium", 0, 1100, 17),
                new Product(18, 0, "Malaria medium", "malariamedium", 0, 1100, 18),
                new Product(19, 0, "Gut worms medium", "gutwormsmedium", 0, 1100, 19),
                new Product(20, 0, "Muscle parasites medium", "muscleparasitesmedium", 0, 1100, 20),

                new Product(21, 0, "Plague hard", "plaguehard", 0, 1500, 21),
                new Product(22, 0, "Flu hard", "fluhard", 0, 1500, 22),
                new Product(23, 0, "Infection hard", "infectionhard", 0, 1500, 23),
                new Product(24, 0, "Malaria hard", "malariahard", 0, 1500, 24),
                new Product(25, 0, "Gut worms hard", "gutwormshard", 0, 1500, 25),
                new Product(26, 0, "Muscle parasites hard", "muscleparasiteshard", 0, 1500, 26),
                new Product(27, 0, "Blindness", "blindness", 0, 1500, 27),

                new Product(28, 0, "Clear weather", "clearweather", 1, 300, 28),
                new Product(29, 0, "Rain", "rain", 1, 300, 29),
                new Product(30, 0, "Rainy thunderstorm", "rainythunderstorm", 2, 200, 30),
                new Product(31, 0, "Dry thunderstorm", "drythunderstorm", 0, 250, 31),
                new Product(32, 0, "Snow gentle", "snowgentle", 2, 300, 32),
                new Product(33, 0, "Snow hard", "snowhard", 0, 400, 33),
                new Product(34, 0, "Fog", "fog", 2, 150, 34),
                new Product(35, 0, "Foggy rain", "foggyrain", 2, 250, 35),
                new Product(36, 0, "Flash storm", "flashstorm", 0, 300, 36),

                new Product(37, 0, "Eclipse", "eclipse", 0, 400, 37),
                new Product(38, 0, "Aurora", "aurora", 1, 150, 38),
                new Product(39, 0, "Vomit rain", "vomitrain", 0, 150, 39),
                new Product(40, 0, "Ambrosia sprout", "ambrosiasprout", 1, 150, 40),
                new Product(41, 0, "Meteorite", "meteorite", 2, 150, 41),
                new Product(42, 0, "Meteorite shower", "meteoriteshower", 2, 600, 42),

                new Product(43, 0, "Blight", "blight", 0, 500, 43),
                new Product(44, 0, "Solar flare", "solarflare", 0, 300, 44),
                new Product(45, 0, "Volcanic winter", "volcanicwinter", 3, 900, 45),
                new Product(46, 0, "Toxic fallout", "toxicfallout", 3, 1500, 46),
                new Product(47, 0, "Heat wave", "heatwave", 0, 500, 47),
                new Product(48, 0, "Cold snap", "coldsnap", 0, 550, 48),
                new Product(49, 0, "Tornado", "tornado", 0, 500, 49),
                new Product(50, 0, "Tornados", "tornados", 3, 1100, 50),

                new Product(51, 0, "Wild human", "wildhuman", 1, 200, 51),
                new Product(52, 0, "Wanderer joins", "wandererjoins", 2, 250, 52),
                new Product(53, 0, "Gender swap", "genderswap", 2, 150, 53),
                new Product(54, 0, "Skill increase", "skillincrease", 1, 150, 54),
                new Product(55, 0, "Party", "party", 1, 300, 55),
                new Product(56, 0, "Cannibal", "cannibal", 0, 450, 56),
                new Product(57, 0, "Luciferium", "luciferium", 3, 5000, 57),

                new Product(58, 0, "Mad animal", "mananimal", 0, 600, 58),
                new Product(59, 0, "Herd migration", "herdmigration", 1, 400, 59),
                new Product(60, 0, "Animals wander in", "animalswanderin", 1, 300, 60),
                new Product(61, 0, "Rare thrumbos", "thrumbos", 1, 600, 61),
                new Product(62, 0, "Farm animals", "farmanimals", 1, 200, 62),
                new Product(63, 0, "Animal self-tamed", "animalselftame", 1, 200, 63),
                new Product(64, 0, "Yorkshire terriers", "yorkshireterriers", 1, 300, 64),
                new Product(65, 0, "Manhunter pack", "manhunterpack", 0, 400, 65),
                new Product(66, 0, "Predators", "predators", 3, 1500, 66),

                new Product(67, 0, "Inspiration", "inspiration", 1, 150, 67),
                new Product(68, 0, "Psychic wave", "psychicwave", 0, 300, 68),
                new Product(69, 0, "Psychic drone", "psychicdrone", 0, 400, 69),
                new Product(70, 0, "Psychic soothe", "psychicsoothe", 1, 250, 70),
                new Product(71, 0, "Minor mental break", "minormentalbreak", 0, 200, 71),
                new Product(72, 0, "Major mental break", "majormentalbreak", 0, 200, 72),
                new Product(73, 0, "Extreme mental break", "extremementalbreak", 3, 1800, 73),
                new Product(74, 0, "Berserk mental break", "berserkmentalbreak", 3, 10000, 74),

                new Product(75, 0, "Ship chunk drop", "shipchunkdrop", 1, 300, 75),
                new Product(76, 0, "Cargo pod dropped", "cargopoddropped", 1, 300, 76),
                new Product(77, 0, "Transport pod dropped", "transportpoddropped", 1, 600, 77),
                new Product(78, 0, "Ship part poison", "shippartpoison", 0, 900, 78),
                new Product(79, 0, "Ship part psychic", "shippartpsychic", 0, 1000, 79),
                new Product(80, 1, "Care package", "carepackage", 1, 0, 80),

                new Product(81, 0, "Man in black", "maninblack", 1, 250, 81),
                new Product(82, 0, "Refugee chased", "refugeechased", 2, 450, 82),
                new Product(83, 0, "Traveler", "traveler", 1, 300, 83),
                new Product(84, 0, "Visitor", "visitor", 1, 150, 84),
                new Product(85, 0, "Trader visting", "tradervisiting", 1, 225, 85),
                new Product(86, 0, "Trader ship", "tradership", 1, 300, 86),
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
        public int id;
        public int type; //0 event - 1 care package
        public string name; //Small raid
        public string abr; //smallraid
        public int karmatype; //0 bad - 1 good - 2 neutral - 3 doom
        public int amount;
        public int evtId;
        public Event evt;


        public Product(int id, int type, string name, string abr, int karmatype, int amount, int evtId)
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.abr = abr;
            this.karmatype = karmatype;
            this.amount = amount;
            this.evtId = evtId;
            this.evt = Products.defaultEvents[this.evtId];    
        }

        public static int GetProductIdFromAbr(string abr)
        {
            return Settings.products.Find(x => x.abr == abr).id;
        }

        public static Product GetProductFromId(int id)
        {
            return Settings.products.Find(x => x.id == id);
        }

        public void SetProductAmount(int id, int amount)
        {
            Settings.ProductAmounts[id] = amount;
            this.amount = amount;
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
                    this.item = command[0];
                    int itemPrice = CarePackage.ItemPriceResolver(this.item);
                    bool isNumeric = int.TryParse(command[2], out this.quantity);
                    if (isNumeric && itemPrice > 0)
                    { 
                        Helper.Log($"item: {this.item} - price: {itemPrice} - isnumeric: {isNumeric} - quantity{this.quantity}");
                        this.calculatedprice = this.quantity * itemPrice;
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
                this.viewer.SetViewerKarma(Karma.CalculateNewKarma(this.viewer.GetViewerKarma(), this.product.karmatype));
            }
            else if (this.product.type == 1)
            {
                // care package
                this.product.evt = CarePackage.CarePackageGenerate(this.item, this.quantity);

                if (this.product.evt == null)
                {
                    Helper.Log("Could not generate care package");
                    return;
                }   

                this.successmessage = $"{this.quantity} {this.item} purchased by @{this.viewer.username}";
                this.product.evt.chatmessage = craftedmessage;
                this.viewer.SetViewerKarma(Karma.CalculateNewKarma(this.viewer.GetViewerKarma(), this.product.karmatype, this.calculatedprice));
            }
            
            // create purchase event
            Ticker.Events.Enqueue(this.product.evt);
        }
    }

    public class CarePackage
    {
        public static Event CarePackageGenerate(string item, int amount)
        {
            try
            {
                return new Event(80, EventType.Good, EventCategory.Drop, 3, "Gold", () => true, (quote) => Helper.CargoDropItem(quote, amount, item));
            }
            catch (InvalidCastException e)
            {
                Helper.Log("Carepackage error " + e.Message);
            }
            
            return null;
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

