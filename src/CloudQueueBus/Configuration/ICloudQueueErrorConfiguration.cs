using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueErrorConfiguration
    {
        string ErrorQueue { get; }

        QueueRequestOptions QueueRequestOptions { get; }
        TimeSpan? TimeToLive { get; }
        TimeSpan? InitialVisibilityDelay { get; }
        int DequeueCountThreshold { get; }
    }
}