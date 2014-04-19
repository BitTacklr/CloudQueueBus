using System;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelopeJsonWriter : ICloudQueueMessageEnvelopeJsonWriter
    {
        public void Write(IConfigureCloudQueueMessageEnvelope envelope, JsonWriter writer)
        {
            if (envelope == null) throw new ArgumentNullException("envelope");
            if (writer == null) throw new ArgumentNullException("writer");
            writer.WriteStartObject();
            writer.WritePropertyName("From");
            writer.WriteValue(envelope.From);
            writer.WritePropertyName("To");
            writer.WriteValue(envelope.To);
            writer.WritePropertyName("MessageId");
            writer.WriteValue(envelope.MessageId);
            if (envelope.RelatesToMessageId.HasValue)
            {
                writer.WritePropertyName("RelatesToMessageId");
                writer.WriteValue(envelope.RelatesToMessageId.Value);
            }
            writer.WritePropertyName("CorrelationId");
            writer.WriteValue(envelope.CorrelationId);
            writer.WritePropertyName("ContentType");
            writer.WriteValue(envelope.ContentType);
            writer.WritePropertyName("Content");
            writer.WriteValue(envelope.Content);
            writer.WriteEndObject();
        }
    }
}