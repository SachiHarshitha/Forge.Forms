﻿using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions.ValueConverters;

internal class ConverterWrapper : IValueConverter
{
    private readonly IValueConverter innerConverter;
    private readonly IValueConverter outerConverter;

    public ConverterWrapper(IValueConverter outerConverter, IValueConverter innerConverter)
    {
        this.outerConverter = outerConverter;
        this.innerConverter = innerConverter;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var inner = innerConverter.Convert(value, targetType, parameter, culture);
        return outerConverter.Convert(inner, targetType, parameter, culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return BindingOperations.DoNothing;
    }
}