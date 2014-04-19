using System.Collections.Generic;

namespace CloudQueueBus.Configuration
{
    public interface ISubscriptionSource
    {
        IEnumerable<Subscription> Read();
    }
}