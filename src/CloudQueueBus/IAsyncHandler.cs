using System.Threading;
using System.Threading.Tasks;

namespace CloudQueueBus
{
    public interface IAsyncHandler<in T>
    {
        Task HandleAsync(T value);
        Task HandleAsync(T value, CancellationToken cancellationToken);
    }
}