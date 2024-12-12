using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions.ValueConverters;

public class AsBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}