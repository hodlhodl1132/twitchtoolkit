using RimWorld;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.VotingHelpers.VotingHelpers_Weather;

public abstract class Weather : VotingHelper
{
	public string weatherDefName;

	private WeatherDef weather;

	public override bool IsPossible()
	{
		weather = DefDatabase<WeatherDef>.GetNamed(weatherDefName, true);
		IIncidentTarget obj = target;
		Map map = (Map)(object)((obj is Map) ? obj : null);
		if (map != null && map.weatherManager.curWeather != weather)
		{
			return true;
		}
		return false;
	}

	public override void TryExecute()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0011: Unknown result type (might be due to invalid IL or missing erences)
		Helper.Weather((TaggedString)("Chat has voted to change the weather to: " + ((Def)weather).LabelCap), weather, LetterDefOf.PositiveEvent);
	}
}
