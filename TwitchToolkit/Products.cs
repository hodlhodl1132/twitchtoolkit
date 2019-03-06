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
                new Product(0, 0, "Small raid", "smallraid", 0, 900, 0),
                new Product(1, 0, "Medium raid", "mediumraid", 0, 1200, 1),
                new Product(2, 0, "Big raid", "bigraid", 0, 1800, 2),
                new Product(3, 0, "Medium raid drop", "mediumraiddrop", 0, 2100, 3),
                new Product(4, 0, "Big raid drop", "bigraiddrop", 0, 2200, 4),
                new Product(5, 0, "Big siege", "bigsiege", 0, 1900, 5),
                new Product(6, 0, "Big mechanoid raid", "bigmechanoidraid", 0, 2400, 6),
                new Product(7, 0, "Infestation", "infestation", 0, 1600, 7),

                new Product(8, 0, "Heart attack", "heartattack", 0, 1100, 8),
                new Product(9, 0, "Plague", "plague", 0, 650, 9),
                new Product(10, 0, "Flu", "flu", 0, 600, 10),
                new Product(11, 0, "Infection", "infection", 0, 300, 11),
                new Product(12, 0, "Malaria", "malaria", 0, 550, 12),
                new Product(13, 0, "Gut worms", "gutworms", 0, 400, 13),
                new Product(14, 0, "Muscle parasites", "muscleparasites", 0, 600, 14),

                new Product(15, 0, "Plague medium", "plaguemedium", 0, 1100, 15),
                new Product(16, 0, "Flu medium", "flumedium", 0, 1100, 16),
                new Product(17, 0, "Infection medium", "infectionmedium", 0, 1300, 17),
                new Product(18, 0, "Malaria medium", "malariamedium", 0, 1300, 18),
                new Product(19, 0, "Gut worms medium", "gutwormsmedium", 0, 1300, 19),
                new Product(20, 0, "Muscle parasites medium", "muscleparasitesmedium", 0, 1300, 20),

                new Product(21, 0, "Plague hard", "plaguehard", 0, 1900, 21),
                new Product(22, 0, "Flu hard", "fluhard", 0, 1900, 22),
                new Product(23, 0, "Infection hard", "infectionhard", 0, 1900, 23),
                new Product(24, 0, "Malaria hard", "malariahard", 0, 1900, 24),
                new Product(25, 0, "Gut worms hard", "gutwormshard", 0, 1900, 25),
                new Product(26, 0, "Muscle parasites hard", "muscleparasiteshard", 0, 1900, 26),
                new Product(27, 0, "Blindness", "blindness", 0, 1900, 27),

                new Product(28, 0, "Clear weather", "clearweather", 1, 400, 28),
                new Product(29, 0, "Rain", "rain", 1, 350, 29),
                new Product(30, 0, "Rainy thunderstorm", "rainythunderstorm", 2, 250, 30),
                new Product(31, 0, "Dry thunderstorm", "drythunderstorm", 0, 450, 31),
                new Product(32, 0, "Snow gentle", "snowgentle", 2, 350, 32),
                new Product(33, 0, "Snow hard", "snowhard", 0, 450, 33),
                new Product(34, 0, "Fog", "fog", 2, 300, 34),
                new Product(35, 0, "Foggy rain", "foggyrain", 2, 300, 35),
                new Product(36, 0, "Flash storm", "flashstorm", 0, 300, 36),

                new Product(37, 0, "Eclipse", "eclipse", 0, 450, 37),
                new Product(38, 0, "Aurora", "aurora", 1, 200, 38),
                new Product(39, 0, "Vomit rain", "vomitrain", 0, -1, 39),
                new Product(40, 0, "Ambrosia sprout", "ambrosiasprout", 1, 150, 40),
                new Product(41, 0, "Meteorite", "meteorite", 2, 150, 41),
                new Product(42, 0, "Meteorite shower", "meteoriteshower", 2, 600, 42),

                new Product(43, 0, "Blight", "blight", 0, 700, 43),
                new Product(44, 0, "Solar flare", "solarflare", 0, 500, 44),
                new Product(45, 0, "Volcanic winter", "volcanicwinter", 3, 2600, 45),
                new Product(46, 0, "Toxic fallout", "toxicfallout", 3, 4200, 46),
                new Product(47, 0, "Heat wave", "heatwave", 0, 750, 47),
                new Product(48, 0, "Cold snap", "coldsnap", 0, 800, 48),
                new Product(49, 0, "Tornado", "tornado", 0, -1, 49),
                new Product(50, 0, "Tornados", "tornados", 3, -1, 50),

                new Product(51, 0, "Wild human", "wildhuman", 1, 300, 51),
                new Product(52, 0, "Wanderer joins", "wandererjoins", 1, 450, 52),
                new Product(53, 0, "Gender swap", "genderswap", 2, 150, 53),
                new Product(54, 0, "Skill increase", "skillincrease", 1, 200, 54),
                new Product(55, 0, "Party", "party", 1, 400, 55),
                new Product(56, 0, "Cannibal", "cannibal", 0, 750, 56),
                new Product(57, 0, "Luciferium", "luciferium", 3, 6000, 57),

                new Product(58, 0, "Mad animal", "mananimal", 0, 600, 58),
                new Product(59, 0, "Herd migration", "herdmigration", 1, 400, 59),
                new Product(60, 0, "Animals wander in", "animalswanderin", 1, 300, 60),
                new Product(61, 0, "Rare thrumbos", "thrumbos", 1, 500, 61),
                new Product(62, 0, "Farm animals", "farmanimals", 1, 200, 62),
                new Product(63, 0, "Animal self-tamed", "animalselftame", 1, 150, 63),
                new Product(64, 0, "Yorkshire terriers", "yorkshireterriers", 1, 300, 64),
                new Product(65, 0, "Manhunter pack", "manhunterpack", 0, 400, 65),
                new Product(66, 0, "Predators", "predators", 3, 1500, 66),

                new Product(67, 0, "Inspiration", "inspiration", 1, 150, 67),
                new Product(68, 0, "Psychic wave", "psychicwave", 0, 300, 68),
                new Product(69, 0, "Psychic drone", "psychicdrone", 0, 400, 69),
                new Product(70, 0, "Psychic soothe", "psychicsoothe", 1, 250, 70),
                new Product(71, 0, "Minor mental break", "minormentalbreak", 0, 300, 71),
                new Product(72, 0, "Major mental break", "majormentalbreak", 0, 900, 72),
                new Product(73, 0, "Extreme mental break", "extremementalbreak", 3, 1800, 73),
                new Product(74, 0, "Berserk mental break", "berserkmentalbreak", 3, 10000, 74),

                new Product(75, 0, "Ship chunk drop", "shipchunkdrop", 1, 300, 75),
                new Product(76, 0, "Cargo pod dropped", "cargopoddropped", 1, 300, 76),
                new Product(77, 0, "Transport pod dropped", "transportpoddropped", 1, 600, 77),
                new Product(78, 0, "Ship part poison", "shippartpoison", 0, 1500, 78),
                new Product(79, 0, "Ship part psychic", "shippartpsychic", 0, 2200, 79),
                new Product(80, 1, "Care package", "carepackage", 1, -1, 80),

                new Product(81, 0, "Man in black", "maninblack", 1, 350, 81),
                new Product(82, 0, "Refugee chased", "refugeechased", 2, 650, 82),
                new Product(83, 0, "Traveler", "traveler", 1, 350, 83),
                new Product(84, 0, "Visitor", "visitor", 1, 200, 84),
                new Product(85, 0, "Trader visting", "tradervisiting", 1, 275, 85),
                new Product(86, 0, "Trader ship", "tradership", 1, 350, 86),
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
        public Item itemtobuy = null;

        public ShopCommand(string message, Viewer viewer)
        {
            string[] command = message.Split(' ');
            string productabr = command[1];
            this.message = message;
            this.viewer = viewer;

            this.product = Products.GetProduct(productabr.ToLower());

            if (this.product == null)
            {
                Helper.Log("Product is null");
                return;
            }

            Helper.Log("Configuring purchase");
            if (product.type == 0)
            { //event
                Helper.Log("Calculating price for event");
                if (this.product.amount < 0)
                {
                    return;
                }
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

                    Item itemtobuy = Item.GetItemFromAbr(command[0]);

                    if (itemtobuy == null)
                    {
                        return;
                    }

                    if (itemtobuy.price < 0)
                    {
                        return;
                    }

                    int itemPrice = itemtobuy.price;

                    // check if 2nd index of command is a number
                    if (command.Count() > 2)
                    { 
                        bool isNumeric = int.TryParse(command[2], out this.quantity);
                        int messageStartsAt = 3;
                        if (!isNumeric)
                        {
                            this.quantity = 1;
                            messageStartsAt = 2;
                        }

                        string[] chatmessage = command;
                        craftedmessage = $"{this.viewer.username}: ";
                        for (int i = messageStartsAt; i < chatmessage.Length; i++)
                        {
                            craftedmessage += chatmessage[i] + " ";
                        }
                    }
                    else if (command.Count() == 2)
                    {
                        // short command
                        this.quantity = 1;
                    }

                    //Thing itemThing = ThingMaker.MakeThing(ThingDef.Named(itemtobuy.defname));
                    //if (itemThing.def.Minifiable)
                    //{
                    //    this.quantity = 1;
                    //}

                    if (itemPrice > 0)
                    {
                        Helper.Log($"item: {this.item} - price: {itemPrice} - quantity{this.quantity}");
                        this.calculatedprice = itemtobuy.CalculatePrice(this.quantity);
                        this.itemtobuy = itemtobuy;
                    }
                }
                catch (InvalidCastException e)
                {
                    Helper.Log("Invalid product or quantity - " + e.Message);
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
                this.errormessage = $"@{this.viewer.username}, your selected purchase price is {this.calculatedprice} coins but you need to spend a minimum of {Settings.MinimumPurchasePrice}";
            }
            else if (this.product.type == 0 && !this.product.evt.IsPossible())
            {
                 this.errormessage = $"@{this.viewer.username} Event not possible";
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
                if (this.product.evt.IsPossible())
                { 
                    // normal event
                    this.successmessage = $"Event {this.product.name} purchased by @{this.viewer.username}";
                    this.viewer.SetViewerKarma(Karma.CalculateNewKarma(this.viewer.GetViewerKarma(), this.product.karmatype));
                }
            }
            else if (this.product.type == 1)
            {
                // care package 
                try
                {
                    this.product.evt = new Event(80, EventType.Good, EventCategory.Drop, 3, "Gold", () => true, (quote) => this.itemtobuy.PutItemInCargoPod(quote, this.quantity));
                }
                catch (InvalidCastException e)
                {
                    Helper.Log("Carepackage error " + e.Message);
                }

                if (this.product.evt == null)
                {
                    Helper.Log("Could not generate care package");
                    return;
                }

                this.successmessage = $"{this.quantity} {this.itemtobuy.abr} purchased by @{this.viewer.username}";
                this.product.evt.chatmessage = craftedmessage;
                this.viewer.SetViewerKarma(Karma.CalculateNewKarma(this.viewer.GetViewerKarma(), this.product.karmatype, this.calculatedprice));
            }

            // create purchase event
            Ticker.Events.Enqueue(this.product.evt);
        }
    }
}

