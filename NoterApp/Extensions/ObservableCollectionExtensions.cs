using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NoterApp.Extensions;

public static class ObservableCollectionExtensions
{
    public static void Sort<TSource, TKey>(this ObservableCollection<TSource> collection,
        Func<TSource, TKey> keySelector, bool descending = false)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));

        List<TSource> Sorted;
        if (descending)
        {
            Sorted = collection.OrderByDescending(keySelector).ToList();
        }
        else
        {
            Sorted = collection.OrderBy(keySelector).ToList();
        }

        for (int i = 0; i < Sorted.Count; i++)
        {
            int oldIndex = collection.IndexOf(Sorted[i]);

            if (oldIndex != i)
            {
                collection.Move(oldIndex, i);
            }
        }
    }
}