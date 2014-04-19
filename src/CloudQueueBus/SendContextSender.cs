using System;
using System.IO;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class SendContextSender : ISendContextSender
    {
        private readonly ICloudQueueMessageEnvelopeSender _innerSender;
        private readonly IMessageTypeResolver _resolver;
        private readonly JsonSerializer _serializer;

        public SendContextSender(
            ICloudQueueMessageEnvelopeSender innerSender, 
            IMessageTypeResolver resolver,
            JsonSerializer serializer)
        {
            if (innerSender == null) throw new ArgumentNullException("innerSender");
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (serializer == null) throw new ArgumentNullException("serializer");
            _innerSender = innerSender;
            _resolver = resolver;
            _serializer = serializer;
        }

        public ICloudQueueMessageEnvelopeSender InnerSender
        {
            get { return _innerSender; }
        }

        public JsonSerializer Serializer
        {
            get { return _serializer; }
        }

        public IMessageTypeResolver Resolver
        {
            get { return _resolver; }
        }

        public void Send(IConfigureSendContext context)
        {
            var envelope = new CloudQueueMessageEnvelope();
            envelope.SetFrom(context.From);
            envelope.SetTo(context.To);
            envelope.SetMessageId(context.MessageId);
            envelope.SetRelatesToMessageId(context.RelatesToMessageId);
            envelope.SetCorrelationId(context.CorrelationId);
            envelope.SetContentType(Resolver.GetContentType(context.Message));
            envelope.SetContent(SerializeMessage(context));

            InnerSender.Send(envelope);
        }

        private byte[] SerializeMessage(ISendContext context)
        {
            using (var stream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter))
                    {
                        Serializer.Serialize(jsonWriter, context.Message);
                        jsonWriter.Flush();
                        return stream.ToArray();
                    }
                }
            }
        }
    }
}