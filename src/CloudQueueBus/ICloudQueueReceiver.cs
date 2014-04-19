using System;

namespace CloudQueueBus
{
    public interface ICloudQueueReceiver : IDisposable
    {
        void Start();
    }
}