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
