using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit.Utilities;
using TwitchToolkitDev;

namespace TwitchToolkit
{
    public class StreamLabs
    {
        public static int SetViewerPoints(Viewer viewer)
        {
            string[] args = { "http://127.0.0.1:6779", $"{{\"method\":\"set\",\"username\":\"{viewer.username}\",\"points\":{viewer.coins}}}" };
            return Convert.ToInt32(WebClientHelper.UploadString(args));
        }

        public static int GetViewerPoints(Viewer viewer)
        {
            string[] args = { "http://127.0.0.1:6779", $"{{\"method\":\"get\",\"username\":\"{viewer.username}\"}}" };
            return Convert.ToInt32(WebClientHelper.UploadString(args));
        }

        public static bool ParseJsonResponse(RequestState request)
        {
            Helper.Log(request.jsonString);
            return true;
        }
    }
}
