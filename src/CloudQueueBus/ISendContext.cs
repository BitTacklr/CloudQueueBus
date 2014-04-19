using System;

namespace CloudQueueBus
{
    public interface ISendContext
    {
        Uri From { get; }
        Uri To { get; }
        Guid MessageId { get; }
        Guid? RelatesToMessageId { get; }
        Guid CorrelationId { get; }
        object Message { get; }
    }
}