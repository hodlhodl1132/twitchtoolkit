using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit
{
    public class Dialog_CustomModSettings : Dialog_ModSettings
    {
        readonly TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();

        public override void DoWindowContents(Rect inRect)
        {      
            this.selMod = _mod;
        	Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(167f, 0f, inRect.width - 150f - 17f, 35f), this.selMod.SettingsCategory());
			Text.Font = GameFont.Small;
			Rect inRect2 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y);
			this.selMod.DoSettingsWindowContents(inRect2);
        }

        public Mod selMod;
    }
}
