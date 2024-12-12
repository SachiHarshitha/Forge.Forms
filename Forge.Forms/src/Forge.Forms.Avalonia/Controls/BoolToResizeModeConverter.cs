using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.Controls;

internal class BoolToResizeModeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        //return value is bool && (bool)value ? Resize.CanResize : ResizeMode.CanMinimize;
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}