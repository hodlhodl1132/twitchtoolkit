using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit;

public class TwitchToolkit : Mod
{
	public string Version = "2.0.10";

	private Ticker ticker;

	public DateTime StartTime;

	private DateTime _lastTick = DateTime.MinValue;

	private DateTime _timerElapsed = DateTime.MinValue;

	private DateTime _lastEventCheck = DateTime.MinValue;

	private bool _paused;

	private double _extraWait = 0.0;

	public TwitchToolkit(ModContentPack content)
		: base(content)
	{
		Toolkit.Mod = this;
		if (ticker == null)
		{
			RegisterTicker();
		}
	}

	public override string SettingsCategory()
	{
		return "Twitch Toolkit";
	}

	public override void DoSettingsWindowContents(Rect inRect)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing erences)
		((Mod)this).GetSettings<ToolkitSettings>().DoWindowContents(inRect);
	}

	public void Tick()
	{
		DateTime tick = DateTime.Now;
		if (!(_lastTick == DateTime.MinValue))
		{
			if (_paused)
			{
				double wait = (_timerElapsed - _lastTick).TotalMilliseconds;
				_extraWait = Math.Min(Math.Max(wait, 0.0), 60000.0);
				_paused = false;
				RegisterTicker();
			}
			else if ((tick - _lastTick).TotalSeconds > 5.0)
			{
				_extraWait += (tick - _lastTick).TotalMilliseconds;
			}
		}
		_lastTick = tick;
	}

	public void RegisterTicker()
	{
		ticker = Ticker.Instance;
	}
}
