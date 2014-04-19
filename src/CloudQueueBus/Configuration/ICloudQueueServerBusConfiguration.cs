using System;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;

namespace CloudQueueBus.Configuration
{
    public interface ICloudQueueServerBusConfiguration
    {
        CloudStorageAccount StorageAccount { get; }
        JsonSerializer Serializer { get; }

        IObserver<IReceiveContext> Observer { get; }

        ICloudQueueReceiverConfiguration ReceiverConfiguration { get; }
        Type[] Messages { get; }

        ICloudQueueSenderConfiguration SenderConfiguration { get; }
        Route[] Routes { get; }
    }
}