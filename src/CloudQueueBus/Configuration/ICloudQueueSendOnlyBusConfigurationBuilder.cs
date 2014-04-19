using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueSendOnlyBusConfigurationBuilder
    {
        ICloudQueueSendOnlyBusConfigurationBuilder SendFrom(Uri address, Action<ICloudQueueSenderConfigurationBuilder> configure);
        ICloudQueueSendOnlyBusConfigurationBuilder RouteTo(Uri address, Action<IRouteConfigurationBuilder> configure);
        ICloudQueueSendOnlyBusConfigurationBuilder RouteUsing(IRouteSource source);
        ICloudQueueSendOnlyBusConfigurationBuilder UsingSerializer(JsonSerializer instance);
        ICloudQueueSendOnlyBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance);
        ICloudQueueSendOnlyBusConfiguration Build();
    }
}