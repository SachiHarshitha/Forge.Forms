using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class TextElement : ContentElement
{
    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new TextPresenter(context, Resources, formResources);
    }
}

public class TextPresenter : BindingProvider
{
    protected override Type StyleKeyOverride => typeof(TextPresenter);

    static TextPresenter()
    {
       
    }

    public TextPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}