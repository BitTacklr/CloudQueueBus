using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class AsyncCloudQueueMessageEnvelopeHandler : IAsyncHandler<ICloudQueueMessageEnvelope>
    {
        private readonly IMessageContentTypeResolver _resolver;
        private readonly JsonSerializer _serializer;
        private readonly IAsyncHandler<IConfigureAsyncReceiveContext> _next;

        public AsyncCloudQueueMessageEnvelopeHandler(
            IMessageContentTypeResolver resolver,
            JsonSerializer serializer,
            IAsyncHandler<IConfigureAsyncReceiveContext> next)
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

        public IAsyncHandler<IConfigureAsyncReceiveContext> Next
        {
            get { return _next; }
        }

        public IMessageContentTypeResolver Resolver
        {
            get { return _resolver; }
        }

        public Task HandleAsync(ICloudQueueMessageEnvelope value)
        {
            return HandleAsync(value, CancellationToken.None);
        }

        public Task HandleAsync(ICloudQueueMessageEnvelope value, CancellationToken cancellationToken)
        {
            if (value == null) throw new ArgumentNullException("value");
            var context =
                new AsyncReceiveContext().
                    SetFrom(value.From).
                    SetTo(value.To).
                    SetMessageId(value.MessageId).
                    SetRelatesToMessageId(value.RelatesToMessageId).
                    SetCorrelationId(value.CorrelationId).
                    SetMessage(DeserializeMessage(value));

            return Next.HandleAsync(context, cancellationToken);
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