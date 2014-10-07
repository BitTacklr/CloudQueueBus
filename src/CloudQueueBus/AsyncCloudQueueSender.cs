using System;
using System.Threading;
using System.Threading.Tasks;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class AsyncCloudQueueSender : IAsyncCloudQueueSender
    {
        private readonly ICloudQueuePool _pool;
        private readonly ICloudQueueSenderConfiguration _configuration;

        public AsyncCloudQueueSender(ICloudQueuePool pool, ICloudQueueSenderConfiguration configuration)
        {
            if (pool == null) throw new ArgumentNullException("pool");
            if (configuration == null) throw new ArgumentNullException("configuration");
            _pool = pool;
            _configuration = configuration;
        }

        public ICloudQueuePool Pool
        {
            get { return _pool; }
        }

        public ICloudQueueSenderConfiguration Configuration
        {
            get { return _configuration; }
        }

        public Task SendAsync(string queueName, CloudQueueMessage message)
        {
            return SendAsync(queueName, message, CancellationToken.None);
        }

        public async Task SendAsync(string queueName, CloudQueueMessage message, CancellationToken cancellationToken)
        {
            if (queueName == null) throw new ArgumentNullException("address");
            if (message == null) throw new ArgumentNullException("message");
            var queue = Pool.Take(queueName);
            try
            {
                await queue.AddMessageAsync(
                    message,
                    Configuration.TimeToLive,
                    Configuration.InitialVisibilityDelay,
                    Configuration.QueueRequestOptions.Clone(),
                    null,
                    cancellationToken);
            }
            finally
            {
                Pool.Return(queue);
            }
        }
    }
}