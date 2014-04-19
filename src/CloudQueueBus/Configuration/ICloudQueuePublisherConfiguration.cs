using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueuePublisherConfiguration
    {
        CloudStorageAccount StorageAccount { get; }
        JsonSerializer Serializer { get; }
        Subscription[] Subscriptions { get; }

        ICloudQueueSenderConfiguration SenderConfiguration { get; }
    }
}