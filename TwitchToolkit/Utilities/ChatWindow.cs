using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.Utilities
{
    public class ChatWindow : Window
    {
        public List<ChatMessage> lastMessages = new List<ChatMessage>();
        
        public ChatWindow()
        {
            draggable = true;
            preventCameraMotion = false;
            resizeable = true;
            drawShadow = false;
            closeOnCancel = false;
            layer = WindowLayer.GameUI;
        }

        public override void DoWindowContents(Rect inRect)
        {
            if (lastMessages.NullOrEmpty())
            {
                return;
            }
            GameFont old = Text.Font;
            Text.Font = GameFont.Small;

            inRect.x += 2f;
            Text.Anchor = TextAnchor.LowerLeft;
            lastMessages.Reverse();
            foreach (ChatMessage chtmsg in lastMessages)
            {
                float spacing = Text.CalcHeight(chtmsg.GetMessageString(), inRect.width);
                Widgets.Label(inRect, chtmsg.GetMessageString());
                inRect.y -= spacing;
                
            }
            lastMessages.Reverse();
            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = old;
        }

        public override void ExtraOnGUI()
        {
            base.ExtraOnGUI();
            //DrawChatWindow();
        }

        public override void WindowOnGUI()
		{
			if (this.resizeable)
			{
				if (this.resizer == null)
				{
					this.resizer = new WindowResizer();
				}
				if (this.resizeLater)
				{
					this.resizeLater = false;
					this.windowRect = this.resizeLaterRect;
				}
			}
			this.windowRect = this.windowRect.Rounded();
			Rect winRect = this.windowRect.AtZero();
			this.windowRect = GUI.Window(this.ID, this.windowRect, delegate(int x)
			{
				UnityGUIBugsFixer.OnGUI();
				Find.WindowStack.currentlyDrawnWindow = this;
				if (this.doWindowBackground)
				{
					//DrawWindowBackground(winRect);
				}
				if (KeyBindingDefOf.Accept.KeyDownEvent)
				{
					Find.WindowStack.Notify_PressedAccept();
				}
				if (UnityEngine.Event.current.type == UnityEngine.EventType.KeyDown && !Find.WindowStack.GetsInput(this))
				{
                    UnityEngine.Event.current.Use();
				}
				if (!this.optionalTitle.NullOrEmpty())
				{
					GUI.Label(new Rect(this.Margin, this.Margin, this.windowRect.width, 25f), this.optionalTitle);
				}
				if (this.resizeable && UnityEngine.Event.current.type != UnityEngine.EventType.Repaint)
				{
					Rect lhs = this.resizer.DoResizeControl(this.windowRect);
					if (lhs != this.windowRect)
					{
						this.resizeLater = true;
						this.resizeLaterRect = lhs;
					}
				}
				Rect rect = winRect;
				if (!this.optionalTitle.NullOrEmpty())
				{
					rect.yMin += this.Margin + 25f;
				}
				GUI.BeginGroup(rect);
				try
				{
					this.DoWindowContents(rect.AtZero());
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception filling window for ",
						this.GetType(),
						": ",
						ex
					}), false);
				}
				GUI.EndGroup();
				if (this.resizeable && UnityEngine.Event.current.type == UnityEngine.EventType.Repaint)
				{
					this.resizer.DoResizeControl(this.windowRect);
				}
				if (this.doCloseButton)
				{
					Text.Font = GameFont.Small;
					Rect rect2 = new Rect(winRect.width / 2f - this.CloseButSize.x / 2f, winRect.height - 55f, this.CloseButSize.x, this.CloseButSize.y);
					if (Widgets.ButtonText(rect2, "CloseButton".Translate(), true, false, true))
					{
						this.Close(true);
					}
				}
				if (this.draggable)
				{
					GUI.DragWindow();
				}
				else if (UnityEngine.Event.current.type == UnityEngine.EventType.MouseDown)
				{
                    UnityEngine.Event.current.Use();
				}
				ScreenFader.OverlayOnGUI(winRect.size);
				Find.WindowStack.currentlyDrawnWindow = null;
			}, string.Empty, Widgets.EmptyStyle);
		}

        protected override void SetInitialSizeAndPosition()
        {
            base.SetInitialSizeAndPosition();
            this.windowRect = new Rect(((float)UI.screenWidth - this.InitialSize.x) / 2f, ((float)UI.screenHeight - this.InitialSize.y) / 2f, 300, 400);
            this.windowRect = this.windowRect.Rounded();
        }

        protected virtual Color BGColor
        {
            get
            {
                return new Color(23, 23, 23, 0.5f);
            }
        }

        public static void DrawWindowBackground(Rect rect)
		{
			GUI.color = new Color(23, 23, 23, 0.1f);
			GUI.DrawTexture(rect, SolidColorMaterials.NewSolidColorTexture(Color.white));
			GUI.color = Color.white;
			//Widgets.DrawBox(rect, 1);
			GUI.color = Color.white;
		}

        public void AddMessage(string message, string username, string colorcode)
        {
            lastMessages.Add(new ChatMessage(message, username, colorcode));
            if (lastMessages.Count() > windowRect.height / 20)
            {
                RemoveOldestMessage();
            }
        }

        public void RemoveOldestMessage()
        {
            lastMessages = lastMessages.Where(x => x != lastMessages.First()).ToList();
        }

        private WindowResizer resizer;
        private bool resizeLater;
        private Rect resizeLaterRect;
    }

    public class ChatMessage
    {
        private string Message;
        private string Colorcode;
        private string Username;

        public ChatMessage(string message, string username, string colorcode)
        {
            Message = message;
            Username = username;
            Colorcode = colorcode;
        }

        public string GetMessageString()
        {
            return FilterForMentions($"<b><color=#{Colorcode}>{Username}</color>: {Message}</b>");
        }

        public string FilterForMentions(string message)
        {
            return message.Replace("@" + Settings.Channel, "<color=#000000>@" + Settings.Channel + "</color>" ?? "");
        }
    }
}
