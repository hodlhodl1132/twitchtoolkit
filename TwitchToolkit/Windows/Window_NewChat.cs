//using RimWorld;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using Verse;

//namespace TwitchToolkit.Utilities
//{
//    public class ChatWindowNew
//    {
//        public List<ChatMessage> lastMessages = new List<ChatMessage>();
        
//        public ChatWindowNew()
//        {

//        }

//        public  void DoWindowContents(Rect inRect)
//        {
//            if (lastMessages.NullOrEmpty())
//            {
//                return;
//            }
//            GameFont old = Text.Font;
//            Text.Font = GameFont.Small;

//            inRect.x += 2f;
//            Text.Anchor = TextAnchor.LowerLeft;
//            lastMessages.Reverse();
//            foreach (ChatMessage chtmsg in lastMessages)
//            {
//                float spacing = Text.CalcHeight(chtmsg.GetMessageString(), inRect.width);
//                Widgets.Label(inRect, chtmsg.GetMessageString());
//                inRect.y -= spacing;
                
//            }
//            lastMessages.Reverse();
//            Text.Anchor = TextAnchor.UpperLeft;
//            Text.Font = old;
//        }

//        protected virtual Color BGColor
//        {
//            get
//            {
//                return new Color(23, 23, 23, 0.5f);
//            }
//        }

//        public static void DrawWindowBackground(Rect rect)
//		{
//			GUI.color = new Color(23, 23, 23, 0.1f);
//			GUI.DrawTexture(rect, SolidColorMaterials.NewSolidColorTexture(Color.white));
//			GUI.color = Color.white;
//			GUI.color = Color.white;
//		}

//        public void AddMessage(string message, string username, string colorcode)
//        {
//            lastMessages.Add(new ChatMessage(message, username, colorcode));
//            if (lastMessages.Count() > windowRect.height / 20)
//            {
//                RemoveOldestMessage();
//            }
//        }

//        public void RemoveOldestMessage()
//        {
//            lastMessages = lastMessages.Where(x => x != lastMessages.First()).ToList();
//        }

//        private WindowResizer resizer;
//        private bool resizeLater;
//        private Rect resizeLaterRect;
//    }
//}
