using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudQueueBus
{
    public interface ICloudQueuePool
    {
        CloudQueue Take(string name);
        void Return(CloudQueue queue);
    }
}