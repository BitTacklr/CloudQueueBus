using System;

namespace CloudQueueBus
{
    public interface ICloudQueueMessageEnvelope
    {
        Uri From { get; }
        Uri To { get; }
        Guid MessageId { get; }
        Guid? RelatesToMessageId { get; }
        Guid CorrelationId { get; }
        string ContentType { get; }
        byte[] Content { get; }
    }
}