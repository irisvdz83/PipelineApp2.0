using System.Diagnostics.CodeAnalysis;

namespace PipelineService.Utilities.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Executes an action on each element of the list.
    /// This is a "fluent interface" (dotted syntax) version of foreach
    /// </summary>
    /// <typeparam name="TValue">The type of elements in the list being operated on.</typeparam>
    /// <param name="list">The list of elements to iterate on.</param>
    /// <param name="action">The action to perform on each element in the list.</param>
    public static void ForEach<TValue>(this IEnumerable<TValue> list, Action<TValue> action)
    {
        list.ForEach((n, item) => action(item));
    }

    /// <summary>
    /// Executes an action on each element of the list. Action gets an item from the list together with the index of that item
    /// </summary>
    /// <typeparam name="TValue">The type of elements in the list being operated on.</typeparam>
    /// <param name="list">The list of elements to iterate on.</param>
    /// <param name="action">The action to perform on each element in the list but given the index of the element as well.</param>
    public static void ForEach<TValue>(this IEnumerable<TValue> list, Action<int, TValue> action)
    {
        int counter = 0;
        foreach (var item in list)
        {
            action(counter, item);
            counter++;
        }
    }

    /// <summary>
    /// Returns true if list contains no elements.
    /// </summary>
    public static bool IsEmpty<T>(this IEnumerable<T> list)
    {
        return !list.Any();
    }

    /// <summary>
    /// Returns true if the list is null or contains no elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="list">THe list being validated.</param>
    /// <returns>Returns true if the list is null or contains no elements.</returns>
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? list)
    {
        return list == null || !list.Any();
    }

    /// <summary>
    /// Returns true if list contains no elements for which the predicate is true.
    /// </summary>
    public static bool None<T>(this IEnumerable<T> list, Predicate<T> predicate)
    {
        return !list.Any(t => predicate(t));
    }

    /// <summary>
    /// Returns true if list contains no elements for which the predicate is true.
    /// </summary>
    public static bool None<T>(this IEnumerable<T> list)
    {
        return !list.Any();
    }

    public static bool Multiple<TValue>(this IEnumerable<TValue> list)
    {
        return list.Count() > 1;
    }

    /// <summary>
    /// Searches for the specified object and returns the zero-based indexes of all occurrences within the range of elements in the IEnumerable<T>.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="list"></param>
    /// <param name="predicate"></param>
    /// <returns>Indexes of matching entries</returns>
    public static IEnumerable<int> IndexesOf<TValue>(this IEnumerable<TValue> list, Func<TValue, bool> predicate)
    {
        var i = 0;
        foreach (var value in list)
        {
            if (predicate.Invoke(value))
            {
                yield return i;
            }

            i++;
        }
    }

    /// <summary>
    /// Returns true if list contains at least one element.
    /// </summary>
    public static bool IsNotEmpty<T>(this IEnumerable<T> list)
    {
        return list.Any();
    }

    /// <summary>
    /// Returns true if list contains at least one element or is not null.
    /// </summary>
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T>? list)
    {
        return list is not null && list.Any();
    }

    /// <summary>
    /// Returns true if the list contains at least one element for which the predicate is true
    /// </summary>
    public static bool Contains<T>(this IEnumerable<T> list, Func<T, bool> predicate)
    {
        return list.Any(predicate);
    }

    /// <summary>
    /// Evaluates the IEnumerable (performing any pending deferred actions).
    /// Does nothing if the IEnumerable is a List or an Array 
    /// </summary>
    public static IEnumerable<T> Evaluate<T>(this IEnumerable<T> elements)
    {
        if (elements is List<T>) return elements;
        if (elements is T[]) return elements;
        return elements.ToList();
    }

    /// <summary>
    /// Returns pairs of values that appear next to each other in the original list
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumeration.</typeparam>
    /// <param name="list">THe given enumeration.</param>
    /// <returns>An enumeration of pairs that lie adjacent to each other in the given enumeration.</returns>
    public static IEnumerable<Tuple<T, T>> SelectAdjacentPairs<T>(this IEnumerable<T> list)
    {
        using var enumerator = list.GetEnumerator();
        if (enumerator.MoveNext())
        {
            T prev = enumerator.Current;
            while (enumerator.MoveNext())
            {
                yield return Tuple.Create(prev, enumerator.Current);
                prev = enumerator.Current;
            }
        }
    }

    /// <summary>
    /// Returns all possible combinations of 2 values from the list 
    /// (order is not important, i.e. {A,B} = {B,A} and only one of them is returned)
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumeration.</typeparam>
    /// <param name="elements">The enumerable of elements.</param>
    /// <returns>An enumeration of all possible combinations of two elements in the original enumerable.</returns>
    public static IEnumerable<Tuple<T, T>> SelectAllCombinations<T>(this IEnumerable<T> elements)
    {
        List<T> list = elements.ToList();
        for (int a = 0; a < list.Count - 1; a++)
        {
            for (int b = a + 1; b < list.Count; b++)
            {
                yield return Tuple.Create(list[a], list[b]);
            }
        }
    }

    /// <summary>
    /// Returns all possible combinations of 2 lists 
    /// SelectAllCombinations({1,2}, {A,B}) will return {1A, 1B, 2A, 2B}.
    /// </summary>
    /// <typeparam name="T1">The type of elements in the first enumerable.</typeparam>
    /// <typeparam name="T2">The type of elements in the second enumerable.</typeparam>
    /// <param name="list1">The first enumerable.</param>
    /// <param name="list2">The second enumerable.</param>
    /// <returns>An enumeration of all combinations of elements between the two enumerables.</returns>
    public static IEnumerable<Tuple<T1, T2>> SelectAllCombinations<T1, T2>(this IEnumerable<T1> list1, IEnumerable<T2> list2)
    {
        return list1.SelectMany(x => list2, Tuple.Create);
    }

    /// <summary>
    /// Gets a slice out of the IEnumerable. Starting from the item at position [start] and returning [length] items.
    /// </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="start">The start.</param>
    /// <param name="length">The length.</param>
    /// <returns>The sliced enumerable.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the start or length are negative.</exception>
    public static IEnumerable<TValue> GetSlice<TValue>(this IEnumerable<TValue> list, long start, long length)
    {
        if (start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start), "Parameter start must be 0 or greater.");
        }
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "Parameter length must be 1 or greater");
        }

        return GetSliceIterator(list, start, length);
    }

    private static IEnumerable<TValue> GetSliceIterator<TValue>(IEnumerable<TValue> list, long start, long length)
    {
        using var enumerator = list.GetEnumerator();
        var end = start + length - 1;
        var cnt = 0;
        while (enumerator.MoveNext())
        {
            if (cnt >= start && cnt <= end)
            {
                yield return enumerator.Current;
            }
            cnt++;
        }
    }

    /// <summary>
    /// Divides the list in chunks of (at most) sliceSize elements. The last chunk can contain fewer elements
    /// </summary>
    public static IEnumerable<IList<TValue>> Slice<TValue>(this IEnumerable<TValue> list, long sliceSize)
    {
        List<TValue> collector;
        using (var enumerator = list.GetEnumerator())
        {
            var cnt = 0;
            collector = new List<TValue>();
            while (enumerator.MoveNext())
            {
                collector.Add(enumerator.Current);
                cnt++;
                if (cnt >= sliceSize)
                {
                    yield return collector;
                    collector = new List<TValue>();
                    cnt = 0;
                }
            }
        }

        if (collector.Count != 0)
        {
            yield return collector;
        }
    }

    /// <summary>
    /// Divides the list in chunks using the delimiter value
    /// </summary>
    /// <typeparam name="TValue">The type of elements in the enumerable</typeparam>
    /// <param name="list">The source enumerable.</param>
    /// <param name="delimiter">The delimiter used for separating the enumerable in separate lists.</param>
    /// <returns>An enumerable of lists of elements that are divided by the delimiter.</returns>
    public static IEnumerable<IList<TValue>> Split<TValue>(this IEnumerable<TValue> list, TValue delimiter)
    {
        List<TValue> collector;
        using (var enumerator = list.GetEnumerator())
        {
            collector = new List<TValue>();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current != null && enumerator.Current.Equals(delimiter))
                {
                    yield return collector;
                    collector = new List<TValue>();
                }
                else
                {
                    collector.Add(enumerator.Current);
                }
            }
        }

        if (collector.Count != 0)
        {
            yield return collector;
        }
    }

    /// <summary>
    /// The enumerable is sliced in equal portions using the sliceSize
    /// </summary>
    /// <typeparam name="TValue">The type of elements in the enumerable.</typeparam>
    /// <param name="list">The source enumerable.</param>
    /// <param name="sliceSize">The size of slices to take each time.</param>
    /// <returns>An enumerable of inner enumerable with sliceSize as size (except the last one)</returns>
    public static IEnumerable<IEnumerable<TValue>> SliceAsEnumerable<TValue>(this IEnumerable<TValue> list, long sliceSize)
    {
        using var enumerator = list.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return SlicePart(enumerator, sliceSize);
        }
    }

    private static IEnumerable<TValue> SlicePart<TValue>(IEnumerator<TValue> enumerator, long sliceSize)
    {
        var cnt = 0;
        while (cnt == 0 || (cnt < sliceSize && enumerator.MoveNext()))
        {
            yield return enumerator.Current;
            cnt++;
        }
    }

    /// <summary>
    /// Divides the enumerable in chunks using the delimiter value
    /// </summary>
    /// <typeparam name="TValue">The type of elements in the enumerable</typeparam>
    /// <param name="list">The source enumerable.</param>
    /// <param name="delimiter">The delimiter used for separating the enumerable in separate lists.</param>
    /// <returns>An enumerable of lists of elements that are divided by the delimiter.</returns>
    public static IEnumerable<IEnumerable<TValue>> SplitAsEnumerable<TValue>(this IEnumerable<TValue> list, TValue delimiter)
    {
        using var enumerator = list.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return SplitPart(enumerator, delimiter);
        }
    }

    private static IEnumerable<TValue> SplitPart<TValue>(IEnumerator<TValue> enumerator, TValue delimiter)
    {
        bool first = true;
        while (first || enumerator.MoveNext())
        {
            first = false;
            if (enumerator.Current != null && enumerator.Current.Equals(delimiter))
            {
                yield break;
            }
            yield return enumerator.Current;
        }
    }

    /// <summary>
    /// Checks if list contains more than count elements.
    /// Does not enumerate the whole list, as opposed to using IEnumerable.Count()
    /// </summary>
    public static bool CountIsGreaterThan<T>(this IEnumerable<T> list, int count)
    {
        return list.Take(count + 1).Count() == count + 1;
    }

    /// <summary>
    /// Checks if list contains fewer than count elements.
    /// Does not enumerate the whole list, as opposed to using IEnumerable.Count()
    /// </summary>
    public static bool CountIsLessThan<T>(this IEnumerable<T> list, int count)
    {
        return list.Take(count).Count() < count;
    }

    /// <summary>
    /// Returns true if the two list contain the same elements (not necessarily in the same order)
    /// (e.g. {1,2} is equivalent to {2,1}), but {1,1,2} is NOT equivalent to {1,2,2}) 
    /// </summary>
    public static bool SequenceEquivalent<T>(this IEnumerable<T>? left, IEnumerable<T>? right)
    {
        return SequenceEquivalent(left, right, EqualityComparer<T>.Default);
    }

    public static bool SequenceEquivalent<T>(this IEnumerable<T>? left, IEnumerable<T>? right, IEqualityComparer<T> comparer)
    {
        if (ReferenceEquals(left, right)) return true;
        if (ReferenceEquals(left, null)) return ReferenceEquals(right, null);
        if (ReferenceEquals(right, null)) return false;

        var leftList = left as IList<T> ?? left.ToList();
        var rightList = right as IList<T> ?? right.ToList();

        if (leftList.Count != rightList.Count) return false;

        ILookup<T, T> leftLookup = leftList.ToLookup(l => l, comparer);
        return rightList
            .GroupBy(r => r)
            .All(g => leftLookup.Contains(g.Key) && leftLookup[g.Key].Count() == g.Count());
    }

    /// <summary>
    /// Flattens a tree-structured list. Returns parents and children, and their children, and ... in one flat list
    /// </summary>
    /// <typeparam name="T">Elements in the enumerable that may have children.</typeparam>
    /// <param name="structuredList">The source enumerable of root elements.</param>
    /// <param name="getChildren">A function that returns the children of an element in the tree.</param>
    /// <returns>An enumerable of elements retrieved from the tree ordered from top to bottom and left to right</returns>
    public static IEnumerable<T> Flatten<T>(this IEnumerable<T>? structuredList, Func<T, IEnumerable<T>?> getChildren)
    {
        if (structuredList == null) return Array.Empty<T>();

        return structuredList.SelectMany(item =>
        {
            var children = getChildren(item);
            if (children == null)
            {
                return new[] { item };
            }
            return children.Flatten(getChildren).Prepend(item);
        });
    }

    public static string ToDisplayString<T>(this IEnumerable<T> sequence)
    {
        return $"[{string.Join(",", sequence.Select(x => (x == null) ? "null" : x.ToString()).ToArray())}]";
    }

    public static bool CaseInsensitiveContains(this IEnumerable<string>? list, string value)
    {
        return list != null && list.Contains(value, StringComparer.InvariantCultureIgnoreCase);
    }
}
