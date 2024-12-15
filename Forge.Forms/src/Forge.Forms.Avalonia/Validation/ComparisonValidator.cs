using System;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.Validation
{
    public abstract class ComparisonValidator : FieldValidator
    {
        protected ComparisonValidator(
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
                errorProvider, 
                isEnforced, 
                valueConverter, 
                strictValidation, 
                validatesOnTargetUpdated,
                ignoreNullOrEmpty)
        {
            Argument = argument ?? throw new ArgumentNullException(nameof(argument));
        }

        public IProxy Argument { get; }
    }
}
