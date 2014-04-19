using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueSendOnlyBusConfigurationBuilder : ICloudQueueSendOnlyBusConfigurationBuilder
    {
        private ICloudQueueSenderConfiguration _senderConfiguration;
        private JsonSerializer _serializer;
        private CloudStorageAccount _storageAccount;
        private readonly HashSet<Route> _routes;

        public CloudQueueSendOnlyBusConfigurationBuilder()
        {
            _routes = new HashSet<Route>();
        }

        public ICloudQueueSendOnlyBusConfigurationBuilder SendFrom(Uri address, Action<ICloudQueueSenderConfigurationBuilder> configure)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (configure == null) throw new ArgumentNullException("configure");
            var builder = new CloudQueueSenderConfigurationBuilder(address);
            configure(builder);
            _senderConfiguration = builder.Build();
            return this;
        }

        public ICloudQueueSendOnlyBusConfigurationBuilder UsingSerializer(JsonSerializer instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _serializer = instance;
            return this;
        }

        public ICloudQueueSendOnlyBusConfigurationBuilder UsingStorageAccount(CloudStorageAccount instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _storageAccount = instance;
            return this;
        }

        public ICloudQueueSendOnlyBusConfigurationBuilder RouteTo(Uri address, Action<IRouteConfigurationBuilder> configure)
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

        public ICloudQueueSendOnlyBusConfigurationBuilder RouteUsing(IRouteSource source)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (var route in source.Read())
            {
                _routes.Add(route);
            }
            return this;
        }


        public ICloudQueueSendOnlyBusConfiguration Build()
        {
            return new CloudQueueSendOnlyBusConfiguration(
                _storageAccount,
                _serializer,
                _senderConfiguration,
                _routes.ToArray());
        }
    }
}