using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class StringField : DataFormField
{
    public StringField(string key)
        : base(key, typeof(string))
    {
    }

    #region Methods

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        if (IsMultiline) return new MultiLineStringPresenter(context, Resources, formResources);

        if (IsPassword) return new PasswordPresenter(context, Resources, formResources);

        return new StringPresenter(context, Resources, formResources);
    }

    #endregion Methods

    #region Properties

    public bool IsPassword { get; set; }

    public bool IsMultiline { get; set; }

    #endregion Properties
}

public class StringPresenter : ValueBindingProvider
{
    static StringPresenter()
    {
    }

    public StringPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(StringPresenter);
}

public class PasswordPresenter : ValueBindingProvider
{
    static PasswordPresenter()
    {
    }

    public PasswordPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(PasswordPresenter);
}

public class MultiLineStringPresenter : ValueBindingProvider
{
    static MultiLineStringPresenter()
    {
    }

    public MultiLineStringPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(MultiLineStringPresenter);
}

public class NumericPresenter : ValueBindingProvider
{
    public NumericPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(NumericPresenter);
}