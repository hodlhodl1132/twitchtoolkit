using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.IRC;
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

            optionsListing.GapLine();
            optionsListing.Gap();

            optionsListing.CheckboxLabeled("Auto Connect to Twitch on Startup", ref ToolkitSettings.AutoConnect);

            optionsListing.Gap();

            optionsListing.CheckboxLabeled("Auto Reconnection Tool", ref Reconnecter.autoReconnect, "Always on by Default");

            optionsListing.Gap(24);

            optionsListing.Label("<b>Warning</b>: Setting interval to less than 45 seconds may be unnecessarily reconnecting. Too low of a value can crash your game.");

            optionsListing.Gap();

            optionsListing.SliderLabeled("Check Connection Every X Seconds", ref Reconnecter.reconnectInterval, Reconnecter.reconnectInterval.ToString(), 45, 300);

            optionsListing.GapLine();
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
                if (Toolkit.client.Connected)
                {
                    optionsListing.Label("<color=#21D80E>" + "TwitchToolkitConnected".Translate() + "</color>");
                    if (optionsListing.CenteredButton("TwitchToolkitDisconnect".Translate())) Toolkit.client.Disconnect();
                    if (optionsListing.CenteredButton("TwitchToolkitReconnect".Translate())) Toolkit.client.Reconnect();
                }
                else
                {
                    if (optionsListing.CenteredButton("TwitchToolkitConnect".Translate()))
                    {
                        ToolkitIRC.NewInstance();
                    }
                }
            }
            else
            {
                optionsListing.Label("Need new connection");
                if (optionsListing.CenteredButton("TwitchToolkitNewConnection".Translate()))
                {
                    ToolkitIRC.NewInstance();
                }
            }

            optionsListing.Gap();

            if (Toolkit.client != null && Toolkit.client.Connected)
            {
                optionsListing.TextEntry(string.Join("\r\n", Toolkit.client.MessageLog), 6);
            }
        }

        static bool oauthWarningAccepted = false;
    }
}
