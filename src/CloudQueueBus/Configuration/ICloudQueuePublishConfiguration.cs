using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueuePublishConfiguration
    {
        Uri PublishAddress { get; }

        QueueRequestOptions QueueRequestOptions { get; }
        TimeSpan? TimeToLive { get; }
        TimeSpan? InitialVisibilityDelay { get; }

        Subscription[] Subscriptions { get; }
    }
}