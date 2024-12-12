using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Forge.Forms.Avalonia.Demo.ViewModels;

namespace Forge.Forms.Avalonia.Demo.Views;

public partial class ExamplesView : UserControl
{
    public ExamplesView()
    {
        InitializeComponent();
        this.DataContext = new ExamplesViewModel();
    }
}