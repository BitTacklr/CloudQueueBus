using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueSenderConfiguration
    {
        string FromQueue { get; }

        QueueRequestOptions QueueRequestOptions { get; }
        TimeSpan? TimeToLive { get; }
        TimeSpan? InitialVisibilityDelay { get; }
    }
}