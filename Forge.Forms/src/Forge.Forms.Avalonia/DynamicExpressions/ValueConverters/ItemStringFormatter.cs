using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions.ValueConverters;

public class ItemStringFormatter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null) return null;

        if (parameter is null) return value;

        if (parameter is string format) return string.Format(format, value);

        if (parameter is Binding binding)
        {
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}