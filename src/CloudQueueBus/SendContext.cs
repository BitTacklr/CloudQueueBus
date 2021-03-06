using System;

namespace CloudQueueBus
{
    public class SendContext : IConfigureSendContext
    {
        public SendContext()
        {
            From = null;
            To = null;
            MessageId = Guid.Empty;
            RelatesToMessageId = null;
            CorrelationId = Guid.Empty;
            Message = null;
        }

        SendContext(string @from, string to, Guid messageId, Guid? relatesToMessageId, Guid correlationId, object message)
        {
            From = @from;
            To = to;
            MessageId = messageId;
            RelatesToMessageId = relatesToMessageId;
            CorrelationId = correlationId;
            Message = message;
        }

        public string From { get; private set; }
        public string To { get; private set; }
        public Guid MessageId { get; private set; }
        public Guid? RelatesToMessageId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public object Message { get; private set; }

        public IConfigureSendContext SetFrom(string value)
        {
            return new SendContext(value, To, MessageId, RelatesToMessageId, CorrelationId, Message);
        }

        public IConfigureSendContext SetTo(string value)
        {
            return new SendContext(From, value, MessageId, RelatesToMessageId, CorrelationId, Message);
        }

        public IConfigureSendContext SetMessageId(Guid value)
        {
            return new SendContext(From, To, value, RelatesToMessageId, CorrelationId, Message);
        }

        public IConfigureSendContext SetRelatesToMessageId(Guid? value)
        {
            return new SendContext(From, To, MessageId, value, CorrelationId, Message);
        }

        public IConfigureSendContext SetCorrelationId(Guid value)
        {
            return new SendContext(From, To, MessageId, RelatesToMessageId, value, Message);
        }

        public IConfigureSendContext SetMessage(object value)
        {
            return new SendContext(From, To, MessageId, RelatesToMessageId, CorrelationId, value);
        }
    }
}