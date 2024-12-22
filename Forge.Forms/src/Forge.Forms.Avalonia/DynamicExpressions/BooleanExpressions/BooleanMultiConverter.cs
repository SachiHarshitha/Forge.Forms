using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions.BooleanExpressions;

internal class BooleanMultiConverter : IMultiValueConverter
{
    public BooleanMultiConverter(BooleanExpression expression, IValueConverter innerConverter)
    {
        Expression = expression;
        Converter = innerConverter;
    }

    public BooleanExpression Expression { get; }

    public IValueConverter Converter { get; }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        var boolResult = Expression.Evaluate(values.Select(c => c is true).ToArray());
        object result = boolResult;
        if (Converter != null) result = Converter.Convert(boolResult, targetType, parameter, culture);

        return result;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return new object[0];
    }

    private static IValueConverter GetValueConverter(IResourceContext context, string valueConverter)
    {
        if (string.IsNullOrEmpty(valueConverter)) return null;

        if (Resource.ValueConverters.TryGetValue(valueConverter, out var c)) return c;

        if (context != null && context.TryFindResource(valueConverter) is IValueConverter converter) return converter;

        throw new InvalidOperationException($"Value converter '{valueConverter}' not found.");
    }
}