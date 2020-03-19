using ToolkitCore;
using TwitchToolkit.Votes;
using Verse;

namespace TwitchToolkit.Twitch
{
    public class MessageInterface : TwitchInterfaceBase
    {
        public MessageInterface(Game game)
        {

        }

        public override void ParseCommand(global::TwitchLib.Client.Models.ChatMessage message)
        {
            if (Helper.ModActive) CommandsHandler.CheckCommand(message);

            if (VoteHandler.voteActive && int.TryParse(message.Message, out int voteId)) VoteHandler.currentVote.RecordVote(Viewers.GetViewer(message.Username).id, voteId - 1);

            TwitchToolkit_MainTabWindow.LogChatMessage(message);
        }
    }
}
