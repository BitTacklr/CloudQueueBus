using System;
using System.Linq;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueueServerBus : ICloudQueueServerBus
    {
        private readonly ICloudQueueServerBusConfiguration _configuration;
        private readonly ICloudQueueReceiver _receiver;
        private CloudQueueClient _client;

        public CloudQueueServerBus(ICloudQueueServerBusConfiguration configuration, ICloudQueueReceiver receiver)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (receiver == null) throw new ArgumentNullException("receiver");
            _configuration = configuration;
            _receiver = receiver;
        }

        public ICloudQueueServerBusConfiguration Configuration
        {
            get { return _configuration; }
        }

        public ICloudQueueReceiver Receiver
        {
            get { return _receiver; }
        }

        private CloudQueueClient Client
        {
            get { return _client ?? (_client = Configuration.StorageAccount.CreateCloudQueueClient()); }
        }

        public void Initialize()
        {
            var receiveQueue = 
                Client.GetQueueReference(
                    CloudQueueUri.ParseUsing(Client.BaseUri, Configuration.ReceiverConfiguration.ReceiveAddress).Name);
            receiveQueue.CreateIfNotExists();
            foreach (var sendQueue in
                Configuration.Routes.
                    Select(_ => _.Address).
                    Distinct().
                    Select(address =>
                        Client.GetQueueReference(
                            CloudQueueUri.ParseUsing(Client.BaseUri, address).Name)))
            {
                sendQueue.CreateIfNotExists();
            }
        }

        public void Start()
        {
            Receiver.Start();
        }

        public void Dispose()
        {
            Receiver.Dispose();
        }
    }
}