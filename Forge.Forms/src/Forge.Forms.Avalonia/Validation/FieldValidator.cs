using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;
using ReactiveUI.Validation.Helpers;

namespace Forge.Forms.AvaloniaUI.Validation
{
    public abstract class FieldValidator 
    {
        protected FieldValidator(
            ValidationPipe pipe,
            IErrorStringProvider errorProvider,
            IBoolProxy isEnforced,
            IValueConverter valueConverter,
            bool strictValidation,
            bool validatesOnTargetUpdated,
            bool ignoreNullOrEmpty)
            // : base(ValidationStep.ConvertedProposedValue, validatesOnTargetUpdated)
        {
            ValidationPipe = pipe;
            ErrorProvider = errorProvider;
            ValueConverter = valueConverter;
            IsEnforced = isEnforced;
            StrictValidation = strictValidation;
            IgnoreNullOrEmpty = ignoreNullOrEmpty;
        }

        public IValueConverter ValueConverter { get; }

        public IErrorStringProvider ErrorProvider { get; }

        public IBoolProxy IsEnforced { get; }

        public ValidationPipe ValidationPipe { get; }

        public bool StrictValidation { get; }

        public bool IgnoreNullOrEmpty { get; }

        public  ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (ValidationPipe != null)
            {
                // Pass if another validator has already reported an error.
                if (ValidationPipe.Error != null)
                {
                    return ValidationResult.Success;
                }

                // Optionally ignore null/empty values.
                if (IgnoreNullOrEmpty && (value == null || value is ""))
                {
                    return ValidationResult.Success;
                }

                // Checking this later might be a tiny bit faster.
                if (!IsEnforced.Value)
                {
                    return ValidationResult.Success;
                }

                var isValid = ValidateValue(ValueConverter != null
                    ? ValueConverter.Convert(value, typeof(object), null, cultureInfo)
                    : value, cultureInfo);

                if (!isValid)
                {
                    var error = ErrorProvider.GetErrorMessage(value);
                    if (StrictValidation)
                    {
                        // If we're going to stop propagation, we need to make sure
                        // that the pipe is clean for the next turn.
                        ValidationPipe.Error = null;
                        return new ValidationResult( error);
                    }

                    ValidationPipe.Error = error;
                }

                return ValidationResult.Success;
            }
            else
            {
                // Optionally ignore null/empty values.
                if (IgnoreNullOrEmpty && value == null || value is "")
                {
                    return ValidationResult.Success;
                }

                if (!IsEnforced.Value)
                {
                    return ValidationResult.Success;
                }

                // When there's no pipe, validation must return eagerly.
                // Properties will not be updated this way as validation will stop binding.
                var isValid = ValidateValue(ValueConverter != null
                    ? ValueConverter.Convert(value, typeof(object), null, cultureInfo)
                    : value, cultureInfo);

                return isValid
                    ? ValidationResult.Success
                    : new ValidationResult(ErrorProvider.GetErrorMessage(value));
            }
        }

        protected abstract bool ValidateValue(object value, CultureInfo cultureInfo);
    }
}
