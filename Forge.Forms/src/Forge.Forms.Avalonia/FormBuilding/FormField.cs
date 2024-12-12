using Forge.Forms.AvaloniaUI.DynamicExpressions;

using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

/// <summary>
///     Base class for all form field definitions.
/// </summary>
public abstract class FormField : FormElement
{
    /// <summary>
    ///     Gets or sets the unique name of this field.
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    ///     Gets or sets the string expression of the field's title.
    /// </summary>
    public IValueProvider Name { get; set; }

    /// <summary>
    ///     Gets or sets the string expression of the field's tooltip.
    /// </summary>
    public IValueProvider ToolTip { get; set; }

    /// <summary>
    ///     Gets or sets the field's PackIconKind resource. Not all controls may display an icon.
    /// </summary>
    public IValueProvider Icon { get; set; }
    

    /// <summary>
    ///     Finalizes the field state by adding all appropriate values as resources.
    ///     Changing properties after this method has been called is strongly discouraged.
    /// </summary>
    protected internal override void Freeze()
    {
        base.Freeze();
        Resources.Add(nameof(Name), Name ?? LiteralValue.Null);
        Resources.Add(nameof(ToolTip), ToolTip ?? LiteralValue.Null);

        if (Icon != null && !(Icon is LiteralValue v && v.Value == null))
        {
            Resources.Add("IconVisibility", LiteralValue.True);
            Resources.Add(nameof(Icon), Icon);
        }
        else
        {
            Resources.Add("IconVisibility", LiteralValue.False);
            Resources.Add(nameof(Icon), new LiteralValue((PackIconKind)(-2)));
        }
    }
}