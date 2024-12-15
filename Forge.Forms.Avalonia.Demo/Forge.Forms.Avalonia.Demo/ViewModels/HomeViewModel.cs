using CommunityToolkit.Mvvm.ComponentModel;
using Forge.Forms.Avalonia.Demo.Models;

namespace Forge.Forms.Avalonia.Demo.ViewModels;

public class HomeViewModel : ObservableObject
{
    public object Model => new Introduction();
}