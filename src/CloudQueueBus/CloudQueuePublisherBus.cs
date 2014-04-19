using System;
using System.Linq;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueuePublisherBus : ICloudQueuePublisherBus
    {
        private readonly ICloudQueuePublisherConfiguration _configuration;
        private readonly ISendContextSender _sender;
        private CloudQueueClient _client;

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

        private CloudQueueClient Client
        {
            get { return _client ?? (_client = Configuration.StorageAccount.CreateCloudQueueClient()); }
        }

        public void Initialize()
        {
            foreach (var sendQueue in 
                Configuration.Subscriptions.
                    Select(_ => _.Address).
                    Distinct().
                    Select(address => 
                        Client.GetQueueReference(
                            CloudQueueUri.ParseUsing(Client.BaseUri, address).Name)))
            {
                sendQueue.CreateIfNotExists();
            }
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
                Publish(context, subscription.Address);
            }
        }

        void Publish(IConfigureSendContext context, Uri address)
        {
            Sender.Send(context.
                SetFrom(Configuration.SenderConfiguration.FromAddress).
                SetTo(address).
                SetCorrelationId(SerialGuid.NewGuid()));
        }
    }
}