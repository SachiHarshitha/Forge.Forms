using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.Validation
{
    public class MethodInvocationValidator : FieldValidator
    {
        private readonly Func<object, CultureInfo, bool, bool> method;

        public MethodInvocationValidator(
            ValidationPipe pipe,
            Func<object, CultureInfo, bool, bool> method,
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
            this.method = method;
        }

        protected override bool ValidateValue(object value, CultureInfo cultureInfo)
        {
            return method(value, cultureInfo, StrictValidation);
        }
    }
}
