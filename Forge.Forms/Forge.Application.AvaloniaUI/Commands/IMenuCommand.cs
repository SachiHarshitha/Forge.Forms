using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Application.AvaloniaUI.Commands
{
    public interface IMenuCommand : IRefreshableCommand
    {
        string CommandText { get; }

        PackIconKind? IconKind { get; }
    }
}
