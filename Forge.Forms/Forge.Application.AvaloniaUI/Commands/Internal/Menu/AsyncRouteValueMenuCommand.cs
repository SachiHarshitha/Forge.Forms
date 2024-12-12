using System;
using System.Threading.Tasks;
using Forge.Application.AvaloniaUI.Routing;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Application.AvaloniaUI.Commands.Internal.Menu
{
    internal class AsyncRouteValueMenuCommand<TParameter> : AsyncRouteValueCommand<TParameter>, IMenuCommand
        where TParameter : struct
    {
        public AsyncRouteValueMenuCommand(Route route, string commandText, PackIconKind? iconKind,
            Func<TParameter, Task> execute, Predicate<object> canExecute)
            : base(route, execute, canExecute)
        {
            this.CommandText = commandText;
            this.IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
