using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Forge.Forms.AvaloniaUI.Validation;

public sealed class ConversionValidator /*: ValidationRule*/
{
    private readonly Func<string, CultureInfo, object> deserializer;
    private readonly IErrorStringProvider errorProvider;
    private readonly CultureInfo overrideCulture;

    public ConversionValidator(Func<string, CultureInfo, object> deserializer, IErrorStringProvider errorProvider,
            CultureInfo overrideCulture)
        // : base(ValidationStep.RawProposedValue, false)
    {
        this.deserializer = deserializer;
        this.errorProvider = errorProvider;
        this.overrideCulture = overrideCulture;
    }

    public ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        try
        {
            deserializer(value as string, overrideCulture ?? cultureInfo);
            return ValidationResult.Success;
        }
        catch
        {
            return new ValidationResult(errorProvider.GetErrorMessage(value));
        }
    }
}