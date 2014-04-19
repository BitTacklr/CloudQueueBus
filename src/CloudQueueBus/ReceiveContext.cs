using System;

namespace CloudQueueBus
{
    public class ReceiveContext : IConfigureReceiveContext
    {
        public ReceiveContext()
        {
            From = null;
            To = null;
            MessageId = Guid.Empty;
            RelatesToMessageId = null;
            CorrelationId = Guid.Empty;
            Message = null;
            Sender = null;
        }

        ReceiveContext(Uri @from, Uri to, Guid messageId, Guid? relatesToMessageId, Guid correlationId, object message, IReceiveContextSender sender)
        {
            From = @from;
            To = to;
            MessageId = messageId;
            RelatesToMessageId = relatesToMessageId;
            CorrelationId = correlationId;
            Message = message;
            Sender = sender;
        }

        public Uri From { get; private set; }
        public Uri To { get; private set; }
        public Guid MessageId { get; private set; }
        public Guid? RelatesToMessageId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public object Message { get; private set; }
        public IReceiveContextSender Sender { get; private set; }

        public IConfigureReceiveContext SetFrom(Uri value)
        {
            return new ReceiveContext(value, To, MessageId, RelatesToMessageId, CorrelationId, Message, Sender);
        }

        public IConfigureReceiveContext SetTo(Uri value)
        {
            return new ReceiveContext(From, value, MessageId, RelatesToMessageId, CorrelationId, Message, Sender);
        }

        public IConfigureReceiveContext SetMessageId(Guid value)
        {
            return new ReceiveContext(From, To, value, RelatesToMessageId, CorrelationId, Message, Sender);
        }

        public IConfigureReceiveContext SetRelatesToMessageId(Guid? value)
        {
            return new ReceiveContext(From, To, MessageId, value, CorrelationId, Message, Sender);
        }

        public IConfigureReceiveContext SetCorrelationId(Guid value)
        {
            return new ReceiveContext(From, To, MessageId, RelatesToMessageId, value, Message, Sender);
        }

        public IConfigureReceiveContext SetMessage(object value)
        {
            return new ReceiveContext(From, To, MessageId, RelatesToMessageId, CorrelationId, value, Sender);
        }

        public IConfigureReceiveContext SetSender(IReceiveContextSender value)
        {
            return new ReceiveContext(From, To, MessageId, RelatesToMessageId, CorrelationId, Message, value);
        }
    }
}
