// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNumber
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONNumber : JSONNode
  {
    private double m_Data;

    public override JSONNodeType Tag => JSONNodeType.Number;

    public override bool IsNumber => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator();

    public override string Value
    {
      get => this.m_Data.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      set
      {
        double result;
        if (!double.TryParse(value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result))
          return;
        this.m_Data = result;
      }
    }

    public override double AsDouble
    {
      get => this.m_Data;
      set => this.m_Data = value;
    }

    public override long AsLong
    {
      get => (long) this.m_Data;
      set => this.m_Data = (double) value;
    }

    public JSONNumber(double aData) => this.m_Data = aData;

    public JSONNumber(string aData) => this.Value = aData;

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append(this.Value);
    }

    private static bool IsNumeric(object value)
    {
      int num;
      switch (value)
      {
        case int _:
        case uint _:
        case float _:
        case double _:
        case Decimal _:
        case long _:
        case ulong _:
        case short _:
        case ushort _:
        case sbyte _:
          num = 1;
          break;
        default:
          num = value is byte ? 1 : 0;
          break;
      }
      return num != 0;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      if (base.Equals(obj))
        return true;
      JSONNumber jsonNumber = obj as JSONNumber;
      if ((JSONNode) jsonNumber != (object) null)
        return this.m_Data == jsonNumber.m_Data;
      return JSONNumber.IsNumeric(obj) && Convert.ToDouble(obj) == this.m_Data;
    }

    public override int GetHashCode() => this.m_Data.GetHashCode();

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 4);
      aWriter.Write(this.m_Data);
    }
  }
}
