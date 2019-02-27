using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace TwitchToolkit
{
  public class TwitchToolkit_MainTabWindow : MainTabWindow
  {
    readonly TwitchToolkit _mod = LoadedModManager.GetMod<TwitchToolkit>();

    public override MainTabWindowAnchor Anchor 
    {
      get 
      {
        return MainTabWindowAnchor.Right;;
      }
    } 

    public override Vector2 RequestedTabSize
    {
      get
      {
        return new Vector2(450f, 355f);
      }
    }

    public static int inputValue = 0;
    public static string inputBuffer = "00";

    public override void DoWindowContents(Rect inRect)
    {
      base.DoWindowContents(inRect);

      float padding = 5f;
      float btnWidth = ((inRect.width - (padding * 3)) / 2);
      float btnHeight = 30f;

      var rectDifficulty = new Rect(padding, padding, btnWidth, 20);
      Widgets.TextArea(rectDifficulty, "TwitchStoriesMenuDifficulty".Translate() + ": " + (_mod.Doomsday ? "TwitchStoriesMenuDoomsday".Translate() : _mod.Difficulty.ToString()), true);


      var rectBtn = new Rect(rectDifficulty.width + rectDifficulty.x + padding, rectDifficulty.y, (inRect.width - rectDifficulty.width - (padding * 3)) / 2, btnHeight);
      if (Widgets.ButtonText(rectBtn, "TwitchStoriesMenuButtonDifficultyIncrease".Translate()))
      {
        _mod.SetDifficulty(_mod.Difficulty + 1, true);
      }

      rectBtn.x += rectBtn.width + padding;
      if (Widgets.ButtonText(rectBtn, "TwitchStoriesMenuButtonDifficultyDecrease".Translate()))
      {
        _mod.SetDifficulty(_mod.Difficulty - 1, true);
      }


      var rectMessages = new Rect(padding,  rectBtn.height + padding, inRect.width - (padding * 2), 180f);
      Widgets.TextArea(rectMessages, string.Join("\r\n", _mod.MessageLog), true);

      /*var rectInput = new Rect(padding, inRect.height - btnHeight, 20, btnHeight);
      Widgets.TextFieldNumeric<int>(rectInput, ref inputValue, ref inputBuffer);*/

      rectBtn = new Rect(padding, rectMessages.y + rectMessages.height, btnWidth, btnHeight);
      if (Widgets.ButtonText(rectBtn, "TwitchStoriesMenuButtonDoomsday".Translate()))
      {
        _mod.Doomsday = true;
      }

      rectBtn.y += btnHeight + padding;
      if (Widgets.ButtonText(rectBtn, "TwitchStoriesMenuButtonFun".Translate()))
      {
        Events.Start(5, 5);
      }

      rectBtn.y += btnHeight + padding;
      if (Widgets.ButtonText(rectBtn, "TwitchStoriesMenuButtonVote".Translate()))
      {
        //_mod.StartVote();
      }

      rectBtn.x += btnWidth + padding;
      rectBtn.y -= (btnHeight + padding) * 2;
      if (Widgets.ButtonText(rectBtn, "TwitchStoriesReconnect".Translate()))
      {
        _mod.Reconnect();
      }

      rectBtn.y += btnHeight + padding;
      if (Widgets.ButtonText(rectBtn, "TwitchStoriesDisconnect".Translate()))
      {
        _mod.Disconnect();
      }

      rectBtn.y += btnHeight + padding;
      if (Widgets.ButtonText(rectBtn, ""))
      {
        //Helper.Mushroom(null);
      }
    }
  }
}
