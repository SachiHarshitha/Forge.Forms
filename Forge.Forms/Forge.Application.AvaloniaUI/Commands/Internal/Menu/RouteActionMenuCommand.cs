using System;
using Forge.Application.AvaloniaUI.Routing;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Application.AvaloniaUI.Commands.Internal.Menu
{
    internal class RouteActionMenuCommand : RouteActionCommand, IMenuCommand
    {
        public RouteActionMenuCommand(Route route, string commandText, PackIconKind? iconKind, Action execute,
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
