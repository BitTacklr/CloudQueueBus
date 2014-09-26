using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueReceiverConfiguration
    {
        string ReceiveQueue { get; }
        QueueRequestOptions QueueRequestOptions { get; }
        TimeSpan? VisibilityTimeout { get; }
        TimeSpan DelayBetweenIdleReceives { get; }
    }
}