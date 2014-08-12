using System;

namespace prep.ranges
{
  public interface IContainValues<T> where T : IComparable<T>
  {
    bool contains(T value);
  }
}