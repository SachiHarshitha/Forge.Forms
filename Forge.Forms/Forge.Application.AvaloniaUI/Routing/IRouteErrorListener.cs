using System;
using System.Windows.Input;

namespace Forge.Application.AvaloniaUI.Routing
{
    public interface IRouteErrorListener
    {
        void OnRouteEventException(Route route, RouteEventType eventType, Exception exception);

        void OnRouteCommandException(Route route, ICommand command, Exception exception);
    }
}
