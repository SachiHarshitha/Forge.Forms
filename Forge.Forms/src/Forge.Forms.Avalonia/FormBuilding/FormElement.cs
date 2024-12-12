using System;
using System.Collections.Generic;
using Forge.Forms.AvaloniaUI.Annotations;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

/// <summary>
///     Represents a form element, which is not necessarily an input field.
/// </summary>
public abstract class FormElement
{
    #region Constructor
    protected FormElement()
    {
        Resources = new Dictionary<string, IValueProvider>();
        Metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }
    #endregion


    #region Properties
    protected internal Position LinePosition { get; set; }

    public IDictionary<string, IValueProvider> Resources { get; set; }

    public IDictionary<string, string> Metadata { get; }

    /// <summary>
    ///     Gets or sets the bool resource that determines whether this element will be visible.
    /// </summary>
    public IValueProvider IsVisible { get; set; }

    /// <summary>
    ///     Gets or sets the bool resource that determines whether this element will be enabled.
    /// </summary>
    public IValueProvider IsEnabled { get; set; }

    /// <summary>
    ///     Gets or sets the bool resource that determines whether this element will receive initial focus.
    /// </summary>
    public IValueProvider InitialFocus { get; set; }
    #endregion

    #region Methods
    protected internal virtual void Freeze()
    {
        Resources.Add(nameof(IsVisible), IsVisible ?? LiteralValue.True);
        Resources.Add(nameof(IsEnabled), IsEnabled ?? LiteralValue.True);
        Resources.Add(nameof(InitialFocus), InitialFocus ?? LiteralValue.False);
    }

    public FormElement FreezeResources()
    {
        Freeze();
        return this;
    }

    protected internal abstract IBindingProvider CreateBindingProvider(
        IResourceContext context,
        IDictionary<string, IValueProvider> formResources);
    #endregion



}