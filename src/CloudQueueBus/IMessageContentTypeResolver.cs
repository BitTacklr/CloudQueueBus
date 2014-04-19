using System;

namespace CloudQueueBus
{
    public interface IMessageContentTypeResolver
    {
        Type GetMessageType(string contentType);
    }
}