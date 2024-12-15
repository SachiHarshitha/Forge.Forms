using Avalonia.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.Avalonia.Demo.Models;

public class Introduction : ObservableObject
{
    private string yourName;

    [Title("Forge.Forms.AvaloniaUI")]
    [Heading("Introduction")]
    [Text("Welcome to Forge.Forms.AvaloniaUI, a library for building dynamic forms.")]
    [Text("This library offers a declarative and MVVM friendly approach " +
          "to consistent forms with minimal boilerplate.")]
    [Text("The control you are currently seeing is a dynamic form, " +
          "as is the field below. Try entering your name:")]
    [Binding(UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged)]
    public string YourName
    {
        get => yourName;
        set
        {
            yourName = value;
            OnPropertyChanged();
        }
    }

    [Text("You entered: {Binding YourName}", IsVisible = "{Binding YourName|IsNotEmpty}")]
    [Text("Although everything shown here is possible in XAML, you will find " +
          "that for many cases this library will automate your V in MVVM " +
          "so that you can focus on more important tasks.")]
    [Text("To get started, read the following sections and see the examples.")]
    [Break]
    [Heading("Usage")]
    [Text("Using this library is straightforward. DynamicForm " +
          "is a control that will render controls bound " +
          "to an associated model. A model can be an object, a type, " +
          "a primitive, or a custom IFormDefinition.")]
    [Break]
    [Card(3)]
    [Break]
    [Text("<DynamicForm Model=\"{{Binding Model}}\" />", ShareLine = true)]
    [Action("copy", "COPY", Icon = "ContentCopy", Parameter = "<DynamicForm Model=\"{{Binding Model}}\" />",
        Placement = Placement.Before)]
    [Break]
    [Break(Height = 16d)]
    [Text("A DynamicForm has two key properties, the 'Model' property, which " +
          "represents the form being rendered, and the 'Context' property, which " +
          "allows models to access data outside of their scope, such as a selection " +
          "field or action handling.")]
    [Text("Reflection happens only the first time a model is inspected, after " +
          "which its definition is cached and subsequent renders will be quite fast.")]
    [Text("See live examples for a quick overview of features.", ShareLine = true)]
    [Action("examples", "VIEW EXAMPLES", Placement = Placement.Before)]
    [Field(IsVisible = false)]
    public string AnnotationDummy { get; set; }
}