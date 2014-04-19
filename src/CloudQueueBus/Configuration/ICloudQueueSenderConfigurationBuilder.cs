using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueSenderConfigurationBuilder
    {
        ICloudQueueSenderConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance);
        ICloudQueueSenderConfigurationBuilder WithTimeToLive(TimeSpan? value);
        ICloudQueueSenderConfigurationBuilder WithInitialVisibilityDelay(TimeSpan? value);
    }
}