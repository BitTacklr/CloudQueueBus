using System;
using System.Collections.Generic;

namespace CloudQueueBus.Configuration
{
    public interface IRouteConfigurationBuilder
    {
        IRouteConfigurationBuilder Route<TMessage>();
        IRouteConfigurationBuilder Route(Type message);
        IRouteConfigurationBuilder RouteAll(IEnumerable<Type> messages);
    }
}