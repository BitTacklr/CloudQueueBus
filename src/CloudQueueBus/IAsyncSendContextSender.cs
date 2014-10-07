using System.Threading;
using System.Threading.Tasks;

namespace CloudQueueBus
{
    public interface IAsyncSendContextSender
    {
        Task SendAsync(IConfigureSendContext context);
        Task SendAsync(IConfigureSendContext context, CancellationToken cancellationToken);
    }
}