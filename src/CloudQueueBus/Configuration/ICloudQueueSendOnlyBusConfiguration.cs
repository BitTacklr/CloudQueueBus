using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueSendOnlyBusConfiguration
    {
        CloudStorageAccount StorageAccount { get; }
        JsonSerializer Serializer { get; }

        ICloudQueueSenderConfiguration SenderConfiguration { get; }
        Route[] Routes { get; }

        string OverflowBlobContainerName { get; }
    }
}