using System;

namespace CloudQueueBus
{
    public interface ICloudBlobMessageEnvelopeReader
    {
        ICloudBlobMessageEnvelope Read(Guid blobId);
    }
}