using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueSenderConfigurationBuilder : ICloudQueueSenderConfigurationBuilder
    {
        private readonly Uri _address;
        private QueueRequestOptions _queueRequestOptions;
        private TimeSpan? _timeToLive;
        private TimeSpan? _initialVisibilityDelay;

        public CloudQueueSenderConfigurationBuilder(Uri address)
        {
            if (address == null) throw new ArgumentNullException("address");
            _address = address;
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
            return new CloudQueueSenderConfiguration(
                _address,
                _queueRequestOptions,
                _timeToLive,
                _initialVisibilityDelay);
        }
    }
}