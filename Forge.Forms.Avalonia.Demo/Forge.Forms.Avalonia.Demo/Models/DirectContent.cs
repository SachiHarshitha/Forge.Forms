using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.Avalonia.Demo.Models;

[Form(Mode = DefaultFields.AllIncludingReadonly)]
public class DirectContent
{
    [DirectContent] public string RawText { get; set; } = "This is a raw string";

    [DirectContent]
    public AvaloniaObject RawElement => new Ellipse
    {
        Width = 100d,
        Height = 100d,
        Fill = Brushes.Green
    };

    [Break]
    [DirectContent]
    public CustomContent CustomControl { get; } = new()
    {
        FirstName = "John",
        LastName = "Doe"
    };
}