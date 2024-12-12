using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public class MultiValueBinding : IValueProvider
{
    public MultiValueBinding(IEnumerable<IValueProvider> valueProviders, IMultiValueConverter converter)
    {
        ValueProviders = valueProviders?.ToArray() ?? throw new ArgumentNullException(nameof(valueProviders));
        Converter = converter;
    }

    public IValueProvider[] ValueProviders { get; }

    public IMultiValueConverter Converter { get; }

    public IBinding ProvideBinding(IResourceContext context)
    {
        var multiBinding = new Avalonia.Data.MultiBinding
        {
            Converter = Converter
        };

        foreach (var binding in ValueProviders.Select(provider => provider.ProvideBinding(context)))
            multiBinding.Bindings.Add(binding);

        return multiBinding;
    }

    public object ProvideValue(IResourceContext context)
    {
        return ProvideBinding(context);
    }
}