using System;
using System.Collections.Generic;
using TwitchToolkit.Store;

namespace TwitchToolkit
{
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

    public Viewer(string username)
    {
      this.username = username;
      this.id = Viewers.All.Count;
      Viewers.All.Add(this);
    }

    public bool IsSub => this.subscriber;

    public bool IsVIP => this.vip;

    public static bool IsModerator(string user)
    {
      if (Viewers.GetViewer(user).mod)
        return true;
      return ToolkitSettings.ViewerModerators != null && ToolkitSettings.ViewerModerators.ContainsKey(user);
    }

    public void SetAsModerator()
    {
      if (ToolkitSettings.ViewerModerators == null)
        ToolkitSettings.ViewerModerators = new Dictionary<string, bool>();
      if (Viewer.IsModerator(this.username))
        return;
      ToolkitSettings.ViewerModerators.Add(this.username, true);
    }

    public void RemoveAsModerator()
    {
      if (!Viewer.IsModerator(this.username))
        return;
      ToolkitSettings.ViewerModerators.Remove(this.username);
    }

    public int GetViewerCoins() => this.coins;

    public int GetViewerKarma() => this.karma;

    public void SetViewerKarma(int karma) => this.karma = karma;

    public int GiveViewerKarma(int karma)
    {
      this.karma = this.GetViewerKarma() + karma;
      return this.GetViewerKarma();
    }

    public int TakeViewerKarma(int karma)
    {
      this.karma = this.GetViewerKarma() - karma;
      return this.GetViewerKarma();
    }

    public void CalculateNewKarma(KarmaType karmaType, int price)
    {
      int viewerKarma = this.GetViewerKarma();
      int newKarma = Karma.CalculateNewKarma(viewerKarma, karmaType, price);
      this.SetViewerKarma(newKarma);
      Store_Logger.LogKarmaChange(this.username, viewerKarma, newKarma);
    }

    public void SetViewerCoins(int coins) => this.coins = coins;

    public void GiveViewerCoins(int coins)
    {
      if (this.coins + coins < 0)
      {
        this.coins = 0;
        this.SetViewerCoins(0);
      }
      else
        this.SetViewerCoins(this.coins + coins);
    }

    public void TakeViewerCoins(int coins) => this.SetViewerCoins(this.coins - coins);

    public bool IsBanned => ToolkitSettings.BannedViewers.Contains(this.username);

    public void BanViewer()
    {
      if (this.IsBanned)
        return;
      ToolkitSettings.BannedViewers.Add(this.username);
    }

    public void UnBanViewer()
    {
      if (!this.IsBanned)
        return;
      ToolkitSettings.BannedViewers.Remove(this.username);
    }

    public static string GetViewerColorCode(string username)
    {
      if (ToolkitSettings.ViewerColorCodes == null)
        ToolkitSettings.ViewerColorCodes = new Dictionary<string, string>();
      if (!ToolkitSettings.ViewerColorCodes.ContainsKey(username))
        Viewer.SetViewerColorCode(Helper.GetRandomColorCode(), username);
      return ToolkitSettings.ViewerColorCodes[username];
    }

    public static void SetViewerColorCode(string colorcode, string username) => ToolkitSettings.ViewerColorCodes[username] = colorcode;
  }
}
