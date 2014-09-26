using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueSenderConfigurationBuilder : ICloudQueueSenderConfigurationBuilder
    {
        private readonly string _queueName;
        private QueueRequestOptions _queueRequestOptions;
        private TimeSpan? _timeToLive;
        private TimeSpan? _initialVisibilityDelay;

        public CloudQueueSenderConfigurationBuilder(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            _queueName = queueName;
        }

        public ICloudQueueSenderConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance)
        {
            _queueRequestOptions = instance;
            return this;
        }

        public ICloudQueueSenderConfigurationBuilder WithTimeToLive(TimeSpan? value)
        {
            _timeToLive = value;
            return this;
        }

        public ICloudQueueSenderConfigurationBuilder WithInitialVisibilityDelay(TimeSpan? value)
        {
            _initialVisibilityDelay = value;
            return this;
        }

        public ICloudQueueSenderConfiguration Build()
        {
            return new CloudQueueSenderConfiguration
            {
                FromQueue = _queueName,
                QueueRequestOptions = _queueRequestOptions,
                InitialVisibilityDelay = _initialVisibilityDelay,
                TimeToLive = _timeToLive
            };
        }
    }
}