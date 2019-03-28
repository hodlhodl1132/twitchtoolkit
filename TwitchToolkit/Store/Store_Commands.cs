using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using Verse;

namespace TwitchToolkit.Store
{
    public class StoreCommands
    {
        public string message;
        public Viewer viewer;
        public IncItem incItem;
        public int calculatedprice = 0;
        public string errormessage = null;
        public string successmessage = null;
        private string item = null;
        private int quantity = 0;
        private string craftedmessage;
        public Item itemtobuy = null;

        public StoreCommands(string message, Viewer viewer)
        {
            string[] command = message.Split(' ');
            string productabr = command[1];
            this.message = message;
            this.viewer = viewer;

            this.incItem = IncidentItems.GetIncItem(productabr.ToLower());
            if (this.incItem == null)
            {
                Helper.Log("Product is null");
                return;
            }

            Helper.Log("Configuring purchase");
            if (incItem.type == 0)
            { //event
                Helper.Log("Calculating price for event");
                if (this.incItem.price < 0)
                {
                    return;
                }

                this.calculatedprice = this.incItem.price;
                string[] chatmessage = command;
                craftedmessage = $"{this.viewer.username}: ";
                for (int i = 2; i < chatmessage.Length; i++)
                {
                    craftedmessage += chatmessage[i] + " ";
                }
                this.incItem.evt.chatmessage = craftedmessage;
            }
            else if (incItem.type == 1)
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
                        this.quantity = this.viewer.coins / itemtobuy.price;
                        messageStartsAt = 3;
                        Helper.Log(this.quantity.ToString());
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

            int MaxEventsTypeToCheck = ToolkitSettings.MaxNeutralEventsPerInterval;
            if (this.incItem.karmatype == KarmaType.Good)
            {
                MaxEventsTypeToCheck = ToolkitSettings.MaxGoodEventsPerInterval;
            }
            else if (this.incItem.karmatype == KarmaType.Neutral)
            {
                MaxEventsTypeToCheck = ToolkitSettings.MaxNeutralEventsPerInterval;
            }
            else if (this.incItem.karmatype == KarmaType.Doom || this.incItem.karmatype == KarmaType.Bad)
            {
                MaxEventsTypeToCheck = ToolkitSettings.MaxBadEventsPerInterval;
            }

        Helper.Log($"count {StorePurchaseLogger.CountRecentEventsOfType(this.incItem.karmatype, (int)ToolkitSettings.MaxEventsPeriod)} > max type {MaxEventsTypeToCheck - 1} ");

            if (this.calculatedprice <= 0)
            {
                // invalid price
                Helper.Log("Invalid price detected?");
            }
            else if (viewer.GetViewerCoins() < this.calculatedprice && !ToolkitSettings.UnlimitedCoins)
            {
                // send message not enough coins
                this.errormessage = Helper.ReplacePlaceholder("TwitchToolkitNotEnoughCoins".Translate(), viewer: viewer.username, amount: this.calculatedprice.ToString(), first: viewer.GetViewerCoins().ToString());
            }
            else if (calculatedprice < ToolkitSettings.MinimumPurchasePrice)
            {
                // does not meet minimum purchase price
                this.errormessage = Helper.ReplacePlaceholder("TwitchToolkitMinPurchaseNotMet".Translate(), viewer: this.viewer.username, amount: this.calculatedprice.ToString(), first: ToolkitSettings.MinimumPurchasePrice.ToString());
            }
            else if (this.incItem.type == 0 && !this.incItem.evt.IsPossible())
            {
                 this.errormessage = $"@{this.viewer.username} " + "TwitchToolkitEventNotPossible".Translate();
            }
            else if (this.incItem.maxEvents < 1 && ToolkitSettings.EventsHaveCooldowns)
            {
                this.errormessage = $"@{this.viewer.username} " + "TwitchToolkitEventOnCooldown".Translate();
            }
            else if (ToolkitSettings.MaxEvents && (StorePurchaseLogger.CountRecentEventsOfType(this.incItem.karmatype, (int)ToolkitSettings.MaxEventsPeriod) > MaxEventsTypeToCheck - 1))
            {
                this.errormessage = $"@{this.viewer.username} " + "TwitchToolkitMaxEvents".Translate();
            }
            else
            {
                this.ExecuteCommand();
            }
        }

        private void ExecuteCommand()
        {
            // take user coins
            if (!ToolkitSettings.UnlimitedCoins)
            {
                this.viewer.TakeViewerCoins(this.calculatedprice);
            }
            
            // create success message
            if (this.incItem.type == 0)
            {
                if (this.incItem.evt.IsPossible())
                { 
                    
                    // normal event
                    this.successmessage = Helper.ReplacePlaceholder("TwitchToolkitEventPurchaseConfirm".Translate(), first: this.incItem.name, viewer: this.viewer.username);
                    this.viewer.SetViewerKarma(Karma.CalculateNewKarma(this.viewer.GetViewerKarma(), this.incItem.karmatype, this.calculatedprice));

                    if (ToolkitSettings.EventsHaveCooldowns)
                    {       
                        // take of a cooldown for event and schedule for it to be taken off
                        this.incItem.maxEvents--;
                        Toolkit.JobManager.AddNewJob(new ScheduledJob(ToolkitSettings.EventCooldownInterval, new Func<object, bool>(IncrementProduct), incItem));
                    }
                }
                else
                {
                    // refund if event not possible anymore
                    this.viewer.GiveViewerCoins(this.calculatedprice);
                    return;
                }
            }
            else if (this.incItem.type == 1)
            {
                // care package 
                try
                {
                    this.incItem.evt = new Event(80, EventType.Good, EventCategory.Drop, 3, "Gold", () => true, (quote) => this.itemtobuy.PutItemInCargoPod(quote, this.quantity, this.viewer.username));
                }
                catch (InvalidCastException e)
                {
                    Helper.Log("Carepackage error " + e.Message);
                }

                if (this.incItem.evt == null)
                {
                    Helper.Log("Could not generate care package");
                    return;
                }

                this.successmessage = Helper.ReplacePlaceholder("TwitchToolkitItemPurchaseConfirm".Translate(), amount: this.quantity.ToString(), item: this.itemtobuy.abr, viewer: this.viewer.username);
                this.incItem.evt.chatmessage = craftedmessage;
                this.viewer.SetViewerKarma(Karma.CalculateNewKarma(this.viewer.GetViewerKarma(), this.incItem.karmatype, this.calculatedprice));

            }

            StorePurchaseLogger.LogPurchase(new Purchase(this.viewer.username, this.incItem.name, this.incItem.karmatype, this.calculatedprice, this.successmessage, DateTime.Now));
            // create purchase event
            Ticker.Events.Enqueue(this.incItem.evt);
        }

        public bool IncrementProduct(object obj)
        {
            IncItem product = obj as IncItem;
            product.maxEvents++;
            return true;
        }
    }
}
