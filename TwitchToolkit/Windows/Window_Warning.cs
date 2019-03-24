using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities
{
    public class WarningWindow : Window
    {
        public string warning = "";

        public WarningWindow()
        {
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            GameFont old = Text.Font;
            Text.Font = GameFont.Medium;
            Widgets.Label(inRect, "<b><color=#FC3636>" + "WARNING:" + "</color></b>");
            Text.Font = GameFont.Small;
            inRect.y += 30;
            Widgets.Label(inRect, warning);
            Text.Font = old;
        }

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(500f, 300f);
            }
        }
    }
}
