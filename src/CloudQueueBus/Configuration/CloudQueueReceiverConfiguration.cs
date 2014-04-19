using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueReceiverConfiguration : ICloudQueueReceiverConfiguration
    {
        public CloudQueueReceiverConfiguration(Uri address, TimeSpan delayBetweenIdleReceives, QueueRequestOptions queueRequestOptions, int batchCount, TimeSpan? visibilityTimeout)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (queueRequestOptions == null) throw new ArgumentNullException("queueRequestOptions");
            ReceiveAddress = address;
            DelayBetweenIdleReceives = delayBetweenIdleReceives;
            QueueRequestOptions = queueRequestOptions;
            BatchCount = batchCount;
            VisibilityTimeout = visibilityTimeout;
        }

        public Uri ReceiveAddress { get; private set; }
        public TimeSpan DelayBetweenIdleReceives { get; private set; }
        public QueueRequestOptions QueueRequestOptions { get; private set; }
        public int BatchCount { get; private set; }
        public TimeSpan? VisibilityTimeout { get; private set; }
    }
}