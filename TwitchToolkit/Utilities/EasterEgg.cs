using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;

namespace TwitchToolkit.Utilities
{
    public static class EasterEgg
    {
        public static void Execute()
        {
            int duration = 3 * 60000;
            GameCondition_PsychicEmanation maleSoothe = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);
            GameCondition_PsychicEmanation femaleSoothe = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicSoothe, duration);

            maleSoothe.gender = Gender.Male;
            femaleSoothe.gender = Gender.Female;

            Map map = Find.CurrentMap;

            map.gameConditionManager.RegisterCondition(maleSoothe);
            map.gameConditionManager.RegisterCondition(femaleSoothe);

            string text = "A soothing feeling rushes over your colonists. The thought of hodl will help keep your colonists mood up for the next few days.";

            Find.LetterStack.ReceiveLetter("Hodl is here", text, LetterDefOf.PositiveEvent);

            SoundDefOf.OrbitalBeam.PlayOneShotOnCamera(map);
        }
    }
}
