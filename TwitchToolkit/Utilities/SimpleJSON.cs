// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONNode
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace SimpleJSON
{
  public abstract class JSONNode
  {
    public static bool forceASCII = false;
    public static bool longAsString = false;
    [ThreadStatic]
    private static StringBuilder m_EscapeBuilder;
    public static JSONContainerType VectorContainerType = JSONContainerType.Array;
    public static JSONContainerType QuaternionContainerType = JSONContainerType.Array;
    public static JSONContainerType RectContainerType = JSONContainerType.Array;

    public abstract JSONNodeType Tag { get; }

    public virtual JSONNode this[int aIndex]
    {
      get => (JSONNode) null;
      set
      {
      }
    }

    public virtual JSONNode this[string aKey]
    {
      get => (JSONNode) null;
      set
      {
      }
    }

    public virtual string Value
    {
      get => "";
      set
      {
      }
    }

    public virtual int Count => 0;

    public virtual bool IsNumber => false;

    public virtual bool IsString => false;

    public virtual bool IsBoolean => false;

    public virtual bool IsNull => false;

    public virtual bool IsArray => false;

    public virtual bool IsObject => false;

    public virtual bool Inline
    {
      get => false;
      set
      {
      }
    }

    public virtual void Add(string aKey, JSONNode aItem)
    {
    }

    public virtual void Add(JSONNode aItem) => this.Add("", aItem);

    public virtual JSONNode Remove(string aKey) => (JSONNode) null;

    public virtual JSONNode Remove(int aIndex) => (JSONNode) null;

    public virtual JSONNode Remove(JSONNode aNode) => aNode;

    public virtual IEnumerable<JSONNode> Children
    {
      get
      {
        yield break;
      }
    }

    public IEnumerable<JSONNode> DeepChildren
    {
      get
      {
        foreach (JSONNode C in this.Children)
        {
          foreach (JSONNode D in C.DeepChildren)
            yield return D;
        }
      }
    }

    public virtual bool HasKey(string aKey) => false;

    public virtual JSONNode GetValueOrDefault(string aKey, JSONNode aDefault) => aDefault;

    public override string ToString()
    {
      StringBuilder aSB = new StringBuilder();
      this.WriteToStringBuilder(aSB, 0, 0, JSONTextMode.Compact);
      return aSB.ToString();
    }

    public virtual string ToString(int aIndent)
    {
      StringBuilder aSB = new StringBuilder();
      this.WriteToStringBuilder(aSB, 0, aIndent, JSONTextMode.Indent);
      return aSB.ToString();
    }

    internal abstract void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode);

    public abstract JSONNode.Enumerator GetEnumerator();

    public IEnumerable<KeyValuePair<string, JSONNode>> Linq => (IEnumerable<KeyValuePair<string, JSONNode>>) new JSONNode.LinqEnumerator(this);

    public JSONNode.KeyEnumerator Keys => new JSONNode.KeyEnumerator(this.GetEnumerator());

    public JSONNode.ValueEnumerator Values => new JSONNode.ValueEnumerator(this.GetEnumerator());

    public virtual double AsDouble
    {
      get
      {
        double result = 0.0;
        return double.TryParse(this.Value, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? result : 0.0;
      }
      set => this.Value = value.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public virtual int AsInt
    {
      get => (int) this.AsDouble;
      set => this.AsDouble = (double) value;
    }

    public virtual float AsFloat
    {
      get => (float) this.AsDouble;
      set => this.AsDouble = (double) value;
    }

    public virtual bool AsBool
    {
      get
      {
        bool result = false;
        return bool.TryParse(this.Value, out result) ? result : !string.IsNullOrEmpty(this.Value);
      }
      set => this.Value = value ? "true" : "false";
    }

    public virtual long AsLong
    {
      get
      {
        long result = 0;
        return long.TryParse(this.Value, out result) ? result : 0L;
      }
      set => this.Value = value.ToString();
    }

    public virtual JSONArray AsArray => this as JSONArray;

    public virtual JSONObject AsObject => this as JSONObject;

    public static implicit operator JSONNode(string s) => (JSONNode) new JSONString(s);

    public static implicit operator string(JSONNode d) => d == (object) null ? (string) null : d.Value;

    public static implicit operator JSONNode(double n) => (JSONNode) new JSONNumber(n);

    public static implicit operator double(JSONNode d) => d == (object) null ? 0.0 : d.AsDouble;

    public static implicit operator JSONNode(float n) => (JSONNode) new JSONNumber((double) n);

    public static implicit operator float(JSONNode d) => d == (object) null ? 0.0f : d.AsFloat;

    public static implicit operator JSONNode(int n) => (JSONNode) new JSONNumber((double) n);

    public static implicit operator int(JSONNode d) => d == (object) null ? 0 : d.AsInt;

    public static implicit operator JSONNode(long n) => JSONNode.longAsString ? (JSONNode) new JSONString(n.ToString()) : (JSONNode) new JSONNumber((double) n);

    public static implicit operator long(JSONNode d) => d == (object) null ? 0L : d.AsLong;

    public static implicit operator JSONNode(bool b) => (JSONNode) new JSONBool(b);

    public static implicit operator bool(JSONNode d) => !(d == (object) null) && d.AsBool;

    public static implicit operator JSONNode(KeyValuePair<string, JSONNode> aKeyValue) => aKeyValue.Value;

    public static bool operator ==(JSONNode a, object b)
    {
      if ((object) a == b)
        return true;
      bool flag1 = a is JSONNull || (object) a == null || a is JSONLazyCreator;
      int num;
      switch (b)
      {
        case JSONNull _:
        case null:
          num = 1;
          break;
        default:
          num = b is JSONLazyCreator ? 1 : 0;
          break;
      }
      bool flag2 = num != 0;
      return flag1 & flag2 || !flag1 && a.Equals(b);
    }

    public static bool operator !=(JSONNode a, object b) => !(a == b);

    public override bool Equals(object obj) => (object) this == obj;

    public override int GetHashCode() => base.GetHashCode();

    internal static StringBuilder EscapeBuilder
    {
      get
      {
        if (JSONNode.m_EscapeBuilder == null)
          JSONNode.m_EscapeBuilder = new StringBuilder();
        return JSONNode.m_EscapeBuilder;
      }
    }

    internal static string Escape(string aText)
    {
      StringBuilder escapeBuilder = JSONNode.EscapeBuilder;
      escapeBuilder.Length = 0;
      if (escapeBuilder.Capacity < aText.Length + aText.Length / 10)
        escapeBuilder.Capacity = aText.Length + aText.Length / 10;
      foreach (char ch in aText)
      {
        switch (ch)
        {
          case '\b':
            escapeBuilder.Append("\\b");
            break;
          case '\t':
            escapeBuilder.Append("\\t");
            break;
          case '\n':
            escapeBuilder.Append("\\n");
            break;
          case '\f':
            escapeBuilder.Append("\\f");
            break;
          case '\r':
            escapeBuilder.Append("\\r");
            break;
          case '"':
            escapeBuilder.Append("\\\"");
            break;
          case '\\':
            escapeBuilder.Append("\\\\");
            break;
          default:
            if (ch < ' ' || JSONNode.forceASCII && ch > '\u007F')
            {
              ushort num = (ushort) ch;
              escapeBuilder.Append("\\u").Append(num.ToString("X4"));
              break;
            }
            escapeBuilder.Append(ch);
            break;
        }
      }
      string str = escapeBuilder.ToString();
      escapeBuilder.Length = 0;
      return str;
    }

    private static JSONNode ParseElement(string token, bool quoted)
    {
      if (quoted)
        return (JSONNode) token;
      string lower = token.ToLower();
      if (lower == "false" || lower == "true")
        return (JSONNode) (lower == "true");
      if (lower == "null")
        return (JSONNode) JSONNull.CreateOrGet();
      double result;
      return double.TryParse(token, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture, out result) ? (JSONNode) result : (JSONNode) token;
    }

    public static JSONNode Parse(string aJSON)
    {
      Stack<JSONNode> jsonNodeStack = new Stack<JSONNode>();
      JSONNode jsonNode = (JSONNode) null;
      int index = 0;
      StringBuilder stringBuilder = new StringBuilder();
      string aKey = "";
      bool flag = false;
      bool quoted = false;
      for (; index < aJSON.Length; ++index)
      {
        switch (aJSON[index])
        {
          case '\t':
          case ' ':
            if (flag)
            {
              stringBuilder.Append(aJSON[index]);
              continue;
            }
            continue;
          case '\n':
          case '\r':
            continue;
          case '"':
            flag = !flag;
            quoted |= flag;
            continue;
          case ',':
            if (flag)
            {
              stringBuilder.Append(aJSON[index]);
              continue;
            }
            if (stringBuilder.Length > 0 | quoted)
              jsonNode.Add(aKey, JSONNode.ParseElement(stringBuilder.ToString(), quoted));
            aKey = "";
            stringBuilder.Length = 0;
            quoted = false;
            continue;
          case ':':
            if (flag)
            {
              stringBuilder.Append(aJSON[index]);
              continue;
            }
            aKey = stringBuilder.ToString();
            stringBuilder.Length = 0;
            quoted = false;
            continue;
          case '[':
            if (flag)
            {
              stringBuilder.Append(aJSON[index]);
              continue;
            }
            jsonNodeStack.Push((JSONNode) new JSONArray());
            if (jsonNode != (object) null)
              jsonNode.Add(aKey, jsonNodeStack.Peek());
            aKey = "";
            stringBuilder.Length = 0;
            jsonNode = jsonNodeStack.Peek();
            continue;
          case '\\':
            ++index;
            if (flag)
            {
              char ch = aJSON[index];
              switch (ch)
              {
                case 'b':
                  stringBuilder.Append('\b');
                  break;
                case 'f':
                  stringBuilder.Append('\f');
                  break;
                case 'n':
                  stringBuilder.Append('\n');
                  break;
                case 'r':
                  stringBuilder.Append('\r');
                  break;
                case 't':
                  stringBuilder.Append('\t');
                  break;
                case 'u':
                  string s = aJSON.Substring(index + 1, 4);
                  stringBuilder.Append((char) int.Parse(s, NumberStyles.AllowHexSpecifier));
                  index += 4;
                  break;
                default:
                  stringBuilder.Append(ch);
                  break;
              }
              continue;
            }
            continue;
          case ']':
          case '}':
            if (flag)
            {
              stringBuilder.Append(aJSON[index]);
              continue;
            }
            if (jsonNodeStack.Count == 0)
              throw new Exception("JSON Parse: Too many closing brackets");
            jsonNodeStack.Pop();
            if (stringBuilder.Length > 0 | quoted)
              jsonNode.Add(aKey, JSONNode.ParseElement(stringBuilder.ToString(), quoted));
            quoted = false;
            aKey = "";
            stringBuilder.Length = 0;
            if (jsonNodeStack.Count > 0)
            {
              jsonNode = jsonNodeStack.Peek();
              continue;
            }
            continue;
          case '{':
            if (flag)
            {
              stringBuilder.Append(aJSON[index]);
              continue;
            }
            jsonNodeStack.Push((JSONNode) new JSONObject());
            if (jsonNode != (object) null)
              jsonNode.Add(aKey, jsonNodeStack.Peek());
            aKey = "";
            stringBuilder.Length = 0;
            jsonNode = jsonNodeStack.Peek();
            continue;
          default:
            stringBuilder.Append(aJSON[index]);
            continue;
        }
      }
      if (flag)
        throw new Exception("JSON Parse: Quotation marks seems to be messed up.");
      return jsonNode == (object) null ? JSONNode.ParseElement(stringBuilder.ToString(), quoted) : jsonNode;
    }

    public abstract void SerializeBinary(BinaryWriter aWriter);

    public void SaveToBinaryStream(Stream aData) => this.SerializeBinary(new BinaryWriter(aData));

    public void SaveToCompressedStream(Stream aData) => throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");

    public void SaveToCompressedFile(string aFileName) => throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");

    public string SaveToCompressedBase64() => throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");

    public void SaveToBinaryFile(string aFileName)
    {
      Directory.CreateDirectory(new FileInfo(aFileName).Directory.FullName);
      using (FileStream aData = File.OpenWrite(aFileName))
        this.SaveToBinaryStream((Stream) aData);
    }

    public string SaveToBinaryBase64()
    {
      using (MemoryStream aData = new MemoryStream())
      {
        this.SaveToBinaryStream((Stream) aData);
        aData.Position = 0L;
        return Convert.ToBase64String(aData.ToArray());
      }
    }

    public static JSONNode DeserializeBinary(BinaryReader aReader)
    {
      JSONNodeType jsonNodeType = (JSONNodeType) aReader.ReadByte();
      switch (jsonNodeType)
      {
        case JSONNodeType.Array:
          int num1 = aReader.ReadInt32();
          JSONArray jsonArray = new JSONArray();
          for (int index = 0; index < num1; ++index)
            jsonArray.Add(JSONNode.DeserializeBinary(aReader));
          return (JSONNode) jsonArray;
        case JSONNodeType.Object:
          int num2 = aReader.ReadInt32();
          JSONObject jsonObject = new JSONObject();
          for (int index = 0; index < num2; ++index)
          {
            string aKey = aReader.ReadString();
            JSONNode aItem = JSONNode.DeserializeBinary(aReader);
            jsonObject.Add(aKey, aItem);
          }
          return (JSONNode) jsonObject;
        case JSONNodeType.String:
          return (JSONNode) new JSONString(aReader.ReadString());
        case JSONNodeType.Number:
          return (JSONNode) new JSONNumber(aReader.ReadDouble());
        case JSONNodeType.NullValue:
          return (JSONNode) JSONNull.CreateOrGet();
        case JSONNodeType.Boolean:
          return (JSONNode) new JSONBool(aReader.ReadBoolean());
        default:
          throw new Exception("Error deserializing JSON. Unknown tag: " + (object) jsonNodeType);
      }
    }

    public static JSONNode LoadFromCompressedFile(string aFileName) => throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");

    public static JSONNode LoadFromCompressedStream(Stream aData) => throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");

    public static JSONNode LoadFromCompressedBase64(string aBase64) => throw new Exception("Can't use compressed functions. You need include the SharpZipLib and uncomment the define at the top of SimpleJSON");

    public static JSONNode LoadFromBinaryStream(Stream aData)
    {
      using (BinaryReader aReader = new BinaryReader(aData))
        return JSONNode.DeserializeBinary(aReader);
    }

    public static JSONNode LoadFromBinaryFile(string aFileName)
    {
      using (FileStream aData = File.OpenRead(aFileName))
        return JSONNode.LoadFromBinaryStream((Stream) aData);
    }

    public static JSONNode LoadFromBinaryBase64(string aBase64)
    {
      MemoryStream aData = new MemoryStream(Convert.FromBase64String(aBase64));
      aData.Position = 0L;
      return JSONNode.LoadFromBinaryStream((Stream) aData);
    }

    private static JSONNode GetContainer(JSONContainerType aType) => aType == JSONContainerType.Array ? (JSONNode) new JSONArray() : (JSONNode) new JSONObject();

    public static implicit operator JSONNode(Vector2 aVec)
    {
      JSONNode container = JSONNode.GetContainer(JSONNode.VectorContainerType);
      container.WriteVector2(aVec);
      return container;
    }

    public static implicit operator JSONNode(Vector3 aVec)
    {
      JSONNode container = JSONNode.GetContainer(JSONNode.VectorContainerType);
      container.WriteVector3(aVec);
      return container;
    }

    public static implicit operator JSONNode(Vector4 aVec)
    {
      JSONNode container = JSONNode.GetContainer(JSONNode.VectorContainerType);
      container.WriteVector4(aVec);
      return container;
    }

    public static implicit operator JSONNode(Quaternion aRot)
    {
      JSONNode container = JSONNode.GetContainer(JSONNode.QuaternionContainerType);
      container.WriteQuaternion(aRot);
      return container;
    }

    public static implicit operator JSONNode(Rect aRect)
    {
      JSONNode container = JSONNode.GetContainer(JSONNode.RectContainerType);
      container.WriteRect(aRect);
      return container;
    }

    public static implicit operator JSONNode(RectOffset aRect)
    {
      JSONNode container = JSONNode.GetContainer(JSONNode.RectContainerType);
      container.WriteRectOffset(aRect);
      return container;
    }

    public static implicit operator Vector2(JSONNode aNode) => aNode.ReadVector2();

    public static implicit operator Vector3(JSONNode aNode) => aNode.ReadVector3();

    public static implicit operator Vector4(JSONNode aNode) => aNode.ReadVector4();

    public static implicit operator Quaternion(JSONNode aNode) => aNode.ReadQuaternion();

    public static implicit operator Rect(JSONNode aNode) => aNode.ReadRect();

    public static implicit operator RectOffset(JSONNode aNode) => aNode.ReadRectOffset();

    public Vector2 ReadVector2(Vector2 aDefault)
    {
      if (this.IsObject)
        return new Vector2(this["x"].AsFloat, this["y"].AsFloat);
      return this.IsArray ? new Vector2(this[0].AsFloat, this[1].AsFloat) : aDefault;
    }

    public Vector2 ReadVector2(string aXName, string aYName) => this.IsObject ? new Vector2(this[aXName].AsFloat, this[aYName].AsFloat) : Vector2.zero;

    public Vector2 ReadVector2() => this.ReadVector2(Vector2.zero);

    public JSONNode WriteVector2(Vector2 aVec, string aXName = "x", string aYName = "y")
    {
      if (this.IsObject)
      {
        this.Inline = true;
        this[aXName].AsFloat = aVec.x;
        this[aYName].AsFloat = aVec.y;
      }
      else if (this.IsArray)
      {
        this.Inline = true;
        this[0].AsFloat = aVec.x;
        this[1].AsFloat = aVec.y;
      }
      return this;
    }

    public Vector3 ReadVector3(Vector3 aDefault)
    {
      if (this.IsObject)
        return new Vector3(this["x"].AsFloat, this["y"].AsFloat, this["z"].AsFloat);
      return this.IsArray ? new Vector3(this[0].AsFloat, this[1].AsFloat, this[2].AsFloat) : aDefault;
    }

    public Vector3 ReadVector3(string aXName, string aYName, string aZName) => this.IsObject ? new Vector3(this[aXName].AsFloat, this[aYName].AsFloat, this[aZName].AsFloat) : Vector3.zero;

    public Vector3 ReadVector3() => this.ReadVector3(Vector3.zero);

    public JSONNode WriteVector3(
      Vector3 aVec,
      string aXName = "x",
      string aYName = "y",
      string aZName = "z")
    {
      if (this.IsObject)
      {
        this.Inline = true;
        this[aXName].AsFloat = aVec.x;
        this[aYName].AsFloat = aVec.y;
        this[aZName].AsFloat = aVec.z;
      }
      else if (this.IsArray)
      {
        this.Inline = true;
        this[0].AsFloat = aVec.x;
        this[1].AsFloat = aVec.y;
        this[2].AsFloat = aVec.z;
      }
      return this;
    }

    public Vector4 ReadVector4(Vector4 aDefault)
    {
      if (this.IsObject)
        return new Vector4(this["x"].AsFloat, this["y"].AsFloat, this["z"].AsFloat, this["w"].AsFloat);
      return this.IsArray ? new Vector4(this[0].AsFloat, this[1].AsFloat, this[2].AsFloat, this[3].AsFloat) : aDefault;
    }

    public Vector4 ReadVector4() => this.ReadVector4(Vector4.zero);

    public JSONNode WriteVector4(Vector4 aVec)
    {
      if (this.IsObject)
      {
        this.Inline = true;
        this["x"].AsFloat = aVec.x;
        this["y"].AsFloat = aVec.y;
        this["z"].AsFloat = aVec.z;
        this["w"].AsFloat = aVec.w;
      }
      else if (this.IsArray)
      {
        this.Inline = true;
        this[0].AsFloat = aVec.x;
        this[1].AsFloat = aVec.y;
        this[2].AsFloat = aVec.z;
        this[3].AsFloat = aVec.w;
      }
      return this;
    }

    public Quaternion ReadQuaternion(Quaternion aDefault)
    {
      if (this.IsObject)
        return new Quaternion(this["x"].AsFloat, this["y"].AsFloat, this["z"].AsFloat, this["w"].AsFloat);
      return this.IsArray ? new Quaternion(this[0].AsFloat, this[1].AsFloat, this[2].AsFloat, this[3].AsFloat) : aDefault;
    }

    public Quaternion ReadQuaternion() => this.ReadQuaternion(Quaternion.identity);

    public JSONNode WriteQuaternion(Quaternion aRot)
    {
      if (this.IsObject)
      {
        this.Inline = true;
        this["x"].AsFloat = aRot.x;
        this["y"].AsFloat = aRot.y;
        this["z"].AsFloat = aRot.z;
        this["w"].AsFloat = aRot.w;
      }
      else if (this.IsArray)
      {
        this.Inline = true;
        this[0].AsFloat = aRot.x;
        this[1].AsFloat = aRot.y;
        this[2].AsFloat = aRot.z;
        this[3].AsFloat = aRot.w;
      }
      return this;
    }

    public Rect ReadRect(Rect aDefault)
    {
      if (this.IsObject)
        return new Rect(this["x"].AsFloat, this["y"].AsFloat, this["width"].AsFloat, this["height"].AsFloat);
      return this.IsArray ? new Rect(this[0].AsFloat, this[1].AsFloat, this[2].AsFloat, this[3].AsFloat) : aDefault;
    }

    public Rect ReadRect() => this.ReadRect(new Rect());

    public JSONNode WriteRect(Rect aRect)
    {
      if (this.IsObject)
      {
        this.Inline = true;
        this["x"].AsFloat = aRect.x;
        this["y"].AsFloat = aRect.y;
        this["width"].AsFloat = aRect.width;
        this["height"].AsFloat = aRect.height;
      }
      else if (this.IsArray)
      {
        this.Inline = true;
        this[0].AsFloat = aRect.x;
        this[1].AsFloat = aRect.y;
        this[2].AsFloat = aRect.width;
        this[3].AsFloat = aRect.height;
      }
      return this;
    }

    public RectOffset ReadRectOffset(RectOffset aDefault)
    {
      switch (this)
      {
        case JSONObject _:
          return new RectOffset(this["left"].AsInt, this["right"].AsInt, this["top"].AsInt, this["bottom"].AsInt);
        case JSONArray _:
          return new RectOffset(this[0].AsInt, this[1].AsInt, this[2].AsInt, this[3].AsInt);
        default:
          return aDefault;
      }
    }

    public RectOffset ReadRectOffset() => this.ReadRectOffset(new RectOffset());

    public JSONNode WriteRectOffset(RectOffset aRect)
    {
      if (this.IsObject)
      {
        this.Inline = true;
        this["left"].AsInt = aRect.left;
        this["right"].AsInt = aRect.right;
        this["top"].AsInt = aRect.top;
        this["bottom"].AsInt = aRect.bottom;
      }
      else if (this.IsArray)
      {
        this.Inline = true;
        this[0].AsInt = aRect.left;
        this[1].AsInt = aRect.right;
        this[2].AsInt = aRect.top;
        this[3].AsInt = aRect.bottom;
      }
      return this;
    }

    public Matrix4x4 ReadMatrix()
    {
      Matrix4x4 identity = Matrix4x4.identity;
      if (this.IsArray)
      {
        for (int index = 0; index < 16; ++index)
          identity[index] = this[index].AsFloat;
      }
      return identity;
    }

    public JSONNode WriteMatrix(Matrix4x4 aMatrix)
    {
      if (this.IsArray)
      {
        this.Inline = true;
        for (int index = 0; index < 16; ++index)
          this[index].AsFloat = aMatrix[index];
      }
      return this;
    }

    public struct Enumerator
    {
      private JSONNode.Enumerator.Type type;
      private Dictionary<string, JSONNode>.Enumerator m_Object;
      private List<JSONNode>.Enumerator m_Array;

      public bool IsValid => this.type != 0;

      public Enumerator(List<JSONNode>.Enumerator aArrayEnum)
      {
        this.type = JSONNode.Enumerator.Type.Array;
        this.m_Object = new Dictionary<string, JSONNode>.Enumerator();
        this.m_Array = aArrayEnum;
      }

      public Enumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
      {
        this.type = JSONNode.Enumerator.Type.Object;
        this.m_Object = aDictEnum;
        this.m_Array = new List<JSONNode>.Enumerator();
      }

      public KeyValuePair<string, JSONNode> Current
      {
        get
        {
          if (this.type == JSONNode.Enumerator.Type.Array)
            return new KeyValuePair<string, JSONNode>(string.Empty, this.m_Array.Current);
          return this.type == JSONNode.Enumerator.Type.Object ? this.m_Object.Current : new KeyValuePair<string, JSONNode>(string.Empty, (JSONNode) null);
        }
      }

      public bool MoveNext()
      {
        if (this.type == JSONNode.Enumerator.Type.Array)
          return this.m_Array.MoveNext();
        return this.type == JSONNode.Enumerator.Type.Object && this.m_Object.MoveNext();
      }

      private enum Type
      {
        None,
        Array,
        Object,
      }
    }

    public struct ValueEnumerator
    {
      private JSONNode.Enumerator m_Enumerator;

      public ValueEnumerator(List<JSONNode>.Enumerator aArrayEnum)
        : this(new JSONNode.Enumerator(aArrayEnum))
      {
      }

      public ValueEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
        : this(new JSONNode.Enumerator(aDictEnum))
      {
      }

      public ValueEnumerator(JSONNode.Enumerator aEnumerator) => this.m_Enumerator = aEnumerator;

      public JSONNode Current => this.m_Enumerator.Current.Value;

      public bool MoveNext() => this.m_Enumerator.MoveNext();

      public JSONNode.ValueEnumerator GetEnumerator() => this;
    }

    public struct KeyEnumerator
    {
      private JSONNode.Enumerator m_Enumerator;

      public KeyEnumerator(List<JSONNode>.Enumerator aArrayEnum)
        : this(new JSONNode.Enumerator(aArrayEnum))
      {
      }

      public KeyEnumerator(Dictionary<string, JSONNode>.Enumerator aDictEnum)
        : this(new JSONNode.Enumerator(aDictEnum))
      {
      }

      public KeyEnumerator(JSONNode.Enumerator aEnumerator) => this.m_Enumerator = aEnumerator;

      public string Current => this.m_Enumerator.Current.Key;

      public bool MoveNext() => this.m_Enumerator.MoveNext();

      public JSONNode.KeyEnumerator GetEnumerator() => this;
    }

    public class LinqEnumerator : 
      IEnumerator<KeyValuePair<string, JSONNode>>,
      IDisposable,
      IEnumerator,
      IEnumerable<KeyValuePair<string, JSONNode>>,
      IEnumerable
    {
      private JSONNode m_Node;
      private JSONNode.Enumerator m_Enumerator;

      internal LinqEnumerator(JSONNode aNode)
      {
        this.m_Node = aNode;
        if (!(this.m_Node != (object) null))
          return;
        this.m_Enumerator = this.m_Node.GetEnumerator();
      }

      public KeyValuePair<string, JSONNode> Current => this.m_Enumerator.Current;

      object IEnumerator.Current => (object) this.m_Enumerator.Current;

      public bool MoveNext() => this.m_Enumerator.MoveNext();

      public void Dispose()
      {
        this.m_Node = (JSONNode) null;
        this.m_Enumerator = new JSONNode.Enumerator();
      }

      public IEnumerator<KeyValuePair<string, JSONNode>> GetEnumerator() => (IEnumerator<KeyValuePair<string, JSONNode>>) new JSONNode.LinqEnumerator(this.m_Node);

      public void Reset()
      {
        if (!(this.m_Node != (object) null))
          return;
        this.m_Enumerator = this.m_Node.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new JSONNode.LinqEnumerator(this.m_Node);
    }
  }
}
