using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class HeadingElement : ContentElement
{
    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new HeadingPresenter(context, Resources, formResources);
    }
}

public class HeadingPresenter : BindingProvider
{
    static HeadingPresenter()
    {
    }

    public HeadingPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(HeadingPresenter);
}