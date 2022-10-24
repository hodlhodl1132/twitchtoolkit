// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNull
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONNull : JSONNode
  {
    private static JSONNull m_StaticInstance = new JSONNull();
    public static bool reuseSameInstance = true;

    public static JSONNull CreateOrGet() => JSONNull.reuseSameInstance ? JSONNull.m_StaticInstance : new JSONNull();

    private JSONNull()
    {
    }

    public override JSONNodeType Tag => JSONNodeType.NullValue;

    public override bool IsNull => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => "null";
      set
      {
      }
    }

    public override bool AsBool
    {
      get => false;
      set
      {
      }
    }

    public override bool Equals(object obj) => this == obj || obj is JSONNull;

    public override int GetHashCode() => 0;

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append("null");
    }

    public override void SerializeBinary(BinaryWriter aWriter) => aWriter.Write((byte) 5);
  }
}
