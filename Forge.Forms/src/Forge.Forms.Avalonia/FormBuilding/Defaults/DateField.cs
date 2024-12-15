using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class DateField : DataFormField
{
    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    public DateField(string key) : base(key, typeof(DateTime))
    {
    }

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        var datePresenter = new DatePresenter(context, Resources, formResources);
        return datePresenter;
    }
}

public class DatePresenter : ValueBindingProvider
{
    static DatePresenter()
    {
    }

    public DatePresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(DatePresenter);
}