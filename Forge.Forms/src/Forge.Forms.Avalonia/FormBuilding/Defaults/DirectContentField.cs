using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class DirectContentField : DataFormField
{
    public DirectContentField(string key, Type propertyType)
        : base(key, propertyType)
    {
    }

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new DirectContentPresenter(context, Resources, formResources);
    }
}

public class DirectContentPresenter : ValueBindingProvider
{
    protected override Type StyleKeyOverride => typeof(DirectContentPresenter);

    static DirectContentPresenter()
    {

    }

    public DirectContentPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}