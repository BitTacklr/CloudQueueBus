using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus.Configuration
{
    public class CloudQueuePublishConfigurationBuilder : ICloudQueuePublishConfigurationBuilder
    {
        private readonly Uri _address;
        private QueueRequestOptions _queueRequestOptions;
        private TimeSpan? _timeToLive;
        private TimeSpan? _initialVisibilityDelay;
        private readonly HashSet<Subscription> _subscriptions;

        public CloudQueuePublishConfigurationBuilder(Uri address)
        {
            if (address == null) throw new ArgumentNullException("address");
            _address = address;
            _subscriptions = new HashSet<Subscription>();
        }

        public ICloudQueuePublishConfigurationBuilder WithQueueRequestOptions(QueueRequestOptions instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _queueRequestOptions = instance;
            return this;
        }

        public ICloudQueuePublishConfigurationBuilder WithTimeToLive(TimeSpan? value)
        {
            _timeToLive = value;
            return this;
        }

        public ICloudQueuePublishConfigurationBuilder WithInitialVisibilityDelay(TimeSpan? value)
        {
            _initialVisibilityDelay = value;
            return this;
        }

        public ICloudQueuePublishConfigurationBuilder SubscribeAt(Uri address,
            Action<ISubscriptionConfigurationBuilder> configure)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new SubscriptionConfigurationBuilder(address);
            configure(builder);
            foreach (var subscription in builder.Build())
            {
                _subscriptions.Add(subscription);
            }
            return this;
        }

        public ICloudQueuePublishConfigurationBuilder SubscribeUsing(ISubscriptionSource source)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (var subscription in source.Read())
            {
                _subscriptions.Add(subscription);
            }
            return this;
        }

        public ICloudQueuePublishConfiguration Build()
        {
            return new CloudQueuePublishConfiguration(
                _address,
                _queueRequestOptions,
                _timeToLive,
                _initialVisibilityDelay,
                _subscriptions.ToArray());
        }
    }
}