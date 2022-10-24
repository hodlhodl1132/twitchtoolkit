// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONObject
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SimpleJSON
{
  public class JSONObject : JSONNode
  {
    private Dictionary<string, JSONNode> m_Dict = new Dictionary<string, JSONNode>();
    private bool inline = false;

    public override bool Inline
    {
      get => this.inline;
      set => this.inline = value;
    }

    public override JSONNodeType Tag => JSONNodeType.Object;

    public override bool IsObject => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator(this.m_Dict.GetEnumerator());

    public override JSONNode this[string aKey]
    {
      get => this.m_Dict.ContainsKey(aKey) ? this.m_Dict[aKey] : (JSONNode) new JSONLazyCreator((JSONNode) this, aKey);
      set
      {
        if (value == (object) null)
          value = (JSONNode) JSONNull.CreateOrGet();
        if (this.m_Dict.ContainsKey(aKey))
          this.m_Dict[aKey] = value;
        else
          this.m_Dict.Add(aKey, value);
      }
    }

    public override JSONNode this[int aIndex]
    {
      get => aIndex < 0 || aIndex >= this.m_Dict.Count ? (JSONNode) null : this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Value;
      set
      {
        if (value == (object) null)
          value = (JSONNode) JSONNull.CreateOrGet();
        if (aIndex < 0 || aIndex >= this.m_Dict.Count)
          return;
        this.m_Dict[this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex).Key] = value;
      }
    }

    public override int Count => this.m_Dict.Count;

    public override void Add(string aKey, JSONNode aItem)
    {
      if (aItem == (object) null)
        aItem = (JSONNode) JSONNull.CreateOrGet();
      if (!string.IsNullOrEmpty(aKey))
      {
        if (this.m_Dict.ContainsKey(aKey))
          this.m_Dict[aKey] = aItem;
        else
          this.m_Dict.Add(aKey, aItem);
      }
      else
        this.m_Dict.Add(Guid.NewGuid().ToString(), aItem);
    }

    public override JSONNode Remove(string aKey)
    {
      if (!this.m_Dict.ContainsKey(aKey))
        return (JSONNode) null;
      JSONNode jsonNode = this.m_Dict[aKey];
      this.m_Dict.Remove(aKey);
      return jsonNode;
    }

    public override JSONNode Remove(int aIndex)
    {
      if (aIndex < 0 || aIndex >= this.m_Dict.Count)
        return (JSONNode) null;
      KeyValuePair<string, JSONNode> keyValuePair = this.m_Dict.ElementAt<KeyValuePair<string, JSONNode>>(aIndex);
      this.m_Dict.Remove(keyValuePair.Key);
      return keyValuePair.Value;
    }

    public override JSONNode Remove(JSONNode aNode)
    {
      try
      {
        this.m_Dict.Remove(this.m_Dict.Where<KeyValuePair<string, JSONNode>>((Func<KeyValuePair<string, JSONNode>, bool>) (k => k.Value == (object) aNode)).First<KeyValuePair<string, JSONNode>>().Key);
        return aNode;
      }
      catch
      {
        return (JSONNode) null;
      }
    }

    public override bool HasKey(string aKey) => this.m_Dict.ContainsKey(aKey);

    public override JSONNode GetValueOrDefault(string aKey, JSONNode aDefault)
    {
      JSONNode jsonNode;
      return this.m_Dict.TryGetValue(aKey, out jsonNode) ? jsonNode : aDefault;
    }

    public override IEnumerable<JSONNode> Children
    {
      get
      {
        foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
        {
          KeyValuePair<string, JSONNode> N = keyValuePair;
          yield return N.Value;
          N = new KeyValuePair<string, JSONNode>();
        }
      }
    }

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append('{');
      bool flag = true;
      if (this.inline)
        aMode = JSONTextMode.Compact;
      foreach (KeyValuePair<string, JSONNode> keyValuePair in this.m_Dict)
      {
        if (!flag)
          aSB.Append(',');
        flag = false;
        if (aMode == JSONTextMode.Indent)
          aSB.AppendLine();
        if (aMode == JSONTextMode.Indent)
          aSB.Append(' ', aIndent + aIndentInc);
        aSB.Append('"').Append(JSONNode.Escape(keyValuePair.Key)).Append('"');
        if (aMode == JSONTextMode.Compact)
          aSB.Append(':');
        else
          aSB.Append(" : ");
        keyValuePair.Value.WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
      }
      if (aMode == JSONTextMode.Indent)
        aSB.AppendLine().Append(' ', aIndent);
      aSB.Append('}');
    }

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 2);
      aWriter.Write(this.m_Dict.Count);
      foreach (string key in this.m_Dict.Keys)
      {
        aWriter.Write(key);
        this.m_Dict[key].SerializeBinary(aWriter);
      }
    }
  }
}
