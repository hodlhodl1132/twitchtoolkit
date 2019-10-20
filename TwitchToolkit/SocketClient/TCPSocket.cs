using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Verse;

namespace TwitchToolkit.SocketClient
{
    public abstract class TCPSocket
    {
        private TcpClient TcpClient { get; set; } = null;

        private SslStream SslStream { get; set; } = null;

        private readonly string server;

        private readonly int port;

        private ConcurrentCircularBuffer<string> socketMessages = new ConcurrentCircularBuffer<string>(10);

        public TCPSocket(string server, int port)
        {
            this.server = server;
            this.port = port;

            ConnectClient();
        }

        void ConnectClient()
        {

            IPHostEntry hostEntry = null;

            hostEntry = Dns.GetHostEntry(server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);

                TcpClient tempClient = new TcpClient(ipe);

                tempClient.BeginConnect(address, port, new AsyncCallback(ProcessClient), tempClient);
            }
        }

        void ProcessClient(IAsyncResult result)
        {
            try
            {
                TcpClient tempClient = result.AsyncState as TcpClient;
                if (TcpClient == null && tempClient.Connected)
                {
                    TcpClient = tempClient;
                }
                else
                {
                    return;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

                SslStream = new SslStream(TcpClient.GetStream(), false, (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyError) => { return true; });

                Helper.Log("Attemping authentication " + server);

                SslStream.AuthenticateAsClient(server, null, SslProtocols.Tls, true);

                PostAuthenticate();

                SslStream.ReadTimeout = 5000;
                SslStream.WriteTimeout = 5000;

                Read();
            }
            catch (Exception e)
            {
                Log.Error("Client Exception: " + e.Message);
                if (e.InnerException != null)
                {
                    Log.Error("Inner: " + e.InnerException.Message);
                }

                SslStream.Close();
                TcpClient.Close();
            }

            return;
        }

        public virtual void PostAuthenticate()
        {

        }

        public bool Send(string message)
        {
            if (!SslStream.CanWrite)
                throw new Exception("Stream Write Failure");

            Helper.Log("attemping stream write");

            try
            {
                var data = Encoding.UTF8.GetBytes(message);
                SslStream.BeginWrite(data, 0, data.Length, new AsyncCallback(PostWrite), null);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return false;
            }

            return true;
        }

        public virtual void PostWrite(IAsyncResult result)
        {
            SslStream.EndWrite(result);
        }

        byte[] buffer = new byte[2048];

        void Read()
        {
            try
            {
                SslStream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCallback), null);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        void ReadCallback(IAsyncResult asyncResult)
        {
            int bytes = SslStream.EndRead(asyncResult);
            string message = ReadMessage(bytes);
            if (message.Length > 0)
            {
                socketMessages.Put(message);

                ParseMessage(message);
            }
            Read();
        }

        string ReadMessage(int bytes = -1)
        {
            StringBuilder messageData = new StringBuilder();

            do
            {
                Decoder decoder = Helper.LanguageEncoding().GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);

                if (bytes > 2048)
                {
                    bytes -= 2048;
                }
                else
                {
                    bytes -= bytes;
                }

            } while (bytes != 0);

            return messageData.ToString();
        }

        public virtual void ParseMessage(string message)
        {

        }

        public string[] MessageLog
        {
            get
            {
                return socketMessages.Read();
            }
        }

        public bool Connected
        {
            get
            {
                return TcpClient.Connected;
            }
        }

        public void Disconnect()
        {
            if (!Connected) return;

            socketMessages.Clear();

            if (SslStream != null)
            {
                SslStream.Close();
            }

            if (TcpClient != null)
            {
                TcpClient.Close();
            }
        }

        public bool MyRemoteCertificateValidationCallback(object sender,
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
