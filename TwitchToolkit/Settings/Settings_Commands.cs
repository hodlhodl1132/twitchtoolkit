using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    public static class Settings_Commands
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.Label("TwitchToolkitCommandsMenuExpl".Translate());

            optionsListing.Gap();

            optionsListing.AddLabeledTextField("TwitchToolkitBalancecmd".Translate(), ref ToolkitSettings.BalanceCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitBuyeventcmd".Translate(), ref ToolkitSettings.BuyeventCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitBuyitemcmd".Translate(), ref ToolkitSettings.BuyitemCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitInstructionscmd".Translate(), ref ToolkitSettings.InstructionsCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitPurchaselistcmd".Translate(), ref ToolkitSettings.PurchaselistCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitModinfocmd".Translate(), ref ToolkitSettings.ModinfoCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitModsettingscmd".Translate(), ref ToolkitSettings.ModsettingsCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitKarmacmd".Translate(), ref ToolkitSettings.KarmaCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitGiftcmd".Translate(), ref ToolkitSettings.GiftCmd);
            optionsListing.AddLabeledTextField("TwitchToolkitCommandhelpcmd".Translate(), ref ToolkitSettings.CommandHelpCmd);
        }
    }
}
