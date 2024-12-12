using Avalonia;

namespace Forge.Forms.AvaloniaUI.Controls;

public static class TextProperties
{
    public static readonly StyledProperty<double> TitleFontSizeProperty =
        AvaloniaProperty.Register<AvaloniaObject,double>(
            "TitleFontSize",
            20d,
            true);

    public static readonly AvaloniaProperty<double> HeadingFontSizeProperty =
        AvaloniaProperty.Register<AvaloniaObject,double>(
            "HeadingFontSize",
            15d,
            true);
       

    public static readonly AvaloniaProperty<double> TextFontSizeProperty =
        AvaloniaProperty.Register<AvaloniaObject,double>(
            "TextFontSize",
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