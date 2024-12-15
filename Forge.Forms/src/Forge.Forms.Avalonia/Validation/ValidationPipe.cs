using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Forge.Forms.AvaloniaUI.Validation;

public sealed class ValidationPipe /*: ValidationAttribute*/
{
    public string Error { get; set; }

    public ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var error = Error;
        if (error != null)
        {
            Error = null;
            return new ValidationResult(error);
        }

        return ValidationResult.Success;
    }
}