using System;
using System.Collections.Generic;
using TwitchToolkit.Store;

namespace TwitchToolkit;

public class Viewer
{
	public string username;

	public int id;

	public bool mod = false;

	public bool subscriber = false;

	public bool vip = false;

	public DateTime last_seen;

	public int coins { get; set; }

	public int karma { get; set; }

	public bool IsSub => subscriber;

	public bool IsVIP => vip;

	public bool IsBanned => ToolkitSettings.BannedViewers.Contains(username);

	public Viewer(string username)
	{
		this.username = username;
		id = Viewers.All.Count;
		Viewers.All.Add(this);
	}

	public static bool IsModerator(string user)
	{
		if (Viewers.GetViewer(user).mod)
		{
			return true;
		}
		if (ToolkitSettings.ViewerModerators == null)
		{
			return false;
		}
		return ToolkitSettings.ViewerModerators.ContainsKey(user);
	}

	public void SetAsModerator()
	{
		if (ToolkitSettings.ViewerModerators == null)
		{
			ToolkitSettings.ViewerModerators = new Dictionary<string, bool>();
		}
		if (!IsModerator(username))
		{
			ToolkitSettings.ViewerModerators.Add(username, value: true);
		}
	}

	public void RemoveAsModerator()
	{
		if (IsModerator(username))
		{
			ToolkitSettings.ViewerModerators.Remove(username);
		}
	}

	public int GetViewerCoins()
	{
		return coins;
	}

	public int GetViewerKarma()
	{
		return karma;
	}

	public void SetViewerKarma(int karma)
	{
		this.karma = karma;
	}

	public int GiveViewerKarma(int karma)
	{
		this.karma = GetViewerKarma() + karma;
		return GetViewerKarma();
	}

	public int TakeViewerKarma(int karma)
	{
		this.karma = GetViewerKarma() - karma;
		return GetViewerKarma();
	}

	public void CalculateNewKarma(KarmaType karmaType, int price)
	{
		int old = GetViewerKarma();
		int newKarma = Karma.CalculateNewKarma(old, karmaType, price);
		SetViewerKarma(newKarma);
		Store_Logger.LogKarmaChange(username, old, newKarma);
	}

	public void SetViewerCoins(int coins)
	{
		this.coins = coins;
	}

	public void GiveViewerCoins(int coins)
	{
		if (this.coins + coins < 0)
		{
			this.coins = 0;
			SetViewerCoins(0);
		}
		else
		{
			SetViewerCoins(this.coins + coins);
		}
	}

	public void TakeViewerCoins(int coins)
	{
		SetViewerCoins(this.coins - coins);
	}

	public void BanViewer()
	{
		if (!IsBanned)
		{
			ToolkitSettings.BannedViewers.Add(username);
		}
	}

	public void UnBanViewer()
	{
		if (IsBanned)
		{
			ToolkitSettings.BannedViewers.Remove(username);
		}
	}

	public static string GetViewerColorCode(string username)
	{
		if (ToolkitSettings.ViewerColorCodes == null)
		{
			ToolkitSettings.ViewerColorCodes = new Dictionary<string, string>();
		}
		if (!ToolkitSettings.ViewerColorCodes.ContainsKey(username))
		{
			SetViewerColorCode(Helper.GetRandomColorCode(), username);
		}
		return ToolkitSettings.ViewerColorCodes[username];
	}

	public static void SetViewerColorCode(string colorcode, string username)
	{
		ToolkitSettings.ViewerColorCodes[username] = colorcode;
	}
}
