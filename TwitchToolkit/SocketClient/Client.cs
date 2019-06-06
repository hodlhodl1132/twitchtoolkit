using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using Verse;

namespace TwitchToolkit.SocketClient
{
    public class Client
    {
        private Socket Socket { get; set; } = null;

        private SslStream SslStream { get; set;  } = null;

        private readonly string server;

        private readonly int port;

        public Client(string server, int port)
        {
            this.server = server;
            this.port = port;

            ConnectSocket();

            //Socket.BeginConnect(server, port, new AsyncCallback(ProcessClient), null);
        }

        void ConnectSocket()
        {
            IPHostEntry hostEntry = null;

            // get host related information
            hostEntry = Dns.GetHostEntry(server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                Log.Warning("Attempting socket connect");
                tempSocket.BeginConnect(ipe, new AsyncCallback(ProcessClient), tempSocket);
            }
        }

        void ProcessClient(IAsyncResult asyncResult)
        {
            Socket tempSocket = asyncResult.AsyncState as Socket;
            if (tempSocket.Connected)
            {
                Socket = tempSocket;
            }
            else
            {
                return;
            }

            SslStream = new SslStream(new NetworkStream(Socket), false, (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyError) => { return true; });

            try
            {
                Log.Warning("attempting authentication");
                SslStream.AuthenticateAsClient(server);

                SslStream.ReadTimeout = 5000;
                SslStream.WriteTimeout = 5000;

                byte[] message = Helper.LanguageEncoding().GetBytes("{\"type\":\"hello\",\"id\":1,\"version\":\"2\"}");
                SslStream.Write(message);

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
                Socket.Close();
            }
            
        }

        byte[] buffer;

        void Read()
        {
            try
            {
                buffer = new byte[2048];
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
            if (message.Length > -1)
            {
                Log.Warning("message length: " + message.Length + " - " + message);
            }
            Read();
        }

        string ReadMessage(int bytes = -1)
        {
            StringBuilder messageData = new StringBuilder();

            do
            {
                Decoder decoder = Helper.LanguageEncoding().GetDecoder();
                char [] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
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
    }
}
