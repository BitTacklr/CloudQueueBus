using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueSenderConfiguration
    {
        Uri FromAddress { get; }

        QueueRequestOptions QueueRequestOptions { get; }
        TimeSpan? TimeToLive { get; }
        TimeSpan? InitialVisibilityDelay { get; }
    }
}