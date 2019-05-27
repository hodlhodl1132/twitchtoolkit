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
            {
                optionsListing.AddLabeledTextField("TwitchToolkitOauthKey".Translate(), ref ToolkitSettings.OAuth);
                if (optionsListing.ButtonTextLabeled("TwitchToolkitHideOauth".Translate(), "TwitchToolkitHideOauthButton".Translate()))
                    oauthWarningAccepted = false;
            }
                
            if (!oauthWarningAccepted)
                if (optionsListing.ButtonTextLabeled("TwitchToolkitOauthWarning".Translate(), "TwitchToolkitOauthUnderstand".Translate()))
                    oauthWarningAccepted = true;

            optionsListing.Gap();

            if (optionsListing.CenteredButton("TwitchToolkitGetNewOauthKey".Translate()))
                Application.OpenURL("https://twitchapps.com/tmi/");

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("Auto Connect to Twitch on Startup", ref ToolkitSettings.AutoConnect);

            optionsListing.Gap();    

            optionsListing.Label("TwitchToolkitWhatIsSeparateChannel".Translate());
            if (optionsListing.CenteredButton("TwitchToolkitLearnMore".Translate()))
                Application.OpenURL("https://techcrunch.com/2018/02/15/twitch-launches-always-on-chat-rooms-for-channels/");

            optionsListing.CheckboxLabeled("TwitchToolkitUseSeparateChannel".Translate(), ref ToolkitSettings.UseSeparateChatRoom);
            optionsListing.CheckboxLabeled("Allow viewers to use any chat room?", ref ToolkitSettings.AllowBothChatRooms);

            optionsListing.Gap();

            optionsListing.AddLabeledTextField("TwitchToolkitChannelID".Translate(), ref ToolkitSettings.ChannelID);
            optionsListing.AddLabeledTextField("TwitchToolkitChatroomID".Translate(), ref ToolkitSettings.ChatroomUUID);

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("Should whispers be responded to in separate chat room?", ref ToolkitSettings.WhispersGoToChatRoom);

            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperAllowed".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsAllowed);
            optionsListing.CheckboxLabeled(Helper.ReplacePlaceholder("TwitchToolkitWhisperOnly".Translate(), first: ToolkitSettings.Username), ref ToolkitSettings.WhisperCmdsOnly);

            if (Toolkit.client != null)
            {
                if (Toolkit.client.client != null && Toolkit.client.client.Connected)
                {
                    optionsListing.Label("<color=#21D80E>" + "TwitchToolkitConnected".Translate() + "</color>");
                    if (optionsListing.CenteredButton("TwitchToolkitDisconnect".Translate())) Toolkit.client.Disconnect();
                    if (optionsListing.CenteredButton("TwitchToolkitReconnect".Translate())) Toolkit.client.Reconnect();
                }

                if (Toolkit.client.client == null || !Toolkit.client.client.Connected)
                {
                    if (optionsListing.CenteredButton("TwitchToolkitConnect".Translate())) Toolkit.client.Connect();
                }
            }
            else
            {
                optionsListing.Label("Need new connection");
                if (optionsListing.CenteredButton("TwitchToolkitNewConnection".Translate())) Toolkit.client = new IRC.ToolkitIRC();
            }

                
        }

        static bool oauthWarningAccepted = false;
    }
}
