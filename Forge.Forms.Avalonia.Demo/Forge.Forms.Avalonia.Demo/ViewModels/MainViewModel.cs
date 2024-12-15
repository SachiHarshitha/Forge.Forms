using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Forge.Forms.Avalonia.Demo.Models;
using Forge.Forms.AvaloniaUI;

namespace Forge.Forms.Avalonia.Demo.ViewModels;

public class MainViewModel : ViewModelBase
{
    public ICommand _openFormCommand;
    public string Greeting => "Welcome to Avalonia!";
    public object Model => new User { FirstName = "A", LastName = "B" };

    public ICommand OpenFormCommand => _openFormCommand ??= new RelayCommand(OpenForm_Clicked);

    private async void OpenForm_Clicked()
    {
        var result = await Show.Window().For(Model);
    }
}