using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.Avalonia.Demo.Models;

[Title("Boolean Operators")]
[Text("A message will appear when you set the right configuration.")]
[Text("Toggle one must be switched ON, the text box must not be empty, and toggle two must be switched OFF.")]
[Heading("Congratulations, you got the right combination!",
    IsVisible =
        "{Binding GivenUp} || ({Binding ToggleOne} && {Binding TypeSomething|IsNotEmpty} && !{Binding ToggleTwo})")]
[Divider]
public partial class BooleanLogic : ObservableObject
{
    [ObservableProperty] [property: Field(Name = "I give up, just show it already!")]
    private bool givenUp;

    [ObservableProperty] [property: Toggle]
    private bool toggleOne;

    [ObservableProperty] [property: Toggle]
    private bool toggleTwo;

    [ObservableProperty] [property: Binding(UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged)]
    private string typeSomething;
}