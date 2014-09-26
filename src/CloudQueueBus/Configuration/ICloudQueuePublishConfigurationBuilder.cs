using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueuePublishConfigurationBuilder
    {
        ICloudQueuePublishConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance);
        ICloudQueuePublishConfigurationBuilder WithTimeToLive(TimeSpan? value);
        ICloudQueuePublishConfigurationBuilder WithInitialVisibilityDelay(TimeSpan? value);

        ICloudQueuePublishConfigurationBuilder SubscribeAt(string queueName, Action<ISubscriptionConfigurationBuilder> configure);
        ICloudQueuePublishConfigurationBuilder SubscribeUsing(ISubscriptionSource source);
    }
}