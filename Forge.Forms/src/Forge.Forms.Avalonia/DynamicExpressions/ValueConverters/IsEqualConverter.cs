using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions.ValueConverters;

public class IsEqualConverter : IValueConverter
{
    public IsEqualConverter(object value)
    {
        Value = value;
    }

    public object Value { get; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Equals(Value, value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}