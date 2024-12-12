using System;
using System.Collections.Generic;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class BreakElement : FormElement
{
    public IValueProvider Height { get; set; }

    protected internal override void Freeze()
    {
        base.Freeze();
        Resources.Add(nameof(Height), Height ?? new LiteralValue(8d));
    }

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new BreakPresenter(context, Resources, formResources);
    }
}

public class BreakPresenter : BindingProvider
{
    protected override Type StyleKeyOverride => typeof(BreakPresenter);

    static BreakPresenter()
    {

    }

    public BreakPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}