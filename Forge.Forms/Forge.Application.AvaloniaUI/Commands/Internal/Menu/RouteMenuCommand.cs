using System;
using Forge.Application.AvaloniaUI.Routing;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Application.AvaloniaUI.Commands.Internal.Menu
{
    internal class RouteMenuCommand<TParameter> : RouteCommand<TParameter>, IMenuCommand where TParameter : class
    {
        public RouteMenuCommand(Route route, string commandText, PackIconKind? iconKind, Action<TParameter> execute,
            Predicate<TParameter> canExecute, bool ignoreNullParameters)
            : base(route, execute, canExecute, ignoreNullParameters)
        {
            this.CommandText = commandText;
            this.IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
