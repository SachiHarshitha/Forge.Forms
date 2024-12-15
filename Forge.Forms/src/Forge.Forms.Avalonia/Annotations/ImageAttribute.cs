using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Avalonia.Layout;
using Avalonia.Media;
using Forge.Forms.AvaloniaUI.FormBuilding;
using Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

namespace Forge.Forms.AvaloniaUI.Annotations;

/// <summary>
///     Draws images from ImageSources or string paths.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true)]
public sealed class ImageAttribute : FormContentAttribute
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ImageAttribute" /> class.
    /// </summary>
    /// <param name="source">ImageSource value. Accepts a string or a dynamic resource.</param>
    /// <param name="position">Do not provide a value for this argument.</param>
    public ImageAttribute(string source, [CallerLineNumber] int position = 0)
        : base(position)
    {
        Source = source;
    }

    /// <summary>
    ///     Gets the image source path or dynamic resource that resolves to the image source.
    /// </summary>
    public string Source { get; }

    /// <summary>
    ///     Gets or sets the image width. Accepts "auto", a double, or a dynamic resource resolving to one of those.
    ///     Defaults to auto.
    /// </summary>
    public object Width { get; set; }

    /// <summary>
    ///     Gets or sets the image height. Accepts "auto", a double, or a dynamic resource resolving to one of those.
    ///     Defaults to auto.
    /// </summary>
    public object Height { get; set; }

    /// <summary>
    ///     Gets or sets the image horizontal alignment. Accepts a <see cref="Avalonia.Layout.HorizontalAlignment" />, string,
    ///     or a dynamic resource.
    ///     Defaults to <see cref="Avalonia.Media.Stretch" />.
    /// </summary>
    public object HorizontalAlignment { get; set; }

    /// <summary>
    ///     Gets or sets the image horizontal alignment. Accepts a <see cref="Avalonia.Layout.VerticalAlignment" />, string, or
    ///     a dynamic resource.
    ///     Defaults to <see cref="Vertical.Center" />.
    /// </summary>
    public object VerticalAlignment { get; set; }

    /// <summary>
    ///     Gets or sets the image stretch behavior. Accepts a <see cref="Avalonia.Media.Stretch" />, string, or a
    ///     dynamic resource.
    ///     Defaults to <see cref="WpfStretch.Uniform" />.
    /// </summary>
    public object Stretch { get; set; }

    /// <summary>
    ///     Gets or sets the image stretch direction. Accepts a <see cref="Avalonia.Media.StretchDirection" />,
    ///     string, or a dynamic resource.
    ///     Defaults to <see cref="WpfStretchDirection.DownOnly" />.
    /// </summary>
    public object StretchDirection { get; set; }

    protected override FormElement CreateElement()
    {
        return new ImageElement
        {
            Source = Utilities.GetResource<object>(Source, null, x => x),
            Width = Utilities.GetResource<double>(Width, double.NaN, SizeDeserializer),
            Height = Utilities.GetResource<double>(Height, double.NaN, SizeDeserializer),
            HorizontalAlignment = Utilities.GetResource<HorizontalAlignment>(HorizontalAlignment,
                Avalonia.Layout.HorizontalAlignment.Stretch,
                Deserializers.Enum<HorizontalAlignment>()),
            VerticalAlignment =
                Utilities.GetResource<VerticalAlignment>(VerticalAlignment, Avalonia.Layout.VerticalAlignment.Center,
                    Deserializers.Enum<VerticalAlignment>()),
            Stretch = Utilities.GetResource<Stretch>(Stretch, Avalonia.Media.Stretch.Uniform,
                Deserializers.Enum<Stretch>()),
            StretchDirection = Utilities.GetResource<StretchDirection>(StretchDirection,
                Avalonia.Media.StretchDirection.DownOnly, Deserializers.Enum<StretchDirection>())
        };
    }

    private static object SizeDeserializer(string arg)
    {
        return string.Equals(arg, "auto", StringComparison.OrdinalIgnoreCase)
            ? double.NaN
            : double.Parse(arg, CultureInfo.InvariantCulture);
    }
}