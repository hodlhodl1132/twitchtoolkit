using ToolkitCore;
using TwitchLib.Client.Interfaces;
using TwitchLib.Client.Models.Interfaces;
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
            Log.Message("Starting toolkit message parsing");
            if (Helper.ModActive) CommandsHandler.CheckCommand(twitchMessage);

            if (VoteHandler.voteActive && int.TryParse(twitchMessage.Message, out int voteId)) VoteHandler.currentVote.RecordVote(Viewers.GetViewer(twitchMessage.Username).id, voteId - 1);
        }
    }
}
