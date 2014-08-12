using System;
using System.Collections.Generic;
using prep.matching;
using prep.matching_core;

namespace prep.extensions
{
  public static class EnumerableExtensions
  {
    public static IEnumerable<T> one_at_a_time<T>(this IEnumerable<T> items)
    {
      foreach (var item in items)
        yield return item;
    }

    public static IEnumerable<T> filter<T>(this IEnumerable<T> items, Predicate<T> predicate)
    {
      foreach (var item in items)
      {
        if (predicate(item))
          yield return item;
      }
    }

    public static IEnumerable<T> sort<T>(this IEnumerable<T> items, IComparer<T> comparer)
    {
      var sorted = new List<T>(items);
      sorted.Sort(comparer);
      return sorted;
    }

    public static IEnumerable<T> filter<T>(this IEnumerable<T> items, IMatchA<T> specification)
    {
      return items.filter(specification.matches);
    }

    public static FilteringExtensionPoint<ItemType, AttributeType> where<ItemType, AttributeType>(
      this IEnumerable<ItemType> items, IGetTheValueOfAnAttribute<ItemType, AttributeType> accessor)
    {
      return new FilteringExtensionPoint<ItemType, AttributeType>(Match<ItemType>.with_attribute(accessor),
        items);
    }
  }
}