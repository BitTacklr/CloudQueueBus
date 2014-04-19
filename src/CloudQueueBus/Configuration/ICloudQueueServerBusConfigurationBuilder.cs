using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueServerBusConfigurationBuilder
    {
        ICloudQueueServerBusConfigurationBuilder ReceiveFrom(Uri address, Action<ICloudQueueReceiverConfigurationBuilder> configure);
        ICloudQueueServerBusConfigurationBuilder ObserveOn(IObserver<IReceiveContext> observer);
        ICloudQueueServerBusConfigurationBuilder Accept<TMessage>();
        ICloudQueueServerBusConfigurationBuilder Accept(Type message);
        ICloudQueueServerBusConfigurationBuilder AcceptAll(IEnumerable<Type> messages);
        ICloudQueueServerBusConfigurationBuilder RouteTo(Uri address, Action<IRouteConfigurationBuilder> configure);
        ICloudQueueServerBusConfigurationBuilder RouteUsing(IRouteSource source);
        ICloudQueueServerBusConfigurationBuilder UsingSerializer(JsonSerializer instance);
        ICloudQueueServerBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance);
        ICloudQueueServerBusConfiguration Build();
    }
}