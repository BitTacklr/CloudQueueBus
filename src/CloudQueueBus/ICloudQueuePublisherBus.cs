using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudQueueBus
{
    public interface ICloudQueuePublisherBus
    {
        void Publish(Guid id, object message);

        Task PublishAsync(Guid id, object message);
        Task PublishAsync(Guid id, object message, CancellationToken cancellationToken);
    }
}