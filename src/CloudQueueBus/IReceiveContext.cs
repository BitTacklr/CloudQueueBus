using System;

namespace CloudQueueBus
{
    public interface IReceiveContext
    {
        Uri From { get; }
        Uri To { get; }
        Guid MessageId { get; }
        Guid? RelatesToMessageId { get; }
        Guid CorrelationId { get; }
        object Message { get; }

        IReceiveContextSender Sender { get; }
    }
}