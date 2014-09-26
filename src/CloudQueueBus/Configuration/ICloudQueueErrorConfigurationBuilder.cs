using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueErrorConfigurationBuilder
    {
        ICloudQueueErrorConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance);
        ICloudQueueErrorConfigurationBuilder WithTimeToLive(TimeSpan? value);
        ICloudQueueErrorConfigurationBuilder WithInitialVisibilityDelay(TimeSpan? value);
        ICloudQueueErrorConfigurationBuilder WithDequeueCountThreshold(int value);
    }
}