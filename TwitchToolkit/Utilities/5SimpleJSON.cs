// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONBool
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONBool : JSONNode
  {
    private bool m_Data;

    public override JSONNodeType Tag => JSONNodeType.Boolean;

    public override bool IsBoolean => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => this.m_Data.ToString();
      set
      {
        bool result;
        if (!bool.TryParse(value, out result))
          return;
        this.m_Data = result;
      }
    }

    public override bool AsBool
    {
      get => this.m_Data;
      set => this.m_Data = value;
    }

    public JSONBool(bool aData) => this.m_Data = aData;

    public JSONBool(string aData) => this.Value = aData;

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append(this.m_Data ? "true" : "false");
    }

    public override bool Equals(object obj) => obj != null && obj is bool flag && this.m_Data == flag;

    public override int GetHashCode() => this.m_Data.GetHashCode();

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 6);
      aWriter.Write(this.m_Data);
    }
  }
}
