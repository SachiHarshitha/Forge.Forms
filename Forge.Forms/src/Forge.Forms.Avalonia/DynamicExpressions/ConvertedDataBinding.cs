using System;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public sealed class ConvertedDataBinding : IValueProvider
{
    public ConvertedDataBinding(string propertyPath, BindingOptions bindingOptions, ReplacementPipe replacementPipe)
        : this(propertyPath, bindingOptions, replacementPipe, false)
    {
    }

    public ConvertedDataBinding(string propertyPath, BindingOptions bindingOptions, ReplacementPipe replacementPipe,
        bool oneWay)
    {
        PropertyPath = propertyPath;
        BindingOptions = bindingOptions ?? throw new ArgumentNullException(nameof(bindingOptions));
        ReplacementPipe = replacementPipe ?? throw new ArgumentNullException(nameof(replacementPipe));
        OneWay = oneWay;
    }

    public string PropertyPath { get; }

    public BindingOptions BindingOptions { get; }

    public ReplacementPipe ReplacementPipe { get; }

    public bool OneWay { get; }

    public IBinding ProvideBinding(IResourceContext context)
    {
        var binding = context.CreateModelBinding(PropertyPath);
        if (OneWay) binding.Mode = BindingMode.OneWay;

        BindingOptions.Apply(binding);
        var deserializer = ReplacementPipe.CreateDeserializer(context);
        binding.Converter = new StringTypeConverter(deserializer);
        return binding;
    }

    public object ProvideValue(IResourceContext context)
    {
        return ProvideBinding(context);
    }
}