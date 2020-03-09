using rim_twitch;
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
            Helper.Log("RT: " + message.Message);

            if (Helper.ModActive) CommandsHandler.CheckCommand(message);
        }
    }
}
