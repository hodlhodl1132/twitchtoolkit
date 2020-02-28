using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TwitchToolkit.IRC;
using UnityEngine;
using Verse;
using System.Reflection;
using System.Text.RegularExpressions;
using TwitchToolkit.Utilities;
using TwitchToolkit.Store;
using MoonSharp.Interpreter;

namespace TwitchToolkit
{
    public class TwitchToolkit : Mod
    {
        public TwitchToolkit(ModContentPack content) : base(content)
        {
            Toolkit.Mod = this;
            if (ticker == null) RegisterTicker();
        }

        public override string SettingsCategory() => "Twitch Toolkit";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<ToolkitSettings>().DoWindowContents(inRect);
        }

        public void Tick()
        {
            DateTime tick = DateTime.Now;

            if (_lastTick == DateTime.MinValue)
            {

            }
            else if (_paused == true)
            {
                var wait = (_timerElapsed - _lastTick).TotalMilliseconds;
                _extraWait = Math.Min(Math.Max(wait, 0), 60 * 1000);
                _paused = false;
                RegisterTicker();
            }
            else if ((tick - _lastTick).TotalSeconds > 5)
            {
                _extraWait += (tick - _lastTick).TotalMilliseconds;
            }

            _lastTick = tick;
        }

        public void RegisterTicker() => ticker = Ticker.Instance;

        public string Version = "2.0.10";

        Ticker ticker;
        public DateTime StartTime;
        DateTime _lastTick = DateTime.MinValue;
        DateTime _timerElapsed = DateTime.MinValue;
        DateTime _lastEventCheck = DateTime.MinValue;
        bool _paused;
        double _extraWait = 0;
    }

    public static class Toolkit
    {
        public static TwitchToolkit Mod = null;
        public static Scheduled JobManager = new Scheduled();
        public static ToolkitIRC client = null;
    }
}
