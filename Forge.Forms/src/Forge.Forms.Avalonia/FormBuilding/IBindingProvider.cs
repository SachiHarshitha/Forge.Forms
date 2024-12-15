using Avalonia.Data;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding
{
    /// <summary>
    /// Provides bindings by resource name.
    /// </summary>
    public interface IBindingProvider
    {
        /// <summary>
        /// Returns a <see cref="ProvideValue" /> bound to the value returned by <see cref="name" />.
        /// </summary>
        /// <param name="name">Resource name. This is not the object property name.</param>
        /// <returns>Returns a <see cref="BindingProxy" />.</returns>
        BindingProxy this[string name] { get; }

        /// <summary>
        /// Resolves the value for the specified resource.
        /// The result may be a <see cref="name" /> or a literal value.
        /// </summary>
        /// <param name="name">Resource name. This is not the object property name.</param>
        /// <returns>Returns a <see cref="BindingBase" /> or a literal value.</returns>
        object ProvideValue(string name);

        /// <summary>
        /// Gets called when a binding expression is resolved.
        /// </summary>
        void BindingCreated(BindingExpressionBase expression, string resource);
    }
}