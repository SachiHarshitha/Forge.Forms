using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Forge.Forms.AvaloniaUI.DynamicExpressions;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Defaults;

public class ImageElement : FormElement
{
    private class ImageSourceValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case string path:
                    return path;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BindingOperations.DoNothing;
        }
    }

    #region Properties

    public IValueProvider Source { get; set; }

    public IValueProvider Width { get; set; }

    public IValueProvider Height { get; set; }

    public IValueProvider VerticalAlignment { get; set; }

    public IValueProvider HorizontalAlignment { get; set; }

    public IValueProvider Stretch { get; set; }

    public IValueProvider StretchDirection { get; set; }

    #endregion

    #region Methoods

    private static IValueProvider GetSource(IValueProvider source)
    {
        switch (source)
        {
            case LiteralValue literal:
                switch (literal.Value)
                {
                    case string str:
                        return new LiteralValue(str);
                    default:
                        return LiteralValue.Null;
                }
            case null:
                return LiteralValue.Null;
            default:
                return source.Wrap(new ImageSourceValueConverter());
        }
    }

    protected internal override void Freeze()
    {
        base.Freeze();
        Resources.Add(nameof(Source), GetSource(Source));
        Resources.Add(nameof(Width), Width ?? new LiteralValue(double.NaN));
        Resources.Add(nameof(Height), Height ?? new LiteralValue(double.NaN));
        Resources.Add(nameof(VerticalAlignment),
            VerticalAlignment ?? new LiteralValue(Avalonia.Layout.VerticalAlignment.Center));
        Resources.Add(nameof(HorizontalAlignment),
            HorizontalAlignment ?? new LiteralValue(Stretch));
        Resources.Add(nameof(Stretch), Stretch ?? new LiteralValue(Avalonia.Media.Stretch.Uniform));
        Resources.Add(nameof(StretchDirection),
            StretchDirection ?? new LiteralValue(Avalonia.Media.StretchDirection.DownOnly));
    }

    protected internal override IBindingProvider CreateBindingProvider(IResourceContext context,
        IDictionary<string, IValueProvider> formResources)
    {
        return new ImagePresenter(context, Resources, formResources);
    }

    #endregion
}

public class ImagePresenter : BindingProvider
{
    static ImagePresenter()
    {
    }

    public ImagePresenter(IResourceContext context, IDictionary<string, IValueProvider> fieldResources,
        IDictionary<string, IValueProvider> formResources)
        : base(context, fieldResources, formResources, true)
    {
    }

    protected override Type StyleKeyOverride => typeof(ImagePresenter);
}