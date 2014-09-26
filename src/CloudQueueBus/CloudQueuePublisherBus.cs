using System;
using System.Linq;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueuePublisherBus : ICloudQueuePublisherBus
    {
        private readonly ICloudQueuePublisherConfiguration _configuration;
        private readonly ISendContextSender _sender;
        private CloudQueueClient _queueClient;
        private CloudBlobClient _blobClient;

        public CloudQueuePublisherBus(ICloudQueuePublisherConfiguration configuration, ISendContextSender sender)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (sender == null) throw new ArgumentNullException("sender");
            _configuration = configuration;
            _sender = sender;
        }

        public ICloudQueuePublisherConfiguration Configuration
        {
            get { return _configuration; }
        }

        public ISendContextSender Sender
        {
            get { return _sender; }
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
            foreach (var sendQueue in 
                Configuration.Subscriptions.
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

        public void Publish(Guid id, object message)
        {
            if (message == null) throw new ArgumentNullException("message");

            var context = 
                new SendContext().
                    SetMessage(message).
                    SetMessageId(id);

            foreach (var subscription in Configuration.Subscriptions.Where(_ => _.Message == message.GetType()))
            {
                Publish(context, subscription.QueueName);
            }
        }

        void Publish(IConfigureSendContext context, string address)
        {
            Sender.Send(
                context.
                    SetFrom(Configuration.SenderConfiguration.FromQueue).
                    SetTo(address).
                    SetCorrelationId(SerialGuid.NewGuid()));
        }
    }
}