using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace NoterApp.Converters;

public class IsEqualConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values == null || values.Count < 2 || values.Any(v => v == null))
        {
            return false;
        }

        return ReferenceEquals(values[0], values[1]);
    }
}