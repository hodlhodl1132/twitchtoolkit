using System;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Verse;
using TwitchToolkit.Store;

namespace TwitchToolkit.IRC
{
    public delegate void OnPrivMsg(IRCMessage message);
    

    public class IRCClient
    {
        public event OnPrivMsg OnPrivMsg;
        public event OnPrivMsg OnUnkwnMsg;

        string _host;
        short _port;
        string _user;
        string _pass;
        string _channel;

        bool _ping;
        byte[] _buffer;
        IRCParser _parser;
        Socket _socket;
        bool _socketReady;
        NetworkStream _networkStream;
        SslStream _sslStream;

        Queue<string> _messageQueue;
        int _messageInterval = 2;
        Thread _messageThread;
        Thread _pingThread;

        ConcurrentCircularBuffer<string> _ircMessages = new ConcurrentCircularBuffer<string>(10);

        public IRCClient(string host, short port, string user, string pass, string channel)
        {
            _socketReady = false;
            _host = host;
            _port = port;
            _user = user.ToLower();

            if (!pass.StartsWith("oauth:", StringComparison.InvariantCultureIgnoreCase))
                _pass = "oauth:" + pass;
            else
                _pass = pass;

            _channel = channel.ToLower();
            _messageQueue = new Queue<string>();
            _buffer = new byte[8192];
            _parser = new IRCParser();
            _messageThread = new Thread(MessageThread);
            _messageThread.Start();
            _pingThread = new Thread(PingThread);
            _pingThread.Start();
        }

        public string[] MessageLog
        {
            get
            {
                return _ircMessages.Read();
            }
        }

        AutoResetEvent _messageHandle = new AutoResetEvent(false);
        void MessageThread()
        {
            while (true)
            {
            wait:
                _messageHandle.WaitOne(_messageInterval * 1000);

                if (_socketReady == false || _messageQueue.Count == 0)
                {
                    continue;
                }

                while (_messageQueue.Count > 0)
                {
                    var message = _messageQueue.Peek();
                    if (Send(message) == false)
                    {
                        goto wait;
                    }
                    _messageQueue.Dequeue();
                }
            }
        }

        void PingThread()
        {
            while (true)
            {
                if (_ping && _socketReady == true && _socket != null)
                {
                    Send("PING\n");
                }

                Thread.Sleep(60000);
            }
        }

        public void Connect()
        {
            if (_socket != null && _socket.Connected)
            {
                return;
            }
            _ping = true;
            _socketReady = false;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.BeginConnect(_host, _port, new AsyncCallback(ConnectCallback), null);
        }

        public bool Connected
        {
            get
            {
                if (_socket == null)
                {
                    return false;
                }

                return _socket.Connected;
            }
        }

        public void Disconnect()
        {
            if (!Connected) { return; }

            _ircMessages.Clear();

            _ping = false;
            _socketReady = false;

            if (_socket != null && _socket.Connected)
            {
                try
                {
                    _socket.Close();
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
        }

        public void Reconnect()
        {
            Disconnect();
            Toolkit.client.Connect();
        }

        void ConnectCallback(IAsyncResult asyncResult)
        {
            if (_socket != null)
            {
                _socket.EndConnect(asyncResult);
            }
            
            if (_socket == null || _socket.Connected == false)
            {
                Connect();
            }
            else
            {
                _networkStream = new NetworkStream(_socket);
                _sslStream = new SslStream(_networkStream, false, (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyError) => { return true; });
                _sslStream.AuthenticateAsClient(_host);
                Send("PASS " + _pass + "\nNICK " + _user + "\n");
                Read();
            }
        }

        void Read()
        {
            try
            {
                _sslStream.BeginRead(_buffer, 0, _buffer.Length, new AsyncCallback(ReadCallback), null);
            }
            catch (Exception)
            {
                Connect();
            }
        }

        void ReadCallback(IAsyncResult asyncResult)
        {
            var read = _sslStream.EndRead(asyncResult);
            _parser.Parse(_buffer, read, OnCommand);
            Read();
        }

        void OnCommand(IRCMessage message)
        {
            _ircMessages.Put(message.Cmd + " " + message.Args);

            switch (message.Cmd)
            {
                case "USERSTATE":
                    if (message.Parameters.ContainsKey("mod") && message.Parameters["mod"] == "1")
                    {
                        _messageInterval = 1;
                    }
                    else
                    {
                        _messageInterval = 2;
                    }
                    break;
                case "PING":
                    Send("PONG\n");
                    break;
                case "376":
                    if (ToolkitSettings.UseSeparateChatRoom && ToolkitSettings.ChatroomUUID != "" && ToolkitSettings.ChannelID != "")
                    {
                        Send(
                            "CAP REQ :twitch.tv/membership\n" +
                            "CAP REQ :twitch.tv/tags\n" +
                            "CAP REQ :twitch.tv/commands\n" +
                            "JOIN #" + _channel + "\n" +
                            "JOIN #chatrooms:" + ToolkitSettings.ChannelID + ":" + ToolkitSettings.ChatroomUUID + "\n"
                            );
                    }
                    else
                    {
                        Send(
                            "CAP REQ :twitch.tv/membership\n" +
                            "CAP REQ :twitch.tv/tags\n" +
                            "CAP REQ :twitch.tv/commands\n" +
                            "JOIN #" + _channel + "\n"
                            );
                    }

                    _socketReady = true;
                    break;
                case "PRIVMSG":
                    if (OnPrivMsg != null && !ToolkitSettings.WhisperCmdsOnly)
                    {
                        OnPrivMsg.Invoke(message);
                    }
                    break;
                case "WHISPER":
                    if (OnPrivMsg != null && ToolkitSettings.WhisperCmdsAllowed)
                    {
                        message.Whisper = true;
                        OnPrivMsg.Invoke(message);
                    }
                    break;
                case "PONG":
                    break;
                default:
                    if (OnUnkwnMsg != null)
                    {
                        OnUnkwnMsg.Invoke(message);
                    }
                    break;
            }
        }

        public void SendMessage(string message, bool botchannel = false)
        {
            if (ToolkitSettings.UseSeparateChatRoom && ToolkitSettings.ChatroomUUID != "" && ToolkitSettings.ChannelID != "" && botchannel)
            {
                _messageQueue.Enqueue("PRIVMSG #chatrooms:" + ToolkitSettings.ChannelID + ":" + ToolkitSettings.ChatroomUUID + " :" + message + "\n");
            }
            else
            {
                _messageQueue.Enqueue("PRIVMSG #" + _channel + " :" + message + "\n");
            }
            _messageHandle.Set();
        }

        bool Send(string message)
        {
            try
            {
                Encoding encoding = Helper.LanguageEncoding();
                var _data = Encoding.UTF8.GetBytes(message);
                _sslStream.BeginWrite(_data, 0, _data.Length, new AsyncCallback(WriteCallback), null);
            }
            catch (Exception)
            {
                Connect();
                return false;
            }

            return true;
        }

        void WriteCallback(IAsyncResult asyncResult)
        {
            _sslStream.EndWrite(asyncResult);
        }
    }
}
