using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudQueueBus.Configuration
{
    public class RouteConfigurationBuilder : IRouteConfigurationBuilder
    {
        private readonly Uri _address;
        private readonly HashSet<Type> _messages;
        public RouteConfigurationBuilder(Uri address)
        {
            if (address == null) throw new ArgumentNullException("address");
            _address = address;
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
            return _messages.Select(_ => new Route(_, _address)).ToArray();
        }
    }
}