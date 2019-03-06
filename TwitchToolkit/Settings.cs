using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;
using TwitchToolkitDev;

namespace TwitchToolkit
{
    public class Settings : ModSettings
    {
        public static string Channel = "";
        public static string Username = "";
        public static string OAuth = "";

        public static int VoteInterval = 5;
        public static int VoteTime;
        public static int VoteOptions;
        public static bool VoteEnabled = true;
        public static bool AutoConnect = true;
        public static bool OtherStorytellersEnabled = true;
        public static bool CommandsModsEnabled = true;
        public static bool CommandsAliveEnabled = true;
        public static bool QuotesEnabled = true;

        public static int CoinInterval;
        public static int CoinAmount;
        public static int MinimumPurchasePrice;
        public static int KarmaCap;

        public static string CustomPricingSheetLink;

        public static bool EarningCoins = true;
        public static bool StoreOpen = true;
        public static bool GiftingCoins;

        public static bool WhisperCmdsOnly;
        public static bool WhisperCmdsAllowed;
        public static bool PurchaseConfirmations;

        public static string JWTToken;
        public static string AccountID;

        public static bool MinifiableBuildings = false;

        public static string BalanceCmd;
        public static string BuyeventCmd;
        public static string BuyitemCmd;
        public static string InstructionsCmd;
        public static string PurchaselistCmd;
        public static string ModinfoCmd;
        public static string ModsettingsCmd;
        public static string KarmaCmd;
        public static string GiftCmd;

        // viewer storage
        public static Dictionary<string, int> ViewerIds = null;
        public static Dictionary<int, int> ViewerCoins = new Dictionary<int, int>();
        public static Dictionary<int, int> ViewerKarma = new Dictionary<int, int>();

        public static Dictionary<string, bool> ViewerModerators = new Dictionary<string, bool>();

        public static List<Viewer> listOfViewers;

        // product storage
        public static Dictionary<string, int> ProductIds = null;
        public static Dictionary<int, int> ProductTypes = new Dictionary<int, int>();
        public static Dictionary<int, string> ProductNames = new Dictionary<int, string>();
        public static Dictionary<int, int> ProductKarmaTypes = new Dictionary<int, int>();
        public static Dictionary<int, int> ProductAmounts = new Dictionary<int, int>();
        public static Dictionary<int, int> ProductEventIds = new Dictionary<int, int>();

        public static List<Product> products = null;

        // item storage
        public static Dictionary<string, int> ItemIds = new Dictionary<string, int>();
        public static Dictionary<int, int> ItemPrices = new Dictionary<int, int>();
        public static Dictionary<int, string> ItemDefnames = new Dictionary<int, string>();
        public static Dictionary<int, string> ItemStuffnames = new Dictionary<int, string>();

        public static List<Item> items = null;

        private static List<string> _Categories = Enum.GetNames(typeof(EventCategory)).ToList();
        public static List<int> CategoryWeights = Enumerable.Repeat<int>(100, _Categories.Count).ToList();

        public static double CategoryWeight(EventCategory category)
        {
            var index = _Categories.IndexOf(Enum.GetName(typeof(EventCategory), category));
            if (index < 0 || index >= CategoryWeights.Count)
            {
                return 1;
            }

            return CategoryWeights[index] / 100.0;
        }

        private static Dictionary<int, string> _Events = Events.GetEvents().ToDictionary(e => e.Id, e => e.Description);
        public static Dictionary<int, int> EventWeights = Events.GetEvents().ToDictionary(e => e.Id, e => 100);

        public static double EventWeight(int id)
        {
            if (!EventWeights.ContainsKey(id))
            {
                return 1;
            }

            return EventWeights[id] / 100.0;
        }

        public void Save()
        {
            LoadedModManager.WriteModSettings(this.Mod.Content.Identifier, this.Mod.GetType().Name, this);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref Channel, "Channel", "", true);
            Scribe_Values.Look(ref Username, "Username", "", true);
            Scribe_Values.Look(ref OAuth, "OAuth", "", true);

