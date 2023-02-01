using System;
using UnityEngine;
using Verse;

namespace TwitchToolkit
{
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
			if (this.ticker != null)
				return;
			this.RegisterTicker();
		}

		public override string SettingsCategory() => "Twitch Toolkit";

		public override void DoSettingsWindowContents(Rect inRect) => this.GetSettings<ToolkitSettings>().DoWindowContents(inRect);

		public void Tick()
		{
			DateTime now = DateTime.Now;
			if (!(this._lastTick == DateTime.MinValue))
			{
				if (this._paused)
				{
					this._extraWait = Math.Min(Math.Max((this._timerElapsed - this._lastTick).TotalMilliseconds, 0.0), 60000.0);
					this._paused = false;
					this.RegisterTicker();
				}
				else if ((now - this._lastTick).TotalSeconds > 5.0)
					this._extraWait += (now - this._lastTick).TotalMilliseconds;
			}
			this._lastTick = now;
		}

		public void RegisterTicker() => this.ticker = Ticker.Instance;
	}
}
