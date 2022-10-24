using RimWorld;

namespace TwitchToolkit.IncidentHelpers;

public class PointsWagerTarget
{
	public float points;

	public IIncidentTarget target;

	public PointsWagerTarget(float points, IIncidentTarget target)
	{
		this.points = points;
		this.target = target;
	}
}
