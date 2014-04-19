using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudQueueBus
{
    public class MessageContentTypeResolver : IMessageContentTypeResolver
    {
        private readonly Dictionary<string, Type> _types;

        public MessageContentTypeResolver(IEnumerable<Type> messages)
        {
            if (messages == null) throw new ArgumentNullException("messages");
            _types = messages.ToDictionary(_ => _.Name);
        }

        public Type GetMessageType(string contentType)
        {
            return _types[contentType];
        }
    }
}