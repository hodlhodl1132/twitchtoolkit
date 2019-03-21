using Verse;

namespace TwitchToolkit.PawnQueue
{
    public class CompProperties_PawnNamed : CompProperties
    {
        public CompProperties_PawnNamed()
        {
            this.compClass = typeof(CompPawnNamed);
        }

        public bool isNamed;
    }
}