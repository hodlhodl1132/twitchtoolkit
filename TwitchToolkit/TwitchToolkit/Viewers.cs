using System;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using ToolkitCore;
using TwitchToolkit.Store;
using TwitchToolkit.Utilities;
using TwitchToolkitDev;
using Verse;

namespace TwitchToolkit;

public static class Viewers
{
	public static string jsonallviewers;

	public static List<Viewer> All = new List<Viewer>();

	public static void AwardViewersCoins(int setamount = 0)
	{
		List<string> usernames = ParseViewersFromJsonAndFindActiveViewers();
		if (usernames == null)
		{
			return;
		}
		foreach (string username in usernames)
		{
			Viewer viewer = GetViewer(username);
			if (viewer.IsBanned)
			{
				continue;
			}
			if (setamount > 0)
			{
				viewer.GiveViewerCoins(setamount);
				continue;
			}
			int baseCoins = ToolkitSettings.CoinAmount;
			float baseMultiplier = (float)viewer.GetViewerKarma() / 100f;
			if (viewer.IsSub)
			{
				baseCoins += ToolkitSettings.SubscriberExtraCoins;
				baseMultiplier *= ToolkitSettings.SubscriberCoinMultiplier;
			}
			else if (viewer.IsVIP)
			{
				baseCoins += ToolkitSettings.VIPExtraCoins;
				baseMultiplier *= ToolkitSettings.VIPCoinMultiplier;
			}
			else if (viewer.mod)
			{
				baseCoins += ToolkitSettings.ModExtraCoins;
				baseMultiplier *= ToolkitSettings.ModCoinMultiplier;
			}
			int minutesSinceViewerWasActive = TimeHelper.MinutesElapsed(viewer.last_seen);
			if (ToolkitSettings.ChatReqsForCoins)
			{
				if (minutesSinceViewerWasActive > ToolkitSettings.TimeBeforeHalfCoins)
				{
					baseMultiplier *= 0.5f;
				}
				if (minutesSinceViewerWasActive > ToolkitSettings.TimeBeforeNoCoins)
				{
					baseMultiplier *= 0f;
				}
			}
			double coinsToReward = (double)baseCoins * (double)baseMultiplier;
			Store_Logger.LogString($"{viewer.username} gets {baseCoins} * {baseMultiplier} coins, total {(int)Math.Ceiling(coinsToReward)}");
			viewer.GiveViewerCoins((int)Math.Ceiling(coinsToReward));
		}
	}

	public static void GiveAllViewersCoins(int amount, List<Viewer> viewers = null)
	{
		if (viewers != null)
		{
			foreach (Viewer viewer2 in viewers)
			{
				viewer2.GiveViewerCoins(amount);
			}
			return;
		}
		List<string> usernames = ParseViewersFromJsonAndFindActiveViewers();
		if (usernames == null)
		{
			return;
		}
		foreach (string username in usernames)
		{
			Viewer viewer = GetViewer(username);
			if (viewer != null && viewer.GetViewerKarma() > 1)
			{
				viewer.GiveViewerCoins(amount);
			}
		}
	}

	public static void SetAllViewersCoins(int amount, List<Viewer> viewers = null)
	{
		if (viewers != null)
		{
			foreach (Viewer viewer in viewers)
			{
				viewer.SetViewerCoins(amount);
			}
			return;
		}
		if (All == null)
		{
			return;
		}
		foreach (Viewer item in All)
		{
			item?.SetViewerCoins(amount);
		}
	}

	public static void GiveAllViewersKarma(int amount, List<Viewer> viewers = null)
	{
		if (viewers != null)
		{
			foreach (Viewer viewer2 in viewers)
			{
				viewer2.SetViewerKarma(Math.Min(ToolkitSettings.KarmaCap, viewer2.GetViewerKarma() + amount));
			}
			return;
		}
		List<string> usernames = ParseViewersFromJsonAndFindActiveViewers();
		if (usernames == null)
		{
			return;
		}
		foreach (string username in usernames)
		{
			Viewer viewer = GetViewer(username);
			if (viewer != null && viewer.GetViewerKarma() > 1)
			{
				viewer.SetViewerKarma(Math.Min(ToolkitSettings.KarmaCap, viewer.GetViewerKarma() + amount));
			}
		}
	}

