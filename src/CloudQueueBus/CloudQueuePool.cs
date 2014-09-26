using System;
using System.Collections.Concurrent;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueuePool : ICloudQueuePool
    {
        private readonly CloudQueueClient _client;
        private readonly ConcurrentDictionary<string, ConcurrentBag<CloudQueue>> _queues;

        public CloudQueuePool(CloudQueueClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            _client = client;
            _queues = new ConcurrentDictionary<string, ConcurrentBag<CloudQueue>>();
        }

        public CloudQueueClient Client
        {
            get { return _client; }
        }

        public CloudQueue Take(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            ConcurrentBag<CloudQueue> queues;
            if (!_queues.TryGetValue(name, out queues))
            {
                return Client.GetQueueReference(name);
            }
            CloudQueue queue;
            return !queues.TryTake(out queue) ?
                Client.GetQueueReference(name) :
                queue;
        }

        public void Return(CloudQueue queue)
        {
            _queues.AddOrUpdate(
                queue.Name,
                new ConcurrentBag<CloudQueue>
                {
                    queue
                },
                (name, queues) =>
                {
                    queues.Add(queue);
                    return queues;
                });
        }
    }
}