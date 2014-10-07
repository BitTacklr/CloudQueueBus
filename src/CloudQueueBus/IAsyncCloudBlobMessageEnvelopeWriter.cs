using System.Threading;
using System.Threading.Tasks;

namespace CloudQueueBus
{
    public interface IAsyncCloudBlobMessageEnvelopeWriter
    {
        Task WriteAsync(IConfigureCloudBlobMessageEnvelope envelope);
        Task WriteAsync(IConfigureCloudBlobMessageEnvelope envelope, CancellationToken cancellationToken);
    }
}