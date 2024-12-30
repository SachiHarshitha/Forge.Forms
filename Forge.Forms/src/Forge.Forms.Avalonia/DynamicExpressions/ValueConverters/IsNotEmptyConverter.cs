using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions.ValueConverters;

public class IsNotEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case string s:
                return !string.IsNullOrEmpty(s);
            case IEnumerable<object> e:
                return e.Any();
            case Enum en:
                if (Enum.IsDefined(value.GetType(), value))
                    return !en.Equals(value);
                return false;
            default:
                return false;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}