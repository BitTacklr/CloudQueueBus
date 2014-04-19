using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueuePublishConfiguration : ICloudQueuePublishConfiguration
    {
        public CloudQueuePublishConfiguration(
            Uri address,
            QueueRequestOptions queueRequestOptions, 
            TimeSpan? timeToLive, 
            TimeSpan? initialVisibilityDelay,
            Subscription[] subscriptions)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (queueRequestOptions == null) throw new ArgumentNullException("queueRequestOptions");
            if (subscriptions == null) throw new ArgumentNullException("subscriptions");
            PublishAddress = address;
            QueueRequestOptions = queueRequestOptions;
            TimeToLive = timeToLive;
            InitialVisibilityDelay = initialVisibilityDelay;
            Subscriptions = subscriptions;
        }

        public Uri PublishAddress { get; private set; }
        public Subscription[] Subscriptions { get; private set; }

        public QueueRequestOptions QueueRequestOptions { get; private set; }
        public TimeSpan? TimeToLive { get; private set; }
        public TimeSpan? InitialVisibilityDelay { get; private set; }
    }
}