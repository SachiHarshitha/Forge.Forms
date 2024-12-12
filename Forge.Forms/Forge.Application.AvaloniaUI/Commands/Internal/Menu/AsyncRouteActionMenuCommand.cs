using System;
using System.Threading.Tasks;
using Forge.Application.AvaloniaUI.Routing;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Application.AvaloniaUI.Commands.Internal.Menu
{
    internal class AsyncRouteActionMenuCommand : AsyncRouteActionCommand, IMenuCommand
    {
        public AsyncRouteActionMenuCommand(Route route, string commandText, PackIconKind? iconKind, Func<Task> execute,
            Func<bool> canExecute)
            : base(route, execute, canExecute)
        {
            this.CommandText = commandText;
            this.IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
