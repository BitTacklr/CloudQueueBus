using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueErrorConfiguration : ICloudQueueErrorConfiguration
    {
        public string ErrorQueue { get; set; }
        public QueueRequestOptions QueueRequestOptions { get; set; }
        public TimeSpan? TimeToLive { get; set; }
        public TimeSpan? InitialVisibilityDelay { get; set; }
        public int DequeueCountThreshold { get; set; }
    }
}