using Forge.Forms.AvaloniaUI.DynamicExpressions;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public abstract class ContentElement : FormElement
{
    #region Methods

    protected internal override void Freeze()
    {
        base.Freeze();
        Resources.Add(nameof(Content), Content ?? LiteralValue.Null);
        Resources.Add(nameof(IconPadding), IconPadding ?? LiteralValue.False);
        Resources.Add(nameof(IconVisibility), IconVisibility ?? LiteralValue.False);

        if (Icon != null && !(Icon is LiteralValue v && v.Value == null))
            Resources.Add(nameof(Icon), Icon);
        else
            Resources.Add(nameof(Icon), new LiteralValue((PackIconKind)(-2)));
    }

    #endregion

    #region Properties

    public IValueProvider Content { get; set; }

    public IValueProvider Icon { get; set; }

    public IValueProvider IconPadding { get; set; }

    public IValueProvider IconVisibility { get; set; }

    #endregion
}