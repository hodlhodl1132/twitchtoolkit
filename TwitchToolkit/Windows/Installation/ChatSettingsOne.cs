using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows.Installation
{
    public class ChatSettingsOne : WindowContentsDriver
    {
        public override void DoWindowContents(Listing_Standard listing)
        {
            listing.AddLabeledTextField("What is your Twitch Channel?", ref ToolkitSettings.Channel);

            listing.Gap();

            if (ToolkitSettings.Channel != "")
            {
                listing.CheckboxLabeled("Will you be using a separate twitch account for bot responses?", ref UseSeparateAccount, separateTwitchAccountBlurb);
            }

            listing.Gap();

            if (UseSeparateAccount)
            {
                listing.AddLabeledTextField("What is your Twitch bot account's name?", ref ToolkitSettings.Username);
            }

            if (ToolkitSettings.Channel != "")
            {
                listing.GapLine();

                listing.Gap();

                listing.Label("You will need a Oauth token to connect to your channel's chatroom.");

                if (UseSeparateAccount)
                {
                    listing.Label("Since you are using a separate bot account. Go into a web browser, logout of your streaming account, and login to your Twitch bot account.");
                }

                listing.Label("Visit twitchapps.com/tmi to make an Oauth token.");

                listing.Gap();

                if (listing.ButtonTextLabeled("Open TwitchApps Website in Browser", "Open Website"))
                {
                    Application.OpenURL("https://twitchapps.com/tmi/");
                }

                if (!AcceptedOauthWarning && listing.ButtonTextLabeled("I am not publicly showing my oauth on a live stream.", "I Understand"))
                {
                    AcceptedOauthWarning = true;
                }
                
                if (AcceptedOauthWarning)
                {
                    listing.Label("Enter your oauth token, you can leave in the \"oauth:\" part.");
                    listing.AddLabeledTextField("Oauth Token:", ref ToolkitSettings.OAuth);

                    if (listing.ButtonTextLabeled("Hide Oauth Token", "Hide"))
                    {
                        AcceptedOauthWarning = false;
                    }
                }
            }

            if (ToolkitSettings.OAuth != "")
            {
                listing.GapLine();

                listing.CheckboxLabeled("Would you like to auto connect to your channel's chatroom on startup?", ref ToolkitSettings.AutoConnect, "Activating this option will auto connect you to your twitch channel chatroom each time you load a savegame.");

                listing.Gap();

                listing.CheckboxLabeled("Will you be using a separate chatroom for commands?", ref ToolkitSettings.UseSeparateChatRoom, "Using a separate chatroom allows you to connect to a separate chatroom in your channel for performing viewer commands");

                listing.Gap();

                if (ToolkitSettings.UseSeparateChatRoom)
                {
                    listing.Label("Before you can connect, you will need to create a separate chat room in you channel.");

                    if (listing.ButtonTextLabeled("Learn how to make Chatroom", "Open Browser"))
                    {
                        Application.OpenURL("https://cdn-images-1.medium.com/max/1600/1*jLwK45n0SEh7pcQi_Ed4sg.gif");
                    }

                    listing.Gap();

                    listing.Label("You will need to fetch the channel id and chatroom uuid for your chatroom.");

                    if (listing.ButtonTextLabeled("Channel ID and Chatroom UUID", "Open Browser"))
                    {
                        Application.OpenURL("https://twitch.honest.chat/list-chat-rooms/?get_room_ids=true&username=" + ToolkitSettings.Channel);
                    }

                    listing.Gap();

                    listing.AddLabeledTextField("Channel ID:", ref ToolkitSettings.ChannelID);
                    listing.AddLabeledTextField("Chatroom UUID:", ref ToolkitSettings.ChatroomUUID);

                    listing.CheckboxLabeled("Will viewers be able to use store commands in both the separate chatroom and the main channel?", ref ToolkitSettings.AllowBothChatRooms);
                }
            }
        }

        public override void PostInstall()
        {
            if (ToolkitSettings.Username == "")
            {
                ToolkitSettings.Username = ToolkitSettings.Channel;
            }
        }

        bool UseSeparateAccount = false;

        bool AcceptedOauthWarning = false;

        string separateTwitchAccountBlurb = "You are going to be asked for an Oauth code in the next step. Using a separate Twitch account is one step you can take to keep this code safe.";
    }
}
