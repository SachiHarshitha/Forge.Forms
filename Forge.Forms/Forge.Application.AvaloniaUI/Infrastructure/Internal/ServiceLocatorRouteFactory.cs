using System;
using System.Collections.Generic;
using Forge.Application.AvaloniaUI.Routing;
using Forge.Application.AvaloniaUI.Routing.Internal;

namespace Forge.Application.AvaloniaUI.Infrastructure.Internal
{
    internal class ServiceLocatorRouteFactory : IRouteFactory
    {
        private readonly IServiceLocator serviceLocator;

        public ServiceLocatorRouteFactory(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public Route Get(Type routeType, IDictionary<string, object> parameters)
            => (Route)this.serviceLocator.Get(routeType, parameters);

        public IRouteWrapper<Route> Get(Route caller, Type routeType, IDictionary<string, object> parameters)
            => new RouteWrapperInternal<Route>(caller, (Route)this.serviceLocator.Get(routeType, parameters));

        public TRoute Get<TRoute>(IDictionary<string, object> parameters) where TRoute : Route
            => this.serviceLocator.Get<TRoute>(parameters);

        public IRouteWrapper<TRoute> Get<TRoute>(Route caller, IDictionary<string, object> parameters)
            where TRoute : Route => new RouteWrapperInternal<TRoute>(caller, this.serviceLocator.Get<TRoute>(parameters));
    }
}
