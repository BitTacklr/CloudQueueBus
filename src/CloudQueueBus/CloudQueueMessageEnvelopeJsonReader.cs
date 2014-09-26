using System;
using Newtonsoft.Json;

namespace CloudQueueBus
{
    public class CloudQueueMessageEnvelopeJsonReader : ICloudQueueMessageEnvelopeJsonReader
    {
        public IConfigureCloudQueueMessageEnvelope Read(JsonReader reader)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            IConfigureCloudQueueMessageEnvelope envelope = new CloudQueueMessageEnvelope();
            reader.Read(); //StartObject
            reader.Read(); //PropertyName:From
            envelope = envelope.SetFrom(reader.ReadAsString()); //String:From value
            reader.Read(); //PropertyName:To
            envelope = envelope.SetTo(reader.ReadAsString()); //String:To value
            reader.Read(); //PropertyName:MessageId
            envelope = envelope.SetMessageId(new Guid(reader.ReadAsString())); //String:MessageId value
            reader.Read();
            if (((string)reader.Value) == "RelatesToMessageId") //PropertyName:Possibly RelatesToMessageId
            {
                envelope = envelope.SetRelatesToMessageId(new Guid(reader.ReadAsString()));
                reader.Read(); //PropertyName:CorrelationId
            }
            envelope = envelope.SetCorrelationId(new Guid(reader.ReadAsString())); //String:CorrelationId value
            reader.Read(); //PropertyName:ContentType
            envelope = envelope.SetContentType(reader.ReadAsString()); //String:ContentType value
            reader.Read(); //PropertyName:Content
            envelope = envelope.SetContent(reader.ReadAsBytes()); //String:Content value
            reader.Read(); //PropertyName:Time
            envelope = envelope.SetTime(reader.ReadAsDateTimeOffset().GetValueOrDefault()); //String:Time value
            reader.Read(); //EndObject
            return envelope;
        }
    }
}