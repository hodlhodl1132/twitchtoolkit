using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.PawnQueue
{
    [StaticConstructorOnStartup]
    public class Alert_UnnamedColonist : Alert
    {
        public Alert_UnnamedColonist()
        {
            defaultPriority = AlertPriority.High;
            defaultLabel = "Colonists need names";
        }

        public override AlertReport GetReport()
        {
            if (!ToolkitSettings.ViewerNamedColonistQueue) return false;
            Dictionary<string, Pawn> pawnHistory = Current.Game.GetComponent<GameComponentPawns>().pawnHistory;
            IEnumerable<Pawn> freeColonists = Helper.AnyPlayerMap.mapPawns.FreeColonistsSpawned;
            
            if (freeColonists.Count() != pawnHistory.Count)
            {
                IEnumerable<Pawn> newPawns = freeColonists.Where(k => !pawnHistory.Values.Contains(k));
                if (newPawns.Count() > 0)
                {
                    return AlertReport.CulpritsAre(newPawns.Cast<Thing>().ToList());
                }
            }
            return false;
        }

        public override TaggedString GetExplanation()
        {
            return "These colonists need names";
        }

        public override Rect DrawAt(float topY, bool minimized)
        {
			Text.Font = GameFont.Small;
			string label = this.GetLabel();
			float height = Text.CalcHeight(label, 148f);
			Rect rect = new Rect((float)UI.screenWidth - 154f, topY, 154f, height);
			GUI.color = this.BGColor;
			GUI.DrawTexture(rect, AlertBGTex);
			GUI.color = Color.white;
			GUI.BeginGroup(rect);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(new Rect(0f, 0f, 148f, height), label);
			GUI.EndGroup();
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, AlertBGTexHighlight);
			}
			if (Widgets.ButtonInvisible(rect, false))
			{
                Find.WindowStack.TryRemove(typeof(QueueWindow));
                Find.WindowStack.Add(new QueueWindow());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return rect;
        }

        private static readonly Texture2D AlertBGTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

        private static readonly Texture2D AlertBGTexHighlight = TexUI.HighlightTex;

        private static List<GlobalTargetInfo> tmpTargets = new List<GlobalTargetInfo>();
    }
}
