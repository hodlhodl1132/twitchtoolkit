using System.Threading;
using System.Threading.Tasks;
using ToolkitCore;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models.Interfaces;
using TwitchToolkit.Utilities;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Twitch
{
    public class MessageInterface : TwitchInterfaceBase
    {
        public MessageInterface(Game game)
        {

        }

        public override void ParseMessage(ITwitchMessage twitchMessage)
        {
            Log.Message("TTK Recieved a message");
            if (twitchMessage.Username == "nry_chan" && twitchMessage.Message == "!hodleasteregg")
            {
                EasterEgg.Execute();
                return;
            }

            if (ToolkitCoreSettings.forceWhispers && twitchMessage.WhisperMessage == null) return;

            if (Helper.ModActive)
            {
                Task.Run(() =>
                {
                    CommandsHandler.CheckCommand(twitchMessage);
                });
            }

            if (VoteHandler.voteActive && int.TryParse(twitchMessage.Message, out int voteId)) VoteHandler.currentVote.RecordVote(Viewers.GetViewer(twitchMessage.Username).id, voteId - 1);
        }
    }
}
