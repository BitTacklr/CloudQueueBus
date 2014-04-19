using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueuePublisherBusConfigurationBuilder
    {
        ICloudQueuePublisherBusConfigurationBuilder PublishFrom(Uri address, Action<ICloudQueueSenderConfigurationBuilder> configure);
        ICloudQueuePublisherBusConfigurationBuilder PublishTo(Uri address, Action<ISubscriptionConfigurationBuilder> configure);
        ICloudQueuePublisherBusConfigurationBuilder PublishUsing(ISubscriptionSource source);

        ICloudQueuePublisherBusConfigurationBuilder UsingSerializer(JsonSerializer instance);
        ICloudQueuePublisherBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance);
        ICloudQueuePublisherConfiguration Build();
    }
}