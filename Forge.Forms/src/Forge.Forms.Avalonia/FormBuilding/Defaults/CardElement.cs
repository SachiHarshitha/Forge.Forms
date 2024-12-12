using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class CardElement : FormElement
{
    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new CardPresenter(context, Resources, formResources);
    }
}

public class CardPresenter : BindingProvider
{
    protected override Type StyleKeyOverride => typeof(CardPresenter);

    static CardPresenter()
    {

    }

    public CardPresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}