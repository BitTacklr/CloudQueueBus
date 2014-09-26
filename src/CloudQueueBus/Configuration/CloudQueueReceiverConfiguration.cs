using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueReceiverConfiguration : ICloudQueueReceiverConfiguration
    {
        public string ReceiveQueue { get; set; }
        public QueueRequestOptions QueueRequestOptions { get; set; }
        public TimeSpan? VisibilityTimeout { get; set; }
        public TimeSpan DelayBetweenIdleReceives { get; set; }
    }
}