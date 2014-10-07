using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueServerBusConfigurationBuilder
    {
        ICloudQueueServerBusConfigurationBuilder ReceiveFrom(string queueName, Action<ICloudQueueReceiverConfigurationBuilder> configure);
        ICloudQueueServerBusConfigurationBuilder ErrorTo(string queueName, Action<ICloudQueueErrorConfigurationBuilder> configure);
        ICloudQueueServerBusConfigurationBuilder HandleUsing(IAsyncHandler<IAsyncReceiveContext> handler);
        ICloudQueueServerBusConfigurationBuilder Accept<TMessage>();
        ICloudQueueServerBusConfigurationBuilder Accept(Type message);
        ICloudQueueServerBusConfigurationBuilder AcceptAll(IEnumerable<Type> messages);
        ICloudQueueServerBusConfigurationBuilder RouteTo(string queueName, Action<IRouteConfigurationBuilder> configure);
        ICloudQueueServerBusConfigurationBuilder RouteUsing(IRouteSource source);
        ICloudQueueServerBusConfigurationBuilder UsingSerializer(JsonSerializer instance);
        ICloudQueueServerBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance);
        ICloudQueueServerBusConfigurationBuilder UsingOverflowBlobContainer(string containerName);
        ICloudQueueServerBusConfiguration Build();
    }
}