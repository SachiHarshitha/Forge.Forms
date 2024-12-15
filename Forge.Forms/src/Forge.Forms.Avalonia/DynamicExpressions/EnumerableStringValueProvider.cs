using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public class EnumerableStringValueProvider : IValueProvider
{
    private readonly IEnumerable<IValueProvider> elements;

    public EnumerableStringValueProvider(IEnumerable<IValueProvider> elements)
    {
        this.elements = elements ?? throw new ArgumentNullException(nameof(elements));
    }

    public IBinding ProvideBinding(IResourceContext context)
    {
        return new Binding
        {
            Source = ProvideValue(context),
            Mode = BindingMode.OneTime
        };
    }

    public object ProvideValue(IResourceContext context)
    {
        return elements.Select(e => e.GetStringValue(context, true)).ToList();
    }
}