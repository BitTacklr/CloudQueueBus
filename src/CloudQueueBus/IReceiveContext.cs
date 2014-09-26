using System;

namespace CloudQueueBus
{
    public interface IReceiveContext
    {
        string From { get; }
        string To { get; }
        Guid MessageId { get; }
        Guid? RelatesToMessageId { get; }
        Guid CorrelationId { get; }
        object Message { get; }

        IReceiveContextSender Sender { get; }
    }
}