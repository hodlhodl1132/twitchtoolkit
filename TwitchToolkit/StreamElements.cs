using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwitchToolkit;

namespace TwitchToolkitDev
{
    public class StreamElements
    {
        public string JWTToken;
        public string AccountID;

        public StreamElements(string accountid, string jwttoken)
        {
            this.JWTToken = jwttoken;
            this.AccountID = accountid;
        }

        public void ImportPoints()
        {
            Helper.Log($"https://api.streamelements.com/kappa/v2/points/{AccountID}/alltime?offset=0&page=1");
            //WebRequest_BeginGetResponse.Main($"https://api.streamelements.com/kappa/v2/points/{AccountID}/alltime?offset=0&page=1", "ParseJsonResponse", this);
        }

        public void SavePoints()
        {

        }

        public void ParseJsonResponse(RequestState Request)
        {
            string json = Request.jsonString;
            Helper.Log("Streamlabs Request: " + json);
        }
    }
}
