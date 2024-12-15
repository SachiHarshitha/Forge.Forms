using CommunityToolkit.Mvvm.ComponentModel;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.Avalonia.Demo.Models;

public class FoodSelection : ObservableObject
{
    private string firstFood = "Pizza";
    private string secondFood = "Steak";
    private string thirdFood = "Salad";
    private string yourFavoriteFood;

    [Field(DefaultValue = "Pizza")]
    [Value(Must.NotBeEmpty)]
    public string FirstFood
    {
        get => firstFood;
        set => SetProperty(ref firstFood, value);
    }

    [Field(DefaultValue = "Steak")]
    [Value(Must.NotBeEmpty)]
    public string SecondFood
    {
        get => secondFood;
        set => SetProperty(ref secondFood, value);
    }

    [Field(DefaultValue = "Salad")]
    [Value(Must.NotBeEmpty)]
    public string ThirdFood
    {
        get => thirdFood;
        set => SetProperty(ref thirdFood, value);
    }

    [Text("You have selected {Binding YourFavoriteFood}", Placement = Placement.After)]
    [SelectFrom(new[]
        { "{Binding FirstFood}, obviously.", "{Binding SecondFood} is best!", "I love {Binding ThirdFood}" })]
    public string YourFavoriteFood
    {
        get => yourFavoriteFood;
        set => SetProperty(ref yourFavoriteFood, value);
    }
}