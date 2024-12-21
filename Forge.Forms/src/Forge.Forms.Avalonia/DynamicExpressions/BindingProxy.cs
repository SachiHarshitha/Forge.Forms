using System;
using Avalonia;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

/// <summary>
///     Encapsulates an object bound to a resource.
/// </summary>
public class BindingProxy : AvaloniaObject, IProxy
{
    // Define a bindable AvaloniaProperty equivalent to DependencyProperty
    public static readonly AvaloniaProperty<object> ValueProperty =
        AvaloniaProperty.Register<BindingProxy, object>(nameof(Value));

    static BindingProxy()
    {
        ValueProperty.Changed.AddClassHandler<BindingProxy>(PropertyChangedCallback);
    }

    // The Key property (not bindable, used internally)
    public object Key { get; set; }

    // The bindable Value property
    public object Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    // Action to invoke when the value changes
    public Action ValueChanged { get; set; }

    private static void PropertyChangedCallback(AvaloniaObject dependencyObject,
        AvaloniaPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        ((BindingProxy)dependencyObject).ValueChanged?.Invoke();
    }
}

/// <summary>
///     Represents a key for a binding proxy.
/// </summary>
internal struct BindingProxyKey : IEquatable<BindingProxyKey>
{
    public BindingProxyKey(string key)
    {
        Key = key;
    }

    public string Key { get; }

    public bool Equals(BindingProxyKey other)
    {
        return Key == other.Key;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;

        return obj is BindingProxyKey key && Equals(key);
    }

    public override int GetHashCode()
    {
        return Key?.GetHashCode() ?? 0;
    }
}