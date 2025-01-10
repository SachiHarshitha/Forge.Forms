using System.Security.Cryptography;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaUI.Controls.Banking;
using Forge.Forms.Avalonia.Demo.ViewModels;
using Forge.Forms.Avalonia.Demo.Views;

namespace Forge.Forms.Avalonia.Demo;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };

        // Initialize SecureDataHelper with a key and IV
        var key = new byte[32]; // Generate or load a 32-byte key
        var iv = new byte[16]; // Generate or load a 16-byte IV
        RandomNumberGenerator.Fill(key); // Generate random key
        RandomNumberGenerator.Fill(iv); // Generate random IV
        SecureDataHelper.Initialize(key, iv);

        base.OnFrameworkInitializationCompleted();
    }
}