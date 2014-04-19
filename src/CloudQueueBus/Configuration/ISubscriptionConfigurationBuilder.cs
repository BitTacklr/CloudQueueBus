using System;
using System.Collections.Generic;

namespace CloudQueueBus.Configuration
{
    public interface ISubscriptionConfigurationBuilder
    {
        ISubscriptionConfigurationBuilder Subscribe<TMessage>();
        ISubscriptionConfigurationBuilder Subscribe(Type message);
        ISubscriptionConfigurationBuilder SubscribeAll(IEnumerable<Type> messages);
    }
}