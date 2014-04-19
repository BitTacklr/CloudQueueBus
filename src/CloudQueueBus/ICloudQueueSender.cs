using System;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public interface ICloudQueueSender
    {
        void Send(Uri address, CloudQueueMessage message);
    }
}