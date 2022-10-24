using System;
using RimWorld;
using Verse;

namespace TwitchToolkit.Storytellers;

public class StorytellerPack : Def
{
	public bool enabled = false;

	public int minDaysBetweenEvents = 1;

	public Type storytellerCompPropertiesType = typeof(StorytellerCompProperties);

	public StorytellerComp storytellerComp = null;

	public StorytellerCompProperties storytellerCompProps = null;

	public StorytellerComp StorytellerComp
	{
		get
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing erences)
			//IL_0026: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing erences)
			//IL_005c: Expected O, but got Unknown
			try
			{
				if (storytellerComp == null)
				{
					storytellerCompProps = (StorytellerCompProperties)Activator.CreateInstance(storytellerCompPropertiesType);
					storytellerCompProps.ResolveReferences(Current.Game.storyteller.def);
					storytellerComp = (StorytellerComp)Activator.CreateInstance(storytellerCompProps.compClass);
					storytellerComp.props = storytellerCompProps;
				}
			}
			catch (Exception e)
			{
				Log.Error(e.Message);
			}
			return storytellerComp;
		}
	}
}
