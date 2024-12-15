using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI.Validation.Helpers;

namespace Forge.Forms.AvaloniaUI.Validation
{
    public sealed class ValidationPipe /*: ValidationAttribute*/
    {
        public ValidationPipe()
            // : base(ValidationStep.CommittedValue, true)
        {
        }

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
}
