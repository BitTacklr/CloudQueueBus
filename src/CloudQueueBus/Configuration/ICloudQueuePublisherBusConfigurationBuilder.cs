using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueuePublisherBusConfigurationBuilder
    {
        ICloudQueuePublisherBusConfigurationBuilder PublishFrom(string queueName, Action<ICloudQueueSenderConfigurationBuilder> configure);
        ICloudQueuePublisherBusConfigurationBuilder PublishTo(string queueName, Action<ISubscriptionConfigurationBuilder> configure);
        ICloudQueuePublisherBusConfigurationBuilder PublishUsing(ISubscriptionSource source);

        ICloudQueuePublisherBusConfigurationBuilder UsingSerializer(JsonSerializer instance);
        ICloudQueuePublisherBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance);
        ICloudQueuePublisherBusConfigurationBuilder UsingOverflowBlobContainer(string containerName);
        ICloudQueuePublisherConfiguration Build();
    }
}