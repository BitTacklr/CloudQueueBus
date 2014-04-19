using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueServerBusConfigurationBuilder : ICloudQueueServerBusConfigurationBuilder
    {
        private ICloudQueueReceiverConfiguration _receiverConfiguration;
        private JsonSerializer _serializer;
        private CloudStorageAccount _storageAccount;
        private readonly HashSet<Route> _routes;
        private IObserver<IReceiveContext> _observer;
        private readonly HashSet<Type> _messages;

        public CloudQueueServerBusConfigurationBuilder()
        {
            _routes = new HashSet<Route>();
            _messages = new HashSet<Type>();
        }

        public ICloudQueueServerBusConfigurationBuilder ReceiveFrom(Uri address, Action<ICloudQueueReceiverConfigurationBuilder> configure)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueReceiverConfigurationBuilder(address);
            configure(builder);
            _receiverConfiguration = builder.Build();
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder ObserveOn(IObserver<IReceiveContext> observer)
        {
            if (observer == null) throw new ArgumentNullException("observer");
            _observer = observer;
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder Accept<TMessage>()
        {
            _messages.Add(typeof(TMessage));
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder Accept(Type message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _messages.Add(message);
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder AcceptAll(IEnumerable<Type> messages)
        {
            if (messages == null) throw new ArgumentNullException("messages");
            foreach (var message in messages)
            {
                _messages.Add(message);
            }
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder UsingSerializer(JsonSerializer instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _serializer = instance;
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _storageAccount = instance;
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder RouteTo(Uri address, Action<IRouteConfigurationBuilder> configure)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new RouteConfigurationBuilder(address);
            configure(builder);
            foreach (var route in builder.Build())
            {
                _routes.Add(route);
            }
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder RouteUsing(IRouteSource source)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (var route in source.Read())
            {
                _routes.Add(route);
            }
            return this;
        }


        public ICloudQueueServerBusConfiguration Build()
        {
            return new CloudQueueServerBusConfiguration(
                _storageAccount,
                _serializer,
                _observer,
                _receiverConfiguration,
                _messages.ToArray(),
                new CloudQueueSenderConfiguration(
                    _receiverConfiguration.ReceiveAddress,
                    _receiverConfiguration.QueueRequestOptions,
                    null,
                    null), 
                _routes.ToArray());
        }
    }
}