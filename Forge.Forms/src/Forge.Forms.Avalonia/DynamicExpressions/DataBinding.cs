using System;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class DataBinding : Resource
{
    public DataBinding(string propertyPath, BindingOptions bindingOptions)
        : this(propertyPath, bindingOptions, null, false)
    {
    }

    public DataBinding(string propertyPath, BindingOptions bindingOptions, bool oneWay)
        : this(propertyPath, bindingOptions, null, oneWay)
    {
    }

    public DataBinding(string propertyPath, BindingOptions bindingOptions, string valueConverter)
        : this(propertyPath, bindingOptions, valueConverter, false)
    {
    }

    public DataBinding(string propertyPath, BindingOptions bindingOptions, string valueConverter, bool oneWay)
        : base(valueConverter)
    {
        PropertyPath = propertyPath;
        BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
        OneWay = oneWay;
    }

    public string PropertyPath { get; }

    public BindingOptions BindingOptions { get; }

    public bool OneWay { get; }

    public override bool IsDynamic => true;

    public override IBinding ProvideBinding(IResourceContext context)
    {
        var binding = context.CreateModelBinding(PropertyPath);
        if (OneWay) binding.Mode = BindingMode.OneWay;

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
        return PropertyPath.GetHashCode();
    }
}