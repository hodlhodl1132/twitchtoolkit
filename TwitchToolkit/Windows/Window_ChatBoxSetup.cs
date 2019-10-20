using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities
{
    public class Window_ChatBoxSetup : Window
    {
        public Window_ChatBoxSetup()
        {
            this.doCloseButton = true;
            this.draggable = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.CheckboxLabeled("Show Chat on Screen", ref ToolkitSettings.ChatBoxEnabled);
    
            int xValue = (int)ToolkitSettings.ChatBoxPositionX;
            string xBuffer = xValue.ToString();
            listing.IntEntry(ref xValue, ref xBuffer, 1);
            ToolkitSettings.ChatBoxPositionX = (float)xValue;

            int yValue = (int)ToolkitSettings.ChatBoxPositionY;
            string yBuffer = yValue.ToString();
            listing.IntEntry(ref yValue, ref yBuffer, 1);
            ToolkitSettings.ChatBoxPositionY = (float)yValue;

            string mCountBuffer = ToolkitSettings.ChatBoxMessageCount.ToString();
            listing.TextFieldNumericLabeled("Max Chat Messages", ref ToolkitSettings.ChatBoxMessageCount, ref mCountBuffer, 1, 30);

            string mWidthBuffer = ToolkitSettings.ChatBoxMaxWidth.ToString();
            listing.TextFieldNumericLabeled("Max Width", ref ToolkitSettings.ChatBoxMaxWidth, ref mWidthBuffer, 100, UI.screenWidth - 100);

            listing.End();
        }

        public override Vector2 InitialSize => new Vector2(500f, 300f);
    }
}
