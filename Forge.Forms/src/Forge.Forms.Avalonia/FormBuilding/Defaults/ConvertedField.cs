using System;
using System.Collections.Generic;

using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public sealed class ConvertedField : DataFormField
{
    public ConvertedField(string key, Type propertyType, ReplacementPipe replacementPipe)
        : base(key, propertyType)
    {
        ReplacementPipe = replacementPipe;
        CreateBinding = false;
    }

    public ReplacementPipe ReplacementPipe { get; }

    protected internal override void Freeze()
    {
        base.Freeze();
        if (IsDirectBinding)
            Resources.Add("Value",
                new ConvertedDirectBinding(BindingOptions, ReplacementPipe));
        else if (string.IsNullOrEmpty(Key))
            Resources.Add("Value", LiteralValue.Null);
        else
            Resources.Add("Value",
                new ConvertedDataBinding(Key, BindingOptions, ReplacementPipe, StrictlyReadOnly));
    }

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new ConvertedPresenter(context, Resources, formResources);
    }
}

public class ConvertedPresenter : ValueBindingProvider
{
    protected override Type StyleKeyOverride => typeof(ConvertedPresenter);

    static ConvertedPresenter()
    {

    }

    public ConvertedPresenter(IResourceContext context,
        IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }
}