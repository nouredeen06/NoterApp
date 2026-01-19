using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using NoterApp.Models;
using NoterApp.Services;

namespace NoterApp.Converters;

public class TagToHexConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (string.IsNullOrEmpty(value?.ToString())) return null;
        if (value is string tag)
        {
            var colorsLookup = AppColors.DarkHexLookup;
            var tagsLookup = DataService.Instance.GetAllTags().ToDictionary(Tag => Tag.Name, Tag => Tag.ColorName);

            return colorsLookup[tagsLookup[tag]];
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}