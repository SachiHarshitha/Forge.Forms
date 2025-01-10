using System;
using System.Collections.Generic;
using AvaloniaUI.Controls.Banking;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class CreditCardField : DataFormField
{
    public CreditCardField(string key)
        : base(key, typeof(SecureCreditCard))
    {
    }

    #region Methods

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new CreditCardPresenter(context, Resources, formResources);
    }

    #endregion Methods
}

public class CreditCardPresenter : ValueBindingProvider
{
    public CreditCardPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(CreditCardPresenter);
}