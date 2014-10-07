using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueServerBusConfiguration
    {
        CloudStorageAccount StorageAccount { get; }
        JsonSerializer Serializer { get; }

        IAsyncHandler<IAsyncReceiveContext> Handler { get; }

        ICloudQueueReceiverConfiguration ReceiverConfiguration { get; }
        Type[] Messages { get; }
        string OverflowBlobContainerName { get; }

        ICloudQueueSenderConfiguration SenderConfiguration { get; }
        Route[] Routes { get; }

        ICloudQueueErrorConfiguration ErrorConfiguration { get; }
    }
}