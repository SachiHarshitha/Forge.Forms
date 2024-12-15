using Avalonia;

namespace Forge.Forms.AvaloniaUI.Controls;

public static class TextProperties
{
    public static readonly AttachedProperty<double> TitleFontSizeProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject,double>(
            "TitleFontSize",
            typeof(TextProperties),
            20d,
            true);

    public static readonly AttachedProperty<double> HeadingFontSizeProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject,double>(
            "HeadingFontSize",
            typeof(TextProperties),
            15d,
            true);


    public static readonly AttachedProperty<double> TextFontSizeProperty =
        AvaloniaProperty.RegisterAttached<AvaloniaObject,double>(
            "TextFontSize",
            typeof(TextProperties),
            13d,
            true);
    
    public static double GetTitleFontSize(AvaloniaObject element)
    {
        return (double)element.GetValue(TitleFontSizeProperty);
    }

    public static void SetTitleFontSize(AvaloniaObject element, double value)
    {
        element.SetValue(TitleFontSizeProperty, value);
    }

    public static double GetHeadingFontSize(AvaloniaObject element)
    {
        return (double)element.GetValue(HeadingFontSizeProperty);
    }

    public static void SetHeadingFontSize(AvaloniaObject element, double value)
    {
        element.SetValue(HeadingFontSizeProperty, value);
    }

    public static double GetTextFontSize(AvaloniaObject element)
    {
        return (double)element.GetValue(TextFontSizeProperty);
    }

    public static void SetTextFontSize(AvaloniaObject element, double value)
    {
        element.SetValue(TextFontSizeProperty, value);
    }
}