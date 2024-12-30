using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using Forge.Forms.Avalonia.Demo.Models;
using Forge.Forms.AvaloniaUI;

namespace Forge.Forms.Avalonia.Demo.ViewModels;

public partial class ExamplesViewModel : ObservableObject
{
    private static readonly string ModelsDir = Path.Combine(
        Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? Directory.GetCurrentDirectory(),
        "Models");

    [ObservableProperty] private ExamplePresenter currentModel;

    public ExamplesViewModel()
    {
        Models = new ObservableCollection<ExamplePresenter>(GetModels());
        CurrentModel = Models?.FirstOrDefault();
    }

    public ObservableCollection<ExamplePresenter> Models { get; }

    private IEnumerable<ExamplePresenter> GetModels()
    {
        const double small = 320d;
        const double large = 540d;
        const double xLarge = 800d;

        yield return new ExamplePresenter(new Login(), "Login", large);

        yield return new ExamplePresenter(new Settings(), "Settings", large);

        yield return new ExamplePresenter(new User(), "User", large);

        yield return new ExamplePresenter(new TextFields(), "Text Fields", large);

        yield return new ExamplePresenter(new TextElements(), "Text Elements", large);

        yield return new ExamplePresenter(new Selection(), "Selection", large);

        yield return new ExamplePresenter(new SelectionMemberPaths(), "Selection Member Paths", large);

        yield return new ExamplePresenter(new FoodSelection(), "Food Selection", large);

        yield return new ExamplePresenter(new Dialogs(), "Dialogs", large);

        yield return new ExamplePresenter(new BooleanLogic(), "Boolean Expressions", large);

        yield return new ExamplePresenter(new InlineElements(), "Inline Elements", large);

        //yield return new ExamplePresenter(new Crud(), "CRUD", 2 * large);

        yield return new ExamplePresenter(new EnvManager(), "Environments", large);

        yield return new ExamplePresenter(new FileBindings(), "File Binding", large);

        yield return new ExamplePresenter(new CustomValidation(), "Validation", large);

        yield return new ExamplePresenter(new DirectContent(), "Direct Content", large);

        yield return new ExamplePresenter(new Alert
        {
            Message = "Item deleted."
        }, "Alert", small)
        {
            Source = @"new Alert(""Item deleted."");"
        };

        yield return new ExamplePresenter(new Confirmation
        {
            Message = "Discard draft?",
            PositiveAction = "DISCARD"
        }, "Confirm 1", small)
        {
            Source = @"new Confirmation(""Discard draft?"") { PositiveAction = ""DISCARD"" };"
        };

        yield return new ExamplePresenter(new Confirmation
        {
            Title = "Use Google's location service?",
            Message =
                "Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running.",
            PositiveAction = "AGREE",
            NegativeAction = "DISAGREE"
        }, "Confirm 2", small)
        {
            Source = @"new Confirmation(
    ""Let Google help apps determine location. This means sending anonymous location data to Google, even when no apps are running."",
    ""Use Google's location service?"", ""AGREE"", ""DISAGREE"");"
        };

        yield return new ExamplePresenter(new Prompt<string>
        {
            Title = "Enter your name"
        }, "Prompt 1", small)
        {
            Source = @"new Prompt<string> { Title = ""Enter your name"" };"
        };

        yield return new ExamplePresenter(new Prompt<bool>
        {
            Message = "Discard draft?",
            PositiveAction = "DISCARD",
            Name = "Prevent future dialogs"
        }, "Prompt 2", small)
        {
            Source = @"new Prompt<bool> 
{ 
    Message = ""Discard draft?"",
    PositiveAction = ""DISCARD"",
    Name = ""Prevent future dialogs""
};"
        };

        yield return new ExamplePresenter(new DataTypes(), "Data types", xLarge);
    }
}