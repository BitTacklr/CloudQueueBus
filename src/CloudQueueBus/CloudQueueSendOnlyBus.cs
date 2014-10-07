using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueueSendOnlyBus : ICloudQueueSendOnlyBus
    {
        private readonly ICloudQueueSendOnlyBusConfiguration _configuration;
        private readonly ISendContextSender _sender;
        private readonly IAsyncSendContextSender _asyncSender;
        private CloudQueueClient _queueClient;
        private CloudBlobClient _blobClient;

        public CloudQueueSendOnlyBus(ICloudQueueSendOnlyBusConfiguration configuration, ISendContextSender sender, IAsyncSendContextSender asyncSender)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (sender == null) throw new ArgumentNullException("sender");
            if (asyncSender == null) throw new ArgumentNullException("asyncSender");
            _configuration = configuration;
            _sender = sender;
            _asyncSender = asyncSender;
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

        public ISendContextSender Sender
        {
            get { return _sender; }
        }

        public IAsyncSendContextSender AsyncSender
        {
            get { return _asyncSender; }
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

        public Task InitializeAsync()
        {
            return InitializeAsync(CancellationToken.None);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
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

        public Task SendAsync(Guid id, object message)
        {
            return SendAsync(id, message, CancellationToken.None);
        }

        public Task SendAsync(Guid id, object message, CancellationToken cancellationToken)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            var route = Configuration.Routes.SingleOrDefault(_ => _.Message == message.GetType());
            if (route != null)
            {
                return _asyncSender.SendAsync(
                    new SendContext().
                        SetFrom(Configuration.SenderConfiguration.FromQueue).
                        SetTo(route.QueueName).
                        SetMessageId(id).
                        SetCorrelationId(id).
                        SetMessage(message), cancellationToken);
            }
            return Task.FromResult<object>(null);
        }
    }
}