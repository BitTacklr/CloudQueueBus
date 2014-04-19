using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public interface ICloudQueuePool
    {
        CloudQueue Take(Uri address);
        void Return(CloudQueue queue);
    }
}