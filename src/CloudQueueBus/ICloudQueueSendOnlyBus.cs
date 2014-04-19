using System;

namespace CloudQueueBus
{
    public interface ICloudQueueSendOnlyBus
    {
        void Send(Guid id, object message);
    }
}