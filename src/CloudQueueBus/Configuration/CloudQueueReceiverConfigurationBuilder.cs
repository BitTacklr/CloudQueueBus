using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueReceiverConfigurationBuilder : ICloudQueueReceiverConfigurationBuilder
    {
        private readonly string _queueName;
        private QueueRequestOptions _receiveQueueRequestOptions;
        private TimeSpan? _receiveQueueVisibilityTimeout;
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
            _receiveQueueRequestOptions = instance;
            return this;
        }

        public ICloudQueueReceiverConfigurationBuilder WithQueueVisibilityTimeout(TimeSpan? value)
        {
            _receiveQueueVisibilityTimeout = value;
            return this;
        }

        public ICloudQueueReceiverConfiguration Build()
        {
            return new CloudQueueReceiverConfiguration
            {
                ReceiveQueue = _queueName,
                DelayBetweenIdleReceives = _delayBetweenIdleReceives,
                QueueRequestOptions = _receiveQueueRequestOptions,
                VisibilityTimeout = _receiveQueueVisibilityTimeout
            };
        }
    }
}