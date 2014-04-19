using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueReceiverConfigurationBuilder : ICloudQueueReceiverConfigurationBuilder
    {
        private readonly Uri _address;
        private QueueRequestOptions _queueRequestOptions;
        private int _batchCount;
        private TimeSpan? _visibilityTimeout;
        private TimeSpan _delayBetweenIdleReceives;

        public CloudQueueReceiverConfigurationBuilder(Uri address)
        {
            _address = address;
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

        public ICloudQueueReceiverConfigurationBuilder WithBatchCount(int value)
        {
            _batchCount = value;
            return this;
        }

        public ICloudQueueReceiverConfigurationBuilder WithVisibilityTimeout(TimeSpan? value)
        {
            _visibilityTimeout = value;
            return this;
        }

        public ICloudQueueReceiverConfiguration Build()
        {
            return new CloudQueueReceiverConfiguration(
                _address,
                _delayBetweenIdleReceives,
                _queueRequestOptions,
                _batchCount,
                _visibilityTimeout);
        }
    }
}