            Scribe_Values.Look(ref VoteTime, "VoteTime", 1, true);
            Scribe_Values.Look(ref VoteOptions, "VoteOptions", 3, true);
            Scribe_Values.Look(ref VoteEnabled, "VoteEnabled", false, true);
            Scribe_Values.Look(ref AutoConnect, "AutoConnect", false, true);
            Scribe_Values.Look(ref OtherStorytellersEnabled, "OtherStorytellersEnabled", false, true);
            Scribe_Values.Look(ref CommandsModsEnabled, "CommandsModsEnabled", true, true);
            Scribe_Values.Look(ref CommandsAliveEnabled, "CommandsAliveEnabled", true, true);
            Scribe_Values.Look(ref QuotesEnabled, "QuotesEnabled", true, true);

            Scribe_Values.Look(ref VoteInterval, "VoteInterval", 5, true);
            Scribe_Values.Look(ref CoinInterval, "CoinInterval", 2, true);
            Scribe_Values.Look(ref CoinAmount, "CoinAmount", 30, true);
            Scribe_Values.Look(ref KarmaCap, "KarmaCap", 500, true);
            Scribe_Values.Look(ref MinimumPurchasePrice, "MinimumPurchasePrice", 50, true);

            Scribe_Values.Look(ref CustomPricingSheetLink, "CustomPricingSheetLink", "https://bit.ly/2GT5daR", true);

            Scribe_Values.Look(ref EarningCoins, "EarningCoins", false, true);
            Scribe_Values.Look(ref StoreOpen, "StoreOpen", false, true);
            Scribe_Values.Look(ref GiftingCoins, "GiftingCoins", false, true);
            Scribe_Values.Look(ref WhisperCmdsAllowed, "WhisperCmdsAllowed", true, true);
            Scribe_Values.Look(ref WhisperCmdsOnly, "WhisperCmdsOnly", false, true);
            Scribe_Values.Look(ref PurchaseConfirmations, "PurchaseConfirmations", true, true);

            Scribe_Values.Look(ref JWTToken, "JWTToken", "", true);
            Scribe_Values.Look(ref AccountID, "AccountID", "", true);

            Scribe_Values.Look(ref MinifiableBuildings, "MinifiableBuildings", false, true);

            Scribe_Values.Look(ref BalanceCmd, "BalanceCmd", "!bal", true);
            Scribe_Values.Look(ref BuyeventCmd, "BuyeventCmd", "!buyevent", true);
            Scribe_Values.Look(ref BuyitemCmd, "BuyitemCmd", "!buyitem", true);
            Scribe_Values.Look(ref InstructionsCmd, "InstructionsCmd", "!instructions", true);
            Scribe_Values.Look(ref PurchaselistCmd, "PurchaselistCmd", "!purchaselist", true);
            Scribe_Values.Look(ref ModinfoCmd, "ModinfoCmd", "!modinfo", true);
            Scribe_Values.Look(ref ModsettingsCmd, "ModsettingsCmd", "!modsettings", true);
            Scribe_Values.Look(ref KarmaCmd, "KarmaCmd", "!whatiskarma", true);
            Scribe_Values.Look(ref GiftCmd, "GiftCmd", "!giftcoins", true);

            Scribe_Collections.Look(ref ViewerIds, "ViewerIds", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerCoins, "ViewerCoins", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerKarma, "ViewerKarma", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ViewerModerators, "ViewerModerators", LookMode.Value, LookMode.Value);

            Scribe_Collections.Look(ref ItemIds, "ItemIds", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ItemPrices, "ItemPrices", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ItemDefnames, "ItemDefnames", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ItemStuffnames, "ItemStuffnames", LookMode.Value, LookMode.Value);

            Scribe_Collections.Look(ref ProductIds, "ProductIds", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductTypes, "ProductTypes", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductNames, "ProductNames", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductKarmaTypes, "ProductKarmaTypes", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductAmounts, "ProductAmounts", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref ProductEventIds, "ProductEventIds", LookMode.Value, LookMode.Value);

            Scribe_Collections.Look(ref CategoryWeights, "CategoryWeights", LookMode.Value);


            if (ViewerIds == null)
            {
                ViewerIds = new Dictionary<string, int>();
                ViewerCoins = new Dictionary<int, int>();
                ViewerKarma = new Dictionary<int, int>();
            }


