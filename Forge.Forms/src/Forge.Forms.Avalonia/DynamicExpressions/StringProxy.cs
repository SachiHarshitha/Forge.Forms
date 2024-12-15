using System;
using Avalonia;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

/// <summary>
///     Encapsulates a string bound to a resource.
/// </summary>
public class StringProxy : AvaloniaObject, IStringProxy, IProxy
{
    // Define a bindable AvaloniaProperty equivalent to DependencyProperty
    public static readonly AvaloniaProperty<object> KeyProperty =
        AvaloniaProperty.Register<StringProxy, object>(nameof(Key));

    // Define a bindable AvaloniaProperty equivalent to DependencyProperty
    public static readonly AvaloniaProperty<object> ValueProperty =
        AvaloniaProperty.Register<StringProxy, object>(nameof(Value));

    static StringProxy()
    {
        ValueProperty.Changed.AddClassHandler<StringProxy>(PropertyChangedCallback);
    }

    public object Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    object IProxy.Value => Value;

    public Action ValueChanged { get; set; }

    public string Value
    {
        get => (string)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    private static void PropertyChangedCallback(AvaloniaObject dependencyObject,
        AvaloniaPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        ((StringProxy)dependencyObject).ValueChanged?.Invoke();
    }

    public override string ToString()
    {
        return Value;
    }

    // protected override Freezable CreateInstanceCore()
    // {
    //     return new StringProxy();
    // }
}