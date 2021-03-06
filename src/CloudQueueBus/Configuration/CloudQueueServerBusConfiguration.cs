using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueServerBusConfiguration : ICloudQueueServerBusConfiguration
    {
        public CloudQueueServerBusConfiguration(CloudStorageAccount storageAccount, JsonSerializer serializer, IObserver<IReceiveContext> observer, ICloudQueueReceiverConfiguration receiverConfiguration, Type[] messages, ICloudQueueSenderConfiguration senderConfiguration, Route[] routes, ICloudQueueErrorConfiguration errorConfiguration, string overflowBlobContainerName)
        {
            if (storageAccount == null) throw new ArgumentNullException("storageAccount");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (observer == null) throw new ArgumentNullException("observer");
            if (receiverConfiguration == null) throw new ArgumentNullException("receiverConfiguration");
            if (messages == null) throw new ArgumentNullException("messages");
            if (senderConfiguration == null) throw new ArgumentNullException("senderConfiguration");
            if (routes == null) throw new ArgumentNullException("routes");
            if (errorConfiguration == null) throw new ArgumentNullException("errorConfiguration");
            if (overflowBlobContainerName == null) throw new ArgumentNullException("overflowBlobContainerName");
            StorageAccount = storageAccount;
            Serializer = serializer;
            Observer = observer;
            ReceiverConfiguration = receiverConfiguration;
            Messages = messages;
            SenderConfiguration = senderConfiguration;
            Routes = routes;
            ErrorConfiguration = errorConfiguration;
            OverflowBlobContainerName = overflowBlobContainerName;
        }

        public CloudStorageAccount StorageAccount { get; private set; }
        public JsonSerializer Serializer { get; private set; }
        public IObserver<IReceiveContext> Observer { get; private set; }
        public ICloudQueueReceiverConfiguration ReceiverConfiguration { get; private set; }
        public Type[] Messages { get; private set; }
        public string OverflowBlobContainerName { get; private set; }
        public ICloudQueueSenderConfiguration SenderConfiguration { get; private set; }
        public Route[] Routes { get; private set; }
        public ICloudQueueErrorConfiguration ErrorConfiguration { get; private set; }
    }
}