            if (CategoryWeights == null)
            {
                CategoryWeights = Enumerable.Repeat<int>(100, _Categories.Count).ToList();
            }

            Scribe_Collections.Look(ref EventWeights, "EventWeights", LookMode.Value);
            if (EventWeights == null)
            {
                EventWeights = Events.GetEvents().ToDictionary(e => e.Id, e => 100);
            }

            if (listOfViewers == null)
            {
                listOfViewers = new List<Viewer>();
                foreach (KeyValuePair<string, int> viewer in ViewerIds)
                {
                    int viewerkarma = ViewerKarma[viewer.Value];
                    int viewercoins = ViewerCoins[viewer.Value];
                    Viewer newviewer = new Viewer(viewer.Key, viewer.Value);
                    listOfViewers.Add(newviewer);
                }
            }

            if (products == null)
            {
                if (ProductIds == null)
                {
                    Helper.Log("Ressetting Products");
                    ResetProductData();
                }
                else
                {
                    Helper.Log("Loading Product Settings");
                    products = new List<Product>();
                    // load products from settings, then load them into settings class
                    foreach (KeyValuePair<string, int> product in ProductIds)
                    {
                        int id = product.Value;
                        string abr = product.Key;
                        int type = ProductTypes[id];
                        string name = ProductNames[id];
                        int karmatype = ProductKarmaTypes[id];
                        int amount = ProductAmounts[id];
                        int evtId = ProductEventIds[id];
                        products.Add(new Product(id, type, name, abr, karmatype, amount, evtId));
                    }
                }
            }

            if (items == null)
            {
                if (ItemIds == null)
                {
                    Helper.Log("Creating all items");
                    ResetItemData();
                }
                else
                {
                    Helper.Log("Loading items from settings");
                    items = new List<Item>();
                    
                    foreach(KeyValuePair<string, int> item in ItemIds)
                    {
                        int id = item.Value;
                        string abr = item.Key;
                        int price = ItemPrices[id];
                        string defname = ItemDefnames[id];
                        string stuffname = ItemStuffnames[id];
                        items.Add(new Item(price, abr, defname, id, stuffname));
                    }
                }
            }
        }

        private static int _showOAuth = 0;
        public static void HideOAuth()
        {
            _showOAuth = 0;
        }

