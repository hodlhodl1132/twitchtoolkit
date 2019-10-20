using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace TwitchToolkit.SocketClient
{
    public class MessageLog
    {
        ConcurrentCircularBuffer<string> socketMessages = new ConcurrentCircularBuffer<string>(10);

        public string[] Output
        {
            get
            {
                return socketMessages.Read();
            }
        }
    }
}
