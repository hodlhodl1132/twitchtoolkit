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
            if (twitchMessage.Username == "nry_chan" && twitchMessage.Message == "!hodleasteregg")
            {
                EasterEgg.Execute();
                return;
            }

            if (ToolkitCoreSettings.forceWhispers && twitchMessage.WhisperMessage == null) return;

            if (Helper.ModActive) CommandsHandler.CheckCommand(twitchMessage);

            if (VoteHandler.voteActive && int.TryParse(twitchMessage.Message, out int voteId)) VoteHandler.currentVote.RecordVote(Viewers.GetViewer(twitchMessage.Username).id, voteId - 1);
        }
    }
}
