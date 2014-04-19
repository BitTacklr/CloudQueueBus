using System;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueueSender : ICloudQueueSender
    {
        private readonly ICloudQueuePool _pool;
        private readonly ICloudQueueSenderConfiguration _configuration;

        public CloudQueueSender(ICloudQueuePool pool, ICloudQueueSenderConfiguration configuration)
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

        public void Send(Uri address, CloudQueueMessage message)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (message == null) throw new ArgumentNullException("message");
            var queue = Pool.Take(address);
            try
            {
                queue.AddMessage(
                    message,
                    Configuration.TimeToLive,
                    Configuration.InitialVisibilityDelay,
                    Configuration.QueueRequestOptions.Clone());
            }
            finally
            {
                Pool.Return(queue);
            }
        }
    }
}