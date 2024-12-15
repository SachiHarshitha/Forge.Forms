﻿using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.Validation;

public class MatchPatternValidator : ComparisonValidator
{
    public MatchPatternValidator(
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
        if (!(Argument.Value is string pattern)) return true;

        if (value is string s) return Regex.IsMatch(s, pattern);

        return false;
    }
}