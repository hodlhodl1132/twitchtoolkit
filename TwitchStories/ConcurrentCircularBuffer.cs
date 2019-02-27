using System;
using System.Linq;
using System.Collections.Generic;

namespace TwitchStories
{
  public class ConcurrentCircularBuffer<T>
  {
    readonly LinkedList<T> _buffer;
    int _maxItemCount;

    public ConcurrentCircularBuffer(int maxItemCount)
    {
      _maxItemCount = maxItemCount;
      _buffer = new LinkedList<T>();
    }

    public void Put(T item)
    {
      lock (_buffer)
      {
        _buffer.AddFirst(item);
        if (_buffer.Count > _maxItemCount)
        {
          _buffer.RemoveLast();
        }
      }
    }

    public T[] Read()
    {
      lock (_buffer) 
      { 
        return _buffer.ToArray(); 
      }
    }

    public void Clear()
    {
      lock (_buffer)
      {
        _buffer.Clear();
      }
    }
  }
}
