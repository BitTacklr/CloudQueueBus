using System;
using System.IO;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelopeHandler : IHandler<ICloudQueueMessageEnvelope>
    {
        private readonly IMessageContentTypeResolver _resolver;
        private readonly JsonSerializer _serializer;
        private readonly IHandler<IConfigureReceiveContext> _next;

        public CloudQueueMessageEnvelopeHandler(
            IMessageContentTypeResolver resolver,
            JsonSerializer serializer,
            IHandler<IConfigureReceiveContext> next)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (next == null) throw new ArgumentNullException("next");
            _resolver = resolver;
            _serializer = serializer;
            _next = next;
        }

        public JsonSerializer Serializer
        {
            get { return _serializer; }
        }

        public IHandler<IConfigureReceiveContext> Next
        {
            get { return _next; }
        }

        public IMessageContentTypeResolver Resolver
        {
            get { return _resolver; }
        }

        public void Handle(ICloudQueueMessageEnvelope value)
        {
            if (value == null) throw new ArgumentNullException("value");
            var context =
                new ReceiveContext().
                    SetFrom(value.From).
                    SetTo(value.To).
                    SetMessageId(value.MessageId).
                    SetRelatesToMessageId(value.RelatesToMessageId).
                    SetCorrelationId(value.CorrelationId).
                    SetMessage(DeserializeMessage(value));

            Next.Handle(context);
        }

        private object DeserializeMessage(ICloudQueueMessageEnvelope envelope)
        {
            var messageType = Resolver.GetMessageType(envelope.ContentType);
            using (var stream = new MemoryStream(envelope.Content))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        return Serializer.Deserialize(jsonReader, messageType);
                    }
                }
            }
        }
    }
}