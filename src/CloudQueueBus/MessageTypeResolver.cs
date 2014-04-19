using System;

namespace CloudQueueBus
{
    public class MessageTypeResolver : IMessageTypeResolver
    {
        public string GetContentType(object message)
        {
            if (message == null) throw new ArgumentNullException("message");
            return message.GetType().Name;
        }
    }
}