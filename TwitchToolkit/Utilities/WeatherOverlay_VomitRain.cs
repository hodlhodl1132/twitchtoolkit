using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities
{
    [StaticConstructorOnStartup]
    public class WeatherOverlay_VomitRain : SkyOverlay
    {
        public WeatherOverlay_VomitRain()
        {
            RainOverlayWorld.color = new Color(1f / 216f, 1f/ 255f, 0f);
            this.worldOverlayMat = RainOverlayWorld;
            this.worldOverlayPanSpeed1 = 0.015f;
            this.worldPanDir1 = new Vector2(-0.25f, -1f);
            this.worldPanDir1.Normalize();
            this.worldOverlayPanSpeed2 = 0.022f;
            this.worldPanDir2 = new Vector2(-0.24f, -1f);
            this.worldPanDir2.Normalize();
        }

        private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld", -1);
    }
}
