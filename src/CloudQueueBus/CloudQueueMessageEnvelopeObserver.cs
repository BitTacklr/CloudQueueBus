using System;
using System.IO;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelopeObserver : IObserver<ICloudQueueMessageEnvelope>
    {
        private readonly IMessageContentTypeResolver _resolver;
        private readonly JsonSerializer _serializer;
        private readonly IObserver<IConfigureReceiveContext> _observer;

        public CloudQueueMessageEnvelopeObserver(
            IMessageContentTypeResolver resolver, 
            JsonSerializer serializer,
            IObserver<IConfigureReceiveContext> observer)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (observer == null) throw new ArgumentNullException("observer");
            _resolver = resolver;
            _serializer = serializer;
            _observer = observer;
        }

        public JsonSerializer Serializer
        {
            get { return _serializer; }
        }

        public IObserver<IConfigureReceiveContext> Observer
        {
            get { return _observer; }
        }

        public IMessageContentTypeResolver Resolver
        {
            get { return _resolver; }
        }

        public void OnNext(ICloudQueueMessageEnvelope value)
        {
            //TODO: If content was stored in blob storage, now's a good time to go retrieve that
            //Or maybe we should move that piece into another observer

            var context = 
                new ReceiveContext().
                SetFrom(value.From).
                SetTo(value.To).
                SetMessageId(value.MessageId).
                SetRelatesToMessageId(value.RelatesToMessageId).
                SetCorrelationId(value.CorrelationId).
                SetMessage(DeserializeMessage(value));

            Observer.OnNext(context);
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

        public void OnError(Exception error)
        {
            Observer.OnError(error);
        }

        public void OnCompleted()
        {
            Observer.OnCompleted();
        }
    }
}