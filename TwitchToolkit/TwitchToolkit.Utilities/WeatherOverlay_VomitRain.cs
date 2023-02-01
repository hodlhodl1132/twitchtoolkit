using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities
{
    [StaticConstructorOnStartup]
    public class WeatherOverlay_VomitRain : SkyOverlay
    {
        private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld");

        public WeatherOverlay_VomitRain()
        {
            WeatherOverlay_VomitRain.RainOverlayWorld.color = new Color(1f / 216f, 0.003921569f, 0.0f);
            this.worldOverlayMat = WeatherOverlay_VomitRain.RainOverlayWorld;
            this.worldOverlayPanSpeed1 = 0.015f;
            this.worldPanDir1 = new Vector2(-0.25f, -1f);
            this.worldPanDir1.Normalize();
            this.worldOverlayPanSpeed2 = 0.022f;
            this.worldPanDir2 = new Vector2(-0.24f, -1f);
            this.worldPanDir2.Normalize();
        }
    }
}