	public static void TakeAllViewersKarma(int amount, List<Viewer> viewers = null)
	{
		if (viewers != null)
		{
			foreach (Viewer viewer2 in viewers)
			{
				viewer2.SetViewerKarma(Math.Max(0, viewer2.GetViewerKarma() - amount));
			}
			return;
		}
		if (All == null)
		{
			return;
		}
		foreach (Viewer viewer in All)
		{
			viewer?.SetViewerKarma(Math.Max(0, viewer.GetViewerKarma() - amount));
		}
	}

	public static void SetAllViewersKarma(int amount, List<Viewer> viewers = null)
	{
		if (viewers != null)
		{
			foreach (Viewer viewer in viewers)
			{
				viewer.SetViewerKarma(amount);
			}
			return;
		}
		if (All == null)
		{
			return;
		}
		foreach (Viewer item in All)
		{
			item?.SetViewerKarma(amount);
		}
	}

	public static List<string> ParseViewersFromJsonAndFindActiveViewers()
	{
		List<string> usernames = new List<string>();
		string json = jsonallviewers;
		if (GenText.NullOrEmpty(json))
		{
			return null;
		}
		JSONNode parsed = JSON.Parse(json);
		List<JSONArray> groups = new List<JSONArray>();
		groups.Add(parsed["chatters"]["moderators"].AsArray);
		groups.Add(parsed["chatters"]["staff"].AsArray);
		groups.Add(parsed["chatters"]["admins"].AsArray);
		groups.Add(parsed["chatters"]["global_mods"].AsArray);
		groups.Add(parsed["chatters"]["viewers"].AsArray);
		groups.Add(parsed["chatters"]["vips"].AsArray);
		foreach (JSONArray group in groups)
		{
			JSONNode.Enumerator enumerator2 = group.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				JSONNode username = enumerator2.Current;
				string usernameconvert = username.ToString();
				usernameconvert = usernameconvert.Remove(0, 1);
				usernameconvert = usernameconvert.Remove(usernameconvert.Length - 1, 1);
				usernames.Add(usernameconvert);
			}
		}
		foreach (Viewer viewer in All.Where(delegate(Viewer s)
		{
			_ = s.last_seen;
			return TimeHelper.MinutesElapsed(s.last_seen) <= ToolkitSettings.TimeBeforeHalfCoins;
		}))
		{
			if (!usernames.Contains(viewer.username))
			{
				Helper.Log("Viewer " + viewer.username + " added to active viewers through chat participation but not in chatter list.");
				usernames.Add(viewer.username);
			}
		}
		return usernames;
	}

	public static bool SaveUsernamesFromJsonResponse(RequestState request)
	{
		Helper.Log("Saving Usernames From Json Response");
		jsonallviewers = request.jsonString;
		return true;
	}

	public static void ResetViewers()
	{
		All = new List<Viewer>();
	}

	public static Viewer GetViewer(string user)
	{
		Viewer viewer = All.Find((Viewer x) => x.username == user.ToLower());
		if (viewer == null)
		{
			viewer = new Viewer(user);
			viewer.SetViewerCoins(ToolkitSettings.StartingBalance);
			viewer.karma = ToolkitSettings.StartingKarma;
		}
		return viewer;
	}

	public static Viewer GetViewerById(int id)
	{
		return All.Find((Viewer s) => s.id == id);
	}

	public static void RefreshViewers()
	{
		Helper.Log("reshing Viewers");
		WebRequest_BeginGetResponse.Main("https://tmi.twitch.tv/group/user/" + ToolkitCoreSettings.channel_username.ToLower() + "/chatters", SaveUsernamesFromJsonResponse);
	}

	public static void ResetViewersCoins()
	{
		foreach (Viewer viewer in All)
		{
			viewer.coins = ToolkitSettings.StartingBalance;
		}
	}

	public static void ResetViewersKarma()
	{
		foreach (Viewer viewer in All)
		{
			viewer.karma = ToolkitSettings.StartingKarma;
		}
	}
}
