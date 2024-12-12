using System.Collections.Generic;
using Avalonia.Data;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public interface IDataBindingProvider : IBindingProvider
{
    IEnumerable<BindingExpressionBase> GetBindings();

    void ClearBindings();
}