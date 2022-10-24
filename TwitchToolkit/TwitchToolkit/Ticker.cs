using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using TwitchToolkit.Store;
using TwitchToolkit.Votes;
using UnityEngine;
using Verse;

namespace TwitchToolkit;

public class Ticker : Thing
{
	public Timer timer = null;

	public static long LastIRCPong = 0L;

	public static Queue<FiringIncident> FiringIncidents = new Queue<FiringIncident>();

	public static Queue<VoteEvent> VoteEvents = new Queue<VoteEvent>();

	public static Queue<IncidentWorker> Incidents = new Queue<IncidentWorker>();

	public static Queue<IncidentHelper> IncidentHelpers = new Queue<IncidentHelper>();

	public static Queue<IncidentHelperVariables> IncidentHelperVariables = new Queue<IncidentHelperVariables>();

	private static Thread _registerThread;

	private static Game _game;

	private static TwitchToolkit _mod = Toolkit.Mod;

	private static Ticker _instance;

	private int[] _baseTimes = new int[5] { 20, 60, 120, 180, 999999 };

	private int _lastMinute = -1;

	private int _lastCoinReward = -1;

	public static DateTime lastEvent;

	public bool CreatedByController { get; internal set; }

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

	public Ticker()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003a: Unknown result type (might be due to invalid IL or missing erences)
		//IL_003c: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0041: Unknown result type (might be due to invalid IL or missing erences)
		//IL_004d: Expected O, but got Unknown
		base.def = new ThingDef
		{
			tickerType = (TickerType)1,
			isSaveable = false
		};
		_registerThread = new Thread(Register);
		_registerThread.Start();
		lastEvent = DateTime.Now;
		LastIRCPong = DateTime.Now.ToFileTime();
	}

	private void Register()
	{
		while (true)
		{
			try
			{
				if (_game != Current.Game)
				{
					if (_game != null)
					{
						_game.tickManager.DeRegisterAllTickabilityFor((Thing)(object)this);
						_game = null;
					}
					_game = Current.Game;
					if (_game != null)
					{
						_game = Current.Game;
						_game.tickManager.RegisterAllTickabilityFor((Thing)(object)this);
						Toolkit.Mod.RegisterTicker();
					}
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

	public override void Tick()
	{
		//IL_0191: Unknown result type (might be due to invalid IL or missing erences)
		//IL_0198: Expected O, but got Unknown
		try
		{
			if (_game == null || _mod == null)
			{
				return;
			}
			_mod.Tick();
			int minutes = (int)(_game.Info.RealPlayTimeInteracting / 60f);
			double getTime = (double)Time.time / 60.0;
			int time = Convert.ToInt32(Math.Truncate(getTime));
			if (IncidentHelpers.Count > 0)
			{
				while (IncidentHelpers.Count > 0)
				{
					IncidentHelper incidentHelper2 = IncidentHelpers.Dequeue();
					if (!(incidentHelper2 is VotingHelper))
					{
						Purchase_Handler.QueuePlayerMessage(incidentHelper2.Viewer, incidentHelper2.message);
					}
					incidentHelper2.TryExecute();
				}
				Helper.playerMessages = new List<string>();
			}
			if (IncidentHelperVariables.Count > 0)
			{
				while (IncidentHelperVariables.Count > 0)
				{
					IncidentHelperVariables incidentHelper = IncidentHelperVariables.Dequeue();
					Purchase_Handler.QueuePlayerMessage(incidentHelper.Viewer, incidentHelper.message, incidentHelper.storeIncident.variables);
					incidentHelper.TryExecute();
					if (Purchase_Handler.viewerNamesDoingVariableCommands.Contains(incidentHelper.Viewer.username))
					{
						Purchase_Handler.viewerNamesDoingVariableCommands.Remove(incidentHelper.Viewer.username);
					}
				}
				Helper.playerMessages = new List<string>();
			}
			if (Incidents.Count > 0)
			{
				IncidentWorker incident2 = Incidents.Dequeue();
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = (IIncidentTarget)(object)Helper.AnyPlayerMap;
				incident2.TryExecute(incidentParms);
			}
			if (FiringIncidents.Count > 0)
			{
				Helper.Log("Firing " + ((Def)FiringIncidents.First().def).defName);
				FiringIncident incident = FiringIncidents.Dequeue();
				incident.def.Worker.TryExecute(incident.parms);
			}
			VoteHandler.CheckForQueuedVotes();
			if (_lastCoinReward < 0)
			{
				_lastCoinReward = time;
			}
			else if (ToolkitSettings.EarningCoins && time - _lastCoinReward >= ToolkitSettings.CoinInterval && Viewers.jsonallviewers != null)
			{
				_lastCoinReward = time;
				Viewers.AwardViewersCoins();
			}
			if (_lastMinute < 0)
			{
				_lastMinute = time;
			}
			else if (_lastMinute < time)
			{
				_lastMinute = time;
				Toolkit.JobManager.CheckAllJobs();
				Viewers.RefreshViewers();
			}
		}
		catch (Exception ex)
		{
			Helper.Log("Exception: " + ex.Message + ex.StackTrace);
		}
	}
}
