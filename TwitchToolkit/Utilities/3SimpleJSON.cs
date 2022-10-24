// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONString
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONString : JSONNode
  {
    private string m_Data;

    public override JSONNodeType Tag => JSONNodeType.String;

    public override bool IsString => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => this.m_Data;
      set => this.m_Data = value;
    }

    public JSONString(string aData) => this.m_Data = aData;

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append('"').Append(JSONNode.Escape(this.m_Data)).Append('"');
    }

    public override bool Equals(object obj)
    {
      if (base.Equals(obj))
        return true;
      if (obj is string str)
        return this.m_Data == str;
      JSONString jsonString = obj as JSONString;
      return (JSONNode) jsonString != (object) null && this.m_Data == jsonString.m_Data;
    }

    public override int GetHashCode() => this.m_Data.GetHashCode();

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 3);
      aWriter.Write(this.m_Data);
    }
  }
}
