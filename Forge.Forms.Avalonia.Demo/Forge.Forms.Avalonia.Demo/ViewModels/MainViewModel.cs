using CommunityToolkit.Mvvm.Input;

using Forge.Forms.Avalonia.Demo.Models;
using Forge.Forms.AvaloniaUI;

using System;
using System.Windows.Input;

namespace Forge.Forms.Avalonia.Demo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    public ICommand _openFormCommand;
    public object Model => new User();

    public ICommand OpenFormCommand => _openFormCommand ??= new RelayCommand(OpenForm_Clicked);

    private async void OpenForm_Clicked()
    {
        await Show.Window().For(Model);
    }
}