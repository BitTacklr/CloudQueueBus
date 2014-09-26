using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public interface ICloudQueueSender
    {
        void Send(string queueName, CloudQueueMessage message);
    }
}