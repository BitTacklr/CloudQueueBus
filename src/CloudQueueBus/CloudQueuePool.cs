using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueuePool : ICloudQueuePool
    {
        private readonly CloudQueueClient _client;
        private readonly Dictionary<Uri, CloudQueue> _queues;

        public CloudQueuePool(CloudQueueClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            _client = client;
            _queues = new Dictionary<Uri, CloudQueue>();
        }

        public CloudQueueClient Client
        {
            get { return _client; }
        }

        public CloudQueue Take(Uri address)
        {
            if (address == null) 
                throw new ArgumentNullException("address");

            if (!Client.BaseUri.IsBaseOf(address))
                throw new ArgumentException(string.Format("The base of the specified address ({0}) does not match the address ({1}) of the cloud queue client configured for this pool.", Client.BaseUri, address));

            CloudQueue queue;
            if (!_queues.TryGetValue(address, out queue))
            {
                return Client.GetQueueReference(CloudQueueUri.ParseUsing(Client.BaseUri, address).Name);
            }
            _queues.Remove(address);
            return queue;
        }

        public void Return(CloudQueue queue)
        {
            if (!_queues.ContainsKey(queue.Uri))
            {
                _queues.Add(queue.Uri, queue);
            }
        }
    }
}