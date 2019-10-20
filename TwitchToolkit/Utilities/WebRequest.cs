using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using TwitchToolkit;

namespace TwitchToolkitDev
{
    public class RequestState
    {
        // This class stores the state of the request.
        const int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] bufferRead;
        public WebRequest request;
        public WebResponse response;
        public Stream responseStream;
        public Func<RequestState, bool> Callback;
        public string jsonString;
        public string urlCalled;

        public RequestState()
        {
            bufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            responseStream = null;
        }

        public object CallbackClass { get; internal set; }
    }
    public class WebRequest_BeginGetResponse
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 1024;
        public static void Main(string requesturl, Func<RequestState, bool> func = null)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
                WebRequest myWebRequest = WebRequest.Create(requesturl);
                RequestState myRequestState = new RequestState();
                myRequestState.urlCalled = requesturl;
                myRequestState.Callback = func;
                myRequestState.request = myWebRequest;
                IAsyncResult asyncResult = (IAsyncResult)myWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
                allDone.WaitOne();
                Console.Read();

            }
            catch (WebException e)
            {
                Helper.Log("WebException raised!");
                Helper.Log($"\n{e.Message}");
                Helper.Log($"\n{e.Status}");
            }
            catch (Exception e)
            {
                Helper.Log("Exception raised! - get");
                Helper.Log("Source : " + e.Source);
                Helper.Log("Message : " + e.Message + " " + e.StackTrace);
            }
        }

        public static void Delete(string requesturl, Func<RequestState, bool> func = null, WebHeaderCollection headers = null)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
                WebRequest myWebRequest = WebRequest.Create(requesturl);
                myWebRequest.Method = "DELETE";
                myWebRequest.Headers = headers;
                RequestState myRequestState = new RequestState();
                myRequestState.urlCalled = requesturl;
                myRequestState.Callback = func;
                myRequestState.request = myWebRequest;
                IAsyncResult asyncResult = (IAsyncResult)myWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
                allDone.WaitOne();
                Console.Read();

            }
            catch (WebException e)
            {
                Helper.Log("WebException raised!");
                Helper.Log($"\n{e.Message}");
                Helper.Log($"\n{e.Status}");
            }
            catch (Exception e)
            {
                Helper.Log("Exception raised! - get");
                Helper.Log("Source : " + e.Source);
                Helper.Log("Message : " + e.Message + " " + e.StackTrace);
            }
        }
        public static void Put(string requesturl, Func<RequestState, bool> func = null, WebHeaderCollection headers = null)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
                WebRequest myWebRequest = WebRequest.Create(requesturl);
                myWebRequest.Method = "PUT";
                myWebRequest.Headers = headers;
                RequestState myRequestState = new RequestState();
                myRequestState.urlCalled = requesturl;
                myRequestState.Callback = func;
                myRequestState.request = myWebRequest;
                IAsyncResult asyncResult = (IAsyncResult)myWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
                allDone.WaitOne();
                Console.Read();

            }
            catch (WebException e)
            {
                Helper.Log("WebException raised!");
                Helper.Log($"\n{e.Message}");
                Helper.Log($"\n{e.Status}");
            }
            catch (Exception e)
            {
                Helper.Log("Exception raised! - get");
                Helper.Log("Source : " + e.Source);
                Helper.Log("Message : " + e.Message + " " + e.StackTrace);
            }
        }
        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
                WebRequest myWebRequest1 = myRequestState.request;
                myRequestState.response = myWebRequest1.EndGetResponse(asynchronousResult);
                Stream responseStream = myRequestState.response.GetResponseStream();
                myRequestState.responseStream = responseStream;
                IAsyncResult asynchronousResultRead = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                myRequestState.response.Close();

            }
            catch (WebException e)
            {
                Helper.Log("WebException raised - callback!");
                Helper.Log($"\n{e.Message}");
                Helper.Log($"\n{e.Status}");
            }
            catch (Exception e)
            {
                Helper.Log("Exception raised!");
                Helper.Log("Source : " + e.Source);
                Helper.Log("Message : " + e.Message + " " + e.StackTrace);
            }
        }
        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                // Result state is set to AsyncState.
                RequestState myRequestState = (RequestState)asyncResult.AsyncState;
                Stream responseStream = myRequestState.responseStream;
                int read = responseStream.EndRead(asyncResult);
                // Read the contents of the HTML page and then print to the console.
                if (read > 0)
                {
                    myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.bufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                }
                else
                {
                    if (myRequestState.requestData.Length > 1)
                    {
                        string stringContent;
                        stringContent = myRequestState.requestData.ToString();
                        myRequestState.jsonString = stringContent;
                        Helper.Log(stringContent);

                        myRequestState.Callback?.Invoke(myRequestState);

                    }
                    responseStream.Close();
                    allDone.Set();
                }
            }
            catch (WebException e)
            {
                Helper.Log("WebException raised - read!");
                Helper.Log($"\n{e.Message}");
                Helper.Log($"\n{e.Status}");
            }
            catch (Exception e)
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
            if (sslPolicyErrors != SslPolicyErrors.None)
            {
                for (int i = 0; i < chain.ChainStatus.Length; i++)
                {
                    if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        continue;
                    }
                    chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                    chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                    chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                    bool chainIsValid = chain.Build((X509Certificate2)certificate);
                    if (!chainIsValid)
                    {
                        isOk = false;
                        break;
                    }
                }
            }
            return isOk;
        }

    }
}
