using System;
using System.Linq;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueueSendOnlyBus : ICloudQueueSendOnlyBus
    {
        private readonly ICloudQueueSendOnlyBusConfiguration _configuration;
        private readonly SendContextSender _sender;
        private CloudQueueClient _queueClient;
        private CloudBlobClient _blobClient;

        public CloudQueueSendOnlyBus(ICloudQueueSendOnlyBusConfiguration configuration, SendContextSender sender)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (sender == null) throw new ArgumentNullException("sender");
            _configuration = configuration;
            _sender = sender;
        }

        public ICloudQueueSendOnlyBusConfiguration Configuration
        {
            get { return _configuration; }
        }

        private CloudQueueClient QueueClient
        {
            get { return _queueClient ?? (_queueClient = Configuration.StorageAccount.CreateCloudQueueClient()); }
        }

        private CloudBlobClient BlobClient
        {
            get { return _blobClient ?? (_blobClient = Configuration.StorageAccount.CreateCloudBlobClient()); }
        }

        public SendContextSender Sender
        {
            get { return _sender; }
        }

        public void Initialize()
        {
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

        public void Send(Guid id, object message)
        {
            if (message == null) 
                throw new ArgumentNullException("message");
            var route = Configuration.Routes.SingleOrDefault(_ => _.Message == message.GetType());
            if (route != null)
            {
                _sender.Send(
                    new SendContext().
                        SetFrom(Configuration.SenderConfiguration.FromQueue).
                        SetTo(route.QueueName).
                        SetMessageId(id).
                        SetCorrelationId(id).
                        SetMessage(message));
            }
        }
    }
}