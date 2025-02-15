﻿using System;
using System.Collections.Generic;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class BooleanField : DataFormField
{
    public BooleanField(string key) : base(key, typeof(bool))
    {
    }

    public bool IsSwitch { get; set; }

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        if (IsSwitch) return new SwitchPresenter(context, Resources, formResources);

        return new CheckBoxPresenter(context, Resources, formResources);
    }
}

public class CheckBoxPresenter : ValueBindingProvider
{
    static CheckBoxPresenter()
    {
    }

    public CheckBoxPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(CheckBoxPresenter);
}

public class SwitchPresenter : ValueBindingProvider
{
    static SwitchPresenter()
    {
    }

    public SwitchPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(SwitchPresenter);
}