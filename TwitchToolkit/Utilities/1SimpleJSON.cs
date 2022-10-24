// Decompiled with JetBrains decompiler
// Type: SimpleJSON.JSONArray
// Assembly: TwitchToolkit, Version=2.0.10.0, Culture=neutral, PublicKeyToken=null
// MVID: EDBA5B80-2283-4D46-98DF-2FAD203695A7
// Assembly location: C:\Users\Kirito\Downloads\steamcmd\steamapps\workshop\content\294100\1718525787\v1.3\Assemblies\TwitchToolkit.dll

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SimpleJSON
{
  public class JSONArray : JSONNode
  {
    private List<JSONNode> m_List = new List<JSONNode>();
    private bool inline = false;

    public override bool Inline
    {
      get => this.inline;
      set => this.inline = value;
    }

    public override JSONNodeType Tag => JSONNodeType.Array;

    public override bool IsArray => true;

    public override JSONNode.Enumerator GetEnumerator() => new JSONNode.Enumerator(this.m_List.GetEnumerator());

    public override JSONNode this[int aIndex]
    {
      get => aIndex < 0 || aIndex >= this.m_List.Count ? (JSONNode) new JSONLazyCreator((JSONNode) this) : this.m_List[aIndex];
      set
      {
        if (value == (object) null)
          value = (JSONNode) JSONNull.CreateOrGet();
        if (aIndex < 0 || aIndex >= this.m_List.Count)
          this.m_List.Add(value);
        else
          this.m_List[aIndex] = value;
      }
    }

    public override JSONNode this[string aKey]
    {
      get => (JSONNode) new JSONLazyCreator((JSONNode) this);
      set
      {
        if (value == (object) null)
          value = (JSONNode) JSONNull.CreateOrGet();
        this.m_List.Add(value);
      }
    }

    public override int Count => this.m_List.Count;

    public override void Add(string aKey, JSONNode aItem)
    {
      if (aItem == (object) null)
        aItem = (JSONNode) JSONNull.CreateOrGet();
      this.m_List.Add(aItem);
    }

    public override JSONNode Remove(int aIndex)
    {
      if (aIndex < 0 || aIndex >= this.m_List.Count)
        return (JSONNode) null;
      JSONNode jsonNode = this.m_List[aIndex];
      this.m_List.RemoveAt(aIndex);
      return jsonNode;
    }

    public override JSONNode Remove(JSONNode aNode)
    {
      this.m_List.Remove(aNode);
      return aNode;
    }

    public override IEnumerable<JSONNode> Children
    {
      get
      {
        foreach (JSONNode N in this.m_List)
          yield return N;
      }
    }

    internal override void WriteToStringBuilder(
      StringBuilder aSB,
      int aIndent,
      int aIndentInc,
      JSONTextMode aMode)
    {
      aSB.Append('[');
      int count = this.m_List.Count;
      if (this.inline)
        aMode = JSONTextMode.Compact;
      for (int index = 0; index < count; ++index)
      {
        if (index > 0)
          aSB.Append(',');
        if (aMode == JSONTextMode.Indent)
          aSB.AppendLine();
        if (aMode == JSONTextMode.Indent)
          aSB.Append(' ', aIndent + aIndentInc);
        this.m_List[index].WriteToStringBuilder(aSB, aIndent + aIndentInc, aIndentInc, aMode);
      }
      if (aMode == JSONTextMode.Indent)
        aSB.AppendLine().Append(' ', aIndent);
      aSB.Append(']');
    }

    public override void SerializeBinary(BinaryWriter aWriter)
    {
      aWriter.Write((byte) 1);
      aWriter.Write(this.m_List.Count);
      for (int index = 0; index < this.m_List.Count; ++index)
        this.m_List[index].SerializeBinary(aWriter);
    }
  }
}
