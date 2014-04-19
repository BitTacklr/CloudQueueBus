using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class CloudQueuePublisherBusConfigurationBuilder : ICloudQueuePublisherBusConfigurationBuilder
    {
        private JsonSerializer _serializer;
        private CloudStorageAccount _storageAccount;
        private ICloudQueueSenderConfiguration _senderConfiguration;
        private readonly HashSet<Subscription> _subscriptions;

        public CloudQueuePublisherBusConfigurationBuilder()
        {
            _senderConfiguration = null;
            _serializer = null;
            _storageAccount = null;
            _subscriptions = new HashSet<Subscription>();
        }

        public ICloudQueuePublisherBusConfigurationBuilder PublishFrom(Uri address, Action<ICloudQueueSenderConfigurationBuilder> configure)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueSenderConfigurationBuilder(address);
            configure(builder);
            _senderConfiguration = builder.Build();
            return this;
        }

        public ICloudQueuePublisherBusConfigurationBuilder PublishTo(Uri address,
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

        public ICloudQueuePublisherBusConfigurationBuilder PublishUsing(ISubscriptionSource source)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (var subscription in source.Read())
            {
                _subscriptions.Add(subscription);
            }
            return this;
        }

        public ICloudQueuePublisherBusConfigurationBuilder UsingSerializer(JsonSerializer instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _serializer = instance;
            return this;
        }

        public ICloudQueuePublisherBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _storageAccount = instance;
            return this;
        }

        public ICloudQueuePublisherConfiguration Build()
        {
            return new CloudQueuePublisherConfiguration(
                _storageAccount,
                _serializer,
                _subscriptions.ToArray(),
                _senderConfiguration);
        }
    }
}