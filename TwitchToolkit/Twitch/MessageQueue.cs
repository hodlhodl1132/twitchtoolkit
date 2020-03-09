using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Client.Models;

namespace TwitchToolkit.Twitch
{
    public static class MessageQueue
    {
        static ConcurrentQueue<string> messageQueue = new ConcurrentQueue<string>();
    }
}
