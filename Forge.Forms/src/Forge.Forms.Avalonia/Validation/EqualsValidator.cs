﻿using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.Validation;

internal class EqualsValidator : ComparisonValidator
{
    public EqualsValidator(
        ValidationPipe pipe,
        IProxy argument,
        IErrorStringProvider errorProvider,
        IBoolProxy isEnforced,
        IValueConverter valueConverter,
        bool strictValidation,
        bool validatesOnTargetUpdated,
        bool ignoreNullOrEmpty)
        : base(
            pipe,
            argument,
            errorProvider,
            isEnforced,
            valueConverter,
            strictValidation,
            validatesOnTargetUpdated,
            ignoreNullOrEmpty)
    {
    }

    protected override bool ValidateValue(object value, CultureInfo cultureInfo)
    {
        var comparand = Argument.Value;
        if (value != null && comparand is IConvertible && value.GetType() != comparand.GetType())
            comparand = Convert.ChangeType(comparand, value.GetType(), CultureInfo.InvariantCulture);

        return Equals(comparand, value);
    }
}