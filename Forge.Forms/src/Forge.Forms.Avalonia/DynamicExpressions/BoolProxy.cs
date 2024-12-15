using System;
using Avalonia;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

/// <summary>
///     Encapsulates a string bound to a resource.
/// </summary>
public class BoolProxy : AvaloniaObject, IBoolProxy, IProxy
{
    // Define a bindable AvaloniaProperty equivalent to DependencyProperty
    public static readonly AvaloniaProperty<object> ValueProperty =
        AvaloniaProperty.Register<BindingProxy, object>(nameof(Value));

    static BoolProxy()
    {
        ValueProperty.Changed.AddClassHandler<BoolProxy>(PropertyChangedCallback);
    }

    // The Key property (not bindable, used internally)
    public object Key { get; set; }

    public bool Value
    {
        get => (bool)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    object IProxy.Value => Value;

    public Action ValueChanged { get; set; }

    private static void PropertyChangedCallback(AvaloniaObject dependencyObject,
        AvaloniaPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
        ((BoolProxy)dependencyObject).ValueChanged?.Invoke();
    }


    // protected override AvaloniaObject CreateInstanceCore()
    // {
    //     return new BoolProxy();
    // }
}