using System;

namespace CloudQueueBus
{
    public interface ICloudQueueMessageEnvelope
    {
        string From { get; }
        string To { get; }
        Guid MessageId { get; }
        Guid? RelatesToMessageId { get; }
        Guid CorrelationId { get; }
        string ContentType { get; }
        byte[] Content { get; }
        DateTimeOffset Time { get; }

        IConfigureCloudBlobMessageEnvelope ToBlobEnvelope(Guid blobId);
    }
}