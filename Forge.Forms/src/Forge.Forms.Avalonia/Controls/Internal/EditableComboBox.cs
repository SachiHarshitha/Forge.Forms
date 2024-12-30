using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;

namespace Forge.Forms.AvaloniaUI.Controls.Internal;

public class EditableComboBox : ComboBox
{
    public static readonly StyledProperty<string?> TextProperty =
        TextBlock.TextProperty.AddOwner<EditableComboBox>(
            new StyledPropertyMetadata<string?>(string.Empty, BindingMode.TwoWay));

    public static readonly DirectProperty<EditableComboBox, StringComparer> ItemTextComparerProperty =
        AvaloniaProperty.RegisterDirect<EditableComboBox, StringComparer>(nameof(ItemTextComparer),
            x => x.ItemTextComparer,
            (x, v) => x.ItemTextComparer = v,
            StringComparer.CurrentCultureIgnoreCase);


    private TextBox? _inputText;
    private bool _supressSelectedItemChange;

    static EditableComboBox()
    {
        TextProperty.Changed.AddClassHandler<EditableComboBox>((x, e) => x.TextChanged(e));
        SelectedItemProperty.Changed.AddClassHandler<EditableComboBox>((x, e) => x.SelectedItemChanged(e));
        //when the items change we need to simulate a text change to validate the text being an item or not and selecting it
        ItemsSourceProperty.Changed.AddClassHandler<EditableComboBox>((x, e) => x.TextChanged(
            new AvaloniaPropertyChangedEventArgs<string?>(e.Sender, TextProperty, x.Text, x.Text, e.Priority)));
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    public StringComparer ItemTextComparer { get; set; } = StringComparer.CurrentCultureIgnoreCase;

    private void TextChanged(AvaloniaPropertyChangedEventArgs e)
    {
        //don't check for an item if there are no items or if we are already processing a change
        if (Items == null || _supressSelectedItemChange)
            return;

        var newVal = e.GetNewValue<string>();
        var selectedIdx = -1;
        object? selectedItem = null;
        var i = -1;
        foreach (var o in Items)
        {
            i++;
            if (ItemTextComparer.Equals(newVal, o.ToString()))
            {
                selectedIdx = i;
                selectedItem = o;
                break;
            }
        }

        var clearingSelectedItem = SelectedIndex > -1 && selectedIdx == -1;
        var settingSelectedItem = SelectedIndex == -1 && selectedIdx > -1;

        _supressSelectedItemChange = true;
        SelectedIndex = selectedIdx;
        SelectedItem = selectedItem;
        //set the text to the Item.ToString() if an item has been selected (or text matched)
        if (settingSelectedItem)
            SetCurrentValue(TextProperty, SelectedItem?.ToString() ?? newVal);
        _supressSelectedItemChange = false;
    }

    private void SelectedItemChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (_supressSelectedItemChange)
            return;

        _supressSelectedItemChange = true;
        if (_inputText != null)
            _inputText.Text = e.NewValue?.ToString() ?? string.Empty;
        SetValue(TextProperty, e.NewValue?.ToString() ?? string.Empty);
        _supressSelectedItemChange = false;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        //if the use clicks in the text box we don't want to open the dropdown
        if (_inputText != null && e.Source is StyledElement styledElement &&
            styledElement.TemplatedParent == _inputText)
            return;
        base.OnPointerReleased(e);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        _inputText = e.NameScope.Get<TextBox>("PART_InputText");
        if (_inputText != null)
            _inputText.Text = PlaceholderText;
        base.OnApplyTemplate(e);
    }
}