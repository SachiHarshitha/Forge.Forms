using System;
using Forge.Application.AvaloniaUI.Routing;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Application.AvaloniaUI.Commands.Internal.Menu
{
    internal class RouteValueMenuCommand<TParameter> : RouteValueCommand<TParameter>, IMenuCommand
        where TParameter : struct
    {
        public RouteValueMenuCommand(Route route, string commandText, PackIconKind? iconKind, Action<TParameter> execute,
            Predicate<object> canExecute)
            : base(route, execute, canExecute)
        {
            this.CommandText = commandText;
            this.IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
