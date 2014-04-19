using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueReceiverConfiguration
    {
        Uri ReceiveAddress { get; }

        QueueRequestOptions QueueRequestOptions { get; }
        int BatchCount { get; }
        TimeSpan? VisibilityTimeout { get; }

        TimeSpan DelayBetweenIdleReceives { get; }
    }
}