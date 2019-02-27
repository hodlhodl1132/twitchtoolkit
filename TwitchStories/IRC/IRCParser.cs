using System;

namespace TwitchStories.IRC
{
  public class IRCParser
  {
    enum IRCParserState
    {
      Start,
      P,
      Pi,
      Pin,
      Ping,
      Po,
      Pon,
      Pong,
      Host,
      User,
      Cmd,
      Args,
      Unknown,
      Key,
      Value
    };

    IRCParserState _state;
    IRCMessage _message;
    string _key;
    string _value;

    public void Parse(byte[] buffer, int length, OnMessage callback)
    {
      for (int i = 0; i < length; i++)
      {
        var b = (char)buffer[i];
        switch (_state)
        {
          case IRCParserState.Start:
            _message = new IRCMessage();
            _key = "";
            _value = "";
            switch (b)
            {
              case 'P':
                _state = IRCParserState.P;
                break;
              case '@':
                _state = IRCParserState.Key;
                break;
              case ':':
                _state = IRCParserState.Host;
                break;
              default:
                _state = IRCParserState.Unknown;
                break;
            }
            break;
          case IRCParserState.P:
            if (b == 'I') _state = IRCParserState.Pi;
            else if (b == 'O') _state = IRCParserState.Po;
            else _state = IRCParserState.Unknown;
            break;
          case IRCParserState.Pi:
            if (b == 'N') _state = IRCParserState.Pin;
            else _state = IRCParserState.Unknown;
            break;
          case IRCParserState.Pin:
            if (b == 'G') _state = IRCParserState.Ping;
            else _state = IRCParserState.Unknown;
            break;
          case IRCParserState.Ping:
            if (b == '\n')
            {
              _message.Cmd = "PING";
              callback(_message);
              _state = IRCParserState.Start;
            }
            break;
          case IRCParserState.Po:
            if (b == 'N') _state = IRCParserState.Pon;
            else _state = IRCParserState.Unknown;
            break;
          case IRCParserState.Pon:
            if (b == 'G') _state = IRCParserState.Pong;
            else _state = IRCParserState.Unknown;
            break;
          case IRCParserState.Pong:
            if (b == '\n')
            {
              _message.Cmd = "PONG";
              callback(_message);
              _state = IRCParserState.Start;
            }
            break;
          case IRCParserState.Host:
            if (b == ' ') _state = IRCParserState.Cmd;
            else if (b == '@') _state = IRCParserState.User;
            else if (b != '\r')
            {
              _message.Host += b;
            }
            break;
          case IRCParserState.User:
            if (b == '.') _state = IRCParserState.Host;
            else
            {
              _message.User += b;
            }
            break;
          case IRCParserState.Cmd:
            if (b == '\n')
            {
              callback(_message);
              _state = IRCParserState.Start;
            }
            else if (b == ' ')
            {
              _state = IRCParserState.Args;
            }
            else if (b != '\r')
            {
              _message.Cmd += b;
            }
            break;
          case IRCParserState.Args:
            if (b == '\n')
            {
              if (Array.IndexOf(new string[] { "PRIVMSG", "WHISPER", "USERSTATE", "USERNOTICE", "NOTICE" }, _message.Cmd) != -1)
              {
                int state = 0;
                for (int j = 0; j < _message.Args.Length; j++)
                {
                  switch (state)
                  {
                    case 0:
                      if (_message.Args[j] == ' ') state = 1;
                      else
                      {
                        _message.Channel += _message.Args[j];
                      }
                      break;
                    case 1:
                      if (_message.Args[j] == ':') state = 2;
                      break;
                    default:
                      _message.Message += _message.Args[j];
                      break;
                  }
                }
              }

              callback(_message);
              _state = IRCParserState.Start;
            }
            else if (b != '\r')
            {
              _message.Args += b;
            }
            break;
          case IRCParserState.Unknown:
            if (b == '\n') _state = IRCParserState.Start;
            break;
          case IRCParserState.Key:
            if (b == '=') _state = IRCParserState.Value;
            else if (b == ' ') _state = IRCParserState.Host;
            else
            {
              _key += b;
            }
            break;
          case IRCParserState.Value:
            if (b == ';' || b == ' ')
            {
              _message.Parameters.Add(_key, _value);
            }

            if (b == ';') _state = IRCParserState.Key;
            else if (b == ' ') _state = IRCParserState.Host;
            else
            {
              _value += b;
            }
            break;
          default:
            _state = IRCParserState.Start;
            break;
        }
      }
    }
  }
}
