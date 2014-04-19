using System;

namespace CloudQueueBus
{
    public interface IReceiveContextSender
    {
        void Send(Guid id, object message);
    }
}