using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public class StringTypeConverter : IValueConverter
{
    private readonly Func<string, CultureInfo, object> deserializer;

    public StringTypeConverter(Func<string, CultureInfo, object> deserializer)
    {
        this.deserializer = deserializer;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            return deserializer(value as string, culture);
        }
        catch
        {
            return AvaloniaProperty.UnsetValue;
        }
    }
}