using System;
using prep.matching;
using prep.matching_core;

namespace prep.ranges
{
  public static class Range
  {
    public static IStartContain<AttributeType> start_from<AttributeType>(AttributeType start)
      where AttributeType : IComparable<AttributeType>
    {
      return new StartFromContain<AttributeType>(start);
    }

    public static EndAtContain<AttributeType> ends_at<AttributeType>(this IStartContain<AttributeType> containValue,
      AttributeType end) where AttributeType : IComparable<AttributeType>
    {
      return new EndAtContain<AttributeType>(containValue, end);
    }

    public static EndAtContain<AttributeType> ends_at<AttributeType>(AttributeType end)
      where AttributeType : IComparable<AttributeType>
    {
      return new EndAtContain<AttributeType>(end);
    }
  }

  public interface IStartContain<AttributeType> : IContainValues<AttributeType>
    where AttributeType : IComparable<AttributeType>
  {
    IStartContain<AttributeType> inclusive { get; }
  }

  class StartFromContain<AttributeType> : IContainValues<AttributeType>, IStartContain<AttributeType>
    where AttributeType : IComparable<AttributeType>
  {
    IMatchA<AttributeType> matcher;
    readonly IMatchA<AttributeType> equalTo;

    public StartFromContain(AttributeType start)
    {
      var accessor = Match<AttributeType>.with_attribute(any => any);
      equalTo = accessor.equal_to(start);
      matcher = accessor.greater_than(start);
    }

    public bool contains(AttributeType value)
    {
      return matcher.matches(value);
    }

    public IStartContain<AttributeType> inclusive
    {
      get
      {
        matcher = matcher.or(equalTo);
        return this;
      }
    }
  }

  public class EndAtContain<AttributeType> : IContainValues<AttributeType>
    where AttributeType : IComparable<AttributeType>
  {
    readonly IContainValues<AttributeType> containValue;
    IMatchA<AttributeType> matcher;
    readonly IMatchA<AttributeType> equalTo;

    public EndAtContain(IContainValues<AttributeType> containValue, AttributeType end)
    {
      this.containValue = containValue;
      var accessor = Match<AttributeType>.with_attribute(any => any);
      equalTo = accessor.equal_to(end);
      matcher = accessor.less_than(end);
    }

    public EndAtContain(AttributeType end)
      : this(new DummyContain(), end)
    {
    }

    class DummyContain : IContainValues<AttributeType>
    {
      public bool contains(AttributeType value)
      {
        return true;
      }

      public IContainValues<AttributeType> inclusive
      {
        get { throw new NotImplementedException(); }
      }
    }

    public bool contains(AttributeType value)
    {
      return containValue.contains(value) && matcher.matches(value);
    }

    public IContainValues<AttributeType> inclusive
    {
      get
      {
        matcher = matcher.or(equalTo);
        return this;
      }
    }
  }
}