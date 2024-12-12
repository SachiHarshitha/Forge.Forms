using System;

namespace Forge.Application.AvaloniaUI.Routing
{
    public class RouteEventError
    {
        public RouteEventError(RouteEventType routeEventType, Exception exception)
        {
            this.RouteEventType = routeEventType;
            this.Exception = exception;
        }

        public RouteEventType RouteEventType { get; }

        public Exception Exception { get; }
    }
}
