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
        private ICloudQueueErrorConfiguration _errorConfiguration;
        private JsonSerializer _serializer;
        private CloudStorageAccount _storageAccount;
        private readonly HashSet<Route> _routes;
        private IAsyncHandler<IAsyncReceiveContext> _handler;
        private readonly HashSet<Type> _messages;
        private string _overflowBlobContainerName;

        public CloudQueueServerBusConfigurationBuilder()
        {
            _routes = new HashSet<Route>();
            _messages = new HashSet<Type>();
        }

        public ICloudQueueServerBusConfigurationBuilder ReceiveFrom(string queueName, Action<ICloudQueueReceiverConfigurationBuilder> configure)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueReceiverConfigurationBuilder(queueName);
            configure(builder);
            _receiverConfiguration = builder.Build();
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder ErrorTo(string queueName, Action<ICloudQueueErrorConfigurationBuilder> configure)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueErrorConfigurationBuilder(queueName);
            configure(builder);
            _errorConfiguration = builder.Build();
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder HandleUsing(IAsyncHandler<IAsyncReceiveContext> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            _handler = handler;
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

        public ICloudQueueServerBusConfigurationBuilder UsingOverflowBlobContainer(string containerName)
        {
            if (containerName == null) throw new ArgumentNullException("containerName");
            _overflowBlobContainerName = containerName;
            return this;
        }

        public ICloudQueueServerBusConfigurationBuilder RouteTo(string queueName, Action<IRouteConfigurationBuilder> configure)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new RouteConfigurationBuilder(queueName);
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
                _handler,
                _receiverConfiguration,
                _messages.ToArray(),
                new CloudQueueSenderConfiguration
                {
                    FromQueue = _receiverConfiguration.ReceiveQueue,
                    QueueRequestOptions = _receiverConfiguration.QueueRequestOptions
                }, 
                _routes.ToArray(),
                _errorConfiguration, 
                _overflowBlobContainerName);
        }
    }
}