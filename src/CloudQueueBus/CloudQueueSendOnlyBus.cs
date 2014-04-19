using System;
using System.Linq;
using CloudQueueBus.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public class CloudQueueSendOnlyBus : ICloudQueueSendOnlyBus
    {
        private readonly ICloudQueueSendOnlyBusConfiguration _configuration;
        private readonly SendContextSender _sender;
        private CloudQueueClient _client;

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

        private CloudQueueClient Client
        {
            get { return _client ?? (_client = Configuration.StorageAccount.CreateCloudQueueClient()); }
        }

        public SendContextSender Sender
        {
            get { return _sender; }
        }

        public void Initialize()
        {
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

        public void Send(Guid id, object message)
        {
            if (message == null) 
                throw new ArgumentNullException("message");
            var route = Configuration.Routes.SingleOrDefault(_ => _.Message == message.GetType());
            if (route != null)
            {
                _sender.Send(
                    new SendContext().
                        SetFrom(Configuration.SenderConfiguration.FromAddress).
                        SetTo(route.Address).
                        SetMessageId(id).
                        SetCorrelationId(id).
                        SetMessage(message));
            }
        }
    }
}