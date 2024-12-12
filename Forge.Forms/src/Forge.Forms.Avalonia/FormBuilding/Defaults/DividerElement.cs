using System;
using System.Collections.Generic;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class DividerElement : FormElement
{
    public IValueProvider HasMargin { get; set; }

    protected internal override void Freeze()
    {
        base.Freeze();
        Resources.Add(nameof(HasMargin), HasMargin ?? LiteralValue.True);
    }

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new DividerPresenter(context, Resources, formResources);
    }
}

public class DividerPresenter : BindingProvider
{
    protected override Type StyleKeyOverride => typeof(DividerPresenter);

    static DividerPresenter()
    {

    }

    public DividerPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}