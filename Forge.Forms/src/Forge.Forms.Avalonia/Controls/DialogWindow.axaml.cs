using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;

namespace Forge.Forms.AvaloniaUI.Controls;

public partial class DialogWindow : Window
{
    private readonly WindowOptions options;

    public DialogWindow(object model, object context, WindowOptions options)
    {
        this.options = options;
        DataContext = options;
        WindowStartupLocation = options.WindowStartupLocation;
        if (options.WindowStartupLocation == WindowStartupLocation.CenterOwner)
            Owner = options.Owner;
        if (!options.ShowCaptionButtons)
        {
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
            SystemDecorations = SystemDecorations.BorderOnly;
        }
        else
        {
            ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.PreferSystemChrome;
        }

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

    // private void CloseDialogCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
    // {
    //     //DialogResult = e.Parameter as bool?;
    //     Close();
    // }
    //
    // private void CloseDialogCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    // {
    //     e.CanExecute = true;
    // }
}