        private static float _padding = 5f;
        private static float _height = 35f;
        private static int _menu = 0;
        public static void DoSettingsWindowContents(Rect rect)
        {
            float buttonWidth = 100f;

            var buttonRect = new Rect(rect.width - _padding - buttonWidth, _padding + _height, buttonWidth, 20f);
            if (Widgets.ButtonText(buttonRect, "Main"))
            {
                _menu = 0;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Coins"))
            {
                _menu = 3;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Events"))
            {
                _menu = 4;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Items"))
            {
                _menu = 1;
            }

            if (Prefs.DevMode)
            {
                buttonRect.y += _height;
                if (Widgets.ButtonText(buttonRect, "StreamElements"))
                {
                    _menu = 2;
                }
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Experimental"))
            {
                _menu = 5;
            }

            buttonRect.y += _height;
            if (Widgets.ButtonText(buttonRect, "Commands"))
            {
                _menu = 6;
            }

            rect.width -= buttonWidth + _padding;
            switch (_menu)
            {
                case 0:
                default:
                    MainMenu(rect);
                    break;
                case 1:
                    ItemMenu(rect);
                    break;
                case 2:
                    StreamElementsMenu(rect);
                    break;
                case 3:
                    CoinMenu(rect);
                    break;
                case 4:
                    if (products == null)
                    {
                        Helper.Log("Ressetting Products");
                        ResetProductData();
                    }
                    EventMenu(rect);
                    break;
                case 5:
                    DevMenu(rect);
                    break;
                case 6:
                    CommandMenu(rect);
                    break;
            }
        }

        private static void MainMenu(Rect rect)
        {
            var labelRect = new Rect(_padding, _padding + _height, rect.width - (_padding * 2), rect.height - (_padding * 2));
            var inputRect = new Rect(_padding + 140f, _padding + _height, rect.width - (_padding * 2) - 140f, 20f);
            Text.Anchor = TextAnchor.UpperLeft;

            Widgets.Label(labelRect, "TwitchStoriesSettingsTwitchChannel".Translate() + ": ");
            Channel = Widgets.TextField(inputRect, Channel, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsTwitchUsername".Translate() + ": ");
            Username = Widgets.TextField(inputRect, Username, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsOAuth".Translate() + ": ");
            if (_showOAuth > 2)
            {
                OAuth = Widgets.TextField(inputRect, OAuth, 999, new Regex("^[a-z0-9:]*$", RegexOptions.IgnoreCase));
            }
            else
            {
                if (Widgets.ButtonText(inputRect, ("TwitchStoriesSettingsOAuthWarning" + _showOAuth).Translate()))
                {
                    _showOAuth++;
                }
            }

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsTwitchAutoConnect".Translate() + ": ");
            if (Widgets.ButtonText(inputRect, (AutoConnect ? "TwitchStoriesEnabled".Translate() : "TwitchStoriesDisabled".Translate())))
            {
                AutoConnect = !AutoConnect;
            }

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsVoteTime".Translate() + ": ");
            VoteTime = (int)Widgets.HorizontalSlider(inputRect, VoteTime, 1, 15, false, VoteTime.ToString() + " " + "TwitchStoriesMinutes".Translate(), null, null, 1);

            labelRect.y += _height;
            inputRect.y += _height;
            Widgets.Label(labelRect, "TwitchStoriesSettingsVoteOptions".Translate() + ": ");
            VoteOptions = (int)Widgets.HorizontalSlider(inputRect, VoteOptions, 1, 5, false, VoteOptions.ToString() + " " + "TwitchStoriesOptions".Translate(), null, null, 1);

            labelRect.y += _height;
            inputRect.y += _height;
            inputRect.width = ((inputRect.width - _padding) / 2);
            Widgets.Label(labelRect, "TwitchStoriesSettingsOtherCommands".Translate() + ": ");
            if (Widgets.ButtonText(inputRect, "!installedmods " + (CommandsModsEnabled ? "TwitchStoriesEnabled".Translate() : "TwitchStoriesDisabled".Translate())))
            {
                CommandsModsEnabled = !CommandsModsEnabled;
            }

            inputRect.x += inputRect.width + _padding;
            if (Widgets.ButtonText(inputRect, "!alive " + (CommandsAliveEnabled ? "TwitchStoriesEnabled".Translate() : "TwitchStoriesDisabled".Translate())))
            {
                CommandsAliveEnabled = !CommandsAliveEnabled;
            }

            var mod = LoadedModManager.GetMod<TwitchToolkit>();
            labelRect.y += _height;
            labelRect.height = 30f;
            labelRect.width = 100f;
            if (Widgets.ButtonText(labelRect, "TwitchStoriesReconnect".Translate()))
            {
                mod.Reconnect();
            }

            labelRect.x += 100f + _padding;
            if (Widgets.ButtonText(labelRect, "TwitchStoriesDisconnect".Translate()))
            {
                mod.Disconnect();
            }

            labelRect.x = _padding;
            labelRect.y += _height;
            labelRect.height = rect.height - labelRect.y;
            labelRect.width = rect.width - (_padding * 2);
            Widgets.TextArea(labelRect, string.Join("\r\n", mod.MessageLog), true);
        }

        private static void CoinMenu(Rect rect)
        {
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled("Reward Coins: ", ref EarningCoins, "Should viewers earn coins while watching?");
            listingStandard.CheckboxLabeled("Store Open: ", ref StoreOpen, "Enable purchasing of events and items");
            listingStandard.CheckboxLabeled("Viewers can gift other viewers coins: ", ref GiftingCoins, "Enable gifting");
            listingStandard.Label("Coins Per Interval: " + CoinAmount);
            CoinAmount = listingStandard.Slider((float)CoinAmount, 1, 250);
            listingStandard.Label("Max Karma: " + KarmaCap);
            KarmaCap = listingStandard.Slider((float)KarmaCap, 100, 1000);
            listingStandard.Label("Minimum Purchase Price: " + MinimumPurchasePrice);
            MinimumPurchasePrice = listingStandard.Slider((float)MinimumPurchasePrice, 10, 500);
            listingStandard.Label("Minutes between coin reward: " + CoinInterval);
            CoinInterval = listingStandard.Slider((float)CoinInterval, 1, 60);
            listingStandard.Label("Link to custom pricing sheet: (check steamworkshop description for instructions)");
            CustomPricingSheetLink = listingStandard.TextEntry(CustomPricingSheetLink);

            listingStandard.End();
        }

        public static void DevMenu(Rect rect)
        {
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled("Should buildings unable to be uninstalled be included in the item list? ", ref MinifiableBuildings, "Non-Minifiable Buildings?");
            listingStandard.End();
        }

        public static void CommandMenu(Rect rect)
        {
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            listingStandard.CheckboxLabeled($"Commands can be whispered to {Username}: ", ref WhisperCmdsAllowed, "Allow whispers");
            listingStandard.CheckboxLabeled($"Commands must be whispered to {Username}: ", ref WhisperCmdsOnly, "Require whispers");  
            listingStandard.CheckboxLabeled("Should purchases be confirmed in chat: ", ref PurchaseConfirmations, "Purchase confirmations");
            listingStandard.Label("Check your coin balance");
            BalanceCmd = listingStandard.TextEntry(BalanceCmd);
            listingStandard.Label("Buy an event");
            BuyeventCmd = listingStandard.TextEntry(BuyeventCmd);
            listingStandard.Label("Buy an item");
            BuyitemCmd = listingStandard.TextEntry(BuyitemCmd);
            listingStandard.Label("Instructions");
            InstructionsCmd = listingStandard.TextEntry(InstructionsCmd);
            listingStandard.Label("Purchase list");
            PurchaselistCmd = listingStandard.TextEntry(PurchaselistCmd);
            listingStandard.Label("Mod info");
            ModinfoCmd = listingStandard.TextEntry(ModinfoCmd);
            listingStandard.Label("Mod settings");
            ModsettingsCmd = listingStandard.TextEntry(ModsettingsCmd);
            listingStandard.Label("Karma explanation");
            KarmaCmd = listingStandard.TextEntry(KarmaCmd);
            listingStandard.Label("Gift coins");
            GiftCmd = listingStandard.TextEntry(GiftCmd);

            listingStandard.End();
        }

        private static void StreamElementsMenu(Rect rect)
        {
            Listing_TwitchToolkit listingStandard = new Listing_TwitchToolkit();
            listingStandard.Begin(rect);
            JWTToken = listingStandard.TextEntry(JWTToken, 3);
            AccountID = listingStandard.TextEntry(AccountID);

            var inputRect = new Rect(_padding + 140f, _padding + _height * 3, rect.width - (_padding * 2) - 140f, 20f);

            if (Widgets.ButtonText(inputRect, "Import Points"))
            {
                StreamElements element = new StreamElements(AccountID, JWTToken);
                element.ImportPoints();
            }

            listingStandard.End();
        }

        public static int ProductScroll = 0;
        private static string searchquery;

        public static int ResetProductStage { get; private set; }
        public static string ResetAdminWarning { get; private set; }
        public static int ItemScroll { get; private set; }
        public static int ResetItemStage { get; private set; }
        public static int ResetViewerStage { get; internal set; }

        public static void EventMenu(Rect rect)
        {
            var scrollRect = new Rect(_padding + 300f, _padding + _height, 40f, 20f);

            var searchRect = new Rect(_padding, _padding + _height, 300f, 20f);

            searchquery = Widgets.TextField(searchRect, searchquery, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));

            if (ProductScroll > 0)
            {
                if (Widgets.ButtonText(scrollRect, "up"))
                {
                    ProductScroll = Math.Max(0, ProductScroll - 1);
                }
            }

            int count = 0;
            int scroll = 0;

            if (ProductScroll < (products.Count - count) - 8)
            {
                scrollRect.x += 40f;
                if (Widgets.ButtonText(scrollRect, "down"))
                {
                    ProductScroll++;
                }
            }

            if (ProductScroll > 0)
            {
                scrollRect.x += 40f;
                if (Widgets.ButtonText(scrollRect, "top"))
                {
                    ProductScroll = 0;
                }
            }

            scrollRect.x = _padding + (rect.width - (_padding * 2)) / 2 + (rect.width - (_padding * 2)) / 4;
            scrollRect.width = (rect.width - (_padding * 2)) / 4;

            if (ResetProductStage == 0)
            {
                ResetAdminWarning = "Reset to Default";
            }
            else if (ResetProductStage == 1)
            {
                ResetAdminWarning = "Are you sure?";
            }
            else if (ResetProductStage == 2)
            {
                ResetAdminWarning = "One more time";
            }
            else if (ResetProductStage == 3)
            {
                ResetProductStage = 0;
                ResetProductData();
                EventMenu(rect);
            }

            if (Widgets.ButtonText(scrollRect, ResetAdminWarning))
            {
                ResetProductStage += 1;
            }

            scrollRect.y += _height;
            
            Rect productline = new Rect(_padding, _padding + _height, 600f, 30f);
            
            List<Product> query = products.Where(a => (a.abr.Contains(searchquery))).ToList();
            foreach (Product product in query)
            {
                if (++scroll <= ProductScroll)
                {
                    continue;
                }

                productline.y += 30f;

                if (productline.y  > rect.height - 50f)
                {
                    continue;
                }

                Rect smallButton = new Rect(300f, productline.y, 40f, 30f);

                string pricelabel = (product.amount) < 0 ? "Disabled" : product.amount.ToString();
                Widgets.Label(productline, $"{ProductScroll + count + 1} - {product.name}: {pricelabel}");
                
                int newprice = product.amount;

                if (Widgets.ButtonText(smallButton, "-" + 500))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 500 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 50)
                    {
                        newprice = 50;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 50))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 50 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 50)
                    {
                        newprice = 50;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 10))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 10 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 50)
                    {
                        newprice = 50;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 10))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 10 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 50))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 50 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 500))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 500 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;

                string karmabutton = "";

                if (product.karmatype == 0)
                {
                    karmabutton = "Bad";
                }
                else if (product.karmatype == 1)
                {
                    karmabutton = "Good";
                }
                else if (product.karmatype == 2)
                {
                    karmabutton = "Neutral";
                }
                else
                {
                    karmabutton = "Doom";
                }

                smallButton.x += 60f;
                smallButton.width = 60f;
                if (Widgets.ButtonText(smallButton, karmabutton))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    if (product.karmatype == 3)
                    {
                        product.karmatype = 0;
                        ProductKarmaTypes[product.id] = 0;
                    }
                    else
                    {
                        product.karmatype = product.karmatype + 1;
                        ProductKarmaTypes[product.id] = ProductKarmaTypes[product.id] + 1;
                    }
                }

                smallButton.x += 60f;
                if (Widgets.ButtonText(smallButton, "Disable"))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice = -10;
                }

                ProductAmounts[product.id] = newprice;
                product.amount = newprice;

                count++;
            }
        }


        public static void ItemMenu(Rect rect)
        {
            var scrollRect = new Rect(_padding + 300f, _padding + _height, 40f, 20f);

            var searchRect = new Rect(_padding, _padding + _height, 300f, 20f);

            searchquery = Widgets.TextField(searchRect, searchquery, 999, new Regex("^[a-z0-9_]*$", RegexOptions.IgnoreCase));

            if (ItemScroll > 0)
            {
                if (Widgets.ButtonText(scrollRect, "up"))
                {
                    ItemScroll = Math.Max(0, ItemScroll - 1);
                }
            }

            int count = 0;
            int scroll = 0;

            if (ItemScroll < (items.Count - count) - 8)
            {
                scrollRect.x += 40f;
                if (Widgets.ButtonText(scrollRect, "down"))
                {
                    ItemScroll++;
                }
            }

            if (ItemScroll > 0)
            {
                scrollRect.x += 40f;
                if (Widgets.ButtonText(scrollRect, "top"))
                {
                    ItemScroll = 0;
                }
            }

            scrollRect.x = _padding + (rect.width - (_padding * 2)) / 2 + (rect.width - (_padding * 2)) / 4;
            scrollRect.width = (rect.width - (_padding * 2)) / 4;

            if (ResetItemStage == 0)
            {
                ResetAdminWarning = "Reset to Default";
            }
            else if (ResetItemStage == 1)
            {
                ResetAdminWarning = "Are you sure?";
            }
            else if (ResetItemStage == 2)
            {
                ResetAdminWarning = "One more time";
            }
            else if (ResetItemStage == 3)
            {
                ResetItemStage = 0;
                ResetItemData();
                ItemMenu(rect);
            }

            if (Widgets.ButtonText(scrollRect, ResetAdminWarning))
            {
                ResetItemStage += 1;
            }

            scrollRect.y += _height;
            
            Rect itemline = new Rect(_padding, _padding + _height, 600f, 30f);
            
            List<Item> query = items.Where(a => (a.abr.Contains(searchquery.ToLower()))).ToList();
            foreach (Item item in query)
            {
                if (++scroll <= ItemScroll)
                {
                    continue;
                }

                itemline.y += 30f;

                if (itemline.y  > rect.height - 50f)
                {
                    continue;
                }

                Rect smallButton = new Rect(300f, itemline.y, 40f, 30f);

                string pricelabel = (item.price) < 0 ? "Disabled" : item.price.ToString();
                Widgets.Label(itemline, $"{ItemScroll + count + 1} - {item.abr}: {pricelabel}");

                int newprice = item.price;
                if (Widgets.ButtonText(smallButton, "-" + 100))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 100 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 1)
                    {
                        newprice = 1;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 10))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 10 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 1)
                    {
                        newprice = 1;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "-" + 1))
                {
                    SoundDefOf.AmountDecrement.PlayOneShotOnCamera();
                    newprice -= 1 * GenUI.CurrentAdjustmentMultiplier();
                    if (newprice < 1)
                    {
                        newprice = 1;
                    }
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 1))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 1 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 10))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 10 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                if (Widgets.ButtonText(smallButton, "+" + 100))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice += 100 * GenUI.CurrentAdjustmentMultiplier();
                }

                smallButton.x += 40f;
                smallButton.width = 60f;
                if (Widgets.ButtonText(smallButton, "Disable"))
                {
                    SoundDefOf.AmountIncrement.PlayOneShotOnCamera();
                    newprice = -10;
                }

                ItemPrices[item.id] = newprice;
                item.price = newprice;

                count++;
            }
        }

        public static void ResetProductData()
        {
            products = new List<Product>();
            ProductIds = new Dictionary<string, int>();
            ProductTypes = new Dictionary<int, int>();
            ProductNames = new Dictionary<int, string>();
            ProductKarmaTypes = new Dictionary<int, int>();
            ProductAmounts = new Dictionary<int, int>();
            ProductEventIds = new Dictionary<int, int>();
            // if no previous save data create new products
            List<Product> defaultProducts = Products.GenerateDefaultProducts().ToList();
            foreach (Product product in defaultProducts)
            {
                int id = product.id;
                ProductIds.Add(product.abr, id);
                ProductTypes.Add(id, product.type);
                ProductNames.Add(id, product.name);
                ProductKarmaTypes.Add(id, product.karmatype);
                ProductAmounts.Add(id, product.amount);
                ProductEventIds.Add(id, product.evtId);
                products.Add(product);
            }
        }

        public static void ResetItemData()
        {
            items = new List<Item>();
            ItemIds = new Dictionary<string, int>();
            ItemPrices = new Dictionary<int, int>();
            ItemDefnames = new Dictionary<int, string>();
            ItemStuffnames = new Dictionary<int, string>();

            List<Item> defaultitems = Item.GetDefaultItems().ToList();

            Item.TryMakeAllItems();

            //foreach(Item item in defaultitems)
            //{
            //    int id = item.id;
            //    ItemIds.Add(item.abr, id);
            //    ItemPrices.Add(id, item.price);
            //    ItemDefnames.Add(id, item.defname);
            //    if (item.stuffname != "null")
            //    {
            //        ItemStuffnames[id] = item.stuffname;
            //    }
            //    else
            //    {
            //        ItemStuffnames[id] = "null";
            //    }
            //    items.Add(item);
            //}
        }

    }
}
