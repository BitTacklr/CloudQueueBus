using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueReceiverConfigurationBuilder
    {
        ICloudQueueReceiverConfigurationBuilder WithDelayBetweenIdleReceives(TimeSpan value);
        ICloudQueueReceiverConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance);
        ICloudQueueReceiverConfigurationBuilder WithQueueVisibilityTimeout(TimeSpan? value);
    }
}