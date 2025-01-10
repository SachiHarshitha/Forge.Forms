using System;
using System.Globalization;
using System.Linq;
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
            if (fieldName.Contains("."))
            {
                var formValue = field.ProvideValue("Value") as Binding;
                if (formValue == null) return null;

                return new Binding
                {
                    Source = formValue.Source,
                    Mode = formValue.Mode,
                    Converter = formValue.Converter,
                    ConverterParameter = formValue.ConverterParameter,
                    Priority = formValue.Priority,
                    Path = formValue.Path + "." + fieldName.Split(".").Last()
                };
            }

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