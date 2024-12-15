﻿using System.Globalization;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.Validation;

public class EnforcedValidator : FieldValidator
{
    public EnforcedValidator(
        ValidationPipe pipe,
        IErrorStringProvider errorProvider,
        IBoolProxy isEnforced,
        IValueConverter valueConverter,
        bool strictValidation,
        bool validatesOnTargetUpdated,
        bool ignoreNullOrEmpty)
        : base(
            pipe,
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
        return false;
    }
}