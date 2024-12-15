using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.Validation;

public interface IValidatorProvider
{
    FieldValidator GetValidator(IResourceContext context, ValidationPipe pipe);
}