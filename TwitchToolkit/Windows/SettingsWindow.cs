using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Windows
{
    public class SettingsWindow : Window
    {
        public SettingsWindow(Mod mod)
        {
            this.Mod = mod;
            this.doCloseButton = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Mod.DoSettingsWindowContents(inRect);
        }

        public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(900f, 700f);
			}
		}

        public override void PostClose()
        {
            Mod.WriteSettings();
        }

        public Mod Mod { get; set; }
    }
}
