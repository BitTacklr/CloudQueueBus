using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudQueueBus
{
    public interface IAsyncReceiveContextSender
    {
        Task SendAsync(Guid id, object message);
        Task SendAsync(Guid id, object message, CancellationToken cancellationToken);
    }
}