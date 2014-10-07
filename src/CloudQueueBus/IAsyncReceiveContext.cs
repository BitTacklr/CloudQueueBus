using System;

namespace CloudQueueBus
{
    public interface IAsyncReceiveContext
    {
        string From { get; }
        string To { get; }
        Guid MessageId { get; }
        Guid? RelatesToMessageId { get; }
        Guid CorrelationId { get; }
        object Message { get; }

        IAsyncReceiveContextSender AsyncSender { get; }
    }
}