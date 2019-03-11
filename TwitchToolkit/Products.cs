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
                new Product(0, 0, "Small raid", "TwitchToolkitSmallraid".Translate(), KarmaType.Bad, 900, 0, 3),
                new Product(1, 0, "Medium raid", "TwitchToolkitMediumraid".Translate(), KarmaType.Bad, 1200, 1, 2),
                new Product(2, 0, "Big raid", "TwitchToolkitBigraid".Translate(), KarmaType.Bad, 1800, 2, 1),
                new Product(3, 0, "Medium raid drop", "TwitchToolkitMediumraiddrop".Translate(), KarmaType.Bad, 2100, 3, 1),
                new Product(4, 0, "Big raid drop", "TwitchToolkitBigraiddrop".Translate(), KarmaType.Bad, 2200, 4, 2),
                new Product(5, 0, "Big siege", "TwitchToolkitBigsiege".Translate(), KarmaType.Bad, 1900, 5, 1),
                new Product(6, 0, "Big mechanoid raid", "TwitchToolkitBigmechanoidraid".Translate(), KarmaType.Bad, 2400, 6, 1),
                new Product(7, 0, "Infestation", "TwitchToolkitInfestation".Translate(), KarmaType.Bad, 1600, 7, 1),

                new Product(8, 0, "Heart attack", "TwitchToolkitHeartattack".Translate(), KarmaType.Bad, 1100, 8, 1),
                new Product(9, 0, "Plague", "TwitchToolkitPlague".Translate(), KarmaType.Bad, 650, 9, 1),
                new Product(10, 0, "Flu", "TwitchToolkitFlu".Translate(), KarmaType.Bad, 600, 10, 1),
                new Product(11, 0, "Infection", "TwitchToolkitInfection".Translate(), KarmaType.Bad, 300, 11, 1),
                new Product(12, 0, "Malaria", "TwitchToolkitMalaria".Translate(), KarmaType.Bad, 550, 12, 1),
                new Product(13, 0, "Gut worms", "TwitchToolkitGutworms".Translate(), KarmaType.Bad, 400, 13, 1),
                new Product(14, 0, "Muscle parasites", "TwitchToolkitMuscleparasites".Translate(), KarmaType.Bad, 600, 14, 1),

                new Product(15, 0, "Plague medium", "TwitchToolkitPlaguemedium".Translate(), KarmaType.Bad, 1100, 15, 1),
                new Product(16, 0, "Flu medium", "TwitchToolkitFlumedium".Translate(), KarmaType.Bad, 1100, 16, 1),
                new Product(17, 0, "Infection medium", "TwitchToolkitInfectionmedium".Translate(), KarmaType.Bad, 1300, 17, 1),
                new Product(18, 0, "Malaria medium", "TwitchToolkitMalariamedium".Translate(), KarmaType.Bad, 1300, 18, 1),
                new Product(19, 0, "Gut worms medium", "TwitchToolkitGutwormsmedium".Translate(), KarmaType.Bad, 1300, 19, 1),
                new Product(20, 0, "Muscle parasites medium", "TwitchToolkitMuscleparasitesmedium".Translate(), KarmaType.Bad, 1300, 20, 1),

                new Product(21, 0, "Plague hard", "TwitchToolkitPlaguehard".Translate(), KarmaType.Bad, 1900, 21, 1),
                new Product(22, 0, "Flu hard", "TwitchToolkitFluhard".Translate(), KarmaType.Bad, 1900, 22, 1),
                new Product(23, 0, "Infection hard", "TwitchToolkitInfectionhard".Translate(), KarmaType.Bad, 1900, 23, 1),
                new Product(24, 0, "Malaria hard", "TwitchToolkitMalariahard".Translate(), KarmaType.Bad, 1900, 24, 1),
                new Product(25, 0, "Gut worms hard", "TwitchToolkitGutwormshard".Translate(), KarmaType.Bad, 1900, 25, 1),
                new Product(26, 0, "Muscle parasites hard", "TwitchToolkitMuscleparasiteshard".Translate(), KarmaType.Bad, 1900, 26, 1),
                new Product(27, 0, "Blindness", "TwitchToolkitBlindness".Translate(), KarmaType.Bad, 1900, 27, 1),

                new Product(28, 0, "Clear weather", "TwitchToolkitClearweather".Translate(), KarmaType.Good, 400, 28, 2),
                new Product(29, 0, "Rain", "TwitchToolkitRain".Translate(), KarmaType.Good, 350, 29, 2),
                new Product(30, 0, "Rainy thunderstorm", "TwitchToolkitRainythunderstorm".Translate(), KarmaType.Neutral, 250, 30, 2),
                new Product(31, 0, "Dry thunderstorm", "TwitchToolkitDrythunderstorm".Translate(), KarmaType.Bad, 450, 31, 2),
                new Product(32, 0, "Snow gentle", "TwitchToolkitSnowgentle".Translate(), KarmaType.Neutral, 350, 32, 2),
                new Product(33, 0, "Snow hard", "TwitchToolkitSnowhard".Translate(), KarmaType.Bad, 450, 33, 2),
                new Product(34, 0, "Fog", "TwitchToolkitFog".Translate(), KarmaType.Neutral, 300, 34, 2),
                new Product(35, 0, "Foggy rain", "TwitchToolkitFoggyrain".Translate(), KarmaType.Neutral, 300, 35, 2),
                new Product(36, 0, "Flash storm", "TwitchToolkitFlashstorm".Translate(), KarmaType.Bad, 300, 36, 2),

                new Product(37, 0, "Eclipse", "TwitchToolkitEclipse".Translate(), KarmaType.Bad, 450, 37, 2),
                new Product(38, 0, "Aurora", "TwitchToolkitAurora".Translate(), KarmaType.Good, 200, 38, 2),
                new Product(39, 0, "Vomit rain", "TwitchToolkitVomitrain".Translate(), KarmaType.Bad, -1, 39, 2),
                new Product(40, 0, "Ambrosia sprout", "TwitchToolkitAmbrosiasprout".Translate(), KarmaType.Good, 150, 40, 2),
                new Product(41, 0, "Meteorite", "TwitchToolkitMeteorite".Translate(), KarmaType.Neutral, 150, 41, 2),
                new Product(42, 0, "Meteorite shower", "TwitchToolkitMeteoriteshower".Translate(), KarmaType.Neutral, 600, 42, 2),

                new Product(43, 0, "Blight", "TwitchToolkitBlight".Translate(), KarmaType.Bad, 700, 43, 1),
                new Product(44, 0, "Solar flare", "TwitchToolkitSolarflare".Translate(), KarmaType.Bad, 500, 44, 1),
                new Product(45, 0, "Volcanic winter", "TwitchToolkitVolcanicwinter".Translate(), KarmaType.Doom, 2600, 45, 1),
                new Product(46, 0, "Toxic fallout", "TwitchToolkitToxicfallout".Translate(), KarmaType.Doom, 4200, 46, 1),
                new Product(47, 0, "Heat wave", "TwitchToolkitHeatwave".Translate(), KarmaType.Bad, 750, 47, 1),
                new Product(48, 0, "Cold snap", "TwitchToolkitColdsnap".Translate(), KarmaType.Bad, 800, 48, 1),
                new Product(49, 0, "Tornado", "TwitchToolkitTornado".Translate(), KarmaType.Bad, -1, 49, 1),
                new Product(50, 0, "Tornados", "TwitchToolkitTornados".Translate(), KarmaType.Bad, -1, 50, 1),

                new Product(51, 0, "Wild human", "TwitchToolkitWildhuman".Translate(), KarmaType.Good, 300, 51, 1),
                new Product(52, 0, "Wanderer joins", "TwitchToolkitWandererjoins".Translate(), KarmaType.Good, 450, 52, 2),
                new Product(53, 0, "Gender swap", "TwitchToolkitGenderswap".Translate(), KarmaType.Neutral, 150, 53, 2),
                new Product(54, 0, "Skill increase", "TwitchToolkitSkillincrease".Translate(), KarmaType.Good, 200, 54, 10),
                new Product(55, 0, "Party", "TwitchToolkitParty".Translate(), KarmaType.Good, 400, 55, 2),
                new Product(56, 0, "Cannibal", "TwitchToolkitCannibal".Translate(), KarmaType.Bad, 750, 56, 1),
                new Product(57, 0, "Luciferium", "TwitchToolkitLuciferium".Translate(), KarmaType.Doom, 6000, 57, 1),

                new Product(58, 0, "Mad animal", "TwitchToolkitMadanimal".Translate(), KarmaType.Bad, 600, 58, 2),
                new Product(59, 0, "Herd migration", "TwitchToolkitHerdmigration".Translate(), KarmaType.Good, 400, 59, 2),
                new Product(60, 0, "Animals wander in", "TwitchToolkitAnimalswanderin".Translate(), KarmaType.Good, 300, 60, 2),
                new Product(61, 0, "Rare thrumbos", "TwitchToolkitRarethrumbos".Translate(), KarmaType.Good, 500, 61, 2),
                new Product(62, 0, "Farm animals", "TwitchToolkitFarmanimals".Translate(), KarmaType.Good, 200, 62, 2),
                new Product(63, 0, "Animal self-tamed", "TwitchToolkitAnimalself".Translate(), KarmaType.Good, 150, 63, 2),
                new Product(64, 0, "Yorkshire terriers", "TwitchToolkitYorkshireterriers".Translate(), KarmaType.Good, 300, 64, 2),
                new Product(65, 0, "Manhunter pack", "TwitchToolkitManhunterpack".Translate(), KarmaType.Bad, 400, 65, 2),
                new Product(66, 0, "Predators", "TwitchToolkitPredators".Translate(), KarmaType.Doom, 1500, 66, 2),

                new Product(67, 0, "Inspiration", "TwitchToolkitInspiration".Translate(), KarmaType.Good, 150, 67, 3),
                new Product(68, 0, "Psychic wave", "TwitchToolkitPsychicwave".Translate(), KarmaType.Bad, 300, 68, 1),
                new Product(69, 0, "Psychic drone", "TwitchToolkitPsychicdrone".Translate(), KarmaType.Bad, 400, 69, 1),
                new Product(70, 0, "Psychic soothe", "TwitchToolkitPsychicsoothe".Translate(), KarmaType.Good, 250, 70, 2),
                new Product(71, 0, "Minor mental break", "TwitchToolkitMinormentalbreak".Translate(), KarmaType.Bad, 300, 71, 1),
                new Product(72, 0, "Major mental break", "TwitchToolkitMajormentalbreak".Translate(), KarmaType.Bad, 900, 72, 1),
                new Product(73, 0, "Extreme mental break", "TwitchToolkitExtremementalbreak".Translate(), KarmaType.Doom, 1800, 73, 1),
                new Product(74, 0, "Berserk mental break", "TwitchToolkitBerserkmentalbreak".Translate(), KarmaType.Doom, 10000, 74, 1),

                new Product(75, 0, "Ship chunk drop", "TwitchToolkitShipchunkdrop".Translate(), KarmaType.Good, 300, 75, 3),
                new Product(76, 0, "Cargo pod dropped", "TwitchToolkitCargopoddropped".Translate(), KarmaType.Good, 300, 76, 3),
                new Product(77, 0, "Transport pod dropped", "TwitchToolkitTransportpoddropped".Translate(), KarmaType.Good, 600, 77, 1),
                new Product(78, 0, "Ship part poison", "TwitchToolkitShippartpoison".Translate(), KarmaType.Bad, 1500, 78, 1),
                new Product(79, 0, "Ship part psychic", "TwitchToolkitShippartpsychic".Translate(), KarmaType.Bad, 2200, 79, 1),
                new Product(80, 1, "Care package", "TwitchToolkitCarepackage".Translate(), KarmaType.Good, -1, 80, 1),

                new Product(81, 0, "Man in black", "TwitchToolkitManinblack".Translate(), KarmaType.Good, 350, 81, 1),
                new Product(82, 0, "Refugee chased", "TwitchToolkitRefugeechased".Translate(), KarmaType.Neutral, 650, 82, 1),
                new Product(83, 0, "Traveler", "TwitchToolkitTraveler".Translate(), KarmaType.Good, 350, 83, 3),
                new Product(84, 0, "Visitor", "TwitchToolkitVisitor".Translate(), KarmaType.Good, 200, 84, 3),
                new Product(85, 0, "Trader visting", "TwitchToolkitTradervisting".Translate(), KarmaType.Good, 275, 85, 3),
                new Product(86, 0, "Trader ship", "TwitchToolkitTradership".Translate(), KarmaType.Good, 350, 86, 2),
                new Product(87, 0, "Military Aid", "TwitchToolkitMilitaryAid".Translate(), KarmaType.Good, 700, 87, 2),
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

        public static void MultiplyProductPrices(double multiplier)
        {
            foreach(Product product in Settings.products)
            {
                product.amount = Convert.ToInt32((double)product.amount * multiplier);
                if (product.amount < 1)
                {
                    product.amount = 1;
                }
                Settings.ProductAmounts[product.id] = product.amount;
            }
        }
    }

    public class Product
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


        public Product(int id, int type, string name, string abr, KarmaType karmatype, int amount, int evtId, int maxEvents)
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
                    int messageStartsAt = 3;

                    // check if 2nd index of command is a number
                    if (command.Count() > 2 && int.TryParse(command[2], out this.quantity))
                    { 
                        messageStartsAt = 3;
                    }
                    else if (command.Count() == 2)
                    {
                        // short command
                        this.quantity = 1;
                        messageStartsAt = 2;
                    }
                    else if (command[2] == "*")
                    {
                        Helper.Log("Getting max");
                        this.quantity = this.viewer.GetViewerCoins() / itemtobuy.price;
                        messageStartsAt = 3;
                        Helper.Log(this.quantity);
                    }
                    else
                    {
                        this.quantity = 1;
                        messageStartsAt = 2;
                        Helper.Log("Quantity not calculated");
                    }

                    string[] chatmessage = command;
                    craftedmessage = $"{this.viewer.username}: ";
                    for (int i = messageStartsAt; i < chatmessage.Length; i++)
                    {
                        craftedmessage += chatmessage[i] + " ";
                    }

                    try
                    {
                        if (itemPrice > 0)
                        {
                            Helper.Log($"item: {this.item} - price: {itemPrice} - quantity{this.quantity}");
                            this.calculatedprice = checked(itemtobuy.CalculatePrice(this.quantity));
                            this.itemtobuy = itemtobuy;
                        }
                    }
                    catch (OverflowException e)
                    {
                        Helper.Log("overflow in calculated price " + e.Message);
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
            else if (viewer.GetViewerCoins() < this.calculatedprice && !Settings.UnlimitedCoins)
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
            else if (this.product.maxEvents < 1 && Settings.EventsHaveCooldowns)
            {
                this.errormessage = $"@{this.viewer.username} Event on cooldown";
            }
            else
            {
                this.ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {
            // take user coins
            if (!Settings.UnlimitedCoins)
            {
                this.viewer.TakeViewerCoins(this.calculatedprice);
            }
            
            // create success message
            if (this.product.type == 0)
            {
                if (this.product.evt.IsPossible())
                { 
                    
                    // normal event
                    this.successmessage = $"Event {this.product.name} purchased by @{this.viewer.username}";
                    this.viewer.SetViewerKarma(Karma.CalculateNewKarma(this.viewer.GetViewerKarma(), this.product.karmatype));

                    if (Settings.EventsHaveCooldowns)
                    {       
                        // take of a cooldown for event and schedule for it to be taken off
                        this.product.maxEvents--;
                        Settings.JobManager.AddNewJob(new ScheduledJob(Settings.EventCooldownInterval, new Func<Product, bool>(IncrementProduct), product));
                    }
                }
                else
                {
                    // refund if event not possible anymore
                    this.viewer.GiveViewerCoins(this.calculatedprice);
                    return;
                }
            }
            else if (this.product.type == 1)
            {
                // care package 
                try
                {
                    this.product.evt = new Event(80, EventType.Good, EventCategory.Drop, 3, "Gold", () => true, (quote) => this.itemtobuy.PutItemInCargoPod(quote, this.quantity, this.viewer.username));
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

        public bool IncrementProduct(Product product)
        {
            product.maxEvents++;
            return true;
        }
    }
}

