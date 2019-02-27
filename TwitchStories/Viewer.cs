using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Runtime.Serialization.Json;
using System.Collections;
using UnityEngine;
using SimpleJSON;
using Verse;
using System.Linq;

namespace TwitchStories
{
    public class Viewer
    {
        public string username;
        public int id;

        public Viewer(string username, int id)
        {
            this.username = username;
            this.id = id;
        }

        public static Viewer GetViewer(string user)
        {
            Viewer viewer = Settings.listOfViewers.Find(x => x.username == user);
            if (viewer == null)
            { 
                Helper.Log("Creating new user");
                viewer = new Viewer(user, Settings.ViewerIds.Count());
                Settings.ViewerIds.Add(viewer.username, viewer.id);
                Settings.ViewerCoins.Add(viewer.id, 150);
                Settings.ViewerKarma.Add(viewer.id, 100);
                Settings.listOfViewers.Add(viewer);
            }
            Helper.Log(viewer.username);
            return Settings.listOfViewers.Find(x => x.username == user);
        }

        public static void AwardViewersCoins()
        {
            List<string> usernames = ParseViewersFromJson();
            foreach(string username in usernames)
            {
                Viewer viewer = Viewer.GetViewer(username);
                double karmabonus = ((double) viewer.GetViewerKarma() / 100d) * (double) Settings.CoinAmount;
                Helper.Log($"Karma bonus for {username} is {karmabonus}");
                viewer.GiveViewerCoins( Convert.ToInt32(karmabonus) );
            }
        }

        public static List<string> ParseViewersFromJson()
        {
            List<string> usernames = new List<string>();

            string json = WebRequest_BeginGetResponse.jsonString;
            var parsed = JSON.Parse(json);
            List<JSONArray> groups = new List<JSONArray>();
            groups.Add(parsed["chatters"]["moderators"].AsArray);
            groups.Add(parsed["chatters"]["staff"].AsArray);
            groups.Add(parsed["chatters"]["admins"].AsArray);
            groups.Add(parsed["chatters"]["global_mods"].AsArray);
            groups.Add(parsed["chatters"]["viewers"].AsArray);
            groups.Add(parsed["chatters"]["vips"].AsArray);
            foreach(JSONArray group in groups)
            {             
                foreach(JSONNode username in group)
                {
                    string usernameconvert = username.ToString();
                    usernameconvert = usernameconvert.Remove(0, 1);
                    usernameconvert = usernameconvert.Remove(usernameconvert.Length - 1, 1);
                    Helper.Log(usernameconvert);
                    usernames.Add(usernameconvert);
                }
            }
            return usernames;
        }

        public int GetViewerCoins()
        {
            return Settings.ViewerCoins[this.id];
        }

        public int GetViewerKarma()
        {
            return Settings.ViewerKarma[this.id];
        }

        public void SetViewerKarma(int karma)
        {
            Settings.ViewerKarma[this.id] = karma;
        }
        
        public int GiveViewerKarma(int karma)
        {
            Settings.ViewerKarma[this.id] = this.GetViewerKarma() + karma;
            return this.GetViewerKarma();
        }
        
        public int TakeViewerKarma(int karma)
        {
            Settings.ViewerKarma[this.id] = this.GetViewerKarma() - karma;
            return this.GetViewerKarma();
        }
        public void SetViewerCoins(int coins)
        {
            Settings.ViewerCoins[this.id] = coins;
        }
        
        public int GiveViewerCoins(int coins)
        {
            Settings.ViewerCoins[this.id] = this.GetViewerCoins() + coins;
            return this.GetViewerCoins();
        }
        
        public int TakeViewerCoins(int coins)
        {
            Settings.ViewerCoins[this.id] = this.GetViewerCoins() - coins;
            return this.GetViewerCoins();
        }
    }

    public class RequestState
    {
      // This class stores the state of the request.
      const int BUFFER_SIZE = 1024;
      public StringBuilder requestData;
      public byte[] bufferRead;
      public WebRequest request;
      public WebResponse response;
      public Stream responseStream;
      public RequestState()
      {
        bufferRead = new byte[BUFFER_SIZE];
        requestData = new StringBuilder("");
        request = null;
        responseStream = null;
      }
    }
    public class WebRequest_BeginGetResponse
    {
      public static ManualResetEvent allDone= new ManualResetEvent(false);
      const int BUFFER_SIZE = 1024;
      public static string jsonString;
      public static void Main()
      {
        try
        {
        ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
        WebRequest myWebRequest = WebRequest.Create("https://tmi.twitch.tv/group/user/" + Settings.Channel + "/chatters");
        RequestState myRequestState = new RequestState();
        myRequestState.request = myWebRequest;
        IAsyncResult asyncResult=(IAsyncResult) myWebRequest.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);
        allDone.WaitOne();
        Console.Read();

        }
        catch(WebException e)
        {
          Helper.Log("WebException raised!");
          Helper.Log($"\n{e.Message}");
          Helper.Log($"\n{e.Status}");
        } 
        catch(Exception e)
        {
          Helper.Log("Exception raised!");
          Helper.Log("Source : " + e.Source);
          Helper.Log("Message : " + e.Message + " " + e.StackTrace);
        }
      }
      private static void RespCallback(IAsyncResult asynchronousResult)
      {  
        try
        {
          RequestState myRequestState=(RequestState) asynchronousResult.AsyncState;
          WebRequest  myWebRequest1=myRequestState.request;
          myRequestState.response =  myWebRequest1.EndGetResponse(asynchronousResult);
          Stream responseStream = myRequestState.response.GetResponseStream();
          myRequestState.responseStream=responseStream;
          IAsyncResult asynchronousResultRead = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
          myRequestState.response.Close();
    
        }
        catch(WebException e)
        {
          Helper.Log("WebException raised!");
          Helper.Log($"\n{e.Message}");
          Helper.Log($"\n{e.Status}");
        } 
        catch(Exception e)
        {
          Helper.Log("Exception raised!");
          Helper.Log("Source : " + e.Source);
          Helper.Log("Message : " + e.Message + " " + e.StackTrace);
        }
      }
      private static  void ReadCallBack(IAsyncResult asyncResult)
      {
        try
        {
          // Result state is set to AsyncState.
          RequestState myRequestState = (RequestState)asyncResult.AsyncState;
          Stream responseStream = myRequestState.responseStream;
          int read = responseStream.EndRead( asyncResult );
          // Read the contents of the HTML page and then print to the console.
          if (read > 0)
          {
            myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.bufferRead, 0, read));
            IAsyncResult asynchronousResult = responseStream.BeginRead( myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
          }
          else
          {
            if(myRequestState.requestData.Length>1)
            {
              string stringContent;
              stringContent = myRequestState.requestData.ToString();
              Helper.Log(stringContent);
              jsonString = stringContent;
              
            }
            responseStream.Close();
            allDone.Set();
          }
        }
        catch(WebException e)
        {
          Helper.Log("WebException raised!");
          Helper.Log($"\n{e.Message}");
          Helper.Log($"\n{e.Status}");
        } 
        catch(Exception e)
        {
          Helper.Log("Exception raised!");
          Helper.Log("Source : " + e.Source);
          Helper.Log("Message : " + e.Message + " " + e.StackTrace);
        }

      }

    static public bool MyRemoteCertificateValidationCallback(System.Object sender,
        X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain,
        // look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None) {
            for (int i=0; i<chain.ChainStatus.Length; i++) {
                if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown) {
                    continue;
                }
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan (0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build ((X509Certificate2)certificate);
                if (!chainIsValid) {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }

    }
}