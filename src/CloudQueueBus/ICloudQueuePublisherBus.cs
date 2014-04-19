using System;

namespace CloudQueueBus
{
    public interface ICloudQueuePublisherBus
    {
        void Publish(Guid id, object message);
    }
}