using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudQueueBus.Configuration
{
    public class SubscriptionConfigurationBuilder : ISubscriptionConfigurationBuilder
    {
        private readonly string _queueName;
        private readonly HashSet<Type> _messages;

        public SubscriptionConfigurationBuilder(string queueName)
        {
            if (queueName == null) throw new ArgumentNullException("queueName");
            _queueName = queueName;
            _messages = new HashSet<Type>();
        }

        public ISubscriptionConfigurationBuilder Subscribe<TMessage>()
        {
            _messages.Add(typeof (TMessage));
            return this;
        }

        public ISubscriptionConfigurationBuilder Subscribe(Type message)
        {
            if (message == null) throw new ArgumentNullException("message");
            _messages.Add(message);
            return this;
        }

        public ISubscriptionConfigurationBuilder SubscribeAll(IEnumerable<Type> messages)
        {
            if (messages == null) throw new ArgumentNullException("messages");
            return messages.
                Aggregate<Type, ISubscriptionConfigurationBuilder>(
                    this,
                    (builder, current) => builder.Subscribe(current)
                );
        }

        public Subscription[] Build()
        {
            return _messages.Select(_ => new Subscription(_, _queueName)).ToArray();
        }
    }
}