using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public class CloudQueueSendOnlyBusConfiguration : ICloudQueueSendOnlyBusConfiguration
    {
        public CloudQueueSendOnlyBusConfiguration(CloudStorageAccount storageAccount, JsonSerializer serializer, ICloudQueueSenderConfiguration senderConfiguration, Route[] routes, string overflowBlobContainerName)
        {
            if (storageAccount == null) throw new ArgumentNullException("storageAccount");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (senderConfiguration == null) throw new ArgumentNullException("senderConfiguration");
            if (routes == null) throw new ArgumentNullException("routes");
            if (overflowBlobContainerName == null) throw new ArgumentNullException("overflowBlobContainerName");
            StorageAccount = storageAccount;
            Serializer = serializer;
            SenderConfiguration = senderConfiguration;
            Routes = routes;
            OverflowBlobContainerName = overflowBlobContainerName;
        }

        public CloudStorageAccount StorageAccount { get; private set; }
        public JsonSerializer Serializer { get; private set; }
        public ICloudQueueSenderConfiguration SenderConfiguration { get; set; }
        public Route[] Routes { get; private set; }
        public string OverflowBlobContainerName { get; private set; }
    }
}