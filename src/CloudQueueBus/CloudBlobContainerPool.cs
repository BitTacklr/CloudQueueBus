using System;
using System.Collections.Concurrent;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CloudQueueBus
{
    public class CloudBlobContainerPool : ICloudBlobContainerPool
    {
        private readonly CloudBlobClient _client;
        private readonly ConcurrentDictionary<string, ConcurrentBag<CloudBlobContainer>> _containers;

        public CloudBlobContainerPool(CloudBlobClient client)
        {
            if (client == null) throw new ArgumentNullException("client");
            _client = client;
            _containers = new ConcurrentDictionary<string, ConcurrentBag<CloudBlobContainer>>();
        }

        public CloudBlobClient Client
        {
            get { return _client; }
        }

        public CloudBlobContainer Take(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            ConcurrentBag<CloudBlobContainer> containers;
            if (!_containers.TryGetValue(name, out containers))
            {
                return Client.GetContainerReference(name);
            }
            CloudBlobContainer container;
            return !containers.TryTake(out container) ? 
                Client.GetContainerReference(name) : 
                container;
        }

        public void Return(CloudBlobContainer container)
        {
            _containers.AddOrUpdate(
                container.Name, 
                new ConcurrentBag<CloudBlobContainer>
                {
                    container
                },
                (name, containers) =>
                {
                    containers.Add(container);
                    return containers;
                });
        }
    }
}