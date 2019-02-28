using System;
using System.Linq;
using Verse;
using RimWorld;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_OrbitalTraderArrival : IncidentWorker
    {
        private const int MaxShips = 5;

        readonly string Quote;

        public IncidentWorker_OrbitalTraderArrival(string quote)
        {
            Quote = quote;
        }

        protected override bool CanFireNowSub(IncidentParms parms)
        {
            if (!base.CanFireNowSub(parms))
            {
                return false;
            }
            Map map = (Map)parms.target;
            return map.passingShipManager.passingShips.Count < 5;
        }

        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            if (map.passingShipManager.passingShips.Count >= 5)
            {
                return false;
            }
            TraderKindDef def;
            if ((from x in DefDatabase<TraderKindDef>.AllDefs
                 where x.orbital
                 select x).TryRandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality, out def))
            {
                TradeShip tradeShip = new TradeShip(def);
                if (map.listerBuildings.allBuildingsColonist.Any((Building b) => b.def.IsCommsConsole && b.GetComp<CompPowerTrader>().PowerOn))
                {
                    var text = "TraderArrival".Translate(tradeShip.name, tradeShip.def.label);

                    if (Quote != null)
                    {
                        text += "\n\n";
                        text += Quote;
                    }

                    Find.LetterStack.ReceiveLetter(tradeShip.def.LabelCap, text, LetterDefOf.PositiveEvent, null);
                }
                map.passingShipManager.AddShip(tradeShip);
                tradeShip.GenerateThings();
                return true;
            }
            throw new InvalidOperationException();
        }
    }
}