using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities;

[StaticConstructorOnStartup]
public class WeatherOverlay_VomitRain : SkyOverlay
{
	private static readonly Material RainOverlayWorld = MatLoader.LoadMat("Weather/RainOverlayWorld", -1);

	public WeatherOverlay_VomitRain()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0048: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004d: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0074: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0079: Unknown result type (might be due to invalid IL or missing erences)
		RainOverlayWorld.color =(new Color(0.00462962966f, 0.003921569f, 0f));
		base.worldOverlayMat = RainOverlayWorld;
		base.worldOverlayPanSpeed1 = 0.015f;
		base.worldPanDir1 = new Vector2(-0.25f, -1f);
		((Vector2)( base.worldPanDir1)).Normalize();
		base.worldOverlayPanSpeed2 = 0.022f;
		base.worldPanDir2 = new Vector2(-0.24f, -1f);
		((Vector2)( base.worldPanDir2)).Normalize();
	}
}
