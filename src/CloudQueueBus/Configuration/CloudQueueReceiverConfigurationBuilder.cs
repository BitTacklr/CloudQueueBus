using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueReceiverConfigurationBuilder : ICloudQueueReceiverConfigurationBuilder
    {
        private readonly string _queueName;
        private QueueRequestOptions _queueRequestOptions;
        private TimeSpan? _visibilityTimeout;
        private TimeSpan _delayBetweenIdleReceives;

        public CloudQueueReceiverConfigurationBuilder(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            _queueName = queueName;
            _delayBetweenIdleReceives = TimeSpan.FromSeconds(5.0);
        }

        public ICloudQueueReceiverConfigurationBuilder WithDelayBetweenIdleReceives(TimeSpan value)
        {
            _delayBetweenIdleReceives = value;
            return this;
        }

        public ICloudQueueReceiverConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _queueRequestOptions = instance;
            return this;
        }

        public ICloudQueueReceiverConfigurationBuilder WithVisibilityTimeout(TimeSpan? value)
        {
            _visibilityTimeout = value;
            return this;
        }

        public ICloudQueueReceiverConfiguration Build()
        {
            return new CloudQueueReceiverConfiguration
            {
                ReceiveQueue = _queueName,
                DelayBetweenIdleReceives = _delayBetweenIdleReceives,
                QueueRequestOptions = _queueRequestOptions,
                VisibilityTimeout = _visibilityTimeout
            };
        }
    }
}