using Microsoft.WindowsAzure.Storage.Blob;

namespace CloudQueueBus
{
    public interface ICloudBlobContainerPool
    {
        CloudBlobContainer Take(string name);
        void Return(CloudBlobContainer container);
    }
}