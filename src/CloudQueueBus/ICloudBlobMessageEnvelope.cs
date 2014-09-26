using System;

namespace CloudQueueBus
{
    public interface ICloudBlobMessageEnvelope
    {
        Guid BlobId { get; }
        Guid MessageId { get; }
        string ContentType { get; }
        byte[] Content { get; }
        DateTimeOffset Time { get; }
    }
}