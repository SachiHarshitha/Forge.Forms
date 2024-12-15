using Avalonia.Controls;

namespace Forge.Forms.AvaloniaUI.Controls;

public partial class DynamicFormWrap : UserControl
{
    public DynamicFormWrap(object model, object context, DialogOptions options)
    {
        DataContext = options;
        InitializeComponent();
        Form.Environment.Add(options.EnvironmentFlags);
        Form.Context = context;
        Form.Model = model;
    }
}