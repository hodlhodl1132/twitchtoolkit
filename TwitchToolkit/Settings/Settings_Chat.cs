using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Settings
{
    static class Settings_Chat
    {
        public static void DoWindowContents(Rect rect, Listing_Standard optionsListing)
        {
            optionsListing.AddLabeledTextField("TwitchToolkitChannel".Translate(), ref ToolkitSettings.Channel);
            optionsListing.AddLabeledTextField("TwitchToolkitUsername".Translate(), ref ToolkitSettings.Username);
            
            if (oauthWarningAccepted)
                optionsListing.AddLabeledTextField("TwitchToolkitOauthKey".Translate(), ref ToolkitSettings.OAuth);

            if (!oauthWarningAccepted)
                if (optionsListing.ButtonTextLabeled("TwitchToolkitOauthWarning".Translate(), "TwitchToolkitOauthUnderstand".Translate()))
                    oauthWarningAccepted = true;

            optionsListing.CheckboxLabeled("TwitchToolkitAutoConnect".Translate(), ref ToolkitSettings.AutoConnect);
            optionsListing.AddLabeledTextField("TwitchToolkitChannelID".Translate(), ref ToolkitSettings.ChannelID);
            optionsListing.AddLabeledTextField("TwitchToolkitChatroomID".Translate(), ref ToolkitSettings.ChatroomUUID);
        }

        static bool oauthWarningAccepted = false;
    }
}
