using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudQueueBus.Configuration
{
    public class RouteConfigurationBuilder : IRouteConfigurationBuilder
    {
        private readonly string _queueName;
        private readonly HashSet<Type> _messages;
        public RouteConfigurationBuilder(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            _queueName = queueName;
            _messages = new HashSet<Type>();
        }

        public IRouteConfigurationBuilder Route<TMessage>()
        {
            _messages.Add(typeof (TMessage));
            return this;
        }

        public IRouteConfigurationBuilder Route(Type message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _messages.Add(message);
            return this;
        }

        public IRouteConfigurationBuilder RouteAll(IEnumerable<Type> messages)
        {
            if (messages == null) throw new ArgumentNullException("messages");
            foreach (var message in messages)
            {
                _messages.Add(message);
            }
            return this;
        }

        public Route[] Build()
        {
            return _messages.Select(_ => new Route(_, _queueName)).ToArray();
        }
    }
}