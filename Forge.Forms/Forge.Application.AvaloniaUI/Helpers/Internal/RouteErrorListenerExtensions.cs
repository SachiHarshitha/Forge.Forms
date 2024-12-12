using System;
using Forge.Application.AvaloniaUI.Routing;

namespace Forge.Application.AvaloniaUI.Helpers.Internal
{
    internal static class RouteErrorListenerExtensions
    {
        public static void TryOnRouteEventException(this IRouteErrorListener listener, Route route,
            RouteEventType eventType, Exception exception)
        {
            if (listener == null)
            {
                return;
            }

            try
            {
                listener.OnRouteEventException(route, eventType, exception);
            }
            catch
            {
                // ignored
            }
        }
    }
}
