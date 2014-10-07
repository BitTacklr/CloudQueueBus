using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudQueueBus
{
    public interface IAsyncCloudBlobMessageEnvelopeReader
    {
        Task<ICloudBlobMessageEnvelope> ReadAsync(Guid blobId);
        Task<ICloudBlobMessageEnvelope> ReadAsync(Guid blobId, CancellationToken cancellationToken);
    }
}