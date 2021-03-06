using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueSenderConfiguration : ICloudQueueSenderConfiguration
    {
        public string FromQueue { get; set; }
        public QueueRequestOptions QueueRequestOptions { get; set; }
        public TimeSpan? TimeToLive { get; set; }
        public TimeSpan? InitialVisibilityDelay { get; set; }
    }
}