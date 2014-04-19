using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueSenderConfiguration : ICloudQueueSenderConfiguration
    {
        public CloudQueueSenderConfiguration(Uri from, QueueRequestOptions queueRequestOptions, TimeSpan? timeToLive, TimeSpan? initialVisibilityDelay)
        {
            if (@from == null) throw new ArgumentNullException("from");
            if (queueRequestOptions == null) throw new ArgumentNullException("queueRequestOptions");
            FromAddress = @from;
            QueueRequestOptions = queueRequestOptions;
            TimeToLive = timeToLive;
            InitialVisibilityDelay = initialVisibilityDelay;
        }

        public Uri FromAddress { get; private set; }
        public QueueRequestOptions QueueRequestOptions { get; private set; }
        public TimeSpan? TimeToLive { get; private set; }
        public TimeSpan? InitialVisibilityDelay { get; private set; }
    }
}