using System;

namespace CloudQueueBus
{
    public interface ISenderConfiguration
    {
        Uri GetMessageAddress(Type message);
    }
}