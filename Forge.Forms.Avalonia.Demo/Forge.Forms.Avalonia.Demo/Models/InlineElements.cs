using Forge.Forms.AvaloniaUI.Annotations;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Forms.Avalonia.Demo.Models;

public class InlineElements
{
    [Action("available", "CHECK AVAILABILITY", Placement = Placement.Inline)]
    public string Username { get; set; }

    [Image("captcha.png")]
    [Text("Verify you're human", Placement = Placement.Inline)]
    [Action("listen", "LISTEN", Placement = Placement.Inline, Icon = PackIconKind.VolumeMedium)]
    [Action("refresh", "REFRESH", Placement = Placement.Inline, Icon = PackIconKind.Refresh)]
    public string Captcha { get; set; }
}