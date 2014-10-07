using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueueServerBus : ICloudQueueServerBus
    {
        private readonly ICloudQueueServerBusConfiguration _configuration;
        private readonly ICloudQueueReceiver _receiver;
        private CloudQueueClient _queueClient;
        private CloudBlobClient _blobClient;

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

        private CloudQueueClient QueueClient
        {
            get { return _queueClient ?? (_queueClient = Configuration.StorageAccount.CreateCloudQueueClient()); }
        }

        private CloudBlobClient BlobClient
        {
            get { return _blobClient ?? (_blobClient = Configuration.StorageAccount.CreateCloudBlobClient()); }
        }

        public void Initialize()
        {
            var receiveQueue =
                QueueClient.GetQueueReference(Configuration.ReceiverConfiguration.ReceiveQueue);
            receiveQueue.CreateIfNotExists();
            var errorQueue =
                QueueClient.GetQueueReference(Configuration.ErrorConfiguration.ErrorQueue);
            errorQueue.CreateIfNotExists();
            foreach (var sendQueue in
                Configuration.Routes.
                    Select(_ => _.QueueName).
                    Distinct().
                    Select(address =>
                        QueueClient.GetQueueReference(address)))
            {
                sendQueue.CreateIfNotExists();
            }
            var overflowContainer =
                BlobClient.GetContainerReference(Configuration.OverflowBlobContainerName);
            overflowContainer.CreateIfNotExists();
        }

        public Task InitializeAsync()
        {
            return InitializeAsync(CancellationToken.None);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            var receiveQueue =
                QueueClient.GetQueueReference(Configuration.ReceiverConfiguration.ReceiveQueue);
            await receiveQueue.CreateIfNotExistsAsync(cancellationToken);
            var errorQueue =
                QueueClient.GetQueueReference(Configuration.ErrorConfiguration.ErrorQueue);
            await errorQueue.CreateIfNotExistsAsync(cancellationToken);
            foreach (var sendQueue in
                Configuration.Routes.
                    Select(_ => _.QueueName).
                    Distinct().
                    Select(address =>
                        QueueClient.GetQueueReference(address)))
            {
                await sendQueue.CreateIfNotExistsAsync(cancellationToken);
            }
            var overflowContainer =
                BlobClient.GetContainerReference(Configuration.OverflowBlobContainerName);
            await overflowContainer.CreateIfNotExistsAsync(cancellationToken);
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