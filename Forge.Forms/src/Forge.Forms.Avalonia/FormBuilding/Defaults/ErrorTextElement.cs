using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class ErrorTextElement : ContentElement
{
    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new ErrorTextPresenter(context, Resources, formResources);
    }
}

public class ErrorTextPresenter : BindingProvider
{
    protected override Type StyleKeyOverride => typeof(ErrorTextPresenter);

    static ErrorTextPresenter()
    {

    }

    public ErrorTextPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}