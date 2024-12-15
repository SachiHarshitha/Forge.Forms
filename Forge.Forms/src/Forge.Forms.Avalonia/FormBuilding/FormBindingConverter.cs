using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public class FormBindingConverter : IValueConverter
{
    public static readonly FormBindingConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is IBindingProvider field && parameter is string fieldName)
        {
            var providedValue = field.ProvideValue(fieldName);
            return providedValue;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}