using System.Threading;
using System.Threading.Tasks;

namespace CloudQueueBus
{
    public interface IAsyncCloudQueueMessageEnvelopeSender
    {
        Task SendAsync(IConfigureCloudQueueMessageEnvelope envelope);
        Task SendAsync(IConfigureCloudQueueMessageEnvelope envelope, CancellationToken cancellationToken);
    }
}