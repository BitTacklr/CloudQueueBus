using System;

namespace CloudQueueBus
{
    public interface ICloudQueueServerBus : IDisposable
    {
        void Start();
    }
}