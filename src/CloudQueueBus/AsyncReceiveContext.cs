using System;

namespace CloudQueueBus
{
    public class AsyncReceiveContext : IConfigureAsyncReceiveContext
    {
        public AsyncReceiveContext()
        {
            From = null;
            To = null;
            MessageId = Guid.Empty;
            RelatesToMessageId = null;
            CorrelationId = Guid.Empty;
            Message = null;
            AsyncSender = null;
        }

        AsyncReceiveContext(string @from, string to, Guid messageId, Guid? relatesToMessageId, Guid correlationId, object message, IAsyncReceiveContextSender asyncSender)
        {
            From = @from;
            To = to;
            MessageId = messageId;
            RelatesToMessageId = relatesToMessageId;
            CorrelationId = correlationId;
            Message = message;
            AsyncSender = asyncSender;
        }

        public string From { get; private set; }
        public string To { get; private set; }
        public Guid MessageId { get; private set; }
        public Guid? RelatesToMessageId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public object Message { get; private set; }
        public IAsyncReceiveContextSender AsyncSender { get; private set; }

        public IConfigureAsyncReceiveContext SetFrom(string value)
        {
            return new AsyncReceiveContext(value, To, MessageId, RelatesToMessageId, CorrelationId, Message, AsyncSender);
        }

        public IConfigureAsyncReceiveContext SetTo(string value)
        {
            return new AsyncReceiveContext(From, value, MessageId, RelatesToMessageId, CorrelationId, Message, AsyncSender);
        }

        public IConfigureAsyncReceiveContext SetMessageId(Guid value)
        {
            return new AsyncReceiveContext(From, To, value, RelatesToMessageId, CorrelationId, Message, AsyncSender);
        }

        public IConfigureAsyncReceiveContext SetRelatesToMessageId(Guid? value)
        {
            return new AsyncReceiveContext(From, To, MessageId, value, CorrelationId, Message, AsyncSender);
        }

        public IConfigureAsyncReceiveContext SetCorrelationId(Guid value)
        {
            return new AsyncReceiveContext(From, To, MessageId, RelatesToMessageId, value, Message, AsyncSender);
        }

        public IConfigureAsyncReceiveContext SetMessage(object value)
        {
            return new AsyncReceiveContext(From, To, MessageId, RelatesToMessageId, CorrelationId, value, AsyncSender);
        }

        public IConfigureAsyncReceiveContext SetAsyncSender(IAsyncReceiveContextSender value)
        {
            return new AsyncReceiveContext(From, To, MessageId, RelatesToMessageId, CorrelationId, Message, value);
        }
    }
}