using System;
using System.Threading.Tasks;
using Forge.Application.AvaloniaUI.Routing;
using MaterialDesign.Avalonia.PackIcon;

namespace Forge.Application.AvaloniaUI.Commands.Internal.Menu
{
    internal class AsyncRouteMenuCommand<TParameter> : AsyncRouteCommand<TParameter>, IMenuCommand
        where TParameter : class
    {
        public AsyncRouteMenuCommand(Route route, string commandText, PackIconKind? iconKind,
            Func<TParameter, Task> execute, Predicate<TParameter> canExecute, bool ignoreNullParameters)
            : base(route, execute, canExecute, ignoreNullParameters)
        {
            this.CommandText = commandText;
            this.IconKind = iconKind;
        }

        public string CommandText { get; }

        public PackIconKind? IconKind { get; }
    }
}
