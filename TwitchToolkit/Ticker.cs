using System.Threading;
using System.Collections.Generic;
using System.Collections;
using Verse;
using System;
using RimWorld;
using System.Linq;
using UnityEngine;

namespace TwitchToolkit
{
    public class Ticker : Thing
    {
        TwitchToolkit _mod;
        public static Queue<Event> Events = new Queue<Event>();
        public static Queue<FiringIncident> FiringIncidents = new Queue<FiringIncident>();
        public static Queue<VoteEvent> VoteEvents = new Queue<VoteEvent>();

        public bool CreatedByController { get; internal set; }

        static Thread _registerThread;
        static Game _game;
        static Map _map;

        static Ticker _instance;
        public static Ticker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Ticker();
                }
                return _instance;
            }
        }

        Ticker()
        {
            def = new ThingDef { tickerType = TickerType.Normal, isSaveable = false };
            _registerThread = new Thread(Register);
            _registerThread.Start();
        }

        public static void Initialize(TwitchToolkit mod)
        {
            Instance._mod = mod;
        }

        void Register()
        {
            while (true)
            {
                try
                {
                    if (_game != Current.Game)
                    {
                        if (_game != null)
                        {
                            _game.tickManager.DeRegisterAllTickabilityFor(this);
                            _game = null;
                        }

                        _game = Current.Game;
                        if (_game != null)
                        {
                            _game = Current.Game;
                            _game.tickManager.RegisterAllTickabilityFor(this);
                        }
                    }

                    if (_map != Helper.AnyPlayerMap)
                    {
                        _map = Helper.AnyPlayerMap;
                        _mod.Reset();
                    }
                }
                catch (Exception ex)
                {
                    Helper.Log("Exception: " + ex.Message + "\n" + ex.StackTrace);
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        int[] _baseTimes = { 20, 60, 120, 180, 999999 };
        private int _lastMinute = -1;
        private int _lastCoinReward = -1;

        public override void Tick()
        {
            try
            {
                if (_game == null)
                {
                    return;
                }

                _mod.Tick();

                var minutes = (int)(_game.Info.RealPlayTimeInteracting / 60f);
                double getTime = (double)Time.time / 60f;
                int time = Convert.ToInt32(Math.Truncate(getTime));

                if (FiringIncidents.Count > 0)
                {
                    var incident = FiringIncidents.Dequeue();
                    incident.def.Worker.TryExecute(incident.parms);
                }

                if (_mod._voteActive == false && VoteEvents.Count > 0)
                {
                    Helper.Log("VoteEvent Detected, Queueing");
                    var next = VoteEvents.Dequeue();
                    _mod.StartVote(next);
                }

                if (Events.Count() > 0)
                {
                    var evt = Events.Dequeue();
                    evt.Start();
                }
                if (_lastCoinReward < 0)
                {
                    _lastCoinReward = time;
                }
                else if (Settings.EarningCoins && ((time - _lastCoinReward) >= Settings.CoinInterval) && WebRequest_BeginGetResponse.jsonString != null)
                {
                    _lastCoinReward = time;
                    Viewer.AwardViewersCoins();
                }
                if (_lastMinute < 0)
                {
                    _lastMinute = time;
                }
                else if (_lastMinute < time)
                {
                    _lastMinute = time;
                    WebRequest_BeginGetResponse.Main();
                    _mod.WriteSettings();
                }
                //Helper.Log($"last minute: {_lastMinute}, chatter exists: {WebRequest_BeginGetResponse.jsonString != null}, last coin reward:  {_lastCoinReward}, minute: {minutes}, Time: {time}");
            }
            catch (Exception ex)
            {
                Helper.Log("Exception: " + ex.Message + ex.StackTrace);
            }
        }
    }
}
