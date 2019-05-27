using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Verse;

namespace TwitchToolkit.IRC
{
    //public static class SocketManager
    //{
    //    static SocketManager()
    //    {
    //        try
    //        {
    //            using (var nf = new Notifier())
    //            using (var ws = new WebSocket("ws://localhost:8081/socket.io/?EIO=3&transport=websocket"))
    //            {
    //                ws.OnMessage += (sender, e) =>
    //                    Log.Warning(e.Data);

    //                ws.OnMessage += (sender, e) =>
    //                    nf.Notify(
    //                      new NotificationMessage
    //                      {
    //                          Summary = "WebSocket Message",
    //                          Body = !e.IsPing ? e.Data : "Received a ping.",
    //                          Icon = "notification-message-im"
    //                      }
    //                    );

    //                ws.OnClose += (sender, e) =>
    //                    Log.Warning("socket closed");

    //                ws.Connect();

    //                if (ws.IsAlive) Log.Warning("socket connected");

    //                Console.ReadKey(true);
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Error(e.Message);
    //        }
    //    }
    //}

}
