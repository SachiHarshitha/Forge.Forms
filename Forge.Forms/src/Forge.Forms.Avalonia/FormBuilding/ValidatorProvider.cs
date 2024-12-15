using System;
using Forge.Forms.AvaloniaUI.Validation;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

internal class ValidatorProvider : IValidatorProvider
{
    private readonly Func<IResourceContext, ValidationPipe, FieldValidator> func;

    public ValidatorProvider(Func<IResourceContext, ValidationPipe, FieldValidator> func)
    {
        this.func = func;
    }

    public FieldValidator GetValidator(IResourceContext context, ValidationPipe pipe)
    {
        return func(context, pipe);
    }
}