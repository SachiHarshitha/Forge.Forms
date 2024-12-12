using System;
using Avalonia.Data;

using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class DirectBinding : Resource
{
    public DirectBinding(BindingOptions bindingOptions)
        : this(bindingOptions, null)
    {
    }

    public DirectBinding(BindingOptions bindingOptions, string valueConverter)
        : base(valueConverter)
    {
        BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
    }

    public BindingOptions BindingOptions { get; }

    public override bool IsDynamic => true;

    public override IBinding ProvideBinding(IResourceContext context)
    {
        var binding = context.CreateDirectModelBinding();
        binding.Converter = GetValueConverter(context);
        BindingOptions.Apply(binding);
        return binding;
    }

    public override bool Equals(Resource other)
    {
        return ReferenceEquals(this, other);
    }

    public override int GetHashCode()
    {
        return BindingOptions.GetHashCode();
    }
}