using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueuePublisherBus : ICloudQueuePublisherBus
    {
        private readonly ICloudQueuePublisherConfiguration _configuration;
        private readonly ISendContextSender _sender;
        private readonly IAsyncSendContextSender _asyncSender;
        private CloudQueueClient _queueClient;
        private CloudBlobClient _blobClient;

        public CloudQueuePublisherBus(ICloudQueuePublisherConfiguration configuration, ISendContextSender sender, IAsyncSendContextSender asyncSender)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (sender == null) throw new ArgumentNullException("sender");
            if (asyncSender == null) throw new ArgumentNullException("asyncSender");
            _configuration = configuration;
            _sender = sender;
            _asyncSender = asyncSender;
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

        public IAsyncSendContextSender AsyncSender
        {
            get { return _asyncSender; }
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

        public Task InitializeAsync()
        {
            return InitializeAsync(CancellationToken.None);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            foreach (var sendQueue in
                Configuration.Subscriptions.
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

        public Task PublishAsync(Guid id, object message)
        {
            return PublishAsync(id, message, CancellationToken.None);
        }

        public async Task PublishAsync(Guid id, object message, CancellationToken cancellationToken)
        {
            if (message == null) throw new ArgumentNullException("message");

            var context =
                new SendContext().
                    SetMessage(message).
                    SetMessageId(id);

            foreach (var subscription in Configuration.Subscriptions.Where(_ => _.Message == message.GetType()))
            {
                await PublishAsync(context, subscription.QueueName, cancellationToken);
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

        Task PublishAsync(IConfigureSendContext context, string address, CancellationToken cancellationToken)
        {
            return AsyncSender.SendAsync(
                context.
                    SetFrom(Configuration.SenderConfiguration.FromQueue).
                    SetTo(address).
                    SetCorrelationId(SerialGuid.NewGuid()),
                cancellationToken);
        }
    }
}