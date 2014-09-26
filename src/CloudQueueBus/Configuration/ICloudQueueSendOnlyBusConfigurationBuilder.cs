using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueSendOnlyBusConfigurationBuilder
    {
        ICloudQueueSendOnlyBusConfigurationBuilder SendFrom(string queueName, Action<ICloudQueueSenderConfigurationBuilder> configure);
        ICloudQueueSendOnlyBusConfigurationBuilder RouteTo(string queueName, Action<IRouteConfigurationBuilder> configure);
        ICloudQueueSendOnlyBusConfigurationBuilder RouteUsing(IRouteSource source);
        ICloudQueueSendOnlyBusConfigurationBuilder UsingSerializer(JsonSerializer instance);
        ICloudQueueSendOnlyBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance);
        ICloudQueueSendOnlyBusConfigurationBuilder UsingOverflowBlobContainer(string containerName);
        ICloudQueueSendOnlyBusConfiguration Build();
    }
}