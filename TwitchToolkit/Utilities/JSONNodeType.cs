// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNodeType
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

namespace SimpleJSON
{
  public enum JSONNodeType
  {
    Array = 1,
    Object = 2,
    String = 3,
    Number = 4,
    NullValue = 5,
    Boolean = 6,
    None = 7,
    Custom = 255, // 0x000000FF
  }
}
