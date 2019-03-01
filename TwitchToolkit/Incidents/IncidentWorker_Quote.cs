using RimWorld;
using Verse;

namespace TwitchToolkit.Incidents
{
    public class IncidentWorker_Quote : IncidentWorker
    {
        readonly string Quote;

        public IncidentWorker_Quote(string quote)
        {
            Quote = quote;
        }

        protected new void SendStandardLetter()
        {
            if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
            {
                Log.Error("Sending standard incident letter with no label or text.", false);
            }

            var text = this.def.letterText;
            if (Quote != null)
            {
                text += "\n\n";
                text += Quote;
            }

            Find.LetterStack.ReceiveLetter(this.def.letterLabel, text, this.def.letterDef, null);
        }

        protected new void SendStandardLetter(LookTargets lookTargets, Faction relatedFaction = null, params string[] textArgs)
        {
            if (this.def.letterLabel.NullOrEmpty() || this.def.letterText.NullOrEmpty())
            {
                Log.Error("Sending standard incident letter with no label or text.", false);
            }

            var text = string.Format(this.def.letterText, textArgs).CapitalizeFirst();
            if (Quote != null)
            {
                text += "\n\n";
                text += Quote;
            }

            Find.LetterStack.ReceiveLetter(this.def.letterLabel, text, this.def.letterDef, lookTargets, relatedFaction, null);
        }
    }
}
