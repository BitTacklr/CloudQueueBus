using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueErrorConfigurationBuilder : ICloudQueueErrorConfigurationBuilder
    {
        private readonly string _queueName;
        private QueueRequestOptions _queueRequestOptions;
        private TimeSpan? _timeToLive;
        private TimeSpan? _initialVisibilityDelay;
        private int _dequeueCountThreshold;

        public CloudQueueErrorConfigurationBuilder(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            _queueName = queueName;
        }

        public ICloudQueueErrorConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance)
        {
            _queueRequestOptions = instance;
            return this;
        }

        public ICloudQueueErrorConfigurationBuilder WithTimeToLive(TimeSpan? value)
        {
            _timeToLive = value;
            return this;
        }

        public ICloudQueueErrorConfigurationBuilder WithInitialVisibilityDelay(TimeSpan? value)
        {
            _initialVisibilityDelay = value;
            return this;
        }

        public ICloudQueueErrorConfigurationBuilder WithDequeueCountThreshold(int value)
        {
            _dequeueCountThreshold = value;
            return this;
        }

        public ICloudQueueErrorConfiguration Build()
        {
            return new CloudQueueErrorConfiguration
            {
                ErrorQueue = _queueName,
                QueueRequestOptions = _queueRequestOptions,
                InitialVisibilityDelay = _initialVisibilityDelay,
                TimeToLive = _timeToLive,
                DequeueCountThreshold = _dequeueCountThreshold,
            };
        }
    }
}