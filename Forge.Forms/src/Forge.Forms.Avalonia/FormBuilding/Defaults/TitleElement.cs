using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class TitleElement : ContentElement
{
    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new TitlePresenter(context, Resources, formResources);
    }
}

public class TitlePresenter : BindingProvider
{
    protected override Type StyleKeyOverride => typeof(TitlePresenter);

    static TitlePresenter()
    {

    }

    public TitlePresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}