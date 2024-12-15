using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Data;
using Forge.Forms.AvaloniaUI.FormBuilding;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

public class EnumerableKeyValueProvider : IValueProvider
{
    private readonly bool addNull;
    private readonly IEnumerable<KeyValuePair<ValueType, IValueProvider>> elements;

    public EnumerableKeyValueProvider(IEnumerable<KeyValuePair<ValueType, IValueProvider>> elements, bool addNull)
    {
        this.addNull = addNull;
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
        var list = elements.Select(e =>
        {
            var proxy = e.Value.GetStringValue(context);
            proxy.Key = e.Key;
            return proxy;
        }).ToList();
        
        if (addNull) list.Insert(0, new StringProxy { Key = null, Value = "" });

        return list;
    }
}