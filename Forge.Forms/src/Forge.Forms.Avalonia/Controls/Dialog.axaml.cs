using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Forge.Forms.AvaloniaUI.Controls;

public partial class Dialog : Window
{
    private readonly WindowOptions options;

    public Dialog(object model, object context, WindowOptions options)
    {
        this.options = options;
        DataContext = options;
        WindowStartupLocation = options.WindowStartupLocation;
        if (options.WindowStartupLocation == WindowStartupLocation.CenterOwner)
            Owner = options.Owner;

        InitializeComponent();

        Loaded += DialogWindow_Loaded;
        Form.Environment.Add(options.EnvironmentFlags);
        Form.Context = context;
        Form.Model = model;
    }

    private void DialogWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (options.BringToFront)
        {
            Activate();
            Focus();
        }

        //MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }
}