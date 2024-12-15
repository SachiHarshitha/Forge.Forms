using System;
using Avalonia;
using Avalonia.Controls;

namespace Forge.Forms.AvaloniaUI.Controls.Internal;

// Source: https://stackoverflow.com/questions/817610/wpf-and-initial-focus
internal class FocusHelper : AvaloniaObject
{
    static FocusHelper()
    {
        InitialFocusProperty.Changed.AddClassHandler<DynamicForm>(OnInitialFocusPropertyChanged);
    }
    
    public static readonly AttachedProperty<bool> InitialFocusProperty =
        AvaloniaProperty.RegisterAttached<FocusHelper,Control,bool>(
            "InitialFocus",
            false);

    public static bool GetInitialFocus(Control control)
    {
        return (bool)control.GetValue(InitialFocusProperty);
    }

    public static void SetInitialFocus(Control control, bool value)
    {
        control.SetValue(InitialFocusProperty, value);
    }

    private static void OnInitialFocusPropertyChanged(
        AvaloniaObject obj, AvaloniaPropertyChangedEventArgs args)
    {
        if (!(obj is Control control)) return;

        if (args.NewValue is true)
            control.Loaded += HandleFocus;
        else
            control.Loaded -= HandleFocus;
    }

    private static void HandleFocus(object sender, EventArgs e)
    {
        ((Control)sender).Focus();
    }
}