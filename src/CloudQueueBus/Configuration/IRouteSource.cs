using System.Collections.Generic;

namespace CloudQueueBus.Configuration
{
    public interface IRouteSource
    {
        IEnumerable<Route> Read();
    }
}