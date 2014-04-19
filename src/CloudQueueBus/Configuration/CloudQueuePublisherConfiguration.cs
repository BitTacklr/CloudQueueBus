using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class CloudQueuePublisherConfiguration : ICloudQueuePublisherConfiguration
    {
        public CloudQueuePublisherConfiguration(CloudStorageAccount storageAccount, JsonSerializer serializer, Subscription[] subscriptions, ICloudQueueSenderConfiguration senderConfiguration)
        {
            if (storageAccount == null) throw new ArgumentNullException("storageAccount");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (subscriptions == null) throw new ArgumentNullException("subscriptions");
            if (senderConfiguration == null) throw new ArgumentNullException("senderConfiguration");
            StorageAccount = storageAccount;
            Serializer = serializer;
            Subscriptions = subscriptions;
            SenderConfiguration = senderConfiguration;
        }

        public CloudStorageAccount StorageAccount { get; private set; }
        public JsonSerializer Serializer { get; private set; }
        public Subscription[] Subscriptions { get; private set; }
        public ICloudQueueSenderConfiguration SenderConfiguration { get; private set; }
    }
}