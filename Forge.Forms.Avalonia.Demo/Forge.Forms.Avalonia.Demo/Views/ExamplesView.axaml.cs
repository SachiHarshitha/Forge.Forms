using Avalonia.Controls;
using Forge.Forms.Avalonia.Demo.ViewModels;

namespace Forge.Forms.Avalonia.Demo.Views;

public partial class ExamplesView : UserControl
{
    public ExamplesView()
    {
        InitializeComponent();
        DataContext = new ExamplesViewModel();
    }
}