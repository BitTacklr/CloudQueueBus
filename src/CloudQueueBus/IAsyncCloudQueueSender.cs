using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public interface IAsyncCloudQueueSender
    {
        Task SendAsync(string queueName, CloudQueueMessage message);
        Task SendAsync(string queueName, CloudQueueMessage message, CancellationToken cancellationToken);
    }
}